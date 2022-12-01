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
	public class ServicioDestinoController : BaseController
	{
		private readonly IServicioDestinoAsyncBaseCrudAppService _serviciosService;
		private readonly ICatalogoAsyncBaseCrudAppService _catalogoservice;
		private readonly IServicioDestinoComidaAsyncBaseCrudAppService _comidaService;

		public ServicioDestinoController(
			IHandlerExcepciones manejadorExcepciones,
			IServicioDestinoAsyncBaseCrudAppService serviciosService,
			ICatalogoAsyncBaseCrudAppService catalogoservice,
			IServicioDestinoComidaAsyncBaseCrudAppService comidaService
			) : base(manejadorExcepciones)
		{
			_serviciosService = serviciosService;
			_catalogoservice = catalogoservice;
			_comidaService = comidaService;
		}

		// GET: RRHH/ServicioDestino
		public ActionResult Index()
		{
			return View();
		}

		public ActionResult Create()
		{
			ServicioDestinoDto requisito = new ServicioDestinoDto();
			return View(requisito);
		}

		public ActionResult Edit()
		{
			ServicioDestinoDto requisito = new ServicioDestinoDto();
			return View(requisito);
		}

		[System.Web.Mvc.HttpPost]
		public async System.Threading.Tasks.Task<ActionResult> CreateApiAsync(ServicioDestinoDto r, int[] idComidas)
		{
            var existe = _serviciosService.UniqueServicio(r.destinoId);
            if (existe == "NO")
            {
                r.vigente = true;

                var a = await _serviciosService.Create(r);

                if (idComidas != null && idComidas.Length != 0)
                {

                    ServicioDestinoComidaDto comida = new ServicioDestinoComidaDto();

                    foreach (var item in idComidas)
                    {
                        comida.tipo_comida = item; //comidas
                        comida.ServicioDestinoId = a.Id;
                        comida.vigente = true;

                        await _comidaService.Create(comida);

                    }
                }

                return Content("OK");
            }
            else {
                return Content("SI");
            }

			
		}

		[System.Web.Mvc.HttpPost]
		public async System.Threading.Tasks.Task<ActionResult> EditApiAsync(ServicioDestinoDto r, int[] idComidas)
		{

			ServicioDestinoDto sd = await _serviciosService.Get(new EntityDto<int>(r.Id));

			await _serviciosService.Update(r);

			if (idComidas != null && idComidas.Length != 0)
			{

				ServicioDestinoComidaDto comida = new ServicioDestinoComidaDto();

				var sdc = _comidaService.GetComidas(r.Id);

				foreach (var item in idComidas)
				{
					var foundItem = sdc.FirstOrDefault(e => e.ServicioDestinoId == item);

					if (foundItem == null)
					{
						comida.tipo_comida = item; //comidas
						comida.ServicioDestinoId = r.Id;

						await _comidaService.InsertOrUpdateAsync(comida);
					}
					
				}

				foreach (var item in sdc)
				{
					var foundItem = idComidas.FirstOrDefault(e => e == item.ServicioDestinoId);

					if (foundItem == 0)
					{
						await _comidaService.Delete(item);
					}

				}
			}

			return RedirectToAction("Index");

		}

		[System.Web.Mvc.HttpPost]
		public async System.Threading.Tasks.Task<ActionResult> DeleteApiAsync(int id)
		{

			ServicioDestinoDto r = await _serviciosService.Get(new EntityDto<int>(id));
			r.vigente = false;

			await _serviciosService.Update(r);

			var list = _comidaService.GetComidas(id);

			foreach (var c in list)
			{
				await _comidaService.Delete(c);
			}

			return RedirectToAction("Index");

		}

		[System.Web.Mvc.HttpPost]
		public ActionResult GetCatalogosPorTipoApi(int Id)
		{
			//Obtiene los Catlogos del Tipo Identificacion
			var lista = _catalogoservice.ListarCatalogos(Id);//Revisar ID
			var result = JsonConvert.SerializeObject(lista);
			return Content(result);
		}

		[System.Web.Mvc.HttpPost]
		public ActionResult GetServicioApi(int Id)
		{
			var requisito = _serviciosService.GetServicio(Id);
			var result = JsonConvert.SerializeObject(requisito);
			return Content(result);
		}

		[System.Web.Mvc.HttpPost]
		public ActionResult GetServiciosApi()
		{
			var list = _serviciosService.GetList();
			var result = JsonConvert.SerializeObject(list);
			return Content(result);
		}

		[System.Web.Mvc.HttpPost]
		public ActionResult GetComidasApi(int Id)
		{
			var list = _comidaService.GetComidas(Id);
			var result = JsonConvert.SerializeObject(list);
			return Content(result);
		}

	}
}