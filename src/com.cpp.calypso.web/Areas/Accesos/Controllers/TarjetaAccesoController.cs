using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.UI;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Abp.Application.Services.Dto;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.framework;
using com.cpp.calypso.proyecto.aplicacion.Acceso.Dto;
using com.cpp.calypso.proyecto.aplicacion.Acceso.Interface;
using com.cpp.calypso.proyecto.aplicacion.Interfaces;
using com.cpp.calypso.proyecto.dominio.Acceso;
using JsonResult = com.cpp.calypso.framework.JsonResult;
using com.cpp.calypso.proyecto.aplicacion.Proveedor.Interfaces;
using com.cpp.calypso.proyecto.aplicacion.WebService;

namespace com.cpp.calypso.web.Areas.Accesos.Controllers
{
    public class TarjetaAccesoController : BaseAccesoSpaController<TarjetaAcceso, TarjetaAccesoDto, PagedAndFilteredResultRequestDto>
    {
        private readonly ITarjetaAccesoAsyncBaseCrudAppService _service;
        public   IArchivoAsyncBaseCrudAppService _archivoService;
        //ACCIONES AGREGADAS
        private readonly IColaboradoresAsyncBaseCrudAppService _colaboradoresService;
        private readonly IReservaHotelAsyncBaseCrudAppService _reservaservice;
        //Web service
        private readonly IWebServiceAsyncBaseCrudAppService _webService;
        private readonly IContactoEmergenciaAsyncBaseCrudAppService _contactoEmergenciaService;

        public TarjetaAccesoController(
            ITarjetaAccesoAsyncBaseCrudAppService service,
            IHandlerExcepciones manejadorExcepciones, 
            IViewService viewService,
            IArchivoAsyncBaseCrudAppService archivoService,
               IColaboradoresAsyncBaseCrudAppService colaboradoresService,
            IReservaHotelAsyncBaseCrudAppService reservaservice,
                            IContactoEmergenciaAsyncBaseCrudAppService contactoEmergenciaService,
                   IWebServiceAsyncBaseCrudAppService webService
            ) : base(manejadorExcepciones, viewService)
        {
            _service = service;
            _archivoService = archivoService;
            _colaboradoresService = colaboradoresService;
            _reservaservice = reservaservice;
            _webService = webService;
            _contactoEmergenciaService = contactoEmergenciaService;
            Title = "Tarjetas de Acceso";
            Key = "tarjetas_acceso";
            ComponentJS = "~/Scripts/build/buscar_colaborador.js";
        }


        public ActionResult BuscarColaborador(string source)
        {
            var model = new FormReactModelView();
            model.Id = "buscar_colaborador_container";
            model.ReactComponent = "~/Scripts/build/buscar_colaborador_tarjeta.js";

            if (!string.IsNullOrEmpty(Title))
            {
                model.Title = Title;
            }
            ViewBag.ruta = new string[] { "Inicio", "Seguridad Patrimonial", "Buscar Colaborador" };
            return View(model);
        }

        // Colaborador Id
        public ActionResult Index(int colaboradorId)
        {
            var model = new FormReactModelView();
            model.Id = "gestion_tarjetas_requisitos";
            model.ReactComponent = "~/Scripts/build/gestion_tarjetas_requisitos.js";

            if (!string.IsNullOrEmpty(Title))
            {
                model.Title = Title;
            }
          

           ViewBag.ruta = new string[] { "Inicio", "Seguridad Patrimonial", "Tarjetas y Requisitos" };
           
            return View(model);
        }



        #region Api

        public ActionResult GetByColaborador(int? id)
        {
            if (!id.HasValue)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var tarjetas = _service.GetByColaborador(id.Value);
            return WrapperResponseGetApi(ModelState, () => tarjetas);
        }
       
       
        [HttpPost]
        public async Task<JsonResult> CreateApi(TarjetaAccesoDto input)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var puedeCrear = _service.PuedeCrear(input.ColaboradorId);
                    if (puedeCrear)
                    {
                        input.secuencial = _service.obtenersecuencialtarjetas(input.ColaboradorId);
                        var dto = await _service.Create(input);
                        var result = new { id = dto.Id };
                        return new JsonResult
                        {
                            Data = new { success = true, result }
                        };
                    }
                    else
                    {
                        var result = new { message = "Ya existe una tarjeta activa" };
                        return new JsonResult
                        {
                            Data = new { success = false, validation = false, result }
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
        public async Task<JsonResult> UpdateApi(ActualizarTarjetaDto input)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    
                    var archivo = Request.GenerateFileFromRequest("uploadFile");
                    if (archivo != null)
                    {
                        var archivoDto = await _archivoService.Create(archivo);
                        _service.AnularTarjeta(input, archivoDto.Id);
                    }
                    else
                    {
                        _service.AnularTarjeta(input);
                    }
                    
                    return new JsonResult
                    {
                        Data = new { success = true}
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

        //TarjetaId
        [HttpPost]
        public ActionResult SwitchEntregada(int? id)
        {
            if (!id.HasValue)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            _service.SwitchEntregado(id.Value);
            return new JsonResult
            {
                Data = new { success = true }
            };
            
        }

        [HttpPost]
        public ActionResult SubirArchivo(int id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var archivo = Request.GenerateFileFromRequest("uploadFile");
                    _service.SubirPdf(id, archivo);
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
        #endregion

        public ActionResult Search(string identificacion = "", string nombres = "")
        {
            var colaboradores = _reservaservice.BuscarPorIdentificacionNombre(identificacion, nombres);
            return WrapperResponseGetApi(ModelState, () => colaboradores);
        }

        public ActionResult Detalles(int id) //Detalles Colaborador
        {
            var pagedResultDto = _colaboradoresService.Detalles(id);
            return WrapperResponseGetApi(ModelState, () => pagedResultDto);
        }
        public ActionResult DetallesContactosE(int? id)
        {
            if (!id.HasValue)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var dtos = _contactoEmergenciaService.GetByColaboradorId(id.Value);
            return WrapperResponseGetApi(ModelState, () => dtos);
        }

        public async Task<ActionResult> Descargar(int id)
        {
            var entity = await _archivoService.Get(new EntityDto<int>(id));

            if (entity == null)
            {
                var msg = string.Format("El Archivo con identificacion {0} no existe",
                    id);

                return HttpNotFound(msg);
            }

            return File(entity.hash, entity.tipo_contenido, entity.nombre);
        }

    }
}