using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.proyecto.dominio;

namespace com.cpp.calypso.proyecto.aplicacion
{
    public interface IDetalleItemIngenieriaAsyncBaseCrudAppService : IAsyncBaseCrudAppService<DetalleItemIngenieria, DetalleItemIngenieriaDto, PagedAndFilteredResultRequestDto>
    {
        List<DetalleItemIngenieriaDto> ListarPorDetalleAvance(int detalleAvanceId);
    }
}
