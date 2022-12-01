using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.framework;
using com.cpp.calypso.proyecto.aplicacion.Dto;
using com.cpp.calypso.proyecto.aplicacion.Interfaces;
using com.cpp.calypso.proyecto.dominio;
using CommonServiceLocator;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace com.cpp.calypso.web.Areas.Proveedor.Controllers
{
    public class ReporteConsumoController : BaseController
    {
        public IProveedorAsyncBaseCrudAppService _proveedorService;
        public ReporteConsumoController(
               IHandlerExcepciones manejadorExcepciones,
               IProveedorAsyncBaseCrudAppService proveedorService
            ) : base(manejadorExcepciones)
        {
            _proveedorService = proveedorService;
        }

        // GET: Proveedor/ReporteConsumo
        public ActionResult Consumos()
        {
            return View();
        }
        public ActionResult ConsumosExcel()
        {
            return View();
        }
        public ActionResult Viandas()
        {
            return View();
        }
        public ActionResult Hospedaje()
        {
            return View();
        }
        public ActionResult GetProveedores()//Proveedores de Alimentación
        {
            var proveedores_alimentacion = _proveedorService.ListProveedorAlimentacion();

            return WrapperResponseGetApi(ModelState, () => proveedores_alimentacion);
        }
        public ActionResult GetZonas()//>onas
        {
            var zonas = _proveedorService.Zonas();

            return WrapperResponseGetApi(ModelState, () => zonas);
        }
        public ActionResult ActualizarReservas()//>onas
        {
            var zonas = _proveedorService.ActualizarCamposNuevos();

            return WrapperResponseGetApi(ModelState, () => zonas);
        }

        public ActionResult GetProveedoresHospedaje()//Proveedores de Alimentación
        {
            var proveedores = _proveedorService.ListProveedorHospedaje();

            return WrapperResponseGetApi(ModelState, () => proveedores);
        }

        public ActionResult  ReporteDiario(int proveedorId, DateTime fecha) //RdoCabeceraId
        {
            var excelPackage = _proveedorService.ReporteDiarioVianda(proveedorId, fecha);

            string excelName = "RViandas-" +DateTime.Now.Date.ToShortDateString();
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
        public ActionResult ReporteMensual(int proveedorIdM, DateTime fechaInicio,DateTime fechaFin) //RdoCabeceraId
        {
            var excelPackage = _proveedorService.ReporteDiarioViandaMensula( proveedorIdM, fechaInicio, fechaFin);

            string excelName = "RViandas-" + DateTime.Now.Date.ToShortDateString();
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

        public ActionResult ReporteConsumoDiario(string Ids, DateTime fecha) //RdoCabeceraId
        {
            List<int> ProveedoresId = new List<int>();
            var a = Ids.Split(',');
            foreach (var item in a)
            {
                ProveedoresId.Add(Int32.Parse(item));

            }
            var excelPackage = _proveedorService.ReporteDiarioConsumo(ProveedoresId, fecha);

            string excelName = "RConsumos-" + DateTime.Now.Date.ToShortDateString();
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

        public ActionResult ReporteConsumoMensual(string Ids, DateTime fechaInicio, DateTime fechaFin) //RdoCabeceraId
        {

            List<int> ProveedoresId = new List<int>();
            var a = Ids.Split(',');
            foreach (var item in a)
            {
                ProveedoresId.Add(Int32.Parse(item));

            }
            var excelPackage = _proveedorService.ReporteDiarioConsumoMensual(ProveedoresId, fechaInicio, fechaFin);

            string excelName = "RConsumoM-" + DateTime.Now.Date.ToShortDateString();
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


        public ActionResult ReporteConsumoMensualConsolidado(DateTime fechaInicio, DateTime fechaFin, string zonaIds = "") //RdoCabeceraId
        {
            List<int> ZonaId = new List<int>();
            var a = zonaIds.Split(',');
            if (a.Length > 0)
            {
                foreach (var item in a)
                {
                    if (item != "")
                    {
                        ZonaId.Add(Int32.Parse(item));
                    }

                }
            }
            List<int> ProveedoresId = _proveedorService.ProveedoresConsolidadosporZona(ZonaId);
            var excelPackage = _proveedorService.ConsumoMensualConsolidado(ProveedoresId, fechaInicio, fechaFin, ZonaId);

            string excelName = "RConsumoM-" + DateTime.Now.Date.ToShortDateString();
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




        public ActionResult ReporteHospedaje(string Ids, DateTime fecha) //RdoCabeceraId
        {
            List<int> ProveedoresId = new List<int>();
            var a = Ids.Split(',');
            foreach (var item in a)
            {
                ProveedoresId.Add(Int32.Parse(item));

            }
            var excelPackage = _proveedorService.ReporteDiarioHospedaje(ProveedoresId, fecha);

            string excelName = "RHospedaje-" + DateTime.Now.Date.ToShortDateString();
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
        public ActionResult ReporteHospedajeMensual(string Ids, DateTime fechaInicio, DateTime fechaFin) //RdoCabeceraId
        {

            List<int> ProveedoresId = new List<int>();
            var a = Ids.Split(',');
            foreach (var item in a)
            {
                ProveedoresId.Add(Int32.Parse(item));

            }
            var excelPackage = _proveedorService.ReporteHospedajeMensual(ProveedoresId, fechaInicio, fechaFin);

            string excelName = "RHospedajeM-" + DateTime.Now.Date.ToShortDateString();
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


        public ActionResult ReporteHospedajeConsolidado(DateTime fechaInicio, DateTime fechaFin, string zonaIds="") //RdoCabeceraId
        {
            List<int> ZonaId = new List<int>();
            var a = zonaIds.Split(',');
           if( a.Length > 0){ 
            foreach (var item in a)
            {
                    if (item != "") {
                ZonaId.Add(Int32.Parse(item));
                    }

                }
            }
            List<int> ProveedoresId = _proveedorService.ProveedoresConsolidadosporZona(ZonaId);
            
            var excelPackage = _proveedorService.HospedajeMensualConsolidado(ProveedoresId, fechaInicio, fechaFin, ZonaId);

            string excelName = "RHospedajeM-" + DateTime.Now.Date.ToShortDateString();
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




        public ActionResult ReporteVencimientoContrato()
        {

            var excelPackage = _proveedorService.ReporteVencimientoContratosProveedor();

            string excelName = "VENCIMIENTOSCONTRATOS-" + DateTime.Now.Date.ToShortDateString();
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

        public ActionResult ReporteConsumoDuplicado(DateTime fechaInicio, DateTime fechaFin) //RdoCabeceraId
        {

            var excelPackage = _proveedorService.ReporteDuplicados(new List<int>(),fechaInicio, fechaFin); //Lista de Proveedores no se selecciona por el rango de fechas, objeto vacio

            string excelName = "CONTROL_ALIMENTACION-" + DateTime.Now.Date.ToShortDateString()+"_"+DateTime.Now.Second.ToString();
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

        public ActionResult ReporteHospedajeSerge(string Ids, DateTime fechaInicio, DateTime fechaFin) //RdoCabeceraId
        {

            List<int> ProveedoresId = new List<int>();
            var a = Ids.Split(',');
            foreach (var item in a)
            {
                ProveedoresId.Add(Int32.Parse(item));

            }
            var excelPackage = _proveedorService.ReporteHospedajeSerge(ProveedoresId, fechaInicio, fechaFin);

            string excelName = "RSerge-INGRESOS_SALIDAS" + DateTime.Now.Date.ToShortDateString();
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



        public ActionResult ReporteHospedajeIniciados(string Ids, DateTime fechaInicio, DateTime fechaFin) //RdoCabeceraId
        {

            List<int> ProveedoresId = new List<int>();
            var a = Ids.Split(',');
            foreach (var item in a)
            {
                ProveedoresId.Add(Int32.Parse(item));

            }
            var excelPackage = _proveedorService.ReporteHospedajeFinalizados(ProveedoresId, fechaInicio, fechaFin);

            string excelName = "RSerge-RESERVASNOFINALIZADAS" + DateTime.Now.Date.ToShortDateString();
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

    }
}
