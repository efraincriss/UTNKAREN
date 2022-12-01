using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.proyecto.aplicacion.Dto;
using com.cpp.calypso.proyecto.dominio;

namespace com.cpp.calypso.proyecto.aplicacion.Interfaces
{
    public interface IOrdenServicioTempAsyncBaseCrudAppService : IAsyncBaseCrudAppService<OrdenServicioTemp, OrdenServicioTempDto, PagedAndFilteredResultRequestDto>
    {
        Task<List<OrdenServicioTemp>> MigrarOrdenesServicio();

        Task RegistrarErrorEnOrden(OrdenServicioTemp orden, string error);

        Task<bool> CreateDetalleOrdenServicioAsync(decimal monton, int ofertaId, int proyectoId, int ordenServicioId, DetalleOrdenServicio.GrupoItems grupo);
    }
}
