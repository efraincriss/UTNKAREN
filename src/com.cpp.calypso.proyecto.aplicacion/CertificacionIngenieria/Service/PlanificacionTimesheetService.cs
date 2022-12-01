using AutoMapper;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.aplicacion.CertificacionIngenieria.Dto;
using com.cpp.calypso.proyecto.aplicacion.CertificacionIngenieria.Interface;
using com.cpp.calypso.proyecto.dominio.CertificacionIngenieria;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion.CertificacionIngenieria.Service
{
    public class PlanificacionTimesheetBaseCrudAppService : AsyncBaseCrudAppService<PlanificacionTimesheet, PlanificacionTimesheetDto, PagedAndFilteredResultRequestDto>, IPlanificacionTimesheetBaseCrudAppService
    {
        public PlanificacionTimesheetBaseCrudAppService(
            IBaseRepository<PlanificacionTimesheet> repository
        ) : base(repository)
        {
        }

        public List<PlanificacionTimesheetDto> GetPlanificaciones()
        {
            var planificacion = Repository.GetAll().ToList();
            return Mapper.Map<List<PlanificacionTimesheet>, List<PlanificacionTimesheetDto>>(planificacion);
        }

        public async Task<bool> CrearPlanificacionAsync(PlanificacionTimesheetDto dto)
        {
            var entity = Mapper.Map<PlanificacionTimesheet>(dto);
            await Repository.InsertAsync(entity);
            return true;
        }

        public async Task<bool> ActualizarPlanificacionAsync(PlanificacionTimesheetDto dto)
        {
            var entity = Mapper.Map<PlanificacionTimesheet>(dto);
            await Repository.UpdateAsync(entity);
            return true;
        }

        public bool CrearPlanificacionPorAño(int year)
        {
            var startDate = new DateTime(year, 1, 1);
            var endDate = new DateTime(year, 12, 31);
            var dates = Enumerable.Range(0, 1 + endDate.Subtract(startDate).Days)
                .Select(offset => startDate
                .AddDays(offset));

            var mondays = dates.Where(i => i.DayOfWeek == DayOfWeek.Monday).ToArray();

            var wednesdays = dates.Where(i => i.DayOfWeek == DayOfWeek.Wednesday).ToArray();

            var cortesIngenieriaMensual = dates.Where(i => i.Day == 20).ToArray();

            var rsIngenieriaMensual = dates.Where(i => i.Day == 22).ToArray();

            var certificacionesMensuales = dates.Where(i => i.Day == 27).ToArray();

            foreach (var monday in mondays)
            {
                var count = Repository.GetAll()
                    .Where(o => o.Fecha == monday)
                    .Where(o => o.TipoPlanificacion == TipoPlanificacion.CorteIngenieria)
                    .Count();

                if (count == 0)
                {
                    var entity = new PlanificacionTimesheet()
                    {
                        TipoPlanificacion = TipoPlanificacion.CorteIngenieria,
                        Fecha = monday,
                        Descripcion = "Corte Ingeniería",
                    };
                    Repository.Insert(entity);
                }
            }

            foreach (var wednesday in wednesdays)
            {
                var countTs = Repository.GetAll()
                    .Where(o => o.Fecha == wednesday)
                    .Where(o => o.TipoPlanificacion == TipoPlanificacion.EnvioTsParaSr)
                    .Count();

                var countRs = Repository.GetAll()
                    .Where(o => o.Fecha == wednesday)
                    .Where(o => o.TipoPlanificacion == TipoPlanificacion.RsIngenieria)
                    .Count();

                if (countTs == 0)
                {
                    var ts = new PlanificacionTimesheet()
                    {
                        TipoPlanificacion = TipoPlanificacion.EnvioTsParaSr,
                        Fecha = wednesday,
                        Descripcion = "Envío de TS para RS",
                    };
                    Repository.Insert(ts);
                   
                }
                if (countRs == 0)
                {
                    var rs = new PlanificacionTimesheet()
                    {
                        TipoPlanificacion = TipoPlanificacion.RsIngenieria,
                        Fecha = wednesday,
                        Descripcion = "RS Ingeniería",
                    };
                    Repository.Insert(rs);
                }
            }


            foreach (var corteIngenieriaMensual in cortesIngenieriaMensual)
            {
                var count = Repository.GetAll()
                    .Where(o => o.Fecha == corteIngenieriaMensual)
                    .Where(o => o.TipoPlanificacion == TipoPlanificacion.CorteIngenieriaMensual)
                    .Count();

                if (count == 0)
                {
                    var entity = new PlanificacionTimesheet()
                    {
                        TipoPlanificacion = TipoPlanificacion.CorteIngenieriaMensual,
                        Fecha = corteIngenieriaMensual,
                        Descripcion = "Corte ingeniería mensual",
                    };
                    Repository.Insert(entity);
                }
            }

            foreach (var rsIngenieria in rsIngenieriaMensual)
            {
                var count = Repository.GetAll()
                    .Where(o => o.Fecha == rsIngenieria)
                    .Where(o => o.TipoPlanificacion == TipoPlanificacion.RsIngenieriaCsMensual)
                    .Count();

                if (count == 0)
                {
                    var entity = new PlanificacionTimesheet()
                    {
                        TipoPlanificacion = TipoPlanificacion.RsIngenieriaCsMensual,
                        Fecha = rsIngenieria,
                        Descripcion = "RS Ingeniería SC mensual",
                    };
                    Repository.Insert(entity);
                }
            }

            foreach (var certificacionMensual in certificacionesMensuales)
            {
                var count = Repository.GetAll()
                    .Where(o => o.Fecha == certificacionMensual)
                    .Where(o => o.TipoPlanificacion == TipoPlanificacion.Certificacion)
                    .Count();

                if (count == 0)
                {
                    var entity = new PlanificacionTimesheet()
                    {
                        TipoPlanificacion = TipoPlanificacion.Certificacion,
                        Fecha = certificacionMensual,
                        Descripcion = "C. Certificación mensual",
                    };
                    Repository.Insert(entity);
                }
            }

            return true;
        }

        public ExcelPackage DescargarPlanificacionPorMes(DateTime fechaReporte)
        {
            ExcelPackage excel = new ExcelPackage();

            string filename = System.Web.HttpContext.Current.Server.MapPath("~/Views/PlantillaWord/CertificacionIngenieria/FormatoPlanificacionTimesheet.xlsx");
            if (File.Exists((string)filename))
            {
                FileInfo newFile = new FileInfo(filename);
                ExcelPackage pck = new ExcelPackage(newFile);
                excel.Workbook.Worksheets.Add("Hoja1", pck.Workbook.Worksheets[1]);
            }

            ExcelWorksheet planificacion = excel.Workbook.Worksheets[1];

            var firstDay = new DateTime(fechaReporte.Year, fechaReporte.Month, 1);
            var lastDay = firstDay.AddMonths(1).AddDays(-1);

            var fechas = Enumerable.Range(0, 1 + lastDay
                .Subtract(firstDay).Days)
                .Select(offset => firstDay
                .AddDays(offset))
                .ToArray();

            var contadorIndiceColumna = (int)firstDay.DayOfWeek;
            var contadorFila = 3;
            var contadorFilaDatos = contadorFila + 1;

            /* Nombre del mes año */
            var nombreMes = fechaReporte.ToString("MMMM", CultureInfo.CreateSpecificCulture("es"));
            planificacion.Cells["A1"].Value = char.ToUpper(nombreMes[0]) + nombreMes.Substring(1) + " " + fechaReporte.Year;
            foreach (var fecha in fechas)
            {
                /* Obtener la columna para el excel */
                var column = ObtenerLetraDeLaColumna((int)fecha.DayOfWeek);
                /* Obtiene el día del mes para la cabecera*/
                var dateOfMonth = fecha.Day;
                /* Buscar los registros en la BDD de la fecha */
                var events = Repository.GetAll().Where(o => o.Fecha == fecha).Select(o => o.Descripcion).ToArray();
                var eventsString = string.Join(", ", events);
                /* Llenar la cabecera */
                planificacion.Cells[column + contadorFila].Value = dateOfMonth;
                /* Llenar descripción del día*/
                planificacion.Cells[column + contadorFilaDatos].Value = eventsString;

                if (column == "G")
                {
                    contadorFila += 2;
                    contadorFilaDatos += 2;
                }

            }

            return excel;
        }

        public string ObtenerLetraDeLaColumna(int columna)
        {
            switch (columna)
            {
                case 1:
                    return "A";
                case 2:
                    return "B";
                case 3:
                    return "C";
                case 4:
                    return "D";
                case 5:
                    return "E";
                case 6:
                    return "F";
                case 0:
                    return "G";
                default:
                    return "";
            }
        }

        public bool EliminarPlanificacion(int id)
        {
            Repository.Delete(id);
            return true;
        }
    }
}
