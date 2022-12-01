using System;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Mvc;
using Abp.Application.Services.Dto;
using com.cpp.calypso.framework;
using com.cpp.calypso.proyecto.aplicacion;
using com.cpp.calypso.proyecto.dominio;
using Newtonsoft.Json;

namespace com.cpp.calypso.web.Areas.Proyecto.Controllers
{
    
    public class ProcesoNotificacionController : BaseController
    {
        private readonly IProcesoNotificacionAsyncBaseCrudAppService _procesoNotificacionService;

        // GET: Proyecto/ProcesoNotificacion
        public ProcesoNotificacionController(
            IHandlerExcepciones manejadorExcepciones,
            IProcesoNotificacionAsyncBaseCrudAppService procesoNotificacionService
        ) : base(manejadorExcepciones)
        {
            _procesoNotificacionService = procesoNotificacionService;
        }

        public ActionResult Index()
        {
            var procesos = _procesoNotificacionService.Listar();
            return View(procesos);
        }


        public ActionResult Create()
        {
            var proceso = new ProcesoNotificacionDto()
            {
                vigente = true
            };

            return View(proceso);
        }

        [System.Web.Mvc.HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> Create(ProcesoNotificacionDto procesoDto)
        {
            if (ModelState.IsValid)
            {
                var proceso = await _procesoNotificacionService.Create(procesoDto);
                return RedirectToAction("Details", "ProcesoNotificacion", new {id = proceso.Id});
            }

            return View("Create", procesoDto);
        }


        public async Task<ActionResult> Edit(int? id)
        {
            if (id.HasValue)
            {
                var proceso = await _procesoNotificacionService.Get(new EntityDto<int>(id.Value));
                return View(proceso);
            }
            return RedirectToAction("Index", "Inicio", new { area = "" });
        }


        [System.Web.Mvc.HttpPost]
        public async Task<ActionResult> Edit(ProcesoNotificacionDto proceso)
        {
            if (ModelState.IsValid)
            {
                var edited = await _procesoNotificacionService.InsertOrUpdateAsync(proceso);
                return RedirectToAction("Details", "ProcesoNotificacion", new {id = edited.Id});
            }

            return View("Edit", proceso);
        }

        public async Task<ActionResult> Details(int? id)
        {
            if (id.HasValue)
            {
                var proceso = await _procesoNotificacionService.Get(new EntityDto<int>(id.Value));
                return View(proceso);
            }
            return RedirectToAction("Index", "Inicio", new { area = "" });
        }


        [System.Web.Mvc.HttpPost]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id.HasValue)
            {
                var proceso = await _procesoNotificacionService.Get(new EntityDto<int>(id.Value));
                proceso.vigente = false;

                await _procesoNotificacionService.Update(proceso);
                return RedirectToAction("Index", "ProcesoNotificacion");
            }
            return RedirectToAction("Index", "Inicio", new { area = "" });
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult GetProcesos([FromBody] ProcesoNotificacion.TipoProceso tipo)
        {
            var procesos = _procesoNotificacionService.ListarPorTipo(tipo);
            var result = JsonConvert.SerializeObject(procesos);
            return Content(result);
        }

       
    }
}