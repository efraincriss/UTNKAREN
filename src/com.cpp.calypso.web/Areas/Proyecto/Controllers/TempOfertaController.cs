using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using com.cpp.calypso.framework;
using com.cpp.calypso.proyecto.aplicacion;
using Action = Antlr.Runtime.Misc.Action;

namespace com.cpp.calypso.web.Areas.Proyecto.Controllers
{
    
    public class TempOfertaController : BaseController
    {
        private readonly ITempOfertaAsyncBaseCrudAppService _tempOfertaService;

        // GET: Proyecto/TempOferta
        public TempOfertaController(
            IHandlerExcepciones manejadorExcepciones,
            ITempOfertaAsyncBaseCrudAppService tempOfertaService
            ) : base(manejadorExcepciones)
        {
            _tempOfertaService = tempOfertaService;
        }

        public ActionResult Index(string flag="")
        {
            if (flag != "")
            {
                ViewBag.msg = "Ofertas insertadas con éxito!";
            }
            return View();
        }

        [HttpPost]
        public ActionResult CargarOfertas()
        {
            _tempOfertaService.CargarOfertas();
            return RedirectToAction("Index", "TempOferta", new {flag = "Save"});
        }

        [HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> CargarOfertas2Async(int desde=0,int hasta=0)
        {
            try
            {
                await _tempOfertaService.CargarOfertas2Async(desde, hasta);
                return Content("OK");
            }
            catch (Exception e)
            {
                return Content(e.Message.ToString());

            }
         
        }

    }
}