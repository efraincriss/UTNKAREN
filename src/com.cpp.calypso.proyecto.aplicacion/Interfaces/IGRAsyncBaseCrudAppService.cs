using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.proyecto.dominio;

namespace com.cpp.calypso.proyecto.aplicacion
{
    public interface IGRAsyncBaseCrudAppService : IAsyncBaseCrudAppService<GR, GRDto, PagedAndFilteredResultRequestDto>
    {

        List<GRDto> Listar();

        GRDto GetGr(int id);

        decimal GetMontoTotal(int GrId);

        bool Eliminar(int GrId);
    }
}
