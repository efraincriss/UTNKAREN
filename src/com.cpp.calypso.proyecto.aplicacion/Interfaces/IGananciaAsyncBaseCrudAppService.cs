using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.proyecto.dominio;

namespace com.cpp.calypso.proyecto.aplicacion.Interfaces
{
   public interface IGananciaAsyncBaseCrudAppService: IAsyncBaseCrudAppService<Ganancia, GananciaDto, PagedAndFilteredResultRequestDto>
    {
        List<Ganancia> GetGanacias();
        List<Ganancia> GetGanaciasporContrato(int ContradoId);
        GananciaDto GetDetalle(int GanaciaId);

        GananciaModel GetGananciasContrato(int ContratoId, DateTime FechaOferta);

        string ValidacionesGanancia(GananciaDto dto);
    }
}
