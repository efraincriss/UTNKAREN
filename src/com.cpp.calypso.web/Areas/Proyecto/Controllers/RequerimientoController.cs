using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
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

namespace com.cpp.calypso.web.Areas.Proyecto.Controllers
{
    
    public class RequerimientoController : BaseController
    {
        private readonly IRequerimientoAsyncBaseCrudAppService _requerimieService;
        private readonly IProyectoAsyncBaseCrudAppService _proyectoService;
        private readonly IOrdenServicioAsyncBaseCrudAppService _ordenServicioService;
        private readonly ISecuencialAsyncBaseCrudAppService _secuencialsService;
        private readonly IOfertaAsyncBaseCrudAppService _ofertaService;
        private readonly IArchivoAsyncBaseCrudAppService _archivoService;
        private readonly ICatalogoAsyncBaseCrudAppService _catalogoService;

        public RequerimientoController(
            IHandlerExcepciones manejadorExcepciones,
            IRequerimientoAsyncBaseCrudAppService requerimieService,
            IProyectoAsyncBaseCrudAppService proyectoService,
            IOrdenServicioAsyncBaseCrudAppService ordenServicioService,
            ISecuencialAsyncBaseCrudAppService secuencialsService,
            IOfertaAsyncBaseCrudAppService ofertaService,
        IArchivoAsyncBaseCrudAppService archivoService,
        ICatalogoAsyncBaseCrudAppService catalogoService
            ) : base(manejadorExcepciones)
        {
            _requerimieService = requerimieService;
            _proyectoService = proyectoService;
            _ordenServicioService = ordenServicioService;
            _secuencialsService = secuencialsService;
            _archivoService = archivoService;
            _ofertaService = ofertaService;
            _catalogoService = catalogoService;
        }

        public ActionResult GetReqPorProyecto(int? id)
        {
            var req = _requerimieService.ObtenerRequerimientosDeProyecto(id.Value);
            var result = JsonConvert.SerializeObject(req);
            return Content(result);
        }

        public async Task<ActionResult> Create(int? id)
        {
            var proyecto = await _proyectoService.Get(new EntityDto<int>(id.Value));

            var secuencial = _requerimieService.ObtenerSecuencial(id.Value);


            var estados_oferta = _catalogoService.APIObtenerCatalogos("TESTATUSPROCESOS");
            if (id.HasValue)
            {
                RequerimientoDto requerimiento = new RequerimientoDto()
                {
                    requiere_cronograma = true,
                    estado = true,
                    fecha_recepcion = DateTime.Now,
                    fecha_maxima_presupuesto = (DateTime.Today.AddDays(6)),
                    fecha_limite_cronograma= (DateTime.Today.AddDays(7)),
                    fecha_maxima_oferta = (DateTime.Today.AddDays(8)),
                    codigo = proyecto.codigo //+ "-"+ secuencial
                };
                requerimiento.ProyectoId = id.Value;
                ViewBag.EstadosRequerimiento = estados_oferta;
                return View(requerimiento);
            }

            return RedirectToAction("Index", "Proyecto");

        }

        [System.Web.Mvc.HttpPost]
        public async Task<ActionResult> Create (RequerimientoDto requerimiento)
        {
            var estados_oferta = _catalogoService.APIObtenerCatalogos("TESTATUSPROCESOS");
            if (ModelState.IsValid)
            {
                var proyecto = await _proyectoService.Get(new EntityDto<int>(requerimiento.ProyectoId));
                requerimiento.vigente = true;
                if (requerimiento.tipo_requerimiento == 0)
                {
                    requerimiento.codigo = proyecto.codigo;
                    var existePrincial = _requerimieService.ExistePrincipal(requerimiento.ProyectoId);
                    if (existePrincial)
                    {
                        ViewBag.Msg = "Ya existe un requerimiento principal";
     
                        ViewBag.EstadosRequerimiento = estados_oferta;
                        return View("Create", requerimiento);
                    }
                    
                }
                else
                {

                    //Por Revisar Activacion CAPEX Y OPEX
                   /* var existePrincial = _requerimieService.ExistePrincipal(requerimiento.ProyectoId);
                    if (!existePrincial) { 

                        ViewBag.Msg = "Deber existir un requerimiento principal, antes de generar requerimientos adicionales";
                 
                        ViewBag.EstadosRequerimiento = estados_oferta;
                        return View("Create", requerimiento);
                    }
                    */

                    var existeadicional = _requerimieService.ExisteAdicional(requerimiento.codigo);
                    if (existeadicional)
                    {
                        ViewBag.Msg = "Ya existe un requerimiento adicional con ese código";
 
                        ViewBag.EstadosRequerimiento = estados_oferta;
                        return View("Create", requerimiento);
                    }
                    //requerimiento.codigo = "ADC" + _requerimieService.ObtenerSecuencial(requerimiento.ProyectoId);
                    requerimiento.codigo = requerimiento.codigo;
                }

                requerimiento.monto_total = requerimiento.monto_construccion + requerimiento.monto_ingenieria +
                                            requerimiento.monto_procura;
                requerimiento.estado_presupuesto = 4177;
                var requerimientoDto = await _requerimieService.Create(requerimiento);
                _requerimieService.FormatoCorreoRequerimientoCreado(1,1);
                //var oferta = _requerimieService.CrearOfertaParaRequerimiento(requerimientoDto.Id, requerimientoDto.ProyectoId);
                return RedirectToAction("Details", "Proyecto", new { id = requerimiento.ProyectoId });
            }

            return View("Create", requerimiento);
        }

        public ActionResult Details (int? id, string flag = "", string message = "" )
        {
            if (id.HasValue)
            {
                var requerimiento = _requerimieService.GetDetalles(id.Value);
                if (flag == "NoHayPresupuestoDefinitivo" || message == "NO_EXISTE_PRESUPUESTO_DEFINITIVO")
                {
                    ViewBag.Msg = "No existe un presupuesto marcado como definitivo y aprobado.";
                }
                else if (message == "EXISTE_MAS_DE_UN_PRESUPUESTO_DEFINITIVO")
                {
                    ViewBag.Msg = "Existe dos o mas presupuestos marcados como definitivos.";
                }
                if (message == "CANTIDADES_ACTUALIZADAS")
                {
                    ViewBag.SuccessMessage = "Presupuesto cargado en RDO";
                }
                if (message == "NO_EXISTE_RDO_DEFINITIVO")
                {
                    ViewBag.Msg = "No Existe un Rdo Definito";
                }
                if (message == "BASERDOEXISTE")
                {
                    ViewBag.Msg = "Ya Existe una Base Rdo Definitiva";
                }
                if (message == "CREADO")
                {
                    ViewBag.SuccessMessage = "Presupuesto cargado en RDO";
                }
                else if (message == "NUEVA_VERSION_CREADA")
                {
                    ViewBag.SuccessMessage = "Nueva version creada exitosamente";
                }
                if (flag == "avances")
                {
                    ViewBag.Msg = "No se puede Cancelar Requerimiento Tiene Registrados Avances de Obra";
                }
                if (flag == "ok")
                {
                    ViewBag.SuccessMessage = "Requerimiento Cancelado";
                }
                if (flag == "oka")
                {
                    ViewBag.SuccessMessage = "Requerimiento Activado";
                }
                ViewBag.ruta = new string[] { "Inicio", "Proyecto", "Requerimiento - " + requerimiento.codigo, "Detalles" };
                return View(requerimiento);

                
                
            }
            return RedirectToAction("Index", "Proyecto");
        }
        public ActionResult DetailsArchivos(int? id, string flag = "", string message = "")
        {
            if (id.HasValue)
            {
                var requerimiento = _requerimieService.GetDetalles(id.Value);
                var archivos = _requerimieService.ListaArchivos(requerimiento.Id);

                ViewBag.ruta = new string[] { "Inicio", "Proyecto", "Requerimiento - " + requerimiento.codigo, "Detalles- Archivos" };
                RequerimientoArchivos model = new RequerimientoArchivos()
                {
                    Requerimiento=requerimiento,
                    ListaArchivos=archivos
                };

                return View(model);



            }
            return RedirectToAction("Index", "Proyecto");
        }

        public ActionResult Edit (int? id)
        {
            if (id.HasValue)
            {
                var requerimiento = _requerimieService.GetDetalles(id.Value);

                var estados_oferta = _catalogoService.APIObtenerCatalogos("TESTATUSPROCESOS");
                ViewBag.EstadosRequerimiento = estados_oferta;
              
                ViewBag.Proyectos = _proyectoService.ListarCambiarProyectoRequerimiento();
                return View(requerimiento);
            }

            return RedirectToAction("Index", "Proyecto");
        }

        public async System.Threading.Tasks.Task<ActionResult> Update (RequerimientoDto requerimiento)
        {
            var estados_oferta = _catalogoService.APIObtenerCatalogos("TESTATUSPROCESOS");
            if (ModelState.IsValid)
            {
                var existeCodigo = _requerimieService.ComprobarExisteCodigo(requerimiento);
                if (existeCodigo)
                {
                requerimiento.MsgCodigo = "Ya existe un requerimiento con este código en el contrato";
                    ViewBag.EstadosRequerimiento = estados_oferta;
                    return View("Edit", requerimiento);
                }
                
                requerimiento.monto_total = requerimiento.monto_construccion + requerimiento.monto_ingenieria +
                                            requerimiento.monto_procura;
                try
                {
                    
                  await _requerimieService.Update(requerimiento);
                  _requerimieService.cambiarProyectoReferenciaPresupuesto(requerimiento.Id,requerimiento.ProyectoId);
                   // _requerimieService.ActualizarSolicitanteAsync(requerimiento);
                    return RedirectToAction("Details", "Proyecto", new { id = requerimiento.ProyectoId });

                }
                catch (Exception e)
                {
                    
                    throw;
                }
         

               
            }

            return View("Edit", requerimiento);
        }

        [System.Web.Mvc.HttpPost]
#pragma warning disable CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        public async System.Threading.Tasks.Task<ActionResult> Delete(int? id)
#pragma warning restore CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        {
            if (id.HasValue)
            {
                var req = _requerimieService.EliminarVigencia(id.Value);
                
                if (req.Id != 0)
                {
                    return RedirectToAction("Details", "Proyecto", new { id = req.ProyectoId });
                }
                return RedirectToAction("Details", "Proyecto", new { id = req.ProyectoId, flag = "errorDelete" });
            }
            return RedirectToAction("Index", "Proyecto");
        }

        public ActionResult Listar()
        {
            var requerimientos = _requerimieService.Listar();
            var result = JsonConvert.SerializeObject(requerimientos);
            return Content(result);
        }

        public ActionResult ListarporContrato([FromBody]int Id)
        {
            var requerimientos = _requerimieService.ListarporContrato(Id);
            var result = JsonConvert.SerializeObject(requerimientos);
            return Content(result);
        }

        public ActionResult DetailsApi(int id)
        {
            var requerimientos = _requerimieService.GetDetalles(id);
            var result = JsonConvert.SerializeObject(requerimientos);
            return Content(result);
        }


        //CANCELAR REQUERIMIENTO

        [System.Web.Mvc.HttpPost]
        public async Task<ActionResult> GetCancelarRequerimiento(int? id)
        {
            if (id.HasValue)
            {
                var re = _requerimieService.cambiar_estado_cancelado(id.Value);
                var req = await _requerimieService.Get(new EntityDto<int>(id.Value));

                if (re)
                {
                    return RedirectToAction("Details", "Requerimiento", new { id = req.Id,flag = "ok" });
                }
                else
                {
                    return RedirectToAction("Details", "Requerimiento", new { id = req.Id, flag = "avances" });
                }
            }
            return RedirectToAction("Index", "Proyecto");
        }

        [System.Web.Mvc.HttpPost]
        public async Task<ActionResult> GetActivarRequerimiento(int? id)
        {
            if (id.HasValue)
            {
                var re = _requerimieService.cambiar_estado_activado(id.Value);
                var req = await _requerimieService.Get(new EntityDto<int>(id.Value));

                if (re)
                {
                    return RedirectToAction("Details", "Requerimiento", new { id = req.Id, flag = "oka" });
                }
                else
                {
                    return RedirectToAction("Details", "Requerimiento", new { id = req.Id });
                }
            }
            return RedirectToAction("Index", "Proyecto");
        }



        public ActionResult CreateArchivo(int id) // AvanceObraId
        {
         
                ViewBag.RequerimientoId = id;
                return View();
            
        }


        [System.Web.Mvc.HttpPost]
        public async Task<ActionResult> CreateArchivo(int RequerimientoId, HttpPostedFileBase[] UploadedFile, int tipo=2)
        {
            var requerimiento = await _requerimieService.Get(new EntityDto<int>(RequerimientoId));

            if (ModelState.IsValid)
            {

                var resultado = _requerimieService.GuardarArchivo(RequerimientoId, UploadedFile,tipo);
                return RedirectToAction("DetailsArchivos", "Requerimiento", new { id = requerimiento.Id });
            }

            ViewBag.RequerimientoId = requerimiento.Id;
            return View("CreateArchivo");
        }
#pragma warning disable CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        public async Task<ActionResult> EditarArchivo(int? id) // AvanceObraId
#pragma warning restore CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        {
            if (id.HasValue)
            {
                var archivo = _requerimieService.getdetallesarchivo(id.Value);
                ViewBag.RequerimientoId = id;
                return View(archivo);
            }
            return RedirectToAction("Index", "Inicio", new { area = "" });
        }


        [System.Web.Mvc.HttpPost]
        public async Task<ActionResult> EditarArchivo(ArchivosRequerimiento a, HttpPostedFileBase UploadedFile, int tipo=2)
        {
            var avanceObra = await _requerimieService.Get(new EntityDto<int>(a.RequerimientoId));

            if (ModelState.IsValid)
            {

                var resultado = _requerimieService.EditarArchivo(a.Id, UploadedFile,tipo);
                return RedirectToAction("DetailsArchivos", "Requerimiento", new { id = avanceObra.Id });
            }

            var archivo = _requerimieService.getdetallesarchivo(a.Id);
            return View("EditarArchivo",archivo);
        }
        [System.Web.Http.HttpPost]
        public ActionResult DeleteArchivo(int? id) // ArchivoAvanceObraId
        {
            if (!id.HasValue)
            {
                return RedirectToAction("Index", "Inicio", new { area = "" });
            }

            int avanceid = _requerimieService.EliminarVigenciaArchivo(id.Value);
            return RedirectToAction("DetailsArchivos", "Requerimiento", new { id = avanceid });
        }


        [System.Web.Http.HttpPost]
        public ActionResult DeleteOferta(int? id) // ArchivoAvanceObraId
        {
            if (!id.HasValue)
            {
                return RedirectToAction("Index", "Inicio", new { area = "" });
            }
            var idrequerimiento = _ofertaService.EliminarOferta(id.Value);

            return RedirectToAction("Details", "Requerimiento", new { id = idrequerimiento });
        }

        public async System.Threading.Tasks.Task<ActionResult> descargararchivo(int id)
        {
            var a = await _archivoService.Get(new EntityDto<int>(id));
            return File(a.hash, a.tipo_contenido, a.nombre);
        }
        public async Task<ActionResult> ObtenerSecuencialAsync(int id,TipoRequerimiento tipo)
        {
            string result = "";
            var proyecto = await _proyectoService.Get(new EntityDto<int>(id));
            var secuencial = _requerimieService.ObtenerSecuencial(id);
            if (tipo == TipoRequerimiento.Principal)
            {
                result = proyecto.codigo;
            }
            else {
                result = "ADC" + secuencial;
            }

                     return Content(result);
        }

        [System.Web.Http.HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> GetEnviar(int Id, string asunto="",string body = "")
        {
            var send = await _requerimieService.Send_Files_Requerimiento(Id,asunto,body);
            return Content(send);

        }

        public ActionResult GetMailto(int Id)
        {
            var result = _requerimieService.hrefoutlook(Id);
            return Content(result);
        }

    }
}
