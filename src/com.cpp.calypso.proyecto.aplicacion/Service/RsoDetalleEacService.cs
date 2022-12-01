using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.aplicacion.Dto;
using com.cpp.calypso.proyecto.aplicacion.Interfaces;
using com.cpp.calypso.proyecto.dominio;

namespace com.cpp.calypso.proyecto.aplicacion
{
    public class RsoDetalleEacAsyncBaseCrudAppService : AsyncBaseCrudAppService<RsoDetalleEac, RsoDetalleEacDto, PagedAndFilteredResultRequestDto>, IRsoDetalleEacAsyncBaseCrudAppService
    {
        private readonly IBaseRepository<RsoCabecera> _rdoCabeceraRepository;
        private readonly IBaseRepository<RdoCabecera> _RDO_CabeceraRepository;
        private readonly IBaseRepository<Computo> _computoRepository;
        private readonly IBaseRepository<DetalleAvanceObra> _detalleAvanceObraRepository;
        private readonly IBaseRepository<Ganancia> _gananciaRepository;
        private readonly IBaseRepository<DetalleGanancia> _detallegananciaRepository;
        private readonly IBaseRepository<RdoDetalleEac> _eacRDORepository;

        public RsoDetalleEacAsyncBaseCrudAppService(
            IBaseRepository<RsoDetalleEac> repository,
            IBaseRepository<RsoCabecera> rdoCabeceraRepository,
            IBaseRepository<Computo> computoRepository,
            IBaseRepository<DetalleAvanceObra> detalleAvanceObraRepository,
                   IBaseRepository<Ganancia> gananciaRepository,
            IBaseRepository<DetalleGanancia> detallegananciaRepository,
                  IBaseRepository<RdoCabecera> RDO_CabeceraRepository,
                   IBaseRepository<RdoDetalleEac> eacRDORepository
            ) : base(repository)
        {
            _rdoCabeceraRepository = rdoCabeceraRepository;
            _computoRepository = computoRepository;
            _detalleAvanceObraRepository = detalleAvanceObraRepository;
            _gananciaRepository = gananciaRepository;
            _detallegananciaRepository = detallegananciaRepository;
            _RDO_CabeceraRepository = RDO_CabeceraRepository;
            _eacRDORepository = eacRDORepository;
        }

        public List<RsoDetalleEacDto> Listar(int id)
        {
            var query = Repository.GetAll()
                .Where(o => o.vigente)
                .Where(o => o.RsoCabeceraId == id);

            var items = (from r in query
                         select new RsoDetalleEacDto()
                         {
                             Id = r.Id,
                             ComputoId = r.ComputoId,
                             ItemId = r.ItemId,
                             ac_actual = r.ac_actual,
                             ac_anterior = r.ac_anterior,
                             ac_diario = r.ac_diario,
                             RsoCabeceraId = r.RsoCabeceraId,
                             costo_presupuesto = r.costo_presupuesto,
                             costo_eac = r.costo_eac,
                             Computo = r.Computo,
                             cantidad_acumulada = r.cantidad_acumulada,
                             Item = r.Item,
                             porcentaje_presupuesto_total = r.porcentaje_presupuesto_total,
                             porcentaje_costo_eac_total = r.porcentaje_costo_eac_total,
                             cantidad_eac = r.cantidad_eac,
                             cantidad_planificada = r.cantidad_planificada,

                         }).ToList();

            return items;
        }

        public decimal CalcularRdoDetallesEAC(int RdoCabeceraId, DateTime fecha_reporte)
        {
            int precisionDecimales = 20;
            decimal PU = 1;
            decimal Ganancia = 1; //tiene que quedar 1.5619
            //Traigo la Ganancia
            decimal gananciacontruccion = 0;
            //calculo de ganancia
            var rdocabecera = _rdoCabeceraRepository.GetAllIncluding(c => c.Proyecto).Where(c => c.vigente).Where(c => c.Id == RdoCabeceraId).FirstOrDefault();
            var ganancia = _gananciaRepository.GetAll().Where(c => c.ContratoId == rdocabecera.Proyecto.contratoId)
                // .Where(c => c.fecha_inicio <= fecha_reporte) //Revisar aqui va fecha reporte
                // .Where(c => c.fecha_fin >= fecha_reporte)
                .Where(c => c.vigente == true).FirstOrDefault();

            if (ganancia != null && ganancia.Id > 0)
            {
                var detalleganancia = _detallegananciaRepository.GetAll().Where(c => c.GananciaId == ganancia.Id).
                     Where(c => c.vigente == true).ToList();

                if (detalleganancia.Count > 0)
                {

                    gananciacontruccion = (from e in detalleganancia where e.GrupoItemId == 2 select e.valor).Sum();

                }

            }

            Ganancia = Ganancia + gananciacontruccion / 100;


            var cabecera = _rdoCabeceraRepository.Get(RdoCabeceraId);

            //*/RDO Anterior/

            var RDOdeRDOSAnterior = _RDO_CabeceraRepository.GetAll().Where(c => c.vigente)
                                                         .Where(c => c.ProyectoId == cabecera.ProyectoId)
                                                         .Where(c => c.es_definitivo)
                                                         .Where(c => c.fecha_rdo <= fecha_reporte)
                                                        .OrderByDescending(c => c.fecha_rdo)
                                                         .FirstOrDefault();
            //RSO Anterior
            var rdoAnterior = _rdoCabeceraRepository.GetAll().Where(c => c.vigente)
                                                           .Where(c => c.ProyectoId == cabecera.ProyectoId)
                                                           .Where(c => c.es_definitivo)
                                                           .Where(c => c.fecha_rdo < fecha_reporte)
                                                          .OrderByDescending(c => c.fecha_rdo)
                                                           .FirstOrDefault();
            var detallesRDOdeRDOSAnterior = new List<RdoDetalleEac>();

            var detallesRDOAnterior = new List<RsoDetalleEac>();

            if (rdoAnterior != null)
            {

                var list = Repository.GetAll().Where(c => c.vigente)
                                            .Where(c => c.RsoCabeceraId == rdoAnterior.Id)
                                            .ToList();
                detallesRDOAnterior.AddRange(list);

            }
            else
            {

            }
            if (RDOdeRDOSAnterior != null)
            {

                var list = _eacRDORepository.GetAll().Where(c => c.vigente)
                                         .Where(c => c.RdoCabeceraId == RDOdeRDOSAnterior.Id)
                                         .ToList();
                detallesRDOdeRDOSAnterior.AddRange(list);
            }


            var computoQuery = _computoRepository.GetAllIncluding(o => o.Wbs.Oferta, o => o.Wbs, o => o.Item.Catalogo, o => o.Item.Especialidad, o => o.Item.Grupo)
                .Where(o => o.vigente == true)
                .Where(o => o.Wbs.Oferta.ProyectoId == cabecera.ProyectoId)
                .Where(o => o.Wbs.Oferta.es_final == true)
                .Where(o => o.Wbs.Oferta.vigente == true)
                .Where(o => o.Wbs.vigente)
                .Where(o => o.Item.GrupoId == 2)
                .Where(o => o.Wbs.Oferta.estado == 1);


            var computos = (from c in computoQuery
                            select new ComputoDto()
                            {
                                Id = c.Id,
                                ItemId = c.ItemId,
                                cantidad = c.cantidad,
                                costo_total = c.costo_total,
                                precio_unitario = c.precio_unitario,
                                Wbs = c.Wbs,
                                Item = c.Item,
                                cantidad_eac = c.cantidad_eac,
                                WbsId = c.WbsId,
                                codigo_especialidad = c.Item.Especialidad.codigo,
                                codigo_grupo = c.Item.Grupo.codigo,
                                es_temporal = c.es_temporal,
                                id_rubro_RDO = c.id_rubro_RDO

                            }).ToList();

            decimal sumatoria_costo_budget = 0;
            decimal sumatoria_costo_eac = 0;



            //Concepto Temporales a Nivel de Rubros Computo.

            if (detallesRDOdeRDOSAnterior.Count > 0) {

                foreach (var d in detallesRDOdeRDOSAnterior)
                {
                    sumatoria_costo_budget += Decimal.Round(d.costo_presupuesto, precisionDecimales);
                    sumatoria_costo_eac += Decimal.Round(d.costo_eac  , precisionDecimales);
                }
            }
            else { 

            foreach (var c in computos.ToList()) //Costo Prespuesto incluido los item Temporales
            {
                sumatoria_costo_budget += Decimal.Round(Decimal.Round(c.precio_unitario, 2) *
                                                        Decimal.Round(c.cantidad, precisionDecimales) *
                                                        Decimal.Round(Ganancia, 4)
                                                        , precisionDecimales);
                sumatoria_costo_eac += Decimal.Round(Decimal.Round(c.precio_unitario, 2) *
                                                        Decimal.Round(c.cantidad_eac, precisionDecimales) *
                                                        Decimal.Round(Ganancia, 4)
                                                        , precisionDecimales);
            }
            }

            var detalles = new List<RsoDetalleEac>();

            foreach (var c in computos)
            {
                var ComputoAnterior = (from ca in detallesRDOdeRDOSAnterior
                                       where ca.ComputoId == c.Id
                                       select ca
                                      ).FirstOrDefault();


                 

                //CANTIDAD ANTERIOr= CANTIDAD ACUMULADA DEL RDO ANTERIOR
                decimal cantidad_acumulada_r = rdoAnterior != null ? this.ObtenerCantidadAcumuladaAnteriorRDO(c.Id, fecha_reporte, cabecera.ProyectoId, detallesRDOAnterior) :
                                               RDOdeRDOSAnterior != null ? this.ObtenerCantidadAcumuladaAnteriorUltimoRDOAprobado(c.Id, fecha_reporte, cabecera.ProyectoId, detallesRDOdeRDOSAnterior) :
                                         Decimal.Round(
                                               this.ObtenerCantidadAcumuladaAnterior(c.Id, fecha_reporte, cabecera.ProyectoId) // DELOS AVANCES
                                               , 8);


                //ACTUAL AVANCES DIARIOS
                /*  decimal cantidad_actual_r = Decimal.Round(
                                               this.ObtenerCantidadActual(c.Id, fecha_reporte, cabecera.ProyectoId)
                                               , 8);
                                               */
                decimal cantidad_actual_r = rdoAnterior == null && RDOdeRDOSAnterior != null ? 
                                                (this.ObtenerCantidadAcumuladaAnteriorUltimoRDOAprobado(c.Id, fecha_reporte, cabecera.ProyectoId, detallesRDOdeRDOSAnterior) 
                                                 + this.ObtenerCantidadActualUltimoRdoAprobado(c.Id, fecha_reporte, cabecera.ProyectoId, RDOdeRDOSAnterior.fecha_rdo))


                                               : Decimal.Round(this.ObtenerCantidadActual(c.Id, fecha_reporte, cabecera.ProyectoId)               , 8);


                //CANTIDAD TOTAL REAL

                decimal cantidad_total_r =
                                           //c.cantidad_eac==0?0:    

                                           Decimal.Round(cantidad_acumulada_r + cantidad_actual_r, 8);




                //AC ANTERIR=Acumulado Anterior RDO ANterior
                decimal ac_anterior_r = rdoAnterior != null ? this.ACAnterior_RDOAnterior(c.Id, fecha_reporte, cabecera.ProyectoId, detallesRDOAnterior) :
                    RDOdeRDOSAnterior != null ? this.ACAnterior_RDOAnteriorRDOSUltimoAprobado(c.Id, fecha_reporte, cabecera.ProyectoId, detallesRDOdeRDOSAnterior) :
                                        Decimal.Round(Decimal.Round(c.precio_unitario, 2) * Decimal.Round(cantidad_acumulada_r, 8) * Decimal.Round(Ganancia, 4)
                                        , 8);


                //CANTIDAD ACUMULADA ACTUAL
                decimal ac_actual_r =
                    //c.cantidad_eac==0?0:                  
                    Decimal.Round(Decimal.Round(c.precio_unitario, 2) *
                            Decimal.Round(cantidad_total_r, 8) *
                             Decimal.Round(Ganancia, 4), 8);


                decimal ac_diario_r = Decimal.Round(ac_actual_r - ac_anterior_r, 8);




                decimal costo_presupuesto_r = ComputoAnterior!=null?Decimal.Round(c.precio_unitario * ComputoAnterior.cantidad_planificada * Ganancia
                                            , precisionDecimales):
                Decimal.Round(c.precio_unitario * c.cantidad * Ganancia
                                            , precisionDecimales);

                decimal costo_eac_r = ComputoAnterior != null ? Decimal.Round(c.precio_unitario * ComputoAnterior.cantidad_eac * Ganancia
                                       , precisionDecimales)

                                        : Decimal.Round(c.precio_unitario * c.cantidad_eac * Ganancia
                                       , precisionDecimales);



                // porcentaje_avance_anterio
                decimal porcentaje_avance_anterior_r = 0;


                if (costo_eac_r > 0)
                {

                    // PORCENTAJE AVANCE ANTERIOR= PORCENTAJE AVANCE ACTUAL RDO ANTERIOR
                    porcentaje_avance_anterior_r = Decimal.Round(

                                    (Decimal.Round(c.cantidad_eac, 2) == 0) ? 1 :
                                      rdoAnterior != null ? this.PorcentajeAvanceAnterior_RDOAnterior(c.Id, fecha_reporte, cabecera.ProyectoId, detallesRDOAnterior)
                                      :
                                      RDOdeRDOSAnterior != null ? this.PorcentajeAvanceAnterior_RDOAnteriorUltimaRDOAprobado(c.Id, fecha_reporte, cabecera.ProyectoId, detallesRDOdeRDOSAnterior) :

                                    (Decimal.Round(ac_anterior_r, 8) / Decimal.Round(costo_eac_r, 8))
                                   , 8);//si
                }


                if (c.cantidad_eac == 0)
                {
                    porcentaje_avance_anterior_r = 1; //
                }


                // porcentaje avance actual acumulado
                decimal porcentaje_avance_actual_acumularo_r = 0;
                if (costo_eac_r > 0)
                {

                    porcentaje_avance_actual_acumularo_r = ComputoAnterior != null ? ComputoAnterior.porcentaje_avance_actual_acumulado:

                    Decimal.Round((Decimal.Round(c.cantidad_eac, 2) == 0) ? 1 :
                        (Decimal.Round(ac_actual_r, 8) / Decimal.Round(costo_eac_r, 8)) , 8);
                }

                // Si cantidad EAC= porcentaje avance actual ==1
                if (c.cantidad_eac == 0)
                {


                    porcentaje_avance_actual_acumularo_r = 1;

                }


                //PORCENTAJE DE AVANCE DIARIO
                decimal porcentaje_avance_diario_r = 0;

                if (costo_eac_r > 0)
                {


                    porcentaje_avance_diario_r =ComputoAnterior!=null ? ComputoAnterior .porcentaje_avance_diario:


                    Decimal.Round(
                                     (Decimal.Round(c.cantidad_eac, 2) == 0) ? 0 :

                                     (Decimal.Round(porcentaje_avance_actual_acumularo_r, 8) - Decimal.Round(porcentaje_avance_anterior_r, 8)) //NUEVO
                                     , 8);
                }

                if (c.cantidad_eac == 0)
                {
                    porcentaje_avance_diario_r = 0; //
                }



                // porcentaje avance previsto acumulado
                decimal porcentaje_avance_previsto_acumulado_r = 0;

                if (fecha_reporte > c.Wbs.fecha_final.GetValueOrDefault())
                {
                    porcentaje_avance_previsto_acumulado_r = 1;
                }
                else if (fecha_reporte < c.Wbs.fecha_inicial.GetValueOrDefault())
                {
                    porcentaje_avance_previsto_acumulado_r = 0;
                }
                else
                {
                    var a = (fecha_reporte - c.Wbs.fecha_inicial.GetValueOrDefault()).Days;
                    var b = (c.Wbs.fecha_final.GetValueOrDefault() - c.Wbs.fecha_inicial.GetValueOrDefault()).Days;
                    if (b > 0)
                    {
                        porcentaje_avance_previsto_acumulado_r = Decimal.Round((Decimal.Round(a, 2) / Decimal.Round(b, 2)), 8);
                    }
                }


                var fecha_inicio_real_r = this.ObtenerFechaMinima(c.Id, cabecera.ProyectoId);
                if (!fecha_inicio_real_r.HasValue)
                {
                    fecha_inicio_real_r = null;
                }


                var fecha_fin_real_r = this.ObtenerFechaMaxima(c.Id, cabecera.ProyectoId);
                if (!fecha_fin_real_r.HasValue || cantidad_total_r >= c.cantidad_eac)
                {
                    fecha_fin_real_r = null;
                }
                if (cantidad_acumulada_r < c.cantidad_eac)
                {
                    fecha_fin_real_r = null;
                }

                if (porcentaje_avance_actual_acumularo_r < 1)
                {
                    fecha_fin_real_r = null;
                }
                else
                {
                    fecha_fin_real_r = this.ObtenerFechaMaxima(c.Id, cabecera.ProyectoId);
                }

                if (!c.Wbs.fecha_inicial.HasValue || !c.Wbs.fecha_final.HasValue)
                {

                    porcentaje_avance_previsto_acumulado_r = 0;
                }


                /* Costo Budget Version Anterior */
                decimal costo_budget_version_anterior =ComputoAnterior!=null?ComputoAnterior.costo_budget_version_anterior:
                                                        rdoAnterior != null ? this.CostoBudgetVersionRDOAnterior(c.Id, fecha_reporte, cabecera.ProyectoId, detallesRDOAnterior) :
                                                        RDOdeRDOSAnterior != null ? this.CostoBudgetVersionRDOAnteriorUltimoAprobado(c.Id, fecha_reporte, cabecera.ProyectoId, detallesRDOdeRDOSAnterior) : 0;


                decimal ev_actual_version_anterior = ComputoAnterior!=null? ComputoAnterior.ev_actual_version_anterior:
                                                    rdoAnterior != null ? this.EvActualVersionRDOAnterior
                    (c.Id, fecha_reporte, cabecera.ProyectoId, detallesRDOAnterior) :
                    RDOdeRDOSAnterior != null ? this.EvActualVersionRDOAnteriorUltimoAprobado(c.Id, fecha_reporte, cabecera.ProyectoId, detallesRDOdeRDOSAnterior) : 0;

                decimal ev_actual_Real = Decimal.Round(
                         Decimal.Round(costo_presupuesto_r, 8) * Decimal.Round(porcentaje_avance_actual_acumularo_r, 8)
                         , 8);


                var detalle = new RsoDetalleEac()
                {
                    RsoCabeceraId = RdoCabeceraId,
                    ItemId = c.ItemId,
                    codigo_preciario = c.Item.codigo,
                    nombre_actividad = c.Item.nombre,
                    presupuesto_total = c.cantidad,

                    costo_presupuesto = Decimal.Round(costo_presupuesto_r, precisionDecimales),
                    fecha_inicio_prevista = c.Wbs.fecha_inicial,
                    fecha_fin_prevista = c.Wbs.fecha_final,


                    cantidad_planificada = ComputoAnterior != null ? ComputoAnterior.cantidad_planificada : Decimal.Round(c.cantidad, 8),


                    porcentaje_presupuesto_total =

                    sumatoria_costo_budget > 0 ? Decimal.Round(Decimal.Round(costo_presupuesto_r, precisionDecimales) / Decimal.Round(sumatoria_costo_budget, precisionDecimales), precisionDecimales) : 0
                   ,

                    porcentaje_costo_eac_total =

                    sumatoria_costo_eac > 0 ? Decimal.Round(Decimal.Round(costo_eac_r, precisionDecimales) / Decimal.Round(sumatoria_costo_eac, precisionDecimales), precisionDecimales) : 0
                    ,



                    costo_eac = Decimal.Round(costo_eac_r, precisionDecimales),
                    cantidad_diaria = Decimal.Round(cantidad_actual_r, 8),
                    ac_diario = Decimal.Round(ac_diario_r, 8),
                    ac_anterior = Decimal.Round(ac_anterior_r, 8),
                    cantidad_acumulada = Decimal.Round(cantidad_total_r, 8),
                    ac_actual = Decimal.Round(ac_actual_r, 8),


                    /*ev_anterior = Decimal.Round(Decimal.Round(costo_presupuesto_r, 8)
                                    * Decimal.Round(porcentaje_avance_anterior_r, 8)
                                    , 8),*/
                    //EV ANTERIOR=EVACTUAL ANTERIOR RDO
                    ev_anterior = Decimal.Round(ev_actual_version_anterior, 8),


                    porcentaje_avance_anterior = Decimal.Round(porcentaje_avance_anterior_r, 8),
                    porcentaje_avance_diario = Decimal.Round(porcentaje_avance_diario_r, 8),

                    porcentaje_avance_actual_acumulado = Decimal.Round(porcentaje_avance_actual_acumularo_r, 8),
                    porcentaje_avance_previsto_acumulado = Decimal.Round(porcentaje_avance_previsto_acumulado_r, 8),
                    ev_actual = ev_actual_Real,

                    /*ev_diario = Decimal.Round(
                                         Decimal.Round(costo_presupuesto_r, 8) * Decimal.Round(porcentaje_avance_diario_r, 8)
                                         , 8),*/

                    //EV DIARIO= EV ACTUAL- EVANTERIOR
                    ev_diario = ev_actual_Real - ev_actual_version_anterior,

                    pv_costo_planificado = Decimal.Round(Decimal.Round(costo_presupuesto_r, 8) * Decimal.Round(porcentaje_avance_previsto_acumulado_r, 8), 8),

                    ComputoId = c.Id,
                    precio_unitario = Decimal.Round(c.precio_unitario, 2),
                    vigente = true,


                    cantidad_anterior = Decimal.Round(cantidad_acumulada_r, 8),


                    fecha_inicio_real = c.cantidad_eac == 0 ?
                                                              //c.Wbs.fecha_inicial.HasValue ? c.Wbs.fecha_inicial : 
                                                              null
                                                            : this.ObtenerFechaMinima(c.Id, cabecera.ProyectoId),
                    fecha_fin_real = c.cantidad_eac == 0 ?
                                                           //c.Wbs.fecha_inicial.HasValue ? c.Wbs.fecha_inicial :
                                                           null
                                                         : fecha_fin_real_r,
                    cantidad_eac = ComputoAnterior!=null? ComputoAnterior.cantidad_eac: Decimal.Round(c.cantidad_eac, precisionDecimales),
                    WbsId = c.WbsId,
                    UM = c.Item.Catalogo.nombre,
                    ern_value = ComputoAnterior != null ? ComputoAnterior.ern_value : Decimal.Round(Decimal.Round(costo_eac_r, 8) * Decimal.Round(porcentaje_avance_actual_acumularo_r, 8), 8),
                    ganancia = Decimal.Round(Ganancia, 4),

                    /*Pendiente de Aprobacion*/
                    PendienteAprobacion = c.Item.PendienteAprobacion,

                    /* Formato Second Format*/
                    codigo_especialidad = c.codigo_especialidad,
                    codigo_grupo = c.codigo_grupo,
                    es_temporal = c.es_temporal,

                    /*Costo BudgetRdoAnterior*/
                    costo_budget_version_anterior = costo_budget_version_anterior,
                    ev_actual_version_anterior = ev_actual_version_anterior,
                    id_rubro = c.id_rubro_RDO

                };
                if (detalle.cantidad_planificada == 0 && detalle.cantidad_eac == 0) { }
                else
                {
                    detalles.Add(detalle);
                    Repository.Insert(detalle);
                }
            }


            decimal avance_Actual_Acumulado = 0;
            var RdoFinal = _RDO_CabeceraRepository.GetAll()
                                                 .Where(c => c.fecha_rdo <= fecha_reporte)
                                                 .Where(c => c.es_definitivo)
                                                 .Where(c => c.ProyectoId == rdocabecera.ProyectoId)
                                                 .Where(c => c.vigente)
                                                 .OrderByDescending(c => c.fecha_rdo).FirstOrDefault();

            avance_Actual_Acumulado = RdoFinal != null ? RdoFinal.avance_real_acumulado : 0;
            return avance_Actual_Acumulado;

        }



        public decimal CalcularRdoDetallesEACRDOAnterior(int RdoCabeceraId, DateTime fecha_reporte)
        {
            return 0;

        }


        decimal ObtenerCantidadAcumuladaAnterior(int computoId, DateTime fecha_reporte, int proyectoId)
        {
            decimal cantidad_acumulada = 0;
            var query = _detalleAvanceObraRepository.GetAllIncluding(o => o.AvanceObra.Oferta)
                .Where(o => o.vigente == true)
                .Where(o => o.AvanceObra.Oferta.ProyectoId == proyectoId) // Agregado
                .Where(o => o.ComputoId == computoId)
                .Where(o => o.AvanceObra.aprobado == true)
                 .Where(o => o.AvanceObra.vigente == true)
                .Where(o => o.AvanceObra.fecha_presentacion < fecha_reporte);

            var detalles = (from d in query
                            select new DetalleAvanceObraDto()
                            {
                                cantidad_diaria = d.cantidad_diaria,
                                fecha_registro = d.fecha_registro,
                                total = d.total
                            }).ToList();

            foreach (var d in detalles)
            {
                cantidad_acumulada += Decimal.Round(d.cantidad_diaria, 8);
            }

            return Decimal.Round(cantidad_acumulada, 8);
        }


        decimal ObtenerCantidadAcumuladaAnteriorUltimoRDOAprobado(int computoId, DateTime fecha_reporte, int proyectoId, List<RdoDetalleEac> RdoAnterior)
        {
            decimal cantidadAnterior = 0;
            var query = (from o in RdoAnterior
                         where o.vigente
                         where o.ComputoId == computoId
                         select o).ToList();

            foreach (var d in query)
            {
                cantidadAnterior += Decimal.Round(d.cantidad_acumulada, 8);
            }

            return Decimal.Round(cantidadAnterior, 8);
        }

        decimal ObtenerCantidadAcumuladaAnteriorRDO(int computoId, DateTime fecha_reporte, int proyectoId, List<RsoDetalleEac> RdoAnterior)
        {
            decimal cantidadAnterior = 0;
            var query = (from o in RdoAnterior
                         where o.vigente
                         where o.ComputoId == computoId
                         select o).ToList();

            foreach (var d in query)
            {
                cantidadAnterior += Decimal.Round(d.cantidad_acumulada, 8);
            }

            return Decimal.Round(cantidadAnterior, 8);
        }



        decimal ACAnterior_RDOAnteriorRDOSUltimoAprobado(int computoId, DateTime fecha_reporte, int proyectoId, List<RdoDetalleEac> RdoAnterior)
        {
            decimal AcAnterior = 0;
            var query = (from o in RdoAnterior
                         where o.vigente
                         where o.ComputoId == computoId
                         select o).ToList();

            foreach (var d in query)
            {
                AcAnterior += Decimal.Round(d.ac_actual, 8);
            }

            return Decimal.Round(AcAnterior, 8);
        }



        decimal ACAnterior_RDOAnterior(int computoId, DateTime fecha_reporte, int proyectoId, List<RsoDetalleEac> RdoAnterior)
        {
            decimal AcAnterior = 0;
            var query = (from o in RdoAnterior
                         where o.vigente
                         where o.ComputoId == computoId
                         select o).ToList();

            foreach (var d in query)
            {
                AcAnterior += Decimal.Round(d.ac_actual, 8);
            }

            return Decimal.Round(AcAnterior, 8);
        }
        decimal EvAnterior_RDOAnterior(int computoId, DateTime fecha_reporte, int proyectoId, List<RsoDetalleEac> RdoAnterior)
        {
            decimal EvAnterior = 0;
            var query = (from o in RdoAnterior
                         where o.vigente
                         where o.ComputoId == computoId
                         select o).ToList();

            foreach (var d in query)
            {
                EvAnterior += Decimal.Round(d.ev_actual, 8);
            }

            return Decimal.Round(EvAnterior, 8);
        }



        decimal CostoBudgetVersionRDOAnterior(int computoId, DateTime fecha_reporte, int proyectoId, List<RsoDetalleEac> RdoAnterior)
        {
            decimal CostoBudgetVersionAnterior = 0;
            var query = (from o in RdoAnterior
                         where o.vigente
                         where o.ComputoId == computoId
                         select o).ToList();

            foreach (var d in query)
            {
                CostoBudgetVersionAnterior += Decimal.Round(d.costo_presupuesto, 8);
            }

            return Decimal.Round(CostoBudgetVersionAnterior, 8);
        }

        decimal CostoBudgetVersionRDOAnteriorUltimoAprobado(int computoId, DateTime fecha_reporte, int proyectoId, List<RdoDetalleEac> RdoAnterior)
        {
            decimal CostoBudgetVersionAnterior = 0;
            var query = (from o in RdoAnterior
                         where o.vigente
                         where o.ComputoId == computoId
                         select o).ToList();

            foreach (var d in query)
            {
                CostoBudgetVersionAnterior += Decimal.Round(d.costo_presupuesto, 8);
            }

            return Decimal.Round(CostoBudgetVersionAnterior, 8);
        }

        decimal EvActualVersionRDOAnterior(int computoId, DateTime fecha_reporte, int proyectoId, List<RsoDetalleEac> RdoAnterior)
        {
            decimal EvActualVersionAnterior = 0;
            var query = (from o in RdoAnterior
                         where o.vigente
                         where o.ComputoId == computoId
                         select o).ToList();

            foreach (var d in query)
            {
                EvActualVersionAnterior += Decimal.Round(d.ev_actual, 8);
            }

            return Decimal.Round(EvActualVersionAnterior, 8);
        }

        decimal EvActualVersionRDOAnteriorUltimoAprobado(int computoId, DateTime fecha_reporte, int proyectoId, List<RdoDetalleEac> RdoAnterior)
        {
            decimal EvActualVersionAnterior = 0;
            var query = (from o in RdoAnterior
                         where o.vigente
                         where o.ComputoId == computoId
                         select o).ToList();

            foreach (var d in query)
            {
                EvActualVersionAnterior += Decimal.Round(d.ev_actual, 8);
            }

            return Decimal.Round(EvActualVersionAnterior, 8);
        }


        decimal PorcentajeAvanceAnterior_RDOAnterior(int computoId, DateTime fecha_reporte, int proyectoId, List<RsoDetalleEac> RdoAnterior)
        {
            decimal procentajeAvanceAnterior = 0;
            var query = (from o in RdoAnterior
                         where o.vigente
                         where o.ComputoId == computoId
                         select o).ToList();

            foreach (var d in query)
            {
                procentajeAvanceAnterior += Decimal.Round(d.porcentaje_avance_actual_acumulado, 8);
            }
            return Decimal.Round(procentajeAvanceAnterior, 8);

        }

        decimal PorcentajeAvanceAnterior_RDOAnteriorUltimaRDOAprobado(int computoId, DateTime fecha_reporte, int proyectoId, List<RdoDetalleEac> RdoAnterior)
        {
            decimal procentajeAvanceAnterior = 0;
            var query = (from o in RdoAnterior
                         where o.vigente
                         where o.ComputoId == computoId
                         select o).ToList();

            foreach (var d in query)
            {
                procentajeAvanceAnterior += Decimal.Round(d.porcentaje_avance_actual_acumulado, 8);
            }
            return Decimal.Round(procentajeAvanceAnterior, 8);

        }



        decimal ObtenerCantidadActual(int computoId, DateTime fecha_reporte, int proyectoId)
        {
            decimal cantidad_acumulada = 0;
            var query = _detalleAvanceObraRepository.GetAllIncluding(o => o.AvanceObra.Oferta)
                .Where(o => o.vigente == true)
                .Where(o => o.AvanceObra.Oferta.ProyectoId == proyectoId)
                .Where(o => o.ComputoId == computoId)
                .Where(o => o.AvanceObra.aprobado == true)
                .Where(o => o.AvanceObra.vigente == true)
                .Where(o => o.AvanceObra.fecha_presentacion == fecha_reporte);

            var detalles = (from d in query
                            select new DetalleAvanceObraDto()
                            {
                                cantidad_diaria = d.cantidad_diaria,
                                fecha_registro = d.fecha_registro,
                                total = d.total
                            }).ToList();

            foreach (var d in detalles)
            {
                cantidad_acumulada += Decimal.Round(d.cantidad_diaria, 8);
            }

            return Decimal.Round(cantidad_acumulada, 8);
        }
        decimal ObtenerCantidadActualUltimoRdoAprobado(int computoId, DateTime fecha_reporte, int proyectoId, DateTime FechaRdoBase)
        {
            decimal cantidad_acumulada = 0;
            var query = _detalleAvanceObraRepository.GetAllIncluding(o => o.AvanceObra.Oferta)
                .Where(o => o.vigente == true)
                .Where(o => o.AvanceObra.Oferta.ProyectoId == proyectoId)
                .Where(o => o.ComputoId == computoId)
                .Where(o => o.AvanceObra.aprobado == true)
                .Where(o => o.AvanceObra.vigente == true)
                .Where(o => o.AvanceObra.fecha_presentacion > FechaRdoBase)
                .Where(o => o.AvanceObra.fecha_presentacion <= fecha_reporte);

            var detalles = (from d in query
                            select new DetalleAvanceObraDto()
                            {
                                cantidad_diaria = d.cantidad_diaria,
                                fecha_registro = d.fecha_registro,
                                total = d.total
                            }).ToList();

            foreach (var d in detalles)
            {
                cantidad_acumulada += Decimal.Round(d.cantidad_diaria, 8);
            }

            return Decimal.Round(cantidad_acumulada, 8);
        }



        public DateTime? ObtenerFechaMaxima(int computoId, int proyectoId)
        {
            var query = _detalleAvanceObraRepository.GetAllIncluding(o => o.AvanceObra.Oferta)
                .Where(o => o.vigente == true)
                .Where(o => o.ComputoId == computoId)
                .Where(o => o.AvanceObra.Oferta.ProyectoId == proyectoId)
                .Where(o => o.AvanceObra.Oferta.vigente) //Solo de Base RDO Vigentes
                .Where(o => o.AvanceObra.Oferta.es_final)//Solo de Base RDO Definitivas
                .Where(o => o.AvanceObra.vigente == true)
                .Where(o => o.AvanceObra.aprobado == true);

            var detalles = (from d in query
                            select d.AvanceObra.fecha_presentacion).Max();

            return detalles;
        }

        public DateTime? ObtenerFechaMinima(int computoId, int proyectoId)
        {
            var query = _detalleAvanceObraRepository.GetAllIncluding(o => o.AvanceObra.Oferta)
                .Where(o => o.vigente == true)
                .Where(o => o.AvanceObra.Oferta.ProyectoId == proyectoId)
                .Where(o => o.AvanceObra.Oferta.vigente) //Solo de Base RDO Vigentes
                .Where(o => o.AvanceObra.Oferta.es_final)//Solo de Base RDO Definitivas
                .Where(o => o.ComputoId == computoId)
                .Where(o => o.AvanceObra.vigente == true)
                .Where(o => o.AvanceObra.aprobado == true);

            var detalles = (from d in query
                            select d.AvanceObra.fecha_presentacion).Min();


            return detalles;
        }

        public int ActualizarRdoCabecera(int RdoCabeceraId, decimal avance_real_acumulado)
        {
            var RdoActualizado = _rdoCabeceraRepository.Get(RdoCabeceraId);
            RdoActualizado.avance_real_acumulado = Decimal.Round(avance_real_acumulado, 6);
            var IdNuevo = _rdoCabeceraRepository.Update(RdoActualizado);
            return RdoActualizado.Id;
        }



        public decimal CalcularRdoDetallesEAC_PENDIENTEAPROBACION_PRESUPUESTO_INDEPENDIENTE(int RdoCabeceraId, DateTime fecha_reporte)
        {
            int precisionDecimales = 20;
            decimal PU = 1;
            decimal Ganancia = 1; //tiene que quedar 1.5619
                                  //Traigo la Ganancia
            decimal gananciacontruccion = 0;

            var rdocabecera = _rdoCabeceraRepository.GetAllIncluding(c => c.Proyecto).Where(c => c.vigente).Where(c => c.Id == RdoCabeceraId).FirstOrDefault();

            /* Sección Obtención Ganancias*/
            Ganancia ganancia = null;
            var existeGananciaSNFecha = _gananciaRepository.GetAll().Where(c => c.ContratoId == rdocabecera.Proyecto.contratoId)
                                                         .Where(c => !c.fecha_fin.HasValue)
                                                         .Where(c => c.vigente)
                                                         .Where(c => fecha_reporte >= c.fecha_inicio)
                                                         .FirstOrDefault();
            if (existeGananciaSNFecha != null)
            {
                ganancia = existeGananciaSNFecha;
            }
            else
            {
                var gananciaConFechas = _gananciaRepository.GetAll().Where(c => c.ContratoId == rdocabecera.Proyecto.contratoId)
                     .Where(c => c.fecha_inicio <= fecha_reporte)
                     .Where(c => c.fecha_fin >= fecha_reporte).Where(c => c.vigente == true).FirstOrDefault();

                if (gananciaConFechas != null)
                {
                    ganancia = gananciaConFechas;
                }
            }
            /* FN*/

            /*var ganancia = _gananciaRepository.GetAll().Where(c => c.ContratoId == rdocabecera.Proyecto.contratoId)
                .Where(c => c.fecha_inicio <= fecha_reporte) //Revisar aqui va fecha reporte
                .Where(c => c.fecha_fin >= fecha_reporte).Where(c => c.vigente == true).FirstOrDefault();*/

            if (ganancia != null && ganancia.Id > 0)
            {
                var detalleganancia = _detallegananciaRepository.GetAll().Where(c => c.GananciaId == ganancia.Id).
                     Where(c => c.vigente == true).ToList();

                if (detalleganancia.Count > 0)
                {

                    gananciacontruccion = (from e in detalleganancia where e.GrupoItemId == 2 select e.valor).Sum();

                }

            }

            Ganancia = Ganancia + gananciacontruccion / 100;


            var cabecera = _rdoCabeceraRepository.Get(RdoCabeceraId);

            var computoQuery = _computoRepository.GetAllIncluding(o => o.Wbs.Oferta, o => o.Wbs, o => o.Item.Catalogo, o => o.Item.Especialidad, o => o.Item.Grupo)
                .Where(o => o.vigente == true)
                .Where(o => o.Wbs.Oferta.ProyectoId == cabecera.ProyectoId)
                .Where(o => o.Wbs.Oferta.es_final == true)
                .Where(o => o.Wbs.Oferta.vigente == true)
                .Where(o => o.Wbs.vigente)
                .Where(o => o.Item.GrupoId == 2)
                .Where(o => o.Wbs.Oferta.estado == 1);


            var computos = (from c in computoQuery
                            select new ComputoDto()
                            {
                                Id = c.Id,
                                ItemId = c.ItemId,
                                cantidad = c.cantidad,
                                costo_total = c.costo_total,
                                precio_unitario = c.precio_unitario,
                                Wbs = c.Wbs,
                                Item = c.Item,
                                cantidad_eac = c.cantidad_eac,
                                WbsId = c.WbsId,
                                codigo_especialidad = c.Item.Especialidad.codigo,
                                codigo_grupo = c.Item.Grupo.codigo,
                                es_temporal = c.es_temporal

                            }).ToList();

            decimal sumatoria_costo_budget = 0;
            decimal sumatoria_costo_eac = 0;

            decimal sumatoria_costo_budget_pendiente_aprobacion = 0;
            decimal sumatoria_costo_eac_pendiente_aprobacion = 0;

            //Concepto Temporales a Nivel de Rubros Computo.
            //foreach (var c in computos.Where(c => !c.Item.PendienteAprobacion).Where(c=>!c.es_temporal).ToList())  //Costo Presupuestado Sin Temporales



            foreach (var c in computos.Where(c => !c.Item.PendienteAprobacion).ToList()) //Costo Prespuesto incluido los item Temporales
            {
                sumatoria_costo_budget += Decimal.Round(Decimal.Round(c.precio_unitario, 2) *
                                                        Decimal.Round(c.cantidad, precisionDecimales) *
                                                        Decimal.Round(Ganancia, 4)
                                                        , precisionDecimales);
                sumatoria_costo_eac += Decimal.Round(Decimal.Round(c.precio_unitario, 2) *
                                                        Decimal.Round(c.cantidad_eac, precisionDecimales) *
                                                        Decimal.Round(Ganancia, 4)
                                                        , precisionDecimales);
            }
            foreach (var c in computos.Where(c => c.Item.PendienteAprobacion).ToList()) //Sumar Porcentaje solo items pendientes de aprobacion
            {
                sumatoria_costo_budget_pendiente_aprobacion += Decimal.Round(Decimal.Round(c.precio_unitario, 2) *
                                                        Decimal.Round(c.cantidad, precisionDecimales) *
                                                        Decimal.Round(Ganancia, 4)
                                                        , 8);
                sumatoria_costo_eac_pendiente_aprobacion += Decimal.Round(Decimal.Round(c.precio_unitario, 2) *
                                                        Decimal.Round(c.cantidad_eac, precisionDecimales) *
                                                        Decimal.Round(Ganancia, 4)
                                                        , precisionDecimales);
            }

            var detalles = new List<RsoDetalleEac>();

            foreach (var c in computos)
            {
                decimal cantidad_acumulada_r = Decimal.Round(
                                                this.ObtenerCantidadAcumuladaAnterior(c.Id, fecha_reporte, cabecera.ProyectoId)
                                                , 8);

                decimal cantidad_actual_r = Decimal.Round(
                                            this.ObtenerCantidadActual(c.Id, fecha_reporte, cabecera.ProyectoId)
                                            , 8);

                decimal cantidad_total_r = Decimal.Round(cantidad_acumulada_r + cantidad_actual_r, 8);

                decimal ac_anterior_r = Decimal.Round
                                        (Decimal.Round(c.precio_unitario, 2) *
                                         Decimal.Round(cantidad_acumulada_r, 8) *
                                          Decimal.Round(Ganancia, 4)
                                        , 8);


                decimal ac_diario_r = Decimal.Round(
                                             Decimal.Round(c.precio_unitario, 2) *
                                              Decimal.Round(cantidad_actual_r, 2) *
                                               Decimal.Round(Ganancia, 4)
                                               , 8);

                decimal costo_presupuesto_r = Decimal.Round(c.precio_unitario * c.cantidad * Ganancia
                                            , precisionDecimales);

                decimal costo_eac_r = Decimal.Round(c.precio_unitario * c.cantidad_eac * Ganancia
                                       , precisionDecimales);

                decimal ac_actual_r = Decimal.Round(Decimal.Round(c.precio_unitario, 2) *
                                     Decimal.Round(cantidad_total_r, 8) *
                                      Decimal.Round(Ganancia, 4), 8);

                // porcentaje_avance_anterio
                decimal porcentaje_avance_anterior_r = 0;


                if (costo_eac_r > 0)
                {
                    porcentaje_avance_anterior_r = Decimal.Round(

                                    (Decimal.Round(c.cantidad_eac, 2) == 0) ? 1 : (Decimal.Round(ac_anterior_r, 8) / Decimal.Round(costo_eac_r, 8))
                                   , 8);//si
                }
                if (c.cantidad_eac == 0)
                {
                    porcentaje_avance_anterior_r = 1; //
                }
                // porcentaje avance diario
                decimal porcentaje_avance_diario_r = 0;

                if (costo_eac_r > 0)
                {

                    porcentaje_avance_diario_r = Decimal.Round(
                                (Decimal.Round(c.cantidad_eac, 2) == 0) ? 0 : (Decimal.Round(ac_diario_r, 8) / Decimal.Round(costo_eac_r, 8))
                                , 8);
                }

                if (c.cantidad_eac == 0)
                {
                    porcentaje_avance_diario_r = 0; //
                }


                // porcentaje avance actual acumulado
                decimal porcentaje_avance_actual_acumularo_r = 0;
                if (costo_eac_r > 0)
                {

                    porcentaje_avance_actual_acumularo_r = Decimal.Round(

                                (Decimal.Round(c.cantidad_eac, 2) == 0) ? 1 : (Decimal.Round(ac_actual_r, 8) / Decimal.Round(costo_eac_r, 8))
                                , 8);
                }

                // Si cantidad EAC= porcentaje avance actual ==1
                if (c.cantidad_eac == 0)
                {


                    porcentaje_avance_actual_acumularo_r = 1;

                }

                // porcentaje avance previsto acumulado
                decimal porcentaje_avance_previsto_acumulado_r = 0;

                if (fecha_reporte > c.Wbs.fecha_final.GetValueOrDefault())
                {
                    porcentaje_avance_previsto_acumulado_r = 1;
                }
                else if (fecha_reporte < c.Wbs.fecha_inicial.GetValueOrDefault())
                {
                    porcentaje_avance_previsto_acumulado_r = 0;
                }
                else
                {
                    var a = (fecha_reporte - c.Wbs.fecha_inicial.GetValueOrDefault()).Days;
                    var b = (c.Wbs.fecha_final.GetValueOrDefault() - c.Wbs.fecha_inicial.GetValueOrDefault()).Days;
                    if (b > 0)
                    {
                        porcentaje_avance_previsto_acumulado_r = Decimal.Round((Decimal.Round(a, 2) / Decimal.Round(b, 2)), 8);
                    }
                }


                var fecha_inicio_real_r = this.ObtenerFechaMinima(c.Id, cabecera.ProyectoId);
                if (!fecha_inicio_real_r.HasValue)
                {
                    fecha_inicio_real_r = null;
                }


                var fecha_fin_real_r = this.ObtenerFechaMaxima(c.Id, cabecera.ProyectoId);
                if (!fecha_fin_real_r.HasValue || cantidad_total_r >= c.cantidad_eac)
                {
                    fecha_fin_real_r = null;
                }
                if (cantidad_acumulada_r < c.cantidad_eac)
                {
                    fecha_fin_real_r = null;
                }

                if (porcentaje_avance_actual_acumularo_r < 1)
                {
                    fecha_fin_real_r = null;
                }
                else
                {
                    fecha_fin_real_r = this.ObtenerFechaMaxima(c.Id, cabecera.ProyectoId);
                }

                if (!c.Wbs.fecha_inicial.HasValue || !c.Wbs.fecha_final.HasValue)
                {

                    porcentaje_avance_previsto_acumulado_r = 0;
                }




                var detalle = new RsoDetalleEac()
                {
                    RsoCabeceraId = RdoCabeceraId,
                    ItemId = c.ItemId,
                    codigo_preciario = c.Item.codigo,
                    nombre_actividad = c.Item.nombre,
                    presupuesto_total = c.cantidad,

                    costo_presupuesto = Decimal.Round(costo_presupuesto_r, precisionDecimales),
                    fecha_inicio_prevista = c.Wbs.fecha_inicial,
                    fecha_fin_prevista = c.Wbs.fecha_final,


                    cantidad_planificada = Decimal.Round(c.cantidad, 8),


                    porcentaje_presupuesto_total = !c.Item.PendienteAprobacion ?

                    sumatoria_costo_budget > 0 ? Decimal.Round(Decimal.Round(costo_presupuesto_r, precisionDecimales) / Decimal.Round(sumatoria_costo_budget, precisionDecimales), precisionDecimales) : 0
                    :
                     sumatoria_costo_budget_pendiente_aprobacion > 0 ? Decimal.Round(Decimal.Round(costo_presupuesto_r, precisionDecimales) / Decimal.Round(sumatoria_costo_budget_pendiente_aprobacion, precisionDecimales), precisionDecimales) : 0
                    ,

                    porcentaje_costo_eac_total = !c.Item.PendienteAprobacion ?

                    sumatoria_costo_eac > 0 ? Decimal.Round(Decimal.Round(costo_eac_r, precisionDecimales) / Decimal.Round(sumatoria_costo_eac, precisionDecimales), precisionDecimales) : 0
                    :
                     sumatoria_costo_eac_pendiente_aprobacion > 0 ? Decimal.Round(Decimal.Round(costo_eac_r, precisionDecimales) / Decimal.Round(sumatoria_costo_eac_pendiente_aprobacion, precisionDecimales), precisionDecimales) : 0
                    ,



                    costo_eac = Decimal.Round(costo_eac_r, precisionDecimales),
                    cantidad_diaria = Decimal.Round(cantidad_actual_r, 8),
                    ac_diario = Decimal.Round(ac_diario_r, 8),
                    ac_anterior = Decimal.Round(ac_anterior_r, 8),
                    cantidad_acumulada = Decimal.Round(cantidad_total_r, 8),
                    ac_actual = Decimal.Round(ac_actual_r, 8),
                    ev_anterior = Decimal.Round(Decimal.Round(costo_presupuesto_r, 8)
                                    * Decimal.Round(porcentaje_avance_anterior_r, 8)
                                    , 8),

                    porcentaje_avance_anterior = Decimal.Round(porcentaje_avance_anterior_r, 8),
                    porcentaje_avance_diario = Decimal.Round(porcentaje_avance_diario_r, 8),
                    ev_diario = Decimal.Round(
                                        Decimal.Round(costo_presupuesto_r, 8) * Decimal.Round(porcentaje_avance_diario_r, 8)
                                        , 8),
                    porcentaje_avance_actual_acumulado = Decimal.Round(porcentaje_avance_actual_acumularo_r, 8),
                    porcentaje_avance_previsto_acumulado = Decimal.Round(porcentaje_avance_previsto_acumulado_r, 8),
                    ev_actual = Decimal.Round(
                         Decimal.Round(costo_presupuesto_r, 8) * Decimal.Round(porcentaje_avance_actual_acumularo_r, 8)
                         , 8),


                    pv_costo_planificado = Decimal.Round(Decimal.Round(costo_presupuesto_r, 8) * Decimal.Round(porcentaje_avance_previsto_acumulado_r, 8), 8),

                    ComputoId = c.Id,
                    precio_unitario = Decimal.Round(c.precio_unitario, 2),
                    vigente = true,
                    cantidad_anterior = Decimal.Round(cantidad_acumulada_r, 8),
                    fecha_inicio_real = c.cantidad_eac == 0 ?
                                                              //c.Wbs.fecha_inicial.HasValue ? c.Wbs.fecha_inicial : 
                                                              null
                                                            : this.ObtenerFechaMinima(c.Id, cabecera.ProyectoId),
                    fecha_fin_real = c.cantidad_eac == 0 ?
                                                           //c.Wbs.fecha_inicial.HasValue ? c.Wbs.fecha_inicial :
                                                           null
                                                         : fecha_fin_real_r,
                    cantidad_eac = Decimal.Round(c.cantidad_eac, precisionDecimales),
                    WbsId = c.WbsId,
                    UM = c.Item.Catalogo.nombre,
                    ern_value = Decimal.Round(Decimal.Round(costo_eac_r, 8) * Decimal.Round(porcentaje_avance_actual_acumularo_r, 8), 8),
                    ganancia = Decimal.Round(Ganancia, 4),

                    /*Pendiente de Aprobacion*/
                    PendienteAprobacion = c.Item.PendienteAprobacion,

                    /* Formato Second Format*/
                    codigo_especialidad = c.codigo_especialidad,
                    codigo_grupo = c.codigo_grupo,

                    es_temporal = c.es_temporal

                };
                if (detalle.cantidad_planificada == 0 && detalle.cantidad_eac == 0) { }
                else
                {
                    detalles.Add(detalle);
                    Repository.Insert(detalle);
                }
            }


            decimal avance_Actual_Acumulado = 0;

            decimal ev_actual = Decimal.Round((from c in detalles
                                                   /* Suma sin items pendientes de Aprobacion*/
                                               where !c.PendienteAprobacion
                                               select Decimal.Round(c.ev_actual, 8)).Sum(), 8);
            decimal costo_presupuesto = Decimal.Round((from c in detalles
                                                           /* Suma sin items pendientes de Aprobacion*/
                                                       where !c.PendienteAprobacion
                                                       select Decimal.Round(c.costo_presupuesto, precisionDecimales)).Sum(), precisionDecimales);

            if (costo_presupuesto > 0)
            {
                avance_Actual_Acumulado = Decimal.Round(ev_actual / costo_presupuesto, 8);
            }

            return avance_Actual_Acumulado;

        }

    }
}