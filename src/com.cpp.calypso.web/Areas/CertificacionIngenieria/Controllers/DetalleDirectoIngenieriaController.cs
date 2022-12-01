using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.framework;
using com.cpp.calypso.proyecto.aplicacion.CertificacionIngenieria.Dto;
using com.cpp.calypso.proyecto.aplicacion.CertificacionIngenieria.Interface;
using com.cpp.calypso.proyecto.aplicacion.Interfaces;
using com.cpp.calypso.proyecto.dominio.CertificacionIngenieria;
using com.cpp.calypso.web.Areas.Accesos.Controllers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using JsonResult = System.Web.Mvc.JsonResult;

namespace com.cpp.calypso.web.Areas.CertificacionIngenieria.Controllers
{
    public class DetalleDirectoIngenieriaController : BaseAccesoSpaController<DetallesDirectosIngenieria, DetallesDirectosIngenieriaDto, PagedAndFilteredResultRequestDto>
    {
        private readonly IDetallesDirectosIngenieriaAsyncBaseCrudAppService _detallesIngenieriaService;
        private readonly ICatalogoAsyncBaseCrudAppService _catalogoService;

        public DetalleDirectoIngenieriaController(
            IHandlerExcepciones manejadorExcepciones,
            IViewService viewService,
            IDetallesDirectosIngenieriaAsyncBaseCrudAppService detallesIngenieriaService,
            ICatalogoAsyncBaseCrudAppService catalogoService
        ) : base(manejadorExcepciones, viewService)
        {
            _detallesIngenieriaService = detallesIngenieriaService;
            _catalogoService = catalogoService;
        }
        // GET: CertificacionIngenieria/DetalleDirectoIngenieria
        public ActionResult Index()
        {
            var model = new FormReactModelView();
            model.Id = "detalles_directos_ingenieria";
            model.ReactComponent = "~/Scripts/build/detalles_directos_ingenieria_container.js";

            if (!string.IsNullOrEmpty(Title))
            {
                model.Title = Title;
            }


            ViewBag.ruta = new string[] { "Inicio", "Directos", "Lista" };

            return View(model);
        }

        [HttpGet]
        public ActionResult GetDetalles(DateTime? fechaInicio, DateTime? fechaFin)
        {
            // var listdetalles = _detallesIngenieriaService.ObtenerDetallesIngenieria(fechaInicio, fechaFin);

             var listdetalles = _detallesIngenieriaService.ObtenerDetallesDirectosIngenieria(fechaInicio, fechaFin);
            return WrapperResponseGetApi(ModelState, () => listdetalles);
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult DescargarPlantillaCargaMasiva()
        {
            var excel = _detallesIngenieriaService.DescargarPlantillaCargaMasivaDetallesIngenieria();
            string excelName = "CargaDetalles-" + DateTime.Now.ToShortDateString();
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

        [System.Web.Mvc.HttpPost]
        public ActionResult CargaMasivaDetalles(HttpPostedFileBase file)
        {
            var excel = _detallesIngenieriaService.CargaMasivaDetallesIngenieria(file);
            string excelName = "ResultadoDeCarga" + DateTime.Now.ToShortDateString();
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

        public ActionResult GetCatalogosIngenieria()
        {
            var entityDto = _detallesIngenieriaService.CatalogosIngenieria();
            return WrapperResponseGetApi(ModelState, () => entityDto);

        }

        public ActionResult GetSecuencial() {
            var secuencia = _detallesIngenieriaService.SecuencialCargaDirectos();
            var result = JsonConvert.SerializeObject(secuencia);
            return Content(result);
        }



        public ActionResult GetProyectos()
        {
            var entityDto = _detallesIngenieriaService.ObtenerProyectos();
            return WrapperResponseGetApi(ModelState, () => entityDto);

        }
        public ActionResult ObtenerColaborador(string search)
        {
            var list = _detallesIngenieriaService.ObtenerColaboradores(search);
            var result = JsonConvert.SerializeObject(list);
            return Content(result);
        }

        [HttpPost]
        public  ActionResult Crear(DetallesDirectosIngenieria input)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var dto =  _detallesIngenieriaService.CrearDetalle(input);
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
        public ActionResult Editar(DetallesDirectosIngenieria input)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var dto = _detallesIngenieriaService.ActualizarDetalle(input);
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
        public ActionResult ActualizarEstado(int Id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var dto = _detallesIngenieriaService.ActualizarEstadoValidadoIngenieria(Id);
                    return new JsonResult
                    {
                        Data = new { success = true ,result=dto}
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
        public ActionResult Delete(int Id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var dto = _detallesIngenieriaService.DeleteDetalle(Id);
                    return new JsonResult
                    {
                        Data = new { success = true,resut=dto }
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

        public ActionResult ObtenerUltimaCargaTimesheet()
        {
            var entityDto = _detallesIngenieriaService.ObtenerUltimaCargaTimesheet();
            return WrapperResponseGetApi(ModelState, () => entityDto);

        }

        [HttpPost]
        public async Task<ActionResult> ValidarCargaTimesheetAsync(int Id)
        {
            try
            {
                var dto = await _detallesIngenieriaService.ValidarCargaTimesheetAsync(Id);
                return new JsonResult
                {
                    Data = new { success = dto }
                };
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
