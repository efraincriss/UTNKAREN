using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using Abp.Application.Services.Dto;
using com.cpp.calypso.framework;
using com.cpp.calypso.proyecto.aplicacion.Dto;
using com.cpp.calypso.proyecto.aplicacion.Interfaces;

namespace com.cpp.calypso.web.Areas.Proyecto.Controllers
{
   
    public class NovedadController : BaseController
    {
        private readonly INovedadAsyncBaseCrudAppService _novedadService;

        public NovedadController(
            IHandlerExcepciones manejadorExcepciones,
            INovedadAsyncBaseCrudAppService novedadService
            ) : base(manejadorExcepciones)
        {
            _novedadService = novedadService;
        }


        public ActionResult Create(int? id)
        {
            if (id.HasValue)
            {
                NovedadDto novedad = new NovedadDto()
                {
                    fecha_solucion = DateTime.Now,
                    fecha_novedad = DateTime.Now
                };
                novedad.RequerimientoId = id.Value;
                novedad.vigente = true;
                return View(novedad);
            }

            return RedirectToAction("Details", "Requerimiento", new {id = id.Value});

        }

        [System.Web.Mvc.HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> Create(NovedadDto novedad)
        {
            if (ModelState.IsValid)
            {
                novedad.vigente = true;
                await _novedadService.Create(novedad);
                return RedirectToAction("Details", "Requerimiento", new { id = novedad.RequerimientoId });
            }

            return View("Create", novedad);
        }

        public async System.Threading.Tasks.Task<ActionResult> Details(int? id)
        {
            if (id.HasValue)
            {
                var novedad = await _novedadService.Get(new EntityDto<int>(id.Value));
                return View(novedad);
            }

            return RedirectToAction("Index", "Proyecto");
        }

        public ActionResult Edit(int? id)
        {
            if (id.HasValue)
            {
                var novedad =  _novedadService.GetDetalles(id.Value);
                return View(novedad);
            }

            return RedirectToAction("Index", "Proyecto");
        }


        
        [System.Web.Mvc.HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> Edit(NovedadDto novedad)
        {
            if (ModelState.IsValid)
            {
                await _novedadService.Update(novedad);
                return RedirectToAction("Details", "Novedad", new { id = novedad.Id });
            }

            return View("Edit", novedad);
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult Delete(int? id)
        {
            if (id.HasValue)
            {
                var requerimientoId = _novedadService.EliminarVigencia(id.Value);
                return RedirectToAction("Details", "Requerimiento", new {id = requerimientoId});
            }

            return RedirectToAction("Index", "Proyecto");
        }
    }
}
