using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.framework;
using com.cpp.calypso.proyecto.aplicacion.CertificacionIngenieria.Dto;
using com.cpp.calypso.proyecto.aplicacion.CertificacionIngenieria.Interface;
using com.cpp.calypso.proyecto.dominio.CertificacionIngenieria;
using com.cpp.calypso.web.Areas.Accesos.Controllers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using JsonResult = System.Web.Mvc.JsonResult;

namespace com.cpp.calypso.web.Areas.CertificacionIngenieria.Controllers
{
    public class PlanificacionTimesheetController : BaseAccesoSpaController<PlanificacionTimesheet, PlanificacionTimesheetDto, PagedAndFilteredResultRequestDto>
    {
        private readonly IPlanificacionTimesheetBaseCrudAppService _planificacionTimesheetService;

        public PlanificacionTimesheetController(
            IHandlerExcepciones manejadorExcepciones, 
            IViewService viewService,
            IPlanificacionTimesheetBaseCrudAppService planificacionTimesheetService
            ) : base(manejadorExcepciones, viewService)
        {
            _planificacionTimesheetService = planificacionTimesheetService;
        }

        public ActionResult Index()
        {
            var model = new FormReactModelView();
            model.Id = "planificacion_timesheet_container";
            model.ReactComponent = "~/Scripts/build/planificacion_timesheet_container.js";

            if (!string.IsNullOrEmpty(Title))
            {
                model.Title = Title;
            }


            ViewBag.ruta = new string[] { "Inicio", "Configuración", "Planificación Timesheet" };

            return View(model);
        }

        #region Api
        [HttpGet]
        public ActionResult ObtenerPlanificaciones()
        {
            var rubros = _planificacionTimesheetService.GetPlanificaciones();
            return WrapperResponseGetApi(ModelState, () => rubros);
        }

        [HttpDelete]
        public ActionResult EliminarPlanificacion(int id)
        {
            var result = _planificacionTimesheetService.EliminarPlanificacion(id);
            return new JsonResult
            {
                Data = new { success = result }
            };
        }

        [HttpPost]
        public async Task<ActionResult> CrearPlanificacionAsync(PlanificacionTimesheetDto input)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _planificacionTimesheetService.CrearPlanificacionAsync(input);
                    return new JsonResult
                    {
                        Data = new { success = result }
                    };
                }
            }
            catch (Exception ex)
            {
                var result = ManejadorExcepciones.HandleException(ex);
                ModelState.AddModelError("", result.Message);
            }
            return new JsonResult
            {
                Data = new { success = false, errors = ModelState.ToSerializedDictionary() }
            };
        }

        [HttpPost]
        public ActionResult CrearPlanificacionPorAño(int year)
        {
            try
            {
                var result = _planificacionTimesheetService.CrearPlanificacionPorAño(year);
                return new JsonResult
                {
                    Data = new { success = result }
                };
            }
            catch (Exception ex)
            {
                var result = ManejadorExcepciones.HandleException(ex);
                ModelState.AddModelError("", result.Message);
            }
            return new JsonResult
            {
                Data = new { success = false, errors = ModelState.ToSerializedDictionary() }
            };
        }

        [HttpPost]
        public async Task<ActionResult> EditarPlanificacionAsync(PlanificacionTimesheetDto input)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var dto = await _planificacionTimesheetService.ActualizarPlanificacionAsync(input);
                    return new JsonResult
                    {
                        Data = new { success = true }
                    };
                }
            }
            catch (Exception ex)
            {
                var result = ManejadorExcepciones.HandleException(ex);
                ModelState.AddModelError("", result.Message);
            }
            return new JsonResult
            {
                Data = new { success = false, errors = ModelState.ToSerializedDictionary() }
            };
        }

        [HttpPost]
        public ActionResult DescargarPlanificacionPorMes(DateTime fecha)
        {
            var excel = _planificacionTimesheetService.DescargarPlanificacionPorMes(fecha);
            string excelName = "Planificacion-" + DateTime.Now.ToShortDateString();
            using (var memoryStream = new MemoryStream())
            {
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment; filename=" + excelName + ".xlsx");
                excel.SaveAs(memoryStream);
                memoryStream.WriteTo(Response.OutputStream);
                Response.Flush();
                Response.End();
                return Content("");
            }
        }
        #endregion
    }
}