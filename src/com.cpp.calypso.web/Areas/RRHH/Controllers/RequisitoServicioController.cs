using Abp.Application.Services.Dto;
using com.cpp.calypso.framework;
using com.cpp.calypso.proyecto.aplicacion.Dto;
using com.cpp.calypso.proyecto.aplicacion.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace com.cpp.calypso.web.Areas.RRHH.Controllers
{
    public class RequisitoServicioController : BaseController
    {
        private readonly IRequisitoServicioAsyncBaseCrudAppService _requisitosService;

        public RequisitoServicioController(
            IHandlerExcepciones manejadorExcepciones,
            IRequisitoServicioAsyncBaseCrudAppService requisitosService
            ) : base(manejadorExcepciones)
        {
            _requisitosService = requisitosService;
        }
        // GET: RRHH/RequisitoServicio
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            RequisitoServicioDto requisito = new RequisitoServicioDto();
            return View(requisito);
        }

        public ActionResult Edit()
        {
            RequisitoServicioDto requisito = new RequisitoServicioDto();
            return View(requisito);
        }

        [HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> CreateApiAsync(RequisitoServicioDto requisito)
        {
            if (ModelState.IsValid)
            {
                if(requisito.Id > 0)
                {
                    RequisitoServicioDto r = await _requisitosService.Get(new EntityDto<int>(requisito.Id));

                    requisito.vigente = true;
                    requisito.CreationTime = r.CreationTime;
                    requisito.CreatorUserId = r.CreatorUserId;

                    var a = await _requisitosService.Update(requisito);

                    return Content("OK");
                }
                else
                {
                    var existe = _requisitosService.UniqueServicio(requisito.catalogo_servicio_id, requisito.RequisitosId, requisito.Id);
                    if (existe == "NO")
                    {
                        requisito.vigente = true;

                        var a = await _requisitosService.InsertOrUpdateAsync(requisito);

                        return Content("OK");
                    }
                    else if (existe == "UPDATE")
                    {

                        RequisitoServicioDto r = await _requisitosService.Get(new EntityDto<int>(requisito.Id));

                        requisito.vigente = true;
                        requisito.CreationTime = r.CreationTime;
                        requisito.CreatorUserId = r.CreatorUserId;

                        var a = await _requisitosService.Update(requisito);

                        return Content("OK");
                    }
                    else
                    {
                        return Content("SI");
                    }
                }
                
            }
            return Content("NO");
        }

        [System.Web.Mvc.HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> DeleteApiAsync(int id)
        {
            try
            {
                RequisitoServicioDto r = await _requisitosService.Get(new EntityDto<int>(id));
                r.vigente = false;
                r.IsDeleted = true;
                await _requisitosService.Update(r);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }


        [System.Web.Mvc.HttpPost]
        public ActionResult GetRequisitoApi(int Id)
        {
            var requisito = _requisitosService.GetRequisitoServicio(Id);
            var result = JsonConvert.SerializeObject(requisito);
            return Content(result);
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult GetRequisitosApi()
        {
            var list = _requisitosService.GetList();
            var result = JsonConvert.SerializeObject(list);
            return Content(result);
        }
    }
}