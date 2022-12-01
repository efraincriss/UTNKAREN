using AutoMapper;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.aplicacion.Dto;
using com.cpp.calypso.proyecto.aplicacion.Interfaces;
using com.cpp.calypso.proyecto.dominio;
using com.cpp.calypso.proyecto.dominio.Constantes;
using com.cpp.calypso.proyecto.dominio.Models;
using com.cpp.calypso.seguridad.aplicacion;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace com.cpp.calypso.proyecto.aplicacion.Service
{
    public class ColaboradorBajaAsyncBaseCrudAppService : AsyncBaseCrudAppService<ColaboradorBaja, ColaboradorBajaDto, PagedAndFilteredResultRequestDto>, IColaboradorBajaAsyncBaseCrudAppService
    {
        private readonly ICatalogoAsyncBaseCrudAppService _catalogoRepository;
        private readonly IBaseRepository<Archivo> _archivoRepository;
        private readonly IBaseRepository<ParametroSistema> _parametroRepository;
        private readonly IBaseRepository<Usuario> _usuarioRepository;
        private readonly IBaseRepository<Colaboradores> _colaboradoresRepository;

        private readonly IdentityEmailMessageService _correoservice;
        private readonly IBaseRepository<CorreoLista> _correslistarepository;
        public ColaboradorBajaAsyncBaseCrudAppService(
            IBaseRepository<ColaboradorBaja> repository,
            ICatalogoAsyncBaseCrudAppService catalogoRepository,
            IBaseRepository<Archivo> archivoRepository,
            IBaseRepository<ParametroSistema> parametroRepository,
            IBaseRepository<Usuario> usuarioRepository,
            IBaseRepository<Colaboradores> colaboradoresRepository,
            IdentityEmailMessageService correoservice,
            IBaseRepository<CorreoLista> correslistarepository
            ) : base(repository)
        {
            _catalogoRepository = catalogoRepository;
            _archivoRepository = archivoRepository;
            _usuarioRepository = usuarioRepository;
            _parametroRepository = parametroRepository;
            _colaboradoresRepository = colaboradoresRepository;
            _correoservice = correoservice;
            _correslistarepository = correslistarepository;
        }

        public ColaboradorBajaDto GetBaja(int Id)
        {
            var c = Repository.GetAll().Where(d => d.ColaboradoresId == Id && d.vigente == true).FirstOrDefault();

            if (c != null)
            {
                ColaboradorBajaDto baja = new ColaboradorBajaDto()

                {
                    Id = c.Id,
                    ColaboradoresId = c.ColaboradoresId,
                    ArchivoId = c.ArchivoId,
                    catalogo_motivo_baja_id = c.catalogo_motivo_baja_id,
                    fecha_baja = c.fecha_baja,
                    detalle_baja = c.detalle_baja,
                    requiere_entrevista = c.requiere_entrevista,
                    tiene_encuesta = c.tiene_encuesta,
                    fecha_pago_liquidacion = c.fecha_pago_liquidacion,
                    registro_masivo = c.registro_masivo,
                    estado = c.estado,
                    motivo_desestimacion = c.motivo_desestimacion,
                    vigente = c.vigente,
                    motivo_baja = c.MotivoBaja.nombre,
                    estado_baja = Enum.GetName(typeof(BajaEstado), c.estado)
                };

                return baja;
            }

            return null;
        }

        public List<ColaboradorBajaDto> GetBajas()
        {
            var e = 1;
            var query = Repository.GetAll().Where(d => d.estado != BajaEstado.DESESTIMADA && d.vigente == true);

            if (query != null)
            {
                List<ColaboradorBajaDto> bajas = (from c in query
                                                  select new ColaboradorBajaDto
                                                  {
                                                      Id = c.Id,
                                                      ColaboradoresId = c.ColaboradoresId,
                                                      Colaboradores = c.Colaboradores,
                                                      ArchivoId = c.ArchivoId,
                                                      catalogo_motivo_baja_id = c.catalogo_motivo_baja_id,
                                                      fecha_baja = c.fecha_baja,
                                                      detalle_baja = c.detalle_baja,
                                                      requiere_entrevista = c.requiere_entrevista,
                                                      tiene_encuesta = c.tiene_encuesta,
                                                      fecha_pago_liquidacion = c.fecha_pago_liquidacion,
                                                      registro_masivo = c.registro_masivo,
                                                      estado = c.estado,
                                                      motivo_desestimacion = c.motivo_desestimacion,
                                                      vigente = c.vigente,
                                                      archivo_liquidacion_id = c.archivo_liquidacion_id,
                                                      motivo_baja = c.MotivoBaja.nombre,
                                                      nombre_grupo_personal = c.Colaboradores.EncargadoPersonal.nombre,
                                                      nombre_identificacion = c.Colaboradores.TipoIdentificacion.nombre,
                                                      motivo_edicion = c.motivo_edicion
                                                  }).ToList();

                foreach (var i in bajas)
                {
                    i.nro = e++;
                    i.apellidos_nombres = i.Colaboradores.primer_apellido + " " + i.Colaboradores.segundo_apellido;
                    i.liquidado = i.fecha_pago_liquidacion == null ? "NO" : "SI";
                    i.estado_baja = Enum.GetName(typeof(BajaEstado), i.estado);
                }

                return bajas;
            }

            return null;
        }

        public string CargarArchivoBaja(ColaboradorBajaDto baja, HttpPostedFileBase archivo)
        {
            /* cargamos el archivo */
            if (archivo != null)
            {

                string fileName = archivo.FileName;
                string fileContentType = archivo.ContentType;
                byte[] fileBytes = new byte[archivo.ContentLength];
                var data = archivo.InputStream.Read(fileBytes, 0,
                    Convert.ToInt32(archivo.ContentLength));

                Archivo n = new Archivo
                {
                    Id = 0,
                    codigo = "COL_BAJA_" + baja.ColaboradoresId,
                    nombre = fileName,
                    vigente = true,
                    fecha_registro = DateTime.Now,
                    hash = fileBytes,
                    tipo_contenido = fileContentType,
                };
                var archivoid = _archivoRepository.InsertAndGetId(n);

                /* Registramos la fotografia al colaborador */
                baja.ArchivoId = archivoid;


                var result = Repository.InsertAndGetId(MapToEntity(baja));
                return "OK";

            }
            return "NO";
        }

        public List<ColaboradorBajaDto> GetBajasGenerarArchivo(BajaEstado estado)
        {
            var e = 1;
            var query = Repository.GetAll().Where(d => d.estado == estado && d.vigente == true);

            if (query != null)
            {
                List<ColaboradorBajaDto> bajas = (from c in query
                                                  select new ColaboradorBajaDto
                                                  {
                                                      Id = c.Id,
                                                      ColaboradoresId = c.ColaboradoresId,
                                                      Colaboradores = c.Colaboradores,
                                                      ArchivoId = c.ArchivoId,
                                                      catalogo_motivo_baja_id = c.catalogo_motivo_baja_id,
                                                      fecha_baja = c.fecha_baja,
                                                      detalle_baja = c.detalle_baja,
                                                      requiere_entrevista = c.requiere_entrevista,
                                                      tiene_encuesta = c.tiene_encuesta,
                                                      fecha_pago_liquidacion = c.fecha_pago_liquidacion,
                                                      registro_masivo = c.registro_masivo,
                                                      estado = c.estado,
                                                      motivo_desestimacion = c.motivo_desestimacion,
                                                      vigente = c.vigente,
                                                      motivo_baja = c.MotivoBaja.nombre,
                                                      apellidos_nombres = c.Colaboradores.primer_apellido + " " + c.Colaboradores.segundo_apellido
                                                  }).ToList();

                foreach (var i in bajas)
                {
                    i.nro = e++;
                    i.estado_baja = Enum.GetName(typeof(BajaEstado), i.estado);
                }

                return bajas;
            }

            return null;
        }


        public ColaboradorBajaDto GetBajasEnviarSap(int Id)
        {
            var c = Repository.Get(Id);

            ColaboradorBajaDto bajas = new ColaboradorBajaDto()
            {
                Id = c.Id,
                ColaboradoresId = c.ColaboradoresId,
                Colaboradores = c.Colaboradores,
                ArchivoId = c.ArchivoId,
                catalogo_motivo_baja_id = c.catalogo_motivo_baja_id,
                fecha_baja = c.fecha_baja,
                detalle_baja = c.detalle_baja,
                requiere_entrevista = c.requiere_entrevista,
                tiene_encuesta = c.tiene_encuesta,
                fecha_pago_liquidacion = c.fecha_pago_liquidacion,
                registro_masivo = c.registro_masivo,
                estado = c.estado,
                motivo_desestimacion = c.motivo_desestimacion,
                vigente = c.vigente,
                motivo_baja = c.MotivoBaja.nombre,
                motivo_baja_codigo = c.MotivoBaja.valor_texto,
                apellidos_nombres = c.Colaboradores.primer_apellido + " " + c.Colaboradores.segundo_apellido,
                estado_baja = Enum.GetName(typeof(BajaEstado), c.estado)
            };

            return bajas;
        }


        public ColaboradorBajaTemp GetBajasEnviarSapTemp(int Id)
        {
            var c = Repository.GetAllIncluding(x => x.Colaboradores, x => x.MotivoBaja).Where(x => x.Id == Id).FirstOrDefault();

            if (c != null)
            {
                ColaboradorBajaTemp baja = new ColaboradorBajaTemp()
                {
                    Id=c.Id,
                    empleado_id_sap = c.Colaboradores != null ? c.Colaboradores.empleado_id_sap.HasValue ? c.Colaboradores.empleado_id_sap.Value.ToString() : "" : "",
                    apellidos_nombres = c.Colaboradores != null ? c.Colaboradores.primer_apellido + " " + c.Colaboradores.segundo_apellido : "",
                    nombres = c.Colaboradores != null ? c.Colaboradores.nombres : "",
                    fecha_baja = c.fecha_baja.GetValueOrDefault(),
                    motivo_baja = c.MotivoBaja != null ? c.MotivoBaja.nombre : "",
                    numero_identificacion = c.Colaboradores != null ? c.Colaboradores.numero_identificacion : "",
                    numero_legajo_temporal = c.Colaboradores != null ? c.Colaboradores.numero_legajo_temporal : "",

                };
                return baja;
                

            }
            else
            {
                return new ColaboradorBajaTemp();
            }
           }

        public int GuardarLiquidacionArchivoAsync(int baja, HttpPostedFileBase[] UploadedFile)
        {
            /* cargamos el archivo */
            if (UploadedFile != null && UploadedFile.Length > 0)
            {
                foreach (var archivo in UploadedFile)
                {

                    string fileName = archivo.FileName;
                    string fileContentType = archivo.ContentType;
                    byte[] fileBytes = new byte[archivo.ContentLength];
                    var data = archivo.InputStream.Read(fileBytes, 0,
                        Convert.ToInt32(archivo.ContentLength));

                    Archivo n = new Archivo
                    {
                        Id = 0,
                        codigo = "LIQ_BAJ_" + baja,
                        nombre = fileName,
                        vigente = true,
                        fecha_registro = DateTime.Now,
                        hash = fileBytes,
                        tipo_contenido = fileContentType,
                    };
                    var archivoid = _archivoRepository.InsertAndGetId(n);

                    ///* Registramos el archivo a la baja  */
                    //baja.archivo_liquidacion_id = archivoid;
                    //baja.vigente = true;

                    //var b = Mapper.Map<ColaboradorBajaDto, ColaboradorBaja>(baja);

                    //var result =Repository.Update(b);
                    return archivoid;
                }
            }
            return 0;
        }

        public async Task<string> GenerarExcelBajas(List<ColaboradorBajaDto> bajas, bool es_manual)
        {

            DateTime fechaActual = DateTime.Now;

            var aux = fechaActual.ToString("ddMMyyyyhhmm");
            var fecha = fechaActual.ToString("dd/MM/yyyy");

            var usuario = "";
            var proyecto = "";
            string user = System.Web.HttpContext.Current.User.Identity.Name.ToString();
            var usuarioencontrado = _usuarioRepository.GetAll().Where(c => c.Cuenta == user).FirstOrDefault();
            if (usuarioencontrado != null && usuarioencontrado.Id > 0)
            {
                usuario = usuarioencontrado.NombresCompletos;
                var colaborador = _colaboradoresRepository.GetAll().Where(c => c.numero_identificacion == usuarioencontrado.Identificacion).FirstOrDefault();
                if (colaborador != null && colaborador.Id > 0)
                {
                    proyecto = colaborador.ContratoId > 0 ? colaborador.Proyecto.nombre : "";
                }
            }

            FileInfo prueba = new FileInfo("prueba.xlsx");
            using (ExcelPackage excel = new ExcelPackage(prueba))
            {
                //Crear hoja en archivo excel
                var hoja = excel.Workbook.Worksheets.Add("Baja Empleado");
                var row = 12;

                hoja.Column(1).Width = 15.30;
                hoja.Column(2).Width = 17.80;
                hoja.Column(3).Width = 8;
                hoja.Column(4).Width = 24;
                hoja.Column(5).Width = 20;
                hoja.Column(6).Width = 13;
                hoja.Column(7).Width = 15;
                hoja.Column(8).Width = 1;


                hoja.Row(1).Height = 45;
                hoja.Row(2).Height = 9;
                hoja.Row(3).Height = 9;
                hoja.Row(4).Height = 9;
                hoja.Row(5).Height = 26;
                hoja.Row(6).Height = 9;
                hoja.Row(7).Height = 15;
                hoja.Row(8).Height = 15;
                hoja.Row(9).Height = 15;
                hoja.Row(10).Height = 9;
                hoja.Row(11).Height = 37;

                //Cabecera de documento


                //hoja.Cells["B2:J2,B4:J4"].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
                hoja.Cells["A1:G1"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                hoja.Cells["A1:G1"].Style.Fill.BackgroundColor.SetColor(Color.DarkGray);
                hoja.Cells["C1"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                hoja.Cells["C1"].Style.Font.Name = "Arial";
                hoja.Cells["C1"].Style.Font.Size = 20;
                hoja.Cells["C1"].Style.Font.Color.SetColor(Color.White);
                hoja.Cells["C1"].Value = "Baja de Empleado Gestión B";
                hoja.Cells["C1"].Style.Font.Bold = true;

                hoja.Cells["A5:G5"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                hoja.Cells["A5:G5"].Style.Fill.BackgroundColor.SetColor(Color.DarkGray);
                hoja.Cells["A5"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                hoja.Cells["A5"].Style.Font.Name = "Arial";
                hoja.Cells["A5"].Style.Font.Size = 12;
                hoja.Cells["A5"].Style.Font.Color.SetColor(Color.White);
                hoja.Cells["A5"].Value = "Una vez completado carga en SP del CSC";
                hoja.Cells["A5"].Style.Font.Bold = true;

                hoja.Cells["A7:G7"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                hoja.Cells["A7:G7"].Style.Fill.BackgroundColor.SetColor(Color.DarkGray);
                hoja.Cells["A7"].Style.Font.Name = "Arial";
                hoja.Cells["A7"].Style.Font.Size = 12;
                hoja.Cells["A7"].Style.Font.Color.SetColor(Color.Black);
                hoja.Cells["A7"].Value = "Proyecto:";
                hoja.Cells["A7"].Style.Font.Bold = true;
                hoja.Cells["B7"].Style.Font.Name = "Arial";
                hoja.Cells["B7"].Style.Font.Size = 12;
                hoja.Cells["B7"].Style.Font.Color.SetColor(Color.Black);
                hoja.Cells["B7"].Value = proyecto;
                hoja.Cells["B7"].Style.Font.Bold = true;

                hoja.Cells["A8:G8"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                hoja.Cells["A8:G8"].Style.Fill.BackgroundColor.SetColor(Color.DarkGray);
                hoja.Cells["A8"].Style.Font.Name = "Arial";
                hoja.Cells["A8"].Style.Font.Size = 12;
                hoja.Cells["A8"].Style.Font.Color.SetColor(Color.Black);
                hoja.Cells["A8"].Value = "Cargado por:";
                hoja.Cells["A8"].Style.Font.Bold = true;
                hoja.Cells["B8"].Style.Font.Name = "Arial";
                hoja.Cells["B8"].Style.Font.Size = 12;
                hoja.Cells["B8"].Style.Font.Color.SetColor(Color.Black);
                hoja.Cells["B8"].Value = usuario;
                hoja.Cells["B8"].Style.Font.Bold = true;

                hoja.Cells["A9:G9"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                hoja.Cells["A9:G9"].Style.Fill.BackgroundColor.SetColor(Color.DarkGray);
                hoja.Cells["A9"].Style.Font.Name = "Arial";
                hoja.Cells["A9"].Style.Font.Size = 12;
                hoja.Cells["A9"].Style.Font.Color.SetColor(Color.Black);
                hoja.Cells["A9"].Value = "Fecha:";
                hoja.Cells["A9"].Style.Font.Bold = true;
                hoja.Cells["B9"].Style.Font.Name = "Arial";
                hoja.Cells["B9"].Style.Font.Size = 12;
                hoja.Cells["B9"].Style.Font.Color.SetColor(Color.Black);
                hoja.Cells["B9"].Value = fecha;
                hoja.Cells["B9"].Style.Font.Bold = true;



                //Cabecera de tabla
                var titleCell = hoja.Cells["A11:G11"]; // Celdas de títulos

                titleCell.Style.Font.Bold = true;
                titleCell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                titleCell.Style.Fill.BackgroundColor.SetColor(Color.DarkGray);
                titleCell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                titleCell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                titleCell.Style.WrapText = true;
                titleCell.Style.Font.Name = "Arial";
                titleCell.Style.Font.Size = 8;

                titleCell.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                titleCell.Style.Border.Right.Color.SetColor(Color.White);

                hoja.Cells["A11"].Value = "ID SAP";
                hoja.Cells["B11"].Value = "Cédula/Pasaporte";
                hoja.Cells["C11"].Value = "Legajo";
                hoja.Cells["D11"].Value = "Apellidos";
                hoja.Cells["E11"].Value = "Nombres";
                hoja.Cells["F11"].Value = "Fecha de baja";
                hoja.Cells["G11"].Value = "Motivo salida";

                foreach (var i in bajas)
                {
                    var body = hoja.Cells["A" + row + ":G" + row];
                    body.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    body.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    body.Style.Font.Name = "Calibri";
                    body.Style.Font.Size = 9;
                    body.Style.WrapText = true;
                    body.Style.Border.BorderAround(ExcelBorderStyle.Dotted, Color.Black);

                    body.Style.Border.Right.Style = ExcelBorderStyle.Dotted;
                    body.Style.Border.Right.Color.SetColor(Color.Black);

                    hoja.Row(row).Height = 13;

                    hoja.Cells["A" + row].Value = i.Colaboradores.empleado_id_sap;
                    hoja.Cells["B" + row].Value = i.Colaboradores.numero_identificacion;
                    hoja.Cells["C" + row].Value = i.Colaboradores.numero_legajo_temporal;
                    hoja.Cells["D" + row].Value = i.apellidos_nombres;
                    hoja.Cells["E" + row].Value = i.Colaboradores.nombres;
                    hoja.Cells["F" + row].Value = String.Format("{0:dd/MM/yyyy}", i.fecha_baja);
                    hoja.Cells["G" + row].Value = i.motivo_baja;

                    row++;
                }

                if (es_manual == true)
                {
                    var correos = _correslistarepository.GetAll().Where(c => c.vigente)
                                                              .Where(c => c.ListaDistribucion.vigente)
                                                              .Where(c => c.ListaDistribucion.nombre == CatalogosCodigos.DEFAULT_LISTADISTRIBUCION).ToList();
                    if (correos.Count > 0)
                    {


                        /* ES: Envio de Excel al Correo*/
                        MailMessage message = new MailMessage();
                        message.Subject = "PMDIS: Bajas Colaboradores";
                        foreach (var item in correos)
                        {
                            message.To.Add(item.correo);

                        }

                        excel.SaveAs(new FileInfo(System.Web.HttpContext.Current.Server.MapPath("~/Views/ArchivosColaboradores/FormularioBajasEcuador" + aux + ".xlsx")));

                        string url = System.Web.HttpContext.Current.Server.MapPath("~/Views/ArchivosColaboradores/FormularioBajasEcuador" + aux + ".xlsx");
                        if (File.Exists((string)url))
                        {
                            message.Attachments.Add(new Attachment(url));
                        }
                        /*********/
                        await _correoservice.SendWithFilesAsync(message);


                    }




                    /*System.IO.FileInfo filename = new System.IO.FileInfo(@"C:\CPP\Colaboradores\Bajas\FormularioBajasEcuador" + aux + ".xlsx");
                    excel.SaveAs(filename);*/
                }
                else
                {

                    var correos = _correslistarepository.GetAll().Where(c => c.vigente)
                                                             .Where(c => c.ListaDistribucion.vigente)
                                                             .Where(c => c.ListaDistribucion.nombre == CatalogosCodigos.DEFAULT_LISTADISTRIBUCION).ToList();
                    if (correos.Count > 0)
                    {


                        /* ES: Envio de Excel al Correo*/
                        MailMessage message = new MailMessage();
                        message.Subject = "PMDIS: Bajas Colaboradores";
                        foreach (var item in correos)
                        {
                            message.To.Add(item.correo);

                        }

                        excel.SaveAs(new FileInfo(System.Web.HttpContext.Current.Server.MapPath("~/Views/ArchivosColaboradores/FormularioBajasEcuador" + aux + ".xlsx")));

                        string url = System.Web.HttpContext.Current.Server.MapPath("~/Views/ArchivosColaboradores/FormularioBajasEcuador" + aux + ".xlsx");
                        if (File.Exists((string)url))
                        {
                            message.Attachments.Add(new Attachment(url));
                        }
                        /*********/
                        await _correoservice.SendWithFilesAsync(message);


                    }




                    /*System.IO.FileInfo filename = new System.IO.FileInfo(@"C:\CPP\Colaboradores\Bajas\FormularioBajasEcuador" + aux + ".xlsx");
                    excel.SaveAs(filename);*/
                }


                return "OK";
            }


        }

        public async Task<string> GenerarExcelBajasTemp(List<ColaboradorBajaTemp> bajas, bool es_manual)
        {

            DateTime fechaActual = DateTime.Now;

            var aux = fechaActual.ToString("ddMMyyyyhhmm");
            var fecha = fechaActual.ToString("dd/MM/yyyy");

            var usuario = "";
            var proyecto = "";
            string user = System.Web.HttpContext.Current.User.Identity.Name.ToString();
            var usuarioencontrado = _usuarioRepository.GetAll().Where(c => c.Cuenta == user).FirstOrDefault();
            if (usuarioencontrado != null && usuarioencontrado.Id > 0)
            {
                usuario = usuarioencontrado.NombresCompletos;
                var colaborador = _colaboradoresRepository.GetAll().Where(c => c.numero_identificacion == usuarioencontrado.Identificacion).FirstOrDefault();
                if (colaborador != null && colaborador.Id > 0)
                {
                    proyecto = colaborador.ContratoId > 0 ? colaborador.Proyecto.nombre : "";
                }
            }

            FileInfo prueba = new FileInfo("prueba.xlsx");
            using (ExcelPackage excel = new ExcelPackage(prueba))
            {
                //Crear hoja en archivo excel
                var hoja = excel.Workbook.Worksheets.Add("Baja Empleado");
                var row = 12;

                hoja.Column(1).Width = 15.30;
                hoja.Column(2).Width = 17.80;
                hoja.Column(3).Width = 8;
                hoja.Column(4).Width = 24;
                hoja.Column(5).Width = 20;
                hoja.Column(6).Width = 13;
                hoja.Column(7).Width = 15;
                hoja.Column(8).Width = 1;


                hoja.Row(1).Height = 45;
                hoja.Row(2).Height = 9;
                hoja.Row(3).Height = 9;
                hoja.Row(4).Height = 9;
                hoja.Row(5).Height = 26;
                hoja.Row(6).Height = 9;
                hoja.Row(7).Height = 15;
                hoja.Row(8).Height = 15;
                hoja.Row(9).Height = 15;
                hoja.Row(10).Height = 9;
                hoja.Row(11).Height = 37;

                //Cabecera de documento


                //hoja.Cells["B2:J2,B4:J4"].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
                hoja.Cells["A1:G1"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                hoja.Cells["A1:G1"].Style.Fill.BackgroundColor.SetColor(Color.DarkGray);
                hoja.Cells["C1"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                hoja.Cells["C1"].Style.Font.Name = "Arial";
                hoja.Cells["C1"].Style.Font.Size = 20;
                hoja.Cells["C1"].Style.Font.Color.SetColor(Color.White);
                hoja.Cells["C1"].Value = "Baja de Empleado Gestión B";
                hoja.Cells["C1"].Style.Font.Bold = true;

                hoja.Cells["A5:G5"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                hoja.Cells["A5:G5"].Style.Fill.BackgroundColor.SetColor(Color.DarkGray);
                hoja.Cells["A5"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                hoja.Cells["A5"].Style.Font.Name = "Arial";
                hoja.Cells["A5"].Style.Font.Size = 12;
                hoja.Cells["A5"].Style.Font.Color.SetColor(Color.White);
                hoja.Cells["A5"].Value = "Una vez completado carga en SP del CSC";
                hoja.Cells["A5"].Style.Font.Bold = true;

                hoja.Cells["A7:G7"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                hoja.Cells["A7:G7"].Style.Fill.BackgroundColor.SetColor(Color.DarkGray);
                hoja.Cells["A7"].Style.Font.Name = "Arial";
                hoja.Cells["A7"].Style.Font.Size = 12;
                hoja.Cells["A7"].Style.Font.Color.SetColor(Color.Black);
                hoja.Cells["A7"].Value = "Proyecto:";
                hoja.Cells["A7"].Style.Font.Bold = true;
                hoja.Cells["B7"].Style.Font.Name = "Arial";
                hoja.Cells["B7"].Style.Font.Size = 12;
                hoja.Cells["B7"].Style.Font.Color.SetColor(Color.Black);
                hoja.Cells["B7"].Value = proyecto;
                hoja.Cells["B7"].Style.Font.Bold = true;

                hoja.Cells["A8:G8"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                hoja.Cells["A8:G8"].Style.Fill.BackgroundColor.SetColor(Color.DarkGray);
                hoja.Cells["A8"].Style.Font.Name = "Arial";
                hoja.Cells["A8"].Style.Font.Size = 12;
                hoja.Cells["A8"].Style.Font.Color.SetColor(Color.Black);
                hoja.Cells["A8"].Value = "Cargado por:";
                hoja.Cells["A8"].Style.Font.Bold = true;
                hoja.Cells["B8"].Style.Font.Name = "Arial";
                hoja.Cells["B8"].Style.Font.Size = 12;
                hoja.Cells["B8"].Style.Font.Color.SetColor(Color.Black);
                hoja.Cells["B8"].Value = usuario;
                hoja.Cells["B8"].Style.Font.Bold = true;

                hoja.Cells["A9:G9"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                hoja.Cells["A9:G9"].Style.Fill.BackgroundColor.SetColor(Color.DarkGray);
                hoja.Cells["A9"].Style.Font.Name = "Arial";
                hoja.Cells["A9"].Style.Font.Size = 12;
                hoja.Cells["A9"].Style.Font.Color.SetColor(Color.Black);
                hoja.Cells["A9"].Value = "Fecha:";
                hoja.Cells["A9"].Style.Font.Bold = true;
                hoja.Cells["B9"].Style.Font.Name = "Arial";
                hoja.Cells["B9"].Style.Font.Size = 12;
                hoja.Cells["B9"].Style.Font.Color.SetColor(Color.Black);
                hoja.Cells["B9"].Value = fecha;
                hoja.Cells["B9"].Style.Font.Bold = true;



                //Cabecera de tabla
                var titleCell = hoja.Cells["A11:G11"]; // Celdas de títulos

                titleCell.Style.Font.Bold = true;
                titleCell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                titleCell.Style.Fill.BackgroundColor.SetColor(Color.DarkGray);
                titleCell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                titleCell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                titleCell.Style.WrapText = true;
                titleCell.Style.Font.Name = "Arial";
                titleCell.Style.Font.Size = 8;

                titleCell.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                titleCell.Style.Border.Right.Color.SetColor(Color.White);

                hoja.Cells["A11"].Value = "ID SAP";
                hoja.Cells["B11"].Value = "Cédula/Pasaporte";
                hoja.Cells["C11"].Value = "Legajo";
                hoja.Cells["D11"].Value = "Apellidos";
                hoja.Cells["E11"].Value = "Nombres";
                hoja.Cells["F11"].Value = "Fecha de baja";
                hoja.Cells["G11"].Value = "Motivo salida";

                foreach (var i in bajas)
                {
                    var body = hoja.Cells["A" + row + ":G" + row];
                    body.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    body.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    body.Style.Font.Name = "Calibri";
                    body.Style.Font.Size = 9;
                    body.Style.WrapText = true;
                    body.Style.Border.BorderAround(ExcelBorderStyle.Dotted, Color.Black);

                    body.Style.Border.Right.Style = ExcelBorderStyle.Dotted;
                    body.Style.Border.Right.Color.SetColor(Color.Black);

                    hoja.Row(row).Height = 13;

                    hoja.Cells["A" + row].Value = i.empleado_id_sap;
                    hoja.Cells["B" + row].Value = i.numero_identificacion;
                    hoja.Cells["C" + row].Value = i.numero_legajo_temporal;
                    hoja.Cells["D" + row].Value = i.apellidos_nombres;
                    hoja.Cells["E" + row].Value = i.nombres;
                    hoja.Cells["F" + row].Value = String.Format("{0:dd/MM/yyyy}", i.fecha_baja);
                    hoja.Cells["G" + row].Value = i.motivo_baja;

                    row++;
                }

                if (es_manual == true)
                {
                    var correos = _correslistarepository.GetAll().Where(c => c.vigente)
                                                              .Where(c => c.ListaDistribucion.vigente)
                                                              .Where(c => c.ListaDistribucion.nombre == CatalogosCodigos.DEFAULT_LISTADISTRIBUCION).ToList();
                    if (correos.Count > 0)
                    {


                        /* ES: Envio de Excel al Correo*/
                        MailMessage message = new MailMessage();
                        message.Subject = "PMDIS: Bajas Colaboradores";
                        foreach (var item in correos)
                        {
                            message.To.Add(item.correo);

                        }

                        excel.SaveAs(new FileInfo(System.Web.HttpContext.Current.Server.MapPath("~/Views/ArchivosColaboradores/FormularioBajasEcuador" + aux + ".xlsx")));

                        string url = System.Web.HttpContext.Current.Server.MapPath("~/Views/ArchivosColaboradores/FormularioBajasEcuador" + aux + ".xlsx");
                        if (File.Exists((string)url))
                        {
                            message.Attachments.Add(new Attachment(url));
                        }
                        /*********/
                        await _correoservice.SendWithFilesAsync(message);


                    }




                    /*System.IO.FileInfo filename = new System.IO.FileInfo(@"C:\CPP\Colaboradores\Bajas\FormularioBajasEcuador" + aux + ".xlsx");
                    excel.SaveAs(filename);*/
                }
                else
                {

                    var correos = _correslistarepository.GetAll().Where(c => c.vigente)
                                                             .Where(c => c.ListaDistribucion.vigente)
                                                             .Where(c => c.ListaDistribucion.nombre == CatalogosCodigos.DEFAULT_LISTADISTRIBUCION).ToList();
                    if (correos.Count > 0)
                    {


                        /* ES: Envio de Excel al Correo*/
                        MailMessage message = new MailMessage();
                        message.Subject = "PMDIS: Bajas Colaboradores";
                        foreach (var item in correos)
                        {
                            message.To.Add(item.correo);

                        }

                        excel.SaveAs(new FileInfo(System.Web.HttpContext.Current.Server.MapPath("~/Views/ArchivosColaboradores/FormularioBajasEcuador" + aux + ".xlsx")));

                        string url = System.Web.HttpContext.Current.Server.MapPath("~/Views/ArchivosColaboradores/FormularioBajasEcuador" + aux + ".xlsx");
                        if (File.Exists((string)url))
                        {
                            message.Attachments.Add(new Attachment(url));
                        }
                        /*********/
                        await _correoservice.SendWithFilesAsync(message);


                    }




                    /*System.IO.FileInfo filename = new System.IO.FileInfo(@"C:\CPP\Colaboradores\Bajas\FormularioBajasEcuador" + aux + ".xlsx");
                    excel.SaveAs(filename);*/
                }


                return "OK";
            }


        }

        public List<ColaboradorBajaDto> GetBajasArchivoIESS()
        {
            var e = 1;
            var query = Repository.GetAll().Where(d => d.enviado_archivo_iess == false && d.vigente == true);

            if (query != null)
            {
                List<ColaboradorBajaDto> bajas = (from c in query
                                                  select new ColaboradorBajaDto
                                                  {
                                                      Id = c.Id,
                                                      ColaboradoresId = c.ColaboradoresId,
                                                      Colaboradores = c.Colaboradores,
                                                      ArchivoId = c.ArchivoId,
                                                      catalogo_motivo_baja_id = c.catalogo_motivo_baja_id,
                                                      fecha_baja = c.fecha_baja,
                                                      detalle_baja = c.detalle_baja,
                                                      requiere_entrevista = c.requiere_entrevista,
                                                      tiene_encuesta = c.tiene_encuesta,
                                                      fecha_pago_liquidacion = c.fecha_pago_liquidacion,
                                                      registro_masivo = c.registro_masivo,
                                                      estado = c.estado,
                                                      motivo_desestimacion = c.motivo_desestimacion,
                                                      vigente = c.vigente,
                                                      motivo_baja = c.MotivoBaja.nombre,
                                                      motivo_baja_codigo = c.MotivoBaja.valor_texto,
                                                      apellidos_nombres = c.Colaboradores.primer_apellido + " " + c.Colaboradores.segundo_apellido
                                                  }).ToList();

                foreach (var i in bajas)
                {
                    i.nro = e++;
                    i.estado_baja = Enum.GetName(typeof(BajaEstado), i.estado);
                }

                return bajas;
            }

            return null;
        }

        public async Task<string> GenerarArchivoIESS(List<ColaboradorBajaDto> bajas, bool es_manual)
        {

            DateTime fechaActual = DateTime.Now;

            var aux = fechaActual.ToString("ddMMyyyyhhmm");
            var fecha = fechaActual.ToString("dd_MM_yyyy_hh_mm");

            var ruc = _parametroRepository.GetAll().Where(c => c.Codigo == "RUC.CPP").Select(c => c.Valor).FirstOrDefault();

            //string fileName = @"C:\CPP\Colaboradores\Bajas\Archivo_IESS_" + fecha + ".txt";

            string fileName = System.Web.HttpContext.Current.Server.MapPath("~/Views/ArchivosColaboradores/Bajas/Archivo_IESS_" + fecha + ".txt");

            using (StreamWriter sw = File.CreateText(fileName))
            {

                foreach (var b in bajas)
                {
                    string proyecto = b.Colaboradores.Proyecto != null ? b.Colaboradores.Proyecto.valor_texto : "SN";
                    string motivo_baja_codigo = b.motivo_baja_codigo != null ? b.motivo_baja_codigo : "SN";
                    sw.WriteLine(
                        ruc + ";"
                        + proyecto + ";"
                        + String.Format("{0:yyyy}", fechaActual) + ";"
                        + String.Format("{0:MM}", fechaActual) + ";"
                        + "SAL;"
                        + b.Colaboradores.numero_identificacion + ";"
                        + String.Format("{0:yyyyMMdd}", b.Colaboradores.fecha_ingreso) + ";" +
                        motivo_baja_codigo + ";"
                        + "00000000"
                        + ""
                        );
                }
            }

            var correos = _correslistarepository.GetAll().Where(c => c.vigente)
                                                              .Where(c => c.ListaDistribucion.vigente)
                                                              .Where(c => c.ListaDistribucion.nombre == CatalogosCodigos.DEFAULT_LISTADISTRIBUCION).ToList();
            if (correos.Count > 0)
            {

                MailMessage message = new MailMessage();
                message.Subject = "PMDIS: Colaboradores Bajas";
                foreach (var item in correos)
                {
                    message.To.Add(item.correo);

                }
                if (File.Exists((string)fileName))
                {
                    message.Attachments.Add(new Attachment(fileName));
                }
                /*********/
                await _correoservice.SendWithFilesAsync(message);
            }

            return "OK";
        }

        public void SubirPdf(int ColaboradorBajaId, ArchivoDto archivo)
        {
            var baja = Repository.Get(ColaboradorBajaId);
            if (baja.ArchivoId > 0)
            {
                var archivoId = baja.ArchivoId;
                var file = baja.Archivo;

                baja.ArchivoId = null;
                Repository.Update(baja);
                _archivoRepository.Delete(file);

            }
            var entity = _archivoRepository.Insert(Mapper.Map<ArchivoDto, Archivo>(archivo));


            baja.Archivo = entity;
            Repository.Update(baja);
        }

        public void SubirPago(int ColaboradorBajaId, ArchivoDto archivo)
        {
            var baja = Repository.Get(ColaboradorBajaId);
            if (baja.archivo_liquidacion_id > 0)
            {
                var archivoId = baja.archivo_liquidacion_id;
                var file = baja.ArchivoLiquidacion;

                baja.archivo_liquidacion_id = null;
                Repository.Update(baja);
                _archivoRepository.Delete(file);

            }
            var entity = _archivoRepository.Insert(Mapper.Map<ArchivoDto, Archivo>(archivo));


            baja.ArchivoLiquidacion = entity;
            Repository.Update(baja);
        }

        public bool InsertarBajaColaborador(ColaboradorBajaModel baja)
        {
            ColaboradorBaja e = new ColaboradorBaja();
            e.Id = 0;
            e.ColaboradoresId = baja.ColaboradoresId;
            e.estado = BajaEstado.ENVIAR_SAP;
            e.requiere_entrevista = baja.requiere_entrevista;
            e.detalle_baja = baja.detalle_baja;
            e.fecha_baja = baja.fecha_baja;
            e.catalogo_motivo_baja_id = baja.catalogo_motivo_baja_id;

            if (baja.UploadedFile != null)
            {

                string fileName = baja.UploadedFile.FileName;
                string fileContentType = baja.UploadedFile.ContentType;
                byte[] fileBytes = new byte[baja.UploadedFile.ContentLength];
                var data = baja.UploadedFile.InputStream.Read(fileBytes, 0,
                    Convert.ToInt32(baja.UploadedFile.ContentLength));

                Archivo n = new Archivo
                {
                    Id = 0,
                    codigo = "COL_BAJA_" + baja.ColaboradoresId,
                    nombre = fileName,
                    vigente = true,
                    fecha_registro = DateTime.Now,
                    hash = fileBytes,
                    tipo_contenido = fileContentType,
                };
                var archivoid = _archivoRepository.InsertAndGetId(n);
                e.ArchivoId = archivoid;
            }
            var idbaja = Repository.InsertAndGetId(e);
            if (idbaja > 0)
            {
                var colaborador = _colaboradoresRepository.Get(baja.ColaboradoresId);
                colaborador.estado = RRHHCodigos.ESTADO_INACTIVO;
                var r = _colaboradoresRepository.Update(colaborador);
                return true;
            }
            else
            {
                return false;
            }




        }

        public bool UpdateColaboradorBaja(int Id)
        {
            var e = Repository.Get(Id);
            if (e != null && e.Id > 0)
            {
                e.estado = proyecto.dominio.BajaEstado.ENVIADO_SAP;
                e.fecha_generacion_archivo_sap = DateTime.Now;
                e.envio_manual = true;
                Repository.Update(e);
                return true;

            }
            else {
                return false;
            }
        }
    }
}
