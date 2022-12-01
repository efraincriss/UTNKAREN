using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.framework;
using com.cpp.calypso.proyecto.aplicacion.Transporte.Dto;
using com.cpp.calypso.proyecto.aplicacion.Transporte.Interface;
using com.cpp.calypso.proyecto.dominio.Transporte;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace com.cpp.calypso.web.Areas.Transporte.Controllers
{
    public class RutaParadaController : BaseTransporteSpaController<RutaParada, RutaParadaDto, PagedAndFilteredResultRequestDto>
    {

        private readonly IRutaAsyncBaseCrudAppService _RutaService;
        private readonly IRutaParadaAsyncBaseCrudAppService _RutaParadaService;


        public RutaParadaController(
          IRutaAsyncBaseCrudAppService RutaService,
          IRutaParadaAsyncBaseCrudAppService RutaParadaService,
        IHandlerExcepciones manejadorExcepciones,
          IViewService viewService,
          IAsyncBaseCrudAppService<RutaParada, RutaParadaDto, PagedAndFilteredResultRequestDto, RutaParadaDto> entityService

          ) : base(manejadorExcepciones, viewService, entityService)
        {
            _RutaService = RutaService;
            _RutaParadaService = RutaParadaService;
        }

        // GET: Transporte/Chofer
        public ActionResult Index()
        {
            return View();
        }

        // GET: Transporte/Chofer/Details/5
        public ActionResult Details(int id)

        {
            var ruta = _RutaService.GetDetalles(id);
            return View(ruta);
        }

        // GET: Transporte/Chofer/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Transporte/Chofer/Create
        [HttpPost]
        public ActionResult Create(RutaParada ruta)
        {

            var rutaid = _RutaParadaService.Ingresar(ruta);
            return Content(rutaid > 0 ? "OK" : "ERROR");

        }

        // GET: Transporte/Chofer/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Transporte/Chofer/Edit/5
        [HttpPost]
        public ActionResult Edit(RutaParada ruta)
        {

            var rutaid = _RutaParadaService.Editar(ruta);
            return Content(rutaid > 0 ? "OK" : "ERROR");
        }

        // GET: Transporte/Chofer/Delete/5
        public ActionResult Delete()
        {
            return View();
        }

        // POST: Transporte/Chofer/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            var rutaid = _RutaParadaService.Eliminar(id);
            return Content(rutaid > 0 ? "OK" : "ERROR");
        }

    }
}