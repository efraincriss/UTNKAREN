using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Abp.Application.Services.Dto;
using com.cpp.calypso.framework;
using com.cpp.calypso.proyecto.aplicacion;
using com.cpp.calypso.proyecto.aplicacion.Interfaces;
using com.cpp.calypso.proyecto.dominio;
using com.cpp.calypso.proyecto.dominio.Models;
using com.cpp.calypso.web.Areas.Proyecto.Models;
using OfficeOpenXml;

namespace com.cpp.calypso.web.Areas.Proyecto.Controllers
{

    public class AvanceIngenieriaController : BaseController
    {
        private readonly IAvanceIngenieriaAsyncBaseCrudAppService _avanceIngenieriaService;
        private readonly IOfertaAsyncBaseCrudAppService _ofertaService;
        private readonly IWbsOfertaAsyncBaseCrudAppService _wbsService;
        private readonly IComputoAsyncBaseCrudAppService _computoService;
        private readonly IProyectoAsyncBaseCrudAppService _proyectoService;
        private readonly ICertificadoIngenieriaAsyncBaseCrudAppService _CIService;
        // GET: Proyecto/AvanceIngenieria
        public AvanceIngenieriaController(
            IHandlerExcepciones manejadorExcepciones,
            IAvanceIngenieriaAsyncBaseCrudAppService avanceIngenieriaService,
            IOfertaAsyncBaseCrudAppService ofertaService,
            IWbsOfertaAsyncBaseCrudAppService wbsService,
            IComputoAsyncBaseCrudAppService computoService,
            IProyectoAsyncBaseCrudAppService proyectoService,
            ICertificadoIngenieriaAsyncBaseCrudAppService CIService

        ) : base(manejadorExcepciones)
        {
            _avanceIngenieriaService = avanceIngenieriaService;
            _ofertaService = ofertaService;
            _wbsService = wbsService;
            _computoService = computoService;
            _proyectoService = proyectoService;
            _CIService = CIService;
        }

        public ActionResult Index(int? id, string flag = "")
        {
            if (id.HasValue)
            {
                var avances = _avanceIngenieriaService.ListarPorOferta(id.Value);
                if (flag != "")
                {
                    ViewBag.Msg = "No se puede eliminar el registro porque tiene datos relacionados";
                }

                ViewBag.OfertaId = id.Value;

                return View(avances);
            }

            return RedirectToAction("Index", "Inicio", new { area = "" });
        }

        public ActionResult ListaProyectos()
        {

            var proyectos = _proyectoService.GetProyectos();
            ViewBag.ruta = new string[] { "Inicio", "Avance Ingenieria", "Lista Proyectos" };
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
                var certificados = _CIService.ListAll(proyecto.Id);

                if (proyecto == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.NotFound);
                }
                else
                {
                    var ViewModel = new ProyectoCertificadoIngenieria()
                    {
                        Proyecto = proyecto,
                        Certificados = certificados

                    };
                    ViewBag.ruta = new string[] { "Inicio", "Certificados", proyecto.codigo };
                    return View(ViewModel);
                }
            }
        }





        // GET: Proyecto/AvanceIngenieria/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id.HasValue)
            {

                var avance = await _avanceIngenieriaService.Get(new EntityDto<int>(id.Value));
                var proyecto = await _ofertaService.Get(new EntityDto<int>(avance.OfertaId));
                ViewBag.nombre_proyecto = proyecto.descripcion;

                return View(avance);
            }

            return RedirectToAction("Index", "Inicio", new { area = "" });
        }




        // GET: Proyecto/AvanceIngenieria/Create
        public ActionResult Create(int? id) // OfertaId
        {
            if (id.HasValue)
            {
                var avance = new AvanceIngenieriaDto()
                {
                    fecha_presentacion = DateTime.Now,
                    fecha_desde = DateTime.Now,
                    fecha_hasta = DateTime.Now,
                };
                avance.OfertaId = id.Value;
                return View(avance);
            }

            return RedirectToAction("Index", "Inicio", new { area = "" });
        }


        // POST: Proyecto/AvanceIngenieria/Create
        [System.Web.Mvc.HttpPost]
        public async Task<ActionResult> Create(AvanceIngenieriaDto avance)
        {
            if (ModelState.IsValid)
            {
                var dto = await _avanceIngenieriaService.Create(avance);
                return RedirectToAction("Index", "AvanceIngenieria", new { id = avance.OfertaId });
            }

            return View("Create", avance);
        }


        // GET: Proyecto/AvanceIngenieria/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id.HasValue)
            {
                var avance = await _avanceIngenieriaService.Get(new EntityDto<int>(id.Value));
                return View(avance);
            }

            return RedirectToAction("Index", "Inicio", new { area = "" });
        }



        // POST: Proyecto/AvanceIngenieria/Edit/5
        [System.Web.Mvc.HttpPost]
        public async Task<ActionResult> Edit(AvanceIngenieriaDto avance)
        {
            if (ModelState.IsValid)
            {
                await _avanceIngenieriaService.Update(avance);
                return RedirectToAction("Index", "AvanceIngenieria", new { id = avance.OfertaId });
            }

            return View("Edit", avance);
        }

        // POST: Proyecto/AvanceIngenieria/Delete/5
        [System.Web.Mvc.HttpPost]
        public async Task<ActionResult> Delete(int? id) //AvanceIngenieriaId
        {
            if (ModelState.IsValid)
            {
                var eliminado = _avanceIngenieriaService.Eliminar(id.Value);
                var avance = await _avanceIngenieriaService.Get(new EntityDto<int>(id.Value));
                if (eliminado)
                {

                    return RedirectToAction("Index", "AvanceIngenieria", new { id = avance.OfertaId });
                }
                else
                {
                    return RedirectToAction("Index", "AvanceIngenieria",
                        new { id = avance.OfertaId, flag = "error_delete" });
                }
            }

            return RedirectToAction("Index", "Inicio", new { area = "" });
        }

        public ActionResult SubirExcel()

        {
            AvanceIngenieriaExcelViewModel a = new AvanceIngenieriaExcelViewModel
            {
                fechadesde = DateTime.Now,
                fechahasta = DateTime.Now,
                fechapresentacion = DateTime.Now,
                ListaAvanceIngenieria = new List<AvanceIngenieriaExcel>(),
                ListaAvanceIngenieriaNoValidos = new List<AvanceIngenieriaExcel>()

            };
            return View(a);
        }
        [System.Web.Mvc.HttpPost]
        public ActionResult SubirExcel(AvanceIngenieriaExcelViewModel l)
        {

            List<AvanceIngenieriaExcel> Lista = new List<AvanceIngenieriaExcel>();
            if (l.UploadedFile != null)
            {
                // tdata.ExecuteCommand("truncate table OtherCompanyAssets");  
                if (l.UploadedFile.ContentType == "application/vnd.ms-excel" || l.UploadedFile.ContentType ==
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                {
                    string fileName = l.UploadedFile.FileName;
                    string fileContentType = l.UploadedFile.ContentType;
                    byte[] fileBytes = new byte[l.UploadedFile.ContentLength];
                    var data = l.UploadedFile.InputStream.Read(fileBytes, 0, Convert.ToInt32(l.UploadedFile.ContentLength));

                    using (var package = new ExcelPackage(l.UploadedFile.InputStream))
                    {
                        var currentSheet = package.Workbook.Worksheets;
                        var workSheet = currentSheet.First();
                        var noOfCol = workSheet.Dimension.End.Column;

                        if (noOfCol > 13 || noOfCol < 13)
                        {
                            ViewBag.Error =
                                "El archivo cargado no correponde al número de datos de Avance de Ingeniería";

                            return View("SubirExcel", l);
                        }
                        else
                        {

                            var noOfRow = workSheet.Dimension.End.Row;
                            for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                            {
                                /*   AvanceIngenieriaExcel a = new AvanceIngenieriaExcel
                                   {
                                       count = workSheet.Cells[rowIterator, 1].Value.ToString(),
                                       tipo_registro = workSheet.Cells[rowIterator, 2].Value.ToString(),
                                       wo = workSheet.Cells[rowIterator, 3].Value.ToString(),
                                       hh = workSheet.Cells[rowIterator, 4].Value.ToString(),
                                       cedula = workSheet.Cells[rowIterator, 5].Value.ToString(),
                                       ejecutante = workSheet.Cells[rowIterator, 6].Value.ToString(),
                                       fecha = workSheet.Cells[rowIterator, 7].Value.ToString(),
                                       observaciones = workSheet.Cells[rowIterator, 8].Value.ToString(),
                                       etapa = workSheet.Cells[rowIterator, 9].Value.ToString(),
                                       proyecto = workSheet.Cells[rowIterator, 10].Value.ToString(),
                                       codigoproyecto = workSheet.Cells[rowIterator, 11].Value.ToString(),
                                       especialidad = workSheet.Cells[rowIterator, 12].Value.ToString(),
                                       item = workSheet.Cells[rowIterator, 13].Value.ToString(),

                                   };
                                   Lista.Add(a);
                                   */
                            }

                            //Filtrar Excel por Fecha
                            var filtradofecha = _avanceIngenieriaService.FiltrarAvancesExcelFechas(Lista, l.fechadesde, l.fechahasta);

                            l.ListaAvanceIngenieria = filtradofecha;

                            return View("SubirExcel", l);

                        }

                    }
                }
                else
                {


                    ViewBag.Error = "El Formato del Archivo Subido debe ser en formato Excel";
                    return View("SubirExcel", l);


                }
            }

            return View("SubirExcel", l);

        }

        [System.Web.Mvc.HttpPost]
        public ActionResult GuardarDetallesExcel(AvanceIngenieriaExcelViewModel l)
        {

            return View("SubirExcel", null);
        }

        public ActionResult SubirExcelAll(string message)
        {
            if (message != null)
            {
                ViewBag.Error = "Verifique su Archivos posee Observaciones";
            }

            List<AvanceIngenieriaExcel> Lista = new List<AvanceIngenieriaExcel>();
            return View(Lista);
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult SubirExcelAll(DateTime fechapresentacion, DateTime fechadesde,
            DateTime fechahasta, HttpPostedFileBase UploadedFile)
        {
            List<String[]> Observaciones = new List<string[]>();
            List<AvanceIngenieriaExcel> Lista = new List<AvanceIngenieriaExcel>();
            if (UploadedFile != null)
            {
                // tdata.ExecuteCommand("truncate table OtherCompanyAssets");  
                if (UploadedFile.ContentType == "application/vnd.ms-excel" || UploadedFile.ContentType ==
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                {
                    string fileName = UploadedFile.FileName;
                    string fileContentType = UploadedFile.ContentType;
                    byte[] fileBytes = new byte[UploadedFile.ContentLength];
                    var data = UploadedFile.InputStream.Read(fileBytes, 0, Convert.ToInt32(UploadedFile.ContentLength));

                    using (var package = new ExcelPackage(UploadedFile.InputStream))
                    {
                        var currentSheet = package.Workbook.Worksheets;
                        var workSheet = currentSheet.First();
                        var noOfCol = workSheet.Dimension.End.Column;


                        var noOfRow = workSheet.Dimension.End.Row;
                        for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                        {
                            /*   AvanceIngenieriaExcel a = new AvanceIngenieriaExcel
                               {
                                   fila=rowIterator,
                                   count = (workSheet.Cells[rowIterator, 1].Value ?? "").ToString(),
                                   tipo_registro = (workSheet.Cells[rowIterator, 2].Value ?? "").ToString(),
                                   wo = (workSheet.Cells[rowIterator, 3].Value ?? "").ToString(),
                                   hh = (workSheet.Cells[rowIterator, 4].Value ?? "").ToString(),
                                   cedula = (workSheet.Cells[rowIterator, 5].Value ?? "").ToString(),
                                   ejecutante = (workSheet.Cells[rowIterator, 6].Value ?? "").ToString(),
                                   fecha = (workSheet.Cells[rowIterator, 7].Value ?? "").ToString(),
                                   observaciones = (workSheet.Cells[rowIterator, 8].Value ?? "").ToString(),
                                   etapa = (workSheet.Cells[rowIterator, 9].Value ?? "").ToString(),
                                   proyecto = (workSheet.Cells[rowIterator, 10].Value ?? "").ToString(),
                                   codigoproyecto = (workSheet.Cells[rowIterator, 11].Value ?? "").ToString(),
                                   especialidad = (workSheet.Cells[rowIterator, 12].Value ?? "").ToString(),
                                   item = (workSheet.Cells[rowIterator, 13].Value ?? "").ToString(),

                               };
                               Lista.Add(a);*/
                        }

                        foreach (var el in Lista)
                        {
                            var proyecto = _proyectoService.existeproyecto(el.codigoProyecto);
                            if (!proyecto)
                            {
                                String[] error = { "" + el.fila, el.codigoProyecto, " El proyecto no existe" };

                                Observaciones.Add(error);
                            }

                        }
                        if (Observaciones.Count > 0)
                        {
                            TempData["CustomNullError"] = "El archivo tiene Observaciones Verifique";

                            ExcelPackage excel = new ExcelPackage();
                            var errores = excel.Workbook.Worksheets.Add("Observaciones");
                            errores.Cells[1, 1].Value = "Fila";
                            errores.Cells[1, 2].Value = "Dato";
                            errores.Cells[1, 3].Value = "Observación";
                            workSheet.Column(2).Width = 20;
                            workSheet.Column(3).Width = 100;
                            var row = 2;
                            foreach (var pitem in Observaciones)
                            {
                                errores.Cells[row, 1].Value = pitem[0].ToString();
                                errores.Cells[row, 2].Value = pitem[1].ToString();
                                errores.Cells[row, 3].Value = pitem[2].ToString();
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


                            List<AvanceIngenieriaExcel> s = new List<AvanceIngenieriaExcel>();
                            return View("SubirExcelAll", s);
                        }
                        else
                        {


                            //Filtrar Excel por Fecha
                            var filtradofecha =
                                _avanceIngenieriaService.FiltrarAvancesExcelFechas(Lista, fechadesde, fechahasta);
                            /*
                                //Sacar Ofertas Definitivas de Todos los Proyectos Que Coinciden
                                var ofertasdefinitivas =
                                    _avanceIngenieriaService.ListaOfertasDefinitivas(filtradofecha);
                                //var resultado = _avanceIngenieriaService.CrearDetalle(ofertasdefinitivas, filtradofecha,
                                //fechapresentacion, fechadesde, fechahasta);
                                */
                            bool r = _avanceIngenieriaService.Detalles(filtradofecha, fechapresentacion, fechadesde,
                                fechahasta);
                            ViewBag.Msg =
                                "Archivo Cargado Correctamente";
                            return View("SubirExcelAll", filtradofecha);
                        }


                    }
                }
                else
                {


                    ViewBag.Error = "El Formato del Archivo Subido debe ser en formato Excel";
                    return View("SubirExcelAll", Lista);


                }
            }

            return View("SubirExcelAll", Lista);
        }


        [System.Web.Mvc.HttpPost]
        public ActionResult SubirExcelMasivo(AvanceUpload e)
        {
            string result = _avanceIngenieriaService.UploadAvanceMasivo(e);
            return Content(result);

        }
        public async Task<ActionResult> CertificadoIngenieria(int id) //Id CertificadoId
        {

            var excel = _CIService.ObtenerCertificadoIngenieria(id);
            var data = _CIService.GetDetalle(id);

            string excelName = " " + data.numero_certificado +"-"+data.formatFechaEmision;
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

        public ActionResult GenerarCertificado(int Id, DateTime fechaCorte, DateTime fechaEmision)
        {
            int result = _CIService.GenerarCertificado(Id, fechaCorte.Date, fechaEmision.Date);
       
            if (result > 0)
            {
                //bool secuencial = _proyectoService.CambiarSecuencial(result, "REGISTRADO");
                return Content("" + result);
            }
            return Content("ERROR");
        }

        public ActionResult DeleteCertificado(int Id)
        {
            var proyectoid = _CIService.GetDetalle(Id).ProyectoId;
            int result = _CIService.DeleteCertificado(Id);
            return RedirectToAction("DetailsProyecto", "AvanceIngenieria", new { id = proyectoid });

        }
        public ActionResult AnularCertificado(int Id)
        {
            var proyectoid = _CIService.GetDetalle(Id).ProyectoId;
            int result = _CIService.Anular(Id);
          //  bool secuencial = _proyectoService.CambiarSecuencial(proyectoid, "ANULADO");
            if (ModelState.IsValid) {

            }
            return RedirectToAction("DetailsProyecto", "AvanceIngenieria", new { id = proyectoid });
        }
        public ActionResult Aprobar(int Id)
        {
            var proyectoid = _CIService.GetDetalle(Id).ProyectoId;
            int result = _CIService.Aprobar(Id);
            //  bool secuencial = _proyectoService.CambiarSecuencial(proyectoid, "ANULADO");
            if (ModelState.IsValid)
            {

            }
            return RedirectToAction("DetailsProyecto", "AvanceIngenieria", new { id = proyectoid });
        }

    }
}


