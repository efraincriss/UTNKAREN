using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.framework;
using com.cpp.calypso.proyecto.aplicacion.Documentos.Dto;
using com.cpp.calypso.proyecto.aplicacion.Documentos.Interface;
using com.cpp.calypso.proyecto.aplicacion.Interfaces;
using com.cpp.calypso.proyecto.dominio.Documentos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using JsonResult = com.cpp.calypso.framework.JsonResult;

namespace com.cpp.calypso.web.Areas.Documentos.Controllers
{
    public class CarpetaController : BaseAccesoSpaController<Carpeta, CarpetaDto, PagedAndFilteredResultRequestDto>
    {
        private readonly ICarpetaAsyncBaseCrudAppService _carpetaService;
        private readonly ICatalogoAsyncBaseCrudAppService _catalogoRepository;
        private readonly IDocumentoAsyncBaseCrudAppService _documentoRepository;

        public CarpetaController(
            IHandlerExcepciones manejadorExcepciones, 
            IViewService viewService,
            ICarpetaAsyncBaseCrudAppService carpetaService,
            ICatalogoAsyncBaseCrudAppService catalogoRepository,
             IDocumentoAsyncBaseCrudAppService documentoRepository
            ) : base(manejadorExcepciones, viewService)
        {
            _carpetaService = carpetaService;
            _catalogoRepository = catalogoRepository;
            _documentoRepository = documentoRepository;
        }

        public ActionResult Index()
        {
            var model = new FormReactModelView();
            model.Id = "gestion_carpetas_container";
            model.ReactComponent = "~/Scripts/build/gestion_carpetas_container.js";

            if (!string.IsNullOrEmpty(Title))
            {
                model.Title = Title;
            }


            ViewBag.ruta = new string[] { "Inicio", "Gestión de Contratos", "Listado de Contratos" };

            return View(model);
        }
        public ActionResult Reportes()
        {
            

            ViewBag.ruta = new string[] { "Inicio", "Gestión de Contratos", "Reportes" };

            return View();
        }


        #region Api
        [HttpGet]
        public ActionResult ObtenerCarpeta(int id)
        {
            var carpeta = _carpetaService.ObtenerCarpetaPorId(id);
            return WrapperResponseGetApi(ModelState, () => carpeta);
        }

        [HttpGet]
        public ActionResult ObtenerTodas()
        {
            var carpetas = _carpetaService.ObtenerCarpetas();
            return WrapperResponseGetApi(ModelState, () => carpetas);
        }

        [HttpDelete]
        public ActionResult EliminarCarpeta(int id)
        {
            var result = _carpetaService.Eliminar(id);
            return new JsonResult
            {
                Data = new { success = result.Eliminado, result = result.Error }
            };
        }

        [HttpPost]
        public async Task<ActionResult> CreateAsync(CarpetaDto input)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var dto = await _carpetaService.CrearCarpetaAsync(input);
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


        [HttpPost]
        public ActionResult EditarCarpetaAsync(CarpetaDto input)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var dto = _carpetaService.EditarCarpetaAsync(input);
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

        [HttpGet]
        public ActionResult ObtenerCatalogoEstados()
        {
            var catalogos = _catalogoRepository.APIObtenerCatalogos("ESTADO_CONTRATO");
            return WrapperResponseGetApi(ModelState, () => catalogos);
        }

        [HttpGet]
        public ActionResult ObtenerCarpetasautorizadasAUsuario()
        {
            var estructura = _carpetaService.ObtenerCarpetasAutorizadas();
            return WrapperResponseGetApi(ModelState, () => estructura);
        }


        public ActionResult ReporteDocumentos() //RdoCabeceraId
        {


            var excelPackage = _documentoRepository.ListadoDocumentos();

            string excelName = "RDocumentos" + DateTime.Now.Date.ToShortDateString();
            using (var memoryStream = new MemoryStream())
            {
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment; filename=" + excelName + ".xlsx");
                excelPackage.SaveAs(memoryStream);
                memoryStream.WriteTo(Response.OutputStream);
                Response.Flush();
                Response.End();
                return Content("");
            }
        }

        public ActionResult ReporteUsuariosAut() //RdoCabeceraId
        {


            var excelPackage = _documentoRepository.ListadoUsuariosAutorizados();

            string excelName = "RUsuarios" + DateTime.Now.Date.ToShortDateString();
            using (var memoryStream = new MemoryStream())
            {
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment; filename=" + excelName + ".xlsx");
                excelPackage.SaveAs(memoryStream);
                memoryStream.WriteTo(Response.OutputStream);
                Response.Flush();
                Response.End();
                return Content("");
            }
        }

        #endregion
    }
}