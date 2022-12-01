using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Abp.Application.Services.Dto;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.framework;
using com.cpp.calypso.proyecto.aplicacion.Interfaces;
using com.cpp.calypso.proyecto.aplicacion.Proveedor.Dto;
using com.cpp.calypso.proyecto.aplicacion.Proveedor.Interfaces;
using com.cpp.calypso.proyecto.dominio.Proveedor;
using JsonResult = com.cpp.calypso.framework.JsonResult;

namespace com.cpp.calypso.web.Areas.Proveedor
{
    public class ConsumoExtemporaneoController : BaseSPAController<ConsumoExtemporaneo, ConsumoExtemporaneoDto, PagedAndFilteredResultRequestDto>
    {
        private readonly IConsumoExtemporaneoAsyncBaseCrudAppService _consumoExtemporaneo;
        private readonly IArchivoAsyncBaseCrudAppService _archivoService;

        public ConsumoExtemporaneoController(
            IHandlerExcepciones manejadorExcepciones, 
            IViewService viewService, 
            IAsyncBaseCrudAppService<ConsumoExtemporaneo, ConsumoExtemporaneoDto, PagedAndFilteredResultRequestDto, ConsumoExtemporaneoDto> entityService,
            IConsumoExtemporaneoAsyncBaseCrudAppService consumoExtemporaneo,
            IArchivoAsyncBaseCrudAppService archivoService
            ) : base(manejadorExcepciones, viewService, entityService)
        {
            _consumoExtemporaneo = consumoExtemporaneo;
            _archivoService = archivoService;
            Title = "Consumos Extemporaneos - Listado";
            Key = "consumos_extemporaneos_container";
            ComponentJS = "~/Scripts/build/consumos_extemporaneos_container.js";
        }

        public ActionResult Inicio()
        {
            ViewBag.ruta = new string[] { "Inicio", "Operación Alimentación", "Consumos Extemporaneos" };

            var model = new TreeReactModelView();
            model.Id = Key;
            model.ReactComponent = ComponentJS;
            model.Title = Title;

            return View(model );
        }


        #region Api

        public ActionResult ObtenerConsumosExtemporaneos()
        {

            var dtos = _consumoExtemporaneo.ObtenerTodos();
            
            return new JsonResult
            {
                Data = new { success = true, result = dtos }
            };

            //return WrapperResponseGetApi(ModelState, () => list);
        }

        public ActionResult ObtenerProveedoresAlimentacion()
        {
            var dtos = _consumoExtemporaneo.ObtenerProveedoresAlimentacion();
            return new JsonResult
            {
                Data = new { success = true, result = dtos }
            };
        }

        public ActionResult ObtenerTiposComida(int proveedorId)
        {
            var dtos = _consumoExtemporaneo.ObtenerTiposComida(proveedorId);
            return new JsonResult
            {
                Data = new { success = true, result = dtos }
            };
        }

        public async Task<ActionResult> CrearConsumoExtemporaneo(ConsumoExtemporaneoDto consumo)
        {
            if (ModelState.IsValid)
            {
                if (!_consumoExtemporaneo.ValidarRepetidos(consumo.ProveedorId, consumo.Fecha))
                {
                    return new JsonResult
                    {
                        Data = new { success = true, created = false, result = "Ya existe un consumo registrado por el proveedor en esta fecha" }
                    };
                }
                var archivo = Request.GenerateFileFromRequest("uploadFile");
                if (archivo != null)
                {
                    var created = await _archivoService.Create(archivo);
                    consumo.DocumentoRespaldoId = created.Id;
                }

                var consumoCreated = await _consumoExtemporaneo.Create(consumo);
                return new JsonResult
                {
                    Data = new { success = true, created = true, result = consumoCreated }
                };
            }
            return new JsonResult
            {
                Data = new { success = false, created = false }
            };
        }

        public async Task<ActionResult> ActualizarConsumoExtemporaneo(ConsumoExtemporaneoDto consumo)
        {
            if (ModelState.IsValid)
            {
                var archivo = Request.GenerateFileFromRequest("uploadFile");
                if (archivo != null)
                {
                    /*if (consumo.DocumentoRespaldoId != null)
                    {
                        await _archivoService.Delete(new EntityDto<int>(consumo.DocumentoRespaldoId.Value));
                    }*/
                    var created = await _archivoService.Create(archivo);
                    consumo.DocumentoRespaldoId = created.Id;
                }
                var consumoUpdated = await _consumoExtemporaneo.Update(consumo);
                return new JsonResult
                {
                    Data = new { success = true, updated = true, result = consumoUpdated }
                };
            }
            return new JsonResult
            {
                Data = new { success = false, updated = false }
            };
        }


        #endregion
    }
}