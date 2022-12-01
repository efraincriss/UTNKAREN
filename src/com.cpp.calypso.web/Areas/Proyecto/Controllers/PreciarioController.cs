using AutoMapper;
using com.cpp.calypso.framework;
using com.cpp.calypso.proyecto.aplicacion;
using com.cpp.calypso.proyecto.aplicacion.Interfaces;
using com.cpp.calypso.web.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace com.cpp.calypso.web.Areas.Proyecto.Controllers
{
    
    public class PreciarioController : BaseController
    {

        private readonly IPreciarioAsyncBaseCrudAppService preciarioService;
        private readonly IDetallePreciarioAsyncBaseCrudAppService detallepreciarioService;
        private readonly IContratoAsyncBaseCrudAppService contratoService;
        private readonly IItemAsyncBaseCrudAppService itemService;
        private readonly IComputoAsyncBaseCrudAppService _computoService;
        public PreciarioController(IHandlerExcepciones manejadorExcepciones,
            IPreciarioAsyncBaseCrudAppService preciarioService,
            IDetallePreciarioAsyncBaseCrudAppService detallepreciarioService,
            IContratoAsyncBaseCrudAppService contratoService,
            IItemAsyncBaseCrudAppService itemService,
            IComputoAsyncBaseCrudAppService computoService) :
            base(manejadorExcepciones)
        {
            this.preciarioService = preciarioService;
            this.detallepreciarioService = detallepreciarioService;
            this.contratoService = contratoService;
            this.itemService = itemService;
            _computoService = computoService;
        }

        // GET: Proyecto/Preciario
        public ActionResult Index(String message, String message2)
        {
            var preciario = preciarioService.GetPreciarios();
            if (message != null)
            {

                ViewBag.Msg = message;
            }
            if (message2 != null)
            {

                ViewBag.Msg2 = message2;
            }


            return View(preciario);
        }

        // GET: Proyecto/Preciario/Details/5
#pragma warning disable CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        public async Task<ActionResult> Details(int? id)
#pragma warning restore CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        {
            if (!id.HasValue)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                var preciario = preciarioService.GetDetalle(id.Value);
           //     var nuevo = new DetallePreciarioDto();
               //nuevo.Items = itemService.GetItemsparaOferta();
                //nuevo.PreciarioId = preciario.Id;
                var detalles = detallepreciarioService.GetDetallesPreciarios(id.Value);

                if (preciario == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.NotFound);
                }
                else
                {
                    var ViewModel = new PreciarioDetalleViewModel
                    {

                        Preciario = preciario,
                        //dp= nuevo,
                        DetallesPreciario = detalles
                    };
                    /*
                    foreach (var items in detalles)
                    {
                        var iditempadre = itemService.buscaridentificadorpadre(items.Item.item_padre);
                        if (iditempadre != 0)
                        {
                            var item = await itemService.GetDetalle(iditempadre);
                            items.nombreitempadre = item.nombre;

                        }
                       
                    }*/
                    return View(ViewModel);
                }
            }
        }

        // GET: Proyecto/Preciario/Create
        public ActionResult Create()
        {
            PreciarioDto nuevo = new PreciarioDto();
            nuevo.Contratos = contratoService.GetContratos();
            nuevo.fecha_desde = DateTime.Now;
            nuevo.fecha_hasta = DateTime.Now;
            nuevo.estado = true;
            return View(nuevo);
        }

        // POST: Proyecto/Preciario/Create
        [HttpPost]
        public async Task<ActionResult> Create(PreciarioDto preciario)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    if (preciario.fecha_desde > preciario.fecha_hasta)
                    {
                        ViewBag.Error = "Fecha desde no puede ser mayor a la fecha hasta";
                        var dto = Mapper.Map<PreciarioDto>(preciario);
                        dto.Contratos = contratoService.GetContratos();
                        return View("Create", dto);
                    }
                    else
                    {
                        if (preciario.fecha_hasta > preciario.fecha_desde)
                        {

                            if (preciarioService.ComprobarExistenciaPreciarioContrato(preciario.fecha_desde,
                                preciario.fecha_hasta, preciario.ContratoId))
                            {
                                ViewBag.Error = "El contrato ya tiene un preciario en la fecha que quiere ingresar";
                                var dto = Mapper.Map<PreciarioDto>(preciario);
                                dto.Contratos = contratoService.GetContratos();
                                return View("Create", dto);

                            }

                            preciario.vigente = true;
                            var preciarios = await preciarioService.Create(preciario);

                            return RedirectToAction("Index", "Preciario");
                        }
                        else
                        {
                            ViewBag.Error = "Fecha hasta  no puede ser menor a la fecha desde";
                            var dto = Mapper.Map<PreciarioDto>(preciario);
                            dto.Contratos = contratoService.GetContratos();
                            return View("Create", dto);

                        }
                    }
                }
                else
                {

                    var dto = Mapper.Map<PreciarioDto>(preciario);
                    dto.Contratos = contratoService.GetContratos();
                    return View("Create", dto);

                }


            }
            catch
            {
                return View();
            }
        }

        // GET: Proyecto/Preciario/Edit/5
#pragma warning disable CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        public async Task<ActionResult> Edit(int? id)
#pragma warning restore CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        {
            if (!id.HasValue)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {


                var preciariodto = preciarioService.GetDetalle(id.Value);
                preciariodto.Contratos = contratoService.GetContratos();
                if (preciariodto == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.NotFound);
                }
                else
                {
                    return View(preciariodto);
                }
            }
        }

        // POST: Proyecto/Preciario/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(int id, PreciarioDto preciarioDto)
        {

            if (preciarioDto.fecha_desde > preciarioDto.fecha_hasta)
            {
                ViewBag.Error = "Fecha desde no puede ser mayor a la fecha hasta";
                var dto = Mapper.Map<PreciarioDto>(preciarioDto);
                dto.Contratos = contratoService.GetContratos();
                return View("Edit", dto);
            }
            else
            {
                if (!(preciarioDto.fecha_hasta > preciarioDto.fecha_desde))
                {

                   ViewBag.Error = "Fecha hasta  no puede ser menor a la fecha desde";
                    var dto = Mapper.Map<PreciarioDto>(preciarioDto);
                    return View("Edit", dto);
                }

                if (preciarioService.ComprobarExistenciaPreciarioContratoEdit(preciarioDto.Id,preciarioDto.fecha_desde,
                    preciarioDto.fecha_hasta, preciarioDto.ContratoId))
                {
                    ViewBag.Error = "El contrato ya tiene un preciario en la fecha que quiere ingresar";
                    var dto = Mapper.Map<PreciarioDto>(Mapper.Map<PreciarioDto>(preciarioDto));
                    dto.Contratos = contratoService.GetContratos();
                    return View("Edit", dto);

                }

                if (ModelState.IsValid)
                {
                }

                if (ModelState.IsValid) {
                }
                var result =await preciarioService.Update(preciarioDto);
                return RedirectToAction("Index", "Preciario");
            }
        }



        // POST: Proyecto/Preciario/Delete/5
        [HttpPost]
#pragma warning disable CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        public async Task<ActionResult> Delete(int? id)
#pragma warning restore CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        {
            try
            {
               
                if (id.HasValue)
                {
                    int req =preciarioService.EliminarVigenciaAsync(id.Value);

                    if ( req>0)
                    {
                        String Mensaje = "No se puede eliminar el preciario, tiene registros relacionados";
                        return RedirectToAction("Index", "Preciario", new { message = Mensaje });
                    }
                    else
                    {
                        String Mensaje = "Eliminado Correctamente";
                        return RedirectToAction("Index", "Preciario",new {message2=Mensaje});
                      
                    }
                }
               
                    return RedirectToAction("Index");
                
            }
            catch
            {
                return View();
            }
        }

    


    public ActionResult CargaExcel(int id)
    {
        ViewBag.idpreciario = id;
        return View();
    }

    [HttpPost]
    public async Task<ActionResult> CargaExcel(HttpPostedFileBase UploadedFile)
    {
            List<String[]> Observaciones = new List<string[]>();

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
                            var preciarioid = (workSheet.Cells[1, 1].Value ?? "").ToString();
                            var itemid = (workSheet.Cells[rowIterator, 1].Value ?? "").ToString();
                                var preciounitario = (workSheet.Cells[rowIterator, 5].Value ?? "").ToString();

                                if (itemid.Length > 0 && preciounitario.Length > 0)
                                {

                                    DetallePreciarioDto Nuevo = new DetallePreciarioDto()
                                    {
                                        PreciarioId =Int32.Parse(preciarioid),
                                        ItemId = Int32.Parse(itemid),
                                        precio_unitario = Convert.ToDecimal(preciounitario),
                                        vigente = true,
                                        comentario = "",
                                    };
                                    DetallePreciarioDto e =
                                        detallepreciarioService.comprobarexistenciaitem(Int32.Parse(preciarioid),Int32.Parse(itemid));
                               
                                    if (e==null)
                                    {
                                        var r = await detallepreciarioService.Create(Nuevo);

                                    }
                                    else
                                    {

                                        var codigo = (workSheet.Cells[rowIterator, 2].Value ?? "").ToString();
                                        String[] error = { codigo, " Ya existe el registro de ese item  en el Preciario" + preciarioid };

                                        Observaciones.Add(error);
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

                            return View();
                        }
                        else
                        {
                            ViewBag.Msg = "Cargado Correctamente";
                          
                            return View();

                        }
                    }

                }
                else
                {


                    ViewBag.Error = "El Formato del Archivo Subido debe ser en formato Excel";

                    return View();


                }
            }

        return View();


        }

        [HttpPost]
        public ActionResult ClonarPreciario(int id)
        {
            int result = preciarioService.ClonaPreciario(id);
            var preciario = preciarioService.GetDetalle(id);

           if (result > 0) {
                  String Mensaje = "Preciario Clonado para el contrato: "+preciario.Contrato.Codigo ;
                        return RedirectToAction("Index", "Preciario", new { message2 = Mensaje });
            }
            else {
                String Mensaje = "Sucedio Algo Al Clonar";
                return RedirectToAction("Index", "Preciario", new { message = Mensaje });
            }
        }

        public ActionResult ExportarExcel(int id)
        {

            var items = itemService.GetItemsParaOferta().Where(x=>x.GrupoId!=3).OrderBy(dto =>dto.codigo );

            ExcelPackage excel = new ExcelPackage();
            var workSheet = excel.Workbook.Worksheets.Add("Carga Preciarios");

            workSheet.DefaultRowHeight = 12;

            //Header of table  

      

            workSheet.Cells[1, 2].Value = "Código Item";
            workSheet.Cells[1, 3].Value = "Nombre Item";
            workSheet.Cells[1, 4].Value = "Unidad";
            workSheet.Cells[1, 5].Value = "Precio Unitario";
            workSheet.Row(1).Style.Font.Bold = true;
            workSheet.Row(1).Height =20;
            workSheet.Cells["B1:E1"].AutoFilter = true;
            workSheet.Column(1).Hidden = true;
            workSheet.Column(2).Width = 20;
            workSheet.Column(3).Width = 40;
            workSheet.Column(5).Width = 40;



            int c = 2;
            workSheet.Cells[1, 1].Value = id;
            foreach (var pitem in items)
            {
                workSheet.Cells[c, 1].Value = pitem.Id;
                workSheet.Cells[c, 2].Value = pitem.codigo;
                workSheet.Cells[c, 3].Value = pitem.nombre;
                workSheet.Cells[c, 4].Value = _computoService.nombrecatalogo(pitem.UnidadId);
                c = c + 1;
            }



            string excelName = "Formato Carga Items";
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

        public ActionResult ExportarExcelPrecios(int id) //Pasar pametros preciario 
        {
            ExcelPackage excel = detallepreciarioService.GenerarExcelPreciarioValores(id);

            string excelName = "Preciario Actual";
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


