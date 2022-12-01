using Abp.Application.Services.Dto;
using Microsoft.AspNet.Identity;
using com.cpp.calypso.proyecto.aplicacion.Interfaces;
using com.cpp.calypso.seguridad.aplicacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using com.cpp.calypso.framework;
using AutoMapper;
using com.cpp.calypso.proyecto.aplicacion.Dto;
using com.cpp.calypso.proyecto.dominio.Constantes;
using Newtonsoft.Json;
using System.Web.Mvc;
using com.cpp.calypso.proyecto.dominio;
using System.Web.Http;
using JsonResult = com.cpp.calypso.framework.JsonResult;
using com.cpp.calypso.proyecto.dominio.Models;

namespace com.cpp.calypso.web.Areas.RRHH.Controllers
{
    public class ColaboradorBajaController : BaseController
    {
        private readonly IColaboradorBajaAsyncBaseCrudAppService _bajaService;
        private readonly ICatalogoAsyncBaseCrudAppService _catalogservice;
        private readonly IColaboradoresAsyncBaseCrudAppService _colaboradoresService;
        private readonly IUsuarioService _usuarioService;
        private readonly IArchivoAsyncBaseCrudAppService _ArchivoService;

        public ColaboradorBajaController(
            IHandlerExcepciones manejadorExcepciones,
            IColaboradorBajaAsyncBaseCrudAppService bajaService,
            IColaboradoresAsyncBaseCrudAppService colaboradoresService,
            ICatalogoAsyncBaseCrudAppService catalogservice,
            IArchivoAsyncBaseCrudAppService ArchivoService,
        IUsuarioService usuarioService
            ) : base(manejadorExcepciones)
        {
            _bajaService = bajaService;
            _usuarioService = usuarioService;
            _colaboradoresService = colaboradoresService;
            _catalogservice = catalogservice;
            _ArchivoService = ArchivoService;
        }
        // GET: RRHH/ColaboradorBaja
        public ActionResult Index()
        {
            return View();
        }

        [System.Web.Mvc.HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> CreatePagoApi([FromBody] int idBaja, DateTime fecha_pago_liquidacion, HttpPostedFileBase[] UploadedFile)
        {
            ColaboradorBajaDto c = await _bajaService.Get(new EntityDto<int>(idBaja));
            c.fecha_pago_liquidacion = fecha_pago_liquidacion;
            c.estado = BajaEstado.LIQUIDADO;

            if (UploadedFile == null)
            {
                await _bajaService.Update(c);
                return Content("OK");
            }
            else
            {
                var resultado = _bajaService.GuardarLiquidacionArchivoAsync(c.Id, UploadedFile);
                c.archivo_liquidacion_id = resultado;
                await _bajaService.Update(c);
                return Content("OK");
            }

        }

        [System.Web.Mvc.HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> CreateDesestimacionApi(ColaboradorBajaDto baja)
        {
            if (ModelState.IsValid)
            {
                ColaboradorBajaDto c = await _bajaService.Get(new EntityDto<int>(baja.Id));

                c.motivo_desestimacion = baja.motivo_desestimacion;
                c.estado = BajaEstado.DESESTIMADA;
                c.vigente = false;
                c.IsDeleted = true;

                await _bajaService.Update(c);

                ColaboradoresDto col = await _colaboradoresService.Get(new EntityDto<int>(baja.ColaboradoresId));

                col.estado = RRHHCodigos.ESTADO_ACTIVO;

                await _colaboradoresService.Update(col);

                return Content("OK");
            }
            return Content("NO");

        }

        [System.Web.Mvc.HttpPost]
        public async Task<ActionResult> GetBajaManualApi(int[] ids)
        {
            List<ColaboradorBajaTemp> bajas = new List<ColaboradorBajaTemp>();
            if (ids != null)
            {
                foreach (var e in ids)
                {
                    var c = _bajaService.GetBajasEnviarSapTemp(e);
                    bajas.Add(c);
                }


                if (bajas != null)
                {
                   
                        var excel = await _bajaService.GenerarExcelBajasTemp(bajas, true);

                        if (excel == "OK")
                        {
                            foreach (var e in bajas)
                            {
                            var x= _bajaService.UpdateColaboradorBaja(e.Id);
                               
                            }
                            return Content("OK");
                        }
                    
                 
                }
            }

            return Content("NO");
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult GetBajasEnviarSapApi()
        {
            var list = _bajaService.GetBajasGenerarArchivo(BajaEstado.ENVIAR_SAP);

            var result = JsonConvert.SerializeObject(list);
            return Content(result);
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult GetBajasEnviadoSapApi()
        {
            var list = _bajaService.GetBajasGenerarArchivo(BajaEstado.ENVIADO_SAP);

            var result = JsonConvert.SerializeObject(list);
            return Content(result);
        }

        [System.Web.Mvc.HttpPost]
        public async Task<ActionResult> EditEstadoBaja(int[] ids)
        {
            List<ColaboradorBajaDto> bajas = new List<ColaboradorBajaDto>();
            if (ids != null)
            {
                foreach (var e in ids)
                {
                    ColaboradorBajaDto c = await _bajaService.Get(new EntityDto<int>(e));
                    c.estado = proyecto.dominio.BajaEstado.POR_LIQUIDAR;
                    await _bajaService.Update(c);

                }
                return Content("OK");
            }

            return Content("NO");
        }

        public async Task<ActionResult> GetArchivoIESSApi()
        {
            var colaboradores = _bajaService.GetBajasArchivoIESS();
            if (colaboradores.Count > 0)
            {
                var excel = await _bajaService.GenerarArchivoIESS(colaboradores, false);
                if (excel == "OK")
                {
                    foreach (var e in colaboradores)
                    {
                        ColaboradorBajaDto c = await _bajaService.Get(new EntityDto<int>(e.Id));
                        c.enviado_archivo_iess = true;
                        c.fecha_envio_archivo_iess = DateTime.Now;
                        c.generacion_archivo_iess = "A";

                        await _bajaService.Update(c);
                    }

                    return Content("OK");
                }
            }

            return Content("No existen datos");
        }

        public async Task<ActionResult> GetArchivoIESSManualApi(int[] ids)
        {
            List<ColaboradorBajaDto> bajas = new List<ColaboradorBajaDto>();
            if (ids != null)
            {
                foreach (var e in ids)
                {
                    var c = _bajaService.GetBajasEnviarSap(e);
                    bajas.Add(c);
                }

            }

            if (bajas.Count > 0)
            {
                var excel = await _bajaService.GenerarArchivoIESS(bajas, false);
                if (excel == "OK")
                {
                    foreach (var e in bajas)
                    {
                        ColaboradorBajaDto c = await _bajaService.Get(new EntityDto<int>(e.Id));
                        c.enviado_archivo_iess = true;
                        c.fecha_envio_archivo_iess = DateTime.Now;
                        c.generacion_archivo_iess = "M";

                        await _bajaService.Update(c);
                    }

                    return Content("OK");
                }
            }

            return Content("No existen datos");
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult GetBajasArchivoIESS()
        {
            var list = _bajaService.GetBajasArchivoIESS();

            var result = JsonConvert.SerializeObject(list);
            return Content(result);
        }

        public async System.Threading.Tasks.Task<ActionResult> EditBaja(int Id, DateTime fecha_baja, string motivo_edicion, string detalle_baja, int catalogo_motivo_baja_id)
        {
            if (ModelState.IsValid)
            {
                ColaboradorBajaDto c = await _bajaService.Get(new EntityDto<int>(Id));

                c.motivo_edicion = motivo_edicion;
                c.fecha_baja = fecha_baja;
                c.catalogo_motivo_baja_id = catalogo_motivo_baja_id;
                c.detalle_baja = detalle_baja;

                await _bajaService.Update(c);

                return Content("OK");
            }
            return Content("NO");

        }

        public ActionResult GetByCodeApi(string code)
        {
            var result = _catalogservice.APIObtenerCatalogos(code);
            return new JsonResult
            {
                Data = new { success = true, result },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };

        }

        public async Task<ActionResult> GetArchivo(int id)
        {
            var entity = await _ArchivoService.Get(new EntityDto<int>(id));

            if (entity == null)
            {
                var msg = string.Format("El Archivo con identificacion {0} no existe",
                    id);

                return HttpNotFound(msg);
            }

            return File(entity.hash, entity.tipo_contenido, entity.nombre);
        }


        [System.Web.Mvc.HttpPost]
        public ActionResult GetSubirArchivo(ColaboradorBajaDto dto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var archivo = Request.GenerateFileFromRequest("uploadFile");
                    var Uploadfile = Request.Files["uploadFile"];
                    _bajaService.SubirPdf(dto.Id, archivo);
                    return new JsonResult
                    {
                        Data = new { success = true }
                    };

                }
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

        [System.Web.Mvc.HttpPost]
        public ActionResult GetSubirArchivoPago(ColaboradorBajaDto dto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var archivo = Request.GenerateFileFromRequest("uploadFile");
                    var Uploadfile = Request.Files["uploadFile"];
                    _bajaService.SubirPago(dto.Id, archivo);
                    return new JsonResult
                    {
                        Data = new { success = true }
                    };

                }
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

    }
}