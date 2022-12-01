using Abp.Application.Services.Dto;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.framework;
using com.cpp.calypso.proyecto.aplicacion.Dto;
using com.cpp.calypso.proyecto.aplicacion.Interfaces;
using com.cpp.calypso.proyecto.aplicacion.Transporte.Interface;
using com.cpp.calypso.proyecto.dominio;
using CommonServiceLocator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using JsonResult = com.cpp.calypso.framework.JsonResult;

namespace com.cpp.calypso.web.Areas.Proveedor.Controllers
{
    public class DistribucionViandaController : BaseSPAController<DistribucionVianda,DistribucionViandaDto,PagedAndFilteredResultRequestDto>
    {
        private readonly ICatalogoAsyncBaseCrudAppService _catalogoService;
        private static readonly ILogger log =
ServiceLocator.Current.GetInstance<ILoggerFactory>().Create(typeof(DistribucionViandaController));

        public ISolicitudViandaAsyncBaseCrudAppService EntitySolicitudService { get; }
        public IZonaProveedorAsyncBaseCrudAppService EntityZonaService { get; }
        public ICatalogoAsyncBaseCrudAppService EntityCatalogService { get; }
        public IColaboradoresAsyncBaseCrudAppService ColaboradoresService { get; }
        public IChoferAsyncBaseCrudAppService ChoferService { get; }


        public DistribucionViandaController(
            IHandlerExcepciones manejadorExcepciones,
            IViewService viewService,
            IDistribucionViandaAsyncBaseCrudAppService entityService,
             ISolicitudViandaAsyncBaseCrudAppService entitySolicitudService,
            IZonaProveedorAsyncBaseCrudAppService entityZonaService,
            ICatalogoAsyncBaseCrudAppService entityCatalogService,
                 ICatalogoAsyncBaseCrudAppService catalogoService,
            IColaboradoresAsyncBaseCrudAppService colaboradoresService,
            IChoferAsyncBaseCrudAppService choferService
            ) : base(manejadorExcepciones, viewService, entityService)
        {
             
            EntitySolicitudService = entitySolicitudService;
            EntityZonaService = entityZonaService;
            EntityCatalogService = entityCatalogService;
            _catalogoService = catalogoService;
            ColaboradoresService = colaboradoresService;
            ChoferService = choferService;

            Title = "Distribución de Pedidos";
            Key = "distribucion_vianda";
            ComponentJS = "~/Scripts/build/distribucionVianda.js";
        }

          

#pragma warning disable CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        /// <summary>
        /// Editar
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<ActionResult> Edit(DateTime? fecha, int? tipoComidaId)
#pragma warning restore CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        {

            if (!fecha.HasValue || !tipoComidaId.HasValue)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //1. Model
            var model = new FormReactModelView();
            model.Id = "distribucion_vianda";

            model.Title = "ASIGNACIÓN DE VIANDAS A TRANSPORTE";
            model.ReactComponent = "~/Scripts/build/distribucionViandaEdit.js";

            if (!string.IsNullOrEmpty(Title))
            {
                model.Title = Title;
            }
 
            return View(model);
        }


#pragma warning disable CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        public virtual async Task<ActionResult> EditTransportista(DateTime? fecha, int? tipoComidaId)
#pragma warning restore CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        {

            if (!fecha.HasValue || !tipoComidaId.HasValue)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //1. Model
            var model = new FormReactModelView();
            model.Id = "distribucion_transporte";
            model.ReactComponent = "~/Scripts/build/distribucionTransporte.js";


            if (!string.IsNullOrEmpty(Title))
            {
                model.Title = Title;
            }

            return View(model);
        }

#pragma warning disable CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        public virtual async Task<ActionResult> Ver(DateTime? fecha, int? tipoComidaId)
#pragma warning restore CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        {

            if (!fecha.HasValue || !tipoComidaId.HasValue)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //1. Model
            var model = new FormReactModelView();
            model.Id = "distribucion_transporte";
            model.ReactComponent = "~/Scripts/build/distribucionViandaVer.js";


            if (!string.IsNullOrEmpty(Title))
            {
                model.Title = Title;
            }

            return View(model);
        }


        public async Task<ActionResult> EnviarCorreos(DateTime fecha)
        {
            try
            {
                var distribucionService = EntityService as IDistribucionViandaAsyncBaseCrudAppService;
                var enviar = await distribucionService.EnviarDistribucionaProveedores(fecha);

                return Content("OK");
            }
            catch (Exception ex)
            {

                return Content("Error: " + ex.Message);
            }

        }
        #region API


        public async Task<ActionResult> GetPagedApi(PagedAndSortedResultRequestDto pagedAndSorted, DateTime? fecha, int? tipoComidaId)
        {
            var distribucionService = EntityService as IDistribucionViandaAsyncBaseCrudAppService;
            var list = await distribucionService.GetSolicitudesAsignadasGrupo(pagedAndSorted,fecha, tipoComidaId);

            return WrapperResponseGetApi(ModelState, () => list);
        }


        [HttpPost]
        public async Task<JsonResult> CreateDistribucionApi(DateTime fecha, int tipoComidaId)
        {

            try
            {
                if (ModelState.IsValid)
                {
                    var distribucionService = EntityService as IDistribucionViandaAsyncBaseCrudAppService;
                    var result = await distribucionService.Exists(fecha, tipoComidaId);

                    if (result)
                    {
                        return new JsonResult
                        {
                            Data = new { success = false, error = "Ya existe una distribución para la fecha y Tipo de Comida ingresados" }
                        };
                    }
                    else
                    {
                        return new JsonResult
                        {
                            Data = new { success = true, result }
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


        public async Task<ActionResult> EditDistribucionApi(DateTime fecha, int tipoComidaId)
        {
            var distribucionService = EntityService as IDistribucionViandaAsyncBaseCrudAppService;
            var list = await distribucionService.GetSolicitudesAsignadas(fecha, tipoComidaId);

            var solicitudes = await EntitySolicitudService.GetSolicitudesPendientes(fecha, tipoComidaId);

            var proveedores = await EntityZonaService.GetInfoAll(tipoComidaId);

            var tipoComida = await EntityCatalogService.Get(new EntityDto<int>(tipoComidaId));

            var model = new
            {
                solicitudes,
                proveedores,
                tipoComida = new
                {
                    tipoComida.Id,
                    tipoComida.nombre
                },
                distribuciones = list
            };

            return WrapperResponseGetApi(ModelState, () => model);

        }


        public async Task<ActionResult> VerDistribucionApi(DateTime fecha, int tipoComidaId)
        {
            var distribucionService = EntityService as IDistribucionViandaAsyncBaseCrudAppService;

            //Cabecera
            var tipoComida = await EntityCatalogService.Get(new EntityDto<int>(tipoComidaId));

            //Lista de Distribucciones
            var list = await distribucionService.GetSolicitudesAsignadas(fecha, tipoComidaId);

            //Totales
            var totales = await distribucionService.GetTotales(fecha, tipoComidaId);
            totales.TotalDistribucciones =   list.Sum(d => d.total_pedido);
            totales.TotalPorConsumir = totales.TotalPedidos - totales.TotalConsumidos;

            var distribuidos = list.GroupBy(d => d.ProveedorId)
                .Select(g => new
                {
                    Id = g.Key,
                    total_pedido = g.Sum(s => s.pedido_viandas),
                    proveedor_nombre = g.FirstOrDefault().proveedor_nombre,
                    proveedor_zona = g.FirstOrDefault().proveedor_zona,
                    proveedor_identificacion = g.FirstOrDefault().proveedor_identificacion
                });

            var model = new
            {
                total_pedidos = totales.TotalPedidos,
                total_distribucciones = totales.TotalDistribucciones,
                total_consumidos = totales.TotalConsumidos,
                total_por_consumir = totales.TotalPorConsumir, //TODO: Calculado
                tipoComida = new
                {
                    tipoComida.Id,
                    tipoComida.nombre
                },
                distribuciones = list,
                distribuidos = distribuidos
            };

            return WrapperResponseGetApi(ModelState, () => model);

        }

        

        [HttpPost]
        public async Task<JsonResult> EditDistribucionApi(DateTime fecha, int tipoComidaId, List<DistribucionViandaProveedorDto> model, List<int> deleteIds)
        {

            try
            {
                if (ModelState.IsValid)
                {

                    var deleteIdsNotNull = new List<int>();
                    if (deleteIds != null && deleteIds.Count > 0)
                    {
                        deleteIdsNotNull = deleteIds;
                    }

                    var modelNotNull = new List<DistribucionViandaProveedorDto>();
                    if (model != null && model.Count > 0)
                    {
                        modelNotNull = model;
                    }

                    var distribucionService = EntityService as IDistribucionViandaAsyncBaseCrudAppService;
                    var result = await distribucionService.Distribute(fecha, tipoComidaId, modelNotNull, deleteIdsNotNull);

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
        public async Task<JsonResult> EditAprobarApi(DateTime fecha, int tipoComidaId)
        {

            try
            {
                if (ModelState.IsValid)
                {

                     
                    var distribucionService = EntityService as IDistribucionViandaAsyncBaseCrudAppService;
                    var result = await distribucionService.ApproveDistribute(fecha, tipoComidaId);

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

        /// <summary>
        /// Obtener la distribuccion de transporte
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="tipoComidaId"></param>
        /// <returns></returns>
        public async Task<JsonResult> GetTransporteApi(DateTime fecha, int tipoComidaId)
        {

            try
            {
                if (ModelState.IsValid)
                {
                    var distribucionService = EntityService as IDistribucionViandaAsyncBaseCrudAppService;
                    var result = await distribucionService.GetDistribucionTransporte(fecha, tipoComidaId);
                    var detalle = await distribucionService.GetSolicitudesAsignadas(fecha, tipoComidaId);

                    var tipoComida = await EntityCatalogService.Get(new EntityDto<int>(tipoComidaId));
                    var transportistas =  ColaboradoresService.GetTransportistasLookupAll();

                    var model = new
                    {
                        distribucionTrasporte = result,
                        distribucionSolicitudes = detalle,
                        transportistas = transportistas,
                        tipoComida = new
                        {
                            tipoComida.Id,
                            tipoComida.nombre
                        },
                    };

                    return new JsonResult
                    {
                        Data = new { success = true, result= model }
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
        public async Task<JsonResult> EditTransporteApi(DateTime fecha, int tipoComidaId, List<DistribucionTransporteUpdateDto> model)
        {

            try
            {
                if (ModelState.IsValid)
                {

                     

                    var modelNotNull = new List<DistribucionTransporteUpdateDto>();
                    if (model != null && model.Count > 0)
                    {
                        modelNotNull = model;
                    }

                    var distribucionService = EntityService as IDistribucionViandaAsyncBaseCrudAppService;
                    var result = await distribucionService.DistributeTransport(fecha, tipoComidaId, modelNotNull);

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
        public async Task<JsonResult> EditApproveTransporteApi(DateTime fecha, int tipoComidaId)
        {

            try
            {
                if (ModelState.IsValid)
                {

                    var distribucionService = EntityService as IDistribucionViandaAsyncBaseCrudAppService;
                    var result = await distribucionService.ApproveDistributeTransport(fecha, tipoComidaId);

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
        public ActionResult SearchByCodeApiComida(string code)
        {
            var result = _catalogoService.APIObtenerCatalogos(code).Where(c=>c.valor_texto== "VIANDA").ToList();
            return new JsonResult
            {
                Data = new { success = true, result },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };

        }

    }
}