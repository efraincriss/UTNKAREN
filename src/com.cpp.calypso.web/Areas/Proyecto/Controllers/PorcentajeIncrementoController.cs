using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using com.cpp.calypso.framework;
using com.cpp.calypso.proyecto.aplicacion;
using com.cpp.calypso.proyecto.aplicacion.Interfaces;

namespace com.cpp.calypso.web.Areas.Proyecto.Controllers
{
    
    public class PorcentajeIncrementoController : BaseController
    {

        private readonly IPorcentajeIncrementoAsyncBaseCrudAppService _porcentajeService;

        public PorcentajeIncrementoController(IHandlerExcepciones manejadorExcepciones,
            IPorcentajeIncrementoAsyncBaseCrudAppService porcentajeService) : base(manejadorExcepciones)
        {
            _porcentajeService = porcentajeService;

        }

        // GET: Proyecto/PorcentajeIncremento
        public ActionResult Index()
        {
            var lista = _porcentajeService.Listar();
            return View(lista);
        }

        // GET: Proyecto/PorcentajeIncremento/Details/5
        public ActionResult Details(int id)
        {
            var porcentaje = _porcentajeService.getdetalle(id);
            return View(porcentaje);
        }

        // GET: Proyecto/PorcentajeIncremento/Create
        public ActionResult Create()
        {
            PorcentajeIncrementoDto n = new PorcentajeIncrementoDto();
            n.vigente = true;
            return View();
        }

        // POST: Proyecto/PorcentajeIncremento/Create
        [HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> Create(PorcentajeIncrementoDto n)

        {
            n.vigente = true;
            var r =await  _porcentajeService.Create(n);

            return RedirectToAction("Index");
        }

        // GET: Proyecto/PorcentajeIncremento/Edit/5
        public ActionResult Edit(int id)
        {
            var porcentaje = _porcentajeService.getdetalle(id);
            return View(porcentaje);
        }

        // POST: Proyecto/PorcentajeIncremento/Edit/5
        [HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> Edit(PorcentajeIncrementoDto n)
        {

            var r = await _porcentajeService.Update(n);
                return RedirectToAction("Index");
            
              
          
        }


        // POST: Proyecto/PorcentajeIncremento/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            var r = _porcentajeService.Eliminar(id);

            if (r)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }

#pragma warning disable CS0162 // Unreachable code detected
            return View();
#pragma warning restore CS0162 // Unreachable code detected
        }
    }
}
