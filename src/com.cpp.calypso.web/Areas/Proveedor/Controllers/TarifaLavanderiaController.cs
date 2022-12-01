using Abp.Application.Services.Dto;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.framework;
using com.cpp.calypso.proyecto.aplicacion.Interfaces;
using com.cpp.calypso.proyecto.aplicacion.Proveedor.Dto;
using com.cpp.calypso.proyecto.aplicacion.Proveedor.Interfaces;
using com.cpp.calypso.proyecto.dominio.Proveedor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using JsonResult = com.cpp.calypso.framework.JsonResult;
namespace com.cpp.calypso.web.Areas.Proveedor.Controllers
{
    public class TarifaLavanderiaController : BaseSPAController<TarifaLavanderia, TarifaLavanderiaDto, PagedAndFilteredResultRequestDto>
    {
        private readonly ITarifaLavanderiaAsyncBaseCrudAppService _entityService;
        private readonly IContratoProveedorAsyncBaseCrudAppService _contratoProveedorService;
        private readonly IProveedorAsyncBaseCrudAppService _proveedorService;
        private readonly ICatalogoAsyncBaseCrudAppService _catalogoService;
        public TarifaLavanderiaController(
           IHandlerExcepciones manejadorExcepciones,
           IViewService viewService,
           ITarifaLavanderiaAsyncBaseCrudAppService entityService,
           IContratoProveedorAsyncBaseCrudAppService contratoProveedorService,
            ICatalogoAsyncBaseCrudAppService catalogoService,
           IProveedorAsyncBaseCrudAppService proveedorService
           ) : base(manejadorExcepciones, viewService, entityService)
        {
            _entityService = entityService;
            _contratoProveedorService = contratoProveedorService;
            _proveedorService = proveedorService;
            _catalogoService = catalogoService;
        }

        // GET: Proveedor/TarifaLavanderia
        public ActionResult Index()
        {
            return View();
        }

        // GET: Proveedor/TarifaLavanderia/Details/5
        public ActionResult Details(int id)
        {

            var model = new FormReactModelView();
            model.Id = "tarifas_lavanderia";
            model.ReactComponent = "~/Scripts/build/tarifas_lavanderia.js";
            model.Title = "Tarifas Lavanderia";

            if (!string.IsNullOrEmpty(Title))
            {
                model.Title = Title;
            }
            ViewBag.ruta = new string[] { "Inicio", "Proveedores", "Gestión", "Tarifas Lavanderia" };
            return View(model);
        }


        [HttpPost]
        public async Task<JsonResult> Create(TarifaLavanderia entity)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var tarifaUnica =
                        _entityService.TarifaUnica(entity.ContratoProveedorId, entity.TipoServicioId);
                    if (!tarifaUnica)
                    {
                        return new JsonResult
                        {
                            Data = new { success = false, errors = "Tarifa ya registrada" }
                        };
                    }
                    var resultEntity = _entityService.CrearTarifa(entity);

                    var result = new { id = resultEntity };
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
        public async Task<JsonResult> Editar(TarifaLavanderia entity)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var resultEntity = _entityService.EditarTarifa(entity);

                    var result = new { id = resultEntity };
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
        public override async Task<JsonResult> DeleteApi(int? id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var tarifaHotelService = EntityService as ITarifaLavanderiaAsyncBaseCrudAppService;
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
        public   JsonResult EliminarApi(int? id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var tarifaHotelService = EntityService as ITarifaLavanderiaAsyncBaseCrudAppService;
                    tarifaHotelService.EliminarTarifa(id.Value);

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
                    var tarifaHotelService = EntityService as ITarifaLavanderiaAsyncBaseCrudAppService;
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


        public ActionResult ListarPorContrato(int? id)
        {
            if (!id.HasValue)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var result = _entityService.ListarPorContrato(id.Value);
            return WrapperResponseGetApi(ModelState, () => result);
        }
        public async Task<ActionResult> GetContratoInfo(int? id)
        {
            if (!id.HasValue)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var info = await _contratoProveedorService.GetInfo(new EntityDto<int>(id.Value));
            return WrapperResponseGetApi(ModelState, () => info);
        }
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
