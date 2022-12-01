using System.Web;
using com.cpp.calypso.comun.dominio;
#pragma warning restore CS0105 // The using directive for 'com.cpp.calypso.comun.dominio' appeared previously in this namespace
using CommonServiceLocator;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;

namespace com.cpp.calypso.web
{
    public static class UsuarioExtensions
    {
       


        public static string GetIdentityUser(this Usuario usuario)
        {
            var CampoIdentidad = ManagerUser.CampoIdentidad;
            if (CampoIdentidad == CamposIdentidadUsuario.Id)
                return usuario.Id.ToString();

            if (CampoIdentidad == CamposIdentidadUsuario.UserName)
                return usuario.Cuenta;

            return null;
        }


        public static MvcHtmlString GetUserName(this HtmlHelper htmlHelper)
        {
                var application = ServiceLocator.Current.GetInstance<IApplication>();
                var usuario = application.GetCurrentUser();
                if (usuario != null)
                    return MvcHtmlString.Create(usuario.Nombres);

                return MvcHtmlString.Create(string.Empty);
        }
    }

    public enum CamposIdentidadUsuario
    {
        Id,
        UserName 
    }

    
}