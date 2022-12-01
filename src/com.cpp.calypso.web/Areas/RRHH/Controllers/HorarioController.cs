using Abp.Application.Services.Dto;
using com.cpp.calypso.framework;
using com.cpp.calypso.proyecto.aplicacion;
using com.cpp.calypso.proyecto.aplicacion.Dto;
using com.cpp.calypso.proyecto.aplicacion.Interfaces;
using com.cpp.calypso.proyecto.dominio;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace com.cpp.calypso.web.Areas.RRHH.Controllers
{
	public class HorarioController : BaseController
	{
		private readonly IHorarioAsyncBaseCrudAppService _horariosService;

		public HorarioController(
			IHandlerExcepciones manejadorExcepciones,
			IHorarioAsyncBaseCrudAppService horariosService
			) : base(manejadorExcepciones)
		{
			_horariosService = horariosService;
		}

		// GET: Proyecto/Horario
		public ActionResult Index()
		{
			return View();
		}


		// GET: Proyecto/Horario/Create
		public ActionResult Create()
		{
			HorarioDto r = new HorarioDto();
			return View(r);
		}

		[System.Web.Mvc.HttpPost]
		public async System.Threading.Tasks.Task<ActionResult> CreateApiAsync(HorarioDto r)
		{
			try
			{
				r.hora_inicio = TimeSpan.Parse(r.h_inicio);
				r.hora_fin = TimeSpan.Parse(r.h_fin);
				r.vigente = true;

				var result = _horariosService.UniqueCodigo(r.codigo);
				if (result == "NO")
				{
					await _horariosService.Create(r);
					return Content("NO");
				}
				else
				{
					return Content("SI");
				}
				
			}
			catch
			{
				return View();
			}
		}

		// GET: Proyecto/Horario/Edit
		public ActionResult Edit()
		{
			HorarioDto horario = new HorarioDto();
			return View(horario);
		}

		[System.Web.Mvc.HttpPost]
		public async System.Threading.Tasks.Task<ActionResult> EditApiAsync(HorarioDto r)
		{
			try
			{
				HorarioDto h = await _horariosService.Get(new EntityDto<int>(r.Id));
				
				r.vigente = h.vigente;
				r.hora_inicio = TimeSpan.Parse(r.h_inicio);
				r.hora_fin = TimeSpan.Parse(r.h_fin);

				await _horariosService.Update(r);

				return RedirectToAction("Index");
			}
			catch
			{
				return View();
			}

		}
		[System.Web.Mvc.HttpPost]
		public ActionResult DeleteApiAsync(int id)
		{
			
			var horarioid = _horariosService.EliminarHorario(id);
			if (horarioid > 0)
			{
				return Content("OK");
			}
			else {
				return Content("Error");
			}

		}


		[System.Web.Mvc.HttpPost]
		public ActionResult GetHorariosApi()
		{
			var list = _horariosService.GetList();
			var result = JsonConvert.SerializeObject(list);
			return Content(result);
		}

		[System.Web.Mvc.HttpPost]
		public ActionResult GetHorarioApi(int Id)
		{
			var horario = _horariosService.GetHorario(Id);
			var result = JsonConvert.SerializeObject(horario);
			return Content(result);
		}

	}
}
