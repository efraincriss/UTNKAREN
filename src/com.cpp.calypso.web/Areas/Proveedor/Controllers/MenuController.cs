using Abp.Application.Services.Dto;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.framework;
using com.cpp.calypso.proyecto.aplicacion.Dto;
using com.cpp.calypso.proyecto.aplicacion.Interfaces;
using CommonServiceLocator;
using System;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using JsonResult = com.cpp.calypso.framework.JsonResult;

namespace com.cpp.calypso.web.Areas.Proveedor.Controllers
{
    public class MenuController : BaseController
    {

        private static readonly ILogger log =
ServiceLocator.Current.GetInstance<ILoggerFactory>().Create(typeof(MenuController));


        public IViewService ViewService;
        public IProveedorAsyncBaseCrudAppService EntityService { get; }
        public IMenuProveedorAsyncBaseCrudAppService MenuProveedorService { get; }
        protected INovedadProveedorAsyncBaseCrudAppService NovedadProveedorService;
        private readonly ICatalogoAsyncBaseCrudAppService _catalogoService;
        public IArchivoAsyncBaseCrudAppService _archivoService;

        public MenuController(
            IHandlerExcepciones manejadorExcepciones,
            IViewService viewService, 
            IProveedorAsyncBaseCrudAppService entityService,
            IMenuProveedorAsyncBaseCrudAppService menuProveedorService, ICatalogoAsyncBaseCrudAppService catalogoService,
             IArchivoAsyncBaseCrudAppService archivoService,
            INovedadProveedorAsyncBaseCrudAppService novedadProveedorService) :
            base(manejadorExcepciones)
        {
            ViewService = viewService;
            EntityService = entityService;
            MenuProveedorService = menuProveedorService;
            NovedadProveedorService = novedadProveedorService;
            _catalogoService = catalogoService;
            _archivoService = archivoService;

            Title = "Lista de Restaurantes";

        }

        /// <summary>
        /// Titulo del formulario de listado
        /// </summary>
        private string _Title = string.Empty;
        /// <summary>
        /// Titulo del Formulario de Listado (Tree), si no se especifica, se obtiene desde la descripcion del modelo
        /// </summary>
        public string Title
        {
            get { return _Title; }
            set { _Title = value; }
        }

        /// <summary>
        /// Configuraciones de Contexto, etc.
        /// </summary>
        private string _NameViewTree = string.Empty;
        /// <summary>
        ///  Nombre de vista tipo Tree (lista), que se utilizada para visualizar listado del modelo.
        ///  Si no esta definido, se buscara la vista por defecto 
        /// </summary>
        public string NameViewTree
        {
            get { return _NameViewTree; }
            set { _NameViewTree = value; }
        }

     


        /// <summary>
        /// Obtener la vista tree
        /// </summary>
        /// <returns></returns>
        protected virtual View GetViewTree()
        {

            if (string.IsNullOrEmpty(NameViewTree))
                return ViewService.Get(typeof(ProveedorDto), typeof(Tree));

            return ViewService.Get(NameViewTree);
        }


        public ActionResult Index(string msg, TipoMensaje? TipoMensaje)
        {
            //1. Model
            var model = new TreeReactModelView();
            model.Id = "proveedores_menus";

            if (!string.IsNullOrEmpty(Title))
            {
                model.Title = Title;
            }


            //2. Get View 
            var view = GetViewTree();

            var treeView = view.Layout as Tree;
            model.View = treeView;

            //5. Message
            model.Mensaje = new MensajeHelper();
            if (TipoMensaje.HasValue)
            {
                model.Mensaje.Texto = msg;
                model.Mensaje.Tipo = TipoMensaje.Value;
            }

            model.ReactComponent = "~/Scripts/build/proveedorMenu.js";

            return View(model);
        }

        public ActionResult Details(int? id)
        {

            log.DebugFormat("Details({0})", id.Value);

            if (!id.HasValue)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //1. Model
            var model = new FormReactModelView();
            model.Id = "proveedores_menus";

            if (!string.IsNullOrEmpty(Title))
            {
                model.Title = Title;
            }


            return View(model);
        }

        #region API

        public async Task<ActionResult> GetProveedorMenuApi(int id)
        {
            //Padre
            var entityDto = await EntityService.Get(new EntityDto<int>(id));

            //Lista
            var menuProveedorList = await MenuProveedorService.GetMenuProveedor(id);

            var novedadProveedorList = await NovedadProveedorService.GetNovedadProveedor(id);

            var model = new { proveedor = entityDto, menus = menuProveedorList, novedades = novedadProveedorList };

            return WrapperResponseGetApi(ModelState, () => model);
        }

        [HttpPost]
        public virtual async Task<ActionResult> EditApi(bool activar, int[] selectedIds)
        {
            try
            {
                if (ModelState.IsValid)
                {

                     
                    await MenuProveedorService.UpdateApproved(activar, selectedIds);
 
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