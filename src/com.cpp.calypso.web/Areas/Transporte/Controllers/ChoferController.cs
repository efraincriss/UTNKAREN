using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.framework;
using com.cpp.calypso.proyecto.aplicacion.Transporte.Dto;
using com.cpp.calypso.proyecto.aplicacion.Transporte.Interface;
using com.cpp.calypso.proyecto.dominio;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace com.cpp.calypso.web.Areas.Transporte.Controllers
{
    public class ChoferController : BaseTransporteSpaController<Chofer, ChoferDto, PagedAndFilteredResultRequestDto>
    {

        private readonly IChoferAsyncBaseCrudAppService _ChoferService;


        public ChoferController(
          IChoferAsyncBaseCrudAppService ChoferService,
             IHandlerExcepciones manejadorExcepciones,
          IViewService viewService,
          IAsyncBaseCrudAppService<Chofer, ChoferDto, PagedAndFilteredResultRequestDto, ChoferDto> entityService

          ) : base(manejadorExcepciones, viewService, entityService)
        {
            _ChoferService = ChoferService;
        }

        // GET: Transporte/Chofer
        public ActionResult Index()
        {
            ViewBag.ruta = new string[] { "Inicio", "Conductores", "Listado de Conductores" };
           // await _ChoferService.EnviarCorreo();
            return View();
        }

        // GET: Transporte/Chofer/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Transporte/Chofer/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Transporte/Chofer/Create
        [HttpPost]
        public ActionResult Create(Chofer chofer)
        {
            var buscar = _ChoferService.BuscarChoferProveedor(chofer.NumeroIdentificacion,chofer.TipoIdentificacionId);
            if (buscar != null) {
               return Content("EXISTE");
            }
            else { 

            var choferid = _ChoferService.IngresarChofer(chofer);
            return Content(choferid > 0 ? "OK" : "ERROR");
            }
        }

        // GET: Transporte/Chofer/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Transporte/Chofer/Edit/5
        [HttpPost]
        public ActionResult Edit(Chofer chofer)
        {
            var buscar = _ChoferService.BuscarChoferProveedorEditar(chofer.NumeroIdentificacion, chofer.TipoIdentificacionId,chofer.Id);
            if (buscar != null)
            {
                return Content("EXISTE");
            }
            else {
            var choferid = _ChoferService.EditarChofer(chofer);
            return Content(choferid > 0 ? "OK" : "ERROR");

            }

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
            var choferid = _ChoferService.EliminarChofer(id);
            return Content(choferid > 0 ? "OK" : "ERROR");
        }

        public ActionResult ListaChoferes()
        {
            var lista_choferes = _ChoferService.Listar();

            return WrapperResponseGetApi(ModelState, () => lista_choferes);
        }
        public ActionResult ListaProveedoresTransporte()
        {
            var proveedores_transporte = _ChoferService.ListaProveedoresTransporte();

            return WrapperResponseGetApi(ModelState, () => proveedores_transporte);
        }
        public ActionResult ObtenerChoferCedula(string cedula)
        {
    

            var chofer = _ChoferService.BuscarChofer(cedula);
            if (chofer != null)
            {
                var result = JsonConvert.SerializeObject(chofer);
                return Content(result);
            }
            else
            {
                return Content("Error");
            }
        }
        public ActionResult ObtenerDetallesChofer(int  id)
        {


            var chofer = _ChoferService.GetDetalles(id);
            if (chofer != null)
            {
                var result = JsonConvert.SerializeObject(chofer);
                return Content(result);
            }
            else
            {
                return Content("Error");
            }
        }
    }
}
