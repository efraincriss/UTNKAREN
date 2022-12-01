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
    public interface IEspacioHabitacionAsyncBaseCrudAppService : IAsyncBaseCrudAppService<EspacioHabitacion, EspacioHabitacionDto, PagedAndFilteredResultRequestDto>
    {
        List<EspacioHabitacionDto> GetEspaciosHabitacionPorProveedore(int ProveedorId);

        bool ActivarDesactivarEspacio(int espacioId);

        void CrearEspacios(int HabitacionId, int espacios);

        void CrearNuevosEspacios(int HabitacionId,int capacidad ,int capacidadAnterior);


        void EliminarEspaciosDeHabitacion(int habitacionId);

        List<EspacioLibreDto> EspaciosLibresConDatos(int habitacionId, DateTime fecha);
    }
}
