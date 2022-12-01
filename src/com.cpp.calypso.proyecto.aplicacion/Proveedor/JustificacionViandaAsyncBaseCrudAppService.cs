using Abp.Application.Services.Dto;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.aplicacion.Dto;
using com.cpp.calypso.proyecto.aplicacion.Interfaces;
using com.cpp.calypso.proyecto.dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion.Service
{
    public class JustificacionViandaAsyncBaseCrudAppService : 
        AsyncBaseCrudAppService<JustificacionVianda, JustificacionViandaDto, PagedAndFilteredResultRequestDto>,
        IJustificacionViandaAsyncBaseCrudAppService
    {
        public ISolicitudViandaAsyncBaseCrudAppService SolicitudViandaService { get; }
        public IDistribucionViandaAsyncBaseCrudAppService DistribucionViandaService { get; }

        public JustificacionViandaAsyncBaseCrudAppService(
            IBaseRepository<JustificacionVianda> repository,
            ISolicitudViandaAsyncBaseCrudAppService solicitudViandaService,
            IDistribucionViandaAsyncBaseCrudAppService distribucionViandaService
            ) : base(repository)
        {
            SolicitudViandaService = solicitudViandaService;
            DistribucionViandaService = distribucionViandaService;
        }

      
        public async Task<JustificacionViandaDto> InitNew(int solicitudId)
        {
            //Crear un objeto de justificacion de vianda.. 
            //1. Recuperar Solicitud de Vianda
            var distribucionSolicitudDto = await DistribucionViandaService.GetDistribucionSolicitudVianda(solicitudId);
            if (distribucionSolicitudDto == null)
            {
                var msg = string.Format("No existe solicitud de vianda con el identificador {0}",solicitudId);
                throw new GenericException(msg, "No existe solicitud de vianda");
            }

            if (distribucionSolicitudDto.estado_solicitud == SolicitudViandaEstado.Cancelado)
            {
                var msg = string.Format("La solicitud de vianda se encuentra cancelada {0}", solicitudId);
                throw new GenericException(msg, "La solicitud de vianda se encuentra cancelada");
            }

            if (distribucionSolicitudDto.total_pedido == distribucionSolicitudDto.total_consumido) {
                var msg = string.Format("La solicitud de vianda ya se encuentra justificada {0}", solicitudId);
                throw new GenericException(msg, "La solicitud de vianda, ya posee el total de solicitado igual al total de consumido. No se puede agregar justificaciones");
            }

            var item = new JustificacionViandaDto();
            item.Id = 0;
         
            item.anotador_id = distribucionSolicitudDto.anotador_id;
            item.anotador_nombre = distribucionSolicitudDto.anotador_nombre;
            item.conductor_asignado_id = distribucionSolicitudDto.conductor_asignado_id;
            item.conductor_asignado_nombre = distribucionSolicitudDto.conductor_asignado_nombre;

            item.estado = JustificacionViandaEstado.Aprobado;
            item.estado_nombre = JustificacionViandaEstado.Aprobado.ToString();

            item.fecha_solicitud = distribucionSolicitudDto.fecha_solicitud;
            item.solicitante_id = distribucionSolicitudDto.solicitante_id;
            item.solicitante_nombre = distribucionSolicitudDto.solicitante_nombre;
            item.SolicitudViandaId = distribucionSolicitudDto.solicitud_id;

            item.total_pedido = distribucionSolicitudDto.total_pedido;
            item.total_consumido = distribucionSolicitudDto.total_consumido;
            return item;
        }

        public async override Task<JustificacionViandaDto> Create(JustificacionViandaDto input)
        {
            //Rules
            //Verificar si se puede ingresar nueva justificacion Vianda
            var solicitudViandaDto = await SolicitudViandaService.Get(new EntityDto<int>(input.SolicitudViandaId));

            if ((solicitudViandaDto.total_consumido + input.numero_viandas) > solicitudViandaDto.total_pedido)
            {
                var msg = string.Format("La solicitud de vianda ya se encuentra justificada {0}", input.solicitante_id);
                throw new GenericException(msg, "La solicitud de vianda, ya posee el total de solicitado igual al total de consumido. No se puede agregar justificaciones");
            }

            //2. Guardar
            var result = await base.Create(input);

            //3. Actualizar valores en solicitud
            solicitudViandaDto.consumo_justificado = solicitudViandaDto.consumo_justificado + input.numero_viandas;
            solicitudViandaDto.total_consumido = solicitudViandaDto.consumido + solicitudViandaDto.consumo_justificado;
            var solicitudUpdate = await SolicitudViandaService.Update(solicitudViandaDto);

            return result;
        }
    }
}
