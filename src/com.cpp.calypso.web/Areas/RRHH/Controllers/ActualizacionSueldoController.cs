using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.framework;
using com.cpp.calypso.proyecto.aplicacion.RecursosHumanos.Dto;
using com.cpp.calypso.proyecto.aplicacion.RecursosHumanos.Interfaces;
using com.cpp.calypso.proyecto.dominio.RecursosHumanos;
using com.cpp.calypso.web.Models;

namespace com.cpp.calypso.web.Areas.RRHH.Controllers
{
    public class ActualizacionSueldoController : BaseRecursosHumanosSpaController<ActualizacionSueldo, ActualizacionSueldoDto, PagedAndFilteredResultRequestDto>
    {
        private readonly IActualizacionSueldoAsyncBaseCrudAppService _actualizacionSueldoService;

        public ActualizacionSueldoController(
            IHandlerExcepciones manejadorExcepciones, 
            IViewService viewService,
            IActualizacionSueldoAsyncBaseCrudAppService actualizacionSueldoService
            
            ) : base(manejadorExcepciones, viewService)
        {
            _actualizacionSueldoService = actualizacionSueldoService;
            Title = "Capacitaciones de Colaboradores";
        }

        public ActionResult Index()
        {
            var model = new FormReactModelView();
            model.Id = "carga_masiva_de_sueldos";
            model.ReactComponent = "~/Scripts/build/actualizacion_sueldos.js";

            if (!string.IsNullOrEmpty(Title))
            {
                model.Title = Title;
            }
          

            ViewBag.ruta = new string[] { "Inicio", "Actualizaciones Masivas", "Jornales" };
           
            return View(model);
        }

        public ActionResult ActualizacionColaboradores()
        {
            var model = new FormReactModelView();
            model.Id = "actualizacion_masiva_colaboradores";
            model.ReactComponent = "~/Scripts/build/actualizacion_masiva_colaboradores.js";

            if (!string.IsNullOrEmpty(Title))
            {
                model.Title = Title;
            }
          

            ViewBag.ruta = new string[] { "Inicio", "Actualizaciones Masivas", "Colaboradores" };
           
            return View(model);
        }

        #region Api

        [System.Web.Mvc.HttpPost]
        public ActionResult DescargarPlantillaCargaMasivaDeJornales()
        {
            var excel = _actualizacionSueldoService.DescargarPlantillaCargaMasivaDeJornales();
            string excelName = "CargaDeJornales-"+DateTime.Now.ToShortDateString();
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


        public ActionResult CargaMasivaDeSueldosJornales(CargaMasivaSueldosModel model)
        {
            var excel = _actualizacionSueldoService.CargaMasivaDeSueldosJornales(model.file, model.observaciones, model.fecha);
            string excelName = "ResultadoCargaDeSueldosJornales " + DateTime.Now.ToShortDateString();
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
        public ActionResult DescargarPlantillaActualizacionMasivaDeColaboradores()
        {
            var excel = _actualizacionSueldoService.DescargarPlantillaActualizacionMasivaDeColaboradores();
            string excelName = "ActualizacionDeColaboradores "+DateTime.Now.ToShortDateString();
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

        public ActionResult ObtenerTodasLasActualizacionesDeSaldos()
        {
            var pagedResultDto = _actualizacionSueldoService.ObtenerTodasLasActualizacionesDeSaldos();
            return WrapperResponseGetApi(ModelState, () => pagedResultDto);
        }

        public ActionResult ObtenerDetallesDeUnaActualizacion(int id)
        {
            var pagedResultDto = _actualizacionSueldoService.ObtenerDetallesDeUnaActualizacion(id);
            return WrapperResponseGetApi(ModelState, () => pagedResultDto);
        }
         
        
        [System.Web.Mvc.HttpPost]
        public ActionResult CargaMasivaDeActualizacionColaboradores(HttpPostedFileBase file)
        {
            var excel = _actualizacionSueldoService.CargaMasivaDeActualizacionColaboradores(file);
            string excelName = "ResultadoDeCargaDeColaboradores" +DateTime.Now.ToShortDateString();
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
        public ActionResult CargaMasivaDeReingresosColaboradores(HttpPostedFileBase file)
        {
            var excel = _actualizacionSueldoService.ActualizacionMasivaReingresoColaboradores(file);
            string excelName = "ResultadoDeCargaDeColaboradores" + DateTime.Now.ToShortDateString();
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

        #endregion
    }
}