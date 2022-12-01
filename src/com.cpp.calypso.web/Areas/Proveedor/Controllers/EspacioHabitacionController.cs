using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.framework;
using com.cpp.calypso.proyecto.aplicacion.Proveedor.Dto;
using com.cpp.calypso.proyecto.aplicacion.Proveedor.Interfaces;
using com.cpp.calypso.proyecto.dominio.Proveedor;
using Exception = System.Exception;
using JsonResult = com.cpp.calypso.framework.JsonResult;

namespace com.cpp.calypso.web.Areas.Proveedor.Controllers
{
    public class EspacioHabitacionController : BaseSPAController<EspacioHabitacion, EspacioHabitacionDto, PagedAndFilteredResultRequestDto>
    {
        public EspacioHabitacionController(
            IHandlerExcepciones manejadorExcepciones, 
            IViewService viewService, 
            IEspacioHabitacionAsyncBaseCrudAppService entityService
            ) : base(manejadorExcepciones, viewService, entityService)
        {
        }






        #region Api

        // Id Proveedor
        public ActionResult GetEspaciosHabitacionesPorProveedor(int? id)
        {
            if (!id.HasValue)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var espacioHabitacionService = EntityService as IEspacioHabitacionAsyncBaseCrudAppService;
            var espacios = espacioHabitacionService.GetEspaciosHabitacionPorProveedore(id.Value);
            return WrapperResponseGetApi(ModelState, () => espacios);
        }

        // Espacio Id
        public ActionResult ActivarDesactivarEspacio(int? id)
        {
            if (!id.HasValue)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var espacioHabitacionService = EntityService as IEspacioHabitacionAsyncBaseCrudAppService;
            var result = espacioHabitacionService.ActivarDesactivarEspacio(id.Value);

            if (result)
            {
                return new JsonResult
                {
                    Data = new { success = true }
                };
            }
            return new JsonResult
            {
                Data = new { success = false, error = "No se puede habilitar un espacio de una habitación inactiva" }
            };

        }

        // Habitacion Id
        public JsonResult EspaciosLibresConDatos(int id, DateTime fecha)
        {
            var espacioService = EntityService as IEspacioHabitacionAsyncBaseCrudAppService;
            var espacios = espacioService.EspaciosLibresConDatos(id, fecha);
            return WrapperResponseGetApi(ModelState, () => espacios);
        }

        #endregion 
    }
}