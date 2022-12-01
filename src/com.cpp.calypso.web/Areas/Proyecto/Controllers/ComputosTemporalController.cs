using com.cpp.calypso.framework;
using com.cpp.calypso.proyecto.aplicacion;
using com.cpp.calypso.proyecto.aplicacion.Interfaces;
using com.cpp.calypso.proyecto.dominio;
using com.cpp.calypso.web.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using HttpPostAttribute = System.Web.Http.HttpPostAttribute;

namespace com.cpp.calypso.web.Areas.Proyecto.Controllers
{
    
    public class ComputosTemporalController : BaseController
    {
        private readonly ICatalogoAsyncBaseCrudAppService _catalogoservice;
        private readonly IComputoAsyncBaseCrudAppService _computoService;
        private readonly IComputosTemporalAsyncBaseCrudAppService _computosTemporalService;
        private readonly IOfertaAsyncBaseCrudAppService _ofertaService;
        private readonly IWbsAsyncBaseCrudAppService _wbsService;
        public ComputosTemporalController(
            IHandlerExcepciones manejadorExcepciones,
            ICatalogoAsyncBaseCrudAppService catalogoservice,
            IComputoAsyncBaseCrudAppService computoService,
            IOfertaAsyncBaseCrudAppService ofertaService,
            IComputosTemporalAsyncBaseCrudAppService computosTemporalService,
            IWbsAsyncBaseCrudAppService wbsService
            ) : base(manejadorExcepciones)
        {
            this._computoService = computoService;
            this._ofertaService = ofertaService;
            _computosTemporalService = computosTemporalService;
            _wbsService = wbsService;
                        _catalogoservice = catalogoservice;
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult GetComputosPorProyectoApi(int id) // AvanceObra Id
        {
            var list = _computoService.GetComputosPorOferta(id);
            var result = JsonConvert.SerializeObject(list);
            return Content(result);
        }

        [System.Web.Mvc.HttpPost]
        public void EmitirPresupuesto(int idOferta) // idOferta 
        {
            _ofertaService.Aprobar(idOferta);

        }

        public ActionResult ApiComputo(int id)
        {
            //var lsita = _wbsofertaService.GenerarArbolComputo(id); //arbol antiguo
            var lsita = _wbsService.GenerarArbolComputosTemporal(id); // arbol nuevo wbs recursivo
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

        [System.Web.Mvc.HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> EditComputosTemporal(ComputosTemporalDto computosTemporalDto, [FromBody] decimal cantidad_eac = 0)
        {

            var e = await _computosTemporalService.GetDetalle(computosTemporalDto.Id);

            bool r = _computosTemporalService.EditarCantidadComputoTemporal(e.Id, computosTemporalDto.cantidad, cantidad_eac);


            return Content(r == true ? "OK" : "Error");

        }

        [System.Web.Mvc.HttpPost]
        public  ActionResult DeleteComputoArbol(int id)
        {
            var r = _computosTemporalService.EliminarVigencia(id);

            if (r.Result)
            {

                return Content("Eliminado");
            }

            return Content("ErrorEliminado");

        }

        [System.Web.Mvc.HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> CreateComputosTemporalArbol(ComputosTemporalDto computoTemporalDto, [FromBody]string nuevonombrei)
        {
            if (ModelState.IsValid)
            {
            }
            computoTemporalDto.codigo_primavera = "a";
            computoTemporalDto.fecha_registro = DateTime.Now;
            computoTemporalDto.estado = true;
            computoTemporalDto.vigente = true;
            if (computoTemporalDto.cantidad_eac == 0)
            {
                computoTemporalDto.cantidad_eac = computoTemporalDto.cantidad;
            }

            bool e = _computosTemporalService.comprobarexistenciaitem(computoTemporalDto.WbsId, computoTemporalDto.ItemId);
            if (!e)
            {
                var computo = await _computosTemporalService.InsertOrUpdateAsync(computoTemporalDto);
                if (computo.Id > 0)
                    return Content("OK");
                else
                    return Content("ERROR");
            }
            else
            {
                return Content("Repetido");
            }
        }

        [System.Web.Mvc.HttpPost]
        [Abp.Runtime.Validation.DisableValidation]
        public ActionResult ActualizarValorEacAsync(int idOferta, int[] computos, List<ComputosTemporalDto> lista)
        {
            foreach (var comp in lista)
            {
                foreach (var o in computos)
                {
                    if (comp.Id == o)
                    {
                        comp.cantidad = comp.cantidad_eac;
                        _computosTemporalService.Actualizar(o, comp.cantidad_eac, comp.precio_ajustado);
                    }
                }
            }
            return Content(JsonConvert.SerializeObject(lista));
        }

        public void GenerarNuevaVersion(int id)
        {
            _computosTemporalService.GenerarNuevaVersion(id);

        }

        public ActionResult GetComputosTemporal(int id)
        {
            var result = _computosTemporalService.GetComputosTempPorOferta(id);
            return Content(JsonConvert.SerializeObject(result));
        }

        public ActionResult AgregarItems(int id)
        {
            return RedirectToAction("EstructuraComputos", "ComputosTemporal", new { id });
        }

        public ActionResult EstructuraComputos(int? id, string mensaje)
        {
            if (id.HasValue)
            {
                if (mensaje != null)
                {
                    ViewBag.msg = mensaje;
                }

                var ofertac = _ofertaService.getdetalle(id.Value);
                var computos = _computosTemporalService.GetComputosporOferta(id.Value);
                OfertaComputoViewModel viewModel = new OfertaComputoViewModel()
                {
                    Oferta = ofertac,
                    computosTemporal = computos,
                    Contrato = ofertac.Proyecto.Contrato
                };

                return View(viewModel);
            }

            return RedirectToAction("Index", "Inicio", new { area = "" });
        }


        // GET: Proyecto/ComputosTemporal
        public ActionResult Index(string mensaje)
        {
            ViewBag.Msg = mensaje;
            return View();
        }

        // GET: Proyecto/ComputosTemporal/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Proyecto/ComputosTemporal/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Proyecto/ComputosTemporal/Create
        [System.Web.Http.HttpPost]
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

        // GET: Proyecto/ComputosTemporal/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Proyecto/ComputosTemporal/Edit/5
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

        // GET: Proyecto/ComputosTemporal/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Proyecto/ComputosTemporal/Delete/5
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
    }
}
