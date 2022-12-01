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
    public class VehiculoController : BaseTransporteSpaController<Vehiculo, VehiculoDto, PagedAndFilteredResultRequestDto>
    {
        private readonly IVehiculoAsyncBaseCrudAppService _vehiculoService;

        public VehiculoController(
            IHandlerExcepciones manejadorExcepciones, 
            IViewService viewService, 
            IAsyncBaseCrudAppService<Vehiculo, VehiculoDto, PagedAndFilteredResultRequestDto, VehiculoDto> entityService,
            IVehiculoAsyncBaseCrudAppService vehiculoService
            ) : base(manejadorExcepciones, viewService, entityService)
        {
            _vehiculoService = vehiculoService;
        }


        public ActionResult Index()
        {

            var model = new FormReactModelView();
            model.Id = "vehiculos";
            model.ReactComponent = "~/Scripts/build/gestion_vehiculos.js";
            model.Title = "Gestión de Vehículos";
            if (!string.IsNullOrEmpty(Title))
            {
                model.Title = Title;
            }
            ViewBag.ruta = new string[] { "Inicio", "Vehículos", "Listado de Vehículos" };
            return View(model);
        }


        #region Api

        public ActionResult GetAll()
        {
            var result = _vehiculoService.GetAllVehiculos();
            return WrapperResponseGetApi(ModelState, () => result);
        }
        public ActionResult GetTipoVehiculo(int id)
        {
            var result = _vehiculoService.GetTipoVehiculo(id);
            return Content(result);
        }
        [HttpPost]
        public async Task<JsonResult> CreateApi(VehiculoDto input)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var canCreate = _vehiculoService.CanCreate(input.Codigo, input.NumeroPlaca,input.AnioFabricacion, input.FechaVencimientoMatricula);
                    if (canCreate == "PUEDE_CREAR")
                    {
                        input.Codigo = _vehiculoService.nextcode();
                        var resultEntity = await _vehiculoService.Create(input);

                        var result = new { id = resultEntity.Id };
                        return new JsonResult
                        {
                            Data = new { success = true, result }
                        };
                    }

                    return new JsonResult
                    {
                        Data = new { success = false, errors = canCreate }
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
        public async Task<JsonResult> UpdateApi(VehiculoDto input)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var canUpdate = _vehiculoService.CanUpdate(input.Codigo, input.NumeroPlaca,input.AnioFabricacion, input.FechaVencimientoMatricula, input.ProveedorId);
                    if (canUpdate == "PUEDE_ACTUALIZAR")
                    {
                        var estadoCambiado = _vehiculoService.RegistrarHistorico(input.Id, input.Estado);
                        if (estadoCambiado)
                        {
                            input.FechaEstado = DateTime.Now;
                        }
                        var resultEntity = await _vehiculoService.Update(input);
                        var result = new { id = resultEntity.Id };
                        return new JsonResult
                        {
                            Data = new { success = true, result }
                        };
                    }

                    return new JsonResult
                    {
                        Data = new { success = false, errors= canUpdate }
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
                    string puedeeliminar = _vehiculoService.CanDelete(id.Value);
                    if (puedeeliminar== "PUEDE_ELIMINAR")
                    {

                        await _vehiculoService.Delete(new EntityDto<int>(id.Value));
                        return new JsonResult
                        {
                            Data = new { success = true }
                        };
                    }
                    else
                    {
                        return new JsonResult
                        {
                            Data = new { success = false, errors = puedeeliminar }
                        };
                    }
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