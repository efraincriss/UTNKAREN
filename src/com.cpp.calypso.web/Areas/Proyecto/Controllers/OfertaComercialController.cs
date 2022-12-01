using Abp.Application.Services.Dto;
using com.cpp.calypso.framework;
using com.cpp.calypso.proyecto.aplicacion;
using com.cpp.calypso.proyecto.aplicacion.Dto;
using com.cpp.calypso.proyecto.aplicacion.Interfaces;
using com.cpp.calypso.proyecto.aplicacion.Service;
using com.cpp.calypso.proyecto.dominio;
using com.cpp.calypso.proyecto.dominio.Models;
using com.cpp.calypso.web.Areas.Proyecto.Models;
using Newtonsoft.Json;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using JsonResult = com.cpp.calypso.framework.JsonResult;
namespace com.cpp.calypso.web.Areas.Proyecto.Controllers
{
    public class OfertaComercialController : BaseController
    {

        private readonly IOfertaComercialAsyncBaseCrudAppService _ofertacomercial;
        private readonly ICatalogoAsyncBaseCrudAppService _catalogo;

        private readonly IOfertaComercialPresupuestoAsyncBaseCrudAppService _ofertacomercialpresupuesto;
        private readonly IOrdenServicioAsyncBaseCrudAppService _ordenservicio;
        private readonly IDetalleOrdenServicioAsyncBaseCrudAppService _detalleordenservicio;
        private readonly IItemAsyncBaseCrudAppService _itemservice;
        private readonly IGrupoItemAsyncBaseCrudAppService _grupoitemservice;
        private readonly ITransmitalCabeceraAsyncBaseCrudAppService _transmitalservice;
        private readonly IPresupuestoAsyncBaseCrudAppService _presupuestoService;
        public IArchivoAsyncBaseCrudAppService ArchivoService { get; }

        public OfertaComercialController(IHandlerExcepciones manejadorExcepciones,
            IOfertaComercialAsyncBaseCrudAppService ofertacomercial,
            ICatalogoAsyncBaseCrudAppService catalogo,
            IOfertaComercialPresupuestoAsyncBaseCrudAppService ofertacomercialpresupuesto,
            IOrdenServicioAsyncBaseCrudAppService ordenservicio,
            IItemAsyncBaseCrudAppService itemservice,
            IPresupuestoAsyncBaseCrudAppService presupuestoService,
             IDetalleOrdenServicioAsyncBaseCrudAppService detalleordenservicio,
             IGrupoItemAsyncBaseCrudAppService grupoitemservice,
             ITransmitalCabeceraAsyncBaseCrudAppService transmitalservice,
                     IArchivoAsyncBaseCrudAppService archivoService


      ) : base(manejadorExcepciones)
        {
            _ofertacomercial = ofertacomercial;
            _catalogo = catalogo;
            _ofertacomercialpresupuesto = ofertacomercialpresupuesto;
            _ordenservicio = ordenservicio;
            _itemservice = itemservice;
            _presupuestoService = presupuestoService;
            _detalleordenservicio = detalleordenservicio;
            _grupoitemservice = grupoitemservice;
            _transmitalservice = transmitalservice;
            ArchivoService = archivoService;
        }

        // GET: Proyecto/OfertaComercial
        public ActionResult Index()
        {
            ViewBag.ruta = new string[] { "Inicio", "Ofertas Comerciales", "Listado" };

            return View();
        }

        public ActionResult IndexPresupuestos()
        {
            ViewBag.ruta = new string[] { "Inicio", "Presupuestos Liberados" };

            return View();
        }

        // GET: Proyecto/OfertaComercial/Details/5
        public ActionResult Details(int id = 0)
        {
            ViewBag.ruta = new string[] { "Inicio", "Ofertas Comerciales", "Gestión" };
            var ofertacomercial = _ofertacomercial.GetDetalles(id);

            return View(ofertacomercial);
        }
        public ActionResult DetailsOferta(int id)
        {
            ViewBag.ruta = new string[] { "Inicio", "Ofertas Comerciales", "Gestión" };
            ViewBag.Id = id;
            return View();
        }

        public ActionResult Reportes()
        {
            return View();

        }

        // GET: Proyecto/OfertaComercial/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Proyecto/OfertaComercial/Create
        [System.Web.Http.HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Proyecto/OfertaComercial/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Proyecto/OfertaComercial/Edit/5
        [System.Web.Http.HttpPost]
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

        // GET: Proyecto/OfertaComercial/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Proyecto/OfertaComercial/Delete/5
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

        //Oferta Comercial

        public ActionResult UpdateMontos()
        {
            var result = _ofertacomercial.ActualizarMontoAprobadoSegunEstadoOferta();

            return Content(result);
        }

        public ActionResult Listar()
        {
            var lsita = _ofertacomercial.Lista();

            var result = JsonConvert.SerializeObject(lsita,
                Newtonsoft.Json.Formatting.None,
                new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    NullValueHandling = NullValueHandling.Ignore

                });
            return Content(result);
        }

        public ActionResult ListarporContrato(int Id) // Oferta Comercial por Contrato
        {
            var lsita = _ofertacomercial.ListaContrato(Id);

            var result = JsonConvert.SerializeObject(lsita,
                Newtonsoft.Json.Formatting.None,
                new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    NullValueHandling = NullValueHandling.Ignore

                });
            return Content(result);
        }
        public ActionResult ListarArchivos(int Id)
        {
            var lsita = _ofertacomercial.ListaArchivos(Id);

            var result = JsonConvert.SerializeObject(lsita,
                Newtonsoft.Json.Formatting.None,
                new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    NullValueHandling = NullValueHandling.Ignore

                });
            return Content(result);
        }
        public ActionResult ListarVersiones(int id = 0)
        {
            if (id > 0)
            {


                var lsita = _ofertacomercial.ListaVersiones(id);

                var result = JsonConvert.SerializeObject(lsita,
                    Newtonsoft.Json.Formatting.None,
                    new JsonSerializerSettings
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                        NullValueHandling = NullValueHandling.Ignore
                    });
                return Content(result);
            }
            else
            {
                return Content("n");
            }

        }

        public ActionResult ListarPresupuesto(int id = 0)
        {
            if (id > 0)
            {


                var lsita = _ofertacomercialpresupuesto.Listar(id);

                var result = JsonConvert.SerializeObject(lsita,
                    Newtonsoft.Json.Formatting.None,
                    new JsonSerializerSettings
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                        NullValueHandling = NullValueHandling.Ignore
                    });
                return Content(result);
            }
            else
            {
                return Content("n");
            }

        }

        public ActionResult ListarOS(int id = 0)
        {
            if (id > 0)
            {


                var lsita = _ordenservicio.ListarOsByOferta(id);

                var result = JsonConvert.SerializeObject(lsita,
                    Newtonsoft.Json.Formatting.None,
                    new JsonSerializerSettings
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                        NullValueHandling = NullValueHandling.Ignore
                    });
                return Content(result);
            }
            else
            {
                return Content("n");
            }

        }

        public ActionResult ListarDetalleOS(int id = 0)
        {
            if (id > 0)
            {


                var lsita = _detalleordenservicio.listar(id);

                var result = JsonConvert.SerializeObject(lsita,
                    Newtonsoft.Json.Formatting.None,
                    new JsonSerializerSettings
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                        NullValueHandling = NullValueHandling.Ignore
                    });
                return Content(result);
            }
            else
            {
                return Content("n");
            }

        }
        public ActionResult GetDetalleOferta(int Id)
        {
            var ofertacomercial = _ofertacomercial.GetDetalles(Id);

            var result = JsonConvert.SerializeObject(ofertacomercial,
                Newtonsoft.Json.Formatting.None,
                new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    NullValueHandling = NullValueHandling.Ignore
                });
            return Content(result);
        }
        public ActionResult GetMontosRequerimientosOferta(int Id)
        {
            var model = _ofertacomercial.ObtenerMontosRequerimientosOfertComercial(Id);

            var result = JsonConvert.SerializeObject(model);
            return Content(result);
        }
        [System.Web.Http.HttpPost]
        public ActionResult Crear([FromBody] OfertaComercial oferta)
        {

            var x = _ofertacomercial.CrearOfertaComercia(oferta);

            if (x > 0)
            {
                return Content("o");
            }
            else
            {
                return Content("e");
            }
        }

        public ActionResult EditarOfertaComercial([FromBody] OfertaComercial oferta)
        {

            var x = _ofertacomercial.EditarOfertaComercial(oferta);

            if (x > 0)
            {
                return Content("o");
            }
            else
            {
                return Content("e");
            }
        }

        [System.Web.Http.HttpPost]
        public ActionResult CrearNuevaVersion([FromBody] OfertaComercial oferta)
        {

            var x = _ofertacomercial.CrearNuevaVersion(oferta);

            if (x > 0)
            {
                return Content("" + x);
            }
            else
            {
                return Content("e");
            }
        }

        [System.Web.Http.HttpPost]
        public ActionResult ActualizarDatos([FromBody] OfertaComercialPresupuesto oferta)
        {

            try
            {
                var x = _ofertacomercialpresupuesto.ActualizarDatos(oferta);
                if (x == -1)
                {
                    return Content("ne");
                }

                if (x > 0)
                {
                    var monto_ofertado = _ofertacomercialpresupuesto.montoOfertado(oferta.OfertaComercialId);
                    /* var monto_aprobado = _ofertacomercial.monto_ordenes_servicio(oferta.OfertaComercialId); //Montos Ordenes de Servicio
                     bool actualizado = _ofertacomercial.ActualizarMontoOfertaComercial(oferta.OfertaComercialId, monto_ofertado, monto_aprobado.montototalos);*/

                    bool actualizado = _ofertacomercial.ActualizarMontoOfertaComercial(oferta.OfertaComercialId, monto_ofertado, 0);
                    return Content("o");
                }
                else
                {
                    return Content("e");
                }
            }
            catch (DbEntityValidationException ex)
            {
                ElmahExtension.LogToElmah(new Exception("error " + ex.Message));
                ElmahExtension.LogToElmah(ex);
                ElmahExtension.LogToElmah(new Exception("datos" + ex.EntityValidationErrors.ToString()));
                ElmahExtension.LogToElmah(new Exception("datos" + ex.EntityValidationErrors));
                ElmahExtension.LogToElmah(new Exception("datos" + ex.DbEntityValidationExceptionToString(";")));
                return Content("e");
            }

        }
        [System.Web.Http.HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> GetEnviar([FromBody] int Id, bool user_transmittal, string body = "", string asunto = "")
        {
            var urntransmital = _transmitalservice.GenerarWordTransmittal(Id);
            var send = await _ofertacomercial.Send_Files_OfertaComercial(Id, user_transmittal, asunto, body, urntransmital);
            return Content(send);

        }
        [System.Web.Http.HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> GetEnviarOferta([FromBody] int Id, bool user_transmittal, List<UserCorreos> list, string body = "")
        {
            var send = await _ofertacomercial.Send_Files_OfertaComercialList(Id, user_transmittal, list, body);
            return Content(send);

        }
        [System.Web.Http.HttpPost]
        public ActionResult GetAnular([FromBody] int Id)
        {

            var x = _ofertacomercial.CambiarEstadoOferta(Id, 5185);


            if (x)
            {

                return Content("o");
            }
            else
            {
                return Content("e");
            }
        }
        [System.Web.Http.HttpPost]
        public ActionResult GetCancelar([FromBody] int Id)
        {

            var x = _ofertacomercial.CambiarEstadoOferta(Id, 5186);


            if (x)
            {

                return Content("o");
            }
            else
            {
                return Content("e");
            }
        }

        [System.Web.Http.HttpPost]
        public ActionResult Eliminar([FromBody] int Id)
        {

            var x = _ofertacomercialpresupuesto.Eliminar(Id);

            if (x > 0)
            {
                return Content("o");
            }
            else
            {
                return Content("e");
            }
        }


        [System.Web.Http.HttpPost]
        public ActionResult CrearOfertaCPresupuesto([FromBody] OfertaComercialPresupuesto oferta)
        {

            var mensaje = _ofertacomercialpresupuesto.CrearOfertaComercialPresupuesto(oferta);

            return Content(mensaje);
        }

        [System.Web.Http.HttpPost]
        public ActionResult CrearOS([FromBody] OrdenServicio os)
        {

            var x = _ordenservicio.CrearOs(os);

            if (x > 0)
            {
                return Content("o");
            }
            else
            {
                return Content("e");
            }
        }

        public ActionResult GenerarPrespuesto(int OfertaId)
        {
            var oferta = _ofertacomercial.GetDetalles(OfertaId);
            int nivelmaximo = _ofertacomercialpresupuesto.mas_alto_multiple(oferta.Id);
            if (nivelmaximo == 0)
            {
                nivelmaximo++;
            }

            //var excel = _ofertacomercialpresupuesto.GenerarExcelCabecera(OfertaId,nivelmaximo);

            if (oferta.Contrato.Formato == FormatoContrato.Contrato_2019)
            {
                var excel = _ofertacomercialpresupuesto.SecondFormatPropuestaEconomica(OfertaId, nivelmaximo);

                string excelName = oferta.codigo + "_" + oferta.version + " Anexo Propuesta Economica";
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
            else
            {
                var excel = _ofertacomercialpresupuesto.GenerarPropuestaEconomica(OfertaId, nivelmaximo);

                string excelName = oferta.codigo + "_" + oferta.version + " Anexo Propuesta Economica";
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

        public ActionResult ExportarCronograma(int id) //Pasar pametros oferta 
        {


            ExcelPackage excel = _ofertacomercialpresupuesto.GenerarExcelCargaFechas(id);

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

        public ActionResult ListarPorRequerimiento(int id) //RequerimientoId
        {
            var presupuestos = _presupuestoService.ListarPresupuestosDefinitivosAprobados(id);
            var result = JsonConvert.SerializeObject(presupuestos);
            return Content(result);
        }

        public ActionResult GetMontos(int Id) //Id Oferta COmercial , monto aprobado, monto ofertado
        {
            /* */

            var ofertacomercial = _ofertacomercial.GetDetalles(Id);

            var monto_ofertado = _ofertacomercialpresupuesto.montoOfertado(Id);
            var monto_aprobado = _ofertacomercial.monto_ordenes_servicio(Id); //Montos Ordenes de Servicio
            bool actualizado = _ofertacomercial.ActualizarMontoOfertaComercial(Id, monto_ofertado, monto_aprobado.montototalos);

            MontosOfertasComerciales a = new MontosOfertasComerciales()
            {
                monto_ofertado = monto_ofertado,
                monto_aprobado = monto_aprobado.montototalos,
                monto_pendiente_aprobacion = monto_ofertado - monto_aprobado.montototalos,
                monto_ingenieria = monto_aprobado.ingenieria,
                monto_construccion = monto_aprobado.construccion,
                monto_suminitros = monto_aprobado.suminitros,
                monto_subcontratos = monto_aprobado.subcontratos,
                monto_total_os = monto_aprobado.montototalos
            };

            var result = JsonConvert.SerializeObject(a,
                Newtonsoft.Json.Formatting.None,
                new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    NullValueHandling = NullValueHandling.Ignore
                });
            return Content(result);
        }

        public ActionResult GetDetallesOrdenes(int id) //Id Orden Servicio
        {
            var lista = _detalleordenservicio.listar(id);
            var result = JsonConvert.SerializeObject(lista);
            return Content(result);
        }
        public ActionResult GetContratos()
        {
            var lista = _ofertacomercial.ObtenerContratos();
            var result = JsonConvert.SerializeObject(lista);
            return Content(result);
        }

        public ActionResult GetTransmital(int id) //Id Ofertacial
        {
            var objecto = _transmitalservice.IdOfertaComercialTransmital(id);
            if (objecto.Id > 0)
            {
                var result = JsonConvert.SerializeObject(objecto,
                      Newtonsoft.Json.Formatting.None,
                      new JsonSerializerSettings
                      {
                          ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                          NullValueHandling = NullValueHandling.Ignore
                      });
                return Content(result);
            }
            else
            {
                return Content("0");
            }

        }
        public async System.Threading.Tasks.Task<ActionResult> GetItem(int id) //Id Orden Servicio
        {
            var lista = await _grupoitemservice.GetAll();
            var result = JsonConvert.SerializeObject(lista);
            return Content(result);
        }
        [System.Web.Mvc.HttpPost]
        public ActionResult SubirArchivo([FromBody]HttpPostedFileBase UploadedFile, int Id = 0) //Ofertaid
        {
            var resultado = _ofertacomercial.GuardarArchivo(UploadedFile, Id);
            if (resultado)
            {


                return Content("OK");
            }
            else
            {
                return Content("E");
            }
        }

        //transmital
        public ActionResult ListarTransmital() //Listado de Transmitals
        {
            var listatransmital = _ofertacomercial.ListarTransmitals();
            var result = JsonConvert.SerializeObject(listatransmital);
            return Content(result);
        }
        public ActionResult ListarTransmitalPorContrato(int id) //Listado de Transmitals
        {
            var listatransmital = _ofertacomercial.ListarTransmitalsPorContrato(id);
            var result = JsonConvert.SerializeObject(listatransmital);
            return Content(result);
        }

        public ActionResult ListarPresupuestosLiberados() //RequerimientoId
        {
            var presupuestos = _presupuestoService.ListaPresupuestosLiberados();
            var result = JsonConvert.SerializeObject(presupuestos,
                Newtonsoft.Json.Formatting.None,
                new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    NullValueHandling = NullValueHandling.Ignore

                });
            return Content(result);
        }


        public ActionResult ListarWordOfertaComercial(int id) // Exporta Word OfertaComercial
        {

            var word = _ofertacomercial.GenerarWordOfertaComercial(id);
            string filepath = word;
            byte[] filedata = System.IO.File.ReadAllBytes(filepath);
            string contentType = MimeMapping.GetMimeMapping(filepath);
            string name = System.IO.Path.GetFileName(filepath);

            var cd = new System.Net.Mime.ContentDisposition
            {
                FileName = name,
                Inline = true,
            };

            Response.AppendHeader("Content-Disposition", cd.ToString());

            return File(filedata, contentType);

        }
        public async Task<ActionResult> Descargar(int id)
        {
            var entity = await ArchivoService.Get(new EntityDto<int>(id));

            if (entity == null)
            {
                var msg = string.Format("El Archivo con identificacion {0} no existe",
                    id);

                return HttpNotFound(msg);
            }

            return File(entity.hash, entity.tipo_contenido, entity.nombre);
        }

        public ActionResult GetOrdenServicio(int Id)
        {
            var os = _ordenservicio.Detalles(Id);

            var result = JsonConvert.SerializeObject(os,
                Newtonsoft.Json.Formatting.None,
                new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    NullValueHandling = NullValueHandling.Ignore
                });
            return Content(result);
        }

        /* DEBUG OS MANTEN*/
        public ActionResult GetCreate(OrdenServicio o, HttpPostedFileBase UploadedFile = null)
        {
            if (UploadedFile != null)
            {
                var ArchivoId = ArchivoService.InsertArchivo(UploadedFile);
                if (ArchivoId > 0)
                {
                    o.ArchivoId = ArchivoId;
                }
            }
            var result = _ordenservicio.InsertOrden(o);
            return Content(result);


        }
        public ActionResult GetEdit(OrdenServicio o, HttpPostedFileBase UploadedFile = null)
        {
            if (UploadedFile != null)
            {
                var ArchivoId = ArchivoService.InsertArchivo(UploadedFile);
                if (ArchivoId > 0)
                {
                    o.ArchivoId = ArchivoId;
                }
            }
            var result = _ordenservicio.EditOrden(o);
            return Content(result);

        }
        public ActionResult GetDelete(int Id)
        {
            var result = _ordenservicio.DeleteOrden(Id);
            return Content(result);
        }
        public ActionResult GetTieneTransmital(int Id)
        {
            var result = _transmitalservice.tieneTransmital(Id);
            return Content(result ? "SI" : "NO");
        }
        public ActionResult GetCodigoTransmital(int Id)
        {
            var result = _transmitalservice.nombresTransmital(Id);
            return Content(result);
        }
        public ActionResult GetMailto(int Id)
        {
            var result = _ofertacomercial.hrefoutlook(Id);
            return Content(result);
        }

        public ActionResult GetMailtoOrdenProceder(int Id)
        {
            var result = _ofertacomercial.hrefoutlookOrdenProceder(Id);
            return Content(result);
        }
        public ActionResult GetOSbyOfertaComercial(int Id)
        {
            var lsita = _ordenservicio.ListarOsByOferta(Id);
            var result = JsonConvert.SerializeObject(lsita,
                Newtonsoft.Json.Formatting.None,
                new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    NullValueHandling = NullValueHandling.Ignore

                });
            return Content(result);
        }


        public async System.Threading.Tasks.Task<ActionResult> GetEnviarContrato()
        {
            var send = await _ofertacomercial.SendMailAdministracionContratosAsync();
            return Content(send);

        }
        
        /*2020*/
        public ActionResult GetInformacionOferta(int Id)
        {
            try
            {
                var data = _ofertacomercial.ObtenerDataOferta(Id);

                var result = JsonConvert.SerializeObject(data,
                  Newtonsoft.Json.Formatting.None,
                  new JsonSerializerSettings
                  {
                      ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                      NullValueHandling = NullValueHandling.Ignore

                  });
                return Content(result);
            }
            catch (Exception ex)
            {
                var result = ManejadorExcepciones.HandleException(ex);
                ModelState.AddModelError("", result.Message);
            }
            return new JsonResult
            {
                Data = new { success = false, errors = ModelState.ToSerializedDictionary() }
            };
        }
        [System.Web.Http.HttpPost]
        public ActionResult ActualizarMontoOS(int Id, decimal monto_aprobado)
        {

            var x = _ofertacomercial.ActualizarMontoAprobadoOferta(Id, monto_aprobado);
            return Content(x);
        }

        [System.Web.Http.HttpPost]
        public ActionResult ActualizarOSMigradoActual(int Id, decimal monto_so_aprobado_migracion_actual)
        {

            var x = _ofertacomercial.Actualizarmonto_so_aprobado_migracion_actual(Id, monto_so_aprobado_migracion_actual);
            return Content(x);
        }

        [System.Web.Http.HttpPost]
        public ActionResult ActualizarOSMigradoAnterior(int Id, decimal monto_so_aprobado_migracion_anterior)
        {

            var x = _ofertacomercial.Actualizarmonto_so_aprobado_migracion_anterior(Id, monto_so_aprobado_migracion_anterior);
            return Content(x);
        }

        [System.Web.Http.HttpPost]
        public ActionResult ActualizarOfertaMigrado(int Id, decimal monto_ofertado_migracion_actual)
        {

            var x = _ofertacomercial.Actualizarmonto_ofertado_migracion_actual(Id, monto_ofertado_migracion_actual);
            return Content(x);
        }





        [System.Web.Http.HttpPost]
        public ActionResult CreateArchivo(int Id,HttpPostedFileBase UploadedFile)
        {
            var dataid = _ofertacomercial.GuardarArchivoOrden(Id,UploadedFile);
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

            int avanceid = _ofertacomercial.EliminarArchivoOrdenProceder(id.Value);
            return Content("OK");
        }
        public ActionResult UploadFileList(int id) //lISTA DE COLABORADORES
        {
            var list = _ofertacomercial.ListaArchivosOrden(id);
            var result = JsonConvert.SerializeObject(list,
          Newtonsoft.Json.Formatting.None,

          new JsonSerializerSettings
          {
              ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
              NullValueHandling = NullValueHandling.Ignore,


          });
            return Content(result);
        }

        public ActionResult DescargarArchivoOrden(int Id)
        {
            var entity = _ofertacomercial.DetalleArchivo(Id);
            return File(entity.hash, entity.tipo_contenido, entity.nombre);
        }

    }
}


