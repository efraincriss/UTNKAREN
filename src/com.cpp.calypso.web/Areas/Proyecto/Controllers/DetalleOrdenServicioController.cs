using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using Abp.Application.Services.Dto;
using com.cpp.calypso.framework;
using com.cpp.calypso.proyecto.aplicacion;
using com.cpp.calypso.proyecto.aplicacion.Interfaces;
using com.cpp.calypso.web.Areas.Proyecto.Models;

namespace com.cpp.calypso.web.Areas.Proyecto.Controllers
{
    public class DetalleOrdenServicioController : BaseController
    {
        private readonly IDetalleOrdenServicioAsyncBaseCrudAppService _detalleOrdenServicioService;
        private readonly IOrdenServicioAsyncBaseCrudAppService _ordenServicioService;
        private readonly IOfertaComercialPresupuestoAsyncBaseCrudAppService _ofertacomercialPresupuestoService;

        // GET: Proyecto/DetalleOrdenServicio
        public DetalleOrdenServicioController(
            IHandlerExcepciones manejadorExcepciones,
            IDetalleOrdenServicioAsyncBaseCrudAppService detalleOrdenServicioService,
            IOrdenServicioAsyncBaseCrudAppService ordenServicioService,
            IOfertaComercialPresupuestoAsyncBaseCrudAppService ofertacomercialPresupuestoService
        ) : base(manejadorExcepciones)
        {
            _detalleOrdenServicioService = detalleOrdenServicioService;
            _ordenServicioService = ordenServicioService;
            _ofertacomercialPresupuestoService = ofertacomercialPresupuestoService;
        }

        public async Task<ActionResult> Index(int? id, int recargar = 0,int principal=0) // OrdenServicioId
        {
            ViewBag.ruta = new string[] { "Inicio", "Ofertas Comerciales", "Ordenes de Servicio","Detalles" };
            if (id.HasValue)
            {
                if (recargar == 0)
                {
                    _ordenServicioService.ActualizarMontosOrdenServicio(id.Value);
                    var orden = await _ordenServicioService.Get(new EntityDto<int>(id.Value));
                    ViewBag.Id = id.Value;
                 //   ViewBag.OfertaId = orden.OfertaComercialId;
                    ViewBag.Principal = principal;
                   /* ViewBag.CodigoProyecto = orden.Oferta.Proyecto.codigo;*/
                    var detalles = _detalleOrdenServicioService.listar(id.Value);
                    IndexDetalleOrdenServicioViewModel viewModel = new IndexDetalleOrdenServicioViewModel()
                    {
                        OrdenServicioDto = orden,
                        DetalleOrdenServicioDto = detalles
                    };

                    return View(viewModel);
                }
                else
                {
                    _ordenServicioService.ActualizarMontosOrdenServicio(id.Value);
                    return RedirectToAction("Index", "DetalleOrdenServicio", new { id = id.Value });
                }
            }

            return RedirectToAction("Index", "Inicio", new { area = "" });
        }


        public ActionResult Create(int? id) // OrdenServicioId
        {
            ViewBag.ruta = new string[] { "Inicio", "Ofertas Comerciales", "Ordenes de Servicio", "Detalles","Nuevo" };
            if (id.HasValue)
            {
                var ordenservicio = _ordenServicioService.Detalles(id.Value);
                var detalle = new DetalleOrdenServicioDto();
                detalle.OrdenServicioId = id.Value;
              //  detalle.listaproyectos = _ofertacomercialPresupuestoService.ListadoProyectos(ordenservicio.OfertaComercialId);


                return View(detalle);
            }
            return RedirectToAction("Index", "Inicio", new { area = "" });
        }


        [HttpPost]
        public async Task<ActionResult> Create(DetalleOrdenServicioDto detalle) // OrdenServicioId
        {
            if (ModelState.IsValid)
            {
                var newDetalle = await _detalleOrdenServicioService.Create(detalle);
               
                _ordenServicioService.ActualizarMontosOrdenServicio(newDetalle.OrdenServicioId);
                
                return RedirectToAction("Index", "DetalleOrdenServicio", new {id = newDetalle.OrdenServicioId});
            }

            return View("Create", detalle);
        }

        public ActionResult Edit(int? id) // DetalleOrdenServicioId
        {
            ViewBag.ruta = new string[] { "Inicio", "Ofertas Comerciales", "Ordenes de Servicio", "Detalles", "Edición" };
            if (id.HasValue)
            {
                var detalle = _detalleOrdenServicioService.GetDetalles(id.Value);
                var ordenservicio = _ordenServicioService.Detalles(detalle.OrdenServicioId);
               // detalle.listaproyectos= _ofertacomercialPresupuestoService.ListadoProyectos(ordenservicio.OfertaComercialId);

                return View(detalle);
            }
            return RedirectToAction("Index", "Inicio", new { area = "" });
        }

        [HttpPost]
        public async Task<ActionResult> Edit(DetalleOrdenServicioDto detalle)
        {
            if (ModelState.IsValid)
            {
                var updated = await _detalleOrdenServicioService.InsertOrUpdateAsync(detalle);
               
                _ordenServicioService.ActualizarMontosOrdenServicio(updated.OrdenServicioId);
                
                return RedirectToAction("Index", "DetalleOrdenServicio", new {id = updated.OrdenServicioId});
            }

            return View("Edit", detalle);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var ordenServicioId =  _detalleOrdenServicioService.Eliminar(id);
            
            return RedirectToAction("Index", "DetalleOrdenServicio", new {id = ordenServicioId, recargar = 1});
        }
    }
}