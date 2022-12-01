﻿using com.cpp.calypso.framework;
using System.Configuration;
using System.Web;
using System.Web.Mvc;

namespace com.cpp.calypso.web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            //Referencia del Codigo:
            //https://github.com/ASP-NET-MVC/aspnetwebstack/blob/master/src/System.Web.Mvc/HandleErrorAttribute.cs

            filters.Add(new HandleErrorFilterAttribute());


            //Filtro para pedir autentificacion a todos
            filters.Add(new AuthorizeAttribute());


            //Filtro para verificar si el usuario mantiene la sesion autentificada
            filters.Add(new SessionExpireFilterAttribute());


            //Filtro para controla autorizacion del sistema. Basado en los permisos del rol y usuario autentificado


            //Add ByPass Authorize
            var skipAutorizationStr = ConfigurationManager.AppSettings["Security.Authorization.Skip"];
            var skipAutorization = false;
            bool.TryParse(skipAutorizationStr, out skipAutorization);

            if (!skipAutorization) {
                filters.Add(new AuthorizeFilterAttribute());
            }

            


        }
    }
}
