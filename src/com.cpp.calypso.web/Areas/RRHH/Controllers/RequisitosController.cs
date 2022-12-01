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
   
    public class RequisitosController : BaseController
    {
        private readonly IRequisitosAsyncBaseCrudAppService _requisitosService;
        private readonly ICatalogoAsyncBaseCrudAppService _catalogoservice;

        public RequisitosController(
            IHandlerExcepciones manejadorExcepciones,
            IRequisitosAsyncBaseCrudAppService requisitosService,
            ICatalogoAsyncBaseCrudAppService catalogoservice
            ) : base(manejadorExcepciones)
        {
            _requisitosService = requisitosService;
            _catalogoservice = catalogoservice;
        }

        // GET: Proyecto/Requisitos
        public ActionResult Index()
        {
            return View();
        }

        // GET: Proyecto/Requisitos/Details/5
        public async System.Threading.Tasks.Task<ActionResult> Details(int id)
        {

            RequisitosDto requisito = await _requisitosService.Get(new EntityDto<int>(id));

            var tipo_requisito = _catalogoservice.GetCatalogo(requisito.requisitoId);
            requisito.nombre_requisito = tipo_requisito.nombre;
            if (requisito.caducidad == true)
            {
                requisito.nombre_caducidad = "SI";
            }
            else
            {
                requisito.nombre_caducidad = "NO";
            }

            return View(requisito);
        }

        public ActionResult Create()
        {
			RequisitosDto requisito = new RequisitosDto();
			return View(requisito);
		}

		[System.Web.Mvc.HttpPost]
		public async System.Threading.Tasks.Task<ActionResult> CreateApiAsync(RequisitosDto requisito)
		{
			if (ModelState.IsValid) {
				requisito.vigente = true;
                requisito.activo = true;
                requisito.codigo = _requisitosService.nextcode();

                var result = _requisitosService.UniqueCodigo(requisito.codigo);
                if (result == "NO")
                {
                    await _requisitosService.Create(requisito);
                    return Content("NO");
                }
                else
                {
                    return Content("SI");
                }
			}
			return View();
        }

		public ActionResult Edit()
        {
            RequisitosDto requisito = new RequisitosDto();
			return View(requisito);
        }

		[System.Web.Mvc.HttpPost]
		public async System.Threading.Tasks.Task<ActionResult> EditApiAsync(RequisitosDto requisito)
        {
            try
            {
                RequisitosDto r = await _requisitosService.Get(new EntityDto<int>(requisito.Id));
				
                requisito.vigente = r.vigente;
                requisito.codigo = r.codigo;
                requisito.CreationTime = r.CreationTime;
                requisito.CreatorUserId = r.CreatorUserId;
                
				await _requisitosService.Update(requisito);

                if(requisito.activo != r.activo)
                {
                    _requisitosService.ActualizaActivo(requisito.Id, requisito.activo);
                }

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
                RequisitosDto r = await _requisitosService.Get(new EntityDto<int>(id));
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
		public ActionResult GetCatalogosPorTipoApi(int Id)
        {
            //Obtiene los Catlogos del Tipo Identificacion
            var lista = _catalogoservice.ListarCatalogos(Id);//Revisar ID
            var result = JsonConvert.SerializeObject(lista);
            return Content(result);
        }

		[System.Web.Mvc.HttpPost]
		public ActionResult GetRequisitoApi(int Id)
		{
			var requisito = _requisitosService.GetRequisito(Id);
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
