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
    public interface ICargosSectorAsyncBaseCrudAppService : IAsyncBaseCrudAppService<CargosSector, CargosSectorDto, PagedAndFilteredResultRequestDto>
    {
        List<CargosSectorDto> GetList();
        CargosSector GetCargosSector(int Id);
        string CrearCargosSectorAsync(CargosSectorDto cargosSector);
        string ActualizarCargosSectorAsync(CargosSectorDto cargosSector);
        bool EliminarCargosSector(int Id);
        CargosSector BuscarCargoSector(int IdCargo, int IdSector);
        List<CargosSector> GetListCargosPorSector(int IdSector);

        String CatalogosPorSector(int Id);

        String CargosDisponibles(int Id);

        bool ActualizarCargosSector(int Id, String username);
    }
}
