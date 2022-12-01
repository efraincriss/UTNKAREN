using Abp.Application.Services.Dto;
using com.cpp.calypso.framework;
using com.cpp.calypso.proyecto.aplicacion.Dto;
using com.cpp.calypso.proyecto.aplicacion.Interfaces;
using com.cpp.calypso.proyecto.dominio;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace com.cpp.calypso.web.Areas.RRHH.Controllers
{
    public class ColaboradoresVisitaController : BaseController
    {
        private readonly IColaboradoresVisitaAsyncBaseCrudAppService _colaboradoresVisitaService;
        private readonly IColaboradoresAsyncBaseCrudAppService _colaboradoresService;
        private readonly IColaboradoresComidaAsyncBaseCrudAppService _comidaService;
        private readonly IColaboradorServicioAsyncBaseCrudAppService _colaboradorServicioService;
        private readonly IColaboradorMovilizacionAsyncBaseCrudAppService _colaboradorMovilizacionService;

        public ColaboradoresVisitaController(
            IHandlerExcepciones manejadorExcepciones,
            IColaboradoresVisitaAsyncBaseCrudAppService colaboradoresVisitaService,
            IColaboradoresAsyncBaseCrudAppService colaboradoresService,
            IColaboradoresComidaAsyncBaseCrudAppService comidaService,
            IColaboradorServicioAsyncBaseCrudAppService colaboradorServicioService,
            IColaboradorMovilizacionAsyncBaseCrudAppService colaboradorMovilizacionService
            ) : base(manejadorExcepciones)
        {
            _colaboradoresVisitaService = colaboradoresVisitaService;
            _colaboradoresService = colaboradoresService;
            _comidaService = comidaService;
            _colaboradorServicioService = colaboradorServicioService;
            _colaboradorMovilizacionService = colaboradorMovilizacionService;
        }



        // GET: RRHH/ColaboradoresVisita
        public ActionResult Index()
        {
            return View();
        }

        [System.Web.Mvc.HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> CreateApiAsync(ColaboradoresVisitaDto c)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var visitas = _colaboradoresVisitaService.GetVisitasPorColaborador(c.ColaboradoresId.Value);
                    if (visitas == null)
                    {
                        await _colaboradoresVisitaService.InsertOrUpdateAsync(c);
                    }
                    else {
                        c.Id = visitas.Id;
                        c.Colaboradores = visitas.Colaboradores;
                        c.CreationTime = visitas.CreationTime;
                        c.CreatorUserId = visitas.CreatorUserId;
                        c.ColaboradoresResponsable = visitas.ColaboradoresResponsable;
                        c.estado = visitas.estado;
                        await _colaboradoresVisitaService.Update(c);
                    }

                    return Content("SI");
                }
                return Content("NO");

            }
            catch
            {
                return View();
            }
        }

        [System.Web.Mvc.HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> EditApiAsync(ColaboradoresVisitaDto c)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ColaboradoresVisitaDto visita = await _colaboradoresVisitaService.Get(new EntityDto<int>(c.Id));
                    c.estado = visita.estado;
                    c.Colaboradores = visita.Colaboradores;
                    c.CreationTime = visita.CreationTime;
                    c.CreatorUserId = visita.CreatorUserId;
                    c.ColaboradoresResponsable = visita.ColaboradoresResponsable;
                    await _colaboradoresVisitaService.Update(c);

                    var validarServicios = _colaboradorServicioService.ValidarEliminacionEInsercionServicio(visita.Colaboradores.Id);
                    return Content("SI");
                }
                return Content("NO");

            }
            catch
            {
                return View();
            }
        }

        public ActionResult DeleteUsuarioExternoApi(int id)
        {
            /* ColaboradoresDto colaborador = await _colaboradoresService.Get(new EntityDto<int>(id));

            ColaboradoresVisitaDto visitas = _colaboradoresVisitaService.GetVisitasPorColaborador(id);
            if (visitas != null)
            {
                visitas.vigente = false;
                visitas.IsDeleted = true;
            }

            await _colaboradoresService.Update(colaborador);
            */
            var ms = _colaboradoresVisitaService.DeleteColaboradorExterno(id);
            return Content(ms);
        }

        public ActionResult GetVisitasPorColaborador(int id)
        {
            var list = _colaboradoresVisitaService.GetVisitasPorColaborador(id);
            var result = JsonConvert.SerializeObject(list);
            return Content(result);
        }

        public ActionResult GetUsuariosExternosApi()
        {
            var usuarios = _colaboradoresVisitaService.GetVisitaUsuariosExternos();
            var result = JsonConvert.SerializeObject(usuarios);
            return Content(result);
        }
        public ActionResult GetSwitch(int id)
        {
            var update = _colaboradoresVisitaService.SwitchEstadoColaboradorExterno(id);
            return Content(update?"OK":"ERROR");
        }

        public ActionResult CreateValidacionCedula(int id)

        {
            var Id = _colaboradoresService.UpdateValidacionCedula(id);
            return Content(Id > 0 ? "OK" : "Error");
        }

    }
}