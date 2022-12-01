using Abp.Application.Services.Dto;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.framework;
using com.cpp.calypso.proyecto.aplicacion;
using com.cpp.calypso.proyecto.aplicacion.Dto;
using com.cpp.calypso.proyecto.aplicacion.Interfaces;
using com.cpp.calypso.proyecto.dominio;
using CommonServiceLocator;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using JsonResult = com.cpp.calypso.framework.JsonResult;
namespace com.cpp.calypso.web.Areas.Proveedor.Controllers
{

    public class TipoAccionController :
        BaseSPAController<TipoAccionEmpresa, TipoAccionEmpresaDto, PagedAndFilteredResultRequestDto>
    {
        private readonly ICatalogoAsyncBaseCrudAppService _catalogoService;
        private readonly IEmpresaAsyncBaseCrudAppService _empresaService;
        public TipoAccionController(
            IHandlerExcepciones manejadorExcepciones, 
            IViewService viewService,
            ICatalogoAsyncBaseCrudAppService catalogoService,
            IEmpresaAsyncBaseCrudAppService empresaService,
            ITipoAccionEmpresaAsyncBaseCrudAppService entityService) :
            base(manejadorExcepciones, viewService, entityService)
        {
            _catalogoService = catalogoService;
            _empresaService = empresaService;
            Title = "Gestión de Alimentación- Acción";
            Key = "tipo_accion";
            ComponentJS = "~/Scripts/build/tipoAccion.js";
        }

        public ActionResult SearchByCodeApi(string code)
        {
            var result = _catalogoService.APIObtenerCatalogos(code);
            return new JsonResult
            {
                Data = new { success = true, result },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };

        }
        public ActionResult SearchEmpresasApi()
        {
            var lista = _empresaService.GetEmpresas();
            var result = JsonConvert.SerializeObject(lista);
            return Content(result);
        }
    }
}