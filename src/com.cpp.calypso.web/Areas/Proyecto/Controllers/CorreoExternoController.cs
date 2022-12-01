using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Abp.Application.Services.Dto;
using com.cpp.calypso.framework;
using com.cpp.calypso.proyecto.aplicacion;
using com.cpp.calypso.proyecto.aplicacion.Dto;

namespace com.cpp.calypso.web.Areas.Proyecto.Controllers
{
    
    public class CorreoExternoController : BaseController
    {
        private readonly ICorreoExternoAsyncBaseCrudAppService _correoExternoService;

        // GET: Proyecto/CorreoExterno
        public CorreoExternoController(
            IHandlerExcepciones manejadorExcepciones,
            ICorreoExternoAsyncBaseCrudAppService correoExternoService
            ) : base(manejadorExcepciones)
        {
            _correoExternoService = correoExternoService;
        }

        public ActionResult Index()
        {
            var correos = _correoExternoService.listar();
            return View(correos);
        }

        public async Task<ActionResult> Details(int? id)
        {
            if (id.HasValue)
            {
                var correo = await _correoExternoService.Get(new EntityDto<int>(id.Value));
                return View(correo);
            }

            return RedirectToAction("Index", "Inicio", new {area = ""});
        }


        public async Task<ActionResult> Edit(int? id)
        {
            if (id.HasValue)
            {
                var correo = await _correoExternoService.Get(new EntityDto<int>(id.Value));
                return View(correo);
            }
            return RedirectToAction("Index", "Inicio", new { area = "" });
        }


        [HttpPost]
        public async Task<ActionResult> Edit(CorreoExternoDto correoDto)
        {
            if (ModelState.IsValid)
            {
                var correo = await _correoExternoService.InsertOrUpdateAsync(correoDto);
                return RedirectToAction("Index", "CorreoExterno", new {id = correo.Id});
            }

            return View("Edit", correoDto);
        }


        public ActionResult Create()
        {
            var correo = new CorreoExternoDto()
            {
                vigente = true
            };

            return View(correo);
        }


        [HttpPost]
        public async Task<ActionResult> Create(CorreoExternoDto correoDto)
        {
            if (ModelState.IsValid)
            {
                var correo = await _correoExternoService.InsertOrUpdateAsync(correoDto);
                return RedirectToAction("Index", "CorreoExterno", new {id = correo.Id});
            }

            return View("Create", correoDto);
        }


        [HttpPost]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id.HasValue)
            {
                var correo = await _correoExternoService.Get(new EntityDto<int>(id.Value));
                correo.vigente = false;
                await _correoExternoService.Update(correo);
                return RedirectToAction("Index", "CorreoExterno");
            }

            return RedirectToAction("Index", "Inicio", new {area = ""});
        }
    }
}