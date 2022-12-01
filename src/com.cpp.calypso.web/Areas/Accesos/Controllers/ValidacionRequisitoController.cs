using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Abp.Application.Services.Dto;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.framework;
using com.cpp.calypso.proyecto.aplicacion;
using com.cpp.calypso.proyecto.aplicacion.Acceso.Dto;
using com.cpp.calypso.proyecto.aplicacion.Acceso.Interface;
using com.cpp.calypso.proyecto.aplicacion.Dto;
using com.cpp.calypso.proyecto.aplicacion.Interfaces;
using com.cpp.calypso.proyecto.aplicacion.Proveedor.Interfaces;
using com.cpp.calypso.proyecto.dominio;
using Newtonsoft.Json;
using OfficeOpenXml;
using JsonResult = com.cpp.calypso.framework.JsonResult;

namespace com.cpp.calypso.web.Areas.Accesos.Controllers
{
    public class ValidacionRequisitoController : BaseAccesoSpaController<Colaboradores, ColaboradoresDto, PagedAndFilteredResultRequestDto>
    {
        private readonly IConsultaPublicaAsyncBaseCrudAppService _service;
        private readonly IValidacionRequisitoAsyncBaseCrudAppService _validacionRequisitoService;
        private readonly IColaboradorRequisitoAsyncBaseCrudAppService _colaboradorRequisitoService;
        private readonly IArchivoAsyncBaseCrudAppService _archivoService;
        private readonly IContactoEmergenciaAsyncBaseCrudAppService _contactoEmergenciaService;
        private readonly ICatalogoAsyncBaseCrudAppService _catalogoService;
        private readonly IReservaHotelAsyncBaseCrudAppService _reservaservice;
        private readonly IColaboradoresAsyncBaseCrudAppService _colaboradoresService;

        public ValidacionRequisitoController(
            IHandlerExcepciones manejadorExcepciones, 
            IViewService viewService,
            IValidacionRequisitoAsyncBaseCrudAppService validacionRequisitoService,
            IColaboradorRequisitoAsyncBaseCrudAppService colaboradorRequisitoService,
                IContactoEmergenciaAsyncBaseCrudAppService contactoEmergenciaService,
                IConsultaPublicaAsyncBaseCrudAppService service,
            IArchivoAsyncBaseCrudAppService archivoService,
            IReservaHotelAsyncBaseCrudAppService reservaservice,
             ICatalogoAsyncBaseCrudAppService catalogoService,
            IColaboradoresAsyncBaseCrudAppService colaboradoresService
            ) : base(manejadorExcepciones, viewService)
        {
            _validacionRequisitoService = validacionRequisitoService;
            _colaboradorRequisitoService = colaboradorRequisitoService;
            _archivoService = archivoService;
            _contactoEmergenciaService = contactoEmergenciaService;
            _service = service;
            _reservaservice = reservaservice;
            _catalogoService = catalogoService;
            _colaboradoresService = colaboradoresService;
        }

        public ActionResult BuscarColaborador(string source)
        {
            var model = new FormReactModelView();
            model.Id = "buscar_colaborador_container";
            model.ReactComponent = "~/Scripts/build/buscar_colaborador.js";

            if (!string.IsNullOrEmpty(Title))
            {
                model.Title = Title;
            }
            ViewBag.ruta = new string[] { "Inicio", "Buscar Colaborador" };
            return View(model);
        }

        public ActionResult Index(int colaboradorId)
        {
            var model = new FormReactModelView();
            model.Id = "validacion_requisitos";
            model.ReactComponent = "~/Scripts/build/validacion_requisitos_container.js";
            if (!string.IsNullOrEmpty(Title))
            {
                model.Title = Title;
            }
            ViewBag.ruta = new string[] { "Inicio", "Validacion de Requisitos" };
            return View(model);
        }

        public ActionResult IndexReporte()
        {
          
            ViewBag.ruta = new string[] { "Inicio", "Reportes" };
            return View();
        }

        #region Api

        [HttpPost]
        public JsonResult ObtenerRequisitos(InputRequisitosDto input)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var dtos = _validacionRequisitoService.ObtenerRequisitos(input);
                    return new JsonResult
                    {
                        Data = new { success = true, result = dtos }
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


        [HttpPost]
        public async Task<ActionResult> UpdateApi(CreateColaboradorRequisitoDto input, FormCollection formCollection)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (input.fecha_caducidad.HasValue && input.fecha_emision.HasValue)
                    {
                        var fechasAprobadas = _validacionRequisitoService.FechasValidas(input.RequisitosId,
                            input.fecha_emision.Value, input.fecha_caducidad.Value);
                        if (fechasAprobadas != "OK")
                        {
                            return new JsonResult
                            {
                                Data = new { success = true, aproved = false, errors = fechasAprobadas }
                            };
                        }
                    }
                    var archivo = Request.GenerateFileFromRequest("uploadFile");
                    if (archivo != null)
                    {
                        var archivoDto = await _archivoService.Create(archivo);
                        if (input.ArchivoId.HasValue)
                        {
                            await _archivoService.Delete(new EntityDto<int>(input.ArchivoId.Value));
                        }

                        input.ArchivoId = archivoDto.Id;
                    }

                    var resultEntity = _validacionRequisitoService.UpdateApi(input);
                    var result = new { id = resultEntity };
                    return new JsonResult
                    {
                        Data = new { success = true, aproved = true, result }
                    };
                }
            }
            catch (Exception ex)
            {
                var result = ManejadorExcepciones.HandleException(ex);
                ModelState.AddModelError("", result.Message);
                ElmahExtension.LogToElmah(new Exception("Error: " + result.Message+""+ModelState.ToList()));
            }
            return new JsonResult
            {
                Data = new { success = false, errors = ModelState.ToSerializedDictionary() }
            };
        }

        #endregion

        #region ES: Reporte Requisitos

        public ActionResult ObtenerCumplimientoIndividual( InputRequisitosReporteDto input)
        {

            ExcelPackage excel = _validacionRequisitoService.ExcelCumplimientoIndividual(input);
            

            string excelName = "Cumplimiento_" +input.Identificacion+"_"+input.NombreColaborador+DateTime.Now.ToShortDateString();
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

        public ActionResult ObtenerListaCumplimientos(InputRequisitosReporteDto input)
        {

            ExcelPackage excel = _validacionRequisitoService.ExcelListaCumplimiento(input);


            string excelName = "Lista Cumplimientos" + DateTime.Now.ToShortDateString();
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
        public ActionResult ObtenerTiposRequisitos()//Por el momento solo de accesos
        {
            var tipo_requisitos = _validacionRequisitoService.ListaRequisitosReporte(7325);//7325 no hace nada aun hace referencia al codigo del catalogo Accion acceso.

            var result = JsonConvert.SerializeObject(tipo_requisitos,
                Newtonsoft.Json.Formatting.None,
                new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    NullValueHandling = NullValueHandling.Ignore

                });
            return Content(result);
        }

        public ActionResult ObtenerColaborador(string identificacion = "", string nombres = "")
        {
           
            var colaborador = _validacionRequisitoService.BuscarPorIdentificacionNombre(identificacion, nombres);
            var result = JsonConvert.SerializeObject(colaborador,
              Newtonsoft.Json.Formatting.None,
              new JsonSerializerSettings
              {
                  ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                  NullValueHandling = NullValueHandling.Ignore

              });
            return Content(result);
        }


        public ActionResult ObtenerListaRequisitosporColaborador(InputRequisitosReporteDto input)
        {
            var lista = _validacionRequisitoService.ObtenerRequisitosAccesoColaborador(input);
            var result = JsonConvert.SerializeObject(lista,
                Newtonsoft.Json.Formatting.None,
                new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    NullValueHandling = NullValueHandling.Ignore

                });
            return Content(result);
        }

        public ActionResult ObtenerListaRequisitosAccesos(InputRequisitosReporteDto input)
        {
            var lista = _validacionRequisitoService.ObtenerRequisitosAcceso(input);
            var result = JsonConvert.SerializeObject(lista,
                Newtonsoft.Json.Formatting.None,
                new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    NullValueHandling = NullValueHandling.Ignore

                });
            return Content(result);
        }
        #endregion


        public ActionResult Search(string identificacion = "", string nombres = "") //BUSCAR COLABORADOR
        {
           
            var colaboradores = _reservaservice.BuscarPorIdentificacionNombre(identificacion, nombres);
            return WrapperResponseGetApi(ModelState, () => colaboradores);
        }

        public ActionResult FilterCatalogo(string code)
        {
            var result = _catalogoService.APIObtenerCatalogos(code);
            return new JsonResult
            {
                Data = new { success = true, result },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };

        }
        public ActionResult DetallesApi(int id)
        {
            var pagedResultDto = _colaboradoresService.Detalles(id);
            return WrapperResponseGetApi(ModelState, () => pagedResultDto);
        }

        public async Task<ActionResult> Descargar(int id)
        {
            var entity = await _archivoService.Get(new EntityDto<int>(id));

            if (entity == null)
            {
                var msg = string.Format("El Archivo con identificacion {0} no existe",
                    id);

                return HttpNotFound(msg);
            }

            return File(entity.hash, entity.tipo_contenido, entity.nombre);
        }


    }
}