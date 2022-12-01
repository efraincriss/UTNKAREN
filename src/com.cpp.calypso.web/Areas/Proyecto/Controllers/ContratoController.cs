using Abp.Application.Services.Dto;
using AutoMapper;
using com.cpp.calypso.framework;
using com.cpp.calypso.proyecto.aplicacion;
using com.cpp.calypso.proyecto.aplicacion.Interfaces;
using com.cpp.calypso.web.Models;
using System;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using Newtonsoft.Json;
using System.IO;
using com.cpp.calypso.proyecto.dominio.Models;
using com.cpp.calypso.proyecto.dominio.Documentos;
using com.cpp.calypso.proyecto.aplicacion.Documentos.Interface;
using com.cpp.calypso.proyecto.aplicacion.Documentos.Dto;
using System.Collections.Generic;

namespace com.cpp.calypso.web.Areas.Proyecto.Controllers
{

    public class ContratoController : BaseController
    {
        private readonly IContratoAsyncBaseCrudAppService contratoService;
        private readonly IEmpresaAsyncBaseCrudAppService empresaService;
        private readonly IClienteAsyncBaseCrudAppService clienteService;
        private readonly IInstitucionFinancieraAsyncBaseCrudAppService ifinacieraService;
        private readonly IGananciaAsyncBaseCrudAppService _gananciaService;
        private readonly IOfertaComercialAsyncBaseCrudAppService _oferta;
        private readonly IOrdenServicioAsyncBaseCrudAppService _ordenServicioService;
        private readonly ICarpetaAsyncBaseCrudAppService _carpetaService;
        private readonly ISeccionAsyncBaseCrudAppService _seccionService;
        public ContratoController(IHandlerExcepciones manejadorExcepciones, IContratoAsyncBaseCrudAppService contratoService,
         IEmpresaAsyncBaseCrudAppService empresaService,
             IClienteAsyncBaseCrudAppService clienteService,
             IInstitucionFinancieraAsyncBaseCrudAppService ifinacieraService,
             IOfertaComercialAsyncBaseCrudAppService ofertaRepository,
              IOrdenServicioAsyncBaseCrudAppService ordenServicioService,
        IGananciaAsyncBaseCrudAppService gananciaService,
        ISeccionAsyncBaseCrudAppService seccionService,
        ICarpetaAsyncBaseCrudAppService carpetaService) : base(manejadorExcepciones)
        {
            this.empresaService = empresaService;
            this.clienteService = clienteService;
            this.contratoService = contratoService;
            this.ifinacieraService = ifinacieraService;
            _gananciaService = gananciaService;
            _oferta = ofertaRepository;
            _ordenServicioService = ordenServicioService;
            _carpetaService = carpetaService;
            _seccionService = seccionService;
        }

        // GET: Proyecto/Contrato
        public ActionResult Index(String message)
        {
            var input = new comun.aplicacion.PagedAndFilteredResultRequestDto();
            input.MaxResultCount = 50;

            //var result = await contratoService.GetAll(input);
            var result = contratoService.GetContratos();
            if (message != null)
            {

                ViewBag.Msg = message;
            }

            return View(result);
        }

        /*GET: Proyecto/Contrato/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }*/
        public async System.Threading.Tasks.Task<ActionResult> Details(int? id, string message)
        {

            if (message != null)
            {

                ViewBag.Msg = message;
            }
            if (!id.HasValue)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                var contrato = await contratoService.GetDetalle(id.Value);
                var cliente = await clienteService.Get(new EntityDto<int>(contrato.ClienteId));
                var empresa = await empresaService.Get(new EntityDto<int>(contrato.EmpresaId));
                var contratodocumentobancario = contratoService.GetContratoDocumentoBancarios(id.Value);
                var centrocostoscontrato = contratoService.GetCentrocostosContratos(id.Value);
                var adenda = contratoService.GetAdendas(id.Value);
                var proyecto = contratoService.GetProyectos(id.Value);
                var ganancias = _gananciaService.GetGanaciasporContrato(id.Value);
                if (contrato == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.NotFound);
                }
                else
                {
                    var ViewModel = new ContratoDocumentoCentrosViewModel
                    {

                        Contrato = contrato,
                        Cliente = cliente,
                        Empresa = empresa,
                        ContratoDocumentoBancario = contratodocumentobancario,
                        CentrocostosContrato = centrocostoscontrato,
                        Adenda = adenda,
                        Proyecto = proyecto,
                        Ganancias = ganancias
                    };
                    ViewBag.ruta = new string[] { "Inicio", "Administración de Contratos", "Contratos" };
                    return View(ViewModel);
                }
            }
        }


        // GET: Proyecto/Contrato/Create
        public ActionResult Create()
        {
            ContratoDto contrato = new ContratoDto();
            contrato.estado_contrato = true;
            contrato.fecha_firma = DateTime.Now;
            contrato.fecha_inicio = DateTime.Now;
            contrato.Empresas = empresaService.GetEmpresas();
            contrato.Clientes = clienteService.GetClientes();
            return View(contrato);

        }

        // POST: Proyecto/Contrato/Create
        [System.Web.Mvc.HttpPost]
        [ValidateAntiForgeryToken]
        public async System.Threading.Tasks.Task<ActionResult> Create(ContratoDto contrato)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    contrato.vigente = true;
                    var result = await contratoService.Create(contrato);
                    // var proyecto = contratoService.CrearProyectoporContratoAsync(contrato,result.Id);
                    ViewBag.Msg = "Contrato creado satisfactoriamente";
                    return RedirectToAction("Details", new RouteValueDictionary(
                        new { controller = "Contrato", action = "Details", Id = result.Id }));
                }
                else
                {

                    var dto = Mapper.Map<ContratoDto>(contrato);
                    dto.vigente = true;
                    dto.Empresas = empresaService.GetEmpresas();
                    dto.Clientes = clienteService.GetClientes();
                    return View("Create", dto);

                }


            }
            catch
            {
                return View();
            }
        }


        // GET: Proyecto/Contrato/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (!id.HasValue)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                //var input = new comun.aplicacion.PagedAndFilteredResultRequestDto();
                //input.Filter.FirstOrDefault()
                var contratoDto = await contratoService.GetDetalle(id.Value);
                contratoDto.Empresas = empresaService.GetEmpresas();
                contratoDto.Clientes = clienteService.GetClientes();
                if (contratoDto == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.NotFound);
                }
                else
                {
                    return View(contratoDto);
                }
            }
        }


        // POST: Proyecto/Contrato/Edit/5
        [System.Web.Mvc.HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> Edit(int id, ContratoDto contratoDto)
        {
            try
            {
                //empresaService.InsertOrUpdateAsync(empresaDto);
                var contrato = await contratoService.InsertOrUpdateAsync(contratoDto);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult GetContratos()
        {
            var contratos = contratoService.GetContratosDto();
            var result = JsonConvert.SerializeObject(contratos);
            return Content(result);
        }

        // POST: Proyecto/Contrato/Delete/5
        [System.Web.Mvc.HttpPost]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id.HasValue)
            {
                var result = await contratoService.EliminarVigencia(id.Value);
                if (result)
                {
                    string Mensaje = "No se puede Eliminar tiene datos relacionados";
                    return RedirectToAction("Index", "Contrato", new { message = Mensaje });
                }
                else
                {
                    return RedirectToAction("Index", "Contrato");

                }


            }

            return RedirectToAction("Index");
        }


        [System.Web.Mvc.HttpPost]
        public ActionResult GetContratosApi()
        {
            var contratos = contratoService.GetContratos();
            var result = JsonConvert.SerializeObject(contratos);
            return Content(result);
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult GetClientes()
        {
            var clientes = contratoService.InfoCliente();
            var result = JsonConvert.SerializeObject(clientes);
            return Content(result);
        }
        [System.Web.Mvc.HttpPost]
        public ActionResult GetContratosbyCliente(int Id)
        {
            var contratos = contratoService.InfoContrato(Id);
            var result = JsonConvert.SerializeObject(contratos);
            return Content(result);
        }
        public ActionResult GetReportes()
        {
            return View();
        }

        public ActionResult GetReporteChart(ReportDto r)
        {
            var excel = contratoService.StackedColumn(r);
            string excelName = "Stacked Column";
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
        public ActionResult GetReportePo(ReportDto r)
        {
            var excel = _ordenServicioService.ReportePOS(r);
            string excelName = "Reporte_POS_" + DateTime.Now.ToShortDateString();
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


        public ActionResult GetReporteAdicionales(ReportDto r)
        {
            var excel = _oferta.ReporteAdicionales(r);
            string excelName = "Reporte_Adicionales_" + DateTime.Now.ToShortDateString();
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
        public ActionResult GetReporteDetalleProyectos(ReportDto r)
        {
            var excel = _oferta.ReporteDetalladosAdicionales(r);
            string excelName = "Reporte_Detalle_de_Proyectos" + DateTime.Now.ToShortDateString();
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
        public ActionResult GetReporteSeguimientoOferta(ReportDto r)
        {
            var excel = _oferta.SeguimientoComercial(r);
            string excelName = "Seguimiento_Comercial" + DateTime.Now.ToShortDateString();
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

        public ActionResult GetOpenOutlook()
        {
            var rs = contratoService.OpenOutlookProcess();
            var result = JsonConvert.SerializeObject(rs);
            return Content(result);
        }

        public ActionResult GetOpenOutlook2()
        {
            var rs = contratoService.OpenOutlook();
            var result = JsonConvert.SerializeObject(rs);
            return Content(result);
        }
        [System.Web.Mvc.HttpPost]
        public ActionResult GetActualizarAzureSecciones(List<SeccionDto>data)
        {
            string result = "OK";
            // _seccionService.actualizarSeccion();
            //var rs = _carpetaService.SyncSecciones(data);
            // result = JsonConvert.SerializeObject(rs);
            return Content(result);
        }
        [System.Web.Mvc.HttpPost]
        public ActionResult GetActualizarAzureImagenes(List<ImagenSeccionDto> data)
        {
            string result = "OK";
            // _seccionService.actualizarSeccion();
            //var rs = _carpetaService.SyncImagenesSecciones(data);
            // result = JsonConvert.SerializeObject(rs);
            return Content(result);
        }


        public ActionResult GetDataSyncSecciones()
        {
            var secciones = _carpetaService.SeccionesDES();
            var imangesSecciones = _carpetaService.ImagenesDES();
            var sync = new Sync()
            {
                secciones = secciones,
                imagenesSeccion = imangesSecciones
            };


            var result = JsonConvert.SerializeObject(sync);
            return Content(result);

        }

        public ActionResult GetActualizarAzure()
        {
            string result = "OK";
             //_seccionService.actualizarSeccion();
           // var rs = _carpetaService.SyncSecciones();
           // var ris = _carpetaService.SyncImagenesSecciones();
            // result = JsonConvert.SerializeObject(rs);
            return Content(result);
        }
        public ActionResult GetPass(string pass) {

            var result = _carpetaService.VerificarContraseña(pass);
            return Content(result);
        }

        public ActionResult GetFechasSincronizacion()
        {
            var fechas = _carpetaService.UltimasFechasSincronizacion();
            var result = JsonConvert.SerializeObject(fechas);
            return Content(result);
        }
        public ActionResult SyncUsuarios()
        {
            var result = _carpetaService.SyncUsuarios("UsuariosAutorizados");
            return Content(result);
        }
        public ActionResult SyncCarpetas()
        {          
           var result = _carpetaService.SyncCarpetas("Carpetas");     
           return Content(result);
        }
        public ActionResult SyncDocumentos()
        {
            var result = _carpetaService.SyncDocumentos("Documentos");
            return Content(result);
        }
        public ActionResult SyncSecciones()
        {    
            var result = _carpetaService.SyncSecciones("Secciones");      
            return Content(result);
        }

         
        public ActionResult SyncImg() {
            var result = _carpetaService.SyncImagenesSecciones("ImagenesSeccion");
            return Content(result);
        }
        
            public ActionResult SyncImagenes()
        {
            var result = _carpetaService.SyncImagenesSecciones("ImagenesSeccion");
            return Content(result);
        }



        public ActionResult Sync()
        {
            string result = "OK";
                 //    var rsi = _carpetaService.SyncImagenesSecciones(null,null);
            result = JsonConvert.SerializeObject(result);
            return Content(result);
        }
        public ActionResult SyncImagenes(int id, int id2)
        {
            string result = "OK";
           ///var doc = _carpetaService.ActualizarDocumentos();
            //var rs = _carpetaService.SyncSecciones();
           // var rsi = _carpetaService.SyncImagenesSecciones(id, id2);
            // result = JsonConvert.SerializeObject(rsi);
            return Content(result);
        }
        public ActionResult SyncImagenesLista(List<int>id)
        {
            string result = "OK";
            ///var doc = _carpetaService.ActualizarDocumentos();
            //var rs = _carpetaService.SyncSecciones();
            var rsi = _carpetaService.SyncImagenesSeccionesLista(id);
            // result = JsonConvert.SerializeObject(rsi);
            return Content(result);
        }
      
        public ActionResult GetSync()
        {
            var excel = _carpetaService.SyncImagenesExcel();
            string excelName = "ImagenesSYNC" + DateTime.Now.ToShortDateString();
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
