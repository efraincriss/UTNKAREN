using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.proyecto.dominio;

namespace com.cpp.calypso.proyecto.aplicacion
{
    public interface IObraDisruptivoAsyncBaseCrudAppService : IAsyncBaseCrudAppService<ObraDisruptivo, ObraDisruptivoDto, PagedAndFilteredResultRequestDto>
    {

        int EliminarVigencia(int ObraDisruptivoId);

        List<ObraDisruptivoDto> listar(int AvacenObraId);

        List<CatalogoDto> getCatalogosImproductividad();

        List<CatalogoDto> getCatalogosRecursos();
    }
}
