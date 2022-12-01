using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.Mvc;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.framework;
using com.cpp.calypso.proyecto.aplicacion;
using com.cpp.calypso.proyecto.aplicacion.Acceso.Dto;
using com.cpp.calypso.proyecto.aplicacion.Acceso.Interface;
using com.cpp.calypso.proyecto.aplicacion.Interfaces;
using com.cpp.calypso.proyecto.dominio.Accesos;
using JsonResult = com.cpp.calypso.framework.JsonResult;
using com.cpp.calypso.proyecto.dominio.Constantes;
using Newtonsoft.Json;
using com.cpp.calypso.proyecto.aplicacion.Proveedor.Interfaces;
using com.cpp.calypso.proyecto.aplicacion.WebService;

namespace com.cpp.calypso.web.Areas.Accesos.Controllers
{


    public class ConsultaPublicaController : BaseAccesoSpaController<ConsultaPublica, ConsultaPublicaDto, PagedAndFilteredResultRequestDto>
    {
        private readonly IConsultaPublicaAsyncBaseCrudAppService _service;

        private readonly IProvinciaAsyncBaseCrudAppService _provinciaService;
        private readonly ICatalogoAsyncBaseCrudAppService _catalogoService;
        private readonly ICiudadAsyncBaseCrudAppService _ciudadService;
        private readonly IPaisAsyncBaseCrudAppService _paisService;

        //ACCIONES AGREGADAS
        private readonly IColaboradoresAsyncBaseCrudAppService _colaboradoresService;
        private readonly IReservaHotelAsyncBaseCrudAppService _reservaservice;
        //Web service
        private readonly IWebServiceAsyncBaseCrudAppService _webService;

        public ConsultaPublicaController(
            IHandlerExcepciones manejadorExcepciones,
            IViewService viewService,
            IConsultaPublicaAsyncBaseCrudAppService service,
            IProvinciaAsyncBaseCrudAppService provinciaService,
            ICatalogoAsyncBaseCrudAppService catalogoService,
            ICiudadAsyncBaseCrudAppService ciudadService,
            IPaisAsyncBaseCrudAppService paisService,
     

        IColaboradoresAsyncBaseCrudAppService colaboradoresService,
            IReservaHotelAsyncBaseCrudAppService reservaservice,
                   IWebServiceAsyncBaseCrudAppService webService
            ) : base(manejadorExcepciones, viewService)
        {
            _service = service;
            _provinciaService = provinciaService;
            _catalogoService = catalogoService;
            _ciudadService = ciudadService;
            _paisService = paisService;
            _colaboradoresService = colaboradoresService;
            _reservaservice = reservaservice;
            _webService = webService;
        }



        public ActionResult Index()
        {
            var model = new FormReactModelView();
            model.Id = "consulta_publica_listado";
            model.ReactComponent = "~/Scripts/build/create_consulta_publica.js";
            if (!string.IsNullOrEmpty(Title))
            {
                model.Title = Title;
            }
            ViewBag.ruta = new string[] { "Inicio", "Consulta Pública", "Gestion" };
            return View(model);
        }



        #region Api


        public ActionResult BuscarConsultaPublica(string identificacion = "", string nombres = "")
        {
            var colaboradores = _service.BuscarPorIdentificacionNombre(identificacion, nombres);
            return WrapperResponseGetApi(ModelState, () => colaboradores);
        }

        public ActionResult ObtenerPaises()
        {
            var paises = _paisService.GetPaises();
            return WrapperResponseGetApi(ModelState, () => paises);
        }

        //Pais Id
        public ActionResult ObtenerProvinciasPorPais(int id)
        {
            var provincias = _provinciaService.ObtenerProvinciaPorPais(id);
            return WrapperResponseGetApi(ModelState, () => provincias);
        }

        public ActionResult ObtenerTiposIdentificacion()
        {
            var tiposIdentificacion = _catalogoService.ListarCatalogosporcodigo(CatalogosCodigos.TIPO_IDENTIFICACION);
            return WrapperResponseGetApi(ModelState, () => tiposIdentificacion);
        }

        // Provincia Id
        public ActionResult ObtenerCiudadesPorProvincia(int id)
        {
            var ciudades = _ciudadService.ObtenerCantonPorProvincia(id);
            return WrapperResponseGetApi(ModelState, () => ciudades);
        }

        [HttpPost]
        public async Task<JsonResult> CreateApi(ConsultaPublicaDto entity)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var resultEntity = await _service.Create(entity);
                    var result = new { id = resultEntity.Id };
                    return new JsonResult
                    {
                        Data = new { success = true, result }
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
        public async Task<JsonResult> UpdateApi(ConsultaPublicaDto entity)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var resultEntity = await _service.Update(entity);
                    var result = new { id = resultEntity.Id };
                    return new JsonResult
                    {
                        Data = new { success = true, result }
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


        // ConsultaPublicaId
        public ActionResult DescargarPlantilla(int id)
        {

            var word = _service.GenerarWord(id);
            string path = (word);
            string name = Path.GetFileName(path);
            string ext = Path.GetExtension(path);

            var type = WordHelper.GetExtension(ext);

            Response.AppendHeader("content-disposition", "inline; filename=" + name);

            if (type != "")
                Response.ContentType = type;
            Response.WriteFile(path);

            Response.End();

            return Content("");
        }

        [HttpPost]
        public ActionResult SubirArchivoArchivo(ArchivoConsultaPublicaDto dto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var archivo = Request.GenerateFileFromRequest("uploadFile");
                    var Uploadfile = Request.Files["uploadFile"];
                    _service.SubirPdf(dto.Id, archivo);
                    _service.EnviarCorreoElectronico(dto.Id, "", "", true, Uploadfile, dto.listadistribucionids);
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


        public JsonResult ExisteCandidato(string identificacion)
        {
            var dto = _service.ExisteCandidato(identificacion);

            if (dto.Id > 0)
            {
                return new JsonResult
                {
                    Data = new { success = true, result = dto }
                };
            }
            return new JsonResult
            {
                Data = new { success = false }
            };
        }

        #endregion

        public ActionResult ObtenerListasDistribucion()
        {
            var listas = _service.ListaDistribucion(0);
            return WrapperResponseGetApi(ModelState, () => listas);
        }

        public JsonResult QueryApi(string query)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = _service.RealizarConsulta(query);

                    return new JsonResult
                    {
                        Data = new { success = true, result }
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
        public ActionResult DoMultipleQuery()
        {
            var bodyStream = new StreamReader(HttpContext.Request.InputStream);
            bodyStream.BaseStream.Seek(0, SeekOrigin.Begin);
            var bodyText = bodyStream.ReadToEnd();

            try
            {
                if (ModelState.IsValid)
                {
                    var data = _service.RealizarMultiplesConsultas(bodyText);

                    var result = JsonConvert.SerializeObject(data,
                       Newtonsoft.Json.Formatting.None,
                       new JsonSerializerSettings
                       {
                           ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                           NullValueHandling = NullValueHandling.Ignore
                       });
                    return Content(result);

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

        public ActionResult ObtenerFotoUsuario()
        {
            string base64String = "";
            var UsuarioDto = _service.ConsultarFotoUsuario();
            if (UsuarioDto != null)
            {
                base64String = Convert.ToBase64String(UsuarioDto, 0, UsuarioDto.Length);

                var result = JsonConvert.SerializeObject(base64String);

                return Content(result);
            }
            else
            {
                return Content("NOFOTO");
            }
        }

        public ActionResult GuardarFirmaUsuario()
        {
            var Uploadfile = Request.Files["uploadFile"];
            if (Uploadfile != null)
            {
                var usuario = _service.GuardarFotoUsuario(Uploadfile);
                var result = JsonConvert.SerializeObject(usuario);
                return Content(result);
            }
            else
            {
                return Content("ERROR");
            }
        }

        public ActionResult ObtenerCatalogos(string code)
        {
            var observaciones = _catalogoService.ObtenerCatalogos(code);
            var result = JsonConvert.SerializeObject(observaciones,
                    Newtonsoft.Json.Formatting.None,
                    new JsonSerializerSettings
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                        NullValueHandling = NullValueHandling.Ignore
                    });
            return Content(result);
        }
        public ActionResult GetByCodeApi(string code)
        {
            var result = _catalogoService.APIObtenerCatalogos(code);
            return new JsonResult
            {
                Data = new { success = true, result },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };

        }

        public ActionResult ObtenerDatosWs(string cedula)
        {
            try
            {
                var objeto = _webService.BusquedaPorCedulaRegistroCivil(cedula);
                var result = JsonConvert.SerializeObject(objeto,
                    Newtonsoft.Json.Formatting.None,
                    new JsonSerializerSettings
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                        NullValueHandling = NullValueHandling.Ignore
                    });
                return Content(result);
            }
            catch (Exception e)
            {

                return Content("SIN_RESPUESTA");
            }
        }
        public ActionResult ObtenerWsHuella(string cedula,string huella)
        {
            try
            {
                var objeto = _webService.BusquedaPorCedulaRegistroCivilHuellaDigital(cedula,huella);
                var result = JsonConvert.SerializeObject(objeto,
                    Newtonsoft.Json.Formatting.None,
                    new JsonSerializerSettings
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                        NullValueHandling = NullValueHandling.Ignore
                    });
                return Content(result);
            }
            catch (Exception e)
            {

                return Content("SIN_RESPUESTA");
            }
        }
        public ActionResult GetReporte(DateTime? desde,DateTime? hasta)
        {
            var excel = _service.Reporte(desde,hasta);
            string excelName = "ConsultRCivil" + DateTime.Now.ToShortDateString()+"_"+DateTime.Now.Second;
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