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
    public interface IConsumoExtemporaneoAsyncBaseCrudAppService : IAsyncBaseCrudAppService<ConsumoExtemporaneo, ConsumoExtemporaneoDto,  PagedAndFilteredResultRequestDto>
    {
        List<ConsumoExtemporaneoDto> ObtenerTodos();

        List<ProveedorDto> ObtenerProveedoresAlimentacion();

        List<TipoOpcionComidaDto> ObtenerTiposComida(int proveedorId);


        bool ValidarRepetidos(int proveedorId, DateTime date);


    }
}
