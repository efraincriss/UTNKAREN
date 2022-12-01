using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.aplicacion.RecursosHumanos.Dto;
using com.cpp.calypso.proyecto.aplicacion.RecursosHumanos.Interfaces;
using com.cpp.calypso.proyecto.dominio;
using com.cpp.calypso.proyecto.dominio.RecursosHumanos;
using Castle.Core.Internal;
using OfficeOpenXml;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using Xceed.Words.NET;
using System.Web;
using OfficeOpenXml.Style;
using Border = Xceed.Words.NET.Border;

namespace com.cpp.calypso.proyecto.aplicacion.RecursosHumanos.Services
{
    public class CapacitacionAsyncBaseCrudAppService : AsyncBaseCrudAppService<Capacitacion, CapacitacionDto, PagedAndFilteredResultRequestDto>, ICapacitacionAsyncBaseCrudAppService
    {
        private readonly IBaseRepository<Colaboradores> _colaboradoresBaseRepository;
        private readonly IBaseRepository<Catalogo> _catalogoBaseRepository;
        private readonly IBaseRepository<Usuario> _usuarioBaseRepository;

        public CapacitacionAsyncBaseCrudAppService(
            IBaseRepository<Capacitacion> repository,
            IBaseRepository<Colaboradores> colaboradoresBaseRepository,
            IBaseRepository<Catalogo> catalogoBaseRepository,
            IBaseRepository<Usuario> usuarioBaseRepository
            ) : base(repository)
        {
            _colaboradoresBaseRepository = colaboradoresBaseRepository;
            _catalogoBaseRepository = catalogoBaseRepository;
            _usuarioBaseRepository = usuarioBaseRepository;
        }

        public CapacitacionesColaboradorDto ObtenerCapacitacionesPorColaborador(int colaboradorId)
        {
            var colaborador = _colaboradoresBaseRepository.Get(
                colaboradorId, 
                o => o.Capacitaciones.Select(y => y.CatalogoTipoCapacitacion), 
                o => o.Capacitaciones.Select(z => z.CatalogoNombreCapacitacion)
            );

            var resumenCapacitacionesDto = Repository.GetAll().Include(o => o.CatalogoTipoCapacitacion).Include(o => o.CatalogoNombreCapacitacion)
                .Where(o => o.ColaboradoresId == colaboradorId)
                .GroupBy(o => new {o.CatalogoNombreCapacitacionId, o.CatalogoTipoCapacitacionId, TipoCapacitacion = o.CatalogoTipoCapacitacion.nombre, NombreCapacitacion = o.CatalogoNombreCapacitacion.nombre})
                .Select(c => new CapacitacionDto
                {
                    Id = 0,
                    ColaboradoresId = 0,
                    CatalogoNombreCapacitacionId = c.Key.CatalogoNombreCapacitacionId,
                    CatalogoTipoCapacitacionId = c.Key.CatalogoTipoCapacitacionId,
                    Horas = c.Sum(x => x.Horas),
                    TipoCapacitacionNombre = c.Key.TipoCapacitacion,
                    NombreCapacitacion = c.Key.NombreCapacitacion
                }).ToList();

            var detalleCapacitacionesDto =
                Mapper.Map<List<Capacitacion>, List<CapacitacionDto>>(colaborador.Capacitaciones);


            var catalogoNombreCapacitaciones = _catalogoBaseRepository.GetAll().Where(o => o.TipoCatalogo.codigo == "CAPACITACIONES").ToList();
            var catalogoTipoCapacitaciones = _catalogoBaseRepository.GetAll().Where(o => o.TipoCatalogo.codigo == "TIPO_CAPACITACION").ToList();
            
            var result = new CapacitacionesColaboradorDto
            {
                NombresColaborador = colaborador.nombres_apellidos,
                CodigoSap = colaborador.empleado_id_sap.ToString(),
                Estado = colaborador.estado,
                NumeroIdentificacion = colaborador.numero_identificacion,
                DetalleCapacitaciones = detalleCapacitacionesDto,
                ResumenCapacitaciones = resumenCapacitacionesDto,
                CatalogoNombreCapacitacion = Mapper.Map<List<Catalogo>, List<CatalogoDto>>(catalogoNombreCapacitaciones),
                CatalogoTipoCapacitacion = Mapper.Map<List<Catalogo>, List<CatalogoDto>>(catalogoTipoCapacitaciones),
            };

            return result;
        }

        public bool CrearCapacitacion(CapacitacionDto capacitacion)
        {
            var entity = Mapper.Map<Capacitacion>(capacitacion);
            Repository.Insert(entity);
            return true;
        }

        public bool ActualizarCapacitacion(CapacitacionDto capacitacion)
        {
            var entity = Mapper.Map<Capacitacion>(capacitacion);
            Repository.Update(entity);
            return true;
        }

        public bool EliminarCapacitacion(int capacitacionId)
        {
            Repository.Delete(capacitacionId);
            return true;
        }

        public List<Dto.ColaboradorDto> BuscarColaboradores(string filtro, string estado)
        {
            var colaboradores = _colaboradoresBaseRepository.GetAllIncluding(a => a.Area).Include(o => o.ColaboradorBajas);

            if (filtro != "")
            {
                colaboradores = colaboradores.Where(o => o.nombres_apellidos.Contains(filtro) || o.numero_identificacion.Contains(filtro) || o.empleado_id_sap.ToString().Contains(filtro) );
            }

            if(!estado.IsNullOrEmpty())
            {
                if(estado != "Todos")
                {
                    colaboradores = colaboradores.Where(o => o.estado == estado).Where(o=>o.estado!= "ALTA ANULADA");
                }
            }

            var listado = colaboradores.Where(o => o.estado != "ALTA ANULADA").ToList();

            var dtos = Mapper.Map<List<Colaboradores>, List<Dto.ColaboradorDto>>(listado);

            return dtos;
        }

        public string DescargarCertificados(int[] colaboradoresId)
        {
            string user = System.Web.HttpContext.Current.User.Identity.Name.ToString();

            var usuarioencontrado = _usuarioBaseRepository.GetAll().FirstOrDefault(c => c.Cuenta == user);


            if (usuarioencontrado != null && usuarioencontrado.Id > 0)
            {
               

                // Path Plantilla
                string filename = System.Web.HttpContext.Current.Server.MapPath("~/Views/PlantillaWord/RecursosHumanos/FormatoCertificadoCapacitacion.docx");

                if (File.Exists((string)filename))
                {
                    string salida = System.Web.HttpContext.Current.Server.MapPath("~/Views/PlantillaWord/RecursosHumanos/DocumentosGenerados/Certificado_De_Capacitaciones" + DateTime.Now.ToString("yyyy-MM-dd_hh-mm-ss") + "_" + ".docx");

                    using (var plantilla = DocX.Load(filename))
                    {
                        var document = DocX.Create(salida);

                        

                        var indexTables = 1;
                        var secondaryTableIndex = 0;
                        foreach (var id in colaboradoresId)
                        {
                            document.InsertDocument(plantilla);
                            var colaborador = _colaboradoresBaseRepository.Get(id);

                            document.ReplaceText("{cedula_colaborador}", colaborador.numero_identificacion);
                            document.ReplaceText("{apellidos_nombres}", colaborador.nombres_apellidos);
                            document.ReplaceText("{nombre_usuario}", usuarioencontrado.NombresCompletos.ToUpper());

                            var tables = document.Tables.ToList();

                            var resumenCapacitacionesDto = Repository.GetAll().Include(o => o.CatalogoTipoCapacitacion).Include(o => o.CatalogoNombreCapacitacion)
                                .Where(o => o.ColaboradoresId == id)
                                .GroupBy(o => new {o.CatalogoNombreCapacitacionId, o.CatalogoTipoCapacitacionId, TipoCapacitacion = o.CatalogoTipoCapacitacion.nombre, NombreCapacitacion = o.CatalogoNombreCapacitacion.nombre})
                                .Select(c => new CapacitacionDto
                                {
                                    Id = 0,
                                    ColaboradoresId = id,
                                    CatalogoNombreCapacitacionId = c.Key.CatalogoNombreCapacitacionId,
                                    CatalogoTipoCapacitacionId = c.Key.CatalogoTipoCapacitacionId,
                                    Horas = c.Sum(x => x.Horas),
                                    TipoCapacitacionNombre = c.Key.TipoCapacitacion,
                                    NombreCapacitacion = c.Key.NombreCapacitacion,
                                    Tipo = "Datos"
                                }).ToList();

                            // Poner la tabla con headers y totales
                            var tiposCapacitacionEncontrados = resumenCapacitacionesDto
                                .Select(o => o.TipoCapacitacionNombre).Distinct();

                            var resumenCapacitaciones = new List<CapacitacionDto>();
                            foreach (var tiposCapacitacion in tiposCapacitacionEncontrados)
                            {
                                // Header
                                resumenCapacitaciones.Add(new CapacitacionDto
                                {
                                    Id = 0,
                                    ColaboradoresId = id,
                                    CatalogoNombreCapacitacionId = 0,
                                    CatalogoTipoCapacitacionId = 0,
                                    Horas = resumenCapacitacionesDto.Where(o => o.TipoCapacitacionNombre == tiposCapacitacion).Sum(o => o.Horas),
                                    TipoCapacitacionNombre = tiposCapacitacion,
                                    NombreCapacitacion = tiposCapacitacion,
                                    Tipo = "Header"
                                });

                                // Datos
                                var capacitaciones = resumenCapacitacionesDto.Where(o =>
                                    o.TipoCapacitacionNombre == tiposCapacitacion);
                                resumenCapacitaciones.AddRange(capacitaciones);
                            }

                            // Agregar Totales
                            var total = resumenCapacitacionesDto.Sum(o => o.Horas);
                            resumenCapacitaciones.Add(new CapacitacionDto
                            {
                                Id = 0,
                                ColaboradoresId = id,
                                CatalogoNombreCapacitacionId = 0,
                                CatalogoTipoCapacitacionId = 0,
                                Horas = resumenCapacitacionesDto.Sum(o => o.Horas),
                                TipoCapacitacionNombre = "TOTAL DE HORAS",
                                NombreCapacitacion = "TOTAL DE HORAS",
                                Tipo = "Header"
                            });


                            // Datos de la tabla
                            int fila = 1;

                            foreach (var capacitacion in resumenCapacitaciones)
                            {
                                if (capacitacion.Tipo == "Header")
                                {
                                    tables[indexTables].Rows[fila].Cells[0].Paragraphs[0]
                                        .Append("  " + capacitacion.NombreCapacitacion).Bold();
                                    tables[indexTables].Rows[fila].Cells[1].Paragraphs[0]
                                        .Append(capacitacion.Horas.ToString()).Bold().Alignment = Alignment.center;
                                }
                                else
                                {
                                    tables[indexTables].Rows[fila].Cells[0].Paragraphs[0]
                                        .Append("      " + capacitacion.NombreCapacitacion);
                                    tables[indexTables].Rows[fila].Cells[1].Paragraphs[0]
                                        .Append(capacitacion.Horas.ToString()).Alignment = Alignment.center;
                                }
                                
                                tables[indexTables].InsertRow();
                                fila += 1;
                            }
                            

                            // Poner bordes a la tabla principal de datos
                            tables[indexTables].SetBorder(TableBorderType.InsideH, new Border(BorderStyle.Tcbs_single,BorderSize.two,1,Color.Black));
                            tables[indexTables].SetBorder(TableBorderType.InsideV, new Border(BorderStyle.Tcbs_single,BorderSize.two,1,Color.Black));
                            tables[indexTables].SetBorder(TableBorderType.Top, new Border(BorderStyle.Tcbs_single,BorderSize.two,1,Color.Black));
                            tables[indexTables].SetBorder(TableBorderType.Bottom, new Border(BorderStyle.Tcbs_single,BorderSize.two,1,Color.Black));
                            tables[indexTables].SetBorder(TableBorderType.Right, new Border(BorderStyle.Tcbs_single,BorderSize.two,1,Color.Black));
                            tables[indexTables].SetBorder(TableBorderType.Left, new Border(BorderStyle.Tcbs_single,BorderSize.two,1,Color.Black));


                            // Poner bordes a la tabla de informacion del colaborador
                            tables[secondaryTableIndex].SetBorder(TableBorderType.InsideH, new Border(BorderStyle.Tcbs_single,BorderSize.two,1,Color.Black));
                            tables[secondaryTableIndex].SetBorder(TableBorderType.InsideV, new Border(BorderStyle.Tcbs_single,BorderSize.two,1,Color.Black));
                            tables[secondaryTableIndex].SetBorder(TableBorderType.Top, new Border(BorderStyle.Tcbs_single,BorderSize.two,1,Color.Black));
                            tables[secondaryTableIndex].SetBorder(TableBorderType.Bottom, new Border(BorderStyle.Tcbs_single,BorderSize.two,1,Color.Black));
                            tables[secondaryTableIndex].SetBorder(TableBorderType.Right, new Border(BorderStyle.Tcbs_single,BorderSize.two,1,Color.Black));
                            tables[secondaryTableIndex].SetBorder(TableBorderType.Left, new Border(BorderStyle.Tcbs_single,BorderSize.two,1,Color.Black));

                            // Imagen Firma
                            MemoryStream ms = new MemoryStream(usuarioencontrado.Firma);
                            System.Drawing.Image Imagen = System.Drawing.Image.FromStream(ms);
                            using (MemoryStream imgStream = new MemoryStream())
                            {
                                Imagen.Save(imgStream, System.Drawing.Imaging.ImageFormat.Bmp);
                                imgStream.Seek(0, SeekOrigin.Begin);
                                var image = document.AddImage(imgStream);
                                Picture picture = image.CreatePicture(89, 89);

                                // Insert a new Paragraph into the document.
                                Paragraph p1 = document.Paragraphs.Where(x => x.Text.Contains("{firma}")).FirstOrDefault();
                                p1.AppendPicture(picture);

                                
                            }

                            document.ReplaceText("{firma}", "");
                            document.InsertSectionPageBreak();
                            indexTables += 2;
                            secondaryTableIndex += 2;
                            Imagen.Dispose();

                        }
                        
                        document.Save();
                        return salida;
                    }
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        public ExcelPackage DescargarPlantillaCargaMasivaDeCapacitaciones()
        {
            ExcelPackage excel = new ExcelPackage();

            string filename = System.Web.HttpContext.Current.Server.MapPath("~/Views/PlantillaWord/FormatoCargaDeCapacitaciones.xlsx");
            if (File.Exists((string)filename))
            {
                FileInfo newFile = new FileInfo(filename);
                ExcelPackage pck = new ExcelPackage(newFile);
                excel.Workbook.Worksheets.Add("Capacitaciones", pck.Workbook.Worksheets[1]);
                excel.Workbook.Worksheets.Add("catálogos", pck.Workbook.Worksheets[2]);
            }

            ExcelWorksheet capacitaciones = excel.Workbook.Worksheets[1];
            ExcelWorksheet catalogos = excel.Workbook.Worksheets[2];

            var catalogosTipoCapacitaciones = _catalogoBaseRepository.GetAll().Include(o => o.TipoCatalogo)
                .Where(o => o.TipoCatalogo.codigo == "TIPO_CAPACITACION");

            var catalogoCapacitaciones = _catalogoBaseRepository.GetAll().Include(o => o.TipoCatalogo)
                .Where(o => o.TipoCatalogo.codigo == "CAPACITACIONES");

            int countFilasTipoCatalogos = 1;
            foreach (var tipo in catalogosTipoCapacitaciones)
            {
                catalogos.Cells["A" + countFilasTipoCatalogos].Value = tipo.Id;
                catalogos.Cells["B" + countFilasTipoCatalogos].Value = tipo.nombre;
                countFilasTipoCatalogos++;
            }

            int countFilasCapacitaciones = 1;
            foreach (var capacitacion in catalogoCapacitaciones)
            {
                catalogos.Cells["D" + countFilasCapacitaciones].Value = capacitacion.Id;
                catalogos.Cells["E" + countFilasCapacitaciones].Value = capacitacion.nombre;
                countFilasCapacitaciones++;
            }

            var validacionNombreCapacitaciones = capacitaciones.DataValidations.AddListValidation("F:F");
            validacionNombreCapacitaciones.Formula.ExcelFormula = "=catálogos!$E$1:$E$" + countFilasCapacitaciones;
            validacionNombreCapacitaciones.AllowBlank = true;

            var validacionTipoCapacitaciones = capacitaciones.DataValidations.AddListValidation("E:E");
            validacionTipoCapacitaciones.Formula.ExcelFormula = "=catálogos!$B$1:$B$" + countFilasTipoCatalogos;
            validacionTipoCapacitaciones.AllowBlank = true;

            return excel;
        }

        public byte[] ObtenerFirmaDelUsuarioLogeado()
        {
            string user = System.Web.HttpContext.Current.User.Identity.Name.ToString();

            var usuarioencontrado = _usuarioBaseRepository.GetAll().Where(c => c.Cuenta == user).FirstOrDefault();
            if (usuarioencontrado != null && usuarioencontrado.Id > 0)
            {
                return usuarioencontrado.Firma != null ? usuarioencontrado.Firma : null;
            }
            else
            {
                return null;
            }

        }

        public ExcelPackage CargarCapacitaciones(HttpPostedFileBase uploadedFile)
        {
             if (uploadedFile != null)
                {

  
                    if (uploadedFile.ContentType == "application/vnd.ms-excel" || uploadedFile.ContentType ==
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                    {

                        ExcelPackage excel = new ExcelPackage();
                    
                        string fileContentType = uploadedFile.ContentType;
                        byte[] fileBytes = new byte[uploadedFile.ContentLength];
                        
                        ExcelWorksheet capacitaciones = null;
                        
                        using (var package = new ExcelPackage(uploadedFile.InputStream))
                        {
                            capacitaciones = excel.Workbook.Worksheets.Add("Capacitaciones", package.Workbook.Worksheets[1]);
                            excel.Workbook.Worksheets.Add("catálogos", package.Workbook.Worksheets[2]);

                            var numberOfColumns = capacitaciones.Dimension.End.Column;
                            var numberOfRows = capacitaciones.Dimension.End.Row;

                            for (int rowIterator = 2; rowIterator <= numberOfRows; rowIterator++)
                            {
                                var tipoCapacitacionNombre = (capacitaciones.Cells["E" + rowIterator].Value ?? "").ToString();
                                if (tipoCapacitacionNombre.IsNullOrEmpty())
                                {
                                    capacitaciones.Cells["J" + rowIterator].Value = "No procesado";
                                    capacitaciones.Cells["K" + rowIterator].Value = "Tipo de capacitación es obligatorio";

                                    capacitaciones.Cells["A" + rowIterator + ":I" + rowIterator].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    capacitaciones.Cells["A" + rowIterator + ":I" + rowIterator].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Red);
                                    continue;
                                }

                                var capacitacionNombre = (capacitaciones.Cells["F" + rowIterator].Value ?? "").ToString();
                                if (capacitacionNombre.IsNullOrEmpty())
                                {
                                    capacitaciones.Cells["J" + rowIterator].Value = "Campos vacios";
                                    capacitaciones.Cells["K" + rowIterator].Value = "Capacitación es obligatorio";
                                    capacitaciones.Cells["A" + rowIterator + ":I" + rowIterator].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    capacitaciones.Cells["A" + rowIterator + ":I" + rowIterator].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Red);
                                    continue;
                                }

                                var horas = (capacitaciones.Cells["G" + rowIterator].Value ?? "").ToString();
                                if (horas.IsNullOrEmpty())
                                {
                                    capacitaciones.Cells["J" + rowIterator].Value = "Campos vacios";
                                    capacitaciones.Cells["K" + rowIterator].Value = "Horas es obligatorio";
                                    capacitaciones.Cells["A" + rowIterator + ":I" + rowIterator].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    capacitaciones.Cells["A" + rowIterator + ":I" + rowIterator].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Red);
                                    continue;
                                }

                                var fecha = (capacitaciones.Cells["A" + rowIterator].Text ?? "").ToString();
                                if (fecha.IsNullOrEmpty())
                                {
                                    capacitaciones.Cells["J" + rowIterator].Value = "Campos vacios";
                                    capacitaciones.Cells["K" + rowIterator].Value = "Fecha es obligatorio";
                                    capacitaciones.Cells["A" + rowIterator + ":I" + rowIterator].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    capacitaciones.Cells["A" + rowIterator + ":I" + rowIterator].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Red);
                                    continue;
                                }

                                var cedula = (capacitaciones.Cells["B" + rowIterator].Value ?? "").ToString();
                                var codigoSap = (capacitaciones.Cells["C" + rowIterator].Value ?? "").ToString();
                                if (cedula.IsNullOrEmpty() && codigoSap.IsNullOrEmpty())
                                {
                                    capacitaciones.Cells["J" + rowIterator].Value = "Datos incorrectos";
                                    capacitaciones.Cells["K" + rowIterator].Value = "Se debe ingresar la cédula ó código sap";
                                    capacitaciones.Cells["A" + rowIterator + ":I" + rowIterator].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    capacitaciones.Cells["A" + rowIterator + ":I" + rowIterator].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Red);
                                    continue;
                                }

                                var intCodigoSap = codigoSap.IsNullOrEmpty() ? -1 : Int32.Parse(codigoSap);
                                var colaborador = _colaboradoresBaseRepository
                                    .GetAll().FirstOrDefault(o =>
                                        o.numero_identificacion == cedula || o.empleado_id_sap == intCodigoSap);

                                if (colaborador == null)
                                {
                                    capacitaciones.Cells["J" + rowIterator].Value = "Datos incorrectos";
                                    capacitaciones.Cells["K" + rowIterator].Value = "No se encontro el colaborador con los datos proporcionados";
                                    capacitaciones.Cells["A" + rowIterator + ":I" + rowIterator].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    capacitaciones.Cells["A" + rowIterator + ":I" + rowIterator].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Red);
                                    continue;
                                }

                                var catalogoTipoCapacitacion = _catalogoBaseRepository.GetAll()
                                    .FirstOrDefault(o => o.nombre == tipoCapacitacionNombre);

                                if (catalogoTipoCapacitacion == null)
                                {
                                    capacitaciones.Cells["J" + rowIterator].Value = "Datos incorrectos";
                                    capacitaciones.Cells["K" + rowIterator].Value = "No se encontro el tipo de capacitación proporcionada";
                                    capacitaciones.Cells["A" + rowIterator + ":I" + rowIterator].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    capacitaciones.Cells["A" + rowIterator + ":I" + rowIterator].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Red);
                                    continue;
                                }

                                var catalogoCapacitacion = _catalogoBaseRepository.GetAll()
                                    .FirstOrDefault(o => o.nombre == capacitacionNombre);

                                if (catalogoCapacitacion == null)
                                {
                                    capacitaciones.Cells["J" + rowIterator].Value = "Datos incorrectos";
                                    capacitaciones.Cells["K" + rowIterator].Value = "No se encontro la capacitación proporcionada";
                                    capacitaciones.Cells["A" + rowIterator + ":I" + rowIterator].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    capacitaciones.Cells["A" + rowIterator + ":I" + rowIterator].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Red);
                                    continue;
                                }



                                DateTime fechaFormateada = Convert.ToDateTime(fecha).Date;


                                var fuente = (capacitaciones.Cells["H" + rowIterator].Value ?? "").ToString();
                                var observaciones = (capacitaciones.Cells["I" + rowIterator].Value ?? "").ToString();

                                var existeCapacitacion = Repository.GetAll()
                                    .Where(o => o.ColaboradoresId == colaborador.Id)
                                    .Where(o => o.CatalogoTipoCapacitacionId == catalogoTipoCapacitacion.Id)
                                    .Where(o => o.CatalogoNombreCapacitacionId == catalogoCapacitacion.Id)
                                    .FirstOrDefault(o => o.Fecha == fechaFormateada);

                                if (existeCapacitacion != null)
                                {
                                    capacitaciones.Cells["J" + rowIterator].Value = "Registro duplicado";
                                    capacitaciones.Cells["K" + rowIterator].Value = "Ya existe un registro con los mismos datos";
                                    capacitaciones.Cells["A" + rowIterator + ":I" + rowIterator].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    capacitaciones.Cells["A" + rowIterator + ":I" + rowIterator].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.DodgerBlue);
                                    continue;
                                }
                                    
                                Capacitacion ingreso = new Capacitacion
                                {
                                    Horas = Decimal.Parse(horas),
                                    Fuente = fuente,
                                    Fecha = fechaFormateada,
                                    Observaciones = observaciones,
                                    CatalogoNombreCapacitacion = catalogoCapacitacion,
                                    CatalogoNombreCapacitacionId = catalogoCapacitacion.Id,
                                    CatalogoTipoCapacitacion = catalogoTipoCapacitacion,
                                    CatalogoTipoCapacitacionId = catalogoTipoCapacitacion.Id,
                                    Colaboradores = colaborador,
                                    ColaboradoresId = colaborador.Id,
                                };

                                capacitaciones.Cells["J" + rowIterator].Value = "Procesado correctamente";
                                capacitaciones.Cells["K" + rowIterator].Value = "";
                                capacitaciones.Cells["A" + rowIterator + ":I" + rowIterator].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                capacitaciones.Cells["A" + rowIterator + ":I" + rowIterator].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.White);

                                Repository.Insert(ingreso);

                            }
                        }
                        
                        return excel;
                    }
                    return new ExcelPackage();
                }
            return new ExcelPackage();
        }

        public CapacitacionesColaboradorDto ObtenerCapacitaciones(string filtroColaborador, string tipoCapacitacion, string nombreCapacitacion, string fechaDesde, string fechaHasta)
        {

            var capacitaciones = Repository.GetAll().Include(a => a.Colaboradores).Include(a => a.CatalogoNombreCapacitacion).Include(a => a.CatalogoTipoCapacitacion);

            if (filtroColaborador != "")
            {
                capacitaciones = capacitaciones.Where(o =>
                    o.Colaboradores.nombres_apellidos.Contains(filtroColaborador) ||
                    o.Colaboradores.numero_identificacion.Contains(filtroColaborador) ||
                    o.Colaboradores.empleado_id_sap.ToString().Contains(filtroColaborador));
            }

            if(!tipoCapacitacion.IsNullOrEmpty())
            {
                int tipoCapacitacionId = Int32.Parse(tipoCapacitacion);
                capacitaciones = capacitaciones.Where(o => o.CatalogoTipoCapacitacionId == tipoCapacitacionId);
            }

            if(!nombreCapacitacion.IsNullOrEmpty())
            {
                int nombreCapacitacionId = Int32.Parse(nombreCapacitacion);
                capacitaciones = capacitaciones.Where(o => o.CatalogoNombreCapacitacionId == nombreCapacitacionId);
            }

            if(!fechaDesde.IsNullOrEmpty())
            {
                DateTime fd = Convert.ToDateTime(fechaDesde);
                capacitaciones = capacitaciones.Where(o => o.Fecha >= fd);
            }

            if(!fechaHasta.IsNullOrEmpty())
            {
                DateTime fh = Convert.ToDateTime(fechaHasta);
                capacitaciones = capacitaciones.Where(o => o.Fecha <= fh);
            }

            var listadoCapacitaciones = capacitaciones.ToList();

            var listadoCapacitacionesDtos = Mapper.Map<List<Capacitacion>, List<Dto.CapacitacionDto>>(listadoCapacitaciones);

            var result = new CapacitacionesColaboradorDto
            {
                DetalleCapacitaciones = listadoCapacitacionesDtos,
            };

            return result;
        }

        public CapacitacionesColaboradorDto ObtenerCatalogosDeCapacitaciones()
        {
            var catalogoNombreCapacitaciones = _catalogoBaseRepository.GetAll().Where(o => o.TipoCatalogo.codigo == "CAPACITACIONES").ToList();
            var catalogoTipoCapacitaciones = _catalogoBaseRepository.GetAll().Where(o => o.TipoCatalogo.codigo == "TIPO_CAPACITACION").ToList();

            var result = new CapacitacionesColaboradorDto
            {
                CatalogoNombreCapacitacion = Mapper.Map<List<Catalogo>, List<CatalogoDto>>(catalogoNombreCapacitaciones),
                CatalogoTipoCapacitacion = Mapper.Map<List<Catalogo>, List<CatalogoDto>>(catalogoTipoCapacitaciones),
            };

            return result;
        }
    
         public ExcelPackage DescargarReporteDeCapacitaciones(int capacitacion, int tipoCapacitacion, DateTime? fechaDesde, DateTime? fechaHasta)
         {
            ExcelPackage excel = new ExcelPackage();

            string filename = System.Web.HttpContext.Current.Server.MapPath("~/Views/PlantillaWord/RecursosHumanos/FormatoReporteDeCapacitaciones.xlsx");
            if (File.Exists((string)filename))
            {
                FileInfo newFile = new FileInfo(filename);
                ExcelPackage pck = new ExcelPackage(newFile);
                excel.Workbook.Worksheets.Add("RESUMEN", pck.Workbook.Worksheets[1]);
                excel.Workbook.Worksheets.Add("DETALLE", pck.Workbook.Worksheets[2]);
            }

            ExcelWorksheet resumen = excel.Workbook.Worksheets[1];
            ExcelWorksheet detalle = excel.Workbook.Worksheets[2];

            var capacitacionesQuery = Repository.GetAll()
                .Include(o => o.Colaboradores)
                .Include(o => o.CatalogoTipoCapacitacion)
                .Include(o => o.CatalogoNombreCapacitacion)
                .Include(o => o.Colaboradores.Area);

            if (capacitacion != 0)
            {
                capacitacionesQuery = capacitacionesQuery.Where(o => o.CatalogoNombreCapacitacionId == capacitacion);
            }

            if (tipoCapacitacion != 0)
            {
                capacitacionesQuery = capacitacionesQuery.Where(o => o.CatalogoTipoCapacitacionId == tipoCapacitacion);
            }

            if (fechaDesde.HasValue)
            {
                capacitacionesQuery = capacitacionesQuery.Where(o => o.Fecha >= fechaDesde.Value);
            }

            if (fechaHasta.HasValue)
            {
                capacitacionesQuery = capacitacionesQuery.Where(o => o.Fecha <= fechaHasta.Value);
            }

            var capacitaciones = capacitacionesQuery.ToList();

            // Detalle de capacitaciones
            int countDetalles = 2;
            foreach (var cap in capacitaciones)
            {
                detalle.Cells["A" + countDetalles].Value = countDetalles - 1;
                detalle.Cells["B" + countDetalles].Value = cap.Colaboradores.numero_identificacion;
                detalle.Cells["C" + countDetalles].Value = cap.Colaboradores.nombres_apellidos;
                detalle.Cells["D" + countDetalles].Value = cap.CatalogoNombreCapacitacion.nombre;
                detalle.Cells["E" + countDetalles].Value = cap.CatalogoTipoCapacitacion.nombre;
                detalle.Cells["F" + countDetalles].Value = cap.Fecha;
                detalle.Cells["G" + countDetalles].Value = cap.Horas;
                detalle.Cells["H" + countDetalles].Value = cap.Fuente;
                detalle.Cells["I" + countDetalles].Value = cap.Colaboradores.Area == null ? "" : cap.Colaboradores.Area.nombre;
                countDetalles++;
            }

            // Resumen de capacitaciones
            int countResumen = 2;
            var resumenCapacitacionesQuery = Repository.GetAll()
                .Include(o => o.CatalogoTipoCapacitacion)
                .Include(o => o.CatalogoNombreCapacitacion)
                .Include(o => o.Colaboradores);

            if (capacitacion != 0)
            {
                resumenCapacitacionesQuery = resumenCapacitacionesQuery.Where(o => o.CatalogoNombreCapacitacionId == capacitacion);
            }

            if (tipoCapacitacion != 0)
            {
                resumenCapacitacionesQuery = resumenCapacitacionesQuery.Where(o => o.CatalogoTipoCapacitacionId == tipoCapacitacion);
            }

            if (fechaDesde.HasValue)
            {
                resumenCapacitacionesQuery = resumenCapacitacionesQuery.Where(o => o.Fecha >= fechaDesde.Value);
            }

            if (fechaHasta.HasValue)
            {
                resumenCapacitacionesQuery = resumenCapacitacionesQuery.Where(o => o.Fecha <= fechaHasta.Value);
            }

            var resumenCapacitacionesDto = resumenCapacitacionesQuery.GroupBy(o => new {
                    o.CatalogoNombreCapacitacionId, 
                    o.CatalogoTipoCapacitacionId, 
                    o.ColaboradoresId,
                    TipoCapacitacion = o.CatalogoTipoCapacitacion.nombre, 
                    NombreCapacitacion = o.CatalogoNombreCapacitacion.nombre,
                    NombreColaborador = o.Colaboradores.nombres_apellidos,
                    NumeroIdentificacion = o.Colaboradores.numero_identificacion
                }
            )
            .Select(c => new CapacitacionDto
            {
                Id = 0,
                ColaboradoresId = c.Key.ColaboradoresId,
                CatalogoNombreCapacitacionId = c.Key.CatalogoNombreCapacitacionId,
                CatalogoTipoCapacitacionId = c.Key.CatalogoTipoCapacitacionId,
                Horas = c.Sum(x => x.Horas),
                TipoCapacitacionNombre = c.Key.TipoCapacitacion,
                NombreCapacitacion = c.Key.NombreCapacitacion,
                ColaboradorNombre = c.Key.NombreColaborador,
                ColaboradorIdentificacion = c.Key.NumeroIdentificacion
            }).ToList();

            foreach (var cap in resumenCapacitacionesDto)
            {
                resumen.Cells["A" + countResumen].Value = countResumen - 1;
                resumen.Cells["B" + countResumen].Value = cap.ColaboradorIdentificacion;
                resumen.Cells["C" + countResumen].Value = cap.ColaboradorNombre;
                resumen.Cells["D" + countResumen].Value = cap.NombreCapacitacion;
                resumen.Cells["E" + countResumen].Value = cap.TipoCapacitacionNombre;
                resumen.Cells["F" + countResumen].Value = cap.Horas;
                countResumen++;
            }

            return excel;
        }
    }
}
