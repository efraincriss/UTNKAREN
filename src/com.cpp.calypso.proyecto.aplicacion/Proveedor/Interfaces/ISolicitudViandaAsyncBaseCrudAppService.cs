using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.proyecto.aplicacion.Dto;
using com.cpp.calypso.proyecto.dominio;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion.Interfaces
{
    public interface ISolicitudViandaAsyncBaseCrudAppService :
        IAsyncBaseCrudAppService<SolicitudVianda, SolicitudViandaDto, PagedAndFilteredResultRequestDto>
    {

        /// <summary>
        /// Obtener todos los elementos
        /// </summary>
        /// <returns></returns>
        Task<IList<SolicitudViandaDto>>
            GetSolicitudesPendientes(DateTime fecha, int tipoComidaId);

        Task<int> UpdateState(IEnumerable<int> solicitudesId, SolicitudViandaEstado estado);

        Task<IList<SolicitudViandaDto>> GetSolicitudDiaria(DateTime? fecha);

        /// <summary>
        /// Obtener las solicitudes del usuario actual
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        Task<IList<SolicitudViandaDto>> GetMySolicitud(DateTime? fecha, List<SolicitudViandaEstado> incluir);

        Task<bool> Cancel(int id, string observaciones);
    }
}
