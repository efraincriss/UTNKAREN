using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.framework;
using com.cpp.calypso.proyecto.aplicacion.Documentos.Dto;
using com.cpp.calypso.proyecto.aplicacion.Documentos.Interface;
using com.cpp.calypso.proyecto.dominio.Documentos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using JsonResult = System.Web.Mvc.JsonResult;

namespace com.cpp.calypso.web.Areas.Documentos.Controllers
{
    public class SeccionController : BaseAccesoSpaController<Seccion, SeccionDto, PagedAndFilteredResultRequestDto>
    {
        private readonly ICarpetaAsyncBaseCrudAppService _carpetaRepository;
        private readonly ISeccionAsyncBaseCrudAppService _seccionRepository;

        public SeccionController(
            IHandlerExcepciones manejadorExcepciones,
            IViewService viewService,
            ISeccionAsyncBaseCrudAppService seccionRepository,
            ICarpetaAsyncBaseCrudAppService carpetaRepository
            ) : base(manejadorExcepciones, viewService)
        {
            _seccionRepository = seccionRepository;
            _carpetaRepository = carpetaRepository;
        }

        public ActionResult Secciones(int documentoId)
        {
            var model = new FormReactModelView();
            model.Id = "gestion_documentos_contratos";
            model.ReactComponent = "~/Scripts/build/gestion_secciones_container.js";

            if (!string.IsNullOrEmpty(Title))
            {
                model.Title = Title;
            }


            ViewBag.ruta = new string[] { "Inicio", "Gestión de Contratos", "Listado Contratos", "Gestión de Contenido" };

            return View(model);
        }

        #region Api
        [HttpGet]
        public ActionResult ObtenerSeccionPorId(int id)
        {
            var documentos = _seccionRepository.ObtenerSeccionPorId(id);
            return WrapperResponseGetApi(ModelState, () => documentos);
        }

        [HttpGet]
        public ActionResult ObtenerEstructuraArbol(int id)
        {
            var arbol = _seccionRepository.GenerarArbol(id);
            return WrapperResponseGetApi(ModelState, () => arbol);
        }

        [HttpDelete]
        public ActionResult EliminarSeccion(int id)
        {
            var result = _seccionRepository.EliminarSeccion(id);
            return new JsonResult
            {
                Data = new { success = result.Eliminado, result = result.Error }
            };
        }
        [HttpDelete]
        public ActionResult EliminarImagen(int id)
        {
            var result = _seccionRepository.EliminarImagen(id);
            return new JsonResult
            {
                Data = new { success = result.Eliminado, result = result.Error }
            };
        }

        [HttpPost]
        public async Task<ActionResult> CrearDocumentoAsync(SeccionDto input)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var Encryptados = _carpetaRepository.superaCaracteres(input.Contenido);
                    if (Encryptados > 28000)
                    {
                        return new JsonResult
                        {
                            Data = new { success = false, result = Encryptados, errors = "MAXIM_ENCRYPT" }
                        };
                    }
                    var dto = await _seccionRepository.CrearSeccionAsync(input);
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
        public ActionResult EditarSeccionAsync(SeccionDto input)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var dto = _seccionRepository.ActualizarSeccion(input);
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
        public ActionResult GuardarArbol(List<EstructuraArbol> arbol)
        {
            var result = _seccionRepository.GuardarArbolDragDrop(arbol);
            return new JsonResult
            {
                Data = new { success = result, result = "Error al actualizar el árbol" }
            };
        }
        #endregion
    }
}