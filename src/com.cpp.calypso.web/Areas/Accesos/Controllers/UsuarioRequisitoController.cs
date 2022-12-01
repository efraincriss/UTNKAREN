using com.cpp.calypso.framework;
using com.cpp.calypso.proyecto.aplicacion.Acceso.Dto;
using com.cpp.calypso.proyecto.aplicacion.Acceso.Interface;
using com.cpp.calypso.proyecto.aplicacion.Interfaces;
using com.cpp.calypso.proyecto.dominio;
using com.cpp.calypso.proyecto.dominio.Constantes;
using com.cpp.calypso.proyecto.dominio.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using JsonResult = com.cpp.calypso.framework.JsonResult;

namespace com.cpp.calypso.web.Areas.Accesos.Controllers
{
    public class UsuarioRequisitoController : BaseController
    {
        private readonly IValidacionRequisitoAsyncBaseCrudAppService _service;
        private readonly ICatalogoAsyncBaseCrudAppService _catalgoService;
        public UsuarioRequisitoController
            (
                IHandlerExcepciones manejadorExcepciones,
                IValidacionRequisitoAsyncBaseCrudAppService service,
                ICatalogoAsyncBaseCrudAppService catalgoService
            ) : base(manejadorExcepciones)
        {

            _service = service;
            _catalgoService = catalgoService;
        }

        // GET: Accesos/UsuarioRequisito
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ObtenerReponsableRequisito()
        {
            var list = _catalgoService.APIObtenerCatalogos(CatalogosCodigos.RESPONSABLEREQUISITO);
            var result = JsonConvert.SerializeObject(list);
            return Content(result);
        }
        public ActionResult ObtenerAsignaciones(int colaboradorId)
        {
            var list = _service.ListaAsignados(colaboradorId);
            var result = JsonConvert.SerializeObject(list);
            return Content(result);
        }
        public ActionResult ObtenerCatalogosAsignados(int ColaboradorId)
        {
            var list = _service.SearchAsignacionesUsuario(ColaboradorId);
            var result = JsonConvert.SerializeObject(list);
            return Content(result);
        }
       /* public ActionResult ObtenerColaboradores(string search, int catalogoResponsableId)
        {
            var list = _service.SearchUsuario(search, catalogoResponsableId);
            var result = JsonConvert.SerializeObject(list);
            return Content(result);
        }*/
        public ActionResult ObtenerColaborador(string search)
        {
            var list = _service.buscarColaborador(search);
            var result = JsonConvert.SerializeObject(list);
            return Content(result);
        }
        [HttpPost]
        public async Task<JsonResult> CreateApi(int CatalogoResponsableId, int ColaboradorId,bool read, bool write)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string result = "";
                    if (read) {
                     result = _service.Asignar(CatalogoResponsableId,ColaboradorId,"R");
                    }
                    if (write) {
                        result = _service.Asignar(CatalogoResponsableId, ColaboradorId, "M");
                    }
                    if (result == "OK")
                    {                    
                        return new JsonResult
                        {
                            Data = new { success = true, result }
                        };
                    } else
                    {                 
                        return new JsonResult
                        {
                            Data = new { success = false, validation = false, result }
                        };
                    }


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

        [HttpPost]
        public ActionResult ActualizaryCrear(ModelAsiganciones m)
        {
            var result = _service.ActualizaryCrear(m);
             return Content(result);
        }

    }
}
