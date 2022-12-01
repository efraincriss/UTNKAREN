using System;
using System.Web.Mvc;
using com.cpp.calypso.framework;
using com.cpp.calypso.proyecto.aplicacion;

namespace com.cpp.calypso.web.Areas.Proyecto.Controllers
{
    
    public class RdoDetalleController : BaseController
    {
        private readonly IRdoDetalleAsyncBaseCrudAppService _rdoDetalleService;

        // GET: Proyecto/RdoDetalle
        public RdoDetalleController(
            IHandlerExcepciones manejadorExcepciones,
            IRdoDetalleAsyncBaseCrudAppService rdoDetalleService
            ) : base(manejadorExcepciones)
        {
            _rdoDetalleService = rdoDetalleService;
        }

        public ActionResult Index(int? id) // CabeceraId
        {
            if (id.HasValue)
            {
                var detalles = _rdoDetalleService.GetRdoDetalles(id.Value);
                return View(detalles);
            }
            return RedirectToAction("Index", "Inicio", new { area = "" });
        }

        // GET: Proyecto/RdoDetalle/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Proyecto/RdoDetalle/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Proyecto/RdoDetalle/Create
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

        // GET: Proyecto/RdoDetalle/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Proyecto/RdoDetalle/Edit/5
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

        // GET: Proyecto/RdoDetalle/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Proyecto/RdoDetalle/Delete/5
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
