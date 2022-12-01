using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.Mvc;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.framework;
using CommonServiceLocator;

namespace com.cpp.calypso.web.Areas.Documentos.Controllers
{
    public abstract class BaseAccesoSpaController<TEntity, TEntityDto, TGetAllInput> : BaseController
        where TEntity : class, IEntity<int>
        where TEntityDto : class, IEntityDto<int>
        where TGetAllInput : PagedAndFilteredResultRequestDto
    {

        public IViewService ViewService { get; }

        private static readonly ILogger log =
            ServiceLocator.Current.GetInstance<ILoggerFactory>().Create(typeof(BaseAccesoSpaController<,,>));

        protected BaseAccesoSpaController(
            IHandlerExcepciones manejadorExcepciones,
            IViewService viewService
            
            ) : base(manejadorExcepciones)
        {
   
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
    }
}