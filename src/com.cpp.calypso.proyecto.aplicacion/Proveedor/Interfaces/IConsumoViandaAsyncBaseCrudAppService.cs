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
    public interface IConsumoViandaAsyncBaseCrudAppService : IAsyncBaseCrudAppService<ConsumoVianda, ConsumoViandaDto, PagedAndFilteredResultRequestDto>
    {
         
        /// <summary>
        /// Obtener la conciliacion diaria
        /// </summary>
        /// <returns></returns>
        Task<IList<ConsumoViandaTotalesDto>>
            GetConciliacionDiaria(DateTime? fecha);

        /// <summary>
        /// Obtener el detalle de consumo de una solicitud de vianda
        /// </summary>
        /// <returns></returns>
        Task<ConsumoViandaDetalleDto>
            GetConsumoDetalle(int solicitudViandaId);

    }
}
