using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Abp.Application.Services.Dto;
using com.cpp.calypso.framework;
using com.cpp.calypso.proyecto.aplicacion;
using com.cpp.calypso.proyecto.aplicacion.Interfaces;
using com.cpp.calypso.proyecto.dominio;
using Newtonsoft.Json;
using OfficeOpenXml;

namespace com.cpp.calypso.web.Areas.Proyecto
{
    public class WbsPresupuestoController : BaseController
    {
        private readonly IPresupuestoAsyncBaseCrudAppService _presupuestoService;
        private readonly IWbsPresupuestoAsyncBaseCrudAppService _wbsPresupuestoService;
        private readonly ICatalogoAsyncBaseCrudAppService _catalogoService;

        // GET: Proyecto/WbsPresupuesto
        public WbsPresupuestoController(
            IHandlerExcepciones manejadorExcepciones,
            IPresupuestoAsyncBaseCrudAppService presupuestoService,
            IWbsPresupuestoAsyncBaseCrudAppService wbsPresupuestoService,
            ICatalogoAsyncBaseCrudAppService catalogoService
            ) : base(manejadorExcepciones)
        {
            _presupuestoService = presupuestoService;
            _wbsPresupuestoService = wbsPresupuestoService;
            _catalogoService = catalogoService;
        }

        public async Task<ActionResult> Index(int? id)
        {
            if (id.HasValue)
            {
                var presupuesto = await _presupuestoService.Get(new EntityDto<int>(id.Value));
                var requerimiento = presupuesto.Requerimiento.tipo_requerimiento;
               
                ViewBag.Id = id;

                if (requerimiento == TipoRequerimiento.Principal)
                {
                    ViewBag.Requerimiento = 1;

                }
                else
                {
                    ViewBag.Requerimiento = 0;
                }

                ViewBag.ruta = new string[] { "Inicio", "Presupuesto", "WBS", "Gestionar WBS" };
                return View(presupuesto);
            }
            return RedirectToAction("Index", "Inicio", new { area = "" });
        }

        public ActionResult ApiWbs(int? id) //PresupuestoId
        {
            var lista = _wbsPresupuestoService.GenerarArbol(id.Value);
            var result = JsonConvert.SerializeObject(lista,
                Newtonsoft.Json.Formatting.None,
                new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });
            return Content(result);
        }

        public ActionResult GetDiagramaApi(int? id) //OfertaId
        {
            var lista = _wbsPresupuestoService.GenerarDiagrama(id.Value);
            var wbs = new JerarquiaWbs()
            {
                label = "Proyecto",
                expanded = true,
                data = "Proyecto",
                className = "ui-top",
                type = "person",
                children = lista
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
        public ActionResult ApiWbsK(int id)
        {
            var x = _wbsPresupuestoService.ObtenerKeysArbol(id);
            List<int> wbs = new List<int>();
            foreach (var item in x)
            {
                wbs.Add(item.Id);
            }
            var result = JsonConvert.SerializeObject(wbs,
                Newtonsoft.Json.Formatting.None,
                new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });

            return Content(result);
        }


        public ActionResult ApiWbsL(int? id) //OfertaId
        {
            var lista = _wbsPresupuestoService.GenerarArbolDrag(id.Value);
            var result = JsonConvert.SerializeObject(lista,
                Newtonsoft.Json.Formatting.None,
                new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });
            return Content(result);
        }


        [System.Web.Mvc.HttpPost]
        public ActionResult ApiWbsD(List<TreeWbs> data) //OfertaId
        {
            var x = _wbsPresupuestoService.GuardarArbolDrag(data);
            return Content(x);
        }


        [System.Web.Mvc.HttpPost]
        public async Task<ActionResult> Edit(WbsPresupuestoDto wbs)
        {
            if (ModelState.IsValid)
            {
                if (wbs.fecha_inicial > wbs.fecha_final)
                {
                    return Content("ErrorFechas");
                }

                wbs.Cambio = WbsPresupuesto.TipoCambio.Editado;
                await _wbsPresupuestoService.InsertOrUpdateAsync(wbs);
                return Content("Guardado");
            }
            return Content("Error en guardar");
        }

        [System.Web.Mvc.HttpPost]
        public async Task<ActionResult> Delete(int id)
        {
            var CountComputos = _wbsPresupuestoService.ContarComputosPorWbs(id);
            if (CountComputos > 0)
            {
                return Content("ErrorComputos");
            }
            else
            {
                var wbs = await _wbsPresupuestoService.Get(new EntityDto<int>(id));
                wbs.Cambio = WbsPresupuesto.TipoCambio.Eliminado;
                wbs.vigente = false;
                await _wbsPresupuestoService.Update(wbs);
                return Content("Ok");
            }

        }

        [System.Web.Mvc.HttpPost]
        public ActionResult Create(WbsPresupuestoDto wbs)
        {
            wbs.id_nivel_codigo = "pendiente";
            var w = _wbsPresupuestoService.CrearPadre(wbs);
            return Content(wbs.Id > 0 ? "Ok" : "Error");
        }

        // Api Eliminar Nivel
        [System.Web.Mvc.HttpPost]
        public ActionResult DeleteNivel(int id)
        {
            var eliminado = _wbsPresupuestoService.EliminarNivel(id);
            return Content(eliminado ? "Ok" : "Error");
        }

        // Api Editar Nombre Nivel
        [System.Web.Mvc.HttpPost]
        public ActionResult EditarNivel(int id, [FromBody] string nombre)
        {
            _wbsPresupuestoService.Editar(id, nombre);
            return Content("Ok");
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult CreateActividades(WbsPresupuestoDto wbs, [FromBody] string[] ActividadesIds)
        {
            wbs.id_nivel_codigo = "pendiente";
            _wbsPresupuestoService.CrearActividades(wbs, ActividadesIds);
            return Content("Ok");
        }


        [System.Web.Mvc.HttpPost]
        public async Task<ActionResult> CreateCatalogo(CatalogoDto catalogo)
        {
            bool existe = _catalogoService.existecatalogo(catalogo.nombre);

            if (!existe)
            {
                var x = await _catalogoService.Create(catalogo);
                return Content("Ok");
            }
            else {
                return Content("Existe");
            }
        }


        public async Task<ActionResult> ExportarExcel(int id) //Pasar pametros oferta 
        {
            var ofertac =await _presupuestoService.Get(new EntityDto<int>(id));
            ExcelPackage excel = _wbsPresupuestoService.GenerarExcelCargaFechas(ofertac);

            string excelName = "Formato Carga Fechas";
            using (var memoryStream = new MemoryStream())
            {
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment; filename=" + excelName + ".xlsx");
                excel.SaveAs(memoryStream);
                memoryStream.WriteTo(Response.OutputStream);
                Response.Flush();
                Response.End();
                return Content("");
            }
        }


        [System.Web.Mvc.HttpPost]
        public ActionResult CreateCopiaWbs([FromBody]int origen, [FromBody]int destino, [FromBody]int PresupuestoId)
        {
            var result = _wbsPresupuestoService.CopiarWBS(PresupuestoId, destino, origen);
            return Content(result ? "OK" : "ERROR");
        }
    }
}