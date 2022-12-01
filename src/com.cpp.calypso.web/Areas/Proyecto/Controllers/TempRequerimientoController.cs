using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using com.cpp.calypso.framework;
using com.cpp.calypso.proyecto.aplicacion;
using com.cpp.calypso.proyecto.aplicacion.Interfaces;
using com.cpp.calypso.web.Areas.Proyecto.Models;

namespace com.cpp.calypso.web.Areas.Proyecto.Controllers
{

    public class TempRequerimientoController : BaseController
    {
        private readonly ITempRequerimientoAsyncBaseCrudAppService _tempRequerimientoService;
        private readonly IOfertaComercialAsyncBaseCrudAppService _ofertaComercial;

        // GET: Proyecto/TempRequerimiento
        public TempRequerimientoController(
            IHandlerExcepciones manejadorExcepciones,
            ITempRequerimientoAsyncBaseCrudAppService tempRequerimientoService,
            IOfertaComercialAsyncBaseCrudAppService ofertaComercial
            ) : base(manejadorExcepciones)
        {
            _tempRequerimientoService = tempRequerimientoService;
            _ofertaComercial = ofertaComercial;
        }

        public ActionResult Index(string flag = "")
        {
            if (flag != "")
            {
                ViewBag.msg = "Requerimientos migrados con exito!";
            }
            return View();
        }

        public ActionResult CargarRequerimeintos()
        {
         //   _tempRequerimientoService.CargarRequerimientossAsync(0, 0);
            return RedirectToAction("Index", "TempRequerimiento", new { flag = "Migrados" });
        }


        public ActionResult CargarOfertaComercialPresupuesto(int desde, int hasta)
        {
            try
            {

           //     _tempRequerimientoService.CargarTablaRelacion(desde, hasta);
                return Content("OK");
            }
            catch (Exception e)
            {
                return Content(e.Message.ToString());

            }
        }

        public async System.Threading.Tasks.Task<ActionResult> CargarProyectosAsync()
        {
            //await _tempRequerimientoService.CargarProyectoAsync();
            return RedirectToAction("Index", "TempRequerimiento", new { flag = "Migrados" });
        }
        public async System.Threading.Tasks.Task<ActionResult> CargarTransmital(int desde, int hasta)
        {
            try
            {
              //  await _tempRequerimientoService.CargarTransmittalsAsync(desde, hasta);
                return Content("OK");
            }
            catch (Exception e)
            {
                return Content(e.Message.ToString());

            }

        }

        public async System.Threading.Tasks.Task<ActionResult> CargarRequerimientosAsync(int desde, int hasta)
        {
            try
            {
                //await _tempRequerimientoService.CargarRequerimientossAsync(desde, hasta);
                return Content("OK");
            }
            catch (Exception e)
            {
                return Content(e.Message.ToString());

            }

        }

        public ActionResult ActualizarMontosAProbados()
        {
            try
            {
                //_ofertaComercial.ActualizarMontoAprobado();
                return Content("OK");
            }
            catch (Exception e)
            {
                return Content(e.Message.ToString());

            }

        }
        public async System.Threading.Tasks.Task<ActionResult> CargassOsAsync(int desde, int hasta)
        {
            try
            {
                //await _tempRequerimientoService.CargarOsAsync(desde, hasta);
                return Content("OK");
            }
            catch (Exception e)
            {
                return Content(e.Message.ToString());

            }

        }

        public async System.Threading.Tasks.Task<ActionResult> CargaCartasOsAsync(int desde, int hasta)
        {
            try
            {
                //await _tempRequerimientoService.CargarCartas(desde, hasta);
                return Content("OK");
            }
            catch (Exception e)
            {
                return Content(e.Message.ToString());

            }

        }
        public async System.Threading.Tasks.Task<ActionResult> CargarEstado(int desde, int hasta)
        {
            try
            {
                //await _tempRequerimientoService.ActualizarReferenciaAsync(desde, hasta);
                return Content("OK");
            }
            catch (Exception e)
            {
                return Content(e.Message.ToString());

            }

        }

        public async System.Threading.Tasks.Task<ActionResult> CargarActualizaciones(string list)
        {
            try
            {
                //await _tempRequerimientoService.ActualizarMontosRequerimientosAsync(list);
                return Content("OK");
            }
            catch (Exception e)
            {
                return Content(e.Message.ToString());

            }

        }

        public ActionResult ActualizarFechasUltimoEnvio()
        {
            try
            {
        //        _tempRequerimientoService.ActualizarFechasOfertasComerciales();
                return Content("OK");
            }
            catch (Exception e)
            {
                return Content(e.Message.ToString());

            }

        }
        public ActionResult ActualizarClaseRequerimiento()
        {
            try
            {
          //      _tempRequerimientoService.ActualizarClaseRequerimientoOfertaComercial();
                return Content("OK");
            }
            catch (Exception e)
            {
                return Content(e.Message.ToString());

            }


        }
        [HttpPost]
        public ActionResult NPS1(ViewModel model)
        {

            if (ModelState.IsValid)
            {
                var UploadedFile = Request.Files["File"];

                _tempRequerimientoService.ActualizarNombresProyectos(UploadedFile);

            }
            return Content("PROYECTOS ACTUAIZADOS CORRECTAMENTE");

        }

    }
}