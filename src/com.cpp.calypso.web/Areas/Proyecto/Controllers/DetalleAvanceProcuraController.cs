using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using com.cpp.calypso.framework;
using com.cpp.calypso.proyecto.aplicacion;
using com.cpp.calypso.proyecto.aplicacion.Interfaces;
using com.cpp.calypso.proyecto.dominio;
using Newtonsoft.Json;

namespace com.cpp.calypso.web.Areas.Proyecto.Controllers
{
    

    public class DetalleAvanceProcuraController : BaseController
    {
        private readonly IOrdenCompraAsyncBaseCrudAppService _ordencompraService;
        private readonly IDetalleOrdenCompraAsyncBaseCrudAppService _detalleordencompraService;
        private readonly IAvanceProcuraAsyncBaseCrudAppService _avanceProcuraService;
        private readonly IDetalleAvanceProcuraAsyncBaseCrudAppService _detalleavanceProcuraService;
        private readonly IOfertaAsyncBaseCrudAppService _ofertaService;
        private readonly IWbsOfertaAsyncBaseCrudAppService _wbsService;
        private readonly IComputoAsyncBaseCrudAppService _computoService;
        private readonly IItemAsyncBaseCrudAppService _itemService;

        private readonly IDetalleAvanceIngenieriaAsyncBaseCrudAppService _davanceingenieria;

        public DetalleAvanceProcuraController(
            IHandlerExcepciones manejadorExcepciones,
            IAvanceProcuraAsyncBaseCrudAppService avanceProcuraService,
            IOfertaAsyncBaseCrudAppService ofertaService,
            IWbsOfertaAsyncBaseCrudAppService wbsService,
            IComputoAsyncBaseCrudAppService computoService,
            IDetalleAvanceProcuraAsyncBaseCrudAppService detalleavanceProcuraService,
            IItemAsyncBaseCrudAppService itemService,
            IDetalleAvanceIngenieriaAsyncBaseCrudAppService davanceingenieria,
            IOrdenCompraAsyncBaseCrudAppService ordencompraService,
            IDetalleOrdenCompraAsyncBaseCrudAppService detalleordencompraService

        ) : base(manejadorExcepciones)
        {
            _avanceProcuraService = avanceProcuraService;
            _ofertaService = ofertaService;
            _wbsService = wbsService;
            _itemService = itemService;
            _computoService = computoService;
            _detalleavanceProcuraService = detalleavanceProcuraService;
            _davanceingenieria = davanceingenieria;
            _ordencompraService = ordencompraService;
            _detalleordencompraService = detalleordencompraService;
        }

        // GET: Proyecto/DetalleAvanceProcura
        public ActionResult Index()
        {
            return View();
        }

        // GET: Proyecto/DetalleAvanceProcura/Details/5
        public ActionResult Details(int id)
        {
            var detalle = _detalleavanceProcuraService.getdetalles(id);
            return View(detalle);
        }

        // GET: Proyecto/DetalleAvanceProcura/Create
        public ActionResult Create(int? id) // AvanceProcuraId
        {
            if (id.HasValue)
            {
                var avanceprocura = _avanceProcuraService.getdetalles(id.Value);
                var computos = _detalleavanceProcuraService.GetComputos(avanceprocura.OfertaId);
                var ordenescompra = _ordencompraService.listar(avanceprocura.OfertaId);

                var avance = new DetalleAvanceProcuraDto()
                {
                    fecha_real = DateTime.Now,
                    vigente = true,
                    ItemsOrdenCompra = computos,
                    AvanceProcuraId = id.Value,
                    estado = DetalleAvanceProcura.EstadoDetalleProcura.Registrado,
                    OrdenesCompra = ordenescompra
                };
                avance.AvanceProcura = AutoMapper.Mapper.Map<AvanceProcura>(avanceprocura);
                return View(avance);
            }

            return RedirectToAction("Index", "Inicio", new {area = ""});
        }


        // POST: Proyecto/DetalleAvanceProcura/Create
        [System.Web.Mvc.HttpPost]
        public async Task<ActionResult> Create(DetalleAvanceProcuraDto avance)
        {
            avance.Id = 0;
            var detalle = _detalleordencompraService.GetDetalles(avance.DetalleOrdenCompraId);
            var computo = await _computoService.GetDetalle(detalle.ComputoId);


            if (ModelState.IsValid)
            {
                avance.precio_unitario = computo.precio_unitario;
          
                decimal totalw = avance.cantidad * avance.precio_unitario;
                avance.valor_real = totalw;
                avance.DetalleOrdenCompraId = detalle.Id;
                var dto = await _detalleavanceProcuraService.Create(avance);
                decimal total = _detalleavanceProcuraService.calcularvalor(avance.AvanceProcuraId);

                bool r = _detalleordencompraService.PasarOrdenAprobado(detalle.Id);
                return RedirectToAction("Details", "AvanceProcura", new {id = dto.AvanceProcuraId});
            }

            var avanceprocura = _avanceProcuraService.getdetalles(avance.AvanceProcuraId);
            avance.AvanceProcura = AutoMapper.Mapper.Map<AvanceProcura>(avanceprocura);
            var computos = _detalleavanceProcuraService.GetComputos(avanceprocura.OfertaId);
            avance.ItemsOrdenCompra = computos;
            var ordenescompra = _ordencompraService.listar(avanceprocura.OfertaId);
            avance.OrdenesCompra = ordenescompra;
            return View("Create", avance);
        }

        // GET: Proyecto/DetalleAvanceProcura/Edit/5
        public ActionResult Edit(int id)
        {

            var davance = _detalleavanceProcuraService.getdetalles(id);
            var avance = _avanceProcuraService.getdetalles(davance.AvanceProcuraId);
            var oferta = _ofertaService.getdetalle(avance.OfertaId);
            var computos = _detalleavanceProcuraService.GetComputos(avance.OfertaId);
            var computo = _computoService.GetDetalle(davance.DetalleOrdenCompra.ComputoId);
            davance.DetalleOrdenCompra.Computo = davance.DetalleOrdenCompra.Computo;
            davance.ItemsOrdenCompra = computos;
            davance.AvanceProcura = AutoMapper.Mapper.Map<AvanceProcura>(avance);
            davance.Oferta = AutoMapper.Mapper.Map<Oferta>(oferta);


            return View(davance);
        }

        // POST: Proyecto/DetalleAvanceProcura/Edit/5
        [System.Web.Mvc.HttpPost]
        public async Task<ActionResult> Edit(DetalleAvanceProcuraDto avance)
        {
            var avancep = _avanceProcuraService.getdetalles(avance.AvanceProcuraId);
            var computos = _detalleavanceProcuraService.GetComputos(avancep.OfertaId);
            if (ModelState.IsValid)
            {
                avance.precio_unitario = avance.DetalleOrdenCompra.Computo.precio_unitario;
                decimal totalw = avance.cantidad * avance.precio_unitario;
                avance.valor_real = totalw;
                var result = await _detalleavanceProcuraService.InsertOrUpdateAsync(avance);
                decimal total = _detalleavanceProcuraService.calcularvalor(avance.AvanceProcuraId);
                return RedirectToAction("Details", "AvanceProcura", new {id = result.AvanceProcuraId});
            }

            avance.ItemsOrdenCompra = computos;
            var avance2 = _avanceProcuraService.getdetalles(avance.AvanceProcuraId);
            var oferta = _ofertaService.getdetalle(avance2.OfertaId);
            avance.Oferta = AutoMapper.Mapper.Map<Oferta>(oferta);
            return View(avance);
        }


        // POST: Proyecto/DetalleAvanceProcura/Delete/5
        [System.Web.Mvc.HttpPost]
        public ActionResult Delete(int? id)
        {
            var davance = _detalleavanceProcuraService.getdetalles(id.Value);
            if (id.HasValue)
            {
                var ofertaId = _detalleavanceProcuraService.EliminarVigencia(id.Value);
             
                bool r = _detalleordencompraService.PasarOrdenRegistrado(davance.DetalleOrdenCompraId);
                decimal total = _detalleavanceProcuraService.calcularvalor(davance.AvanceProcuraId);
                return RedirectToAction("Details", "AvanceProcura", new {id = davance.AvanceProcuraId});
            }

            return RedirectToAction("Index", "Inicio");
        }

        public async Task<ActionResult> DetalleComputo(int id)
        {

            var computo = await _computoService.GetDetalle(id);
            if (computo.cantidad == 0 && computo.cantidad_eac > 0)
            {
                computo.cantidad = computo.cantidad_eac;
            }

            var resultado = JsonConvert.SerializeObject(computo,
                Newtonsoft.Json.Formatting.None,
                new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });
            return Content(resultado);
        }

        public async Task<ActionResult> DetalleComputo2(int id)
        {
            var detalle = _detalleordencompraService.GetDetalles(id);

            var computo = await _computoService.GetDetalle(detalle.ComputoId);

            var resultado = JsonConvert.SerializeObject(computo,
                Newtonsoft.Json.Formatting.None,
                new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });
            return Content(resultado);
        }

        public ActionResult ObtenerComputosOrdenes(int id)
        {
            var computosordenes = _detalleordencompraService.GetComputosOrdenescompra(id);


            var resultado = JsonConvert.SerializeObject(computosordenes,
                Newtonsoft.Json.Formatting.None,
                new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });
            return Content(resultado);
        }

        public ActionResult GetDetallesOrdenes(int id) //Ofertaid
        {
            var computosordenes = _detalleordencompraService.listarporoferta(id);


            var resultado = JsonConvert.SerializeObject(computosordenes,
                Newtonsoft.Json.Formatting.None,
                new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });
            return Content(resultado);
        }
        [System.Web.Mvc.HttpPost]
        public async Task<ActionResult> CrearDetalles([FromBody] int[] detallesseleccionados, [FromBody] int  AvanceProcuraId)
        {
          
            foreach (var i in detallesseleccionados)
            {
                var detalleorde = _detalleordencompraService.GetDetalles(i);

                decimal cantidadanterior = _detalleavanceProcuraService.obtenercalculoanterior(detalleorde.ComputoId);


                DetalleAvanceProcuraDto n = new DetalleAvanceProcuraDto
                {
                    Id = 0,
                    AvanceProcuraId = AvanceProcuraId,
                    DetalleOrdenCompraId = i,
                    cantidad = detalleorde.Computo.cantidad*(detalleorde.porcentaje/100),//cantidad calculada 
                    calculo_diario = detalleorde.valor, //cantidad diaria la que ingresa
                    calculo_anterior = cantidadanterior, // sumaria de la cantidades anteriores
                    ingreso_acumulado = detalleorde.valor+cantidadanterior, //diara + acumulado
                    precio_unitario = detalleorde.Computo.precio_unitario, // precio unitario
                    estado = DetalleAvanceProcura.EstadoDetalleProcura.Registrado,
                    vigente = true,
                    fecha_real = DateTime.Now,
                   
                };
               n.valor_real = n.cantidad * n.precio_unitario;

                var result = await _detalleavanceProcuraService.Create(n);
                bool r = _detalleordencompraService.PasarOrdenAprobado(i);
                decimal total = _detalleavanceProcuraService.calcularvalor(AvanceProcuraId);

            }
            return Content("OK");
        }
    }
}

