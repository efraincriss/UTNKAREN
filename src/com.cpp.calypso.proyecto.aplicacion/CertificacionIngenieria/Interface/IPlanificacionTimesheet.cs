using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.proyecto.aplicacion.CertificacionIngenieria.Dto;
using com.cpp.calypso.proyecto.dominio.CertificacionIngenieria;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion.CertificacionIngenieria.Interface
{
    public interface IPlanificacionTimesheetBaseCrudAppService : IAsyncBaseCrudAppService<PlanificacionTimesheet, PlanificacionTimesheetDto, PagedAndFilteredResultRequestDto>
    {

        List<PlanificacionTimesheetDto> GetPlanificaciones();

        Task<bool> CrearPlanificacionAsync(PlanificacionTimesheetDto dto);

        Task<bool> ActualizarPlanificacionAsync(PlanificacionTimesheetDto dto);

        bool EliminarPlanificacion(int id);

        bool CrearPlanificacionPorAño(int year);

        ExcelPackage DescargarPlanificacionPorMes(DateTime fechaReporte);

    }
}
