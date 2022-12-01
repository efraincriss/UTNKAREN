using com.cpp.calypso.framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Mvc;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.proyecto.aplicacion;
using com.cpp.calypso.web.Areas.Proyecto.Models;

namespace com.cpp.calypso.web.Areas.Proyecto.Controllers
{
    
    public class CuentaEmpresaController : BaseController
    {
        private readonly ICuentaEmpresaAsyncBaseCrudAppService _cuentaEmpresaService;
        private readonly IInstitucionFinancieraAsyncBaseCrudAppService _institucionFinancieraService;

        public CuentaEmpresaController(
            IHandlerExcepciones manejadorExcepciones,
            ICuentaEmpresaAsyncBaseCrudAppService cuentaEmpresaService,
            IInstitucionFinancieraAsyncBaseCrudAppService institucionFinancieraService
            ) : base(manejadorExcepciones)
        {
            _cuentaEmpresaService = cuentaEmpresaService;
            _institucionFinancieraService = institucionFinancieraService;
        }

        public ActionResult Create(int? id)
        {
            if (id.HasValue)
            {
                CuentaEmpresaDto cuenta = new CuentaEmpresaDto();
                cuenta.EmpresaId = id.Value;
                var viewModel = new CreateCuentaEmpresaViewModel
                {
                    InstitucionesFinancieras = _institucionFinancieraService.GetInstitucionesFinancieras(),
                    CuentaEmpresaDto = cuenta
                };
                
                return View(viewModel);
            }
            else
            {
                return RedirectToAction("Index", "Empresa");
            }
        }

        [System.Web.Mvc.HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> Create(CreateCuentaEmpresaViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                viewModel.CuentaEmpresaDto.vigente = true;
                var cuentas = await _cuentaEmpresaService.Create(viewModel.CuentaEmpresaDto);
                return RedirectToAction("Details", "Empresa", new { id = viewModel.CuentaEmpresaDto.EmpresaId });
            }

            viewModel.InstitucionesFinancieras = _institucionFinancieraService.GetInstitucionesFinancieras();
            return View("Create", viewModel);
        }


        [System.Web.Mvc.HttpPost]
        public ActionResult Delete(int? id)
        {
            if (id.HasValue)
            {
                int empresaId = _cuentaEmpresaService.EliminarVigencia(id.Value);
                return RedirectToAction("Details", "Empresa", new { id = empresaId });
            }
            return RedirectToAction("Index", "Empresa");
        }

        public ActionResult Details(int? id)
        {
            if (id.HasValue)
            {
                var cuenta = _cuentaEmpresaService.GetDetalles(id.Value);
                if (cuenta != null)
                {
                    return View(cuenta);
                }
            }
            return RedirectToAction("Index", "Empresa");
        }

        public ActionResult Edit(int? id)
        {
            if (id.HasValue)
            {
                var cuentaEmpresa = _cuentaEmpresaService.GetDetalles(id.Value);
                if (cuentaEmpresa != null)
                {
                    var viewModel = new CreateCuentaEmpresaViewModel
                    {
                        InstitucionesFinancieras = _institucionFinancieraService.GetInstitucionesFinancieras(),
                        CuentaEmpresaDto = cuentaEmpresa
                    };

                    return View(viewModel);
                }
            }
            return RedirectToAction("Index", "Empresa"); 
        }

        [System.Web.Mvc.HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> Edit(CreateCuentaEmpresaViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                await _cuentaEmpresaService.Update(viewModel.CuentaEmpresaDto);
                return RedirectToAction("Details", "Empresa", new { id = viewModel.CuentaEmpresaDto.EmpresaId });
            }
            else
            {
                return View("Edit", viewModel);
            }

        }
    }
}
