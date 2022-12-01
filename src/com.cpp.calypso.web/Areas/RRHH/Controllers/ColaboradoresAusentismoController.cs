using Abp.Application.Services.Dto;
using com.cpp.calypso.framework;
using com.cpp.calypso.proyecto.aplicacion.Dto;
using com.cpp.calypso.proyecto.aplicacion.Interfaces;
using com.cpp.calypso.proyecto.dominio;
using com.cpp.calypso.seguridad.aplicacion;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Threading.Tasks;
using JsonResult = com.cpp.calypso.framework.JsonResult;


namespace com.cpp.calypso.web.Areas.RRHH.Controllers
{
    public class ColaboradoresAusentismoController : BaseController
    {
        private readonly IColaboradoresAusentismoAsyncBaseCrudAppService _colaboradoresAusentismoService;
        private readonly IColaboradoresAusentismoReintegrosAsyncBaseCrudAppService _colaboradoresAusentismoReintegrosService;
        private readonly IColaboradoresAsyncBaseCrudAppService _colaboradoresService;
        private readonly IColaboradoresAusentismoRequisitosAsyncBaseCrudAppService _colaboradoresAusentismoRequisitosService;
        private readonly IUsuarioService _usuarioService;
        public IArchivoAsyncBaseCrudAppService _ArchivoService;
        public ICatalogoAsyncBaseCrudAppService _catalogoservice;

        public ColaboradoresAusentismoController(
            IHandlerExcepciones manejadorExcepciones,
            IColaboradoresAusentismoAsyncBaseCrudAppService colaboradoresAusentismoService,
            IColaboradoresAusentismoReintegrosAsyncBaseCrudAppService colaboradoresAusentismoReintegrosService,
            IColaboradoresAsyncBaseCrudAppService colaboradoresService,
            IColaboradoresAusentismoRequisitosAsyncBaseCrudAppService colaboradoresAusentismoRequisitosService,
            IUsuarioService usuarioService,
             IArchivoAsyncBaseCrudAppService ArchivoService,
             ICatalogoAsyncBaseCrudAppService catalogoservice
            ) : base(manejadorExcepciones)
        {
            _colaboradoresAusentismoService = colaboradoresAusentismoService;
            _colaboradoresService = colaboradoresService;
            _colaboradoresAusentismoReintegrosService = colaboradoresAusentismoReintegrosService;
            _colaboradoresAusentismoRequisitosService = colaboradoresAusentismoRequisitosService;
            _usuarioService = usuarioService;
            _ArchivoService = ArchivoService;
            _catalogoservice = catalogoservice;
        }

        public ActionResult Index()
        {
            return View();
        }

        // POST: RRHH/ColaboradoresAusentismo/Create
        public ActionResult Create()
        {

            return View();
        }

        // POST: RRHH/ColaboradoresAusentismo/Edit/5
        public ActionResult Edit()
        {
            return View();
        }

        // GET: RRHH/ColaboradoresAusentismo/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // GET: RRHH/ColaboradoresAusentismo/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult GetListado()
        {
            //Obtiene los Catlogos del Tipo Identificacion
            List<ColaboradoresAusentismoDto> lista = _colaboradoresAusentismoService.GetAusentismos();//Revisar ID
            var result = JsonConvert.SerializeObject(lista);
            return Content(result);
        }


        [System.Web.Mvc.HttpPost]
        public ActionResult GetColaborador(int tipoIdentificacion, String nroIdentificacion)
        {
            //Obtiene los Catlogos del Tipo Identificacion
            Colaboradores colaborador = _colaboradoresService.GetColaboradorPorTipoIdentificacion(tipoIdentificacion, nroIdentificacion);

            if (colaborador != null)
            {
                var result = JsonConvert.SerializeObject(colaborador);
                return Content(result);
            }

            return Content("NO");
        }

        [System.Web.Mvc.HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> CrearAusentismo([FromBody] int idColaborador, int inTipoAusentismo, DateTime fecha_desde, DateTime fecha_hasta, int? idAusentismo, string observacion = "")
        {
            if (ModelState.IsValid)
            {



                var id = "";

                if (idAusentismo > 0)
                {
                    var ausentismo = await _colaboradoresAusentismoService.Get(new EntityDto<int>(idAusentismo.Value));

                    ausentismo.fecha_inicio = fecha_desde;
                    ausentismo.fecha_fin = fecha_hasta;
                    ausentismo.observacion = observacion;

                    await _colaboradoresAusentismoService.Update(ausentismo);
                    id = idAusentismo.ToString();
                }
                else
                {
                    bool existe = _colaboradoresAusentismoService.ValidarExisteAusentimo(inTipoAusentismo, idColaborador, fecha_desde, fecha_hasta, 0);

                    if (existe)
                    {
                        return Content("EXISTE");
                    }
                    else
                    {



                        ColaboradoresAusentismoDto colaboradoresAusentismo = new ColaboradoresAusentismoDto();
                        colaboradoresAusentismo.colaborador_id = idColaborador;
                        colaboradoresAusentismo.catalogo_tipo_ausentismo_id = inTipoAusentismo;
                        colaboradoresAusentismo.fecha_inicio = fecha_desde;
                        colaboradoresAusentismo.fecha_fin = fecha_hasta;
                        colaboradoresAusentismo.vigente = true;
                        colaboradoresAusentismo.estado = "ACTIVO";
                        colaboradoresAusentismo.observacion = observacion;

                        /* Gurdamos el registro */
                        id = _colaboradoresAusentismoService.CrearAusentismoAsync(colaboradoresAusentismo);


                        var tieneausentismo = _colaboradoresAusentismoService.ActualizarAusentismoColaborador(idColaborador);
                        /* var colaborador = await _colaboradoresService.Get(new EntityDto<int>(idColaborador));
                         colaborador.tiene_ausentismo = true;
                         await _colaboradoresService.Update(colaborador);*/


                    }

                }
                var result = JsonConvert.SerializeObject(id);
                return Content(result);
            }
            else
            {
                return Content("ERROR");
            }
        }

        public async System.Threading.Tasks.Task<ActionResult> CrearRequisitoAusentismo([FromBody] int idAusentismo, int idRequisito, bool cumple, HttpPostedFileBase[] UploadedFile, int? Id, int? archivo_id)
        {
            var id = "";
            if (Id > 0)
            {
                var requisito = await _colaboradoresAusentismoRequisitosService.Get(new EntityDto<int>(Id.Value));
                requisito.archivo_id = archivo_id;
                requisito.cumple = cumple;
                await _colaboradoresAusentismoRequisitosService.Update(requisito);
                id = await _colaboradoresAusentismoRequisitosService.EditarAusentismoRequisitoAsync(requisito, UploadedFile);

            }
            else
            {
                ColaboradoresAusentismoRequisitosDto colaboradoresAusentismoRequisito = new ColaboradoresAusentismoRequisitosDto();
                colaboradoresAusentismoRequisito.colaborador_ausentismo_id = idAusentismo;
                colaboradoresAusentismoRequisito.requisito_id = idRequisito;
                colaboradoresAusentismoRequisito.cumple = cumple == true ? true : false;

                /* Gurdamos el registro */
                id = await _colaboradoresAusentismoRequisitosService.CrearAusentismoRequisitoAsync(colaboradoresAusentismoRequisito, UploadedFile);

            }


            var result = JsonConvert.SerializeObject(id);
            return Content(result);
        }

        public async System.Threading.Tasks.Task<ActionResult> CrearRequisitoAusentismoNoFile([FromBody] int idAusentismo, int idRequisito, bool cumple, int? Id)
        {
            if (Id > 0)
            {
                var requisito = await _colaboradoresAusentismoRequisitosService.Get(new EntityDto<int>(Id.Value));
                requisito.cumple = cumple == true ? true : false;
                await _colaboradoresAusentismoRequisitosService.Update(requisito);
            }
            else
            {
                ColaboradoresAusentismoRequisitosDto colaboradoresAusentismoRequisito = new ColaboradoresAusentismoRequisitosDto();
                colaboradoresAusentismoRequisito.colaborador_ausentismo_id = idAusentismo;
                colaboradoresAusentismoRequisito.requisito_id = idRequisito;
                colaboradoresAusentismoRequisito.cumple = cumple == true ? true : false;

                /* Gurdamos el registro */
                await _colaboradoresAusentismoRequisitosService.Create(colaboradoresAusentismoRequisito);
            }



            return Content("OK");
        }


        [System.Web.Mvc.HttpPost]
        public ActionResult GetAusentismo(int Id)
        {
            //Obtiene los Catlogos del Tipo Identificacion
            ColaboradoresAusentismo ausentismo = _colaboradoresAusentismoService.GetAusentismo(Id);

            if (ausentismo != null)
            {
                var result = JsonConvert.SerializeObject(ausentismo);
                return Content(result);
            }

            return Content("NO");
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult GetAusentismoColaborador(int Id)
        {
            //Obtiene los Catlogos del Tipo Identificacion
            var ausentismo = _colaboradoresAusentismoService.GetAusentismosColaborador(Id);

            if (ausentismo != null)
            {
                var result = JsonConvert.SerializeObject(ausentismo);
                return Content(result);
            }

            return Content("NO");
        }

        [System.Web.Mvc.HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> CrearReintegro([FromBody] int idAusentismo, DateTime fecha_reintegro, string motivo, HttpPostedFileBase[] UploadedFile)
        {
            //Obtiene los Catlogos del Tipo Identificacion
            ColaboradoresAusentismo ausentismo = _colaboradoresAusentismoService.GetAusentismo(idAusentismo);

            if (ausentismo != null && ausentismo.estado == "ACTIVO")
            {
                /* Actualizamos el estado del ausentismo */
                ausentismo.estado = "FIN ANTICIPADO";
                //ausentismo.vigente = false;
                _colaboradoresAusentismoService.ActualizarAusentismoAsync(ausentismo);

                var colaborador = await _colaboradoresService.Get(new EntityDto<int>(ausentismo.colaborador_id));
                colaborador.tiene_ausentismo = false;
                await _colaboradoresService.Update(colaborador);

                ColaboradoresAusentismoReintegrosDto colaboradoresReintegro = new ColaboradoresAusentismoReintegrosDto();
                colaboradoresReintegro.colaborador_ausentismo_id = idAusentismo;
                colaboradoresReintegro.motivo_reintegro = motivo;
                colaboradoresReintegro.fecha_reintegro = fecha_reintegro;
                colaboradoresReintegro.vigente = true;

                if (UploadedFile == null)
                {
                    await _colaboradoresAusentismoReintegrosService.InsertOrUpdateAsync(colaboradoresReintegro);
                    return Content("OK");
                }
                else
                {

                    var resultado = _colaboradoresAusentismoReintegrosService.CrearReintegrosAsync(colaboradoresReintegro, UploadedFile);
                    return Content("OK");
                }


            }
            return Content("NO");
        }

        public async System.Threading.Tasks.Task<ActionResult> GetReporteAusentismosApi(ColaboradorReporteDto colaborador)
        {
            ExcelPackage excel = _colaboradoresAusentismoService.reporteInformacionGeneral(colaborador);

            if (excel != null)
            {
                var usuario = "";
                var idUser = User.Identity.GetUserId();
                var usuarioencontrado = await _usuarioService.Get(new EntityDto<int>(int.Parse(idUser)));
                if (usuarioencontrado != null && usuarioencontrado.Id > 0)
                {
                    usuario = usuarioencontrado.Nombres + usuarioencontrado.Apellidos + "_";
                }

                string excelName = "ReporteAusentismos_" + usuario + DateTime.Now.ToString("dd_MM_yyyy_hh_mm");
                using (var memoryStream = new MemoryStream())
                {
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment; filename=" + excelName + ".xlsx");
                    excel.SaveAs(memoryStream);
                    memoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                    return Content("OK");
                }
            }
            return Content("ERROR");
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
        public ActionResult GetChangeArchivo(ColaboradoresAusentismoRequisitosDto dto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var archivo = Request.GenerateFileFromRequest("uploadFile");
                    var Uploadfile = Request.Files["uploadFile"];
                    _colaboradoresAusentismoService.SubirPdf(dto.Id, archivo);
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

                return new JsonResult
                {
                    Data = new { success = false, errores = result.Message }
                };
            }

            return new JsonResult
            {
                Data = new { success = false, errores = "ERROR" }
            };

        }



        [System.Web.Mvc.HttpPost]
        public ActionResult GetEditAusentimo(ColaboradoresAusentismo entity)
        {
            if (ModelState.IsValid)
            {
                bool existe = _colaboradoresAusentismoService.ValidarExisteAusentimo(entity.catalogo_tipo_ausentismo_id,
                    entity.colaborador_id, entity.fecha_inicio.Value, entity.fecha_fin.Value, entity.Id);

                if (existe)
                {
                    return Content("EXISTE");
                }
                else
                {
                    var update = _colaboradoresAusentismoService.EditarAusentismo(entity);

                    return Content(update > 0 ? "OK" : "ERROR");
                }
            }
            else
            {
                return Content("ERROR");
            }
        }

        public ActionResult GetByCodeApi(string code)
        {
            var result = _catalogoservice.APIObtenerCatalogos(code);
            return new JsonResult
            {
                Data = new { success = true, result },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };

        }

        public ActionResult GetEliminarAusentismo(int id)
        {
            var result = _colaboradoresAusentismoService.DeleteAusentimo(id);
            return Content(result ? "OK" : "ERROR");
          

        }
    }
}