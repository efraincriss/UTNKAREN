﻿using System.Collections.Generic;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities;
using com.cpp.calypso.comun.dominio;

namespace com.cpp.calypso.web
{
    /// <summary>
    /// Model View
    /// </summary>
    public class FormReactModelView : IViewModel
    {
        /// <summary>
        /// TODO: Mejorar
        /// Identificador Unico. Un id unico, para establecerlo en los controles de presentacion. 
        /// </summary>
        public string Id;

        /// <summary>
        /// Titulo utilizado en formulario. Si no se especifica, se obtiene el titulo desde la
        /// descripcion del modelo
        /// </summary>
        public string Title;


        /// <summary>
        /// Model
        /// </summary>
        public IEntity Model;

        /// <summary>
        /// View
        /// </summary>
        public Form View;

        /// <summary>
        /// Metadata view
        /// </summary>
        public Dictionary<string, object> Metadata;

        /// <summary>
        /// 
        /// </summary>
        public IEntityDto ModelDto { get; internal set; }

        public string ReactComponent { get; internal set; }
    }
}