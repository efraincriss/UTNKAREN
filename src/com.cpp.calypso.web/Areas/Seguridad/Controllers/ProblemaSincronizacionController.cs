using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Abp.Runtime.Security;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.framework;
using com.cpp.calypso.proyecto.aplicacion.Seguridades.Dto;
using com.cpp.calypso.proyecto.aplicacion.Seguridades.Interface;
using com.cpp.calypso.proyecto.dominio.Seguridades;
using com.cpp.calypso.web.Areas.Accesos.Controllers;

namespace com.cpp.calypso.web.Areas.Seguridad.Controllers
{
    public class ProblemaSincronizacionController : BaseAccesoSpaController<ProblemaSincronizacion, ProblemaSincronizacionDto, PagedAndFilteredResultRequestDto>
    {
        private readonly IProblemaSincronizacionAsyncBaseCrudAppService _problemaSincronizacionService;

        public ProblemaSincronizacionController(
            IHandlerExcepciones manejadorExcepciones, 
            IProblemaSincronizacionAsyncBaseCrudAppService problemaSincronizacionService,
            IViewService viewService
            ) : base(manejadorExcepciones, viewService)
        {
            _problemaSincronizacionService = problemaSincronizacionService;
            Title = "Problemas de Sincronización";
        }

        public ActionResult Index()
        {
            var model = new FormReactModelView();
            model.Id = "problema_sincronizacion_container";
            model.ReactComponent = "~/Scripts/build/problema_sincronizacion_container.js";

            if (!string.IsNullOrEmpty(Title))
            {
                model.Title = Title;
            }
          

            ViewBag.ruta = new string[] { "Inicio", "Problema de Sincronizacion", "Listado" };
           
            return View(model);
        }

        public ActionResult Search(DateTime? fechaInicio, DateTime? fechaFin, bool solucionado)
        {
            var data = _problemaSincronizacionService.ObtenerProblemas(fechaInicio, fechaFin, solucionado);
            return WrapperResponseGetApi(ModelState, () => data);
        }

        [HttpPost]
        public ActionResult Solucionar(int problemaSincronizacionId, string observacion)
        {
            _problemaSincronizacionService.SolucionarProblema(problemaSincronizacionId, observacion);
            return WrapperResponseGetApi(ModelState, () => true);
        }

        [HttpPost]
        public ActionResult MarcarNoSolucionado(int problemaSincronizacionId)
        {
            _problemaSincronizacionService.MarcarNoSolucionado(problemaSincronizacionId);
            return WrapperResponseGetApi(ModelState, () => true);
        }

        [HttpPost]
        public ActionResult SolucionarMultiple(List<int> ids, string observacion)
        {
            var result = _problemaSincronizacionService.SolucionarMultiple(ids, observacion);
            return WrapperResponseGetApi(ModelState, () => result);
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult DescargarPlantillaCargaMasivaDeJornales(List<int> ids)
        {
            var excel = _problemaSincronizacionService.DescargarListadoErroresDeSincronizacion(ids);
            string excelName = "ErroreDeSincronizacion-"+DateTime.Now.ToShortDateString();
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