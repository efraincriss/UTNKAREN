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
    public class PorcentajeIndirectoIngenieriaController : BaseAccesoSpaController<PorcentajeIndirectoIngenieria, FeriadoDto, PagedAndFilteredResultRequestDto>
    {
        private readonly IPorcentajeIndirectoIngenieriaAsyncBaseCrudAppService _porcentajeIndirectosService;

        public PorcentajeIndirectoIngenieriaController(
            IHandlerExcepciones manejadorExcepciones, 
            IViewService viewService,
            IPorcentajeIndirectoIngenieriaAsyncBaseCrudAppService porcentajeIndirectosService
            ) : base(manejadorExcepciones, viewService)
        {
            _porcentajeIndirectosService = porcentajeIndirectosService;
        }


        #region Api
        [HttpDelete]
        public ActionResult Eliminar(int id)
        {
            var result = _porcentajeIndirectosService.Eliminar(id);
            return new System.Web.Mvc.JsonResult
            {
                Data = new { success = result.Success, message = result.Message }
            };
        }

        [HttpPost]
        public async Task<ActionResult> CrearPorcentajeIndirectoIngenieriaAsync(PorcentajeIndirectoIngenieriaDto input)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _porcentajeIndirectosService.CrearAsync(input);
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
        public async Task<ActionResult> EditarPorcentajeIndirectoIngenieriaAsync(PorcentajeIndirectoIngenieriaDto input)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _porcentajeIndirectosService.ActualizarAsync(input);
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

        [HttpGet]
        public ActionResult ObtenerPorcentajesDelDetalleIndirecto(int id)
        {
            var list = _porcentajeIndirectosService.ObtenerPorcentajesDelDetalleIndirecto(id);
            return WrapperResponseGetApi(ModelState, () => list);
        }
        #endregion
    }
}