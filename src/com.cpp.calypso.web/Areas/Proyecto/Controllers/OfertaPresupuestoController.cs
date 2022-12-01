using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Abp.Application.Services.Dto;
using Abp.Runtime.Validation;
using com.cpp.calypso.framework;
using com.cpp.calypso.proyecto.aplicacion;
using com.cpp.calypso.proyecto.aplicacion.Interfaces;
using com.cpp.calypso.proyecto.dominio;
using Newtonsoft.Json;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace com.cpp.calypso.web.Areas.Proyecto.Controllers
{
    public class OfertaPresupuestoController : BaseController
    {
        private readonly IPresupuestoAsyncBaseCrudAppService _presupuestoService;
        private readonly IOfertaComercialAsyncBaseCrudAppService _ofertaPresupuestoService;
        private readonly IComputoPresupuestoAsyncBaseCrudAppService _computoPresupuestoService;
        private readonly IWbsPresupuestoAsyncBaseCrudAppService _wbsPresupuestoService;
        private readonly IItemAsyncBaseCrudAppService _itemService;
        private readonly IRequerimientoAsyncBaseCrudAppService _requerimientoService;


        // GET: Proyecto/OfertaPresupuesto
        public OfertaPresupuestoController(
            IHandlerExcepciones manejadorExcepciones,
            IPresupuestoAsyncBaseCrudAppService presupuestoService,
            IOfertaComercialAsyncBaseCrudAppService ofertaPresupuestoService,
            IComputoPresupuestoAsyncBaseCrudAppService computoPresupuestoService,
            IWbsPresupuestoAsyncBaseCrudAppService wbsPresupuestoService,
            IItemAsyncBaseCrudAppService itemService,
            IRequerimientoAsyncBaseCrudAppService requerimientoService

            ) : base(manejadorExcepciones)
        {
            _presupuestoService = presupuestoService;
            _ofertaPresupuestoService = ofertaPresupuestoService;
            _computoPresupuestoService = computoPresupuestoService;
            _wbsPresupuestoService = wbsPresupuestoService;
            _itemService = itemService;
            _requerimientoService = requerimientoService;
        }

        public ActionResult Index(int RequerimientoId = 0)
        {
            ViewBag.ruta = new string[] { "Inicio", "Presupuesto", "Listado" };
            ViewBag.RequerimientoId = RequerimientoId;
            return View();
        }
        public ActionResult IndexRequerimientos()
        {
            ViewBag.ruta = new string[] { "Inicio", "Presupuesto", "Listado de Requerimientos en Cola" };

            return View();
        }


        [System.Web.Mvc.HttpPost]
        public async Task<ActionResult> CreatePresupuesto(PresupuestoDto presupuesto)
        {
            if (ModelState.IsValid)
            {
                var id = await _presupuestoService.CrearPresupuesto(presupuesto);
                var p = await _presupuestoService.Get(new EntityDto<int>(id));
                var x = _requerimientoService.cambiar_estado_requerimiento(p, 4179);
                return Content(id + "");
            }
            return Content("Error");
        }



#pragma warning disable CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.f
        public async Task<ActionResult> Details(int? id)
#pragma warning restore CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        {
            if (id.HasValue)
            {
                ViewBag.ruta = new string[] { "Inicio", "Presupuesto", id.Value + "", "Detalle" };
                ViewBag.PresupuestoId = id.Value;
                return View();
            }

            return RedirectToAction("Index", "Inicio", new { area = "" });
        }

        public ActionResult DetailsPresupuestoApi(int id)
        {
            var presupuesto = _presupuestoService.DetallePresupuestoConEnumerable(id);
            var result = JsonConvert.SerializeObject(presupuesto);
            return Content(result);
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult EditPresupuesto(PresupuestoDto presupuesto)
        {
            _presupuestoService.ActualizarPresupuesto(presupuesto);
            var x = _requerimientoService.cambiar_estado_requerimiento(presupuesto, 4179);
            return Content("Ok");
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult EditPresupuestoEmail(PresupuestoDto presupuesto)
        {
            _presupuestoService.ActualizarPresupuestoEmail(presupuesto);
  
            return Content("Ok");
        }

        public ActionResult GetMailto(int Id)
        {
            var result = _presupuestoService.hrefoutlook(Id);
            return Content(result);
        }


        [System.Web.Mvc.HttpPost]
        public async Task<ActionResult> AprobarPresupuesto(int id) // PresupuestoId
        {
            /*comprueba si esta generado Presupuesto
            var generadopresupuesto = _presupuestoService.estageneradopresupuesto(id);
            if (!generadopresupuesto) {
                return Content("NO_GENERADO");
            }
            */
            try
            {
                var actualizado = await _presupuestoService.AprobarPresupuesto(id);
                var presupuesto = await _presupuestoService.Get(new EntityDto<int>(id));

                var x = _requerimientoService.cambiar_estado_requerimiento(presupuesto, 4180);
                _presupuestoService.EnviarMontosPresupuestoReq(presupuesto.Id);
                return Content(actualizado ? "Si" : "No");
            }
            catch (AbpValidationException ex)
            {
                var e = ex.ValidationErrors
                        .Select(err => err.ToString())
                        .Aggregate(string.Empty, (current, next) => string.Format("{0}\n{1}", current, next));
            }

            return Content("");
        }


        [System.Web.Mvc.HttpPost]
        public async Task<ActionResult> DesaprobarPresupuesto(int id) // PresupuestoId
        {
            var actualizado = await _presupuestoService.DesaprobarPresupuesto(id);
            var presupuesto = await _presupuestoService.Get(new EntityDto<int>(id));
            var x = _requerimientoService.cambiar_estado_requerimiento(presupuesto, 4179);
            _presupuestoService.EnviarMontosPresupuestoReq(presupuesto.Id);
            return Content(actualizado ? "Si" : "No");
        }

        public ActionResult ListarPorRequerimiento(int id) //RequerimientoId
        {
            var presupuestos = _presupuestoService.ListarPorRequerimiento(id);
            var result = JsonConvert.SerializeObject(presupuestos);
            return Content(result);
        }


        [HttpPost]
        public ActionResult NuevaVersion(PresupuestoDto presupuesto) //RequerimientoId
        {
            var result = _presupuestoService.CrearNuevaVersion(presupuesto);
            // cambia estado requermiento a en Proceso
            return Content(result);
        }


        public async Task<ActionResult> ExportarE(int id) //Pasar pametros presupuesto  .. FORMATO CARGA EXCEL COMPUTOS PRESPUEPUESTO
        {

            var ofertac = await _presupuestoService.Get(new Abp.Application.Services.Dto.EntityDto<int>(id));
            int maximo_nivel = _wbsPresupuestoService.nivel_mas_alto(ofertac.Id);
            IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);

            ExcelPackage excel = _presupuestoService.GenerarExcelCarga(ofertac, maximo_nivel);

            try
            {

                string excelName = "Formato" + ofertac.Proyecto.codigo + "-" + ofertac.Requerimiento.codigo + "-" + DateTime.Now.Year + "" + DateTime.Now.Month + "" + DateTime.Now.Day;
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
            catch (Exception e)
            {
                return Content("");
            }




        }


        public async Task<ActionResult> GenerarPrespuesto(int OfertaId)
        {
            var ofertac = await _presupuestoService.Get(new Abp.Application.Services.Dto.EntityDto<int>(OfertaId));

            var computos = _computoPresupuestoService.GetComputosPorPresupuesto(OfertaId);
            var wbs = _wbsPresupuestoService.Listar(OfertaId);

            var excel = _computoPresupuestoService.GenerarExcelCabecera(ofertac);
            var workSheet = excel.Workbook.Worksheets[1];

            int columna = 5;
            int c = 10;
            workSheet.View.ShowGridLines = true;
            workSheet.View.FreezePanes(5, 1);
            workSheet.View.FreezePanes(10, 5);
            foreach (var itemswbs in wbs)
            {


                workSheet.Cells[6, columna].Value = itemswbs.nombre_padre;
                workSheet.Cells[7, columna].Value = itemswbs.nivel_nombre;
                workSheet.Cells[8, columna].Value = itemswbs.Id;
                workSheet.Cells[6, columna].Style.Fill.PatternType = ExcelFillStyle.Solid;
                workSheet.Cells[6, columna].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                workSheet.Cells[7, columna].Style.Fill.PatternType = ExcelFillStyle.Solid;
                workSheet.Cells[7, columna].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                workSheet.Cells[9, columna].Style.Fill.PatternType = ExcelFillStyle.Solid;
                workSheet.Cells[9, columna].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                workSheet.Cells[6, 2].Style.Fill.PatternType = ExcelFillStyle.Solid;
                workSheet.Cells[6, 2].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                workSheet.Cells[6, 3].Style.Fill.PatternType = ExcelFillStyle.Solid;
                workSheet.Cells[6, 3].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                workSheet.Cells[6, 4].Style.Fill.PatternType = ExcelFillStyle.Solid;
                workSheet.Cells[6, 4].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                workSheet.Cells[7, 2].Style.Fill.PatternType = ExcelFillStyle.Solid;
                workSheet.Cells[7, 2].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                workSheet.Cells[7, 3].Style.Fill.PatternType = ExcelFillStyle.Solid;
                workSheet.Cells[7, 3].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                workSheet.Cells[7, 4].Style.Fill.PatternType = ExcelFillStyle.Solid;
                workSheet.Cells[7, 4].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                workSheet.Cells[9, 2].Style.Fill.PatternType = ExcelFillStyle.Solid;
                workSheet.Cells[9, 2].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                workSheet.Cells[9, 3].Style.Fill.PatternType = ExcelFillStyle.Solid;
                workSheet.Cells[9, 3].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                workSheet.Cells[9, 4].Style.Fill.PatternType = ExcelFillStyle.Solid;
                workSheet.Cells[9, 4].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                workSheet.Column(columna).Width = 10;
                workSheet.Column(columna).Width = 30;
                columna = columna + 1;
            }

            //  var itemss = _itemservice.ArbolWbsExcel(ofertac.Proyecto.contratoId, ofertac.fecha_oferta.Value);

            var itemss = _itemService.GetItems();
            workSheet.Cells[9, 1].Value = "ID";
            workSheet.Cells["B6:B9"].Merge = true;
            workSheet.Cells["B6:B9"].Value = "ITEM";
            workSheet.Cells["B6:B9"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            workSheet.Cells["B6:B9"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            workSheet.Cells["C6:C9"].Merge = true;
            workSheet.Cells["C6:C9"].Value = "DESCRIPCIÓN";
            workSheet.Cells["C6:C9"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            workSheet.Cells["C6:C9"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            workSheet.Cells["D6:D9"].Merge = true;
            workSheet.Cells["D6:D9"].Value = "UNIDAD";
            workSheet.Cells["B6:D9"].AutoFilter = true;
            workSheet.Cells["D6:D9"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            workSheet.Cells["D6:D9"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            workSheet.Cells["B6:D9"].Style.Font.SetFromFont(new System.Drawing.Font("Arial", 9, FontStyle.Bold));
            workSheet.Cells["B10:B1569"].Style.Font.SetFromFont(new System.Drawing.Font("Arial", 9, FontStyle.Bold));
            workSheet.Column(3).Width = 90;

            workSheet.Column(1).Style.Font.Color.SetColor(Color.White);

            workSheet.Row(8).Hidden = true;

            foreach (var pitem in itemss)


            {
                workSheet.Cells[c, 1].Value = pitem.Id;
                workSheet.Cells[c, 2].Value = pitem.codigo;
                workSheet.Cells[c, 3].Value = pitem.nombre;
                workSheet.Cells[c, 4].Value = _computoPresupuestoService.nombrecatalogo2(pitem.UnidadId);



                c = c + 1;

            }
            var noOfCol = workSheet.Dimension.End.Column;
            var noOfRow = workSheet.Dimension.End.Row;



            for (int j = 5; j <= noOfCol; j++)
            {
                var wbsid = (workSheet.Cells[8, j].Value ?? "").ToString();
                for (int i = 10; i <= noOfRow; i++)
                {
                    var itemid = (workSheet.Cells[i, 1].Value ?? "").ToString();

                    foreach (var items in computos)
                    {
                        if (Convert.ToString(items.WbsPresupuestoId) == wbsid && Convert.ToString(items.ItemId) == itemid)
                        {

                            workSheet.Cells[i, j].Value = items.cantidad;

                        }

                    }


                }

            }


            string excelName = "Computos Cantidades";
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


        public async Task<ActionResult> SubirExcel(int id)
        {
            var ofertac = await _presupuestoService.Get(new Abp.Application.Services.Dto.EntityDto<int>(id));
            ViewBag.OfertaId = ofertac.Id;
            ViewBag.Contratoid = ofertac.Proyecto.contratoId;
            ViewBag.Fechaoferta = ofertac.fecha_registro.Value.ToShortDateString();
            ViewBag.ruta = new string[] { "Inicio", "Ingeniería", "Computo", ofertac.codigo + "-" + ofertac.version, "Carga Masiva" };
            return View();
        }

        [System.Web.Mvc.HttpPost]
        public async Task<ActionResult> SubirExcel(HttpPostedFileBase UploadedFile, int ofertaid)
        {

            if (UploadedFile != null)
            {



                int maximo_nivel = _wbsPresupuestoService.nivel_mas_alto(ofertaid);

                var items_procura = _itemService.ObtenerItemsProcuraporPresupuesto(ofertaid);

                var validarnombres = _presupuestoService.ValidarItemsProcuraExcel(UploadedFile, ofertaid, maximo_nivel, items_procura);

                if (validarnombres == "Ok")
                {

                }
                else
                {
                    String[] resultados = { "Error", validarnombres };
                    var result = JsonConvert.SerializeObject(resultados);
                    return Content(result);
                }


                var mprocura = _presupuestoService.IngresarItemsProcuraExce(UploadedFile, maximo_nivel, ofertaid);

                if (mprocura == "Ok")
                {

                }
                else
                {
                    String[] resultados = { "Error", mprocura };
                    var result = JsonConvert.SerializeObject(resultados);
                    return Content(result);
                }
                var msub = _presupuestoService.IngresarItemsSubContratosExce(UploadedFile, maximo_nivel, ofertaid);
                if (msub == "Ok")
                {

                }
                else
                {
                    String[] resultados = { "Error", msub };
                    var result = JsonConvert.SerializeObject(resultados);
                    return Content(result);
                }

                //AQUI SE SUBE LOS NUEVOS ITEMS DEL GRUPO 6 


                var mitemsgrupos = _presupuestoService.IngresarItemsPresupuestoExcel(UploadedFile, maximo_nivel, ofertaid);

                if (mitemsgrupos == "Ok")
                {
                    String[] resultados = { "OK", "Cargado Correctamente" };
                    var result = JsonConvert.SerializeObject(resultados);
                    return Content(result);

                }
                else
                {
                    String[] resultados = { "Error", "Ocurrió un error su transacción no se completo correctamente" };
                    var result = JsonConvert.SerializeObject(resultados);
                    return Content(result);
                }

            }
            String[] resultado = { "SA", "Error sin Archivo" };
            var resulta = JsonConvert.SerializeObject(resultado);
            return Content(resulta);


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

            var statusfecha = _wbsPresupuestoService.VerficarExcelFechas(UploadedFile);

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
                                var fechainicial = (workSheet.Cells[rowIterator, 4].Value ?? "01/01/1999").ToString();
                                var fechafinal = (workSheet.Cells[rowIterator, 5].Value ?? "01/01/1999").ToString();


                                if (disciplinaid.Length > 0 && fechainicial.Length > 0 && fechafinal.Length > 0 && fechafinal != "01/01/1999" && fechainicial != "01/01/1999")
                                {

                                    var wbs = await _wbsPresupuestoService.Get(new EntityDto<int>(Int32.Parse(disciplinaid)));


                                    if (wbs != null && wbs.es_actividad == true)
                                    {


                                        wbs.fecha_inicial = DateTime.Parse(fechainicial);
                                        wbs.fecha_final = DateTime.Parse(fechafinal);
                                        var r = await _wbsPresupuestoService.Update(wbs);
                                        var pre = await _presupuestoService.Get(new EntityDto<int>(wbs.PresupuestoId));
                                        var requeremiento = await _requerimientoService.Get(new EntityDto<int>(pre.RequerimientoId));
                                        requeremiento.fecha_carga_cronograma = DateTime.Now;
                                        var req = await _requerimientoService.Update(requeremiento);

                                    }

                                }

                            }


                        }

                    }

                }

                return Content("OK");

            }
            else
            {
                return Content(statusfecha);
            }
        }




        [System.Web.Mvc.HttpPost]
        public ActionResult CambiarComputoAprobado(int id) //PresupuestoId
        {
            var result = _presupuestoService.CambiarComputoCompleto(id);
            if (result)
            {
                return RedirectToAction("Index", "ComputoPresupuesto", new { id = id, mensaje = "Computos Aprobados" });
            }
            else
            {
                return RedirectToAction("Index", "ComputoPresupuesto", new { id = id, mensaje = "Computos Desaprobados" });
            }
        }


        [System.Web.Mvc.HttpPost]
#pragma warning disable CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        public async Task<ActionResult> ActualizarCostos(int Id)
#pragma warning restore CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        {

            var presupuesto = _presupuestoService.ActualizarCostos(Id);
            var contrato = _presupuestoService.ObtenerContratoFromPresupuesto(Id);
            if (contrato.Formato == FormatoContrato.Contrato_2016)
            {
                _presupuestoService.CalcularMontosPresupuesto(Id);

            }
            else
            {
                var data = _presupuestoService.TotalesSecondFormatPresupuesto(Id);
                bool req = _requerimientoService.actualizarmontosrequerimiento(Id, data.A_VALOR_COSTO_TOTAL_INGENIERÍA_BASICA_YDETALLE_AIU_ANEXO1, data.D_VALOR_COSTO_DIRECTO_CONSTRUCCIÓN, data.B_VALOR_COSTO_DIRECTO_PROCURA_CONTRATISTA_ANEXO2, data.C_VALOR_COSTO_DIRECTO_SUBCONTRATOS_CONTRATISTA, data.COSTO_TOTAL_DEL_PROYECTO_ABCDE);
            }
            var result = JsonConvert.SerializeObject(presupuesto,
                Newtonsoft.Json.Formatting.None,
                new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    NullValueHandling = NullValueHandling.Ignore
                });
            return Content(result);

        }
        public async Task<ActionResult> MontosPresupuestos(int id)
        {
            var ofertac = await _presupuestoService.Get(new Abp.Application.Services.Dto.EntityDto<int>(id));
            _presupuestoService.CalcularMontosPresupuesto(ofertac.Id);
            decimal[] Observaciones = new decimal[10];

            //Montos Presupuesto
            decimal montopconstrucion = ofertac.monto_construccion;
            decimal montopingenieria = ofertac.monto_ingenieria;
            decimal montopprocura = ofertac.monto_suministros;
            decimal totalp = montopconstrucion + montopingenieria + montopprocura;

            //Montos Certificados
            decimal montoa = montopconstrucion * (Convert.ToDecimal(0.4119));
            decimal montoi = montopconstrucion * (Convert.ToDecimal(0.03));
            decimal montou = montopconstrucion * (Convert.ToDecimal(0.12));
            decimal montopc = montopprocura * (Convert.ToDecimal(0.10));
            decimal total = montoa + montoi + montou + montopc;

            //Montos //saldos


            decimal totals = totalp + total;


            Observaciones[0] = montopconstrucion;
            Observaciones[1] = montopingenieria;
            Observaciones[2] = montopprocura;
            Observaciones[3] = totalp;
            Observaciones[4] = montoa;
            Observaciones[5] = montoi;
            Observaciones[6] = montou;
            Observaciones[7] = montopc;
            Observaciones[8] = total;
            Observaciones[9] = totals;

            var result = JsonConvert.SerializeObject(Observaciones,
                Newtonsoft.Json.Formatting.None,
                new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    NullValueHandling = NullValueHandling.Ignore
                });
            return Content(result);
        }
        [System.Web.Mvc.HttpPost]
        public ActionResult ObtenerValoresNegativos(HttpPostedFileBase UploadedFile, int ofertaid)
        {

            if (UploadedFile != null)
            {

                int maximo_nivel = _wbsPresupuestoService.nivel_mas_alto(ofertaid);    
                    var list = _presupuestoService.ValidarNumerosNegativos(UploadedFile, maximo_nivel, ofertaid);

                if (list.Count > 0)
                {
                    String[] resultados = { "NEGATIVOS", JsonConvert.SerializeObject(list) };
                    var result = JsonConvert.SerializeObject(resultados);
                    return Content(result);

                }
                else {
                    String[] resultados = { "SIN_NEGATIVOS", "OK" };
                    var result = JsonConvert.SerializeObject(resultados);
                    return Content(result);
                }
                
            }
            String[] resultado = { "SA", "Error sin Archivo" };
            var resulta = JsonConvert.SerializeObject(resultado);
            return Content(resulta);




        }
        public ActionResult ListarRequerimientoenCola() //RequerimientoId
        {
            var presupuestos = _presupuestoService.ListadoRequerimientosCola();
            var result = JsonConvert.SerializeObject(presupuestos,
                Newtonsoft.Json.Formatting.None,
                new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    NullValueHandling = NullValueHandling.Ignore

                });
            return Content(result);
        }





        [System.Web.Http.HttpPost]
        public ActionResult CreateArchivo(int Id, HttpPostedFileBase UploadedFile)
        {
            var dataid = _presupuestoService.GuardarArchivo(Id, UploadedFile);
            if (dataid > 0)
            {
                return Content("OK");

            }
            else
            {
                return Content("ERROR");
            }
        }

        [System.Web.Http.HttpPost]
        public ActionResult DeleteArchivo(int? id) // ArchivoAvanceObraId
        {
            if (!id.HasValue)
            {
                return Content("ERROR");
            }

            int avanceid = _presupuestoService.EliminarArchivo(id.Value);
            return Content("OK");
        }
        public ActionResult UploadFileList(int id) //lISTA DE COLABORADORES
        {
            var list = _presupuestoService.ListaArchivos(id);
            var result = JsonConvert.SerializeObject(list,
          Newtonsoft.Json.Formatting.None,

          new JsonSerializerSettings
          {
              ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
              NullValueHandling = NullValueHandling.Ignore,


          });
            return Content(result);
        }

        public ActionResult DescargarArchivo(int Id)
        {
            var entity = _presupuestoService.DetalleArchivo(Id);
            return File(entity.hash, entity.tipo_contenido, entity.nombre);
        }


    }
}