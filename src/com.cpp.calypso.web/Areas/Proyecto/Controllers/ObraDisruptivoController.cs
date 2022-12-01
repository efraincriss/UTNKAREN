using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Abp.Application.Services.Dto;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.framework;
using com.cpp.calypso.proyecto.aplicacion;
using Newtonsoft.Json;

namespace com.cpp.calypso.web.Areas.Proyecto.Controllers
{
   
    public class ObraDisruptivoController : BaseController
    {
        private readonly IObraDisruptivoAsyncBaseCrudAppService _obraDisruptivoService;

        public ObraDisruptivoController(
            IHandlerExcepciones manejadorExcepciones,
            IObraDisruptivoAsyncBaseCrudAppService obraDisruptivoService
            ) : base(manejadorExcepciones)
        {
            _obraDisruptivoService = obraDisruptivoService;
        }


        [HttpPost]
        // Api para obtener el listado de las obras disruptivas de un avance de obra
        public ActionResult IndexApi(int id) // AvanceObra Id
        {
            var list =  _obraDisruptivoService.listar(id);

            var result = JsonConvert.SerializeObject(list);
            return Content(result);
        }

        [HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> DetailsApi(int id) // ObraDisruptivo Id
        {
            var disruptivo = await  _obraDisruptivoService.Get(new EntityDto<int>(id));
            var result = JsonConvert.SerializeObject(disruptivo);
            return Content(result);
        }

        [HttpPost]
        public ActionResult DeleteApi(int id) // ObraDisruptivo Id
        {
            var disruptivoId = _obraDisruptivoService.EliminarVigencia(id);
            return Content(disruptivoId+"");
        }

        [HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> CreateApi(ObraDisruptivoDto disruptivo) 
        {
            if (ModelState.IsValid)
            {
                var a = disruptivo.hora_inicio.ToString();
                if (disruptivo.hora_inicio.ToString().Equals("00:00:00") && disruptivo.hora_fin.ToString().Equals("00:00:00"))
                {
                    disruptivo.numero_horas = 10;
                }
                var disruptivoNew = await _obraDisruptivoService.InsertOrUpdateAsync(disruptivo);
                return Content((disruptivoNew.Id > 0) ? "Ok" : "Error");
            }
            return Content("Error");
        }

        [HttpPost]
        public ActionResult GetImproductividadCatalogoApi() //CatalogoId
        {
            var catalogos = _obraDisruptivoService.getCatalogosImproductividad();
            var result = JsonConvert.SerializeObject(catalogos);
            return Content(result);
        }

        [HttpPost]
        public ActionResult GetTipoRecursoApi()
        {
            var catalogos = _obraDisruptivoService.getCatalogosRecursos();
            var result = JsonConvert.SerializeObject(catalogos);
            return Content(result);
        }


    }
}
