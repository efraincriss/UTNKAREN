using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.aplicacion.CertificacionIngenieria.Dto;
using com.cpp.calypso.proyecto.aplicacion.CertificacionIngenieria.Interface;
using com.cpp.calypso.proyecto.aplicacion.CertificacionIngenieria.Models;
using com.cpp.calypso.proyecto.aplicacion.Interfaces;
using com.cpp.calypso.proyecto.dominio;
using com.cpp.calypso.proyecto.dominio.CertificacionIngenieria;
using com.cpp.calypso.proyecto.dominio.Constantes;
using com.cpp.calypso.proyecto.dominio.Models;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;


namespace com.cpp.calypso.proyecto.aplicacion.CertificacionIngenieria.Service
{
    public class AvancePorcentajeProyectoAsyncBaseCrudAppService : AsyncBaseCrudAppService<AvancePorcentajeProyecto, AvancePorcentajeProyectoDto, PagedAndFilteredResultRequestDto>, IAvancePorcentajeProyectoAsyncBaseCrudAppService
    {
        private readonly IBaseRepository<Proyecto> _proyectoRepository;
        private readonly IBaseRepository<Catalogo> _catalogoRepository;
        private readonly IBaseRepository<CertificadoIngenieriaProyecto> _certificadoRepository;
        private readonly IBaseRepository<ComentarioCertificado> _comentarioRepository;
        public AvancePorcentajeProyectoAsyncBaseCrudAppService(
            IBaseRepository<AvancePorcentajeProyecto> repository,
                           IBaseRepository<Proyecto> proyectoRepository,
               IBaseRepository<Catalogo> catalogoRepository,
               IBaseRepository<CertificadoIngenieriaProyecto> certificadoRepository,
               IBaseRepository<ComentarioCertificado> comentarioRepository

        ) : base(repository)
        {
            _proyectoRepository = proyectoRepository;
            _catalogoRepository = catalogoRepository;
            _certificadoRepository = certificadoRepository;
            _comentarioRepository = comentarioRepository;
        }

        public bool ActualizarDetalle(AvancePorcentajeProyecto input)
        {
            var entity = Repository.Get(input.Id);
            entity.FechaCertificado = input.FechaCertificado;
            entity.ProyectoId = input.ProyectoId;
            entity.AvancePrevistoActualIB = input.AvancePrevistoActualIB;
            entity.AvancePrevistoAnteriorIB = input.AvancePrevistoAnteriorIB;
            entity.AvanceRealActualIB = input.AvanceRealActualIB;
            entity.AvanceRealAnteriorIB = input.AvanceRealAnteriorIB;
            entity.AvancePrevistoActualID = input.AvancePrevistoActualID;
            entity.AvancePrevistoAnteriorID = input.AvancePrevistoAnteriorID;
            entity.AvanceRealActualID = input.AvanceRealActualID;
            entity.AvanceRealAnteriorID = input.AvanceRealAnteriorID;
            entity.Justificacion = input.Justificacion;

            var result = Repository.Update(entity);
            return result.Id > 0 ? true : false;
        }

        public ExcelPackage CargaMasiva(HttpPostedFileBase uploadedFile)
        {
            if (uploadedFile != null)
            {
                if (uploadedFile.ContentType == "application/vnd.ms-excel" || uploadedFile.ContentType ==
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                {
                    ExcelPackage excel = new ExcelPackage();

                    string fileContentType = uploadedFile.ContentType;
                    byte[] fileBytes = new byte[uploadedFile.ContentLength];

                    ExcelWorksheet detalles = null;
                    ExcelWorksheet catalogos = null;

                    using (var package = new ExcelPackage(uploadedFile.InputStream))
                    {
                        detalles = excel.Workbook.Worksheets.Add("DETALLES", package.Workbook.Worksheets[1]);
                        catalogos = excel.Workbook.Worksheets.Add("PROYECTOS", package.Workbook.Worksheets[2]);

                        var numberOfColumns = detalles.Dimension.End.Column;
                        var numberOfRows = detalles.Dimension.End.Row;
                        string celdaResultado = "X";
                        var fechaArchivoString = (detalles.Cells["A" + 1].Text ?? "").ToString();

                        if (fechaArchivoString == null)
                        {
                            return new ExcelPackage();
                        }


                        for (int rowIterator = 7; rowIterator <= numberOfRows; rowIterator++)
                        {


                            DateTime fechaCertificado = DateTime.Parse(fechaArchivoString);




                            var CodigoProyecto = (detalles.Cells["B" + rowIterator].Value ?? "").ToString();
                            if (CodigoProyecto == "")
                            {
                                detalles.Cells[celdaResultado + rowIterator].Value = "El Codigo del Proyecto es Obligatorio";
                                detalles.Cells[celdaResultado + rowIterator].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                detalles.Cells[celdaResultado + rowIterator].Style.Fill.BackgroundColor.SetColor(Color.Red);
                                detalles.Cells[celdaResultado + rowIterator].Style.Font.Color.SetColor(Color.White);
                                continue;
                            }

                            string CodigoProyectoString = CodigoProyecto.Replace(" ", "").Trim().TrimEnd().TrimStart();
                            var ProyectoExistente = _proyectoRepository.GetAll().Where(c => c.codigo.Trim().TrimEnd().TrimStart() == CodigoProyectoString || c.codigo_cliente == CodigoProyectoString || c.codigo_interno == CodigoProyectoString).FirstOrDefault();
                            if (ProyectoExistente == null)
                            {
                                detalles.Cells[celdaResultado + rowIterator].Value = "No se encontro proyecto registrado con el codigo " + CodigoProyecto;
                                detalles.Cells[celdaResultado + rowIterator].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                detalles.Cells[celdaResultado + rowIterator].Style.Fill.BackgroundColor.SetColor(Color.Red);
                                detalles.Cells[celdaResultado + rowIterator].Style.Font.Color.SetColor(Color.White);
                                continue;
                            }


                            var AvancePrevistoIBString = (detalles.Cells["I" + rowIterator].Value ?? "").ToString();
                            var AvancePrevistoIDString = (detalles.Cells["J" + rowIterator].Value ?? "").ToString();
                            var AvanceRealIBString = (detalles.Cells["G" + rowIterator].Value ?? "").ToString();
                            var AvanceRealIDString = (detalles.Cells["H" + rowIterator].Value ?? "").ToString();
                            var AsbuiltString = (detalles.Cells["K" + rowIterator].Value ?? "").ToString();


                            var AvancePrevistoIB = Decimal.Parse(AvancePrevistoIBString == "" || AvancePrevistoIBString == "NA" ? "0" : AvancePrevistoIBString, NumberStyles.Float);
                            var AvancePrevistoID = Decimal.Parse(AvancePrevistoIDString == "" || AvancePrevistoIDString == "NA" ? "0" : AvancePrevistoIDString, NumberStyles.Float);
                            var AvanceRealIB = Decimal.Parse(AvanceRealIBString == "" || AvanceRealIBString == "NA" ? "0" : AvanceRealIBString, NumberStyles.Float);
                            var AvanceRealID = Decimal.Parse(AvanceRealIDString == "" || AvanceRealIDString == "NA" ? "0" : AvanceRealIDString, NumberStyles.Float);
                            var AsbuiltActual = Decimal.Parse(AsbuiltString == "" || AsbuiltString == "NA" ? "0" : AsbuiltString, NumberStyles.Float);

                            var unaFase = AvanceRealIBString == "NA" || AvanceRealIDString == "NA" ? true : false;

                            var FechaCertificadoDate = fechaCertificado.Date;
                            var AvanceProyectoAnterior = Repository.GetAll()
                                                               .Where(c => c.ProyectoId == ProyectoExistente.Id)
                                                               .Where(c => c.FechaCertificado < FechaCertificadoDate)
                                                               .OrderByDescending(c => c.FechaCertificado)
                                                               .FirstOrDefault();


                            var AvanceProyectoActual = Repository.GetAll()
                                                             .Where(c => c.ProyectoId == ProyectoExistente.Id)
                                                             .Where(c => c.FechaCertificado == FechaCertificadoDate)
                                                             .OrderByDescending(c => c.FechaCertificado)
                                                             .FirstOrDefault();

                            var comentario = (detalles.Cells["W" + rowIterator].Value ?? "").ToString();

                            var entityComentario = _comentarioRepository.GetAll().Where(x => x.ProyectoId == ProyectoExistente.Id)
                                                                               .Where(x => x.FechaCarga == FechaCertificadoDate)
                                                                               .OrderByDescending(x => x.FechaCarga)
                                                                               .FirstOrDefault();
                            if (entityComentario != null)
                            {

                                var entity = _comentarioRepository.Get(entityComentario.Id);
                                entity.Comentario = comentario;
                            }
                            else
                            {
                                var insert = new ComentarioCertificado()
                                {
                                    Comentario = comentario,
                                    ProyectoId = ProyectoExistente.Id,
                                    FechaCarga = FechaCertificadoDate

                                };
                                _comentarioRepository.Insert(insert);

                            }

                            if (AvanceProyectoActual == null)
                            {


                                var entity = new AvancePorcentajeProyecto()
                                {
                                    Id = 0,
                                    AvancePrevistoAnteriorIB = AvanceProyectoAnterior != null ? AvanceProyectoAnterior.AvancePrevistoActualIB : 0, //Valor Actual del Registro Anterior BDD
                                    AvancePrevistoAnteriorID = AvanceProyectoAnterior != null ? AvanceProyectoAnterior.AvancePrevistoActualID : 0,
                                    AvancePrevistoActualIB = AvancePrevistoIB, //Valor Nuevo del Archivo
                                    AvancePrevistoActualID = AvancePrevistoID, //Valor Nuevo del Archivo
                                    AvanceRealAnteriorID = AvanceProyectoAnterior != null ? AvanceProyectoAnterior.AvanceRealActualID : 0, //Valor Actual del Registro Anterior
                                    AvanceRealAnteriorIB = AvanceProyectoAnterior != null ? AvanceProyectoAnterior.AvanceRealActualIB : 0,

                                    AvanceRealActualID = AvanceRealID,
                                    AvanceRealActualIB = AvanceRealIB,
                                    AsbuiltAnterior = AvanceProyectoAnterior != null ? AvanceProyectoAnterior.AsbuiltActual : 0,
                                    AsbuiltActual = AsbuiltActual,



                                    ProyectoId = ProyectoExistente.Id,
                                    FechaCertificado = fechaCertificado,
                                    CertificadoId = null,
                                    Justificacion = "",
                                    unaFase=unaFase
                                };
                                Repository.Insert(entity);


                                //* Actualizar Certificado Vigente*/

                                var CertificadoProyecto = _certificadoRepository.GetAll().Where(c => c.ProyectoId == ProyectoExistente.Id)
                                                                                         .Where(c => c.GrupoCertificadoIngenieria.FechaCertificado == FechaCertificadoDate)
                                                                                         .FirstOrDefault();

                                if (CertificadoProyecto != null)
                                {
                                    decimal avanceRealIngenieria = Decimal.Parse("0");
                                    double PorcentajeIB = 0.30; //Porcentaje Ingenieria Basica
                                    double PorcentajeID = 0.70;//Porcentaje Ingenieria Detalle

                                    var valorIB = AvanceRealIB * Convert.ToDecimal(PorcentajeIB);
                                    var valorID = AvanceRealID * Convert.ToDecimal(PorcentajeID);
                                    //avanceRealIngenieria = valorIB + valorID;

                                    avanceRealIngenieria = (valorIB + valorID) * Convert.ToDecimal(0.95) * AsbuiltActual * Convert.ToDecimal(0.05);

                                    var entityCertificado = _certificadoRepository.Get(CertificadoProyecto.Id);
                                    entityCertificado.AvanceRealIngenieria = avanceRealIngenieria;
                                    entityCertificado.PorcentajeAsbuilt = AsbuiltActual;
                                }


                            }
                            else
                            {
                                var entity = Repository.Get(AvanceProyectoActual.Id);
                                entity.AvancePrevistoAnteriorIB = AvanceProyectoAnterior != null ? AvanceProyectoAnterior.AvancePrevistoActualIB : 0; //Valor Actual del Registro Anterior BDD
                                entity.AvancePrevistoAnteriorID = AvanceProyectoAnterior != null ? AvanceProyectoAnterior.AvancePrevistoActualID : 0;
                                entity.AvancePrevistoActualIB = AvancePrevistoIB; //Valor Nuevo del Archivo
                                entity.AvancePrevistoActualID = AvancePrevistoID; //Valor Nuevo del Archivo
                                entity.AvanceRealAnteriorID = AvanceProyectoAnterior != null ? AvanceProyectoAnterior.AvanceRealActualID : 0; //Valor Actual del Registro Anterior
                                entity.AvanceRealAnteriorIB = AvanceProyectoAnterior != null ? AvanceProyectoAnterior.AvanceRealActualIB : 0;
                                entity.AvanceRealActualID = AvanceRealID;
                                entity.AvanceRealActualIB = AvanceRealIB;

                                entity.AsbuiltAnterior = AvanceProyectoAnterior != null ? AvanceProyectoAnterior.AsbuiltActual : 0;
                                entity.AsbuiltActual = AsbuiltActual;
                                entity.unaFase = unaFase;

                                Repository.Update(entity);



                                //* Actualizar Certificado Vigente*/

                                var CertificadoProyecto = _certificadoRepository.GetAll().Where(c => c.ProyectoId == ProyectoExistente.Id)
                                                                                         .Where(c => c.GrupoCertificadoIngenieria.FechaCertificado == FechaCertificadoDate)
                                                                                         .FirstOrDefault();

                                if (CertificadoProyecto != null)
                                {
                                    decimal avanceRealIngenieria = Decimal.Parse("0");
                                    double PorcentajeIB = 0.30; //Porcentaje Ingenieria Basica
                                    double PorcentajeID = 0.70;//Porcentaje Ingenieria Detalle

                                    var valorIB = AvanceRealIB * Convert.ToDecimal(PorcentajeIB);
                                    var valorID = AvanceRealID * Convert.ToDecimal(PorcentajeID);

                                    //avanceRealIngenieria = valorIB + valorID;
                                    avanceRealIngenieria = (valorIB + valorID) * Convert.ToDecimal(0.95) * AsbuiltActual * Convert.ToDecimal(0.05);

                                    

                                    var entityCertificado = _certificadoRepository.Get(CertificadoProyecto.Id);
                                    entityCertificado.AvanceRealIngenieria = avanceRealIngenieria;
                                    entityCertificado.PorcentajeAsbuilt = AsbuiltActual;
                                }
                            }

                            detalles.Cells[celdaResultado + rowIterator].Value = "Actualizado Correctamente";
                        }
                    }
                    return excel;
                }

                return new ExcelPackage();
            }

            return new ExcelPackage();
        }

        public bool CrearDetalle(AvancePorcentajeProyecto input)
        {

            var fechaCargaDate = input.FechaCertificado.Value.Date;

            var exist = Repository.GetAll()
                                  .Where(c => c.FechaCertificado.HasValue)
                                  .Where(c => c.FechaCertificado.Value >= fechaCargaDate)
                                  .Where(c => c.ProyectoId == input.ProyectoId)
                                  .FirstOrDefault();

            var avanceProyectoAnterior = Repository.GetAll()
                                  .Where(c => c.FechaCertificado.HasValue)
                                  .Where(c => c.FechaCertificado.Value < fechaCargaDate)
                                  .Where(c => c.ProyectoId == input.ProyectoId)
                                  .OrderByDescending(c => c.FechaCertificado)
                                  .FirstOrDefault();

            if (avanceProyectoAnterior != null)
            {
                input.AvancePrevistoAnteriorIB = avanceProyectoAnterior.AvancePrevistoActualIB;
                input.AvancePrevistoAnteriorID = avanceProyectoAnterior.AvancePrevistoActualID;
                input.AvanceRealAnteriorIB = avanceProyectoAnterior.AvanceRealActualIB;
                input.AvanceRealAnteriorID = avanceProyectoAnterior.AvanceRealActualID;
                input.AsbuiltAnterior = avanceProyectoAnterior.AsbuiltActual;
            }
            if (exist != null)
            {
                return false;
            }
            var result = Repository.InsertAndGetId(input);
            return result > 0 ? true : false;
        }

        public string DeleteDetalle(int Id)
        {
            var entity = Repository.GetAll().Where(c => c.Id == Id).FirstOrDefault();
            var fechaCargaDate = entity.FechaCertificado.Value.Date;
            var exist = Repository.GetAll().Where(c => c.FechaCertificado.HasValue).Where(c => c.FechaCertificado.Value > fechaCargaDate)
       .Where(c => c.ProyectoId == entity.ProyectoId).FirstOrDefault();

            if (exist == null)
            {
                Repository.Delete(entity);
                return "OK";
            }
            else
            {
                return "CERTIFICADO_MAYOR";
            }




        }

        public ExcelPackage DescargarPlantillaCargaMasiva()
        {
            ExcelPackage excel = new ExcelPackage();

            string filename = System.Web.HttpContext.Current.Server.MapPath("~/Views/PlantillaWord/CertificacionIngenieria/FormatoCargaPuntual.xlsx");
            if (File.Exists((string)filename))
            {
                FileInfo newFile = new FileInfo(filename);
                ExcelPackage pck = new ExcelPackage(newFile);
                excel.Workbook.Worksheets.Add("DETALLES", pck.Workbook.Worksheets[1]);
                excel.Workbook.Worksheets.Add("PROYECTOS", pck.Workbook.Worksheets[2]);
            }

            ExcelWorksheet detalles = excel.Workbook.Worksheets[1];
            ExcelWorksheet catalogos = excel.Workbook.Worksheets[2];

            return excel;
        }

        public AvancePorcentajeProyectoDto ObtenerDato(int ProyectoId, DateTime fecha)
        {
            var fechaDate = fecha.Date;
            var proyecto = _proyectoRepository.GetAllIncluding(c => c.Contrato).Where(c => c.Id == ProyectoId).FirstOrDefault();
            var q = Repository.GetAllIncluding(c => c.Proyecto.Contrato)
                              .Where(c => c.FechaCertificado.HasValue)
                              .Where(c => c.ProyectoId == ProyectoId)
                              .Where(c => c.FechaCertificado.Value < fechaDate)
                              .OrderByDescending(c => c.FechaCertificado)
                              .FirstOrDefault();
            if (q != null)
            {

                var dto = new AvancePorcentajeProyectoDto()
                {
                    Id = q.Id,
                    CertificadoId = q.CertificadoId,
                    FechaCertificado = q.FechaCertificado,
                    formatFechaCertificado = q.FechaCertificado.HasValue ? q.FechaCertificado.Value.ToShortDateString() : "",
                    AvancePrevistoActualIB = q.AvancePrevistoActualIB,
                    AvancePrevistoAnteriorIB = q.AvancePrevistoAnteriorIB,
                    AvanceRealActualIB = q.AvanceRealActualIB,
                    AvanceRealAnteriorIB = q.AvanceRealAnteriorIB,
                    AvancePrevistoActualID = q.AvancePrevistoActualID,
                    AvancePrevistoAnteriorID = q.AvancePrevistoAnteriorID,
                    AvanceRealActualID = q.AvanceRealActualID,
                    AvanceRealAnteriorID = q.AvanceRealAnteriorID,
                    AsbuiltAnterior = q.AsbuiltAnterior,
                    AsbuiltActual = q.AsbuiltActual,

                    Justificacion = q.Justificacion,
                    nombreProyecto = q.Proyecto.codigo,
                    nombreContrato = q.Proyecto.Contrato.Codigo,
                    ProyectoId = q.ProyectoId,
                    nombreCertificado = q.CertificadoId.HasValue ? "SI" : "NO"
                };
                return dto;
            }
            else
            {
                var a = new AvancePorcentajeProyectoDto();
                if (proyecto != null)
                {
                    a.nombreContrato = proyecto.Contrato.Codigo;
                    a.nombreProyecto = proyecto.codigo_cliente + " / " + proyecto.nombre_proyecto + " / " + proyecto.codigo;
                }
                return a;

            }
        }

        public List<AvancePorcentajeProyectoDto> ObtenerDetalles(DateTime? FechaCarga)
        {
            var query = Repository.GetAllIncluding(c => c.Proyecto.Contrato).ToList();
            if (FechaCarga.HasValue)
            {
                var FechaCargaDate = FechaCarga.Value.Date;

                query = query.Where(c => c.FechaCertificado.HasValue).Where(c => c.FechaCertificado == FechaCargaDate).ToList();
            }



            var list = (from q in query
                        select new AvancePorcentajeProyectoDto()
                        {
                            Id = q.Id,
                            CertificadoId = q.CertificadoId,
                            FechaCertificado = q.FechaCertificado,
                            formatFechaCertificado = q.FechaCertificado.HasValue ? q.FechaCertificado.Value.ToShortDateString() : "",
                            AvancePrevistoActualIB = q.AvancePrevistoActualIB,
                            AvancePrevistoAnteriorIB = q.AvancePrevistoAnteriorIB,
                            AvanceRealActualIB = q.AvanceRealActualIB,
                            AvanceRealAnteriorIB = q.AvanceRealAnteriorIB,
                            AvancePrevistoActualID = q.AvancePrevistoActualID,
                            AvancePrevistoAnteriorID = q.AvancePrevistoAnteriorID,
                            AvanceRealActualID = q.AvanceRealActualID,
                            AvanceRealAnteriorID = q.AvanceRealAnteriorID,
                            AsbuiltActual = q.AsbuiltActual,
                            AsbuiltAnterior = q.AsbuiltAnterior,
                            Justificacion = q.Justificacion,
                            nombreProyecto = q.Proyecto.codigo,
                            nombreContrato = q.Proyecto.Contrato.Codigo,
                            ProyectoId = q.ProyectoId,
                            nombreCertificado = q.CertificadoId.HasValue ? "SI" : "NO"


                        }).ToList();
            return list;
        }

        public List<ModelClassReact> ObtenerProyectos()
        {
            var query = _proyectoRepository.GetAll().Where(c => c.vigente).ToList();

            var proyectos = (from p in query
                             select new ModelClassReact()
                             {
                                 dataKey = p.Id,
                                 label = p.codigo + " - " + p.nombre_proyecto,
                                 value = p.Id

                             }).ToList();
            return proyectos;
        }
    }
}
