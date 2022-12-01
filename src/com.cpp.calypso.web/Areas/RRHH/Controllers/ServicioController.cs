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
    public class ServicioController : BaseController
    {
		private readonly IServicioAsyncBaseCrudAppService _serviciosService;

		public ServicioController(
			IHandlerExcepciones manejadorExcepciones,
			IServicioAsyncBaseCrudAppService serviciosService
			) : base(manejadorExcepciones)
		{
			_serviciosService = serviciosService;
		}
		// GET: RRHH/Servicio
		public ActionResult Index()
        {
            return View();
        }

		[System.Web.Mvc.HttpPost]
		public async System.Threading.Tasks.Task<ActionResult> CreateApiAsync(ServicioDto servicio)
		{
			if (ModelState.IsValid)
			{
				if (servicio.Id != 0)
				{
					await _serviciosService.Update(servicio);
					return Content("NO");
				}
				else {
					var result = _serviciosService.UniqueCodigo(servicio.codigo);
					if (result == "NO")
					{
						await _serviciosService.Create(servicio);
						return Content("NO");
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
				ServicioDto s = await _serviciosService.Get(new EntityDto<int>(id));
				
				await _serviciosService.Delete(s);

				return Content("OK");
			}
			catch
			{
				return View();
			}
		}

		[System.Web.Mvc.HttpPost]
		public ActionResult GetServicioApi(int Id)
		{
			var requisito = _serviciosService.GetServicio(Id);
			var result = JsonConvert.SerializeObject(requisito);
			return Content(result);
		}

		[System.Web.Mvc.HttpPost]
		public ActionResult GetListaServiciosApi()
		{
			var list = _serviciosService.GetList();
			var result = JsonConvert.SerializeObject(list);
			return Content(result);
		}
	}
}