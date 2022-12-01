using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.proyecto.aplicacion.Proveedor.Dto;
using com.cpp.calypso.proyecto.dominio.Proveedor;

namespace com.cpp.calypso.proyecto.aplicacion.Proveedor.Interfaces
{
    public interface IDetalleReservaAsyncBaseCrudAppService : IAsyncBaseCrudAppService<DetalleReserva, DetalleReservaDto, PagedAndFilteredResultRequestDto>
    {

        bool EliminarDetallesPorReservaId(int reservaId);

        bool EliminarReservasPosterioresA(int reservaId, DateTime fecha);

        List<DetalleReservaDto> ListarPorReservaId(int reservaId);
    }
}
