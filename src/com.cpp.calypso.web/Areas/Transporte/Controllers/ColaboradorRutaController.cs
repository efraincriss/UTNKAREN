using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.framework;
using com.cpp.calypso.proyecto.aplicacion.Interfaces;
using com.cpp.calypso.proyecto.aplicacion.Proveedor.Interfaces;
using com.cpp.calypso.proyecto.aplicacion.Transporte.Dto;
using com.cpp.calypso.proyecto.aplicacion.Transporte.Interface;
using com.cpp.calypso.proyecto.dominio.Transporte;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;

namespace com.cpp.calypso.web.Areas.Transporte.Controllers
{
    public class ColaboradorRutaController :BaseTransporteSpaController<ColaboradorRuta, ColaboradorRutaDto, PagedAndFilteredResultRequestDto>
    {

            private readonly IRutaAsyncBaseCrudAppService _RutaService;
            private readonly IRutaHorarioVehiculoAsyncBaseCrudAppService _RutaHorarioVehiculoService;
            private readonly IRutaParadaAsyncBaseCrudAppService _RutaParadaService;
            private readonly IVehiculoAsyncBaseCrudAppService _vehiculoService;
            private readonly IColaboradorRutaAsyncBaseCrudAppService _colaboradorRutaService;

            private readonly IColaboradoresAsyncBaseCrudAppService _ColaboradoresService;
             private readonly IReservaHotelAsyncBaseCrudAppService _service;



        public ColaboradorRutaController(
            IRutaAsyncBaseCrudAppService RutaService,
            IRutaHorarioVehiculoAsyncBaseCrudAppService RutaHorarioVehiculoService,
            IRutaParadaAsyncBaseCrudAppService RutaParadaService,
            IVehiculoAsyncBaseCrudAppService vehiculoService,
            IColaboradoresAsyncBaseCrudAppService ColaboradoresService,
        IColaboradorRutaAsyncBaseCrudAppService colaboradorRutaService,
        IReservaHotelAsyncBaseCrudAppService service,
        IHandlerExcepciones manejadorExcepciones,
            IViewService viewService,
            IAsyncBaseCrudAppService<ColaboradorRuta, ColaboradorRutaDto, PagedAndFilteredResultRequestDto, ColaboradorRutaDto> entityService

            ) : base(manejadorExcepciones, viewService, entityService)
            {
            _RutaService = RutaService;
            _RutaHorarioVehiculoService = RutaHorarioVehiculoService;
            _RutaParadaService = RutaParadaService;
            _vehiculoService = vehiculoService;
            _colaboradorRutaService=colaboradorRutaService;
            _ColaboradoresService = ColaboradoresService;
            _service = service;
            }
        public ActionResult Index(string id)
        {
            ViewBag.ruta = new string[] { "Inicio", "Asignación de Colaborador a Ruta", "Asignación de Rutas" };
            var colaborador = _colaboradorRutaService.BuscarColaborador(id);

            return View(colaborador);
        }
        public ActionResult BuscarColaborador()
        {
            ViewBag.ruta = new string[] { "Inicio", "Asignación de Colaborador a Ruta", "Buscar Colaborador" };

            return View();
        }

        // GET: Transporte/ColaboradorRuta/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Transporte/ColaboradorRuta/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Transporte/ColaboradorRuta/Create
        [HttpPost]
        public  async Task<ActionResult> Create(ColaboradorRuta ruta)
        {

            var rutaid = _colaboradorRutaService.Ingresar(ruta);
            await _colaboradorRutaService.EnviarMensajeAsync(rutaid);
            return Content(rutaid > 0 ? "OK" : "ERROR");

        }

        // GET: Transporte/ColaboradorRuta/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Transporte/ColaboradorRuta/Edit/5
        [HttpPost]
        public ActionResult Edit(ColaboradorRuta ruta)
        {

            var rutaid = _colaboradorRutaService.Editar(ruta);
            return Content(rutaid > 0 ? "OK" : "ERROR");


        }

        // GET: Transporte/ColaboradorRuta/Delete/5
        public ActionResult Delete()
        {
            return View();
        }

        // POST: Transporte/ColaboradorRuta/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            var rutaid = _colaboradorRutaService.Eliminar(id);
            return Content(rutaid > 0 ? "OK" : "ERROR");
        }

        public ActionResult ListaColaboradoresRutas(int id)//Colaborador ID
        {
            var listarutas = _colaboradorRutaService.ListarbyColaborador(id);

            return WrapperResponseGetApi(ModelState, () => listarutas);
        }

        public ActionResult ObtenerRutaHorarios(int id)
        {
            if (id > 0)
            {

                var lista = _colaboradorRutaService.ListaRutasHorario(id);
                if (lista != null)
                {
                    var result = JsonConvert.SerializeObject(lista);
                    return Content(result);
                }
                else
                {
                    return Content("Error");
                }
            
                }else
            {
                return Content("Error");
          }
        }

        public ActionResult ObtenerDetallesRuta(int id)
        {


            var ruta = _colaboradorRutaService.GetDetallesRuta(id);
            if (ruta != null)
            {
                var result = JsonConvert.SerializeObject(ruta);
                return Content(result);
            }
            else
            {
                return Content("Error");
            }
        }
        public ActionResult ObtenerColaborador(string identificacion)
        {
            var colaborador = _colaboradorRutaService.BuscarColaborador(identificacion);

            if (colaborador != null)
            {
                var result = JsonConvert.SerializeObject(colaborador);
                return Content(result);
            }
            else
            {
                return Content("Error");
            }
        }
        public ActionResult BuscarColaboradorIdentificacion(string identificacion = "", string nombres = "")
        {
           
            var colaboradores = _service.BuscarPorIdentificacionNombre(identificacion, nombres).Where(c=>c.estado!= "INACTIVO").ToList();
            return WrapperResponseGetApi(ModelState, () => colaboradores);
        }
    }
}
