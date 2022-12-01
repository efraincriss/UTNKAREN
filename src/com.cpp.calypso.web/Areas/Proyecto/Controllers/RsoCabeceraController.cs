using Abp.Application.Services.Dto;
using com.cpp.calypso.framework;
using com.cpp.calypso.proyecto.aplicacion;
using com.cpp.calypso.proyecto.aplicacion.Dto;
using com.cpp.calypso.proyecto.aplicacion.Interfaces;
using com.cpp.calypso.proyecto.dominio.Models;
using com.cpp.calypso.web.Areas.Proyecto.Models;
using Newtonsoft.Json;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace com.cpp.calypso.web.Areas.Proyecto.Controllers
{
    public class RsoCabeceraController : BaseController
    {
        private IProyectoAsyncBaseCrudAppService _proyectoService;
        private readonly IRequerimientoAsyncBaseCrudAppService _requerimeintoService;
        private IRsoCabeceraAsyncBaseCrudAppService _rsoCService;
        private readonly IRsoDetalleEacAsyncBaseCrudAppService _rsoEacService;
        private readonly IOfertaAsyncBaseCrudAppService _ofertaService;
        private readonly IAvanceObraAsyncBaseCrudAppService _avanceService;
        public RsoCabeceraController(
            IHandlerExcepciones manejadorExcepciones,
            IProyectoAsyncBaseCrudAppService proyectoService,
            IRsoCabeceraAsyncBaseCrudAppService rsoCService,
            IRsoDetalleEacAsyncBaseCrudAppService rsoEacService,
            IAvanceObraAsyncBaseCrudAppService avanceService,
            IOfertaAsyncBaseCrudAppService ofertaService,
            IRequerimientoAsyncBaseCrudAppService requerimeintoService,
        IRdoDetalleAsyncBaseCrudAppService rdoDService) : base(manejadorExcepciones)
        {
            _proyectoService = proyectoService;
            _rsoCService = rsoCService;
            _rsoEacService = rsoEacService;
            _avanceService = avanceService;
            _ofertaService = ofertaService;
            _requerimeintoService = requerimeintoService;
        }

        public ActionResult HistoricosCurva()
        {

            return View();
        }
        public ActionResult Index()
        {

            var proyectos = _proyectoService.GetProyectos();
            ViewBag.ruta = new string[] { "Inicio", "RSO", "Lista Proyectos" };
            return View(proyectos);
        }


        public ActionResult Detailsproyecto(int? id)
        {
            if (!id.HasValue)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                var proyecto = _proyectoService.GetDetalles(id.Value);
                var rsocabeceras = _rsoCService.GetRdoCabeceras(proyecto.Id);

                if (proyecto == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.NotFound);
                }
                else
                {
                    var ViewModel = new ProyectoRsoCabecera()
                    {
                        Proyecto = proyecto,
                        RdoCabeceras = rsocabeceras

                    };
                    ViewBag.ruta = new string[] { "Inicio", "RSO", proyecto.codigo };
                    return View(ViewModel);
                }
            }
        }

        public ActionResult IndexApi(int id)
        {
            var rdocabeceras = _rsoCService.GetRdoCabecerasTable(id);
            var result = JsonConvert.SerializeObject(rdocabeceras);
            return Content(result);
        }

        [System.Web.Mvc.HttpPost]
        public async Task<ActionResult> EmitirRdo(int id)
        {
            await _rsoCService.EmitirRdoAsync(id);
            return RedirectToAction("Details", "RsoCabecera", new { id = id, flag = "Emitido" });
        }


        public async Task<ActionResult> Details(int? id, string flag = "", int id2 = 0, int id3 = 0)// id2 avance de obra 
        {
            if (!id.HasValue)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                if (flag != "")
                {
                    ViewBag.msg = "RSO Emitido";
                }
                if (id3 > 0)
                {
                    ViewBag.RequerimientoId = id3;
                }
                else
                {
                    ViewBag.RequerimientoId = 0;
                }
                if (id2 > 0)
                {
                    ViewBag.avance = id2;
                }
                var rdocabecera = await _rsoCService.Get(new EntityDto<int>(id.Value));

                var rdodetallesEac = _rsoEacService.Listar(id.Value);
                if (rdocabecera == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.NotFound);
                }
                else
                {
                    var ViewModel = new RsoViewModel()
                    {
                        RdoCabeceraDto = rdocabecera,
                        DetallesEac = rdodetallesEac,
                    };
                    ViewBag.ruta = new string[] { "Inicio", "RSO", rdocabecera.Proyecto.codigo, "Detalles RSO" };
                    return View(ViewModel);
                }
            }
        }

        public ActionResult GetRdo(int id) //RdoCabeceraId
        {
            //var excelPackage = _rdoCService.GenerarExcelRdo(id, "BUDGET");

            //aumento de montos en paquete rdo

            var excelPackage = _rsoCService.MontoTotales(id, "BUDGET");
            //

            string excelName = "RSO";
            using (var memoryStream = new MemoryStream())
            {
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment; filename=" + excelName + ".xlsx");

                excelPackage.SaveAs(memoryStream);

                memoryStream.WriteTo(Response.OutputStream);
                Response.Flush();
                Response.End();
                return Content("");
            }
        }

        public async Task<ActionResult> GetRdoEac(int id) //RdoCabeceraId
        {
            var rdocabecera = await _rsoCService.Get(new EntityDto<int>(id));
            var excelPackage = _rsoCService.MontoTotales(id, "EAC");
            excelPackage.Workbook.Worksheets.Delete(excelPackage.Workbook.Worksheets[2]);
            string excelName = "";
            if (rdocabecera.Proyecto.es_RSO)
            {

                excelName = rdocabecera.Proyecto.codigo_reporte_RDO != null && rdocabecera.Proyecto.codigo_reporte_RDO.Length > 0 ? rdocabecera.Proyecto.codigo_reporte_RDO + "-" + "RSO-" + rdocabecera.fecha_rdo.ToString("yy") + "" + rdocabecera.fecha_rdo.ToString("MM") + "" + rdocabecera.fecha_rdo.ToString("dd") : "RSO-" + rdocabecera.fecha_rdo.ToString("yy") + "" + rdocabecera.fecha_rdo.ToString("MM") + "" + rdocabecera.fecha_rdo.ToString("dd");
            }
            else
            {
                excelName = rdocabecera.Proyecto.codigo_reporte_RDO != null && rdocabecera.Proyecto.codigo_reporte_RDO.Length > 0 ? rdocabecera.Proyecto.codigo_reporte_RDO + "-" + "RSO-" + rdocabecera.fecha_rdo.ToString("yy") + "" + rdocabecera.fecha_rdo.ToString("MM") + "" + rdocabecera.fecha_rdo.ToString("dd") : "RSO-" + rdocabecera.fecha_rdo.ToString("yy") + "" + rdocabecera.fecha_rdo.ToString("MM") + "" + rdocabecera.fecha_rdo.ToString("dd");
            }
            using (var memoryStream = new MemoryStream())
            {
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment; filename=" + excelName + ".xlsx");
                excelPackage.SaveAs(memoryStream);
                memoryStream.WriteTo(Response.OutputStream);
                Response.Flush();
                Response.End();
                return Content("");
            }
        }

        // GET: Proyecto/RdoCabecera/Create

        public ActionResult _Create(int proyectoid)
        {
            RsoCabeceraDto n = new RsoCabeceraDto();
            var p = _proyectoService.GetDetalles(proyectoid);
            n.ProyectoId = proyectoid;
            var rdocabeceras = _rsoCService.GetRdoCabeceras(proyectoid);
            n.codigo_rdo = p.Contrato.Codigo + "-" + "RSO-" + (rdocabeceras.Count() + 1);
            n.fecha_rdo = DateTime.Now;
            return PartialView(n);
        }

        // POST: Proyecto/RdoCabecera/Create
        [System.Web.Mvc.HttpPost]
        public ActionResult Create([FromBody] DateTime fecha_registro, [FromBody] int ProyectoId, [FromBody] int Id)
        {
            if (Id > 0)
            {
                var rdoAnterior = _rsoCService.GetDetalles(Id);
                if (rdoAnterior != null && fecha_registro < rdoAnterior.fecha_rdo)
                {
                    return Content("MENOR");
                }
                bool deleteRsoPosteriores = _rsoCService.EliminarPeriodoRdoCabecera(Id,fecha_registro);
            }
            var id = _rsoCService.CreateRdoCabecera(ProyectoId, fecha_registro,Id);
            decimal avance_actual_acumulado = _rsoEacService.CalcularRdoDetallesEAC(id, fecha_registro);
            var IdActual = _rsoEacService.ActualizarRdoCabecera(id, avance_actual_acumulado);
            return Content("Ok");
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult CreateRso([FromBody] DateTime fecha_registro, [FromBody] int ProyectoId, [FromBody] int Id, [FromBody] bool tieneFechaInicio)
        {
            if (tieneFechaInicio)
            {
                var rdoAnterior = _rsoCService.GetDetalles(Id);
                if (rdoAnterior != null && fecha_registro < rdoAnterior.fecha_rdo)
                {
                    return Content("MENOR");
                }
                bool deleteRsoPosteriores = _rsoCService.EliminarPeriodoRdoCabecera(Id, fecha_registro);
            }
            var id = _rsoCService.CreateRsoLast(ProyectoId, fecha_registro, Id);
            return Content(id);
        }

        // GET: Proyecto/RdoCabecera/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id.HasValue)
            {
                var cabecera = await _rsoCService.Get(new EntityDto<int>(id.Value));
                return View(cabecera);
            }

            return RedirectToAction("Index", "Inicio", new { area = "" });
        }

        // POST: Proyecto/RdoCabecera/Edit/5
        [System.Web.Mvc.HttpPost]
        public async Task<ActionResult> Edit(RsoCabeceraDto cabecera)
        {
            if (ModelState.IsValid)
            {
                await _rsoCService.Update(cabecera);
                return RedirectToAction("Details", "RsoCabecera", new { id = cabecera.Id });
            }

            return View("Edit", cabecera);
        }


        public ActionResult Delete(int id)
        {
            try
            {
                var eliminado = _rsoCService.EliminarRdoCabecera(id);
                if (eliminado == 1)
                {
                    return Content("OK");
                }
                else
                {
                    return Content("ERROR");
                }
            }
            catch
            {
                return Content("ERROR");
            }
        }

        // Generar Curva Rdo

        public ActionResult GenerarCurva(int id)
        {
            ExcelPackage curva = _rsoCService.GenerarCurvaRDO(id, 1);

            string excelName = "Curva Rso";
            using (var memoryStream = new MemoryStream())
            {
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment; filename=" + excelName + ".xlsx");
                curva.SaveAs(memoryStream);
                memoryStream.WriteTo(Response.OutputStream);
                Response.Flush();
                Response.End();
                return Content("");
            }
        }
        [System.Web.Http.HttpPost]
        public async Task<ActionResult> CreateRdoAvance(int id)//avance de obra
        {
            var avance_obra = await _avanceService.Get(new EntityDto<int>(id));

            var idRDO = _rsoCService.CreateRdoCabecera(avance_obra.Oferta.ProyectoId, avance_obra.fecha_presentacion.Value,0);

            decimal avance_actual_acumulado = _rsoEacService.CalcularRdoDetallesEAC(idRDO, avance_obra.fecha_presentacion.Value);
            var IdActual = _rsoEacService.ActualizarRdoCabecera(idRDO, avance_actual_acumulado);

            return RedirectToAction("Details", "RsoCabecera", new { id = idRDO, id2 = avance_obra.Id });


        }
        [System.Web.Http.HttpPost]
        public async Task<ActionResult> CreateBaseRdo([FromBody] DateTime fecha_registro, int RequerimientoId)//avance de obra
        {
            var requerimiento = _requerimeintoService.GetDetalles(RequerimientoId);
            var resultado = _ofertaService.CargarPresupuestoBaseRdo(RequerimientoId);
            if (resultado == "AVANCES")
            {
                return Content(resultado);
            }
            var idRDO = _rsoCService.CreateRdoCabecera(requerimiento.ProyectoId, fecha_registro,0);

            decimal avance_actual_acumulado = _rsoEacService.CalcularRdoDetallesEAC(idRDO, fecha_registro);
            var IdActual = _rsoEacService.ActualizarRdoCabecera(idRDO, avance_actual_acumulado);
            string result = idRDO + "," + RequerimientoId;
            return Content(result);
            //return RedirectToAction("Details", "RdoCabecera", new { id = , id3 = RequerimientoId });


        }

        public async Task<ActionResult> CreateRdoFechas(int Id, DateTime fechadesde, DateTime fechahasta)//avance de obra
        {
            var fecha_inicial = fechadesde.Date;
            for (DateTime fecha = fecha_inicial; fecha <= fechahasta.Date; fecha = fecha.AddDays(1))
            {
                var idRDO = _rsoCService.CreateRdoCabecera(Id, fecha.Date,0);
                decimal avance_actual_acumulado = _rsoEacService.CalcularRdoDetallesEAC(idRDO, fecha.Date);
                var IdActual = _rsoEacService.ActualizarRdoCabecera(idRDO, avance_actual_acumulado);
            }

            return Content("OK");


        }


        public ActionResult ApiContratos()
        {
            var list = _rsoCService.GetContratos();
            var result = JsonConvert.SerializeObject(list);
            return Content(result);
        }
        public ActionResult ApiProyectos(int Id)
        {
            var list = _rsoCService.GetProyectos(Id);
            var result = JsonConvert.SerializeObject(list);
            return Content(result);
        }
        public ActionResult APIFechaMin(int Id)
        {
            var result = _rsoCService.FechaMinimaAvanceObra(Id);
            return Content(result != null && result != new DateTime(1990, 01, 01) ? result.ToString() : "NO_FECHA");
        }
        public ActionResult ApiExcel(ModelHistoricoCurva model)
        {
            var excel = _rsoCService.GenerarExcelCurva(model);
            string excelName = "Formato_Curva" + DateTime.Now.ToShortDateString();
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
        public ActionResult Actualizar(HttpPostedFileBase UploadedFile)
        {

            if (UploadedFile != null)
            {
                var result = _rsoCService.ActualizarFechasHistoricos(UploadedFile);
                return Content(result);
            }

            return Content("SIN_ARCHIVO");


        }
    }
}
