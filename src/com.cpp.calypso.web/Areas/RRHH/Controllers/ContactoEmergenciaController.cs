using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.framework;
using com.cpp.calypso.proyecto.aplicacion.Dto;
using com.cpp.calypso.proyecto.aplicacion.Interfaces;
using com.cpp.calypso.proyecto.dominio;
using com.cpp.calypso.web.Areas.Accesos.Controllers;
using Newtonsoft.Json;

namespace com.cpp.calypso.web.Areas.RRHH.Controllers
{
    public class ContactoEmergenciaController : BaseAccesoSpaController<ContactoEmergencia, ContactoEmergenciaDto, PagedAndFilteredResultRequestDto>
    {
        private readonly IContactoEmergenciaAsyncBaseCrudAppService _contactoEmergenciaService;

        public ContactoEmergenciaController(
            IHandlerExcepciones manejadorExcepciones,
            IContactoEmergenciaAsyncBaseCrudAppService contactoEmergenciaService,
            IViewService viewService
            ) : base(manejadorExcepciones, viewService)
        {
            _contactoEmergenciaService = contactoEmergenciaService;
        }



        #region Api


        // ColaboradorId
        public ActionResult GetByColaboradorId(int? id)
        {
            if (!id.HasValue)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var dtos = _contactoEmergenciaService.GetByColaboradorId(id.Value);
            return WrapperResponseGetApi(ModelState, () => dtos);
        }

        #endregion


        public ActionResult CreateContactoEmergenciaApi(ContactoEmergenciaDto contacto)
        {
            var list = _contactoEmergenciaService.CreateContacto(contacto);
            var result = JsonConvert.SerializeObject(list);
            return Content(result);
        }

        public ActionResult DeleteContactoEmergenciaApi(int id)
        {
            var list = _contactoEmergenciaService.EliminarContacto(id);
            var result = JsonConvert.SerializeObject(list);
            return Content(result);
        }
    }
}