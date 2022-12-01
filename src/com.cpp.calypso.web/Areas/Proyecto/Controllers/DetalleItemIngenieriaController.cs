using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Abp.Application.Services.Dto;
using com.cpp.calypso.framework;
using com.cpp.calypso.proyecto.aplicacion;
using Newtonsoft.Json;

namespace com.cpp.calypso.web.Areas.Proyecto.Controllers
{
    
    public class DetalleItemIngenieriaController : BaseController
    {
        private readonly IDetalleItemIngenieriaAsyncBaseCrudAppService _itemIngenieriaService;

        // GET: Proyecto/DetalleItemIngenieria
        public DetalleItemIngenieriaController(
            IHandlerExcepciones manejadorExcepciones,
            IDetalleItemIngenieriaAsyncBaseCrudAppService itemIngenieriaService
            ) : base(manejadorExcepciones)
        {
            _itemIngenieriaService = itemIngenieriaService;
        }


        // GET: Proyecto/DetalleItemIngenieria/Details/5
        [HttpPost]
        public async Task<ActionResult> DetailsApi(int? id)
        {
            if (id.HasValue)
            {
                var item = await _itemIngenieriaService.Get(new EntityDto<int>(id.Value));
                var result = JsonConvert.SerializeObject(item);
                return Content(result);
            }
            return Content("ErrorId");
        }



        // POST: Proyecto/DetalleItemIngenieria/Create
        [HttpPost]
        public async Task<ActionResult> Create(DetalleItemIngenieriaDto item)
        {
            if (ModelState.IsValid)
            {
                var itemIngenieria = await _itemIngenieriaService.InsertOrUpdateAsync(item);
                return Content("Ok");
            }
            return Content("Error");
        }


        // POST: Proyecto/DetalleItemIngenieria/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(DetalleItemIngenieriaDto item)
        {
            if (ModelState.IsValid)
            {
                var itemIngenieria = await _itemIngenieriaService.Update(item);
                return Content("Ok");
            }
            return Content("Error");
        }

        // POST: Proyecto/DetalleItemIngenieria/Delete/5
        [HttpPost]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id.HasValue)
            {
                var item = await _itemIngenieriaService.Get(new EntityDto<int>(id.Value));
                item.vigente = false;
                await _itemIngenieriaService.Update(item);
                return Content("Ok");
            }
            return Content("ErrorId");
        }

        [HttpPost]
        public ActionResult GetItemsPorDetalleApi(int id) // DetalleAvanceingenieriaId
        {
            var items = _itemIngenieriaService.ListarPorDetalleAvance(id);
            var result = JsonConvert.SerializeObject(items);
            return Content(result);
        }


    }
}
