using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.proyecto.dominio;
using com.cpp.calypso.web.Areas.Accesos.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Results;
using System.Web.Mvc;
using com.cpp.calypso.framework;
using com.cpp.calypso.proyecto.aplicacion.Dto;
using com.cpp.calypso.proyecto.aplicacion.Interfaces;
using JsonResult = com.cpp.calypso.framework.JsonResult;

namespace com.cpp.calypso.web.Areas.Proyecto.Controllers
{
    public class OrdenServicioTempController : BaseAccesoSpaController<OrdenServicioTemp, OrdenServicioTempDto, PagedAndFilteredResultRequestDto>
    {
        private readonly IOrdenServicioTempAsyncBaseCrudAppService _ordenServicioService;

        public OrdenServicioTempController(
            IHandlerExcepciones manejadorExcepciones, 
            IViewService viewService,
            IOrdenServicioTempAsyncBaseCrudAppService ordenServicioService
            ) : base(manejadorExcepciones, viewService)
        {
            _ordenServicioService = ordenServicioService;
        }


        public async Task<JsonResult> Index()
        {
            var result = await _ordenServicioService.MigrarOrdenesServicio();
            return new JsonResult
            {
                Data = new { success = true, result = result }
            };
        }
    }
}