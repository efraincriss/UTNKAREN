using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.aplicacion.Interfaces;
using com.cpp.calypso.proyecto.dominio;

namespace com.cpp.calypso.proyecto.aplicacion
{
    public class RdoDetalleServiceAsyncBaseCrudAppService :
        AsyncBaseCrudAppService<RdoDetalle, RdoDetalleDto, PagedAndFilteredResultRequestDto>,
        IRdoDetalleAsyncBaseCrudAppService
    {
        private readonly IBaseRepository<RdoCabecera> _rdoCabeceraRepository;
        private readonly IBaseRepository<Computo> _computoRepository;
        private readonly IBaseRepository<AvanceObra> _avanceObraRepository;
        private readonly IBaseRepository<DetalleAvanceObra> _detalleAvanceObraRepository;
        private readonly IBaseRepository<Ganancia> _gananciaRepository;
        private readonly IBaseRepository<DetalleGanancia> _detallegananciaRepository;

        public RdoDetalleServiceAsyncBaseCrudAppService(
            IBaseRepository<RdoDetalle> repository,
            IBaseRepository<RdoCabecera> rdoCabeceraRepository,
            IBaseRepository<Computo> computoRepository,
            IBaseRepository<AvanceObra> avanceObraRepository,
            IBaseRepository<DetalleAvanceObra> detalleAvanceObraRepository,
            IBaseRepository<Ganancia> gananciaRepository,
            IBaseRepository<DetalleGanancia> detallegananciaRepository
        ) : base(repository)
        {
            _rdoCabeceraRepository = rdoCabeceraRepository;
            _computoRepository = computoRepository;
            _avanceObraRepository = avanceObraRepository;
            _detalleAvanceObraRepository = detalleAvanceObraRepository;
            _gananciaRepository = gananciaRepository;
           _detallegananciaRepository = detallegananciaRepository;
        }

        public RdoDetalleDto GetDetalles(int RdoDetalleId)
        {
            var rdoquery = Repository.GetAll().Where(e => e.vigente == true).ToList();
            RdoDetalleDto item = (from c in rdoquery

                where c.Id == RdoDetalleId
                select new RdoDetalleDto
                {
                    Id = c.Id,
                   Computo = c.Computo,
                    Item = c.Item,
                    RdoCabecera = c.RdoCabecera,
                    ComputoId = c.ComputoId,
                    ItemId = c.ItemId,
                    RdoCabeceraId = c.RdoCabeceraId,
                    ac_actual = c.ac_actual,
                    ac_anterior = c.ac_anterior,
                    ac_diario = c.ac_diario,
                    cantidad_acumulada = c.cantidad_acumulada,
                    cantidad_anterior = c.cantidad_anterior,
                    cantidad_diaria = c.cantidad_diaria,
                    cantidad_planificada = c.cantidad_planificada,
                    costo_eac = c.costo_eac,
                     costo_presupuesto = c.costo_presupuesto,
                    ev_actual = c.ev_actual,
                    ev_anterior = c.ev_anterior,
                    ev_diario = c.ev_diario,
                    fecha_fin_prevista = c.fecha_fin_prevista,
                    fecha_fin_real = c.fecha_fin_real,
                    fecha_inicio_prevista = c.fecha_inicio_prevista,
                    fecha_inicio_real = c.fecha_inicio_real,
                    porcentaje_avance_actual_acumulado = c.porcentaje_avance_actual_acumulado,
                    porcentaje_avance_anterior = c.porcentaje_avance_anterior,
                    porcentaje_avance_diario = c.porcentaje_avance_diario,
                    porcentaje_avance_previsto_acumulado = c.porcentaje_avance_previsto_acumulado,
                    porcentaje_costo_eac_total = c.porcentaje_costo_eac_total,
                    porcentaje_presupuesto_total = c.porcentaje_presupuesto_total,
                    precio_unitario = c.precio_unitario,
                    presupuesto_total = c.presupuesto_total,
                    pv_costo_planificado = c.pv_costo_planificado,
                    vigente = c.vigente

                }).FirstOrDefault();

            return item;
        }

        public void CalcularRdoDetalles(int RdoCabeceraId, DateTime fecha_reporte)
        {
            var PU = 1;
            decimal Ganancia = 1; //tiene que quedar 1.5619


            //Traigo la Ganancia
            decimal gananciacontruccion = 0;
          


            //calculo de ganancia

            var rdocabecera = _rdoCabeceraRepository.GetAllIncluding(c=>c.Proyecto).Where(c => c.vigente).Where(c => c.Id == RdoCabeceraId).FirstOrDefault();
                var ganancia = _gananciaRepository.GetAll().Where(c => c.ContratoId == rdocabecera.Proyecto.contratoId)
                    .Where(c => c.fecha_inicio <= fecha_reporte) //Revisar aqui va fecha reporte
                    .Where(c => c.fecha_fin >= fecha_reporte).Where(c => c.vigente == true).FirstOrDefault();

                if (ganancia != null && ganancia.Id > 0)
                {
                    var detalleganancia = _detallegananciaRepository.GetAll().Where(c => c.GananciaId == ganancia.Id).
                         Where(c => c.vigente == true).ToList();

                    if (detalleganancia.Count > 0)
                    {

                        gananciacontruccion = (from e in detalleganancia where e.GrupoItemId == 2 select e.valor).Sum();

                      }

                }

            Ganancia = Ganancia + gananciacontruccion/100;




                var cabecera = _rdoCabeceraRepository.Get(RdoCabeceraId);

            var computoQuery = _computoRepository.GetAllIncluding(o => o.Wbs.Oferta, o => o.Wbs, o => o.Item.Catalogo)
                .Where(o => o.vigente == true)
                .Where(o => o.Wbs.Oferta.ProyectoId == cabecera.ProyectoId)
                .Where(o => o.Wbs.Oferta.es_final == true)
                .Where(o => o.Wbs.Oferta.vigente == true)
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
                    WbsId = c.WbsId
                    
                }).ToList();

            decimal sumatoria_costo_budget = 0;
            decimal sumatoria_costo_eac = 0;
            foreach (var c in computos)
            {

                sumatoria_costo_budget += (c.precio_unitario * c.cantidad * Ganancia);
                sumatoria_costo_eac += (c.precio_unitario * c.cantidad_eac * Ganancia);
            }

            if (sumatoria_costo_budget == 0)
            {
                sumatoria_costo_budget = 1;
            }

            if (sumatoria_costo_eac == 0)
            {
                sumatoria_costo_eac = 1;
            }

            var detalles = new List<RdoDetalle>();

            foreach (var c in computos)
            {
                var cantidad_acumulada_r = this.ObtenerCantidadAcumuladaAnterior(c.Id, fecha_reporte, cabecera.ProyectoId);
                var cantidad_actual_r = this.ObtenerCantidadActual(c.Id, fecha_reporte, cabecera.ProyectoId);
                var cantidad_total_r = cantidad_acumulada_r + cantidad_actual_r;
                var ac_anterior_r = (c.precio_unitario * cantidad_acumulada_r * Ganancia);
                var ac_diario_r = (c.precio_unitario * cantidad_actual_r * Ganancia);
                var costo_presupuesto_r = c.precio_unitario * c.cantidad * Ganancia;



                var costo_eac_r = c.precio_unitario * c.cantidad_eac * Ganancia;
                var ac_actual_r = (c.precio_unitario * cantidad_total_r * Ganancia);


                // porcentaje_avance_anterior
                
                var porcentaje_avance_anterior_r  = (c.cantidad_eac == 0) ? 0 : 
                                                    (ac_anterior_r / costo_presupuesto_r); ;
 

                // porcentaje avance diario
                var porcentaje_avance_diario_r = (c.cantidad_eac == 0) ? 0 : (ac_diario_r / costo_presupuesto_r);

                // porcentaje avance actual acumulado
                var porcentaje_avance_actual_acumularo_r = (c.cantidad_eac == 0) ? 0 : (ac_actual_r / costo_presupuesto_r);

                // porcentaje avance previsto acumulado
                var porcentaje_avance_previsto_acumulado_r = 0;
                if (!c.Wbs.fecha_final.HasValue || !c.Wbs.fecha_inicial.HasValue)
                {
                    porcentaje_avance_previsto_acumulado_r = 0;
                } else if (fecha_reporte > c.Wbs.fecha_final.GetValueOrDefault())
                {
                    porcentaje_avance_previsto_acumulado_r = 100;
                } else if (fecha_reporte < c.Wbs.fecha_inicial.GetValueOrDefault())
                {
                    porcentaje_avance_previsto_acumulado_r = 0;
                }
                else
                {
                    var a = (fecha_reporte - c.Wbs.fecha_inicial.GetValueOrDefault()).Days;
                    var b = (c.Wbs.fecha_final.GetValueOrDefault() - c.Wbs.fecha_inicial.GetValueOrDefault()).Days;
                    porcentaje_avance_previsto_acumulado_r = a / b;
                }

                var fecha_inicio_real_r = this.ObtenerFechaMinima(c.Id, cabecera.ProyectoId);
                if (!fecha_inicio_real_r.HasValue)
                {
                    fecha_inicio_real_r = null;
                }

                var fecha_fin_real_r = this.ObtenerFechaMaxima(c.Id, cabecera.ProyectoId);
                if (!fecha_fin_real_r.HasValue || cantidad_total_r >= c.cantidad)
                {
                    fecha_fin_real_r = null;
                }

                if (cantidad_acumulada_r < c.cantidad_eac)
                {
                    fecha_fin_real_r = null;
                }

                var detalle = new RdoDetalle()
                {
                    RdoCabeceraId = RdoCabeceraId,
                    ItemId = c.ItemId,
                    codigo_preciario = c.Item.codigo,
                    nombre_actividad = c.Item.nombre,
                    presupuesto_total = c.cantidad,
                    costo_presupuesto = costo_presupuesto_r,
                    fecha_inicio_prevista = c.Wbs.fecha_inicial,
                    fecha_fin_prevista = c.Wbs.fecha_final,
                    porcentaje_presupuesto_total = costo_presupuesto_r / sumatoria_costo_budget,
                    cantidad_planificada = cantidad_acumulada_r,
                    porcentaje_costo_eac_total = costo_eac_r / sumatoria_costo_eac,
                    costo_eac = costo_eac_r,
                    cantidad_diaria = cantidad_actual_r,
                    ac_diario = ac_diario_r,
                    ac_anterior = ac_anterior_r,
                    cantidad_acumulada = cantidad_total_r,
                    ac_actual = ac_actual_r,
                    ev_anterior = costo_presupuesto_r * porcentaje_avance_anterior_r,
                    porcentaje_avance_anterior = porcentaje_avance_anterior_r,
                    porcentaje_avance_diario = porcentaje_avance_diario_r,
                    ev_diario = costo_presupuesto_r * porcentaje_avance_diario_r,
                    porcentaje_avance_actual_acumulado = porcentaje_avance_actual_acumularo_r,
                    porcentaje_avance_previsto_acumulado = porcentaje_avance_previsto_acumulado_r,
                    ev_actual = costo_presupuesto_r * porcentaje_avance_actual_acumularo_r,
                    pv_costo_planificado = costo_presupuesto_r * porcentaje_avance_previsto_acumulado_r / 100,//*CAMBIO-A division 100

                    ern_value = costo_presupuesto_r * porcentaje_avance_previsto_acumulado_r,
                    ComputoId = c.Id,
                    precio_unitario = c.precio_unitario,
                    vigente = true,
                    cantidad_anterior = cantidad_acumulada_r,
                    fecha_inicio_real = this.ObtenerFechaMinima(c.Id, cabecera.ProyectoId),
                    fecha_fin_real = this.ObtenerFechaMaxima(c.Id, cabecera.ProyectoId),
                    cantidad_eac = c.cantidad_eac,
                    WbsId = c.WbsId,
                    UM = c.Item.Catalogo.nombre
                };
                //detalles.Add(detalle);
                Repository.Insert(detalle);
            }

            //var d = detalles;
        }

        decimal ObtenerCantidadAcumuladaAnterior(int computoId, DateTime fecha_reporte, int proyectoId)
        {
            decimal cantidad_acumulada = 0;
            var query = _detalleAvanceObraRepository.GetAllIncluding(o => o.AvanceObra.Oferta)
                .Where(o => o.vigente == true)
                .Where(o => o.AvanceObra.Oferta.ProyectoId == proyectoId) // Agregado
                .Where(o => o.ComputoId == computoId)
                .Where(o => o.AvanceObra.aprobado == true)
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
                cantidad_acumulada += d.cantidad_diaria;
            }
             
            return cantidad_acumulada;
        }

        decimal ObtenerCantidadActual(int computoId, DateTime fecha_reporte, int proyectoId)
        {
            decimal cantidad_acumulada = 0;
            var query = _detalleAvanceObraRepository.GetAllIncluding(o => o.AvanceObra.Oferta)
                .Where(o => o.vigente == true)
                .Where(o => o.AvanceObra.Oferta.ProyectoId == proyectoId)
                .Where(o => o.ComputoId == computoId)
                .Where(o => o.AvanceObra.aprobado == true)
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
                cantidad_acumulada += d.cantidad_diaria;
            }

            return cantidad_acumulada;
        }

        public DateTime? ObtenerFechaMaxima(int computoId, int proyectoId)
        {
            var query = _detalleAvanceObraRepository.GetAllIncluding(o => o.AvanceObra.Oferta)
                .Where(o => o.vigente == true)
                .Where(o => o.ComputoId == computoId)
                .Where(o => o.AvanceObra.Oferta.ProyectoId == proyectoId)
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
                .Where(o => o.ComputoId == computoId)
                .Where(o => o.AvanceObra.aprobado == true);

            var detalles = (from d in query
                select d.AvanceObra.fecha_presentacion).Min();

            return detalles;
        }

        public List<RdoDetalle> GetRdoDetalles(int RdoCabeceraId)
        {
           List<RdoDetalle>  lista= Repository.GetAllIncluding(
                c => c.RdoCabecera,c=>c.Computo,c=>c.Item).Where(e => e.vigente == true
                ).Where(a=>a.RdoCabeceraId==RdoCabeceraId).ToList();
            return lista;

        }
    }
}
