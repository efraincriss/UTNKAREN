using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Abp.Application.Services.Dto;
using com.cpp.calypso.framework;
using com.cpp.calypso.proyecto.aplicacion;
using com.cpp.calypso.proyecto.aplicacion.Dto;
using com.cpp.calypso.proyecto.aplicacion.Interfaces;
using com.cpp.calypso.proyecto.dominio;
using com.cpp.calypso.web.Areas.Proyecto.Models;
using Newtonsoft.Json;
using OfficeOpenXml;

namespace com.cpp.calypso.web.Areas.Proyecto.Controllers
{


    public class CertificadoController : BaseController
    {
        private readonly ICertificadoAsyncBaseCrudAppService _certificadoService;
        private readonly IDetalleCertificadoAsyncBaseCrudAppService _detallescertificadoService;
        private readonly IEmpresaAsyncBaseCrudAppService _empresaService;
        private readonly IContratoAsyncBaseCrudAppService _contratoService;
        private readonly IProyectoAsyncBaseCrudAppService _proyectoService;
        private readonly IClienteAsyncBaseCrudAppService _clienteService;
        private readonly IAvanceObraAsyncBaseCrudAppService _avanceObraService;
        private readonly IAvanceIngenieriaAsyncBaseCrudAppService _avanceIngenieriaService;
        private readonly IAvanceProcuraAsyncBaseCrudAppService _avanceProcuraService;
        private readonly IDetalleAvanceObraAsyncBaseCrudAppService _davanceObraService;
        private readonly IDetalleAvanceIngenieriaAsyncBaseCrudAppService _davanceIngenieriaService;
        private readonly IDetalleAvanceProcuraAsyncBaseCrudAppService _davanceProcuraService;

        public CertificadoController(IHandlerExcepciones manejadorExcepciones,
            ICertificadoAsyncBaseCrudAppService certificadoService,
            IDetalleCertificadoAsyncBaseCrudAppService detallescertificadoService,
            IEmpresaAsyncBaseCrudAppService empresaService,
            IClienteAsyncBaseCrudAppService clienteService,
            IProyectoAsyncBaseCrudAppService proyectoService,
            IContratoAsyncBaseCrudAppService contratoService,
            IAvanceObraAsyncBaseCrudAppService avanceObraService,
            IAvanceIngenieriaAsyncBaseCrudAppService avanceIngenieriaService,
            IAvanceProcuraAsyncBaseCrudAppService avanceProcuraService,
            IDetalleAvanceObraAsyncBaseCrudAppService davanceObraService,
        IDetalleAvanceIngenieriaAsyncBaseCrudAppService davanceIngenieriaService,
         IDetalleAvanceProcuraAsyncBaseCrudAppService davanceProcuraService
        ) : base(manejadorExcepciones)
        {
            _certificadoService = certificadoService;
            _detallescertificadoService = detallescertificadoService;
            _empresaService = empresaService;
            _clienteService = clienteService;
            _proyectoService = proyectoService;
            _contratoService = contratoService;
            _avanceObraService = avanceObraService;
            _avanceIngenieriaService = avanceIngenieriaService;
            _avanceProcuraService = avanceProcuraService;
            _davanceObraService = davanceObraService;
            _davanceIngenieriaService = davanceIngenieriaService;
            _davanceProcuraService = davanceProcuraService; ;
        }

        // GET: Proyecto/Certificado
        public ActionResult Index(string message)
        {

            if (message != null)
            {
                ViewBag.Msg = message;
            }

            var certificados = _certificadoService.Listar();
            return View(certificados);
        }

        public ActionResult IndexExcel(int id, int id2)
        {
            var certificado = _certificadoService.GetDetalleCertificado(id);

            ExcelPackage excel = _certificadoService.GenerarExcelCertificado(id, id2);


            //string excelName = "Certificado" + "-" + certificado.fecha_corte.ToShortDateString();
            string excelName = _certificadoService.NombreCertificado(id, id2);
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

        // GET: Proyecto/Certificado/Details/5
        public ActionResult Details()
        {


            return View();
        }
        public ActionResult EnableDisableApi(int id, string pass)
        {
            var lsita = _certificadoService.desaprobar(id, pass);     
            return Content(lsita?"OK":"ERROR");
        }



      



        public ActionResult DetailsCertificado(int id, string message, string message2)
        {
            if (message != null)
            {
                ViewBag.Msg = message;
            }
            if (message2 != null)
            {
                ViewBag.Error = message2;
            }
            var certificado = _certificadoService.getdetalle(id);
            var _detalle = _detallescertificadoService.Listar(certificado.Id, certificado.tipo_certificado);
            var montosTotales = _certificadoService.ObtenerMontosCertificadosCabeceras(certificado.Id, certificado.ProyectoId);

            CertificadoDetalleViewModel n = new CertificadoDetalleViewModel
            {
                Certificado = certificado,
                detalles = _detalle,
                montoTotalAIU = montosTotales.montoTotalAIU,
                montoTotalSinAIU=montosTotales.montoTotalsinAIU

            };
            return View(n);
        }
        // GET: Proyecto/Certificado/Create
        public ActionResult Create()
        {

            CertificadoDto n = new CertificadoDto();
            n.Empresas = _empresaService.GetEmpresas();
            n.Clientes = _clienteService.GetClientes();
            n.Contratos = _contratoService.GetContratos();
            n.fecha_emision = DateTime.Now;
            n.fecha_corte = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 25);
            n.estado_actual = 0;
            n.vigente = true;
            n.numero_certificado = "aa";

            return View(n);
        }

        // POST: Proyecto/Certificado/Create
        [System.Web.Mvc.HttpPost]
#pragma warning disable CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        public async System.Threading.Tasks.Task<ActionResult> Create(CertificadoDto n)
#pragma warning restore CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        {
            if (ModelState.IsValid)
            {


                var p = _proyectoService.GetDetalles(n.ProyectoId);
                var c = p.Contrato;
                var cli = c.Cliente;
                string fechacorte = n.fecha_corte.ToShortDateString();
                string fechaemision = n.fecha_emision.ToShortDateString();
                string periodohasta = n.fecha_corte.ToShortDateString();
                string periododesde = n.fecha_corte.ToShortDateString();


                CertificadoViewModel v = new CertificadoViewModel
                {

                    Proyecto = p,
                    Contrato = c,
                    Cliente = cli,
                    fechacorte = fechacorte,
                    certificado = n.Id,
                    fechaemision = fechaemision
                }
                    ;


                return View("Details", v);

            }

            return View();
        }

        // GET: Proyecto/Certificado/Edit/5
        public ActionResult Edit(int id)
        {

            var c = _certificadoService.getdetalle(id);
            c.Empresas = _empresaService.GetEmpresas();
            c.Clientes = _clienteService.GetClientes();
            c.Contratos = _contratoService.GetContratos();
            return View(c);
        }

        // POST: Proyecto/Certificado/Edit/5
        [System.Web.Mvc.HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> Edit(CertificadoDto c)
        {
            if (ModelState.IsValid)
            {
                c.vigente = true;
                var resultado = await _certificadoService.Update(c);

                return RedirectToAction("Index");

            }

            return View();
        }

        // POST: Proyecto/Certificado/Delete/5
        [System.Web.Mvc.HttpPost]
        public ActionResult Delete(int id)
        {
            bool r = _certificadoService.Eliminar(id);
            if (r)
            {
                return RedirectToAction("Index");


            }
            else
            {

                string Mensaje = "No se puede eliminar el registro, tiene datos relacionados";
                return RedirectToAction("Index", "Certificado", new { message = Mensaje });
            }


        }

        public ActionResult AvanceObraSinCertificar(int id, DateTime fechaCorte)
        {
            var lsita = _avanceObraService.ListarDetallesAvanceObraProyectoFast(id, fechaCorte);
            var result = JsonConvert.SerializeObject(lsita,
                Newtonsoft.Json.Formatting.None,
                new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    NullValueHandling = NullValueHandling.Ignore
                });
            return Content(result);
        }

        public ActionResult AvancesCertificadosC(int id)
        {
            var lsita = _detallescertificadoService.Listar(id, 2);
            var result = JsonConvert.SerializeObject(lsita,
                Newtonsoft.Json.Formatting.None,
                new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    NullValueHandling = NullValueHandling.Ignore
                });
            return Content(result);
        }

        public ActionResult AvancesCertificadosI(int id)
        {
            var lsita = _detallescertificadoService.ListarI(id);
            var result = JsonConvert.SerializeObject(lsita,
                Newtonsoft.Json.Formatting.None,
                new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    NullValueHandling = NullValueHandling.Ignore
                });
            return Content(result);
        }

        public ActionResult AvancesCertificadosP(int id)
        {
            var lsita = _detallescertificadoService.ListarP(id);
            var result = JsonConvert.SerializeObject(lsita,
                Newtonsoft.Json.Formatting.None,
                new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    NullValueHandling = NullValueHandling.Ignore
                });
            return Content(result);
        }

        public async System.Threading.Tasks.Task<ActionResult> DetalleProyecto(int id)
        {
            var proyecto = await _proyectoService.Get(new EntityDto<int>(id));

            var contrato = await _contratoService.GetDetalle(id);
            proyecto.Contrato = AutoMapper.Mapper.Map<Contrato>(contrato);
            var result = JsonConvert.SerializeObject(proyecto,
                Newtonsoft.Json.Formatting.None,
                new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    NullValueHandling = NullValueHandling.Ignore
                });
            return Content(result);
        }

        public ActionResult DetalleCertificado(int id)
        {
            var certificado = _certificadoService.getdetalle(id);

            var result = JsonConvert.SerializeObject(certificado,
                Newtonsoft.Json.Formatting.None,
                new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    NullValueHandling = NullValueHandling.Ignore
                });
            return Content(result);
        }

        public ActionResult AprobarCertificado(int id)
        {
            bool r = _certificadoService.cambiarestadocertificado(id);
            if (r)
            {
                return Content("OK");
            }
            else
            {
                return Content("Error");
            }

        }
        public ActionResult CarcelarCertificado(int id)
        {
            bool r = _certificadoService.cancelarestadocertificado(id);
            if (r)
            {
                return Content("OK");
            }
            else
            {
                return Content("Error");
            }

        }

        public ActionResult AvanceIngenieriaSinCertificar(int id)
        {
            var lsita = _avanceIngenieriaService.ListarDetallesAvanceIngenieriaProyecto(id);
            var result = JsonConvert.SerializeObject(lsita,
                Newtonsoft.Json.Formatting.None,
                new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    NullValueHandling = NullValueHandling.Ignore
                });
            return Content(result);
        }

        public ActionResult AvanceProcuraSinCertificar(int id)
        {
            var lsita = _avanceProcuraService.ListarDetallesAvanceProcuraProyecto(id);
            var result = JsonConvert.SerializeObject(lsita,
                Newtonsoft.Json.Formatting.None,
                new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    NullValueHandling = NullValueHandling.Ignore
                });
            return Content(result);
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult IngresarDetallesCertificadosConstruccion([FromBody] List<DetalleAvanceObraDto> data,
            [FromBody] int CertificadoId)
        {
            if (data != null && data.Count > 0 && CertificadoId > 0)
            {
                var r = _detallescertificadoService.InsertarDetallesObra(data, CertificadoId);
                if (r)
                {
                    return Content("OK");
                }
                else
                {

                    return Content("ERROR");
                }
            }
            else
            {
                return Content("ERROR2");
            }

        }

        [System.Web.Mvc.HttpPost]
        public ActionResult IngresarDetallesCertificadosIngenieria([FromBody] List<DetalleAvanceIngenieriaDto> data,
            [FromBody] int CertificadoId)
        {
            if (data != null && data.Count > 0 && CertificadoId > 0)
            {
                var r = _detallescertificadoService.InsertarDetallesIngenieria(data, CertificadoId);
                if (r)
                {
                    return Content("OK");
                }
                else
                {

                    return Content("ERROR");
                }
            }
            else
            {
                return Content("ERROR2");
            }
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult IngresarDetallesCertificadosProcura([FromBody] List<DetalleAvanceProcuraDto> data,
            [FromBody] int CertificadoId)
        {

            if (data != null && data.Count > 0 && CertificadoId > 0)
            {
                var r = _detallescertificadoService.InsertarDetallesProcura(data, CertificadoId);
                if (r)
                {
                    return Content("OK");
                }
                else
                {

                    return Content("ERROR");
                }
            }
            else
            {
                return Content("ERROR2");
            }
        }

        public ActionResult MontoPresupuestoConstruccion(int id)
        {
            var presupuesto = _certificadoService.MontoPresupuestoConstruccion(id);
            var result = JsonConvert.SerializeObject(presupuesto,
                Newtonsoft.Json.Formatting.None,
                new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    NullValueHandling = NullValueHandling.Ignore
                });
            return Content(result);
        }

        public ActionResult MontoPresupuestoIngenieria(int id)
        {
            var presupuesto = _certificadoService.MontoPresupuestoIngenieria(id);
            var result = JsonConvert.SerializeObject(presupuesto,
                Newtonsoft.Json.Formatting.None,
                new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    NullValueHandling = NullValueHandling.Ignore
                });
            return Content(result);
        }

        public ActionResult MontoPresupuestoProcura(int id)
        {
            var presupuesto = _certificadoService.MontoPresupuestoProcura(id);
            var result = JsonConvert.SerializeObject(presupuesto,
                Newtonsoft.Json.Formatting.None,
                new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    NullValueHandling = NullValueHandling.Ignore
                });
            return Content(result);
        }

        public ActionResult MontosCertificados(int id)
        {
            decimal[] Observaciones = new decimal[10];

            //Montos Presupuesto
            decimal montopconstrucion = _certificadoService.MontoPresupuestoConstruccion(id);
            decimal montopingenieria = _certificadoService.MontoPresupuestoIngenieria(id);
            decimal montopprocura = _certificadoService.MontoPresupuestoProcura(id);
            decimal totalp = montopconstrucion + montopingenieria + montopprocura;

            //Montos Certificados
            decimal montoa = montopconstrucion * (Convert.ToDecimal(45.2 / 100));
            decimal montoi = montopconstrucion * (Convert.ToDecimal(3 / 100));
            decimal montou = montopconstrucion * (Convert.ToDecimal(12 / 100));
            decimal montopc = montopprocura * (Convert.ToDecimal(10 / 100));
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

        public ActionResult ObtenerProyectos(int id)
        {
            List<ProyectoDto> listaproyectos = _contratoService.GetProyectos(id);


            var resultado = JsonConvert.SerializeObject(listaproyectos,
                Newtonsoft.Json.Formatting.None,
                new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    NullValueHandling = NullValueHandling.Ignore
                });
            return Content(resultado);
        }

        [System.Web.Mvc.HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> IngresarDetallesCertificados([FromBody]  int[] data,
          [FromBody] int proyectoId, [FromBody] string fechaCorte)
        {
            var pro = _proyectoService.GetDetalles(proyectoId);

            if (data != null && data.Length > 0)
            {
                CertificadoDto n = new CertificadoDto();
                n.fecha_emision = DateTime.Now;
                n.fecha_corte = DateTime.Parse(fechaCorte);
                n.estado_actual = 0;
                n.tipo_certificado = 2;
                n.numero_certificado = pro.codigo + "-CON-EC-" + new Random().Next(1, 10);
                n.vigente = true;
                n.ProyectoId = pro.Id;

                var certificado = await _certificadoService.Create(n);
                if (data != null && data.Length > 0)
                {

                    List<DetalleAvanceObraDto> nuevo = new List<DetalleAvanceObraDto>();

                    foreach (var ids in data)
                    {
                        var d = await _davanceObraService.Get(new EntityDto<int>(ids));
                        nuevo.Add(AutoMapper.Mapper.Map<DetalleAvanceObraDto>(d));
                    }
                    var r = _detallescertificadoService.InsertarDetallesObra(nuevo, certificado.Id);

                }

            }

         
            var x = _detallescertificadoService.actualizarmontoscertificados();
            return Content("OK");
        }



        public ActionResult APIContratos()
        {
            var data = _certificadoService.GetListContratos();
            var result = JsonConvert.SerializeObject(data,
                Newtonsoft.Json.Formatting.None,
                new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    NullValueHandling = NullValueHandling.Ignore
                });
            return Content(result);
        }
        public ActionResult APIProyectos(int Id)//ContratoId
        {
            var data = _certificadoService.GetListProyecto(Id);
            var result = JsonConvert.SerializeObject(data,
                Newtonsoft.Json.Formatting.None,
                new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    NullValueHandling = NullValueHandling.Ignore
                });
            return Content(result);
        }
        public ActionResult APIDetalles(int Id)//Certificado
        {
            var data = _certificadoService.GetDetalleCertificado(Id);
            if (data != null)
            {
                var result = JsonConvert.SerializeObject(data,
                    Newtonsoft.Json.Formatting.None,
                    new JsonSerializerSettings
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                        NullValueHandling = NullValueHandling.Ignore
                    });
                return Content(result);
            }
            else {
                return Content("SIN_CERTIFICADO");
            }
        }

        [System.Web.Http.HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> GenerarCertificados(int[]data,int proyectoId, string fechaCorte, string fechaEmision)
        {
            int CertificadoID = 0;
            var pro = _proyectoService.GetDetalles(proyectoId);
            string Id = "";

            if (data != null && data.Length > 0)
            {
                CertificadoDto n = new CertificadoDto();
                n.fecha_emision = DateTime.Parse(fechaEmision);
                n.fecha_corte = DateTime.Parse(fechaCorte);
                n.estado_actual = 0;
                n.tipo_certificado = 2;
                n.numero_certificado = pro.codigo + "-CON-EC-" + new Random().Next(1, 10);
                n.vigente = true;
                n.ProyectoId = pro.Id;

                var certificado = await _certificadoService.Create(n);
                if (data != null && data.Length > 0)
                {

                    /* List<DetalleAvanceObraDto> nuevo = new List<DetalleAvanceObraDto>();

                     foreach (var ids in data)
                     {
                         var d = await _davanceObraService.Get(new EntityDto<int>(ids));
                         nuevo.Add(AutoMapper.Mapper.Map<DetalleAvanceObraDto>(d));
                     }
                     var r = _detallescertificadoService.InsertarDetallesObra(nuevo, certificado.Id);
                      */
                    var r = _detallescertificadoService.InsertarDetallesObraFast(data, certificado.Id);
                }
                Id = certificado.Id.ToString();
                CertificadoID = certificado.Id;

            }


            var x = _detallescertificadoService.actualizarmontoscertificadosCerficado(CertificadoID);
            return Content(Id);
        }
    }
}
