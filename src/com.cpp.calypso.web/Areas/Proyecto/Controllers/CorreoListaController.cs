using System;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Mvc;
using Abp.Application.Services.Dto;
using com.cpp.calypso.framework;
using com.cpp.calypso.proyecto.aplicacion;

namespace com.cpp.calypso.web.Areas.Proyecto.Controllers
{
    
    public class CorreoListaController : BaseController
    {
        private readonly ICorreoListaAsyncBaseCrudAppService _correoListaService;

        // GET: Proyecto/CorreoLista
        public CorreoListaController(
            IHandlerExcepciones manejadorExcepciones,
            ICorreoListaAsyncBaseCrudAppService correoListaService
        ) : base(manejadorExcepciones)
        {
            _correoListaService = correoListaService;
        }

        public ActionResult Index()
        {
            var correos = _correoListaService.listar();
            return View(correos);
        }


        public async Task<ActionResult> Details(int? id)
        {
            if (id.HasValue)
            {
                var correo = await _correoListaService.Get(new EntityDto<int>(id.Value));
                return View(correo);
            }

            return RedirectToAction("Index", "Inicio", new {area = ""});

        }


        public async Task<ActionResult> Edit(int? id)
        {
            if (id.HasValue)
            {
                var correo = await _correoListaService.Get(new EntityDto<int>(id.Value));
                return View(correo);

            }
            return RedirectToAction("Index", "Inicio", new { area = "" });
        }


        [System.Web.Mvc.HttpPost]
        public async Task<ActionResult> Edit(CorreoListaDto correoDto)
        {
            if (ModelState.IsValid)
            {
                var correo = await _correoListaService.InsertOrUpdateAsync(correoDto);
                return RedirectToAction("Details", "CorreoLista", new {id = correo.Id});
            }

            return View("Edit", correoDto);
        }


        [System.Web.Mvc.HttpPost]
        public ActionResult Delete(int? id)
        {
            if (id.HasValue)
            {
                
            }
            return RedirectToAction("Index", "Inicio", new { area = "" });
        }

        [System.Web.Mvc.HttpPost]
        public async Task<ActionResult> CreateApi([FromBody]int ListaDistribucionId, [FromBody] bool externo, [FromBody] int UsuarioId, [FromBody] string correo, [FromBody] string nombres)
        {
            var entity = new CorreoListaDto()
            {
                ListaDistribucionId = ListaDistribucionId,
                UsuarioId = UsuarioId,
                correo = correo,
                externo = externo,
                nombres = nombres,
                vigente = true,
            };
            var newCorreo = await _correoListaService.InsertOrUpdateAsync(entity);
            return Content(newCorreo.Id > 0 ? "Ok" : "Error");
        }
    }
}