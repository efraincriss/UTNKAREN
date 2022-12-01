using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.framework;
using com.cpp.calypso.proyecto.aplicacion.CertificacionIngenieria.Dto;
using com.cpp.calypso.proyecto.aplicacion.CertificacionIngenieria.Service;
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
    public class DetalleIndirectosIngenieriaController : BaseAccesoSpaController<DetalleIndirectosIngenieria, DetalleIndirectosIngenieriaDto, PagedAndFilteredResultRequestDto>
    {
        private readonly DetalleIndirectosIngenieriaAsyncBaseCrudAppService _indirectosService;

        public DetalleIndirectosIngenieriaController(
            IHandlerExcepciones manejadorExcepciones,
            IViewService viewService,
            DetalleIndirectosIngenieriaAsyncBaseCrudAppService indirectosService
        ) : base(manejadorExcepciones, viewService)
        {
            _indirectosService = indirectosService;
        }

        public ActionResult Index()
        {
            var model = new FormReactModelView();
            model.Id = "detalles_indirectos_ingenieria_container";
            model.ReactComponent = "~/Scripts/build/detalles_indirectos_ingenieria_container.js";

            if (!string.IsNullOrEmpty(Title))
            {
                model.Title = Title;
            }

            ViewBag.ruta = new string[] { "Inicio", "Configuraciones", "Detalles Indirectos" };

            return View(model);
        }

        #region Api
        [HttpGet]
        public ActionResult ObtenerIndirectosIngenieriaPorFechas(DateTime? fechaInicio, DateTime? fechaFin)
        {
            var list = _indirectosService.ObtenerIndirectosIngenieriaPorFechas(fechaDesde: fechaInicio, fechaHasta: fechaFin);
            return WrapperResponseGetApi(ModelState, () => list);
        }

        [HttpGet]
        public ActionResult CalcularDiasLaborados(int colaboradorId, DateTime fechaInicio, DateTime fechaFin)
        {
            var result = _indirectosService.CalcularDiasLaborados(colaboradorId, fechaDesde: fechaInicio, fechaHasta: fechaFin);
            return new JsonResult
            {
                Data = new { success = result.Success, message = result.Message, contador = result.Contador },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        [HttpDelete]
        public ActionResult Eliminar(int id)
        {
            var result = _indirectosService.Eliminar(id);
            return new JsonResult
            {
                Data = new { success = result.Success, message = result.Message }
            };
        }

        [HttpPost]
        public async Task<ActionResult> CrearDetalleIndirectoIngenieriaAsync(DetalleIndirectosIngenieriaDto input)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _indirectosService.CrearIndirectoAsync(input);
                    return new JsonResult
                    {
                        Data = new { success = result.Success, message = result.Message }
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
        public async Task<ActionResult> EditarDetalleIndirectoIngenieriaAsync(DetalleIndirectosIngenieriaDto input)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _indirectosService.ActualizarAsync(input);
                    return new JsonResult
                    {
                        Data = new { success = result.Success, message = result.Message }
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

        [System.Web.Mvc.HttpPost]
        public ActionResult DescargarPlantillaCargaMasivaGastosIndirectos()
        {
            var excel = _indirectosService.DescargarPlantillaCargaMasivaGastosIndirectos();
            string excelName = "CargaDeGastosIndirectos-" + DateTime.Now.ToShortDateString();
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

        [System.Web.Mvc.HttpPost]
        public async Task<ActionResult> CargaMasivaDeGastosIndirectosAsync(HttpPostedFileBase file)
        {
            var excel = await _indirectosService.CargaMasivaDeGastosIndirectosAsync(file);
            string excelName = "ResultadoDeCargaDeIndirectos" + DateTime.Now.ToShortDateString();
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