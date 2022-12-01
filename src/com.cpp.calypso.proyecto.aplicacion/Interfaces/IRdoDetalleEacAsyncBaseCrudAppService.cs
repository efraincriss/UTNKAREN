using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.proyecto.dominio;

namespace com.cpp.calypso.proyecto.aplicacion
{
    public interface IRdoDetalleEacAsyncBaseCrudAppService : IAsyncBaseCrudAppService<RdoDetalleEac, RdoDetalleEacDto, PagedAndFilteredResultRequestDto>
    {
        decimal CalcularRdoDetallesEAC(int RdoCabeceraId, DateTime fecha_reporte);

        decimal CalcularRdoDetallesEACRDOAnterior(int RdoCabeceraId, DateTime fecha_reporte);
        List<RdoDetalleEacDto> Listar(int id);

        int ActualizarRdoCabecera(int RdoCabeceraId, decimal avance_real_acumulado);

    }
}
