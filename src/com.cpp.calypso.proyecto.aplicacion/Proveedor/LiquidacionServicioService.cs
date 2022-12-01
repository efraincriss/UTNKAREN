using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.aplicacion.Acceso.Dto;
using com.cpp.calypso.proyecto.aplicacion.Proveedor.Dto;
using com.cpp.calypso.proyecto.aplicacion.Proveedor.Interfaces;
using com.cpp.calypso.proyecto.dominio;
using com.cpp.calypso.proyecto.dominio.Constantes;
using com.cpp.calypso.proyecto.dominio.Proveedor;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion.Proveedor
{
    public class LiquidacionServicioAsyncBaseCrudAppService : AsyncBaseCrudAppService<LiquidacionServicio, LiquidacionServicioDto, PagedAndFilteredResultRequestDto>, ILiquidacionServicioAsyncBaseCrudAppService
    {
        private readonly IBaseRepository<DetalleLiquidacion> _detallerepository;
        private readonly IBaseRepository<DetalleReserva> _detalleReservarepository;
        private readonly IBaseRepository<TarifaHotel> _tarifahotelrepository;
        private readonly IBaseRepository<ConsumoVianda> _consumoviandarepository;
        private readonly IBaseRepository<SolicitudVianda> _solicitudviandarepository;
        private readonly IBaseRepository<DistribucionVianda> _distribucionviandarepository;
        private readonly IBaseRepository<DetalleDistribucion> _detalledistribucionviandarepository;
        private readonly IBaseRepository<Consumo> _consumosrepository;
        private readonly IBaseRepository<ContratoProveedor> _contratoproveedorrepository;
        private readonly IBaseRepository<Catalogo> _catalogorepository;
   
        private readonly IBaseRepository<TipoOpcionComida> _tipoOpcionComida;

        public LiquidacionServicioAsyncBaseCrudAppService(
            IBaseRepository<DetalleLiquidacion> detallerepository,
            IBaseRepository<LiquidacionServicio> repository,
            IBaseRepository<DetalleReserva> detalleReservarepository,
              IBaseRepository<TarifaHotel> tarifahotelrepository,
            IBaseRepository<ConsumoVianda> consumoviandarepository,
            IBaseRepository<Consumo> consumosrepository,
            IBaseRepository<ContratoProveedor> contratoproveedorrepository,
            IBaseRepository<Catalogo> catalogorepository,
          
            IBaseRepository<TipoOpcionComida> tipoOpcionComida,

            IBaseRepository<SolicitudVianda> solicitudviandarepository,
            IBaseRepository<DistribucionVianda> distribucionviandarepository,
            IBaseRepository<DetalleDistribucion> detalledistribucionviandarepository
            ) : base(repository)
        {
            _detalleReservarepository = detalleReservarepository;
            _tarifahotelrepository = tarifahotelrepository;
            _consumoviandarepository = consumoviandarepository;
            _consumosrepository = consumosrepository;
            _detallerepository = detallerepository;
            _contratoproveedorrepository = contratoproveedorrepository;
            _catalogorepository = catalogorepository;
           
            _tipoOpcionComida = tipoOpcionComida;

            _solicitudviandarepository = solicitudviandarepository;
            _distribucionviandarepository = distribucionviandarepository;
            _detalledistribucionviandarepository = detalledistribucionviandarepository;
        }


        public string GenerarLiquidacionConsumo(int ContratoProveedorId, InputLiquidacionDto input, List<FormatLiquidacionConsumo> pendientes)
        {

            var servicioconsumo = _catalogorepository.GetAll()
                                                       .Where(c => c.vigente)
                                                       .Where(c => c.codigo == CatalogosCodigos.LIQUIDACION_ALMUERZO)
                                                       .FirstOrDefault();

            var existefechaDesde = Repository.GetAll().Where(c => input.FechaDesde >= c.FechaDesde && input.FechaDesde <= c.FechaHasta
                                               ).Where(c => c.TipoServicioId == servicioconsumo.Id)
                                               .Where(c => c.Estado != EstadoLiquidacion.Anulado)
                                         .ToList().Count;

            var existefechaHasta = Repository.GetAll().Where(c => input.FechaHasta >= c.FechaDesde && input.FechaHasta <= c.FechaHasta
                                              ).Where(c => c.TipoServicioId == servicioconsumo.Id)
                                                 .Where(c => c.Estado != EstadoLiquidacion.Anulado)
                                         .ToList().Count;

            if (existefechaDesde > 0 || existefechaHasta > 0)
            {
                return "Ya existe una Liquidación Consumo generada con referencia a las fechas: " + input.FechaDesde.ToShortDateString() + " - " + input.FechaHasta.ToShortDateString();

            }
            else
            {

                LiquidacionServicio l = new LiquidacionServicio()
                {
                    Codigo = this.Nextcode(),
                    FechaDesde = input.FechaDesde,
                    FechaHasta = input.FechaHasta,
                    FechaPago = null,
                    ContratoProveedorId = ContratoProveedorId,
                    Estado = EstadoLiquidacion.Generado,
                    MontoConsumido = (from s in pendientes select s.Tarifa).Sum(),
                    TipoServicioId = servicioconsumo != null && servicioconsumo.Id > 0 ? servicioconsumo.Id : 0,

                };

                var LiquidacionId = Repository.InsertAndGetId(l);

                foreach (var p in pendientes)
                {
                    var Consumo = _consumosrepository.Get(p.Id);
                    if (Consumo != null)
                    {
                        try
                        {


                            DetalleLiquidacion dliquidacion = new DetalleLiquidacion()
                            {
                                LiquidacionId = LiquidacionId,
                                Valor = p.Tarifa,
                                Fecha = p.FechaConsumo != "dd/mm/aaaa" ? DateTime.Parse(p.FechaConsumo) : DateTime.Now,//Revisar Fecha de Consumo Obligatorio
                                Descripcion = p.TipoComida + "-" + p.OpcionComida + "-" + p.Identificacion + "-" + p.Nombres,

                            };
                            var DetalleLiquidacionId = _detallerepository.InsertAndGetId(dliquidacion);
                            //Actualizo Estados Liquidados
                            Consumo.liquidado = true;
                            Consumo.liquidacion_detalle_id = DetalleLiquidacionId;
                            Consumo.observacion = Consumo.observacion != null ? Consumo.observacion : ".";
                            var actualizado = _consumosrepository.Update(Consumo);
                        }
                        catch (DbEntityValidationException ee)
                        {

                        }
                    }
                }
                return "OK";
            }


        }

        public string GenerarLiquidacionHospedaje(int ContratoProveedorId, InputLiquidacionDto input, List<FormatLiquidacionReserva> pendientes)
        {

            var serviciohospedaje = _catalogorepository.GetAll()
                                                       .Where(c => c.vigente)
                                                       .Where(c => c.codigo == CatalogosCodigos.LIQUIDACION_HOSPEDAJE)
                                                       .FirstOrDefault();

            var existefechaDesde = Repository.GetAll().Where(c => input.FechaDesde >= c.FechaDesde && input.FechaDesde <= c.FechaHasta
                                                ).Where(c => c.TipoServicioId == serviciohospedaje.Id)
                                                   .Where(c => c.Estado != EstadoLiquidacion.Anulado)
                                                .ToList().Count;

            var existefechaHasta = Repository.GetAll().Where(c => input.FechaHasta >= c.FechaDesde && input.FechaHasta <= c.FechaHasta
                                                ).Where(c => c.TipoServicioId == serviciohospedaje.Id)
                                                   .Where(c => c.Estado != EstadoLiquidacion.Anulado)
                                         .ToList().Count;

            if (existefechaDesde > 0 || existefechaHasta > 0)
            {
                return "Ya existe una Liquidación Hospedaje generada con referencia a las fechas: " + input.FechaDesde.ToShortDateString() + " - " + input.FechaHasta.ToShortDateString();

            }
            else
            {

                LiquidacionServicio l = new LiquidacionServicio()
                {
                    Codigo = this.Nextcode(),
                    FechaDesde = input.FechaDesde,
                    FechaHasta = input.FechaHasta,
                    FechaPago = null,
                    ContratoProveedorId = ContratoProveedorId,
                    Estado = EstadoLiquidacion.Generado,
                    MontoConsumido = (from s in pendientes select s.Tarifa).Sum(),
                    TipoServicioId = serviciohospedaje != null && serviciohospedaje.Id > 0 ? serviciohospedaje.Id : 0,

                };

                var LiquidacionId = Repository.InsertAndGetId(l);

                foreach (var p in pendientes)
                {
                    var DetalleReserva = _detalleReservarepository.Get(p.Id);
                    if (DetalleReserva != null)
                    {

                        DetalleLiquidacion dliquidacion = new DetalleLiquidacion()
                        {
                            LiquidacionId = LiquidacionId,
                            Valor = p.Tarifa,
                            Fecha = p.FechaConsumo != "dd/mm/aaaa" ? DateTime.Parse(p.FechaConsumo) : DateTime.Now,//Revisar Fecha de Consumo Obligatorio
                            Descripcion = p.Tipo + "-" + p.Habitacion + "-" + p.Espacio + "-" + p.Identificacion + "-" + p.Nombres,

                        };
                        var DetalleLiquidacionId = _detallerepository.InsertAndGetId(dliquidacion);
                        //Actualizo Estados Liquidados
                        DetalleReserva.liquidado = true;
                        DetalleReserva.liquidacion_detalle_id = DetalleLiquidacionId;
                        var actualizado = _detalleReservarepository.Update(DetalleReserva);
                    }
                }
                return "OK";
            }

        }

        public string GenerarLiquidacionVianda(int ContratoProveedorId, InputLiquidacionDto input, List<FormatLiquidacionSolicitudVianda> pendientes)
        {

            var servicioconsumo = _catalogorepository.GetAll()
                                                       .Where(c => c.vigente)
                                                       .Where(c => c.codigo == CatalogosCodigos.LIQUIDACION_VIANDA)
                                                       .FirstOrDefault();
            var existefechaDesde = 0;

            var existefechaHasta = 0;
            /*var existefechaDesde = Repository.GetAll().Where(c => input.FechaDesde >= c.FechaDesde && input.FechaDesde <= c.FechaHasta
                                               ).Where(c => c.TipoServicioId == servicioconsumo.Id)
                                               .Where(c => c.Estado != EstadoLiquidacion.Anulado)
                                         .ToList().Count;

            var existefechaHasta = Repository.GetAll().Where(c => input.FechaHasta >= c.FechaDesde && input.FechaHasta <= c.FechaHasta
                                              ).Where(c => c.TipoServicioId == servicioconsumo.Id)
                                                 .Where(c => c.Estado != EstadoLiquidacion.Anulado)
                                         .ToList().Count;
            */
            if (existefechaDesde > 0 || existefechaHasta > 0)
            {
                return "Ya existe una Liquidación Consumo generada con referencia a las fechas: " + input.FechaDesde.ToShortDateString() + " - " + input.FechaHasta.ToShortDateString();

            }
            else
            {

                LiquidacionServicio l = new LiquidacionServicio()
                {
                    Codigo = this.Nextcode(),
                    FechaDesde = input.FechaDesde,
                    FechaHasta = input.FechaHasta,
                    FechaPago = null,
                    ContratoProveedorId = ContratoProveedorId,
                    Estado = EstadoLiquidacion.Generado,
                    MontoConsumido = (from s in pendientes select s.Tarifa).Sum(),
                    TipoServicioId = servicioconsumo != null && servicioconsumo.Id > 0 ? servicioconsumo.Id : 0,

                };

                var LiquidacionId = Repository.InsertAndGetId(l);

                foreach (var p in pendientes)
                {
                    try
                    {

                        var SolicitudVianda = _solicitudviandarepository.GetAllIncluding(c => c.solicitante).Where(c => c.Id == p.Id).FirstOrDefault();
                        if (SolicitudVianda != null)
                        {

                            DetalleLiquidacion dliquidacion = new DetalleLiquidacion()
                            {
                                LiquidacionId = LiquidacionId,
                                Valor = p.Tarifa,
                                Fecha = p.FechaConsumo != null ? DateTime.Parse(p.FechaConsumo) : DateTime.Now,//Revisar Fecha de Consumo Obligatorio
                                Descripcion = p.IdSolicitante + "-" + p.TipoComida + "-" + SolicitudVianda.solicitante.numero_identificacion + "-" + SolicitudVianda.solicitante.nombres_apellidos,

                            };
                            var DetalleLiquidacionId = _detallerepository.InsertAndGetId(dliquidacion);

                            //Actualizo Estados Liquidados
                            SolicitudVianda.liquidado = true;
                            SolicitudVianda.liquidacion_detalle_id = DetalleLiquidacionId;
                            var actualizado = _solicitudviandarepository.Update(SolicitudVianda);

                        }
                    }
                    catch (DbEntityValidationException ee)
                    {

                    }

                }
                return "OK";
            }
        }

        public LiquidacionServicioDto GetDetalles(int id)
        {
            var detalle = Repository.GetAll().Where(c => c.Id == id).FirstOrDefault();
            var liquidacion = AutoMapper.Mapper.Map<LiquidacionServicio, LiquidacionServicioDto>(detalle);
            liquidacion.MontoConsumido = _detallerepository.GetAll().Select(c => c.Valor).ToList().Sum();


            return liquidacion;
        }

        public List<FormatLiquidacionConsumo> ListaConsumosLiquidadas(InputLiquidacionDto input)
        {
            var consumo = _consumosrepository.GetAllIncluding(c => c.Proveedor)
                                                  .Where(c => c.ProveedorId == input.ProveedorId)
                                                  .Where(c => c.fecha >= input.FechaDesde)
                                                  .Where(c => c.fecha <= input.FechaHasta)
                                                   .Where(c => c.liquidado)
                                                  .ToList();

            var lista = AutoMapper.Mapper.Map<List<Consumo>, List<FormatLiquidacionConsumo>>(consumo);
            return lista;
        }

        public List<FormatLiquidacionConsumo> ListaConsumosPendientesdeLiquidacion(InputLiquidacionDto input)
        {
            var ContratoProveedorId = this.ObtenerContratoProveedor(input.ProveedorId);
            var TiposOpcionesComida = _tipoOpcionComida.GetAll().Where(c => c.ContratoId == ContratoProveedorId)
                                                               .ToList();

            var consumo = _consumosrepository.GetAllIncluding(c => c.Proveedor, c => c.colaborador, c => c.TipoComida, c => c.OpcionComida)
                                                  .Where(c => c.ProveedorId == input.ProveedorId)
                                                  .Where(c => c.fecha >= input.FechaDesde)
                                                  .Where(c => c.fecha <= input.FechaHasta)
                                                   .Where(c => c.liquidacion_detalle_id == 0)
                                                   .Where(c => !c.liquidado)
                                                  .ToList();

            var lista = AutoMapper.Mapper.Map<List<Consumo>, List<FormatLiquidacionConsumo>>(consumo);
            foreach (var l in lista)
            {
                var valor = (from v in TiposOpcionesComida
                             where v.opcion_comida_id == l.OpcionComidaId
                             where v.tipo_comida_id == l.TipoComidaId
                             where v.ContratoId == ContratoProveedorId
                             select v).FirstOrDefault();

                l.Tarifa = valor != null && valor.Id > 0 ? valor.costo : 0;

            }
            return lista;
        }

        public List<LiquidacionServicioDto> ListadoLiquidaciones()
        {
            var query = Repository.GetAll().ToList();
            var liquidaciones = (from q in query
                                 select new LiquidacionServicioDto()
                                 {
                                     Id = q.Id,
                                     Codigo = q.Codigo,
                                     ContratoProveedorId = q.ContratoProveedorId,
                                     NombreContratoProveedor = q.ContratoProveedor.codigo + " - " + q.ContratoProveedor.Proveedor.razon_social,
                                     FechaDesde = q.FechaDesde,
                                     FormatFechaDesde = q.FechaDesde.ToShortDateString(),
                                     FechaHasta = q.FechaHasta,
                                     FormatFechaHasta = q.FechaHasta.ToShortDateString(),
                                     FechaPago = q.FechaPago,
                                     FormatFechaPago = q.FechaPago.HasValue ? q.FechaPago.Value.ToShortDateString() : "",
                                     MontoConsumido = q.MontoConsumido,
                                     Estado = q.Estado,
                                     NombreEstado = Enum.GetName(typeof(EstadoLiquidacion), q.Estado).ToUpper(),
                                     TipoServicioId = q.TipoServicioId,
                                     NombreTipoServicio = q.TipoServicio.nombre
                                 }).ToList();

            return liquidaciones;


        }

        public List<FormatLiquidacionReserva> ListaReservasLiquidadas(InputLiquidacionDto input)
        {
            var ContratoProveedorId = this.ObtenerContratoProveedor(input.ProveedorId);
            var detallesreserva = _detalleReservarepository.GetAllIncluding(c => c.ReservaHotel.EspacioHabitacion.Habitacion.Proveedor,
                                                                                    c => c.ReservaHotel.Colaborador,
                                                                                    c => c.ReservaHotel.EspacioHabitacion.Habitacion.TipoHabitacion
                                                                                    )
                                                          .Where(c => c.ReservaHotel.EspacioHabitacion.Habitacion.ProveedorId == input.ProveedorId)
                                                  .Where(c => c.fecha_reserva.HasValue)
                                                  .Where(c => c.fecha_reserva >= input.FechaDesde)
                                                  .Where(c => c.fecha_reserva <= input.FechaHasta)
                                                  .Where(c => c.liquidado)
                                                  .ToList();
            var lista = AutoMapper.Mapper.Map<List<DetalleReserva>, List<FormatLiquidacionReserva>>(detallesreserva);
            if (ContratoProveedorId > 0)
            {
                foreach (var l in lista)
                {
                    l.Tarifa = this.ObtenerTarifaHotel(ContratoProveedorId, l.TipoHabitacionId);
                }
            }
            return lista;
        }

        public List<FormatLiquidacionReserva> ListaReservasPendientesdeLiquidacion(InputLiquidacionDto input)
        {
            var ContratoProveedorId = this.ObtenerContratoProveedor(input.ProveedorId);
            var detallesreserva = _detalleReservarepository.GetAllIncluding(c => c.ReservaHotel.EspacioHabitacion.Habitacion.Proveedor,
                                                                            c => c.ReservaHotel.Colaborador,
                                                                            c => c.ReservaHotel.EspacioHabitacion.Habitacion.TipoHabitacion
                                                                            )
                                                 .Where(c => c.ReservaHotel.EspacioHabitacion.Habitacion.ProveedorId == input.ProveedorId)
                                                 .Where(c => c.liquidacion_detalle_id == 0)
                                                 .Where(c => c.fecha_reserva.HasValue)
                                                 .Where(c => c.fecha_reserva >= input.FechaDesde)
                                                 .Where(c => c.fecha_reserva <= input.FechaHasta)
                                                 .Where(c => !c.liquidado)
                                                 .ToList();

            var lista = AutoMapper.Mapper.Map<List<DetalleReserva>, List<FormatLiquidacionReserva>>(detallesreserva);
            if (ContratoProveedorId > 0)
            {
                foreach (var l in lista)
                {
                    l.Tarifa = this.ObtenerTarifaHotel(ContratoProveedorId, l.TipoHabitacionId);
                }
            }
            return lista;
        }

        public List<FormatLiquidacionSolicitudVianda> ListaSolicitudesViandasLiquidadas(InputLiquidacionDto input)
        {
            var lista = new List<FormatLiquidacionSolicitudVianda>();

            var consumos_viandas = _consumoviandarepository.GetAll()
                                                  //.Where(c => c.ProveedorId == input.ProveedorId)
                                                  //.Where(c => c.fecha_consumo_vianda >= input.FechaDesde)
                                                  // .Where(c => c.fecha_consumo_vianda <= input.FechaHasta)
                                                  // .Where(c => c.liquidado)
                                                  .ToList();
            var solicitudesViandas = _solicitudviandarepository.GetAllIncluding(c => c.locacion,
                                                                                c => c.disciplina,
                                                                                c => c.area,
                                                                                c => c.tipo_comida,
                                                                                c => c.solicitante)
                                                                //.Where(c => c.estado == SolicitudViandaEstado.EntregadaAnotador)
                                                                // .Where(c => c.liquidacion_detalle_id == 0)
                                                                .Where(c => c.liquidado)
                                                                .ToList();

            var detalles_distribucions = _detalledistribucionviandarepository
                                                                            .GetAllIncluding(c => c.DistribucionVianda.Proveedor,
                                                                                             c => c.SolicitudVianda)

                                                                            .ToList();

            if (solicitudesViandas.Count > 0)
            {


                var item = (from s in solicitudesViandas
                            where s.fecha_solicitud.Date >= input.FechaDesde.Date
                            where s.fecha_solicitud.Date <= input.FechaHasta.Date
                            select s).ToList();
                if (item.Count > 0)
                {
                    var query = (from q in item
                                 select new FormatLiquidacionSolicitudVianda
                                 {
                                     Id = q.Id,
                                     IdSolicitante = q.solicitante_id.ToString(),
                                     NombreSolicitante = q.solicitante.nombres_apellidos,
                                     liquidado = q.liquidado,
                                     Locacion = q.locacion.nombre,
                                     TipoComida = q.tipo_comida.nombre,
                                     TotalSolicitado = q.total_pedido,
                                     FechaPedido=q.fecha_solicitud.ToShortDateString()

                                 }).ToList();
                    if (query.Count > 0)
                    {
                        foreach (var l in query)
                        {
                            var detalle = (from dd in detalles_distribucions where dd.SolicitudViandaId == l.Id select dd).FirstOrDefault();
                            if (detalle != null && detalle.Id > 0)
                            {
                                var datafecha = (from cc in consumos_viandas where cc.SolicitudViandaId == l.Id select cc).FirstOrDefault();
                                if (datafecha != null && datafecha.Id > 0)
                                {
                                    l.FechaConsumo = datafecha.fecha_consumo_vianda.ToShortDateString();
                                }
                                l.ProveedorId = detalle.DistribucionVianda.ProveedorId;
                                l.NombreProveedor = detalle.DistribucionVianda.Proveedor.razon_social;
                                var ContratoProveedorId = this.ObtenerContratoProveedor(detalle.DistribucionVianda.ProveedorId);
                                var TiposOpcionesComida = _tipoOpcionComida.GetAll().Where(c => c.ContratoId == ContratoProveedorId)
                                                                                   .ToList();
                                var valor = (from v in TiposOpcionesComida
                                             where v.tipo_comida_id == detalle.SolicitudVianda.tipo_comida_id
                                             where v.ContratoId == ContratoProveedorId
                                             select v).FirstOrDefault();


                                decimal tarifa = valor != null && valor.Id > 0 ? valor.costo : 0;
                                l.Tarifa = tarifa;
                                l.Total = tarifa * l.TotalSolicitado;
                            }
                        }

                        var resultado = (from r in query where r.ProveedorId == input.ProveedorId select r).ToList();

                        lista.AddRange(resultado);

                    }



                }




            }

            return lista;
        }

        public List<FormatLiquidacionSolicitudVianda> ListaSolicitudesViandasPendientesdeLiquidacion(InputLiquidacionDto input)
        {


            var lista = new List<FormatLiquidacionSolicitudVianda>();

            var consumos_viandas = _consumoviandarepository.GetAll()
                                                  //.Where(c => c.ProveedorId == input.ProveedorId)
                                                  //.Where(c => c.fecha_consumo_vianda >= input.FechaDesde)
                                                  // .Where(c => c.fecha_consumo_vianda <= input.FechaHasta)
                                                  // .Where(c => c.liquidado)
                                                  .ToList();
            var solicitudesViandas = _solicitudviandarepository.GetAllIncluding(c => c.locacion,
                                                                                c => c.disciplina,
                                                                                c => c.area,
                                                                                c => c.tipo_comida,
                                                                                c => c.solicitante)
                                                                // .Where(c => c.estado == SolicitudViandaEstado.EntregadaAnotador)
                                                                .Where(c => c.liquidacion_detalle_id == 0)
                                                                .Where(c => !c.liquidado)
                                                                .ToList();

            var detalles_distribucions = _detalledistribucionviandarepository
                                                                            .GetAllIncluding(c => c.DistribucionVianda.Proveedor,
                                                                                             c => c.SolicitudVianda)

                                                                            .ToList();

            if (solicitudesViandas.Count > 0)
            {


                var item = (from s in solicitudesViandas
                            where s.fecha_solicitud.Date >= input.FechaDesde.Date
                            where s.fecha_solicitud.Date <= input.FechaHasta.Date
                            select s).ToList();
                if (item.Count > 0)
                {
                    var query = (from q in item
                                 select new FormatLiquidacionSolicitudVianda
                                 {
                                     Id = q.Id,
                                     IdSolicitante = q.solicitante_id.ToString(),
                                     NombreSolicitante = q.solicitante.nombres_apellidos,
                                     liquidado = q.liquidado,
                                     Locacion = q.locacion.nombre,
                                     TipoComida = q.tipo_comida.nombre,
                                     TotalSolicitado = q.total_pedido,
                                     FechaPedido=q.fecha_solicitud.ToShortDateString()

                                 }).ToList();
                    if (query.Count > 0)
                    {
                        foreach (var l in query)
                        {
                            var detalle = (from dd in detalles_distribucions where dd.SolicitudViandaId == l.Id select dd).FirstOrDefault();
                            if (detalle != null && detalle.Id > 0)
                            {
                                var datafecha = (from cc in consumos_viandas where cc.SolicitudViandaId == l.Id select cc).FirstOrDefault();
                                if (datafecha != null && datafecha.Id > 0)
                                {
                                    l.FechaConsumo = datafecha.fecha_consumo_vianda.ToShortDateString();
                                }
                                l.ProveedorId = detalle.DistribucionVianda.ProveedorId;
                                l.NombreProveedor = detalle.DistribucionVianda.Proveedor.razon_social;
                                var ContratoProveedorId = this.ObtenerContratoProveedor(detalle.DistribucionVianda.ProveedorId);
                                var TiposOpcionesComida = _tipoOpcionComida.GetAll().Where(c => c.ContratoId == ContratoProveedorId)
                                                                                   .ToList();
                                var valor = (from v in TiposOpcionesComida
                                             where v.tipo_comida_id == detalle.SolicitudVianda.tipo_comida_id
                                             where v.ContratoId == ContratoProveedorId
                                             select v).FirstOrDefault();


                                decimal tarifa = valor != null && valor.Id > 0 ? valor.costo : 0;
                                l.Tarifa = tarifa;
                                l.Total = tarifa * l.TotalSolicitado;
                            }
                        }

                        var resultado = (from r in query where r.ProveedorId == input.ProveedorId select r).ToList();

                        lista.AddRange(resultado);

                    }



                }




            }


            return lista;

        }

        public string Nextcode()
        {
            int sec_number = 1;
            var list_code = Repository.GetAll().Where(c => !c.IsDeleted).Select(c => c.Codigo).ToList();
            if (list_code.Count > 0)
            {
                List<int> numeracion = (from l in list_code
                                        where l.Length == 8
                                        select Convert.ToInt32(l.Substring(3, 5))).ToList();

                if (numeracion.Count > 0)
                {
                    sec_number = numeracion.Max() + 1;
                }


            }
            return "LIQ" + String.Format("{0:00000}", sec_number);
        }

        public int ObtenerContratoProveedor(int ProveedorId)
        {
            var contrato = _contratoproveedorrepository.GetAll()
                                                        .Where(c => c.ProveedorId == ProveedorId)
                                                        .Where(c => c.fecha_inicio <= DateTime.Today)
                                                        .Where(c => c.fecha_fin >= DateTime.Today)
                                                        .Where(c => c.estado == ContratoEstado.Activo).FirstOrDefault();
            return contrato != null && contrato.Id > 0 ? contrato.Id : -1;


        }

        public decimal ObtenerTarifaHotel(int ContratoProveedorId, int TipoHabitacionId)
        {
            var tarifa = _tarifahotelrepository.GetAll()
                                               .Where(c => c.ContratoProveedorId == ContratoProveedorId)
                                               .Where(c => c.TipoHabitacionId == TipoHabitacionId)
                                               .Where(c => c.estado)
                                               .FirstOrDefault();
            if (tarifa != null)
            {
                return tarifa.costo_persona;
            }
            else
            {
                return 0;
            }
        }

        public string RemoverLiquidacionHospedaje(int LiquidacionId, List<FormatLiquidacionReserva> pendientes)
        {
            string msg = "";
            var liquidacion = Repository.GetAll().Where(c => c.Id == LiquidacionId).FirstOrDefault();
            if (pendientes.Count == 0)
            {
                msg = "NO_SELECCIONADOS";
            }
            else
            {
                foreach (var p in pendientes)
                {
                    var DetalleReserva = _detalleReservarepository.Get(p.Id);
                    if (DetalleReserva != null && DetalleReserva.liquidacion_detalle_id > 0)
                    {

                        _detallerepository.Delete(DetalleReserva.liquidacion_detalle_id);

                        DetalleReserva.liquidado = false;
                        DetalleReserva.liquidacion_detalle_id = 0;
                        var actualizado = _detalleReservarepository.Update(DetalleReserva);
                    }
                }
                msg = "OK";
            }
            return msg;
        }

        public string AgregarLiquidacionHospedaje(int LiquidacionId, List<FormatLiquidacionReserva> pendientes)
        {
            string msg = "";
            var liquidacion = Repository.GetAll().Where(c => c.Id == LiquidacionId).FirstOrDefault();
            if (pendientes.Count == 0)
            {
                msg = "NO_SELECCIONADOS";
            }
            else
            {
                foreach (var p in pendientes)
                {
                    var DetalleReserva = _detalleReservarepository.Get(p.Id);
                    if (DetalleReserva != null)
                    {

                        DetalleLiquidacion dliquidacion = new DetalleLiquidacion()
                        {
                            LiquidacionId = LiquidacionId,
                            Valor = p.Tarifa,
                            Fecha = p.FechaConsumo != "dd/mm/aaaa" ? DateTime.Parse(p.FechaConsumo) : DateTime.Now,//Revisar Fecha de Consumo Obligatorio
                            Descripcion = p.Tipo + "-" + p.Habitacion + "-" + p.Espacio + "-" + p.Identificacion + "-" + p.Nombres,

                        };
                        var DetalleLiquidacionId = _detallerepository.InsertAndGetId(dliquidacion);
                        //Actualizo Estados Liquidados
                        DetalleReserva.liquidado = true;
                        DetalleReserva.liquidacion_detalle_id = DetalleLiquidacionId;
                        var actualizado = _detalleReservarepository.Update(DetalleReserva);
                    }
                }


                msg = "OK";
            }
            return msg;
        }



        public string RemoverLiquidacionConsumo(int LiquidacionId, List<FormatLiquidacionConsumo> pendientes)
        {
            string msg = "";
            var liquidacion = Repository.GetAll().Where(c => c.Id == LiquidacionId).FirstOrDefault();
            if (pendientes.Count == 0)
            {
                msg = "NO_SELECCIONADOS";
            }
            else
            {
                foreach (var p in pendientes)
                {
                    var Consumo = _consumosrepository.Get(p.Id);
                    if (Consumo != null && Consumo.liquidacion_detalle_id > 0)
                    {

                        _detallerepository.Delete(Consumo.liquidacion_detalle_id);

                        Consumo.liquidado = false;
                        Consumo.liquidacion_detalle_id = 0;
                        var actualizado = _consumosrepository.Update(Consumo);
                    }
                }
                msg = "OK";
            }
            return msg;
        }

        public string AgregarLiquidacionConsumo(int LiquidacionId, List<FormatLiquidacionConsumo> pendientes)
        {
            string msg = "";
            var liquidacion = Repository.GetAll().Where(c => c.Id == LiquidacionId).FirstOrDefault();
            if (pendientes.Count == 0)
            {
                msg = "NO_SELECCIONADOS";
            }
            else
            {
                foreach (var p in pendientes)
                {
                    var Consumo = _consumosrepository.Get(p.Id);
                    if (Consumo != null)
                    {

                        DetalleLiquidacion dliquidacion = new DetalleLiquidacion()
                        {
                            LiquidacionId = LiquidacionId,
                            Valor = p.Tarifa,
                            Fecha = p.FechaConsumo != "dd/mm/aaaa" ? DateTime.Parse(p.FechaConsumo) : DateTime.Now,//Revisar Fecha de Consumo Obligatorio
                            Descripcion = p.TipoComida + "-" + p.OpcionComida + "-" + "-" + p.Identificacion + "-" + p.Nombres,

                        };
                        var DetalleLiquidacionId = _detallerepository.InsertAndGetId(dliquidacion);
                        //Actualizo Estados Liquidados
                        Consumo.liquidado = true;
                        Consumo.liquidacion_detalle_id = DetalleLiquidacionId;
                        var actualizado = _consumosrepository.Update(Consumo);
                    }
                }


                msg = "OK";
            }
            return msg;
        }

        public bool ChangeEstadoPagadoLiquidacion(int Id)
        {
            var l = Repository.Get(Id);
            if (l.Estado != EstadoLiquidacion.Pagado)
            {
                l.Estado = EstadoLiquidacion.Pagado;
                l.FechaPago = DateTime.Now;
            }
            else
            {
                l.Estado = EstadoLiquidacion.Generado;
                l.FechaPago = null;
            }
            var update = Repository.Update(l);
            return update.Id > 0 ? true : false;
        }

        public bool ChangeEliminado(int Id)
        {
            var l = Repository.Get(Id);

            var detalles = _detallerepository.GetAll().Where(c => c.LiquidacionId == Id).ToList();
            if (detalles.Count > 0)
            {
                foreach (var d in detalles)
                {
                    if (l.TipoServicio.codigo == CatalogosCodigos.SERVICIO_HOSPEDAJE)
                    {
                        var detallereserva = _detalleReservarepository.GetAll().Where(c => c.liquidacion_detalle_id == d.Id).FirstOrDefault();
                        if (detallereserva != null && detallereserva.Id > 0)
                        {
                            detallereserva.liquidado = false;
                            detallereserva.liquidacion_detalle_id = 0;
                            _detalleReservarepository.Update(detallereserva);
                        }


                    }
                    if (l.TipoServicio.codigo == CatalogosCodigos.SERVICIO_ALMUERZO)
                    {
                        var consumo = _consumosrepository.GetAll().Where(c => c.liquidacion_detalle_id == d.Id).FirstOrDefault();
                        if (consumo != null && consumo.Id > 0)
                        {
                            consumo.liquidado = false;
                            consumo.liquidacion_detalle_id = 0;
                            _consumosrepository.Update(consumo);
                        }
                    }

                }
            }


            l.Estado = EstadoLiquidacion.Anulado;
            var update = Repository.Update(l);
            return update.Id > 0 ? true : false;
        }

        public string AgregarLiquidacionVianda(int LiquidacionId, List<FormatLiquidacionSolicitudVianda> pendientes)
        {
            string msg = "";
            var liquidacion = Repository.GetAll().Where(c => c.Id == LiquidacionId).FirstOrDefault();
            if (pendientes.Count == 0)
            {
                msg = "NO_SELECCIONADOS";
            }
            else
            {
                foreach (var p in pendientes)
                {
                    var SolicitudVianda = _solicitudviandarepository.GetAllIncluding(c => c.solicitante).Where(c => c.Id == p.Id).FirstOrDefault();
                    if (SolicitudVianda != null)
                    {

                        DetalleLiquidacion dliquidacion = new DetalleLiquidacion()
                        {
                            LiquidacionId = LiquidacionId,
                            Valor = p.Tarifa,
                            Fecha = p.FechaConsumo != null ? DateTime.Parse(p.FechaConsumo) : DateTime.Now,//Revisar Fecha de Consumo Obligatorio
                            Descripcion = p.IdSolicitante + "-" + p.TipoComida + "-" + SolicitudVianda.solicitante.numero_identificacion + "-" + SolicitudVianda.solicitante.nombres_apellidos,

                        };
                        var DetalleLiquidacionId = _detallerepository.InsertAndGetId(dliquidacion);
                        //Actualizo Estados Liquidados
                        SolicitudVianda.liquidado = true;
                        SolicitudVianda.liquidacion_detalle_id = DetalleLiquidacionId;
                        var actualizado = _solicitudviandarepository.Update(SolicitudVianda);
                    }
                }


                msg = "OK";
            }
            return msg;
        }

        public string RemoverLiquidacionVianda(int LiquidacionId, List<FormatLiquidacionSolicitudVianda> pendientes)
        {

            string msg = "";
            var liquidacion = Repository.GetAll().Where(c => c.Id == LiquidacionId).FirstOrDefault();
            if (pendientes.Count == 0)
            {
                msg = "NO_SELECCIONADOS";
            }
            else
            {
                foreach (var p in pendientes)
                {
                    var solicitudVianda = _solicitudviandarepository.Get(p.Id);
                    if (solicitudVianda != null && solicitudVianda.liquidacion_detalle_id > 0)
                    {

                        _detallerepository.Delete(solicitudVianda.liquidacion_detalle_id);

                        solicitudVianda.liquidado = false;
                        solicitudVianda.liquidacion_detalle_id = 0;
                        var actualizado = _solicitudviandarepository.Update(solicitudVianda);
                    }
                }
                msg = "OK";
            }
            return msg;
        }

        public ExcelPackage ObtenerExcelLiquidacion(int Id)
        {
            var liquidacion = Repository.GetAllIncluding(c => c.ContratoProveedor.Proveedor, c => c.TipoServicio).Where(c => c.Id == Id).FirstOrDefault();
            var query_detalleLiquidacion = _detallerepository.GetAllIncluding(c => c.Liquidacion.ContratoProveedor.Proveedor).Where(c => c.LiquidacionId == Id).ToList();
            ExcelPackage excel = new ExcelPackage();

            string filename = System.Web.HttpContext.Current.Server.MapPath("~/Views/PlantillaWord/PlantillaAlimentacion.xlsx");
            if (File.Exists((string)filename))
            {


                FileInfo newFile = new FileInfo(filename);

                ExcelPackage pck = new ExcelPackage(newFile);
                excel.Workbook.Worksheets.Add("Alimentación", pck.Workbook.Worksheets[1]);

            }


            if (query_detalleLiquidacion.Count == 0)
            {
                ExcelPackage nope = new ExcelPackage();
                var hoja = nope.Workbook.Worksheets.Add("NO EXISTEN REGISTROS");
                hoja.DefaultRowHeight = 18.75;
                hoja.View.ZoomScale = 100;

                //var roles = _usuarioRepository.Get(1).Roles; Verificar


                // CABECERA
                hoja.Cells["A1:H1"].Merge = true;
                hoja.Cells["A1:H1"].Value = $"{liquidacion.FechaDesde.ToShortDateString()} - {liquidacion.TipoServicio}";
                hoja.Cells["A1:H1"].Style.WrapText = true;
                hoja.Cells["A1:H2"].Style.Font.Size = 14;
                hoja.Cells["A1:H2"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                hoja.Cells["A1:H2"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                hoja.Cells["A1:H2"].Style.Font.Bold = true;
                hoja.Cells["A2:H2"].Merge = true;
                hoja.Cells["A2:H2"].Value = "NO ENCONTRARON REGISTROS";
                hoja.Cells["A2:H2"].Style.WrapText = true;

                hoja.Column(1).Width = 7.30;
                hoja.Column(2).Width = 23.30;
                hoja.Column(3).Width = 23.30;
                hoja.Column(4).Width = 14.71;
                hoja.Column(5).Width = 21.43;
                hoja.Column(6).Width = 9.57;
                hoja.Column(7).Width = 6.71;
                hoja.Column(8).Width = 5.71;

                // TABLAS

                return nope;
            }
            else
            {

                var proveedores = (from t in query_detalleLiquidacion select t.Liquidacion.ContratoProveedor.Proveedor).Distinct().ToList();


                foreach (var t in proveedores)
                {
                    var hoja = excel.Workbook.Worksheets.Add(t.razon_social);
                    hoja.DefaultRowHeight = 18.75;
                    hoja.View.ZoomScale = 100;

                    //var roles = _usuarioRepository.Get(1).Roles; Verificar

                    // var catalogo = _catalogoRepository.Get(input.tipoComidaId);
                    // CABECERA
                    hoja.Cells["A1:H1"].Merge = true;
                    hoja.Cells["A1:H1"].Value = $"{DateTime.Now} - {liquidacion.TipoServicio.nombre}";
                    hoja.Cells["A1:H1"].Style.WrapText = true;
                    hoja.Cells["A1:H2"].Style.Font.Size = 14;
                    hoja.Cells["A1:H2"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    hoja.Cells["A1:H2"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    hoja.Cells["A1:H2"].Style.Font.Bold = true;
                    hoja.Cells["A2:H2"].Merge = true;
                    hoja.Cells["A2:H2"].Value = "PROVEEDOR: " + t.razon_social;
                    hoja.Cells["A2:H2"].Style.WrapText = true;

                    hoja.Column(1).Width = 7.30;
                    hoja.Column(2).Width = 23.30;
                    hoja.Column(3).Width = 23.30;
                    hoja.Column(4).Width = 14.71;
                    hoja.Column(5).Width = 21.43;
                    hoja.Column(6).Width = 9.57;
                    hoja.Column(7).Width = 6.71;
                    hoja.Column(8).Width = 5.71;

                    // TABLAS


                    int initrow = hoja.Dimension.End.Row;
                    var group = query_detalleLiquidacion.Where(c => c.Liquidacion.ContratoProveedor.ProveedorId == t.Id)
                        .GroupBy(c => c.Liquidacion.ContratoProveedor.ProveedorId).ToList();

                    foreach (var p in group)
                    {
                        var listado = new List<DetalleLiquidacion>();
                        foreach (var x in p)
                        {


                            listado.Add(x);



                        }

                        initrow = hoja.Dimension.End.Row;

                        var excelTemp = CrearTablaRecivoVianda(excel, hoja, initrow + 2, liquidacion.ContratoProveedor.Proveedor.razon_social, listado);

                    }






                    hoja.View.PageBreakView = true;
                    hoja.PrinterSettings.PrintArea = hoja.Cells[1, 1, hoja.Dimension.End.Row, hoja.Dimension.End.Column];
                    hoja.PrinterSettings.FitToPage = true;
                }



                return excel;

            }


        }


        public ExcelPackage CrearTablaRecivoVianda(ExcelPackage excel, ExcelWorksheet hoja, int initRow, string razonSocial, List<DetalleLiquidacion> listado)
        {
            //var hoja = excel.Workbook.Worksheets[1];
            int count = initRow;


            string row = "B" + count + ":G" + count;
            hoja.Cells[row].Merge = true;
            hoja.Cells[row].Value = "RESTAURANT '" + razonSocial + "' ((DAYUMA)";
            hoja.Cells[row].Style.WrapText = true;
            hoja.Cells[row].Style.Font.Size = 18;
            hoja.Cells[row].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            hoja.Cells[row].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            hoja.Cells[row].Style.Font.Bold = true;
            hoja.Row(count).Height = 24;

            count++;
            hoja.Cells[$"B{count}"].Value = "SOLICITANTE";
            hoja.Cells[$"C{count}"].Value = "AREA";
            hoja.Cells[$"D{count}"].Value = "LOCACION";
            hoja.Cells[$"E{count}"].Value = "VIANDS";
            hoja.Cells[$"F{count}"].Value = "HORA";
            hoja.Cells[$"G{count}"].Value = "TIPO";
            hoja.Cells[$"B{count}:G{count}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            hoja.Cells[$"B{count}:G{count}"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            hoja.Cells[$"B{count}:G{count}"].Style.WrapText = true;
            hoja.Cells[$"B{count}:G{count}"].Style.Font.Bold = true;
            hoja.Cells[$"B{count}:G{count}"].Style.Font.Size = 12;
            hoja.Cells[$"B{count}:G{count}"].Style.Border.BorderAround(ExcelBorderStyle.Medium);
            hoja.Cells[$"B{count}:F{count}"].Style.Fill.PatternType = ExcelFillStyle.Solid;
            hoja.Cells[$"B{count}:F{count}"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(242, 220, 219));
            hoja.Row(count).Height = 19.50;

            count++;
            int indexDatosInit = count;
            /* foreach (var retiro in listado)
             {
                 hoja.Row(count).Height = 18.75;
                 hoja.Cells[$"B{count}"].Value = retiro.Solicitante;
                 hoja.Cells[$"C{count}"].Value = retiro.Transportista;
                 hoja.Cells[$"D{count}"].Value = retiro.Disciplina;
                 hoja.Cells[$"E{count}"].Value = retiro.Locacion;
                 hoja.Cells[$"F{count}"].Value = retiro.Viandas;
                 hoja.Cells[$"G{count}"].Value = retiro.Hielo;
                 hoja.Cells[$"B{count}"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                 hoja.Cells[$"C{count}"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                 hoja.Cells[$"D{count}"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                 hoja.Cells[$"E{count}"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                 hoja.Cells[$"F{count}"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                 hoja.Cells[$"G{count}"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                 hoja.Cells[$"F{count}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                 hoja.Cells[$"G{count}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                 hoja.Cells[$"B{count}:G{count}"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                 hoja.Cells[$"B{count}:G{count}"].Style.Font.Size = 11;
                 //hoja.Cells[$"B{count}:G{count}"].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                 count++;
             }
             */
            hoja.Cells[$"E{count}"].Value = "TOTAL: ";
            hoja.Cells[$"E{count}"].Style.Font.Bold = true;
            hoja.Cells[$"E{count}"].Style.Font.Size = 12;
            hoja.Cells[$"E{count}"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            hoja.Cells[$"F{count}"].Formula = $"=SUM(F{indexDatosInit}:F{count - 1})";
            hoja.Cells[$"F{count}"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            hoja.Cells[$"G{count}"].Formula = $"=SUM(G{indexDatosInit}:G{count - 1})";
            hoja.Cells[$"G{count}"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            hoja.Cells[$"E{count}:G{count}"].Style.Border.BorderAround(ExcelBorderStyle.Medium);
            hoja.Cells[$"E{count}:G{count}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            hoja.Cells[$"E{count}:G{count}"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            hoja.Cells[$"F{count}:G{count}"].Style.Font.Color.SetColor(Color.Red);

            hoja.Cells[$"F{indexDatosInit}:G{count}"].Style.Font.Bold = true;

            return excel;
        }



        public ExcelPackage CrearTablaRecivoAlimentacion(ExcelPackage excel, ExcelWorksheet hoja, int initRow, string razonSocial, List<FormatLiquidacionConsumo> listado)
        {
            //var hoja = excel.Workbook.Worksheets[1];
            int count = initRow;


            string row = "B" + count + ":G" + count;
            hoja.Cells[row].Merge = true;
            hoja.Cells[row].Value = "RESTAURANT '" + razonSocial + "' ((DAYUMA)";
            hoja.Cells[row].Style.WrapText = true;
            hoja.Cells[row].Style.Font.Size = 18;
            hoja.Cells[row].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            hoja.Cells[row].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            hoja.Cells[row].Style.Font.Bold = true;
            hoja.Row(count).Height = 24;

            count++;
            hoja.Cells[$"B{count}"].Value = "SOLICITANTE";
            hoja.Cells[$"C{count}"].Value = "FECHA CONSUMO";
            hoja.Cells[$"D{count}"].Value = "OPCIÓN COMIDA";
            hoja.Cells[$"E{count}"].Value = "VIANDAS";
            hoja.Cells[$"F{count}"].Value = "HORA";
            hoja.Cells[$"G{count}"].Value = "TIPO";
            hoja.Cells[$"B{count}:G{count}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            hoja.Cells[$"B{count}:G{count}"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            hoja.Cells[$"B{count}:G{count}"].Style.WrapText = true;
            hoja.Cells[$"B{count}:G{count}"].Style.Font.Bold = true;
            hoja.Cells[$"B{count}:G{count}"].Style.Font.Size = 12;
            hoja.Cells[$"B{count}:G{count}"].Style.Border.BorderAround(ExcelBorderStyle.Medium);
            hoja.Cells[$"B{count}:F{count}"].Style.Fill.PatternType = ExcelFillStyle.Solid;
            hoja.Cells[$"B{count}:F{count}"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(242, 220, 219));
            hoja.Row(count).Height = 19.50;

            count++;
            int indexDatosInit = count;
             foreach (var retiro in listado)
             {
                 hoja.Row(count).Height = 18.75;
                 hoja.Cells[$"B{count}"].Value = retiro.Nombres;
                 hoja.Cells[$"C{count}"].Value = retiro.FechaConsumo;
                 hoja.Cells[$"D{count}"].Value = retiro.OpcionComida;
                 hoja.Cells[$"E{count}"].Value ="";
                hoja.Cells[$"F{count}"].Value = "";
                hoja.Cells[$"G{count}"].Value = "";
                 hoja.Cells[$"B{count}"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                 hoja.Cells[$"C{count}"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                 hoja.Cells[$"D{count}"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                 hoja.Cells[$"E{count}"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                 hoja.Cells[$"F{count}"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                 hoja.Cells[$"G{count}"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                 hoja.Cells[$"F{count}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                 hoja.Cells[$"G{count}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                 hoja.Cells[$"B{count}:G{count}"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                 hoja.Cells[$"B{count}:G{count}"].Style.Font.Size = 11;
                 //hoja.Cells[$"B{count}:G{count}"].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                 count++;
             }
             
            hoja.Cells[$"E{count}"].Value = "TOTAL: ";
            hoja.Cells[$"E{count}"].Style.Font.Bold = true;
            hoja.Cells[$"E{count}"].Style.Font.Size = 12;
            hoja.Cells[$"E{count}"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            hoja.Cells[$"F{count}"].Formula = $"=SUM(F{indexDatosInit}:F{count - 1})";
            hoja.Cells[$"F{count}"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            hoja.Cells[$"G{count}"].Formula = $"=SUM(G{indexDatosInit}:G{count - 1})";
            hoja.Cells[$"G{count}"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            hoja.Cells[$"E{count}:G{count}"].Style.Border.BorderAround(ExcelBorderStyle.Medium);
            hoja.Cells[$"E{count}:G{count}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            hoja.Cells[$"E{count}:G{count}"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            hoja.Cells[$"F{count}:G{count}"].Style.Font.Color.SetColor(Color.Red);

            hoja.Cells[$"F{indexDatosInit}:G{count}"].Style.Font.Bold = true;

            return excel;
        }


        public ExcelPackage ObtenerExcelHospedaje(int Id)
        {
            throw new NotImplementedException();
        }

        public ExcelPackage ObtenerExcelAlimentacion(int Id)
        {

            var liquidacion = Repository.GetAllIncluding(c => c.ContratoProveedor.Proveedor, c => c.TipoServicio).Where(c => c.Id == Id).FirstOrDefault();
            InputLiquidacionDto input = new InputLiquidacionDto
            {
                ProveedorId = liquidacion.ContratoProveedor.ProveedorId,
                FechaDesde = liquidacion.FechaDesde,
                FechaHasta = liquidacion.FechaHasta
            };
            var consumos_liquidadas = this.ListaConsumosLiquidadas(input);
            if (consumos_liquidadas.Count > 0) {

            
            }

            ExcelPackage excel = new ExcelPackage();





            var group = consumos_liquidadas.GroupBy(c => c.TipoComida).ToList();

            foreach (var p in group)
            {
                var listado = new List<FormatLiquidacionConsumo>();
                foreach (var x in p)
                {


                    listado.Add(x);



                }



                var hoja = excel.Workbook.Worksheets.Add(p.Key);
                hoja.DefaultRowHeight = 18.75;
                hoja.View.ZoomScale = 100;

                //var roles = _usuarioRepository.Get(1).Roles; Verificar

                // var catalogo = _catalogoRepository.Get(input.tipoComidaId);
                // CABECERA
                hoja.Cells["A1:H1"].Merge = true;
                hoja.Cells["A1:H1"].Value = $"{DateTime.Now} - {liquidacion.TipoServicio.nombre}";
                hoja.Cells["A1:H1"].Style.WrapText = true;
                hoja.Cells["A1:H2"].Style.Font.Size = 14;
                hoja.Cells["A1:H2"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                hoja.Cells["A1:H2"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                hoja.Cells["A1:H2"].Style.Font.Bold = true;
                hoja.Cells["A2:H2"].Merge = true;
                hoja.Cells["A2:H2"].Value = "PROVEEDOR: " + liquidacion.ContratoProveedor.Proveedor.razon_social;
                hoja.Cells["A2:H2"].Style.WrapText = true;

                hoja.Column(1).Width = 7.30;
                hoja.Column(2).Width = 23.30;
                hoja.Column(3).Width = 23.30;
                hoja.Column(4).Width = 14.71;
                hoja.Column(5).Width = 21.43;
                hoja.Column(6).Width = 9.57;
                hoja.Column(7).Width = 6.71;
                hoja.Column(8).Width = 5.71;

             

               var initrow = hoja.Dimension.End.Row;

                var excelTemp = CrearTablaRecivoAlimentacion(excel, hoja, initrow + 2, liquidacion.ContratoProveedor.Proveedor.razon_social, listado);

            }

            return excel;
        }

        public ExcelPackage ObtenerExcelViandas(int Id)
        {

            var liquidacion = Repository.GetAllIncluding(c => c.ContratoProveedor.Proveedor, c => c.TipoServicio).Where(c => c.Id == Id).FirstOrDefault();
            InputLiquidacionDto input = new InputLiquidacionDto
            {
                ProveedorId = liquidacion.ContratoProveedor.ProveedorId,
                FechaDesde = liquidacion.FechaDesde,
                FechaHasta = liquidacion.FechaHasta
            };
            var viandas_liquidadas = this.ListaSolicitudesViandasLiquidadas(input);
            var query_detalleLiquidacion = _detallerepository.GetAllIncluding(c => c.Liquidacion.ContratoProveedor.Proveedor).Where(c => c.LiquidacionId == Id).ToList();
            ExcelPackage excel = new ExcelPackage();

            string filename = System.Web.HttpContext.Current.Server.MapPath("~/Views/PlantillaWord/PlantillaLiquidacion.xlsx");


            if (File.Exists((string)filename))
            {


                FileInfo newFile = new FileInfo(filename);

                ExcelPackage pck = new ExcelPackage(newFile);
                pck.Workbook.FullCalcOnLoad = true;
                pck.Workbook.Calculate();
                var hoja = pck.Workbook.Worksheets[1];

                excel.Workbook.Worksheets.Add("ALL", pck.Workbook.Worksheets[1]);

 /*
                //  excel.Workbook.Worksheets.Add("mes", pck.Workbook.Worksheets[2]);
                hoja.Calculate();


                var hoja2 = pck.Workbook.Worksheets[2];
                hoja2.Calculate();
                var hoja3 = pck.Workbook.Worksheets[3];
                hoja3.Calculate();
                var hoja4 = pck.Workbook.Worksheets[4];
                hoja4.Calculate();*/
                hoja.Cells["J2:K2"].Merge = true;

                hoja.Cells["J2:K2"].Value = "FECHA: ";

                var ws = excel.Workbook.Worksheets[1];
                ws.InsertRow(7, query_detalleLiquidacion.Count-1);

                int f = 6;
                foreach (var item in viandas_liquidadas)
                {
                    ws.Cells[f, 4].Value = item.FechaConsumo;
                    ws.Cells[f, 4].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells[f, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    ws.Cells[f, 4].Style.Font.Size=10;
                    ws.Cells[f, 4].Style.WrapText = true;

                    ws.Cells[f, 5].Value = "";
                    ws.Cells[f, 5].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells[f, 5].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    ws.Cells[f, 5].Style.Font.Size = 10;
                    ws.Cells[f, 5].Style.WrapText = true;

                    ws.Cells[f, 6].Value = item.NombreProveedor;
                    ws.Cells[f, 6].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells[f, 6].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    ws.Cells[f, 6].Style.Font.Size = 10;
                    ws.Cells[f, 6].Style.WrapText = true;

                    ws.Cells[f, 7].Value =item.NombreSolicitante;
                    ws.Cells[f, 7].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells[f, 7].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    ws.Cells[f, 7].Style.Font.Size = 10;
                    ws.Cells[f, 7].Style.WrapText = true;

                    ws.Cells[f, 8].Value = "";
                    ws.Cells[f, 8].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells[f, 8].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    ws.Cells[f, 8].Style.Font.Size = 10;
                    ws.Cells[f, 8].Style.WrapText = true;

                    ws.Cells[f, 9].Value =item.Locacion;
                    ws.Cells[f, 9].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells[f, 9].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    ws.Cells[f, 9].Style.Font.Size = 10;
                    ws.Cells[f, 9].Style.WrapText = true;

                    ws.Cells[f, 10].Value = item.TotalSolicitado;
                    ws.Cells[f, 10].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells[f, 10].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    ws.Cells[f, 10].Style.Font.Size = 10;
                    ws.Cells[f, 10].Style.WrapText = true;


                    ws.Cells[f, 11].Value = item.TipoComida;
                    ws.Cells[f, 11].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells[f, 11].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    ws.Cells[f, 11].Style.Font.Size = 10;
                    ws.Cells[f, 11].Style.WrapText = true;

                    f++;
                }
                return excel;
            }
            return excel;
        }
    }
}