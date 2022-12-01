using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.proyecto.aplicacion.Dto;
using com.cpp.calypso.proyecto.dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion.Interfaces
{
    public interface IRsoDetalleEacAsyncBaseCrudAppService : IAsyncBaseCrudAppService<RsoDetalleEac, Dto.RsoDetalleEacDto, PagedAndFilteredResultRequestDto>
    {
        decimal CalcularRdoDetallesEAC(int RdoCabeceraId, DateTime fecha_reporte);

        decimal CalcularRdoDetallesEACRDOAnterior(int RdoCabeceraId, DateTime fecha_reporte);
        List<Dto.RsoDetalleEacDto> Listar(int id);

        int ActualizarRdoCabecera(int RdoCabeceraId, decimal avance_real_acumulado);

    }
}
