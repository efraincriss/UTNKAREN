using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.framework;
using com.cpp.calypso.proyecto.aplicacion.Proveedor.Dto;
using com.cpp.calypso.proyecto.aplicacion.Proveedor.Interfaces;
using com.cpp.calypso.proyecto.dominio.Proveedor;

namespace com.cpp.calypso.web.Areas.Proveedor.Controllers
{
    public class DetalleReservaController : BaseSPAController<DetalleReserva, DetalleReservaDto, PagedAndFilteredResultRequestDto>
    {

        private readonly IReservaHotelAsyncBaseCrudAppService _service;
        // GET: Proveedor/DetalleReserva
        public ActionResult Index()
        {
            return View();
        }

        public DetalleReservaController(
            IHandlerExcepciones manejadorExcepciones, 
            IViewService viewService, 
            IDetalleReservaAsyncBaseCrudAppService entityService,
                       IReservaHotelAsyncBaseCrudAppService service
            ) : base(manejadorExcepciones, viewService, entityService)
        {
            _service = service;
        }



        #region Api

        public ActionResult ListarDetallesPorReserva(int id)
        {
            var detalleReservaService = EntityService as IDetalleReservaAsyncBaseCrudAppService;
            var detalles = detalleReservaService.ListarPorReservaId(id);
            return WrapperResponseGetApi(ModelState, () => detalles);
        }

        #endregion

        public ActionResult CambiarEstadoDetallesExtemporaneoConsumido(List<DetalleReservaDto> data)
        {
            var detalleReservaService = EntityService as IDetalleReservaAsyncBaseCrudAppService;
            var resultado = _service.CambiarEstadoDetallesConsumidoE(data);
            return Content(resultado);
        }
        public ActionResult CambiarEstadoDetallesExtemporaneoNoConsumido(List<DetalleReservaDto> data)
        {
            var detalleReservaService = EntityService as IDetalleReservaAsyncBaseCrudAppService;
            var resultado = _service.CambiarEstadoDetallesNoConsumidoE(data);
            return Content(resultado);
        }

        public ActionResult CambiarEstadoLavanderia(List<DetalleReservaDto> data)
        {
            var detalleReservaService = EntityService as IDetalleReservaAsyncBaseCrudAppService;
            var resultado = _service.CambiarEstadoDetallesLavanderia(data);
            return Content(resultado);
        }
        public ActionResult CambiarEstadoNoLavanderia(List<DetalleReservaDto> data)
        {
            var detalleReservaService = EntityService as IDetalleReservaAsyncBaseCrudAppService;
            var resultado = _service.CambiarEstadoDetallesNoLavanderia(data);
            return Content(resultado);
        }

    }
}