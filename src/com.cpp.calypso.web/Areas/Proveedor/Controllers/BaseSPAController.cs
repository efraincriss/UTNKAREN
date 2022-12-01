using Abp.Application.Services.Dto;
using Abp.Domain.Entities;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.framework;
using com.cpp.calypso.proyecto.dominio;
using CommonServiceLocator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using JsonResult = com.cpp.calypso.framework.JsonResult;

namespace com.cpp.calypso.web
{
    /// <summary>
    /// Controlador base, para aplicaciones SPA
    /// </summary>
    public abstract class BaseSPAController<TEntity, TEntityDto, TGetAllInput> : BaseController
         where TEntity : class, IEntity<int>
         where TEntityDto : class, IEntityDto<int>
         where TGetAllInput : PagedAndFilteredResultRequestDto
    {

        private static readonly ILogger log =
ServiceLocator.Current.GetInstance<ILoggerFactory>().Create(typeof(BaseSPAController<,,>));


        public IViewService ViewService { get; }
        public IAsyncBaseCrudAppService<TEntity, TEntityDto, TGetAllInput, TEntityDto> EntityService { get; }

        protected BaseSPAController(IHandlerExcepciones manejadorExcepciones,
            IViewService viewService,
            IAsyncBaseCrudAppService<TEntity, TEntityDto, TGetAllInput, TEntityDto> entityService) :
            base(manejadorExcepciones)
        {
            ViewService = viewService;
            EntityService = entityService;
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
        /// Clave para generar controles de la UI 
        /// </summary>
        private string _Key = string.Empty;
        /// <summary>
        /// Clave para generar controles de la UI 
        /// </summary>
        public string Key
        {
            get { return _Key; }
            set { _Key = value; }
        }

        /// <summary>
        /// Componente Javascript asociada
        /// </summary>
        private string _ComponentJS = string.Empty;
        /// <summary>
        /// Componente Javascript asociada
        /// </summary>
        public string ComponentJS
        {
            get { return _ComponentJS; }
            set { _ComponentJS = value; }
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
                return ViewService.Get(typeof(TEntityDto), typeof(Tree));

            return ViewService.Get(NameViewTree);
        }


        public virtual ActionResult Index(string msg, TipoMensaje? TipoMensaje)
        {
            //1. Model
            var model = new TreeReactModelView();
            model.Id = Key;

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

            model.ReactComponent = ComponentJS;

            return View(model);
        }


         
       

        #region API
  
        public virtual async Task<ActionResult> GetAllApi()
        {
            //TODO: Wrapper todo el bloque, para controlar excepciones

            var result = await EntityService.GetAll();

            return WrapperResponseGetApi(ModelState, () => result);

        }
 

        public async virtual Task<ActionResult> GetApi(int id)
        {
            //TODO: Wrapper todo el bloque, para controlar excepciones
            var entityDto = await EntityService.Get(new EntityDto<int>(id));

            return WrapperResponseGetApi(ModelState, () => entityDto);

        }


        [HttpPost]
        public async virtual Task<JsonResult> CreateApi(TEntityDto entity, FormCollection formCollection)
        {
            try
            {
                if (ModelState.IsValid)
                {

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
        public async virtual Task<JsonResult> EditApi(TEntityDto entity, FormCollection formCollection)
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
        public async virtual Task<JsonResult> DeleteApi(int? id)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    await EntityService.Delete(new EntityDto<int>(id.Value));

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


    }
}