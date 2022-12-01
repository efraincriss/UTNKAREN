using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
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
using OfficeOpenXml.Style;
using Action = Antlr.Runtime.Misc.Action;

namespace com.cpp.calypso.web.Areas.Proyecto.Controllers
{

    public class DetalleAvanceObraController : BaseController
    {
        private readonly IDetalleAvanceObraAsyncBaseCrudAppService _detalleAvanceObraService;
        private readonly IAvanceObraAsyncBaseCrudAppService _avanceObraService;
        private readonly IOfertaAsyncBaseCrudAppService _ofertaService;
        private readonly IComputoAsyncBaseCrudAppService _computoService;

        public DetalleAvanceObraController(
            IHandlerExcepciones manejadorExcepciones,
            IDetalleAvanceObraAsyncBaseCrudAppService detalleAvanceObraService,
            IAvanceObraAsyncBaseCrudAppService avanceObraService,
            IOfertaAsyncBaseCrudAppService ofertaService,
            IComputoAsyncBaseCrudAppService computoService
            ) : base(manejadorExcepciones)
        {
            _detalleAvanceObraService = detalleAvanceObraService;
            _avanceObraService = avanceObraService;
            _ofertaService = ofertaService;
            _computoService = computoService;
        }

        public ActionResult IndexSeleccionComputo(int? id) // Oferta Id
        {
            if (!id.HasValue)
            {
                return RedirectToAction("Index", "Inicio", new { area = "" });
            }
            var computos = _detalleAvanceObraService.ListaComputosPorOferta(id.Value);
            return View(computos);
        }


        public async Task<ActionResult> CreateDetalleAvance(int? id) // AvanceObra Id
        {
            if (id.HasValue)
            {
                var avance = await _avanceObraService.Get(new EntityDto<int>(id.Value));
                var oferta = await _ofertaService.Get(new EntityDto<int>(avance.OfertaId));
                CreateDetalleAvanceObraViewModel viewModel = new CreateDetalleAvanceObraViewModel()
                {
                    OfertaId = avance.OfertaId,
                    AvanceObraId = id.Value,
                    codigo_avance_obra = avance.Id + "",
                    fecha_presentacion = avance.fecha_presentacion.GetValueOrDefault(),
                    codigo_oferta = oferta.codigo,
                    nombre_proyecto = oferta.Proyecto.nombre_proyecto
                };
                ViewBag.ruta = new string[] { "Inicio", "Construcción", "Avance Obra", avance.Oferta.codigo, "Detalles Avance Obra", "Crear Detalle" };
                return View(viewModel);
            }
            return RedirectToAction("Index", "Inicio", new { area = "" });
        }

        [System.Web.Mvc.HttpPost]
        public async Task<ActionResult> CreateDetalleAvance(DetalleAvanceObraDto detalle, [FromBody] decimal cantidad_eac) // AvanceObra Id
        {
            var id = await _detalleAvanceObraService.CreateDetalleAvance(detalle, cantidad_eac);

            if (id == -1)
            {
                return Content("ErrorDuplicate");
            }
            else
            {
                decimal total = _detalleAvanceObraService.calcularvalor(detalle.AvanceObraId);
                return Content(id > 0 ? "OK" : "Error");
            }
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult GetComputos(int id, [FromBody] DateTime fecha_presentacion) // Wbs Oferta
        {
            var computos = _computoService.GetComputosporWbsOferta(id, fecha_presentacion);
            var result = JsonConvert.SerializeObject(computos);
            return Content(result);
        }


        public async Task<ActionResult> Edit(int? id) // DetalleAvanceObraId
        {
            if (id.HasValue)
            {
                var detalle = await _detalleAvanceObraService.Get(new EntityDto<int>(id.Value));
                return View(detalle);
            }
            return RedirectToAction("Index", "Inicio", new { area = "" });
        }

        [System.Web.Mvc.HttpPost]
        public async Task<ActionResult> Edit(DetalleAvanceObraDto detalle) // DetalleAvanceObraId
        {
            if (ModelState.IsValid)
            {
                detalle.cantidad_diaria = detalle.cantidad_acumulada - detalle.cantidad_acumulada_anterior;
                detalle.total = (detalle.cantidad_acumulada - detalle.cantidad_acumulada_anterior) *
                                detalle.precio_unitario;

                var edited = await _detalleAvanceObraService.Update(detalle);
                decimal total = _detalleAvanceObraService.calcularvalor(detalle.AvanceObraId);
                return RedirectToAction("Details", "AvanceObra", new { id = edited.AvanceObraId });
            }

            return View("Edit", detalle);
        }



        public async Task<ActionResult> Create(int? id) //AvanceObraId
        {
            if (id.HasValue)
            {
                var avance = await _avanceObraService.Get(new EntityDto<int>(id.Value));
                ViewBag.ruta = new string[] { "Inicio", "Construcción", "Avance Obra", avance.Oferta.codigo, "Detalles Avance Obra", "Carga Masiva" };
                return View(avance);
            }

            return RedirectToAction("Index", "Inicio", new { area = "" });
        }

        [System.Web.Mvc.HttpPost]
        public async Task<ActionResult> Create([FromBody] List<ComputoAvanceObra> data, [FromBody] int AvanceObraId) //AvanceObraId
        {
            var total = await _detalleAvanceObraService.GuardarDetalles(data, AvanceObraId);
            return Content(total + "");
        }
        [System.Web.Mvc.HttpPost]
        public async Task<ActionResult> CreateAvancesNegativos([FromBody] ComputoAvanceObra data, [FromBody] int AvanceObraId) //AvanceObraId
        {
            var total = await _detalleAvanceObraService.GuardarDetallesNegativos(data, AvanceObraId);
            return Content(total + "");
        }


        [System.Web.Mvc.HttpPost]
        public ActionResult Delete(int id) // DetalleId
        {
            var AvanceObraId = _detalleAvanceObraService.Eliminar(id);

            return Content("Ok");
        }

        [System.Web.Mvc.HttpPost]
        public async Task<ActionResult> CreateExcelAvance(HttpPostedFileBase UploadedFile, int Id) //Base Rdo
        {
            if (UploadedFile != null)
            {
                if (UploadedFile.ContentType == "application/vnd.ms-excel" || UploadedFile.ContentType ==
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                {
                    List<String> errores = _detalleAvanceObraService.ValidarSubirExcelAvanceObra(UploadedFile,Id);
                    if (errores.Count > 0)
                    {
                       /* ExcelPackage excel = new ExcelPackage();
                        string nombretab = "Errores ";
                        var workSheet = excel.Workbook.Worksheets.Add(nombretab);

                        //PRIMERA HOJA
                        workSheet.TabColor = System.Drawing.Color.Navy;
                        workSheet.DefaultRowHeight = 15;
                        workSheet.View.ZoomScale = 80;



                        workSheet.Cells[1, 1].Value = "Id";
                        workSheet.Cells[1, 2].Value = "Error";
                        workSheet.Cells[1, 2].Style.WrapText = true;
                        workSheet.Cells[1, 2].Style.Font.Bold = true;
                        workSheet.Cells[1, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        workSheet.Cells[1, 2].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        workSheet.Cells[1, 2].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        workSheet.Cells[1, 2].Style.Fill.BackgroundColor.SetColor(Color.DodgerBlue);
                        workSheet.Cells[1, 2].Style.Font.Color.SetColor(Color.White);
                        workSheet.Column(1).Width = 30;
                        workSheet.Column(2).Width = 70;
                        int fila = 2;
                        foreach (var item in errores)
                        {
                            workSheet.Cells[fila, 1].Value = fila;
                            workSheet.Cells[fila, 2].Value = item;
                            fila = fila++;
                        }
                        string excelName = "Errores Subida Avance  de Obra";
                        using (var memoryStream = new MemoryStream())
                        {
                            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                            Response.AddHeader("content-disposition", "attachment; filename=" + excelName + ".xlsx");
                            excel.SaveAs(memoryStream);
                            memoryStream.WriteTo(Response.OutputStream);
                            Response.Flush();
                            Response.End();

                        }
                       */
                        return Content("DESCARGAR");
                    }
                    else
                    {
                        string resultado = await _detalleAvanceObraService.SubirExcelAvanceObra(UploadedFile, Id);
                        return Content(resultado);
                    }
                }
                else
                {
                    return Content("SIN_FORMATO");
                }

            }
            else
            {
                return Content("SIN_ARCHIVO");

            }
        }

        public async Task<ActionResult> GetExcelCargaIDS(int? id) // AvanceObraId
        {
            var base_rdo = await _ofertaService.Get(new Abp.Application.Services.Dto.EntityDto<int>(id.Value));
            ExcelPackage excel = _avanceObraService.CargaMasivaIDSWBS(base_rdo.Id);


            string excelName = "Formato Carga IDs-" +DateTime.Now.ToShortDateString();
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
        public async Task<ActionResult> CreateExcelIds(HttpPostedFileBase UploadedFile, int Id) //Base Rdo
        {
            if (UploadedFile != null)
            {
                if (UploadedFile.ContentType == "application/vnd.ms-excel" || UploadedFile.ContentType ==
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                {
                    var base_rdo = await _ofertaService.Get(new Abp.Application.Services.Dto.EntityDto<int>(Id));
                    string resultado = await _detalleAvanceObraService.CargarArchivoIDS(UploadedFile, base_rdo.Id);
                        return Content(resultado);
                    
                }
                else
                {
                    return Content("SIN_FORMATO");
                }

            }
            else
            {
                return Content("SIN_ARCHIVO");

            }
        }



    }
}
