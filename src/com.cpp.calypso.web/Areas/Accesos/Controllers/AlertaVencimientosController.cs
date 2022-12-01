using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.proyecto.aplicacion.Dto;
using com.cpp.calypso.proyecto.dominio;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using com.cpp.calypso.framework;
using com.cpp.calypso.proyecto.aplicacion.Acceso.Dto;
using com.cpp.calypso.proyecto.aplicacion.Acceso.Interface;
using com.cpp.calypso.proyecto.aplicacion.Interfaces;
using OfficeOpenXml;
using JsonResult = com.cpp.calypso.framework.JsonResult;

namespace com.cpp.calypso.web.Areas.Accesos.Controllers
{
    public class AlertaVencimientosController : BaseAccesoSpaController<ColaboradorRequisito, ColaboradorRequisitoDto, PagedAndFilteredResultRequestDto>
    {
        private readonly IRequisitosAsyncBaseCrudAppService _requisitoService;
        private readonly IAlertaVencimientosAsyncBaseCrudAppService _alertaVencimientoService;

        public AlertaVencimientosController(
            IHandlerExcepciones manejadorExcepciones,
            IViewService viewService,
            IRequisitosAsyncBaseCrudAppService requisitoService,
            IAlertaVencimientosAsyncBaseCrudAppService alertaVencimientoService
            ) : base(manejadorExcepciones, viewService)
        {
            _requisitoService = requisitoService;
            _alertaVencimientoService = alertaVencimientoService;
        }

        public ActionResult Index()
        {
            var model = new FormReactModelView();
            model.Id = "alerta_vencimiento_container";
            model.ReactComponent = "~/Scripts/build/alerta_vencimiento.js";
            model.Title = "Vencimiento de Requisitos";
            ViewBag.ruta = new string[] { "Inicio", "Alerta de Vencimiento de Requisitos" };
            return View(model);
        }

        #region API

        public async System.Threading.Tasks.Task<JsonResult> ObtenerRequisitos()
        {
            try
            {
                var dtos = await _requisitoService.GetAll();
                return new JsonResult
                {
                    Data = new { success = true, result = dtos }
                };
            }
            catch (Exception ex)
            {
                return new JsonResult
                {
                    Data = new { success = false, errors = ModelState.ToSerializedDictionary() }
                };
            }
        }


        [HttpPost]
        public ActionResult ObtenerVencimientos(InputAlertaVencimientoReporteDto input)
        {

            ExcelPackage excel = _alertaVencimientoService.ExcelCumplimientoIndividual(input);

            string excelName = "Lista vencimientos" + DateTime.Now.ToShortDateString();
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