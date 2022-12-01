using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Abp.Application.Services.Dto;
using com.cpp.calypso.framework;
using com.cpp.calypso.proyecto.aplicacion;
using com.cpp.calypso.web.Areas.Proyecto.Models;
using Newtonsoft.Json;

namespace com.cpp.calypso.web.Areas.Proyecto.Controllers
{
    
    public class ProcesoListaDistribucionController : BaseController
    {
        private readonly IProcesoListaDistribucionAsyncBaseCrudAppService _procesoListaDistribucionService;
        private readonly IProcesoNotificacionAsyncBaseCrudAppService _procesoNotificacionService;
        private readonly IListaDistribucionAsyncBaseCrudAppService _listaDistribucionService;

        // GET: Proyecto/ProcesoListaDistribucion
        public ProcesoListaDistribucionController(
            IHandlerExcepciones manejadorExcepciones,
            IProcesoListaDistribucionAsyncBaseCrudAppService procesoListaDistribucionService,
            IProcesoNotificacionAsyncBaseCrudAppService procesoNotificacionService,
            IListaDistribucionAsyncBaseCrudAppService listaDistribucionService
            ) : base(manejadorExcepciones)
        {
            _procesoListaDistribucionService = procesoListaDistribucionService;
            _procesoNotificacionService = procesoNotificacionService;
            _listaDistribucionService = listaDistribucionService;
        }

        public ActionResult Index(string msg = "")
        {
            var listas = _procesoListaDistribucionService.listar();
            if (msg != "")
            {
                ViewBag.Msg = "El registro que se inteta crear ya existe";
            }
            return View(listas);
        }

        public ActionResult Create()
        {
            var proceso = new ProcesoListaDistribucionDto()
            {
                vigente = true,
                
            };
            var viewModel = new CreateProcesoListaViewModel()
            {
                ProcesoListaDistribucion = proceso,
                procesos = _procesoNotificacionService.Listar(),
                listas = _listaDistribucionService.listar()
            };
            return View(viewModel);
        }

        [HttpPost]
#pragma warning disable CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        public async Task<ActionResult> Create(CreateProcesoListaViewModel viewModel)
#pragma warning restore CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        {
            if (ModelState.IsValid)
            {
                var proceso = viewModel.ProcesoListaDistribucion;
                var id = _procesoListaDistribucionService.Crear(proceso);

                if (id == 0)
                {
                    return RedirectToAction("Index", new {msg = "entityexist"});
                }
                else
                {
                    return RedirectToAction("Index", "ProcesoListaDistribucion");
                }
                
            }

            return View("Create", viewModel);
        }

        [HttpPost]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id.HasValue)
            {
                var proceso = await _procesoListaDistribucionService.Get(new EntityDto<int>(id.Value));
                proceso.vigente = false;

                await _procesoListaDistribucionService.Update(proceso);
                return RedirectToAction("Index", "ProcesoListaDistribucion");
            }
            return RedirectToAction("Index", "Inicio", new { area = ""});
        
        }


        [HttpPost]
        public ActionResult GetCorreosProceso(int id)
        {
            var correos = _procesoListaDistribucionService.CorreosDeProceso(id);
            var result = JsonConvert.SerializeObject(correos);
            return Content(result);
        }
    }
}