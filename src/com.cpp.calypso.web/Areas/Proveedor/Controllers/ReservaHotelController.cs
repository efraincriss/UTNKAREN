using System;
using System.Collections.Generic;
using System.Linq;
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
using Action = Antlr.Runtime.Misc.Action;
using JsonResult = System.Web.Mvc.JsonResult;

namespace com.cpp.calypso.web.Areas.Proveedor.Controllers
{
    public class ReservaHotelController : BaseSPAController<ReservaHotel, ReservaHotelDto, PagedAndFilteredResultRequestDto>
    {
        private readonly IReservaHotelAsyncBaseCrudAppService _service;
        private readonly ICatalogoAsyncBaseCrudAppService _catalogoService;

        public ReservaHotelController(
            IHandlerExcepciones manejadorExcepciones, 
            IViewService viewService, 
            IReservaHotelAsyncBaseCrudAppService entityService,
            IReservaHotelAsyncBaseCrudAppService service,
            ICatalogoAsyncBaseCrudAppService catalogoService
            ) : base(manejadorExcepciones, viewService, entityService)
        {
            _service = service;
            _catalogoService = catalogoService;
        }

        
        public ActionResult CrearReservas()
        {
            var model = new FormReactModelView();
            model.Id = "reservas_hotel";
            model.ReactComponent = "~/Scripts/build/crear_reservas.js";
            model.Title = "Crear Reservas";
            if (!string.IsNullOrEmpty(Title))
            {
                model.Title = Title;
            }
            ViewBag.ruta = new string[] { "Inicio", "Gestión de Hospedaje", "Crear Reservas"};
            return View(model);
        }
        public ActionResult CrearReservasExtemporaneas()
        {
            var model = new FormReactModelView();
            model.Id = "reservas_hotel";
            model.ReactComponent = "~/Scripts/build/crear_reservas_extemporaneas.js";
            model.Title = "Crear Reservas Extemporáneas";
            if (!string.IsNullOrEmpty(Title))
            {
                model.Title = Title;
            }
            ViewBag.ruta = new string[] { "Inicio", "Gestión de Hospedaje", "Crear Reservas Extemporáneas" };
            return View(model);
        }


        public ActionResult GestionarReservas()
        {
            var model = new FormReactModelView();
            model.Id = "gestionar_reservas";
            model.ReactComponent = "~/Scripts/build/gestion_reservas.js";
            model.Title = "Gestionar Reservas";
            if (!string.IsNullOrEmpty(Title))
            {
                model.Title = Title;
            }
            ViewBag.ruta = new string[] { "Inicio", "Gestión de Hospedaje", "Gestión de Reservas"};
            return View(model);
        }

        public ActionResult GestionarReservasExtemporaneas()
        {
            var model = new FormReactModelView();
            model.Id = "gestionar_reservas";
            model.ReactComponent = "~/Scripts/build/gestion_reservas_extemporaneas.js";
            model.Title = "Gestionar Reservas";
            if (!string.IsNullOrEmpty(Title))
            {
                model.Title = Title;
            }
            ViewBag.ruta = new string[] { "Inicio", "Gestión de Hospedaje", "Gestión de Reservas" };
            return View(model);
        }

        public ActionResult PanelControl()
        {
            var model = new FormReactModelView();
            model.Id = "panel_control";
            model.ReactComponent = "~/Scripts/build/panel_control_reservas.js";
            model.Title = "Panel de Control de Reservas";
            if (!string.IsNullOrEmpty(Title))
            {
                model.Title = Title;
            }
            ViewBag.ruta = new string[] { "Inicio", "Gestión de Hospedaje", "Panel de Control"};
            return View(model);
        }


        #region Api

        [HttpPost]
        public ActionResult EspciosDisponibles(DateTime fechaInicio, DateTime fechaFin)
        {
            var reservaHotelService = EntityService as IReservaHotelAsyncBaseCrudAppService;
            var espacios = reservaHotelService.BuscarEspaciosLibres(fechaInicio, fechaFin);
            return WrapperResponseGetApi(ModelState, () => espacios);
        }

        [HttpPost]
        public async Task<JsonResult> CrearReservaApi(ReservaHotelDto dto)
        {
            try
            {
               
                if (ModelState.IsValid)
                {
                    
                    var reservaValida = _service.ReservaValidada(dto.ColaboradorId, dto.fecha_desde, dto.fecha_hasta, dto.EspacioHabitacionId);
                    if (reservaValida != "OK")
                    {
                        return new JsonResult
                        {
                            Data = new { success = true, created = false, errors = reservaValida }
                        };
                    }

                    var entityDto = await EntityService.Create(dto);
                    var reservaHotelService = EntityService as IReservaHotelAsyncBaseCrudAppService;
                    reservaHotelService.CrearDetallesReserva(entityDto.Id, entityDto.fecha_desde, entityDto.fecha_hasta);
                    bool msg = await reservaHotelService.SendMessageAsync(entityDto.Id," ASIGNACIÓN ");
                    var result = new { id = entityDto.Id };
                    return new JsonResult
                    {
                        Data = new { success = true, created = true,  result }
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
        public ActionResult CrearReservaExtemporaneaApi(ReservaHotelDto dto)
        {

            if (ModelState.IsValid)
            {
                if (dto.extemporaneo)
                {
                    var reservaValidaExtemporaneo = _service.ReservaExtemporaneaValidada(dto.ColaboradorId, dto.fecha_desde, dto.fecha_hasta, dto.EspacioHabitacionId);
                    if (reservaValidaExtemporaneo != "OK")
                    {
                        return new JsonResult
                        {
                            Data = new { success = true, created = false, errors = reservaValidaExtemporaneo }
                        };
                    }

                }
                HttpPostedFileBase file = Request.Files["uploadFile"];

                if (file != null)
                {

                    var ReservaId = _service.CrearReservaExtemporanea(dto, file);
                    if (ReservaId > 0) {
                    _service.CrearDetallesReserva(ReservaId, dto.fecha_desde, dto.fecha_hasta);
                    }
                    var result = new { id = ReservaId };
                    return new JsonResult
                    {
                        Data = new { success = true, created = true, result }
                    };
                }
            }
            else {
                return Content("VALIDACIONES");
            }

            return Content("");

        }


        public ActionResult BuscarColaborador(string identificacion = "", string nombres = "")
        {
            var reservaHotelService = EntityService as IReservaHotelAsyncBaseCrudAppService;
            var colaboradores = reservaHotelService.BuscarPorIdentificacionNombre(identificacion, nombres);
            return WrapperResponseGetApi(ModelState, () => colaboradores);
        }

        public ActionResult BuscarColaboradorHospedaje(string identificacion = "", string nombres = "")
        {
            var reservaHotelService = EntityService as IReservaHotelAsyncBaseCrudAppService;
            var colaboradores = reservaHotelService.BuscarColaboradoresHospsdaje(identificacion, nombres);
            return WrapperResponseGetApi(ModelState, () => colaboradores);
        }

        public ActionResult ListarReservas(DateTime fechaInicio, DateTime fechaFin)
        {
            var reservaHotelService = EntityService as IReservaHotelAsyncBaseCrudAppService;
            var reservas = reservaHotelService.ListarReservas(fechaInicio, fechaFin).Where(c => !c.extemporaneo).ToList();
            return WrapperResponseGetApi(ModelState, () => reservas);
        }
        public ActionResult ListarReservasExtemporaneas(DateTime fechaInicio, DateTime fechaFin)
        {
            var reservaHotelService = EntityService as IReservaHotelAsyncBaseCrudAppService;
            var reservas = reservaHotelService.ListarReservas(fechaInicio, fechaFin).ToList();
            return WrapperResponseGetApi(ModelState, () => reservas);
        }

        public ActionResult UpdateServicioColaborador(int id)
        {
            var servicio = _service.UpdateServicioHospedajeColaborador(id);
            return Content(servicio);
        }

        public ActionResult ObtenerDiasJornada()
        {
            var servicio = _service.DiasJornadaCampo();
            return Content(servicio);
        }
        [HttpPost]
        public JsonResult EnableDisableApi(string pass)
        {
            try
            {
                if (ModelState.IsValid)
                {


                    bool result = _service.Activar(pass);
                    string mensaje = result ? "OK" : "ERROR";

                    return new JsonResult
                    {
                        Data = new { success = true, result= mensaje }
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
        public async Task<JsonResult> EliminarReserva(int id)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    var reservaService = EntityService as IReservaHotelAsyncBaseCrudAppService;

                    //bool msg = await reservaService.SendMessageAsync(id, " ELIMINACIÓN ");
                    var eliminado = reservaService.EliminarReserva(id);
                    if (eliminado)
                    {
                     
                        return new JsonResult
                        {
                            Data = new { success = true }
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

        [HttpPost]
        public JsonResult EditarReserva(int id, DateTime fecha)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var reservaService = EntityService as IReservaHotelAsyncBaseCrudAppService;
                    var eliminado = reservaService.EditarReserva(id, fecha);

                    if (eliminado)
                    {
                        return new JsonResult
                        {
                            Data = new { success = true }
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


        public ActionResult ListarTarifasTipoHabitacion(int id)
        {
            var reservaHotelService = EntityService as IReservaHotelAsyncBaseCrudAppService;
            var reservas = reservaHotelService.ListarTarifasTipoHabitacion(id);
            return WrapperResponseGetApi(ModelState, () => reservas);
        }

        #endregion

        #region MyRegion

        public JsonResult ListarHoteles(DateTime fecha)
        {
            var reservaHotelService = EntityService as IReservaHotelAsyncBaseCrudAppService;
            var hoteles = reservaHotelService.ListarHoteles(fecha);
            return WrapperResponseGetApi(ModelState, () => hoteles);
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



        [HttpPost]
        public ActionResult IniciarFinalizarConsumo(int Id, bool inicio, DateTime fecha, string justificacion)
        {
            var result = _service.Iniciar_FinalizarConsumo(Id, inicio, fecha, justificacion);
            return Content(result);
 
        }
        [HttpPost]
        public ActionResult EditarFecha(int Id, bool inicio, DateTime fecha, string justificacion)
        {
            var result = _service.EditarFecha(Id, inicio, fecha, justificacion);
            return Content(result);

        }



    }
}