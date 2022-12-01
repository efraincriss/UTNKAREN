using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.proyecto.dominio;

namespace com.cpp.calypso.proyecto.aplicacion
{
    public interface IObraAdicionalAsyncBaseCrudAppService : IAsyncBaseCrudAppService<ObraAdicional, ObraAdicionalDto, PagedAndFilteredResultRequestDto>
    {

        Task<int> Eliminar(int id);

        List<ObraAdicionalDto> listar(int AvanceObraId);
    }
}
