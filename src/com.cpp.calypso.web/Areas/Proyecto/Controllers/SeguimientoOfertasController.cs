using com.cpp.calypso.framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using com.cpp.calypso.proyecto.aplicacion.Interfaces;

namespace com.cpp.calypso.web.Areas.Proyecto.Controllers
{
    
    public class SeguimientoOfertasController : BaseController
    {
        private readonly IComputoAsyncBaseCrudAppService _computoService;

        // GET: Proyecto/SeguimientoOfertas
        public SeguimientoOfertasController(
            IHandlerExcepciones manejadorExcepciones,
            IComputoAsyncBaseCrudAppService computoService
            ) : base(manejadorExcepciones)
        {
            _computoService = computoService;
        }

        public ActionResult Index()
        {

            var avances = _computoService.GetComputosPorOferta(1);
            return View(avances);
        }

        // GET: Proyecto/SeguimientoOfertas/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Proyecto/SeguimientoOfertas/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Proyecto/SeguimientoOfertas/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Proyecto/SeguimientoOfertas/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Proyecto/SeguimientoOfertas/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Proyecto/SeguimientoOfertas/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Proyecto/SeguimientoOfertas/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
