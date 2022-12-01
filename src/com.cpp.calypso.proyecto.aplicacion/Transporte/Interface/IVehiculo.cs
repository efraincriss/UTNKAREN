using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.proyecto.aplicacion.Transporte.Dto;
using com.cpp.calypso.proyecto.dominio.Transporte;

namespace com.cpp.calypso.proyecto.aplicacion.Transporte.Interface
{
    public interface IVehiculoAsyncBaseCrudAppService : IAsyncBaseCrudAppService<Vehiculo, VehiculoDto, PagedAndFilteredResultRequestDto>
    {
        ICollection<VehiculoDto> GetAllVehiculos();

        string CanCreate(string codigo, string placa, int anioFabricacion, DateTime fechaMatricula);

        string CanUpdate(string codigo, string placa, int anioFabricacion, DateTime fechaMatricula, int proveedorId);

        bool RegistrarHistorico(int id, string nuevoEstado);

        string CanDelete(int id);

        string nextcode();

        string GetTipoVehiculo(int id);
    }
}
