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
    public class FeriadoController : BaseAccesoSpaController<Feriado, FeriadoDto, PagedAndFilteredResultRequestDto>
    {
        private readonly IFeriadoAsyncBaseCrudAppService _feriadoService;

        public FeriadoController(
            IHandlerExcepciones manejadorExcepciones, 
            IViewService viewService,
            IFeriadoAsyncBaseCrudAppService feriadoService
            ) : base(manejadorExcepciones, viewService)
        {
            _feriadoService = feriadoService;
        }

        public ActionResult Index()
        {
            var model = new FormReactModelView();
            model.Id = "dias_feriado_container";
            model.ReactComponent = "~/Scripts/build/dias_feriado_container.js";

            if (!string.IsNullOrEmpty(Title))
            {
                model.Title = Title;
            }


            ViewBag.ruta = new string[] { "Inicio", "Catálogos", "Feriados" };

            return View(model);
        }

        #region Api
        [HttpGet]
        public ActionResult ObtenerFeriados()
        {
            var rubros = _feriadoService.GetFeriados();
            return WrapperResponseGetApi(ModelState, () => rubros);
        }

        [HttpDelete]
        public ActionResult EliminarFeriado(int id)
        {
            var result = _feriadoService.EliminarFeriado(id);
            return new JsonResult
            {
                Data = new { success = result }
            };
        }

        [HttpPost]
        public async Task<ActionResult> CrearFeriadoAsync(FeriadoDto input)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _feriadoService.CrearFeriadoAsync(input);
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
        public ActionResult EditarFeriadoAsync(FeriadoDto input)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var dto = _feriadoService.ActualizarFeriadoAsync(input);
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
        #endregion
    }
}