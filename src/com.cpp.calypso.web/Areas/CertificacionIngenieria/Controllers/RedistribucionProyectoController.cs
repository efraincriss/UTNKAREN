using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.framework;
using com.cpp.calypso.proyecto.aplicacion.CertificacionIngenieria.Dto;
using com.cpp.calypso.proyecto.aplicacion.CertificacionIngenieria.Interface;
using com.cpp.calypso.proyecto.aplicacion.CertificacionIngenieria.Models;
using com.cpp.calypso.proyecto.aplicacion.Interfaces;
using com.cpp.calypso.proyecto.dominio.CertificacionIngenieria;
using com.cpp.calypso.web.Areas.Accesos.Controllers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JsonResult = com.cpp.calypso.framework.JsonResult;

namespace com.cpp.calypso.web.Areas.CertificacionIngenieria.Controllers
{


    public class RedistribucionProyectoController : BaseAccesoSpaController<DetalleDirectoE500, DetalleDirectoE500Dto, PagedAndFilteredResultRequestDto>
    {
        private readonly ICatalogoAsyncBaseCrudAppService _catalogoService;
        private readonly IDetalleDirectoE500AsyncBaseCrudAppService _E500Service;

        public RedistribucionProyectoController(
            IHandlerExcepciones manejadorExcepciones,
            IViewService viewService,
             ICatalogoAsyncBaseCrudAppService catalogoService,
             IDetalleDirectoE500AsyncBaseCrudAppService E500Service

        ) : base(manejadorExcepciones, viewService)
        {
            _catalogoService = catalogoService;
            _E500Service = E500Service;
        }

        // GET: CertificacionIngenieria/Certificado
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult APIProyectos()
        {
            var data = _E500Service.ObtenerProyectos();
            var result = JsonConvert.SerializeObject(data,
                Newtonsoft.Json.Formatting.None,
                new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    NullValueHandling = NullValueHandling.Ignore
                });
            return Content(result);
        }

        [HttpGet]
        public ActionResult ObtenerDirectosPorProyecto(int id)
        {
            var list = _E500Service.ObtenerDetallesDirectosProyecto(id);
            return WrapperResponseGetApi(ModelState, () => list);
        }

        [HttpGet]
        public ActionResult ObtenerDirectosE500()
        {
            var list = _E500Service.ObtenerDetallesDirectosE500();
            return WrapperResponseGetApi(ModelState, () => list);
        }


        [HttpPost]
        public ActionResult CrearE500( int[] Directos)
        {

            var result = _E500Service.EnviaraE500(Directos);
            return Content(result=="OK" ? "OK" : "ERROR");


        }

        [HttpPost]
        public ActionResult CrearDistribucionMasiva(int Id,int[] Directos) //Id ProyectoDestino
        {

            var result = _E500Service.EnviaraDirectosaOtroProyecto(Id,Directos);
            return Content(result == "OK" ? "OK" : "ERROR");


        }


        [HttpPost]
        public ActionResult CrearDistribucion(int Id, List<E500Distribucion> temporales)
        {

            var result = _E500Service.DistribuirHorasDirectasaProyecto(Id,temporales);
            return Content(result == "OK" ? "OK" : "ERROR");


        }


        

    }
}