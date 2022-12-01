using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.aplicacion.Dto;
using com.cpp.calypso.proyecto.aplicacion.Interfaces;
using com.cpp.calypso.proyecto.dominio;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.cpp.calypso.framework.Extensions;

namespace com.cpp.calypso.proyecto.aplicacion.Service
{
    public class ConsumoViandaAsyncBaseCrudAppService :
        AsyncBaseCrudAppService<ConsumoVianda, ConsumoViandaDto, PagedAndFilteredResultRequestDto>,
        IConsumoViandaAsyncBaseCrudAppService
    {

        public IBaseRepository<DetalleDistribucion> RepositoryDetalleDistribucion { get; }


        public ConsumoViandaAsyncBaseCrudAppService(
            IBaseRepository<ConsumoVianda> repository,
            IBaseRepository<DetalleDistribucion> repositoryDetalleDistribucion
            ) : base(repository)
        {
            RepositoryDetalleDistribucion = repositoryDetalleDistribucion;
        }

        public async Task<IList<ConsumoViandaTotalesDto>> 
            GetConciliacionDiaria(DateTime? fecha)
        {

            var detalleQuery = RepositoryDetalleDistribucion.GetAll();

            detalleQuery = detalleQuery.Where(d => d.SolicitudVianda.estado != SolicitudViandaEstado.Cancelado && d.SolicitudVianda.estado != SolicitudViandaEstado.Asignada);

            if (fecha.HasValue)
            {
                detalleQuery = detalleQuery.Where(
                                 detalle => DbFunctions.TruncateTime(detalle.DistribucionVianda.fecha) == DbFunctions.TruncateTime(fecha.Value)
                               );
            }
  
            var queryDto = (from item in detalleQuery   
                            orderby item.SolicitudVianda.solicitante.nombres
                            select new ConsumoViandaTotalesDto
                            {
                                Id = item.DistribucionViandaId,
                                alcance_viandas = item.SolicitudVianda.alcance_viandas,
                                AreaId = item.SolicitudVianda.area_id,
                                area_nombre = item.SolicitudVianda.area.nombre,
                                consumido = item.SolicitudVianda.consumido,
                                consumo_justificado = item.SolicitudVianda.consumo_justificado,
                                detalle_distribuccion_id = item.Id,
                                disciplina_id = item.SolicitudVianda.disciplina_id,
                                disciplina_nombre = item.SolicitudVianda.disciplina.nombre,
                                estado_solicitud = item.SolicitudVianda.estado,
                                estado_solicitud_nombre = item.SolicitudVianda.estado.GetDescription(),
                                fecha_distribuccion = item.DistribucionVianda.fecha,
                                LocacionId = item.SolicitudVianda.LocacionId,
                                locacion_nombre = item.SolicitudVianda.locacion.nombre,
                                pedido_viandas = item.SolicitudVianda.pedido_viandas,
                                ProveedorId = item.DistribucionVianda.ProveedorId,
                                proveedor_nombre = item.DistribucionVianda.Proveedor.razon_social,
                                solicitante_nombre = item.SolicitudVianda.solicitante.nombres,
                                solicitud_id = item.SolicitudVianda.Id,
                                tipo_comida_id = item.SolicitudVianda.tipo_comida_id,
                                tipo_comida_nombre = item.SolicitudVianda.tipo_comida.nombre,
                                total_consumido = item.SolicitudVianda.total_consumido,
                                total_pedido = item.SolicitudVianda.total_pedido
                            }
                             );

     
            return
                 await queryDto.ToListAsync();
             
        }

        public async Task<ConsumoViandaDetalleDto> 
            GetConsumoDetalle(int solicitudViandaId)
        {

            var query = Repository.GetAll();
            var detalleQuery = RepositoryDetalleDistribucion.GetAll();


            var querySolicitudViandaDto = (from detalle in detalleQuery
                                           where detalle.SolicitudViandaId == solicitudViandaId
                            select new ConsumoViandaDetalleDto
                            {
                                Id = detalle.SolicitudViandaId,
                                fecha_solicitud = detalle.SolicitudVianda.fecha_solicitud,
                                alcance_viandas = detalle.SolicitudVianda.alcance_viandas,
                                anotador_id = (detalle.SolicitudVianda.anotador_id) != null ? detalle.SolicitudVianda.anotador_id : null,
                                anotador_nombre = (detalle.SolicitudVianda.anotador_id) != null ? detalle.SolicitudVianda.anotador.nombres : string.Empty,
                                conductor_asignado_id = (detalle.DistribucionVianda.conductor_asignado_id) != null ? detalle.DistribucionVianda.conductor_asignado_id : null,
                                conductor_asignado_nombre = (detalle.DistribucionVianda.conductor_asignado_id) != null ? detalle.DistribucionVianda.conductor_asignado.nombres_apellidos : string.Empty,
                                consumido = detalle.SolicitudVianda.consumido,
                                consumo_justificado = detalle.SolicitudVianda.consumo_justificado,
                                hora_entrega_restaurante = detalle.SolicitudVianda.hora_entrega_restaurante,
                                hora_entrega_transportista = detalle.SolicitudVianda.hora_entrega_transportista,
                                pedido_viandas = detalle.SolicitudVianda.pedido_viandas,
                                ProveedorId = detalle.DistribucionVianda.ProveedorId,
                                proveedor_nombre = detalle.DistribucionVianda.Proveedor.razon_social,
                                solicitante_id = detalle.SolicitudVianda.solicitante_id,
                                solicitante_nombre = detalle.SolicitudVianda.solicitante.nombres,
                                total_consumido = detalle.SolicitudVianda.total_consumido,
                                total_pedido = detalle.SolicitudVianda.total_pedido
                            });

            var result = await querySolicitudViandaDto.SingleOrDefaultAsync();

            //Consumos
            var queryConsumosDto = (from item in query
                                    where item.SolicitudViandaId == solicitudViandaId
                                    select new ConsumoViandaDto {
                                            Id = item.Id,
                                            ColaboradorId = item.colaborador_id,
                                            colaborador_nombre = item.colaborador.nombres,
                                            en_sitio = item.en_sitio,
                                            fecha_consumo_vianda = item.fecha_consumo_vianda,
                                            observaciones = item.observaciones,
                                            SolicitudViandaId = item.SolicitudViandaId,
                                            TipoOpcionComidaId = item.TipoOpcionComidaId
                                    }
                                    );

            var detalleDtoDto = await queryConsumosDto.ToListAsync();

            result.consumo_viandas = detalleDtoDto;

            return result;


        }
    }
}
