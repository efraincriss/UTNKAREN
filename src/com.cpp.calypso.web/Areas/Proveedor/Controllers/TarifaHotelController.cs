using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Abp.Application.Services.Dto;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.framework;
using com.cpp.calypso.proyecto.aplicacion.Interfaces;
using com.cpp.calypso.proyecto.aplicacion.Proveedor.Dto;
using com.cpp.calypso.proyecto.aplicacion.Proveedor.Interfaces;
using com.cpp.calypso.proyecto.dominio.Proveedor;
using JsonResult = com.cpp.calypso.framework.JsonResult;

namespace com.cpp.calypso.web.Areas.Proveedor.Controllers
{
    public class TarifaHotelController : BaseSPAController<TarifaHotel, TarifaHotelDto, PagedAndFilteredResultRequestDto>
    {
        private readonly ITarifaHotelAsyncBaseCrudAppService _entityService;
        private readonly IContratoProveedorAsyncBaseCrudAppService _contratoProveedorService;
        private readonly IProveedorAsyncBaseCrudAppService _proveedorService;
        private readonly ICatalogoAsyncBaseCrudAppService _catalogoService;

        public TarifaHotelController(
            IHandlerExcepciones manejadorExcepciones, 
            IViewService viewService, 
            ITarifaHotelAsyncBaseCrudAppService entityService,
            IContratoProveedorAsyncBaseCrudAppService contratoProveedorService,
             ICatalogoAsyncBaseCrudAppService catalogoService,
            IProveedorAsyncBaseCrudAppService proveedorService
            ) : base(manejadorExcepciones, viewService, entityService)
        {
            _entityService = entityService;
            _contratoProveedorService = contratoProveedorService;
            _proveedorService = proveedorService;
            _catalogoService=catalogoService;
        }


        // GET: ContratoProveedor Id
        public ActionResult Details(int id)
        {

            var model = new FormReactModelView();
            model.Id = "tarifas_hoteles";
            model.ReactComponent = "~/Scripts/build/tarifas_hoteles.js";
            model.Title = "Tarifas Hoteleras";

            if (!string.IsNullOrEmpty(Title))
            {
                model.Title = Title;
            }
            ViewBag.ruta = new string[] { "Inicio", "Proveedores", "Gestión", "Tarifas Hospedaje" };
            return View(model);
        }


        #region Api

        // Contrato Proveedor Id
        public ActionResult ListarPorContrato(int? id)
        {
            if (!id.HasValue)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var tarifaHotelService = EntityService as ITarifaHotelAsyncBaseCrudAppService;
            var habitaciones = tarifaHotelService.ListarPorContrato(id.Value);
            return WrapperResponseGetApi(ModelState, () => habitaciones);
        }

        [HttpPost]
        public override async Task<JsonResult> DeleteApi(int? id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var tarifaHotelService = EntityService as ITarifaHotelAsyncBaseCrudAppService;
                    tarifaHotelService.DesactivarTarifa(id.Value);

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
        public JsonResult ActivarTarifaApi(int? id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var tarifaHotelService = EntityService as ITarifaHotelAsyncBaseCrudAppService;
                    tarifaHotelService.ActivarTarifa(id.Value);

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
        public async Task<JsonResult> UpdateApi(TarifaHotelDto entity)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    var resultEntity = await EntityService.Update(entity);
                    var result = new { id = resultEntity.Id };
                    return new JsonResult
                    {
                        Data = new { success = true, result }
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
        public async Task<JsonResult> CreateTarifasApi(TarifaHotelDto entity)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var tarifaUnica =
                        _entityService.TarifaUnica(entity.ContratoProveedorId, entity.TipoHabitacionId);
                    if (!tarifaUnica)
                    {
                        return new JsonResult
                        {
                            Data = new { success = false, errors = "Tarifa ya registrada" }
                        };
                    }
                    var resultEntity = await EntityService.Create(entity);
                    
                    var result = new { id = resultEntity.Id };
                    return new JsonResult
                    {
                        Data = new { success = true, result }
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


        // Contrato Proveedor Id
        public async Task<ActionResult> GetContratoInfo(int? id)
        {
            if (!id.HasValue)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var info = await _contratoProveedorService.GetInfo(new EntityDto<int>(id.Value));
            return WrapperResponseGetApi(ModelState, () => info);
        }


        // Tarifa Id
        public async Task<ActionResult> DetailsApi(int id)
        {
            var tarifaHotelService = EntityService as ITarifaHotelAsyncBaseCrudAppService;
            var pagedResultDto = await tarifaHotelService.Get(new EntityDto<int>(id));
            return WrapperResponseGetApi(ModelState, () => pagedResultDto);
        }

        #endregion


        public ActionResult SearchByCodeApi(string code)
        {
            var result = _catalogoService.APIObtenerCatalogos(code);
            return new JsonResult
            {
                Data = new { success = true, result },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };

        }
    }
}