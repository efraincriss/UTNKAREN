using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Abp.Application.Services.Dto;
using com.cpp.calypso.framework;
using com.cpp.calypso.proyecto.aplicacion;
using com.cpp.calypso.proyecto.aplicacion.Dto;
using com.cpp.calypso.proyecto.aplicacion.Interfaces;
using com.cpp.calypso.proyecto.dominio;
using Newtonsoft.Json;

namespace com.cpp.calypso.web.Areas.Proyecto.Controllers
{
    
    public class DetalleAvanceIngenieriaController : BaseController
    {
        private readonly IDetalleAvanceIngenieriaAsyncBaseCrudAppService _detalleAvanceIngenieriaService;
        private readonly IAvanceIngenieriaAsyncBaseCrudAppService _avanceIngenieriaService;
        private readonly IComputoAsyncBaseCrudAppService _computoService;

        // GET: Proyecto/DetalleAvanceIngenieria
        public DetalleAvanceIngenieriaController(
            IHandlerExcepciones manejadorExcepciones,
            IDetalleAvanceIngenieriaAsyncBaseCrudAppService detalleAvanceIngenieriaService,
            IAvanceIngenieriaAsyncBaseCrudAppService avanceIngenieriaService,
            IComputoAsyncBaseCrudAppService computoService
            ) : base(manejadorExcepciones)
        {
            _detalleAvanceIngenieriaService = detalleAvanceIngenieriaService;
            _avanceIngenieriaService = avanceIngenieriaService;
            _computoService = computoService;
        }

        public ActionResult Index()
        {
            return View();
        }

        // GET: Proyecto/DetalleAvanceIngenieria/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id.HasValue)
            {
                var detalle = await _detalleAvanceIngenieriaService.Get(new EntityDto<int>(id.Value));
                var result = JsonConvert.SerializeObject(detalle);
                return Content(result);
            }
            return Content("Error");
        }

        // GET: Proyecto/DetalleAvanceIngenieria/Create
        public ActionResult Create()
        {
            return View();
        }



        // POST: Proyecto/DetalleAvanceIngenieria/Create
        [HttpPost]
        public async Task<ActionResult> Create(DetalleAvanceIngenieriaDto detalle)
        {

            if (ModelState.IsValid)
            {
                var computo = await _computoService.Get(new EntityDto<int>(detalle.ComputoId));
                var avance = await _avanceIngenieriaService.Get(new EntityDto<int>(detalle.AvanceIngenieriaId));
                detalle.valor_real = detalle.cantidad_horas * computo.precio_unitario;
                detalle.cantidad_acumulada_anterior = _detalleAvanceIngenieriaService.ObtenerCantidadAcumulada(detalle.ComputoId, avance.fecha_presentacion, avance.OfertaId);
                detalle.cantidad_acumulada = detalle.cantidad_acumulada_anterior + detalle.cantidad_horas;
                var d = await _detalleAvanceIngenieriaService.InsertOrUpdateAsync(detalle);
                decimal total = _detalleAvanceIngenieriaService.calcularvalor(avance.Id);
                return Content("Ok");
            }

            return Content("ErrorValidation");
        }



        // GET: Proyecto/DetalleAvanceIngenieria/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Proyecto/DetalleAvanceIngenieria/Edit/5
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

        // GET: Proyecto/DetalleAvanceIngenieria/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }




        // POST: Proyecto/DetalleAvanceIngenieria/Delete/5
        [HttpPost]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id.HasValue)
            {
                var item = await _detalleAvanceIngenieriaService.Get(new EntityDto<int>(id.Value));
                item.vigente = false;
                await _detalleAvanceIngenieriaService.Update(item);
                decimal total = _detalleAvanceIngenieriaService.calcularvalor(item.AvanceIngenieriaId);
                return Content("Ok");
            }
            return Content("ErrorId");
        }



        [HttpPost]
        public ActionResult GetDetallesAvanceIngenieriaApi(int id)
        {
            var avances = _detalleAvanceIngenieriaService.ListarPorAvanceIngenieria(id);
            var result = JsonConvert.SerializeObject(avances);
            return Content(result);
        }

        [HttpPost]
        public ActionResult GetComputosIngenieriaApi(int id) //OfertaId
        {
            var computos = _detalleAvanceIngenieriaService.GetComputos(id);
            var result = JsonConvert.SerializeObject(computos);
            return Content(result);
        }
    }
}
