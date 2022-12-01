using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.framework;
using com.cpp.calypso.proyecto.aplicacion.CertificacionIngenieria.Dto;
using com.cpp.calypso.proyecto.aplicacion.CertificacionIngenieria.Interface;
using com.cpp.calypso.proyecto.dominio.CertificacionIngenieria;
using com.cpp.calypso.web.Areas.Accesos.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using JsonResult = System.Web.Mvc.JsonResult;

namespace com.cpp.calypso.web.Areas.CertificacionIngenieria.Controllers
{
    public class PorcentajeAvanceIngenieriaController : BaseAccesoSpaController<PorcentajeAvanceIngenieria, PorcentajeAvanceIngenieriaDto, PagedAndFilteredResultRequestDto>
    {
        private readonly IPorcentajeAvanceIngenieriaAsyncBaseCrudAppService _porcentajeAvanceIngenieria;

        public PorcentajeAvanceIngenieriaController(
            IHandlerExcepciones manejadorExcepciones, 
            IViewService viewService,
            IPorcentajeAvanceIngenieriaAsyncBaseCrudAppService porcentajeAvanceIngenieria
        ) : base(manejadorExcepciones, viewService)
        {
            _porcentajeAvanceIngenieria = porcentajeAvanceIngenieria;
        }

        public ActionResult Index()
        {
            var model = new FormReactModelView();
            model.Id = "porcentaje_avance_ingenieria";
            model.ReactComponent = "~/Scripts/build/porcentaje_avance_ingenieria.js";

            if (!string.IsNullOrEmpty(Title))
            {
                model.Title = Title;
            }


            ViewBag.ruta = new string[] { "Inicio", "Configuraciones", "Avance Ingeniería" };

            return View(model);
        }

        #region Api

        [HttpGet]
        public ActionResult ObtenerAvancesIngenieriaPorFecha(DateTime fechaDesde)
        {
            var list = _porcentajeAvanceIngenieria.ObtenerAvancesIngenieriaPorFecha(fechaDesde: fechaDesde);
            return WrapperResponseGetApi(ModelState, () => list);
        }

        [HttpGet]
        public ActionResult ObtenerProyectos()
        {
            var list = _porcentajeAvanceIngenieria.ObtenerProyectos();
            return WrapperResponseGetApi(ModelState, () => list);
        }

        [HttpGet]
        public ActionResult ObtenerCatalogos()
        {
            var parametrizaciones = _porcentajeAvanceIngenieria.ObtenerCatalogos();
            return WrapperResponseGetApi(ModelState, () => parametrizaciones);
        }

        [HttpDelete]
        public ActionResult Eliminar(int id)
        {
            var result = _porcentajeAvanceIngenieria.Eliminar(id);
            return new JsonResult
            {
                Data = new { success = result.Success, message = result.Message }
            };
        }

        [HttpPost]
        public async Task<ActionResult> CrearAvanceIngenieriaAsync(PorcentajeAvanceIngenieriaDto input)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _porcentajeAvanceIngenieria.CrearAsync(input);
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
        public async Task<ActionResult> EditarAvanceIngenieriaAsync(PorcentajeAvanceIngenieriaDto input)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _porcentajeAvanceIngenieria.ActualizarAsync(input);
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
        #endregion
    }
}