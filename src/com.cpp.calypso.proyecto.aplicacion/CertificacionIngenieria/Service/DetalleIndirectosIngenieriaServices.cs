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
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Objects;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace com.cpp.calypso.proyecto.aplicacion.CertificacionIngenieria.Service
{
    public class DetalleIndirectosIngenieriaAsyncBaseCrudAppService : AsyncBaseCrudAppService<DetalleIndirectosIngenieria, DetalleIndirectosIngenieriaDto, PagedAndFilteredResultRequestDto>, IDetalleIndirectosIngenieriaAsyncBaseCrudAppService
    {
        private readonly IBaseRepository<DetalleIndirectosIngenieria> repository;
        private readonly IBaseRepository<ColaboradorRubroIngenieria> _colaboradorRubroRepository;
        private readonly IBaseRepository<ColaboradorCertificacionIngenieria> _colaboradorCertificacionRepository;
        private readonly IBaseRepository<Feriado> _feriadoRepository;
        private readonly IContratoAsyncBaseCrudAppService _contratoService;
        private readonly IBaseRepository<Colaboradores> _colaboradorRepository;
        private readonly IBaseRepository<Contrato> _contratoRepository;
        private readonly IBaseRepository<Cliente> _clienteRepository;
        private readonly IBaseRepository<PorcentajeIndirectoIngenieria> _porcentajeIndirectoRepository;
        private readonly IBaseRepository<GastoDirectoCertificado> _gastoDirectoRepository;
        private readonly IBaseRepository<Catalogo> _catalogoRepository;
        private readonly IBaseRepository<GrupoCertificadoIngenieria> _grupoCertificadoRepository;

        public DetalleIndirectosIngenieriaAsyncBaseCrudAppService(
            IBaseRepository<DetalleIndirectosIngenieria> repository,
            IBaseRepository<ColaboradorRubroIngenieria> colaboradorRubroRepository,
            IBaseRepository<ColaboradorCertificacionIngenieria> colaboradorCertificacionRepository,
            IBaseRepository<Feriado> feriadoRepository,
            IContratoAsyncBaseCrudAppService contratoService,
            IBaseRepository<Colaboradores> colaboradorRepository,
            IBaseRepository<Contrato> contratoRepository,
            IBaseRepository<Cliente> clienteRepository,
            IBaseRepository<PorcentajeIndirectoIngenieria> porcentajeIndirectoRepository,
             IBaseRepository<GastoDirectoCertificado> gastoDirectoRepository,
             IBaseRepository<Catalogo> catalogoRepository,
             IBaseRepository<GrupoCertificadoIngenieria> grupoCertificadoRepository


        ) : base(repository)
        {
            this.repository = repository;
            _colaboradorRubroRepository = colaboradorRubroRepository;
            _colaboradorCertificacionRepository = colaboradorCertificacionRepository;
            _feriadoRepository = feriadoRepository;
            _contratoService = contratoService;
            _colaboradorRepository = colaboradorRepository;
            _contratoRepository = contratoRepository;
            _porcentajeIndirectoRepository = porcentajeIndirectoRepository;
            _clienteRepository = clienteRepository;
            _gastoDirectoRepository = gastoDirectoRepository;
            _catalogoRepository = catalogoRepository;
            _grupoCertificadoRepository = grupoCertificadoRepository;
        }


        public List<DetalleIndirectosIngenieriaDto> ObtenerIndirectosIngenieriaPorFechas(DateTime? fechaDesde, DateTime? fechaHasta)
        {
            var queryCarpetas = Repository.GetAll()
                .Include(o => o.ColaboradorRubro)
                .Include(o => o.ColaboradorRubro.Rubro)
                .Include(o => o.ColaboradorRubro.Colaborador)
                ;


            if (fechaDesde.HasValue)
            {
                queryCarpetas = queryCarpetas
                    .Where(o => o.FechaDesde >= fechaDesde && o.FechaDesde <= fechaHasta)
                    .Where(o => o.FechaHasta <= fechaHasta);
            }
            var list = queryCarpetas.ToList();
            var dtos = Mapper.Map<List<DetalleIndirectosIngenieriaDto>>(list);
            return dtos;
        }

        public ResultadoColaboradorRubro CalcularDiasLaborados(int colaboradorId, DateTime fechaDesde, DateTime fechaHasta)
        {
            /* Existe RubroColaborador entre las fechas? */
            var colaboradorIngenieria = _colaboradorCertificacionRepository.GetAll()
                .Include(o => o.Ubicacion)
                .Where(o => o.ColaboradorId == colaboradorId)
                .Where(o => fechaDesde >= o.FechaDesde)
                .Where(o => fechaHasta <= o.FechaHasta || o.FechaHasta == null)
                .FirstOrDefault();

            if (colaboradorIngenieria == null)
            {
                return new ResultadoColaboradorRubro
                {
                    Success = false,
                    Message = "No existe una parametrización asignada al colaborador entre las fechas ingresadas"
                };
            }

            if (colaboradorIngenieria.Ubicacion.codigo == "UBI_UIO")
            {
                /* Obtener todos los feriados que caigan entre las fechas */
                var feriados = ObtenerFeriadosEntreFechas(fechaDesde, fechaHasta);
                var listado = new List<FechasHorasDto>() { };
                foreach (var feriado in feriados)
                {
                    /* Obtener un IEnumerable<Datetime> con las fechas*/
                    var temp = ObtenerRangoDeFechas(feriado);
                    foreach (var fecha in temp)
                    {
                        /* Recorrer las fechas y asignarle la hora creada en el bdd*/
                        listado.Add(
                            new FechasHorasDto()
                            {
                                Fecha = fecha,
                                Horas = feriado.Horas
                            });
                    }
                }

                decimal countFeriados = 0;
                foreach (var feriado in listado)
                {
                    if (feriado.Fecha >= fechaDesde && feriado.Fecha <= fechaHasta)
                    {
                        countFeriados += feriado.Horas;
                    };
                }

                var numeroDias = (decimal)((fechaHasta - fechaDesde).TotalDays + 1);
                var finesDeSemana = obtenerNumeroDeFinesDeSemana(fechaDesde, fechaHasta);

                decimal total = numeroDias - (countFeriados / 8) - finesDeSemana;
                return new ResultadoColaboradorRubro
                {
                    Success = true,
                    Contador = total
                };

            }
            else
            {
                return new ResultadoColaboradorRubro
                {
                    Success = true,
                    Contador = 0
                };
            }
        }

        public IEnumerable<DateTime> ObtenerRangoDeFechas(Feriado feriado)
        {
            return Enumerable.Range(0, feriado.FechaFin.Subtract(feriado.FechaInicio).Days + 1)
                             .Select(d => feriado.FechaInicio.AddDays(d));
        }


        private int obtenerNumeroDeFinesDeSemana(DateTime start, DateTime stop)
        {
            int days = 0;
            while (start <= stop)
            {
                if (start.DayOfWeek == DayOfWeek.Saturday || start.DayOfWeek == DayOfWeek.Sunday)
                {
                    ++days;
                }
                start = start.AddDays(1);
            }
            return days;
        }

        public async Task<ResultadoColaboradorRubro> CrearIndirectoAsync(DetalleIndirectosIngenieriaDto dto)
        {
            /* Existe RubroColaborador entre las fechas? */
            var rubroColaborador = _colaboradorRubroRepository.GetAll()
                .Where(o => o.ColaboradorId == dto.ColaboradorId)
                .Where(o => dto.FechaDesde >= o.FechaInicio)
                .Where(o => dto.FechaHasta <= o.FechaFin || o.FechaFin == null)
                .FirstOrDefault();

            if (rubroColaborador == null)
            {
                return new ResultadoColaboradorRubro
                {
                    Success = false,
                    Message = "No existe un rubro asignado al colaborador entre las fechas ingresadas"
                };
            }

            /* Comprobar fechas*/
            var existe = ComprobarExistenciaDetalleIndirecto(dto.FechaDesde, dto.FechaHasta, dto.Id, rubroColaborador.Id);
            if (existe)
            {
                return new ResultadoColaboradorRubro
                {
                    Success = false,
                    Message = "Ya existe un rubro vigente para el colaborador en estas fechas"
                };
            }

            /* */

            var entity = Mapper.Map<DetalleIndirectosIngenieria>(dto);
            entity.ColaboradorRubroId = rubroColaborador.Id;
            await Repository.InsertAsync(entity);
            return new ResultadoColaboradorRubro
            {
                Success = true,
                Message = ""
            };
        }

        public async Task<ResultadoColaboradorRubro> ActualizarAsync(DetalleIndirectosIngenieriaDto dto)
        {
            /* Existe RubroColaborador entre las fechas? */
            var rubroColaborador = _colaboradorRubroRepository.GetAll()
                .Where(o => o.ColaboradorId == dto.ColaboradorId)
                .Where(o => dto.FechaDesde >= o.FechaInicio)
                .Where(o => dto.FechaHasta <= o.FechaFin || o.FechaFin == null)
                .FirstOrDefault();

            if (rubroColaborador == null)
            {
                return new ResultadoColaboradorRubro
                {
                    Success = false,
                    Message = "No existe un rubro asignado al colaborador entre las fechas ingresadas"
                };
            }

            /* Comprobar fechas*/
            var existe = ComprobarExistenciaDetalleIndirecto(dto.FechaDesde, dto.FechaHasta, dto.Id, rubroColaborador.Id);
            if (existe)
            {
                return new ResultadoColaboradorRubro
                {
                    Success = false,
                    Message = "Ya existe un detalle registrado entre las fechas ingresadas"
                };
            }

            var entity = Mapper.Map<DetalleIndirectosIngenieria>(dto);
            await Repository.UpdateAsync(entity);
            return new ResultadoColaboradorRubro
            {
                Success = true,
                Message = ""
            }; ;
        }

        public ResultadoColaboradorRubro Eliminar(int id)
        {
            var detalle = Repository.Get(id);
            if (detalle.Certificado)
            {
                return new ResultadoColaboradorRubro
                {
                    Success = true,
                    Message = "No se puede eliminar un detalle certificado"
                };
            }
            else
            {
                var avancePorcentajeDirectos = _porcentajeIndirectoRepository.GetAll().Where(c => c.DetalleIndirectosIngenieriaId == id).ToList();
                foreach (var avances in avancePorcentajeDirectos)
                {
                    _porcentajeIndirectoRepository.Delete(avances.Id);
                };


                Repository.Delete(id);
                return new ResultadoColaboradorRubro
                {
                    Success = true,
                    Message = "Detalle eliminado correctamente"
                };
            }


        }

        public List<Feriado> ObtenerFeriadosEntreFechas(DateTime fechainicio, DateTime fechafin)
        {
            var listado = new List<Feriado>();
            var feriados = _feriadoRepository.GetAll().ToList();
            foreach (var item in feriados)
            {
                /* Sólo la fecha de fin entra en el rango de feriados*/
                if (fechafin >= item.FechaInicio && fechafin <= item.FechaFin)
                {
                    listado.Add(item);
                }
                /* Las dos fechas entran en el rango de feriados */
                else if (fechainicio >= item.FechaInicio && fechafin <= item.FechaFin)
                {
                    listado.Add(item);
                }
                /* Sólo la fecha de fin entra en el rango del feriado*/
                else if (fechainicio >= item.FechaInicio && fechainicio <= item.FechaFin)
                {
                    listado.Add(item);
                }
                /* El rango abarca todo el feriado */
                else if (item.FechaInicio >= fechainicio && item.FechaFin <= fechafin)
                {
                    listado.Add(item);
                }

            }
            return listado;
        }

        public bool ComprobarExistenciaDetalleIndirecto(DateTime fechainicio, DateTime fechafin, int detalleIndirectoId, int rubroColaboradorId)
        {
            var query = Repository.GetAll()
                .Where(o => o.ColaboradorRubroId == rubroColaboradorId);

            if (detalleIndirectoId > 0)
            {
                query = query.Where(o => o.Id != detalleIndirectoId);
            }

            var detalleIndirectos = query.ToList();

            bool result = false;
            foreach (var item in detalleIndirectos)
            {
                if (fechainicio >= item.FechaDesde && fechafin <= item.FechaHasta)
                {
                    result = true;
                    ;
                    break;
                }

                if (item.FechaDesde > fechainicio && item.FechaHasta < fechafin)
                {
                    result = true;
                    ;
                    break;
                }

                if (fechainicio > item.FechaDesde && fechainicio < item.FechaHasta)
                {
                    result = true;
                    ;
                    break;
                }

                if (fechafin > item.FechaDesde && fechafin < item.FechaHasta)
                {
                    result = true;
                    ;
                    break;
                }
            }

            return result;
        }

        public List<ColaboradorUbicacion> getIndirectosCertificados()
        {
            var Colaboradores = new List<ColaboradorUbicacion>();
            var ultimoCertificado = _grupoCertificadoRepository.GetAll().OrderByDescending(c => c.FechaCertificado).FirstOrDefault();
            if (ultimoCertificado != null)
            {
                var colaboradoresIndirectos = _gastoDirectoRepository.GetAllIncluding(c => c.Colaborador)
                                                             .Where(c => c.CertificadoIngenieriaProyecto
                                                             .GrupoCertificadoIngenieriaId == ultimoCertificado.Id)
                                                             .Where(c => c.TipoGastoId == TipoGasto.Indirecto)
                                                             .Where(c => !c.EsViatico)
                                                             .ToList();
                var colaboradorIds = (from s in colaboradoresIndirectos where s.Colaborador.estado=="ACTIVO" select s.ColaboradorId).ToList().Distinct().ToList();

                foreach (var ColaboradorId in colaboradorIds)
                {
                    var colaborador = (from x in colaboradoresIndirectos where x.ColaboradorId == ColaboradorId select x).FirstOrDefault();
                    var Parametrizacion = _colaboradorCertificacionRepository.GetAll().Where(c => ultimoCertificado.FechaCertificado >= c.FechaDesde)
                                                                                    .Where(c => ultimoCertificado.FechaCertificado <= c.FechaHasta)
                                                                                    .Where(c=>c.ColaboradorId== ColaboradorId)
                                                                                    .OrderByDescending(c=>c.FechaDesde)
                                                                                    .FirstOrDefault();
                    if (colaborador != null)
                    {
                        if (!Colaboradores.Any(c => c.ColaboradorId == ColaboradorId))
                        {
                            Colaboradores.Add(new ColaboradorUbicacion() {
                                ColaboradorId = ColaboradorId
                                , NumeroIdentificacion = colaborador.Colaborador.numero_identificacion
                                , NombresApellidos = colaborador.Colaborador.nombres_apellidos
                                , Ubicacion = colaborador.UbicacionTrabajo,
                                HorasLaboradas=Parametrizacion!=null?Parametrizacion.HorasPorDia:8
                            });
                        }


                    }

                }

            }
            return Colaboradores;

        }
        public ExcelPackage DescargarPlantillaCargaMasivaGastosIndirectos()
        {
            ExcelPackage excel = new ExcelPackage();

            string filename = System.Web.HttpContext.Current.Server.MapPath("~/Views/PlantillaWord/CertificacionIngenieria/FormatoCargaGastosIndirectos.xlsx");
            if (File.Exists((string)filename))
            {
                FileInfo newFile = new FileInfo(filename);
                ExcelPackage pck = new ExcelPackage(newFile);
                excel.Workbook.Worksheets.Add("DATOS", pck.Workbook.Worksheets[1]);
                excel.Workbook.Worksheets.Add("catálogo", pck.Workbook.Worksheets[2]);
            }

            ExcelWorksheet gastos = excel.Workbook.Worksheets[1];
            ExcelWorksheet catalogos = excel.Workbook.Worksheets[2];

            var ColaboradoresIndirectos = this.getIndirectosCertificados();
            var contratos = _contratoService.GetContratosDto();

            int countFilasContratos = 1;
            foreach (var contrato in contratos)
            {
                catalogos.Cells["A" + countFilasContratos].Value = contrato.Id;
                catalogos.Cells["B" + countFilasContratos].Value = contrato.Codigo;
                catalogos.Cells["C" + countFilasContratos].Value = contrato.descripcion;
                countFilasContratos++;
            }


            /*var validacionContrato = gastos.DataValidations.AddListValidation("H:H");
            validacionContrato.Formula.ExcelFormula = "=catálogo!$B$1:$B$" + countFilasContratos;
            validacionContrato.AllowBlank = true;*/

            int fila = 2;
            if (ColaboradoresIndirectos.Count > 0)
            {
                foreach (var c in ColaboradoresIndirectos)
                {

                    gastos.Cells["C" + fila].Value = c.NumeroIdentificacion;
                    gastos.Cells["D" + fila].Value = c.NombresApellidos;
                

                    gastos.Cells["D" + fila].Style.Numberformat.Format = "0.00%";
                    gastos.Cells["F" + fila].Value = c.HorasLaboradas;
                    gastos.Cells["I" + fila].Value = c.Ubicacion == CertificacionIngenieriaCodigos.UBICACION_CAMPO ? "CAMPO" : "QUITO";

                    fila++;
                }

            }
            var validacionViatico = gastos.DataValidations.AddListValidation("G:G");
            validacionViatico.Formula.ExcelFormula = "=catálogo!$E$2:$E$3";
            validacionViatico.AllowBlank = true;

            return excel;
        }


        public async Task<ExcelPackage> CargaMasivaDeGastosIndirectosAsync(HttpPostedFileBase uploadedFile)
        {
            if (uploadedFile != null)
            {
                if (uploadedFile.ContentType == "application/vnd.ms-excel" || uploadedFile.ContentType ==
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                {
                    ExcelPackage excel = new ExcelPackage();

                    string fileContentType = uploadedFile.ContentType;
                    byte[] fileBytes = new byte[uploadedFile.ContentLength];

                    ExcelWorksheet gastos = null;
                    ExcelWorksheet catalogos = null;

                    using (var package = new ExcelPackage(uploadedFile.InputStream))
                    {
                        gastos = excel.Workbook.Worksheets.Add("DATOS", package.Workbook.Worksheets[1]);
                        catalogos = excel.Workbook.Worksheets.Add("catálogo", package.Workbook.Worksheets[2]);

                        var numberOfColumns = gastos.Dimension.End.Column;
                        var numberOfRows = gastos.Dimension.End.Row;

                        for (int rowIterator = 2; rowIterator <= numberOfRows; rowIterator++)
                        {


                            /* Validar ingreso de horas laborados*/
                           /* var esViatico = (gastos.Cells["G" + rowIterator].Value ?? "").ToString();
                            if (esViatico == "")
                            {
                                gastos.Cells["I" + rowIterator].Value = "Debe seleccionar el campo 'Es Viatico'";
                                continue;
                            }*/


                            /* Validar ingreso de la identificacion*/
                            var identificacion = (gastos.Cells["C" + rowIterator].Value ?? "").ToString();
                            if (identificacion == "")
                            {
                                gastos.Cells["I" + rowIterator].Value = "Debe ingresar la identificación del colaborador";
                                continue;
                            }

                            /* Validar ingreso del contrato */
                            /*  var contratoCodigo = (gastos.Cells["G" + rowIterator].Value ?? "").ToString();
                              if (contratoCodigo == "")
                              {
                                  gastos.Cells["I" + rowIterator].Value = "Debe seleccionar el contrato";
                                  continue;
                              }
                              */
                            /* Buscar el contrato */
                            /* var contrato = _contratoRepository.GetAll()
                                 .Where(o => o.vigente)
                                 .Where(o => o.Codigo == contratoCodigo)
                                 .FirstOrDefault();*/


                            /* Buscar el colaborador */
                            var colaborador = _colaboradorRepository
                                .GetAll()
                                .Where(c => c.vigente)
                                .OrderByDescending(c => c.fecha_ingreso)
                                .FirstOrDefault(o => o.numero_identificacion == identificacion)
                                ;
                            if (colaborador == null)
                            {
                                gastos.Cells["I" + rowIterator].Value = "No se encontró al colaborador";
                                continue;
                            }


                            /* Validar Fecha Inicio */
                            var fechaInicioString = (gastos.Cells["A" + rowIterator].Text ?? "").ToString();
                            if (fechaInicioString == "")
                            {
                                gastos.Cells["I" + rowIterator].Value = "Debe ingresar la fecha inicial";
                                continue;
                            }

                            /* Validar Fecha Fin */
                            var fechaFinString = (gastos.Cells["B" + rowIterator].Text ?? "").ToString();
                            if (fechaFinString == "")
                            {
                                gastos.Cells["I" + rowIterator].Value = "Debe ingresar la fecha final";
                                continue;
                            }

                            /* Validar existe RubroColaborador entre las fechas? */
                            DateTime fechaInicioStart = DateTime.Parse(fechaInicioString).Date;
                            DateTime fechaInicioEnd = DateTime.Parse(fechaInicioString).Date.AddDays(1).AddTicks(-1);
                            DateTime fechaFinStart = DateTime.Parse(fechaFinString).Date;
                            DateTime fechaFinEnd = DateTime.Parse(fechaFinString).Date.AddDays(1).AddTicks(-1);


                            /* Validar ingreso del porcentaje*/
                            var porcentajeString = "";
                            if (gastos.Cells["H" + rowIterator].Value != null)
                            {
                                porcentajeString = (gastos.Cells["H" + rowIterator].Value ?? "").ToString();
                            }


                            /* Validar ingreso de días laborados*/
                            var diasLaboradosString = (gastos.Cells["E" + rowIterator].Value ?? "").ToString();

                            /* Validar ingreso de horas laborados*/
                            var horasLaboradasString = (gastos.Cells["F" + rowIterator].Value ?? "").ToString();

                            var horasLaboradas = Convert.ToDecimal(0);
                            var diasLaborados = Convert.ToDecimal(0);
                            if (porcentajeString == "")
                            {
                                if (diasLaboradosString != "" && horasLaboradasString != "")
                                {
                                    horasLaboradas = decimal.Parse(horasLaboradasString);
                                    diasLaborados = decimal.Parse(diasLaboradosString);

                                }
                                else
                                {
                                    gastos.Cells["I" + rowIterator].Value = "Debe ingresar los días laborados y horas laboradas";
                                    continue;
                                }

                            }
                            else
                            {
                                var colaboradorCertificacionIngenieria = _colaboradorCertificacionRepository.GetAll()
                                .Where(o => o.ColaboradorId == colaborador.Id)
                                .Where(o => fechaInicioStart >= o.FechaDesde)
                                .Where(o => fechaFinStart <= o.FechaHasta || o.FechaHasta == null)
                                .FirstOrDefault();
                                if (colaboradorCertificacionIngenieria != null)
                                {
                                    horasLaboradas = colaboradorCertificacionIngenieria.HorasPorDia;
                                }
                                else
                                {
                                    horasLaboradas = 8;
                                }
                            }

                            var porcentajePorProyecto = decimal.Parse(porcentajeString != "" ? porcentajeString : "0");




                            var rubroColaborador = _colaboradorRubroRepository.GetAll()
                                .Where(o => o.ColaboradorId == colaborador.Id)
                                .Where(o => fechaInicioStart >= o.FechaInicio)
                                .Where(o => fechaFinStart <= o.FechaFin || o.FechaFin == null)
                                .FirstOrDefault();

                            if (rubroColaborador == null)
                            {
                                gastos.Cells["I" + rowIterator].Value = "No existe un rubro asignado al colaborador entre las fechas ingresadas";
                                continue;
                            }

                            /* Validar existe RubroColaborador entre las fechas? */
                            /*var colaboradorIngenieria = _colaboradorCertificacionRepository.GetAll()
                                .Include(o => o.Ubicacion)
                                .Where(o => o.ColaboradorId == colaborador.Id)
                                .Where(o => fechaInicioStart >= o.FechaDesde)
                                .Where(o => fechaFinStart <= o.FechaHasta || o.FechaHasta == null)
                                .FirstOrDefault();
                            if (colaboradorIngenieria == null)
                            {
                                gastos.Cells["I" + rowIterator].Value = "No existe una parametrización asignada al colaborador entre las fechas ingresadas";
                                continue;
                            }*/

                            /*  var detalleIndirecto2 = Repository.GetAll()
                                  .ToList();*/
                            /* Buscar o crear detalle de gastos indirectos */
                            var detalleIndirecto = Repository.GetAll()
                                .Where(o => o.ColaboradorRubroId == rubroColaborador.Id)
                                .Where(o => o.FechaDesde >= fechaInicioStart && o.FechaDesde <= fechaInicioEnd)
                                .Where(o => o.FechaHasta >= fechaFinStart && o.FechaHasta <= fechaFinEnd)
                                .FirstOrDefault();

                            var detalleId = 0;
                            if (detalleIndirecto == null)
                            {
                                var entity = new DetalleIndirectosIngenieria()
                                {
                                    FechaDesde = fechaInicioStart,
                                    FechaHasta = fechaFinStart,
                                    ColaboradorRubroId = rubroColaborador.Id,
                                    HorasLaboradas = horasLaboradas,
                                    DiasLaborados = diasLaborados,
                                    EsViatico = false, // Manda lo que mande siempre false por nuevoa 
                                    Certificado = false
                                };
                                detalleId = await Repository.InsertAndGetIdAsync(entity);

                                if (porcentajePorProyecto != 0)
                                {
                                    var porcentaje = new PorcentajeIndirectoIngenieria()
                                    {
                                        Horas = horasLaboradas,
                                        PorcentajeIndirecto = porcentajePorProyecto,
                                        ContratoId = rubroColaborador.ContratoId,
                                        DetalleIndirectosIngenieriaId = detalleId
                                    };
                                    _porcentajeIndirectoRepository.Insert(porcentaje);
                                }


                            }
                            else
                            {
                                detalleId = detalleIndirecto.Id;
                                var avance = _porcentajeIndirectoRepository.GetAll().Where(c => c.DetalleIndirectosIngenieriaId == detalleId).FirstOrDefault();
                                if (porcentajePorProyecto != 0 && avance != null)
                                {
                                    var entityav = _porcentajeIndirectoRepository.Get(avance.Id);
                                    entityav.PorcentajeIndirecto = porcentajePorProyecto;
                                    entityav.Horas = horasLaboradas;
                                    _porcentajeIndirectoRepository.Update(entityav);

                                }
                                else
                                {

                                }
                                if (porcentajePorProyecto == 0)
                                {
                                    var detalle = Repository.Get(detalleId);
                                    detalle.HorasLaboradas = horasLaboradas;
                                    detalle.DiasLaborados = diasLaborados;

                                    Repository.Update(detalle);
                                }

                            }


                            if (porcentajePorProyecto != 0)
                            {
                                var diasLaboradosReal = CalcularHorasTotalesDeDetalleIndirecto(detalleId, porcentajePorProyecto);
                                gastos.Cells["E" + rowIterator].Value = diasLaboradosReal;
                            }
                            gastos.Cells["I" + rowIterator].Value = "Actualizado Correctamente";

                        }
                    }
                    return excel;
                }

                return new ExcelPackage();
            }

            return new ExcelPackage();
        }

        public decimal CalcularHorasTotalesDeDetalleIndirecto(int detalleIndirectoId, decimal porcentaje)
        {
            /* var porcentajes = _porcentajeIndirectoRepository
                 .GetAll()
                 .Where(o => o.DetalleIndirectosIngenieriaId == detalleIndirectoId)
                 .ToList();*/

            var detalle = Repository.Get(detalleIndirectoId);

            var totalDias = this.ObtenerDiasLaborables(detalle.FechaDesde, detalle.FechaHasta);// Obtener Dias de la Semana sin FInes de Semana
            var diasFeriados = this.ObtenerDiasFeriados(detalle.FechaDesde, detalle.FechaHasta); // Obtener DIas de Feriado

            var diasTotales = totalDias - diasFeriados; //Dias Laborados Real

            /*decimal diasLaborados = 0;
            foreach (var porcentaje in porcentajes)
            {
                diasLaborados += porcentaje.PorcentajeIndirecto * diasTotales;
            }*/
            if (porcentaje != 0)
            {
                detalle.DiasLaborados = diasTotales * porcentaje;
            }

            return detalle.DiasLaborados;
        }

        public int ObtenerDiasLaborables(DateTime fechaDesde, DateTime fechaHasta)
        {
            DateTime inicio = fechaDesde;
            int dias = 0;
            while (inicio.Date <= fechaHasta.Date)
            {

                if (inicio.DayOfWeek != DayOfWeek.Saturday && inicio.DayOfWeek != DayOfWeek.Sunday)
                    dias++;

                inicio = inicio.AddDays(1);
            }
            return dias;
        }
        public int ObtenerDiasFeriados(DateTime fechaDesde, DateTime fechaHasta)
        {

            var Feriados = this.ObtenerFeriadosEntreFechas(fechaDesde, fechaHasta);
            int dias = Feriados.Count;
            return dias;
        }

        public DateTime GetDateZeroTime(DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
        }
    }
}
