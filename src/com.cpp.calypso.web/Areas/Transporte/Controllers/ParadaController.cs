using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
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
    public class ParadaController : BaseTransporteSpaController<Parada, ParadaDto, PagedAndFilteredResultRequestDto>
    {
        private readonly IParadaAsyncBaseCrudAppService _paradaService;

        public ParadaController(
            IHandlerExcepciones manejadorExcepciones, 
            IViewService viewService, 
            IAsyncBaseCrudAppService<Parada, ParadaDto, PagedAndFilteredResultRequestDto, ParadaDto> entityService,
            IParadaAsyncBaseCrudAppService paradaService
            ) : base(manejadorExcepciones, viewService, entityService)
        {
            _paradaService = paradaService;
        }

        public ActionResult Index()
        {

            var model = new FormReactModelView();
            model.Id = "paradas";
            model.ReactComponent = "~/Scripts/build/gestion_paradas.js";
            model.Title = "Gestión de Paradas";
            if (!string.IsNullOrEmpty(Title))
            {
                model.Title = Title;
            }
            ViewBag.ruta = new string[] { "Inicio", "Paradas", "Listado de Paradas" };
            return View(model);
        }


        #region Api

        public override async Task<ActionResult> GetAllApi()
        {
            var result = await _paradaService.Listar();
            
            return WrapperResponseGetApi(ModelState, () => result);

        }

        [HttpPost]
        public async Task<JsonResult> CreateApi(ParadaDto input)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var canCreate = _paradaService.canCreate(input);
                    if (canCreate == "OK")
                    {
                        input.Codigo = _paradaService.nextcode();
                        var resultEntity = await _paradaService.Create(input);
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
        public async Task<JsonResult> UpdateApi(ParadaDto input)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var canUpdate = _paradaService.canUpdate(input);
                    if (canUpdate == "OK")
                    {
                        var resultEntity = await _paradaService.Update(input);
                        var result = new { id = resultEntity.Id };
                        return new JsonResult
                        {
                            Data = new { success = true, updated = true, result }
                        };
                    }
                    
                    return new JsonResult
                    {
                        Data = new { success = true, updated=false, errors = canUpdate }
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
                    var canDelete = _paradaService.canDelete(id.Value);
                    if (canDelete == "OK")
                    {
                        await _paradaService.Delete(new EntityDto<int>(id.Value));
                        return new JsonResult
                        {
                            Data = new { success = true }
                        };
                    }
                
                    return new JsonResult
                    {
                        Data = new { success = false, errors = canDelete }
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