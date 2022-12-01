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
using System.Web;
using System.Web.Mvc;
using JsonResult = com.cpp.calypso.framework.JsonResult;

namespace com.cpp.calypso.web.Areas.CertificacionIngenieria.Controllers
{
    public class AvancePorcentajeProyectoController : BaseAccesoSpaController<AvancePorcentajeProyecto, AvancePorcentajeProyectoDto, PagedAndFilteredResultRequestDto>
    {
        private readonly IAvancePorcentajeProyectoAsyncBaseCrudAppService _avanceService;
        private readonly ICatalogoAsyncBaseCrudAppService _catalogoService;

        public AvancePorcentajeProyectoController(
            IHandlerExcepciones manejadorExcepciones,
            IViewService viewService,
            IAvancePorcentajeProyectoAsyncBaseCrudAppService avanceService,
        ICatalogoAsyncBaseCrudAppService catalogoService
        ) : base(manejadorExcepciones, viewService)
        {
            _avanceService = avanceService;
            _catalogoService = catalogoService;
        }
        // GET: CertificacionIngenieria/DetalleDirectoIngenieria
        public ActionResult Index()
        {
            var model = new FormReactModelView();
            model.Id = "avances_porcentaje_proyecto";
            model.ReactComponent = "~/Scripts/build/avance_porcentaje_proyecto_container.js";

            if (!string.IsNullOrEmpty(Title))
            {
                model.Title = Title;
            }


            ViewBag.ruta = new string[] { "Inicio", "Avances Porcentajes", "Lista" };

            return View(model);
        }

        [HttpGet]
        public ActionResult GetDetalles(DateTime? fechaCarga)
        {
            var listdetalles = _avanceService.ObtenerDetalles(fechaCarga);
            return WrapperResponseGetApi(ModelState, () => listdetalles);
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult DescargarPlantillaCargaMasiva()
        {
            var excel = _avanceService.DescargarPlantillaCargaMasiva();
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
            var excel = _avanceService.CargaMasiva(file);
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
            /*var entityDto = _avanceService.CatalogosIngenieria();
            return WrapperResponseGetApi(ModelState, () => entityDto);*/
            return Content("OK");

        }
        public ActionResult GetProyectos()
        {
            var entityDto = _avanceService.ObtenerProyectos();
            return WrapperResponseGetApi(ModelState, () => entityDto);

        }
        public ActionResult ObtenerDto(int Id,DateTime fecha)
        {
            var dto = _avanceService.ObtenerDato(Id,fecha);
             var result = JsonConvert.SerializeObject(dto);
             return Content(result);
         
        }

        [HttpPost]
        public ActionResult Crear(AvancePorcentajeProyecto input)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var dto = _avanceService.CrearDetalle(input);
                    return new JsonResult
                    {
                        Data = new { success = dto }
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
        public ActionResult Editar(AvancePorcentajeProyecto input)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var dto = _avanceService.ActualizarDetalle(input);
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
                    //var dto = _avanceService.ActualizarEstadoValidadoIngenieria(Id);
                    var dto = "OK";
                    return new JsonResult
                    {
                        Data = new { success = true, result = dto }
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
                    var dto = _avanceService.DeleteDetalle(Id);
                    return new JsonResult
                    {
                        Data = new { success = true, result = dto }
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

