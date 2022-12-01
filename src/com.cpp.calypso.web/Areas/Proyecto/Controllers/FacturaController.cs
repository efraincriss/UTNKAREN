using AutoMapper;
using com.cpp.calypso.framework;
using com.cpp.calypso.proyecto.aplicacion;
using com.cpp.calypso.proyecto.aplicacion.Interfaces;
using com.cpp.calypso.web.Areas.Proyecto.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using com.cpp.calypso.proyecto.dominio;
using Newtonsoft.Json;
using OfficeOpenXml;
using System.IO;

namespace com.cpp.calypso.web.Areas.Proyecto.Controllers
{
   
    public class FacturaController : BaseController
    {
        private readonly IFacturaAsyncBaseCrudAppService _FacturaService;
        private readonly ICertificadoFacturaAsyncBaseCrudAppService _certificadoFacturaService;
        private readonly IEmpresaAsyncBaseCrudAppService _empresaService;
        private readonly IClienteAsyncBaseCrudAppService _clienteService;
        private readonly ICobroFacturaAsyncBaseCrudAppService _cobroService;
        private readonly ICobroAsyncBaseCrudAppService _cobrorealService;

        public FacturaController(IHandlerExcepciones manejadorExcepciones,
            IFacturaAsyncBaseCrudAppService FacturaService,
            IEmpresaAsyncBaseCrudAppService empresaService,
            IClienteAsyncBaseCrudAppService clienteService,
            ICertificadoFacturaAsyncBaseCrudAppService certificadoFacturaService,
            ICobroFacturaAsyncBaseCrudAppService cobroService,
            ICobroAsyncBaseCrudAppService cobrorealService

        ) : base(manejadorExcepciones)
        {
            _cobrorealService = cobrorealService;
               _cobroService = cobroService;
                _FacturaService = FacturaService;
            _empresaService = empresaService;
            _clienteService = clienteService;
            _certificadoFacturaService = certificadoFacturaService;
        }

        // GET: Proyecto/Factura
        public ActionResult Index()
        {
            List<FacturaDto> a = _FacturaService.GetFacturas();

            return View(a);
        }
        
        public ActionResult VistaCobros()
        {
            var a = _cobroService.ListadeCobros();

            return View(a);
        }

        // GET: Proyecto/Factura/Details/5
        public ActionResult Details(int id,int id2 = 0)
        {
            FacturaDto f = _FacturaService.GetDetalle(id);
            ViewBag.dc = id2;//Id de cobro
            var lcertificados = _certificadoFacturaService.certificadosporfactura(id);

            FacturaViewModel n = new FacturaViewModel
            {
                factura = f,

                certificados = lcertificados,

            };
            return View(n);
        }

        public ActionResult DetailsCobros(int id)
        {
            var r= _cobroService.getdetalle(id);
          
            

            return View(r);
        }

        // GET: Proyecto/Factura/Create
        public ActionResult Create()
        {
            FacturaDto n = new FacturaDto();
            n.fecha_emision = DateTime.Now;
            n.Empresas = _empresaService.GetEmpresas();
            n.Clientes = _clienteService.GetClientes();
            return View(n);
        }

        // POST: Proyecto/Factura/Create
        [System.Web.Mvc.HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> Create(FacturaDto n)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    n.vigente = true;
                    var f = await _FacturaService.Create(n);
                    return RedirectToAction("Index", "Factura");
                }

                return View("Create", Mapper.Map<FacturaDto>(n));
            }

            catch
            {
                return View();
            }
        }

        // GET: Proyecto/Factura/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Proyecto/Factura/Edit/5
        [System.Web.Mvc.HttpPost]
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

        // GET: Proyecto/Factura/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Proyecto/Factura/Delete/5
        [System.Web.Mvc.HttpPost]
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

        public ActionResult SubirExcel()

        {
            
                FacturaExcelModel a = new FacturaExcelModel
                {

                    ListaFacturas = new List<FacturaExcel>(),
                    ListaFacturasNovalidas = new List<FacturaExcel>(),


                };
                return View(a);
         
        
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult SubirExcel(FacturaExcelModel l)
        {

            List<FacturaExcel> Lista = new List<FacturaExcel>(); List<FacturaExcel> ListaNovalidos = new List<FacturaExcel>();
            if (l.UploadedFile != null)
            {
                // tdata.ExecuteCommand("truncate table OtherCompanyAssets");  
                if (l.UploadedFile.ContentType == "application/vnd.ms-excel" || l.UploadedFile.ContentType ==
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                {
                    string fileName = l.UploadedFile.FileName;
                    string fileContentType = l.UploadedFile.ContentType;
                    byte[] fileBytes = new byte[l.UploadedFile.ContentLength];
                    var data = l.UploadedFile.InputStream.Read(fileBytes, 0,
                        Convert.ToInt32(l.UploadedFile.ContentLength));

                    using (var package = new ExcelPackage(l.UploadedFile.InputStream))
                    {
                        var currentSheet = package.Workbook.Worksheets;
                        var workSheet = currentSheet.First();
                        var noOfCol = workSheet.Dimension.End.Column;

                        if (noOfCol > 29 || noOfCol < 29)
                        {
                            ViewBag.Error =
                                "El archivo cargado no correponde al número de datos de Facturas";
                            l.ListaFacturas = Lista;
                            l.ListaFacturasNovalidas = ListaNovalidos;

                            return View( l);
                        }
                        else
                        {

                            var noOfRow = workSheet.Dimension.End.Row;
                            for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                            {
                                var so = (workSheet.Cells[rowIterator, 1].Value ?? "").ToString();
                                if (so.Equals("") || so == "")
                                {
                                    so = "0";
                                }

                                var doc = (workSheet.Cells[rowIterator, 2].Value ?? "").ToString();
                                if (doc.Equals("") || so == "")
                                {
                                    doc = "0";
                                }

                                var fee = (workSheet.Cells[rowIterator, 3].Value ?? "").ToString();
                                if (fee.Equals("") || so == "")
                                {
                                    fee = "01/01/1990";
                                }

                                var fer = (workSheet.Cells[rowIterator, 4].Value ?? "").ToString();
                                if (fer.Equals("") || so == "")
                                {
                                    fer = "01/01/1990";
                                }

                                var fc = (workSheet.Cells[rowIterator, 5].Value ?? "").ToString();

                                if (fc.Equals("") || so == "")
                                {
                                    fc = "01/01/1990";
                                }

                                var fv = (workSheet.Cells[rowIterator, 6].Value ?? "").ToString();


                                if (fv.Equals("") || so == "")
                                {
                                    fv = "01/01/1990";
                                }

                                var fact = (workSheet.Cells[rowIterator, 7].Value ?? "").ToString();
                                if (fact.Equals("") || so == "")
                                {
                                    fact = "0";
                                }

                                var cli = (workSheet.Cells[rowIterator, 8].Value ?? "").ToString();

                                if (cli.Equals("") || so == "")
                                {
                                    cli = "0";
                                }

                                var det = (workSheet.Cells[rowIterator, 9].Value ?? "").ToString();
                                if (det.Equals("") || so == "")
                                {
                                    det = "0";
                                }

                                var ord = (workSheet.Cells[rowIterator, 10].Value ?? "").ToString();

                                if (ord.Equals("") || so == "")
                                {
                                    ord = "0";
                                }

                                var per = (workSheet.Cells[rowIterator, 11].Value ?? "").ToString();

                                if (per.Equals("") || so == "")
                                {
                                    per = "01/01/1990";
                                }

                                var ov1 = (workSheet.Cells[rowIterator, 12].Value ?? "").ToString();

                                if (ov1.Equals("") || so == "")
                                {
                                    ov1 = "0";
                                }

                                var oser = (workSheet.Cells[rowIterator, 13].Value ?? "").ToString();
                                if (oser.Equals("") || so == "")
                                {
                                    oser = "0";
                                }

                                var importe = (workSheet.Cells[rowIterator, 14].Value ?? "").ToString();
                                if (importe.Equals("") || so == "")
                                {
                                    importe = "0";
                                }


                                var ivadoceocartoce = (workSheet.Cells[rowIterator, 15].Value ?? "").ToString();
                                if (ivadoceocartoce.Equals("") || so == "")
                                {
                                    ivadoceocartoce = "0";
                                }


                                var facturado = (workSheet.Cells[rowIterator, 16].Value ?? "").ToString();
                                if (facturado.Equals("") || so == "")
                                {
                                    facturado = "0";
                                }

                                var rent_uno_bienes = (workSheet.Cells[rowIterator, 17].Value ?? "").ToString();

                                if (rent_uno_bienes.Equals("") || so == "")
                                {
                                    rent_uno_bienes = "0";
                                }

                                var rent_uno_construccion = (workSheet.Cells[rowIterator, 18].Value ?? "").ToString();
                                if (rent_uno_construccion.Equals("") || so == "")
                                {
                                    rent_uno_construccion = "0";
                                }

                                var rent_dos_servicios = (workSheet.Cells[rowIterator, 19].Value ?? "").ToString();

                                if (rent_dos_servicios.Equals("") || so == "")
                                {
                                    rent_dos_servicios = "0";
                                }

                                var iva_treinta = (workSheet.Cells[rowIterator, 20].Value ?? "").ToString();

                                if (iva_treinta.Equals("") || so == "")
                                {
                                    iva_treinta = "0";
                                }

                                var iva_setenta = (workSheet.Cells[rowIterator, 21].Value ?? "").ToString();
                                if (iva_setenta.Equals("") || so == "")
                                {
                                    iva_setenta = "0";
                                }

                                var iva_diez = (workSheet.Cells[rowIterator, 22].Value ?? "").ToString();
                                if (iva_diez.Equals("") || so == "")
                                {
                                    iva_diez = "0";
                                }

                                var iva_viente = (workSheet.Cells[rowIterator, 23].Value ?? "").ToString();
                                if (iva_viente.Equals("") || so == "")
                                {
                                    iva_viente = "0";
                                }

                                var acobrar = (workSheet.Cells[rowIterator, 24].Value ?? "").ToString();
                                if (acobrar.Equals("") || so == "")
                                {
                                    acobrar = "0";
                                }

                                var cobrado = (workSheet.Cells[rowIterator, 25].Value ?? "").ToString();
                                if (cobrado.Equals("") || so == "")
                                {
                                    cobrado = "0";
                                }

                                var fecha_cobro = (workSheet.Cells[rowIterator, 26].Value ?? "").ToString();

                                if (fecha_cobro.Equals("") || so == "")
                                {
                                    fecha_cobro = "01/01/1990";
                                }

                                var banco = (workSheet.Cells[rowIterator, 27].Value ?? "").ToString();

                                if (banco.Equals("") || so == "")
                                {
                                    banco = "0";
                                }

                                var situacion = (workSheet.Cells[rowIterator, 28].Value ?? "").ToString();

                                if (situacion.Equals("") || so == "")
                                {
                                    situacion = "0";
                                }

                                var tipo = (workSheet.Cells[rowIterator, 29].Value ?? "").ToString();

                                if (tipo.Equals("") || so == "")
                                {
                                    tipo = "0";
                                }

                                FacturaExcel a = new FacturaExcel
                                {
                                    sociedad = so,
                                    numero_documento =fact,
                                    cliente =cli,
                                    detalle = det,
                                                       

                                };
                                Lista.Add(a);
                            }

                            //Filtrar Excel por Fecha
                            var filtradofecha = _FacturaService.FiltrarExcel(Lista);

                            l.ListaFacturas = filtradofecha;
                            l.ListaFacturasNovalidas = ListaNovalidos;
                            //var resultado=  GuardarDetallesExcel(l);
                            var calculos = new List<FacturaExcel>();

                            if (calculos.Count >= 0)
                            {
                                ViewBag.Error =
                                    "Existen Facturas que pueden estar repetidas compruebe en el Listado de Facturas Nó Validas";
                                l.ListaFacturasNovalidas = calculos;
                                l.ListaFacturas= new List<FacturaExcel>();
                                return View(l);
                            }
                            else
                            {
                                ViewBag.Msg =
                                    "Se Cargo el Archivo  Correctamente";
                                return View(l);
                            }

               



                        }

                    }
                }
                else
                {


                    ViewBag.Error = "El Formato del Archivo Subido debe ser en formato Excel";
                    l.ListaFacturas = Lista;
                    l.ListaFacturasNovalidas = ListaNovalidos;
                    return View(l);


                }
            }
            l.ListaFacturas = Lista;
            l.ListaFacturasNovalidas = ListaNovalidos;
            return View(l);

        }
       
        [System.Web.Mvc.HttpPost]
        public ActionResult GuardarDetallesExcel(HttpPostedFileBase UploadedFile)
     {
            List<FacturaExcel> Lista = new List<FacturaExcel>();
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

                        if (noOfCol > 29 || noOfCol < 29)
                        {
                            ViewBag.Error =
                                "El archivo cargado no correponde al número de datos de Facturas";

                            FacturaExcelModel l = new FacturaExcelModel
                            {

                                ListaFacturas = new List<FacturaExcel>(),


                            };
                            return View("SubirExcel",l);
                        }
                        else
                        {

                            var noOfRow = workSheet.Dimension.End.Row;
                            for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                            {
                                FacturaExcel a = new FacturaExcel
                                {
                                    sociedad = (workSheet.Cells[rowIterator, 1].Value ?? "").ToString(),
                                    numero_documento = (workSheet.Cells[rowIterator, 7].Value ?? "").ToString(),
                                    cliente = (workSheet.Cells[rowIterator, 8].Value ?? "").ToString(),
                                    detalle = (workSheet.Cells[rowIterator, 9].Value ?? "").ToString(),
                         


                                };
                                Lista.Add(a);
                            }

                            //Filtrar Excel por Fecha
                            var filtradofecha = _FacturaService.FiltrarExcel(Lista);

                          
                         
                            FacturaExcelModel l = new FacturaExcelModel
                            {
                                UploadedFile = UploadedFile,
                                ListaFacturas = filtradofecha,


                            };
                            return View("SubirExcel",l);

                        }

                    }
                }
                else
                {


                    ViewBag.Error = "El Formato del Archivo Subido debe ser en formato Excel";
                    return View("SubirExcel");


                }
            }

            return View("SubirExcel");


            }

        public ActionResult AnalisisExcel()
        {
            return View();
        }
        public ActionResult AnalisisExcelC()
        {
            return View();
        }


        public ActionResult CargaFactura([FromBody]HttpPostedFileBase UploadedFile) //Ofertaid
        {

            var TiposFacturas = _FacturaService.CargarArchivosFacturas(UploadedFile);

                            var resultado = JsonConvert.SerializeObject(TiposFacturas,
                                Newtonsoft.Json.Formatting.None,
                                new JsonSerializerSettings
                                {
                                    NullValueHandling = NullValueHandling.Ignore
                                });
             return Content(resultado);


       }
        [System.Web.Mvc.HttpPost]
        public ActionResult Anular(int id)
        {
            var anulado = _FacturaService.AnularFactura(id);

            return RedirectToAction("Index", "Factura");
         
        }


        [System.Web.Mvc.HttpPost]
        public  ActionResult CreateFacturas([FromBody] string a, [FromBody] Facturas data) //AvanceObraId FromBody] List<FacturaExcel> dat
        {

                    var total =  _FacturaService.CrearFacturas(data);
                    if (total.Count > 0) {
                    return Content("Ok");
                    }
                    else
                    {
                    return Content("Error");

                    }
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult CreateFacturasC([FromBody] string a, [FromBody] Facturas data) //Create Cobros List<FacturaExcel> dat
        {
          if (data.Validas != null)
             {
                    var cobros = _FacturaService.CrearCobros(data.Validas);
                    if (cobros.Count > 0)
                    {
                    return Content("Ok");
                    }
                    else
                    {
                    return Content("Error");

                    }

             }
        else
             {
                    return Content("Error");
              }
        }

        public ActionResult ObtenerFacturas()
        {
            var lista_facturas = _FacturaService.GetFacturas();

            var result = JsonConvert.SerializeObject(lista_facturas,
                Newtonsoft.Json.Formatting.None,
                new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    NullValueHandling = NullValueHandling.Ignore
                });
            return Content(result);
        }

        public ActionResult ObtenerFacturasE() // Obtener Empresas
        {
            var lista_facturas = _empresaService.GetEmpresas();

            var result = JsonConvert.SerializeObject(lista_facturas,
                Newtonsoft.Json.Formatting.None,
                new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    NullValueHandling = NullValueHandling.Ignore
                });
            return Content(result);
        }
        public ActionResult ObtenerFacturasC() //ObtenerClientes
        {
            var lista_facturas = _clienteService.GetClientes();

            var result = JsonConvert.SerializeObject(lista_facturas,
                Newtonsoft.Json.Formatting.None,
                new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    NullValueHandling = NullValueHandling.Ignore
                });
            return Content(result);
        }

       
        public ActionResult ObtenerFacturasD(int id=0) //ObtenerDetalles de las Facturas
        {
            var factura = _FacturaService.GetDetalle(id);
            
            if(factura!=null && factura.Id > 0) {
                if (factura.os == null) {
                    factura.os =" ";
                }
                if (factura.ov == null) {
                    factura.ov = " ";
                }
            var result = JsonConvert.SerializeObject(factura,
                Newtonsoft.Json.Formatting.None,
                new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    NullValueHandling = NullValueHandling.Ignore
                });
            return Content(result);
            }
            else { 
            return Content("Q");
            }
        }

        public ActionResult CargaFacturaC([FromBody]HttpPostedFileBase UploadedFile) //Carga de Cobros
        {

            List<FacturaExcel> NoValidas = new List<FacturaExcel>();
            List<FacturaExcel> Lista = new List<FacturaExcel>();
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

                        if (noOfCol > 29 || noOfCol < 29)
                        {
                            return Content("Error");
                        }
                        else
                        {

                            var noOfRow = workSheet.Dimension.End.Row;

                            for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                            {

                                var sociedad = (workSheet.Cells[rowIterator, 4].Value ?? "").ToString();
                                if (sociedad.Equals("") || sociedad == "")
                                {
                                    sociedad = "0";
                                }

                                var fechadocumento = (workSheet.Cells[rowIterator, 15].Value ?? "").ToString();
                                if (fechadocumento.Equals("") || fechadocumento == "")
                                {
                                    fechadocumento = "01/01/1990";
                                }

                                var factura = (workSheet.Cells[rowIterator, 9].Value ?? "").ToString();
                                if (factura.Equals("") || factura == "")
                                {
                                    factura = "0";
                                }

                                var detalle = (workSheet.Cells[rowIterator, 12].Value ?? "").ToString();
                                if (detalle.Equals("") || detalle == "")
                                {
                                    detalle = "0";
                                }

                                var clasedocumento = (workSheet.Cells[rowIterator, 11].Value ?? "").ToString();

                                if (clasedocumento.Equals("") || clasedocumento == "")
                                {
                                    clasedocumento = "0";
                                }

                                var doccompensacion = (workSheet.Cells[rowIterator, 20].Value ?? "").ToString();


                                if (doccompensacion.Equals("") || doccompensacion == "")
                                {
                                    doccompensacion = "0";
                                }

                                var importemoneda = (workSheet.Cells[rowIterator, 19].Value ?? "").ToString();
                                if (importemoneda.Equals("") || importemoneda == "")
                                {
                                    importemoneda = "0";
                                }

                                var fechacompensacion = (workSheet.Cells[rowIterator, 24].Value ?? "").ToString();

                                if (fechacompensacion.Equals("") || fechacompensacion == "")
                                {
                                    fechacompensacion = "01/01/1990";
                                }

                                var fecha_pago = (workSheet.Cells[rowIterator, 16].Value ?? "").ToString();
                                if (fecha_pago.Equals("") || fecha_pago == "")
                                {
                                    fecha_pago = "01/01/1990";
                                }

                                var cliente = (workSheet.Cells[rowIterator, 6].Value ?? "").ToString();

                                if (cliente.Equals("") || cliente == "")
                                {
                                    cliente = "0";
                                }



                                FacturaExcel a = new FacturaExcel
                                {
                                    id = rowIterator,
                                    sociedad = sociedad,
                                    fecha_documento = DateTime.Parse(fechadocumento),
                                    numero_documento = factura,
                                    detalle = detalle,
                                    clase_documento = clasedocumento,
                                    documento_compensacion = doccompensacion,
                                    importe_moneda = Decimal.Parse(importemoneda),
                                    fecha_compensacion = DateTime.Parse(fechacompensacion),
                                    fecha_pago = DateTime.Parse(fecha_pago),
                                    cliente = cliente,
                                    referencia=factura

                                };
                                Lista.Add(a);
                            }
                            //Busca Listados que tengan solo DB
                            var Objeto = _FacturaService.BuscarCobrosDZ(Lista);

                            var resultado = JsonConvert.SerializeObject(Objeto,
                                Newtonsoft.Json.Formatting.None,
                                new JsonSerializerSettings
                                {
                                    NullValueHandling = NullValueHandling.Ignore
                                });
                            return Content(resultado);


                        }

                    }
                }
                else
                {

                    return Content("Formato");


                }
            }

            return Content("SinArchivo");

        }


        public ActionResult ObtenerCobros()
        {
            var lista_cobros = _cobroService.ListaCobrosUnicos();

            var result = JsonConvert.SerializeObject(lista_cobros,
                Newtonsoft.Json.Formatting.None,
                new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    NullValueHandling = NullValueHandling.Ignore
                });
            return Content(result);
        }
        public ActionResult ObtenerCobrosFactura(int Id) //ObtenerCobrosporFactura
        {
            var lista_cobros = _cobroService.ListadeCobrosFactura(Id);

            var result = JsonConvert.SerializeObject(lista_cobros,
                Newtonsoft.Json.Formatting.None,
                new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    NullValueHandling = NullValueHandling.Ignore
                });
            return Content(result);
        }

        public ActionResult ObtenerFacturaporCobro(int Id) //ObtenerCobrosporFactura
        {
            var lista_cobros = _cobroService.ListaFacturaCobros(Id);

            var result = JsonConvert.SerializeObject(lista_cobros,
                Newtonsoft.Json.Formatting.None,
                new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    NullValueHandling = NullValueHandling.Ignore
                });
            return Content(result);
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult Eliminar([FromBody] int  id) //Id Cobro
        {
            var x = _cobrorealService.Eliminar(id);
            if (x) {
                var actualizado = _FacturaService.ActualizarCobros(id);
                return Content("OK");
            }
            return Content("ERROR");
        }

        public ActionResult GetReporteFacturas()
        {
            var excel = _FacturaService.ReporteFacturas();
            string excelName = "ReingresoColaboradores";
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
