using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using com.cpp.calypso.framework;
using com.cpp.calypso.proyecto.aplicacion;
using com.cpp.calypso.proyecto.aplicacion.Interfaces;
using Newtonsoft.Json;

namespace com.cpp.calypso.web.Areas.Proyecto.Controllers
{
    
    public class DetalleGananciaController : BaseController
    {
        private readonly IGananciaAsyncBaseCrudAppService _gananciaService;
        private readonly IDetalleGananciaAsyncBaseCrudAppService _detallegananciaService;
        private readonly IPorcentajeIncrementoAsyncBaseCrudAppService _porcentajeincrementoService;
        private readonly IGrupoItemAsyncBaseCrudAppService _grupoitemService;
        public DetalleGananciaController(IHandlerExcepciones manejadorExcepciones,
            IGananciaAsyncBaseCrudAppService gananciaService,
            IDetalleGananciaAsyncBaseCrudAppService detallegananciaService,
            IPorcentajeIncrementoAsyncBaseCrudAppService porcentajeincrementoService,
            IGrupoItemAsyncBaseCrudAppService grupoitemService
            ) : base(manejadorExcepciones)
        {
            _gananciaService = gananciaService;
            _detallegananciaService = detallegananciaService;
            _porcentajeincrementoService = porcentajeincrementoService;
            _grupoitemService = grupoitemService;
        }

        // GET: Proyecto/DetalleGanancia
        public ActionResult Index()
        {
            return View();
        }

        // GET: Proyecto/DetalleGanancia/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Proyecto/DetalleGanancia/Create
        public ActionResult Create(int Id)
            
        {
            var porcentajes = _porcentajeincrementoService.Listar();
            var grupos = _grupoitemService.lista();
            DetalleGananciaDto n = new DetalleGananciaDto
            {
               GananciaId = Id,
               PorcentajesIncremento = porcentajes,
                GruposItem = grupos
            };

            return View(n);
        }

        // POST: Proyecto/DetalleGanancia/Create
        [HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> Create( DetalleGananciaDto n )
        {
            if (ModelState.IsValid)
            {
                n.vigente = true;
                var r = await _detallegananciaService.Create(n);
                return RedirectToAction("Details", "Ganancia", new {id = r.GananciaId});
            }

            return RedirectToAction("Index");
          
        }

        // GET: Proyecto/DetalleGanancia/Edit/5
        public ActionResult Edit(int id)
        {
            var porcentajes = _porcentajeincrementoService.Listar();
            var grupos = _grupoitemService.lista();
            var n = _detallegananciaService.getdetalle(id);
            n.PorcentajesIncremento = porcentajes;
            n.GruposItem = grupos;
            return View(n);
        }

        // POST: Proyecto/DetalleGanancia/Edit/5
        [HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> Edit( DetalleGananciaDto n)
        {
            if (ModelState.IsValid)
            {
                var r = await _detallegananciaService.Update(n);
                return RedirectToAction("Details", "Ganancia", new { id = r.GananciaId });
            }

            return RedirectToAction("Index", "Ganancia");

        }


        [HttpPost]
        public ActionResult Delete(int id)
        {
            var n = _detallegananciaService.getdetalle(id);
            var d = _detallegananciaService.Eliminar(id);
            if (d)
            {
                return RedirectToAction("Details", "Ganancia", new { id = n.GananciaId });
            }
            return RedirectToAction("Index", "Ganancia");
        }

        public ActionResult ObtenerValorPorcentaje(int id)
        {
           var porcentaje = _porcentajeincrementoService.getdetalle(id);


            var resultado = JsonConvert.SerializeObject(porcentaje,
                Newtonsoft.Json.Formatting.None,
                new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });
            return Content(resultado);
        }
    }
}
