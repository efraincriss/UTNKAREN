using Abp.Application.Services.Dto;
using AutoMapper;
using com.cpp.calypso.framework;
using com.cpp.calypso.proyecto.aplicacion;
using com.cpp.calypso.proyecto.aplicacion.Interfaces;
using com.cpp.calypso.web.Areas.Proyecto.Models;
using com.cpp.calypso.web.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;
using System.Globalization;

namespace com.cpp.calypso.web.Areas.Proyecto.Controllers
{

    public class PresupuestoController : BaseController
    {
        private readonly IComputoAsyncBaseCrudAppService computoService;
        private readonly IOfertaAsyncBaseCrudAppService ofertaService;

        private readonly IWbsAsyncBaseCrudAppService wbsofertaService;
        private readonly IDetallePreciarioAsyncBaseCrudAppService detallepreciarioService;
        private readonly IItemAsyncBaseCrudAppService itemservice;
        private readonly IPreciarioAsyncBaseCrudAppService preciarioService;
        private readonly ICatalogoAsyncBaseCrudAppService _catalogoService;
        private readonly IGananciaAsyncBaseCrudAppService _gananciaService;
        //Presupuesto//

        private readonly IPresupuestoAsyncBaseCrudAppService _presupuestoService;
        private readonly IComputoPresupuestoAsyncBaseCrudAppService _computoPresupuestoService;
        private readonly IWbsPresupuestoAsyncBaseCrudAppService _WbsPresupuestoService;

        IContratoAsyncBaseCrudAppService _ContratoService;
        public PresupuestoController(IHandlerExcepciones manejadorExcepciones,
            IComputoAsyncBaseCrudAppService _computoService,
            IOfertaAsyncBaseCrudAppService ofertaService,
            IWbsAsyncBaseCrudAppService wbsofertaService,
            IItemAsyncBaseCrudAppService itemservice,
            IDetallePreciarioAsyncBaseCrudAppService detallepreciarioService,
            IPreciarioAsyncBaseCrudAppService preciarioService,
            ICatalogoAsyncBaseCrudAppService catalogoService,
            IGananciaAsyncBaseCrudAppService gananciaService,
            IWbsPresupuestoAsyncBaseCrudAppService WbsPresupuestoService,
            IContratoAsyncBaseCrudAppService ContratoService,

        //Presupuesto

        IPresupuestoAsyncBaseCrudAppService presupuestoService,
            IComputoPresupuestoAsyncBaseCrudAppService computoPresupuestoService
        ) : base(manejadorExcepciones)
        {
            _WbsPresupuestoService = WbsPresupuestoService;
            this.itemservice = itemservice;
            this.computoService = _computoService;
            this.ofertaService = ofertaService;
            this.wbsofertaService = wbsofertaService;
            this.detallepreciarioService = detallepreciarioService;
            this.preciarioService = preciarioService;
            _catalogoService = catalogoService;
            _gananciaService = gananciaService;
            //
            _presupuestoService = presupuestoService;
            _computoPresupuestoService = computoPresupuestoService;
            _ContratoService = ContratoService;
        }

        // GET: Proyecto/Presupuesto
        public ActionResult Index()
        {
            var input = new comun.aplicacion.PagedAndFilteredResultRequestDto();
            var ofertas = ofertaService.GetOfertas();

            return View(ofertas);
        }

        // GET: Proyecto/Presupuesto/Details/5
        public async System.Threading.Tasks.Task<ActionResult> DetailsRdo(int? id, String message, String message2)
        {
            if (message != null)
            {

                ViewBag.Error = message;
            }

            if (message2 != null)
            {

                ViewBag.Msg = message2;
            }

            if (!id.HasValue)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                var oferta = await ofertaService.Get(new EntityDto<int>(id.Value));

                var proyecto = oferta.Proyecto;
                var contrato = proyecto.Contrato;
                var cliente = contrato.Cliente;
                var computos = computoService.GetComputosPorOferta(id.Value);
                var preciario = preciarioService.GetPreciarioContrato(contrato.Id);

                var ViewModel = new BaseWbsComputoViewModel
                {
                    Cliente = cliente,
                    Oferta = oferta,
                    Proyecto = proyecto,
                    Contrato = contrato,
                    Computo = computos,
                    Preciario = preciario,
                    activado = false,

                };



                return View(ViewModel);

            }

        }
        // GET: Proyecto/Presupuesto/Details/5
        public async System.Threading.Tasks.Task<ActionResult> Details(int? id, String message, String message2)
        {
            if (message != null)
            {

                ViewBag.Error = message;
            }

            if (message2 != null)
            {

                ViewBag.Msg = message2;
            }

            if (!id.HasValue)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                // var oferta = await ofertaService.Get(new EntityDto<int>(id.Value));
                var oferta = await _presupuestoService.Get(new EntityDto<int>(id.Value));
                var proyecto = oferta.Proyecto;
                var contrato = proyecto.Contrato;
                var cliente = contrato.Cliente;
                var computos = _computoPresupuestoService.GetComputosPorPresupuesto(id.Value);
                var preciario = preciarioService.GetPreciarioContrato(contrato.Id);

                var ViewModel = new OfertaWbsComputoViewModel
                {
                    Cliente = cliente,
                    Oferta = oferta,
                    Proyecto = proyecto,
                    Contrato = contrato,
                    Computo = computos,
                    Preciario = preciario,
                    activado = false,

                };



                return View(ViewModel);

            }

        }

        [System.Web.Mvc.HttpPost]
        public async Task<ActionResult> ActualizarComputosAsync(int id, int oferta)
        {
            OfertaDto datosoferta = await ofertaService.Get(new EntityDto<int>(oferta));
            DateTime foferta = Convert.ToDateTime(datosoferta.fecha_oferta);
            computoService.CalcularPresupuesto(id, foferta);
            return RedirectToAction("Details", "Presupuesto", new { id = oferta });
        }

        public ActionResult ActualizarCostosRdo(int contratoId, int oferta)

        {
            OfertaDto datosoferta = ofertaService.getdetalle(oferta);
            DateTime foferta = Convert.ToDateTime(datosoferta.fecha_oferta);
            var preciario = preciarioService.preciarioporcontratofecha(contratoId, foferta);
            string resultado = "";
            if (preciario != null && preciario.Id > 0)
            {
                resultado = computoService.ActualizarCostoTotal(oferta, contratoId, preciario.Id, foferta, true);
            }
            else
            {
                String mensaje = "No tiene un preciario vigente compruebe su estado";
            }
            return Content("OK");
        }


        [System.Web.Mvc.HttpPost]
        public async Task<ActionResult> ActualizarCostos(int contratoId, int oferta)
        {
            //OfertaDto datosoferta = ofertaService.getdetalle(oferta);
            PresupuestoDto datosoferta = await _presupuestoService.Get(new EntityDto<int>(oferta));
            DateTime foferta = Convert.ToDateTime(datosoferta.fecha_registro);

            // if (!datosoferta.computo_completo) Todo
            var computo_completo = true;
            if (!computo_completo)
            {
                String mensaje = "Los Cómputos de la Oferta: " + datosoferta.codigo + " del proyecto: " + datosoferta.Proyecto.codigo + "  No han sido registrados como completos";
                return RedirectToAction("Details", "Presupuesto", new { id = oferta, message = mensaje });
            }


            var preciario = preciarioService.preciarioporcontratofecha(contratoId, foferta);
            string resultado = "";


            if (preciario != null && preciario.Id > 0)
            {
                resultado = _computoPresupuestoService.ActualizarCostoTotal(oferta, contratoId, preciario.Id, foferta, true);
            }
            else
            {
                String mensaje = "No tiene un preciario vigente compruebe su estado";
                return RedirectToAction("Details", "Presupuesto", new { id = oferta, message = mensaje });


            }


            if (resultado.Length > 0)
            {
                String mensaje = "Se encontraron algunos items sin validar" + "\n" + Environment.NewLine + "" +
                                 resultado;
                return RedirectToAction("Details", "Presupuesto", new { id = oferta, message = mensaje });

            }
            else
            {
                String mensaje2 = "Se Cálculo los items Satisfactoriamente" + "\n\r";
                _presupuestoService.CalcularMontosPresupuesto(datosoferta.Id);
                return RedirectToAction("Details", "Presupuesto", new { id = oferta, message2 = mensaje2 });
            }


        }

        public async Task<ActionResult> Edit(int? id)
        {
            if (!id.HasValue)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {

                var computoDto = await computoService.GetDetalle(id.Value);
                var item = await itemservice.GetDetalle(
                    itemservice.buscaridentificadorpadre(computoDto.Item.item_padre));
                computoDto.nombreitem = item.nombre;
                if (computoDto == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.NotFound);
                }
                else
                {
                    return PartialView(computoDto);
                }
            }
        }

        // POST: Proyecto/Computo/Edit/5
        [System.Web.Mvc.HttpPost]
        public async Task<ActionResult> Edit(ComputoDto computoDto)
        {
            WbsDto datoswbs = await wbsofertaService.Get(new EntityDto<int>(computoDto.WbsId));

            OfertaDto datosoferta = ofertaService.getdetalle(datoswbs.OfertaId);
            DateTime foferta = Convert.ToDateTime(datosoferta.fecha_oferta);

            var preciario = preciarioService.preciarioporcontratofecha(datosoferta.Proyecto.contratoId, foferta);

            try
            {
                ComputoDto computo = computoService.ActualizarprecioAjustado(preciario.Id, computoDto);

                return RedirectToAction("Details", "Presupuesto", new { id = datoswbs.OfertaId });
            }
            catch
            {
                return RedirectToAction("Details", "Presupuesto", new { id = datoswbs.OfertaId });
            }
        }


        public ActionResult _itemnovalidos(int id)
        {
            var novalidos = computoService.GetComputosporOfertaNovalidos(id);
            var ofertadto = ofertaService.getdetalle(id);
            OfertaItemNoValido on = new OfertaItemNoValido
            {
                Oferta = ofertadto,
                NoValidos = novalidos

            };
            return View(on);
        }





        public async Task<ActionResult> _EditarItem(int? id)
        {
            if (!id.HasValue)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {

                var computoDto = await computoService.GetDetalle(id.Value);
                var item = await itemservice.GetDetalle(
                    itemservice.buscaridentificadorpadre(computoDto.Item.item_padre));
                computoDto.nombreitem = item.nombre;
                if (computoDto == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.NotFound);
                }
                else
                {
                    return PartialView(computoDto);
                }
            }
        }

        [System.Web.Mvc.HttpPost]
        public async Task<ActionResult> _EditarItem(ComputoDto computoDto)
        {

            WbsDto datoswbs = await wbsofertaService.Get(new EntityDto<int>(computoDto.WbsId));
            try
            {
                var computo = computoService.Update(computoDto);

                return RedirectToAction("Details", "Presupuesto", new { id = datoswbs.OfertaId });
            }
            catch
            {
                return RedirectToAction("Details", "Presupuesto", new { id = datoswbs.OfertaId });
            }

        }
        public ActionResult ExportReport()
        {
            return View();
        }
        public ActionResult ExportReportE()
        {
            return View();
        }


        public async Task<ActionResult> EditNoValido(int? id, int id2)
        {
            if (!id.HasValue)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {

                var item = await itemservice.GetDetalle(id.Value);
                item.OfertaId = id2;
                var a = itemservice.GetItemsHijos(".");

                if (item == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.NotFound);
                }
                else
                {
                    return View(item);
                }
            }
        }

        // POST: Proyecto/Computo/Edit/5
        [System.Web.Mvc.HttpPost]
        public async Task<ActionResult> EditNoValido(ItemDto i)
        {
            var a = itemservice.GetItemsHijos(".");
            if (ModelState.IsValid)
            {
                var r = await itemservice.Get(new EntityDto<int>(i.Id));
                r.item_padre = i.item_padre;
                r.codigo = i.codigo;

                bool s = itemservice.siexisteid(i.codigo);
                if (s)
                {
                    ViewBag.Msg = "El Código del Item ya Esta Registrado";

                    i.item_padre = i.item_padre;
                    return View("EditNoValido", Mapper.Map<ItemDto>(i));
                }
                var result = await itemservice.Update(r);
                return RedirectToAction("_itemnovalidos", "Presupuesto", new { id = i.OfertaId });
            }

            i.item_padre = i.item_padre;
            ViewBag.Msg = "El Código del Item ya Esta Registrado";
            return View("EditNoValido", Mapper.Map<ItemDto>(i));




        }
        public ActionResult ReportecPresupuesto(int id)
        {
            int a = 0;
            List<ComputoDto> lc = computoService.GetComputosPorOferta(id);
            foreach (var item in lc)
            {
                if (item.costo_total == 0)
                {
                    a = 1;
                }
            }
            if (a == 0)
            {
                return View();
            }
            else
            {
                int id2 = id;

                String mensaje = "Hay Cálculos en Cero genere el presupuesto para continuar..";
                return RedirectToAction("Details", "Presupuesto", new { id = id2, message = mensaje });
            }

        }

        public ActionResult PresupuestoCompleto(int id)
        {
            ofertaService.ActualizarFechaPresupuestoAsync(id);
            String mensaje = "Presupuesto Completado";
            return RedirectToAction("Details", "Presupuesto", new { id = id, message2 = mensaje });
        }

        public ActionResult ReportePresupuestoE(int id)
        {
            return View();
        }

        public ActionResult ExportarExcel(int OfertaId)
        {
            var ofertac = ofertaService.getdetalle(OfertaId);

            var computos = computoService.GetComputosPorOferta(OfertaId);
            var wbs = wbsofertaService.Listar(OfertaId);



            ExcelPackage excel = new ExcelPackage();
            var workSheet = excel.Workbook.Worksheets.Add("Matriz Presupuesto");

            workSheet.TabColor = System.Drawing.Color.Azure;

            workSheet.DefaultRowHeight = 12;

            //Header of table  
            //  
            workSheet.Row(1).Height = 20;
            workSheet.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            workSheet.Row(2).Style.Font.Bold = true;
            workSheet.Row(4).Style.Font.Bold = true;



            int columna = 5;
            int c = 5;
            workSheet.View.FreezePanes(5, 1);
            workSheet.View.FreezePanes(5, 5);
            foreach (var itemswbs in wbs)
            {


                workSheet.Cells[1, columna].Value = itemswbs.nombre_padre;
                workSheet.Cells[2, columna].Value = itemswbs.nivel_nombre;
                workSheet.Cells[3, columna].Value = itemswbs.Id;


                workSheet.Cells[1, 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                workSheet.Cells[1, 1].Style.Fill.BackgroundColor.SetColor(Color.Gainsboro);
                workSheet.Cells[1, 1].Style.Border.BorderAround(ExcelBorderStyle.Medium, System.Drawing.Color.Black);
                workSheet.Cells[1, 2].Style.Fill.PatternType = ExcelFillStyle.Solid;
                workSheet.Cells[1, 2].Style.Fill.BackgroundColor.SetColor(Color.Gainsboro);
                workSheet.Cells[1, 3].Style.Fill.PatternType = ExcelFillStyle.Solid;
                workSheet.Cells[1, 3].Style.Fill.BackgroundColor.SetColor(Color.Gainsboro);
                workSheet.Cells[1, 4].Style.Fill.PatternType = ExcelFillStyle.Solid;
                workSheet.Cells[1, 4].Style.Fill.BackgroundColor.SetColor(Color.Gainsboro);
                workSheet.Cells[2, 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                workSheet.Cells[2, 1].Style.Fill.BackgroundColor.SetColor(Color.Gainsboro);
                workSheet.Cells[2, 2].Style.Fill.PatternType = ExcelFillStyle.Solid;
                workSheet.Cells[2, 2].Style.Fill.BackgroundColor.SetColor(Color.Gainsboro);
                workSheet.Cells[2, 3].Style.Fill.PatternType = ExcelFillStyle.Solid;
                workSheet.Cells[2, 3].Style.Fill.BackgroundColor.SetColor(Color.Gainsboro);
                workSheet.Cells[2, 4].Style.Fill.PatternType = ExcelFillStyle.Solid;
                workSheet.Cells[2, 4].Style.Fill.BackgroundColor.SetColor(Color.Gainsboro);
                workSheet.Cells[3, 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                workSheet.Cells[3, 1].Style.Fill.BackgroundColor.SetColor(Color.Gainsboro);
                workSheet.Cells[3, 2].Style.Fill.PatternType = ExcelFillStyle.Solid;
                workSheet.Cells[3, 2].Style.Fill.BackgroundColor.SetColor(Color.Gainsboro);
                workSheet.Cells[3, 3].Style.Fill.PatternType = ExcelFillStyle.Solid;
                workSheet.Cells[3, 3].Style.Fill.BackgroundColor.SetColor(Color.Gainsboro);
                workSheet.Cells[3, 4].Style.Fill.PatternType = ExcelFillStyle.Solid;
                workSheet.Cells[3, 4].Style.Fill.BackgroundColor.SetColor(Color.Gainsboro);
                workSheet.Cells[4, 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                workSheet.Cells[4, 1].Style.Fill.BackgroundColor.SetColor(Color.Gainsboro);
                workSheet.Cells[4, 2].Style.Fill.PatternType = ExcelFillStyle.Solid;
                workSheet.Cells[4, 2].Style.Fill.BackgroundColor.SetColor(Color.Gainsboro);
                workSheet.Cells[4, 3].Style.Fill.PatternType = ExcelFillStyle.Solid;
                workSheet.Cells[4, 3].Style.Fill.BackgroundColor.SetColor(Color.Gainsboro);
                workSheet.Cells[4, 4].Style.Fill.PatternType = ExcelFillStyle.Solid;
                workSheet.Cells[4, 4].Style.Fill.BackgroundColor.SetColor(Color.Gainsboro);

                workSheet.Cells[1, columna].Style.Fill.PatternType = ExcelFillStyle.Solid;
                workSheet.Cells[1, columna].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);

                workSheet.Cells[2, columna].Style.Fill.PatternType = ExcelFillStyle.Solid;
                workSheet.Cells[2, columna].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);

                workSheet.Cells[3, columna].Style.Fill.PatternType = ExcelFillStyle.Solid;
                workSheet.Cells[3, columna].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                workSheet.Cells[4, columna].Style.Fill.PatternType = ExcelFillStyle.Solid;
                workSheet.Cells[4, columna].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                workSheet.Column(columna).Width = 30;
                columna = columna + 1;
            }

            //  var itemss = computoService.GetItemDistintosComputos(OfertaId);
            var itemss = itemservice.GetItems();
            workSheet.Cells[4, 1].Value = "ID";
            workSheet.Cells[4, 1].AutoFilter = true;
            workSheet.Cells[4, 2].Value = "ITEM";
            workSheet.Cells[4, 2].AutoFilter = true;
            workSheet.Cells[4, 3].Value = "DESCRIPCIÓN";
            workSheet.Cells[4, 3].AutoFilter = true;
            workSheet.Cells[4, 4].Value = "UNIDAD";
            workSheet.Cells[4, 4].AutoFilter = true;






            workSheet.Column(3).Width = 60;
            workSheet.Column(1).Hidden = true;
            workSheet.Row(3).Hidden = true;

            foreach (var pitem in itemss)
            {

                workSheet.Cells[c, 1].Value = pitem.Id;
                workSheet.Cells[c, 2].Value = pitem.codigo;
                workSheet.Cells[c, 3].Value = pitem.nombre;
                workSheet.Cells[c, 4].Value = computoService.nombrecatalogo(pitem.UnidadId);
                c = c + 1;
            }
            var noOfCol = workSheet.Dimension.End.Column;
            var noOfRow = workSheet.Dimension.End.Row;



            for (int j = 5; j <= noOfCol; j++)
            {
                var wbsid = workSheet.Cells[3, j].Value.ToString();
                for (int i = 5; i <= noOfRow; i++)
                {
                    var itemid = workSheet.Cells[i, 1].Value.ToString();

                    foreach (var items in computos)
                    {
                        if (Convert.ToString(items.WbsId) == wbsid && Convert.ToString(items.ItemId) == itemid)
                        {

                            workSheet.Cells[i, j].Value = items.cantidad;

                        }

                    }


                }

            }

            var col = workSheet.Dimension.End.Column;
            for (int i = 5; i <= noOfRow; i++)
            {
                var itemid = workSheet.Cells[i, 1].Value.ToString();

                foreach (var items in computos)
                {

                    if (Convert.ToString(items.ItemId) == itemid)

                    {
                        workSheet.Cells[i, col + 1].Value = computoService.sumacantidades(OfertaId, items.ItemId);
                        workSheet.Cells[i, col + 2].Value = "$   " + String.Format(CultureInfo.InvariantCulture, "{0:0,0.00}", items.precio_unitario);
                        workSheet.Cells[i, col + 3].Value = "$   " + String.Format(CultureInfo.InvariantCulture, "{0:0,0.00}", computoService.sumacantidades(OfertaId, items.ItemId) * items.precio_unitario);
                    }

                }

            }




            var c1 = workSheet.Dimension.End.Column;
            var r1 = workSheet.Dimension.End.Row + 3;

            workSheet.Cells[4, noOfCol + 1].Value = "CANTIDAD ESTIMADA";
            workSheet.Column(noOfCol + 1).Width = 20;
            workSheet.Cells[1, noOfCol + 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
            workSheet.Cells[1, noOfCol + 1].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
            workSheet.Cells[2, noOfCol + 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
            workSheet.Cells[2, noOfCol + 1].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
            workSheet.Cells[4, noOfCol + 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
            workSheet.Cells[4, noOfCol + 1].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
            workSheet.Cells[4, noOfCol + 2].Value = "PRECIO UNITARIO";
            workSheet.Cells[1, noOfCol + 2].Style.Fill.PatternType = ExcelFillStyle.Solid;
            workSheet.Cells[1, noOfCol + 2].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
            workSheet.Cells[2, noOfCol + 2].Style.Fill.PatternType = ExcelFillStyle.Solid;
            workSheet.Cells[2, noOfCol + 2].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
            workSheet.Cells[4, noOfCol + 2].Style.Fill.PatternType = ExcelFillStyle.Solid;
            workSheet.Cells[4, noOfCol + 2].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
            workSheet.Column(noOfCol + 2).Width = 20;
            workSheet.Cells[4, noOfCol + 3].Value = "COSTO TOTAL";
            workSheet.Column(noOfCol + 3).Width = 20;
            workSheet.Cells[1, noOfCol + 3].Style.Fill.PatternType = ExcelFillStyle.Solid;
            workSheet.Cells[1, noOfCol + 3].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
            workSheet.Cells[2, noOfCol + 3].Style.Fill.PatternType = ExcelFillStyle.Solid;
            workSheet.Cells[2, noOfCol + 3].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
            workSheet.Cells[4, noOfCol + 3].Style.Fill.PatternType = ExcelFillStyle.Solid;
            workSheet.Cells[4, noOfCol + 3].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
            workSheet.Cells[r1, 3].Value = "Sub-total Ingeniería:";
            workSheet.Cells[r1, 3].Style.Border.BorderAround(ExcelBorderStyle.Thin, System.Drawing.Color.Black);
            workSheet.Cells[r1 + 1, 3].Value = "Sub-total Procura:";
            workSheet.Cells[r1 + 1, 3].Style.Border.BorderAround(ExcelBorderStyle.Thin, System.Drawing.Color.Black);
            workSheet.Cells[r1 + 2, 3].Value = "Sub-total Construcción:";
            workSheet.Cells[r1 + 2, 3].Style.Border.BorderAround(ExcelBorderStyle.Thin, System.Drawing.Color.Black);
            workSheet.Cells[r1 + 3, 3].Value = "Administración";
            workSheet.Cells[r1 + 3, 3].Style.Border.BorderAround(ExcelBorderStyle.Thin, System.Drawing.Color.Black);
            workSheet.Cells[r1 + 4, 3].Value = "Administracion sobre Obra (%) 41,2%";
            workSheet.Cells[r1 + 4, 3].Style.Border.BorderAround(ExcelBorderStyle.Thin, System.Drawing.Color.Black);
            workSheet.Cells[r1 + 5, 3].Value = "Imprevistos sobre Obra (%) 3,0%";
            workSheet.Cells[r1 + 5, 3].Style.Border.BorderAround(ExcelBorderStyle.Thin, System.Drawing.Color.Black);
            workSheet.Cells[r1 + 6, 3].Value = "Utilidad sobre Obra (%) 12,0%";
            workSheet.Cells[r1 + 6, 3].Style.Border.BorderAround(ExcelBorderStyle.Thin, System.Drawing.Color.Black);
            workSheet.Cells[r1 + 7, 3].Value = "Administracion sobre Procura Contratista (%) 10,0%";
            workSheet.Cells[r1 + 7, 3].Style.Border.BorderAround(ExcelBorderStyle.Thin, System.Drawing.Color.Black);
            workSheet.Cells[r1 + 8, 3].Value = "TOTAL";
            workSheet.Cells[r1 + 8, 3].Style.Border.BorderAround(ExcelBorderStyle.Thin, System.Drawing.Color.Black);
            decimal montopconstrucion = computoService.MontoPresupuestoConstruccion(OfertaId);
            decimal montopingenieria = computoService.MontoPresupuestoIngenieria(OfertaId);
            decimal montopprocura = computoService.MontoPresupuestoProcura(OfertaId);
            decimal totalp = montopconstrucion + montopingenieria + montopprocura;

            //Montos Certificados
            decimal montoa = montopconstrucion * (Convert.ToDecimal(0.452));
            decimal montoi = montopconstrucion * (Convert.ToDecimal(0.003));
            decimal montou = montopconstrucion * (Convert.ToDecimal(0.12));
            decimal montopc = montopprocura * (Convert.ToDecimal(0.10));
            decimal total = montoa + montoi + montou + montopc;

            decimal totals = totalp + total;


            workSheet.Cells[r1, c1].Value = "$  " + String.Format(CultureInfo.InvariantCulture, "{0:0,0.00}", montopingenieria);
            workSheet.Cells[r1, c1].Style.Border.BorderAround(ExcelBorderStyle.Thin, System.Drawing.Color.Black);
            workSheet.Row(r1).Style.Font.Bold = true;
            workSheet.Cells[r1 + 1, c1].Value = "$  " + String.Format(CultureInfo.InvariantCulture, "{0:0,0.00}", montopprocura);
            workSheet.Cells[r1 + 1, c1].Style.Border.BorderAround(ExcelBorderStyle.Thin, System.Drawing.Color.Black);
            workSheet.Row(r1 + 1).Style.Font.Bold = true;
            workSheet.Cells[r1 + 2, c1].Value = "$  " + String.Format(CultureInfo.InvariantCulture, "{0:0,0.00}", montopconstrucion);
            workSheet.Cells[r1 + 2, c1].Style.Border.BorderAround(ExcelBorderStyle.Thin, System.Drawing.Color.Black);
            workSheet.Row(r1 + 2).Style.Font.Bold = true;
            workSheet.Cells[r1 + 3, c1].Value = "$  " + String.Format(CultureInfo.InvariantCulture, "{0:0,0.00}", total);
            workSheet.Cells[r1 + 3, c1].Style.Border.BorderAround(ExcelBorderStyle.Thin, System.Drawing.Color.Black);
            workSheet.Row(r1 + 3).Style.Font.Bold = true;
            workSheet.Cells[r1 + 4, c1].Value = "$  " + String.Format(CultureInfo.InvariantCulture, "{0:0,0.00}", montoa);
            workSheet.Cells[r1 + 4, c1].Style.Border.BorderAround(ExcelBorderStyle.Thin, System.Drawing.Color.Black);
            workSheet.Row(r1 + 4).Style.Font.Bold = true;
            workSheet.Cells[r1 + 5, c1].Value = "$  " + String.Format(CultureInfo.InvariantCulture, "{0:0,0.00}", montoi);
            workSheet.Cells[r1 + 5, c1].Style.Border.BorderAround(ExcelBorderStyle.Thin, System.Drawing.Color.Black);
            workSheet.Row(r1 + 5).Style.Font.Bold = true;
            workSheet.Cells[r1 + 6, c1].Value = "$  " + String.Format(CultureInfo.InvariantCulture, "{0:0,0.00}", montou);
            workSheet.Cells[r1 + 6, c1].Style.Border.BorderAround(ExcelBorderStyle.Thin, System.Drawing.Color.Black);
            workSheet.Row(r1 + 6).Style.Font.Bold = true;
            workSheet.Cells[r1 + 7, c1].Value = "$  " + String.Format(CultureInfo.InvariantCulture, "{0:0,0.00}", montopc);
            workSheet.Cells[r1 + 7, c1].Style.Border.BorderAround(ExcelBorderStyle.Thin, System.Drawing.Color.Black);
            workSheet.Row(r1 + 7).Style.Font.Bold = true;
            workSheet.Cells[r1 + 8, c1].Value = "$  " + String.Format(CultureInfo.InvariantCulture, "{0:0,0.00}", totals);
            workSheet.Cells[r1 + 8, c1].Style.Border.BorderAround(ExcelBorderStyle.Thin, System.Drawing.Color.Black);
            workSheet.Row(r1 + 8).Style.Font.Bold = true;
            workSheet.Column(c1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
            string excelName = "Matriz Presupuesto";
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


        public async Task<ActionResult> GenerarPrespuesto(int OfertaId)
        {

            // var ofertac = ofertaService.getdetalle(OfertaId);
            PresupuestoDto ofertac = await _presupuestoService.Get(new EntityDto<int>(OfertaId));
            var Contrato = _ContratoService.InformacionContratoFromProyecto(ofertac.ProyectoId);
            int maximo_nivel_presupuesto = _WbsPresupuestoService.nivel_mas_alto(ofertac.Id);

            if (Contrato != null && Contrato.Formato.HasValue && Contrato.Formato == proyecto.dominio.FormatoContrato.Contrato_2019)
            {
                ExcelPackage paquetepresupuesto = _presupuestoService.MatrizPresupuestoSecondFormat(ofertac, maximo_nivel_presupuesto, false);


                string excelName = "Matriz Presupuesto" + ofertac.Proyecto.codigo + " -" + ofertac.Requerimiento.codigo + "-" + DateTime.Now.Year + "" + DateTime.Now.Month + "" + DateTime.Now.Day;
                using (var memoryStream = new MemoryStream())
                {
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment; filename=" + excelName + ".xlsx");
                    paquetepresupuesto.SaveAs(memoryStream);
                    memoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                    return Content("");
                }
            }
            else
            {
                ExcelPackage paquetepresupuesto = _presupuestoService.GenerarExcelCargaPresupuesto(ofertac, maximo_nivel_presupuesto, false);


                string excelName = "Matriz Presupuesto" + ofertac.Proyecto.codigo + " -" + ofertac.Requerimiento.codigo + "-" + DateTime.Now.Year + "" + DateTime.Now.Month + "" + DateTime.Now.Day;

                using (var memoryStream = new MemoryStream())
                {
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment; filename=" + excelName + ".xlsx");
                    paquetepresupuesto.SaveAs(memoryStream);
                    memoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                    return Content("");
                }
            }

        }


#pragma warning disable CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        public async Task<ActionResult> GenerarPrespuestoRdo(int OfertaId)
#pragma warning restore CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        {
            var ofertac = ofertaService.getdetalle(OfertaId);
            var computos = computoService.GetComputosPorOferta(OfertaId);
            var wbs = wbsofertaService.Listar(OfertaId);

            var excel = computoService.GenerarExcelCabecera(ofertac);
            var workSheet = excel.Workbook.Worksheets[1]; ;


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

            //var itemss = computoService.GetItemDistintosComputos(OfertaId);

            var itemss = itemservice.GetItems();
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
                workSheet.Cells[c, 4].Value = computoService.nombrecatalogo(pitem.UnidadId);

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
                        if (Convert.ToString(items.WbsId) == wbsid && Convert.ToString(items.ItemId) == itemid)
                        {

                            workSheet.Cells[i, j].Value = items.cantidad;

                        }

                    }


                }

            }

            columna = columna - 1;
            c = c + 3;
            workSheet.Cells[9, columna + 1].Value = "CANTIDAD ESTIMADA";
            workSheet.Column(columna + 1).Width = 20;
            workSheet.Cells[6, columna + 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
            workSheet.Cells[6, columna + 1].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
            workSheet.Cells[7, columna + 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
            workSheet.Cells[7, columna + 1].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
            workSheet.Cells[9, columna + 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
            workSheet.Cells[9, columna + 1].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);

            workSheet.Cells[9, columna + 2].Value = "PRECIO UNITARIO";
            workSheet.Cells[6, columna + 2].Style.Fill.PatternType = ExcelFillStyle.Solid;
            workSheet.Cells[6, columna + 2].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
            workSheet.Cells[7, columna + 2].Style.Fill.PatternType = ExcelFillStyle.Solid;
            workSheet.Cells[7, columna + 2].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
            workSheet.Cells[9, columna + 2].Style.Fill.PatternType = ExcelFillStyle.Solid;
            workSheet.Cells[9, columna + 2].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);

            workSheet.Column(columna + 2).Width = 20;
            workSheet.Cells[9, columna + 3].Value = "COSTO TOTAL";
            workSheet.Column(columna + 3).Width = 20;
            workSheet.Cells[6, columna + 3].Style.Fill.PatternType = ExcelFillStyle.Solid;
            workSheet.Cells[6, columna + 3].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
            workSheet.Cells[7, columna + 3].Style.Fill.PatternType = ExcelFillStyle.Solid;
            workSheet.Cells[7, columna + 3].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
            workSheet.Cells[9, columna + 3].Style.Fill.PatternType = ExcelFillStyle.Solid;
            workSheet.Cells[9, columna + 3].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
            workSheet.Cells[c, 3].Value = "Sub-total Ingeniería:";
            workSheet.Cells[c, 3].Style.Border.BorderAround(ExcelBorderStyle.Thin, System.Drawing.Color.Black);
            workSheet.Cells[c + 1, 3].Value = "Sub-total Procura:";
            workSheet.Cells[c + 1, 3].Style.Border.BorderAround(ExcelBorderStyle.Thin, System.Drawing.Color.Black);
            workSheet.Cells[c + 2, 3].Value = "Sub-total Construcción:";
            workSheet.Cells[c + 2, 3].Style.Border.BorderAround(ExcelBorderStyle.Thin, System.Drawing.Color.Black);
            workSheet.Cells[c + 3, 3].Value = "Administración";
            workSheet.Cells[c + 3, 3].Style.Border.BorderAround(ExcelBorderStyle.Thin, System.Drawing.Color.Black);
            workSheet.Cells[c + 4, 3].Value = "Administracion sobre Obra (%) 41,2%";
            workSheet.Cells[c + 4, 3].Style.Border.BorderAround(ExcelBorderStyle.Thin, System.Drawing.Color.Black);
            workSheet.Cells[c + 5, 3].Value = "Imprevistos sobre Obra (%) 3,0%";
            workSheet.Cells[c + 5, 3].Style.Border.BorderAround(ExcelBorderStyle.Thin, System.Drawing.Color.Black);
            workSheet.Cells[c + 6, 3].Value = "Utilidad sobre Obra (%) 12,0%";
            workSheet.Cells[c + 6, 3].Style.Border.BorderAround(ExcelBorderStyle.Thin, System.Drawing.Color.Black);
            workSheet.Cells[c + 7, 3].Value = "Administracion sobre Procura Contratista (%) 10,0%";
            workSheet.Cells[c + 7, 3].Style.Border.BorderAround(ExcelBorderStyle.Thin, System.Drawing.Color.Black);
            workSheet.Cells[c + 8, 3].Value = "TOTAL";
            workSheet.Cells[c + 8, 3].Style.Border.BorderAround(ExcelBorderStyle.Thin, System.Drawing.Color.Black);

            //var col = workSheet.Dimension.End.Column;
            var col = columna;
            for (int i = 10; i <= noOfRow; i++)
            {
                var itemid = (workSheet.Cells[i, 1].Value ?? "").ToString();

                foreach (var items in computos)
                {

                    if (Convert.ToString(items.ItemId) == itemid)

                    {
                        workSheet.Cells[i, col + 1].Value = computoService.sumacantidades(OfertaId, items.ItemId) + "  ";
                        workSheet.Cells[i, col + 2].Value = items.precio_unitario;
                        workSheet.Cells[i, col + 3].Value = computoService.sumacantidades(OfertaId, items.ItemId) * items.precio_unitario;
                    }

                }

            }




            var c1 = columna + 3;
            var r1 = c;

            decimal montopconstrucion = computoService.MontoPresupuestoConstruccion(OfertaId);
            decimal montopingenieria = computoService.MontoPresupuestoIngenieria(OfertaId);
            decimal montopprocura = computoService.MontoPresupuestoProcura(OfertaId);
            decimal totalp = montopconstrucion + montopingenieria + montopprocura;

            //Montos Ganancias

            // var x = _gananciaService.GetGananciasContrato(ofertac.Proyecto.contratoId, ofertac.fecha_oferta.Value);
            var montoa = decimal.ToDouble(montopconstrucion) * (41.2 / 100);

            var montoi = decimal.ToDouble(montopconstrucion) * (3 / 100);
            var montou = decimal.ToDouble(montopconstrucion) * (12 / 100);
            var montopc = decimal.ToDouble(montopprocura) * (10 / 100);
            var total = montoa + montoi + montou + montopc;

            var totals = decimal.ToDouble(totalp) + total;


            workSheet.Cells[r1, c1].Value = montopingenieria;
            workSheet.Cells[r1, c1].Style.Border.BorderAround(ExcelBorderStyle.Thin, System.Drawing.Color.Black);
            workSheet.Row(r1).Style.Font.Bold = true;
            workSheet.Cells[r1 + 1, c1].Value = montopprocura;
            workSheet.Cells[r1 + 1, c1].Style.Border.BorderAround(ExcelBorderStyle.Thin, System.Drawing.Color.Black);
            workSheet.Row(r1 + 1).Style.Font.Bold = true;
            workSheet.Cells[r1 + 2, c1].Value = montopconstrucion;
            workSheet.Cells[r1 + 2, c1].Style.Border.BorderAround(ExcelBorderStyle.Thin, System.Drawing.Color.Black);
            workSheet.Row(r1 + 2).Style.Font.Bold = true;
            workSheet.Cells[r1 + 3, c1].Value = total;
            workSheet.Cells[r1 + 3, c1].Style.Border.BorderAround(ExcelBorderStyle.Thin, System.Drawing.Color.Black);
            workSheet.Row(r1 + 3).Style.Font.Bold = true;
            workSheet.Cells[r1 + 4, c1].Value = montoa;
            workSheet.Cells[r1 + 4, c1].Style.Border.BorderAround(ExcelBorderStyle.Thin, System.Drawing.Color.Black);
            workSheet.Row(r1 + 4).Style.Font.Bold = true;
            workSheet.Cells[r1 + 5, c1].Value = montoi;
            workSheet.Cells[r1 + 5, c1].Style.Border.BorderAround(ExcelBorderStyle.Thin, System.Drawing.Color.Black);
            workSheet.Row(r1 + 5).Style.Font.Bold = true;
            workSheet.Cells[r1 + 6, c1].Value = montou;
            workSheet.Cells[r1 + 6, c1].Style.Border.BorderAround(ExcelBorderStyle.Thin, System.Drawing.Color.Black);
            workSheet.Row(r1 + 6).Style.Font.Bold = true;
            workSheet.Cells[r1 + 7, c1].Value = montopc;
            workSheet.Cells[r1 + 7, c1].Style.Border.BorderAround(ExcelBorderStyle.Thin, System.Drawing.Color.Black);
            workSheet.Row(r1 + 7).Style.Font.Bold = true;
            workSheet.Cells[r1 + 8, c1].Value = totals;
            workSheet.Cells[r1 + 8, c1].Style.Border.BorderAround(ExcelBorderStyle.Thin, System.Drawing.Color.Black);
            workSheet.Row(r1 + 8).Style.Font.Bold = true;
            workSheet.Column(c1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

            string excelName = "Matriz Presupuesto";
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
    }
}