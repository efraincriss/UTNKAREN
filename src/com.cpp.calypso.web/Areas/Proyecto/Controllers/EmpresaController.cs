using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.framework;
using com.cpp.calypso.proyecto.aplicacion;
using com.cpp.calypso.proyecto.aplicacion.Interfaces;
using com.cpp.calypso.proyecto.dominio;
using System;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using AutoMapper;
using com.cpp.calypso.web.Areas.Proyecto.Models;
using Newtonsoft.Json;

namespace com.cpp.calypso.web.Areas.Proyecto.Controllers
{  
    public class EmpresaController : BaseController
    {

        private readonly IEmpresaAsyncBaseCrudAppService _empresaService;

        public EmpresaController(
            IHandlerExcepciones manejadorExcepciones,
            IEmpresaAsyncBaseCrudAppService empresaService) :
            base(manejadorExcepciones)
        {
            _empresaService = empresaService;
        }

        
        public ActionResult GetEmpresa() 
        {
            var empresas = _empresaService.GetEmpresasApi();
            var result = JsonConvert.SerializeObject(empresas);
            return Content(result);
        }

        public ActionResult Index(string flag = "")
        {
            var input = new comun.aplicacion.PagedAndFilteredResultRequestDto();
            var empresas = _empresaService.GetEmpresas();
            if (flag != "")
            // String.IsNullOrEmpty(flag)
            //String.Empty => vacio
            {
                ViewBag.Msg = "No se puede eliminar el registro ya que tiene datos relacionados";
            }
            return View(empresas);
        }


        // GET: Proyecto/Empresa/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (!id.HasValue)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                var empresa = await _empresaService.GetDetalle(id.Value);
                var representantesEmpresa = _empresaService.GetRepresentanteEmpresa(id.Value);
                var cuentasEmpresa = _empresaService.GetCuentasEmpresa(id.Value);
                if (empresa == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.NotFound);
                }
                else
                {
                    var viewModel = new EmpresaRepresentanteCuentaViewModel
                    {
                        Empresa = empresa,
                        RepresentnatesEmpresa = representantesEmpresa,
                        CuentasEmpresa = cuentasEmpresa
                    };
                    return View(viewModel);
                }
            }
        }


        // GET: Proyecto/Empresa/Create
        public ActionResult Create()
        {
            Empresa empresa = new Empresa();
            //empresa.observaciones = ".";
            return View(empresa);
        }



        // POST: Proyecto/Empresa/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(EmpresaDto empresa)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var mensaje = await _empresaService.CrearEmpresaAsync(empresa);

                    if (mensaje.Equals("cedula"))
                    {
                        ViewBag.Msg = "Cédula inválida";
                        return View("Create", Mapper.Map<Empresa>(empresa));
                    }
                    else if (mensaje.Equals("ruc"))
                    {
                        ViewBag.Msg = "Ruc inválido";
                        return View("Create", Mapper.Map<Empresa>(empresa));
                    }
                    ViewBag.Msg = "Empresa creada satisfactoriamente";
                    return RedirectToAction("Details", new RouteValueDictionary(
                        new { controller = "Empresa", action = "Details", Id = Int32.Parse(mensaje) }));


                }

            }
            catch (Exception ex)
            {
                var result = ManejadorExcepciones.HandleException(ex); // maneja la exception
                ModelState.AddModelError("", result.Message); // en blanco se pinta en la cabecera, con parametro se pinta en la propiedad
                //new GenericException("Foreing key error","No se puede eliminar la empresa porque tiene hijos relacionados!");                
            }
            return View("Create", Mapper.Map<Empresa>(empresa));
        }

        // GET: Proyecto/Empresa/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (!id.HasValue)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                var empresaDto = await _empresaService.GetDetalle(id.Value);

                if (empresaDto == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.NotFound);
                }
                else
                {
                    return View(empresaDto);
                }
            }
        }


        // POST: Proyecto/Empresa/Edit/5
        [HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> Edit(int id, EmpresaDto empresaDto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var empresa = await _empresaService.InsertOrUpdateAsync(empresaDto);
                    return RedirectToAction("Details", new RouteValueDictionary(
                        new { controller = "Empresa", action = "Details", Id = empresaDto.Id }));
                }
                else
                {
                    return View("Edit", empresaDto);
                }
                //empresaService.InsertOrUpdateAsync(empresaDto);

                //return RedirectToAction("Edit", new RouteValueDictionary(new { controller = "Empresa", action = "Index"}));
            }
            catch (Exception ex)
            {
                var result = ManejadorExcepciones.HandleException(ex); // maneja la exception
                ModelState.AddModelError("", result.Message); // en blanco se pinta en la cabecera, con parametro se pinta en la propiedad
                //new GenericException("Foreing key error","No se puede eliminar la empresa porque tiene hijos relacionados!");                
            }
            return View("Edit", empresaDto);
        }

        // POST: Proyecto/Empresa/Delete/5
        [HttpPost]
        public ActionResult Delete(int? id)
        {
            try
            {
                if (id.HasValue)
                {
                    //var empresaDto = await empresaService.GetDetalle(id.Value);                   
                    //empresaDto.vigente = false;
                    //await empresaService.Update(empresaDto);
                    //empresaService.CancelarVigencia(id.Value);
                    var actualizado = _empresaService.ComprobarYBorrarEmpresa(id.Value);
                    if (actualizado)
                    {
                        return RedirectToAction("Index");
                    }
                    return RedirectToAction("Index", new { flag = "errorSave" });
                    // flag = Enviar tod el mensaje compreto
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                var result = ManejadorExcepciones.HandleException(ex); // maneja la exception
                ModelState.AddModelError("", result.Message); // en blanco se pinta en la cabecera, con parametro se pinta en la propiedad
                //new GenericException("Foreing key error","No se puede eliminar la empresa porque tiene hijos relacionados!");                
            }
            return RedirectToAction("Index");
        }
    }
}

