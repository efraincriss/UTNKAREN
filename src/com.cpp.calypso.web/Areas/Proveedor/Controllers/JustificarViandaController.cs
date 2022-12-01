using Abp.Application.Services.Dto;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.framework;
using com.cpp.calypso.proyecto.aplicacion.Dto;
using com.cpp.calypso.proyecto.aplicacion.Interfaces;
using com.cpp.calypso.proyecto.dominio;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using JsonResult = com.cpp.calypso.framework.JsonResult;

namespace com.cpp.calypso.web.Areas.Proveedor.Controllers
{
    public class JustificarViandaController : BaseSPAController<JustificacionVianda, JustificacionViandaDto, PagedAndFilteredResultRequestDto>
    {
        public ISolicitudViandaAsyncBaseCrudAppService SolicitudViandaService { get; }
        public IApplication Application { get; }

        public JustificarViandaController(
            IHandlerExcepciones manejadorExcepciones, 
            IViewService viewService,
            IJustificacionViandaAsyncBaseCrudAppService entityService,
            ISolicitudViandaAsyncBaseCrudAppService solicitudViandaService,
            IApplication application
            ) : base(manejadorExcepciones, viewService, entityService)
        {
            Title = "Justificaciones - Alimentación";
            Key = "justificacion_viandas";
            ComponentJS = "~/Scripts/build/justificarViandas.js";
            SolicitudViandaService = solicitudViandaService;
            Application = application;
        }

      

        #region API

        public async Task<ActionResult> GetSolicitudDiariaApi(DateTime? fecha)
        {
            var incluir = new List<SolicitudViandaEstado>();
            incluir.Add(SolicitudViandaEstado.EntregadaAnotador);
            incluir.Add(SolicitudViandaEstado.Justificada); 

            var result = await SolicitudViandaService.GetMySolicitud(fecha, incluir);
            var mode = new
            {
                nombreUsuario = Application.GetCurrentUser().Nombres,
                lista = result
            };

            return WrapperResponseGetApi(ModelState, () => mode);
        }

       
        public async virtual Task<JsonResult> CreateNewApi(int solicitudId)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    var entityService = EntityService as IJustificacionViandaAsyncBaseCrudAppService;

                    var result = await entityService.InitNew(solicitudId);
                
                    return new JsonResult
                    {
                        Data = new { success = true, result }
                    };

                }
            }
            catch (Exception ex)
            {
                var result = ManejadorExcepciones.HandleException(ex);
                ModelState.AddModelError("", result.Message);
            }

            return new JsonResult
            {
                Data = new { success = false, errors = ModelState.ToSerializedDictionary() }
            };
        }

        #endregion

    }
}