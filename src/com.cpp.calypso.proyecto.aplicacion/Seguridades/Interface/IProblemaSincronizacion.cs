using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.proyecto.aplicacion.Seguridades.Dto;
using com.cpp.calypso.proyecto.dominio.Seguridades;
using OfficeOpenXml;

namespace com.cpp.calypso.proyecto.aplicacion.Seguridades.Interface
{
    public interface IProblemaSincronizacionAsyncBaseCrudAppService : IAsyncBaseCrudAppService<ProblemaSincronizacion, ProblemaSincronizacionDto, PagedAndFilteredResultRequestDto>
    {
        List<ProblemaSincronizacionDto> ObtenerProblemas(DateTime? fechaInicio, DateTime? fechaFin, bool solucionado);

        void SolucionarProblema(int problemaSincronizacionId, string observacion);

        bool SolucionarMultiple(List<int> ids, string observacion);

        ExcelPackage DescargarListadoErroresDeSincronizacion(List<int> ids);

        void MarcarNoSolucionado(int problemaSincronizacionId);
    }
}
