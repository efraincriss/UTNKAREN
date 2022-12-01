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
    public interface IParroquiaAsyncBaseCrudAppService : IAsyncBaseCrudAppService<Parroquia, ParroquiaDto, PagedAndFilteredResultRequestDto>
    {
		List<ParroquiaDto> ObtenerParroquiaPorCanton(int cantonId);
        ParroquiaDto GetParroquia(int Id);

        List<ParroquiaDto> GetParroquias();

    }
}
