using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.framework;
using com.cpp.calypso.proyecto.aplicacion.Documentos.Dto;
using com.cpp.calypso.proyecto.dominio.Documentos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace com.cpp.calypso.web.Areas.Documentos.Controllers
{
    public class ConsultaContratoController : BaseAccesoSpaController<Documento, DocumentoDto, PagedAndFilteredResultRequestDto>
    {
        public ConsultaContratoController(IHandlerExcepciones manejadorExcepciones, IViewService viewService) : base(manejadorExcepciones, viewService)
        {
        }

        public ActionResult Index()
        {
            var model = new FormReactModelView();
            // model.Id = "listado_contratos_container";
            // model.ReactComponent = "~/Scripts/build/listado_contratos_container.js";
             model.Id = "consultas_contrato_container";
            model.ReactComponent = "~/Scripts/build/consultas_contrato_container.js";
            if (!string.IsNullOrEmpty(Title))
            {
                model.Title = Title;
            }


            ViewBag.ruta = new string[] { "Inicio", "Consulta de Contratos", "Listado de Contratos" };

            return View(model);
        }

        public ActionResult Consultar()
        {
            var model = new FormReactModelView();
            model.Id = "content";
            model.ReactComponent = "~/Scripts/build/consultas_contrato_container.js";

            if (!string.IsNullOrEmpty(Title))
            {
                model.Title = Title;
            }


            ViewBag.ruta = new string[] { "Inicio", "Consulta de Contratos", "Listado de Contratos", "Consultar" };

            return View(model);
        }
     
    }
}