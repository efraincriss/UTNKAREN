using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.framework;
using com.cpp.calypso.proyecto.aplicacion;
using com.cpp.calypso.proyecto.dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
#pragma warning disable CS0105 // The using directive for 'com.cpp.calypso.proyecto.aplicacion' appeared previously in this namespace
using com.cpp.calypso.proyecto.aplicacion;
#pragma warning restore CS0105 // The using directive for 'com.cpp.calypso.proyecto.aplicacion' appeared previously in this namespace

namespace com.cpp.calypso.web.Areas.Proyecto.Controllers
{
    
    public class RepresentanteEmpresaController : BaseController
    {
        private readonly IRepresentanteEmpresaAsyncBaseCrudAppService representanteEmpresaService;

        public RepresentanteEmpresaController(
             IHandlerExcepciones manejadorExcepciones,
             IRepresentanteEmpresaAsyncBaseCrudAppService representanteEmpresaService) :
             base(manejadorExcepciones)
        {
            this.representanteEmpresaService = representanteEmpresaService;
        }

        public ActionResult Create(int? id)
        {
            if (id.HasValue)
            {
                RepresentanteEmpresaDto representanteEmpresa = new RepresentanteEmpresaDto()
                {
                    EmpresaId = id.Value,
                    estado_representante = true,
                    fecha_inicio = DateTime.Now,
                    fecha_fin = DateTime.Now
                };

                return View(representanteEmpresa);
            }
            else
            {
                return RedirectToAction("Index", "Empresa");
            }
            
        }

        [HttpPost]
#pragma warning disable CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        public async System.Threading.Tasks.Task<ActionResult> Create(RepresentanteEmpresaDto representanteEmpresa)
#pragma warning restore CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        {
            if (ModelState.IsValid)
            {

                var mensaje = representanteEmpresaService.CrearRepresentante(representanteEmpresa);

                if (mensaje.Equals("cedula"))
                {
                    ViewBag.Msg = "Cédula inválida";
                    return View("Create", representanteEmpresa);
                }
                else if (mensaje.Equals("ruc"))
                {
                    ViewBag.Msg = "Ruc inválido";
                    return View("Create", representanteEmpresa);
                }
                ViewBag.Msg = "Empresa creada satisfactoriamente";
                return RedirectToAction("Details", "Empresa", new { id = Int32.Parse(mensaje) }); 
            }

            return View("Create", representanteEmpresa);
        }

        public ActionResult Edit (int? id)
        {
            if (id.HasValue)
            {
                var representanteEmpresa = representanteEmpresaService.GetDetalles(id.Value);
                if (representanteEmpresa != null)
                {
                    return View(representanteEmpresa);
                }
            }
            return RedirectToAction("Index", "Empresa");
        }

        [HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> Edit (RepresentanteEmpresaDto representante)
        {
            if (ModelState.IsValid)
            {
                await representanteEmpresaService.Update(representante);
                return RedirectToAction("Details", "Empresa", new {id = representante.EmpresaId});
            }
            else
            {
                return View("Edit", representante);
            }
           
        }

        public ActionResult Details (int? id)
        {
            if (id.HasValue)
            {
                var representante = representanteEmpresaService.GetDetalles(id.Value);
                if (representante != null)
                {
                    return View(representante);
                }
            }
            return RedirectToAction("Index", "Empresa");
        }


        [HttpPost]
        public ActionResult Delete(int? id)
        {
            if (id.HasValue)
            {
                int empresaId = representanteEmpresaService.EliminarVigencia(id.Value);
                return RedirectToAction("Details", "Empresa", new {id = empresaId});
            }
            return RedirectToAction("Index", "Empresa");
        }


    }
}
