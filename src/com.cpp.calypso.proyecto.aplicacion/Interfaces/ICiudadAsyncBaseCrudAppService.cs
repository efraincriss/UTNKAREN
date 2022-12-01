using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.proyecto.dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion
{
    public interface ICiudadAsyncBaseCrudAppService : IAsyncBaseCrudAppService<Ciudad, CiudadDto, PagedAndFilteredResultRequestDto>
    {
		List<CiudadDto> ObtenerCantonPorProvincia(int provId);

        CiudadDto GetCiudad(int Id);

        List<CiudadDto> GetCiudades();

        string ObtenerCodigoPostal(int id);//Ciudad Id

    }
}
