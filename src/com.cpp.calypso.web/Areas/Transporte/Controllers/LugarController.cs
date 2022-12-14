using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Results;
using System.Web.Mvc;
using Abp.Application.Services.Dto;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.framework;
using com.cpp.calypso.proyecto.aplicacion.Transporte.Dto;
using com.cpp.calypso.proyecto.aplicacion.Transporte.Interface;
using com.cpp.calypso.proyecto.dominio.Transporte;
using JsonResult = com.cpp.calypso.framework.JsonResult;

namespace com.cpp.calypso.web.Areas.Transporte.Controllers
{
    public class LugarController : BaseTransporteSpaController<Lugar, LugarDto, PagedAndFilteredResultRequestDto>
    {
        private readonly ILugarAsyncBaseCrudAppService _lugarService;

        public LugarController(
            IHandlerExcepciones manejadorExcepciones, 
            IViewService viewService, 
            IAsyncBaseCrudAppService<Lugar, LugarDto, PagedAndFilteredResultRequestDto, LugarDto> entityService,
            ILugarAsyncBaseCrudAppService lugarService
            ) : base(manejadorExcepciones, viewService, entityService)
        {
            _lugarService = lugarService;
        }

        public ActionResult Index()
        {

            var model = new FormReactModelView();
            model.Id = "lugares";
            model.ReactComponent = "~/Scripts/build/gestion_lugares.js";
            model.Title = "Gestión de Lugares";
            if (!string.IsNullOrEmpty(Title))
            {
                model.Title = Title;
            }
            ViewBag.ruta = new string[] { "Inicio", "Lugares", "Listado de Lugares" };
            return View(model);
        }


        #region Api

        public override async Task<ActionResult> GetAllApi()
        {
            var result = await _lugarService.Listar();
            
            return WrapperResponseGetApi(ModelState, () => result);

        }

        [HttpPost]
        public async Task<JsonResult> CreateApi(LugarDto input)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var canCreate = _lugarService.canCreate(input);
                    if (canCreate == "OK")
                    {
                        input.Codigo = _lugarService.nextcode();
                        var resultEntity = await _lugarService.Create(input);
                        var result = new { id = resultEntity.Id };
                        return new JsonResult
                        {
                            Data = new { success = true, created = true, result }
                        };
                    }
                    return new JsonResult
                    {
                        Data = new { success = true, created = false, errors = canCreate }
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
        public async Task<JsonResult> UpdateApi(LugarDto input)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var canUpdate = _lugarService.canUpdate(input);
                    if (canUpdate == "OK")
                    {
                        var resultEntity = await _lugarService.Update(input);
                        var result = new { id = resultEntity.Id };
                        return new JsonResult
                        {
                            Data = new { success = true,  updated = true, result }
                        };
                    }
                    
                    return new JsonResult
                    {
                        Data = new { success = true, updated = false, errors = canUpdate }
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
        public async Task<JsonResult> DeleteApi(int? id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var canDelete = _lugarService.canDelete(id.Value);
                    if (canDelete == "OK")
                    {
                        await _lugarService.Delete(new EntityDto<int>(id.Value));
                        return new JsonResult
                        {
                            Data = new { success = true, deleted = true }
                        };
                    }
                    return new JsonResult
                    {
                        Data = new { success = true, deleted = false, errors = canDelete }
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