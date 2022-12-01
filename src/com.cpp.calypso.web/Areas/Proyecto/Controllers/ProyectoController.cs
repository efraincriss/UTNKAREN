
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.framework;
using com.cpp.calypso.proyecto.aplicacion;
using com.cpp.calypso.proyecto.dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
#pragma warning disable CS0105 // The using directive for 'com.cpp.calypso.proyecto.dominio' appeared previously in this namespace
using com.cpp.calypso.proyecto.dominio;
#pragma warning restore CS0105 // The using directive for 'com.cpp.calypso.proyecto.dominio' appeared previously in this namespace
using AutoMapper;
using System.Net;
using Abp.Application.Services.Dto;
using com.cpp.calypso.proyecto.aplicacion.Interfaces;
using com.cpp.calypso.web.Areas.Proyecto.Models;
using Newtonsoft.Json;

namespace com.cpp.calypso.web.Areas.Proyecto.Controllers
{

    public class ProyectoController : BaseController
    {
        private readonly IProyectoAsyncBaseCrudAppService _proyectoService;
        private readonly IContratoAsyncBaseCrudAppService _contratoService;
        private readonly IOfertaAsyncBaseCrudAppService _ofertaService;
        private readonly IOrdenServicioAsyncBaseCrudAppService _ordenServicioService;
        private readonly ICatalogoAsyncBaseCrudAppService _catalogoservice;
        private readonly ICertificadoAsyncBaseCrudAppService _certificadoService;
        private readonly IRequerimientoAsyncBaseCrudAppService _requerimientoService;
        // GET: Proyecto/Proyecto
        public ProyectoController(
            IHandlerExcepciones manejadorExcepciones,
            IProyectoAsyncBaseCrudAppService proyectoService,
            IContratoAsyncBaseCrudAppService contratoService,
            IOfertaAsyncBaseCrudAppService ofertaService,
            IOrdenServicioAsyncBaseCrudAppService ordenServicioService,
            ICatalogoAsyncBaseCrudAppService catalogoservice,
            ICertificadoAsyncBaseCrudAppService certificadoService,
            IRequerimientoAsyncBaseCrudAppService requerimientoService
        ) : base(manejadorExcepciones)
        {
            _proyectoService = proyectoService;
            _contratoService = contratoService;
            _ofertaService = ofertaService;
            _ordenServicioService = ordenServicioService;
            _catalogoservice = catalogoservice;
            _certificadoService = certificadoService;
            _requerimientoService = requerimientoService;
        }

#pragma warning disable CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        public async Task<ActionResult> Index(String message)
#pragma warning restore CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        {

            var result = _proyectoService.GetProyectos();
            var objectSecuencialActual = _requerimientoService.CodigoAdicionalActualProyectos();
            if (message != null)
            {

                ViewBag.Msg = message;
            }
            ViewBag.codigoProyecto = objectSecuencialActual.codigoProyecto;
            ViewBag.codigoAdicional = objectSecuencialActual.codigoAdicional;
            return View(result);
        }



        public ActionResult Details(int? id, int? idOferta, string flag = "")

        {
            if (id.HasValue)
            {
                var proyecto = _proyectoService.GetDetalles(id.Value);
                if (proyecto != null)
                {
                    if (flag == "errorDelete")
                    {
                        ViewBag.Msg =
                            "No se puede eliminar el trabajo, tiene registros relacionados";
                    }

                    var ordenes = _ordenServicioService.ListarPorProyecto(id.Value);//pendiente
                    decimal ingenieria = 0;
                    decimal construccion = 0;
                    decimal procura = 0;
                    decimal subcontratos = 0;

                    foreach (var o in ordenes)
                    {
                        ingenieria += o.monto_aprobado_ingeniería;
                        construccion += o.monto_aprobado_construccion;
                        procura += o.monto_aprobado_suministros;
                        subcontratos += o.monto_aprobado_subcontrato;

                    }
                    
                  //  var Requerimientos_Ofertas_Lig = _requerimientoService.RequerimientosyOfertasLigadas(proyecto.Requerimientos);
                    var viewModel = new RequerimientoDetailsViewModel()
                    {
                        Proyecto = proyecto,
                        Requerimientos =proyecto.RequerimientosLigados,
                        OrdenesServicio = ordenes,
                        monto_os_construccion = construccion,
                        monto_os_ingenieria = ingenieria,
                        monto_os_procura = procura,
                        monto_os_subcontratos = subcontratos,
                        monto_os_total = ingenieria + construccion + procura + subcontratos,
                        monto_ingenieria = _certificadoService.MontoPresupuestoIngenieria(proyecto.Id),
                        monto_construccion = _certificadoService.MontoPresupuestoConstruccion(proyecto.Id),
                        montoprocura = _certificadoService.MontoPresupuestoProcura(proyecto.Id),
                        montoSubcontratos = _certificadoService.MontoPresupuestoSubcontratos(proyecto.Id),

                    };
                    viewModel.montopresupuesto_total =
                        viewModel.monto_ingenieria + viewModel.monto_construccion + viewModel.montoprocura + viewModel.montoSubcontratos;

                    if (idOferta.HasValue) {
                        ViewBag.idOoferta =idOferta.Value;
                    }


                    return View(viewModel);

                }
                return RedirectToAction("Index");

            }
            return RedirectToAction("Index");
        }


        public ActionResult EditProyectoC(int? id)
        {
            if (!id.HasValue)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {

                var proyectoDto = _proyectoService.GetDetalles(id.Value);

                if (proyectoDto == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.NotFound);
                }
                else
                {
                    ViewBag.centrocostos = _catalogoservice.ListarCatalogos(1004);
                    return View(proyectoDto);
                }

            }
        }

        // POST: Proyecto/Computo/Edit/5
        [HttpPost]
        public async Task<ActionResult> EditProyectoC(int id, ProyectoDto proyectoDto)
        {
            if (ModelState.IsValid)
            {

                var r = _proyectoService.existeproyectoc(proyectoDto);
                if (r)
                {
                    ViewBag.Error = "El Código del Proyecto ya Existe";
                    var dto = Mapper.Map<ProyectoDto>(proyectoDto);
                    return View("EditProyectoC", dto);
                }

                var proyecto = await _proyectoService.Update(proyectoDto);
                return RedirectToAction("Details", "Contrato", new { id = proyecto.contratoId });
            }
            else
            {
                return RedirectToAction("Index", "Proyecto");
            }

        }

        // GET: Proyecto/Computo/Edit/5
        public ActionResult Edit(int? id, int c = 0)
        {
            if (c > 0)
            {
                ViewBag.COK = "OK";
            }

            if (!id.HasValue)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {

                ProyectoDto proyectoDto = _proyectoService.GetDetalles(id.Value);

                if (proyectoDto == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.NotFound);
                }
                else
                {
                    ViewBag.centrocostos = _catalogoservice.ListarCatalogos(1004);
                    ViewBag.lcontratos = _contratoService.GetContratosDto();
                    return View(proyectoDto);
                }

            }
        }

        // POST: Proyecto/Computo/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(int id, ProyectoDto proyectoDto, int retroceso = 0)
        //Campo retroceso es para que al momento de guardar regrese al mismo detalle no  a la lista.
        {
            try

            {
                var r = _proyectoService.existeproyectoc(proyectoDto);
                if (r)
                {
                    ViewBag.Error = "El Código del Proyecto ya Existe";
                    var dto = Mapper.Map<ProyectoDto>(proyectoDto);
                    return View("Edit", dto);
                }

                var proyecto = await _proyectoService.Update(proyectoDto);

                if (retroceso > 0)
                { //compruebo campo retroceso para ver si regresa
                    return RedirectToAction("Details", "Proyecto", new { id = proyectoDto.Id });
                }
                else
                {
                }
                return RedirectToAction("Index", "Proyecto");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Create(int? id)
        {
            if (id.HasValue)
            {
                ProyectoDto proyecto = new ProyectoDto();
                proyecto.estado_proyecto = true;
                proyecto.presupuesto = decimal.Parse("0");
                proyecto.contratoId = id.Value;
                proyecto.anio_certificacion_ingenieria = DateTime.Now.Year;

                ViewBag.centrocostos = _catalogoservice.ListarCatalogos(1004);
                return View(proyecto);
            }
            else
            {
                return RedirectToAction("Index", "Contrato");
            }
        }

        // POST: Proyecto/Contrato/Create
        [HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> Create(ProyectoDto proyecto)
        {

            try
            {

                if (ModelState.IsValid)
                {

                    proyecto.vigente = true;
                    var r = _proyectoService.existeproyectoc(proyecto);

                    if (r)
                    {


                        ViewBag.Error = "El Código del Proyecto ya Existe";
                        var dto = Mapper.Map<ProyectoDto>(proyecto);
                        return View("Create", dto);

                    }

                    var proyectos = await _proyectoService.Create(proyecto);

                    return RedirectToAction("Details", "Contrato", new { id = proyectos.contratoId });
                }
                else
                {

                    var dto = Mapper.Map<ProyectoDto>(proyecto);
                    return View("Create", dto);
                }



            }
            catch
            {
                return View();
            }
        }

#pragma warning disable CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        public async Task<ActionResult> Delete(int? id)
#pragma warning restore CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        {
            if (id.HasValue)
            {
                if (!_proyectoService.EliminarVigencia(id.Value))
                {


                    return RedirectToAction("Index", "Proyecto");
                }
                else
                {

                    string Mensaje =
                        "No se puede eliminar el proyecto, tiene registros relacionados.";
                    return RedirectToAction("Index", "Proyecto", new { message = Mensaje });
                }
            }

            return RedirectToAction("Index", "Proyecto");
        }


#pragma warning disable CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        public async Task<ActionResult> DeleteC(int? id)
#pragma warning restore CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        {
            if (id.HasValue)


            {
                var proyecto = _proyectoService.GetDetalles(id.Value);
                if (!_proyectoService.EliminarVigencia(id.Value))
                {


                    return RedirectToAction("Index", "Proyecto");
                }
                else
                {

                    string Mensaje =
                        "No se puede eliminar el proyecto, tiene registros relacionados.";
                    return RedirectToAction("Details", "Contrato", new { id = proyecto.contratoId, message = Mensaje });
                }
            }

            return RedirectToAction("Index", "Proyecto");
        }


        [HttpPost]
#pragma warning disable CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        public async Task<ActionResult> GetProyectosApi(int? id) // ContratoId
#pragma warning restore CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        {
            if (id.HasValue)
            {
                var proyectos = _proyectoService.ObtenerProyectosPorContrato(id.Value);
                var result = JsonConvert.SerializeObject(proyectos);
                return Content(result);
            }

            return Content("Error");
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult RecuperarProyectos()
        {
            var proyectos = _proyectoService.GetProyectos();
            var result = JsonConvert.SerializeObject(proyectos);
            return Content(result);
        }

        public ActionResult CreateP()
        {

            ProyectoDto proyecto = new ProyectoDto();
            proyecto.estado_proyecto = true;
            proyecto.presupuesto = decimal.Parse("0");
            proyecto.anio_certificacion_ingenieria = DateTime.Now.Year;
            ViewBag.centrocostos = _catalogoservice.ListarCatalogos(1004);
            ViewBag.lcontratos = _contratoService.GetContratosDto();
            return View(proyecto);

        }

        // POST: Proyecto/Contrato/Create
        [HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> CreateP(ProyectoDto proyecto)
        {

            try
            {

                if (ModelState.IsValid)
                {

                    proyecto.vigente = true;
                    var r = _proyectoService.existeproyectoc(proyecto);

                    if (r)
                    {


                        ViewBag.Error = "El Código del Proyecto ya Existe";
                        var dto = Mapper.Map<ProyectoDto>(proyecto);
                        return View("CreateP", dto);

                    }

                    var proyectos = await _proyectoService.Create(proyecto);

                    return RedirectToAction("Index", "Proyecto", new { id = proyectos.contratoId });
                }
                else
                {

                    var dto = Mapper.Map<ProyectoDto>(proyecto);
                    return View("CreateP", dto);
                }



            }
            catch
            {
                return View();
            }
        }
    }
}


