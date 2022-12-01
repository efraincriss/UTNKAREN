using System.Net;
using System.Web.Mvc;

namespace  com.cpp.calypso.web
{
    [AllowAnonymous]
    public class ErrorController : Controller
    {
        public ActionResult Index()
        {

            return InternalServerError();
        }

        public ActionResult NotFound()
        {
            Response.TrySkipIisCustomErrors = true;
            //Response.StatusCode = (int)HttpStatusCode.NotFound;
            return View("NotFound");
        }

        public ActionResult InternalServerError()
        {

            if (User != null && User.Identity != null &&
                User.Identity.IsAuthenticated)
            {
                Response.TrySkipIisCustomErrors = true;
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return View("InternalServerError");
            }
            else
            {
                Response.TrySkipIisCustomErrors = true;
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return View("InternalServerErrorPublic");
            }

        }

    }
}