using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.proyecto.dominio;

namespace com.cpp.calypso.proyecto.aplicacion
{
    public interface IRdoDetalleAsyncBaseCrudAppService: IAsyncBaseCrudAppService<RdoDetalle, RdoDetalleDto, PagedAndFilteredResultRequestDto>
    {
        List<RdoDetalle> GetRdoDetalles(int RdoCabeceraId);

        RdoDetalleDto GetDetalles(int RdoDetalleId);

        void CalcularRdoDetalles(int RdoCabeceraId, DateTime fecha_reporte);


    }
}
