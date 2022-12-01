using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.framework;
using com.cpp.calypso.proyecto.aplicacion.Dto;
using com.cpp.calypso.proyecto.aplicacion.Interfaces;
using com.cpp.calypso.proyecto.dominio;
using CommonServiceLocator;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using JsonResult = com.cpp.calypso.framework.JsonResult;

namespace com.cpp.calypso.web.Areas.Proveedor.Controllers
{
    public class SolicitudViandaController : BaseSPAController<SolicitudVianda,SolicitudViandaDto, PagedAndFilteredResultRequestDto>
    {
        private readonly ICatalogoAsyncBaseCrudAppService _catalogoService;
        private readonly IColaboradoresAsyncBaseCrudAppService _colaboradoresService;
        private static readonly ILogger log =
ServiceLocator.Current.GetInstance<ILoggerFactory>().Create(typeof(SolicitudViandaController));

        
        public ILocacionAsyncBaseCrudAppService LocacionService { get; }
    
        public SolicitudViandaController(IHandlerExcepciones manejadorExcepciones,
            IViewService viewService,
            ISolicitudViandaAsyncBaseCrudAppService entityService,
                IColaboradoresAsyncBaseCrudAppService colaboradoresService,
            ICatalogoAsyncBaseCrudAppService catalogoService,
            ILocacionAsyncBaseCrudAppService locacionService
            ) : 
            
            base(manejadorExcepciones, viewService, entityService)
        {
       
            LocacionService = locacionService;
            _catalogoService = catalogoService;
            _colaboradoresService = colaboradoresService;

            Title = "Visualización de Pedidos - Diarios";
            Key = "solicitud_vianda";
            ComponentJS = "~/Scripts/build/solicitudVianda.js";
        }

           

        #region API

 
        public async Task<ActionResult> GetSolicitudDiariaApi(DateTime? fecha)
        {
            var entityService = EntityService as ISolicitudViandaAsyncBaseCrudAppService;

            var result = await entityService.GetSolicitudDiaria(fecha);

            return WrapperResponseGetApi(ModelState, () => result);

        }


        public async Task<ActionResult> GetSolicitudPendientesApi(DateTime fecha, int tipoComidaId)
        {

            var entityService = EntityService as ISolicitudViandaAsyncBaseCrudAppService;

            var result = await entityService.GetSolicitudesPendientes(fecha,tipoComidaId);

            return WrapperResponseGetApi(ModelState, () => result);

        }

      
        [HttpPost]
        public async Task<JsonResult> EditCancelApi(int id,string observaciones)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var entityService = EntityService as ISolicitudViandaAsyncBaseCrudAppService;


                    var result = await entityService.Cancel(id, observaciones);

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


        #region TEMP
        // TODO: Deben ser proporcionados... por el cliente. (Dependencias)

        public async virtual Task<ActionResult> GetLocacionApi()
        {
            var entityDto = await LocacionService.GetAll();

            return WrapperResponseGetApi(ModelState, () => entityDto);

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
        public ActionResult SearchTipoComida(string code)
        {
            var result = _catalogoService.APIObtenerCatalogos(code).Where(c => c.valor_texto == "VIANDA").ToList(); ;
            return new JsonResult
            {
                Data = new { success = true, result },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };

        }

        public async Task<ActionResult> SearchColaboradores()
        {
            var entityDto = await _colaboradoresService.GetLookupAll();
            return WrapperResponseGetApi(ModelState, () => entityDto);
        }
        public ActionResult SearchAnotadores()
        {
            var entityDto = _colaboradoresService.GetAnotadoresLookupAll();
            return WrapperResponseGetApi(ModelState, () => entityDto);
        }



    }
}