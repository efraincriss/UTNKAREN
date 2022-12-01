using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Mvc;
using Abp.Application.Services.Dto;
using Abp.Json;
using com.cpp.calypso.framework;
using com.cpp.calypso.proyecto.aplicacion;
using com.cpp.calypso.proyecto.aplicacion.Interfaces;
using com.cpp.calypso.proyecto.dominio;
using com.cpp.calypso.web.Areas.Proyecto.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace com.cpp.calypso.web.Areas.Proyecto.Controllers
{
    [System.Web.Mvc.AllowAnonymous]
    public class WbsOfertaController : BaseController
    {
        private readonly IOfertaAsyncBaseCrudAppService _ofertaService;
        private readonly IWbsOfertaAsyncBaseCrudAppService _wbsOfertaService;
        private readonly ICatalogoAsyncBaseCrudAppService _catalogoService;

        public WbsOfertaController(
            IHandlerExcepciones manejadorExcepciones,
            IOfertaAsyncBaseCrudAppService ofertaService,
            IWbsOfertaAsyncBaseCrudAppService wbsOfertaService,
            ICatalogoAsyncBaseCrudAppService catalogoService
            ) : base(manejadorExcepciones)
        {
            _ofertaService = ofertaService;
            _wbsOfertaService = wbsOfertaService;
            _catalogoService = catalogoService;
        }

        public ActionResult Index () // Id Oferta
        {
            
                return View();
            

          
        }


        public ActionResult Create(int? id, string flag="")
        {
            if (flag != "")
            {
                ViewBag.Msg = "La fecha de inicio no puede ser mayor a la fecha de finalización";
            }
            var areas = _wbsOfertaService.GetAreas();
            var disciplinas = _wbsOfertaService.GetDisciplinas();
            var elementos = _wbsOfertaService.GetElementos();
            var actividades = _wbsOfertaService.GetActtividades();

            WbsOfertaDto wbs = new WbsOfertaDto();
            wbs.OfertaId = id.Value;
            WbsOfertaCatalogoViewModel viewModel = new WbsOfertaCatalogoViewModel()
            {
                areas = areas,
                disciplinas = disciplinas,
                elementos = elementos,
                actividades = actividades,
                wbs = wbs
            };
            return View(viewModel);
        }

        public ActionResult CreateEstructura(int id)
        {
            ViewBag.Id = id;
            return View();
        }

        public ActionResult ApiWbsOferta(int? id)
        {   
            var lsita = _wbsOfertaService.GenerarArbol(id.Value);
            
            //var json = Json(lsita, JsonRequestBehavior.AllowGet);
            var result = JsonConvert.SerializeObject(lsita,
                Newtonsoft.Json.Formatting.None,
                new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });

            /*var json = new JsonResult
            {
                Data = JsonConvert.DeserializeObject(result)
            };*/
            //json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return Content(result);
        }

        public ActionResult ApiWbsOfertaJerarquia(int id)
        {
            var lsita = _wbsOfertaService.GenerarJerarquia(id);
            var wbs = new JerarquiaWbs()
            {
                label = "Proyecto",
                expanded = true,
                data = "Proyecto",
                className = "ui-top",
                type = "person",
                children = lsita
            };
            var result = JsonConvert.SerializeObject(wbs,
                Newtonsoft.Json.Formatting.None,
                new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });
            return Content(result);
        }

        [System.Web.Mvc.HttpPost]
        public async Task<ActionResult> CreateWbs(WbsOfertaDto wbs)
        {
            var ids = Request["ActividadId"];
            if (ModelState.IsValid)
            {
                if (wbs.fecha_inicio > wbs.fecha_fin)
                {
                    return Content("ErrorFechas");
                }
                var wbsOferta = await _wbsOfertaService.InsertOrUpdateAsync(wbs);
                return Content("Guardado");
            }
            return Content("Error en guardar");
        }

        [System.Web.Mvc.HttpPost]
        public async Task<ActionResult> CreateActividades(WbsOfertaDto wbs, [FromBody] string[] ActividadId)
        {
            foreach (var id in ActividadId)
            {
                var wbsOferta = new WbsOfertaDto()
                {
                    OfertaId = wbs.OfertaId,
                    AreaId = wbs.AreaId,
                    DisciplinaId = wbs.DisciplinaId,
                    ElementoId = wbs.ElementoId,
                    ActividadId = Int32.Parse(id),
                    estado = WbsOferta.EstadoEnum.Activo,
                    vigente = true,
                    es_estructura = false,
                    fecha_inicio = DateTime.Now,
                    fecha_fin = DateTime.Now,
                    observaciones = "Ingresar",
                };
                var existe = _wbsOfertaService.ComprobarExistenciaWbs(wbsOferta.AreaId, wbsOferta.DisciplinaId,
                    wbsOferta.ElementoId, wbsOferta.ActividadId, wbsOferta.OfertaId);

                if (!existe)
                {
                    await _wbsOfertaService.Create(wbsOferta);
                }
                
            }

            return Content("Guardado");
        }

        [System.Web.Mvc.HttpPost]
        public async Task<ActionResult> Create(WbsOfertaDto wbs)
        {
            if (ModelState.IsValid)
            {
                if (wbs.fecha_inicio > wbs.fecha_fin)
                {
                    return RedirectToAction("Create", "WbsOferta", new { id = wbs.OfertaId, flag = "dateError" });
                }
                wbs.vigente = true;
                await _wbsOfertaService.Create(wbs);
                return RedirectToAction("Index", "WbsOferta", new {id = wbs.OfertaId});
            }

            return View("Create", wbs);
        }

        public async Task<ActionResult> Edit(int? id)
        {
            if (id.HasValue)
            {
                var areas = _wbsOfertaService.GetAreas();
                var disciplinas = _wbsOfertaService.GetDisciplinas();
                var elementos = _wbsOfertaService.GetElementos();
                var actividades = _wbsOfertaService.GetActtividades();

                var wbs = await _wbsOfertaService.Get(new EntityDto<int>(id.Value));

                WbsOfertaCatalogoViewModel viewModel = new WbsOfertaCatalogoViewModel()
                {
                    areas = areas,
                    disciplinas = disciplinas,
                    elementos = elementos,
                    actividades = actividades,
                    wbs = wbs
                };


                
                if (wbs != null)
                {
                    return View(viewModel);
                }
            }

            return RedirectToAction("Index", "Proyecto");
        }

        [System.Web.Mvc.HttpPost]
        public async Task<ActionResult> Edit(WbsOfertaDto wbs)
        {
            if (ModelState.IsValid)
            {
                await _wbsOfertaService.Update(wbs);
                return RedirectToAction("Index", "WbsOferta", new {id = wbs.OfertaId});
            }

            return View("Edit", wbs);
        }


        public async Task<ActionResult> Details(int? id)
        {
            if (id.HasValue)
            {
                var wbs = await _wbsOfertaService.Get(new EntityDto<int>(id.Value));
                if (wbs != null)
                {
                    return View(wbs);
                }
            }
            return RedirectToAction("Index", "Proyecto");
        }

        public async Task<ActionResult> ApiDetails(int? id)
        {
            if (id.HasValue)
            {
                var wbs = await _wbsOfertaService.Get(new EntityDto<int>(id.Value));
                if (wbs != null)
                {
                    var wbsOferta = new WbsOfertaDto()
                    {
                        Id = wbs.Id,
                        ActividadId = wbs.ActividadId,
                        AreaId = wbs.AreaId,
                        DisciplinaId = wbs.DisciplinaId,
                        ElementoId = wbs.ElementoId,
                        OfertaId = wbs.OfertaId,
                        es_estructura = wbs.es_estructura,
                        estado = wbs.estado,
                        fecha_fin = wbs.fecha_fin,
                        fecha_inicio = wbs.fecha_inicio,
                        observaciones = wbs.observaciones,
                        vigente = wbs.vigente,
                        nombre_disciplina = _wbsOfertaService.ObtenerNombreDiciplina(wbs.ActividadId)
                    };
                    if (wbsOferta.vigente)
                    {
                        var result = JsonConvert.SerializeObject(wbsOferta);
                        return Content(result);
                    }
                    else
                    {
                        return Json(new object(),  JsonRequestBehavior.AllowGet);
                    }
                    
                }
            }
            return Content("Error");
        }




        [System.Web.Mvc.HttpPost]
        public ActionResult Delete(int? id)
        {
            if (id.HasValue)
            {
                var result = _wbsOfertaService.EliminarVigencia(id.Value);
                return RedirectToAction("Index", "WbsOferta", new {id = result});
            }

            return RedirectToAction("Index", "Proyecto");
        }

        public ActionResult ApiDelete(int? id)
        {
            if (id.HasValue)
            {
                var result = _wbsOfertaService.EliminarVigencia(id.Value);
                return Content("Ok");
            }

            return Content("Error");
        }

        public async Task<ActionResult> wbs(int? id)
        {
            var x = _wbsOfertaService.ObtenerWbsDistintos(id.Value);             
            var oferta  = await _ofertaService.Get(new EntityDto<int>(id.Value));
            ViewBag.Descripcion = oferta.descripcion;
            ViewBag.WbsOfertaId = id.Value;
            return View(x);
        }

    }
}
