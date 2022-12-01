using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.proyecto.dominio;

namespace com.cpp.calypso.proyecto.aplicacion
{
    public interface IDetalleOrdenServicioAsyncBaseCrudAppService : IAsyncBaseCrudAppService<DetalleOrdenServicio, DetalleOrdenServicioDto, PagedAndFilteredResultRequestDto>
    {

        List<DetalleOrdenServicioDto> listar(int ordenServicioId);

        DetalleOrdenServicioDto GetDetalles(int detalleOrdenServicioId);


        int Eliminar(int detalleOrdenServicioId);

        int creardetallos(DetalleOrdenServicio detalleorden);
    }
}
