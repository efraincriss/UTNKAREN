using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.proyecto.dominio;

namespace com.cpp.calypso.proyecto.aplicacion
{
    public interface IProcesoListaDistribucionAsyncBaseCrudAppService : IAsyncBaseCrudAppService<ProcesoListaDistribucion, ProcesoListaDistribucionDto, PagedAndFilteredResultRequestDto>
    {

        List<ProcesoListaDistribucionDto> listar();

        int Crear(ProcesoListaDistribucionDto proceso);

        List<CorreoListaDto> CorreosDeProceso(int procesoId);
    }
}
