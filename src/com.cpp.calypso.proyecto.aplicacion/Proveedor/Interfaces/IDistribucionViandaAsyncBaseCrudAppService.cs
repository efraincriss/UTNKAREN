using Abp.Application.Services.Dto;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.proyecto.aplicacion.Dto;
using com.cpp.calypso.proyecto.dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion.Interfaces
{
    public interface IDistribucionViandaAsyncBaseCrudAppService : 
        IAsyncBaseCrudAppService<DistribucionVianda, DistribucionViandaDto, PagedAndFilteredResultRequestDto>
    {

        /// <summary>
        /// Obtener todos los elementos
        /// </summary>
        /// <returns></returns>
        Task<IList<DistribucionViandaProveedorDto>>
            GetSolicitudesAsignadas(DateTime fecha,int tipoComidaId);


        /// <summary>
        /// Obtener la distribucion de transporte
        /// </summary>
        /// <returns></returns>
        Task<IList<DistribucionTransporteDto>>
            GetDistribucionTransporte(DateTime fecha, int tipoComidaId);


        /// <summary>
        /// Obtener la distribucion de una solicitud de vianda
        /// </summary>
        /// <returns></returns>
        Task<DistribucionSolicitudDto>
            GetDistribucionSolicitudVianda(int solicitudId);

        /// <summary>
        /// Obtener todos los elementos
        /// </summary>
        /// <returns></returns>
        Task<PagedResultDto<DistribucionViandaGrupoDto>>
            GetSolicitudesAsignadasGrupo(PagedAndSortedResultRequestDto pagedAndSorted,DateTime? fecha, int? tipoComidaId);



        Task<TotalesDistribuccionDto>
            GetTotales(DateTime? fecha, int? tipoComidaId);


        /// <summary>
        /// Realizar distribuccion de Solicitudes
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="tipoComidaId"></param>
        /// <param name="model"></param>
        /// <param name="deleteIds"></param>
        /// <returns></returns>
        Task<bool> Distribute(DateTime fecha, int tipoComidaId, List<DistribucionViandaProveedorDto> model, List<int> deleteIds);

        /// <summary>
        /// Realizar distribuccion de transporte
        /// </summary>
        /// <param name="fecha">Fecha de la distribuccion</param>
        /// <param name="tipoComidaId">Tipo de Comida de la distribuccion</param>
        /// <param name="model">Listado de Distribuciones</param>
        /// <returns></returns>
        Task<bool> DistributeTransport(DateTime fecha, int tipoComidaId, List<DistribucionTransporteUpdateDto> model);

        /// <summary>
        /// Aprobar distribuccion de Transporte.
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="tipoComidaId"></param>
        /// <param name="model"></param>
        /// <param name="deleteIds"></param>
        /// <returns></returns>
        Task<bool> ApproveDistributeTransport(DateTime fecha, int tipoComidaId);

        /// <summary>
        /// Verificar si existe una distribucion creada para la fecha y el tipo de comida enviado como parametro
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="tipoComidaId"></param>
        /// <returns></returns>
        Task<bool> Exists(DateTime fecha, int tipoComidaId);

        /// <summary>
        /// Aprobar distribuccion de pedidos, asociados a la fecha y tipo de comida
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="tipoComidaId"></param>
        /// <returns></returns>
        Task<bool> ApproveDistribute(DateTime fecha, int tipoComidaId);


        Task<bool> EnviarDistribucionaProveedores(DateTime fecha);


    }
}
