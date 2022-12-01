using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.proyecto.aplicacion.Transporte.Dto;
using com.cpp.calypso.proyecto.dominio;
using com.cpp.calypso.proyecto.dominio.Transporte;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion.Transporte.Interface
{
   public interface IRutaHorarioVehiculoAsyncBaseCrudAppService : IAsyncBaseCrudAppService<RutaHorarioVehiculo, RutaHorarioVehiculoDto, PagedAndFilteredResultRequestDto>
    {
        List<RutaHorarioVehiculoDto> Listar();
        int Ingresar(RutaHorarioVehiculo ruta, int horarioid);
        int Editar(RutaHorarioVehiculo ruta);
        int Eliminar(int id);
        RutaHorarioVehiculoDto GetDetalles(int id);
        bool existecode(string code);

        TimeSpan HoraLLegada(int rutaid, int horarioid);

        List<RutaHorarioVehiculoDto> ListarbyRutaHorario(int rutaid, int horarioid);


    }
}