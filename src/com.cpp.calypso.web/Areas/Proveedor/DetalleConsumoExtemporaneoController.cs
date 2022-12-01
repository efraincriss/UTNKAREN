using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Abp.Application.Services.Dto;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.framework;
using com.cpp.calypso.proyecto.aplicacion.Proveedor.Dto;
using com.cpp.calypso.proyecto.aplicacion.Proveedor.Interfaces;
using com.cpp.calypso.proyecto.dominio.Proveedor;
using JsonResult = com.cpp.calypso.framework.JsonResult;

namespace com.cpp.calypso.web.Areas.Proveedor
{
    public class DetalleConsumoExtemporaneoController : BaseSPAController<DetalleConsumoExtemporaneo, DetalleConsumoExtemporaneoDto, PagedAndFilteredResultRequestDto>
    {
        private readonly IDetalleConsumoExtemporaneoAsyncBaseCrudAppService _detalleConsumoExtemporaneoService;

        public DetalleConsumoExtemporaneoController(
            IHandlerExcepciones manejadorExcepciones, 
            IViewService viewService, 
            IAsyncBaseCrudAppService<DetalleConsumoExtemporaneo, DetalleConsumoExtemporaneoDto, PagedAndFilteredResultRequestDto, DetalleConsumoExtemporaneoDto> entityService,
            IDetalleConsumoExtemporaneoAsyncBaseCrudAppService detalleConsumoExtemporaneoService
            ) : base(manejadorExcepciones, viewService, entityService)
        {
            _detalleConsumoExtemporaneoService = detalleConsumoExtemporaneoService;
        }

        public ActionResult Index()
        {
            return View();
        }


        #region Api

        public ActionResult BuscarDetallesPorCabecera(int consumoExtemporaneoId)
        {
            var dtos = _detalleConsumoExtemporaneoService.BuscarDetallesPorCabecera(consumoExtemporaneoId);
            return new JsonResult
            {
                Data = new { success = true, result = dtos }
            };
        }

        public ActionResult BuscarColaborador(string identificacion = "", string nombres = "")
        {
            var dtos = _detalleConsumoExtemporaneoService.BuscarPorIdentificacionNombre(identificacion, nombres);
            return new JsonResult
            {
                Data = new { success = true, result = dtos }
            };
        }

        [HttpPost]
        public ActionResult CrearDetalleConsumo(DetalleConsumoExtemporaneoDto detalle)
        {
            if (ModelState.IsValid)
            {
                if (!_detalleConsumoExtemporaneoService.VerificarDobleConsumo(detalle.ColaboradorId, detalle.ConsumoExtemporaneoId))
                {
                    return new JsonResult
                    {
                        Data = new { success = true, created = false, result = "Ya existe un consumo para el colaborador seleccionado" }
                    };
                }
                var res = _detalleConsumoExtemporaneoService.CrearDetalleConsumo(detalle);
                return new JsonResult
                {
                    Data = new { success = true, created = true, result = res }
                };
            }
            return new JsonResult
            {
                Data = new { success = true, created = false, result = "Error en registrar el consumo" }
            };
        }

        [HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> EliminarDetalleConsumoExtemporaneoAsync(int detalleConsumo)
        {
            await _detalleConsumoExtemporaneoService.Delete(new EntityDto<int>(detalleConsumo));

            return new JsonResult
            {
                Data = new { success = true, deleted = true, result = "OK" }
            }; 
        }

        #endregion
    }
}