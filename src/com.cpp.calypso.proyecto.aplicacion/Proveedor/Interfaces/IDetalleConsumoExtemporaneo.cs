using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.proyecto.aplicacion.Dto;
using com.cpp.calypso.proyecto.aplicacion.Proveedor.Dto;
using com.cpp.calypso.proyecto.dominio.Proveedor;

namespace com.cpp.calypso.proyecto.aplicacion.Proveedor.Interfaces
{
    public interface IDetalleConsumoExtemporaneoAsyncBaseCrudAppService : IAsyncBaseCrudAppService<DetalleConsumoExtemporaneo, DetalleConsumoExtemporaneoDto,  PagedAndFilteredResultRequestDto>
    {
        List<DetalleConsumoExtemporaneoDto> BuscarDetallesPorCabecera(int consumoExtemporaneoId);

        List<ColaboradorNombresDto> BuscarPorIdentificacionNombre(string identificacion = "", string nombre = "");

        string CrearDetalleConsumo(DetalleConsumoExtemporaneoDto detalle);

        bool VerificarDobleConsumo(int colaboradorId, int consumoExtemporaneoId);
    }
}
