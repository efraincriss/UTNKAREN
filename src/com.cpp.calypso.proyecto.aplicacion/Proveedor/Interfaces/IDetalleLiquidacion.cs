using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.proyecto.aplicacion.Proveedor.Dto;
using com.cpp.calypso.proyecto.dominio.Proveedor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion.Proveedor.Interfaces
{
    public interface IDetalleLiquidacionAsyncBaseCrudAppService : IAsyncBaseCrudAppService<DetalleLiquidacion, DetalleLiquidacionDto, PagedAndFilteredResultRequestDto>
    {

    }
}
