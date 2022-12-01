using com.cpp.calypso.framework;
using com.cpp.calypso.proyecto.aplicacion.Dto;
using com.cpp.calypso.proyecto.aplicacion.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace com.cpp.calypso.web.Areas.RRHH.Controllers
{
    public class ColaboradorRequisitoController : BaseController
    {
        private readonly IColaboradorRequisitoAsyncBaseCrudAppService _colaboradorRequisitoService;
        private readonly IRequisitoColaboradorAsyncBaseCrudAppService _requisitoColaboradorService;
        private readonly IColaboradorRequisitoHistoricoAsyncBaseCrudAppService _historicoRequisitoColaboradorService;

        public ColaboradorRequisitoController(
            IHandlerExcepciones manejadorExcepciones,
            IColaboradorRequisitoAsyncBaseCrudAppService colaboradorRequisitoService,
            IRequisitoColaboradorAsyncBaseCrudAppService requisitoColaboradorService,
            IColaboradorRequisitoHistoricoAsyncBaseCrudAppService historicoRequisitoColaboradorService
            ) : base(manejadorExcepciones)
        {
            _colaboradorRequisitoService = colaboradorRequisitoService;
            _requisitoColaboradorService = requisitoColaboradorService;
            _historicoRequisitoColaboradorService = historicoRequisitoColaboradorService;
        }


        // GET: RRHH/ColaboradorRequisito
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        [System.Web.Mvc.HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> CreateApi([FromBody] int? id, int? archivo ,int idColaborador, int idRequisito, DateTime? fecha_caducidad, DateTime? fecha_emision, bool cumple, int accion, HttpPostedFileBase[] UploadedFile)
        {

            ColaboradorRequisitoDto requisito = new ColaboradorRequisitoDto();
            requisito.ColaboradoresId = idColaborador;
            requisito.RequisitosId = idRequisito;
            requisito.fecha_caducidad = fecha_caducidad;
            requisito.fecha_emision = fecha_emision;
            requisito.cumple = cumple;
            requisito.ArchivoId = archivo;
            /*requisito.catalogo_accion_id = accion;*/

            if (id > 0)
            {
                requisito.Id = id.Value;
                if (UploadedFile != null)
                {
                    if(archivo == null)
                    {
                        var archivoId = await _colaboradorRequisitoService.CargarArchivoRequisito(requisito, UploadedFile);
                        requisito.ArchivoId = archivoId;
                    }
                    else
                    {
                        requisito.ArchivoId = archivo;
                        var resultado = await _colaboradorRequisitoService.ActualizaArchivoRequisito(requisito, UploadedFile);
                        requisito.Archivo = resultado;
                    }
                }

                var requisitoSaved = await _colaboradorRequisitoService.Update(requisito);
                ColaboradorRequisitoHistoricoDto historico = new ColaboradorRequisitoHistoricoDto();
                historico = requisitoSaved;
                await _historicoRequisitoColaboradorService.InsertOrUpdateAsync(historico);
                return Content("OK");
            }
            else {
                if (UploadedFile != null)
                {
                    var resultado = await _colaboradorRequisitoService.CargarArchivoRequisito(requisito, UploadedFile);
                    requisito.ArchivoId = resultado;
                }

                var requisitoSaved = await _colaboradorRequisitoService.InsertOrUpdateAsync(requisito);
                ColaboradorRequisitoHistoricoDto historico = new ColaboradorRequisitoHistoricoDto();
                historico = requisitoSaved;
                await _historicoRequisitoColaboradorService.InsertOrUpdateAsync(historico);
                return Content("OK");
            }

        }





        [System.Web.Mvc.HttpPost]
        public ActionResult GetRequisitosApi(int Id)
        {
            var list = _colaboradorRequisitoService.GetList(Id);
            var result = JsonConvert.SerializeObject(list);
            return Content(result);
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult GetRequisitoApi(int Id)
        {
            var rotacion = _colaboradorRequisitoService.GetRequisito(Id);
            var result = JsonConvert.SerializeObject(rotacion);
            return Content(result);
        }

        [System.Web.Mvc.HttpPost]
#pragma warning disable CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        public async System.Threading.Tasks.Task<ActionResult> GetRequisitosPorAccionApi(int idAccion, int idGrupoPersonal)
#pragma warning restore CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        {
            var list = _requisitoColaboradorService.GetRequisitosPorAccionApi(idAccion, idGrupoPersonal);
            var result = JsonConvert.SerializeObject(list);
            return Content(result);
        }
    }
}