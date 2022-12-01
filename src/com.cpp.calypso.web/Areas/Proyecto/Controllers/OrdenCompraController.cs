using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Abp.Application.Services.Dto;
using com.cpp.calypso.framework;
using com.cpp.calypso.proyecto.aplicacion;
using com.cpp.calypso.proyecto.aplicacion.Interfaces;
using com.cpp.calypso.proyecto.dominio;
using com.cpp.calypso.web.Areas.Proyecto.Models;

namespace com.cpp.calypso.web.Areas.Proyecto.Controllers
{

    
    public class OrdenCompraController : BaseController
    {
        private readonly IOrdenCompraAsyncBaseCrudAppService _ordencompraService;
        private readonly IOfertaAsyncBaseCrudAppService _ofertaService;

        public OrdenCompraController(
            IHandlerExcepciones manejadorExcepciones,
            IOrdenCompraAsyncBaseCrudAppService ordencompraService,
            IOfertaAsyncBaseCrudAppService ofertaService
        ) : base(manejadorExcepciones)
        {
            _ordencompraService = ordencompraService;
           
            _ofertaService = ofertaService;
        }

        public ActionResult IndexOrdenCompra()
        {

            var ofertas = _ofertaService.GetOfertasDefinitivas();

            return View(ofertas);

        }
        // GET: Proyecto/OrdenCompra
        public ActionResult Index(int? id)
        {
            if (id.HasValue)
            {
                var oferta = _ofertaService.getdetalle(id.Value);
                var lista = _ordencompraService.listar(id.Value);
                ViewBag.Id = id.Value;
                OfertaOrdenCompraViewModel n = new OfertaOrdenCompraViewModel
                {
                    oferta = oferta,
                    listaordenes =lista

                };
                return View(n);
            }

            return RedirectToAction("Index", "Inicio");
        }
        // GET: Proyecto/OrdenCompra/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Proyecto/OrdenCompra/Create
        public ActionResult Create(int? id)
        {

            var oferta = _ofertaService.getdetalle(id.Value);
            if (id.HasValue)
            {
                OrdenCompraDto orden = new OrdenCompraDto
                {

                    OfertaId = id.Value,
                    vigente = true,
                    fecha_presentacion = DateTime.Now,
                    Oferta = AutoMapper.Mapper.Map<Oferta>(oferta),
                };
                return View(orden);
            }

            return RedirectToAction("Index", "Inicio");
        }
        // POST: Proyecto/OrdenCompra/Create
        [HttpPost]
        public async Task<ActionResult> Create(OrdenCompraDto orden)
        {
            if (ModelState.IsValid)
            {
             
                var ordenServicio = await _ordencompraService.Create(orden);
                return RedirectToAction("Index", "OrdenCompra", new { id = ordenServicio.OfertaId });
            }

            return View("Create", orden);
        }

        // GET: Proyecto/OrdenCompra/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id.HasValue)
            {
           
                var orden = await _ordencompraService.Get(new EntityDto<int>(id.Value));
                var oferta = _ofertaService.getdetalle(orden.OfertaId);
                orden.Oferta = AutoMapper.Mapper.Map<Oferta>(oferta);
                if (orden != null)
                {
                    return View(orden);
                }
            }
            return RedirectToAction("Index", "Inicio");
        }


        // POST: Proyecto/OrdenCompra/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(OrdenCompraDto orden)
        {
            if (ModelState.IsValid)
            {
                var ordenServicio = await _ordencompraService.Update(orden);
                return RedirectToAction("Index", "OrdenCompra", new { id = ordenServicio.OfertaId });

            }

            return View("Edit", orden);
        }


        [HttpPost]
        public ActionResult Delete(int? id)
        {
            if (id.HasValue)
            {
                var ofertaId = _ordencompraService.EliminarVigencia(id.Value);
                return RedirectToAction("Index", "OrdenCompra", new { id = ofertaId });
            }
            return RedirectToAction("Index", "Inicio");
        }
    }
}