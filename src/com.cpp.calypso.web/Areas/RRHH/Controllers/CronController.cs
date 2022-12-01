using Abp.Application.Services.Dto;
using com.cpp.calypso.framework;
using com.cpp.calypso.proyecto.aplicacion.Dto;
using com.cpp.calypso.proyecto.aplicacion.Interfaces;
using com.cpp.calypso.proyecto.dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace com.cpp.calypso.web.Areas.RRHH.Controllers
{
    [AllowAnonymous]
    public class CronController : BaseController
    {
        private readonly IColaboradoresAsyncBaseCrudAppService _colaboradoresService;
        private readonly IColaboradorBajaAsyncBaseCrudAppService _bajaService;
        public CronController(
            IHandlerExcepciones manejadorExcepciones,
            IColaboradoresAsyncBaseCrudAppService colaboradoresService,
            IColaboradorBajaAsyncBaseCrudAppService bajaService
            ) : base(manejadorExcepciones)
        {
            _colaboradoresService = colaboradoresService;
            _bajaService = bajaService;
        }
        // GET: RRHH/Cron
        public ActionResult Index()
        {
            return View();
        }

        //[HttpPost]
        public async Task<ActionResult> GetExcelApi()
        {
            var colaboradores = _colaboradoresService.GetColaboradoresInfoCompleta();
            if (colaboradores.Count > 0)
            {
                var excel =await _colaboradoresService.GenerarExcelCarga(colaboradores, false);
                if (excel == "OK")
                {
                    foreach (var e in colaboradores)
                    {
                        ColaboradoresDto c = await _colaboradoresService.Get(new EntityDto<int>(e.Id));
                        c.estado = "ENVIADO SAP";

                        await _colaboradoresService.Update(c);
                    }

                    return Content("OK");
                }
            }
            
            return Content("No existen datos");
            
        }

        //[System.Web.Mvc.HttpPost]
        public async Task<ActionResult> GetExcelBajasApi()
        {
            var colaboradores = _bajaService.GetBajasGenerarArchivo(BajaEstado.ENVIAR_SAP);
            if (colaboradores.Count > 0)
            {
                var excel = await _bajaService.GenerarExcelBajas(colaboradores, false);
                if (excel == "OK")
                {
                    foreach (var e in colaboradores)
                    {
                        ColaboradorBajaDto c = await _bajaService.Get(new EntityDto<int>(e.Id));
                        c.estado = proyecto.dominio.BajaEstado.ENVIADO_SAP;
                        c.fecha_generacion_archivo_sap = DateTime.Now;
                        c.envio_manual = false;

                        await _bajaService.Update(c);
                    }

                    return Content("OK");
                }
            }

            return Content("No existen datos");
        }
    }
}