using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.framework;
using com.cpp.calypso.proyecto.aplicacion.Transporte.Dto;
using com.cpp.calypso.proyecto.dominio.Transporte;

namespace com.cpp.calypso.web.Areas.Transporte.Controllers
{
    public class VehiculoHistoricoController : BaseTransporteSpaController<VehiculoHistorico, VehiculoHistoricoDto, PagedAndFilteredResultRequestDto>
    {
        public VehiculoHistoricoController(
            IHandlerExcepciones manejadorExcepciones, 
            IViewService viewService, 
            IAsyncBaseCrudAppService<VehiculoHistorico, VehiculoHistoricoDto, PagedAndFilteredResultRequestDto, VehiculoHistoricoDto> entityService
            ) : base(manejadorExcepciones, viewService, entityService)
        {
        }
    }
}