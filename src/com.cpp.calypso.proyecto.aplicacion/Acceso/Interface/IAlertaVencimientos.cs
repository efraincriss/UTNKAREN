using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.proyecto.aplicacion.Acceso.Dto;
using com.cpp.calypso.proyecto.aplicacion.Dto;
using com.cpp.calypso.proyecto.dominio;
using OfficeOpenXml;

namespace com.cpp.calypso.proyecto.aplicacion.Acceso.Interface
{
    public interface IAlertaVencimientosAsyncBaseCrudAppService : IAsyncBaseCrudAppService<ColaboradorRequisito, ColaboradorRequisitoDto, PagedAndFilteredResultRequestDto>
    {

        ExcelPackage ExcelCumplimientoIndividual(InputAlertaVencimientoReporteDto input);


    }
}
