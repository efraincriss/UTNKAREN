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

namespace com.cpp.calypso.proyecto.aplicacion.Service
{
    public class OrdenServicioTempAsyncBaseCrudAppService : AsyncBaseCrudAppService<OrdenServicioTemp, OrdenServicioTempDto, PagedAndFilteredResultRequestDto>, IOrdenServicioTempAsyncBaseCrudAppService
    {
        private readonly IBaseRepository<OrdenServicio> _ordenServicioRepository;
        private readonly IBaseRepository<DetalleOrdenServicio> _detalleOrdenServicioRepository;
        private readonly IBaseRepository<OfertaComercial> _ofertaComercialRepository;
        private readonly IBaseRepository<Proyecto> _proyectoRepository;

        public OrdenServicioTempAsyncBaseCrudAppService(
            IBaseRepository<OrdenServicioTemp> repository,
            IBaseRepository<OrdenServicio> ordenServicioRepository,
            IBaseRepository<DetalleOrdenServicio> detalleOrdenServicioRepository,
            IBaseRepository<OfertaComercial> ofertaComercialRepository,
            IBaseRepository<Proyecto> proyectoRepository
        ) : base(repository)
        {
            _ordenServicioRepository = ordenServicioRepository;
            _detalleOrdenServicioRepository = detalleOrdenServicioRepository;
            _ofertaComercialRepository = ofertaComercialRepository;
            _proyectoRepository = proyectoRepository;
        }

        public async Task<List<OrdenServicioTemp>> MigrarOrdenesServicio()
        {
            var temporales = Repository.GetAll().ToList();

            foreach (var orden in temporales)
            {
                var ofertaString = orden.Oferta.Split('_')[0];

                var oferta = _ofertaComercialRepository.GetAll()
                    .Where(o => o.codigo == ofertaString)
                    .FirstOrDefault(o => o.es_final == 1);

                if (oferta != null)
                {
                    var proyecto = _proyectoRepository.GetAll()
                        .Where(o => o.vigente)
                        .FirstOrDefault(o => o.codigo == orden.CodigoProyecto);

                    if (proyecto != null)
                    {

                        var ordenCabecera = new OrdenServicio()
                        {
                            anio = orden.Anio,
                            referencias_po = orden.ReferenciaOrdenes,
                            codigo_orden_servicio = orden.CodigoOrden,
                            fecha_orden_servicio = orden.Fecha,
                            monto_aprobado_os = orden.Total,
                            monto_aprobado_construccion = orden.Construccion,
                            monto_aprobado_ingeniería = orden.Ingenieria,
                            monto_aprobado_suministros = orden.Compras,
                            version_os = "1",
                            monto_aprobado_subcontrato = 0,
                            EstadoId = 11086
                                //11086, 13884
                        };

                        try
                        {
                            var ordenServicioId = await _ordenServicioRepository.InsertAndGetIdAsync(ordenCabecera);

                            if (orden.Ingenieria > 0)
                            {
                                var result = await this.CreateDetalleOrdenServicioAsync(orden.Ingenieria, oferta.Id, proyecto.Id,
                                    ordenServicioId, DetalleOrdenServicio.GrupoItems.Ingeniería);

                                if (!result)
                                {
                                    await this.RegistrarErrorEnOrden(orden, "Error creando el detalle de ingenieria");
                                }
                                else
                                {
                                    await this.RegistrarErrorEnOrden(orden, "OK");
                                }
                            }

                            if (orden.Construccion > 0)
                            {
                                var result = await this.CreateDetalleOrdenServicioAsync(orden.Construccion, oferta.Id, proyecto.Id,
                                    ordenServicioId, DetalleOrdenServicio.GrupoItems.Construcción);

                                if (!result)
                                {
                                    await this.RegistrarErrorEnOrden(orden, "Error creando el detalle de construccion");
                                }
                                else
                                {
                                    await this.RegistrarErrorEnOrden(orden, "OK");
                                }
                            }

                            if (orden.Compras > 0)
                            {
                                var result = await this.CreateDetalleOrdenServicioAsync(orden.Compras, oferta.Id, proyecto.Id,
                                    ordenServicioId, DetalleOrdenServicio.GrupoItems.Suministros);

                                if (!result)
                                {
                                    await this.RegistrarErrorEnOrden(orden, "Error creando el detalle de compras");
                                }
                                else
                                {
                                    await this.RegistrarErrorEnOrden(orden, "OK");
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            var error = e.Message;
                        }
          
                    }
                    else
                    {
                        await this.RegistrarErrorEnOrden(orden, "No se enontro el proyecto");
                    }
                }
                else
                {
                    await this.RegistrarErrorEnOrden(orden, "No se enontro la oferta");
                }
            }


            return temporales;
        }

        public async Task RegistrarErrorEnOrden(OrdenServicioTemp orden, string error)
        {
            orden.MigracionExitosa = error;
            await Repository.UpdateAsync(orden);
        }

        public async Task<bool> CreateDetalleOrdenServicioAsync(decimal monton, int ofertaId, int proyectoId, int ordenServicioId, DetalleOrdenServicio.GrupoItems grupo)
        {
            var detalle = new DetalleOrdenServicio()
            {
                GrupoItemId = grupo,
                OfertaComercialId = ofertaId,
                ProyectoId = proyectoId,
                vigente = true,
                OrdenServicioId = ordenServicioId,
                valor_os = monton
            };

            var detalleId = await _detalleOrdenServicioRepository.InsertAndGetIdAsync(detalle);

            return detalleId > 0;
        }
    }
}
