using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.framework;
using com.cpp.calypso.proyecto.aplicacion.CertificacionIngenieria.Dto;
using com.cpp.calypso.proyecto.aplicacion.CertificacionIngenieria.Interface;
using com.cpp.calypso.proyecto.aplicacion.CertificacionIngenieria.Service;
using com.cpp.calypso.proyecto.aplicacion.Interfaces;
using com.cpp.calypso.proyecto.dominio.CertificacionIngenieria;
using com.cpp.calypso.web.Areas.Accesos.Controllers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using JsonResult = com.cpp.calypso.framework.JsonResult;

namespace com.cpp.calypso.web.Areas.CertificacionIngenieria.Controllers
{
    public class ColaboradorRubroController : BaseAccesoSpaController<ColaboradorRubroIngenieria, ColaboradorRubroIngenieriaDto, PagedAndFilteredResultRequestDto>
    {
        private readonly IColaboradorRubroIngenieriaAsyncBaseCrudAppService _colaboradorrubroservices;
        private readonly IContratoAsyncBaseCrudAppService _contratoService;

        public ColaboradorRubroController(
            IHandlerExcepciones manejadorExcepciones, 
            IViewService viewService,
            IColaboradorRubroIngenieriaAsyncBaseCrudAppService colaboradorrubroservices,
            IContratoAsyncBaseCrudAppService contratoService
        ) : base(manejadorExcepciones, viewService)
        {
            _colaboradorrubroservices = colaboradorrubroservices;
            _contratoService = contratoService;
        }

        // GET: CertificacionIngenieria/ColaboradorRubro
        public ActionResult Index()
        {
            var model = new FormReactModelView();
            model.Id = "colaborador_rubro_container";
            model.ReactComponent = "~/Scripts/build/colaborador_rubro_container.js";

            if (!string.IsNullOrEmpty(Title))
            {
                model.Title = Title;
            }


            ViewBag.ruta = new string[] { "Inicio", "Configuraciones", "Preciario Colaborador" };

            return View(model);
        }


        #region Api
        [HttpGet]
        public ActionResult ObtenerColaboresRubros()
        {
            var rubros = _colaboradorrubroservices.ObtenerColaboresRubros();
            return WrapperResponseGetApi(ModelState, () => rubros);
        }

        [HttpGet]
        public ActionResult ObtenerColaboresRubrosConFiltro(DateTime? fechaInicio, DateTime? fechaFin)
        {
            var rubros = _colaboradorrubroservices.ObtenerColaboresRubrosConFiltros(fechaInicio, fechaFin);
            return WrapperResponseGetApi(ModelState, () => rubros);
        }

        [HttpGet]
        public ActionResult GetContratos()
        {
            var contratos = _contratoService.GetContratosDto();
            return WrapperResponseGetApi(ModelState, () => contratos);
        }


        [HttpDelete]
        public ActionResult EliminarColaboradorRubro(int id)
        {
            var result = _colaboradorrubroservices.Eliminar(id);
            return new JsonResult
            {
                Data = new { success = result.Success, result = result.Message }
            };
        }

        [HttpGet]
        public ActionResult ObtenerRubrosPorContrato(int id)
        {
            var rubros = _colaboradorrubroservices.GetPreciariosPorContrato(id);
            return WrapperResponseGetApi(ModelState, () => rubros);
        }

        [HttpGet]
        public ActionResult ComprobarExistenciaItemEnContrato(int id)
        {
            var response = _colaboradorrubroservices.ComprobarExistenciaItemEnContrato(id);
            return WrapperResponseGetApi(ModelState, () => response);
        }

        [HttpGet]
        public ActionResult ObtenerItems()
        {
            var response = _colaboradorrubroservices.GetItems();
            return WrapperResponseGetApi(ModelState, () => response);
        }

        [HttpPost]
        public async Task<ActionResult> CrearColaboradorRubroAsync(ColaboradorRubroIngenieriaDto input)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var dto = await _colaboradorrubroservices.CrearColaboradorRubroAsync(input);
                    return new JsonResult
                    {
                        Data = new { success = dto.Success, message = dto.Message }
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
        public ActionResult EditarColaboradorRubroAsync(ColaboradorRubroIngenieriaDto input)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var dto = _colaboradorrubroservices.EditarColaboradorRubroAsync(input);
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

        [System.Web.Mvc.HttpPost]
        public ActionResult DescargarPlantillaCargaMasivaDeTarifas(int contratoId)
        {
            var excel = _colaboradorrubroservices.DescargarPlantillaCargaMasivaTarifas(contratoId);
            string excelName = "CargaDeTarifas-" + DateTime.Now.ToShortDateString();
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
        public ActionResult CargaMasivaDeTarifas(HttpPostedFileBase file)
        {
            var excel = _colaboradorrubroservices.CargaMasivaDeTarifas(file);
            string excelName = "ResultadoDeCargaDeTarifas" + DateTime.Now.ToShortDateString();
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