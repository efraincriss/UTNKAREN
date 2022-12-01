using com.cpp.calypso.framework;
using com.cpp.calypso.proyecto.aplicacion;
using com.cpp.calypso.proyecto.aplicacion.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace com.cpp.calypso.web.Areas.Proyecto.Controllers
{
    
    public class CentroCostosContratoController : BaseController
    {

        private readonly ICentrocostoContratoAsyncBaseCrudAppService centrocostocontratoService;
        private readonly ICatalogoAsyncBaseCrudAppService _catalogoservice;
        public CentroCostosContratoController(IHandlerExcepciones manejadorExcepciones, 
            ICentrocostoContratoAsyncBaseCrudAppService centrocostocontratoService,
             ICatalogoAsyncBaseCrudAppService catalogoservice) :
             base(manejadorExcepciones)
        {
            this.centrocostocontratoService = centrocostocontratoService;
            _catalogoservice = catalogoservice;
        }

        // GET: Proyecto/CentroCostosContrato
        public async System.Threading.Tasks.Task<ActionResult> Index()
        {
            var input = new comun.aplicacion.PagedAndFilteredResultRequestDto();
            input.MaxResultCount = 50;
            var result = await centrocostocontratoService.GetAll(input);
            return View(result);
        }


        // GET: Proyecto/CentroCostosContrato/Details/5
        public async System.Threading.Tasks.Task<ActionResult> Details(int? id)
        {
            if (id.HasValue)
            {
                var centrocostocontrato = await centrocostocontratoService.GetDetalle(id.Value);
                if (centrocostocontrato != null)
                {
                    return View(centrocostocontrato);
                }
            }
            return RedirectToAction("Index", "Contrato");
        }


        // GET: Proyecto/CentroCostosContrato/Create
        public ActionResult Create(int? id)
        {
            if (id.HasValue)
            {
                CentrocostosContratoDto centrocostos = new CentrocostosContratoDto();
                centrocostos.ContratoId = id.Value;
                centrocostos.estado = true;
                ViewBag.centrocostos = _catalogoservice.ListarCatalogos(1004);
                return View(centrocostos);
            }
            else
            {
                return RedirectToAction("Index", "Contrato");
            }
        }

        // POST: Proyecto/CentroCostosContrato/Create
        [HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> Create(CentrocostosContratoDto centrocostos)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    centrocostos.vigente = true;
                    var adenda_contrato = await centrocostocontratoService.Create(centrocostos);

                    return RedirectToAction("Details", "Contrato", new { id = centrocostos.ContratoId });
                }

                return View("Create", centrocostos);


            }
            catch
            {
                return View();
            }
        }

        // GET: Proyecto/CentroCostosContrato/Edit/5
        public async System.Threading.Tasks.Task<ActionResult> Edit(int? id)
        {
            if (!id.HasValue)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                //var input = new comun.aplicacion.PagedAndFilteredResultRequestDto();
                //input.Filter.FirstOrDefault()
                var centrocostocontratoDto = await centrocostocontratoService.GetDetalle(id.Value);

                if (centrocostocontratoDto == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.NotFound);
                }
                else
                {
                    ViewBag.centrocostos = _catalogoservice.ListarCatalogos(1004);
                    return View(centrocostocontratoDto);
                }
            }
        }

        // POST: Proyecto/CentroCostosContrato/Edit/5
        [HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> Edit(int id, CentrocostosContratoDto CentroCostosContratoDto)
        {
            try
            {
                //empresaService.InsertOrUpdateAsync(empresaDto);
                var contrato = await centrocostocontratoService.InsertOrUpdateAsync(CentroCostosContratoDto);
                return RedirectToAction("Details", "Contrato", new { id = CentroCostosContratoDto.ContratoId });
            }
            catch
            {
                return View();
            }
        }

       
        // POST: Proyecto/CentroCostosContrato/Delete/5
        [HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> Delete(int? id)
        {
            try
            {
                int _id_contrato = 0;
                if (id.HasValue)
                {
                    var CentrocostocontratoDto = await centrocostocontratoService.GetDetalle(id.Value);
                    CentrocostocontratoDto.vigente = false;
                    _id_contrato = CentrocostocontratoDto.ContratoId;
                    await centrocostocontratoService.Update(CentrocostocontratoDto);
                            }
                return RedirectToAction("Details", "Contrato", new { id = _id_contrato });
            }
            catch
            {
                return View();
            }
        }
    }
    }

