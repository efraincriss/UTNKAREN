using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.framework;
using com.cpp.calypso.proyecto.aplicacion.CertificacionIngenieria.Dto;
using com.cpp.calypso.proyecto.aplicacion.CertificacionIngenieria.Interface;
using com.cpp.calypso.web.Areas.Accesos.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JsonResult = System.Web.Mvc.JsonResult;

namespace com.cpp.calypso.web.Areas.CertificacionIngenieria.Controllers
{
    public class ParametrosController : BaseAccesoSpaController<ParametroSistema, ParametroSistemaDto, PagedAndFilteredResultRequestDto>
    {
        private readonly IParametroSistemaAsyncBaseCrudAppService _parametrosSistemaService;

        public ParametrosController(
            IHandlerExcepciones manejadorExcepciones, 
            IViewService viewService,
            IParametroSistemaAsyncBaseCrudAppService parametrosSistemaService
        ) : base(manejadorExcepciones, viewService)
        {
            _parametrosSistemaService = parametrosSistemaService;
        }

        // GET: CertificacionIngenieria/Parametros
        public ActionResult Index()
        {

            var model = new FormReactModelView();
            model.Id = "dias_feriado_container";
            model.ReactComponent = "~/Scripts/build/parametros_sistema_container.js";

            if (!string.IsNullOrEmpty(Title))
            {
                model.Title = Title;
            }


            ViewBag.ruta = new string[] { "Inicio", "Catálogos", "Parámetros" };

            return View(model);
        }


        [HttpGet]
        public ActionResult ObtenerParametrosPorModuloCertificacion()
        {
            var rubros = _parametrosSistemaService.ObtenerParametrosPorModuloCertificacion();
            return WrapperResponseGetApi(ModelState, () => rubros);
        }

        [HttpPost]
        public ActionResult ActualizarParametroAsync(ParametroSistemaDto input)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var dto = _parametrosSistemaService.ActualizarParametroAsync(input);
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
    }
}