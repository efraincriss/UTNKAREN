using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
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
    public class HabitacionController : BaseSPAController<Habitacion, HabitacionDto, PagedAndFilteredResultRequestDto>
    {
        private readonly IHabitacionAsyncBaseCrudAppService _entityService;
        private readonly IEspacioHabitacionAsyncBaseCrudAppService _espacioHabitacionService;
        private readonly IReservaHotelAsyncBaseCrudAppService _reservaService;
        private readonly ICatalogoAsyncBaseCrudAppService _catalogoService;
        public HabitacionController(
            IHandlerExcepciones manejadorExcepciones, 
            IViewService viewService, 
            IHabitacionAsyncBaseCrudAppService entityService,
            IEspacioHabitacionAsyncBaseCrudAppService espacioHabitacionService,
            IReservaHotelAsyncBaseCrudAppService reservaService,
            ICatalogoAsyncBaseCrudAppService catalogoService
            ) : base(manejadorExcepciones, viewService, entityService)
        {
            _entityService = entityService;
            _espacioHabitacionService = espacioHabitacionService;
            _reservaService = reservaService;
            _catalogoService = catalogoService;
            Title = "Stock de Habitaciones";
            Key = "stock_habitaciones";
            ComponentJS = "~/Scripts/build/solicitudVianda.js";
        }

        // GET: Proveedor/Habitacion
        public ActionResult IndexProveedores()
        {

            var model = new FormReactModelView();
            model.Id = "proveedores_hospedaje_table";
            model.ReactComponent = "~/Scripts/build/proveedores__hospedaje_table.js";
            model.Title = "Gestión de Vehículos";

            if (!string.IsNullOrEmpty(Title))
            {
                model.Title = Title;
            }
            ViewBag.ruta = new string[] { "Inicio", "Gestión de Hospedaje", "Gestión de Habitaciones", "Listado Proveedores de Hospedaje" };
            return View(model);
        }


        // Proveedor Id
        public ActionResult Details(int id)
        {
            var model = new FormReactModelView();
            model.Id = "habitacion_detalle";
            model.ReactComponent = "~/Scripts/build/detalle_habitacion.js";

            if (!string.IsNullOrEmpty(Title))
            {
                model.Title = Title;
            }

            ViewBag.ruta = new string[] { "Inicio", "Gestión de Hospedaje", "Gestión de Habitaciones", "Listado Proveedores de Hospedaje", "Detalle Proveedor de Hospedaje" };
            return View(model);
        }



        #region Api

        [System.Web.Mvc.HttpPost]
        public async Task<JsonResult> CreateHabitacioYEspaciosnApi(HabitacionDto entity)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var existeNumeroHabitacion =
                        _entityService.ExisteNumeroHabitacion(entity.numero_habitacion, entity.ProveedorId);
                    if (existeNumeroHabitacion)
                    {
                        return new JsonResult
                        {
                            Data = new { success = false, errors = "El número de habitación debe ser único" }
                        };
                    }
                    var resultEntity = await EntityService.Create(entity);
                    _espacioHabitacionService.CrearEspacios(resultEntity.Id, resultEntity.capacidad);
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

        // Habitacion Id
        [System.Web.Mvc.HttpPost]
        public override async Task<JsonResult> DeleteApi(int? id)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    var habitacionService = EntityService as IHabitacionAsyncBaseCrudAppService;
                    //await habitacionService.Delete(new EntityDto<int>(id.Value));
                    var habitacion = await habitacionService.Get(new EntityDto<int>(id.Value));
                    habitacion.estado = false;
                    await habitacionService.Update(habitacion);
                    _espacioHabitacionService.EliminarEspaciosDeHabitacion(id.Value);

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


        // Id Proveedor
        public ActionResult GetHabitacionesPorProveedor(int? id)
        {
            if (!id.HasValue)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var habitacionService = EntityService as IHabitacionAsyncBaseCrudAppService;
            var habitaciones = habitacionService.GetHabitacionesPorProveedor(id.Value);
            return WrapperResponseGetApi(ModelState, () => habitaciones);
        }


        // Habitacion Id
        public async Task<ActionResult> DetailsApi(int id)
        {
            var habitacionService = EntityService as IHabitacionAsyncBaseCrudAppService;
            var pagedResultDto = await habitacionService.GetDetalle(id);
            return WrapperResponseGetApi(ModelState, () => pagedResultDto);
        }


        [System.Web.Mvc.HttpPost]
        public async Task<JsonResult> UpdateApi(HabitacionDto entity, [FromBody] int capacidad_anterior)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    _espacioHabitacionService.CrearNuevosEspacios(entity.Id, entity.capacidad,capacidad_anterior);

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


        // Proveedor Id
        public JsonResult HabitacionArbol(int id)
        {
            var habitacionService = EntityService as IHabitacionAsyncBaseCrudAppService;
            var nodes = habitacionService.GenerarArbolHabitaciones(id);
            return WrapperResponseGetApi(ModelState, () => nodes);
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult SwitchHabitacionEstado(int? habitacionId, bool estado)
        {
            if (!habitacionId.HasValue)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            _entityService.SwitchEstadoHabitacion(habitacionId.Value, estado);
            return WrapperResponseGetApi(ModelState, () => true);
        }


        public ActionResult ListarTarifasTipoHabitacion(int id)
        {
            var reservas = _reservaService.ListarTarifasTipoHabitacion(id);
            return WrapperResponseGetApi(ModelState, () => reservas);
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

        public ActionResult ActivarDesactivarEspacio(int? id)
        {
            if (!id.HasValue)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //var espacioHabitacionService = EntityService as IEspacioHabitacionAsyncBaseCrudAppService;
            var result = _espacioHabitacionService.ActivarDesactivarEspacio(id.Value);

            if (result)
            {
                return new JsonResult
                {
                    Data = new { success = true }
                };
            }
            return new JsonResult
            {
                Data = new { success = false, error = "No se puede habilitar un espacio de una habitación inactiva" }
            };

        }
    }
}