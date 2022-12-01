using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.framework;
using com.cpp.calypso.proyecto.aplicacion.Transporte.Dto;
using com.cpp.calypso.proyecto.aplicacion.Transporte.Interface;
using com.cpp.calypso.proyecto.aplicacion.Transporte.Models;
using com.cpp.calypso.proyecto.dominio.Transporte;
using Newtonsoft.Json;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JsonResult = com.cpp.calypso.framework.JsonResult;

namespace com.cpp.calypso.web.Areas.Transporte.Controllers
{
    public class RutaController : BaseTransporteSpaController<Ruta, RutaDto, PagedAndFilteredResultRequestDto>
    {

        private readonly IRutaAsyncBaseCrudAppService _RutaService;
        private readonly IRutaParadaAsyncBaseCrudAppService _RutaParadaService;
        private readonly IChoferAsyncBaseCrudAppService _ChoferService;

        public RutaController(
          IRutaAsyncBaseCrudAppService RutaService,
          IRutaParadaAsyncBaseCrudAppService RutaParadaService,
        IHandlerExcepciones manejadorExcepciones,
        IChoferAsyncBaseCrudAppService ChoferService,
        IViewService viewService,
          IAsyncBaseCrudAppService<Ruta, RutaDto, PagedAndFilteredResultRequestDto, RutaDto> entityService

          ) : base(manejadorExcepciones, viewService, entityService)
        {
            _RutaService = RutaService;
            _RutaParadaService = RutaParadaService;
            _ChoferService = ChoferService;
        }

        // GET: Transporte/Chofer
        public ActionResult Index()
        {
            ViewBag.ruta = new string[] { "Inicio", "Rutas", "Listado de Rutas" };
            return View();
        }

        // GET: Transporte/Chofer/Details/5
        public ActionResult Details(int id)
            
        {
            ViewBag.ruta = new string[] { "Inicio", "Rutas", "Asignación de Paradas y Horarios" };
            var ruta = _RutaService.GetDetalles(id);
            return View(ruta);
        }
        public ActionResult Reporte()

        {
            ViewBag.ruta = new string[] { "Inicio", "Transporte", "Reportes"};     
            return View();
        }

        // GET: Transporte/Chofer/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Transporte/Chofer/Create
        [HttpPost]
        public ActionResult Create(Ruta ruta)
        {
            bool existe = _RutaService.existecode(ruta.Codigo, 0);
            
            if (existe) {
                return Content("Existe");
            }
                var rutaid = _RutaService.IngresarRuta(ruta);
                return Content(rutaid > 0 ? "OK" :rutaid<0?"ORIGENDESTIO":"ERROR");
            
        }

        // GET: Transporte/Chofer/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Transporte/Chofer/Edit/5
        [HttpPost]
        public ActionResult Edit(Ruta ruta)
        {

            bool existe = _RutaService.existecode(ruta.Codigo, ruta.Id);
            if (existe)
            {
                return Content("Existe");
            }
            var rutaid = _RutaService.EditarRuta(ruta);
                return Content(rutaid > 0 ? "OK" : "ERROR");


    }

        // GET: Transporte/Chofer/Delete/5
        public ActionResult Delete()
        {
            return View();
        }

        // POST: Transporte/Chofer/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {

            var rutaid = _RutaService.EliminarRuta(id);
            if (rutaid > 0)
            {
                return Content(rutaid > 0 ? "OK" : "ERROR");

            }
            else
            {
                return Content("NOPUEDE");
            }

        }

        public ActionResult ListaRutas()
        {
            var listarutas = _RutaService.Listar();

            return WrapperResponseGetApi(ModelState, () => listarutas);
        }
        public ActionResult Listalugares()
        {
            var listalugares = _RutaService.ListarLugares();

            return WrapperResponseGetApi(ModelState, () => listalugares);
        }
       
        public ActionResult ObtenerDetallesRuta(int id)
        {


            var ruta = _RutaService.GetDetalles(id);
            if (ruta != null)
            {
     
                var result = JsonConvert.SerializeObject(ruta,
                    Newtonsoft.Json.Formatting.None,
                    new JsonSerializerSettings
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                        NullValueHandling = NullValueHandling.Ignore
                    });
                return Content(result);
            }
            else
            {
                return Content("Error");
            }
        }

        public ActionResult ObtenerParadasRuta(int id)
        {


            var lista = _RutaParadaService.ListaParadaporRuta(id);
            if (lista != null)
            {
                var result = JsonConvert.SerializeObject(lista);
                return Content(result);
            }
            else
            {
                return Content("Error");
            }
        }

        public ActionResult ObtenerHorariosRuta(int id)
        {


            var lista = _RutaParadaService.ListaHorariosporRuta(id);
            if (lista != null)
            {
                var result = JsonConvert.SerializeObject(lista);
                return Content(result);
            }
            else
            {
                return Content("Error");
            }
        }
        public ActionResult ObtenerParadas()
        {


            var lista = _RutaParadaService.ListaParadas();
            if (lista != null)
            {
                var result = JsonConvert.SerializeObject(lista);
                return Content(result);
            }
            else
            {
                return Content("Error");
            }
        }

        public ActionResult ObtenerReportesPersonasTransportadas(InputReporteTransporte input)
        {


            ExcelPackage excel = _RutaService.ExcelPersonasTransportadas(input);


            string excelName = "Reporte Personas Transportadas" + DateTime.Now.ToShortDateString();
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
        public ActionResult ObtenerReportesDiarioViajes(InputReporteTransporte input)
        {



            ExcelPackage excel = _RutaService.ExcelViajes(input);


            string excelName = "Reporte Viajes" + DateTime.Now.ToShortDateString();
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

        public ActionResult ObtenerProveedoresTransporte()
        {
            var proveedores_transporte = _ChoferService.ListaProveedoresTransporte();

            return WrapperResponseGetApi(ModelState, () => proveedores_transporte);
        }

        public ActionResult ObtenerRutasProveedor(int id)//Provedor Id
        {
            var lista = _RutaService.ListadeRutasporProveedor(id);
            if (lista != null)
            {
                var result = JsonConvert.SerializeObject(lista);
                return Content(result);
            }
            else
            {
                return Content("Error");
            }
        }
        public ActionResult ObtenerVehiculosProveedor(int id)//Provedor Id
        {
            var lista = _RutaService.ListadeVehiculosporProveedor(id);
            if (lista != null)
            {
                var result = JsonConvert.SerializeObject(lista);
                return Content(result);
            }
            else
            {
                return Content("Error");
            }
        }

        public ActionResult ObtenerReporteDiariodeTrabajo(InputReporteTransporte input)
        {
            ExcelPackage excel = _RutaService.ExcelTrabajosDiarios(input);


            string excelName = "Reporte Trabajos Diarios" + DateTime.Now.ToShortDateString();
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


        #region Reporte Retiro de Transportistas Api

        public ActionResult ObtenerReporteRetiroViandas(InputRetiroTransportista input)
        {
            
            ExcelPackage excel = _RutaService.ObtenerReporteRetiroViandas(input);

            string excelName = "Reporte Retiro Viandas" + DateTime.Now.ToShortDateString();
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

        public ActionResult ObtenerTransportistas()
        {
            var dtos = _RutaService.ObtenerTransportistas();
            
            return new JsonResult
            {
                Data = new { success = true, result = dtos }
            };
        }

        public ActionResult ObtenerTiposComidasViandas()
        {
            var dtos = _RutaService.ObtenerTiposComidaViandas();
            
            return new JsonResult
            {
                Data = new { success = true, result = dtos }
            };
        }
        

        #endregion
    }
}
