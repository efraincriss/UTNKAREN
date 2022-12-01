using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.proyecto.aplicacion.Proveedor.Dto;
using com.cpp.calypso.proyecto.dominio;
using com.cpp.calypso.proyecto.dominio.Proveedor;

namespace com.cpp.calypso.proyecto.aplicacion.Proveedor.Interfaces
{
    public interface IHabitacionAsyncBaseCrudAppService : IAsyncBaseCrudAppService<Habitacion, HabitacionDto,  PagedAndFilteredResultRequestDto>
    {
        List<HabitacionDto> GetHabitacionesPorProveedor(int ProveedorId);

        Task<HabitacionDto> GetDetalle(int habitacionId);

        List<HabitacionTree> GenerarArbolHabitaciones(int proveedorId);

        bool ExisteNumeroHabitacion(string nroHabitacion, int proveedorId);

        void SwitchEstadoHabitacion(int habitacionId, bool value);
    }
}
