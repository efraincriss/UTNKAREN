using com.cpp.calypso.framework;
using com.cpp.calypso.proyecto.aplicacion;
using com.cpp.calypso.proyecto.aplicacion.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace com.cpp.calypso.web.Areas.Proyecto.Controllers
{
    
    public class CartaArchivoController : BaseController
    {// GET: Proyecto/Carta
        private readonly IEmpresaAsyncBaseCrudAppService _empresaService;
        private readonly IClienteAsyncBaseCrudAppService _clienteService;
        private readonly IDestinatarioAsyncBaseCrudAppService _destinatarioService;
        private readonly IDestinatarioCartaAsyncBaseCrudAppService _dcartaService;
        private readonly ICartaAsyncBaseCrudAppService _cartaService;
        private readonly ICartaArchivoAsyncBaseCrudAppService _cartaarchivosService;
        public CartaArchivoController(IHandlerExcepciones manejadorExcepciones,
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
        // GET: Proyecto/CartaArchivo
        public ActionResult Index()
        {
            return View();
        }

        // GET: Proyecto/CartaArchivo/Details/5
        public ActionResult Details(int id)
        {

            var ca = _cartaarchivosService.getdetalle(id);
            return View(ca);
        }

        // GET: Proyecto/CartaArchivo/Create
        public ActionResult Create(int id)
        {

            var carta = _cartaService.getdetalle(id);
            CartaArchivoDto ca = new CartaArchivoDto
            {
                CartaId = id,
                InfoCarta = carta
                   };
            return View(ca);
        }

        // POST: Proyecto/CartaArchivo/Create
        [HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> Create(CartaArchivoDto ca)
        {
            try
            {
                if (ModelState.IsValid) {
                    ca.vigente = true;
                    var r = await _cartaarchivosService.Create(ca);
                    return RedirectToAction("Details", new {id=r.Id});
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Proyecto/CartaArchivo/Edit/5
        public ActionResult Edit(int id)
        {
            var ca = _cartaarchivosService.getdetalle(id);
            return View(ca);
        }

        // POST: Proyecto/CartaArchivo/Edit/5
        [HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> Edit(CartaArchivoDto d)
        {
            try
            {
                if (ModelState.IsValid) {
                    var r = await _cartaarchivosService.Update(d);

                    return RedirectToAction("Details", new { id = r.Id });
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Proyecto/CartaArchivo/Delete/5
  
        // POST: Proyecto/CartaArchivo/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
               var r= _cartaarchivosService.getdetalle(id);
                r.vigente = false;


                return RedirectToAction("Details",new { id=r.CartaId});
            }
            catch
            {
                return View();
            }
        }
    }
}
