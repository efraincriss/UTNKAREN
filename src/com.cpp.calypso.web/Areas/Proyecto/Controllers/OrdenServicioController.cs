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
using com.cpp.calypso.proyecto.dominio.Constantes;
using com.cpp.calypso.web.Areas.Proyecto.Models;
using Newtonsoft.Json;

namespace com.cpp.calypso.web.Areas.Proyecto.Controllers
{
  
    public class OrdenServicioController : BaseController
    {
        private readonly IOrdenServicioAsyncBaseCrudAppService _ordenServicioService;
        private readonly IArchivoAsyncBaseCrudAppService ArchivoService;
        private readonly ISecuencialAsyncBaseCrudAppService _secuencialService;
        private readonly IOfertaAsyncBaseCrudAppService _ofertaService;
        private readonly IOfertaComercialAsyncBaseCrudAppService _ofertaComercialService;
        private readonly ICatalogoAsyncBaseCrudAppService _catalgoService;

        public OrdenServicioController(
            IHandlerExcepciones manejadorExcepciones,
            IOrdenServicioAsyncBaseCrudAppService ordenServicioService,
            ISecuencialAsyncBaseCrudAppService secuencialService,
            IOfertaAsyncBaseCrudAppService ofertaService,
            IOfertaComercialAsyncBaseCrudAppService ofertaComercialService,
            ICatalogoAsyncBaseCrudAppService catalgoService,
            IArchivoAsyncBaseCrudAppService _ArchivoService
            ) : base(manejadorExcepciones)
        {
            _ordenServicioService = ordenServicioService;
            _secuencialService = secuencialService;
            _ofertaService = ofertaService;
            _ofertaComercialService = ofertaComercialService;
            _catalgoService = catalgoService;
            ArchivoService = _ArchivoService;
        }


        // /proyecto/OrdenServicio/Index/{OfertaId}
        public async Task<ActionResult> Index()
        {

            return View();
        }

        public ActionResult IndexOfertasComerciales() {
            ViewBag.ruta = new string[] { "Inicio", "Ordenes de servicio"};

            var ofertas = _ordenServicioService.Listar();

            return View(ofertas);
        }
        public ActionResult Create(int? id)
        {
            if (id.HasValue)
            {
                var codigo = _ofertaService.GetCodigoClienteYProyecto(id.Value);
                OrdenServicioDto orden = new OrdenServicioDto()
                {
                    codigo_orden_servicio = codigo,
                    //OfertaComercialId = id.Value,
                    vigente = true,
                    fecha_orden_servicio = DateTime.Now
                };
                ViewBag.ruta = new string[] { "Inicio", "Planificación", "Orden Servicio", "Crear" };
                return View(orden);
            }

            return RedirectToAction("Index", "Inicio");
        }


        [HttpPost]
        public async Task<ActionResult> Create(OrdenServicioDto orden)
        {
            if (ModelState.IsValid)
            {
                var secuencial = _secuencialService.ObtenerIncrementarSecuencial("orden_servicio_sec");
                orden.codigo_orden_servicio = orden.codigo_orden_servicio + secuencial;
                var ordenServicio = await _ordenServicioService.Create(orden);
                return RedirectToAction("Index", "OrdenServicio", new {id = ordenServicio.EstadoId});
            }

            return View("Create", orden);
        }


        public async Task<ActionResult> Edit(int? id)
        {
            if (id.HasValue)
            {
                var orden = await _ordenServicioService.Get(new EntityDto<int>(id.Value));
                if (orden != null)
                {
                    ViewBag.ruta = new string[] { "Inicio", "Planificación", "Orden Servicio", "Editar" };
                    return View(orden);
                }
            }
            return RedirectToAction("Index", "Inicio");
        }


        [HttpPost]
        public async Task<ActionResult> Edit(OrdenServicioDto orden)
        {
            if (ModelState.IsValid)
            {
                var ordenServicio = await _ordenServicioService.Update(orden);
                return RedirectToAction("Index", "OrdenServicio", new {id = ordenServicio.Id});

            }

            return View("Edit", orden);
        }


        [HttpPost]
        public ActionResult Delete(int? id)
        {
            if (id.HasValue)
            {
                var ofertaId = _ordenServicioService.EliminarVigencia(id.Value);
                return RedirectToAction("Index", "OrdenServicio", new {id = ofertaId});
            }
            return RedirectToAction("Index", "Inicio");
        }

        public ActionResult FGetList()
        {
            var list = _ordenServicioService.GetLista();
            var result = JsonConvert.SerializeObject(list);
            return Content(result);
        }
        public ActionResult FGetCatalogos()
        {
            var list = _catalgoService.APIObtenerCatalogos(CatalogosCodigos.POS);
            var result = JsonConvert.SerializeObject(list);
            return Content(result);
        }
        public ActionResult FGetCreate(OrdenServicio o, HttpPostedFileBase UploadedFile = null)
        {
            if (UploadedFile != null)
            {
                var ArchivoId = ArchivoService.InsertArchivo(UploadedFile);
                if (ArchivoId > 0)
                {
                    o.ArchivoId = ArchivoId;
                }
            }
            var result = _ordenServicioService.InsertOrden(o);
            return Content(result);


        }
        public ActionResult FGetEdit(OrdenServicio o, HttpPostedFileBase UploadedFile = null)
        {
            if (UploadedFile != null)
            {
                var ArchivoId = ArchivoService.InsertArchivo(UploadedFile);
                if (ArchivoId > 0)
                {
                    o.ArchivoId = ArchivoId;
                }
            }
            var result = _ordenServicioService.EditOrden(o);
            return Content(result);

        }
        public ActionResult FGetDelete(int Id)
        {
            var result = _ordenServicioService.DeleteOrden(Id);
            return Content(result);
        }
        public ActionResult FGetProyectos()
        {
            var list = _ordenServicioService.ListProyectos();
            var result = JsonConvert.SerializeObject(list);
            return Content(result);
        }
        public ActionResult FGetOfertas()
        {
            var list = _ordenServicioService.ListOfertas();
            var result = JsonConvert.SerializeObject(list);
            return Content(result);
        }
        public ActionResult FGetGrupos()
        {
            var list = _ordenServicioService.ListGrupoItem();
            var result = JsonConvert.SerializeObject(list);
            return Content(result);
        }

        public ActionResult FGetOrdenHijos(int Id)
        {
            var list = _ordenServicioService.ListDetallesByOrden(Id);
            var result = JsonConvert.SerializeObject(list);
            return Content(result);
        }

        public ActionResult FDCreateDetalle(DetalleOrdenServicio o)
        {
            var list = _ordenServicioService.InsertDetalleOrden(o);

            var result = JsonConvert.SerializeObject(list);
            return Content(result);
        }
        public ActionResult FDEditDetalle(DetalleOrdenServicio o)
        {
            var list = _ordenServicioService.EditDetalleOrden(o);
            var result = JsonConvert.SerializeObject(list);
            return Content(result);
        }
        [System.Web.Mvc.HttpPost]
        public ActionResult FDRemoveDetalle(int Id, int OrdenServicioId)
        {
            var list = _ordenServicioService.DeleteDetalleOrden(Id);
          
      
            var result = JsonConvert.SerializeObject(list);
            return Content(result);
        }
        public ActionResult FDGetDetalleOs(int Id)
        {
            var up = _ordenServicioService.UpdateMontosOs(Id);
            var list = _ordenServicioService.GetOSDetalle(Id);
            var result = JsonConvert.SerializeObject(list);
            return Content(result);
        }
        public ActionResult FDUpdateMontosOs(int Id)
        {
            var list = _ordenServicioService.UpdateMontosOs(Id);
            var result = JsonConvert.SerializeObject(list);
            return Content(result);
        }

    }
}
