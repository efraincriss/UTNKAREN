using Abp.Application.Services.Dto;
using com.cpp.calypso.framework;
using com.cpp.calypso.proyecto;
using com.cpp.calypso.proyecto.aplicacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace com.cpp.calypso.web.Areas.Proyecto.Controllers
{
    
    public class HistoricosOfertaController : BaseController
    {
        private readonly IHistoricosOfertaAsyncBaseCrudAppService _historicosOfertaService;

        public HistoricosOfertaController(
            IHandlerExcepciones manejadorExcepciones,
            IHistoricosOfertaAsyncBaseCrudAppService historicosOfertaService
            ) : base(manejadorExcepciones)
        {
            _historicosOfertaService = historicosOfertaService;
        }

        // GET: Proyecto/CreateHistorico
        public ActionResult CreateHistorico(HistoricosOferta historicosOferta, int IdestadoTemp, int estado = 0)
        {
            historicosOferta.fecha = DateTime.Today;
            historicosOferta.vigente = true;
            _historicosOfertaService.CreateHistoricoOferta(historicosOferta, estado, IdestadoTemp);
            return RedirectToAction("Details", "Oferta", new { id = historicosOferta.OfertaId });
        }

        // GET: Proyecto/HistoricosOferta
        public ActionResult Index()
        {
            return View();
        }

        // GET: Proyecto/HistoricosOferta/Details/5
        public ActionResult Details(int id)
        {
            var historico = _historicosOfertaService.GetHistoricosOferta(id);
            return View(historico);
        }

        // GET: Proyecto/HistoricosOferta/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Proyecto/HistoricosOferta/Create
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

        // GET: Proyecto/HistoricosOferta/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Proyecto/HistoricosOferta/Edit/5
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

        // GET: Proyecto/HistoricosOferta/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult Delete(int? id)
        {
            if (id.HasValue)
            {
                var requerimientoId = _historicosOfertaService.EliminarVigencia(id.Value);
                return RedirectToAction("Details", "OFerta", new { id = requerimientoId });
            }

            return RedirectToAction("Index", "Proyecto");
        }
    }
}
