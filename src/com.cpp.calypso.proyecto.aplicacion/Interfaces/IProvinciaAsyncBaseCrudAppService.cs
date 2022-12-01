using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.proyecto.dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion
{
    public interface IProvinciaAsyncBaseCrudAppService : IAsyncBaseCrudAppService<Provincia, ProvinciaDto, PagedAndFilteredResultRequestDto>
    {

        List<ProvinciaDto> ObtenerProvinciaPorPais(int paisId);
        ProvinciaDto GetProvincia(int Id);

        List<ProvinciaDto> GetProvincias();
    }
}
