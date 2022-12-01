using System;
using System.Collections.Generic;
using System.IO;
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
      
    public class AvanceProcuraController : BaseController
    {

        private readonly IAvanceProcuraAsyncBaseCrudAppService _avanceProcuraService;
        private readonly IProyectoAsyncBaseCrudAppService _proyectoService;
        private readonly IDetalleAvanceProcuraAsyncBaseCrudAppService _detalleavanceProcuraService;
        private readonly IOfertaAsyncBaseCrudAppService _ofertaService;
        private readonly IWbsOfertaAsyncBaseCrudAppService _wbsService;
        private readonly IComputoAsyncBaseCrudAppService _computoService;
        // GET: Proyecto/AvanceIngenieria
        public AvanceProcuraController(
            IHandlerExcepciones manejadorExcepciones,
            IAvanceProcuraAsyncBaseCrudAppService avanceProcuraService,
            IOfertaAsyncBaseCrudAppService ofertaService,
            IWbsOfertaAsyncBaseCrudAppService wbsService,
            IComputoAsyncBaseCrudAppService computoService,
        IDetalleAvanceProcuraAsyncBaseCrudAppService detalleavanceProcuraService,
            IProyectoAsyncBaseCrudAppService proyectoService
        ) : base(manejadorExcepciones)
        {
            _avanceProcuraService = avanceProcuraService;
            _ofertaService = ofertaService;
            _wbsService = wbsService;
            _computoService = computoService;
            _detalleavanceProcuraService = detalleavanceProcuraService;
            _proyectoService = proyectoService;
        }
        public ActionResult IndexProcura()
        {

            var ofertas = _ofertaService.GetOfertasDefinitivas();
            ViewBag.OfertaId = 1;
            return View(ofertas);

        }
        // GET: Proyecto/AvanceProcura
        public ActionResult Index(int? id)
        {
            if (id.HasValue)
            {
                var oferta = _ofertaService.getdetalle(id.Value);
                var lista = _avanceProcuraService.ListarPorOferta(id.Value);
                ViewBag.Id = id.Value;
                OfertaAvanceProcuraViewModel n = new OfertaAvanceProcuraViewModel
                {
                    oferta = oferta,
                    listaavances = lista

                };
                ViewBag.Id = id.Value;
                ViewBag.ofertaid = id.Value;
                return View(n);
   
            }

            return RedirectToAction("Index", "Inicio");
        }
        // GET: Proyecto/AvanceProcura/Details/5
#pragma warning disable CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        public async Task<ActionResult> Details(int? id)
#pragma warning restore CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        {
            if (id.HasValue)
            {
                decimal total = _detalleavanceProcuraService.calcularvalor(id.Value);
                var avance =  _avanceProcuraService.getdetalles(id.Value);
                var lista = _detalleavanceProcuraService.listarporavanceprocura(id.Value);

                var proyecto =  _proyectoService.GetDetalles(avance.Oferta.ProyectoId);
                ViewBag.ContratoId = proyecto.contratoId;
                ViewBag.Fecha = avance.Oferta.fecha_oferta;
                DetalleAvanceProcuraViewModel avance2 = new DetalleAvanceProcuraViewModel
                {
                    AvanceProcutProcuraDto = avance,
                    DetalleAvanceProcuraDto = lista
                };
               ViewBag.montoactual = _detalleavanceProcuraService.montoactual(avance.Id);
               ViewBag.montoanterior= _detalleavanceProcuraService.montoanteriores(avance.Id);
                ViewBag.montopresupuestado = _detalleavanceProcuraService.montopresupuesto(avance.Id);
                return View(avance2);
            }

            return RedirectToAction("Index", "Inicio", new { area = "" });
        }

        // GET: Proyecto/AvanceProcura/Create
        public ActionResult Create(int? id) // OfertaId
        {
            if (id.HasValue)
            {
                var oferta = _ofertaService.getdetalle(id.Value);
                var avance = new AvanceProcuraDto()
                {
                    fecha_presentacion = DateTime.Now,
                    fecha_desde = DateTime.Now,
                    fecha_hasta = DateTime.Now,
                };
                avance.OfertaId = id.Value;
            avance.Oferta = AutoMapper.Mapper.Map<Oferta>(oferta);
                return View(avance);
            }

            return RedirectToAction("Index", "Inicio", new { area = "" });
        }

        // POST: Proyecto/AvanceProcura/Create
        [HttpPost]
        public async Task<ActionResult> Create(AvanceProcuraDto avance)
        {
            if (ModelState.IsValid)
              {

            

                        var dto = await _avanceProcuraService.Create(avance);
                        return RedirectToAction("Index", "AvanceProcura", new { id = avance.OfertaId });

                    }

            return View("Create", avance);
        }

        // GET: Proyecto/AvanceProcura/Edit/5
        public ActionResult Edit(int id)
        {
            var avance = _avanceProcuraService.getdetalles(id);
            return View(avance);
        }

        // POST: Proyecto/AvanceProcura/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(AvanceProcuraDto avance)
        {
            if (ModelState.IsValid)
            {
                bool r = _avanceProcuraService.comprobarfecha(avance.fecha_desde, avance.fecha_hasta);
                if (r)
                {
                    bool s = _avanceProcuraService.comprobarfecha(avance.fecha_hasta, avance.fecha_presentacion);

                    if (s)
                    {
                        var result = await _avanceProcuraService.Update(avance);
                        return RedirectToAction("Index", "AvanceProcura", new { id = avance.OfertaId });
                    }
                    else
                    {
                        var oferta = _ofertaService.getdetalle(avance.OfertaId);
                        avance.Oferta = AutoMapper.Mapper.Map<Oferta>(oferta);
                        ViewBag.Msg = "la fecha presentacion debe ser mayor  la fechas";
                        return View("Edit", avance);

                    }


                }
                else
                {
                    var oferta = _ofertaService.getdetalle(avance.OfertaId);
                    avance.Oferta = AutoMapper.Mapper.Map<Oferta>(oferta);
                    ViewBag.Msg = "La fecha hasta debe ser mayor a la fecha desde";
                    return View("Edit", avance);

                }
            
            }
            return View(avance);
        }

        // POST: Proyecto/AvanceProcura/Delete/5
        [HttpPost]
        public ActionResult Delete(int ?id )
        {

            var davance = _avanceProcuraService.getdetalles(id.Value);
            if (id.HasValue)
            {
                var ofertaId = _avanceProcuraService.Eliminar(id.Value);
                decimal total = _detalleavanceProcuraService.calcularvalor(davance.Id);
                return RedirectToAction("Index", "AvanceProcura", new { id = davance.OfertaId });
            }
            return RedirectToAction("Index", "Inicio");
        }
        public async Task<ActionResult> CertificadoProcura(int id) //Id OfertaId
        {

            var excel = _avanceProcuraService.ObtenerCertificadoProcura(id);

            string excelName = "Excel Procura";
            using (var memoryStream = new MemoryStream())
            {
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment; filename=" + excelName + ".xlsx");
                excel.SaveAs(memoryStream);
                memoryStream.WriteTo(Response.OutputStream);
                Response.Flush();
                Response.End();
                return Content("");
            }
        }


    }
}
