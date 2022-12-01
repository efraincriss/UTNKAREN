using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using AutoMapper;
using com.cpp.calypso.framework;
using com.cpp.calypso.proyecto.aplicacion;
using com.cpp.calypso.proyecto.aplicacion.Interfaces;
using com.cpp.calypso.proyecto.dominio;
using Newtonsoft.Json;

namespace com.cpp.calypso.web.Areas.Proyecto.Controllers
{
       public class ClienteController : BaseController
    {

        private readonly IClienteAsyncBaseCrudAppService _clienteService;

        public ClienteController(IHandlerExcepciones manejadorExcepciones,
            IClienteAsyncBaseCrudAppService _clienteService) : base(manejadorExcepciones)
        {
            this._clienteService = _clienteService;

        }

        // GET: Proyecto/Cliente
        public ActionResult Index(String message)
        {
            var cliente = _clienteService.GetClientes();
            if (message != null)
            {

                ViewBag.Msg = message;
            }
            return View(cliente);
        }

        // GET: Proyecto/Cliente/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id.HasValue)
            {
                var cliente = await _clienteService.GetDetalle(id.Value);
                if (cliente != null)
                {
                    return View(cliente);
                }
            }
            return RedirectToAction("Index", "Cliente");
        }

        public ActionResult GetClientesApi()
        {
            var clientes = _clienteService.GetClientes();
            var result = JsonConvert.SerializeObject(clientes);
            return Content(result);
        }

        // GET: Proyecto/Cliente/Create
        public ActionResult Create()
        {
            
                ClienteDto cliente = new ClienteDto();
                cliente.estado = true;
                cliente.fecha_registro = DateTime.Now;
                    return View(cliente);
     
        }

        // POST: Proyecto/CentroCostosContrato/Create
        [HttpPost]
        public async Task<ActionResult> Create(ClienteDto cliente)
        {
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        var mensaje = await _clienteService.CrearClienteAsync(cliente);

                        if (mensaje.Equals("unica"))
                        {
                            ViewBag.Msg = "Ya existe un registro con ese número de Identificación";
                            return View("Create", Mapper.Map<ClienteDto>(cliente));
                        }

                        if (mensaje.Equals("cedula"))
                        {
                            ViewBag.Msg = "Cédula inválida";
                            return View("Create", Mapper.Map<ClienteDto>(cliente));
                        }
                        else if (mensaje.Equals("ruc"))
                        {
                            ViewBag.Msg = "Ruc inválido";
                            return View("Create", Mapper.Map<ClienteDto>(cliente));
                        }
                        ViewBag.Msg = "Cliente creada satisfactoriamente";
                        return RedirectToAction("Details", new RouteValueDictionary(
                            new { controller = "Cliente", action = "Details", Id = Int32.Parse(mensaje) }));


                    }

                }
                catch (Exception ex)
                {
                    var result = ManejadorExcepciones.HandleException(ex); // maneja la exception
                    ModelState.AddModelError("", result.Message); // en blanco se pinta en la cabecera, con parametro se pinta en la propiedad
                    //new GenericException("Foreing key error","No se puede eliminar la empresa porque tiene hijos relacionados!");                
                }
                return View("Create", Mapper.Map<ClienteDto>(cliente));
            }
        }

        // GET: Proyecto/Cliente/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (!id.HasValue)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                var clientedto = await _clienteService.GetDetalle(id.Value);

                if (clientedto == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.NotFound);
                }
                else
                {
                    return View(clientedto);
                }
            }
        }


        // POST: Proyecto/Empresa/Edit/5
        [HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> Edit(int id, ClienteDto cliente)
        {
            
                try
                {
                    if (ModelState.IsValid)
                    {
                        var mensaje = await _clienteService.ActualizarClienteAsync(cliente);
                       
                    if (mensaje.Equals("cedula"))
                        {
                            ViewBag.Msg = "Cédula inválida";
                            return View("Edit", Mapper.Map<ClienteDto>(cliente));
                        }
                        else if (mensaje.Equals("ruc"))
                        {
                            ViewBag.Msg = "Ruc inválido";
                            return View("Edit", Mapper.Map<ClienteDto>(cliente));
                        } else if (mensaje.Equals("unica"))
                        {
                            ViewBag.Msg = "Ya existe un registro con ese número de Identificación";
                            return View("Edit", Mapper.Map<ClienteDto>(cliente));
                        }

                    ViewBag.Msg = "Cliente Actualizado satisfactoriamente";
                        return RedirectToAction("Details", new RouteValueDictionary(
                            new { controller = "Cliente", action = "Details", Id = Int32.Parse(mensaje) }));


                    }

                }
                catch (Exception ex)
                {
                    var result = ManejadorExcepciones.HandleException(ex); // maneja la exception
                    ModelState.AddModelError("", result.Message); // en blanco se pinta en la cabecera, con parametro se pinta en la propiedad
                    //new GenericException("Foreing key error","No se puede eliminar la empresa porque tiene hijos relacionados!");                
                }
                return View("Edit", Mapper.Map<ClienteDto>(cliente));
            
        }


        // GET: Proyecto/Cliente/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Proyecto/Cliente/Delete/5
        [HttpPost]
#pragma warning disable CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
       public async Task<ActionResult> Delete(int? id)
#pragma warning restore CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        {
            try
            {
                if (id.HasValue)
                {
                    var hijos =_clienteService.GetContratosporCliente(id.Value).Count;
                    if (hijos > 0)
                    {
                        string Mensaje = "No se puede eliminar el registro, tiene contratos relacionados";
                        return RedirectToAction("Index", "Cliente", new { message = Mensaje });
                    }
                    else
                    {
                        var r = _clienteService.EliminarCliente(id.Value);
                        return RedirectToAction("Index");

                    }


                }

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

    }
}
