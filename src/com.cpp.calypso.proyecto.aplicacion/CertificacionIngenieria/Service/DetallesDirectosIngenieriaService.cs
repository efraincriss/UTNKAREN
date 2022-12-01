using AutoMapper;
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
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace com.cpp.calypso.proyecto.aplicacion.CertificacionIngenieria.Service
{
    public class DetallesDirectosIngenieriaAsyncBaseCrudAppService : AsyncBaseCrudAppService<DetallesDirectosIngenieria, DetallesDirectosIngenieriaDto, PagedAndFilteredResultRequestDto>, IDetallesDirectosIngenieriaAsyncBaseCrudAppService
    {
        public ICatalogoAsyncBaseCrudAppService _catalogoService { get; }
        private readonly IBaseRepository<Colaboradores> _colaboradoresRepository;
        private readonly IBaseRepository<Proyecto> _proyectoRepository;
        private readonly IBaseRepository<Catalogo> _catalogoRepository;
        private readonly IBaseRepository<DetalleDirectoE500> _e500Repository;
        private readonly IBaseRepository<CargaTimesheet> _cargaTimesheetRepository;
        private readonly IBaseRepository<CorreoLista> _correosListaRepository;
        private readonly IdentityEmailMessageService _correoService;

        public DetallesDirectosIngenieriaAsyncBaseCrudAppService(
               IBaseRepository<DetallesDirectosIngenieria> repository,
               ICatalogoAsyncBaseCrudAppService catalogoService,
               IBaseRepository<Colaboradores> colaboradoresRepository,
               IBaseRepository<Proyecto> proyectoRepository,
                IBaseRepository<DetalleDirectoE500> e500Repository,
               IBaseRepository<Catalogo> catalogoRepository,
               IBaseRepository<CargaTimesheet> cargaTimesheetRepository,
               IdentityEmailMessageService correoService,
               IBaseRepository<CorreoLista> correosListaRepository

           ) : base(repository)
        {
            _catalogoService = catalogoService;
            _colaboradoresRepository = colaboradoresRepository;
            _proyectoRepository = proyectoRepository;
            _catalogoRepository = catalogoRepository;
            _e500Repository = e500Repository;

            _cargaTimesheetRepository = cargaTimesheetRepository;
            _correosListaRepository = correosListaRepository;
            _correoService = correoService;
        }

        public ExcelPackage CargaMasivaDetallesIngenieria(HttpPostedFileBase uploadedFile)
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
                        catalogos = excel.Workbook.Worksheets.Add("CATÁLOGOS", package.Workbook.Worksheets[2]);

                        var numberOfColumns = detalles.Dimension.End.Column;
                        var numberOfRows = detalles.Dimension.End.Row;

                        for (int rowIterator = 2; rowIterator <= numberOfRows; rowIterator++)
                        {
                            /* Validar ingreso de la identificacion*/
                            var identificacion = (detalles.Cells["B" + rowIterator].Value ?? "").ToString();
                            if (identificacion == "")
                            {
                                detalles.Cells["Q" + rowIterator].Value = "Debe ingresar la identifiación del colaborador";
                                detalles.Cells["Q" + rowIterator].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                detalles.Cells["Q" + rowIterator].Style.Fill.BackgroundColor.SetColor(Color.Red);
                                detalles.Cells["Q" + rowIterator].Style.Font.Color.SetColor(Color.White);
                                continue;
                            }

                            /* Buscar el colaborador */
                            var colaborador = _colaboradoresRepository
                                .GetAll()
                                .OrderByDescending(c => c.fecha_ingreso)
                                .FirstOrDefault(o => o.numero_identificacion == identificacion);
                            if (colaborador == null)
                            {
                                detalles.Cells["Q" + rowIterator].Value = "No se encontró al colaborador";
                                detalles.Cells["Q" + rowIterator].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                detalles.Cells["Q" + rowIterator].Style.Fill.BackgroundColor.SetColor(Color.Red);
                                detalles.Cells["Q" + rowIterator].Style.Font.Color.SetColor(Color.White);
                                continue;
                            }

                            var tipoRegistro = (detalles.Cells["C" + rowIterator].Value ?? "").ToString();
                            if (tipoRegistro == "")
                            {
                                detalles.Cells["Q" + rowIterator].Value = "Debe seleccionar el tipo de Registro";
                                detalles.Cells["Q" + rowIterator].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                detalles.Cells["Q" + rowIterator].Style.Fill.BackgroundColor.SetColor(Color.Red);
                                detalles.Cells["Q" + rowIterator].Style.Font.Color.SetColor(Color.White);
                                continue;
                            }

                            var catalogoTipoRegistro = _catalogoRepository.GetAll().Where(c => c.nombre == tipoRegistro)
                                                                          .Where(c => c.TipoCatalogo.codigo == CertificacionIngenieriaCodigos.TIPO_REGISTRO_INGENIERIA)
                                                                          .FirstOrDefault();
                            if (catalogoTipoRegistro == null)
                            {
                                detalles.Cells["Q" + rowIterator].Value = "El Tipo De Registro no existe en el catálogo";
                                detalles.Cells["Q" + rowIterator].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                detalles.Cells["Q" + rowIterator].Style.Fill.BackgroundColor.SetColor(Color.Red);
                                detalles.Cells["Q" + rowIterator].Style.Font.Color.SetColor(Color.White);
                                continue;
                            }


                            var WO = (detalles.Cells["D" + rowIterator].Value ?? "").ToString();


                            var HH = (detalles.Cells["E" + rowIterator].Value ?? "").ToString();
                            if (HH == "")
                            {
                                detalles.Cells["Q" + rowIterator].Value = "Debe Ingresar Numero Horas";
                                detalles.Cells["Q" + rowIterator].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                detalles.Cells["Q" + rowIterator].Style.Fill.BackgroundColor.SetColor(Color.Red);
                                detalles.Cells["Q" + rowIterator].Style.Font.Color.SetColor(Color.White);
                                continue;
                            }
                            var numeroHoras = Decimal.Parse(HH, NumberStyles.Float);


                            var Ejecutante = (detalles.Cells["F" + rowIterator].Value ?? "").ToString();



                            /* Validar el ingreso de la fecha Trabajo*/
                            var fechaTrabajoString = (detalles.Cells["G" + rowIterator].Text ?? "").ToString();
                            if (fechaTrabajoString == "")
                            {
                                detalles.Cells["Q" + rowIterator].Value = "Debe ingresar la fecha de inicio";
                                detalles.Cells["Q" + rowIterator].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                detalles.Cells["Q" + rowIterator].Style.Fill.BackgroundColor.SetColor(Color.Red);
                                detalles.Cells["Q" + rowIterator].Style.Font.Color.SetColor(Color.White);
                                continue;
                            }
                            DateTime fechaTrabajo = DateTime.Parse(fechaTrabajoString);



                            var Observaciones = (detalles.Cells["H" + rowIterator].Value ?? "").ToString();


                            var Etapa = (detalles.Cells["I" + rowIterator].Value ?? "").ToString();
                            if (Etapa == "")
                            {
                                detalles.Cells["Q" + rowIterator].Value = "Debe seleccionar Etapa";
                                detalles.Cells["Q" + rowIterator].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                detalles.Cells["Q" + rowIterator].Style.Fill.BackgroundColor.SetColor(Color.Red);
                                detalles.Cells["Q" + rowIterator].Style.Font.Color.SetColor(Color.White);
                                continue;
                            }

                            var catalogoEtapa = _catalogoRepository.GetAll().Where(c => c.nombre == Etapa)
                                 .Where(c => c.TipoCatalogo.codigo == CertificacionIngenieriaCodigos.ETAPAS_INGENIERIA_TIMESHEET)
                                 .FirstOrDefault();
                            if (catalogoEtapa == null)
                            {
                                detalles.Cells["Q" + rowIterator].Value = "La Etapa no existe en el catálogo";
                                detalles.Cells["Q" + rowIterator].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                detalles.Cells["Q" + rowIterator].Style.Fill.BackgroundColor.SetColor(Color.Red);
                                detalles.Cells["Q" + rowIterator].Style.Font.Color.SetColor(Color.White);
                                continue;
                            }

                            var Proyecto1 = (detalles.Cells["J" + rowIterator].Value ?? "").ToString();

                            var Proyecto2 = (detalles.Cells["K" + rowIterator].Value ?? "").ToString();

                            var CodigoProyecto = (detalles.Cells["L" + rowIterator].Value ?? "").ToString();
                            if (CodigoProyecto == "")
                            {
                                detalles.Cells["Q" + rowIterator].Value = "El Codigo del Proyecto es Obligatorio";
                                detalles.Cells["Q" + rowIterator].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                detalles.Cells["Q" + rowIterator].Style.Fill.BackgroundColor.SetColor(Color.Red);
                                detalles.Cells["Q" + rowIterator].Style.Font.Color.SetColor(Color.White);
                                continue;
                            }
                            if (CodigoProyecto == "E500")
                            {
                                detalles.Cells["Q" + rowIterator].Value = "No se debe registrar lineas del proyecto E500";
                                detalles.Cells["Q" + rowIterator].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                detalles.Cells["Q" + rowIterator].Style.Fill.BackgroundColor.SetColor(Color.Red);
                                detalles.Cells["Q" + rowIterator].Style.Font.Color.SetColor(Color.White);
                                continue;
                            }

                            var ProyectoExistente = _proyectoRepository.GetAll().Where(c => c.codigo_cliente == CodigoProyecto || c.codigo_interno == CodigoProyecto).FirstOrDefault();
                            if (ProyectoExistente == null)
                            {
                                detalles.Cells["Q" + rowIterator].Value = "No se encontro proyecto registrado con el codigo " + CodigoProyecto;
                                detalles.Cells["Q" + rowIterator].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                detalles.Cells["Q" + rowIterator].Style.Fill.BackgroundColor.SetColor(Color.Red);
                                detalles.Cells["Q" + rowIterator].Style.Font.Color.SetColor(Color.White);
                                continue;
                            }


                            var Especialidad = (detalles.Cells["M" + rowIterator].Value ?? "").ToString();
                            if (Especialidad == "")
                            {
                                detalles.Cells["Q" + rowIterator].Value = "Debe seleccionar Especialidad";
                                detalles.Cells["Q" + rowIterator].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                detalles.Cells["Q" + rowIterator].Style.Fill.BackgroundColor.SetColor(Color.Red);
                                detalles.Cells["Q" + rowIterator].Style.Font.Color.SetColor(Color.White);
                                continue;
                            }

                            var catalogoEspecialidad = _catalogoRepository.GetAll().Where(c => c.nombre == Especialidad)
                                 .Where(c => c.TipoCatalogo.codigo == CertificacionIngenieriaCodigos.ESPECIALIDAD_INGENIERIA_TIMESHEET)
                                 .FirstOrDefault();
                            if (catalogoEspecialidad == null)
                            {
                                detalles.Cells["Q" + rowIterator].Value = "La Especialidad no existe en el catálogo";
                                detalles.Cells["Q" + rowIterator].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                detalles.Cells["Q" + rowIterator].Style.Fill.BackgroundColor.SetColor(Color.Red);
                                detalles.Cells["Q" + rowIterator].Style.Font.Color.SetColor(Color.White);
                                continue;
                            }


                            var Locacion = (detalles.Cells["N" + rowIterator].Value ?? "").ToString();
                            if (Locacion == "")
                            {
                                detalles.Cells["Q" + rowIterator].Value = "Debe seleccionar locación";
                                detalles.Cells["Q" + rowIterator].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                detalles.Cells["Q" + rowIterator].Style.Fill.BackgroundColor.SetColor(Color.Red);
                                detalles.Cells["Q" + rowIterator].Style.Font.Color.SetColor(Color.White);
                                continue;
                            }

                            var catalogoLocacion = _catalogoRepository.GetAll().Where(c => c.nombre == Locacion)
                                 .Where(c => c.TipoCatalogo.codigo == CertificacionIngenieriaCodigos.LOCACION_INGENIERIA_TIMESHEET).
                                 FirstOrDefault();
                            if (catalogoLocacion == null)
                            {
                                detalles.Cells["Q" + rowIterator].Value = "La Locación no existe en el catálogo";
                                detalles.Cells["Q" + rowIterator].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                detalles.Cells["Q" + rowIterator].Style.Fill.BackgroundColor.SetColor(Color.Red);
                                detalles.Cells["Q" + rowIterator].Style.Font.Color.SetColor(Color.White);
                                continue;
                            }

                            var Modalidad = (detalles.Cells["O" + rowIterator].Value ?? "").ToString();
                            if (Modalidad == "")
                            {
                                detalles.Cells["Q" + rowIterator].Value = "Debe seleccionar Modalidad";
                                detalles.Cells["Q" + rowIterator].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                detalles.Cells["Q" + rowIterator].Style.Fill.BackgroundColor.SetColor(Color.Red);
                                detalles.Cells["Q" + rowIterator].Style.Font.Color.SetColor(Color.White);
                                continue;
                            }

                            var catalogoModalidad = _catalogoRepository.GetAll().Where(c => c.nombre == Modalidad)
                                 .Where(c => c.TipoCatalogo.codigo == CertificacionIngenieriaCodigos.MODALIDAD_INGENIERIA_TIMESHEET).
                                 FirstOrDefault();
                            if (catalogoModalidad == null)
                            {
                                detalles.Cells["Q" + rowIterator].Value = "La modalidad no existe en el catálogo";
                                detalles.Cells["Q" + rowIterator].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                detalles.Cells["Q" + rowIterator].Style.Fill.BackgroundColor.SetColor(Color.Red);
                                detalles.Cells["Q" + rowIterator].Style.Font.Color.SetColor(Color.White);
                                continue;
                            }


                            var esdirectoString = (detalles.Cells["P" + rowIterator].Value ?? "").ToString();
                            if (esdirectoString == "")
                            {
                                detalles.Cells["Q" + rowIterator].Value = "Debe ingresar la campo es directo";
                                detalles.Cells["Q" + rowIterator].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                detalles.Cells["Q" + rowIterator].Style.Fill.BackgroundColor.SetColor(Color.Red);
                                detalles.Cells["Q" + rowIterator].Style.Font.Color.SetColor(Color.White);
                                continue;
                            }



                            var catalogoEstadoEnRevision = _catalogoRepository.GetAll().Where(c => c.vigente).Where(c => c.codigo == CertificacionIngenieriaCodigos.ESTADO_REVISION)
                                 .Where(c => c.TipoCatalogo.codigo == CertificacionIngenieriaCodigos.ESTADOS_REGISTROS_TIMESHEET)
                                 .FirstOrDefault();
                            var DetallesDirectosIngenieria = new DetallesDirectosIngenieria()
                            {

                                CodigoProyecto = CodigoProyecto,
                                ColaboradorId = colaborador.Id,
                                FechaTrabajo = fechaTrabajo,
                                EtapaId = catalogoEtapa.Id,
                                EstadoRegistroId = catalogoEstadoEnRevision != null ? catalogoEstadoEnRevision.Id : 0,
                                EspecialidadId = catalogoEspecialidad.Id,
                                Identificacion = identificacion,
                                LocacionId = catalogoLocacion.Id,
                                ModalidadId = catalogoModalidad.Id,
                                NombreEjecutante = Ejecutante,
                                NumeroHoras = numeroHoras,
                                Observaciones = Observaciones,
                                TipoRegistroId = catalogoTipoRegistro.Id,
                                CertificadoId = null,
                                ProyectoId = ProyectoExistente.Id,
                                EsDirecto = esdirectoString.ToUpper() == "S" ? true : false,
                                FechaCarga = DateTime.Now.Date,
                                JustificacionActualizacion = "",
                                CargaAutomatica = false,

                            };

                            Repository.Insert(DetallesDirectosIngenieria);
                            detalles.Cells["Q" + rowIterator].Value = "Actualizado Correctamente";
                        }
                    }
                    return excel;
                }

                return new ExcelPackage();
            }

            return new ExcelPackage();

        }

        public ExcelPackage DescargarPlantillaCargaMasivaDetallesIngenieria()
        {
            ExcelPackage excel = new ExcelPackage();

            string filename = System.Web.HttpContext.Current.Server.MapPath("~/Views/PlantillaWord/CertificacionIngenieria/FormatoCargaDetalles.xlsx");
            if (File.Exists((string)filename))
            {
                FileInfo newFile = new FileInfo(filename);
                ExcelPackage pck = new ExcelPackage(newFile);
                excel.Workbook.Worksheets.Add("DETALLES", pck.Workbook.Worksheets[1]);
                excel.Workbook.Worksheets.Add("CATÁLOGOS", pck.Workbook.Worksheets[2]);
            }

            ExcelWorksheet detalles = excel.Workbook.Worksheets[1];
            ExcelWorksheet catalogos = excel.Workbook.Worksheets[2];

            return excel;
        }

        public List<DetallesDirectosIngenieriaDto> ObtenerDetallesIngenieria(DateTime? FechaInicial, DateTime? FechaFinal)
        {
            var query = Repository.GetAllIncluding(c => c.TipoRegistro, c => c.Colaborador, c => c.Proyecto, c => c.Locacion, c => c.Modalidad, c => c.EstadoRegistro)

                        .ToList();
            if (FechaInicial.HasValue || FechaFinal.HasValue)
            {
                var FechaInicialDate = FechaInicial.Value.Date;
                var FechaFinalDate = FechaFinal.Value.Date;

                query = query.Where(c => c.FechaTrabajo >= FechaInicialDate)
                         .Where(c => c.FechaTrabajo <= FechaFinalDate).ToList();
            }



            var list = (from q in query
                        select new DetallesDirectosIngenieriaDto()
                        {
                            Id = q.Id,
                            CertificadoId = q.CertificadoId,
                            CodigoProyecto = q.CodigoProyecto,
                            ColaboradorId = q.ColaboradorId,
                            nombreColaborador = q.Colaborador.nombres_apellidos,
                            EsDirecto = q.EsDirecto,
                            EspecialidadId = q.EspecialidadId,
                            EstadoRegistroId = q.EstadoRegistroId,
                            EtapaId = q.EtapaId,
                            FechaTrabajo = q.FechaTrabajo,
                            formatFechaTrabajo = q.FechaTrabajo.ToShortDateString(),
                            nombreProyecto = q.Proyecto.codigo,
                            NumeroHoras = q.NumeroHoras,
                            NombreEjecutante = q.NombreEjecutante,
                            ModalidadId = q.ModalidadId,
                            Observaciones = q.Observaciones,
                            ProyectoId = q.ProyectoId,
                            Identificacion = q.Identificacion,
                            LocacionId = q.LocacionId,
                            nombreLocacion = q.LocacionId.HasValue?q.Locacion.nombre:"",
                            nombreModalidad = q.ModalidadId.HasValue?q.Modalidad.nombre:"",
                            CargaAutomatica = q.CargaAutomatica,
                            FechaCarga = q.FechaCarga,
                            JustificacionActualizacion = q.JustificacionActualizacion,
                            TipoRegistroId = q.TipoRegistroId,
                            esCargaAutomatica = q.CargaAutomatica ? "AUTOMATICA" : "MANUAL",
                            formatFechaCarga = q.FechaCarga.ToShortDateString(),
                            nombreEstado = q.EstadoRegistro.nombre,
                            Secuencial = q.Secuencial
                        }).ToList();
            return list;


        }

        public CatalogosIngenieria CatalogosIngenieria()
        {
            List<ModelClassReact> catalogoEstado = _catalogoService.APIObtenerCatalogosReact(CertificacionIngenieriaCodigos.ESTADOS_CERTIFICACION_INGENIERIA);
            List<ModelClassReact> catalogoTipoRegistro = _catalogoService.APIObtenerCatalogosReact(CertificacionIngenieriaCodigos.TIPO_REGISTRO_INGENIERIA);
            List<ModelClassReact> catalogoEstadoRegistro = _catalogoService.APIObtenerCatalogosReact(CertificacionIngenieriaCodigos.ESTADOS_REGISTROS_TIMESHEET);
            List<ModelClassReact> catalogoEtapa = _catalogoService.APIObtenerCatalogosReact(CertificacionIngenieriaCodigos.ETAPAS_INGENIERIA_TIMESHEET);
            List<ModelClassReact> catalogoEspecialidad = _catalogoService.APIObtenerCatalogosReact(CertificacionIngenieriaCodigos.ESPECIALIDAD_INGENIERIA_TIMESHEET);
            List<ModelClassReact> catalogoLocacion = _catalogoService.APIObtenerCatalogosReact(CertificacionIngenieriaCodigos.LOCACION_INGENIERIA_TIMESHEET);
            List<ModelClassReact> catalogoModalidad = _catalogoService.APIObtenerCatalogosReact(CertificacionIngenieriaCodigos.MODALIDAD_INGENIERIA_TIMESHEET);

            return new CatalogosIngenieria()
            {
                catalogoEstado = catalogoEstado,
                catalogoTipoRegistro = catalogoTipoRegistro,
                catalogoEstadoRegistro = catalogoEstadoRegistro,
                catalogoEtapa = catalogoEtapa,
                catalogoEspecialidad = catalogoEspecialidad,
                catalogoLocacion = catalogoLocacion,
                catalogoModalidad = catalogoModalidad,
            };
        }

        public List<SimpleColaborador> ObtenerColaboradores(string search)
        {
            var query = _colaboradoresRepository.GetAllIncluding(c => c.Area, c => c.Cargo)
                                             .Where(c => c.vigente)

                                             .Where(c => c.estado == RRHHCodigos.ESTADO_ACTIVO)
                                             .Where(c => c.nombres_apellidos.Contains(search) || c.numero_identificacion.StartsWith(search))
             .OrderBy(c => c.primer_apellido)
                                             .ToList();
            var list = new List<SimpleColaborador>();
            foreach (var q in query)
            {
                var data = new SimpleColaborador()
                {
                    Area = q.catalogo_area_id.HasValue ? q.Area.nombre : "",
                    Cargo = q.catalogo_cargo_id.HasValue ? q.Cargo.nombre : "",
                    CodigoSap = q.es_externo.HasValue && q.es_externo.Value ? q.numero_identificacion : q.empleado_id_sap.HasValue ? q.empleado_id_sap.Value.ToString() : "",
                    CodigoSapLocal = q.es_externo.HasValue && q.es_externo.Value ? q.numero_identificacion : q.empleado_id_sap_local.HasValue ? q.empleado_id_sap_local.Value.ToString() : "",
                    Id = q.Id,
                    Identificacion = q.numero_identificacion,
                    NombreCompleto = q.nombres_apellidos,
                    TipoUsuario = q.es_externo.HasValue ? q.es_externo.Value ? "EXTERNO" : "INTERNO" : "",
                    PrimerApellido = q.primer_apellido,
                    SegundoApellido = q.segundo_apellido,
                    Nombres = q.nombres,
                    esExterno = q.es_externo.HasValue ? q.es_externo.Value : false
                };


                list.Add(data);

            }
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

        public bool CrearDetalle(DetallesDirectosIngenieria input)
        {
            var proyecto = _proyectoRepository.GetAll().Where(c => c.Id == input.ProyectoId).FirstOrDefault();
            if (proyecto != null)
            {
                input.CodigoProyecto = proyecto.codigo_ingenieria + " - " + proyecto.codigo;
            }
            var result = Repository.InsertAndGetId(input);

            return result > 0 ? true : false;
        }

        public bool ActualizarDetalle(DetallesDirectosIngenieria input)
        {

            var catalogoEstadoEnRevision = _catalogoRepository.GetAll().Where(c => c.vigente).Where(c => c.codigo == CertificacionIngenieriaCodigos.ESTADO_REVISION)

                 .Where(c => c.TipoCatalogo.codigo == CertificacionIngenieriaCodigos.ESTADOS_REGISTROS_TIMESHEET)
                 .FirstOrDefault();
            var entity = Repository.Get(input.Id);
            entity.Identificacion = input.Identificacion;
            entity.ColaboradorId = input.ColaboradorId;
            entity.TipoRegistroId = input.TipoRegistroId;
            entity.CodigoProyecto = input.CodigoProyecto;
            entity.ProyectoId = input.ProyectoId;
            entity.NumeroHoras = input.NumeroHoras;
            entity.NombreEjecutante = input.NombreEjecutante;
            entity.FechaTrabajo = input.FechaTrabajo;
            entity.Observaciones = input.Observaciones;
            entity.EtapaId = input.EtapaId;
            entity.EspecialidadId = input.EspecialidadId;
            entity.EstadoRegistroId = catalogoEstadoEnRevision != null ? catalogoEstadoEnRevision.Id : 0;
            entity.LocacionId = input.LocacionId;
            entity.ModalidadId = input.ModalidadId;
            entity.EsDirecto = input.EsDirecto;
            entity.CertificadoId = input.CertificadoId;
            entity.JustificacionActualizacion = input.JustificacionActualizacion;
            entity.FechaCarga = DateTime.Now.Date;
            entity.CargaAutomatica = false;

            var result = Repository.Update(entity);
            return result.Id > 0 ? true : false;
        }

        public string DeleteDetalle(int Id)
        {
            var entity = Repository.GetAllIncluding(c => c.EstadoRegistro).Where(c => c.Id == Id).FirstOrDefault();
            if (entity.EstadoRegistro.codigo == CertificacionIngenieriaCodigos.ESTADO_CERTIFICADO)
            {
                return "CERTIFICADO";
            }
            else
            {
               

                Repository.Delete(entity);
                return "OK";
            }



        }

        public bool ActualizarEstadoValidadoIngenieria(int Id)
        {
            var catalogoEstadoValidado = _catalogoRepository.GetAll().Where(c => c.vigente).Where(c => c.codigo == CertificacionIngenieriaCodigos.ESTADO_VALIDADO)
                 .Where(c => c.TipoCatalogo.codigo == CertificacionIngenieriaCodigos.ESTADOS_REGISTROS_TIMESHEET).FirstOrDefault();
            var entity = Repository.Get(Id);
            entity.EstadoRegistroId = catalogoEstadoValidado.Id;
            Repository.Update(entity);
            return true;
        }

        public DetallesDirectos ObtenerDetallesDirectosIngenieria(DateTime? FechaInicial, DateTime? FechaFinal)
        {
     
            var query = Repository.GetAllIncluding(c => c.TipoRegistro, c => c.Colaborador, c => c.Proyecto, c => c.Locacion, c => c.Modalidad, c => c.EstadoRegistro)

                        .ToList();
            if (FechaInicial.HasValue || FechaFinal.HasValue)
            {
                var FechaInicialDate = FechaInicial.Value.Date;
                var FechaFinalDate = FechaFinal.Value.Date;

                query = query.Where(c => c.FechaTrabajo >= FechaInicialDate)
                         .Where(c => c.FechaTrabajo <= FechaFinalDate).ToList();
            }



            var Directos = (from q in query
                        select new DetallesDirectosIngenieriaDto()
                        {
                            Id = q.Id,
                            CertificadoId = q.CertificadoId,
                            CodigoProyecto = q.CodigoProyecto,
                            ColaboradorId = q.ColaboradorId,
                            nombreColaborador = q.Colaborador.nombres_apellidos,
                            EsDirecto = q.EsDirecto,
                            EspecialidadId = q.EspecialidadId,
                            EstadoRegistroId = q.EstadoRegistroId,
                            EtapaId = q.EtapaId,
                            FechaTrabajo = q.FechaTrabajo,
                            formatFechaTrabajo = q.FechaTrabajo.ToShortDateString(),
                            nombreProyecto = q.Proyecto.codigo,
                            NumeroHoras = q.NumeroHoras,
                            NombreEjecutante = q.NombreEjecutante,
                            ModalidadId = q.ModalidadId,
                            Observaciones = q.Observaciones,
                            ProyectoId = q.ProyectoId,
                            Identificacion = q.Identificacion,
                            LocacionId = q.LocacionId,
                            nombreLocacion = q.LocacionId.HasValue ? q.Locacion.nombre : "",
                            nombreModalidad = q.ModalidadId.HasValue ? q.Modalidad.nombre : "",
                            CargaAutomatica = q.CargaAutomatica,
                            FechaCarga = q.FechaCarga,
                            JustificacionActualizacion = q.JustificacionActualizacion,
                            TipoRegistroId = q.TipoRegistroId,
                            esCargaAutomatica = q.CargaAutomatica ? "AUTOMATICA" : "MANUAL",
                            formatFechaCarga = q.FechaCarga.ToShortDateString(),
                            nombreEstado = q.EstadoRegistro.nombre
                        }).ToList();


            

            var queryE500 = _e500Repository.GetAllIncluding(c => c.Colaborador,c => c.EstadoRegistro)

                          .ToList();
            if (FechaInicial.HasValue || FechaFinal.HasValue)
            {
                var FechaInicialDate = FechaInicial.Value.Date;
                var FechaFinalDate = FechaFinal.Value.Date;

                queryE500 = queryE500.Where(c => c.FechaTrabajo >= FechaInicialDate)
                         .Where(c => c.FechaTrabajo <= FechaFinalDate).ToList();
            }

            var DirectosE500 = (from q in queryE500
                                select new DetalleDirectoE500Dto()
                            {
                                Id = q.Id,
                                ColaboradorId = q.ColaboradorId,
                                nombreColaborador = q.Colaborador.nombres_apellidos,
                                EspecialidadId = q.EspecialidadId,
                                EstadoRegistroId = q.EstadoRegistroId,
                                EtapaId = q.EtapaId,
                                FechaTrabajo = q.FechaTrabajo,
                                formatFechaTrabajo = q.FechaTrabajo.ToShortDateString(),
                                    formatFechaCarga = q.FechaCarga.ToShortDateString(),
                                    NumeroHoras = q.NumeroHoras,
                                NombreEjecutante = q.NombreEjecutante,                          
                                Identificacion = q.Identificacion,   
                                nombreEstado = q.EstadoRegistro.nombre
                            }).ToList();
            var list = new DetallesDirectos()
            {
                Directos = Directos,
                DirectosE500= DirectosE500
            };

            return list;


        }


        public CargaTimesheetDto ObtenerUltimaCargaTimesheet()
        {
            var cargaTimesheet = _cargaTimesheetRepository.GetAll()
                .Where(o => o.ValidacionIngenieria == "N" || o.ValidacionIngenieria == null)
                .OrderByDescending(o => o.FechaFinal)
                .FirstOrDefault();

            return Mapper.Map<CargaTimesheetDto>(cargaTimesheet);
        }

        public async Task<bool> ValidarCargaTimesheetAsync(int cargaTimesheetId)
        {
            var carga = _cargaTimesheetRepository.Get(cargaTimesheetId);
            if (carga == null)
                return false;

            carga.FechaValidacionIngenieria = DateTime.Now;
            carga.ValidacionIngenieria = "S";
            _cargaTimesheetRepository.Update(carga);

            var correos = _correosListaRepository
                .GetAll()
                .Where(c => c.vigente)
                .Where(c => c.ListaDistribucion.vigente)
                .Where(c => c.ListaDistribucion.codigo == CatalogosCodigos.LISTADISTRIBUCION_PYCP_INGENIERIA)
                .ToList();

            if (correos.Count > 0)
            {
                MailMessage message = new MailMessage();
                message.Subject = "Confirmación de carga ingeniería al -" + carga.FechaFinal.Date;


                foreach (var item in correos)
                {
                    message.To.Add(item.correo);
                }

                string body = $@"Se confirma que se ha completado el proceso de carga de la información con el siguiente resumen:
- PERIODO: {carga.FechaInicial.Date} – {carga.FechaFinal.Date}
- REGISTROS CARGADADOS: {carga.NumeroRegistros}";
                message.Body = body;
                
                await _correoService.SendWithFilesAsync(message);
            }
            return true;
        }

        public int SecuencialCargaDirectos()
        {
            var siguienteSecuencial = 1; // si no existe ninguna carga
            var query = Repository.GetAll().Where(s => s.Secuencial.HasValue)
                                           .Select(s => s.Secuencial.Value)
                                           .ToList();

            var query2 = _e500Repository.GetAll().Where(s => s.Secuencial.HasValue)
                                        .Select(s => s.Secuencial.Value)
                                        .ToList();

            if (query2.Count > 0) {
                query.AddRange(query2);
            }
            if (query.Count > 0) {
                siguienteSecuencial = query.Max()+1;
            }
            return siguienteSecuencial;
        }
    }
}

