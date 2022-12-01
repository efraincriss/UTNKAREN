using AutoMapper;
using com.cpp.calypso.framework;
using com.cpp.calypso.proyecto.aplicacion;
using com.cpp.calypso.proyecto.aplicacion.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using com.cpp.calypso.proyecto.dominio;

namespace com.cpp.calypso.web.Areas.Proyecto.Controllers
{
    
    public class DetallePreciarioController : BaseController
    {

        private readonly IDetallePreciarioAsyncBaseCrudAppService detallepreciarioService;
        private readonly IItemAsyncBaseCrudAppService itemService;

        public DetallePreciarioController(IHandlerExcepciones manejadorExcepciones,
            IDetallePreciarioAsyncBaseCrudAppService detallepreciarioService,
            IItemAsyncBaseCrudAppService itemService) :
            base(manejadorExcepciones)
        {
            this.detallepreciarioService = detallepreciarioService;
            this.itemService = itemService;
        }

        // GET: Proyecto/DetallePreciario
        public ActionResult Index()
        {
            return View();
        }

        // GET: Proyecto/DetallePreciario/Details/5
        public ActionResult Details(int? id)
        {
            if (id.HasValue)
            {
                var cuenta = detallepreciarioService.GetDetalles(id.Value);
                if (cuenta != null)
                {
                    return View(cuenta);
                }
            }
            return RedirectToAction("Index", "Empresa");
        }

        // GET: Proyecto/DetallePreciario/Create

#pragma warning disable CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        public async Task<ActionResult> Create(int? id)
#pragma warning restore CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        {
            if (id.HasValue)
            {
                DetallePreciarioDto Nuevo = new DetallePreciarioDto();
                Nuevo.PreciarioId = id.Value;
                List<Item> items= itemService.GetItemsparaOferta();
               Nuevo.ItemsDto = itemService.GetItemsParaOferta();
     
          
                return View(Nuevo);

            }
            else
            {
                return RedirectToAction("Index", "Preciario");
            }
        }

        // POST: Proyecto/DetallePreciario/Create
        [HttpPost]
        public async Task<ActionResult> Create(DetallePreciarioDto preciario)
        {
            try
            {

                preciario.Id = 0;
                if (ModelState.IsValid)
                {



                    DetallePreciarioDto e =
                        detallepreciarioService.comprobarexistenciaitem(preciario.PreciarioId, preciario.ItemId);


                    if (e == null)
                    {
                        if (preciario.precio_unitario <= 0)
                        {
                            var dto = Mapper.Map<DetallePreciarioDto>(preciario);
                            dto.ItemsDto = itemService.GetItemsParaOferta();
                            ViewBag.Msg = "El Precio Unitario no puede ser cero";
                            return View("Create", dto);
                        }
                        else{
                            preciario.vigente = true;
                            var preciarios = await detallepreciarioService.Create(preciario);

                            return RedirectToAction("Details", "Preciario", new { id = preciarios.PreciarioId });

                        }



                    }
                    else
                    {
                        var dto = Mapper.Map<DetallePreciarioDto>(preciario);
                        dto.ItemsDto = itemService.GetItemsParaOferta();
                        ViewBag.Msg = "El Item ya Existe";
                        return View("Create", dto);

                    }


                }
                else
                {

                    var dto = Mapper.Map<DetallePreciarioDto>(preciario);
                 List<Item> items= itemService.GetItemsparaOferta();
                     dto.ItemsDto = itemService.GetItemsParaOferta();
                    return View("Create", dto);

                }


            }
            catch
            {
                return View();
            }
        }

        // GET: Proyecto/DetallePreciario/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (!id.HasValue)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                //var input = new comun.aplicacion.PagedAndFilteredResultRequestDto();
                //input.Filter.FirstOrDefault()
                var depreciario = detallepreciarioService.GetDetalles(id.Value);
            
                if (depreciario == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.NotFound);
                }
                else
                {
                    var iditempadre = itemService.buscaridentificadorpadre(depreciario.Item.item_padre);
                    if (iditempadre != 0)
                    {
                        var item = await itemService.GetDetalle(iditempadre);
                        depreciario.nombreitempadre = item.nombre;
                    }
                 
                    return View(depreciario);
                }
            }
        }

        // POST: Proyecto/DetallePreciario/Edit/5
        [HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> Edit(int id, DetallePreciarioDto depreciario)
        {
            try
            {
                //empresaService.InsertOrUpdateAsync(empresaDto);
                var preciario = await detallepreciarioService.InsertOrUpdateAsync(depreciario);
                return RedirectToAction("Details", "Preciario", new {id = preciario.PreciarioId});
            }
            catch
            {
                return View();
            }
        }



        // GET: Proyecto/Preciario/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Proyecto/Preciario/Delete/5
        [HttpPost]
        public async Task<ActionResult> Delete(int? id)
        {
            try
            {
                if (id.HasValue)
                {

                    var e = detallepreciarioService.GetDetalles(id.Value);
                    e.vigente = false;
                    await detallepreciarioService.Update(e);
                    return RedirectToAction("Details", "Preciario", new {id = e.PreciarioId});
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
