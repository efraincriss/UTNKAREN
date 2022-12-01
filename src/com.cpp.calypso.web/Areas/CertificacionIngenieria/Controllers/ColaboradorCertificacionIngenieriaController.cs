using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.framework;
using com.cpp.calypso.proyecto.aplicacion.CertificacionIngenieria.Dto;
using com.cpp.calypso.proyecto.aplicacion.CertificacionIngenieria.Interface;
using com.cpp.calypso.proyecto.aplicacion.Interfaces;
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
    public class ColaboradorCertificacionIngenieriaController : BaseAccesoSpaController<ColaboradorCertificacionIngenieria, ColaboradorCertificacionIngenieriaDto, PagedAndFilteredResultRequestDto>
    {
        private readonly IColaboradorCertificacionIngenieriaAsyncBaseCrudAppService _colaboradorCertificacioningenieriaServices;
        private readonly ICatalogoAsyncBaseCrudAppService _catalogoService;

        public ColaboradorCertificacionIngenieriaController(
            IHandlerExcepciones manejadorExcepciones, 
            IViewService viewService,
            IColaboradorCertificacionIngenieriaAsyncBaseCrudAppService colaboradorCertificacioningenieriaServices,
            ICatalogoAsyncBaseCrudAppService catalogoService
        ) : base(manejadorExcepciones, viewService)
        {
            _colaboradorCertificacioningenieriaServices = colaboradorCertificacioningenieriaServices;
            _catalogoService = catalogoService;
        }

        public ActionResult Index()
        {
            var model = new FormReactModelView();
            model.Id = "colaborador_certificacion_ingenieria_container";
            model.ReactComponent = "~/Scripts/build/colaborador_certificacion_ingenieria_container.js";

            if (!string.IsNullOrEmpty(Title))
            {
                model.Title = Title;
            }


            ViewBag.ruta = new string[] { "Inicio", "Catálogos", "Colaboradores" };

            return View(model);
        }

        #region Api
        [HttpGet]
        public ActionResult ObtenerColaboradores()
        {
            var colaboradores = _colaboradorCertificacioningenieriaServices.GetColaboradores();
            return WrapperResponseGetApi(ModelState, () => colaboradores);
        }

        [HttpGet]
        public ActionResult ObtenerParametrizacionPorColaboradorId(int id)
        {
            var parametrizaciones = _colaboradorCertificacioningenieriaServices.GetParametrizacionPorColaboradorId(id);
            return WrapperResponseGetApi(ModelState, () => parametrizaciones);
        }

        [HttpGet]
        public ActionResult GetCatalogos()
        {
            var parametrizaciones = _colaboradorCertificacioningenieriaServices.ObtenerCatalogos();
            return WrapperResponseGetApi(ModelState, () => parametrizaciones);
        }

        [HttpDelete]
        public ActionResult Eliminar(int id)
        {
            var result = _colaboradorCertificacioningenieriaServices.Eliminar(id);
            return new JsonResult
            {
                Data = new { success = result }
            };
        }

        [HttpPost]
        public async Task<ActionResult> CrearAsync(ColaboradorCertificacionIngenieriaDto input)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _colaboradorCertificacioningenieriaServices.CrearAsync(input);
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
        public async Task<ActionResult> EditarAsync(ColaboradorCertificacionIngenieriaDto input)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _colaboradorCertificacioningenieriaServices.ActualizarAsync(input);
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