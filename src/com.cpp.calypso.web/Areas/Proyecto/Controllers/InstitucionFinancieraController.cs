using Abp.Application.Services.Dto;
using com.cpp.calypso.framework;
using com.cpp.calypso.proyecto.aplicacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;


namespace com.cpp.calypso.web.Areas.Proyecto.Controllers
{
    
    public class InstitucionFinancieraController : BaseController
    {
        private readonly IInstitucionFinancieraAsyncBaseCrudAppService _institucionFinancieraService;

        public InstitucionFinancieraController(
            IHandlerExcepciones manejadorExcepciones,
            IInstitucionFinancieraAsyncBaseCrudAppService institucionFinancieraService
            ) : base(manejadorExcepciones)
        {
            _institucionFinancieraService = institucionFinancieraService;
        }
        // GET: Proyecto/InstitucionFinanciera
        public ActionResult Index()
        {
            var instituciones = _institucionFinancieraService.GetInstitucionesFinancierasDto();
            return View(instituciones);
        }

        // GET: Proyecto/InstitucionFinanciera/Details/5
        public async System.Threading.Tasks.Task<ActionResult> Details(int? id)
        {
            if (id.HasValue)
            {
                var institucion = await _institucionFinancieraService.Get(new EntityDto<int>(id.Value));
                return View(institucion);
            }
            return RedirectToAction("Index", "Inicio", new { area = "" });
        }

        // GET: Proyecto/InstitucionFinanciera/Create
        public ActionResult Create()
        {
            var institucion = new InstitucionFinancieraDto();
            return View(institucion);
        }

        // POST: Proyecto/InstitucionFinanciera/Create
        [HttpPost]
        public async Task<ActionResult> Create(InstitucionFinancieraDto institucion)
        {
            if (ModelState.IsValid)
            {
                institucion.vigente = true;
                var dto = await _institucionFinancieraService.Create(institucion);
                return RedirectToAction("Index", "InstitucionFinanciera");
            }

            return View("Create", institucion);
        }

        // GET: Proyecto/InstitucionFinanciera/Edit/5
        public async System.Threading.Tasks.Task<ActionResult> Edit(int? id)
        {
            if (id.HasValue)
            {
                var institucion = await _institucionFinancieraService.Get(new EntityDto<int>(id.Value));
                return View(institucion);
            }
            return RedirectToAction("Index", "InstitucionFinanciera");
        }

        // POST: Proyecto/InstitucionFinanciera/Edit/5
        [HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> Edit(InstitucionFinancieraDto institucionFinanciera)
        {
            if (ModelState.IsValid)
            {
                institucionFinanciera.vigente = true;
                await _institucionFinancieraService.Update(institucionFinanciera);
                return RedirectToAction("Index", "InstitucionFinanciera");
            }

            return View("Edit", institucionFinanciera);
        }

        // POST: Proyecto/InstitucionFinanciera/Delete/5
        [HttpPost]
        public ActionResult Delete(int? id)
        {
            if (ModelState.IsValid)
            {
                var eliminado = _institucionFinancieraService.Eliminar(id.Value);

                if (eliminado)
                {
                    return RedirectToAction("Index", "InstitucionFinanciera");
                }
                else
                {
                    return RedirectToAction("Index", "InstitucionFinanciera", new {  flag = "error_delete" });
                }
            }

            return RedirectToAction("Index", "Inicio", new { area = "" });
        }
    }
}

