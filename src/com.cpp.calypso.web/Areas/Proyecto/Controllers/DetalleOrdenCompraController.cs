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
    
    public class DetalleOrdenCompraController : BaseController
    {
        private readonly IOrdenCompraAsyncBaseCrudAppService _ordencompraService;
        private readonly IComputoAsyncBaseCrudAppService _computoService;
        private readonly IItemAsyncBaseCrudAppService _itemService;
        private readonly IDetalleOrdenCompraAsyncBaseCrudAppService _detalleordencompraService;
        private readonly IOfertaAsyncBaseCrudAppService _ofertaService;

        public DetalleOrdenCompraController(
            IHandlerExcepciones manejadorExcepciones,
            IDetalleOrdenCompraAsyncBaseCrudAppService detalleordencompraService,
            IOfertaAsyncBaseCrudAppService ofertaService,
            IOrdenCompraAsyncBaseCrudAppService ordencompraService,
            IComputoAsyncBaseCrudAppService computoService,
            IItemAsyncBaseCrudAppService itemService
        ) : base(manejadorExcepciones)
        {
            _detalleordencompraService = detalleordencompraService;
            _ordencompraService = ordencompraService;
            _ofertaService = ofertaService;
            _computoService = computoService;
            _itemService = itemService;
        }



        // GET: Proyecto/DetalleOrdenCompra
        public async Task<ActionResult> Index(int? id,String message ,int recargar = 0) // OrdenCompraId
        {

            if (id.HasValue)
            {
                if (message != null)
                {

                    ViewBag.Error = message;
                }
                if (recargar == 0)
                {
                    if (id > 0)
                    {
                        decimal total = _detalleordencompraService.calcularvalor(id.Value);
                    }

                    // _ordenServicioService.ActualizarMontosOrdenServicio(id.Value);
                    var orden = await _ordencompraService.Get(new EntityDto<int>(id.Value));
                    ViewBag.Id = id.Value;
                    ViewBag.CodigoProyecto = orden.Oferta.Proyecto.codigo;
                    var detalles = _detalleordencompraService.listar(id.Value);
                    IndexDetalleOrdenCompraViewModel viewModel = new IndexDetalleOrdenCompraViewModel()
                    {
                        OrdenCompraDto = orden,
                        DetalleOrdenServicioDto = detalles
                    };
                    return View(viewModel);
                }
                else
                {
                 //   _ordencompraService.ActualizarMontosOrdenServicio(id.Value);
                    return RedirectToAction("Index", "DetalleOrdenServicio", new { id = id.Value });
                }
            }

            return RedirectToAction("Index", "Inicio", new { area = "" });
        }

        // GET: Proyecto/DetalleOrdenCompra/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Proyecto/DetalleOrdenCompra/Create
        public async Task<ActionResult> Create(int? id,String message ) // OrdenCompraId
        {
            if (id.HasValue)
            {
                if (message != null)
                {

                    ViewBag.Msg = message;
                }

                var ordencompra = await _ordencompraService.Get(new EntityDto<int>(id.Value));
                var oferta = _ofertaService.getdetalle(ordencompra.OfertaId);
                ordencompra.Oferta = AutoMapper.Mapper.Map<Oferta>(oferta);

                var listaitems = _detalleordencompraService.GetComputos(ordencompra.OfertaId);


                DetalleOrdenCompraDto detalle = new DetalleOrdenCompraDto();
                
                   
                detalle.OrdenCompraId = id.Value;
                detalle.ItemsOrdenCompra = listaitems;
                detalle.vigente = true;
                detalle.fecha=DateTime.Now;
                detalle.OrdenCompra = AutoMapper.Mapper.Map<OrdenCompra>(ordencompra);

                return View(detalle);
            }
            return RedirectToAction("Index", "Inicio", new { area = "" });
        }


        [HttpPost]
        public async Task<ActionResult> Create(DetalleOrdenCompraDto detalle) // OrdenServicioId
        {
            
            if (ModelState.IsValid)


            {
                bool d = _detalleordencompraService.comprobarfechaitem(detalle.OrdenCompraId, detalle.ComputoId, detalle.fecha);
                detalle.estado = DetalleOrdenCompra.EstadoDetalleOrdenCompra.Registrado;
                        decimal porcentaje = _detalleordencompraService.porcentaje(detalle);

                        if ((porcentaje+detalle.porcentaje) > 100)
                        {
                            ViewBag.Error = "El ítem  tiene registrado un "+porcentaje+"%, el valor máximo a registrar es de "+(100-porcentaje)+"%";
                            var ordencompras = _ordencompraService.getdetalles(detalle.OrdenCompraId);
                            var ofertas = _ofertaService.getdetalle(ordencompras.OfertaId);
                            ordencompras.Oferta = AutoMapper.Mapper.Map<Oferta>(ofertas);
                            var listaitemss = _detalleordencompraService.GetComputos(ordencompras.OfertaId);
                            detalle.ItemsOrdenCompra = listaitemss;
                            detalle.OrdenCompra = AutoMapper.Mapper.Map<OrdenCompra>(ordencompras);
                            return View("Create", detalle);
                    }
                    else {
                            if (detalle.porcentaje == 0)
                            {
                              ViewBag.Error = "El porcentaje no puede ser 0%";
                                var ordencompras = _ordencompraService.getdetalles(detalle.OrdenCompraId);
                                var ofertas = _ofertaService.getdetalle(ordencompras.OfertaId);
                                ordencompras.Oferta = AutoMapper.Mapper.Map<Oferta>(ofertas);
                                var listaitemss = _detalleordencompraService.GetComputos(ordencompras.OfertaId);
                                detalle.ItemsOrdenCompra = listaitemss;
                                detalle.OrdenCompra = AutoMapper.Mapper.Map<OrdenCompra>(ordencompras);
                                return View("Create", detalle);
                    }
                    else {

                            var newDetalle = await _detalleordencompraService.Create(detalle);

                        //  _ordencompraService.ActualizarMontosOrdenServicio(newDetalle.OrdenServicioId);

                        decimal total = _detalleordencompraService.calcularvalor(detalle.OrdenCompraId);
                        string Mensaje = "Registro Creado ";
                        return RedirectToAction("Create",new {id=detalle.OrdenCompraId, message = Mensaje });
                    }
                }
               

            }
            var ordencompra = _ordencompraService.getdetalles(detalle.OrdenCompraId);
            var oferta = _ofertaService.getdetalle(ordencompra.OfertaId);
            ordencompra.Oferta = AutoMapper.Mapper.Map<Oferta>(oferta);
            var listaitems = _detalleordencompraService.GetComputos(ordencompra.OfertaId);
            detalle.ItemsOrdenCompra = listaitems;
            detalle.OrdenCompra = AutoMapper.Mapper.Map<OrdenCompra>(ordencompra);
            return View("Create", detalle);
        }

#pragma warning disable CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        public async Task<ActionResult> Edit(int? id) // DetalleOrdenServicioId
#pragma warning restore CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        {
            
            if (id.HasValue)
            {
                var detalle = _detalleordencompraService.GetDetalles(id.Value);
                var ordencompra = _ordencompraService.getdetalles(detalle.OrdenCompraId);
                var listaitems = _detalleordencompraService.GetComputos(ordencompra.OfertaId);
                var oferta = _ofertaService.getdetalle(ordencompra.OfertaId);
                ordencompra.Oferta = AutoMapper.Mapper.Map<Oferta>(oferta);
                detalle.ItemsOrdenCompra = listaitems;
                detalle.OrdenCompra =AutoMapper.Mapper.Map < OrdenCompra > (ordencompra);
                return View(detalle);
            }
            return RedirectToAction("Index", "Inicio", new { area = "" });
        }

        [HttpPost]
        public async Task<ActionResult> Edit(DetalleOrdenCompraDto detalle)
        {
            if (ModelState.IsValid)

                {
                    decimal porcentaje = _detalleordencompraService.porcentaje(detalle);

                    if (porcentaje > 100)
                    {
                        var ordencompras = _ordencompraService.getdetalles(detalle.OrdenCompraId);
                        var ofertas = _ofertaService.getdetalle(ordencompras.OfertaId);
                        ordencompras.Oferta = AutoMapper.Mapper.Map<Oferta>(ofertas);
                        var listaitemss = _detalleordencompraService.GetComputos(ordencompras.OfertaId);
                        detalle.ItemsOrdenCompra = listaitemss;
                        detalle.OrdenCompra = AutoMapper.Mapper.Map<OrdenCompra>(ordencompras);
                        return View("Edit", detalle);
                }
                    else
                    {
                        var updated = await _detalleordencompraService.InsertOrUpdateAsync(detalle);

                        //   _ordenServicioService.ActualizarMontosOrdenServicio(updated.OrdenServicioId);
                        decimal total = _detalleordencompraService.calcularvalor(detalle.OrdenCompraId);

                        return RedirectToAction("Index", "DetalleOrdenCompra", new { id = updated.OrdenCompraId });

                }

                   
            }
            var ordencompra = _ordencompraService.getdetalles(detalle.OrdenCompraId);
            var oferta = _ofertaService.getdetalle(ordencompra.OfertaId);
            ordencompra.Oferta = AutoMapper.Mapper.Map<Oferta>(oferta);
            var listaitems = _detalleordencompraService.GetComputos(ordencompra.OfertaId);
            detalle.ItemsOrdenCompra = listaitems;
            detalle.OrdenCompra = AutoMapper.Mapper.Map<OrdenCompra>(ordencompra);
            return View("Edit", detalle);
        }

        [HttpPost]
#pragma warning disable CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        public async Task<ActionResult> Delete(int id)
#pragma warning restore CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        {
            var detalle =  _detalleordencompraService.GetDetalles(id);
            if (detalle.estado == DetalleOrdenCompra.EstadoDetalleOrdenCompra.Aprobado)
            {
                string Mensaje = "No se puede Eliminar el Item, con estado aprobado, el item tiene datos Relacionados";
                return RedirectToAction("Index", "DetalleOrdenCompra", new { id = detalle.OrdenCompraId  , message = Mensaje });
            }
            else { 

            var ordenServicioId = _detalleordencompraService.Eliminar(id);
            decimal total = _detalleordencompraService.calcularvalor(detalle.OrdenCompraId);

            return RedirectToAction("Index", "DetalleOrdenCompra", new { id = detalle.OrdenCompraId });
            }
        }
    }
}