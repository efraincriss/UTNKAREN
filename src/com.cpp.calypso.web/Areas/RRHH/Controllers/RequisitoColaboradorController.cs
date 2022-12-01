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
    public class RequisitoColaboradorController : BaseController
    {
        private readonly IRequisitoColaboradorAsyncBaseCrudAppService _requisitoService;
        private readonly ICatalogoAsyncBaseCrudAppService _catalogoservice;
        private readonly IZonaFrenteAsyncBaseCrudAppService _zonafrenteservice;
        private readonly IRequisitoFrenteAsyncBaseCrudAppService _requisitofrenteservice;

        public RequisitoColaboradorController(
            IHandlerExcepciones manejadorExcepciones,
            IRequisitoColaboradorAsyncBaseCrudAppService requisitoService,
            ICatalogoAsyncBaseCrudAppService catalogoservice,
            IZonaFrenteAsyncBaseCrudAppService zonafrenteservice,
            IRequisitoFrenteAsyncBaseCrudAppService requisitofrenteservice
            ) : base(manejadorExcepciones)
        {
            _requisitoService = requisitoService;
            _catalogoservice = catalogoservice;
            _zonafrenteservice = zonafrenteservice;
            _requisitofrenteservice = requisitofrenteservice;
        }

		// GET: Proyecto/Requisito
		public ActionResult Index()
		{
			return View();
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
        public ActionResult GetRequisitosApi()
        {
            var list = _requisitoService.GetList();
            var result = JsonConvert.SerializeObject(list,
                Newtonsoft.Json.Formatting.None,
                new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    NullValueHandling = NullValueHandling.Ignore
                });
            //var result = JsonConvert.SerializeObject(list);
            return Content(result);
        }


        // GET: Proyecto/RequisitoColaborador/Create
        public ActionResult Create()
        {
            RequisitoColaboradorDto requisito = new RequisitoColaboradorDto();
            return View(requisito);
        }

        // GET: Proyecto/RequisitoColaborador/Edit
        public ActionResult Edit()
        {
            RequisitoColaboradorDto requisito = new RequisitoColaboradorDto();
            return View(requisito);
        }


        // POST: Proyecto/RequisitoColaborador/Create
        [System.Web.Mvc.HttpPost]

        public async System.Threading.Tasks.Task<ActionResult> CreateApiAsync(RequisitoColaboradorDto r)
        {
            RequisitoColaboradorDto a = new RequisitoColaboradorDto();

            if(r.Id> 0)
            {
                RequisitoColaboradorDto req = await _requisitoService.Get(new EntityDto<int>(r.Id));
                r.vigente = true;
                r.CreationTime = req.CreationTime;
                r.CreatorUserId = req.CreatorUserId;
                r.Requisitos = req.Requisitos;


                a = await _requisitoService.Update(r);

                return Content("OK");
            }
            else
            {
                var existe = _requisitoService.UniqueRequisito(r.rolId, r.tipo_usuarioId, r.RequisitosId, r.Id);
                if (existe == "NO")
                {
                    r.vigente = true;

                    a = await _requisitoService.InsertOrUpdateAsync(r);

                    return Content("OK");
                }
                else if (existe == "UPDATE")
                {
                    r.vigente = true;

                    a = await _requisitoService.Update(r);

                    return Content("OK");
                }
                else
                {
                    return Content("SI");
                }
            }
        }


		[System.Web.Mvc.HttpPost]
		public async System.Threading.Tasks.Task<ActionResult> DeleteApiAsync(int id)
        {
            RequisitoColaboradorDto r = await _requisitoService.Get(new EntityDto<int>(id));
            r.vigente = false;
            r.IsDeleted = true;
            await _requisitoService.Update(r);

            return RedirectToAction("Index");
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult GetRequisitoApi(int Id)
        {
            var list = _requisitoService.GetRequisito(Id);
            var result = JsonConvert.SerializeObject(list);
            return Content(result);
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult GetFrentesApi()
        {
            var list = _zonafrenteservice.GetFrentes();
            var result = JsonConvert.SerializeObject(list);
            return Content(result);
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult GetFrentesRequisitoApi(int Id)
        {
            var list = _requisitofrenteservice.GetFrentesPorRequisito(Id);
            var result = JsonConvert.SerializeObject(list);
            return Content(result);
        }

		[System.Web.Mvc.HttpPost]
		public ActionResult GuardarRequisitoSrvice(RequisitoColaboradorDto requisito,   int [] idsfrestes)
		{
			var a = requisito;

			foreach (var item in idsfrestes)
			{
				a.Id=item; //idfrente

				var result = _requisitoService.Create(a);
			}

			return Content("Ok");
		}

        public ActionResult GetFiltrosRequisitosApi(int? tipo_usuario, int? accion, string requisitos)
        {
            if (ModelState.IsValid) { }
            var list = _requisitoService.GetRequisitosPorFiltros(tipo_usuario, accion, requisitos);
            var result = JsonConvert.SerializeObject(list,
                Newtonsoft.Json.Formatting.None,
                new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    NullValueHandling = NullValueHandling.Ignore
                });
            //var result = JsonConvert.SerializeObject(list);
            return Content(result);
        }

    }
}
