using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using com.cpp.calypso.framework;
using com.cpp.calypso.proyecto.aplicacion;
using com.cpp.calypso.proyecto.aplicacion.Interfaces;
using Newtonsoft.Json;

namespace com.cpp.calypso.web.Areas.Proyecto.Controllers
{
    
    public class DetalleGRController : BaseController
    {
        private readonly IDetalleGRAsyncBaseCrudAppService _detalleGrService;
        private readonly ICertificadoAsyncBaseCrudAppService _certificadoService;

        // GET: Proyecto/DetalleGR
        public DetalleGRController(
            IHandlerExcepciones manejadorExcepciones,
            IDetalleGRAsyncBaseCrudAppService detalleGrService,
            ICertificadoAsyncBaseCrudAppService certificadoService
        ) : base(manejadorExcepciones)
        {
            _detalleGrService = detalleGrService;
            _certificadoService = certificadoService;
        }

        public ActionResult Index()
        {
            return View();
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult GetDetalles(int id) // DetalleGr
        {
            var detalles = _detalleGrService.ListarPorGr(id);
            var result = JsonConvert.SerializeObject(detalles);
            return Content(result);
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult GetCertificados(int id) // proyectoId
        {
            var items = _certificadoService.GetCretificadosGr(id);
            var result = JsonConvert.SerializeObject(items);
            return Content(result);
        }

        [System.Web.Mvc.HttpPost]
        public async Task<ActionResult> Create([FromBody] int [] ids, [FromBody] int GrId)
        {
            var cont = await _detalleGrService.CrearDetalles(ids, GrId);
            return Content(cont+"");
        }

        [System.Web.Mvc.HttpPost]
        public async Task<ActionResult> Delete(int id) //DetalleGrId
        {
            await _detalleGrService.Eliminar(id);
            return Content("Ok");
        }
    }
}