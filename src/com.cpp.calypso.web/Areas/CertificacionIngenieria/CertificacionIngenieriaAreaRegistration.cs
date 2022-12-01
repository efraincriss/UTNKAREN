using System.Web.Mvc;

namespace com.cpp.calypso.web.Areas.CertificacionIngenieria
{
    public class CertificacionIngenieriaAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "CertificacionIngenieria";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "CertificacionIngenieria_default",
                "CertificacionIngenieria/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}