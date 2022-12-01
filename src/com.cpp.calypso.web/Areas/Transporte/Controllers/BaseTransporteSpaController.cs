using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.framework;
using CommonServiceLocator;
using JsonResult = com.cpp.calypso.framework.JsonResult;

namespace com.cpp.calypso.web.Areas.Transporte.Controllers
{
    public abstract class BaseTransporteSpaController<TEntity, TEntityDto, TGetAllInput> : BaseController
        where TEntity : class, IEntity<int>
        where TEntityDto : class, IEntityDto<int>
        where TGetAllInput : PagedAndFilteredResultRequestDto
    {

        public IViewService ViewService { get; }

        private static readonly ILogger log =
            ServiceLocator.Current.GetInstance<ILoggerFactory>().Create(typeof(BaseTransporteSpaController<,,>));
        public IAsyncBaseCrudAppService<TEntity, TEntityDto, TGetAllInput, TEntityDto> EntityService { get; }

        protected BaseTransporteSpaController(
            IHandlerExcepciones manejadorExcepciones,
            IViewService viewService,
            IAsyncBaseCrudAppService<TEntity, TEntityDto, TGetAllInput, TEntityDto> entityService
            ) : base(manejadorExcepciones)
        {
            EntityService = entityService;
            ViewService = viewService;
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
#pragma warning disable CS0169 // The field 'BaseTransporteSpaController<TEntity, TEntityDto, TGetAllInput>.manejadorExcepciones' is never used
        private IHandlerExcepciones manejadorExcepciones;
#pragma warning restore CS0169 // The field 'BaseTransporteSpaController<TEntity, TEntityDto, TGetAllInput>.manejadorExcepciones' is never used

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

        public virtual async Task<ActionResult> GetAllApi()
        {
            //TODO: Wrapper todo el bloque, para controlar excepciones

            var result = await EntityService.GetAll();

            return WrapperResponseGetApi(ModelState, () => result);

        }

        public virtual  ActionResult GetAllIncludingApi()
        {
            var result = EntityService.GetAllIncluding();

            return WrapperResponseGetApi(ModelState, () => result);

        }
 

        public async virtual Task<ActionResult> GetApi(int id)
        {
            //TODO: Wrapper todo el bloque, para controlar excepciones
            var entityDto = await EntityService.Get(new EntityDto<int>(id));

            return WrapperResponseGetApi(ModelState, () => entityDto);

        }

    }
}