using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Abp.Application.Services.Dto;
using com.cpp.calypso.framework;
using com.cpp.calypso.proyecto.aplicacion;
using com.cpp.calypso.proyecto.aplicacion.Interfaces;
using com.cpp.calypso.proyecto.dominio;
using com.cpp.calypso.web.Areas.Proyecto.Models;
using Newtonsoft.Json;
using OfficeOpenXml;

namespace com.cpp.calypso.web.Areas.Proyecto.Controllers
{

    public class WbsController : BaseController
    {
        private readonly IWbsAsyncBaseCrudAppService _wbsService;
        private readonly IOfertaAsyncBaseCrudAppService _ofertaService;

        public WbsController(
            IHandlerExcepciones manejadorExcepciones,
            IWbsAsyncBaseCrudAppService wbsService,
            IOfertaAsyncBaseCrudAppService ofertaService
            ) : base(manejadorExcepciones)
        {
            _wbsService = wbsService;
            _ofertaService = ofertaService;
        }



        public async Task<ActionResult> Index(int? id) // OfertaId
        {
            if (id.HasValue)
            {
                var oferta = await _ofertaService.Get(new EntityDto<int>(id.Value));
                ViewBag.Id = id;
                ViewBag.rId = oferta.RequerimientoId;
                ViewBag.ruta = new string[] { "Inicio", "Planificación", "WBS", oferta.codigo, "Gestionar" };
                return View(oferta);
            }

            return RedirectToAction("Index", "Inicio", new { area = "" });
        }

#pragma warning disable CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        public async Task<ActionResult> IndexWbs(int? id) // OfertaId
#pragma warning restore CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        {
            if (id.HasValue)
            {
                var wbs = _wbsService.Listar(id.Value);
                var oferta = _wbsService.GetClienteProyectoFecha(id.Value);

                var viewModel = new IndexWbsViewModel()
                {
                    oferta = oferta,
                    Wbs = wbs,
                };
                ViewBag.ruta = new string[] { "Inicio", "Planificación", "WBS", oferta.codigo };
                return View(viewModel);
            }

            return RedirectToAction("Index", "Inicio", new { area = "" });
        }


        public ActionResult ApiWbs(int? id) //OfertaId
        {
            var lista = _wbsService.GenerarArbol(id.Value);
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
            var lista = _wbsService.GenerarDiagrama(id.Value);
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
        public ActionResult Create(WbsDto wbs)
        {
            wbs.id_nivel_codigo = "pendiente";
            var w = _wbsService.CrearPadre(wbs);
            return Content(wbs.Id > 0 ? "Ok" : "Error");
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult CreateActividades(WbsDto wbs, [FromBody] string[] ActividadesIds)
        {
            wbs.id_nivel_codigo = "pendiente";
            _wbsService.CrearActividades(wbs, ActividadesIds);
            return Content("Ok");
        }

        [System.Web.Mvc.HttpPost]
        public async Task<ActionResult> Edit(WbsDto wbs)
        {
            if (ModelState.IsValid)
            {
                if (wbs.fecha_inicial > wbs.fecha_final)
                {
                    return Content("ErrorFechas");
                }
                var wbsOferta = await _wbsService.InsertOrUpdateAsync(wbs);
                return Content("Guardado");
            }
            return Content("Error en guardar");
        }

        // Editar 
        public async Task<ActionResult> Editar(int? id) // WbsId
        {
            if (id.HasValue)
            {
                var wbs = await _wbsService.Get(new EntityDto<int>(id.Value));
                return View(wbs);
            }

            return RedirectToAction("Index", "Inicio", new { area = "" });
        }

        [System.Web.Mvc.HttpPost]
        public async Task<ActionResult> Editar(WbsDto wbs)
        {
            if (ModelState.IsValid)
            {
                var w = await _wbsService.Update(wbs);
                return RedirectToAction("IndexWbs", new { id = w.OfertaId });
            }
            return View("Editar", wbs);
        }


        [System.Web.Mvc.HttpPost]
        public async Task<ActionResult> DetailsApi(int id)
        {
            var wbs = await _wbsService.Get(new EntityDto<int>(id));
            var result = JsonConvert.SerializeObject(wbs);
            return Content(result);
        }

        // Api
        [System.Web.Mvc.HttpPost]
        public async Task<ActionResult> Delete(int id)
        {
            var CountComputos = _wbsService.ContarComputosPorWbs(id);
            if (CountComputos > 0)
            {
                return Content("ErrorComputos");
            }
            else
            {
                var wbs = await _wbsService.Get(new EntityDto<int>(id));
                wbs.vigente = false;
                await _wbsService.Update(wbs);
                return Content("Ok");
            }

        }

        // Api Eliminar Nivel
        [System.Web.Mvc.HttpPost]
        public ActionResult DeleteNivel(int id)
        {
            var eliminado = _wbsService.EliminarNivel(id);
            return Content(eliminado ? "Ok" : "Error");
        }

        // Api Editar Nombre Nivel
        [System.Web.Mvc.HttpPost]
        public ActionResult EditarNivel(int id, [FromBody] string nombre)
        {
            _wbsService.Editar(id, nombre);
            return Content("Ok");
        }

        // Api clonar wbs
        [System.Web.Mvc.HttpPost]
        public ActionResult ClonarWBS([FromBody] int RequerimientoId, [FromBody] int OfertaId)
        {
            var OfertaDefinitiva = _ofertaService.GetOfertaDefinitiva(RequerimientoId);
            if (OfertaDefinitiva == null)
            {
                return Content("ErrorDefinitiva");
            }
            var clonado = _wbsService.ClonarWbs(OfertaDefinitiva.Id, OfertaId);

            return Content(clonado ? OfertaDefinitiva.Id + "" : "ErrorComputos");
        }

        // Formulario HTML
        [System.Web.Mvc.HttpPost]
        public async Task<ActionResult> Eliminar(int? id)
        {
            if (id.HasValue)
            {
                var wbs = await _wbsService.Get(new EntityDto<int>(id.Value));
                wbs.vigente = false;
                await _wbsService.Update(wbs);
                return RedirectToAction("IndexWbs", "Wbs", new { id = wbs.OfertaId });
            }

            return RedirectToAction("Index", "Inicio", new { area = "" });
        }




        public ActionResult ExportarExcel(int id) //Pasar pametros oferta 
        {
            var ofertac = _ofertaService.getdetalle(id);

            ExcelPackage excel = _wbsService.GenerarExcelCargaFechas(ofertac);

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


        public ActionResult CargaExcelFechas(int id)
        {
            ViewBag.idoferta = id;
            return View();
        }

        [System.Web.Mvc.HttpPost]
        public async Task<ActionResult> CargaExcelFechas(HttpPostedFileBase UploadedFile, int id)
        {
            List<String[]> Observaciones = new List<string[]>();

            var statusfecha = _wbsService.VerficarExcelFechas(UploadedFile);

            if (statusfecha == "OK/")
            {


                if (UploadedFile != null)
                {

                    // tdata.ExecuteCommand("truncate table OtherCompanyAssets");  
                    if (UploadedFile.ContentType == "application/vnd.ms-excel" || UploadedFile.ContentType ==
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                    {
                        string fileName = UploadedFile.FileName;
                        string fileContentType = UploadedFile.ContentType;
                        byte[] fileBytes = new byte[UploadedFile.ContentLength];
                        var data = UploadedFile.InputStream.Read(fileBytes, 0,
                            Convert.ToInt32(UploadedFile.ContentLength));

                        using (var package = new ExcelPackage(UploadedFile.InputStream))
                        {
                            var currentSheet = package.Workbook.Worksheets;
                            var workSheet = currentSheet.First();
                            var noOfCol = workSheet.Dimension.End.Column;

                            var noOfRow = workSheet.Dimension.End.Row;

                            for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                            {

                                var disciplinaid = (workSheet.Cells[rowIterator, 1].Value ?? "").ToString();
                                var fechainicial = (workSheet.Cells[rowIterator, 4].Text ?? "01/01/1999").ToString();
                                var fechafinal = (workSheet.Cells[rowIterator, 5].Text ?? "01/01/1999").ToString();

                                if (disciplinaid.Length > 0 && fechainicial.Length > 0 && fechafinal.Length > 0 && fechafinal != "01/01/1999" && fechainicial != "01/01/1999")
                                {

                                    var wbs = await _wbsService.Get(new EntityDto<int>(Int32.Parse(disciplinaid)));

                                    if (wbs != null && wbs.es_actividad == true)
                                    {
                                        wbs.fecha_inicial = DateTime.Parse(fechainicial);
                                        wbs.fecha_final = DateTime.Parse(fechafinal);
                                        var r = await _wbsService.Update(wbs);

                                    }




                                }

                            }

                            if (Observaciones.Count > 0)
                            {

                                ExcelPackage excel = new ExcelPackage();
                                var errores = excel.Workbook.Worksheets.Add("Observaciones");
                                errores.Cells[1, 1].Value = "Codigo Item";
                                errores.Cells[1, 2].Value = "Observacion";
                                workSheet.Column(1).Width = 20;
                                workSheet.Column(2).Width = 60;
                                var row = 2;
                                foreach (var pitem in Observaciones)
                                {
                                    errores.Cells[row, 1].Value = pitem[0].ToString();
                                    errores.Cells[row, 2].Value = pitem[1].ToString();

                                    row = row + 1;
                                }
                                string excelName = "Observaciones";
                                using (var memoryStream = new MemoryStream())
                                {
                                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                                    Response.AddHeader("content-disposition", "attachment; filename=" + excelName + ".xlsx");
                                    excel.SaveAs(memoryStream);
                                    memoryStream.WriteTo(Response.OutputStream);
                                    Response.Flush();
                                    Response.End();
                                }
                                ViewBag.Error = "Tiene Observaciones Verifique";
                                ViewBag.idoferta = id;
                                return View();
                            }
                            else
                            {
                                ViewBag.Msg = "Cargado Correctamente";
                                ViewBag.idoferta = id;
                                return View();

                            }
                        }

                    }
                    else
                    {


                        ViewBag.Error = "El Formato del Archivo Subido debe ser en formato Excel";
                        ViewBag.idoferta = id;
                        return View();


                    }
                }

                return View();

            }
            else {
                var res = statusfecha.Split('/');
                ViewBag.Error = "Verifique la fila "+res[1]+ "La Fecha Final no puede ser menor a la Inicial";
                ViewBag.idoferta = id;
                return View();
            }

        }



        [System.Web.Mvc.HttpPost]
        public ActionResult ApiWbsK(int id)
        {
            var x = _wbsService.ObtenerKeysArbol(id);
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
            var lista = _wbsService.GenerarArbolDrag(id.Value);
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
            var x = _wbsService.GuardarArbolDrag(data);
            return Content(x);
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult CopiarWbs([FromBody]int origen, [FromBody]int destino, [FromBody]int PresupuestoId, [FromBody]int OfertaId)
        {
            var result = _wbsService.CopiarWBS(OfertaId, PresupuestoId, destino, origen);
            return Content(result ? "OK" : "ERROR");
        }
    }
}
