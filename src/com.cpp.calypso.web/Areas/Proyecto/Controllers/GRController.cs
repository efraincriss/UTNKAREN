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

namespace com.cpp.calypso.web.Areas.Proyecto.Controllers
{
   
    public class GRController : BaseController
    {
        private readonly IGRAsyncBaseCrudAppService _grService;
        private readonly IProyectoAsyncBaseCrudAppService _proyectoService;

        public GRController(
            IHandlerExcepciones manejadorExcepciones,
            IGRAsyncBaseCrudAppService grService,
            IProyectoAsyncBaseCrudAppService proyectoService
        ) : base(manejadorExcepciones)
        {
            _grService = grService;
            _proyectoService = proyectoService;
        }

        public ActionResult Index(string flag = "")
        {
            var grs = _grService.Listar();
            if (flag != "")
            {
                ViewBag.Msg = "No se puede eliminar el GR ya que tiene registros relacionados";
            }
            return View(grs);
        }

        public ActionResult Create()
        {
            var gr = new GRDto(){vigente = true, fecha_registro = DateTime.Now};
            var proyectos = _proyectoService.Listar();
            CreateGrViewModel viewModel = new CreateGrViewModel()
            {
                Gr = gr,
                Proyectos = proyectos
            };
            return View(viewModel);
        }

        [HttpPost]
        public async Task<ActionResult> Create(GRDto gr)
        {
            if (ModelState.IsValid)
            {
                var g = await _grService.Create(gr);
                return RedirectToAction("Index", "GR");
            }
            var proyectos = _proyectoService.Listar();
            CreateGrViewModel viewModel = new CreateGrViewModel()
            {
                Gr = gr,
                Proyectos = proyectos
            };
            return View("Create", viewModel);
        }

#pragma warning disable CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        public async Task<ActionResult> Details(int? id) //GRId
#pragma warning restore CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        {
            if (id.HasValue)
            {
                var gr =  _grService.GetGr(id.Value);
                return View(gr);
            }

            return RedirectToAction("Index", "GR");
        }

        [HttpPost]
        public ActionResult GetGrMonto(int id)
        {
            var monto = _grService.GetMontoTotal(id);
            return Content(monto + "");
        }

        public async Task<ActionResult> Edit(int? id) // GrId
        {
            if (id.HasValue)
            {
                var gr = await _grService.Get(new EntityDto<int>(id.Value));
                return View(gr);
            }

            return RedirectToAction("Index", "Inicio", new {area = ""});
        }

        [HttpPost]
        public async Task<ActionResult> Edit(GRDto gr)
        {
            if (ModelState.IsValid)
            {
                await _grService.Update(gr);
                return RedirectToAction("Index", "GR");
            }

            return View("Edit", gr);
        }

        [HttpPost]
        public ActionResult Delete(int id) //GrId
        {
            var eliminado = _grService.Eliminar(id);
            if (eliminado)
            {
                return RedirectToAction("Index", "GR");
            }

            return RedirectToAction("Index", "GR", new {flag = "ErrorDelete"});
        }
        
    }
}
