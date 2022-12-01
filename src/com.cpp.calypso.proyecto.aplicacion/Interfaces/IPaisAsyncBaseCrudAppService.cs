using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.proyecto.dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion
{
    public interface IPaisAsyncBaseCrudAppService : IAsyncBaseCrudAppService<Pais, PaisDto, PagedAndFilteredResultRequestDto>
    {

        List<PaisDto> GetPaises();
        PaisDto GetPais(int Id);
    }
}
