using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.framework;
using com.cpp.calypso.proyecto.aplicacion.Transporte.Dto;
using com.cpp.calypso.proyecto.aplicacion.Transporte.Interface;
using com.cpp.calypso.proyecto.dominio.Transporte;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace com.cpp.calypso.web.Areas.Transporte.Controllers
{
    public class RutaHorarioVehiculoController :BaseTransporteSpaController<RutaHorarioVehiculo, RutaHorarioVehiculoDto, PagedAndFilteredResultRequestDto>
    {

        private readonly IRutaAsyncBaseCrudAppService _RutaService;
        private readonly IRutaHorarioVehiculoAsyncBaseCrudAppService _RutaHorarioVehiculoService;
        private readonly IRutaParadaAsyncBaseCrudAppService _RutaParadaService;
        private readonly IVehiculoAsyncBaseCrudAppService _vehiculoService;



        public RutaHorarioVehiculoController(
        IRutaAsyncBaseCrudAppService RutaService,
        IRutaHorarioVehiculoAsyncBaseCrudAppService RutaHorarioVehiculoService,
        IRutaParadaAsyncBaseCrudAppService RutaParadaService,
          IVehiculoAsyncBaseCrudAppService vehiculoService,
        IHandlerExcepciones manejadorExcepciones,
        IViewService viewService,
        IAsyncBaseCrudAppService<RutaHorarioVehiculo, RutaHorarioVehiculoDto, PagedAndFilteredResultRequestDto, RutaHorarioVehiculoDto> entityService

      ) : base(manejadorExcepciones, viewService, entityService)
    {
        _RutaService = RutaService;
        _RutaHorarioVehiculoService = RutaHorarioVehiculoService;
        _RutaParadaService = RutaParadaService;
            _vehiculoService = vehiculoService;
        }

    // GET: Transporte/Chofer
    public ActionResult Index()
    {
            ViewBag.ruta = new string[] { "Inicio", "Asignación de Vehículo a Rutas", "Listado de Asignaciones" };
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
    public ActionResult Create(RutaHorarioVehiculo ruta,int horarioid)
    {

        var rutaid = _RutaHorarioVehiculoService.Ingresar(ruta, horarioid);
            if (rutaid == -1) {
                return Content("MISMARUTA");
            } else { 
             return Content(rutaid > 0 ? "OK" : "ERROR");
            }

        }

    // GET: Transporte/Chofer/Edit/5
    public ActionResult Edit(int id)
    {
        return View();
    }

    // POST: Transporte/Chofer/Edit/5
    [HttpPost]
    public ActionResult Edit(RutaHorarioVehiculo ruta)
    {

        var rutaid = _RutaHorarioVehiculoService.Editar(ruta);
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
          
                var rutaid = _RutaHorarioVehiculoService.Eliminar(id);

            if (rutaid == -1)
            {
                return Content("REGISTROS");
            }
            else
            {
                return Content(rutaid > 0 ? "OK" : "ERROR");
            }
    }

        public ActionResult ListaRutaHorasVehiculo()
        {
            var listarutas = _RutaHorarioVehiculoService.Listar();

            return WrapperResponseGetApi(ModelState, () => listarutas);
        }
        public ActionResult ListaByRutaHorario(int rutaid,int horarioid)
        {
            var listarutas = _RutaHorarioVehiculoService.ListarbyRutaHorario(rutaid, horarioid);

            return WrapperResponseGetApi(ModelState, () => listarutas);
        }
        public ActionResult ListaVehiculos()
        {
            var listavehiculo = _vehiculoService.GetAllVehiculos();

            return WrapperResponseGetApi(ModelState, () => listavehiculo);
        }

        public ActionResult ListaRutas()
        {
            var listarutas = _RutaService.Listar();

            return WrapperResponseGetApi(ModelState, () => listarutas);
        }

        public ActionResult ObtenerHorariosRuta(int id)
        {


            var lista = _RutaParadaService.ListaHorariosporRuta(id);
            if (lista != null)
            {
                var result = JsonConvert.SerializeObject(lista);
                return Content(result);
            }
            else
            {
                return Content("Error");
            }
        }

        public ActionResult ObtenerHoraLLegada(int rutaid, int horarioid)
        {
            TimeSpan resultado = _RutaHorarioVehiculoService.HoraLLegada(rutaid, horarioid);

            var result = JsonConvert.SerializeObject(resultado);
            return Content(result);
        }

    }
}

