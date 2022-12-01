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
    public class AdminRotacionController : BaseController
    {

        private readonly IAdminRotacionAsyncBaseCrudAppService _rotacionesService;

        public AdminRotacionController(
            IHandlerExcepciones manejadorExcepciones,
            IAdminRotacionAsyncBaseCrudAppService rotacionesService
            ) : base(manejadorExcepciones)
        {
            _rotacionesService = rotacionesService;
        }



        // GET: Proyecto/AdminRotacion
        public ActionResult Index()
        {
             return View();
        }

        // GET: Proyecto/AdminRotacion/Create
        public ActionResult Create()
        {
            AdminRotacionDto r = new AdminRotacionDto();
            return View(r);
        }

		// GET: Proyecto/AdminRotacion/Edit/5
		public ActionResult Edit()
		{
			AdminRotacionDto r = new AdminRotacionDto();
			return View(r);
		}

		// POST: Proyecto/AdminRotacion/Create
		[System.Web.Mvc.HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> CreateApiAsync(AdminRotacionDto r)
        {
            try
            {
				r.vigente = true;

				var result = _rotacionesService.UniqueCodigo(r.codigo);
				if (result == "NO")
				{
					await _rotacionesService.Create(r);
					return Content("NO");
				}
				else {
					return Content("SI");
				}
                
            }
            catch
            {
				return View();
			}
        }

		[System.Web.Mvc.HttpPost]
		public async System.Threading.Tasks.Task<ActionResult> EditarApiAsync(AdminRotacionDto r)
		{
			try
			{
				// TODO: Add insert logic here
				AdminRotacionDto rq = await _rotacionesService.Get(new EntityDto<int>(r.Id));
				
				r.vigente = rq.vigente;
				r.codigo = rq.codigo;
                r.CreationTime = rq.CreationTime;
                r.CreatorUserId = rq.CreatorUserId;

				await _rotacionesService.Update(r);

				return RedirectToAction("Index");
			}
			catch
			{
				return View();
			}
		}

		
		[System.Web.Mvc.HttpPost]
		public async System.Threading.Tasks.Task<ActionResult> DeleteApiAsync(int id)
        {
            try
            {
                AdminRotacionDto r = await _rotacionesService.Get(new EntityDto<int>(id));
                r.vigente = false;
                r.IsDeleted = true;
				await _rotacionesService.Update(r);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult GetListadoRotacionesApi()
        {
            var list = _rotacionesService.GetList();
            var result = JsonConvert.SerializeObject(list);
            return Content(result);
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult GetRotacionApi(int Id)
        {
            var rotacion = _rotacionesService.GetRotacion(Id);
            var result = JsonConvert.SerializeObject(rotacion);
            return Content(result);
        }
    }
}
