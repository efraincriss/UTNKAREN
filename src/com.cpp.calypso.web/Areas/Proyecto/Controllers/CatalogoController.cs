using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http.Results;
using System.Web.Mvc;
using Abp.Application.Services.Dto;
using com.cpp.calypso.framework;
using com.cpp.calypso.proyecto.aplicacion;
using com.cpp.calypso.proyecto.aplicacion.Interfaces;
using Newtonsoft.Json;

namespace com.cpp.calypso.web.Areas.Proyecto.Controllers
{

    public class CatalogoController : BaseController
    {
        private readonly ICatalogoAsyncBaseCrudAppService _catalogoService;
        private readonly ITipoCatalogoAsyncBaseCrudAppService _tcatalogoService;
        public CatalogoController(
            IHandlerExcepciones manejadorExcepciones,
            ICatalogoAsyncBaseCrudAppService catalogoService,
            ITipoCatalogoAsyncBaseCrudAppService tcatalogoService
            ) : base(manejadorExcepciones)
        {
            _catalogoService = catalogoService;
            _tcatalogoService = tcatalogoService;
        }

        public ActionResult Index(int? id)
        {
            if (id.HasValue)
            {
                ViewBag.Id = id.Value;
                var lista = _catalogoService.ListarCatalogos(id.Value).OrderBy(c => c.ordinal).ToList();//quitar order by
                return View(lista);
            }

            return RedirectToAction("Index", "Inicio");

        }

        public ActionResult Create(int? id)
        {
            if (id.HasValue)
            {
                var tipo = _tcatalogoService.Detalles(id.Value);
                CatalogoDto catalogo = new CatalogoDto()
                {
                    vigente = true,
                    TipoCatalogoId = id.Value,
                    NombreTipoCatalogo=tipo.codigo

                };
                return View(catalogo);
            }
            return RedirectToAction("Index", "Inicio");
        }


        [HttpPost]
        public async Task<ActionResult> Create(CatalogoDto catalogo)
        {
            if (ModelState.IsValid)
            {
                var c = await _catalogoService.Create(catalogo);

                return RedirectToAction("Index", "Catalogo", new { id = c.TipoCatalogoId });
            }
            return View("Create", catalogo);
        }

        public async Task<ActionResult> Edit(int? id)
        {
            if (id.HasValue)
            {
                var cat = await _catalogoService.Get(new EntityDto<int>(id.Value));
                var tipo = _tcatalogoService.Detalles(cat.TipoCatalogoId);
                cat.NombreTipoCatalogo = tipo.codigo;
                return View(cat);
            }
            return RedirectToAction("Index", "Inicio");
        }


        [HttpPost]
        public async Task<ActionResult> Edit(CatalogoDto catalogo)
        {
            if (ModelState.IsValid)
            {
                var c = await _catalogoService.Update(catalogo);
                return RedirectToAction("Index", "Catalogo", new { id = c.TipoCatalogoId });
            }
            return RedirectToAction("Index", "Inicio");
        }


        [HttpPost]
        public ActionResult Delete(int? id)
        {
            if (id.HasValue)
            {
                var result = _catalogoService.EliminarVigencia(id.Value);
                if (result != 0)
                {
                    return RedirectToAction("Index", "Catalogo", new { id = result });
                }
            }
            return RedirectToAction("Index", "Inicio");
        }


        [HttpPost]
        public ActionResult GetCatalogo(int? id)
        {
            if (id.HasValue)
            {
                var resultado = _catalogoService.ListarCatalogos(id.Value).OrderBy(c => c.ordinal).ToList();
                var result = JsonConvert.SerializeObject(resultado,
            Newtonsoft.Json.Formatting.None,

            new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore,


            });
                return Content(result);
            }

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "No se encuentra el id");
        }

        /// <summary>
        /// Exclusive to use in M'odules that need compare the name of exist catalogos in real time /dropdown
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult GetNamebyId(int id)
        {
            var name = _catalogoService.GetNamebyId(id);
            return Content(name);

        }


        /// <summary>
        /// TODO: Campusoft. (No es parte de los requerimientos, debe ser revisado / mejorado por el cliente        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public ActionResult GetByCodeApi(string code)
        {
            var entityDto = _catalogoService.ListarCatalogos(code);
            return WrapperResponseGetApi(ModelState, () => entityDto);

        }
    }
}
