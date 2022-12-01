using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using com.cpp.calypso.framework;
using com.cpp.calypso.proyecto.aplicacion.Interfaces;
using com.cpp.calypso.proyecto.dominio;

namespace com.cpp.calypso.web.Areas.Proyecto.Controllers
{
    
    public class TipoCatalogoController : BaseController
    {
        private readonly ITipoCatalogoAsyncBaseCrudAppService _tipoCatalogoService;

        public TipoCatalogoController(
            IHandlerExcepciones manejadorExcepciones,
            ITipoCatalogoAsyncBaseCrudAppService tipoCatalogoService
            ) : base(manejadorExcepciones)
        {
            _tipoCatalogoService = tipoCatalogoService;
        }

        public ActionResult Index()
        {
            var lista = _tipoCatalogoService.ObtenerListaTipoCatalogos();
            return View(lista);
        }

    }
}
