using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using com.cpp.calypso.framework;
using com.cpp.calypso.proyecto.aplicacion;
using com.cpp.calypso.proyecto.aplicacion.Interfaces;
using com.cpp.calypso.web.Areas.Proyecto.Models;

namespace com.cpp.calypso.web.Areas.Proyecto.Controllers
{

    public class GananciaController : BaseController
    {
        private readonly IContratoAsyncBaseCrudAppService _contratoService;
        private readonly IGananciaAsyncBaseCrudAppService _gananciaService;
        private readonly IDetalleGananciaAsyncBaseCrudAppService _detallegananciaService;
        public GananciaController(IHandlerExcepciones manejadorExcepciones,
            IGananciaAsyncBaseCrudAppService gananciaService,
            IDetalleGananciaAsyncBaseCrudAppService detallegananciaService,
            IContratoAsyncBaseCrudAppService contratoService
            ) : base(manejadorExcepciones)
        {
            _gananciaService = gananciaService;
            _detallegananciaService = detallegananciaService;
            _contratoService = contratoService;
        }


        // GET: Proyecto/Ganancia
        public ActionResult Index()
        {
            var ganancias = _gananciaService.GetGanacias();

            return View(ganancias);
        }

        // GET: Proyecto/Ganancia/Details/5
        public ActionResult Details(int? id)
        {
            if (id.HasValue)
            {
                var ganancia = _gananciaService.GetDetalle(id.Value);

                if (ganancia != null)
                {
                    var detalles = _detallegananciaService.ListarporGanancia(id.Value);
                    GananciaDetalleViewModel n = new GananciaDetalleViewModel
                    {
                        Ganancia = ganancia,
                        DetallesGanancia = detalles
                    };
                    return View(n);


                }
            }
            return RedirectToAction("Index", "Contrato");
        }


        // GET: Proyecto/Ganancia/Create
        public ActionResult Create()
        {
            
            GananciaDto nuevo = new GananciaDto();
            nuevo.fecha_inicio = DateTime.Now;
            nuevo.estado_ganacia = true;
            nuevo.ListaContratos = _contratoService.GetContratos();
            return View(nuevo);


        }

        // POST: Proyecto/Ganancia/Create
        [HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> Create(GananciaDto ganancia)
        {

            if (ModelState.IsValid)
            {


                ganancia.vigente = true;
                var validacion = _gananciaService.ValidacionesGanancia(ganancia);
                if (validacion != "OK") {
                    ViewBag.Error = validacion;
                    ganancia.ListaContratos = _contratoService.GetContratos();
                    return View("Create", ganancia);
                }


                var resultado = await _gananciaService.Create(ganancia);

                return RedirectToAction("Index", "Ganancia");
            }
            else
            {
                return View("Create", ganancia);

            }

        }
        // GET: Proyecto/Ganancia/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id.HasValue)
            {
                var ganancia = _gananciaService.GetDetalle(id.Value);
                return View(ganancia);
            }

            return RedirectToAction("Index", "Contrato");
        }


        [System.Web.Mvc.HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> Edit(GananciaDto ganancia)
        {
            if (ModelState.IsValid)
            {
                await _gananciaService.Update(ganancia);
                return RedirectToAction("Index", "Ganancia");
            }

            return View("Edit", ganancia);
        }

        // GET: Proyecto/Ganancia/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Proyecto/Ganancia/Delete/5
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
