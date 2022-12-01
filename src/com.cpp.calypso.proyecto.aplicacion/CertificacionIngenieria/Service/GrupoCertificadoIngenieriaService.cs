using AutoMapper;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.framework;
using com.cpp.calypso.proyecto.aplicacion.CertificacionIngenieria.Dto;
using com.cpp.calypso.proyecto.aplicacion.CertificacionIngenieria.Interface;
using com.cpp.calypso.proyecto.aplicacion.CertificacionIngenieria.Models;
using com.cpp.calypso.proyecto.aplicacion.Dto;
using com.cpp.calypso.proyecto.dominio;
using com.cpp.calypso.proyecto.dominio.CertificacionIngenieria;
using com.cpp.calypso.proyecto.dominio.Constantes;
using com.cpp.calypso.proyecto.dominio.Models;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;

namespace com.cpp.calypso.proyecto.aplicacion.CertificacionIngenieria.Service
{
    public class GrupoCertificadoIngenieriaAsyncBaseCrudAppService : AsyncBaseCrudAppService<GrupoCertificadoIngenieria, GrupoCertificadoIngenieriaDto, PagedAndFilteredResultRequestDto>, IGrupoCertificadoIngenieriaAsyncBaseCrudAppService
    {

        private readonly IBaseRepository<Proyecto> _proyectoRepository;
        private readonly IBaseRepository<Contrato> _contratoRepository;
        private readonly IBaseRepository<Cliente> _clienteRepository;
        private readonly IBaseRepository<Colaboradores> _colaboradorRepository;
        private readonly IBaseRepository<Catalogo> _catalogoRepository;
        private readonly IBaseRepository<CertificadoIngenieriaProyecto> _certificadoIngenieriaProyectoRepository;
        private readonly IBaseRepository<GastoDirectoCertificado> _gastoDirectoRepository;
        private readonly IBaseRepository<AvancePorcentajeProyecto> _avanceProyectoRepository;
        private readonly IBaseRepository<DetallesDirectosIngenieria> _directosRepository;
        private readonly IBaseRepository<DetalleIndirectosIngenieria> _indirectosRepository;
        private readonly IBaseRepository<Presupuesto> _presupuestoRepository;
        private readonly IBaseRepository<ColaboradorRubroIngenieria> _colaboradoRubroRepository;
        private readonly IBaseRepository<ParametroSistema> _parametroRepository;
        private readonly IBaseRepository<ComputoPresupuesto> _computoPresupuesto;
        private readonly IBaseRepository<Secuencial> _secuencialCertificadoRepository;
        private readonly IBaseRepository<PorcentajeAvanceIngenieria> _porcentajeIngenieriaRepository;
        private readonly IBaseRepository<DetalleDirectoE500> _directoE50;
        private readonly IBaseRepository<ResumenCertificacion> _resumenCertificacion;
        private readonly IBaseRepository<Item> _itemRepository;
        private readonly IBaseRepository<ColaboradorCertificacionIngenieria> _colaboradoCertificacionRepository;
        private readonly IBaseRepository<DetallePreciario> _detallePreciarioRepository;
        private readonly IBaseRepository<DetalleOrdenServicio> _detallePORepository;
        private readonly IBaseRepository<OfertaComercialPresupuesto> _ofertaRepository;

        private readonly IBaseRepository<DistribucionCertificadoIngenieria> _distribucionRepository;


        private readonly IBaseRepository<ComentarioCertificado> _comentarioCertificadoRepository;
        public GrupoCertificadoIngenieriaAsyncBaseCrudAppService(
            IBaseRepository<GrupoCertificadoIngenieria> repository,
            IBaseRepository<Proyecto> proyectoRepository,
            IBaseRepository<Catalogo> catalogoRepository,
            IBaseRepository<CertificadoIngenieriaProyecto> certificadoIngenieriaProyectoRepository,
            IBaseRepository<GastoDirectoCertificado> gastoDirectoRepository,
            IBaseRepository<AvancePorcentajeProyecto> avanceProyectoRepository,
            IBaseRepository<DetallesDirectosIngenieria> directosRepository,
            IBaseRepository<DetalleIndirectosIngenieria> indirectosRepository,
            IBaseRepository<Presupuesto> presupuestoRepository,
            IBaseRepository<ColaboradorRubroIngenieria> colaboradoRubroRepository,
            IBaseRepository<Contrato> contratoRepository,
            IBaseRepository<ParametroSistema> parametroRepository,
            IBaseRepository<Secuencial> secuencialCertificadoRepository,
            IBaseRepository<PorcentajeAvanceIngenieria> porcentajeIngenieriaRepository,
            IBaseRepository<ComputoPresupuesto> computoPresupuesto,
            IBaseRepository<DetalleDirectoE500> directoE500,
            IBaseRepository<Cliente> clienteRepository,
             IBaseRepository<ResumenCertificacion> resumenCertificacion,
             IBaseRepository<Item> itemRepository,
            IBaseRepository<Colaboradores> colaboradorRepository,
            IBaseRepository<ColaboradorCertificacionIngenieria> colaboradoCertificacionRepository,
            IBaseRepository<DetallePreciario> detallePreciarioRepository,
             IBaseRepository<OfertaComercialPresupuesto> ofertaRepository,
             IBaseRepository<DistribucionCertificadoIngenieria> distribucionRepository,
             IBaseRepository<DetalleOrdenServicio> detallePORepository,
              IBaseRepository<ComentarioCertificado> comentarioCertificadoRepository

        ) : base(repository)
        {
            _proyectoRepository = proyectoRepository;
            _catalogoRepository = catalogoRepository;
            _certificadoIngenieriaProyectoRepository = certificadoIngenieriaProyectoRepository;
            _gastoDirectoRepository = gastoDirectoRepository;
            _avanceProyectoRepository = avanceProyectoRepository;
            _directosRepository = directosRepository;
            _indirectosRepository = indirectosRepository;
            _presupuestoRepository = presupuestoRepository;
            _colaboradoRubroRepository = colaboradoRubroRepository;
            _contratoRepository = contratoRepository;
            _parametroRepository = parametroRepository;
            _secuencialCertificadoRepository = secuencialCertificadoRepository;
            _porcentajeIngenieriaRepository = porcentajeIngenieriaRepository;
            _computoPresupuesto = computoPresupuesto;
            _directoE50 = directoE500;
            _clienteRepository = clienteRepository;
            _resumenCertificacion = resumenCertificacion;
            _itemRepository = itemRepository;
            _colaboradorRepository = colaboradorRepository;
            _colaboradoCertificacionRepository = colaboradoCertificacionRepository;
            _detallePreciarioRepository = detallePreciarioRepository;
            _ofertaRepository = ofertaRepository;
            _distribucionRepository = distribucionRepository;
            _detallePORepository = detallePORepository;
            _comentarioCertificadoRepository = comentarioCertificadoRepository;
        }

        public bool Actualizar(GrupoCertificadoIngenieria e)
        {
            var u = Repository.Get(e.Id);
            u.FechaGeneracion = e.FechaGeneracion;
            u.FechaInicio = e.FechaInicio;
            u.FechaCertificado = e.FechaCertificado;
            u.FechaFin = e.FechaFin;
            u.ClienteId = e.ClienteId;
            return true;
        }



        public ResultadoCertificacion Crear(GrupoCertificadoIngenieria e, int[] Directos, int[] Indirectos, int[] E500)
        {
            /*Catalogos*/
            var Unidad = _catalogoRepository.GetAll().Where(c => c.codigo == "TAREA_HOMBRE").FirstOrDefault();

            /*No debe existir un certificado generado a la fecha de generación*/

            var ExisteGrupoCertificadoFechaCerticado = Repository.GetAll()
                                                               .Where(x => x.EstadoId == EstadoGrupoCertificado.Aprobado)
                                                               .Where(x => x.FechaCertificado == e.FechaCertificado)
                                                               .FirstOrDefault();
            if (ExisteGrupoCertificadoFechaCerticado != null)
            {
                return new ResultadoCertificacion
                {
                    Success = false,
                    Message = "Ya Existe un certificado generado con la misma fecha de certificación"
                };
            }

            var GrupoCertificadoId = Repository.InsertAndGetId(e);
            if (GrupoCertificadoId <= 0)
            {

                return new ResultadoCertificacion
                {
                    Success = false,
                    Message = "Ocurrió un error durante la generación elimine el registro y vuelva a intentar"
                };
            }


            /*Directos, Indirectos, E500 Seleccionados*/
            var DirectosIds = Directos.Select(c => c).ToList();
            var IndirectosIds = Indirectos.Select(c => c).ToList();


            /////////*Comportamiento Directos*/*////////

            //Obtiene Directos incluido Tarifa Migrados y De Colaboradores Rubro
            // var DirectosIngenieraDto = this.DtoDetallesDirectosIncluidoTarifa(DirectosIds);

            var DirectosIngenieraDto = this.DtoDetallesBusquedaRurboYSoloTarifaMigrado(DirectosIds);



            //Proyectos para Certificados
            var ProyectosDirectosId = (from p in DirectosIngenieraDto select p.ProyectoId).ToList().Distinct().ToList();
            var ColaboradoresDirectosId = (from p in DirectosIngenieraDto select p.ColaboradorId).ToList().Distinct().ToList();

            /*Precarga de Datos solo de proyectos */
            var ProyectoDirectos = _proyectoRepository.GetAll().Where(c => ProyectosDirectosId.Contains(c.Id)).ToList();
            var presupuestosProyectos = _presupuestoRepository.GetAll().Where(c => ProyectosDirectosId.Contains(c.ProyectoId))
                                                              .Where(c => c.vigente)
                                                              .Where(c => c.es_final)
                                                              .OrderByDescending(p => p.Id)
                                                               .ToList();

            var TodosavanceRealIngenieriaFechaCertificado = _avanceProyectoRepository.GetAll()
                                                                  .Where(c => ProyectosDirectosId.Contains(c.ProyectoId))
                                                                  .Where(c => c.FechaCertificado <= e.FechaFin)
                                                                  .OrderByDescending(c => c.FechaCertificado)
                                                                  .ToList();

            var CertificadosAnteriores = _certificadoIngenieriaProyectoRepository.GetAll()
                                                                                     .Where(c => ProyectosDirectosId.Contains(c.ProyectoId))
                                                                                     .Where(c => c.EstadoId == EstadoCertificadoProyecto.Aprobado)
                                                                                     .Where(c => c.GrupoCertificadoIngenieria.FechaFin < e.FechaInicio)
                                                                                     .Where(c => c.GrupoCertificadoIngenieriaId != GrupoCertificadoId)
                                                                                     .OrderByDescending(c => c.GrupoCertificadoIngenieria.FechaFin)
                                                                                     .ToList();


            var detallesE500 = _directoE50.GetAll().Where(c => E500.Contains(c.Id)).ToList();

            var ColaboradoresIdE500 = (from e5 in detallesE500 select e5.ColaboradorId).ToList().Distinct().ToList();

            if (ColaboradoresIdE500.Count > 0)
            {
                ColaboradoresDirectosId.AddRange(ColaboradoresIdE500);
            }

            ColaboradoresDirectosId = ColaboradoresDirectosId.Distinct().ToList();

            var ColaboradoresRubro = _colaboradoRubroRepository.GetAll()
                                                    .Where(c => ColaboradoresDirectosId.Contains(c.ColaboradorId))
                                                    .ToList();






            var MontoGlobal = (from m in DirectosIngenieraDto select m.monto).ToList().Sum();
            var HorasGlobal = (from m in DirectosIngenieraDto select m.NumeroHoras).ToList().Sum();

            var CertificadosPorProyectos = new List<CertificadoProyectoDirectosE500>();

            foreach (var ProyectoId in ProyectosDirectosId)

            {

                var DirectosporProyecto = (from d in DirectosIngenieraDto where d.ProyectoId == ProyectoId select d).ToList();

                // var entidadProyecto = _proyectoRepository.GetAll().Where(c => c.Id == ProyectoId).Where(c => c.vigente).FirstOrDefault();
                var entidadProyecto = (from p in ProyectoDirectos where p.Id == ProyectoId select p).FirstOrDefault();


                /*Se calcula considerando un secuencial por proyecto, es decir toma el máximo secuencial aprobado del proyecto e incrementa 1*/

                var numeroCertificado = this.NumeroCertificadoActualProyecto(ProyectoId);

                /* */

                /*Avance Real Ingenieria Tomado de valor carga complementaria ingeniería*/

                /*  var avanceRealIngenieriaFechaCertificado = _avanceProyectoRepository.GetAll()
                                                                    .Where(c => c.ProyectoId == ProyectoId)
                                                                    .Where(c => c.FechaCertificado <= e.FechaFin)
                                                                    .OrderByDescending(c => c.FechaCertificado)
                                                                    .FirstOrDefault();*/

                var avanceRealIngenieriaFechaCertificado = (from av in TodosavanceRealIngenieriaFechaCertificado
                                                            where av.ProyectoId == ProyectoId
                                                            orderby av.FechaCertificado descending
                                                            select av

                                                            ).FirstOrDefault();



                double PorcentajeIB = 0.30; //Porcentaje Ingenieria Basica
                double PorcentajeID = 0.70;//Porcentaje Ingenieria Detalle


                /*Porcentaje_asbuilt	Decimal	Si	Tomado de valor carga complementaria ingeniería*/
                decimal PorcentajeAsbuilt = 0; //Pendiente


                decimal avanceRealIngenieria = Decimal.Parse("0");
                if (avanceRealIngenieriaFechaCertificado != null)
                {

                    var valorIB = avanceRealIngenieriaFechaCertificado.AvanceRealActualIB * Convert.ToDecimal(PorcentajeIB);
                    var valorID = avanceRealIngenieriaFechaCertificado.AvanceRealActualID * Convert.ToDecimal(PorcentajeID);
                    avanceRealIngenieria = valorIB + valorID;

                    PorcentajeAsbuilt = avanceRealIngenieriaFechaCertificado.AsbuiltActual;
                }



                /*Horas Presupuestadas	Decimal	SI	Este valor se tomará del último presupuesto aprobado*/

                decimal HorasPresupuestadas = 0;


                var presupuesto = (from pre in presupuestosProyectos where pre.ProyectoId == ProyectoId select pre).FirstOrDefault();
                /*var presupuesto = _presupuestoRepository.GetAll().Where(p => p.vigente)
                                                                .Where(p => p.ProyectoId == ProyectoId)
                                                                .Where(p => p.es_final)
                                                                .OrderByDescending(p => p.Id)
                                                                .FirstOrDefault();*/
                if (presupuesto != null)
                {
                    var computosIngenieria = _computoPresupuesto.GetAll().Where(c => c.vigente)
                                                                       .Where(c => c.WbsPresupuesto.vigente)
                                                                       .Where(c => c.WbsPresupuesto.PresupuestoId == presupuesto.Id)
                                                                       .Where(c => c.Item.Grupo.codigo == ProyectoCodigos.CODE_INGENIERIA)
                                                                       .Where(c => c.WbsPresupuesto.Presupuesto.ProyectoId == ProyectoId)
                                                                       .ToList();

                    if (computosIngenieria.Count > 0)
                    {

                        HorasPresupuestadas = computosIngenieria.Select(c => c.cantidad).Sum(); //HH Presupuestadas , Suma de Cantidades
                    }

                }



                /*Monto_anterior_certificado Decimal Si Toma el monto actual del último certificado aprobado generado del proyecto específico*/

                decimal MontoAnteriorCertificado = 0;
                decimal HorasAnteriorCertificadas = 0;

                decimal MontoActualCertificado = 0;
                decimal HorasActualCertificadas = 0;

                /* var CertificadoProyectoAnterior = _certificadoIngenieriaProyectoRepository.GetAll()
                                                                                      .Where(c => c.ProyectoId == ProyectoId)
                                                                                      .Where(c => c.EstadoId == EstadoCertificadoProyecto.Aprobado)
                                                                                      .Where(c => c.GrupoCertificadoIngenieria.FechaFin < e.FechaInicio)
                                                                                      .Where(c => c.GrupoCertificadoIngenieriaId != GrupoCertificadoId)
                                                                                      .OrderByDescending(c => c.GrupoCertificadoIngenieria.FechaFin)
                                                                                      .FirstOrDefault();*/

                var CertificadoProyectoAnterior = (from cert in CertificadosAnteriores
                                                   where cert.ProyectoId == ProyectoId
                                                   select cert).FirstOrDefault();



                if (CertificadoProyectoAnterior != null)
                {
                    MontoAnteriorCertificado = CertificadoProyectoAnterior.MontoActualCertificado;
                    HorasAnteriorCertificadas = CertificadoProyectoAnterior.HorasActualCertificadas;
                }

                //Monto_actual_certificado	Decimal	
                /*Toma el monto actual del certificado en generación es decir la sumatoria de los rubros incluidos (directos / indirectos / viáticos)*/

                decimal porcentajeParticipacionporProyecto = 0;

                var DirectosParaActualizarCampoCertificadoId = (from d in DirectosporProyecto select d.Id).ToList();

                decimal montoTotalDirectos = 0;
                var montoTotalDirectosProyecto = (from d in DirectosporProyecto select d).ToList();
                if (montoTotalDirectosProyecto.Count > 0)
                {
                    montoTotalDirectos = montoTotalDirectosProyecto.Sum(c => c.monto);
                }

                decimal montoTotalIndirectos = 0;



                /*Suma Montos Directos e Indirectos*/
                MontoActualCertificado = montoTotalDirectos + montoTotalIndirectos;


                /*  Horas_actual_certificadas Decimal     Toma las horas totales de timesheet cargadas a certificarse en el periodo seleccionado*/

                decimal HorasTotalesDirectos = (from h in DirectosporProyecto select h.NumeroHoras).ToList().Sum();



                // decimal HorasTotalesIndirectos = (from hi in detallesIndirectosIngenieria select hi.HorasLaboradas).ToList().Sum();

                HorasActualCertificadas = HorasTotalesDirectos; //+ HorasTotalesIndirectos;


                var certificadoIngenieriaProyecto = new CertificadoIngenieriaProyecto()
                {
                    Id = 0,
                    GrupoCertificadoIngenieriaId = GrupoCertificadoId,
                    ProyectoId = ProyectoId,
                    EstadoId = EstadoCertificadoProyecto.Inicial,
                    NumeroCertificado = numeroCertificado,
                    OrdenServicioId = null,
                    AvanceRealIngenieria = avanceRealIngenieria,
                    HorasPresupuestadas = HorasPresupuestadas,
                    PorcentajeAsbuilt = PorcentajeAsbuilt,
                    MontoAnteriorCertificado = MontoAnteriorCertificado,
                    HorasAnteriorCertificadas = HorasAnteriorCertificadas,
                    MontoActualCertificado = MontoActualCertificado,
                    HorasActualCertificadas = HorasActualCertificadas,
                    DistribucionDirectos = false,
                    PorcentajeParticipacionDirectos = porcentajeParticipacionporProyecto
                };


                //Inser Certificado por Proyecto
                int CertificadoProyectoId = _certificadoIngenieriaProyectoRepository.InsertAndGetId(certificadoIngenieriaProyecto);

                //Guardar Temporal Certificado para Actualizar
                CertificadosPorProyectos.Add(new CertificadoProyectoDirectosE500()
                {
                    CertificadoProyectoId = CertificadoProyectoId,
                    ProyectoId = ProyectoId,
                    MontoActualCertificado = certificadoIngenieriaProyecto.MontoActualCertificado,
                    HorasActualCertificado = certificadoIngenieriaProyecto.HorasActualCertificadas
                });



                if (CertificadoProyectoId <= 0)
                {
                    return new ResultadoCertificacion
                    {
                        Success = false,
                        Message = "Ocurrió un error durante la generación elimine el registro y vuelva a intentar"
                    };
                }


                /*Gastos Directos Tabla Hija Certificado por Certificado Id  Y Proyecto*/

                foreach (var directo in DirectosporProyecto)
                {

                    ElmahExtension.LogToElmah(new Exception("directoId :" + directo.Id));
                    /*  var ColaboradorRubro = _colaboradoRubroRepository.GetAll()
                                                                 .Where(c => c.ColaboradorId == directo.ColaboradorId)
                                                                 .Where(c => c.ContratoId == entidadProyecto.contratoId)
                                                                 .FirstOrDefault();*/

                    var ColaboradorRubro = (from cr in ColaboradoresRubro
                                            where cr.ColaboradorId == directo.ColaboradorId
                                            where cr.ContratoId == entidadProyecto.contratoId
                                            select cr)
                                                              .FirstOrDefault();



                    var GastoDirecto = new GastoDirectoCertificado()
                    {
                        Id = 0,
                        CertificadoIngenieriaProyectoId = CertificadoProyectoId,
                        TipoGastoId = TipoGasto.Directo,
                        ColaboradorId = directo.ColaboradorId,
                        //    RubroId = ColaboradorRubro!=null? ColaboradorRubro.RubroId:null,
                        UnidadId = Unidad != null ? Unidad.Id : 0,
                        TotalHoras = directo.NumeroHoras,
                        TarifaHoras = directo.monto,
                        Tarifa = directo.tarifa,

                        EsDistribucionE500 = false,
                        migrado = directo.migrado,




                    };
                    if (ColaboradorRubro != null)
                    {
                        GastoDirecto.RubroId = ColaboradorRubro.RubroId;
                    }
                    _gastoDirectoRepository.Insert(GastoDirecto);
                }



                /*Actualizar Campo CertificadoId*/

                foreach (var DirectoId in DirectosParaActualizarCampoCertificadoId)

                {
                    var directo = _directosRepository.Get(DirectoId);
                    directo.CertificadoId = CertificadoProyectoId;
                    _directosRepository.Update(directo);

                }


            }






            var detallesIndirectosIngenieria = _indirectosRepository.GetAllIncluding(c => c.ColaboradorRubro)
                                                                    .Where(c => IndirectosIds.Contains(c.Id))
                                                                    .ToList();


            decimal HorasTotalesCertificado = HorasGlobal;

            // var detallesE500 = _directoE50.GetAll().Where(c => E500.Contains(c.Id)).ToList();

            //DISTRIBUCION DIRECTOS E500 A CERTIFICADOS
            foreach (var directoE500 in detallesE500)
            {
                foreach (var ProyectoId in ProyectosDirectosId)
                {
                    var certificado = (from cert in CertificadosPorProyectos
                                       where cert.ProyectoId == ProyectoId
                                       select cert).FirstOrDefault();
                    if (certificado != null)
                    {
                        var porcentajeProyecto = (certificado.HorasActualCertificado) / HorasTotalesCertificado;

                        //var entidadProyecto = _proyectoRepository.GetAll().Where(c => c.Id == ProyectoId).Where(c => c.vigente).FirstOrDefault();
                        var entidadProyecto = (from p in ProyectoDirectos where p.Id == ProyectoId select p).FirstOrDefault();

                        decimal HorasPorcentuales = directoE500.NumeroHoras * porcentajeProyecto; //Horas por el Porcentaje Correspondiente al proyecto

                        /*   var ColaboradorRubro = _colaboradoRubroRepository.GetAll()
                                                                      .Where(c => c.ColaboradorId == directoE500.ColaboradorId)
                                                                      .Where(c => c.ContratoId == entidadProyecto.contratoId)
                                                                      .FirstOrDefault();*/
                        var ColaboradorRubro = (from cr in ColaboradoresRubro
                                                where cr.ColaboradorId == directoE500.ColaboradorId
                                                where cr.ContratoId == entidadProyecto.contratoId
                                                select cr)
                                                        .FirstOrDefault();

                        var GastoDirectoE500 = new GastoDirectoCertificado()
                        {
                            Id = 0,
                            CertificadoIngenieriaProyectoId = certificado.CertificadoProyectoId,
                            TipoGastoId = TipoGasto.Directo,
                            ColaboradorId = directoE500.ColaboradorId,
                            // RubroId = ColaboradorRubro.RubroId,
                            UnidadId = Unidad != null ? Unidad.Id : 0,
                            TotalHoras = HorasPorcentuales,

                            EsDistribucionE500 = true,
                        };


                        if (ColaboradorRubro != null)
                        {
                            var montoUSD = HorasPorcentuales * ColaboradorRubro.Tarifa; //USD Porcentaje de Horas por Tarifa



                            GastoDirectoE500.RubroId = ColaboradorRubro.RubroId;
                            GastoDirectoE500.TarifaHoras = montoUSD;
                            GastoDirectoE500.Tarifa = ColaboradorRubro.Tarifa;
                        }


                        _gastoDirectoRepository.Insert(GastoDirectoE500);

                        var entity = _directoE50.Get(directoE500.Id);
                        entity.CertificadoId = certificado.CertificadoProyectoId;
                        _directoE50.Update(entity);
                    }
                }

            }


            //DISTRIBUCION INDIRECOTS A PROYECTOS
            foreach (var indirecto in detallesIndirectosIngenieria)
            {

                foreach (var ProyectoId in ProyectosDirectosId)
                {
                    var certificado = (from cert in CertificadosPorProyectos
                                       where cert.ProyectoId == ProyectoId
                                       select cert).FirstOrDefault();
                    if (certificado != null)
                    {
                        var porcentajeProyecto = (certificado.HorasActualCertificado) / HorasTotalesCertificado;
                        // var entidadProyecto = _proyectoRepository.GetAll().Where(c => c.Id == ProyectoId).Where(c => c.vigente).FirstOrDefault();
                        var entidadProyecto = (from p in ProyectoDirectos where p.Id == ProyectoId select p).FirstOrDefault();
                        decimal HorasPorcentuales = indirecto.HorasLaboradas * porcentajeProyecto; //Horas por el Porcentaje Correspondiente al proyecto
                        var montoUSD = HorasPorcentuales * indirecto.ColaboradorRubro.Tarifa; //USD Porcentaje de Horas por Tarifa

                        var indirectoEntity = new GastoDirectoCertificado()
                        {
                            Id = 0,
                            CertificadoIngenieriaProyectoId = certificado.CertificadoProyectoId,
                            TipoGastoId = TipoGasto.Indirecto,
                            ColaboradorId = indirecto.ColaboradorRubro.ColaboradorId,
                            RubroId = indirecto.ColaboradorRubro.RubroId,
                            UnidadId = Unidad != null ? Unidad.Id : 0,
                            TotalHoras = HorasPorcentuales,
                            TarifaHoras = montoUSD,
                            Tarifa = indirecto.ColaboradorRubro.Tarifa,
                            EsDistribucionE500 = false,
                            migrado = false
                        };

                        _gastoDirectoRepository.Insert(indirectoEntity);

                        var entity = _indirectosRepository.Get(indirecto.Id);
                        entity.CertificadoId = certificado.CertificadoProyectoId;
                        _indirectosRepository.Update(entity);
                    }
                }

            }

            return new ResultadoCertificacion
            {
                Success = true,
                Message = "OK"
            };

        }

        public ResultadoCertificacion UltimoCertificadiGenerado()
        {
            var entity = Repository.GetAll().OrderByDescending(o => o.FechaFin)
                .FirstOrDefault();

            if (entity == null)
            {
                return new ResultadoCertificacion()
                {
                    Success = false
                };
            }
            else if (entity.EstadoId == EstadoGrupoCertificado.Generado)
            {
                var dto = Mapper.Map<GrupoCertificadoIngenieriaDto>(entity);
                return new ResultadoCertificacion()
                {
                    Grupo = dto,
                    Success = true
                };
            }
            return new ResultadoCertificacion()
            {
                Success = false
            }; ;
        }

        public bool Eliminar(int id)
        {

            try
            {


                var entity = Repository.Get(id);

                string connectionString = ConfigurationManager.ConnectionStrings["DbConnectionString"].ConnectionString;


                //UPDATE REGISTROS CERTIFICADOS

                var fechaInicio = entity.FechaInicio.Date;
                var fechaFin = entity.FechaFin.Date;

                var fechamaximaEliminacion = new DateTime(2022, 05, 20);
                bool puedeeliminar = false;

                if (fechaInicio > fechamaximaEliminacion)
                {
                    puedeeliminar = true;
                }


                if (puedeeliminar)
                {

                    string queryUpdateDirectos = "UPDATE SCH_INGENIERIA.detalles_directos_ingenieria SET certificado_id = NULL WHERE certificado_id in  (select c.Id from SCH_INGENIERIA.certificados_ingenieria c where c.GrupoCertificadoIngenieriaId in (" + id + "));";
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        SqlCommand command = new SqlCommand(queryUpdateDirectos, connection);
                        command.Connection.Open();
                        var afectados = command.ExecuteNonQuery();
                        connection.Close();
                    }
                    string updateDirectosE500 = "UPDATE SCH_INGENIERIA.detalles_directos_e500 SET certificado_id = NULL WHERE certificado_id  in (" + id + "); ";
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        SqlCommand command = new SqlCommand(updateDirectosE500, connection);
                        command.Connection.Open();
                        var afectados = command.ExecuteNonQuery();
                        connection.Close();
                    }
                    string updateIndirectos = "UPDATE SCH_INGENIERIA.detalles_indirectos_ingenieria SET CertificadoId = NULL where CertificadoId in (" + id + ");";
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        SqlCommand command = new SqlCommand(updateIndirectos, connection);
                        command.Connection.Open();
                        var afectados = command.ExecuteNonQuery();
                        connection.Close();
                    }




                    //DELETE REGISTROS CERTIFICADOS
                    string queryElimianarTablaGastosporGrupo = "delete from SCH_INGENIERIA.gastos_directos_certificado where CertificadoIngenieriaProyectoId in (select c.Id from SCH_INGENIERIA.certificados_ingenieria c where c.GrupoCertificadoIngenieriaId in (" + id + ")); ";


                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        SqlCommand command = new SqlCommand(queryElimianarTablaGastosporGrupo, connection);
                        command.Connection.Open();
                        var afectados = command.ExecuteNonQuery();
                        connection.Close();
                    }
                    string queryEliminarCertificadosporGrupo = "delete from SCH_INGENIERIA.certificados_ingenieria where GrupoCertificadoIngenieriaId in (" + id + ");";
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        SqlCommand command = new SqlCommand(queryEliminarCertificadosporGrupo, connection);
                        command.Connection.Open();
                        var afectados = command.ExecuteNonQuery();
                        connection.Close();
                    }
                    string queryEliminarResumenPorGrupo = "delete from SCH_INGENIERIA.resumen_certificado_ingenieria where GrupoCertificadoIngenieriaId in (" + id + "); ";
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        SqlCommand command = new SqlCommand(queryEliminarResumenPorGrupo, connection);
                        command.Connection.Open();
                        var afectados = command.ExecuteNonQuery();
                        connection.Close();
                    }
                    string EliminarDistribucionPorGrupo = "delete from SCH_INGENIERIA.distribucion_certificado_ingenieria where GrupoCertificadoId in (" + id + ");";
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        SqlCommand command = new SqlCommand(EliminarDistribucionPorGrupo, connection);
                        command.Connection.Open();
                        var afectados = command.ExecuteNonQuery();
                        connection.Close();
                    }
                    string EliminarGrupo = "delete from SCH_INGENIERIA.grupo_certificado_ingenieria where id in (" + id + "); ";
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        SqlCommand command = new SqlCommand(EliminarGrupo, connection);
                        command.Connection.Open();
                        var afectados = command.ExecuteNonQuery();
                        connection.Close();
                    }
                }
                return true;

            }
            catch (Exception e)
            {
                ElmahExtension.LogToElmah(e);
                ElmahExtension.LogToElmah(new Exception("Eliminado QUery" + e.Message));

                return false;
            }

            /* try
             {
                 var certificados = _certificadoIngenieriaProyectoRepository.GetAll().Where(c => c.GrupoCertificadoIngenieriaId == id).ToList();
                 if (certificados.Count > 0)
                 {
                     foreach (var cert in certificados)
                     {
                         var gastos = _gastoDirectoRepository.GetAll().Where(c => c.CertificadoIngenieriaProyectoId == cert.Id).ToList();

                         var directos = _directosRepository.GetAll().Where(c => c.CertificadoId.HasValue).Where(c => c.CertificadoId == cert.Id).ToList();
                         var directos_e = _directoE50.GetAll().Where(c => c.CertificadoId.HasValue).Where(c => c.CertificadoId == cert.Id).ToList();
                         var indirectos = _indirectosRepository.GetAll().Where(c => c.CertificadoId.HasValue).Where(c => c.CertificadoId == cert.Id).ToList();


                         foreach (var dir in directos)
                         {
                             var d = _directosRepository.Get(dir.Id);
                             d.CertificadoId = null;
                             _directosRepository.Update(d);

                         }
                         foreach (var dir in directos_e)
                         {
                             var d = _directoE50.Get(dir.Id);
                             d.CertificadoId = null;
                             _directoE50.Update(d);

                         }

                         foreach (var indir in indirectos)
                         {
                             var ind = _indirectosRepository.Get(indir.Id);
                             ind.CertificadoId = null;
                             _indirectosRepository.Update(ind);

                         }

                         foreach (var gas in gastos)
                         {
                             _gastoDirectoRepository.Delete(gas.Id);
                         }
                         _certificadoIngenieriaProyectoRepository.Delete(cert.Id);
                     }
                 }


                 Repository.Delete(entity);
                 return true;
             }
             catch (Exception e)
             {

                 return false;
             }
             */
        }

        public bool ActualizarCampoCertificacion(ProyectosCertificacion input)
        {
            var proyecto = _proyectoRepository.Get(input.Id);
            proyecto.UbicacionId = input.UbicacionId;
            proyecto.PortafolioId = input.PortafolioId;
            proyecto.ProyectoCerrado = input.ProyectoCerrado;
            proyecto.certificable_ingenieria = input.ProyectoCertificable;
            return true;

        }
        public List<ProyectosCertificacion> ObtenerProyectos()
        {
            var query = _proyectoRepository.GetAllIncluding(c => c.Contrato).Where(c => c.vigente).ToList();
            var list = (from p in query
                        select new ProyectosCertificacion()
                        {
                            Id = p.Id,
                            codigo_contrato = p.Contrato.Codigo,
                            nombre_contrato = p.Contrato.descripcion,
                            codigo_proyecto = p.codigo,
                            nombre_proyecto = p.nombre_proyecto,
                            PortafolioId = p.PortafolioId,
                            ProyectoCerrado = p.ProyectoCerrado,
                            UbicacionId = p.UbicacionId,
                            ProyectoCertificable = p.certificable_ingenieria
                        }).ToList();
            return list;
        }
        public List<GrupoCertificadoIngenieriaDto> GetCertificados(DateTime? fechaInicio, DateTime? fechaFin)
        {
            var query = Repository.GetAllIncluding(a => a.Cliente);

            if (fechaInicio.HasValue)
            {
                var fechaInicioDate = fechaInicio.Value.Date;
                var fechaFinDate = fechaFin.Value.Date;

                query = query
                    .Where(o => o.FechaInicio >= fechaInicioDate && o.FechaInicio <= fechaFinDate)
                    .Where(o => o.FechaFin <= fechaFinDate);
            }
            var list = query
                .OrderByDescending(o => o.FechaCertificado)
                .ToList();

            var dto = (from d in list
                       select new GrupoCertificadoIngenieriaDto()
                       {
                           Id = d.Id,
                           ClienteId = d.ClienteId,
                           NombreCliente = d.Cliente.codigo_sap + " - " + d.Cliente.razon_social,
                           FechaCertificado = d.FechaCertificado,
                           FechaCertificadoDate = d.FechaCertificado.ToShortDateString(),
                           FechaGeneracion = d.FechaGeneracion,
                           FechaGeneracionDate = d.FechaGeneracion.ToShortDateString(),
                           FechaInicio = d.FechaInicio,
                           FechaInicioDate = d.FechaInicio.ToShortDateString(),
                           FechaFin = d.FechaFin,
                           FechaFinDate = d.FechaFin.ToShortDateString(),
                           EstadoId = d.EstadoId,
                           EstadoString = Enum.GetName(typeof(EstadoGrupoCertificado), d.EstadoId),
                           Mes = d.Mes,
                           Anio = d.Anio
                       })
                       .OrderByDescending(o => o.FechaCertificado)
                       .ToList();
            return dto;
        }

        public ResultadoCertificacion validarFechasCertificado(DateTime fechaInicio, DateTime fechaFin, DateTime fechaCertificacion, int clienteId)
        {
            var ultimoCertificado = Repository.GetAll()
                .Where(o => o.ClienteId == clienteId)
                .OrderByDescending(o => o.FechaCertificado)
                .FirstOrDefault();

            if (ultimoCertificado == null)
            {
                return new ResultadoCertificacion() { Success = true, Message = "" };
            }
            else if (fechaCertificacion < ultimoCertificado.FechaCertificado)
            {
                return new ResultadoCertificacion()
                {
                    Success = false,
                    Message = "La fecha de certificación no podrá ser menor a la del último certificado generado."
                };
            }

            var tieneFechasSobrepuestas = ComprobarSobrePosicionDeFechas(fechaInicio, fechaFin, clienteId, 0);
            if (tieneFechasSobrepuestas)
            {
                return new ResultadoCertificacion()
                {
                    Success = false,
                    Message = "Los periodos de certificación, no pueden superponerse con otros periodos de certificación."
                };
            }
            return new ResultadoCertificacion() { Success = true, Message = "" };
        }

        public bool ComprobarSobrePosicionDeFechas(DateTime fechainicio, DateTime fechafin, int clienteId, int grupoCertificadoId)
        {
            var grupoCertificado = Repository.GetAll()
                .Where(c => c.ClienteId == clienteId);

            if (grupoCertificadoId > 0)
            {
                grupoCertificado = grupoCertificado.Where(o => o.Id != grupoCertificadoId);
            }

            var grupos = grupoCertificado.ToList();

            bool result = false;
            foreach (var item in grupos)
            {
                if (fechainicio >= item.FechaInicio && fechafin <= item.FechaFin)
                {
                    result = true;
                    ;
                    break;
                }

                if (item.FechaInicio > fechainicio && item.FechaFin < fechafin)
                {
                    result = true;
                    ;
                    break;
                }

                if (fechainicio > item.FechaInicio && fechainicio < item.FechaFin)
                {
                    result = true;
                    ;
                    break;
                }

                if (fechafin > item.FechaInicio && fechafin < item.FechaFin)
                {
                    result = true;
                    ;
                    break;
                }
            }

            return result;
        }

        public string nombreExcelGrupoCertificado(int GrupoCertificadoId)
        {
            var GrupoCertificado = Repository.GetAll().Where(c => c.Id == GrupoCertificadoId).FirstOrDefault();
            var Date = GrupoCertificado != null ? GrupoCertificado.FechaCertificado : DateTime.Now;

            string anio = Date.ToString("yyyy");
            string mes = Date.ToString("MM");
            string dia = Date.ToString("dd");

            string excelName = anio + mes + dia + " REPORTE SEMANAL ING";
            return excelName;
        }

        public List<CertificadoIngenieriaProyectoDto> CertificadosPorGrupo(int GrupoCertificadoId)
        {
            var query = _certificadoIngenieriaProyectoRepository.GetAllIncluding(c => c.GrupoCertificadoIngenieria, c => c.Proyecto.Contrato)
                                                                .Where(c => c.GrupoCertificadoIngenieriaId == GrupoCertificadoId)
                                                                .ToList();

            int precisionDecimales = 2;
            var dto = (from q in query
                       select new CertificadoIngenieriaProyectoDto()
                       {
                           Id = q.Id,
                           GrupoCertificadoIngenieriaId = q.GrupoCertificadoIngenieriaId,
                           ProyectoId = q.ProyectoId,
                           GrupoCertificadoIngenieriaString = q.GrupoCertificadoIngenieria.FechaInicio.ToShortDateString() + " - " + q.GrupoCertificadoIngenieria.FechaFin.ToShortDateString(),
                           AvanceRealIngenieria = q.AvanceRealIngenieria,
                           DistribucionDirectos = q.DistribucionDirectos,
                           DistribucionDirectosString = q.DistribucionDirectos ? "SI" : "NO",
                           EstadoId = q.EstadoId,
                           EstadoString = Enum.GetName(typeof(EstadoCertificadoProyecto), q.EstadoId),
                           HorasActualCertificadas = Decimal.Round(q.HorasActualCertificadas, precisionDecimales),
                           HorasAnteriorCertificadas = Decimal.Round(q.HorasAnteriorCertificadas, precisionDecimales),
                           HorasPresupuestadas = Decimal.Round(q.HorasPresupuestadas, precisionDecimales),
                           MontoActualCertificado = Decimal.Round(q.MontoActualCertificado, precisionDecimales),
                           MontoAnteriorCertificado = Decimal.Round(q.MontoAnteriorCertificado, precisionDecimales),
                           NombreContrato = q.Proyecto.Contrato.Codigo,
                           NombreProyecto = q.Proyecto.codigo,
                           NumeroCertificado = q.NumeroCertificado,
                           NumeroCertificadoString = q.NumeroCertificado.ToString(),
                           OrdenServicioId = q.OrdenServicioId,
                           PorcentajeAsbuilt = q.PorcentajeAsbuilt,
                           TotalHorasDirectos = Decimal.Round(this.HorasTotalesDirectosCertificado(q.Id), precisionDecimales),
                           TotalHorasIndirectos = Decimal.Round(this.HorasTotalesIndirectosCertificado(q.Id), precisionDecimales),
                       }).ToList();

            return dto;

        }

        public decimal HorasTotalesDirectosCertificado(int CertificacionIngenieriaProyectoId)
        {
            var query = _gastoDirectoRepository.GetAllIncluding(a => a.CertificadoIngenieriaProyecto, a => a.Colaborador, a => a.Rubro.Item, a => a.Unidad)
                                              .Where(a => a.CertificadoIngenieriaProyectoId == CertificacionIngenieriaProyectoId)
                                              .Where(a => a.TipoGastoId == TipoGasto.Directo)
                                              .ToList();
            if (query.Count > 0)
            {
                return query.Sum(c => c.TotalHoras * c.Tarifa);
            }
            return 0;

        }
        public decimal HorasTotalesIndirectosCertificado(int CertificacionIngenieriaProyectoId)
        {
            var query = _gastoDirectoRepository.GetAllIncluding(a => a.CertificadoIngenieriaProyecto, a => a.Colaborador, a => a.Rubro.Item, a => a.Unidad)
                                              .Where(a => a.CertificadoIngenieriaProyectoId == CertificacionIngenieriaProyectoId)
                                              .Where(a => a.TipoGastoId == TipoGasto.Indirecto)
                                              .ToList();
            if (query.Count > 0)
            {
                return query.Sum(c => c.TotalHoras * c.Tarifa);
            }
            return 0;

        }

        public List<GastoDirectoCertificadoDto> GastosDirectosCertificado(int CertificacionIngenieriaProyectoId)
        {

            int precisionDecimales = 2;
            var query = _gastoDirectoRepository.GetAllIncluding(a => a.CertificadoIngenieriaProyecto, a => a.Colaborador, a => a.Rubro.Item, a => a.Unidad)
                                               .Where(a => a.CertificadoIngenieriaProyectoId == CertificacionIngenieriaProyectoId)
                                               // .Where(a => a.RubroId.HasValue)
                                               .ToList();
            var dto = (from g in query
                       select new GastoDirectoCertificadoDto()
                       {
                           Id = g.Id,
                           CertificadoIngenieriaProyectoId = g.CertificadoIngenieriaProyectoId,
                           CertificadoIngenieriaProyectoString = g.CertificadoIngenieriaProyecto.NumeroCertificado + "",
                           ColaboradorId = g.ColaboradorId,
                           ColaboradoresNombresCompletos = g.Colaborador.nombres_apellidos,
                           ColaboradoresIdentificacion = g.Colaborador.numero_identificacion,
                           RubroId = g.RubroId,
                           RubroCodigoString = g.RubroId.HasValue ? g.Rubro.Item.codigo : "",
                           RubroString = g.RubroId.HasValue ? g.Rubro.Item.nombre : "",
                           UnidadId = g.UnidadId,
                           UnidadString = g.UnidadId.HasValue ? g.Unidad.nombre.ToUpper() : "",
                           TarifaHoras = g.TarifaHoras,
                           TipoGastoId = g.TipoGastoId,
                           TipoGastoString = Enum.GetName(typeof(TipoGasto), g.TipoGastoId),
                           TotalHoras = Decimal.Round(g.TotalHoras, precisionDecimales),
                           MontoTotal = Decimal.Round((g.TotalHoras * g.Tarifa), precisionDecimales),
                           EsDistribucionE500 = g.EsDistribucionE500,
                           es500String = g.EsDistribucionE500 ? "SI" : "NO"
                       }).ToList();

            return dto;

        }

        public string ObtenerNombreMesIndirectos(DateTime fecha)
        {

            var fechaActual = fecha;
            int day = fecha.Day;
            if (day > 20)
            {
                fechaActual = fecha.AddMonths(1);
            }
            return fechaActual.ToString("MMMM").ToUpper();

        }
        public DetallesSinCertificar GetDetallesSinCertificar(DateTime? fechaInicio, DateTime? fechaFin, int ClienteId)
        {
            var porcentajeMaximoDistribucion = _parametroRepository.GetAll().Where(c => c.Codigo == ProyectoCodigos.MAXIMO_PORCENTAJE_DISTRIBUCION_INDIRECTOS).FirstOrDefault();
            var FechaInicialDate = fechaInicio.Value.Date;
            var FechaFinalDate = fechaFin.Value.Date;

            var queryDirectos = _directosRepository.GetAllIncluding(c => c.TipoRegistro, c => c.Colaborador, c => c.Proyecto.Contrato, c => c.Locacion, c => c.Modalidad, c => c.EstadoRegistro)
                                                    .Where(c => c.FechaTrabajo >= FechaInicialDate)//comentar
                                                    .Where(c => c.FechaTrabajo <= FechaFinalDate)
                                                   .Where(c => c.Proyecto.Contrato.ClienteId == ClienteId)
                                                   .Where(c => !c.CertificadoId.HasValue)//No este Certificado;
                                                    .ToList();


            var precisionDecimal = 2;
            var directoSinCertificar = (from q in queryDirectos
                                        select new DetallesDirectosIngenieriaDto()
                                        {
                                            Id = q.Id,
                                            CertificadoId = q.CertificadoId,
                                            CodigoProyecto = q.CodigoProyecto,
                                            ColaboradorId = q.ColaboradorId,
                                            nombreColaborador = q.ColaboradorId > 0 ? q.Colaborador.nombres_apellidos : "",
                                            EsDirecto = q.EsDirecto,
                                            EspecialidadId = q.EspecialidadId,
                                            EstadoRegistroId = q.EstadoRegistroId,
                                            EtapaId = q.EtapaId,
                                            FechaTrabajo = q.FechaTrabajo,
                                            formatFechaTrabajo = q.FechaTrabajo.ToShortDateString(),
                                            nombreProyecto = q.ProyectoId > 0 ? q.Proyecto.codigo : "",
                                            NumeroHoras = Decimal.Round(q.NumeroHoras, precisionDecimal),
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
                                            nombreEstado = q.EstadoRegistroId > 0 ? q.EstadoRegistro.nombre : "",
                                            contratoId = q.Proyecto.contratoId,
                                            total_migracion = Decimal.Round(q.total_migracion, 2),
                                            tarifa_migracion = Decimal.Round(q.tarifa_migracion, 2),
                                            migrado = q.migrado
                                        }).ToList();


            /*Pendientes de Certificar*/
            var queryDirectosPendientes = _directosRepository.GetAllIncluding(c => c.TipoRegistro, c => c.Colaborador, c => c.Proyecto.Contrato, c => c.Locacion, c => c.Modalidad, c => c.EstadoRegistro)
                                                       .Where(c => c.FechaTrabajo < FechaInicialDate)//comentar
                                                       .Where(c => c.Proyecto.Contrato.ClienteId == ClienteId)
                                                       .Where(c => !c.CertificadoId.HasValue)//No este Certificado;
                                                       .ToList();

            var directoSinCertificarPendientes = (from q in queryDirectosPendientes
                                                  select new DetallesDirectosIngenieriaDto()
                                                  {
                                                      Id = q.Id,
                                                      CertificadoId = q.CertificadoId,
                                                      CodigoProyecto = q.CodigoProyecto,
                                                      ColaboradorId = q.ColaboradorId,
                                                      nombreColaborador = q.ColaboradorId > 0 ? q.Colaborador.nombres_apellidos : "",
                                                      EsDirecto = q.EsDirecto,
                                                      EspecialidadId = q.EspecialidadId,
                                                      EstadoRegistroId = q.EstadoRegistroId,
                                                      EtapaId = q.EtapaId,
                                                      FechaTrabajo = q.FechaTrabajo,
                                                      formatFechaTrabajo = q.FechaTrabajo.ToShortDateString(),
                                                      nombreProyecto = q.ProyectoId > 0 ? q.Proyecto.codigo : "",
                                                      NumeroHoras = Decimal.Round(q.NumeroHoras, precisionDecimal),
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
                                                      nombreEstado = q.EstadoRegistroId > 0 ? q.EstadoRegistro.nombre : "",
                                                      contratoId = q.Proyecto.contratoId,
                                                      total_migracion = Decimal.Round(q.total_migracion, 2),
                                                      tarifa_migracion = Decimal.Round(q.tarifa_migracion, 2),
                                                      migrado = q.migrado
                                                  }).ToList();






            var queryDirectosE500 = _directoE50.GetAllIncluding(c => c.TipoRegistro, c => c.Colaborador, c => c.Cliente, c => c.Locacion, c => c.Modalidad, c => c.EstadoRegistro)
                                       .Where(c => c.FechaTrabajo >= FechaInicialDate) //comentar
                                       .Where(c => c.FechaTrabajo <= FechaFinalDate)
                                       .Where(c => c.ClienteId == ClienteId)
                                       .Where(c => !c.CertificadoId.HasValue)//No este Certificado;
                                        .ToList();
            var directoE500SinCertificar = (from q in queryDirectosE500
                                            select new DetalleDirectoE500Dto()
                                            {
                                                Id = q.Id,
                                                CertificadoId = q.CertificadoId,
                                                ColaboradorId = q.ColaboradorId,
                                                nombreColaborador = q.ColaboradorId > 0 ? q.Colaborador.nombres_apellidos : "",
                                                EspecialidadId = q.EspecialidadId,
                                                EstadoRegistroId = q.EstadoRegistroId,
                                                EtapaId = q.EtapaId,
                                                FechaTrabajo = q.FechaTrabajo,
                                                formatFechaTrabajo = q.FechaTrabajo.ToShortDateString(),
                                                nombreCliente = q.ClienteId > 0 ? q.Cliente.razon_social : "",
                                                NumeroHoras = Decimal.Round(q.NumeroHoras, precisionDecimal),
                                                NombreEjecutante = q.NombreEjecutante,
                                                ModalidadId = q.ModalidadId,
                                                Observaciones = q.Observaciones,
                                                ClienteId = q.ClienteId,
                                                Identificacion = q.Identificacion,
                                                LocacionId = q.LocacionId,
                                                nombreLocacion = q.LocacionId.HasValue ? q.Locacion.nombre : "",
                                                nombreModalidad = q.ModalidadId.HasValue ? q.Modalidad.nombre : "",
                                                FechaCarga = q.FechaCarga,
                                                TipoRegistroId = q.TipoRegistroId,
                                                formatFechaCarga = q.FechaCarga.ToShortDateString(),
                                                nombreEstado = q.EstadoRegistroId > 0 ? q.EstadoRegistro.nombre : "",

                                            }).ToList();



            var queryDirectosE500Pendientes = _directoE50.GetAllIncluding(c => c.TipoRegistro, c => c.Colaborador, c => c.Cliente, c => c.Locacion, c => c.Modalidad, c => c.EstadoRegistro)
                                                        .Where(c => c.FechaTrabajo < FechaInicialDate)
                                                        .Where(c => c.ClienteId == ClienteId)
                                                        .Where(c => !c.CertificadoId.HasValue)//No este Certificado;
                                                        .ToList();

            var directoE500SinCertificarPendientes = (from q in queryDirectosE500Pendientes
                                                      select new DetalleDirectoE500Dto()
                                                      {
                                                          Id = q.Id,
                                                          CertificadoId = q.CertificadoId,
                                                          ColaboradorId = q.ColaboradorId,
                                                          nombreColaborador = q.ColaboradorId > 0 ? q.Colaborador.nombres_apellidos : "",
                                                          EspecialidadId = q.EspecialidadId,
                                                          EstadoRegistroId = q.EstadoRegistroId,
                                                          EtapaId = q.EtapaId,
                                                          FechaTrabajo = q.FechaTrabajo,
                                                          formatFechaTrabajo = q.FechaTrabajo.ToShortDateString(),
                                                          nombreCliente = q.ClienteId > 0 ? q.Cliente.razon_social : "",
                                                          NumeroHoras = Decimal.Round(q.NumeroHoras, precisionDecimal),
                                                          NombreEjecutante = q.NombreEjecutante,
                                                          ModalidadId = q.ModalidadId,
                                                          Observaciones = q.Observaciones,
                                                          ClienteId = q.ClienteId,
                                                          Identificacion = q.Identificacion,
                                                          LocacionId = q.LocacionId,
                                                          nombreLocacion = q.LocacionId.HasValue ? q.Locacion.nombre : "",
                                                          nombreModalidad = q.ModalidadId.HasValue ? q.Modalidad.nombre : "",
                                                          FechaCarga = q.FechaCarga,
                                                          TipoRegistroId = q.TipoRegistroId,
                                                          formatFechaCarga = q.FechaCarga.ToShortDateString(),
                                                          nombreEstado = q.EstadoRegistroId > 0 ? q.EstadoRegistro.nombre : "",

                                                      }).ToList();


            foreach (var directo in directoE500SinCertificar)
            {
                var fechaTrabajoDate = directo.FechaTrabajo.Date;
                var colaboradorRubro = _colaboradoRubroRepository.GetAll()
                                                               .Where(c => c.ColaboradorId == directo.ColaboradorId)
                                                               .Where(c => fechaTrabajoDate >= c.FechaInicio)
                                                               .Where(c => fechaTrabajoDate <= c.FechaFin)
                                                               .Where(c => fechaTrabajoDate >= c.Rubro.Preciario.fecha_desde)
                                                               .Where(c => fechaTrabajoDate <= c.Rubro.Preciario.fecha_hasta)
                                                               .FirstOrDefault();
                if (colaboradorRubro != null)
                {
                    directo.tarifa = colaboradorRubro.Tarifa;
                    directo.monto = Decimal.Round(directo.NumeroHoras * colaboradorRubro.Tarifa, precisionDecimal);
                }
            }

            foreach (var directo in directoE500SinCertificarPendientes)
            {
                var fechaTrabajoDate = directo.FechaTrabajo.Date;
                var colaboradorRubro = _colaboradoRubroRepository.GetAll()
                                                               .Where(c => c.ColaboradorId == directo.ColaboradorId)
                                                               .Where(c => fechaTrabajoDate >= c.FechaInicio)
                                                               .Where(c => fechaTrabajoDate <= c.FechaFin)
                                                               .Where(c => fechaTrabajoDate >= c.Rubro.Preciario.fecha_desde)
                                                               .Where(c => fechaTrabajoDate <= c.Rubro.Preciario.fecha_hasta)
                                                               .FirstOrDefault();
                if (colaboradorRubro != null)
                {
                    directo.tarifa = colaboradorRubro.Tarifa;
                    directo.monto = Decimal.Round(directo.NumeroHoras * colaboradorRubro.Tarifa, precisionDecimal);
                }
            }

            foreach (var directo in directoSinCertificar)
            {


                if (!directo.migrado)
                {
                    var fechaTrabajoDate = directo.FechaTrabajo.Date;
                    var colaboradorRubro = _colaboradoRubroRepository.GetAll()
                                                                   .Where(c => c.ColaboradorId == directo.ColaboradorId)
                                                                   .Where(c => fechaTrabajoDate >= c.FechaInicio)
                                                                   .Where(c => fechaTrabajoDate <= c.FechaFin)
                                                                   .Where(c => c.ContratoId == directo.contratoId)
                                                                   .FirstOrDefault();
                    if (colaboradorRubro != null)
                    {
                        directo.tarifa = colaboradorRubro.Tarifa;
                        directo.monto = Decimal.Round(directo.NumeroHoras * colaboradorRubro.Tarifa, precisionDecimal);
                    }
                }
                else
                {
                    directo.tarifa = Decimal.Round(directo.tarifa_migracion, precisionDecimal);
                    directo.monto = Decimal.Round(directo.total_migracion, precisionDecimal);
                }


            }

            foreach (var directo in directoSinCertificarPendientes)
            {
                if (!directo.migrado)
                {
                    var fechaTrabajoDate = directo.FechaTrabajo.Date;
                    var colaboradorRubro = _colaboradoRubroRepository.GetAll()
                                                                   .Where(c => c.ColaboradorId == directo.ColaboradorId)
                                                                   .Where(c => fechaTrabajoDate >= c.FechaInicio)
                                                                   .Where(c => fechaTrabajoDate <= c.FechaFin)
                                                                   .Where(c => c.ContratoId == directo.contratoId)
                                                                   .FirstOrDefault();
                    if (colaboradorRubro != null)
                    {
                        directo.tarifa = colaboradorRubro.Tarifa;
                        directo.monto = Decimal.Round(directo.NumeroHoras * colaboradorRubro.Tarifa, precisionDecimal);
                    }
                }
                else
                {
                    directo.tarifa = Decimal.Round(directo.tarifa_migracion, precisionDecimal);
                    directo.monto = Decimal.Round(directo.total_migracion, precisionDecimal);
                }


            }




            var queryCarpetas = _indirectosRepository.GetAll()
                                                        .Include(o => o.ColaboradorRubro)
                                                        .Include(o => o.ColaboradorRubro.Rubro.Item)
                                                        .Include(o => o.ColaboradorRubro.Colaborador)
                                                        .Where(c => !c.CertificadoId.HasValue);//Sin Certificar

            var queryIndirectosPendientes = queryCarpetas;



            if (fechaFin.HasValue)
            {
                queryCarpetas = queryCarpetas
                     .Where(o => o.FechaDesde >= fechaInicio)
                     .Where(o => o.FechaDesde <= fechaFin);
                // .Where(o => o.FechaHasta <= fechaFin);
            }
            var list = queryCarpetas.ToList();
            var indirectosSinCertificar = Mapper.Map<List<DetalleIndirectosIngenieriaDto>>(list);

            foreach (var indirecto in indirectosSinCertificar)
            {
                /* Indirectos Mes*
                 */
                var mesString = this.ObtenerNombreMesIndirectos(indirecto.FechaDesde);
                indirecto.mes = mesString;

                /*Valor Colaborador Rubro Tarifa y monto*/
                var fechaTrabajoDate = indirecto.FechaDesde.Date;
                var colaboradorRubro = _colaboradoRubroRepository.GetAll()
                                                               .Where(c => c.ColaboradorId == indirecto.ColaboradorId)
                                                                  .Where(c => fechaTrabajoDate >= c.FechaInicio)
                                                                   .Where(c => fechaTrabajoDate <= c.FechaFin)
                                                               .Where(c => c.ContratoId == indirecto.contratoId)
                                                               .FirstOrDefault();
                if (colaboradorRubro != null)
                {

                    indirecto.tarifa = colaboradorRubro.Tarifa;
                    indirecto.monto = (indirecto.HorasLaboradas * indirecto.DiasLaborados) * colaboradorRubro.Tarifa;
                }


                /*Proyectos 
                string nodistribuir = "";
                if (indirecto.distribucion_proyectos != null && indirecto.distribucion_proyectos.Length > 0)
                {
                    var listproyectos = indirecto.distribucion_proyectos.Split(',');
                    if (listproyectos.Length > 0)
                    {
                        foreach (var p in listproyectos)
                        {
                            if (p.Length > 0)
                            {
                                var id = Int32.Parse(p);
                                var pro = _proyectoRepository.GetAll().Where(c => c.Id == id).FirstOrDefault();
                                if (pro != null)
                                {
                                    nodistribuir = nodistribuir + "," + pro.codigo;
                                }
                            }
                        }
                    }
                }
                indirecto.ProyectosCodigosString = nodistribuir;
                */
            }


            if (fechaInicio.HasValue)
            {
                queryIndirectosPendientes = queryIndirectosPendientes
              // .Where(o => o.FechaDesde >= fechaInicio && o.FechaDesde <= fechaFin)
              .Where(o => o.FechaHasta < fechaInicio);
            }
            var listpendientes = queryIndirectosPendientes.ToList();
            var indirectosSinCertificarPendientes = Mapper.Map<List<DetalleIndirectosIngenieriaDto>>(listpendientes);

            foreach (var indirecto in indirectosSinCertificarPendientes)
            {
                /* Indirectos Mes*
                 */
                var mesString = this.ObtenerNombreMesIndirectos(indirecto.FechaDesde);
                indirecto.mes = mesString;

                /*Valor Colaborador Rubro Tarifa y monto*/
                var fechaTrabajoDate = indirecto.FechaDesde.Date;
                var colaboradorRubro = _colaboradoRubroRepository.GetAll()
                                                               .Where(c => c.ColaboradorId == indirecto.ColaboradorId)
                                                                  .Where(c => fechaTrabajoDate >= c.FechaInicio)
                                                                   .Where(c => fechaTrabajoDate <= c.FechaFin)
                                                               .Where(c => c.ContratoId == indirecto.contratoId)
                                                               .FirstOrDefault();
                if (colaboradorRubro != null)
                {

                    indirecto.tarifa = colaboradorRubro.Tarifa;
                    indirecto.monto = (indirecto.HorasLaboradas * indirecto.DiasLaborados) * colaboradorRubro.Tarifa;
                }


                /*Proyectos
                string nodistribuir = "";
                if (indirecto.distribucion_proyectos != null && indirecto.distribucion_proyectos.Length > 0)
                {
                    var listproyectos = indirecto.distribucion_proyectos.Split(',');
                    if (listproyectos.Length > 0)
                    {
                        foreach (var p in listproyectos)
                        {
                            if (p.Length > 0)
                            {
                                var id = Int32.Parse(p);
                                var pro = _proyectoRepository.GetAll().Where(c => c.Id == id).FirstOrDefault();
                                if (pro != null)
                                {
                                    nodistribuir = nodistribuir + "," + pro.codigo;
                                }
                            }
                        }
                    }
                }
                indirecto.ProyectosCodigosString = nodistribuir;
                 */
            }







            var proyectos = new List<Proyecto>();
            /*if (queryDirectos.Count > 0)
            {
                var proyectosIds = (from d in queryDirectos select d.ProyectoId).Distinct().ToList();
                if (proyectosIds.Count > 0)
                {
                    proyectos = _proyectoRepository.GetAll().Where(c => c.vigente).Where(c => proyectosIds.Contains(c.Id)).ToList();
                }
            }*/

            var DetallesSinCertificar = new DetallesSinCertificar()
            {
                Directos = directoSinCertificar,
                Indirectos = indirectosSinCertificar,
                ProyectosADistribuir = proyectos,
                PorcentajeMaximoDistribucion = porcentajeMaximoDistribucion != null ? Int32.Parse(porcentajeMaximoDistribucion.Valor) : 0,
                DirectosE500 = directoE500SinCertificar,


                DirectosPendientes = directoSinCertificarPendientes,
                DirectosE500Pendientes = directoE500SinCertificarPendientes,
                IndirectosPendientes = indirectosSinCertificarPendientes

            };
            return DetallesSinCertificar;

        }

        public List<Cliente> GetListCliente()
        {
            var query = _clienteRepository.GetAll().Where(c => c.vigente).ToList();
            return query;
        }

        public List<Proyecto> GetProyectosADistribuir(DateTime? fechaInicio, DateTime? fechaFin)
        {
            var proyectos = new List<Proyecto>();
            var FechaInicialDate = fechaInicio.Value.Date;
            var FechaFinalDate = fechaFin.Value.Date;
            var queryDirectos = _directosRepository.GetAllIncluding(c => c.TipoRegistro, c => c.Colaborador, c => c.Proyecto, c => c.Locacion, c => c.Modalidad, c => c.EstadoRegistro)
                                                   .Where(c => c.FechaTrabajo >= FechaInicialDate)
                                                   .Where(c => c.FechaTrabajo <= FechaFinalDate)
                                                   .Where(c => !c.CertificadoId.HasValue)//No este Certificado;
                                                    .ToList();
            if (queryDirectos.Count > 0)
            {

                var proyectosIds = (from d in queryDirectos select d.ProyectoId).Distinct().ToList();

                if (proyectosIds.Count > 0)
                {
                    proyectos = _proyectoRepository.GetAll().Where(c => c.vigente).Where(c => proyectosIds.Contains(c.Id)).ToList();

                }

            }

            return proyectos;

        }

        public string guardarProyectoNodistribucion(int id, string proyectos)
        {
            var indirecto = _indirectosRepository.Get(id);
            indirecto.distribucion_proyectos = proyectos;
            _indirectosRepository.Update(indirecto);
            return "OK";
        }

        public bool AprobarCertificado(int Id)
        {
            try
            {
                var entity = _certificadoIngenieriaProyectoRepository.Get(Id);

                var secuencial = _secuencialCertificadoRepository.GetAll().Where(c => c.ProyectoId == entity.ProyectoId).OrderByDescending(c => c.secuencia).FirstOrDefault();
                if (secuencial != null && entity.NumeroCertificado > secuencial.secuencia)
                {
                    secuencial.secuencia = entity.NumeroCertificado;
                    _secuencialCertificadoRepository.Update(secuencial);
                }
                if (secuencial == null)
                {
                    var nuevaSecuenciaProyecto = new Secuencial()
                    {
                        nombre = ".",
                        ProyectoId = entity.ProyectoId,
                        secuencia = entity.NumeroCertificado,
                    };
                    int IdSecuencia = _secuencialCertificadoRepository.InsertAndGetId(nuevaSecuenciaProyecto);

                }

                entity.EstadoId = EstadoCertificadoProyecto.Aprobado;
                _certificadoIngenieriaProyectoRepository.Update(entity);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }

        }

        public ExcelPackage CertificadoPorProyecto(int ProyectoId, ExcelPackage excel)
        {


            throw new NotImplementedException();
        }

        public ExcelPackage Resumen(ValidacionColaboradorRubro validacion)
        {


            ExcelPackage excel = new ExcelPackage();

            var hojaParametrizacion = excel.Workbook.Worksheets.Add("NO PARAMETRIZADO EN COLABORADORES INGENIERIA");
            var hojaRubro = excel.Workbook.Worksheets.Add("NO REGISTRADO EN PRECIARIO COLABORADOR");
            int filaInicial = 1;
            string cell = "";
            cell = "A" + filaInicial;
            hojaParametrizacion.Cells[cell].Value = "IDENTIFICACION";
            hojaRubro.Cells[cell].Value = "IDENTIFICACION";
            cell = "B" + filaInicial;
            hojaParametrizacion.Cells[cell].Value = "NOMBRES COMPLETOS";
            hojaRubro.Cells[cell].Value = "NOMBRES COMPLETOS";
            cell = "C" + filaInicial;
            hojaParametrizacion.Cells[cell].Value = "FECHA TRABAJO";
            hojaRubro.Cells[cell].Value = "FECHA TRABAJO";
            cell = "D" + filaInicial;
            hojaParametrizacion.Cells[cell].Value = "TIPO";
            hojaRubro.Cells[cell].Value = "TIPO";

            filaInicial = 2;

            foreach (var item in validacion.Parametrizacion.ToList().Distinct().ToList())
            {
                cell = "A" + filaInicial;
                hojaParametrizacion.Cells[cell].Value = item.identificacion;
                cell = "B" + filaInicial;
                hojaParametrizacion.Cells[cell].Value = item.nombreCompleto;
                cell = "C" + filaInicial;
                hojaParametrizacion.Cells[cell].Value = item.fecha;
                cell = "D" + filaInicial;
                hojaParametrizacion.Cells[cell].Value = item.tipo;
                filaInicial++;
            }

            filaInicial = 2;
            foreach (var item in validacion.Rubros.ToList().Distinct().ToList())
            {
                cell = "A" + filaInicial;
                hojaRubro.Cells[cell].Value = item.identificacion;
                cell = "B" + filaInicial;
                hojaRubro.Cells[cell].Value = item.nombreCompleto;
                cell = "C" + filaInicial;
                hojaRubro.Cells[cell].Value = item.fecha;
                cell = "D" + filaInicial;
                hojaRubro.Cells[cell].Value = item.tipo;
                filaInicial++;
            }


            return excel;
        }

        public RedistribucionTotales TotalesRedistribucion(int Id)
        {
            decimal HorasPresupuestadas = 0;
            var presupuesto = _presupuestoRepository.GetAll().Where(p => p.vigente)
                                                            .Where(p => p.ProyectoId == Id)
                                                            .Where(p => p.es_final)
                                                            .OrderByDescending(p => p.Id)
                                                            .FirstOrDefault();
            if (presupuesto != null)
            {
                var computosIngenieria = _computoPresupuesto.GetAll().Where(c => c.vigente)
                                                                   .Where(c => c.WbsPresupuesto.vigente)
                                                                   .Where(c => c.WbsPresupuesto.PresupuestoId == presupuesto.Id)
                                                                   .Where(c => c.Item.Grupo.codigo == ProyectoCodigos.CODE_INGENIERIA)
                                                                   .ToList();
                if (computosIngenieria.Count > 0)
                {

                    HorasPresupuestadas = computosIngenieria.Select(c => c.cantidad).Sum(); //HH Presupuestadas , Suma de Cantidades
                }

            }

            decimal HorasCertificadas = 0;

            var query = _certificadoIngenieriaProyectoRepository.GetAll().Where(c => c.ProyectoId == Id)
                                                                       .ToList();

            if (query.Count > 0)
            {
                HorasCertificadas = query.Select(c => c.HorasActualCertificadas).Sum();
            }
            RedistribucionTotales result = new RedistribucionTotales()
            {
                totalHorasProyecto = HorasPresupuestadas,
                totalTotalCertificado = HorasCertificadas
            };
            return result;

        }

        public List<TotalMensualCertificado> TotalMensualCertificadodelGrupo(List<CertificadoIngenieriaProyecto> certificados)
        {
            throw new NotImplementedException();
        }

        public bool GenerarCertificadosMasivos(DateTime? fechaInicio, DateTime? fechaFin, int ClienteId)

        {
            var MesInicial = fechaInicio.Value.Month;
            var AnioInicial = fechaInicio.Value.Year;

            var mesFinal = fechaFin.Value.Month;
            var AnioFinal = fechaFin.Value.Year;

            for (int anio = AnioInicial; anio <= AnioFinal; anio++)
            {

                for (int mes = MesInicial; mes <= mesFinal; mes++)
                {

                    var fechaInicial = new DateTime(anio, mes, 21);
                    var fechaFinal = new DateTime(anio, (mes + 1), 20);
                    var fechaCertificado = new DateTime(anio, (mes + 1), 20);

                    var GrupoCertificadoIngenieria = new GrupoCertificadoIngenieria()
                    {
                        ClienteId = ClienteId,
                        EstadoId = EstadoGrupoCertificado.Generado,
                        FechaCertificado = fechaCertificado,
                        FechaInicio = fechaInicial,
                        FechaFin = fechaFinal,
                        FechaGeneracion = fechaCertificado,


                    };

                    var a = new int[1];

                    var DtoDirectos = this.DtoDetallesDirectosIncluidoTarifaMasivo(fechaInicial, fechaFinal, ClienteId);
                    // var DetallesSinCertificar = this.GetDetallesSinCertificar(fechaInicio, fechaFin, ClienteId);

                    var crearCertificados = this.Crear(GrupoCertificadoIngenieria
                                                       , DtoDirectos,
                                                       a,
                                                       a);



                }
            }

            return true;


        }

        public List<DetallesDirectosIngenieriaDto> DtoDetallesDirectosIncluidoTarifa(List<int> DirectosId)
        {
            var queryDirectos = _directosRepository.GetAllIncluding(c => c.TipoRegistro, c => c.Colaborador, c => c.Proyecto.Contrato, c => c.Locacion, c => c.Modalidad, c => c.EstadoRegistro)
                                                     .Where(c => DirectosId.Contains(c.Id))
                                                    .ToList();
            var precisionDecimal = 2;
            var dataDirectos = (from q in queryDirectos
                                select new DetallesDirectosIngenieriaDto()
                                {
                                    Id = q.Id,
                                    CertificadoId = q.CertificadoId,
                                    CodigoProyecto = q.CodigoProyecto,
                                    ColaboradorId = q.ColaboradorId,
                                    nombreColaborador = q.ColaboradorId > 0 ? q.Colaborador.nombres_apellidos : "",
                                    EsDirecto = q.EsDirecto,
                                    EspecialidadId = q.EspecialidadId,
                                    EstadoRegistroId = q.EstadoRegistroId,
                                    EtapaId = q.EtapaId,
                                    FechaTrabajo = q.FechaTrabajo,
                                    formatFechaTrabajo = q.FechaTrabajo.ToShortDateString(),
                                    nombreProyecto = q.ProyectoId > 0 ? q.Proyecto.codigo : "",
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
                                    nombreEstado = q.EstadoRegistroId > 0 ? q.EstadoRegistro.nombre : "",
                                    contratoId = q.Proyecto.contratoId,
                                    total_migracion = q.total_migracion,
                                    tarifa_migracion = q.tarifa_migracion,
                                    migrado = q.migrado,
                                    ColaboradorRubroId = 0,

                                }).ToList();

            //Valores Tarifa y Monto Migrado y Colaborador Rubro

            foreach (var directo in dataDirectos)
            {
                if (!directo.migrado) //Si no esta Migrado
                {
                    var fechaTrabajoDate = directo.FechaTrabajo.Date;
                    var colaboradorRubro = _colaboradoRubroRepository.GetAll()
                                                                   .Where(c => c.ColaboradorId == directo.ColaboradorId)
                                                                   .Where(c => fechaTrabajoDate >= c.FechaInicio)
                                                                   .Where(c => fechaTrabajoDate <= c.FechaFin)
                                                                   .Where(c => c.ContratoId == directo.contratoId)
                                                                   .FirstOrDefault();
                    if (colaboradorRubro != null)
                    {
                        directo.ColaboradorRubroId = colaboradorRubro.Id;
                        directo.tarifa = colaboradorRubro.Tarifa;
                        directo.monto = directo.NumeroHoras * colaboradorRubro.Tarifa;
                    }
                }
                else
                {
                    directo.tarifa = directo.tarifa_migracion;
                    directo.monto = directo.total_migracion;
                }


            }

            return dataDirectos;
        }


        public List<DetallesDirectosIngenieriaDto> DtoDetallesBusquedaRurboYSoloTarifaMigrado(List<int> DirectosId)
        {
            var queryDirectos = _directosRepository.GetAllIncluding(c => c.Proyecto, c => c.Especialidad, c => c.Etapa)
                                                     .Where(c => DirectosId.Contains(c.Id))
                                                    .ToList();


            var precisionDecimal = 2;
            var dataDirectos = (from q in queryDirectos
                                select new DetallesDirectosIngenieriaDto()
                                {
                                    Id = q.Id,
                                    FechaTrabajo = q.FechaTrabajo,
                                    CertificadoId = q.CertificadoId,
                                    ColaboradorId = q.ColaboradorId,
                                    NumeroHoras = q.NumeroHoras,
                                    ProyectoId = q.ProyectoId,
                                    total_migracion = q.total_migracion,
                                    tarifa_migracion = q.tarifa_migracion,
                                    migrado = q.migrado,
                                    contratoId = q.Proyecto.contratoId,
                                    EspecialidadId = q.EspecialidadId,
                                    Especialidad = q.Especialidad,
                                    EtapaId = q.EtapaId,
                                    Etapa = q.Etapa,
                                    LocacionId = q.LocacionId,


                                }).ToList();

            //Valores Tarifa y Monto Migrado y Colaborador Rubro

            foreach (var directo in dataDirectos)
            {
                if (!directo.migrado) //Si no esta Migrado
                {
                    var fechaTrabajoDate = directo.FechaTrabajo.Date;
                    var colaboradorRubro = _colaboradoRubroRepository.GetAll()
                                                                   .Where(c => c.ColaboradorId == directo.ColaboradorId)
                                                                   .Where(c => fechaTrabajoDate >= c.FechaInicio)
                                                                   .Where(c => fechaTrabajoDate <= c.FechaFin)
                                                                   .Where(c => c.ContratoId == directo.contratoId)
                                                                   .FirstOrDefault();
                    if (colaboradorRubro != null)
                    {
                        directo.ColaboradorRubroId = colaboradorRubro.Id;
                        directo.tarifa = colaboradorRubro.Tarifa;
                        directo.monto = directo.NumeroHoras * colaboradorRubro.Tarifa;
                    }
                }
                else
                {
                    directo.tarifa = directo.tarifa_migracion;
                    directo.monto = directo.total_migracion;
                }


            }

            return dataDirectos;
        }

        public int NumeroCertificadoActualProyecto(int ProyectoId)
        {

            var numeroCertificadoActual = 0;
            var numeroCertificados = _certificadoIngenieriaProyectoRepository.GetAll()
                                                                          .Where(c => c.ProyectoId == ProyectoId)
                                                                          .Where(c => c.EstadoId == EstadoCertificadoProyecto.Aprobado)
                                                                          .ToList();
            if (numeroCertificados.Count > 0)
            {
                numeroCertificadoActual = (from n in numeroCertificados select n.NumeroCertificado).Max();

                var numeroCertificadoSecuencial = _secuencialCertificadoRepository.GetAll().Where(c => c.ProyectoId == ProyectoId)
                                                                               .OrderByDescending(c => c.secuencia)
                                                                               .FirstOrDefault();

                if (numeroCertificadoSecuencial != null)
                {

                    var entity = _secuencialCertificadoRepository.Get(numeroCertificadoSecuencial.Id);
                    entity.secuencia = numeroCertificadoActual;
                    _secuencialCertificadoRepository.Update(entity);
                }


            }
            else
            {
                var numeroCertificadoSecuencial = _secuencialCertificadoRepository.GetAll().Where(c => c.ProyectoId == ProyectoId)
                                                                                .OrderByDescending(c => c.secuencia)
                                                                                .FirstOrDefault();
                if (numeroCertificadoSecuencial != null)
                {
                    numeroCertificadoActual = numeroCertificadoSecuencial.secuencia;
                }


            }

            return numeroCertificadoActual + 1;
        }


        public int[] DtoDetallesDirectosIncluidoTarifaMasivo(DateTime fechaInicio, DateTime fechaFin, int ClienteId)
        {
            var FechaInicialDate = fechaInicio.Date;
            var FechaFinalDate = fechaFin.Date;
            var queryDirectosAll = _directosRepository.GetAllIncluding(c => c.TipoRegistro, c => c.Colaborador, c => c.Proyecto.Contrato, c => c.Locacion, c => c.Modalidad, c => c.EstadoRegistro)
                                                       .Where(c => c.FechaTrabajo >= FechaInicialDate)//comentar


                                                    .Where(c => c.FechaTrabajo <= FechaFinalDate)
                                                   .Where(c => c.Proyecto.Contrato.ClienteId == ClienteId)
                                                   .Where(c => !c.CertificadoId.HasValue)//No este Certificado;
                                                    .ToList();
            if (queryDirectosAll.Count > 0)
            {
                var DirectosId = (from d in queryDirectosAll select d.Id).ToArray();
                return DirectosId;
            }

            return new int[0];




        }


        public List<Item> RubrosDirectosCertificadosPorProyecto(int ProyectoId,DateTime fechaCertificado)
        {
            var categoriasCertificadasDirectos = _gastoDirectoRepository.GetAll()
                                                                  .Where(c => c.CertificadoIngenieriaProyecto.ProyectoId == ProyectoId)
                                                                  .Where(c => c.TipoGastoId == TipoGasto.Directo) //Solo Directos
                                                              //    .Where(c => c.CertificadoIngenieriaProyecto.GrupoCertificadoIngenieria.FechaGeneracion <= fechaCertificado)
                                                                  .Where(c => c.RubroId.HasValue)
                                                                  .OrderBy(c => c.Rubro.Item.codigo)
                                                                  .Select(c => c.Rubro.Item.Id).ToList().Distinct().ToList();// Unicos sin Repetir

            var rubrosDirectos = _itemRepository.GetAll().Where(c => categoriasCertificadasDirectos.Contains(c.Id)).ToList(); //Item Dependiendo de la Categoria
            return rubrosDirectos;

        }

        public List<Item> RubrosIndirectosCertificadosPorProyectoUIO(int ProyectoId, DateTime fechaCertificado)
        {
            var categoriasCertificadasInDirectos = _gastoDirectoRepository.GetAll()
                                                                  .Where(c => c.CertificadoIngenieriaProyecto.ProyectoId == ProyectoId)
                                                                  .Where(c => c.TipoGastoId == TipoGasto.Indirecto) //Solo Indirectos
                                                                //  .Where(c => c.CertificadoIngenieriaProyecto.GrupoCertificadoIngenieria.FechaGeneracion <= fechaCertificado)
                                                                  .Where(c => c.UbicacionTrabajo == CertificacionIngenieriaCodigos.UBICACION_UIO)
                                                                  .OrderBy(c => c.Rubro.Item.codigo)
                                                                  .Select(c => c.Rubro.Item.Id).ToList().Distinct().ToList();// Unicos sin Repetir

            var rubrosInDirectos = _itemRepository.GetAll().Where(c => categoriasCertificadasInDirectos.Contains(c.Id)).ToList(); //Item Dependiendo de la Categoria
            return rubrosInDirectos;
        }
        public List<Item> RubrosIndirectosCertificadosPorProyectoOIT(int ProyectoId, DateTime fechaCertificado)
        {
            var categoriasCertificadasInDirectos = _gastoDirectoRepository.GetAll()
                                                                  .Where(c => c.CertificadoIngenieriaProyecto.ProyectoId == ProyectoId)
                                                                  .Where(c => c.TipoGastoId == TipoGasto.Indirecto)
                                                                //  .Where(c=>c.CertificadoIngenieriaProyecto.GrupoCertificadoIngenieria.FechaGeneracion<=fechaCertificado)
                                                                  .Where(c => c.UbicacionTrabajo == CertificacionIngenieriaCodigos.UBICACION_CAMPO || c.UbicacionTrabajo == "")
                                                                  .OrderBy(c => c.Rubro.Item.codigo)
                                                                  .Select(c => c.Rubro.Item.Id).ToList().Distinct().ToList();// Unicos sin Repetir

            var rubrosInDirectos = _itemRepository.GetAll().Where(c => categoriasCertificadasInDirectos.Contains(c.Id)).ToList(); //Item Dependiendo de la Categoria
            return rubrosInDirectos;
        }

        public int ConvertirCodigoProyectoANumero(string codigo)
        {
            var valor = 0;
            try
            {
                string codigoString = codigo.ToUpper().Replace("FC", "").Replace(".", "");
                valor = Convert.ToInt32("0" + codigoString);
                return valor;
            }
            catch (Exception)
            {

                return valor;
            }

        }


        public ExcelPackage GrupoCertificadosCompleto(int GrupoCertificadoId)
        {
            /*DATOS GRUPOCERTIFICADO*/
            var grupoCertificado = Repository.GetAll().Where(c => c.Id == GrupoCertificadoId).FirstOrDefault();
            var fechaCertificadoGrupo = grupoCertificado.FechaCertificado.Date;

            var certificadosPorGrupo = _certificadoIngenieriaProyectoRepository.GetAllIncluding(c => c.Proyecto, c => c.GrupoCertificadoIngenieria)
                                                                              .Where(c => c.GrupoCertificadoIngenieriaId == GrupoCertificadoId)
                                                                              .OrderBy(c => c.Proyecto.orden_timesheet)
                                                                              .ToList();
            /*certificadosPorGrupo = (from c in certificadosPorGrupo
                                    orderby this.ConvertirCodigoProyectoANumero(c.Proyecto.codigo) ascending
                                    select c
                                  ).ToList();*/

            ExcelPackage excel = new ExcelPackage();
            //  string filename = System.Web.HttpContext.Current.Server.MapPath("~/Views/PlantillaWord/CertificacionIngenieria/FormatoCertificadoProyecto.xlsx");
            string filenameResumen = System.Web.HttpContext.Current.Server.MapPath("~/Views/PlantillaWord/CertificacionIngenieria/ResumenProyecto2.xlsx");
            string filename = System.Web.HttpContext.Current.Server.MapPath("~/Views/PlantillaWord/CertificacionIngenieria/FormatoCertificadoProyectoCompleto.xlsx");

            if (File.Exists((string)filename))
            {
                FileInfo docplantilla = new FileInfo(filename);
                ExcelPackage plantilla = new ExcelPackage(docplantilla);

                #region  Excel Resumen
                if (File.Exists((string)filenameResumen))
                {
                    FileInfo newFileResumen = new FileInfo(filenameResumen);
                    ExcelPackage pckResumen = new ExcelPackage(newFileResumen);
                    ExcelWorksheet h1 = excel.Workbook.Worksheets.Add("RESUMEN", pckResumen.Workbook.Worksheets[1]);

                    var ProyectosQuery = _certificadoIngenieriaProyectoRepository.GetAllIncluding(c => c.Proyecto)
                                                                           .OrderBy(c => c.Proyecto.orden_timesheet)
                                                                           .ToList();
                    //#Hoy





                    var ProyectosId = ProyectosQuery.Select(c => c.ProyectoId).ToList().Distinct().ToList();




                    var ListProyectoResumen = new List<ResumenCertificacion>();

                    foreach (var Id in ProyectosId)
                    {
                        var entidadProyecto = _proyectoRepository.GetAll().Where(c => c.Id == Id).Where(c => c.vigente).FirstOrDefault();
                        var existResumenProyecto = _resumenCertificacion.GetAllIncluding(p => p.Proyecto)
                                                                   .Where(c => c.ProyectoId == Id)
                                                                   .Where(c => c.GrupoCertificadoIngenieriaId == GrupoCertificadoId)
                                                                   .FirstOrDefault();
                        if (existResumenProyecto != null)
                        {
                            existResumenProyecto.anioProyecto = entidadProyecto.anio_certificacion_ingenieria;
                            ListProyectoResumen.Add(existResumenProyecto);
                        }
                        else
                        {
                            var p = new ResumenCertificacion()
                            {
                                Id = 0,
                                ProyectoId = entidadProyecto.Id,
                                Descripcion = entidadProyecto.descripcion_proyecto,

                            };
                            p.anioProyecto = entidadProyecto.anio_certificacion_ingenieria;
                            var avanceRealIngenieriaFechaCertificado = _avanceProyectoRepository.GetAll()
                                                                .Where(c => c.ProyectoId == Id)
                                                                .Where(c => c.FechaCertificado <= grupoCertificado.FechaFin)
                                                                .OrderByDescending(c => c.FechaCertificado)
                                                                .FirstOrDefault();

                            double PorcentajeIB = 0.30; //Porcentaje Ingenieria Basica
                            double PorcentajeID = 0.70;//Porcentaje Ingenieria Detalle


                            /*Porcentaje_asbuilt	Decimal	Si	Tomado de valor carga complementaria ingeniería*/
                            decimal PorcentajeAsbuilt = 0; //Pendiente


                            decimal avanceRealIngenieria = Decimal.Parse("0");
                            if (avanceRealIngenieriaFechaCertificado != null)
                            {
                                p.IB_PREVISTO = avanceRealIngenieriaFechaCertificado.AvancePrevistoActualIB;
                                p.ID_PREVISTO = avanceRealIngenieriaFechaCertificado.AvancePrevistoActualID;
                                p.IB_REAL = avanceRealIngenieriaFechaCertificado.AvanceRealActualIB * Convert.ToDecimal(PorcentajeIB);
                                p.ID_REAL = avanceRealIngenieriaFechaCertificado.AvanceRealActualID * Convert.ToDecimal(PorcentajeID);


                               
                            p.TOTAL_REAL = ( p.IB_REAL + p.ID_REAL) * Convert.ToDecimal(0.95)+avanceRealIngenieriaFechaCertificado.AsbuiltActual *Convert.ToDecimal(0.05);

                                //  p.TOTAL_REAL = avanceRealIngenieria = p.IB_REAL + p.ID_REAL;
                               // p.PORCENTAJE_AVANCE_FÍSICO_REAL_IB_ID_AB = avanceRealIngenieriaFechaCertificado.AsbuiltActual;
                            }

                            ListProyectoResumen.Add(p);

                        }


                    }






                    var count = 8;
                    string cellr = "B" + count;

                    int CountNumeroProyectos = ProyectosId.Count;
                    int CountInicialCeldasBlancas = (9 + CountNumeroProyectos);
                    int MaximoFilaResumen = 80;

                    var ListProyectoResumenAgrupados = (from t in ListProyectoResumen

                                                        group t by new
                                                        {
                                                            t.anioProyecto
                                                        }
                                                     into g
                                                        select new
                                                        {
                                                            Grupo = g.Key,
                                                            ListProyectoResumen = g.ToList()
                                                        }).ToList();

                    foreach (var grupo in ListProyectoResumenAgrupados)
                    {
                        cellr = "B" + count;

                        string fraseGrupoProyecto = "";
                        int anioProyecto = grupo.Grupo.anioProyecto;
                        if (anioProyecto <= 2018)
                        {
                            fraseGrupoProyecto = "PORTAFOLIO 2016 - 2018";

                        }
                        else
                        {
                            fraseGrupoProyecto = "PORTAFOLIO " + anioProyecto;
                        }
                        h1.Cells[cellr].Value = fraseGrupoProyecto;
                        h1.Cells[cellr].Style.Font.Color.SetColor(Color.FromArgb(166, 166, 166));
                        h1.Cells[cellr].Style.Font.Size = 16;
                        count++;

                        foreach (var r in grupo.ListProyectoResumen)
                        {
                            var entidadProyecto = _proyectoRepository.GetAll().Where(c => c.Id == r.ProyectoId).Where(c => c.vigente).FirstOrDefault();

                            cellr = "A" + count;
                            h1.Cells[cellr].Value = entidadProyecto.codigo.ToUpper();

                            if (entidadProyecto.ProyectoCerrado)
                            {
                                h1.Cells[cellr].Style.Font.Color.SetColor(Color.FromArgb(0, 112, 192));
                            }

                            cellr = "B" + count;
                            h1.Cells[cellr].Value = entidadProyecto.nombre_proyecto.Length > 0 ? entidadProyecto.nombre_proyecto.ToUpper() : "";

                            cellr = "C" + count;

                            var stringUltimaPo = _detallePORepository.GetAll().Where(x => x.ProyectoId == entidadProyecto.Id)
                                                                       .Where(x => x.vigente)
                                                                       .OrderByDescending(x => x.OrdenServicio.fecha_orden_servicio)
                                                                       .Select(c => c.OrdenServicio.codigo_orden_servicio)
                                                                       .FirstOrDefault();
                            h1.Cells[cellr].Value = (stringUltimaPo == null || stringUltimaPo == "") ? "TBD" : stringUltimaPo;


                            var mes = 1;
                            for (int i = 4; i <= 27; i = i + 2)
                            {

                                var anio = 2017;
                                var query = _certificadoIngenieriaProyectoRepository.GetAllIncluding(c => c.GrupoCertificadoIngenieria)
                                                                                   .Where(c => c.GrupoCertificadoIngenieria.FechaCertificado.Month == mes)
                                                                                    .Where(c => c.GrupoCertificadoIngenieria.FechaCertificado.Year == anio)
                                                                                    .Where(c => c.GrupoCertificadoIngenieria.FechaCertificado <= grupoCertificado.FechaCertificado)
                                                                                   .Where(c => c.ProyectoId == r.ProyectoId)

                                                                                   .ToList();
                                var hhmes = query.Count > 0 ? query.Sum(c => c.HorasActualCertificadas) : 0;
                                var usdmes = query.Count > 0 ? query.Sum(c => c.MontoActualCertificado) : 0;

                                h1.Cells[count, i].Value = hhmes;
                                h1.Cells[count, (i + 1)].Value = usdmes;

                                mes++;
                            }
                            mes = 1;
                            for (int i = 30; i <= 53; i = i + 2)
                            {

                                var anio = 2018;
                                var query = _certificadoIngenieriaProyectoRepository.GetAllIncluding(c => c.GrupoCertificadoIngenieria)
                                                                                   .Where(c => c.GrupoCertificadoIngenieria.FechaCertificado.Month == mes)
                                                                                    .Where(c => c.GrupoCertificadoIngenieria.FechaCertificado.Year == anio)
                                                                                       .Where(c => c.GrupoCertificadoIngenieria.FechaCertificado <= grupoCertificado.FechaCertificado)
                                                                                   .Where(c => c.ProyectoId == r.ProyectoId)

                                                                                   .ToList();
                                var hhmes = query.Count > 0 ? query.Sum(c => c.HorasActualCertificadas) : 0;
                                var usdmes = query.Count > 0 ? query.Sum(c => c.MontoActualCertificado) : 0;

                                h1.Cells[count, i].Value = hhmes;
                                h1.Cells[count, (i + 1)].Value = usdmes;

                                mes++;
                            }
                            mes = 1;
                            for (int i = 56; i <= 79; i = i + 2)
                            {

                                var anio = 2019;
                                var query = _certificadoIngenieriaProyectoRepository.GetAllIncluding(c => c.GrupoCertificadoIngenieria)
                                                                                   .Where(c => c.GrupoCertificadoIngenieria.FechaCertificado.Month == mes)
                                                                                    .Where(c => c.GrupoCertificadoIngenieria.FechaCertificado.Year == anio)
                                                                                       .Where(c => c.GrupoCertificadoIngenieria.FechaCertificado <= grupoCertificado.FechaCertificado)
                                                                                   .Where(c => c.ProyectoId == r.ProyectoId)

                                                                                   .ToList();
                                var hhmes = query.Count > 0 ? query.Sum(c => c.HorasActualCertificadas) : 0;
                                var usdmes = query.Count > 0 ? query.Sum(c => c.MontoActualCertificado) : 0;

                                h1.Cells[count, i].Value = hhmes;
                                h1.Cells[count, (i + 1)].Value = usdmes;

                                mes++;
                            }
                            mes = 1;
                            for (int i = 82; i <= 115; i = i + 3)
                            {

                                var anio = 2020;
                                var query = _certificadoIngenieriaProyectoRepository.GetAllIncluding(c => c.GrupoCertificadoIngenieria)
                                                                                   .Where(c => c.GrupoCertificadoIngenieria.FechaCertificado.Month == mes)
                                                                                    .Where(c => c.GrupoCertificadoIngenieria.FechaCertificado.Year == anio)
                                                                                       .Where(c => c.GrupoCertificadoIngenieria.FechaCertificado <= grupoCertificado.FechaCertificado)
                                                                                   .Where(c => c.ProyectoId == r.ProyectoId)

                                                                                   .ToList();
                                var hhmes = query.Count > 0 ? query.Sum(c => c.HorasActualCertificadas) : 0;
                                var usdmes = query.Count > 0 ? query.Sum(c => c.MontoActualCertificado) : 0;

                                h1.Cells[count, i].Value = hhmes;
                                h1.Cells[count, (i + 1)].Value = usdmes;


                                string celdaMonto = h1.Cells[count, (i + 1)].Address;
                                string celdaHoras = h1.Cells[count, i].Address;
                                var formula = "=IF($" + celdaHoras + " = 0,0,$" + celdaMonto + "/$" + celdaHoras + ")";

                                // h1.Cells[count, (i + 2)].Value = usdmes;
                                h1.Cells[count, (i + 2)].Formula = formula;
                                h1.Cells[count, (i + 2)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                h1.Cells[count, (i + 2)].Style.Numberformat.Format = "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";

                                mes++;
                            }
                            mes = 1;
                            for (int i = 120; i <= 153; i = i + 3)
                            {

                                var anio = 2021;
                                var query = _certificadoIngenieriaProyectoRepository.GetAllIncluding(c => c.GrupoCertificadoIngenieria)
                                                                                   .Where(c => c.GrupoCertificadoIngenieria.FechaCertificado.Month == mes)
                                                                                    .Where(c => c.GrupoCertificadoIngenieria.FechaCertificado.Year == anio)
                                                                                       .Where(c => c.GrupoCertificadoIngenieria.FechaCertificado <= grupoCertificado.FechaCertificado)
                                                                                   .Where(c => c.ProyectoId == r.ProyectoId)

                                                                                   .ToList();
                                var hhmes = query.Count > 0 ? query.Sum(c => c.HorasActualCertificadas) : 0;
                                var usdmes = query.Count > 0 ? query.Sum(c => c.MontoActualCertificado) : 0;

                                h1.Cells[count, i].Value = hhmes;
                                h1.Cells[count, (i + 1)].Value = usdmes;


                                string celdaMonto = h1.Cells[count, (i + 1)].Address;
                                string celdaHoras = h1.Cells[count, i].Address;
                                var formula = "=IF($" + celdaHoras + " = 0,0,$" + celdaMonto + "/$" + celdaHoras + ")";

                                // h1.Cells[count, (i + 2)].Value = usdmes;
                                h1.Cells[count, (i + 2)].Formula = formula;
                                h1.Cells[count, (i + 2)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                h1.Cells[count, (i + 2)].Style.Numberformat.Format = "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";

                                mes++;
                            }

                            mes = 1;
                            for (int i = 159; i <= 192; i = i + 3)
                            {

                                var anio = 2022;
                                var query = _certificadoIngenieriaProyectoRepository.GetAllIncluding(c => c.GrupoCertificadoIngenieria)
                                                                                     //   .Where(c => c.GrupoCertificadoIngenieria.FechaCertificado.Month == mes)
                                                                                     //    .Where(c => c.GrupoCertificadoIngenieria.FechaCertificado.Year == anio)
                                                                                     .Where(c => c.GrupoCertificadoIngenieria.Mes == mes)
                                                                                       .Where(c => c.GrupoCertificadoIngenieria.FechaCertificado.Year == anio)
                                                                                   .Where(c => c.ProyectoId == r.ProyectoId)
                                                                                      .Where(c => c.GrupoCertificadoIngenieria.FechaCertificado <= grupoCertificado.FechaCertificado)
                                                                                   .ToList();
                                var hhmes = query.Count > 0 ? query.Sum(c => c.HorasActualCertificadas) : 0;
                                var usdmes = query.Count > 0 ? query.Sum(c => c.MontoActualCertificado) : 0;

                                h1.Cells[count, i].Value = hhmes;
                                h1.Cells[count, (i + 1)].Value = usdmes;


                                string celdaMonto = h1.Cells[count, (i + 1)].Address;
                                string celdaHoras = h1.Cells[count, i].Address;
                                var formula = "=IF($" + celdaHoras + " = 0,0,$" + celdaMonto + "/$" + celdaHoras + ")";

                                // h1.Cells[count, (i + 2)].Value = usdmes;
                                h1.Cells[count, (i + 2)].Formula = formula;
                                h1.Cells[count, (i + 2)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                h1.Cells[count, (i + 2)].Style.Numberformat.Format = "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";

                                mes++;
                            }
                            cellr = "FD" + count;
                            //   h1.Cells[cellr].Value = r.TotalEjecutadoHH;

                            cellr = "FE" + count;
                            //   h1.Cells[cellr].Value = r.TotalEjecutadoUSD;

                            cellr = "GT" + count;
                            h1.Cells[cellr].Value = r.CLASE;

                            cellr = "GU" + count;
                            h1.Cells[cellr].Value = r.USD_BDG;

                            cellr = "GV" + count;
                            h1.Cells[cellr].Value = r.HH_BDG;

                            cellr = "GW" + count;
                            h1.Cells[cellr].Value = r.N_Oferta;

                            cellr = "GX" + count;
                            h1.Cells[cellr].Value = r.USD_AB;

                            cellr = "GY" + count;
                            h1.Cells[cellr].Value = r.HH_AB;


                            cellr = "GZ" + count;
                            h1.Cells[cellr].Value = r.EAC_USD;

                            cellr = "HA" + count;
                            h1.Cells[cellr].Value = r.EAC_HH;

                            cellr = "HB" + count;
                            h1.Cells[cellr].Value = r.TOTAL_PREVISTO;

                            cellr = "HC" + count;
                            h1.Cells[cellr].Value = r.IB_PREVISTO;
                            cellr = "HD" + count;
                            h1.Cells[cellr].Value = r.ID_PREVISTO;

                            cellr = "HE" + count;

                            if (r.unaFase)
                            {
                                if (r.IB_REAL > 0) {
                                    h1.Cells[cellr].Value = r.IB_REAL;
                                }
                                if (r.ID_REAL > 0) {
                                    h1.Cells[cellr].Value = r.ID_REAL;
                                }
                            }
                            else {
                                h1.Cells[cellr].Value = r.TOTAL_REAL;
                            }
                           



                            cellr = "HF" + count;
                            h1.Cells[cellr].Value = r.IB_REAL;


                            cellr = "HG" + count;
                            h1.Cells[cellr].Value = r.ID_REAL;

                            cellr = "HH" + count;
                            h1.Cells[cellr].Value = r.AB_REAL;

                            cellr = "HI" + count;
                            h1.Cells[cellr].Value = r.PORCENTAJE_AVANCE_FÍSICO_PREVISTO_IB_ID_AB;

                            cellr = "HJ" + count;
                            h1.Cells[cellr].Value = r.PORCENTAJE_AVANCE_FÍSICO_REAL_IB_ID_AB;

                            cellr = "HR" + count;
                            h1.Cells[cellr].Value = r.Comentarios;

                            count++;
                            if (entidadProyecto.codigo.ToUpper() == "FC2.40")
                            {
                                count++;
                            }
                            if (entidadProyecto.codigo.ToUpper() == "FC8.15")
                            {
                                count++;

                            }
                            if (entidadProyecto.codigo.ToUpper() == "FC8.15")
                            {
                                count++;

                            }
                        }




                    }


                    /*
                    int f = CountInicialCeldasBlancas;
                    while (f <= MaximoFilaResumen)
                    {
                        h1.DeleteRow(f);
                        f++;

                    }
                    */


                }
                #endregion 


                //Crear Pestaña por Proyecto
                foreach (var c in certificadosPorGrupo)
                {

                    var proyectoEntitdad = _proyectoRepository.GetAllIncluding(y => y.Portafolio, y => y.Ubicacion).Where(y => y.Id == c.ProyectoId).FirstOrDefault();
                    var fechaCertificacion = c.GrupoCertificadoIngenieria.FechaCertificado;
                    var mesCertificacion = fechaCertificacion.ToString("MMM", CultureInfo.CreateSpecificCulture("es-Es")).ToLower().Replace(".", "") + "" + fechaCertificacion.ToString("yy");
                    var nomnbrePestaña = (c.Proyecto.codigo.Length > 0 ? c.Proyecto.codigo.ToUpper().Replace("FC", "").Replace(".", "") : "") + "" + mesCertificacion;
                    var estadoAprobado = _catalogoRepository.GetAll().Where(o => o.codigo == CatalogosCodigos.OFERTA_ESTADO_APROBADO).FirstOrDefault();




                    var detallesAnteriores = new List<GastoDirectoCertificado>();
                    #region DetallesAnterioresCertificados
                    var certificadoAnteriorProyectoListado = _certificadoIngenieriaProyectoRepository.GetAll()
                                                                                            .Where(x => x.ProyectoId == c.ProyectoId)
                                                                                            .Where(x => x.GrupoCertificadoIngenieria.FechaCertificado < c.GrupoCertificadoIngenieria.FechaCertificado)
                                                                                            .OrderByDescending(x => x.GrupoCertificadoIngenieria.FechaCertificado)
                                                                                            .ToList();
                    if (certificadoAnteriorProyectoListado.Count > 0)
                    {
                        foreach (var certificadoAnteriorProyecto in certificadoAnteriorProyectoListado)
                        {
                            var detallesAnteriorCertificado = _gastoDirectoRepository.GetAllIncluding(x => x.Colaborador, x => x.Rubro.Item)
                                                                                                    .Where(x => x.CertificadoIngenieriaProyectoId == certificadoAnteriorProyecto.Id)
                                                                                                    .ToList();
                            if (detallesAnteriorCertificado.Count > 0)
                            {
                                detallesAnteriores.AddRange(detallesAnteriorCertificado);
                            }

                        }

                    }

                    #endregion

                    var detalleCertificados = _gastoDirectoRepository.GetAllIncluding(x => x.Colaborador, x => x.Rubro.Item)
                                                                        .Where(x => x.CertificadoIngenieriaProyectoId == c.Id)
                                                                        .ToList();

                    if (detalleCertificados.Count > 0)
                    {
                        ExcelWorksheet hojaPersonalDirecto = excel.Workbook.Worksheets.Add(nomnbrePestaña, plantilla.Workbook.Worksheets[1]);
                        ExcelWorksheet hojaPersonalInDirecto = plantilla.Workbook.Worksheets[2];
                        ExcelWorksheet hojaSubtotales = plantilla.Workbook.Worksheets[3];
                        ExcelWorksheet hojaComentarios = plantilla.Workbook.Worksheets[4];

                                               
                        //ExcelWorksheet h = excel.Workbook.Worksheets.Add(nomnbrePestaña, pck.Workbook.Worksheets[1]);

                        if (proyectoEntitdad != null)
                        {

                            if (proyectoEntitdad.UbicacionId.HasValue && proyectoEntitdad.Ubicacion.codigo == "PROYECTO_CERT_QUITO")
                            {
                                //Color UBICACION UI   celeste
                                hojaPersonalDirecto.TabColor = Color.FromArgb(0, 176, 240);
                            }
                            if (proyectoEntitdad.UbicacionId.HasValue && proyectoEntitdad.Ubicacion.codigo == "PROYECTO_CERT_OT")
                            {
                                //Color UBICACION OT  verde
                                hojaPersonalDirecto.TabColor = Color.FromArgb(146, 208, 80);
                            }
                            if (proyectoEntitdad.PortafolioId.HasValue && proyectoEntitdad.Portafolio.codigo == "PORT_CERT_2019")
                            {
                                //Color Portafolio 2019 narnja
                                hojaPersonalDirecto.TabColor = Color.FromArgb(247, 150, 70);
                            }
                            if (proyectoEntitdad.PortafolioId.HasValue && proyectoEntitdad.Portafolio.codigo == "PORT_CERT_2020")
                            {
                                //Color Portafolio 2020 lila
                                hojaPersonalDirecto.TabColor = Color.FromArgb(177, 160, 199);
                            }

                        }




                        #region Cabecera Certificado
                        string nombreProyecto = c.Proyecto.descripcion_proyecto;

                        string cell = "";
                        int count = 4;

                        cell = "D" + count + ":L" + count;
                        hojaPersonalDirecto.Cells[cell].Merge = true;
                        hojaPersonalDirecto.Cells[cell].Value = c.Proyecto.codigo.ToUpper() + " " + c.Proyecto.descripcion_proyecto.ToUpper();
                        hojaPersonalDirecto.Cells[cell].Style.WrapText = true;
                        hojaPersonalDirecto.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        hojaPersonalDirecto.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;


                     


                        //#fase
                        count = 5;
                        cell = "D" + count + ":L" + count;
                        hojaPersonalDirecto.Cells[cell].Merge = true;

                        var stringUltimaPo = _detallePORepository.GetAll().Where(x => x.ProyectoId == c.ProyectoId)
                                                                      .Where(x => x.vigente)
                                                                      .OrderByDescending(x => x.OrdenServicio.fecha_orden_servicio)
                                                                      .Select(x => x.OrdenServicio.codigo_orden_servicio)
                                                                      .FirstOrDefault();



                        hojaPersonalDirecto.Cells[cell].Value = (stringUltimaPo == null || stringUltimaPo == "") ? "TBD" : stringUltimaPo;
                        hojaPersonalDirecto.Cells[cell].Style.WrapText = true;
                        hojaPersonalDirecto.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        hojaPersonalDirecto.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;



               




                        //#Mes certificacion
                        count = 7;
                        cell = "D" + count + ":L" + count;
                        hojaPersonalDirecto.Cells[cell].Merge = true;
                        hojaPersonalDirecto.Cells[cell].Value = c.Proyecto.codigo + "-" + (c.NumeroCertificado < 9 ? "0" + c.NumeroCertificado.ToString().Replace(" ", "") : c.NumeroCertificado.ToString().Replace(" ", ""));
                        hojaPersonalDirecto.Cells[cell].Style.WrapText = true;
                        hojaPersonalDirecto.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        hojaPersonalDirecto.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;


                     



                        count = 6;

                        cell = "D" + count + ":L" + count;
                        hojaPersonalDirecto.Cells[cell].Merge = true;
                        hojaPersonalDirecto.Cells[cell].Value = mesCertificacion;
                        hojaPersonalDirecto.Cells[cell].Style.WrapText = true;
                        hojaPersonalDirecto.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        hojaPersonalDirecto.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;


                        //#fechaEmision
                        cell = "O" + count + ":R" + count;
                        hojaPersonalDirecto.Cells[cell].Merge = true;
                        hojaPersonalDirecto.Cells[cell].Value = c.GrupoCertificadoIngenieria.FechaGeneracion.ToShortDateString();
                        hojaPersonalDirecto.Cells[cell].Style.WrapText = true;
                        hojaPersonalDirecto.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        hojaPersonalDirecto.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;






                        //#periodo
                        count = 8;
                        cell = "D" + count + ":L" + count;
                        hojaPersonalDirecto.Cells[cell].Merge = true;
                        hojaPersonalDirecto.Cells[cell].Value = c.GrupoCertificadoIngenieria.FechaInicio.ToShortDateString() + " al " + c.GrupoCertificadoIngenieria.FechaFin.ToShortDateString();
                        hojaPersonalDirecto.Cells[cell].Style.WrapText = true;
                        hojaPersonalDirecto.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        hojaPersonalDirecto.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;





                        count = 7;
                        //#FechaCorte
                        cell = "O" + count + ":R" + count;
                        hojaPersonalDirecto.Cells[cell].Merge = true;
                        hojaPersonalDirecto.Cells[cell].Value = c.GrupoCertificadoIngenieria.FechaCertificado.ToShortDateString();
                        hojaPersonalDirecto.Cells[cell].Style.WrapText = true;
                        hojaPersonalDirecto.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        hojaPersonalDirecto.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;






                        //#codigo
                        count = 9;
                        cell = "D" + count + ":L" + count;
                        hojaPersonalDirecto.Cells[cell].Merge = true;
                        hojaPersonalDirecto.Cells[cell].Value = c.Proyecto.codigo.ToUpper();
                        hojaPersonalDirecto.Cells[cell].Style.WrapText = true;
                        hojaPersonalDirecto.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        hojaPersonalDirecto.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;


                        //#porcentaje avane real
                        decimal porcentajeAsbuiltActual = 0;
                        decimal porcentajeIBActual = 0;
                        decimal porcentajeIDActual = 0;

                        decimal porcentajeAsbuiltAnterior = 0;
                        decimal porcentajeIBAnterior = 0;
                        decimal porcentajeIDAnterior = 0;


                        decimal porcentaje_avance_real = 0;

                        double PorcentajeIB = 0.30; //Porcentaje Ingenieria Basica
                        double PorcentajeID = 0.70;//Porcentaje Ingenieria Detalle
                        var avanceRealIngenieriaFechaCertificado = _avanceProyectoRepository.GetAll()
                                                                     .Where(x => x.ProyectoId == c.ProyectoId)
                                                                     .Where(x => x.FechaCertificado <= c.GrupoCertificadoIngenieria.FechaCertificado)
                                                                     .OrderByDescending(x => x.FechaCertificado)
                                                                     .FirstOrDefault();




                        if (avanceRealIngenieriaFechaCertificado != null)
                        {

                            var valorIB = avanceRealIngenieriaFechaCertificado.AvanceRealActualIB * Convert.ToDecimal(PorcentajeIB);
                            var valorID = avanceRealIngenieriaFechaCertificado.AvanceRealActualID * Convert.ToDecimal(PorcentajeID);
                            porcentaje_avance_real = valorIB + valorID;

                            porcentajeAsbuiltActual = avanceRealIngenieriaFechaCertificado.AsbuiltActual;
                            porcentajeIBActual = avanceRealIngenieriaFechaCertificado.AvanceRealActualIB;
                            porcentajeIDActual = avanceRealIngenieriaFechaCertificado.AvanceRealActualID;


                        }
                        var grupoCertificadoAnterior = Repository.GetAll().Where(g => g.FechaCertificado < c.GrupoCertificadoIngenieria.FechaCertificado).OrderByDescending(g => g.FechaCertificado).FirstOrDefault();
                        if (grupoCertificadoAnterior != null)
                        {
                            var avanceRealIngenieriaFechaCertificadoAnterior = _avanceProyectoRepository.GetAll()
                                                                                                .Where(x => x.ProyectoId == c.ProyectoId)
                                                                                                .Where(x => x.FechaCertificado <= grupoCertificadoAnterior.FechaCertificado)
                                                                                                .OrderByDescending(x => x.FechaCertificado)
                                                                                                .FirstOrDefault();
                            if (avanceRealIngenieriaFechaCertificadoAnterior != null)
                            {
                                porcentajeAsbuiltAnterior = avanceRealIngenieriaFechaCertificadoAnterior.AsbuiltActual;
                                porcentajeIBAnterior = avanceRealIngenieriaFechaCertificadoAnterior.AvanceRealActualIB;
                                porcentajeIDAnterior = avanceRealIngenieriaFechaCertificadoAnterior.AvanceRealActualID;
                            }
                        }

                        //Jtrac 172


                        //porcentaje_avance_real = (porcentaje_avance_real *Convert.ToDecimal(0.95)) + porcentajeAsbuiltActual * Convert.ToDecimal(0.05);

                        count = 4;
                        cell = "O" + count + ":R" + count;
                        hojaPersonalDirecto.Cells[cell].Merge = true;
                        hojaPersonalDirecto.Cells[cell].Value = porcentaje_avance_real;
                        hojaPersonalDirecto.Cells[cell].Style.WrapText = true;
                        hojaPersonalDirecto.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        hojaPersonalDirecto.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        //#pres
                        /*  var presupuesto = _presupuestoRepository.GetAll().Where(p => p.vigente)
                                                                         .Where(p => p.ProyectoId == c.ProyectoId)
                                                                         .Where(p => p.es_final)
                                                                         .OrderByDescending(p => p.Id)
                                                                         .FirstOrDefault();*/



                        /*var presupuestos = _presupuestoRepository.GetAll().Where(p => p.vigente)
                                                       .Where(p => p.ProyectoId == c.ProyectoId)
                                                       .Where(p => p.es_final)
                                                       //.Where(p => p.Requerimiento.tipo_requerimiento == TipoRequerimiento.Principal)
                                                       //.OrderByDescending(p => p.Id)
                                                       //.OrderByDescending(p => p.fecha_registro)
                                                       .Select(p => p.Id)
                                                       .ToList();*/

                        var presupuestos = _ofertaRepository.GetAll().Where(p => p.vigente)
                                                     .Where(p => p.OfertaComercial.estado_oferta == estadoAprobado.Id)
                                                     .Where(p => p.OfertaComercial.vigente)
                                                     .Where(p => p.PresupuestoId.HasValue)
                                                     .Where(p => p.Presupuesto.es_final)
                                                     .Where(p => p.Presupuesto.vigente)
                                                     .Where(p => p.Presupuesto.ProyectoId == c.ProyectoId)
                                                     .Select(p => p.PresupuestoId)
                                                    .ToList();




                        count = 5;
                        cell = "O" + count + ":R" + count;
                        hojaPersonalDirecto.Cells[cell].Merge = true;
                        hojaPersonalDirecto.Cells[cell].Value = c.HorasPresupuestadas;
                        hojaPersonalDirecto.Cells[cell].Style.WrapText = true;
                        hojaPersonalDirecto.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        hojaPersonalDirecto.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;



              



                        count = 8;
                        cell = "O" + count + ":R" + count;
                        hojaPersonalDirecto.Cells[cell].Merge = true;
                        hojaPersonalDirecto.Cells[cell].Value = porcentajeAsbuiltActual;
                        hojaPersonalDirecto.Cells[cell].Style.WrapText = true;
                        hojaPersonalDirecto.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        hojaPersonalDirecto.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        /*
                        cell = "I312";
                        h.Cells[cell].Value = porcentajeIBActual;

                        cell = "K312";
                        h.Cells[cell].Value = porcentajeIDActual;*/

                        #endregion


                        // Desgloce Horas Hombre y Personal Directo

                        int PosicionInicialPersonalDirecto = 13; //Inicio Celda
                        int PosicionFinalPersonalDirecto = 47;//Posicion Final Celda
                        int MaximoFilas = 35; //Maximo Registro en Archivo Original
                        var personalesDirecto = (from p in detalleCertificados
                                                 where p.TipoGastoId == TipoGasto.Directo
                                                 select p).ToList();

                        var personalDirectosAgrupados = (from t in personalesDirecto
                                                         where t.RubroId.HasValue
                                                         group t by new
                                                         {
                                                             t.ColaboradorId,
                                                             t.Rubro.ItemId
                                                         }
                                                        into g
                                                         select new
                                                         {
                                                             Grupo = g.Key,
                                                             SumaHoras = g.Sum(x => x.TotalHoras)
                                                         }).ToList();



                        #region  Personal Directo Dinamico con Inseccion de FIlas Nuevas
                        int FilasPersonalDirecto = personalDirectosAgrupados.Count;
                        if (FilasPersonalDirecto > MaximoFilas)
                        {
                            int Filas_A_Aumentar = FilasPersonalDirecto - MaximoFilas;
                            hojaPersonalDirecto.InsertRow(PosicionFinalPersonalDirecto, Filas_A_Aumentar, PosicionFinalPersonalDirecto - 1); //Aumentar Filas de Directos 
                        }

                        int countInicial = PosicionInicialPersonalDirecto;
                        string cellIndice = "C" + countInicial;
                        string cellPersonal = "D" + countInicial;
                        int indice = 1;
                        foreach (var pd in personalDirectosAgrupados)
                        {
                            var Colaborador = _colaboradorRepository.GetAll().Where(x => x.Id == pd.Grupo.ColaboradorId).FirstOrDefault();
                            var Categoria = _itemRepository.GetAll().Where(x => x.Id == pd.Grupo.ItemId).FirstOrDefault();
                            cellIndice = "C" + countInicial;
                            hojaPersonalDirecto.Cells[cellIndice].Value = indice;
                            hojaPersonalDirecto.Cells[cellIndice].Style.Font.Bold = true;

                            cellPersonal = "D" + countInicial;
                            hojaPersonalDirecto.Cells[cellPersonal].Value = Colaborador != null ? Colaborador.nombres_apellidos.ToUpper() : "";
                            hojaPersonalDirecto.Cells[cellPersonal].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                            cellPersonal = "H" + countInicial + ":J" + countInicial;
                            hojaPersonalDirecto.Cells[cellPersonal].Merge = true;
                            hojaPersonalDirecto.Cells[cellPersonal].Style.WrapText = true;
                            hojaPersonalDirecto.Cells[cellPersonal].Value = Categoria != null ? Categoria.nombre.ToUpper() : "";
                            hojaPersonalDirecto.Cells[cellPersonal].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;


                            cellPersonal = "K" + countInicial;
                            hojaPersonalDirecto.Cells[cellPersonal].Value = pd.SumaHoras;

                            hojaPersonalDirecto.Cells[cellPersonal].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            hojaPersonalDirecto.Cells[cellPersonal].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            countInicial++;
                            indice++;
                        }


                        #endregion

                        


                        var RowFinal = hojaPersonalDirecto.Dimension.End.Row;
                        hojaPersonalDirecto.Cells["A1"].Value = RowFinal; //Indicador Final por Secciones

                        hojaPersonalInDirecto.Cells["B2:R12"].Copy(hojaPersonalDirecto.Cells["B" + RowFinal]); //Añade Seccion Indirectos a Hoja Principal












                        var personalesDirectoAnterior = (from p in detallesAnteriores
                                                         where p.TipoGastoId == TipoGasto.Directo
                                                         select p).ToList();

                        var personalesIndirectoDirectoAnterior = (from p in detallesAnteriores
                                                                  where p.TipoGastoId == TipoGasto.Indirecto
                                                                  select p).ToList();

                        var TodosdirectosCertificados = _directosRepository.GetAllIncluding(i => i.Especialidad, i => i.Etapa)
                                                          .Where(i => i.CertificadoId.HasValue)
                                                          .Where(i => i.CertificadoId == c.Id)
                                                          .ToList();

                        var TodosdirectosCertificadosE500 = (from de500 in detalleCertificados
                                                             where de500.TipoGastoId == TipoGasto.Directo
                                                             where de500.EsDistribucionE500 == true
                                                             where de500.CertificadoIngenieriaProyectoId == c.Id
                                                             select de500
                                                             ).ToList();





                        var TodosdirectosCertificadosAnteriores = new List<DetallesDirectosIngenieria>();
                        var TodosdirectosCertificadosAnterioresE500 = new List<GastoDirectoCertificado>();


                        if (certificadoAnteriorProyectoListado.Count > 0)
                        {
                            foreach (var itecertificadoAnteriorProyecto in certificadoAnteriorProyectoListado)
                            {
                                var data = _directosRepository.GetAllIncluding(i => i.Especialidad, i => i.Etapa)
                                                                                       .Where(i => i.CertificadoId.HasValue)
                                                                                       .Where(i => i.CertificadoId == itecertificadoAnteriorProyecto.Id)
                                                                                       .ToList();
                                if (data.Count > 0)
                                {

                                    TodosdirectosCertificadosAnteriores.AddRange(data);
                                }

                                var datae500 = (from de500 in detallesAnteriores
                                                where de500.TipoGastoId == TipoGasto.Directo
                                                where de500.EsDistribucionE500 == true
                                                where de500.CertificadoIngenieriaProyectoId == itecertificadoAnteriorProyecto.Id
                                                select de500).ToList();


                                if (datae500.Count > 0)
                                {
                                    TodosdirectosCertificadosAnterioresE500.AddRange(datae500);
                                }

                            }
                        }



                        #region DesgloceDirectos Ingenieria Basica
                        //DESGLOCE INGENIERIA B
                        var directosCertificados = (from d in TodosdirectosCertificados
                                                    where d.EtapaId.HasValue
                                                    where d.Etapa.codigo == CertificacionIngenieriaCodigos.ETAPA_INGENIERIAB
                                                    select d
                                                   ).ToList();
                        var directosCertificadosE500 = (from d in TodosdirectosCertificadosE500

                                                        where d.NombreEtapa == CertificacionIngenieriaCodigos.ETAPA_INGENIERIAB
                                                        select d
                                                ).ToList();

                        var totalC = (from t in directosCertificados where t.EspecialidadId.HasValue where t.Especialidad.codigo == CertificacionIngenieriaCodigos.ESPECIALIDAD_C select t.NumeroHoras).ToList();
                        var totalP = (from t in directosCertificados where t.EspecialidadId.HasValue where t.Especialidad.codigo == CertificacionIngenieriaCodigos.ESPECIALIDAD_P select t.NumeroHoras).ToList();
                        var totalE = (from t in directosCertificados where t.EspecialidadId.HasValue where t.Especialidad.codigo == CertificacionIngenieriaCodigos.ESPECIALIDAD_E select t.NumeroHoras).ToList();
                        var totalI = (from t in directosCertificados where t.EspecialidadId.HasValue where t.Especialidad.codigo == CertificacionIngenieriaCodigos.ESPECIALIDAD_I select t.NumeroHoras).ToList();
                        var totalR = (from t in directosCertificados where t.EspecialidadId.HasValue where t.Especialidad.codigo == CertificacionIngenieriaCodigos.ESPECIALIDAD_R select t.NumeroHoras).ToList();
                        var totalM = (from t in directosCertificados where t.EspecialidadId.HasValue where t.Especialidad.codigo == CertificacionIngenieriaCodigos.ESPECIALIDAD_M select t.NumeroHoras).ToList();

                        var totalCE500 = (from t in directosCertificadosE500 where t.NombreEspecialidad == CertificacionIngenieriaCodigos.ESPECIALIDAD_C select t.TotalHoras).ToList();
                        var totalPE500 = (from t in directosCertificadosE500 where t.NombreEspecialidad == CertificacionIngenieriaCodigos.ESPECIALIDAD_P select t.TotalHoras).ToList();
                        var totalEE500 = (from t in directosCertificadosE500 where t.NombreEspecialidad == CertificacionIngenieriaCodigos.ESPECIALIDAD_E select t.TotalHoras).ToList();
                        var totalIE500 = (from t in directosCertificadosE500 where t.NombreEspecialidad == CertificacionIngenieriaCodigos.ESPECIALIDAD_I select t.TotalHoras).ToList();
                        var totalRE500 = (from t in directosCertificadosE500 where t.NombreEspecialidad == CertificacionIngenieriaCodigos.ESPECIALIDAD_R select t.TotalHoras).ToList();
                        var totalME500 = (from t in directosCertificadosE500 where t.NombreEspecialidad == CertificacionIngenieriaCodigos.ESPECIALIDAD_M select t.TotalHoras).ToList();



                        int countdesgloce = 15;
                        string celdaDesgloce = "$O" + countdesgloce;

                        decimal totalCategoriaC = (totalC.Count > 0 ? totalC.Sum() : 0) + (totalCE500.Count > 0 ? totalCE500.Sum() : 0);

                        //C
                        hojaPersonalDirecto.Cells[celdaDesgloce].Value = totalCategoriaC;
                        countdesgloce++;
                        //P
                        celdaDesgloce = "$O" + countdesgloce;

                        decimal totalCategoriaP = (totalP.Count > 0 ? totalP.Sum() : 0) + (totalPE500.Count > 0 ? totalPE500.Sum() : 0);
                        hojaPersonalDirecto.Cells[celdaDesgloce].Value = totalCategoriaP;
                        countdesgloce++;
                        //E

                        celdaDesgloce = "$O" + countdesgloce;
                        decimal totalCategoriaE = (totalE.Count > 0 ? totalE.Sum() : 0) + (totalEE500.Count > 0 ? totalEE500.Sum() : 0);
                        hojaPersonalDirecto.Cells[celdaDesgloce].Value = totalCategoriaE;
                        countdesgloce++;
                        //I
                        celdaDesgloce = "$O" + countdesgloce;

                        decimal totalCategoriaI = (totalI.Count > 0 ? totalI.Sum() : 0) + (totalIE500.Count > 0 ? totalIE500.Sum() : 0);
                        hojaPersonalDirecto.Cells[celdaDesgloce].Value = totalCategoriaI;
                        countdesgloce++;
                        //R
                        celdaDesgloce = "$O" + countdesgloce;

                        decimal totalCategoriaR = (totalR.Count > 0 ? totalR.Sum() : 0) + (totalRE500.Count > 0 ? totalRE500.Sum() : 0);
                        hojaPersonalDirecto.Cells[celdaDesgloce].Value = totalCategoriaR;
                        countdesgloce++;
                        //M
                        celdaDesgloce = "$O" + countdesgloce;
                        decimal totalCategoriaM = (totalM.Count > 0 ? totalM.Sum() : 0) + (totalME500.Count > 0 ? totalME500.Sum() : 0);
                        hojaPersonalDirecto.Cells[celdaDesgloce].Value = totalCategoriaM;
                        countdesgloce++;


                        //Anteriores Ingenieria Basica
                        var directosCertificadosAnteriores = (from d in TodosdirectosCertificadosAnteriores
                                                              where d.EtapaId.HasValue
                                                              where d.Etapa.codigo == CertificacionIngenieriaCodigos.ETAPA_INGENIERIAB
                                                              select d
                                              ).ToList();

                        var directosCertificadosAnterioresE500 = (from d in TodosdirectosCertificadosAnterioresE500

                                                                  where d.NombreEtapa == CertificacionIngenieriaCodigos.ETAPA_INGENIERIAB
                                                                  select d
                                          ).ToList();

                        var totalAC = (from t in directosCertificadosAnteriores where t.EspecialidadId.HasValue where t.Especialidad.codigo == CertificacionIngenieriaCodigos.ESPECIALIDAD_C select t.NumeroHoras).ToList();
                        var totalAP = (from t in directosCertificadosAnteriores where t.EspecialidadId.HasValue where t.Especialidad.codigo == CertificacionIngenieriaCodigos.ESPECIALIDAD_P select t.NumeroHoras).ToList();
                        var totalAE = (from t in directosCertificadosAnteriores where t.EspecialidadId.HasValue where t.Especialidad.codigo == CertificacionIngenieriaCodigos.ESPECIALIDAD_E select t.NumeroHoras).ToList();
                        var totalAI = (from t in directosCertificadosAnteriores where t.EspecialidadId.HasValue where t.Especialidad.codigo == CertificacionIngenieriaCodigos.ESPECIALIDAD_I select t.NumeroHoras).ToList();
                        var totalAR = (from t in directosCertificadosAnteriores where t.EspecialidadId.HasValue where t.Especialidad.codigo == CertificacionIngenieriaCodigos.ESPECIALIDAD_R select t.NumeroHoras).ToList();
                        var totalAM = (from t in directosCertificadosAnteriores where t.EspecialidadId.HasValue where t.Especialidad.codigo == CertificacionIngenieriaCodigos.ESPECIALIDAD_M select t.NumeroHoras).ToList();

                        var totalACE500 = (from t in directosCertificadosAnterioresE500 where t.NombreEspecialidad == CertificacionIngenieriaCodigos.ESPECIALIDAD_C select t.TotalHoras).ToList();
                        var totalAPE500 = (from t in directosCertificadosAnterioresE500 where t.NombreEspecialidad == CertificacionIngenieriaCodigos.ESPECIALIDAD_P select t.TotalHoras).ToList();
                        var totalAEE500 = (from t in directosCertificadosAnterioresE500 where t.NombreEspecialidad == CertificacionIngenieriaCodigos.ESPECIALIDAD_E select t.TotalHoras).ToList();
                        var totalAIE500 = (from t in directosCertificadosAnterioresE500 where t.NombreEspecialidad == CertificacionIngenieriaCodigos.ESPECIALIDAD_I select t.TotalHoras).ToList();
                        var totalARE500 = (from t in directosCertificadosAnterioresE500 where t.NombreEspecialidad == CertificacionIngenieriaCodigos.ESPECIALIDAD_R select t.TotalHoras).ToList();
                        var totalAME500 = (from t in directosCertificadosAnterioresE500 where t.NombreEspecialidad == CertificacionIngenieriaCodigos.ESPECIALIDAD_M select t.TotalHoras).ToList();


                        countdesgloce = 15;
                        celdaDesgloce = "$N" + countdesgloce;

                        //C
                        decimal totalAnteriorEspecialidadC = (totalAC.Count > 0 ? totalAC.Sum() : 0) + (totalACE500.Count > 0 ? totalACE500.Sum() : 0);
                        hojaPersonalDirecto.Cells[celdaDesgloce].Value = totalAnteriorEspecialidadC;
                        countdesgloce++;
                        //P
                        celdaDesgloce = "$N" + countdesgloce;
                        decimal totalAnteriorEspecialidadP = (totalAP.Count > 0 ? totalAP.Sum() : 0) + (totalAPE500.Count > 0 ? totalAPE500.Sum() : 0);
                        hojaPersonalDirecto.Cells[celdaDesgloce].Value = totalAnteriorEspecialidadP;
                        countdesgloce++;
                        celdaDesgloce = "$N" + countdesgloce;
                        //E
                        decimal totalAnteriorEspecialidadE = (totalAE.Count > 0 ? totalAE.Sum() : 0) + (totalAEE500.Count > 0 ? totalAEE500.Sum() : 0);
                        hojaPersonalDirecto.Cells[celdaDesgloce].Value = totalAnteriorEspecialidadE;
                        countdesgloce++;
                        //I
                        celdaDesgloce = "$N" + countdesgloce;
                        decimal totalAnteriorEspecialidaI = (totalAI.Count > 0 ? totalAI.Sum() : 0) + (totalAIE500.Count > 0 ? totalAIE500.Sum() : 0);
                        hojaPersonalDirecto.Cells[celdaDesgloce].Value = totalAnteriorEspecialidaI;
                        countdesgloce++;

                        decimal totalAnteriorEspecialidadR = (totalAR.Count > 0 ? totalAR.Sum() : 0) + (totalARE500.Count > 0 ? totalARE500.Sum() : 0);
                        //R
                        celdaDesgloce = "$N" + countdesgloce;
                        hojaPersonalDirecto.Cells[celdaDesgloce].Value = totalAnteriorEspecialidadR;
                        countdesgloce++;
                        //M
                        celdaDesgloce = "$N" + countdesgloce;
                        decimal totalAnteriorEspecialidadM = (totalAM.Count > 0 ? totalAM.Sum() : 0) + (totalAME500.Count > 0 ? totalAME500.Sum() : 0);
                        hojaPersonalDirecto.Cells[celdaDesgloce].Value = totalAnteriorEspecialidadM;
                        countdesgloce++;

                        #endregion





                        #region DesgloceDirectos Ingenieria Detalle
                        //DESGLOCE INGENIERIA B
                        directosCertificados = (from d in TodosdirectosCertificados
                                                where d.EtapaId.HasValue
                                                where d.Etapa.codigo == CertificacionIngenieriaCodigos.ETAPA_DETALLE || d.Etapa.codigo == CertificacionIngenieriaCodigos.ESTAPA_NA
                                                select d
                                                    ).ToList();
                        directosCertificadosE500 = (from d in TodosdirectosCertificadosE500

                                                    where d.NombreEtapa == CertificacionIngenieriaCodigos.ETAPA_DETALLE || d.NombreEtapa == CertificacionIngenieriaCodigos.ESTAPA_NA
                                                    select d
                                               ).ToList();

                        totalC = (from t in directosCertificados where t.EspecialidadId.HasValue where t.Especialidad.codigo == CertificacionIngenieriaCodigos.ESPECIALIDAD_C select t.NumeroHoras).ToList();
                        totalP = (from t in directosCertificados where t.EspecialidadId.HasValue where t.Especialidad.codigo == CertificacionIngenieriaCodigos.ESPECIALIDAD_P select t.NumeroHoras).ToList();
                        totalE = (from t in directosCertificados where t.EspecialidadId.HasValue where t.Especialidad.codigo == CertificacionIngenieriaCodigos.ESPECIALIDAD_E select t.NumeroHoras).ToList();
                        totalI = (from t in directosCertificados where t.EspecialidadId.HasValue where t.Especialidad.codigo == CertificacionIngenieriaCodigos.ESPECIALIDAD_I select t.NumeroHoras).ToList();
                        totalR = (from t in directosCertificados where t.EspecialidadId.HasValue where t.Especialidad.codigo == CertificacionIngenieriaCodigos.ESPECIALIDAD_R select t.NumeroHoras).ToList();
                        totalM = (from t in directosCertificados where t.EspecialidadId.HasValue where t.Especialidad.codigo == CertificacionIngenieriaCodigos.ESPECIALIDAD_M select t.NumeroHoras).ToList();

                        totalCE500 = (from t in directosCertificadosE500 where t.NombreEspecialidad == CertificacionIngenieriaCodigos.ESPECIALIDAD_C select t.TotalHoras).ToList();
                        totalPE500 = (from t in directosCertificadosE500 where t.NombreEspecialidad == CertificacionIngenieriaCodigos.ESPECIALIDAD_P select t.TotalHoras).ToList();
                        totalEE500 = (from t in directosCertificadosE500 where t.NombreEspecialidad == CertificacionIngenieriaCodigos.ESPECIALIDAD_E select t.TotalHoras).ToList();
                        totalIE500 = (from t in directosCertificadosE500 where t.NombreEspecialidad == CertificacionIngenieriaCodigos.ESPECIALIDAD_I select t.TotalHoras).ToList();
                        totalRE500 = (from t in directosCertificadosE500 where t.NombreEspecialidad == CertificacionIngenieriaCodigos.ESPECIALIDAD_R select t.TotalHoras).ToList();
                        totalME500 = (from t in directosCertificadosE500 where t.NombreEspecialidad == CertificacionIngenieriaCodigos.ESPECIALIDAD_M select t.TotalHoras).ToList();



                        countdesgloce = 23;
                        celdaDesgloce = "$O" + countdesgloce;

                        totalCategoriaC = (totalC.Count > 0 ? totalC.Sum() : 0) + (totalCE500.Count > 0 ? totalCE500.Sum() : 0);

                        //C
                        hojaPersonalDirecto.Cells[celdaDesgloce].Value = totalCategoriaC;
                        countdesgloce++;
                        //P
                        celdaDesgloce = "$O" + countdesgloce;

                        totalCategoriaP = (totalP.Count > 0 ? totalP.Sum() : 0) + (totalPE500.Count > 0 ? totalPE500.Sum() : 0);
                        hojaPersonalDirecto.Cells[celdaDesgloce].Value = totalCategoriaP;
                        countdesgloce++;
                        //E

                        celdaDesgloce = "$O" + countdesgloce;
                        totalCategoriaE = (totalE.Count > 0 ? totalE.Sum() : 0) + (totalEE500.Count > 0 ? totalEE500.Sum() : 0);
                        hojaPersonalDirecto.Cells[celdaDesgloce].Value = totalCategoriaE;
                        countdesgloce++;
                        //I
                        celdaDesgloce = "$O" + countdesgloce;

                        totalCategoriaI = (totalI.Count > 0 ? totalI.Sum() : 0) + (totalIE500.Count > 0 ? totalIE500.Sum() : 0);
                        hojaPersonalDirecto.Cells[celdaDesgloce].Value = totalCategoriaI;
                        countdesgloce++;
                        //R
                        celdaDesgloce = "$O" + countdesgloce;

                        totalCategoriaR = (totalR.Count > 0 ? totalR.Sum() : 0) + (totalRE500.Count > 0 ? totalRE500.Sum() : 0);
                        hojaPersonalDirecto.Cells[celdaDesgloce].Value = totalCategoriaR;
                        countdesgloce++;
                        //M
                        celdaDesgloce = "$O" + countdesgloce;
                        totalCategoriaM = (totalM.Count > 0 ? totalM.Sum() : 0) + (totalME500.Count > 0 ? totalME500.Sum() : 0);
                        hojaPersonalDirecto.Cells[celdaDesgloce].Value = totalCategoriaM;
                        countdesgloce++;


                        //Anteriores Ingenieria Basica
                        directosCertificadosAnteriores = (from d in TodosdirectosCertificadosAnteriores
                                                          where d.EtapaId.HasValue
                                                          where d.Etapa.codigo == CertificacionIngenieriaCodigos.ETAPA_DETALLE || d.Etapa.codigo == CertificacionIngenieriaCodigos.ESTAPA_NA

                                                          select d
                                             ).ToList();

                        directosCertificadosAnterioresE500 = (from d in TodosdirectosCertificadosAnterioresE500


                                                              where d.NombreEtapa == CertificacionIngenieriaCodigos.ETAPA_DETALLE || d.NombreEtapa == CertificacionIngenieriaCodigos.ESTAPA_NA
                                                              select d
                                         ).ToList();

                        totalAC = (from t in directosCertificadosAnteriores where t.EspecialidadId.HasValue where t.Especialidad.codigo == CertificacionIngenieriaCodigos.ESPECIALIDAD_C select t.NumeroHoras).ToList();
                        totalAP = (from t in directosCertificadosAnteriores where t.EspecialidadId.HasValue where t.Especialidad.codigo == CertificacionIngenieriaCodigos.ESPECIALIDAD_P select t.NumeroHoras).ToList();
                        totalAE = (from t in directosCertificadosAnteriores where t.EspecialidadId.HasValue where t.Especialidad.codigo == CertificacionIngenieriaCodigos.ESPECIALIDAD_E select t.NumeroHoras).ToList();
                        totalAI = (from t in directosCertificadosAnteriores where t.EspecialidadId.HasValue where t.Especialidad.codigo == CertificacionIngenieriaCodigos.ESPECIALIDAD_I select t.NumeroHoras).ToList();
                        totalAR = (from t in directosCertificadosAnteriores where t.EspecialidadId.HasValue where t.Especialidad.codigo == CertificacionIngenieriaCodigos.ESPECIALIDAD_R select t.NumeroHoras).ToList();
                        totalAM = (from t in directosCertificadosAnteriores where t.EspecialidadId.HasValue where t.Especialidad.codigo == CertificacionIngenieriaCodigos.ESPECIALIDAD_M select t.NumeroHoras).ToList();

                        totalACE500 = (from t in directosCertificadosAnterioresE500 where t.NombreEspecialidad == CertificacionIngenieriaCodigos.ESPECIALIDAD_C select t.TotalHoras).ToList();
                        totalAPE500 = (from t in directosCertificadosAnterioresE500 where t.NombreEspecialidad == CertificacionIngenieriaCodigos.ESPECIALIDAD_P select t.TotalHoras).ToList();
                        totalAEE500 = (from t in directosCertificadosAnterioresE500 where t.NombreEspecialidad == CertificacionIngenieriaCodigos.ESPECIALIDAD_E select t.TotalHoras).ToList();
                        totalAIE500 = (from t in directosCertificadosAnterioresE500 where t.NombreEspecialidad == CertificacionIngenieriaCodigos.ESPECIALIDAD_I select t.TotalHoras).ToList();
                        totalARE500 = (from t in directosCertificadosAnterioresE500 where t.NombreEspecialidad == CertificacionIngenieriaCodigos.ESPECIALIDAD_R select t.TotalHoras).ToList();
                        totalAME500 = (from t in directosCertificadosAnterioresE500 where t.NombreEspecialidad == CertificacionIngenieriaCodigos.ESPECIALIDAD_M select t.TotalHoras).ToList();


                        countdesgloce = 23;
                        celdaDesgloce = "$N" + countdesgloce;

                        //C
                        totalAnteriorEspecialidadC = (totalAC.Count > 0 ? totalAC.Sum() : 0) + (totalACE500.Count > 0 ? totalACE500.Sum() : 0);
                        hojaPersonalDirecto.Cells[celdaDesgloce].Value = totalAnteriorEspecialidadC;
                        countdesgloce++;
                        //P
                        celdaDesgloce = "$N" + countdesgloce;
                        totalAnteriorEspecialidadP = (totalAP.Count > 0 ? totalAP.Sum() : 0) + (totalAPE500.Count > 0 ? totalAPE500.Sum() : 0);
                        hojaPersonalDirecto.Cells[celdaDesgloce].Value = totalAnteriorEspecialidadP;
                        countdesgloce++;
                        celdaDesgloce = "$N" + countdesgloce;
                        //E
                        totalAnteriorEspecialidadE = (totalAE.Count > 0 ? totalAE.Sum() : 0) + (totalAEE500.Count > 0 ? totalAEE500.Sum() : 0);
                        hojaPersonalDirecto.Cells[celdaDesgloce].Value = totalAnteriorEspecialidadE;
                        countdesgloce++;
                        //I
                        celdaDesgloce = "$N" + countdesgloce;
                        totalAnteriorEspecialidaI = (totalAI.Count > 0 ? totalAI.Sum() : 0) + (totalAIE500.Count > 0 ? totalAIE500.Sum() : 0);
                        hojaPersonalDirecto.Cells[celdaDesgloce].Value = totalAnteriorEspecialidaI;
                        countdesgloce++;

                        totalAnteriorEspecialidadR = (totalAR.Count > 0 ? totalAR.Sum() : 0) + (totalARE500.Count > 0 ? totalARE500.Sum() : 0);
                        //R
                        celdaDesgloce = "$N" + countdesgloce;
                        hojaPersonalDirecto.Cells[celdaDesgloce].Value = totalAnteriorEspecialidadR;
                        countdesgloce++;
                        //M
                        celdaDesgloce = "$N" + countdesgloce;
                        totalAnteriorEspecialidadM = (totalAM.Count > 0 ? totalAM.Sum() : 0) + (totalAME500.Count > 0 ? totalAME500.Sum() : 0);
                        hojaPersonalDirecto.Cells[celdaDesgloce].Value = totalAnteriorEspecialidadM;
                        countdesgloce++;



                        #endregion

                        #region DesgloceDirectos Asbuilt

                        directosCertificados = (from d in TodosdirectosCertificados
                                                where d.EtapaId.HasValue
                                                where d.Etapa.codigo == CertificacionIngenieriaCodigos.ETAPA_ASBUILT
                                                select d
                                                     ).ToList();
                        directosCertificadosE500 = (from d in TodosdirectosCertificadosE500

                                                    where d.NombreEtapa == CertificacionIngenieriaCodigos.ETAPA_ASBUILT
                                                    select d
                                               ).ToList();

                        totalC = (from t in directosCertificados where t.EspecialidadId.HasValue where t.Especialidad.codigo == CertificacionIngenieriaCodigos.ESPECIALIDAD_C select t.NumeroHoras).ToList();
                        totalP = (from t in directosCertificados where t.EspecialidadId.HasValue where t.Especialidad.codigo == CertificacionIngenieriaCodigos.ESPECIALIDAD_P select t.NumeroHoras).ToList();
                        totalE = (from t in directosCertificados where t.EspecialidadId.HasValue where t.Especialidad.codigo == CertificacionIngenieriaCodigos.ESPECIALIDAD_E select t.NumeroHoras).ToList();
                        totalI = (from t in directosCertificados where t.EspecialidadId.HasValue where t.Especialidad.codigo == CertificacionIngenieriaCodigos.ESPECIALIDAD_I select t.NumeroHoras).ToList();
                        totalR = (from t in directosCertificados where t.EspecialidadId.HasValue where t.Especialidad.codigo == CertificacionIngenieriaCodigos.ESPECIALIDAD_R select t.NumeroHoras).ToList();
                        totalM = (from t in directosCertificados where t.EspecialidadId.HasValue where t.Especialidad.codigo == CertificacionIngenieriaCodigos.ESPECIALIDAD_M select t.NumeroHoras).ToList();

                        totalCE500 = (from t in directosCertificadosE500 where t.NombreEspecialidad == CertificacionIngenieriaCodigos.ESPECIALIDAD_C select t.TotalHoras).ToList();
                        totalPE500 = (from t in directosCertificadosE500 where t.NombreEspecialidad == CertificacionIngenieriaCodigos.ESPECIALIDAD_P select t.TotalHoras).ToList();
                        totalEE500 = (from t in directosCertificadosE500 where t.NombreEspecialidad == CertificacionIngenieriaCodigos.ESPECIALIDAD_E select t.TotalHoras).ToList();
                        totalIE500 = (from t in directosCertificadosE500 where t.NombreEspecialidad == CertificacionIngenieriaCodigos.ESPECIALIDAD_I select t.TotalHoras).ToList();
                        totalRE500 = (from t in directosCertificadosE500 where t.NombreEspecialidad == CertificacionIngenieriaCodigos.ESPECIALIDAD_R select t.TotalHoras).ToList();
                        totalME500 = (from t in directosCertificadosE500 where t.NombreEspecialidad == CertificacionIngenieriaCodigos.ESPECIALIDAD_M select t.TotalHoras).ToList();



                        countdesgloce = 31;
                        celdaDesgloce = "$O" + countdesgloce;

                        totalCategoriaC = (totalC.Count > 0 ? totalC.Sum() : 0) + (totalCE500.Count > 0 ? totalCE500.Sum() : 0);

                        //C
                        hojaPersonalDirecto.Cells[celdaDesgloce].Value = totalCategoriaC;
                        countdesgloce++;
                        //P
                        celdaDesgloce = "$O" + countdesgloce;

                        totalCategoriaP = (totalP.Count > 0 ? totalP.Sum() : 0) + (totalPE500.Count > 0 ? totalPE500.Sum() : 0);
                        hojaPersonalDirecto.Cells[celdaDesgloce].Value = totalCategoriaP;
                        countdesgloce++;
                        //E

                        celdaDesgloce = "$O" + countdesgloce;
                        totalCategoriaE = (totalE.Count > 0 ? totalE.Sum() : 0) + (totalEE500.Count > 0 ? totalEE500.Sum() : 0);
                        hojaPersonalDirecto.Cells[celdaDesgloce].Value = totalCategoriaE;
                        countdesgloce++;
                        //I
                        celdaDesgloce = "$O" + countdesgloce;

                        totalCategoriaI = (totalI.Count > 0 ? totalI.Sum() : 0) + (totalIE500.Count > 0 ? totalIE500.Sum() : 0);
                        hojaPersonalDirecto.Cells[celdaDesgloce].Value = totalCategoriaI;
                        countdesgloce++;
                        //R
                        celdaDesgloce = "$O" + countdesgloce;

                        totalCategoriaR = (totalR.Count > 0 ? totalR.Sum() : 0) + (totalRE500.Count > 0 ? totalRE500.Sum() : 0);
                        hojaPersonalDirecto.Cells[celdaDesgloce].Value = totalCategoriaR;
                        countdesgloce++;
                        //M
                        celdaDesgloce = "$O" + countdesgloce;
                        totalCategoriaM = (totalM.Count > 0 ? totalM.Sum() : 0) + (totalME500.Count > 0 ? totalME500.Sum() : 0);
                        hojaPersonalDirecto.Cells[celdaDesgloce].Value = totalCategoriaM;
                        countdesgloce++;


                        //Anteriores Ingenieria Basica
                        directosCertificadosAnteriores = (from d in TodosdirectosCertificadosAnteriores
                                                          where d.EtapaId.HasValue
                                                          where d.Etapa.codigo == CertificacionIngenieriaCodigos.ETAPA_ASBUILT
                                                          select d
                                             ).ToList();

                        directosCertificadosAnterioresE500 = (from d in TodosdirectosCertificadosAnterioresE500

                                                              where d.NombreEtapa == CertificacionIngenieriaCodigos.ETAPA_ASBUILT
                                                              select d
                                         ).ToList();

                        totalAC = (from t in directosCertificadosAnteriores where t.EspecialidadId.HasValue where t.Especialidad.codigo == CertificacionIngenieriaCodigos.ESPECIALIDAD_C select t.NumeroHoras).ToList();
                        totalAP = (from t in directosCertificadosAnteriores where t.EspecialidadId.HasValue where t.Especialidad.codigo == CertificacionIngenieriaCodigos.ESPECIALIDAD_P select t.NumeroHoras).ToList();
                        totalAE = (from t in directosCertificadosAnteriores where t.EspecialidadId.HasValue where t.Especialidad.codigo == CertificacionIngenieriaCodigos.ESPECIALIDAD_E select t.NumeroHoras).ToList();
                        totalAI = (from t in directosCertificadosAnteriores where t.EspecialidadId.HasValue where t.Especialidad.codigo == CertificacionIngenieriaCodigos.ESPECIALIDAD_I select t.NumeroHoras).ToList();
                        totalAR = (from t in directosCertificadosAnteriores where t.EspecialidadId.HasValue where t.Especialidad.codigo == CertificacionIngenieriaCodigos.ESPECIALIDAD_R select t.NumeroHoras).ToList();
                        totalAM = (from t in directosCertificadosAnteriores where t.EspecialidadId.HasValue where t.Especialidad.codigo == CertificacionIngenieriaCodigos.ESPECIALIDAD_M select t.NumeroHoras).ToList();

                        totalACE500 = (from t in directosCertificadosAnterioresE500 where t.NombreEspecialidad == CertificacionIngenieriaCodigos.ESPECIALIDAD_C select t.TotalHoras).ToList();
                        totalAPE500 = (from t in directosCertificadosAnterioresE500 where t.NombreEspecialidad == CertificacionIngenieriaCodigos.ESPECIALIDAD_P select t.TotalHoras).ToList();
                        totalAEE500 = (from t in directosCertificadosAnterioresE500 where t.NombreEspecialidad == CertificacionIngenieriaCodigos.ESPECIALIDAD_E select t.TotalHoras).ToList();
                        totalAIE500 = (from t in directosCertificadosAnterioresE500 where t.NombreEspecialidad == CertificacionIngenieriaCodigos.ESPECIALIDAD_I select t.TotalHoras).ToList();
                        totalARE500 = (from t in directosCertificadosAnterioresE500 where t.NombreEspecialidad == CertificacionIngenieriaCodigos.ESPECIALIDAD_R select t.TotalHoras).ToList();
                        totalAME500 = (from t in directosCertificadosAnterioresE500 where t.NombreEspecialidad == CertificacionIngenieriaCodigos.ESPECIALIDAD_M select t.TotalHoras).ToList();


                        countdesgloce = 31;
                        celdaDesgloce = "$N" + countdesgloce;

                        //C
                        totalAnteriorEspecialidadC = (totalAC.Count > 0 ? totalAC.Sum() : 0) + (totalACE500.Count > 0 ? totalACE500.Sum() : 0);
                        hojaPersonalDirecto.Cells[celdaDesgloce].Value = totalAnteriorEspecialidadC;
                        countdesgloce++;
                        //P
                        celdaDesgloce = "$N" + countdesgloce;
                        totalAnteriorEspecialidadP = (totalAP.Count > 0 ? totalAP.Sum() : 0) + (totalAPE500.Count > 0 ? totalAPE500.Sum() : 0);
                        hojaPersonalDirecto.Cells[celdaDesgloce].Value = totalAnteriorEspecialidadP;
                        countdesgloce++;
                        celdaDesgloce = "$N" + countdesgloce;
                        //E
                        totalAnteriorEspecialidadE = (totalAE.Count > 0 ? totalAE.Sum() : 0) + (totalAEE500.Count > 0 ? totalAEE500.Sum() : 0);
                        hojaPersonalDirecto.Cells[celdaDesgloce].Value = totalAnteriorEspecialidadE;
                        countdesgloce++;
                        //I
                        celdaDesgloce = "$N" + countdesgloce;
                        totalAnteriorEspecialidaI = (totalAI.Count > 0 ? totalAI.Sum() : 0) + (totalAIE500.Count > 0 ? totalAIE500.Sum() : 0);
                        hojaPersonalDirecto.Cells[celdaDesgloce].Value = totalAnteriorEspecialidaI;
                        countdesgloce++;

                        totalAnteriorEspecialidadR = (totalAR.Count > 0 ? totalAR.Sum() : 0) + (totalARE500.Count > 0 ? totalARE500.Sum() : 0);
                        //R
                        celdaDesgloce = "$N" + countdesgloce;
                        hojaPersonalDirecto.Cells[celdaDesgloce].Value = totalAnteriorEspecialidadR;
                        countdesgloce++;
                        //M
                        celdaDesgloce = "$N" + countdesgloce;
                        totalAnteriorEspecialidadM = (totalAM.Count > 0 ? totalAM.Sum() : 0) + (totalAME500.Count > 0 ? totalAME500.Sum() : 0);
                        hojaPersonalDirecto.Cells[celdaDesgloce].Value = totalAnteriorEspecialidadM;
                        countdesgloce++;


                        #endregion




                        //INDIRECTOS


                        int PosicionInicialPersonalIndirecto = RowFinal + 2;
                        int PosicionFinPersonalIndirecto = PosicionInicialPersonalIndirecto + 5;
                        int MaxFilasPersonalIndirecto = 6;

                        hojaPersonalDirecto.Cells["A2"].Value = PosicionInicialPersonalIndirecto; //PosicionInicialIndirectos


                        var personalesIndirectos = (from p in detalleCertificados
                                                    where p.TipoGastoId == TipoGasto.Indirecto
                                                    select p).ToList();


                        //Desgloce I-ING   I - PyCP





                        var RangoDesgloceI_ING = (from celda in hojaPersonalDirecto.Cells
                                                  where celda.Value?.ToString().Contains("#I-ING") == true
                                                  select celda).FirstOrDefault(); ;
                        if (RangoDesgloceI_ING != null)
                        {

                            var finaInicial = RangoDesgloceI_ING.Start.Row;
                            var celda = "$O" + finaInicial;


                            var registrosActuales = (from i in personalesIndirectos
                                                     where i.Area == CertificacionIngenieriaCodigos.AREA_INGENIERIA || i.Area == ""
                                                     where !i.EsViatico
                                                     select i
                                         ).ToList();


                            hojaPersonalDirecto.Cells[celda].Value = registrosActuales.Count > 0 ? registrosActuales.Sum(x => x.TotalHoras) : 0;

                            var celdaAnterior = "$N" + finaInicial;
                            var registrosAnteriores = (from i in personalesIndirectoDirectoAnterior
                                                       where i.Area == CertificacionIngenieriaCodigos.AREA_INGENIERIA || i.Area == ""
                                                       where !i.EsViatico
                                                       select i
                                        ).ToList();

                            hojaPersonalDirecto.Cells[celdaAnterior].Value = registrosAnteriores.Count > 0 ? registrosAnteriores.Sum(x => x.TotalHoras) : 0;

                        }


                        var RangoDesgloceI_PYPC = (from celda in hojaPersonalDirecto.Cells
                                                   where celda.Value?.ToString().Contains("#I-PYCP") == true
                                                   select celda).FirstOrDefault(); ;

                        if (RangoDesgloceI_PYPC != null)
                        {

                            var finaInicial = RangoDesgloceI_PYPC.Start.Row;
                            var celda = "$O" + finaInicial;
                            var registrosActuales = (from i in personalesIndirectos
                                                     where i.Area == CertificacionIngenieriaCodigos.AREA_PYCP
                                                     where !i.EsViatico
                                                     select i
                                         ).ToList();


                            hojaPersonalDirecto.Cells[celda].Value = registrosActuales.Count > 0 ? registrosActuales.Sum(x => x.TotalHoras) : 0;

                            var celdaAnterior = "$N" + finaInicial;
                            var registrosAnteriores = (from i in personalesIndirectoDirectoAnterior
                                                       where i.Area == CertificacionIngenieriaCodigos.AREA_PYCP
                                                       where !i.EsViatico
                                                       select i
                                        ).ToList();

                            hojaPersonalDirecto.Cells[celdaAnterior].Value = registrosAnteriores.Count > 0 ? registrosAnteriores.Sum(x => x.TotalHoras) : 0;

                        }


                        int FilaMayorIndirectos = 0;
                        var personalInDirectosAgrupadosUIO = (from t in personalesIndirectos
                                                              where t.RubroId.HasValue
                                                              where t.UbicacionTrabajo == CertificacionIngenieriaCodigos.UBICACION_UIO || t.UbicacionTrabajo == ""
                                                              where !t.EsViatico
                                                              group t by new
                                                              {
                                                                  t.ColaboradorId,
                                                                  t.Rubro.ItemId
                                                              }
                                                        into g
                                                              select new
                                                              {
                                                                  Grupo = g.Key,
                                                                  SumaHoras = g.Sum(x => x.TotalHoras)
                                                              }).ToList();

                        var personalInDirectosAgrupadosOIT = (from t in personalesIndirectos
                                                              where t.RubroId.HasValue
                                                              where t.UbicacionTrabajo == CertificacionIngenieriaCodigos.UBICACION_CAMPO
                                                              where !t.EsViatico
                                                              group t by new
                                                              {
                                                                  t.ColaboradorId,
                                                                  t.Rubro.ItemId
                                                              }
                                                       into g
                                                              select new
                                                              {
                                                                  Grupo = g.Key,
                                                                  SumaHoras = g.Sum(x => x.TotalHoras)
                                                              }).ToList();




                        int FilasPersonalUIO = personalInDirectosAgrupadosUIO.Count;
                        int FilasPersonalOIT = personalInDirectosAgrupadosOIT.Count;

                        if (FilasPersonalUIO >= FilasPersonalOIT)
                        {
                            FilaMayorIndirectos = FilasPersonalUIO;
                        }
                        else
                        {
                            FilaMayorIndirectos = FilasPersonalOIT;
                        }

                        int FilasPersonalInDirecto = FilaMayorIndirectos;

                        if (FilasPersonalInDirecto > MaxFilasPersonalIndirecto)
                        {
                            int Filas_A_Aumentar = FilasPersonalInDirecto - MaxFilasPersonalIndirecto;
                            hojaPersonalDirecto.InsertRow(PosicionFinPersonalIndirecto, Filas_A_Aumentar, PosicionFinPersonalIndirecto - 1); //Aumentar Filas de Indirectos 
                        }

                        int posicionInicialReal = PosicionInicialPersonalIndirecto;
                        countInicial = PosicionInicialPersonalIndirecto;
                        cellIndice = "C" + countInicial;
                        cellPersonal = "D" + countInicial;
                        int indiceIndirecto = 1;

                        foreach (var pi in personalInDirectosAgrupadosUIO)
                        {
                            var Colaborador = _colaboradorRepository.GetAll().Where(x => x.Id == pi.Grupo.ColaboradorId).FirstOrDefault();
                            var Categoria = _itemRepository.GetAll().Where(x => x.Id == pi.Grupo.ItemId).FirstOrDefault();

                            cellIndice = "B" + countInicial;
                            hojaPersonalDirecto.Cells[cellIndice].Value = indiceIndirecto;
                            hojaPersonalDirecto.Cells[cellIndice].Style.Font.Bold = true;


                            cellPersonal = "C" + countInicial;
                            hojaPersonalDirecto.Cells[cellPersonal].Value = Colaborador != null ? Colaborador.nombres_apellidos.ToUpper() : "";

                            hojaPersonalDirecto.Cells[cellPersonal].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                            cellPersonal = "F" + countInicial;
                            hojaPersonalDirecto.Cells[cellPersonal].Value = Categoria != null ? Categoria.nombre.ToUpper() : "";

                            hojaPersonalDirecto.Cells[cellPersonal].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                            cellPersonal = "I" + countInicial;
                            hojaPersonalDirecto.Cells[cellPersonal].Value = pi.SumaHoras;

                            hojaPersonalDirecto.Cells[cellPersonal].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            hojaPersonalDirecto.Cells[cellPersonal].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            countInicial++;
                            indiceIndirecto++;
                        }
                        countInicial = posicionInicialReal;
                        cellIndice = "C" + countInicial;
                        cellPersonal = "D" + countInicial;
                        indiceIndirecto = 1;
                        foreach (var pi in personalInDirectosAgrupadosOIT)
                        {
                            var Colaborador = _colaboradorRepository.GetAll().Where(x => x.Id == pi.Grupo.ColaboradorId).FirstOrDefault();
                            var Categoria = _itemRepository.GetAll().Where(x => x.Id == pi.Grupo.ItemId).FirstOrDefault();

                            cellIndice = "J" + countInicial;
                            hojaPersonalDirecto.Cells[cellIndice].Value = indiceIndirecto;
                            hojaPersonalDirecto.Cells[cellIndice].Style.Font.Bold = true;


                            cellPersonal = "K" + countInicial;
                            hojaPersonalDirecto.Cells[cellPersonal].Value = Colaborador != null ? Colaborador.nombres_apellidos.ToUpper() : "";

                            hojaPersonalDirecto.Cells[cellPersonal].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                            cellPersonal = "N" + countInicial;
                            hojaPersonalDirecto.Cells[cellPersonal].Value = Categoria != null ? Categoria.nombre.ToUpper() : "";

                            hojaPersonalDirecto.Cells[cellPersonal].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                            cellPersonal = "Q" + countInicial;
                            hojaPersonalDirecto.Cells[cellPersonal].Value = pi.SumaHoras;

                            hojaPersonalDirecto.Cells[cellPersonal].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            hojaPersonalDirecto.Cells[cellPersonal].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            countInicial++;
                            indiceIndirecto++;
                        }

                        RowFinal = hojaPersonalDirecto.Dimension.End.Row;
                        hojaPersonalDirecto.Cells["A3"].Value = RowFinal; //Indicador Final por Secciones


                        hojaSubtotales.Cells["A2:R25"].Copy(hojaPersonalDirecto.Cells["A" + RowFinal]); //Añade Seccion Subtotales a Hoja Principal




                        var computos = new List<ComputoPresupuesto>();
                        if (presupuestos.Count > 0)
                        {
                            var computosPresupuesto = _computoPresupuesto.GetAllIncluding(x => x.Item)
                                                                         .Where(x => x.WbsPresupuesto.vigente)
                                                                       //   .Where(x => x.WbsPresupuesto.PresupuestoId == presupuesto.Id)
                                                                       .Where(x => presupuestos.Contains(x.WbsPresupuesto.PresupuestoId))
                                                                         .Where(x => x.vigente)
                                                                         .Where(x => x.Item.GrupoId == 1)
                                                                         .Where(x => x.Item.codigo.StartsWith("1.1") || x.Item.codigo.StartsWith("1.2"))
                                                                         .ToList();

                            if (computosPresupuesto.Count > 0)
                            {
                                computos.AddRange(computosPresupuesto);
                            }
                        }





                        var rangoSubtotalDirecto = (from celda in hojaPersonalDirecto.Cells
                                                    where celda.Value?.ToString().Contains("#DIRECTOS") == true
                                                    select celda)
                                                    .FirstOrDefault();
                        if (rangoSubtotalDirecto != null)
                        {



                            int PosicionValoresDirectos = rangoSubtotalDirecto.Start.Row + 1;
                            hojaPersonalDirecto.Cells["A7"].Value = PosicionValoresDirectos; //Inicial DIrectos

                            var countInicialRubrosDirectos = PosicionValoresDirectos;
                            var celdaRubroDirecto = "B";
                            var RubrosDirectosProyecto = this.RubrosDirectosCertificadosPorProyecto(c.ProyectoId, fechaCertificadoGrupo);

                            int CountRubrosDinamicos = RubrosDirectosProyecto.Count;

                            if (CountRubrosDinamicos > 0)
                            {
                                hojaPersonalDirecto.InsertRow(PosicionValoresDirectos + 1, CountRubrosDinamicos, PosicionValoresDirectos + 1);

                                for (int i = PosicionValoresDirectos; i <= PosicionValoresDirectos + CountRubrosDinamicos; i++)
                                {

                                    var rangoFila = "B" + i + ":R" + i;
                                    hojaPersonalDirecto.Cells[rangoFila].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    hojaPersonalDirecto.Cells[rangoFila].Style.Fill.BackgroundColor.SetColor(Color.White);

                                    var cellBordeIzquierdo = "B" + i;
                                    hojaPersonalDirecto.Cells[cellBordeIzquierdo].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                    hojaPersonalDirecto.Cells[cellBordeIzquierdo].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                    cellBordeIzquierdo = "I" + i;
                                    hojaPersonalDirecto.Cells[cellBordeIzquierdo].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                    cellBordeIzquierdo = "K" + i;
                                    hojaPersonalDirecto.Cells[cellBordeIzquierdo].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                    cellBordeIzquierdo = "M" + i;
                                    hojaPersonalDirecto.Cells[cellBordeIzquierdo].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                    cellBordeIzquierdo = "O" + i;
                                    hojaPersonalDirecto.Cells[cellBordeIzquierdo].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                    cellBordeIzquierdo = "Q" + i;
                                    hojaPersonalDirecto.Cells[cellBordeIzquierdo].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                    cellBordeIzquierdo = "S" + i;
                                    hojaPersonalDirecto.Cells[cellBordeIzquierdo].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                    cellBordeIzquierdo = "R" + i;
                                    hojaPersonalDirecto.Cells[cellBordeIzquierdo].Style.Border.Right.Style = ExcelBorderStyle.Thin;


                                    var cellTarifa = "G" + i;
                                    hojaPersonalDirecto.Cells[cellTarifa].Style.Numberformat.Format = "#,##0.00";
                                    hojaPersonalDirecto.Cells[cellTarifa].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                                    var cellUnidad = "H" + i;
                                    hojaPersonalDirecto.Cells[cellUnidad].Value = "HH"; //Inicial DIrectos
                                    hojaPersonalDirecto.Cells[cellUnidad].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                                    var cellPresupuesto = "J" + i;
                                    var formula = "=I" + i + "*$G" + i + "";
                                    hojaPersonalDirecto.Cells[cellPresupuesto].Formula = formula;
                                    hojaPersonalDirecto.Cells[cellPresupuesto].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                    hojaPersonalDirecto.Cells[cellPresupuesto].Style.Numberformat.Format = "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";


                                    var cellPrevioHoras = "K" + i;
                                    hojaPersonalDirecto.Cells[cellPrevioHoras].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                    hojaPersonalDirecto.Cells[cellPrevioHoras].Style.Numberformat.Format = "#,##0.00";

                                    var cellPrevioUSD = "L" + i;
                                    hojaPersonalDirecto.Cells[cellPrevioUSD].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                    hojaPersonalDirecto.Cells[cellPrevioUSD].Style.Numberformat.Format = "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";

                                    var cellActualHoras = "M" + i;
                                    hojaPersonalDirecto.Cells[cellActualHoras].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                    hojaPersonalDirecto.Cells[cellActualHoras].Style.Numberformat.Format = "#,##0.00";

                                    var cellActualUSD = "N" + i;
                                    formula = "=M" + i + "*$G" + i;
                                    hojaPersonalDirecto.Cells[cellActualUSD].Formula = formula;
                                    hojaPersonalDirecto.Cells[cellActualUSD].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                    hojaPersonalDirecto.Cells[cellActualUSD].Style.Numberformat.Format = "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";


                                    var cellAcumuladoHoras = "O" + i;
                                    formula = "=M" + i + "+K" + i;
                                    hojaPersonalDirecto.Cells[cellAcumuladoHoras].Formula = formula;
                                    hojaPersonalDirecto.Cells[cellAcumuladoHoras].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                    hojaPersonalDirecto.Cells[cellAcumuladoHoras].Style.Numberformat.Format = "#,##0.00";

                                    var cellAcumuladoUSD = "P" + i;
                                    formula = "=N" + i + "+L" + i + "";
                                    hojaPersonalDirecto.Cells[cellAcumuladoUSD].Formula = formula;
                                    hojaPersonalDirecto.Cells[cellAcumuladoUSD].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                    hojaPersonalDirecto.Cells[cellAcumuladoUSD].Style.Numberformat.Format = "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";


                                    var cellSaldoHoras = "Q" + i;


                                    if (computos.Count > 0) // En la definición Saldo SI almenos hay un computo registrado en presupuestos
                                    {
                                        formula = "=IF(AND(I" + i + "=0,M" + i + "=0),0,I" + i + "-O" + i + ")";
                                        hojaPersonalDirecto.Cells[cellSaldoHoras].Formula = formula;
                                    }
                                    else
                                    {
                                        hojaPersonalDirecto.Cells[cellSaldoHoras].Value = 0;
                                    }

                                    hojaPersonalDirecto.Cells[cellSaldoHoras].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                    hojaPersonalDirecto.Cells[cellSaldoHoras].Style.Numberformat.Format = "#,##0.00";


                                    //MODIFICACION FORMULA PARA PINTAR COLORES. Y SI SOLO TIENE UN COMPUTO VA EL APROBADO
                                    //SI TIENE UN PRESPUESTO VA SALDOS

                                    //SI NO TIENE SALDOS V

                                    var cellSaldoUSD = "$R" + i;

                                    if (computos.Count > 0)
                                    {
                                        formula = "=IF(AND($J" + i + "=0,$N" + i + "=0),0,$J" + i + "-$P" + i + ")";

                                        hojaPersonalDirecto.Cells[cellSaldoUSD].Formula = formula;
                                    }
                                    else
                                    {
                                        hojaPersonalDirecto.Cells[cellSaldoUSD].Value = 0;
                                    }

                                    hojaPersonalDirecto.Cells[cellSaldoUSD].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                    hojaPersonalDirecto.Cells[cellSaldoUSD].Style.Numberformat.Format = "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";


                                    var cf = hojaPersonalDirecto.ConditionalFormatting.AddLessThan(hojaPersonalDirecto.Cells[cellSaldoHoras]);
                                    cf.Formula = "0";
                                    cf.Style.Fill.BackgroundColor.Color = Color.FromArgb(253, 195, 209);
                                    cf.Style.Font.Color.Color = Color.FromArgb(158, 0, 0);

                                    cf = hojaPersonalDirecto.ConditionalFormatting.AddLessThan(hojaPersonalDirecto.Cells[cellSaldoUSD]);
                                    cf.Formula = "0";
                                    cf.Style.Fill.BackgroundColor.Color = Color.FromArgb(253, 195, 209);
                                    cf.Style.Font.Color.Color = Color.FromArgb(158, 0, 0);

                                }
                            }


                            foreach (var item in RubrosDirectosProyecto)
                            {
                                celdaRubroDirecto = "B" + countInicialRubrosDirectos;
                                hojaPersonalDirecto.Cells[celdaRubroDirecto].Value = item.codigo;

                                celdaRubroDirecto = "C" + countInicialRubrosDirectos + ":E" + countInicialRubrosDirectos;
                                hojaPersonalDirecto.Cells[celdaRubroDirecto].Merge = true;
                                hojaPersonalDirecto.Cells[celdaRubroDirecto].Value = item.nombre.ToUpper();
                                countInicialRubrosDirectos++;
                            }

                            for (int i = PosicionValoresDirectos; i <= (PosicionValoresDirectos + CountRubrosDinamicos + 1); i++)
                            {

                                //Valores Actuales

                                string cellHoras = "M" + i;
                                string cellUsd = "N" + i;
                                string codigoItem = (hojaPersonalDirecto.Cells["B" + i].Value ?? "").ToString();

                                if (codigoItem.Length > 0)
                                {

                                    decimal horas = 0;
                                    decimal valorusd = 0;

                                    var tarifaMigrado = (from p in personalesDirecto
                                                         where p.RubroId.HasValue
                                                         where p.Rubro.Item.codigo == codigoItem
                                                         //   where p.migrado
                                                         select p.Tarifa).FirstOrDefault();

                                    var celdaTarifa = "G" + i;
                                    decimal valorTarifa = 0;
                                    var celdaCantidadPresupuestada = "I" + i;
                                    var celdaMontoPresupuestada = "J" + i;

                                    if (tarifaMigrado != 0)
                                    {
                                        hojaPersonalDirecto.Cells[celdaTarifa].Value = tarifaMigrado;
                                        valorTarifa = tarifaMigrado;
                                    }
                                    else
                                    {
                                        var dtoValoresTarifaItem = this.ObtenerValoresPresupuestoporCodigoItem(codigoItem, c.Proyecto.contratoId, computos);

                                        hojaPersonalDirecto.Cells[celdaTarifa].Value = dtoValoresTarifaItem.tarifa;
                                        valorTarifa = dtoValoresTarifaItem.tarifa;
                                        hojaPersonalDirecto.Cells[celdaCantidadPresupuestada].Value = dtoValoresTarifaItem.cantidad;
                                        hojaPersonalDirecto.Cells[celdaMontoPresupuestada].Value = dtoValoresTarifaItem.monto;



                                        hojaPersonalDirecto.Cells[celdaCantidadPresupuestada].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                        hojaPersonalDirecto.Cells[celdaCantidadPresupuestada].Style.Numberformat.Format = "#,##0.00";
                                        hojaPersonalDirecto.Cells[celdaMontoPresupuestada].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                        hojaPersonalDirecto.Cells[celdaMontoPresupuestada].Style.Numberformat.Format = "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";

                                    }


                                    var valores = (from p in personalesDirecto
                                                   where p.RubroId.HasValue
                                                   where p.Rubro.Item.codigo == codigoItem
                                                   select p).ToList();
                                    if (valores.Count > 0)
                                    {

                                        horas = valores.Select(x => x.TotalHoras).Sum();

                                        valorusd = valores.Select(x => x.TarifaHoras).Sum();
                                    }

                                    hojaPersonalDirecto.Cells[cellHoras].Value = horas;
                                    hojaPersonalDirecto.Cells[cellUsd].Value = valorusd;

                                    /*
                                    //presupuesto
                                    decimal valorpresupuesto = 0;
                                    decimal valorusdpresupuesto = 0;
                                    var valorexcel = (from p in computos
                                                      where p.Item.codigo == codigoItem
                                                      select p).ToList();
                                    if (valorexcel.Count > 0)
                                    {

                                        valorpresupuesto = valorexcel.Select(x => x.cantidad).Sum();
                                        valorusdpresupuesto = valorexcel.Select(x => x.costo_total).Sum();


                                    }

                                    //CELDAS PRESUPUESTO
                                    string cellHorasPresupuesto = "H" + i;
                                    string cellUSPresupuesto = "I" + i;
                                    hojaPersonalDirecto.Cells[cellHorasPresupuesto].Value = valorpresupuesto;

                                    */



                                    string cellHorasAnteriores = "K" + i;

                                    string cellMontoUSDAnterior = "L" + i;
                                    decimal valorAnterior = 0;
                                    decimal montoAnterior = 0;

                                    var cantidadAnterior = (from p in personalesDirectoAnterior
                                                            where p.RubroId.HasValue
                                                            where p.Rubro.Item.codigo == codigoItem
                                                            select p).ToList();
                                    if (cantidadAnterior.Count > 0)
                                    {
                                        valorAnterior = cantidadAnterior.Select(x => x.TotalHoras).Sum();

                                        if (!c.Proyecto.certificable_ingenieria)
                                        {

                                            montoAnterior = valorAnterior * valorTarifa;
                                        }
                                        else
                                        {
                                            montoAnterior = cantidadAnterior.Select(x => (x.Tarifa * x.TotalHoras)).Sum();
                                        }


                                    }

                                    hojaPersonalDirecto.Cells[cellHorasAnteriores].Value = valorAnterior;


                                    //
                                    hojaPersonalDirecto.Cells[cellMontoUSDAnterior].Value = montoAnterior;

                                }
                                else
                                {
                                    hojaPersonalDirecto.Row(i).Style.Font.Color.SetColor(Color.White);
                                }

                            }






                        }




                        var rangoSubtotalIUIO = (from celda in hojaPersonalDirecto.Cells
                                                 where celda.Value?.ToString().Contains("#IUIO") == true
                                                 select celda)
                                               .FirstOrDefault();





                        if (rangoSubtotalIUIO != null)
                        {
                            int PosicionValoresUIO = rangoSubtotalIUIO.Start.Row + 1;

                            hojaPersonalDirecto.Cells["A8"].Value = PosicionValoresUIO; //Inicial DIrectos

                            var countInicialIUIO = PosicionValoresUIO;


                            var countInicialRubrosIndirectoUIO = PosicionValoresUIO;
                            var celdaRubroIndirectoUIO = "B";

                            var RubrosInDirectosProyectoUIO = this.RubrosIndirectosCertificadosPorProyectoUIO(c.ProyectoId, fechaCertificadoGrupo);



                            int CountRubrosDinamicosUIO = RubrosInDirectosProyectoUIO.Count;


                            if (CountRubrosDinamicosUIO > 0)
                            {
                                hojaPersonalDirecto.InsertRow(PosicionValoresUIO + 1, CountRubrosDinamicosUIO, PosicionValoresUIO + 1);

                                for (int i = PosicionValoresUIO; i <= (PosicionValoresUIO + CountRubrosDinamicosUIO); i++)
                                {
                                    var rangoFila = "B" + i + ":R" + i;
                                    hojaPersonalDirecto.Cells[rangoFila].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    hojaPersonalDirecto.Cells[rangoFila].Style.Fill.BackgroundColor.SetColor(Color.White);
                                    var cellBordeIzquierdo = "B" + i;
                                    hojaPersonalDirecto.Cells[cellBordeIzquierdo].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                    hojaPersonalDirecto.Cells[cellBordeIzquierdo].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                    cellBordeIzquierdo = "I" + i;
                                    hojaPersonalDirecto.Cells[cellBordeIzquierdo].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                    cellBordeIzquierdo = "K" + i;
                                    hojaPersonalDirecto.Cells[cellBordeIzquierdo].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                    cellBordeIzquierdo = "M" + i;
                                    hojaPersonalDirecto.Cells[cellBordeIzquierdo].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                    cellBordeIzquierdo = "O" + i;
                                    hojaPersonalDirecto.Cells[cellBordeIzquierdo].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                    cellBordeIzquierdo = "Q" + i;
                                    hojaPersonalDirecto.Cells[cellBordeIzquierdo].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                    cellBordeIzquierdo = "S" + i;
                                    hojaPersonalDirecto.Cells[cellBordeIzquierdo].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                    cellBordeIzquierdo = "R" + i;
                                    hojaPersonalDirecto.Cells[cellBordeIzquierdo].Style.Border.Right.Style = ExcelBorderStyle.Thin;


                                    var cellTarifa = "G" + i;
                                    hojaPersonalDirecto.Cells[cellTarifa].Style.Numberformat.Format = "#,##0.00";
                                    hojaPersonalDirecto.Cells[cellTarifa].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                                    var cellUnidad = "H" + i;
                                    hojaPersonalDirecto.Cells[cellUnidad].Value = "HH"; //Inicial DIrectos
                                    hojaPersonalDirecto.Cells[cellUnidad].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                                    var cellPresupuesto = "J" + i;

                                    var formula = "=I" + i + "*$G" + i + "";
                                    hojaPersonalDirecto.Cells[cellPresupuesto].Formula = formula;
                                    hojaPersonalDirecto.Cells[cellPresupuesto].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                    hojaPersonalDirecto.Cells[cellPresupuesto].Style.Numberformat.Format = "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";


                                    var cellPrevioHoras = "K" + i;
                                    hojaPersonalDirecto.Cells[cellPrevioHoras].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                    hojaPersonalDirecto.Cells[cellPrevioHoras].Style.Numberformat.Format = "#,##0.00";

                                    var cellPrevioUSD = "L" + i;
                                    formula = "=K" + i + "*$G" + i;
                                    hojaPersonalDirecto.Cells[cellPrevioUSD].Formula = formula;
                                    hojaPersonalDirecto.Cells[cellPrevioUSD].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                    hojaPersonalDirecto.Cells[cellPrevioUSD].Style.Numberformat.Format = "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";

                                    var cellActualHoras = "M" + i;
                                    // formula = "=L" + i + "*$F" + i;
                                    hojaPersonalDirecto.Cells[cellActualHoras].Formula = formula;
                                    hojaPersonalDirecto.Cells[cellActualHoras].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                    hojaPersonalDirecto.Cells[cellActualHoras].Style.Numberformat.Format = "#,##0.00";

                                    var cellActualUSD = "N" + i;
                                    //formula = "=L" + i + "*$F" + i;
                                    hojaPersonalDirecto.Cells[cellActualUSD].Formula = formula;
                                    hojaPersonalDirecto.Cells[cellActualUSD].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                    hojaPersonalDirecto.Cells[cellActualUSD].Style.Numberformat.Format = "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";


                                    var cellAcumuladoHoras = "O" + i;
                                    formula = "=$M" + i + "+$K" + i;
                                    hojaPersonalDirecto.Cells[cellAcumuladoHoras].Formula = formula;
                                    hojaPersonalDirecto.Cells[cellAcumuladoHoras].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                    hojaPersonalDirecto.Cells[cellAcumuladoHoras].Style.Numberformat.Format = "#,##0.00";

                                    var cellAcumuladoUSD = "P" + i;
                                    formula = "=$O" + i + "*$G" + i + "";
                                    hojaPersonalDirecto.Cells[cellAcumuladoUSD].Formula = formula;
                                    hojaPersonalDirecto.Cells[cellAcumuladoUSD].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                    hojaPersonalDirecto.Cells[cellAcumuladoUSD].Style.Numberformat.Format = "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";


                                    var cellSaldoHoras = "Q" + i;
                                    if (computos.Count > 0)
                                    {

                                        formula = "=IF(AND(I" + i + "=0,M" + i + "=0),0,$I" + i + "-$O" + i + ")";
                                        hojaPersonalDirecto.Cells[cellSaldoHoras].Formula = formula;
                                    }
                                    else
                                    {
                                        hojaPersonalDirecto.Cells[cellSaldoHoras].Value = 0;
                                    }
                                    hojaPersonalDirecto.Cells[cellSaldoHoras].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                    hojaPersonalDirecto.Cells[cellSaldoHoras].Style.Numberformat.Format = "#,##0.00";

                                    var cellSaldoUSD = "$R" + i;//+ ":R" + i;
                                    if (computos.Count > 0)
                                    {
                                        formula = "=IF(AND($J" + i + "=0,N" + i + "=0),0,$Q" + i + "*$G" + i + ")";
                                        //hojaPersonalDirecto.Cells[cellSaldoUSD].Merge = true;
                                        hojaPersonalDirecto.Cells[cellSaldoUSD].Formula = formula;

                                    }
                                    else
                                    {
                                        hojaPersonalDirecto.Cells[cellSaldoUSD].Value = 0;
                                    }
                                    hojaPersonalDirecto.Cells[cellSaldoUSD].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                    hojaPersonalDirecto.Cells[cellSaldoUSD].Style.Numberformat.Format = "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";


                                    var cf = hojaPersonalDirecto.ConditionalFormatting.AddLessThan(hojaPersonalDirecto.Cells[cellSaldoHoras]);
                                    cf.Formula = "0";
                                    cf.Style.Fill.BackgroundColor.Color = Color.FromArgb(253, 195, 209);
                                    cf.Style.Font.Color.Color = Color.FromArgb(158, 0, 0);

                                    cf = hojaPersonalDirecto.ConditionalFormatting.AddLessThan(hojaPersonalDirecto.Cells[cellSaldoUSD]);
                                    cf.Formula = "0";
                                    cf.Style.Fill.BackgroundColor.Color = Color.FromArgb(253, 195, 209);
                                    cf.Style.Font.Color.Color = Color.FromArgb(158, 0, 0);

                                }
                            }


                            foreach (var item in RubrosInDirectosProyectoUIO)
                            {
                                celdaRubroIndirectoUIO = "B" + countInicialRubrosIndirectoUIO;
                                hojaPersonalDirecto.Cells[celdaRubroIndirectoUIO].Value = item.codigo;

                                celdaRubroIndirectoUIO = "C" + countInicialRubrosIndirectoUIO + ":E" + countInicialRubrosIndirectoUIO;
                                hojaPersonalDirecto.Cells[celdaRubroIndirectoUIO].Merge = true;
                                hojaPersonalDirecto.Cells[celdaRubroIndirectoUIO].Value = item.nombre.ToUpper();
                                countInicialRubrosIndirectoUIO++;
                            }
                            for (int i = PosicionValoresUIO; i <= (PosicionValoresUIO + CountRubrosDinamicosUIO + 1); i++)
                            {
                                string cellHoras = "M" + i;
                                string cellUsd = "N" + i;
                                decimal horas = 0;
                                decimal valorusd = 0;
                                string codigoDirecto = (hojaPersonalDirecto.Cells["B" + i].Value ?? "").ToString();

                                if (codigoDirecto.Length > 0)
                                {
                                    var tarifa = (from p in personalesIndirectos
                                                  where p.RubroId.HasValue
                                                  where p.UbicacionTrabajo == CertificacionIngenieriaCodigos.UBICACION_UIO
                                                  where p.Rubro.Item.codigo == codigoDirecto
                                                  select p.Tarifa).FirstOrDefault();

                                    var celdaTarifa = "G" + i;
                                    decimal valorTarifa = 0;

                                    var celdaCantidadPresupuestada = "I" + i;
                                    var celdaMontoPresupuestada = "J" + i;
                                    if (tarifa != 0)
                                    {
                                        hojaPersonalDirecto.Cells[celdaTarifa].Value = tarifa;
                                        valorTarifa = tarifa;
                                    }
                                    else
                                    {
                                        var dtoValoresTarifaItem = this.ObtenerValoresPresupuestoporCodigoItem(codigoDirecto, c.Proyecto.contratoId, computos);

                                        hojaPersonalDirecto.Cells[celdaTarifa].Value = dtoValoresTarifaItem.tarifa;
                                        hojaPersonalDirecto.Cells[celdaCantidadPresupuestada].Value = dtoValoresTarifaItem.cantidad;
                                        hojaPersonalDirecto.Cells[celdaMontoPresupuestada].Value = dtoValoresTarifaItem.monto;
                                        valorTarifa = dtoValoresTarifaItem.tarifa;


                                        hojaPersonalDirecto.Cells[celdaCantidadPresupuestada].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                        hojaPersonalDirecto.Cells[celdaCantidadPresupuestada].Style.Numberformat.Format = "#,##0.00";
                                        hojaPersonalDirecto.Cells[celdaMontoPresupuestada].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                        hojaPersonalDirecto.Cells[celdaMontoPresupuestada].Style.Numberformat.Format = "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";


                                    }


                                    var valores = (from p in personalesIndirectos
                                                   where p.RubroId.HasValue
                                                   where p.UbicacionTrabajo == CertificacionIngenieriaCodigos.UBICACION_UIO
                                                   where p.Rubro.Item.codigo == codigoDirecto
                                                   select p).ToList();
                                    if (valores.Count > 0)
                                    {

                                        horas = valores.Select(x => x.TotalHoras).Sum();

                                        valorusd = valores.Select(x => x.TarifaHoras).Sum();
                                    }

                                    hojaPersonalDirecto.Cells[cellHoras].Value = horas;
                                    hojaPersonalDirecto.Cells[cellUsd].Value = valorusd;


                                    /*
                                    //presupuesto
                                    decimal valorpresupuesto = 0;
                                    decimal valorusdpresupuesto = 0;
                                    var valorexcel = (from p in computos
                                                      where p.Item.codigo == codigoDirecto
                                                      select p).ToList();
                                    if (valorexcel.Count > 0)
                                    {

                                        valorpresupuesto = valorexcel.Select(x => x.cantidad).Sum();
                                        valorusdpresupuesto = valorexcel.Select(x => x.costo_total).Sum();


                                    }

                                    string cellHorasPresupuesto = "H" + i;
                                    hojaPersonalDirecto.Cells[cellHorasPresupuesto].Value = valorpresupuesto;
                                    */

                                    string cellHorasAnteriores = "K" + i;

                                    string cellMontoUSDAnterior = "L" + i;
                                    decimal valorAnterior = 0;
                                    decimal montoAnterior = 0;

                                    var cantidadAnterior = (from p in personalesIndirectoDirectoAnterior
                                                            where p.RubroId.HasValue
                                                            where p.UbicacionTrabajo == CertificacionIngenieriaCodigos.UBICACION_UIO
                                                            where p.Rubro.Item.codigo == codigoDirecto
                                                            select p).ToList();
                                    if (cantidadAnterior.Count > 0)
                                    {
                                        valorAnterior = cantidadAnterior.Select(x => x.TotalHoras).Sum();

                                        if (!c.Proyecto.certificable_ingenieria)
                                        {

                                            montoAnterior = valorAnterior * valorTarifa;
                                        }
                                        else
                                        {
                                            montoAnterior = cantidadAnterior.Select(x => (x.Tarifa * x.TotalHoras)).Sum();
                                        }


                                    }

                                    hojaPersonalDirecto.Cells[cellHorasAnteriores].Value = valorAnterior;
                                    hojaPersonalDirecto.Cells[cellMontoUSDAnterior].Value = montoAnterior;

                                    var esViatico = codigoDirecto.StartsWith(CertificacionIngenieriaCodigos.CODIGOSVIATICOS);
                                    if (esViatico)
                                    {
                                        hojaPersonalDirecto.Cells["H" + i].Value = "";
                                    }

                                }
                                else
                                {
                                    hojaPersonalDirecto.Row(i).Style.Font.Color.SetColor(Color.White);
                                }


                            }

                        }

                        var rangoSubtotalIOT = (from celda in hojaPersonalDirecto.Cells
                                                where celda.Value?.ToString().Contains("#IOT") == true
                                                select celda)
                                                .FirstOrDefault();
                        if (rangoSubtotalIOT != null)
                        {
                            int PosicionValoresOIT = rangoSubtotalIOT.Start.Row + 1;

                            hojaPersonalDirecto.Cells["A8"].Value = PosicionValoresOIT; //Inicial Indectos

                            var countInicialOIT = PosicionValoresOIT;


                            var countInicialRubrosIndirectoOIT = PosicionValoresOIT;
                            var celdaRubroIndirectoOIT = "B";

                            var RubrosInDirectosProyectoOIT = this.RubrosIndirectosCertificadosPorProyectoOIT(c.ProyectoId, fechaCertificadoGrupo);



                            int CountRubrosDinamicosOIT = RubrosInDirectosProyectoOIT.Count;


                            if (CountRubrosDinamicosOIT > 0)
                            {
                                hojaPersonalDirecto.InsertRow(PosicionValoresOIT + 1, CountRubrosDinamicosOIT, PosicionValoresOIT + 1);

                                for (int i = PosicionValoresOIT; i <= (PosicionValoresOIT + CountRubrosDinamicosOIT); i++)
                                {
                                    var rangoFila = "B" + i + ":R" + i;
                                    hojaPersonalDirecto.Cells[rangoFila].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    hojaPersonalDirecto.Cells[rangoFila].Style.Fill.BackgroundColor.SetColor(Color.White);

                                    var cellBordeIzquierdo = "B" + i;
                                    hojaPersonalDirecto.Cells[cellBordeIzquierdo].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                    hojaPersonalDirecto.Cells[cellBordeIzquierdo].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                    cellBordeIzquierdo = "I" + i;
                                    hojaPersonalDirecto.Cells[cellBordeIzquierdo].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                    cellBordeIzquierdo = "K" + i;
                                    hojaPersonalDirecto.Cells[cellBordeIzquierdo].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                    cellBordeIzquierdo = "M" + i;
                                    hojaPersonalDirecto.Cells[cellBordeIzquierdo].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                    cellBordeIzquierdo = "O" + i;
                                    hojaPersonalDirecto.Cells[cellBordeIzquierdo].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                    cellBordeIzquierdo = "Q" + i;
                                    hojaPersonalDirecto.Cells[cellBordeIzquierdo].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                    cellBordeIzquierdo = "S" + i;
                                    hojaPersonalDirecto.Cells[cellBordeIzquierdo].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                    cellBordeIzquierdo = "R" + i;
                                    hojaPersonalDirecto.Cells[cellBordeIzquierdo].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                                    var cellTarifa = "G" + i;
                                    hojaPersonalDirecto.Cells[cellTarifa].Style.Numberformat.Format = "#,##0.00";
                                    hojaPersonalDirecto.Cells[cellTarifa].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                                    var cellUnidad = "H" + i;
                                    hojaPersonalDirecto.Cells[cellUnidad].Value = "HH"; //Inicial DIrectos
                                    hojaPersonalDirecto.Cells[cellUnidad].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                                    var cellPresupuesto = "J" + i;

                                    var formula = "=I" + i + "*$G" + i + "";
                                    hojaPersonalDirecto.Cells[cellPresupuesto].Formula = formula;
                                    hojaPersonalDirecto.Cells[cellPresupuesto].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                    hojaPersonalDirecto.Cells[cellPresupuesto].Style.Numberformat.Format = "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";


                                    var cellPrevioHoras = "K" + i;
                                    hojaPersonalDirecto.Cells[cellPrevioHoras].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                    hojaPersonalDirecto.Cells[cellPrevioHoras].Style.Numberformat.Format = "#,##0.00";

                                    var cellPrevioUSD = "L" + i;
                                    formula = "=K" + i + "*$G" + i;
                                    hojaPersonalDirecto.Cells[cellPrevioUSD].Formula = formula;
                                    hojaPersonalDirecto.Cells[cellPrevioUSD].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                    hojaPersonalDirecto.Cells[cellPrevioUSD].Style.Numberformat.Format = "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";

                                    var cellActualHoras = "M" + i;
                                    // formula = "=L" + i + "*$F" + i;
                                    hojaPersonalDirecto.Cells[cellActualHoras].Formula = formula;
                                    hojaPersonalDirecto.Cells[cellActualHoras].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                    hojaPersonalDirecto.Cells[cellActualHoras].Style.Numberformat.Format = "#,##0.00";

                                    var cellActualUSD = "N" + i;
                                    //formula = "=L" + i + "*$F" + i;
                                    hojaPersonalDirecto.Cells[cellActualUSD].Formula = formula;
                                    hojaPersonalDirecto.Cells[cellActualUSD].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                    hojaPersonalDirecto.Cells[cellActualUSD].Style.Numberformat.Format = "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";


                                    var cellAcumuladoHoras = "O" + i;
                                    formula = "=$M" + i + "+$K" + i;
                                    hojaPersonalDirecto.Cells[cellAcumuladoHoras].Formula = formula;
                                    hojaPersonalDirecto.Cells[cellAcumuladoHoras].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                    hojaPersonalDirecto.Cells[cellAcumuladoHoras].Style.Numberformat.Format = "#,##0.00";

                                    var cellAcumuladoUSD = "P" + i;
                                    formula = "=$O" + i + "*$G" + i + "";
                                    hojaPersonalDirecto.Cells[cellAcumuladoUSD].Formula = formula;
                                    hojaPersonalDirecto.Cells[cellAcumuladoUSD].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                    hojaPersonalDirecto.Cells[cellAcumuladoUSD].Style.Numberformat.Format = "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";


                                    var cellSaldoHoras = "Q" + i;

                                    if (computos.Count > 0)
                                    {
                                        formula = "=IF(AND(I" + i + "=0,M" + i + "=0),0,$I" + i + "-$O" + i + ")";
                                        hojaPersonalDirecto.Cells[cellSaldoHoras].Formula = formula;
                                    }
                                    else
                                    {
                                        hojaPersonalDirecto.Cells[cellSaldoHoras].Value = 0;
                                    }
                                    hojaPersonalDirecto.Cells[cellSaldoHoras].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                    hojaPersonalDirecto.Cells[cellSaldoHoras].Style.Numberformat.Format = "#,##0.00";

                                    var cellSaldoUSD = "R" + i;// + ":R" + i;


                                    if (computos.Count > 0)
                                    {
                                        formula = "=IF(AND($J" + i + "=0,$N" + i + "=0),0,$Q" + i + "*$G" + i + ")";
                                        //hojaPersonalDirecto.Cells[cellSaldoUSD].Merge = true;
                                        hojaPersonalDirecto.Cells[cellSaldoUSD].Formula = formula;
                                    }
                                    else
                                    {
                                        hojaPersonalDirecto.Cells[cellSaldoUSD].Value = 0;
                                    }
                                    hojaPersonalDirecto.Cells[cellSaldoUSD].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                    hojaPersonalDirecto.Cells[cellSaldoUSD].Style.Numberformat.Format = "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";

                                    var cf = hojaPersonalDirecto.ConditionalFormatting.AddLessThan(hojaPersonalDirecto.Cells[cellSaldoHoras]);
                                    cf.Formula = "0";
                                    cf.Style.Fill.BackgroundColor.Color = Color.FromArgb(253, 195, 209);
                                    cf.Style.Font.Color.Color = Color.FromArgb(158, 0, 0);

                                    cf = hojaPersonalDirecto.ConditionalFormatting.AddLessThan(hojaPersonalDirecto.Cells[cellSaldoUSD]);
                                    cf.Formula = "0";
                                    cf.Style.Fill.BackgroundColor.Color = Color.FromArgb(253, 195, 209);
                                    cf.Style.Font.Color.Color = Color.FromArgb(158, 0, 0);


                                }
                            }


                            foreach (var item in RubrosInDirectosProyectoOIT)
                            {
                                celdaRubroIndirectoOIT = "B" + countInicialRubrosIndirectoOIT;
                                hojaPersonalDirecto.Cells[celdaRubroIndirectoOIT].Value = item.codigo;

                                celdaRubroIndirectoOIT = "C" + countInicialRubrosIndirectoOIT + ":E" + countInicialRubrosIndirectoOIT;
                                hojaPersonalDirecto.Cells[celdaRubroIndirectoOIT].Merge = true;
                                hojaPersonalDirecto.Cells[celdaRubroIndirectoOIT].Value = item.nombre.ToUpper();
                                countInicialRubrosIndirectoOIT++;
                            }


                            for (int i = PosicionValoresOIT; i <= (PosicionValoresOIT + CountRubrosDinamicosOIT + 1); i++)
                            {
                                string cellHoras = "M" + i;
                                string cellUsd = "N" + i;
                                decimal horas = 0;
                                decimal valorusd = 0;
                                string codigoDirecto = (hojaPersonalDirecto.Cells["B" + i].Value ?? "").ToString();

                                if (codigoDirecto.Length > 0)
                                {


                                    var tarifa = (from p in personalesIndirectos
                                                  where p.RubroId.HasValue
                                                  where p.UbicacionTrabajo == CertificacionIngenieriaCodigos.UBICACION_CAMPO
                                                  where p.Rubro.Item.codigo == codigoDirecto
                                                  select p.Tarifa).FirstOrDefault();


                                    var celdaTarifa = "G" + i;
                                    decimal valorTarifa = 0;
                                    var celdaCantidadPresupuestada = "I" + i;
                                    var celdaMontoPresupuestada = "J" + i;
                                    if (tarifa != 0)
                                    {
                                        hojaPersonalDirecto.Cells[celdaTarifa].Value = tarifa;
                                        valorTarifa = tarifa;
                                    }
                                    else
                                    {
                                        var dtoValoresTarifaItem = this.ObtenerValoresPresupuestoporCodigoItem(codigoDirecto, c.Proyecto.contratoId, computos);

                                        hojaPersonalDirecto.Cells[celdaTarifa].Value = dtoValoresTarifaItem.tarifa;
                                        hojaPersonalDirecto.Cells[celdaCantidadPresupuestada].Value = dtoValoresTarifaItem.cantidad;
                                        hojaPersonalDirecto.Cells[celdaMontoPresupuestada].Value = dtoValoresTarifaItem.monto;
                                        valorTarifa = dtoValoresTarifaItem.tarifa;


                                        hojaPersonalDirecto.Cells[celdaCantidadPresupuestada].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                        hojaPersonalDirecto.Cells[celdaCantidadPresupuestada].Style.Numberformat.Format = "#,##0.00";
                                        hojaPersonalDirecto.Cells[celdaMontoPresupuestada].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                        hojaPersonalDirecto.Cells[celdaMontoPresupuestada].Style.Numberformat.Format = "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";



                                    }


                                    var valores = (from p in personalesIndirectos
                                                   where p.RubroId.HasValue
                                                   where p.UbicacionTrabajo == CertificacionIngenieriaCodigos.UBICACION_CAMPO || p.UbicacionTrabajo == ""
                                                   where p.Rubro.Item.codigo == codigoDirecto
                                                   select p).ToList();
                                    if (valores.Count > 0)
                                    {

                                        horas = valores.Select(x => x.TotalHoras).Sum();

                                        valorusd = valores.Select(x => x.TarifaHoras).Sum();
                                    }

                                    hojaPersonalDirecto.Cells[cellHoras].Value = horas;
                                    hojaPersonalDirecto.Cells[cellUsd].Value = valorusd;


                                    /*
                                    //presupuesto
                                    decimal valorpresupuesto = 0;
                                    decimal valorusdpresupuesto = 0;
                                    var valorexcel = (from p in computos
                                                      where p.Item.codigo == codigoDirecto
                                                      select p).ToList();
                                    if (valorexcel.Count > 0)
                                    {

                                        valorpresupuesto = valorexcel.Select(x => x.cantidad).Sum();
                                        valorusdpresupuesto = valorexcel.Select(x => x.costo_total).Sum();


                                    }

                                    string cellHorasPresupuesto = "H" + i;
                                    hojaPersonalDirecto.Cells[cellHorasPresupuesto].Value = valorpresupuesto;
                                    */

                                    string cellHorasAnteriores = "K" + i;

                                    string cellMontoUSDAnterior = "L" + i;
                                    decimal valorAnterior = 0;
                                    decimal montoAnterior = 0;

                                    var cantidadAnterior = (from p in personalesIndirectoDirectoAnterior
                                                            where p.RubroId.HasValue
                                                            where p.UbicacionTrabajo == CertificacionIngenieriaCodigos.UBICACION_CAMPO || p.UbicacionTrabajo == ""
                                                            where p.Rubro.Item.codigo == codigoDirecto
                                                            select p).ToList();
                                    if (cantidadAnterior.Count > 0)
                                    {
                                        valorAnterior = cantidadAnterior.Select(x => x.TotalHoras).Sum();


                                        if (!c.Proyecto.certificable_ingenieria)
                                        {

                                            montoAnterior = valorAnterior * valorTarifa;
                                        }
                                        else
                                        {
                                            montoAnterior = cantidadAnterior.Select(x => (x.Tarifa * x.TotalHoras)).Sum();
                                        }

                                    }

                                    hojaPersonalDirecto.Cells[cellHorasAnteriores].Value = valorAnterior;
                                    hojaPersonalDirecto.Cells[cellMontoUSDAnterior].Value = montoAnterior;


                                    var esViatico = codigoDirecto.StartsWith(CertificacionIngenieriaCodigos.CODIGOSVIATICOS);
                                    if (esViatico)
                                    {
                                        hojaPersonalDirecto.Cells["H" + i].Value = "";
                                    }
                                }
                                else
                                {
                                    hojaPersonalDirecto.Row(i).Style.Font.Color.SetColor(Color.White);
                                }

                            }

                        }

                        var formatoCondicional = (from celda in hojaPersonalDirecto.Cells
                                                  where celda.Value?.ToString().Contains("#SUBDIRECTO") == true
                                                  select celda)
                                        .FirstOrDefault();
                        if (formatoCondicional != null)
                        {
                            int filaTotal = formatoCondicional.Start.Row;
                            var SaldoHoras = "$Q" + filaTotal;
                            var SaldoMonto = "$R" + filaTotal;
                            var cf = hojaPersonalDirecto.ConditionalFormatting.AddLessThan(hojaPersonalDirecto.Cells[SaldoHoras]);
                            cf.Formula = "0";
                            cf.Style.Fill.BackgroundColor.Color = Color.FromArgb(253, 195, 209);
                            cf.Style.Font.Color.Color = Color.FromArgb(158, 0, 0);
                            cf = hojaPersonalDirecto.ConditionalFormatting.AddLessThan(hojaPersonalDirecto.Cells[SaldoMonto]);
                            cf.Formula = "0";
                            cf.Style.Fill.BackgroundColor.Color = Color.FromArgb(253, 195, 209);
                            cf.Style.Font.Color.Color = Color.FromArgb(158, 0, 0);
                        }


                        formatoCondicional = (from celda in hojaPersonalDirecto.Cells
                                              where celda.Value?.ToString().Contains("#SUBUIO") == true
                                              select celda)
                                              .FirstOrDefault();
                        if (formatoCondicional != null)
                        {
                            int filaTotal = formatoCondicional.Start.Row;
                            var SaldoHoras = "$Q" + filaTotal;
                            var SaldoMonto = "$R" + filaTotal;
                            var cf = hojaPersonalDirecto.ConditionalFormatting.AddLessThan(hojaPersonalDirecto.Cells[SaldoHoras]);
                            cf.Formula = "0";
                            cf.Style.Fill.BackgroundColor.Color = Color.FromArgb(253, 195, 209);
                            cf.Style.Font.Color.Color = Color.FromArgb(158, 0, 0);
                            cf = hojaPersonalDirecto.ConditionalFormatting.AddLessThan(hojaPersonalDirecto.Cells[SaldoMonto]);
                            cf.Formula = "0";
                            cf.Style.Fill.BackgroundColor.Color = Color.FromArgb(253, 195, 209);
                            cf.Style.Font.Color.Color = Color.FromArgb(158, 0, 0);
                        }

                        formatoCondicional = (from celda in hojaPersonalDirecto.Cells
                                              where celda.Value?.ToString().Contains("#SUBOT") == true
                                              select celda)
                                              .FirstOrDefault();
                        if (formatoCondicional != null)
                        {
                            int filaTotal = formatoCondicional.Start.Row;
                            var SaldoHoras = "$Q" + filaTotal;
                            var SaldoMonto = "$R" + filaTotal;
                            var cf = hojaPersonalDirecto.ConditionalFormatting.AddLessThan(hojaPersonalDirecto.Cells[SaldoHoras]);
                            cf.Formula = "0";
                            cf.Style.Fill.BackgroundColor.Color = Color.FromArgb(253, 195, 209);
                            cf.Style.Font.Color.Color = Color.FromArgb(158, 0, 0);
                            cf = hojaPersonalDirecto.ConditionalFormatting.AddLessThan(hojaPersonalDirecto.Cells[SaldoMonto]);
                            cf.Formula = "0";
                            cf.Style.Fill.BackgroundColor.Color = Color.FromArgb(253, 195, 209);
                            cf.Style.Font.Color.Color = Color.FromArgb(158, 0, 0);
                        }

                        formatoCondicional = (from celda in hojaPersonalDirecto.Cells
                                              where celda.Value?.ToString().Contains("##SUBTOTALGENERAL") == true
                                              select celda)
                                              .FirstOrDefault();
                        if (formatoCondicional != null)
                        {
                            int filaTotal = formatoCondicional.Start.Row;
                            var SaldoHoras = "$Q" + filaTotal;
                            var SaldoMonto = "$R" + filaTotal;
                            var cf = hojaPersonalDirecto.ConditionalFormatting.AddLessThan(hojaPersonalDirecto.Cells[SaldoHoras]);
                            cf.Formula = "0";
                            cf.Style.Fill.BackgroundColor.Color = Color.FromArgb(253, 195, 209);
                            cf.Style.Font.Color.Color = Color.FromArgb(158, 0, 0);
                            cf = hojaPersonalDirecto.ConditionalFormatting.AddLessThan(hojaPersonalDirecto.Cells[SaldoMonto]);
                            cf.Formula = "0";
                            cf.Style.Fill.BackgroundColor.Color = Color.FromArgb(253, 195, 209);
                            cf.Style.Font.Color.Color = Color.FromArgb(158, 0, 0);
                        }



                        RowFinal = hojaPersonalDirecto.Dimension.End.Row;
                        hojaPersonalDirecto.Cells["A9"].Value = RowFinal; //Indicador Final por Secciones


                        hojaComentarios.Cells["B2:R18"].Copy(hojaPersonalDirecto.Cells["B" + RowFinal]); //Añade Seccion Subtotales a Hoja Principal

                        var rangoPocentajesComentarios = (from celda in hojaPersonalDirecto.Cells
                                                          where celda.Value?.ToString().Contains("#PORCENTAJES") == true
                                                          select celda)
                                                .FirstOrDefault();
                        if (rangoPocentajesComentarios != null)
                        {
                            int FilaTotalAvanceIngenieria = rangoPocentajesComentarios.Start.Row;
                            int FilaTotalPorcentajesAvanceIngenieria = rangoPocentajesComentarios.Start.Row + 1;
                            int FilaTotalPorcentajesAvanceIngenieriaAnterior = rangoPocentajesComentarios.Start.Row + 2;


                            var celdaPorcentaje = "F" + FilaTotalAvanceIngenieria;
                            var formulaTotal = "=+$O4";
                            hojaPersonalDirecto.Cells[celdaPorcentaje].Formula = formulaTotal;


                            celdaPorcentaje = "K" + FilaTotalPorcentajesAvanceIngenieria;
                            hojaPersonalDirecto.Cells[celdaPorcentaje].Value = porcentajeIBActual;

                            celdaPorcentaje = "M" + FilaTotalPorcentajesAvanceIngenieria;
                            hojaPersonalDirecto.Cells[celdaPorcentaje].Value = porcentajeIDActual;

                            celdaPorcentaje = "O" + FilaTotalPorcentajesAvanceIngenieria;
                            var formulaASbuilt = "=+$O$8";
                            hojaPersonalDirecto.Cells[celdaPorcentaje].Formula = formulaASbuilt;


                            celdaPorcentaje = "K" + FilaTotalPorcentajesAvanceIngenieriaAnterior;
                            hojaPersonalDirecto.Cells[celdaPorcentaje].Value = porcentajeIBAnterior;

                            celdaPorcentaje = "M" + FilaTotalPorcentajesAvanceIngenieriaAnterior;
                            hojaPersonalDirecto.Cells[celdaPorcentaje].Value = porcentajeIDAnterior;

                            celdaPorcentaje = "O" + FilaTotalPorcentajesAvanceIngenieriaAnterior;

                            hojaPersonalDirecto.Cells[celdaPorcentaje].Value = porcentajeAsbuiltAnterior;

                        }
                        var rangoNotaDos = (from celda in hojaPersonalDirecto.Cells
                                            where celda.Value?.ToString().Contains("#NOTA2") == true
                                            select celda)
                                               .FirstOrDefault();
                        if (rangoNotaDos != null)
                        {
                            var comentarioAvance = _comentarioCertificadoRepository.GetAll()
                                                                                 .Where(x => x.ProyectoId == c.ProyectoId)
                                                                                 .Where(x => x.FechaCarga <= grupoCertificado.FechaCertificado)

                                                                                 .FirstOrDefault();

                            if (comentarioAvance != null)
                            {
                                int FilaInicialNota = rangoNotaDos.Start.Row - 1;
                                int FilaFinalNota = FilaInicialNota + 2;
                                string rango = "C" + FilaInicialNota + ":Q" + FilaFinalNota;

                                hojaPersonalDirecto.Cells[rango].Merge = true;
                                hojaPersonalDirecto.Cells[rango].Style.WrapText = true;
                                // hojaPersonalDirecto.Cells[rango].Value = comentarioAvance.Comentario;
                                hojaPersonalDirecto.Cells[rango].Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                                hojaPersonalDirecto.Cells[rango].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            }



                        }

                        RowFinal = hojaPersonalDirecto.Dimension.End.Row;

                        for (int i = PosicionInicialPersonalDirecto; i < RowFinal; i++)
                        {
                            hojaPersonalDirecto.Row(i).Height = 20;

                        }
                        var rangoCorte = (from celda in hojaPersonalDirecto.Cells
                                          where celda.Value?.ToString().Contains("#CORTE") == true
                                          select celda)
                                          .FirstOrDefault();

                        /*if (rangoCorte != null) {
                            hojaPersonalDirecto.PrinterSettings.PrintArea = hojaPersonalDirecto.Cells[1, 2, hojaPersonalDirecto.Dimension.End.Row, hojaPersonalDirecto.Dimension.End.Column];
                            hojaPersonalDirecto.PrinterSettings.FitToPage = true;
                           // hojaPersonalDirecto.Row(rangoCorte.Start.Row).PageBreak = true;
                            hojaPersonalDirecto.PrinterSettings.FitToWidth = 1;
                            hojaPersonalDirecto.PrinterSettings.FitToHeight = 2;
                        }*/

                        var hojaCompleta = hojaPersonalDirecto;
                        hojaCompleta.PrinterSettings.PaperSize = ePaperSize.A4;
                        hojaPersonalDirecto.PrinterSettings.Orientation = eOrientation.Portrait;
                        hojaPersonalDirecto.PrinterSettings.PrintArea = hojaPersonalDirecto.Cells[1, 2, hojaPersonalDirecto.Dimension.End.Row, 18];
                        hojaPersonalDirecto.PrinterSettings.FitToWidth = 1;

                        if (rangoCorte != null)
                        {
                            hojaPersonalDirecto.Row(rangoCorte.Start.Row + 1).PageBreak = true;
                        }

                        //hojaPersonalDirecto.PrinterSettings.HorizontalCentered = true;
                        //hojaPersonalDirecto.PrinterSettings.FitToPage = true;

                        // hojaPersonalDirecto.PrinterSettings.FitToHeight = 20;
                       
                  

                    }


                }





            }
            return excel;
        }

        public int CrearGrupoCertificados(GrupoCertificadoIngenieria e, List<ProyectoDistribucionModel> distribucionProyectos)
        {
            /*var ExisteGrupoCertificadoFechaCerticado = Repository.GetAll()
                                                             .Where(x => x.EstadoId == EstadoGrupoCertificado.Aprobado)
                                                             .Where(x => x.FechaCertificado == e.FechaCertificado)
                                                             .FirstOrDefault();
            if (ExisteGrupoCertificadoFechaCerticado != null)
            {
                return -1;
            }*/
            e.EstadoId = EstadoGrupoCertificado.Aprobado;
            int mes = DateTime.Now.Month;
            int anio = DateTime.Now.Year;
            if (e.FechaInicio.Date != null)
            {
                var mesCertificacion = e.FechaCertificado.Month;
                var anioCertificacion = e.FechaCertificado.Year;

                var existeCertificadoMes = Repository.GetAll().Where(g => g.FechaInicio.Month == mesCertificacion)
                                                              .Where(g => g.FechaInicio.Year == anioCertificacion)
                                                              .FirstOrDefault();
                if (existeCertificadoMes != null)
                {
                    mes = mesCertificacion + 1;
                    anio = anioCertificacion;
                }
                else
                {

                    if (e.FechaInicio.Date.Day < 21)
                    {
                        mes = e.FechaInicio.Month;
                    }
                    else
                    {
                        mes = e.FechaInicio.Month + 1;
                    }



                    anio = anioCertificacion;
                }


            }

            e.Mes = mes;
            e.Anio = anio;

            var GrupoCertificadoId = Repository.InsertAndGetId(e);

            if (distribucionProyectos.Count > 0)
            {

                foreach (var d in distribucionProyectos)
                {
                    var entity = new DistribucionCertificadoIngenieria()
                    {

                        GrupoCertificadoId = GrupoCertificadoId,
                        ProyectoId = d.Id,
                        AplicaIndirecto = d.AplicaIndirecto,
                        AplicaViatico = d.AplicaViatico,
                        AplicaE500 = d.AplicaE500
                    };

                    _distribucionRepository.Insert(entity);



                }
            }


            return GrupoCertificadoId;
        }

        public bool CrearCertificadosDirectos(int GrupoCertificadoId, int ProyectoId, int[] Directos)
        {
            /* */
            ResumenCertificacion entityResumen = new ResumenCertificacion();
            entityResumen.Id = 0;
            entityResumen.GrupoCertificadoIngenieriaId = GrupoCertificadoId;
            entityResumen.ProyectoId = ProyectoId;

            var UbicacionporDefecto = _catalogoRepository.GetAll().Where(c => c.codigo == "UBICACION_UIO").FirstOrDefault();
            var estadoAprobado = _catalogoRepository.GetAll().Where(c => c.codigo == CatalogosCodigos.OFERTA_ESTADO_APROBADO).FirstOrDefault();
            var Unidad = _catalogoRepository.GetAll().Where(c => c.codigo == "TAREA_HOMBRE").FirstOrDefault();

            var e = Repository.GetAll().Where(c => c.Id == GrupoCertificadoId).FirstOrDefault();

            var entidadProyecto = _proyectoRepository.GetAll().Where(c => c.Id == ProyectoId).FirstOrDefault();

            var DirectosIds = Directos.Select(c => c).ToList();

            var DirectosIngenieraDto = this.DtoDetallesBusquedaRurboYSoloTarifaMigrado(DirectosIds);
            var ColaboradoresDirectosId = (from p in DirectosIngenieraDto select p.ColaboradorId).ToList().Distinct().ToList();

            var ColaboradoresRubro = _colaboradoRubroRepository.GetAll()
                                                    .Where(c => ColaboradoresDirectosId.Contains(c.ColaboradorId))
                                                    .ToList();





            var DirectosporProyecto = DirectosIngenieraDto;

            var numeroCertificado = this.NumeroCertificadoActualProyecto(ProyectoId);

            var avanceRealIngenieriaFechaCertificado = _avanceProyectoRepository.GetAll()
                                                                .Where(c => c.ProyectoId == ProyectoId)
                                                                .Where(c => c.FechaCertificado <= e.FechaCertificado)
                                                                .OrderByDescending(c => c.FechaCertificado)
                                                                .FirstOrDefault();

            double PorcentajeIB = 0.30; //Porcentaje Ingenieria Basica
            double PorcentajeID = 0.70;//Porcentaje Ingenieria Detalle


            /*Porcentaje_asbuilt	Decimal	Si	Tomado de valor carga complementaria ingeniería*/
            decimal PorcentajeAsbuilt = 0; //Pendiente


            decimal avanceRealIngenieria = Decimal.Parse("0");
            if (avanceRealIngenieriaFechaCertificado != null)
            {

                var valorIB = avanceRealIngenieriaFechaCertificado.AvanceRealActualIB * Convert.ToDecimal(PorcentajeIB);
                var valorID = avanceRealIngenieriaFechaCertificado.AvanceRealActualID * Convert.ToDecimal(PorcentajeID);
                PorcentajeAsbuilt = avanceRealIngenieriaFechaCertificado.AsbuiltActual;
                avanceRealIngenieria = (valorIB + valorID)  *Convert.ToDecimal(0.95)+ PorcentajeAsbuilt * Convert.ToDecimal(0.05);


               


                /*PARA_RESUMEN*/

                var valorIB_Previsto = avanceRealIngenieriaFechaCertificado.AvancePrevistoActualIB * Convert.ToDecimal(PorcentajeIB);
                var valorID_Previsto = avanceRealIngenieriaFechaCertificado.AvancePrevistoActualID * Convert.ToDecimal(PorcentajeID);
                var totalPrevisto = valorIB_Previsto + valorID_Previsto;

                /*PREVISTO RESUMEN*/
                entityResumen.TOTAL_PREVISTO = totalPrevisto;
                entityResumen.IB_PREVISTO = valorIB_Previsto;
                entityResumen.ID_PREVISTO = valorID_Previsto;

                /*REAL RESUMEN */
                entityResumen.TOTAL_REAL = avanceRealIngenieria;
                entityResumen.IB_PREVISTO = valorIB;
                entityResumen.ID_PREVISTO = valorID;
        

                /*AB REAL*/
                entityResumen.AB_REAL = PorcentajeAsbuilt;
                entityResumen.PORCENTAJE_AVANCE_FÍSICO_PREVISTO_IB_ID_AB = (entityResumen.TOTAL_PREVISTO * Convert.ToDecimal(0.85)) + (entityResumen.AB_REAL * Convert.ToDecimal(0.15));
                entityResumen.PORCENTAJE_AVANCE_FÍSICO_REAL_IB_ID_AB = (entityResumen.TOTAL_REAL * Convert.ToDecimal(0.85)) + (entityResumen.AB_REAL * Convert.ToDecimal(0.15));

                entityResumen.unaFase = avanceRealIngenieriaFechaCertificado.unaFase;


            }


            /*Horas Presupuestadas	Decimal	SI	Este valor se tomará del último presupuesto aprobado*/

            decimal HorasPresupuestadas = 0;


            /*var presupuestoPrincipal = _presupuestoRepository.GetAll().Where(p => p.vigente)
                                                              .Where(p => p.ProyectoId == ProyectoId)
                                                              .Where(p => p.es_final)
                                                              .Where(p => p.Requerimiento.tipo_requerimiento == TipoRequerimiento.Principal)
                                                              .OrderByDescending(p => p.Id)
                                                              .OrderByDescending(p => p.fecha_registro)
                                                              .FirstOrDefault();*/
           var presupuestos=_ofertaRepository.GetAll().Where(p=>p.vigente)
                                                      .Where(p=>p.OfertaComercial.estado_oferta== estadoAprobado.Id)
                                                      .Where(p=>p.OfertaComercial.vigente)
                                                      .Where(p=>p.PresupuestoId.HasValue)
                                                      .Where(p=>p.Presupuesto.es_final)
                                                      .Where(p=>p.Presupuesto.vigente)
                                                      .Where(p => p.Presupuesto.ProyectoId == ProyectoId)
                                                      .Select(p => p.PresupuestoId)
                                                     .ToList();

            var presupuestoPrincipal = _ofertaRepository.GetAll().Where(p => p.vigente)
                                                    .Where(p => p.OfertaComercial.estado_oferta == estadoAprobado.Id)
                                                    .Where(p => p.OfertaComercial.vigente)
                                                    .Where(p => p.PresupuestoId.HasValue)
                                                    .Where(p => p.Presupuesto.es_final)
                                                    .Where(p => p.Presupuesto.ProyectoId == ProyectoId)
                                                    .Where(p => p.Presupuesto.vigente)
                                                    .Where(p=>p.Requerimiento.tipo_requerimiento == TipoRequerimiento.Principal)
                                                    .OrderByDescending(p => p.Presupuesto.fecha_registro)
                                                    .Select(p=>p.Presupuesto)
                                                    .FirstOrDefault(); 


            /*var presupuestos = _presupuestoRepository.GetAll().Where(p => p.vigente)
                                                    .Where(p => p.ProyectoId == ProyectoId)
                                                    .Where(p => p.es_final)
                                                    .Select(p => p.Id)
                                                    .ToList();*/

            if (presupuestos.Count > 0)
            {

                decimal USD_BDG = 0;
                decimal HH_BDG = 0;

                /* EAC*/

                decimal EAC_HH = 0;
                decimal EAC_USD = 0;

                var computosIngenieria = _computoPresupuesto.GetAll().Where(c => c.vigente)
                                                                   .Where(c => c.WbsPresupuesto.vigente)
                                                                  //  .Where(c => c.WbsPresupuesto.PresupuestoId == presupuesto.Id)
                                                                  .Where(c => presupuestos.Contains(c.WbsPresupuesto.PresupuestoId))
                                                                   .Where(c => c.Item.Grupo.codigo == ProyectoCodigos.CODE_INGENIERIA)
                                                                   .Where(c => c.WbsPresupuesto.Presupuesto.ProyectoId == ProyectoId)
                                                                   .Where(c => c.Item.codigo.StartsWith("1.1") || c.Item.codigo.StartsWith("1.2"))
                                                                   .ToList();

                if (computosIngenieria.Count > 0)
                {

                    HorasPresupuestadas = computosIngenieria.Select(c => c.cantidad).Sum(); //HH Presupuestadas , Suma de Cantidades


                    /*PARA_RESUMEN*/
                    HH_BDG = HorasPresupuestadas;
                    USD_BDG = computosIngenieria.Select(c => c.costo_total).Sum();
                    EAC_HH = computosIngenieria.Select(c => c.cantidad_eac).Sum(); //OJO
                    EAC_USD = computosIngenieria.Select(c => c.cantidad_eac * c.precio_unitario).Sum();//OJO

                }

                /*PARA_RESUMEN*/
                string clase = "";
                if (presupuestoPrincipal != null)
                {
                    if (presupuestoPrincipal.Clase.HasValue)
                    {
                        clase = Enum.GetName(typeof(Presupuesto.ClasePresupuesto), presupuestoPrincipal.Clase.Value);
                    }

                    var ofertaPresupuesto = _ofertaRepository.GetAllIncluding(c => c.OfertaComercial).Where(c => c.PresupuestoId.HasValue).Where(c => c.PresupuestoId == presupuestoPrincipal.Id)
                        .FirstOrDefault();
                    if (ofertaPresupuesto != null)
                    {
                        entityResumen.N_Oferta = ofertaPresupuesto.OfertaComercial.codigo + "_" + ofertaPresupuesto.OfertaComercial.version;

                    }

                }

                entityResumen.CLASE = clase;


                entityResumen.HH_BDG = HH_BDG;
                entityResumen.USD_BDG = USD_BDG;
                entityResumen.EAC_HH = EAC_HH;
                entityResumen.EAC_USD = EAC_USD;




            }



            /*Monto_anterior_certificado Decimal Si Toma el monto actual del último certificado aprobado generado del proyecto específico*/

            decimal MontoAnteriorCertificado = 0;
            decimal HorasAnteriorCertificadas = 0;

            decimal MontoActualCertificado = 0;
            decimal HorasActualCertificadas = 0;

            var CertificadoProyectoAnterior = _certificadoIngenieriaProyectoRepository.GetAll()
                                                                                  .Where(c => c.ProyectoId == ProyectoId)
                                                                                  .Where(c => c.EstadoId == EstadoCertificadoProyecto.Aprobado)
                                                                                  .Where(c => c.GrupoCertificadoIngenieria.FechaFin < e.FechaInicio)
                                                                                  .Where(c => c.GrupoCertificadoIngenieriaId != GrupoCertificadoId)
                                                                                  .OrderByDescending(c => c.GrupoCertificadoIngenieria.FechaFin)
                                                                                  .FirstOrDefault();

            if (CertificadoProyectoAnterior != null)
            {
                MontoAnteriorCertificado = CertificadoProyectoAnterior.MontoActualCertificado;
                HorasAnteriorCertificadas = CertificadoProyectoAnterior.HorasActualCertificadas;
            }

            //Monto_actual_certificado	Decimal	
            /*Toma el monto actual del certificado en generación es decir la sumatoria de los rubros incluidos (directos / indirectos / viáticos)*/

            decimal porcentajeParticipacionporProyecto = 0;

            var DirectosParaActualizarCampoCertificadoId = (from d in DirectosporProyecto select d.Id).ToList();

            decimal montoTotalDirectos = 0;
            var montoTotalDirectosProyecto = (from d in DirectosporProyecto select d).ToList();
            if (montoTotalDirectosProyecto.Count > 0)
            {
                montoTotalDirectos = montoTotalDirectosProyecto.Sum(c => c.monto);
            }

            decimal montoTotalIndirectos = 0;



            /*Suma Montos Directos e Indirectos*/
            MontoActualCertificado = montoTotalDirectos + montoTotalIndirectos;


            /*  Horas_actual_certificadas Decimal     Toma las horas totales de timesheet cargadas a certificarse en el periodo seleccionado*/

            decimal HorasTotalesDirectos = (from h in DirectosporProyecto select h.NumeroHoras).ToList().Sum();



            // decimal HorasTotalesIndirectos = (from hi in detallesIndirectosIngenieria select hi.HorasLaboradas).ToList().Sum();

            HorasActualCertificadas = HorasTotalesDirectos; //+ HorasTotalesIndirectos;




            var certificadoIngenieriaProyecto = new CertificadoIngenieriaProyecto()
            {
                Id = 0,
                GrupoCertificadoIngenieriaId = GrupoCertificadoId,
                ProyectoId = ProyectoId,
                EstadoId = EstadoCertificadoProyecto.Aprobado,
                NumeroCertificado = numeroCertificado,
                OrdenServicioId = null,
                AvanceRealIngenieria = avanceRealIngenieria,
                HorasPresupuestadas = HorasPresupuestadas,
                PorcentajeAsbuilt = PorcentajeAsbuilt,
                MontoAnteriorCertificado = MontoAnteriorCertificado,
                HorasAnteriorCertificadas = HorasAnteriorCertificadas,
                MontoActualCertificado = MontoActualCertificado,
                HorasActualCertificadas = HorasActualCertificadas,
                DistribucionDirectos = false,
                PorcentajeParticipacionDirectos = porcentajeParticipacionporProyecto
            };


            //Inser Certificado por Proyecto
            int CertificadoProyectoId = _certificadoIngenieriaProyectoRepository.InsertAndGetId(certificadoIngenieriaProyecto);
            _resumenCertificacion.Insert(entityResumen);


            if (CertificadoProyectoId <= 0)
            {
                return false;
            }


            /*Gastos Directos Tabla Hija Certificado por Certificado Id  Y Proyecto*/

            foreach (var directo in DirectosporProyecto)
            {
                var directoEntity = _directosRepository.Get(directo.Id);

                //  ElmahExtension.LogToElmah(new Exception("directoId :" + directo.Id));
                /*  var ColaboradorRubro = _colaboradoRubroRepository.GetAll()
                                                             .Where(c => c.ColaboradorId == directo.ColaboradorId)
                                                             .Where(c => c.ContratoId == entidadProyecto.contratoId)
                                                             .FirstOrDefault();*/
                var fechaTrabajoDate = directo.FechaTrabajo.Date;

                var ColaboradorRubro = (from cr in ColaboradoresRubro
                                        where cr.ColaboradorId == directo.ColaboradorId
                                        where fechaTrabajoDate >= cr.FechaInicio
                                        where fechaTrabajoDate <= cr.FechaFin
                                        where cr.ContratoId == entidadProyecto.contratoId
                                        select cr)
                                                          .FirstOrDefault();

                var grupoCertificado = Repository.GetAll().Where(c => c.Id == GrupoCertificadoId).FirstOrDefault();



                /*var ColaboradorCertificadoIngenieria = _colaboradoCertificacionRepository.GetAllIncluding(c => c.Ubicacion)

                                                                                      .Where(c => c.ColaboradorId == directo.ColaboradorId)
                                                                                            .Where(c => fechaTrabajoDate >= c.FechaDesde)
                                                                                            .Where(c => fechaTrabajoDate <= c.FechaHasta)
                                                                                      .FirstOrDefault();*/







              
                var GastoDirecto = new GastoDirectoCertificado()
                {
                    Id = 0,
                    CertificadoIngenieriaProyectoId = CertificadoProyectoId,
                    TipoGastoId = TipoGasto.Directo,
                    ColaboradorId = directo.ColaboradorId,
                    //    RubroId = ColaboradorRubro!=null? ColaboradorRubro.RubroId:null,
                    UnidadId = Unidad != null ? Unidad.Id : 0,
                    TotalHoras = directo.NumeroHoras,
                    TarifaHoras = directo.monto,
                    Tarifa = directo.tarifa,

                    EsDistribucionE500 = false,
                    migrado = directo.migrado,
                    EspecialidadId = directo.EspecialidadId,
                    NombreEspecialidad = directo.EspecialidadId.HasValue ? directo.Especialidad.codigo : "",
                    NombreEtapa = directo.EtapaId.HasValue ? directo.Etapa.codigo : "",
                    /*   UbicacionId = ColaboradorCertificadoIngenieria != null ? ColaboradorCertificadoIngenieria.Ubicacion.Id : UbicacionporDefecto != null ? UbicacionporDefecto.Id : 0,
                       UbicacionTrabajo = ColaboradorCertificadoIngenieria != null ? ColaboradorCertificadoIngenieria.Ubicacion.codigo : "",
                       */

                    UbicacionId = directo.LocacionId.HasValue ? _catalogoRepository.GetAll().Where(c => c.Id == directo.LocacionId).FirstOrDefault().Id : UbicacionporDefecto != null ? UbicacionporDefecto.Id : 0,
                    UbicacionTrabajo = directo.LocacionId.HasValue ? _catalogoRepository.GetAll().Where(c => c.Id == directo.LocacionId).FirstOrDefault().codigo : "",

                };
                if (ColaboradorRubro != null)
                {
                    GastoDirecto.RubroId = ColaboradorRubro.RubroId;
                }
                _gastoDirectoRepository.Insert(GastoDirecto);
                 
                directoEntity.CertificadoId = CertificadoProyectoId; //ACTUALIZA CAMPO CERTIFICADO
            }



            /*Actualizar Campo CertificadoId*/

            /*foreach (var DirectoId in DirectosParaActualizarCampoCertificadoId)

            {
                var directo = _directosRepository.Get(DirectoId);
                directo.CertificadoId = CertificadoProyectoId;
                _directosRepository.Update(directo);

            }*/
            return true;
        }



        public bool AñadirDistribucionIndirectos(int GrupoCertificadoId, int[] Indirectos, List<ProyectoDistribucionModel> distribucionProyectos)
        {
            var grupoCertificado = Repository.GetAll().Where(c => c.Id == GrupoCertificadoId).FirstOrDefault();
            string fileName = System.Web.HttpContext.Current.Server.MapPath("~/Views/PlantillaWord/CertificacionIngenieria/DistribucionIndirectos/DistribucionIndirectosPrincipal"
          + DateTime.Now.Year + "" + DateTime.Now.Month + "" + DateTime.Now.Minute + "" + DateTime.Now.Millisecond + "" + ".txt");
            using (StreamWriter sw = File.CreateText(fileName))
            {

                var Unidad = _catalogoRepository.GetAll().Where(c => c.codigo == "TAREA_HOMBRE").FirstOrDefault();
                var UbicacionporDefecto = _catalogoRepository.GetAll().Where(c => c.codigo == "UBICACION_UIO").FirstOrDefault();
                var IndirectosIds = Indirectos.Select(c => c).ToList();
                var detallesIndirectosIngenieria = _indirectosRepository.GetAllIncluding(c => c.ColaboradorRubro.Colaborador)
                                                                       .Where(c => IndirectosIds.Contains(c.Id))
                                                                       .ToList();
                var ProyectosAplicanIndirectosId = (from d in distribucionProyectos where d.AplicaIndirecto select d.Id).ToList();// Solo los check aplicaIndirecto

                var CertificadosPorProyectos = _certificadoIngenieriaProyectoRepository
                                                                                       .GetAllIncluding(c => c.Proyecto)
                                                                                        //  .Where(c => c.Proyecto.certificable_ingenieria)
                                                                                        .Where(c => ProyectosAplicanIndirectosId.Contains(c.ProyectoId))
                                                                                        .Where(c => c.GrupoCertificadoIngenieriaId == GrupoCertificadoId)
                                                                                       .ToList();
                var ProyectosDirectosId = (from cert in CertificadosPorProyectos
                                               //  where cert.Proyecto.certificable_ingenieria //Se excluyen los proyectos no certificables
                                           select cert.ProyectoId)
                                           .ToList()
                                           .Distinct()
                                           .ToList();

                decimal HorasTotalesCertificado = 0;

                if (CertificadosPorProyectos.Count > 0)
                {
                    HorasTotalesCertificado = CertificadosPorProyectos.Select(c => c.HorasActualCertificadas).Sum();
                }

                sw.WriteLine("TotalIncluidoE500\t" + HorasTotalesCertificado);


                //DISTRIBUCION INDIRECOTS A PROYECTOS
                foreach (var indirecto in detallesIndirectosIngenieria)  // Recoger indirecto
                {
                    var ColaboradorCertificadoIngenieria = _colaboradoCertificacionRepository.GetAllIncluding(c => c.Ubicacion)

                                                                                           .Where(c => c.ColaboradorId == indirecto.ColaboradorRubro.ColaboradorId)
                                                                                              .Where(c => grupoCertificado.FechaInicio >= c.FechaDesde)
                                                                                            .Where(c => grupoCertificado.FechaFin <= c.FechaHasta)
                                                                                           .FirstOrDefault();
                    //**/

                    if (indirecto.ColaboradorRubro.Colaborador.numero_identificacion != "1600195448")
                    {

                        foreach (var ProyectoId in ProyectosDirectosId) // proyecto a los que voy a distribuit
                        {
                            var certificado = (from cert in CertificadosPorProyectos
                                               where cert.ProyectoId == ProyectoId
                                               select cert).FirstOrDefault();
                            if (certificado != null)
                            {
                                var porcentajeProyecto = HorasTotalesCertificado > 0 ? (certificado.HorasActualCertificadas) / HorasTotalesCertificado : 0;

                                var entidadProyecto = _proyectoRepository.GetAll().Where(c => c.Id == ProyectoId).Where(c => c.vigente).FirstOrDefault();


                                decimal HorasPorcentuales = (indirecto.HorasLaboradas * indirecto.DiasLaborados) * porcentajeProyecto; //Horas por el Porcentaje Correspondiente al proyecto
                                var montoUSD = HorasPorcentuales * indirecto.ColaboradorRubro.Tarifa; //USD Porcentaje de Horas por Tarifa


                                sw.WriteLine(entidadProyecto.codigo + "\t" + certificado.HorasActualCertificadas + "\t" + porcentajeProyecto);

                                var indirectoEntity = new GastoDirectoCertificado()
                                {
                                    Id = 0,
                                    CertificadoIngenieriaProyectoId = certificado.Id,
                                    TipoGastoId = TipoGasto.Indirecto,
                                    ColaboradorId = indirecto.ColaboradorRubro.ColaboradorId,
                                    RubroId = indirecto.ColaboradorRubro.RubroId,
                                    UnidadId = Unidad != null ? Unidad.Id : 0,
                                    TotalHoras = HorasPorcentuales,
                                    TarifaHoras = montoUSD,
                                    Tarifa = indirecto.ColaboradorRubro.Tarifa,
                                    EsDistribucionE500 = false,
                                    migrado = false,
                                    Area = ColaboradorCertificadoIngenieria != null ? Enum.GetName(typeof(Area), ColaboradorCertificadoIngenieria.AreaId) : "",
                                    UbicacionId = ColaboradorCertificadoIngenieria != null ? ColaboradorCertificadoIngenieria.Ubicacion.Id : UbicacionporDefecto != null ? UbicacionporDefecto.Id : 0,
                                    UbicacionTrabajo = ColaboradorCertificadoIngenieria != null ? ColaboradorCertificadoIngenieria.Ubicacion.codigo : "",
                                    //EsViatico = indirecto.EsViatico,
                                    AplicaViatico = ColaboradorCertificadoIngenieria != null ? ColaboradorCertificadoIngenieria.AplicaViatico : false,
                                };

                                _gastoDirectoRepository.Insert(indirectoEntity);

                                var entity = _indirectosRepository.Get(indirecto.Id);
                                entity.CertificadoId = GrupoCertificadoId;
                                _indirectosRepository.Update(entity);
                            }
                        }
                    }
                    else
                    {
                        foreach (var ProyectoId in ProyectosDirectosId) // proyecto a los que voy a distribuit
                        {
                            var certificado = (from cert in CertificadosPorProyectos
                                               where cert.ProyectoId == ProyectoId
                                               select cert).FirstOrDefault();

                            var totalProyectosDivision = 0;

                            var existeProyecto52 = (from cert in CertificadosPorProyectos
                                                    where cert.Proyecto.codigo == "FC5.2"
                                                    select cert).FirstOrDefault();
                            var existeProyecto54 = (from cert in CertificadosPorProyectos
                                                    where cert.Proyecto.codigo == "FC5.4"
                                                    select cert).FirstOrDefault();
                            // if (existeProyecto52!=null) {
                            //   totalProyectosDivision++;
                            //}
                            if (existeProyecto54 != null)
                            {
                                totalProyectosDivision++;
                            }

                            if (certificado != null)
                            {

                                var entidadProyecto = _proyectoRepository.GetAll().Where(c => c.Id == ProyectoId).Where(c => c.vigente).FirstOrDefault();
                                //
                                // if (entidadProyecto.codigo == "FC5.2" || entidadProyecto.codigo == "FC5.4")
                                //   {
                                if (entidadProyecto.codigo == "FC5.4")
                                {

                                    decimal HorasPorcentuales = totalProyectosDivision == 0 ? 0 : (indirecto.HorasLaboradas * indirecto.DiasLaborados) / totalProyectosDivision; //Horas por el Porcentaje Correspondiente al proyecto
                                    var montoUSD = HorasPorcentuales * indirecto.ColaboradorRubro.Tarifa; //USD Porcentaje de Horas por Tarifa

                                    var indirectoEntity = new GastoDirectoCertificado()
                                    {
                                        Id = 0,
                                        CertificadoIngenieriaProyectoId = certificado.Id,
                                        TipoGastoId = TipoGasto.Indirecto,
                                        ColaboradorId = indirecto.ColaboradorRubro.ColaboradorId,
                                        RubroId = indirecto.ColaboradorRubro.RubroId,
                                        UnidadId = Unidad != null ? Unidad.Id : 0,
                                        TotalHoras = HorasPorcentuales,
                                        TarifaHoras = montoUSD,
                                        Tarifa = indirecto.ColaboradorRubro.Tarifa,
                                        EsDistribucionE500 = false,
                                        migrado = false,
                                        Area = ColaboradorCertificadoIngenieria != null ? Enum.GetName(typeof(Area), ColaboradorCertificadoIngenieria.AreaId) : "",
                                        UbicacionId = ColaboradorCertificadoIngenieria != null ? ColaboradorCertificadoIngenieria.Ubicacion.Id : UbicacionporDefecto != null ? UbicacionporDefecto.Id : 0,
                                        UbicacionTrabajo = ColaboradorCertificadoIngenieria != null ? ColaboradorCertificadoIngenieria.Ubicacion.codigo : UbicacionporDefecto != null ? UbicacionporDefecto.codigo : "",
                                        EsViatico = indirecto.EsViatico,
                                        AplicaViatico = ColaboradorCertificadoIngenieria != null ? ColaboradorCertificadoIngenieria.AplicaViatico : false,
                                    };

                                    _gastoDirectoRepository.Insert(indirectoEntity);

                                    var entity = _indirectosRepository.Get(indirecto.Id);
                                    entity.CertificadoId = GrupoCertificadoId;
                                    _indirectosRepository.Update(entity);
                                }
                            }
                        }
                    }



                }
            }



            return true;
        }

        public bool AñadirDistribucionE500(int GrupoCertificadoId, int[] E500, List<ProyectoDistribucionModel> distribucionProyectos)
        {

            string fileName = System.Web.HttpContext.Current.Server.MapPath("~/Views/PlantillaWord/CertificacionIngenieria/DistribucionIndirectos/DistribucionE500Principal"
           + DateTime.Now.Year + "" + DateTime.Now.Month + "" + DateTime.Now.Minute + "" + DateTime.Now.Millisecond + "" + ".txt");
            using (StreamWriter sw = File.CreateText(fileName))
            {

                var UbicacionporDefecto = _catalogoRepository.GetAll().Where(c => c.codigo == "UBICACION_UIO").FirstOrDefault();
                var Unidad = _catalogoRepository.GetAll().Where(c => c.codigo == "TAREA_HOMBRE").FirstOrDefault();
                var detallesE500 = _directoE50.GetAllIncluding(c => c.Especialidad, c => c.Etapa).Where(c => E500.Contains(c.Id)).ToList();


                var ProyectosAplicanE500Id = (from d in distribucionProyectos where d.AplicaE500 select d.Id).ToList();// Solo los check aplican E500

                var CertificadosPorProyectos = _certificadoIngenieriaProyectoRepository.GetAllIncluding(c => c.Proyecto)
                                                                                      .Where(c => c.GrupoCertificadoIngenieriaId == GrupoCertificadoId)
                                                                                      .Where(c => ProyectosAplicanE500Id.Contains(c.ProyectoId))
                                                                                      .ToList();
                var ProyectosDirectosId = (from cert in CertificadosPorProyectos
                                           select cert.ProyectoId).ToList().Distinct().ToList();

                // decimal HorasTotalesCertificado = 0;

                decimal MontoTotalCertificado = 0;

                if (CertificadosPorProyectos.Count > 0)
                {
                    // HorasTotalesCertificado = CertificadosPorProyectos.Select(c => c.HorasActualCertificadas).Sum();
                    MontoTotalCertificado = CertificadosPorProyectos.Select(c => c.MontoActualCertificado).Sum();
                }
                sw.WriteLine("MontoTotalDirectos\t" + MontoTotalCertificado);

                decimal TotalE500 = 0;
                //DISTRIBUCION DIRECTOS E500 A CERTIFICADOS
                foreach (var directoE500 in detallesE500)
                {
                    foreach (var ProyectoId in ProyectosDirectosId)
                    {
                        var certificado = (from cert in CertificadosPorProyectos
                                           where cert.ProyectoId == ProyectoId
                                           select cert).FirstOrDefault();
                        if (certificado != null)
                        {

                            var porcentajeProyecto = (certificado.MontoActualCertificado) / MontoTotalCertificado;

                            //var entidadProyecto = _proyectoRepository.GetAll().Where(c => c.Id == ProyectoId).Where(c => c.vigente).FirstOrDefault();

                            sw.WriteLine(certificado.Proyecto.codigo + "\t" + certificado.MontoActualCertificado + "\t" + porcentajeProyecto);

                            decimal HorasPorcentuales = directoE500.NumeroHoras * porcentajeProyecto; //Horas por el Porcentaje Correspondiente al proyecto

                            TotalE500 = TotalE500 + HorasPorcentuales;

                            var fechaDate = directoE500.FechaTrabajo.Date;
                            var ColaboradorRubro = _colaboradoRubroRepository.GetAll()
                                                                          .Where(c => c.ColaboradorId == directoE500.ColaboradorId)
                                                                          .Where(c => fechaDate >= c.FechaInicio)
                                                                          .Where(c => fechaDate <= c.FechaFin)
                                                                          .Where(c => c.ContratoId == certificado.Proyecto.contratoId)
                                                                          .FirstOrDefault();

                            var GastoDirectoE500 = new GastoDirectoCertificado()
                            {
                                Id = 0,
                                CertificadoIngenieriaProyectoId = certificado.Id,
                                TipoGastoId = TipoGasto.Directo,
                                ColaboradorId = directoE500.ColaboradorId,
                                UnidadId = Unidad != null ? Unidad.Id : 0,
                                TotalHoras = HorasPorcentuales,
                                EsDistribucionE500 = true,
                                EspecialidadId = directoE500.EspecialidadId,
                                NombreEspecialidad = directoE500.EspecialidadId.HasValue ? directoE500.Especialidad.codigo : "",
                                NombreEtapa = directoE500.EtapaId.HasValue ? directoE500.Etapa.codigo : "",
                                UbicacionId = directoE500.LocacionId.HasValue ? _catalogoRepository.GetAll().Where(c => c.Id == directoE500.LocacionId).FirstOrDefault().Id : UbicacionporDefecto != null ? UbicacionporDefecto.Id : 0,
                                UbicacionTrabajo = directoE500.LocacionId.HasValue ? _catalogoRepository.GetAll().Where(c => c.Id == directoE500.LocacionId).FirstOrDefault().codigo : "",
                            };


                            if (ColaboradorRubro != null)
                            {
                                var montoUSD = HorasPorcentuales * ColaboradorRubro.Tarifa; //USD Porcentaje de Horas por Tarifa

                                GastoDirectoE500.RubroId = ColaboradorRubro.RubroId;
                                GastoDirectoE500.TarifaHoras = montoUSD;
                                GastoDirectoE500.Tarifa = ColaboradorRubro.Tarifa;
                            }


                            _gastoDirectoRepository.Insert(GastoDirectoE500);

                            var entity = _directoE50.Get(directoE500.Id);
                            entity.CertificadoId = GrupoCertificadoId;
                            //_directoE50.Update(entity);
                        }
                    }

                }
                sw.WriteLine("TotalE500\t" + TotalE500);
            }

            return true;

        }

        public void ActualizarCabeceras(int GrupoCertificadoId)
        {
            var certificadosProyectos = _certificadoIngenieriaProyectoRepository.GetAll().Where(c => c.GrupoCertificadoIngenieriaId == GrupoCertificadoId).ToList();
            foreach (var certificado in certificadosProyectos)
            {
                var detalles = _gastoDirectoRepository.GetAll().Where(c => c.CertificadoIngenieriaProyectoId == certificado.Id).ToList();

                if (detalles.Count > 0)
                {

                    var entity = _certificadoIngenieriaProyectoRepository.Get(certificado.Id);
                    entity.HorasActualCertificadas = detalles.Where(c => !c.EsViatico).Select(c => c.TotalHoras).Sum();
                    entity.MontoActualCertificado = detalles.Select(c => c.TarifaHoras).Sum();

                    _certificadoIngenieriaProyectoRepository.Update(entity);
                }

            }
        }

        public void ViaticosVersion2(int GrupoCertificadoId, List<ProyectoDistribucionModel> distribucionProyectos)
        {
            var grupoCertificado = Repository.GetAll().Where(c => c.Id == GrupoCertificadoId).FirstOrDefault();
            var FechaMigracion = new DateTime(2021, 11, 20);
            if (grupoCertificado.FechaCertificado > FechaMigracion.Date)
            {

                string fileName = System.Web.HttpContext.Current.Server.MapPath("~/Views/PlantillaWord/CertificacionIngenieria/DistribucionIndirectos/PrimeraRonda"
          + DateTime.Now.Year + "" + DateTime.Now.Month + "" + DateTime.Now.Minute + "" + DateTime.Now.Millisecond + "" + ".txt");

                string fileName2 = System.Web.HttpContext.Current.Server.MapPath("~/Views/PlantillaWord/CertificacionIngenieria/DistribucionIndirectos/Porcentajes"
 + DateTime.Now.Year + "" + DateTime.Now.Month + "" + DateTime.Now.Minute + "" + DateTime.Now.Millisecond + "" + ".txt");


                #region
                var Unidad = _catalogoRepository.GetAll().Where(c => c.codigo == "TAREA_HOMBRE").FirstOrDefault();
                var UbicacionporDefecto = _catalogoRepository.GetAll().Where(c => c.codigo == "UBICACION_CAMPO").FirstOrDefault();

                var ProyectosAplicanViaticosId = (from d in distribucionProyectos where d.AplicaViatico select d.Id).ToList();

                var CertificadosPorProyectos = _certificadoIngenieriaProyectoRepository.GetAllIncluding(c => c.Proyecto)

                                                                                      //.Where(c => c.Proyecto.certificable_ingenieria) //Solo de proyectos Certificables

                                                                                      .Where(p => ProyectosAplicanViaticosId.Contains(p.ProyectoId)) //Solo Check viaticos
                                                                                      .Where(c => c.GrupoCertificadoIngenieriaId == GrupoCertificadoId)
                                                                                      .OrderBy(c => c.Proyecto.codigo)
                                                                                       .ToList();

                var ProyectosId = (from cert in CertificadosPorProyectos select cert.ProyectoId).ToList().Distinct().ToList();

                #endregion
                using (StreamWriter sw = File.CreateText(fileName))
                {
                    using (StreamWriter sp = File.CreateText(fileName2))
                    {

                        var ColaboradoresEnCampo = _gastoDirectoRepository.GetAll().Where(c => c.CertificadoIngenieriaProyecto.GrupoCertificadoIngenieriaId == GrupoCertificadoId)
                                                                                 .Where(c => c.UbicacionId.HasValue)
                                                                                 .Where(c => c.TipoGastoId == TipoGasto.Directo)
                                                                                 .Where(c => c.UbicacionTrabajo == CertificacionIngenieriaCodigos.UBICACION_CAMPO)
                                                                                 .Select(c => c.ColaboradorId)

                                                                                 .ToList();

                        sp.WriteLine("ColaboradoresEnCampo", String.Join(",", ColaboradoresEnCampo.ToArray()));


                        var ColaboradoresCertificados = _gastoDirectoRepository.GetAll()
                                                                                 .Where(c => c.CertificadoIngenieriaProyecto.GrupoCertificadoIngenieriaId == GrupoCertificadoId)
                                                                                 .Select(c => c.ColaboradorId).ToList().Distinct().ToList();

                        // SE OBTIENE COLABORADORES QUE CONTENGA APLICA VIATIO SI
                        /*  var ColaboradoresAplicaViatico = _colaboradoCertificacionRepository.GetAll().Where(c => c.AplicaViatico)
                                                                                              .Where(c => grupoCertificado.FechaInicio >= c.FechaDesde)
                                                                                                  .Where(c => grupoCertificado.FechaFin <= c.FechaHasta)
                                                                                                      .Select(c => c.ColaboradorId)
                                                                                                      .ToList().Distinct().ToList();*/

                        var ColaboradoresAplicaViatico = _colaboradoCertificacionRepository.GetAll().Where(c => c.AplicaViatico)
                                                                                                .Where(c => ColaboradoresCertificados.Contains(c.ColaboradorId))
                                                                                                    .Select(c => c.ColaboradorId)
                                                                                                    .ToList().Distinct().ToList();

                        //COLABORADORES TELETRABAJO y APLICA VIATICO
                        /* var ColaboradoresHomeOffice = _colaboradoCertificacionRepository.GetAllIncluding(e => e.Modalidad)
                                                                              .Where(c => c.Modalidad.codigo == CertificacionIngenieriaCodigos.MODALIDAD_HOMEOFFCIE)
                                                                              .Where(c => c.AplicaViatico)
                                                                          .Where(c => grupoCertificado.FechaInicio >= c.FechaDesde)
                                                                                                 .Where(c => grupoCertificado.FechaFin <= c.FechaHasta)
                                                                              .Select(c => c.ColaboradorId)
                                                                              .ToList().Distinct().ToList();*/




                        //COLABORADORES  y APLICA VIATICO
                        /*var ColaboradoresDescuentoViaticos = _colaboradoCertificacionRepository.GetAllIncluding(e => e.Modalidad)
                                                                                                    .Where(c => c.AplicaViaticoDirecto)
                                                                                               .Where(c => grupoCertificado.FechaInicio >= c.FechaDesde)
                                                                                                .Where(c => grupoCertificado.FechaFin <= c.FechaHasta)
                                                                            .Select(c => c.ColaboradorId)
                                                                             .ToList().Distinct().ToList();*/

                        var ColaboradoresDescuentoViaticos = _colaboradoCertificacionRepository.GetAllIncluding(e => e.Modalidad)
                                                                                                 .Where(c => c.AplicaViaticoDirecto)
                                                                                            .Where(c => ColaboradoresCertificados.Contains(c.ColaboradorId))

                                                                         .Select(c => c.ColaboradorId)
                                                                          .ToList().Distinct().ToList();

                        // MONTO TOTAL DIRECTOS SOLO DE CAMPO 
                        var MontoTotalDirectosSinE500 = _gastoDirectoRepository.GetAll().Where(c => c.TipoGastoId == TipoGasto.Directo)
                                                                          .Where(c => !c.EsDistribucionE500)
                                                                          .Where(c => c.CertificadoIngenieriaProyecto.GrupoCertificadoIngenieriaId == GrupoCertificadoId)
                                                                          .Where(p => ProyectosAplicanViaticosId.Contains(p.CertificadoIngenieriaProyecto.ProyectoId)) //Check VIaticos


                                                                          // .Where(c => c.CertificadoIngenieriaProyecto.Proyecto.certificable_ingenieria) //Solo Certificables
                                                                          .Where(c => c.UbicacionTrabajo == CertificacionIngenieriaCodigos.UBICACION_CAMPO)
                                                                          .Select(c => c.TarifaHoras).ToList().Sum();



                        //Total E500 Campo
                        var TotalDirectosE500Campo = _gastoDirectoRepository.GetAll().Where(c => c.TipoGastoId == TipoGasto.Directo)
                                                                    .Where(c => c.EsDistribucionE500)
                                                                    .Where(c => c.CertificadoIngenieriaProyecto.GrupoCertificadoIngenieriaId == GrupoCertificadoId)

                                                                    .Where(c => c.UbicacionTrabajo == CertificacionIngenieriaCodigos.UBICACION_CAMPO)
                                                                    .Select(c => c.TotalHoras).ToList().Sum();

                        //Total Horas Quito y Ot
                        var TotalHorasDirectosQUIT_OOT = _gastoDirectoRepository.GetAll().Where(c => c.TipoGastoId == TipoGasto.Directo)

                                                                              .Where(c => c.CertificadoIngenieriaProyecto.GrupoCertificadoIngenieriaId == GrupoCertificadoId)
                                                                               .Where(p => ProyectosAplicanViaticosId.Contains(p.CertificadoIngenieriaProyecto.ProyectoId)) //Check VIaticos


                                                                              //   .Where(c => c.CertificadoIngenieriaProyecto.Proyecto.certificable_ingenieria) //Solo Certificables
                                                                              .Select(c => c.TotalHoras).ToList().Sum();


                        var TotalMontoDirectosE500Campo = _gastoDirectoRepository.GetAll().Where(c => c.TipoGastoId == TipoGasto.Directo)
                                                         .Where(c => c.EsDistribucionE500)

                                                         .Where(c => c.CertificadoIngenieriaProyecto.GrupoCertificadoIngenieriaId == GrupoCertificadoId)

                                                                 .Where(c => c.UbicacionTrabajo == CertificacionIngenieriaCodigos.UBICACION_CAMPO)
                                                         .Select(c => c.TarifaHoras).ToList().Sum();



                        var TotalHorasDirectosE500Campo = _gastoDirectoRepository.GetAll().Where(c => c.TipoGastoId == TipoGasto.Directo)
                                                      .Where(c => c.EsDistribucionE500)

                                                      .Where(c => c.CertificadoIngenieriaProyecto.GrupoCertificadoIngenieriaId == GrupoCertificadoId)

                                                             .Where(c => c.UbicacionTrabajo == CertificacionIngenieriaCodigos.UBICACION_CAMPO)
                                                      .Select(c => c.TotalHoras).ToList().Sum();

                        sw.WriteLine("ColaboresDescuentos\t" + String.Join(",", ColaboradoresDescuentoViaticos.ToArray()));
                        sw.WriteLine("ColaboradoresAplicaViatico\t" + String.Join(",", ColaboradoresAplicaViatico.ToArray()));
                        sw.WriteLine("MontoTotalDirectosSinE500Campo\t" + MontoTotalDirectosSinE500);
                        sw.WriteLine("TotalMontoDirectosE500Campo\t" + TotalMontoDirectosE500Campo);
                        sw.WriteLine("TotalHorasDirectosE500Campo\t" + TotalHorasDirectosE500Campo);

                        sw.WriteLine("");


                        var ItemsViaticos = _itemRepository.GetAll().Where(c => c.vigente)
                                                                  .Where(c => c.codigo == "1.3.1" ||
                                                                              c.codigo == "1.3.2" ||
                                                                              c.codigo == "1.3.3" ||
                                                                              c.codigo == "1.3.4")
                                                                  .ToList().Distinct().ToList();

                        var viaticosE500 = new List<E500ViaticosModel>();

                        foreach (var Item in ItemsViaticos)
                        {
                            decimal totalhorasE500 = 0;
                            if (Item.codigo == "1.3.1")
                            {

                                var preciario = _detallePreciarioRepository.GetAll().Where(x => x.Item.codigo == Item.codigo)
                                                                                 .Where(c => c.vigente)
                                                                                 .Where(c => c.Preciario.vigente)
                                                                                 .OrderByDescending(c => c.Id)
                                                                                 .FirstOrDefault();
                                decimal tarifa = preciario != null ? preciario.precio_unitario : 0;

                                totalhorasE500 = (TotalDirectosE500Campo / 220) * tarifa;

                                sw.WriteLine(Item.codigo + "\t" + totalhorasE500);

                                viaticosE500.Add(new E500ViaticosModel { codigo = Item.codigo, montoViaticoE500 = totalhorasE500 });
                            }
                            if (Item.codigo == "1.3.2")
                            {
                                var preciario = _detallePreciarioRepository.GetAll().Where(x => x.Item.codigo == Item.codigo)
                                                                                .Where(c => c.vigente)
                                                                                .Where(c => c.Preciario.vigente)
                                                                                .OrderByDescending(c => c.Id)
                                                                                .FirstOrDefault();
                                decimal tarifa = preciario != null ? preciario.precio_unitario : 0;

                                totalhorasE500 = ((TotalDirectosE500Campo / 220) * 2) * tarifa;
                                viaticosE500.Add(new E500ViaticosModel { codigo = Item.codigo, montoViaticoE500 = totalhorasE500 });
                                sw.WriteLine(Item.codigo + "\t" + totalhorasE500);

                            }
                            if (Item.codigo == "1.3.3" || Item.codigo == "1.3.4")
                            {
                                var preciario = _detallePreciarioRepository.GetAll().Where(x => x.Item.codigo == Item.codigo)
                                                                                .Where(c => c.vigente)
                                                                                .Where(c => c.Preciario.vigente)
                                                                                .OrderByDescending(c => c.Id)
                                                                                .FirstOrDefault();
                                decimal tarifa = preciario != null ? preciario.precio_unitario : 0;

                                totalhorasE500 = (TotalDirectosE500Campo / 10) * tarifa;
                                viaticosE500.Add(new E500ViaticosModel { codigo = Item.codigo, montoViaticoE500 = totalhorasE500 });
                                sw.WriteLine(Item.codigo + "\t" + totalhorasE500);

                            }

                        }







                        //Iteracion por Proyecto 
                        foreach (var ProyectoId in ProyectosId)
                        {
                            //SE OBTIENE EL CERTIFICADO DEL PROYECTO
                            var certificado = (from cert in CertificadosPorProyectos where cert.ProyectoId == ProyectoId select cert).FirstOrDefault();
                            if (certificado != null)
                            {

                                var TotalPorProyecto_SINE500 = _gastoDirectoRepository.GetAll().Where(c => c.TipoGastoId == TipoGasto.Directo)
                                                                                                        .Where(c => !c.EsDistribucionE500) //SinE500
                                                                                                        .Where(c => c.CertificadoIngenieriaProyecto.GrupoCertificadoIngenieriaId == GrupoCertificadoId)
                                                                                                        .Where(c => c.CertificadoIngenieriaProyectoId == certificado.Id)
                                                                                                                 .Where(c => c.UbicacionTrabajo == CertificacionIngenieriaCodigos.UBICACION_CAMPO)
                                                                                                        //    .Where(c => ColaboradoresEnCampo.Contains(c.ColaboradorId))
                                                                                                        .Select(c => c.TarifaHoras)
                                                                                                        .ToList()
                                                                                                        .Sum();



                                //PORCENTAJA DE PARTICIPACION MONTOS
                                var porcentajeParticipacionProyecto = MontoTotalDirectosSinE500 > 0 ? (TotalPorProyecto_SINE500 / MontoTotalDirectosSinE500) : 0;


                                // INDIRECTOS SOLO QUE APLIQUEN VIATICO
                                #region DistribucionIndirectos
                                var Indirectos = _indirectosRepository.GetAllIncluding(c => c.ColaboradorRubro.Colaborador)
                                                                      .Where(c => ColaboradoresAplicaViatico.Contains(c.ColaboradorRubro.ColaboradorId))

                                                                      .Where(c => c.FechaDesde >= grupoCertificado.FechaInicio)
                                                                      .Where(c => c.FechaHasta <= grupoCertificado.FechaFin)
                                                                      .ToList();

                                decimal HorasIndirectasAcumulados = 0;
                                var personalInDirectos = (from t in Indirectos
                                                              // where !t.EsViatico   OJO revisar  comportamiento que ya se usa
                                                          group t by new
                                                          {
                                                              t.ColaboradorRubro.ColaboradorId,
                                                          }
                                                          into g
                                                          select new
                                                          {
                                                              Grupo = g.Key,
                                                              SumaHoras = g.Sum(x => x.HorasLaboradas * x.DiasLaborados)
                                                          }).ToList();

                                foreach (var horasIndirecto in personalInDirectos)
                                {
                                    decimal horasIndirectasPorProyecto = horasIndirecto.SumaHoras * porcentajeParticipacionProyecto;

                                    HorasIndirectasAcumulados = HorasIndirectasAcumulados + horasIndirectasPorProyecto;
                                }


                                var entityCertificado = _certificadoIngenieriaProyectoRepository.Get(certificado.Id);

                                entityCertificado.PorcentajeParticipacionDirectos = porcentajeParticipacionProyecto;
                                _certificadoIngenieriaProyectoRepository.Update(entityCertificado);

                                #endregion


                                decimal HorasDescuentoViaticos = _gastoDirectoRepository.GetAll()
                                                                                                .Where(c => c.TipoGastoId == TipoGasto.Directo)
                                                                                                .Where(c => !c.EsDistribucionE500)
                                                                                                .Where(c => !c.EsViatico)
                                                                                                .Where(c => c.CertificadoIngenieriaProyectoId == certificado.Id)
                                                                                                           .Where(c => c.UbicacionTrabajo == CertificacionIngenieriaCodigos.UBICACION_CAMPO)
                                                                                                    //  .Where(c => ColaboradoresEnCampo.Contains(c.ColaboradorId)) //Solo de Campo
                                                                                                    .Where(c => ColaboradoresDescuentoViaticos.Contains(c.ColaboradorId)) //Solo campo Descuento Viatico
                                                                                                .Select(c => c.TotalHoras).ToList().Sum();
                                ///HORAS SUMADAS AL TOTAL DE LA DISTRIBUCION DE INDIRECTOS
                                decimal HorasActualesDirectos = _gastoDirectoRepository.GetAll()
                                                                                                .Where(c => c.TipoGastoId == TipoGasto.Directo)
                                                                                                .Where(c => !c.EsDistribucionE500) //Sin E500
                                                                                                .Where(c => c.CertificadoIngenieriaProyectoId == certificado.Id)
                                                                                                         .Where(c => c.UbicacionTrabajo == CertificacionIngenieriaCodigos.UBICACION_CAMPO)
                                                                                                //.Where(c => ColaboradoresEnCampo.Contains(c.ColaboradorId)) //Solo de Campo
                                                                                                .Select(c => c.TotalHoras).ToList().Sum();

                                decimal TotalHoras = ((HorasActualesDirectos - HorasDescuentoViaticos) + HorasIndirectasAcumulados);
                                sw.WriteLine("");
                                sw.WriteLine(certificado.Proyecto.codigo + "\t" + TotalPorProyecto_SINE500 + "\t" + HorasActualesDirectos + "\t" + porcentajeParticipacionProyecto + "\t" + HorasDescuentoViaticos + "\t" + HorasIndirectasAcumulados);
                                sw.WriteLine("");

                                sp.WriteLine("");
                                sp.WriteLine(certificado.Proyecto.codigo + "\t" + TotalPorProyecto_SINE500 + "\t" + HorasActualesDirectos + "\t" + porcentajeParticipacionProyecto + "\t" + HorasDescuentoViaticos + "\t" + HorasIndirectasAcumulados);



                                //Total de horas incluidas UIO y OT
                                var TotalHorasPorProyecto = _gastoDirectoRepository.GetAll().Where(c => c.TipoGastoId == TipoGasto.Directo)
                                                                                                           .Where(c => c.CertificadoIngenieriaProyecto.GrupoCertificadoIngenieriaId == GrupoCertificadoId)
                                                                                                           .Where(c => c.CertificadoIngenieriaProyectoId == certificado.Id)
                                                                                                           .Select(c => c.TotalHoras)
                                                                                                           .ToList()
                                                                                                           .Sum();

                                var porcentajeParticipacionProyectoE500 = TotalHorasPorProyecto / TotalHorasDirectosQUIT_OOT;


                                var RubroViaticos = _detallePreciarioRepository.GetAllIncluding(i => i.Item).Where(c => c.vigente)
                                                                                                            .Where(c => c.Preciario.vigente)
                                                                                                            .Where(c => c.Item.codigo == "1.3.1" ||
                                                                                                                        c.Item.codigo == "1.3.2" ||
                                                                                                                        c.Item.codigo == "1.3.3" ||
                                                                                                                        c.Item.codigo == "1.3.4")
                                                                                                            .Where(c => c.Preciario.ContratoId == certificado.Proyecto.contratoId)
                                                                                                            .ToList();


                                foreach (var rubro in RubroViaticos)
                                {
                                    decimal totalHorasViatico = 0;
                                    decimal ViaticosConE500 = 0;
                                    if (rubro.Item.codigo == "1.3.1")
                                    {
                                        totalHorasViatico = TotalHoras / 220;
                                    }
                                    if (rubro.Item.codigo == "1.3.2")
                                    {
                                        totalHorasViatico = (TotalHoras / 220) * 2;
                                    }
                                    if (rubro.Item.codigo == "1.3.3" || rubro.Item.codigo == "1.3.4")
                                    {
                                        totalHorasViatico = TotalHoras / 10;
                                    }

                                    var valorItem = (from i in viaticosE500 where i.codigo == rubro.Item.codigo select i).FirstOrDefault(); //Saco el valor de la distribucion E500

                                    decimal montoE500 = (totalHorasViatico * rubro.precio_unitario); //Multiplica por tarifa sale monto
                                    decimal montoViaticoE500 = montoE500 + ((valorItem != null ? valorItem.montoViaticoE500 : 0) * porcentajeParticipacionProyectoE500);
                                    decimal horasViaticoE500 = rubro.precio_unitario > 0 ? (montoViaticoE500 / rubro.precio_unitario) : 0;
                                    sw.WriteLine(certificado.Proyecto.codigo + "\t" + rubro.Item.codigo + "\t" + rubro.precio_unitario + "\t" + porcentajeParticipacionProyecto + "\t" + montoE500 + "\t" + porcentajeParticipacionProyectoE500 + "\t" + montoViaticoE500);

                                    var colaboradorRubroIngenieria = _colaboradoRubroRepository.GetAll().Where(c => c.RubroId == rubro.Id).FirstOrDefault();

                                    if (colaboradorRubroIngenieria != null)
                                    {

                                        var ColaboradorCertificadoIngenieria = _colaboradoCertificacionRepository.GetAll()
                                                                                                .Where(c => c.ColaboradorId == colaboradorRubroIngenieria.ColaboradorId)
                                                                                .Where(c => grupoCertificado.FechaInicio >= c.FechaDesde)
                                                                                                .Where(c => grupoCertificado.FechaFin <= c.FechaHasta)
                                                                                                .FirstOrDefault();
                                        var indirectoEntity = new GastoDirectoCertificado()
                                        {
                                            Id = 0,
                                            CertificadoIngenieriaProyectoId = certificado.Id,
                                            TipoGastoId = TipoGasto.Indirecto,
                                            ColaboradorId = colaboradorRubroIngenieria.ColaboradorId,
                                            RubroId = rubro.Id,
                                            UnidadId = Unidad != null ? Unidad.Id : 0,
                                            TotalHoras = horasViaticoE500,
                                            TarifaHoras = montoViaticoE500,
                                            Tarifa = rubro.precio_unitario,
                                            EsDistribucionE500 = false,
                                            migrado = false,
                                            Area = ColaboradorCertificadoIngenieria != null ? Enum.GetName(typeof(Area), ColaboradorCertificadoIngenieria.AreaId) : "",
                                            UbicacionId = ColaboradorCertificadoIngenieria != null ? ColaboradorCertificadoIngenieria.Ubicacion.Id : UbicacionporDefecto != null ? UbicacionporDefecto.Id : 0,
                                            UbicacionTrabajo = ColaboradorCertificadoIngenieria != null ? ColaboradorCertificadoIngenieria.Ubicacion.codigo : "",
                                            EsViatico = true
                                        };

                                        _gastoDirectoRepository.Insert(indirectoEntity);

                                    }

                                }


                            }
                        }

                    }

                }
            }
        }

        public void GenerarViaticos(int GrupoCertificadoId)
        {
            var grupoCertificado = Repository.GetAll().Where(c => c.Id == GrupoCertificadoId).FirstOrDefault();
            var FechaMigracion = new DateTime(2021, 11, 20);
            if (grupoCertificado.FechaCertificado > FechaMigracion.Date)
            {

                string fileName = System.Web.HttpContext.Current.Server.MapPath("~/Views/PlantillaWord/CertificacionIngenieria/DistribucionIndirectos/DistribucionProyectos"
                    + DateTime.Now.Year + "" + DateTime.Now.Month + "" + DateTime.Now.Minute + "" + DateTime.Now.Millisecond + "" + ".txt");
                string fileNameIndirectos = System.Web.HttpContext.Current.Server.MapPath("~/Views/PlantillaWord/CertificacionIngenieria/DistribucionIndirectos/DistribucionIndirectos"
                   + DateTime.Now.Year + "" + DateTime.Now.Month + "" + DateTime.Now.Minute + "" + DateTime.Now.Millisecond + "" + ".txt");

                string fileNameTabla = System.Web.HttpContext.Current.Server.MapPath("~/Views/PlantillaWord/CertificacionIngenieria/DistribucionIndirectos/Dtabla"
             + DateTime.Now.Year + "" + DateTime.Now.Month + "" + DateTime.Now.Minute + "" + DateTime.Now.Millisecond + "" + ".txt");

                string fileNameTablaE500 = System.Web.HttpContext.Current.Server.MapPath("~/Views/PlantillaWord/CertificacionIngenieria/DistribucionIndirectos/DtablaE500"
        + DateTime.Now.Year + "" + DateTime.Now.Month + "" + DateTime.Now.Minute + "" + DateTime.Now.Millisecond + "" + ".txt");
                using (StreamWriter si = File.CreateText(fileNameIndirectos))
                {

                    using (StreamWriter sw = File.CreateText(fileName))
                    {

                        using (StreamWriter ta = File.CreateText(fileNameTabla))
                        {
                            using (StreamWriter e500 = File.CreateText(fileNameTablaE500))
                            {


                                var Unidad = _catalogoRepository.GetAll().Where(c => c.codigo == "TAREA_HOMBRE").FirstOrDefault();
                                var UbicacionporDefecto = _catalogoRepository.GetAll().Where(c => c.codigo == "UBICACION_CAMPO").FirstOrDefault();



                                var CertificadosPorProyectos = _certificadoIngenieriaProyectoRepository.GetAllIncluding(c => c.Proyecto)
                                                                                                       .Where(c => c.GrupoCertificadoIngenieriaId == GrupoCertificadoId)
                                                                                                       .ToList();

                                var ProyectosId = (from cert in CertificadosPorProyectos select cert.ProyectoId).ToList().Distinct().ToList();



                                // SE OBTIENE COLABORADORES SOLO DE CAMPO
                                var ColaboradoresEnCampo = _colaboradoCertificacionRepository.GetAllIncluding(e => e.Ubicacion)
                                                                                             .Where(c => c.Ubicacion.codigo == CertificacionIngenieriaCodigos.UBICACION_CAMPO)
                                                                                             .Select(c => c.ColaboradorId)
                                                                                             .ToList().Distinct().ToList();

                                // SE OBTIENE COLABORADORES QUE CONTENGA APLICA VIATIO SI
                                var ColaboradoresAplicaViatico = _colaboradoCertificacionRepository.GetAll().Where(c => c.AplicaViatico)
                                                                                                            .Select(c => c.ColaboradorId)
                                                                                                            .ToList().Distinct().ToList();

                                //COLABORADORES TELETRABAJO y APLICA VIATICO
                                var ColaboradoresHomeOffice = _colaboradoCertificacionRepository.GetAllIncluding(e => e.Modalidad)
                                                                                     .Where(c => c.Modalidad.codigo == CertificacionIngenieriaCodigos.MODALIDAD_HOMEOFFCIE)
                                                                                     .Where(c => c.AplicaViatico)

                                                                                     .Select(c => c.ColaboradorId)
                                                                                     .ToList().Distinct().ToList();

                                //COLABORADORES TELETRABAJO y APLICA VIATICO
                                var ColaboradoresDescuentoViaticos = _colaboradoCertificacionRepository.GetAllIncluding(e => e.Modalidad)
                                                                                                            .Where(c => c.AplicaViaticoDirecto)
                                                                                    .Select(c => c.ColaboradorId)
                                                                                     .ToList().Distinct().ToList();


                                // MONTO TOTAL DIRECTOS SOLO DE CAMPO 
                                var TotalDirectosCampo = _gastoDirectoRepository.GetAll().Where(c => c.TipoGastoId == TipoGasto.Directo)

                                                                                  .Where(c => c.CertificadoIngenieriaProyecto.GrupoCertificadoIngenieriaId == GrupoCertificadoId)
                                                                                  .Where(c => ColaboradoresEnCampo.Contains(c.ColaboradorId))
                                                                                  .Select(c => c.TarifaHoras).ToList().Sum();

                                var TotalDirectosCampoHoras = _gastoDirectoRepository.GetAll().Where(c => c.TipoGastoId == TipoGasto.Directo)
                                                                            .Where(c => c.CertificadoIngenieriaProyecto.GrupoCertificadoIngenieriaId == GrupoCertificadoId)
                                                                            .Where(c => ColaboradoresEnCampo.Contains(c.ColaboradorId))
                                                                            .Select(c => c.TotalHoras).ToList().Sum();

                                var TotalDirectosCampoHorasQUITOOT = _gastoDirectoRepository.GetAll().Where(c => c.TipoGastoId == TipoGasto.Directo)
                                                                            //.Where(c=>!c.EsDistribucionE500)
                                                                            .Where(c => c.CertificadoIngenieriaProyecto.GrupoCertificadoIngenieriaId == GrupoCertificadoId)
                                                                            // .Where(c => ColaboradoresEnCampo.Contains(c.ColaboradorId))
                                                                            .Select(c => c.TotalHoras).ToList().Sum();



                                //Total E500 Campo
                                var TotalDirectosE500Campo = _gastoDirectoRepository.GetAll().Where(c => c.TipoGastoId == TipoGasto.Directo)
                                                                            .Where(c => c.EsDistribucionE500)
                                                                            .Where(c => c.CertificadoIngenieriaProyecto.GrupoCertificadoIngenieriaId == GrupoCertificadoId)
                                                                            .Where(c => ColaboradoresEnCampo.Contains(c.ColaboradorId))
                                                                            .Select(c => c.TotalHoras).ToList().Sum();


                                e500.WriteLine("TotalDirectosUIOOT " + TotalDirectosCampoHorasQUITOOT);
                                e500.WriteLine("TotalE500: " + TotalDirectosE500Campo);

                                var ItemsViaticos = _itemRepository.GetAll().Where(c => c.vigente)
                                                                          .Where(c => c.codigo == "1.3.1" ||
                                                                                      c.codigo == "1.3.2" ||
                                                                                      c.codigo == "1.3.3" ||
                                                                                      c.codigo == "1.3.4")
                                                                          .ToList().Distinct().ToList();



                                var viaticosE500 = new List<E500ViaticosModel>();

                                foreach (var Item in ItemsViaticos)
                                {
                                    decimal totalhorasE500 = 0;
                                    if (Item.codigo == "1.3.1")
                                    {

                                        var preciario = _detallePreciarioRepository.GetAll().Where(x => x.Item.codigo == Item.codigo)
                                                                                         .Where(c => c.vigente)
                                                                                         .Where(c => c.Preciario.vigente)
                                                                                         .OrderByDescending(c => c.Id)
                                                                                         .FirstOrDefault();
                                        decimal tarifa = preciario != null ? preciario.precio_unitario : 0;

                                        totalhorasE500 = (TotalDirectosE500Campo / 220);//* tarifa;

                                        e500.WriteLine(Item.codigo + "\t" + "totalhorasE500 = (TotalDirectosE500Campo / 220)" + totalhorasE500);

                                        viaticosE500.Add(new E500ViaticosModel { codigo = Item.codigo, montoViaticoE500 = totalhorasE500 });
                                    }
                                    if (Item.codigo == "1.3.2")
                                    {
                                        var preciario = _detallePreciarioRepository.GetAll().Where(x => x.Item.codigo == Item.codigo)
                                                                                        .Where(c => c.vigente)
                                                                                        .Where(c => c.Preciario.vigente)
                                                                                        .OrderByDescending(c => c.Id)
                                                                                        .FirstOrDefault();
                                        decimal tarifa = preciario != null ? preciario.precio_unitario : 0;

                                        totalhorasE500 = ((TotalDirectosE500Campo / 220) * 2);//* tarifa;
                                        viaticosE500.Add(new E500ViaticosModel { codigo = Item.codigo, montoViaticoE500 = totalhorasE500 });
                                        e500.WriteLine(Item.codigo + "\t" + "(TotalDirectosE500Campo / 220) * 2" + totalhorasE500);

                                    }
                                    if (Item.codigo == "1.3.3" || Item.codigo == "1.3.4")
                                    {
                                        var preciario = _detallePreciarioRepository.GetAll().Where(x => x.Item.codigo == Item.codigo)
                                                                                        .Where(c => c.vigente)
                                                                                        .Where(c => c.Preciario.vigente)
                                                                                        .OrderByDescending(c => c.Id)
                                                                                        .FirstOrDefault();
                                        decimal tarifa = preciario != null ? preciario.precio_unitario : 0;

                                        totalhorasE500 = (TotalDirectosE500Campo / 10);// *tarifa;
                                        viaticosE500.Add(new E500ViaticosModel { codigo = Item.codigo, montoViaticoE500 = totalhorasE500 });
                                        e500.WriteLine(Item.codigo + "\t" + "(TotalDirectosE500Campo / 10)" + totalhorasE500);

                                    }

                                }






                                //IMPRIMIR ARCHIVO
                                sw.WriteLine("PROYECTO\tTOTALPORPROYECTO\tPORCENTAJEPARTICIPACION");
                                sw.WriteLine("\t\t\t\tColaboradoresAplicaViaticoSI:" + String.Join(",", ColaboradoresAplicaViatico.ToList()));
                                sw.WriteLine("\n");
                                sw.WriteLine("\t\t\t\tTotalDirectasCampo:\t" + TotalDirectosCampo);
                                //
                                ta.WriteLine("FC\tUSDDIRECTO\tHHDIRECTA\t%\tHHViaticos\tHHTeletrabajo\tDescontarViatico");
                                //1. RECORRER PROYECTO
                                foreach (var ProyectoId in ProyectosId)
                                {
                                    //SE OBTIENE EL CERTIFICADO DEL PROYECTO
                                    var certificado = (from cert in CertificadosPorProyectos where cert.ProyectoId == ProyectoId select cert).FirstOrDefault();
                                    if (certificado != null)
                                    {




                                        var TotalPorProyecto = _gastoDirectoRepository.GetAll().Where(c => c.TipoGastoId == TipoGasto.Directo)
                                                                                                                .Where(c => !c.EsDistribucionE500)
                                                                                                                .Where(c => c.CertificadoIngenieriaProyecto.GrupoCertificadoIngenieriaId == GrupoCertificadoId)
                                                                                                                .Where(c => c.CertificadoIngenieriaProyectoId == certificado.Id)
                                                                                                                .Where(c => ColaboradoresEnCampo.Contains(c.ColaboradorId))
                                                                                                                .Select(c => c.TarifaHoras)
                                                                                                                .ToList()
                                                                                                                .Sum();




                                        ///HORAS SUMADAS AL TOTAL DE LA DISTRIBUCION DE INDIRECTOS
                                        decimal HorasActualesDirectosCertificados = _gastoDirectoRepository.GetAll()
                                                                                                        .Where(c => c.TipoGastoId == TipoGasto.Directo)
                                                                                                        .Where(c => !c.EsDistribucionE500)
                                                                                                        .Where(c => !c.EsViatico)
                                                                                                        .Where(c => c.CertificadoIngenieriaProyectoId == certificado.Id)
                                                                                                        .Where(c => ColaboradoresEnCampo.Contains(c.ColaboradorId)) //Solo de Campo
                                                                                                        .Select(c => c.TotalHoras).ToList().Sum();


                                        decimal HorasActualesHomeOfficeAplicaViatico = _gastoDirectoRepository.GetAll()
                                                                                                   .Where(c => c.TipoGastoId == TipoGasto.Directo)
                                                                                                   .Where(c => !c.EsDistribucionE500)
                                                                                                   .Where(c => !c.EsViatico)
                                                                                                   .Where(c => c.CertificadoIngenieriaProyectoId == certificado.Id)
                                                                                                   .Where(c => ColaboradoresEnCampo.Contains(c.ColaboradorId)) //Solo de Campo
                                                                                                       .Where(c => ColaboradoresHomeOffice.Contains(c.ColaboradorId)) //Solo HOmeOficce
                                                                                                   .Select(c => c.TotalHoras).ToList().Sum();

                                        decimal HorasDescuentoViaticos = _gastoDirectoRepository.GetAll()
                                                                                                  .Where(c => c.TipoGastoId == TipoGasto.Directo)
                                                                                                  .Where(c => !c.EsDistribucionE500)
                                                                                                  .Where(c => !c.EsViatico)
                                                                                                  .Where(c => c.CertificadoIngenieriaProyectoId == certificado.Id)
                                                                                                  .Where(c => ColaboradoresEnCampo.Contains(c.ColaboradorId)) //Solo de Campo
                                                                                                      .Where(c => ColaboradoresDescuentoViaticos.Contains(c.ColaboradorId)) //Solo HOmeOficce
                                                                                                  .Select(c => c.TotalHoras).ToList().Sum();

                                        decimal HorasActualesNoteletrabajo = HorasActualesDirectosCertificados - HorasActualesHomeOfficeAplicaViatico;





                                        var TotalHPorProyecto = _gastoDirectoRepository.GetAll().Where(c => c.TipoGastoId == TipoGasto.Directo)
                                                                                                                //  .Where(c => !c.EsDistribucionE500)
                                                                                                                .Where(c => c.CertificadoIngenieriaProyecto.GrupoCertificadoIngenieriaId == GrupoCertificadoId)
                                                                                                                .Where(c => c.CertificadoIngenieriaProyectoId == certificado.Id)
                                                                                                                //  .Where(c => ColaboradoresEnCampo.Contains(c.ColaboradorId))
                                                                                                                .Select(c => c.TotalHoras)
                                                                                                                .ToList()
                                                                                                                .Sum();
                                        var TotalMPorProyecto = _gastoDirectoRepository.GetAll().Where(c => c.TipoGastoId == TipoGasto.Directo)
                                                                                                                  //   .Where(c => !c.EsDistribucionE500)
                                                                                                                  .Where(c => c.CertificadoIngenieriaProyecto.GrupoCertificadoIngenieriaId == GrupoCertificadoId)
                                                                                                                  .Where(c => c.CertificadoIngenieriaProyectoId == certificado.Id)
                                                                                                                  //  .Where(c => ColaboradoresEnCampo.Contains(c.ColaboradorId))
                                                                                                                  .Select(c => c.TarifaHoras)
                                                                                                                  .ToList()
                                                                                                                  .Sum();


                                        var TotalHorasPorProyectoE500 = _gastoDirectoRepository.GetAll().Where(c => c.TipoGastoId == TipoGasto.Directo)
                                                                                                             .Where(c => c.EsDistribucionE500)
                                                                                                             .Where(c => c.CertificadoIngenieriaProyecto.GrupoCertificadoIngenieriaId == GrupoCertificadoId)
                                                                                                             .Where(c => c.CertificadoIngenieriaProyectoId == certificado.Id)
                                                                                                             .Where(c => ColaboradoresEnCampo.Contains(c.ColaboradorId))
                                                                                                             .Select(c => c.TotalHoras)
                                                                                                             .ToList()
                                                                                                             .Sum();

                                        var TotalMontoPorProyectoE500 = _gastoDirectoRepository.GetAll().Where(c => c.TipoGastoId == TipoGasto.Directo)
                                                                                                             .Where(c => c.EsDistribucionE500)
                                                                                                             .Where(c => c.CertificadoIngenieriaProyecto.GrupoCertificadoIngenieriaId == GrupoCertificadoId)
                                                                                                             .Where(c => c.CertificadoIngenieriaProyectoId == certificado.Id)
                                                                                                             .Where(c => ColaboradoresEnCampo.Contains(c.ColaboradorId))
                                                                                                             .Select(c => c.TarifaHoras)
                                                                                                             .ToList()
                                                                                                             .Sum();

                                        //PORCENTAJA DE PARTICIPACION MONTOS
                                        var porcentajeParticipacionProyecto = TotalDirectosCampo > 0 ? (TotalPorProyecto / TotalDirectosCampo) : 0;

                                        var porcentajeParticipacionProyectoE500 = TotalHPorProyecto / TotalDirectosCampoHorasQUITOOT;

                                        ta.WriteLine(certificado.Proyecto.codigo + "\t" + TotalPorProyecto + "\t" + HorasActualesDirectosCertificados + "\t" + porcentajeParticipacionProyecto + "\t" + HorasActualesNoteletrabajo + "\t" + HorasActualesHomeOfficeAplicaViatico + "\t" + HorasDescuentoViaticos);



                                        e500.WriteLine(certificado.Proyecto.codigo + "\t" + TotalHPorProyecto + "\t" + TotalMPorProyecto + "\t" + TotalHorasPorProyectoE500 + "\t" + TotalMontoPorProyectoE500 + "\t" + porcentajeParticipacionProyectoE500);



                                        sw.WriteLine("\n");
                                        sw.WriteLine(certificado.Proyecto.codigo + "\t" + TotalPorProyecto + "\t" + porcentajeParticipacionProyecto);
                                        sw.WriteLine("\n");


                                        // INDIRECTOS SOLO QUE APLIQUEN VIATICO

                                        var Indirectos = _indirectosRepository.GetAllIncluding(c => c.ColaboradorRubro.Colaborador)
                                                                              .Where(c => ColaboradoresAplicaViatico.Contains(c.ColaboradorRubro.ColaboradorId))
                                                                              .Where(c => c.FechaDesde >= grupoCertificado.FechaInicio)
                                                                              .Where(c => c.FechaHasta <= grupoCertificado.FechaFin)
                                                                              .ToList();

                                        si.WriteLine("INDIRECTOS:\t" + String.Join(",", Indirectos.Select(c => c.Id).ToList()));
                                        si.WriteLine(certificado.Proyecto.codigo);

                                        decimal HorasIndirectasAcumulados = 0;
                                        var personalInDirectos = (from t in Indirectos
                                                                  where !t.EsViatico
                                                                  group t by new
                                                                  {
                                                                      t.ColaboradorRubro.ColaboradorId,
                                                                  }
                                                                  into g
                                                                  select new
                                                                  {
                                                                      Grupo = g.Key,
                                                                      SumaHoras = g.Sum(x => x.HorasLaboradas * x.DiasLaborados)
                                                                  }).ToList();

                                        si.WriteLine("horas Agrupadas por Colaborador");
                                        foreach (var horasIndirecto in personalInDirectos)
                                        {

                                            decimal horasIndirectasPorProyecto = horasIndirecto.SumaHoras * porcentajeParticipacionProyecto;
                                            si.WriteLine(horasIndirecto.Grupo.ColaboradorId + "\t" + horasIndirecto.SumaHoras + "\t" + porcentajeParticipacionProyecto + "\t" + horasIndirectasPorProyecto);
                                            HorasIndirectasAcumulados = HorasIndirectasAcumulados + horasIndirectasPorProyecto;
                                        }


                                        var entityCertificado = _certificadoIngenieriaProyectoRepository.Get(certificado.Id);

                                        entityCertificado.PorcentajeParticipacionDirectos = porcentajeParticipacionProyecto;
                                        _certificadoIngenieriaProyectoRepository.Update(entityCertificado);



                                        decimal TotalHoras = ((HorasActualesDirectosCertificados - HorasDescuentoViaticos) + HorasIndirectasAcumulados);

                                        si.WriteLine((HorasActualesDirectosCertificados - HorasDescuentoViaticos) + "\t" + HorasIndirectasAcumulados + "\t" + TotalHoras);

                                        si.WriteLine("");
                                        si.WriteLine("Acumuladas" + "\t" + HorasIndirectasAcumulados);

                                        var RubroViaticos = _detallePreciarioRepository.GetAllIncluding(i => i.Item).Where(c => c.vigente)
                                                                                                                    .Where(c => c.Preciario.vigente)
                                                                                                                    .Where(c => c.Item.codigo == "1.3.1" ||
                                                                                                                                c.Item.codigo == "1.3.2" ||
                                                                                                                                c.Item.codigo == "1.3.3" ||
                                                                                                                                c.Item.codigo == "1.3.4")
                                                                                                                    .Where(c => c.Preciario.ContratoId == certificado.Proyecto.contratoId)
                                                                                                                    .ToList();
                                        foreach (var rubro in RubroViaticos)
                                        {
                                            decimal totalHorasViatico = 0;
                                            if (rubro.Item.codigo == "1.3.1")
                                            {


                                                totalHorasViatico = TotalHoras / 220;

                                                var valorItem = (from i in viaticosE500 where i.codigo == rubro.Item.codigo select i).FirstOrDefault(); //Saco el valor de la distribucion E500
                                                decimal viaticoDistribuidoE500 = (valorItem != null ? valorItem.montoViaticoE500 : 0) * porcentajeParticipacionProyectoE500;

                                                totalHorasViatico = totalHorasViatico + viaticoDistribuidoE500;





                                                si.WriteLine(rubro.Item.codigo + "\t" + TotalHoras);


                                            }
                                            if (rubro.Item.codigo == "1.3.2")
                                            {

                                                totalHorasViatico = (TotalHoras / 220) * 2;

                                                var valorItem = (from i in viaticosE500 where i.codigo == rubro.Item.codigo select i).FirstOrDefault(); //Saco el valor de la distribucion E500
                                                decimal viaticoDistribuidoE500 = (valorItem != null ? valorItem.montoViaticoE500 : 0) * porcentajeParticipacionProyectoE500;

                                                totalHorasViatico = totalHorasViatico + viaticoDistribuidoE500;

                                                si.WriteLine(rubro.Item.codigo + "\t" + TotalHoras);
                                            }
                                            if (rubro.Item.codigo == "1.3.3" || rubro.Item.codigo == "1.3.4")
                                            {

                                                totalHorasViatico = TotalHoras / 10;

                                                var valorItem = (from i in viaticosE500 where i.codigo == rubro.Item.codigo select i).FirstOrDefault(); //Saco el valor de la distribucion E500
                                                decimal viaticoDistribuidoE500 = (valorItem != null ? valorItem.montoViaticoE500 : 0) * porcentajeParticipacionProyectoE500;

                                                totalHorasViatico = totalHorasViatico + viaticoDistribuidoE500;


                                                si.WriteLine(rubro.Item.codigo + "\t" + TotalHoras);
                                            }

                                            decimal monto = (totalHorasViatico * rubro.precio_unitario); //Multiplica por tarifa sale monto




                                            var colaboradorRubroIngenieria = _colaboradoRubroRepository.GetAll().Where(c => c.RubroId == rubro.Id).FirstOrDefault();

                                            if (colaboradorRubroIngenieria != null)
                                            {
                                                si.WriteLine("colaboradorRubroIngenieria" + "\t" + colaboradorRubroIngenieria.ColaboradorId + "\t" + monto + "\t" + colaboradorRubroIngenieria.Tarifa);
                                                var ColaboradorCertificadoIngenieria = _colaboradoCertificacionRepository.GetAll().Where(c => c.ColaboradorId == colaboradorRubroIngenieria.ColaboradorId).FirstOrDefault();
                                                var indirectoEntity = new GastoDirectoCertificado()
                                                {
                                                    Id = 0,
                                                    CertificadoIngenieriaProyectoId = certificado.Id,
                                                    TipoGastoId = TipoGasto.Indirecto,
                                                    ColaboradorId = colaboradorRubroIngenieria.ColaboradorId,
                                                    RubroId = rubro.Id,
                                                    UnidadId = Unidad != null ? Unidad.Id : 0,
                                                    TotalHoras = totalHorasViatico,
                                                    TarifaHoras = monto,
                                                    Tarifa = rubro.precio_unitario,
                                                    EsDistribucionE500 = false,
                                                    migrado = false,
                                                    Area = ColaboradorCertificadoIngenieria != null ? Enum.GetName(typeof(Area), ColaboradorCertificadoIngenieria.AreaId) : "",
                                                    UbicacionId = ColaboradorCertificadoIngenieria != null ? ColaboradorCertificadoIngenieria.Ubicacion.Id : UbicacionporDefecto != null ? UbicacionporDefecto.Id : 0,
                                                    UbicacionTrabajo = ColaboradorCertificadoIngenieria != null ? ColaboradorCertificadoIngenieria.Ubicacion.codigo : "",
                                                    EsViatico = true
                                                };

                                                _gastoDirectoRepository.Insert(indirectoEntity);

                                            }

                                        }


                                    }
                                }
                            }

                        }
                    }
                }
            }
        }

        public DtoValoresRubro ObtenerValoresPresupuestoporCodigoItem(string codigoItem, int ContratoId, List<ComputoPresupuesto> computos)
        {
            var dto = new DtoValoresRubro();
            var queryDetallePreciario = _detallePreciarioRepository.GetAll().Where(c => c.vigente)
                                                                 .Where(c => c.Preciario.vigente)
                                                                 .Where(c => c.Item.codigo == codigoItem)
                                                                 .Where(c => c.Preciario.ContratoId == ContratoId)
                                                                 .FirstOrDefault();

            if (queryDetallePreciario != null)
            {
                dto.tarifa = queryDetallePreciario.precio_unitario;

                decimal cantidadPresupuestada = (from c in computos
                                                 where c.vigente
                                                 where c.Item.codigo == codigoItem
                                                 select c.cantidad).ToList().Sum();

                decimal monto = cantidadPresupuestada * queryDetallePreciario.precio_unitario;

                dto.cantidad = cantidadPresupuestada;
                dto.monto = monto;
            }
            return dto;

        }

        public void ActualizarCampoLocacionConUbicacionParametrizacion(DateTime fechaDesde, DateTime FechaHasta)
        {
            var fechaDesdeDate = fechaDesde.Date;
            var fechaHastaDate = FechaHasta.Date;


            /*Directos */
            var directos = _directosRepository.GetAll().Where(c => c.FechaTrabajo >= fechaDesdeDate)
                                                       .Where(c => c.FechaTrabajo <= fechaHastaDate)
                                                       .Where(c => !c.LocacionId.HasValue) // # SI desea actualizar locacion debe ir a directos
                                                       .ToList();


            foreach (var directo in directos)
            {
                var entity = _directosRepository.Get(directo.Id);
                var fechaTrabajo = directo.FechaTrabajo.Date;
                var categorizacionColaborador = _colaboradoCertificacionRepository.GetAll().Where(c => c.ColaboradorId == directo.ColaboradorId).ToList();
                if (categorizacionColaborador.Count > 1)
                {
                    var categorizacionFecha = (from c in categorizacionColaborador where fechaTrabajo >= c.FechaDesde where fechaTrabajo <= c.FechaHasta select c).FirstOrDefault();

                    if (categorizacionFecha != null)
                    {
                        entity.LocacionId = categorizacionFecha.UbicacionId;
                    }
                }
                else
                {
                    var categorizacion = (from c in categorizacionColaborador select c).FirstOrDefault();
                    if (categorizacion != null)
                    {
                        entity.LocacionId = categorizacion.UbicacionId;
                    }

                }

            }
            /*E500*/
            var directosE500 = _directoE50.GetAll().Where(c => c.FechaTrabajo >= fechaDesdeDate).Where(c => c.FechaTrabajo <= fechaHastaDate).ToList();
            //var ColaboradoresDirectos = (from d in directos select d.ColaboradorId).ToList().Distinct().ToList();

            foreach (var directo in directosE500)
            {
                var entity = _directoE50.Get(directo.Id);
                var fechaTrabajo = directo.FechaTrabajo.Date;
                var categorizacionColaborador = _colaboradoCertificacionRepository.GetAll().Where(c => c.ColaboradorId == directo.ColaboradorId).ToList();
                if (categorizacionColaborador.Count > 1)
                {
                    var categorizacionFecha = (from c in categorizacionColaborador where fechaTrabajo >= c.FechaDesde where fechaTrabajo <= c.FechaHasta select c).FirstOrDefault();

                    if (categorizacionFecha != null)
                    {
                        entity.LocacionId = categorizacionFecha.UbicacionId;
                    }
                }
                else
                {
                    var categorizacion = (from c in categorizacionColaborador select c).FirstOrDefault();
                    if (categorizacion != null)
                    {
                        entity.LocacionId = categorizacion.UbicacionId;
                    }

                }

            }



        }

        public bool ResumenCertificacion(int GrupoCertificadoId, int ProyectoId)
        {
            return true;
        }

        public ValidacionColaboradorRubro ValidarRegistros(GrupoCertificadoIngenieria input, int[] Directos, int[] Indirectos, int[] E500)
        {
            List<ColaboradorDato> Parametrizacion = new List<ColaboradorDato>();
            List<ColaboradorDato> Rubros = new List<ColaboradorDato>();
            bool validado = true;
            var DirectosIds = Directos.Select(c => c).ToList();
            var IndirectosIds = Indirectos.Select(c => c).ToList();
            var E500Ids = E500.Select(c => c).ToList();

            var PersonalDirecto = _directosRepository.GetAllIncluding(c => c.Colaborador, c => c.Proyecto).Where(c => DirectosIds.Contains(c.Id)).ToList();
            var PersonalInDirecto = _indirectosRepository.GetAllIncluding(c => c.ColaboradorRubro.Colaborador).Where(c => IndirectosIds.Contains(c.Id)).ToList();
            var PersonalE500 = _directoE50.GetAllIncluding(c => c.Colaborador).Where(c => E500Ids.Contains(c.Id)).ToList();

            foreach (var directo in PersonalDirecto)
            {
                var fechaDate = directo.FechaTrabajo.Date;
                var colaboradorRubro = _colaboradoRubroRepository.GetAll().Where(c => fechaDate >= c.FechaInicio)
                                                                        .Where(c => fechaDate <= c.FechaFin)
                                                                        .Where(c => c.ColaboradorId == directo.ColaboradorId)
                                                                        .Where(c => c.ContratoId == directo.Proyecto.contratoId)
                                                                        .FirstOrDefault();
                if (colaboradorRubro == null)
                {
                    var data = new ColaboradorDato()
                    {
                        identificacion = directo.Colaborador.numero_identificacion,
                        nombreCompleto = directo.Colaborador.nombres_apellidos,
                        fecha = directo.FechaTrabajo.Date.ToShortDateString()
                        ,
                        tipo = "DIRECTO"
                    };
                    Rubros.Add(data);
                    validado = false;
                }

                var colaboradorParacmetrizacion = _colaboradoCertificacionRepository.GetAll().Where(c => fechaDate >= c.FechaDesde)
                                                                        .Where(c => fechaDate <= c.FechaHasta)
                                                                        .Where(c => c.ColaboradorId == directo.ColaboradorId)
                                                                        .FirstOrDefault();

                if (colaboradorParacmetrizacion == null)
                {
                    var data = new ColaboradorDato()
                    {
                        identificacion = directo.Colaborador.numero_identificacion,
                        nombreCompleto = directo.Colaborador.nombres_apellidos,
                        fecha = directo.FechaTrabajo.Date.ToShortDateString()
                          ,
                        tipo = "DIRECTO"

                    };
                    Parametrizacion.Add(data);
                    validado = false;
                }

            }


            foreach (var directo in PersonalE500)
            {
                var fechaDate = directo.FechaTrabajo.Date;
                var colaboradorRubro = _colaboradoRubroRepository.GetAll().Where(c => fechaDate >= c.FechaInicio)
                                                                        .Where(c => fechaDate <= c.FechaFin)
                                                                        .Where(c => c.ColaboradorId == directo.ColaboradorId)
                                                                        .FirstOrDefault();
                if (colaboradorRubro == null)
                {
                    var data = new ColaboradorDato()
                    {
                        identificacion = directo.Colaborador.numero_identificacion,
                        nombreCompleto = directo.Colaborador.nombres_apellidos,
                        fecha = directo.FechaTrabajo.Date.ToShortDateString()
                          ,
                        tipo = "E500"

                    };
                    Rubros.Add(data);
                    validado = false;
                }

                var colaboradorParacmetrizacion = _colaboradoCertificacionRepository.GetAll().Where(c => fechaDate >= c.FechaDesde)
                                                                        .Where(c => fechaDate <= c.FechaHasta)
                                                                        .Where(c => c.ColaboradorId == directo.ColaboradorId)
                                                                        .FirstOrDefault();

                if (colaboradorParacmetrizacion == null)
                {
                    var data = new ColaboradorDato()
                    {
                        identificacion = directo.Colaborador.numero_identificacion,
                        nombreCompleto = directo.Colaborador.nombres_apellidos,
                        fecha = directo.FechaTrabajo.Date.ToShortDateString()
                          ,
                        tipo = "E500"
                    };
                    Parametrizacion.Add(data);
                    validado = false;
                }

            }


            foreach (var indirecto in PersonalInDirecto)
            {

                var fechaDesdeDate = indirecto.FechaDesde.Date;
                var fechaHastaDate = indirecto.FechaHasta.Date;

                var colaboradorRubro = _colaboradoRubroRepository.GetAll().Where(c => fechaDesdeDate >= c.FechaInicio)
                                                                        .Where(c => fechaHastaDate <= c.FechaFin)
                                                                        .Where(c => c.ColaboradorId == indirecto.ColaboradorRubro.Colaborador.Id)
                                                                        .FirstOrDefault();
                if (colaboradorRubro == null)
                {
                    var data = new ColaboradorDato()
                    {
                        identificacion = indirecto.ColaboradorRubro.Colaborador.numero_identificacion,
                        nombreCompleto = indirecto.ColaboradorRubro.Colaborador.nombres_apellidos,
                        fecha = indirecto.FechaDesde.Date.ToShortDateString() + " - " + indirecto.FechaHasta.Date.ToShortDateString()
                          ,
                        tipo = "INDIRECTO"
                    };
                    Rubros.Add(data);
                    validado = false;
                }

                var colaboradorParacmetrizacion = _colaboradoCertificacionRepository.GetAll()
                                                                        .Where(c => fechaDesdeDate >= c.FechaDesde)
                                                                        .Where(c => fechaHastaDate <= c.FechaHasta)
                                                                        .Where(c => c.ColaboradorId == indirecto.ColaboradorRubro.Colaborador.Id)
                                                                        .FirstOrDefault();

                if (colaboradorParacmetrizacion == null)
                {
                    var data = new ColaboradorDato()
                    {
                        identificacion = indirecto.ColaboradorRubro.Colaborador.numero_identificacion,
                        nombreCompleto = indirecto.ColaboradorRubro.Colaborador.nombres_apellidos,
                        fecha = indirecto.FechaDesde.Date.ToShortDateString() + " - " + indirecto.FechaHasta.Date.ToShortDateString()
                          ,
                        tipo = "INDIRECTO"
                    };
                    Parametrizacion.Add(data);
                    validado = false;
                }


            }


            var result = new ValidacionColaboradorRubro()
            {
                Parametrizacion = Parametrizacion,
                Rubros = Rubros,
                puedeCertificar = validado
            };

            return result;

        }

        public bool AprobarGrupoCertificado(int id)
        {
            var entity = Repository.Get(id);
            entity.EstadoId = EstadoGrupoCertificado.Aprobado;
            return true;
        }

        public List<Proyecto> ProyectosACertificar(int[] Directos)
        {
            var DirectosIds = Directos.Select(c => c).ToList();
            var ProyectosIds = _directosRepository.GetAll()
                                                .Where(c => DirectosIds.Contains(c.Id))
                                                .Select(c => c.ProyectoId)
                                                .ToList()
                                                .Distinct()
                                                .ToList();
            if (ProyectosIds.Count > 0)
            {
                var proyectos = _proyectoRepository.GetAll().Where(c => ProyectosIds.Contains(c.Id)).ToList();
            }
            return new List<Proyecto>();
        }

        public string ActualizarProyectoCertificable(int id, bool certificable)
        {
            var entity = _proyectoRepository.Get(id);
            entity.certificable_ingenieria = certificable;
            return "OK";
        }

        public List<ProyectoDistribucionModel> ProyectoDistribuibles(int[] DirectosSeleccionadosId)
        {
            var ProyectosIdFromDirectos = _directosRepository.GetAll().Where(p => DirectosSeleccionadosId.Contains(p.Id))
                                                             .Select(p => p.ProyectoId)
                                                             .ToList().Distinct().ToList();

            var proyectosQuery = _proyectoRepository.GetAll()
                                                    .Where(c => c.vigente)
                                                    .Where(c => ProyectosIdFromDirectos.Contains(c.Id))
                                                    .ToList();

            var list = (from p in proyectosQuery
                        select new ProyectoDistribucionModel()
                        {
                            Id = p.Id,
                            Codigo = p.codigo,
                            Nombre = p.nombre_proyecto,
                            AplicaViatico = true,
                            AplicaIndirecto = true,
                            AplicaE500 = true
                        }).ToList();
            return list;
        }

        public List<ProyectoDistribucionModel> ObtenerParametrizacion(int GrupoCertificadoId)
        {
            var query = _distribucionRepository.GetAllIncluding(c => c.Proyecto)
                                               .Where(c => c.GrupoCertificadoId == GrupoCertificadoId)
                                               .ToList();
            var list = (from p in query
                        select new ProyectoDistribucionModel()
                        {
                            Id = p.Id,
                            Codigo = p.Proyecto.codigo,
                            Nombre = p.Proyecto.nombre_proyecto,
                            AplicaViatico = p.AplicaViatico,
                            AplicaIndirecto = p.AplicaIndirecto,
                            AplicaE500 = p.AplicaE500
                        }).ToList();
            return list;

        }




        public ExcelPackage GrupoCertificadosCompleto2(int GrupoCertificadoId)
        {
            /*DATOS GRUPOCERTIFICADO*/
            var grupoCertificado = Repository.GetAll().Where(c => c.Id == GrupoCertificadoId).FirstOrDefault();

            var certificadosPorGrupo = _certificadoIngenieriaProyectoRepository.GetAllIncluding(c => c.Proyecto, c => c.GrupoCertificadoIngenieria)
                                                                              .Where(c => c.GrupoCertificadoIngenieriaId == GrupoCertificadoId)
                                                                              .OrderBy(c => c.Proyecto.orden_timesheet)
                                                                              .ToList();
       

            ExcelPackage excel = new ExcelPackage();
         
            string filename = System.Web.HttpContext.Current.Server.MapPath("~/Views/PlantillaWord/CertificacionIngenieria/AgrupadoNuevo.xlsx");

            if (File.Exists((string)filename))
            {
                FileInfo docplantilla = new FileInfo(filename);
                ExcelPackage plantilla = new ExcelPackage(docplantilla);

                //Crear Pestaña por Proyecto
                foreach (var c in certificadosPorGrupo)
                {

                    var proyectoEntitdad = _proyectoRepository.GetAllIncluding(y => y.Portafolio, y => y.Ubicacion).Where(y => y.Id == c.ProyectoId).FirstOrDefault();
                    var fechaCertificacion = c.GrupoCertificadoIngenieria.FechaCertificado;
                    var mesCertificacion = fechaCertificacion.ToString("MMM", CultureInfo.CreateSpecificCulture("es-Es")).ToLower().Replace(".", "") + "" + fechaCertificacion.ToString("yy");
                    var nomnbrePestaña = (c.Proyecto.codigo.Length > 0 ? c.Proyecto.codigo.ToUpper().Replace("FC", "").Replace(".", "") : "") + "" + mesCertificacion;






                    var detallesAnteriores = new List<GastoDirectoCertificado>();
                    #region DetallesAnterioresCertificados
                    var certificadoAnteriorProyectoListado = _certificadoIngenieriaProyectoRepository.GetAll()
                                                                                            .Where(x => x.ProyectoId == c.ProyectoId)
                                                                                            .Where(x => x.GrupoCertificadoIngenieria.FechaCertificado < c.GrupoCertificadoIngenieria.FechaCertificado)
                                                                                            .OrderByDescending(x => x.GrupoCertificadoIngenieria.FechaCertificado)
                                                                                            .ToList();
                    if (certificadoAnteriorProyectoListado.Count > 0)
                    {
                        foreach (var certificadoAnteriorProyecto in certificadoAnteriorProyectoListado)
                        {
                            var detallesAnteriorCertificado = _gastoDirectoRepository.GetAllIncluding(x => x.Colaborador, x => x.Rubro.Item)
                                                                                                    .Where(x => x.CertificadoIngenieriaProyectoId == certificadoAnteriorProyecto.Id)
                                                                                                    .ToList();
                            if (detallesAnteriorCertificado.Count > 0)
                            {
                                detallesAnteriores.AddRange(detallesAnteriorCertificado);
                            }

                        }

                    }

                    #endregion

                    var detalleCertificados = _gastoDirectoRepository.GetAllIncluding(x => x.Colaborador, x => x.Rubro.Item)
                                                                        .Where(x => x.CertificadoIngenieriaProyectoId == c.Id)
                                                                        .ToList();

                    if (detalleCertificados.Count > 0)
                    {
                           ExcelWorksheet hoja = excel.Workbook.Worksheets.Add(nomnbrePestaña, plantilla.Workbook.Worksheets[1]);



                        if (proyectoEntitdad != null)
                        {

                            if (proyectoEntitdad.UbicacionId.HasValue && proyectoEntitdad.Ubicacion.codigo == "PROYECTO_CERT_QUITO")
                            {
                                //Color UBICACION UI   celeste
                                hoja.TabColor = Color.FromArgb(0, 176, 240);
                            }
                            if (proyectoEntitdad.UbicacionId.HasValue && proyectoEntitdad.Ubicacion.codigo == "PROYECTO_CERT_OT")
                            {
                                //Color UBICACION OT  verde
                                hoja.TabColor = Color.FromArgb(146, 208, 80);
                            }
                            if (proyectoEntitdad.PortafolioId.HasValue && proyectoEntitdad.Portafolio.codigo == "PORT_CERT_2019")
                            {
                                //Color Portafolio 2019 narnja
                                hoja.TabColor = Color.FromArgb(247, 150, 70);
                            }
                            if (proyectoEntitdad.PortafolioId.HasValue && proyectoEntitdad.Portafolio.codigo == "PORT_CERT_2020")
                            {
                                //Color Portafolio 2020 lila
                                hoja.TabColor = Color.FromArgb(177, 160, 199);
                            }

                        }


                        #region Cabecera Certificado
                        string nombreProyecto = c.Proyecto.descripcion_proyecto;

            
                        int filaNuevo = 7;
                        string cellN = "";
                        cellN = "D" + filaNuevo;
                        hoja.Cells[cellN].Value = c.Proyecto.codigo.ToUpper() + " " + c.Proyecto.descripcion_proyecto.ToUpper();
                        //hoja.Cells[cellN].Style.WrapText = true;
                        hoja.Cells[cellN].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        hoja.Cells[cellN].Style.VerticalAlignment = ExcelVerticalAlignment.Center;



             

                        filaNuevo = 9;

                        cellN = "D" + filaNuevo;

                        var stringUltimaPo = _detallePORepository.GetAll().Where(x => x.ProyectoId == x.ProyectoId)
                                                                   .Where(x => x.vigente)
                                                                   .OrderByDescending(x => x.OrdenServicio.fecha_orden_servicio)
                                                                   .Select(x =>x.OrdenServicio.codigo_orden_servicio)
                                                                   .FirstOrDefault();

                        hoja.Cells[cellN].Value = (stringUltimaPo == null || stringUltimaPo == "") ? "TBD" : stringUltimaPo;
                        hoja.Cells[cellN].Style.WrapText = true;
                        hoja.Cells[cellN].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        hoja.Cells[cellN].Style.VerticalAlignment = ExcelVerticalAlignment.Center;




                        filaNuevo = 10;

                        cellN = "D" + filaNuevo;

                        hoja.Cells[cellN].Value = c.Proyecto.codigo + "-" + (c.NumeroCertificado < 9 ? "0" + c.NumeroCertificado.ToString().Replace(" ", "") : c.NumeroCertificado.ToString().Replace(" ", ""));
                        hoja.Cells[cellN].Style.WrapText = true;
                        hoja.Cells[cellN].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        hoja.Cells[cellN].Style.VerticalAlignment = ExcelVerticalAlignment.Center;




             
                        filaNuevo = 8;

                        cellN = "H" + filaNuevo;
                        hoja.Cells[cellN].Value = c.GrupoCertificadoIngenieria.FechaGeneracion.ToShortDateString();
                        hoja.Cells[cellN].Style.WrapText = true;
                        hoja.Cells[cellN].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        hoja.Cells[cellN].Style.VerticalAlignment = ExcelVerticalAlignment.Center;



                        filaNuevo = 11;

                        cellN = "D" + filaNuevo;
                        hoja.Cells[cellN].Value = c.GrupoCertificadoIngenieria.FechaInicio.ToShortDateString() + " al " + c.GrupoCertificadoIngenieria.FechaFin.ToShortDateString();
                        hoja.Cells[cellN].Style.WrapText = true;
                        hoja.Cells[cellN].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        hoja.Cells[cellN].Style.VerticalAlignment = ExcelVerticalAlignment.Center;



                   

                        filaNuevo = 9;

                        cellN = "H" + filaNuevo;
                        hoja.Cells[cellN].Value = c.GrupoCertificadoIngenieria.FechaCertificado.ToShortDateString();
                        hoja.Cells[cellN].Style.WrapText = true;
                        hoja.Cells[cellN].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        hoja.Cells[cellN].Style.VerticalAlignment = ExcelVerticalAlignment.Center;





                        //#porcentaje avane real
                        decimal porcentajeAsbuiltActual = 0;
                        decimal porcentajeIBActual = 0;
                        decimal porcentajeIDActual = 0;

                        decimal porcentajeAsbuiltAnterior = 0;
                        decimal porcentajeIBAnterior = 0;
                        decimal porcentajeIDAnterior = 0;


                        decimal porcentaje_avance_real = 0;

                        double PorcentajeIB = 0.30; //Porcentaje Ingenieria Basica
                        double PorcentajeID = 0.70;//Porcentaje Ingenieria Detalle
                        var avanceRealIngenieriaFechaCertificado = _avanceProyectoRepository.GetAll()
                                                                     .Where(x => x.ProyectoId == c.ProyectoId)
                                                                     .Where(x => x.FechaCertificado <= c.GrupoCertificadoIngenieria.FechaCertificado)
                                                                     .OrderByDescending(x => x.FechaCertificado)
                                                                     .FirstOrDefault();




                        if (avanceRealIngenieriaFechaCertificado != null)
                        {

                            var valorIB = avanceRealIngenieriaFechaCertificado.AvanceRealActualIB * Convert.ToDecimal(PorcentajeIB);
                            var valorID = avanceRealIngenieriaFechaCertificado.AvanceRealActualID * Convert.ToDecimal(PorcentajeID);
                            porcentaje_avance_real = valorIB + valorID;

                            porcentajeAsbuiltActual = avanceRealIngenieriaFechaCertificado.AsbuiltActual;
                            porcentajeIBActual = avanceRealIngenieriaFechaCertificado.AvanceRealActualIB;
                            porcentajeIDActual = avanceRealIngenieriaFechaCertificado.AvanceRealActualID;


                        }
                        var grupoCertificadoAnterior = Repository.GetAll().Where(g => g.FechaCertificado < c.GrupoCertificadoIngenieria.FechaCertificado).OrderByDescending(g => g.FechaCertificado).FirstOrDefault();
                        if (grupoCertificadoAnterior != null)
                        {
                            var avanceRealIngenieriaFechaCertificadoAnterior = _avanceProyectoRepository.GetAll()
                                                                                                .Where(x => x.ProyectoId == c.ProyectoId)
                                                                                                .Where(x => x.FechaCertificado <= grupoCertificadoAnterior.FechaCertificado)
                                                                                                .OrderByDescending(x => x.FechaCertificado)
                                                                                                .FirstOrDefault();
                            if (avanceRealIngenieriaFechaCertificadoAnterior != null)
                            {
                                porcentajeAsbuiltAnterior = avanceRealIngenieriaFechaCertificadoAnterior.AsbuiltActual;
                                porcentajeIBAnterior = avanceRealIngenieriaFechaCertificadoAnterior.AvanceRealActualIB;
                                porcentajeIDAnterior = avanceRealIngenieriaFechaCertificadoAnterior.AvanceRealActualID;
                            }
                        }

       


                        var presupuestos = _presupuestoRepository.GetAll().Where(p => p.vigente)
                                                       .Where(p => p.ProyectoId == c.ProyectoId)
                                                       .Where(p => p.es_final)
                                                       //.Where(p => p.Requerimiento.tipo_requerimiento == TipoRequerimiento.Principal)
                                                       //.OrderByDescending(p => p.Id)
                                                       //.OrderByDescending(p => p.fecha_registro)
                                                       .Select(p => p.Id)
                                                       .ToList();




                        filaNuevo = 7;

                        cellN = "H" + filaNuevo;
                  //      hoja.Cells[cellN].Value = c.HorasPresupuestadas;
                        hoja.Cells[cellN].Style.WrapText = true;
                        hoja.Cells[cellN].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        hoja.Cells[cellN].Style.VerticalAlignment = ExcelVerticalAlignment.Center;



                                        #endregion


                        // Desgloce Horas Hombre y Personal Directo

                        var personalesDirecto = (from p in detalleCertificados
                                                 where p.TipoGastoId == TipoGasto.Directo
                                                 select p).ToList();

                        var personalDirectosAgrupados = (from t in personalesDirecto
                                                         where t.RubroId.HasValue
                                                         group t by new
                                                         {
                                                             t.ColaboradorId,
                                                             t.Rubro.ItemId
                                                         }
                                                        into g
                                                         select new
                                                         {
                                                             Grupo = g.Key,
                                                             SumaHoras = g.Sum(x => x.TotalHoras)
                                                         }).ToList();









                       var personalesDirectoAnterior = (from p in detallesAnteriores
                                                         where p.TipoGastoId == TipoGasto.Directo
                                                         select p).ToList();

                        var personalesIndirectoDirectoAnterior = (from p in detallesAnteriores
                                                                  where p.TipoGastoId == TipoGasto.Indirecto
                                                                  select p).ToList();

                        var TodosdirectosCertificados = _directosRepository.GetAllIncluding(i => i.Especialidad, i => i.Etapa)
                                                          .Where(i => i.CertificadoId.HasValue)
                                                          .Where(i => i.CertificadoId == c.Id)
                                                          .ToList();

                        var TodosdirectosCertificadosE500 = (from de500 in detalleCertificados
                                                             where de500.TipoGastoId == TipoGasto.Directo
                                                             where de500.EsDistribucionE500 == true
                                                             where de500.CertificadoIngenieriaProyectoId == c.Id
                                                             select de500
                                                             ).ToList();





                        var TodosdirectosCertificadosAnteriores = new List<DetallesDirectosIngenieria>();
                        var TodosdirectosCertificadosAnterioresE500 = new List<GastoDirectoCertificado>();


                        if (certificadoAnteriorProyectoListado.Count > 0)
                        {
                            foreach (var itecertificadoAnteriorProyecto in certificadoAnteriorProyectoListado)
                            {
                                var data = _directosRepository.GetAllIncluding(i => i.Especialidad, i => i.Etapa)
                                                                                       .Where(i => i.CertificadoId.HasValue)
                                                                                       .Where(i => i.CertificadoId == itecertificadoAnteriorProyecto.Id)
                                                                                       .ToList();
                                if (data.Count > 0)
                                {

                                    TodosdirectosCertificadosAnteriores.AddRange(data);
                                }

                                var datae500 = (from de500 in detallesAnteriores
                                                where de500.TipoGastoId == TipoGasto.Directo
                                                where de500.EsDistribucionE500 == true
                                                where de500.CertificadoIngenieriaProyectoId == itecertificadoAnteriorProyecto.Id
                                                select de500).ToList();


                                if (datae500.Count > 0)
                                {
                                    TodosdirectosCertificadosAnterioresE500.AddRange(datae500);
                                }

                            }
                        }


                        var personalesIndirectos = (from p in detalleCertificados
                                                    where p.TipoGastoId == TipoGasto.Indirecto
                                                    select p).ToList();


                        //Desgloce I-ING   I - PyCP





                        int FilaMayorIndirectos = 0;
                        var personalInDirectosAgrupadosUIO = (from t in personalesIndirectos
                                                              where t.RubroId.HasValue
                                                              where t.UbicacionTrabajo == CertificacionIngenieriaCodigos.UBICACION_UIO || t.UbicacionTrabajo == ""
                                                        
                                                              group t by new
                                                              {
                                                                  t.ColaboradorId,
                                                                  t.Rubro.ItemId
                                                              }
                                                        into g
                                                              select new
                                                              {
                                                                  Grupo = g.Key,
                                                                  SumaHoras = g.Sum(x => x.TotalHoras)
                                                              }).ToList();

                        var personalInDirectosAgrupadosOIT = (from t in personalesIndirectos
                                                              where t.RubroId.HasValue
                                                              where t.UbicacionTrabajo == CertificacionIngenieriaCodigos.UBICACION_CAMPO
                                                            
                                                              group t by new
                                                              {
                                                                  t.ColaboradorId,
                                                                  t.Rubro.ItemId
                                                              }
                                                       into g
                                                              select new
                                                              {
                                                                  Grupo = g.Key,
                                                                  SumaHoras = g.Sum(x => x.TotalHoras)
                                                              }).ToList();




                        int FilasPersonalUIO = personalInDirectosAgrupadosUIO.Count;
                        int FilasPersonalOIT = personalInDirectosAgrupadosOIT.Count;

          


                        var computos = new List<ComputoPresupuesto>();
                        if (presupuestos.Count > 0)
                        {
                            var computosPresupuesto = _computoPresupuesto.GetAllIncluding(x => x.Item)
                                                                         .Where(x => x.WbsPresupuesto.vigente)
                                                                       //   .Where(x => x.WbsPresupuesto.PresupuestoId == presupuesto.Id)
                                                                       .Where(x => presupuestos.Contains(x.WbsPresupuesto.PresupuestoId))
                                                                         .Where(x => x.vigente)
                                                                         .Where(x => x.Item.GrupoId == 1)
                                                                         .Where(x => x.Item.codigo.StartsWith("1.1") || x.Item.codigo.StartsWith("1.2"))
                                                                         .ToList();

                            if (computosPresupuesto.Count > 0)
                            {
                                computos.AddRange(computosPresupuesto);
                            }
                        }



                        
                        #region InicioDirectoNuevo

                        var FilaNDirecto = (from celda in hoja.Cells
                                            where celda.Value?.ToString().Contains("#DIRECTOINICIAL") == true
                                            select celda).FirstOrDefault(); ;
                        if (FilaNDirecto != null)
                        {



                            var finaInicial = FilaNDirecto.Start.Row;
                            var celda = "$B" + finaInicial;

                            int Filas_A_Aumentar = personalDirectosAgrupados.Count;
                            hoja.InsertRow(finaInicial+1, Filas_A_Aumentar, finaInicial); //Aumentar Filas de Directos 


                            foreach (var pd in personalDirectosAgrupados)
                            {
                                var Colaborador = _colaboradorRepository.GetAll().Where(x => x.Id == pd.Grupo.ColaboradorId).FirstOrDefault();
                                var Categoria = _itemRepository.GetAll().Where(x => x.Id == pd.Grupo.ItemId).FirstOrDefault();

                                celda = "$B" + finaInicial;
                                hoja.Cells[celda].Value = Categoria != null ? Categoria.codigo.ToUpper() : "";
                                hoja.Cells[celda].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                hoja.Cells[celda].Style.Font.Size = 10;
                                hoja.Cells[celda].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                                celda = "$C" + finaInicial;
                                hoja.Cells[celda].Value = Colaborador != null ? Colaborador.nombres_apellidos.ToUpper() : "";
                                hoja.Cells[celda].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                hoja.Cells[celda].Style.Font.Size = 10;
                                hoja.Cells[celda].Style.Border.BorderAround(ExcelBorderStyle.Thin);


                                celda = "$D" + finaInicial;
                                hoja.Cells[celda].Merge = true;
                                hoja.Cells[celda].Style.WrapText = true;
                                hoja.Cells[celda].Value = Categoria != null ? Categoria.nombre.ToUpper() : "";
                                hoja.Cells[celda].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                hoja.Cells[celda].Style.Font.Size = 10;
                                hoja.Cells[celda].Style.Border.BorderAround(ExcelBorderStyle.Thin);


                                celda = "$E" + finaInicial;
                                hoja.Cells[celda].Value = "HH";
                                hoja.Cells[celda].Style.Font.Size = 10;
                                hoja.Cells[celda].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                                celda = "$F" + finaInicial;
                                hoja.Cells[celda].Value = pd.SumaHoras;
                                hoja.Cells[celda].Style.Font.Size = 10;
                                hoja.Cells[celda].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                hoja.Cells[celda].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                hoja.Cells[celda].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                hoja.Cells[celda].Style.Numberformat.Format = "#,##0.00";

                     

                                if (Categoria != null)
                                {
                                    var dtoValoresTarifaItem = this.ObtenerValoresPresupuestoporCodigoItem(Categoria.codigo, c.Proyecto.contratoId, computos);

                                 
                                    celda = "$G" + finaInicial;
                                    hoja.Cells[celda].Value = dtoValoresTarifaItem.tarifa;
                                    hoja.Cells[celda].Style.Font.Size = 10;
                                    hoja.Cells[celda].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                    hoja.Cells[celda].Style.Numberformat.Format = "#,##0.00";

                                    celda = "$H" + finaInicial;
                                    hoja.Cells[celda].Value = pd.SumaHoras * dtoValoresTarifaItem.tarifa;
                                    hoja.Cells[celda].Style.Font.Size = 10;
                                    hoja.Cells[celda].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                    hoja.Cells[celda].Style.Numberformat.Format = "#,##0.00";
                                }

                                finaInicial++;

                            }

                        }


                        #endregion
                        #region InDirectoNuevo

                        var fIndirectoUIO = (from celda in hoja.Cells
                                             where celda.Value?.ToString().Contains("#UIOINICIAL") == true
                                             select celda).FirstOrDefault(); ;
                        if (fIndirectoUIO != null)
                        {


                            var finaInicial = fIndirectoUIO.Start.Row;
                            var celda = "$B" + finaInicial;


                            int Filas_A_Aumentar = personalInDirectosAgrupadosUIO.Count;
                            hoja.InsertRow(finaInicial + 1, Filas_A_Aumentar, finaInicial); //Aumen
                            foreach (var pd in personalInDirectosAgrupadosUIO)
                            {
                                var Colaborador = _colaboradorRepository.GetAll().Where(x => x.Id == pd.Grupo.ColaboradorId).FirstOrDefault();
                                var Categoria = _itemRepository.GetAll().Where(x => x.Id == pd.Grupo.ItemId).FirstOrDefault();

                                celda = "$B" + finaInicial;
                                hoja.Cells[celda].Value = Categoria != null ? Categoria.codigo.ToUpper() : "";
                                hoja.Cells[celda].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                hoja.Cells[celda].Style.Font.Size = 10;
                                hoja.Cells[celda].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                                celda = "$C" + finaInicial;
                                hoja.Cells[celda].Value = Colaborador != null ? Colaborador.nombres_apellidos.ToUpper() : "";
                                hoja.Cells[celda].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                hoja.Cells[celda].Style.Font.Size = 10;
                                hoja.Cells[celda].Style.Border.BorderAround(ExcelBorderStyle.Thin);



                                celda = "$D" + finaInicial;
                                hoja.Cells[celda].Merge = true;
                                hoja.Cells[celda].Style.WrapText = true;
                                hoja.Cells[celda].Value = Categoria != null ? Categoria.nombre.ToUpper() : "";
                                hoja.Cells[celda].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                hoja.Cells[celda].Style.Font.Size = 10;
                                hoja.Cells[celda].Style.Border.BorderAround(ExcelBorderStyle.Thin);


                                celda = "$F" + finaInicial;
                                hoja.Cells[celda].Value = pd.SumaHoras;
                                hoja.Cells[celda].Style.Font.Size = 10;
                                hoja.Cells[celda].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                hoja.Cells[celda].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                hoja.Cells[celda].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                hoja.Cells[celda].Style.Numberformat.Format = "#,##0.00";

                                celda = "$E" + finaInicial;
                                hoja.Cells[celda].Value = "HH";
                                hoja.Cells[celda].Style.Font.Size = 10;
                                hoja.Cells[celda].Style.Border.BorderAround(ExcelBorderStyle.Thin);
           

                          

                                if (Categoria != null)
                                {
                                    var dtoValoresTarifaItem = this.ObtenerValoresPresupuestoporCodigoItem(Categoria.codigo, c.Proyecto.contratoId, computos);

                                    celda = "$G" + finaInicial;
                                    hoja.Cells[celda].Value = dtoValoresTarifaItem.tarifa;
                                    hoja.Cells[celda].Style.Font.Size = 10;
                                    hoja.Cells[celda].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                    hoja.Cells[celda].Style.Numberformat.Format = "#,##0.00";

                                    celda = "$H" + finaInicial;
                                    hoja.Cells[celda].Value = pd.SumaHoras * dtoValoresTarifaItem.tarifa;
                                    hoja.Cells[celda].Style.Font.Size = 10;
                                    hoja.Cells[celda].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                    hoja.Cells[celda].Style.Numberformat.Format = "#,##0.00";

                                }
                                finaInicial++;
                            }
                            
                        }


                        #endregion

                        #region InDirectoNuevo OT

                        var fIndirectoOT = (from celda in hoja.Cells
                                            where celda.Value?.ToString().Contains("#OTINICIAL") == true
                                            select celda).FirstOrDefault(); ;
                        if (fIndirectoOT != null)
                        {


                            var finaInicial = fIndirectoOT.Start.Row;
                            var celda = "$B" + finaInicial;

                            int Filas_A_Aumentar = personalInDirectosAgrupadosOIT.Count;
                            hoja.InsertRow(finaInicial + 1, Filas_A_Aumentar, finaInicial); //Aumen
                            foreach (var pd in personalInDirectosAgrupadosOIT)
                            {
                                var Colaborador = _colaboradorRepository.GetAll().Where(x => x.Id == pd.Grupo.ColaboradorId).FirstOrDefault();
                                var Categoria = _itemRepository.GetAll().Where(x => x.Id == pd.Grupo.ItemId).FirstOrDefault();

                                celda = "$B" + finaInicial;
                                hoja.Cells[celda].Style.Font.Size = 10;
                                hoja.Cells[celda].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                hoja.Cells[celda].Value = Categoria != null ? Categoria.codigo.ToUpper() : "";
                                hoja.Cells[celda].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                                celda = "$C" + finaInicial;
                                hoja.Cells[celda].Style.Font.Size = 10;
                                hoja.Cells[celda].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                hoja.Cells[celda].Value = Colaborador != null ? Colaborador.nombres_apellidos.ToUpper() : "";
                                hoja.Cells[celda].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;



                                celda = "$D" + finaInicial;
                                hoja.Cells[celda].Merge = true;
                                hoja.Cells[celda].Style.WrapText = true;
                                hoja.Cells[celda].Style.Font.Size = 10;
                                hoja.Cells[celda].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                hoja.Cells[celda].Value = Categoria != null ? Categoria.nombre.ToUpper() : "";
                                hoja.Cells[celda].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;


                                celda = "$E" + finaInicial;
                                hoja.Cells[celda].Value = "HH";
                                hoja.Cells[celda].Style.Font.Size = 10;
                                hoja.Cells[celda].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                                if (Categoria.codigo == "1.3.1") {
                                    hoja.Cells[celda].Value = "EA";
                                }
                                else if (Categoria.codigo == "1.3.2")
                                {
                                    hoja.Cells[celda].Value = "día";

                                }
                                else if (Categoria.codigo == "1.3.3")
                                {
                                    hoja.Cells[celda].Value = "día";
                                }
                                else if (Categoria.codigo == "1.3.4")
                                {
                                    hoja.Cells[celda].Value = "día";
                                }


                                celda = "$F" + finaInicial;
                                hoja.Cells[celda].Value = pd.SumaHoras;
                                hoja.Cells[celda].Style.Font.Size = 10;
                                hoja.Cells[celda].Style.Numberformat.Format = "#,##0.00";
                                hoja.Cells[celda].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                                hoja.Cells[celda].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                hoja.Cells[celda].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                if (Categoria != null)
                                {
                                    var dtoValoresTarifaItem = this.ObtenerValoresPresupuestoporCodigoItem(Categoria.codigo, c.Proyecto.contratoId, computos);

                     

                                    celda = "$G" + finaInicial;
                                    hoja.Cells[celda].Value = dtoValoresTarifaItem.tarifa;
                                    hoja.Cells[celda].Style.Font.Size = 10;
                                    hoja.Cells[celda].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                    hoja.Cells[celda].Style.Numberformat.Format = "#,##0.00";

                                    celda = "$H" + finaInicial;
                                    hoja.Cells[celda].Value = pd.SumaHoras * dtoValoresTarifaItem.tarifa;
                                    hoja.Cells[celda].Style.Font.Size = 10;
                                    hoja.Cells[celda].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                    hoja.Cells[celda].Style.Numberformat.Format = "#,##0.00";

                                }
                                finaInicial++;
                            }
                          

                        }


                        #endregion



                        var fTotalHH = (from celda in hoja.Cells
                                        where celda.Value?.ToString().Contains("#AGRUPADOHH") == true
                                        select celda).FirstOrDefault(); ;
                        if (fTotalHH != null)
                        {
                            var finaInicial = fTotalHH.Start.Row;
                            var celda = "$H" + finaInicial;
                            var celdaAnterior = "$G" + finaInicial;

                            decimal hhActual = 0;
                            decimal hhAnterior = 0;

                            var actuales = (from d in detalleCertificados where !d.EsViatico select d.TotalHoras).ToList();
                            if (actuales.Count > 0) {
                                hhActual = actuales.Sum();

                            }
                            var anteriores = (from d in detallesAnteriores where !d.EsViatico select d.TotalHoras).ToList();
                            if (anteriores.Count > 0)
                            {
                                hhAnterior = anteriores.Sum();
                            }

                            hoja.Cells[celda].Value = hhActual;
                            hoja.Cells[celdaAnterior].Value = hhAnterior;
                        }

                        var fTotalUSD = (from celda in hoja.Cells
                                         where celda.Value?.ToString().Contains("#AGRUPADOUSD") == true
                                         select celda).FirstOrDefault(); ;
                        if (fTotalUSD != null)
                        {
                            var finaInicial = fTotalUSD.Start.Row;
                            var celda = "$H" + finaInicial;
                            var celdaAnterior = "$G" + finaInicial;


                            decimal hhActual = 0;
                            decimal hhAnterior = 0;

                            var actuales = (from d in detalleCertificados  select (d.Tarifa*d.TotalHoras)).ToList();
                            if (actuales.Count > 0)
                            {
                                hhActual = actuales.Sum();

                            
                            var anteriores = (from d in detallesAnteriores  select (d.Tarifa * d.TotalHoras)).ToList();
                            if (anteriores.Count > 0)
                            {
                                hhAnterior = anteriores.Sum();
                            }

                            hoja.Cells[celda].Value = hhActual;
                            hoja.Cells[celdaAnterior].Value = hhAnterior;


                        }

                            var hojaCompleta = hoja;
                            hojaCompleta.PrinterSettings.PaperSize = ePaperSize.A4;
                            hoja.PrinterSettings.Orientation = eOrientation.Portrait;
                            hoja.PrinterSettings.PrintArea = hoja.Cells[1, 2, hoja.Dimension.End.Row, 8];
                            hoja.PrinterSettings.FitToWidth = 1;
                            hoja.PrinterSettings.FitToHeight = 1;




                        }


                }



                }

            }
            return excel;
        }

    }
}

