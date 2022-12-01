using com.cpp.calypso.framework;
using com.cpp.calypso.proyecto.aplicacion.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace com.cpp.calypso.web.Areas.Proyecto.Controllers
{

    public class DestinatarioCartaController : BaseController
    {
        private readonly IEmpresaAsyncBaseCrudAppService _empresaService;
        private readonly IClienteAsyncBaseCrudAppService _clienteService;
        private readonly IDestinatarioAsyncBaseCrudAppService _destinatarioService;
        private readonly IDestinatarioCartaAsyncBaseCrudAppService _dcartaService;
        private readonly ICartaAsyncBaseCrudAppService _cartaService;
        private readonly ICartaArchivoAsyncBaseCrudAppService _cartaarchivosService;
        public DestinatarioCartaController(IHandlerExcepciones manejadorExcepciones,
            IEmpresaAsyncBaseCrudAppService empresaService,
            ICartaAsyncBaseCrudAppService cartaService,
            IClienteAsyncBaseCrudAppService clienteService,
            IDestinatarioAsyncBaseCrudAppService destinatarioService,
            IDestinatarioCartaAsyncBaseCrudAppService dcartaService,
            ICartaArchivoAsyncBaseCrudAppService cartaarchivosService
            ) : base(manejadorExcepciones)
        {

            _empresaService = empresaService;
            _cartaService = cartaService;
            _clienteService = clienteService;
            _destinatarioService = destinatarioService;
            _dcartaService = dcartaService;
            _cartaarchivosService = cartaarchivosService;
        }
        // GET: Proyecto/DestinatarioCarta
        public ActionResult Index()
        {
            return View();
        }

        // GET: Proyecto/DestinatarioCarta/Details/5
        public ActionResult Details(int id)
        {

            var da = _dcartaService.getdetalle(id);

            return View(da);
        }

        // GET: Proyecto/DestinatarioCarta/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Proyecto/DestinatarioCarta/Create
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

        // GET: Proyecto/DestinatarioCarta/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Proyecto/DestinatarioCarta/Edit/5
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

        // GET: Proyecto/DestinatarioCarta/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Proyecto/DestinatarioCarta/Delete/5
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
