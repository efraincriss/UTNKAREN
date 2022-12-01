using Abp.Application.Services.Dto;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.framework;
using com.cpp.calypso.proyecto.aplicacion;
using com.cpp.calypso.proyecto.aplicacion.Dto;
using com.cpp.calypso.proyecto.aplicacion.Interfaces;
using com.cpp.calypso.proyecto.dominio;
using CommonServiceLocator;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using JsonResult = com.cpp.calypso.framework.JsonResult;

namespace com.cpp.calypso.web.Areas.Proveedor.Controllers
{
    public class ProveedorController : BaseSPAController<proyecto.dominio.Proveedor.Proveedor, ProveedorDto, PagedAndFilteredResultRequestDto>
    {

        public IArchivoAsyncBaseCrudAppService _archivoService;
        private readonly IZonaAsyncBaseCrudAppService _zonaService;
        private static readonly ILogger log =
ServiceLocator.Current.GetInstance<ILoggerFactory>().Create(typeof(ProveedorController));


        protected IServicioProveedorAsyncBaseCrudAppService ServicioProveedorService;
        protected INovedadProveedorAsyncBaseCrudAppService NovedadProveedorService;

        protected IServicioAsyncBaseCrudAppService ServicioService;
        protected IZonaProveedorAsyncBaseCrudAppService ZonaProveedorService;


        protected ICiudadAsyncBaseCrudAppService CiudadService;
        public IContactoAsyncBaseCrudAppService ContactoService { get; }
        private readonly IProvinciaAsyncBaseCrudAppService ProvinciaService;
        private readonly IParroquiaAsyncBaseCrudAppService ParroquiaService;


        public IContratoProveedorAsyncBaseCrudAppService ContratoProveedorService { get; }
        public IRequisitoProveedorAsyncBaseCrudAppService RequisitoProveedorService { get; }

        private readonly IProveedorAsyncBaseCrudAppService _ProveedorService;
        private readonly ICatalogoAsyncBaseCrudAppService _catalogoService;

        private readonly IPaisAsyncBaseCrudAppService _paisservice;
        private readonly IEmpresaAsyncBaseCrudAppService _empresaService;
        public ProveedorController
            (IHandlerExcepciones manejadorExcepciones,
            IViewService viewService,
            IProveedorAsyncBaseCrudAppService entityService,
            IProveedorAsyncBaseCrudAppService ProveedorService,
        INovedadProveedorAsyncBaseCrudAppService novedadProveedorService,
            IServicioProveedorAsyncBaseCrudAppService servicioProveedorService,
            IServicioAsyncBaseCrudAppService servicioService,
            IZonaProveedorAsyncBaseCrudAppService zonaProveedorService,
            IContactoAsyncBaseCrudAppService contactoService,
            ICiudadAsyncBaseCrudAppService ciudadService,
            IProvinciaAsyncBaseCrudAppService provinciaService,
             IZonaAsyncBaseCrudAppService zonaService,
            IParroquiaAsyncBaseCrudAppService parroquiaService,
            IContratoProveedorAsyncBaseCrudAppService contratoProveedorService,
            ICatalogoAsyncBaseCrudAppService catalogoService,
            IPaisAsyncBaseCrudAppService paisservice,
                    IArchivoAsyncBaseCrudAppService archivoService,
                        IEmpresaAsyncBaseCrudAppService empresaService,
        IRequisitoProveedorAsyncBaseCrudAppService requisitoProveedorService) :
            base(manejadorExcepciones, viewService, entityService)
        {

            NovedadProveedorService = novedadProveedorService;
            ServicioProveedorService = servicioProveedorService;
            ServicioService = servicioService;
            _catalogoService = catalogoService;
            ZonaProveedorService = zonaProveedorService;
            _paisservice = paisservice;
            CiudadService = ciudadService;
            ContactoService = contactoService;
            ParroquiaService = parroquiaService;
            ProvinciaService = provinciaService;
            _empresaService = empresaService;
            ContratoProveedorService = contratoProveedorService;
            RequisitoProveedorService = requisitoProveedorService;
            Title = "Proveedores";
            Key = "proveedores";
            ComponentJS = "~/Scripts/build/proveedores.js";
            _ProveedorService = ProveedorService;
            _archivoService = archivoService;
            _zonaService = zonaService;
        }


        public ActionResult Details(int? id)
        {


            if (!id.HasValue)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            log.DebugFormat("Details({0})", id.Value);


            //1. Model
            var model = new FormReactModelView();
            model.Id = Key;

            if (!string.IsNullOrEmpty(Title))
            {
                model.Title = Title;
            }


            return View(model);
        }

        public ActionResult EditContrato(int? proveedorId, int? contratoId, string entityAction)
        {

            if (!proveedorId.HasValue || !contratoId.HasValue || string.IsNullOrWhiteSpace(entityAction)
                || proveedorId.Value <= 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }


            //1. Model
            var model = new FormReactModelView();
            model.Id = "proveedores_contratos";

            model.Title = "Contratos Proveedores";

            return View(model);
        }



        #region API

        #region Proveedor

        public override async Task<ActionResult> GetApi(int id)
        {
            //TODO: Wrapper todo el bloque, para controlar excepciones
            var entityDto = await EntityService.Get(new EntityDto<int>(id));

            var list = ContactoService.GetContacto(entityDto.contacto_id);
            if (list.ParroquiaId > 0)
            {
                var p = ParroquiaService.GetParroquia(list.ParroquiaId.Value);
                entityDto.CiudadId = p.CiudadId;
                entityDto.ProvinciaId = p.Ciudad.ProvinciaId;
                var prov = ProvinciaService.GetProvincia(entityDto.ProvinciaId.Value);
                entityDto.PaisId = prov.PaisId;
            }

            return WrapperResponseGetApi(ModelState, () => entityDto);

        }


        [HttpPost]
        public override async Task<JsonResult> CreateApi(ProveedorDto entity, FormCollection formCollection)
        {

            if (entity.correo_electronico != null) {
                bool unique = _ProveedorService.ValidarEmailUnico(entity.correo_electronico);

                if (unique) {
                    var nombre_proveedor = _ProveedorService.ProvedorEmailUnico(entity.correo_electronico);
                    return new JsonResult
                {
                    Data = new { success = false, errors= nombre_proveedor }
                };
                }
            }

            try
            {
                if (ModelState.IsValid)
                {

                    var archivo = Request.GenerateFileFromRequest("uploadFile");
                    entity.documentacion_subida = archivo;

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

        [HttpPost]
        public override async Task<JsonResult> EditApi(ProveedorDto entity, FormCollection formCollection)
        {

            if (entity.correo_electronico != null)
            {
                bool unique = _ProveedorService.ValidarEmailUnicoEdit(entity.correo_electronico,entity.Id);

                if (unique)
                {
                    var nombre_proveedor = _ProveedorService.ProvedorEmailUnico(entity.correo_electronico);
                    return new JsonResult
                    {
                        Data = new { success = false, errors = nombre_proveedor }
                    };
                }
            }

            try
            {
                if (ModelState.IsValid)
                {

                    var archivo = Request.GenerateFileFromRequest("uploadFile");
                    entity.documentacion_subida = archivo;

                    //var resultEntity = await EntityService.Update(entity);

                    var Id = _ProveedorService.EditarProveedor(entity);

                    var result = new { id = Id };

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

        public async Task<ActionResult> GetProveedorDetalleApi(int id)
        {

            var proveedorService = EntityService as IProveedorAsyncBaseCrudAppService;

            var pagedResultDto = await proveedorService.GetDetalle(id);

            return WrapperResponseGetApi(ModelState, () => pagedResultDto);

        }


        [HttpPost]
        public async Task<JsonResult> EnableDisableApi(int id, bool opcion,string pass)
        {

            try
            {
                if (ModelState.IsValid)
                {
                    var proveedorService = EntityService as IProveedorAsyncBaseCrudAppService;


                    var result = await proveedorService.Activar(id, opcion,pass);


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



#pragma warning disable CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        public async Task<ActionResult> GetTipoIdentificadorApi()
#pragma warning restore CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        {
            //TODO: Mejorar
            var enumVals = new List<object>();

            foreach (var item in Enum.GetValues(typeof(proyecto.dominio.Proveedor.ProveedorTipoIdentificacion)))
            {

                enumVals.Add(new
                {
                    Id = (int)item,
                    nombre = item.ToString()
                });
            }

            return WrapperResponseGetApi(ModelState, () => enumVals);

        }


        #endregion Proveedor

        #region Proveedor / Servicio

        public async Task<ActionResult> GetProveedorServicioApi(int id)
        {

            var pagedResultDto = await ServicioProveedorService.Get(new EntityDto<int>(id));

            return WrapperResponseGetApi(ModelState, () => pagedResultDto);

        }

        [HttpPost]
        public async Task<ActionResult> CreateProveedorServiceApi(ServicioProveedorDto entity)
        {



            try
            {
                if (ModelState.IsValid)
                {

                    var resultEntity = await ServicioProveedorService.Create(entity);



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
        public async Task<ActionResult> EditProveedorServiceApi(ServicioProveedorDto entity)
        {



            try
            {
                if (ModelState.IsValid)
                {

                    var resultEntity = await ServicioProveedorService.Update(entity);



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
        public async Task<JsonResult> DeleteProveedorServiceApi(int? id)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    await ServicioProveedorService.Delete(new EntityDto<int>(id.Value));

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


        #endregion Proveedor / Servicio

        #region Proveedor / Novedades
        public async Task<ActionResult> GetProveedorNovedadApi(int id)
        {

            var entityDto = await NovedadProveedorService.Get(new EntityDto<int>(id));

            return WrapperResponseGetApi(ModelState, () => entityDto);

        }

        [HttpPost]
        public async Task<ActionResult> CreateProveedorNovedadApi(NovedadProveedorDto entity)
        {


            try
            {
                if (ModelState.IsValid)
                {

                    var archivo = Request.GenerateFileFromRequest("uploadFile");
                    entity.documentacion_subida = archivo;

                    var resultEntity = await NovedadProveedorService.Create(entity);

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
        public async Task<ActionResult> EditProveedorNovedadApi(NovedadProveedorDto entity)
        {


            try
            {
                if (ModelState.IsValid)
                {

                    var archivo = Request.GenerateFileFromRequest("uploadFile");
                    entity.documentacion_subida = archivo;

                    var resultEntity = await NovedadProveedorService.Update(entity);

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
        public async Task<JsonResult> DeleteProveedorNovedadApi(int? id)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    await NovedadProveedorService.Delete(new EntityDto<int>(id.Value));

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


        #endregion  Proveedor / Novedades

        #region Proveedor / Zona

        public async Task<ActionResult> GetProveedorZonaApi(int id)
        {

            var entityDto = await ZonaProveedorService.Get(new EntityDto<int>(id));

            return WrapperResponseGetApi(ModelState, () => entityDto);

        }

        [HttpPost]
        public async Task<ActionResult> CreateProveedorZonaApi(ZonaProveedorDto entity)
        {



            try
            {
                if (ModelState.IsValid)
                {

                    var resultEntity = await ZonaProveedorService.Create(entity);

                 
                    var result = new { id = resultEntity.Id };
                    var lista = _ProveedorService.GetList(entity.ZonaId);
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
        public async Task<ActionResult> EditProveedorZonaApi(ZonaProveedorDto entity)
        {



            try
            {
                if (ModelState.IsValid)
                {

                    var resultEntity = await ZonaProveedorService.Update(entity);
                    var lista = _ProveedorService.GetList(entity.ZonaId);
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
        public async Task<JsonResult> DeleteProveedorZonaApi(int? id)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    await ZonaProveedorService.Delete(new EntityDto<int>(id.Value));

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

        #endregion Proveedor / Zona

        #region Proveedor / Contrato

        public async Task<ActionResult> GetProveedorContactoApi(int proveedorId, int contratoId)
        {


            var proveedorService = EntityService as IProveedorAsyncBaseCrudAppService;
            var proveedorInfoDto = await proveedorService.GetInfo(proveedorId);
            ContratoProveedorTipoOpcionesDto entityDto = new ContratoProveedorTipoOpcionesDto();
            if (contratoId > 0)
            {
                entityDto = await ContratoProveedorService.GetInfo(new EntityDto<int>(contratoId));
            }


            var result = new
            {
                entity = entityDto,
                info = proveedorInfoDto
            };

            return WrapperResponseGetApi(ModelState, () => result);

        }

        [HttpPost]
        public async Task<ActionResult> CreateProveedorContactoApi(ContratoProveedorTipoOpcionesDto entity)
        {

            try
            {
                if (ModelState.IsValid)
                {
                    var archivo = Request.GenerateFileFromRequest("uploadFile");
                    entity.documentacion_subida = archivo;

                    //Fix Array
                    var tipoOpcionesComidaJson = Request.Form["tipo_opciones_comida_json"];
                    if (!string.IsNullOrWhiteSpace(tipoOpcionesComidaJson))
                    {
                        var tipoOpcionesComidaList = JsonConvert.DeserializeObject<List<TipoOpcionComidaDto>>(tipoOpcionesComidaJson);
                        entity.tipo_opciones_comida = tipoOpcionesComidaList;
                    }

                    var deleteIdsJson = Request.Form["deleteIds_json"];
                    if (!string.IsNullOrWhiteSpace(deleteIdsJson))
                    {
                        var deleteIds = JsonConvert.DeserializeObject<List<int>>(deleteIdsJson);
                        entity.deleteIds = deleteIds;
                    }

                    var resultEntity = await ContratoProveedorService.Create(entity);

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
        public async Task<ActionResult> EditProveedorContactoApi(
            ContratoProveedorTipoOpcionesDto entity)
        {

            try
            {
                if (ModelState.IsValid)
                {


                    var archivo = Request.GenerateFileFromRequest("uploadFile");
                    entity.documentacion_subida = archivo;


                    //Fix Array
                    var tipoOpcionesComidaJson = Request.Form["tipo_opciones_comida_json"];
                    if (!string.IsNullOrWhiteSpace(tipoOpcionesComidaJson))
                    {
                        var tipoOpcionesComidaList = JsonConvert.DeserializeObject<List<TipoOpcionComidaDto>>(tipoOpcionesComidaJson);
                        entity.tipo_opciones_comida = tipoOpcionesComidaList;
                    }

                    var deleteIdsJson = Request.Form["deleteIds_json"];
                    if (!string.IsNullOrWhiteSpace(deleteIdsJson))
                    {
                        var deleteIds = JsonConvert.DeserializeObject<List<int>>(deleteIdsJson);
                        entity.deleteIds = deleteIds;
                    }

                    var resultEntity = await ContratoProveedorService.UpdateAndDelete(entity);

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
        public async Task<JsonResult> DeleteProveedorContactoApi(int? id)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    await ContratoProveedorService.Delete(new EntityDto<int>(id.Value));

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

        #endregion  Proveedor / Contrato


        #region Proveedor / Requerimientos

        [HttpPost]
        public virtual async Task<ActionResult> EditProveedorRequerimientoApi(bool activar, int[] selectedIds)
        {
            try
            {
                if (ModelState.IsValid)
                {


                    await RequisitoProveedorService.UpdateApproved(activar, selectedIds);

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


        #region Proveedor / Hospedaje

        public ActionResult GetProveedoresHospedajeApi()
        {
            var proveedorService = EntityService as IProveedorAsyncBaseCrudAppService;

            var proveedores = proveedorService.ListProveedorHospedaje();

            return WrapperResponseGetApi(ModelState, () => proveedores);
        }

        #endregion

        #region Proveedor / Transporte
        public ActionResult GetProveedoresTransporteApi()
        {
            var proveedorService = EntityService as IProveedorAsyncBaseCrudAppService;

            var proveedores = proveedorService.ListProveedorTransporte();

            return WrapperResponseGetApi(ModelState, () => proveedores);
        }
        #endregion

        #endregion API

        public ActionResult SearchByCodeApi(string code)
        {
            var result = _catalogoService.APIObtenerCatalogos(code);
            return new JsonResult
            {
                Data = new { success = true, result },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };

        }



        [System.Web.Mvc.HttpPost]
        public ActionResult SearchPaisesApi()
        {
            var paises = _paisservice.GetPaises();
            var result = JsonConvert.SerializeObject(paises);
            return Content(result);
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult SearchProvinciasApi(int id)
        {
            var provincias = ProvinciaService.ObtenerProvinciaPorPais(id);
            var result = JsonConvert.SerializeObject(provincias);
            return Content(result);
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult SearchCantonesApi(int id)
        {
            var cantones = CiudadService.ObtenerCantonPorProvincia(id);
            var result = JsonConvert.SerializeObject(cantones);
            return Content(result);
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult SearchParroquiasApi(int id)
        {
            var parroquias = ParroquiaService.ObtenerParroquiaPorCanton(id);
            var result = JsonConvert.SerializeObject(parroquias);
            return Content(result);
        }

        public ActionResult SearchEmpresasApi()
        {
            var lista = _empresaService.GetEmpresas();
            var result = JsonConvert.SerializeObject(lista);
            return Content(result);
        }
        public ActionResult SearchCanDeleteTipoOpcionComida(int id)
        {
            bool candelete = ContratoProveedorService.CanDeleteTipoOpcionComida(id);
            return Content(candelete?"OK":"NO");
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
        public async virtual Task<ActionResult> SearchZonasApi()
        {
            //TODO: Temporal, hasta que proporcione el api correspondiente para 
            //obtener listado de servicios
            var pagedResultDto = await _zonaService.GetAll();

            var result = JsonConvert.SerializeObject(pagedResultDto,
                Formatting.None,
                new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    NullValueHandling = NullValueHandling.Ignore
                });
            return Content(result);
        }

        public ActionResult HorariosTipoComida()
        {

            return View();
        
        }
        public ActionResult GetListHorarios(int  Id)
        {
            var lista = _ProveedorService.GetList(Id);
            var result = JsonConvert.SerializeObject(lista);
            return Content(result);
        }

        public ActionResult GetZonasCobertura()
        {
            var lista = _ProveedorService.GetZonas();
            var result = JsonConvert.SerializeObject(lista);
            return Content(result);
        }
        public ActionResult GetCatalogosdeTiposOpciones()
        {
            var lista = _ProveedorService.TipoOpcionComida();
            var result = JsonConvert.SerializeObject(lista);
            return Content(result);
        }

        public ActionResult GetCreate(int tipoOpcionComidaId, TimeSpan HorarioInicio, TimeSpan HorarioFin,int zonaId)
        {
            var result = _ProveedorService.ActualizarHorarios(tipoOpcionComidaId, HorarioInicio, HorarioFin,zonaId);
            return Content(result);

        }

    }

}