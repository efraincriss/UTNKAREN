using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.proyecto.dominio;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion.Interfaces
{
  public interface IDetallePreciarioAsyncBaseCrudAppService : IAsyncBaseCrudAppService<DetallePreciario, DetallePreciarioDto, PagedAndFilteredResultRequestDto>
    {
        List<DetallePreciarioDto> GetDetallesPreciarios(int PreciarioId);
        DetallePreciarioDto GetDetalles(int DetallePreciarioId);
        Decimal ObtenerPrecioUnitario(Item Item);
        Decimal ObtenerPrecioIncrementado(Item Item, Decimal preciobase,Decimal sumaporcentajes); 
        DetallePreciarioDto comprobarexistenciaitem(int PreciarioId, int ItemId );


        List<DetallePreciario> GetValoresPreciarioActual(int id);//id es del preciario actual

        ExcelPackage GenerarExcelPreciarioValores(int id);

    }
    
}
