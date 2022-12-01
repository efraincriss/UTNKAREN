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
    public class ImagenSeccionController : BaseAccesoSpaController<ImagenSeccion, ImagenSeccionDto, PagedAndFilteredResultRequestDto>
    {
        private readonly IImagenSeccionAsyncBaseCrudAppService _imagenSeccionService;

        public ImagenSeccionController(
            IHandlerExcepciones manejadorExcepciones, 
            IViewService viewService,
            IImagenSeccionAsyncBaseCrudAppService imagenSeccionService
            ) : base(manejadorExcepciones, viewService)
        {
            _imagenSeccionService = imagenSeccionService;
        }

        #region Api
        [HttpDelete]
        public ActionResult EliminarSeccion(int id)
        {
            var result = _imagenSeccionService.EliminarImagen(id);
            return new System.Web.Mvc.JsonResult
            {
                Data = new { success = result.Eliminado, result = result.Error }
            };
        }

        [HttpGet]
        public ActionResult ObtenerImagenes(int id)
        {
            var imagenes = _imagenSeccionService.ObtenerImagensPorSeccion(id);
            return WrapperResponseGetApi(ModelState, () => imagenes);
        }

        [HttpPost]
        public async Task<ActionResult> CrearImagenes(List<ImagenSeccionDto> input)
        {
            try
            {
                var dto = await _imagenSeccionService.CrearImagenesSeccionAsync(input);
                if (dto == "OK")
                {
                    return new JsonResult
                    {
                        Data = new { success = true }
                    };
                }
                else if (dto == "ENCRYPTADO_MAYOR_20") {
                    return new JsonResult
                    {
                        Data = new { success = false, errors = "EL tamaño de la imagen supera las 20 partes del encryptado" }
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