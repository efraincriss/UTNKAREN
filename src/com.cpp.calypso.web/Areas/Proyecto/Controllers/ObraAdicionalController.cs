using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Abp.Application.Services.Dto;
using com.cpp.calypso.framework;
using com.cpp.calypso.proyecto.aplicacion;
using com.cpp.calypso.proyecto.aplicacion.Interfaces;
using com.cpp.calypso.web.Areas.Proyecto.Models;
using Newtonsoft.Json;

namespace com.cpp.calypso.web.Areas.Proyecto.Controllers
{
  
    public class ObraAdicionalController : BaseController
    {
        private readonly IObraAdicionalAsyncBaseCrudAppService _obraAdicionlService;
        private readonly IAvanceObraAsyncBaseCrudAppService _avanceObraService;
        private readonly IOfertaAsyncBaseCrudAppService _ofertaService;
        private readonly IItemAsyncBaseCrudAppService _itemsService;

        public ObraAdicionalController(
            IHandlerExcepciones manejadorExcepciones,
            IObraAdicionalAsyncBaseCrudAppService obraAdicionlService,
            IAvanceObraAsyncBaseCrudAppService avanceObraService,
            IOfertaAsyncBaseCrudAppService ofertaService,
            IItemAsyncBaseCrudAppService itemsService
            ) : base(manejadorExcepciones)
        {
            _obraAdicionlService = obraAdicionlService;
            _avanceObraService = avanceObraService;
            _ofertaService = ofertaService;
            _itemsService = itemsService;
        }



        [HttpPost]
        public async Task<ActionResult> CreateOrUpdateApi(ObraAdicionalDto adicional)
        {
            if (ModelState.IsValid)
            {
                var item = await _obraAdicionlService.InsertOrUpdateAsync(adicional);
                return Content("Ok");
            }
            return Content("Error");
        }


        [HttpPost]
        public ActionResult DetailsApi(int id) // ObraAdicionalId
        {
            var adicional = _obraAdicionlService.Get(new EntityDto<int>(id));
            var result = JsonConvert.SerializeObject(adicional);
            return Content(result);
        }

        [HttpPost]

        public async Task<ActionResult> Delete(int id) // ObraAdicionalId
        {
            var AvanceObraId = await _obraAdicionlService.Eliminar(id);
            return RedirectToAction("Details", "AvanceObra", new {id = AvanceObraId});
        }

        public async Task<ActionResult> CreateObraAdicional(int? id) // AvanceObraId
        {
            if (id.HasValue)
            {
                var avance = await _avanceObraService.Get(new EntityDto<int>(id.Value));
                var oferta = await _ofertaService.Get(new EntityDto<int>(avance.OfertaId));
                CreateObraAdicionalViewModel viewModel = new CreateObraAdicionalViewModel()
                {
                    AvanceObraId = avance.Id,
                    OfertaId = oferta.Id,
                    nombre_proyecto = oferta.Proyecto.nombre_proyecto,
                    codigo_avance_obra = avance.Id + "",
                    fecha_presentacion = avance.fecha_presentacion.GetValueOrDefault()
                };
                return View(viewModel);
            }
            return RedirectToAction("Index", "Inicio", new { area = "" });
        }

        [HttpPost]

#pragma warning disable CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        public async Task<ActionResult> GetAllItems() // ObraAdicionalId
#pragma warning restore CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        {
            var items = _itemsService.GetItemsParaOferta();
            var result = JsonConvert.SerializeObject(items);
            return Content(result);
        }


        public async Task<ActionResult> Edit(int? id) // ObraAdicionalId
        {
            if (id.HasValue)
            {
                var item = await _obraAdicionlService.Get(new EntityDto<int>(id.Value));
                return View(item);

            }
            return RedirectToAction("Index", "Inicio", new { area = "" });
        }

        [HttpPost]

        public async Task<ActionResult> Edit(ObraAdicionalDto adicional)
        {
            if (ModelState.IsValid)
            {
                var avanceId = await _obraAdicionlService.InsertOrUpdateAsync(adicional);
                return RedirectToAction("Details", "AvanceObra", new {id = avanceId.AvanceObraId});
            }
            return View("Edit", adicional);
        }


    }
}
