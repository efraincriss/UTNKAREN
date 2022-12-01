using Abp.Application.Services.Dto;
using com.cpp.calypso.framework;
using com.cpp.calypso.proyecto.aplicacion;
using com.cpp.calypso.proyecto.aplicacion.Interfaces;
using com.cpp.calypso.proyecto.dominio;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace com.cpp.calypso.web.Areas.Proyecto.Controllers
{
    
    public class UbicacionGeograficaController : BaseController
    {
        private readonly IZonaAsyncBaseCrudAppService _zonaService;
        private readonly IZonaFrenteAsyncBaseCrudAppService _zonaFrenteService;
        private readonly IPaisAsyncBaseCrudAppService _paisService;
        private readonly IProvinciaAsyncBaseCrudAppService _provinciaService;
        private readonly IZonaProvinciaAsyncBaseCrudAppService _zonaProvinciaService;
        public UbicacionGeograficaController(
            IHandlerExcepciones manejadorExcepciones,
            IZonaAsyncBaseCrudAppService zonaService,
            IZonaFrenteAsyncBaseCrudAppService zonaFrenteService,
            IPaisAsyncBaseCrudAppService paisService,
            IProvinciaAsyncBaseCrudAppService provinciaService,
            IZonaProvinciaAsyncBaseCrudAppService zonaProvinciaService
            ) : base(manejadorExcepciones)
        {
            _zonaService = zonaService;
            _zonaFrenteService = zonaFrenteService;
            _paisService = paisService;
            _provinciaService = provinciaService;
            _zonaProvinciaService = zonaProvinciaService;
        }

        // GET: Proyecto/UbicacionGeografica
        public ActionResult Index()
        {
            var zonas = _zonaService.ObtenerTodos();
            return View(zonas);
        }

        // GET: Proyecto/UbicacionGeografica/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Proyecto/UbicacionGeografica/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Proyecto/UbicacionGeografica/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Proyecto/UbicacionGeografica/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Proyecto/UbicacionGeografica/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Proyecto/UbicacionGeografica/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Proyecto/UbicacionGeografica/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        [HttpGet]
        public ActionResult GetZonasArbol()
        {
            List<TreeItem> i = _zonaService.GenerarArbolZonasFrente();

            var resultado = JsonConvert.SerializeObject(i,
                Newtonsoft.Json.Formatting.None,
                new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });

            ViewBag.resultado = resultado;
            return Content(resultado);
        }

        public async Task<ActionResult> GetDataApi(int? id)
        {
            if (id > 0)
            {
                var frente = await _zonaService.Get(new EntityDto<int>(id.Value));
                var result = JsonConvert.SerializeObject(frente);
                return Content(result);
            }
            return Content("Error");
        }

        [HttpPost]
        public ActionResult EditApi(int Id, string nombre, string descripcion)
        {
            if (ModelState.IsValid)
            {
                _zonaService.UpdateZona(Id, nombre, descripcion);
                return Content("Guardado");
            }
            return Content("Error");
        }

        [HttpPost]
        public ActionResult DeleteItem(int Id)
        {
            if (ModelState.IsValid)
            {
                var e = _zonaFrenteService.GetFrentesPorZona(Id);
                if (e == null)
                {
                    _zonaService.DeleteZona(Id);
                    return Content("Eliminado");
                }
            }

            return Content("ErrorEliminado");
        }

        [HttpPost]
        public ActionResult CrearFrente(ZonaFrenteDto zonaFrente)
        {
            if (ModelState.IsValid)
            {
                _zonaFrenteService.CrearFrente(zonaFrente);
                return Content("Guardado");
            }
            return Content("Error");
        }

        public async Task<ActionResult> GetDataApiFrente(int? id)
        {
            if (id > 0)
            {
                var frente = await _zonaFrenteService.Get(new EntityDto<int>(id.Value));
                var result = JsonConvert.SerializeObject(frente);
                return Content(result);
            }
            return Content("Error");
        }

        [HttpPost]
        public ActionResult EditApiFrente(int Id, string nombre,
            string descripcion, string cordenadax, string cordenaday)
        {
            if (ModelState.IsValid)
            {
                _zonaFrenteService.UpdateFrente(Id, nombre, descripcion, cordenadax, cordenaday);
                return Content("Guardado");
            }
            return Content("Error");
        }

        [HttpPost]
        public ActionResult DeleteItemFrente(int Id)
        {
            if (ModelState.IsValid)
            {
                _zonaFrenteService.DeleteFrente(Id);
                return Content("Eliminado");
            }
            return Content("ErrorEliminado");
        }

        public ActionResult GetPaisesApi()
        {
            var paises = _paisService.GetPaises();
            var result = JsonConvert.SerializeObject(paises);
            return Content(result);
        }

        [HttpPost]
        public ActionResult GetProvinciasApi(int? id)
        {
            if (id.HasValue)
            {
                var result = _provinciaService.ObtenerProvinciaPorPais(id.Value);
                return Json(result);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "No se encuentra el id");
        }

        [HttpPost]
        public ActionResult CreateZonaApi(int Id, string codigo, string nombre,
            string descripcion, int zonaId, string[] provincias)
        {
            int idTemp = _zonaService.CreateZona(codigo, nombre, descripcion);
            _zonaProvinciaService.CreateMany(idTemp, provincias);
            return Content("Guardado");
        }


        //public ActionResult GetZonasApi()
        //{
        //    var list = _zonaService.GetAll();
        //    var result = JsonConvert.SerializeObject(list);
        //    return Content(result);
        //}

        public async virtual Task<ActionResult> GetZonasApi()
        {
            //TODO: Temporal, hasta que proporcione el api correspondiente para 
            //obtener listado de servicios
            var pagedResultDto = await _zonaService.GetAll();

            var result = JsonConvert.SerializeObject(pagedResultDto,
                Formatting.None,
                new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    NullValueHandling = NullValueHandling.Ignore
                });
            return Content(result);
        }
    }
}
