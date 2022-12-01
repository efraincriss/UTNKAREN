using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Abp.Application.Services.Dto;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.framework;
using com.cpp.calypso.proyecto.aplicacion.RecursosHumanos.Dto;
using com.cpp.calypso.proyecto.aplicacion.RecursosHumanos.Interfaces;
using com.cpp.calypso.proyecto.dominio.Constantes;
using com.cpp.calypso.proyecto.dominio.RecursosHumanos;

namespace com.cpp.calypso.web.Areas.RRHH.Controllers
{
    public class CapacitacionController : BaseRecursosHumanosSpaController<Capacitacion, CapacitacionDto, PagedAndFilteredResultRequestDto>
    {
        private readonly ICapacitacionAsyncBaseCrudAppService _capacitacionService;

        public CapacitacionController(
            IHandlerExcepciones manejadorExcepciones, 
            IViewService viewService,
            ICapacitacionAsyncBaseCrudAppService capacitacionService
            ) : base(manejadorExcepciones, viewService)
        {
            _capacitacionService = capacitacionService;
            Title = "Capacitaciones de Colaboradores";
        }
        

        public ActionResult Index()
        {
            var model = new FormReactModelView();
            model.Id = "capacitaciones_colaboradores_listado";
            model.ReactComponent = "~/Scripts/build/capacitaciones_colaboradores_listado.js";

            if (!string.IsNullOrEmpty(Title))
            {
                model.Title = Title;
            }
          

            ViewBag.ruta = new string[] { "Inicio", "Capacitaciones", "Colaboradores" };
           
            return View(model);
        }

        public ActionResult Detalle(int colaboradorId)
        {
            var model = new FormReactModelView();
            model.Id = "detalle_capacitaciones_por_colaborador";
            model.ReactComponent = "~/Scripts/build/detalle_capacitaciones.js";

            if (!string.IsNullOrEmpty(Title))
            {
                model.Title = Title;
            }
          

            ViewBag.ruta = new string[] { "Inicio", "Capacitaciones", "Colaborador", "Detalle" };
           
            return View(model);
        }


        #region Api
        public ActionResult Search(string filtro, string estado)
        {
            var colaboradores = _capacitacionService.BuscarColaboradores(filtro, estado);
            return WrapperResponseGetApi(ModelState, () => colaboradores);
        }

        public ActionResult ObtenerCapacitacionesPorColaborador(int id)
        {
            var capacitaciones = _capacitacionService.ObtenerCapacitacionesPorColaborador(id);
            return WrapperResponseGetApi(ModelState, () => capacitaciones);
        }

        public ActionResult ObtenerTodasLasCapacitaciones(string filtroColaborador, string tipoCapacitacion, string nombreCapacitacion, string fechaDesde, string fechaHasta)
        {
            var capacitaciones = _capacitacionService.ObtenerCapacitaciones(filtroColaborador, tipoCapacitacion, nombreCapacitacion, fechaDesde, fechaHasta);
            return WrapperResponseGetApi(ModelState, () => capacitaciones);
        }

        public ActionResult ObtenerCatalogosDeCapacitaciones()
        {
            var capacitaciones = _capacitacionService.ObtenerCatalogosDeCapacitaciones();
            return WrapperResponseGetApi(ModelState, () => capacitaciones);
        }

        [System.Web.Mvc.HttpPost]
        public async Task<ActionResult> ActualizarCapacitacion(CapacitacionDto capacitacion)
        {
            var capacitaciones = await _capacitacionService.Update(capacitacion);
            return WrapperResponseGetApi(ModelState, () => capacitaciones);
        }

        [System.Web.Mvc.HttpPost]
        public async Task<ActionResult> CrearCapacitacion(CapacitacionDto capacitacion)
        {
            var capacitaciones = await _capacitacionService.Create(capacitacion);
            return WrapperResponseGetApi(ModelState, () => capacitaciones);
        }

        [System.Web.Mvc.HttpPost]
        public async Task<ActionResult> EliminarCapacitacion(int id)
        {
            await _capacitacionService.Delete(new EntityDto<int>(id));
            return WrapperResponseGetApi(ModelState, () => true);
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult GenerarCertificados(int[] colaboradores)
        {

            var word = _capacitacionService.DescargarCertificados(colaboradores);
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

        [System.Web.Mvc.HttpPost]
        public ActionResult DescargarPlantillaCargaMasivaDeCapacitaciones()
        {
            var excel = _capacitacionService.DescargarPlantillaCargaMasivaDeCapacitaciones();
            string excelName = "CargaDeCapacitaciones"+DateTime.Now.ToShortDateString();
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
        public ActionResult CargaMasivaDeCapacitaciones(HttpPostedFileBase file)
        {
            var excel = _capacitacionService.CargarCapacitaciones(file);
            string excelName = "ResultadoDeCargaDeCapacitaciones" +DateTime.Now.ToShortDateString();
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
        public ActionResult DescargarReporteCapacitaciones(int CatalogoNombreCapacitacionId, int CatalogoTipoCapacitacionId, DateTime? FechaDesde, DateTime? FechaHasta)
        {
            var excel = _capacitacionService.DescargarReporteDeCapacitaciones(CatalogoNombreCapacitacionId, CatalogoTipoCapacitacionId, FechaDesde, FechaHasta);
            string excelName = "ReporteCapacitaciones " + DateTime.Now.ToShortDateString();
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