using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using com.cpp.calypso.framework;
using com.cpp.calypso.proyecto.aplicacion;

namespace com.cpp.calypso.web.Areas.Proyecto.Controllers
{
   
    public class RdoDetalleEacController : BaseController
    {
        private readonly IRdoDetalleEacAsyncBaseCrudAppService _rdoDetalleEacService;

        // GET: Proyecto/RdoDetalleEac
        public RdoDetalleEacController(
            IHandlerExcepciones manejadorExcepciones,
            IRdoDetalleEacAsyncBaseCrudAppService rdoDetalleEacService
            ) : base(manejadorExcepciones)
        {
            _rdoDetalleEacService = rdoDetalleEacService;
        }

        public ActionResult Index()
        {
            return View();
        }
    }
}