using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.framework;
using com.cpp.calypso.proyecto.aplicacion.Acceso.Dto;
using com.cpp.calypso.proyecto.aplicacion.Interfaces;
using com.cpp.calypso.proyecto.aplicacion.Proveedor.Dto;
using com.cpp.calypso.proyecto.aplicacion.Proveedor.Interfaces;
using com.cpp.calypso.proyecto.dominio;
using Newtonsoft.Json;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace com.cpp.calypso.web.Areas.Proveedor.Controllers
{
    public class LiquidacionServicioController : BaseController
    {
        private readonly ILiquidacionServicioAsyncBaseCrudAppService _LiquidacionServicio;
        private readonly IProveedorAsyncBaseCrudAppService _ProveedorServicio;

        public LiquidacionServicioController(
            IHandlerExcepciones manejadorExcepciones,


            ILiquidacionServicioAsyncBaseCrudAppService LiquidacionServicio,
            IProveedorAsyncBaseCrudAppService ProveedorServicio
            ) : base(manejadorExcepciones)
        {
            _LiquidacionServicio = LiquidacionServicio;
            _ProveedorServicio = ProveedorServicio;

        }
        // GET: Proveedor/LiquidacionServicio
        public ActionResult Index()
        {
            ViewBag.ruta = new string[] { "Inicio", "Liquidación Servicio", "Gestión" };
            return View();
        }

        public ActionResult IndexLiquidacion()
        {
            ViewBag.ruta = new string[] { "Inicio", "Liquidación Servicio", "Listado" };
            return View();
        }
        // GET: Proveedor/LiquidacionServicio/Details/5
        public ActionResult Details(int id)
        {
            ViewBag.ruta = new string[] { "Inicio", "Liquidación Servicio", "Generación" };
            ViewBag.Id = id;
            return View();
        }
        // POST: Proveedor/LiquidacionServicio/Create
        [HttpPost]
        public ActionResult Create(InputLiquidacionDto input, List<FormatLiquidacionReserva> hospedaje)
        {
            var ContratoProveedorId = _LiquidacionServicio.ObtenerContratoProveedor(input.ProveedorId);
            if (ContratoProveedorId > 0)
            {
                string generar = _LiquidacionServicio.GenerarLiquidacionHospedaje(ContratoProveedorId, input, hospedaje);
                return Content(generar);
            }
            else
            {
                return Content("NO_CONTRATO");
            }
        }
        [HttpPost]
        public ActionResult CreateCosumo(InputLiquidacionDto input, List<FormatLiquidacionConsumo> consumo, List<FormatLiquidacionSolicitudVianda> viandas)
        {
            var ContratoProveedorId = _LiquidacionServicio.ObtenerContratoProveedor(input.ProveedorId);
            if (ContratoProveedorId > 0)
            {
                if ( consumo!=null && consumo.Count > 0)
                {
                    string generar = _LiquidacionServicio.GenerarLiquidacionConsumo(ContratoProveedorId, input, consumo);
                    return Content(generar);
                }
                if (viandas!=null && viandas.Count > 0)
                {
                    string generar = _LiquidacionServicio.GenerarLiquidacionVianda(ContratoProveedorId, input, viandas);
                    return Content(generar);
                }
                return Content("OK");
            }
            else
            {
                return Content("NO_CONTRATO");
            }
        }


        // GET: Proveedor/LiquidacionServicio/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Proveedor/LiquidacionServicio/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Proveedor/LiquidacionServicio/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Proveedor/LiquidacionServicio/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult ObtenerListadoLiquidaciones()
        {
            var lista = _LiquidacionServicio.ListadoLiquidaciones();
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
        public ActionResult ObtenerDetallesLiquidacion(int id)
        {
            var liquidacion = _LiquidacionServicio.GetDetalles(id);
            if (liquidacion != null)
            {
                var result = JsonConvert.SerializeObject(liquidacion);
                return Content(result);
            }
            else
            {
                return Content("Error");
            }
        }

        public ActionResult ObtenerServiciosLiquidadosProveedor(InputLiquidacionDto input)
        {
            var lista = _LiquidacionServicio.ListaReservasPendientesdeLiquidacion(input);
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

        public ActionResult ObtenerServicioPendientesLiquidadosProveedor(InputLiquidacionDto input)
        {
            var lista = _LiquidacionServicio.ListaReservasPendientesdeLiquidacion(input);
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

        public ActionResult ObtenerProveedores()
        {


            var lista = _ProveedorServicio.ListProveedorLiquidacionServicios();
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

        public ActionResult ObtenerReservasPendientesLiquidacion(InputLiquidacionDto input)
        {


            var lista = _LiquidacionServicio.ListaReservasPendientesdeLiquidacion(input);
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

        public ActionResult ObtenerReservasLiquidadas(InputLiquidacionDto input)
        {


            var lista = _LiquidacionServicio.ListaReservasLiquidadas(input);
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

        public ActionResult ObtenerAgregarLiquidacionHospedaje(int id, List<FormatLiquidacionReserva> lista)
        {
            if (id == 0) return Content("ERROR");
            var msg = _LiquidacionServicio.AgregarLiquidacionHospedaje(id, lista);
            return Content(msg);

        }

        public ActionResult ObtenerRemoveLiquidacionesHospedaje(int id, List<FormatLiquidacionReserva> lista)
        {
            if (id == 0) return Content("ERROR");
            var msg = _LiquidacionServicio.RemoverLiquidacionHospedaje(id, lista);
            return Content(msg);

        }



        public ActionResult ObtenerConsumosPendientesLiquidacion(InputLiquidacionDto input)
        {


            var lista = _LiquidacionServicio.ListaConsumosPendientesdeLiquidacion(input);
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

        public ActionResult ObtenerConsumosLiquidadas(InputLiquidacionDto input)
        {


            var lista = _LiquidacionServicio.ListaConsumosLiquidadas(input);
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


        public ActionResult ObtenerAgregarLiquidacionConsumo(int id, List<FormatLiquidacionConsumo> lista)
        {
            if (id == 0) return Content("ERROR");
            var msg = _LiquidacionServicio.AgregarLiquidacionConsumo(id, lista);
            return Content(msg);

        }

        public ActionResult ObtenerRemoveLiquidacionesConsumo(int id, List<FormatLiquidacionConsumo> lista)
        {
            if (id == 0) return Content("ERROR");
            var msg = _LiquidacionServicio.RemoverLiquidacionConsumo(id, lista);
            return Content(msg);

        }


        public ActionResult ObtenerChangePagado(int id)
        {
          
            var msg = _LiquidacionServicio.ChangeEstadoPagadoLiquidacion(id);
            return Content(msg?"OK":"ERROR");

        }

        public ActionResult ObtenerChangeAnulado(int id)
        {

            var msg = _LiquidacionServicio.ChangeEliminado(id);
            return Content(msg ? "OK" : "ERROR");

        }


        public ActionResult ObtenerViandasPendientesLiquidacion(InputLiquidacionDto input)
        {


            var lista = _LiquidacionServicio.ListaSolicitudesViandasPendientesdeLiquidacion(input);
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

        public ActionResult ObtenerViandasLiquidadas(InputLiquidacionDto input)
        {


            var lista = _LiquidacionServicio.ListaSolicitudesViandasLiquidadas(input);
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


        public ActionResult ObtenerAgregarLiquidacionVianda(int id, List<FormatLiquidacionSolicitudVianda> viandas)
        {
            if (id == 0) return Content("ERROR");
            var msg = _LiquidacionServicio.AgregarLiquidacionVianda(id, viandas);
            return Content(msg);

        }

        public ActionResult ObtenerRemoveLiquidacionesVianda(int id, List<FormatLiquidacionSolicitudVianda> viandas)
        {
            if (id == 0) return Content("ERROR");
            var msg = _LiquidacionServicio.RemoverLiquidacionVianda(id, viandas);
            return Content(msg);

        }
        public ActionResult ObtenerRHospedaje(int id)
        {

            ExcelPackage excel = _LiquidacionServicio.ObtenerExcelHospedaje(id);

            string excelName = "Reporte Liquidacion" + DateTime.Now.ToShortDateString();
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
        public ActionResult ObtenerRAlimentacion(int id)
        {

            ExcelPackage excel = _LiquidacionServicio.ObtenerExcelAlimentacion(id);

            string excelName = "Reporte Liquidacion" + DateTime.Now.ToShortDateString();
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
        public ActionResult ObtenerRViandas(int id)
        {

            ExcelPackage excel = _LiquidacionServicio.ObtenerExcelViandas(id);

            string excelName = "LiquidacionViandas" + DateTime.Now.ToShortDateString();
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
