using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using Abp.Application.Services.Dto;
using com.cpp.calypso.framework;
using com.cpp.calypso.proyecto.aplicacion;
using Newtonsoft.Json;

namespace com.cpp.calypso.web.Areas.Proyecto.Controllers
{
   
    public class ListaDistribucionController : BaseController
    {
        private readonly IListaDistribucionAsyncBaseCrudAppService _listaDistribucionService;

        // GET: Proyecto/ListaDistribucion
        public ListaDistribucionController(
            IHandlerExcepciones manejadorExcepciones,
            IListaDistribucionAsyncBaseCrudAppService listaDistribucionService
        ) : base(manejadorExcepciones)
        {
            _listaDistribucionService = listaDistribucionService;
        }

        public ActionResult Index()
        {
            var lista = _listaDistribucionService.listar();
            return View(lista);
        }



        public async Task<ActionResult> Details(int? id)
        {
            if (id.HasValue)
            {
                var lista = await _listaDistribucionService.Get(new EntityDto<int>(id.Value));
                return View(lista);
            }
            return RedirectToAction("Index", "Inicio", new { area = "" });
        }


        public async Task<ActionResult> Edit(int? id)
        {
            if (id.HasValue)
            {
                var lista = await _listaDistribucionService.Get(new EntityDto<int>(id.Value));
                return View(lista);
            }
            return RedirectToAction("Index", "Inicio", new { area = "" });
        }

        [HttpPost]
        public async Task<ActionResult> Edit(ListaDistribucionDto ListaDto)
        {
            if (ModelState.IsValid)
            {
                var lista = await _listaDistribucionService.InsertOrUpdateAsync(ListaDto);
                return RedirectToAction("Details", "ListaDistribucion", new {id = lista.Id});
            }

            return View("Edit", ListaDto);
        }

        public ActionResult Create()
        {
            var lista = new ListaDistribucionDto()
            {
                vigente = true
            };
            return View(lista);
        }

        [HttpPost]
        public async Task<ActionResult> Create(ListaDistribucionDto ListaDto)
        {
            if (ModelState.IsValid)
            {
                var lista = await _listaDistribucionService.InsertOrUpdateAsync(ListaDto);
                return RedirectToAction("Details", "ListaDistribucion", new {id = lista.Id});
            }

            return View("Create", ListaDto);
        }

        [HttpPost]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id.HasValue)
            {
                var lista = await _listaDistribucionService.Get(new EntityDto<int>(id.Value));
                lista.vigente = false;

                await _listaDistribucionService.Update(lista);
                return RedirectToAction("Index", "ListaDistribucion");
            }

            return RedirectToAction("Index", "Inicio", new { area = "" });

        }


        [HttpPost]
        public ActionResult GetCorreosExternosApi(int id) // Lista de distribucion Id
        {
            var correos = _listaDistribucionService.GetCorreosExternos(id);
            var result = JsonConvert.SerializeObject(correos);
            return Content(result);
        }

        [HttpPost]
        public ActionResult GetCorreosInternosApi(int id) // Lista de distribucion Id
        {
            var correos = _listaDistribucionService.GetCorreosInternos(id);
            var result = JsonConvert.SerializeObject(correos);
            return Content(result);
        }

        [HttpPost]
        public ActionResult GetCorreosExternosParaIngresarApi(int id) // Lista de distribucion Id
        {
            var correos = _listaDistribucionService.GetCorreosExternosParaIngresar(id);
            var result = JsonConvert.SerializeObject(correos);
            return Content(result);
        }

        [HttpPost]
        public ActionResult GetCorreosInternosParaIngresarApi(int id) // Lista de distribucion Id
        {
            var correos = _listaDistribucionService.GetCorreosInternosParaIngresar(id);
            var result = JsonConvert.SerializeObject(correos);
            return Content(result);
        }

        [HttpPost]
        public ActionResult GetEliminar(int id) // Lista de distribucion Id
        {
            var si = _listaDistribucionService.EliminarCorreoLista(id);
            return Content(si?"OK":"ERROR");
        }

        [HttpPost]
        public ActionResult GetUpdateSeccion(CorreoListaDto correo) // Lista de distribucion Id
        {
            var correos = _listaDistribucionService.ActualizarLista(correo);
            var result = JsonConvert.SerializeObject(correos);
            return Content("OK");
        }

        [HttpPost]
        public ActionResult OrdenarCorroes(List<CorreoListaDto> correos) // Lista de distribucion Id
        {
            var result = _listaDistribucionService.OrdenarCorreos(correos);
            
            return Content(result.ToString());
        }

    }
}