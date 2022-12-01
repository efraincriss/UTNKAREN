using AutoMapper;
using com.cpp.calypso.framework;
using com.cpp.calypso.proyecto.aplicacion;
using com.cpp.calypso.proyecto.aplicacion.Interfaces;
using com.cpp.calypso.proyecto.dominio;
using com.cpp.calypso.web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Abp.Application.Services.Dto;
using com.cpp.calypso.web.Areas.Proyecto.Models;
using Newtonsoft.Json;
using Item = Elmah.ContentSyndication.Item;
using com.cpp.calypso.proyecto.dominio.Models;
using JsonResult = com.cpp.calypso.framework.JsonResult;

namespace com.cpp.calypso.web.Areas.Proyecto.Controllers
{

    public class ItemController : BaseController
    {

        private readonly IItemAsyncBaseCrudAppService itemService;
        private readonly ICatalogoAsyncBaseCrudAppService _catalogosService;
        private readonly IGrupoItemAsyncBaseCrudAppService _grupoItemService;
        List<TreeItem> nodes;
        public ItemController(IHandlerExcepciones manejadorExcepciones, IItemAsyncBaseCrudAppService itemService,
            ICatalogoAsyncBaseCrudAppService catalogosService,
        IGrupoItemAsyncBaseCrudAppService grupoItemService) :
            base(manejadorExcepciones)
        {
            this.itemService = itemService;
            _grupoItemService = grupoItemService;
            nodes = new List<TreeItem>();
            _catalogosService = catalogosService;
        }


        // GET: Proyecto/Item
        public ActionResult Index(string message)
        {
            if (message != null)
            {

                ViewBag.Msg = message;
            }

            var items = itemService.GetItems();

            return View(items);
        }

        public async Task<ActionResult> DetailsDatos(int? id)
        {
            if (!id.HasValue)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {

                var item = await itemService.GetDetalle(id.Value);

                return PartialView("DetailsDatos", item);
            }
        }

        public ActionResult DetailsGrupos()
        {
            var Lista = _grupoItemService.lista();

            var result = JsonConvert.SerializeObject(Lista,
                Newtonsoft.Json.Formatting.None,
                new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });

            return Content(result);
        }

        public ActionResult DetailsGruposEspecialidades(string code)
        {
            var result = _catalogosService.APIObtenerCatalogos(code);
            return new JsonResult
            {
                Data = new { success = true, result },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };

        }
        // GET: Proyecto/Item/Details/5
        public async Task<ActionResult> DetailsHijo(int? id)
        {
            if (!id.HasValue)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {

                var item = await itemService.GetDetalle(id.Value);
                // var items = itemService.GetItemsHijos(item.codigo);
                var items = itemService.GetItemsHijosContenido(item.codigo);
                int idpadre = item.Id;
                if (item == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.NotFound);
                }
                else
                {
                    var ViewModel = new ItemHijosViewModel
                    {
                        PadreId = idpadre,
                        ItemPadre = item,
                        ItemsHijos = items
                    };
                    return View(ViewModel);
                }
            }
        }


        public ActionResult CreatePadre(String Padre)
        {
            ItemDto Nuevo = new ItemDto();
            Nuevo.item_padre = Padre;
            Nuevo.para_oferta = false;
            return View(Nuevo);
        }


        [HttpPost]
        public async Task<ActionResult> CreatePadre(ItemDto Nuevo, String padre)
        {

            if (padre.Equals("."))
            {
                Nuevo.item_padre = ".";
            }

            if (ModelState.IsValid)
            {
                var contador = itemService.GetItemsHijos(padre).Count(); //contador de filas
                string nuevocodigo = "";

                if (padre.Equals("."))
                {
                    contador = itemService.GetItemsHijos(".").Count();

                    if (String.IsNullOrEmpty(Nuevo.codigo))
                    {

                        nuevocodigo = (contador + 1) + ".";
                    }
                    else
                    {
                        nuevocodigo = Nuevo.codigo;
                        if (Nuevo.codigo.Substring(Nuevo.codigo.Length - 1, 1) != ".")
                        {
                            nuevocodigo = Nuevo.codigo + ".";
                        }
                    }


                    if (Nuevo.para_oferta == true)
                    {
                        nuevocodigo = nuevocodigo.TrimEnd('.');
                    }


                    Nuevo.codigo = nuevocodigo;
                    Nuevo.vigente = true;
                    if (itemService.siexisteid(Nuevo.codigo))
                    {
                        ViewBag.Msg = "El codigo del Item ya Existe";
                        var dto = Mapper.Map<ItemDto>(Nuevo);
                        return View("CreatePadre", dto);
                    }

                    var item = await itemService.Create(Nuevo);

                    return RedirectToAction("Index", "Item");


                }


            }
            else
            {

                var dto = Mapper.Map<ItemDto>(Nuevo);
                return View("CreatePadre", dto);

            }

            var dto2 = Mapper.Map<ItemDto>(Nuevo);
            return View("CreatePadre", dto2);
        }

        // GET: Proyecto/Item/Create
        public ActionResult Create(String padre)
        {
            ItemDto Nuevo = new ItemDto();
            Nuevo.item_padre = padre;
            var contador = itemService.GetItemsHijos(padre).Count();
            Nuevo.codigo = (contador + 1) + ".";
            Nuevo.para_oferta = false;
            return View(Nuevo);
        }

        // POST: Proyecto/Item/Create

        [HttpPost]
        public async Task<ActionResult> Create(ItemDto Nuevo, String padre)
        {
            try
            {
                Nuevo.Id = 0;
                if (ModelState.IsValid)
                {
                    string nuevocodigo = "";

                    var contador = itemService.GetItemsHijos(padre).Count(); //contador de filas



                    if (String.IsNullOrEmpty(Nuevo.codigo))
                    {

                        if (contador >= 0)
                        {
                            nuevocodigo = padre + (contador + 1) + ".";
                        }

                    }
                    else
                    {
                        nuevocodigo = Nuevo.codigo;
                        if (Nuevo.codigo.Substring(Nuevo.codigo.Length - 1, 1) != ".")
                        {
                            nuevocodigo = Nuevo.codigo + ".";
                        }

                        nuevocodigo = padre + Nuevo.codigo;
                    }


                    if (Nuevo.para_oferta == true)
                    {
                        nuevocodigo = nuevocodigo.TrimEnd('.');
                    }

                    Nuevo.codigo = nuevocodigo;
                    Nuevo.item_padre = padre;
                    Nuevo.vigente = true;
                    if (itemService.siexisteid(Nuevo.codigo))
                    {
                        ViewBag.Msg = "El codigo del Item  Hijo ya Existe";
                        var dto = Mapper.Map<ItemDto>(Nuevo);
                        return View("Create", dto);
                    }

                    var item = await itemService.Create(Nuevo);
                    string a = item.item_padre.ToString();
                    return RedirectToAction("DetailsHijo", "Item",
                        new { id = itemService.buscaridentificadorpadre(padre), padre = item.item_padre });


                }
                else
                {

                    var dto = Mapper.Map<ItemDto>(Nuevo);
                    return View("Create", dto);

                }
            }
            catch
            {
                return View();
            }
        }

        // GET: Proyecto/Item/Edit/5
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
                var itemdto = await itemService.GetDetalle(id.Value);

                if (itemdto == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.NotFound);
                }
                else
                {
                    return View(itemdto);
                }
            }
        }

        // POST: Proyecto/Item/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(int id, ItemDto itemDto)
        {
            try
            {
                if (itemService.siexisteidEdit(itemDto.codigo, itemDto.Id))
                {
                    ViewBag.Msg = "El codigo del Item ya Existe";
                    var dto = Mapper.Map<ItemDto>(itemDto);
                    return View("Edit", dto);
                }

                var result = await itemService.InsertOrUpdateAsync(itemDto);

                if (itemDto.item_padre == ".")
                {
                    return RedirectToAction("Index", "Item");

                }

                return RedirectToAction("DetailsHijo", "Item",
                    new { id = itemService.buscaridentificadorpadre(itemDto.item_padre), padre = itemDto.item_padre });
            }
            catch
            {
                return View();
            }
        }


        // GET: Proyecto/Item/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }




        // POST: Proyecto/Item/Delete/5
        [HttpPost]
        public async Task<ActionResult> Delete(int? id)
        {
            try
            {
                if (id.HasValue)
                {
                    var item = await itemService.Get(new EntityDto<int>(id.Value));
                    var hijos = itemService.GetItemsHijos(item.codigo).Count;
                    if (hijos > 0)
                    {
                        string Mensaje = "No se puede Eliminar tiene datos relacionados";
                        return RedirectToAction("Index", "Item", new { message = Mensaje });
                    }
                    else
                    {
                        ItemDto e = await itemService.GetDetalle(id.Value);
                        e.vigente = false;
                        await itemService.Update(e);

                        string Mensaje = "";
                        return RedirectToAction("Index", "Item", new { message = Mensaje });

                    }


                }

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }



        [HttpGet]
        public ActionResult Notas()
        {
            List<TreeItem> i = itemService.GenerarArbol();


            var resultado = JsonConvert.SerializeObject(i,
                Newtonsoft.Json.Formatting.None,
                new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });

            ViewBag.resultado = resultado;
            return Content(resultado);
        }

        public ActionResult JsTreeDemo()
        {
            var items = itemService.GetItems();
            return View(items);
        }

        public ActionResult ItemsNewFormat()
        {
            return View();
        }




        [HttpPost]
        public async Task<ActionResult> CrearItem(ItemDto Item)
        {
            if (ModelState.IsValid)
            {
                string c = Item.item_padre + Item.codigo;

                if (itemService.siexisteid(c))
                {
                    return Content("Existe");
                }
                Item.codigo = Item.item_padre + Item.codigo + ".";
                if (Item.para_oferta)
                {
                    Item.codigo = Item.codigo.TrimEnd('.');
                }
                if (itemService.siexisteid(Item.codigo))
                {
                    return Content("Existe");
                }
                if (!itemService.comprobaritemmovimiento(Item.item_padre))
                {
                    return Content("Movimiento");
                }

                var item = await itemService.Create(Item);

                return Content("Guardado");
            }
            return Content("Error");
        }
        [HttpPost]
        public async Task<ActionResult> CrearItemPadre(ItemDto Item)
        {
            if (ModelState.IsValid)
            {
                Item.codigo = Item.codigo.TrimEnd('.');

                Item.codigo = Item.codigo + ".";
                if (itemService.siexisteid(Item.codigo))
                {
                    return Content("Existe");
                }

                var item = await itemService.Create(Item);

                return Content("Guardado");
            }
            return Content("Error");
        }

        public ActionResult DetailsApi(int? id)
        {
            if (id.HasValue)
            {

                /*   var item = await itemService.Get(new EntityDto<int>(id.Value));

                var x = item.codigo.TrimEnd('.').Split('.');
                item.apicodigo = x[x.Length - 1];
                */
                var item = itemService.DetailsAPIItem(id.Value);
                var result = JsonConvert.SerializeObject(item,
                      Newtonsoft.Json.Formatting.None,
                      new JsonSerializerSettings
                      {
                          ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                          NullValueHandling = NullValueHandling.Ignore
                      });
                return Content(result);
            }
            return Content("Error");
        }

        [HttpPost]
        public async Task<ActionResult> EditApi(ItemDto item)
        {
            if (ModelState.IsValid)
            {
             
                if (itemService.siexisteidEdit(item.codigo, item.Id))
                {
                    return Content("EXISTE");
                }
                item.codigo = item.codigo.TrimEnd('.');

                item.codigo = item.item_padre + item.codigo + ".";
                if (item.para_oferta)
                {
                    item.codigo = item.codigo.TrimEnd('.');
                }

                if (itemService.siexisteidEdit(item.codigo, item.Id))
                {
                    return Content("EXISTE");
                }


               var update= await itemService.Update(item);
                if (update != null && update.Id > 0)
                {
                    return Content("GUARDADO");
                }
                else
                {
                    return Content("ERROR");
                }

                
            }
            else
            {
                return Content("ERROR");
            }
            
        }
        [HttpPost]
        public ActionResult DeleteItem(int Id)
        {
            if (ModelState.IsValid)
            {
                var item = itemService.DetailsAPIItem(Id);
                if (item != null && item.Id > 0 && item.tieneHijos == 0)
                {
                   var result = itemService.Eliminar(Id);
                    if (result)
                    {
                        return Content("ELIMINADO");
                    }
                    else {
                        return Content("ErrorEliminado");
                    }
                  
                }
                else {
                    return Content("TIENEHIJOS");
                }
                
            }

            return Content("ErrorEliminado");
        }

        [HttpGet]
        public ActionResult ItemsFormato2()
        {
            List<NodeItem> i = itemService.TreeDataArbol();


            var resultado = JsonConvert.SerializeObject(i,
                Newtonsoft.Json.Formatting.None,
                new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    NullValueHandling = NullValueHandling.Ignore
                });

            return Content(resultado);
        }
    }
}
