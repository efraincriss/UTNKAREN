using com.cpp.calypso.framework;
using com.cpp.calypso.proyecto.aplicacion.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace com.cpp.calypso.web.Areas.RRHH.Controllers
{
    public class ColaboradorResponsabilidadController : BaseController
    {
        private readonly IColaboradorResponsabilidadAsyncBaseCrudAppService _responsabilidadesService;
        public ColaboradorResponsabilidadController(
            IHandlerExcepciones manejadorExcepciones,
            IColaboradorResponsabilidadAsyncBaseCrudAppService responsabilidadesService
            ) : base(manejadorExcepciones)
        {
            _responsabilidadesService = responsabilidadesService;
        }

        // GET: RRHH/ColaboradorResponsabilidad
        public ActionResult Index()
        {
            return View();
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult GetListaApi()
        {
            var list = _responsabilidadesService.GetList();
            var result = JsonConvert.SerializeObject(list);
            return Content(result);
        }
    }
}