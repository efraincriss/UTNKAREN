using Abp.Application.Services.Dto;
using AutoMapper;
using com.cpp.calypso.framework;
using com.cpp.calypso.proyecto.aplicacion;
using com.cpp.calypso.proyecto.aplicacion.Interfaces;
using com.cpp.calypso.proyecto.dominio;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using Newtonsoft.Json;
using com.cpp.calypso.web.Models;

using System.IO;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Globalization;

namespace com.cpp.calypso.web.Areas.Proyecto.Controllers
{

    public class ComputoController : BaseController
    {
        private readonly IComputoAsyncBaseCrudAppService _computoService;
        private readonly IComputosTemporalAsyncBaseCrudAppService _computosTemporalService;
        private readonly ICatalogoAsyncBaseCrudAppService _catalogoService;
        private readonly IOfertaAsyncBaseCrudAppService _ofertaService;
        private readonly IWbsOfertaAsyncBaseCrudAppService _wbsofertaService;
        private readonly IWbsAsyncBaseCrudAppService _wbsService;
        private readonly IItemAsyncBaseCrudAppService _itemservice;
        private readonly IProyectoAsyncBaseCrudAppService _proyectoservice;
        private readonly WbsOfertaServiceAsyncBaseCrudAppService _metodonombres;
        private readonly PreciarioServiceAsyncBaseCrudAppService _preciarioService;
        private readonly DetallePreciarioServiceAsyncBaseCrudAppService _dpreciarioService;

        private readonly IPresupuestoAsyncBaseCrudAppService _presupuestoService;
        private readonly IWbsPresupuestoAsyncBaseCrudAppService _wbspresupuestoService;

        private readonly IPreciarioAsyncBaseCrudAppService _ServicePreciario;

        public ComputoController(IHandlerExcepciones manejadorExcepciones,
            IComputoAsyncBaseCrudAppService computoService,
            IComputosTemporalAsyncBaseCrudAppService computosTemporalService,
            IOfertaAsyncBaseCrudAppService ofertaService,
            IWbsOfertaAsyncBaseCrudAppService wbsofertaService,
            IItemAsyncBaseCrudAppService itemservice,
            IProyectoAsyncBaseCrudAppService proyectoservice,
            WbsOfertaServiceAsyncBaseCrudAppService metodonombres,
            DetallePreciarioServiceAsyncBaseCrudAppService dpreciarioService,
            PreciarioServiceAsyncBaseCrudAppService preciarioService,
            IWbsAsyncBaseCrudAppService wbsService,
            ICatalogoAsyncBaseCrudAppService catalogoService,
             IPresupuestoAsyncBaseCrudAppService presupuestoService,
             IWbsPresupuestoAsyncBaseCrudAppService wbspresupuestoService,
             IPreciarioAsyncBaseCrudAppService ServicePreciario
        ) : base(manejadorExcepciones)
        {
            this._itemservice = itemservice;
            this._computoService = computoService;
            this._ofertaService = ofertaService;
            this._wbsofertaService = wbsofertaService;
            _proyectoservice = proyectoservice;
            _presupuestoService = presupuestoService;
            _metodonombres = metodonombres;
            _dpreciarioService = dpreciarioService;
            _preciarioService = preciarioService;
            _wbsService = wbsService;
            _catalogoService = catalogoService;
            _computosTemporalService = computosTemporalService;
            _wbspresupuestoService = wbspresupuestoService;
            _ServicePreciario = ServicePreciario;
        }

        public ActionResult ComputoCompleto(int id)
        {
            _ofertaService.ActualizarComputoCompleto(id);
            String mensaje = "Computo Completado";
            ViewBag.Msg = mensaje;
            return RedirectToAction("EstructuraComputos", "Computo", new { id, message = mensaje });
        }


        // GET: Proyecto/Computo
        public ActionResult Index()
        {
            ViewBag.ruta = new string[] { "Inicio", "Seleccion Oferta", "Computo" };
            var input = new comun.aplicacion.PagedAndFilteredResultRequestDto();
            var ofertas = _ofertaService.GetOfertas();

            return View(ofertas);
        }

        // GET: Proyecto/Computo/Details/5
#pragma warning disable CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        public async System.Threading.Tasks.Task<ActionResult> Details(int? id, int pa, int pd, String message,
#pragma warning restore CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
            String message2)
        {
            /*  if (message != null)
              {

                  ViewBag.Error = message;
              }

              if (message2 != null)
              {

                  ViewBag.Msg = message2;
              }

              if (!id.HasValue)
              {
                  return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
              }
              else
              {
                  var oferta = await _ofertaService.Get(new EntityDto<int>(id.Value));
                  var proyecto = oferta.Requerimiento.Proyecto;
                  var contrato = oferta.Requerimiento.Proyecto.Contrato;
                  var cliente = oferta.Requerimiento.Proyecto.Contrato.Cliente;
                  var computos = _computoService.GetComputosPorOferta(id.Value);
                  var areas = _wbsofertaService.GetAreasWbsRegistrado(id.Value);

                  var disciplinas = _wbsofertaService.GetDisciplinasWbsRegistrado(id.Value, 1);
                                     var ViewModel = new OfertaWbsComputoViewModel
                      {
                            Cliente = cliente,
                          Oferta = oferta,
                          Proyecto = proyecto,
                          Contrato = contrato,
                          Computo = computos,
                          Areas = areas,
                          AreaId = pa,
                          DisciplinaId = pd,
                          //Disciplinas =disciplinas 



                      };
                      */
            return View("");

        }


        public async System.Threading.Tasks.Task<ActionResult> DetailsComputo(int? id, String message)
        {
            if (message != null)
            {

                ViewBag.Msg = message;
            }

            if (!id.HasValue)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                var wbsoferta = await _wbsService.Get(new EntityDto<int>(id.Value));
                var oferta = _ofertaService.getdetalle(wbsoferta.OfertaId);
                var proyecto = _proyectoservice.GetDetalles(oferta.ProyectoId);

                var contrato = proyecto.Contrato;

                var computos = _computoService.GetComputosporWbsOferta(id.Value);

                if (wbsoferta == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.NotFound);
                }
                else
                {
                    foreach (var items in computos)
                    {
                        var iditempadre = _itemservice.buscaridentificadorpadre(items.Item.item_padre);
                        var item = await _itemservice.GetDetalle(iditempadre);
                        items.nombreitem = item.nombre;
                    }

                    var ViewModel = new WbsOfertaComputoViewModel
                    {
                        Oferta = oferta,
                        WbsOferta = wbsoferta,
                        Contrato = contrato,
                        Computo = computos,

                    };
                    return View(ViewModel);
                }
            }
        }

        public async Task<ActionResult> DetailsDatos(int? id)
        {
            if (id.HasValue)
            {
                var computo = await _computoService.GetDetalle(id.Value);

                int idpadre = _itemservice.buscaridentificadorpadre(computo.Item.item_padre);
                var item = await _itemservice.GetDetalle(idpadre);

                /*
                if (computo != null)
                {
                    computo.nombreitem = item.nombre;
                    computo.nombrearea = _computoService.nombrecatalogo(computo.Wbs.AreaId);
                    computo.nombrediciplina = _computoService.nombrecatalogo(computo.Wbs.DisciplinaId);
                    computo.nombreactividad = _computoService.nombrecatalogo(computo.Wbs.ActividadId);
                    computo.nombreelemento = _computoService.nombrecatalogo(computo.Wbs.ElementoId);
                    return View(computo);
                }
                */
            }

            return RedirectToAction("Index", "Computo");
        }


        // GET: Proyecto/Computo/Create
        public async Task<ActionResult> CreateComputo(int? id, int ProyectoId, int pArea, int pDisciplina)
        {
            if (id.HasValue)
            {
                var oferta = await _ofertaService.Get(new EntityDto<int>(id.Value));
                ComputoDto computos = new ComputoDto();
                computos.estado = true;
                computos.cantidad = 1;
                ProyectoDto a = _proyectoservice.GetDetalles(ProyectoId);
                computos.Items =
                _itemservice.GetItemsporContratoActivo(a.contratoId, Convert.ToDateTime(oferta.fecha_oferta));
                var listanuevos = _itemservice.GetItemsPor("validar");
                foreach (var itemnuevo in listanuevos)
                {
                    computos.Items.Add(itemnuevo);
                }

                List<Catalogo> elementos = _wbsofertaService.GetElementosWbsRegistrado(id.Value, pArea, pDisciplina);
                SelectList actividades = new SelectList("");
                String nombreArea = _metodonombres.ObtenerNombreArea(pArea);
                String nombreDsicplina = _metodonombres.ObtenerNombreArea(pDisciplina);

                ComputoNuevoViewModel n = new ComputoNuevoViewModel
                {
                    Proyecto = a,
                    Oferta = oferta,
                    AreaId = pArea,
                    NombreArea = nombreArea,
                    NombreDisciplina = nombreDsicplina,
                    DisciplinaId = pDisciplina,
                    Elementos = elementos,
                    Computo = computos,
                    Actividades = actividades,
                    itemid = 0,


                };
                ViewBag.ComputoObjeto = n;
                return PartialView(n);
            }
            else
            {
                return RedirectToAction("Index", "Computo");
            }

        }

        [System.Web.Mvc.HttpPost]
        public async Task<ActionResult> CreateComputo([FromBody] ComputoNuevoViewModel computo,
            [FromBody] int ProyectoId, [FromBody] int OfertaId, [FromBody] int cantidad, [FromBody] int AreaId,
            [FromBody] int DisciplinaId, [FromBody] int ElementoId, [FromBody] int ActividadId, [FromBody] int itemId)
        {
            if (ModelState.IsValid)
            {

                var item = await _itemservice.Get(new EntityDto<int>(itemId));
                ItemDto dto = await _itemservice.Get(new EntityDto<int>(itemId));
                var doferta = await _ofertaService.Get(new EntityDto<int>(OfertaId));
                var WbsOfertadto =
                    _wbsofertaService.GetWbsOfertaIdpor(OfertaId, AreaId, DisciplinaId, ElementoId, ActividadId);
                ComputoDto computos = new ComputoDto()
                {
                    Id = 0,
                    cantidad = cantidad,
                    ItemId = itemId,
                    WbsId = WbsOfertadto.Id,
                    vigente = true,
                    costo_total = 0,
                    estado = true
                };

                bool e = _computoService.comprobarexistenciaitem(WbsOfertadto.Id, itemId);
                if (!e)
                {
                    computos.vigente = true;

                    var result = await _computoService.Create(computos);

                    String mensaje = "Se guardo correctamente el Item " + dto.nombre + " en el Area:  " +
                                     _wbsofertaService.ObtenerNombreDiciplina(WbsOfertadto.AreaId) +
                                     "  Disciplina:  " +
                                     _wbsofertaService.ObtenerNombreDiciplina(WbsOfertadto.DisciplinaId) +
                                     "  Elemento:  " +
                                     _wbsofertaService.ObtenerNombreDiciplina(WbsOfertadto.ElementoId) +
                                     "  Actividad:  " +
                                     _wbsofertaService.ObtenerNombreDiciplina(WbsOfertadto.ActividadId);

                    ViewBag.Correcto = mensaje;

                    ComputoDto computon = new ComputoDto();
                    computon.estado = true;
                    computon.cantidad = 1;
                    ProyectoDto a = _proyectoservice.GetDetalles(ProyectoId);

                    computos.Items =
                        _itemservice.GetItemsporContratoActivo(a.contratoId, Convert.ToDateTime(doferta.fecha_oferta));
                    var listanuevos = _itemservice.GetItemsPor("validar");
                    foreach (var itemnuevo in listanuevos)
                    {
                        computos.Items.Add(itemnuevo);
                    }

                    List<Catalogo> elementos =
                        _wbsofertaService.GetElementosWbsRegistrado(doferta.Id, AreaId, DisciplinaId);

                    List<Catalogo> actividades =
                        _wbsofertaService.GetActividadesWbsRegistrado(doferta.Id, AreaId, DisciplinaId, ElementoId);
                    SelectList activid = new SelectList(actividades, "Id", "nombre");
                    String nombreArea = _metodonombres.ObtenerNombreArea(AreaId);
                    String nombreDsicplina = _metodonombres.ObtenerNombreArea(DisciplinaId);

                    ComputoNuevoViewModel n = new ComputoNuevoViewModel
                    {
                        Proyecto = a,
                        Oferta = doferta,
                        AreaId = AreaId,
                        NombreArea = nombreArea,
                        NombreDisciplina = nombreDsicplina,
                        DisciplinaId = DisciplinaId,
                        Elementos = elementos,
                        Computo = computos,
                        Actividades = activid,
                        itemid = 0

                    };
                    return PartialView("CreateComputo", n);
                }
                else
                {
                    String mensaje = "El Item " + dto.nombre + " ya se encuentra registrado en el Area:  " +
                                     _wbsofertaService.ObtenerNombreDiciplina(WbsOfertadto.AreaId) +
                                     "  Disciplina:  " +
                                     _wbsofertaService.ObtenerNombreDiciplina(WbsOfertadto.DisciplinaId) +
                                     "  Elemento:  " +
                                     _wbsofertaService.ObtenerNombreDiciplina(WbsOfertadto.ElementoId) +
                                     "  Actividad:  " +
                                     _wbsofertaService.ObtenerNombreDiciplina(WbsOfertadto.ActividadId);
                    ProyectoDto a = _proyectoservice.GetDetalles(ProyectoId);
                    computos.Items =
                        _itemservice.GetItemsporContratoActivo(a.contratoId, Convert.ToDateTime(doferta.fecha_oferta));
                    List<Catalogo> elementos =
                        _wbsofertaService.GetElementosWbsRegistrado(OfertaId, AreaId, DisciplinaId);
                    List<Catalogo> actividades =
                        _wbsofertaService.GetActividadesWbsRegistrado(doferta.Id, AreaId, DisciplinaId, ElementoId);
                    SelectList activid = new SelectList(actividades, "Id", "nombre");
                    var oferta = await _ofertaService.Get(new EntityDto<int>(OfertaId));

                    String nombreArea = _metodonombres.ObtenerNombreArea(AreaId);
                    String nombreDsicplina = _metodonombres.ObtenerNombreArea(DisciplinaId);
                    ComputoNuevoViewModel n = new ComputoNuevoViewModel
                    {
                        Proyecto = a,
                        Oferta = oferta,
                        AreaId = AreaId,
                        NombreArea = nombreArea,
                        NombreDisciplina = nombreDsicplina,
                        DisciplinaId = DisciplinaId,
                        Elementos = elementos,
                        Computo = computos,
                        Actividades = activid,
                        itemid = 0,


                    };
                    ViewBag.Error = mensaje;

                    return PartialView("CreateComputo", n);

                }

            }
            else
            {

                var dtoa = Mapper.Map<ComputoDto>(computo);
                dtoa.Items = _itemservice.GetItems();
                return View("Create", dtoa);

            }
        }


        public async Task<ActionResult> Create(int? id, int ContratoId, int OfertaId)
        {
            if (id.HasValue)
            {
                var wo = await _wbsService.Get(new EntityDto<int>(id.Value));
                var doferta = await _ofertaService.Get(new EntityDto<int>(OfertaId));
                ComputoDto computos = new ComputoDto();
                computos.WbsId = id.Value;
                computos.estado = true;
                List<Item> items =
                    _itemservice.GetItemsporContratoActivo(ContratoId, Convert.ToDateTime(doferta.fecha_oferta));
                List<Item> nombrescompletos = new List<Item>();
                foreach (var r in items)
                {
                    int idpadre = _itemservice.buscaridentificadorpadre(r.item_padre);
                    if (idpadre != 0)
                    {
                        var item = await _itemservice.GetDetalle(idpadre);
                        Item a = new Item
                        {
                            Id = r.Id,
                            nombre = r.item_padre + " - " + item.nombre + " / " + r.codigo + " - " + r.nombre,
                            codigo = "",
                            descripcion = "",
                            GrupoId = 0,
                            item_padre = "",
                            para_oferta = true,
                            UnidadId = 0,
                            vigente = true
                        };
                        nombrescompletos.Add(a);
                    }



                }


                computos.Items = nombrescompletos;

                return View(computos);
            }
            else
            {
                return RedirectToAction("Index", "Computo");
            }

        }

        [System.Web.Mvc.HttpPost]
        public async Task<ActionResult> Create(ComputoDto computo)
        {
            if (ModelState.IsValid)
            {
                computo.Id = 0;
                var item = await _itemservice.Get(new EntityDto<int>(computo.ItemId));
                ItemDto dto = await _itemservice.Get(new EntityDto<int>(computo.ItemId));
                bool e = _computoService.comprobarexistenciaitem(computo.WbsId, computo.ItemId);
                if (!e)
                {
                    computo.vigente = true;
                    var result = await _computoService.Create(computo);

                    return RedirectToAction("DetailsComputo", new RouteValueDictionary(
                        new { controller = "Computo", action = "DetailsComputo", Id = result.WbsId }));
                }
                else
                {
                    ViewBag.Error = "Solo puedo registrar una sola vez el item para la fila de wbs oferta";
                    var dtoa = Mapper.Map<ComputoDto>(computo);
                    dtoa.Items = _itemservice.GetItems();
                    return View("Create", dtoa);

                }
            }
            else
            {

                var dtoa = Mapper.Map<ComputoDto>(computo);
                dtoa.Items = _itemservice.GetItems();
                return View("Create", dtoa);

            }
        }


        // GET: Proyecto/Computo/Edit/5
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
                var computoDto = await _computoService.GetDetalle(id.Value);
                List<Item> items = _itemservice.GetItemsporContratoActivo(
                    computoDto.Wbs.Oferta.Requerimiento.Proyecto.contratoId,
                    Convert.ToDateTime(computoDto.Wbs.Oferta.fecha_oferta));
                List<Item> nombrescompletos = new List<Item>();
                foreach (var r in items)
                {
                    int idpadre = _itemservice.buscaridentificadorpadre(r.item_padre);
                    if (idpadre != 0)
                    {

                        var item = await _itemservice.GetDetalle(idpadre);

                        Item a = new Item
                        {
                            Id = r.Id,
                            nombre = r.item_padre + " - " + item.nombre + " / " + r.codigo + " - " + r.nombre,
                            codigo = "",
                            descripcion = "",
                            GrupoId = 0,
                            item_padre = "",
                            para_oferta = true,
                            UnidadId = 0,
                            vigente = true
                        };
                        nombrescompletos.Add(a);
                    }
                }

                /* computoDto.nombrearea = _computoService.nombrecatalogo(computoDto.Wbs.AreaId);
                 computoDto.nombrediciplina = _computoService.nombrecatalogo(computoDto.Wbs.DisciplinaId);
                 computoDto.nombreactividad = _computoService.nombrecatalogo(computoDto.Wbs.ActividadId);
                 computoDto.nombreactividad = _computoService.nombrecatalogo(computoDto.Wbs.ActividadId);
                 computoDto.nombreelemento = _computoService.nombrecatalogo(computoDto.Wbs.ElementoId);
                 */
                computoDto.Items = nombrescompletos;

                if (computoDto == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.NotFound);
                }
                else
                {
                    return View(computoDto);
                }
            }
        }

        // POST: Proyecto/Computo/Edit/5
        [System.Web.Mvc.HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> Edit(int id, ComputoDto computoDto)
        {
            try
            {
                var computo = await _computoService.InsertOrUpdateAsync(computoDto);
                return RedirectToAction("DetailsComputo", new RouteValueDictionary(
                    new { controller = "Computo", action = "DetailsComputo", Id = computoDto.WbsId }));
            }
            catch
            {
                return View();
            }
        }

        // GET: Proyecto/Computo/Delete/5
        [System.Web.Mvc.HttpPost]
        public async Task<ActionResult> Delete(int? id)
        {
            try
            {
                if (id.HasValue)
                {

                    ComputoDto nuevo = await _computoService.GetDetalle(id.Value);
                    nuevo.vigente = false;
                    var result = await _computoService.InsertOrUpdateAsync(nuevo);
                    return RedirectToAction("DetailsComputo", new RouteValueDictionary(
                        new { controller = "Computo", action = "DetailsComputo", Id = nuevo.WbsId }));

                }

                return RedirectToAction("Index", "Computo");
            }
            catch
            {
                return View();
            }
        }


        public ActionResult GetStateList(int OfertaId, int AreaId)
        {
            List<Catalogo> StateList = _wbsofertaService.GetDisciplinasWbsRegistrado(OfertaId, AreaId);


            var resultado = JsonConvert.SerializeObject(StateList,
                Newtonsoft.Json.Formatting.None,
                new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });

            ViewBag.resultado = resultado;
            return Content(resultado);
        }

        public ActionResult GetStateListActividades(int OfertaId, int AreaId, int DiscId, int ElId)
        {
            List<Catalogo> StateList = _wbsofertaService.GetActividadesWbsRegistrado(OfertaId, AreaId, DiscId, ElId);

            var resultado = JsonConvert.SerializeObject(StateList,
                Newtonsoft.Json.Formatting.None,
                new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });
            return Content(resultado);
        }

        public ActionResult _CreateItem()
        {
            ViewBag.Error = null;
            ItemDto Nuevo = new ItemDto();
            Nuevo.codigo = "validar";
            Nuevo.item_padre = "validar";
            Nuevo.para_oferta = true;
            return PartialView("_CreateItem", Nuevo);
        }

        // POST: Proyecto/Item/Create

        [System.Web.Mvc.HttpPost]
        public async Task<ActionResult> _CreateItem(ItemDto Nuevo)
        {
            try
            {
                if (ModelState.IsValid)
                {
                }

                Nuevo.vigente = true;
                var itemr = await _itemservice.Create(Nuevo);
                ViewBag.Error = "Creado Correctamente " + Nuevo.nombre;
                Nuevo.nombre = "";
                Nuevo.descripcion = "";
                Nuevo = new ItemDto();
                return PartialView("_CreateItem", Nuevo);
            }
            catch
            {
                return PartialView();
            }
        }

#pragma warning disable CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        public async Task<ActionResult> EstructuraComputos(int? id, string mensaje) // AvanceObra Id
#pragma warning restore CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        {
            if (id.HasValue)
            {
                if (mensaje != null)
                {
                    ViewBag.msg = mensaje;
                }

                //  var oferta = await _ofertaService.Get(new EntityDto<int>(id.Value));
                var ofertac = _ofertaService.getdetalle(id.Value);
                ViewBag.rId = ofertac.RequerimientoId;
                ViewBag.ruta = new string[] { "Inicio", "Proyecto", "Requerimiento", "Rdo", "Computo", ofertac.codigo + "-" + ofertac.version, "Gestionar" };
                var computos = new List<ComputoDto>();
                OfertaComputoViewModel viewModel = new OfertaComputoViewModel()
                {
                    Oferta = ofertac,
                    Contrato = ofertac.Proyecto.Contrato
                };

                return View(viewModel);
            }

            return RedirectToAction("Index", "Inicio", new { area = "" });
        }
        /*
                [System.Web.Mvc.HttpPost]
                public async Task<ActionResult> EstructuraComputos(OfertaComputoViewModel es) // AvanceObra Id
                {
                    // var id = await _detalleAvanceObraService.CreateDetalleAvance(detalle);
                    //return Content(id > 0 ? "OK" : "Error");
                    return View();
                }
                */
        public ActionResult ApiComputo(int id)
        {
            //var lsita = _wbsofertaService.GenerarArbolComputo(id); //arbol antiguo
            var lsita = _wbsService.GenerarArbolComputo(id); // arbol nuevo wbs recursivo

            //var json = Json(lsita, JsonRequestBehavior.AllowGet);
            var result = JsonConvert.SerializeObject(lsita,
                Newtonsoft.Json.Formatting.None,
                new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });

            /*var json = new JsonResult
            {
                Data = JsonConvert.DeserializeObject(result)
            };*/
            //json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return Content(result);
        }

        [System.Web.Mvc.HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> EditComputo(ComputoDto computoDto, [FromBody] decimal cantidad_eac = 0)
        {

            var e = await _computoService.GetDetalle(computoDto.Id);

            bool r = _computoService.EditarCantidadComputo(e.Id, computoDto.cantidad, cantidad_eac);


            return Content(r == true ? "OK" : "Error");

        }


        [System.Web.Mvc.HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> CreateComputoArbol(ComputoDto computoDto, [FromBody]string nuevonombrei)
        {
            if (ModelState.IsValid)
            {
            }
            var wbs = await _wbsService.Get(new EntityDto<int>(computoDto.WbsId));
            var ofertac = _ofertaService.getdetalle(wbs.OfertaId);
            var contador = _computoService.GetComputosporOfertaProcura(wbs.OfertaId);
            var Item = _itemservice.DatosItem(computoDto.ItemId);

            computoDto.codigo_primavera = "a";
            computoDto.fecha_registro = DateTime.Now;
            computoDto.estado = true;
            computoDto.vigente = true;
            if (Item != null && Item.Id > 0)
            {
                if (Item.GrupoId == 3)
                {
                    string[] data = Item.codigo.Split('.');
                    computoDto.codigo_item_alterno = data[0] + "." + ofertac.codigo + "." + (_computoService.GetComputosporOfertaProcura(ofertac.Id) + 1);
                    ;
                }
                else
                {

                    computoDto.codigo_item_alterno = Item.codigo;
                }

            }
            if (computoDto.cantidad_eac == 0)
            {
                computoDto.cantidad_eac = computoDto.cantidad;
            }



            bool e = _computoService.comprobarexistenciaitem(computoDto.WbsId, computoDto.ItemId);
            if (!e)
            {
                decimal PU = _ServicePreciario.ObtenerPrecioUnitarioItem(computoDto.ItemId,wbs.OfertaId);
                computoDto.precio_unitario = PU;
                computoDto.costo_total = PU * computoDto.cantidad;

                var computo = await _computoService.InsertOrUpdateAsync(computoDto);
                if (computo.Id > 0)
                {
                    return Content("OK");
                }
                else
                {
                    return Content("ERROR");
                }
            }
            else
            {
                return Content("Repetido");
            }

        }
        [System.Web.Mvc.HttpPost]
        public async Task<ActionResult> CreateAdicional([FromBody]int WbsId,
            [FromBody]string nombre,
            [FromBody]string descripcion,
            [FromBody]int cantidad,
            [FromBody]int unidadid,
            [FromBody]int npadre)
        {

            string mensaje = "";
            if (ModelState.IsValid)
            {
            }
            var wbs = await _wbsService.Get(new EntityDto<int>(WbsId));
            var ofertac = _ofertaService.getdetalle(wbs.OfertaId);
            var contador = _computoService.GetComputosporOfertaProcura(wbs.OfertaId);
            var ipadre = await _itemservice.GetDetalle(npadre);

            if (ipadre.Id > 0)
            {
                var ihijos = _itemservice.GetItemsHijos(ipadre.codigo);
                ItemDto iadicional = new ItemDto
                {
                    Id = 0,
                    codigo = ipadre.codigo + (ihijos.Count + 1),
                    item_padre = ipadre.codigo,
                    GrupoId = 3,
                    descripcion = descripcion,
                    UnidadId = unidadid,
                    nombre = nombre,
                    vigente = true,
                    para_oferta = true
                };


                var itemnuevo = await _itemservice.Create(iadicional);

                if (itemnuevo.Id > 0)
                {

                    ComputoDto adicional = new ComputoDto
                    {
                        Id = 0,
                        WbsId = WbsId,
                        cantidad = cantidad,
                        codigo_primavera = "a",
                        ItemId = itemnuevo.Id,
                        estado = true,
                        vigente = true,
                        fecha_registro = DateTime.Now,
                        cantidad_eac = cantidad,
                        codigo_item_alterno = "7." + ofertac.codigo + "." + (contador + 1)
                    };


                    var rcomputo = await _computoService.Create(adicional);

                    if (rcomputo.Id > 0)
                    {
                        var c = await _computoService.GetDetalle(rcomputo.Id);
                        var wbsoferta = await _wbsService.Get(new EntityDto<int>(c.WbsId));
                        var oferta = _ofertaService.getdetalle(wbsoferta.OfertaId);
                        var fecha = oferta.fecha_oferta.Value;
                        var preciario = _preciarioService.preciarioporcontratofecha(oferta.Proyecto.contratoId, fecha);

                        if (preciario.Id > 0)
                        {

                            DetallePreciarioDto a = new DetallePreciarioDto
                            {
                                Id = 0,
                                ItemId = rcomputo.ItemId,
                                precio_unitario = 0,
                                vigente = true,
                                PreciarioId = preciario.Id
                            };

                            var dp = await _dpreciarioService.Create(a);
                            if (dp.Id > 0)
                            {
                                mensaje = "Guardado";
                            }
                            else
                            {
                                mensaje = "Error";
                            }
                        }



                    }
                    else
                    {
                        mensaje = "Error";
                    }
                }
                else
                {
                    mensaje = "noitem";
                }
            }
            else
            {
                mensaje = "Error";
            }

            return Content(mensaje);

        }
        public ActionResult ItemsparaOferta()
        {
            var lsita = _itemservice.GetItemsParaOferta();

            var result = JsonConvert.SerializeObject(lsita,
                 Newtonsoft.Json.Formatting.None,
                 new JsonSerializerSettings
                 {
                     NullValueHandling = NullValueHandling.Ignore
                 });
            return Content(result);
        }

        public ActionResult ItemsparaOfertaC(int id, [FromBody]DateTime f)
        {
            var lsita = _itemservice.GetItemsporContratoActivo2(id, f);

            var result = JsonConvert.SerializeObject(lsita,
                 Newtonsoft.Json.Formatting.None,
                 new JsonSerializerSettings
                 {
                     NullValueHandling = NullValueHandling.Ignore
                 });
            return Content(result);
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult GetComputosPorProyectoApi(int id) // AvanceObra Id
        {
            var list = _computoService.GetComputosPorOferta(id);
            var result = JsonConvert.SerializeObject(list);
            return Content(result);
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult GetCabeceraApi(int id) // AvanceObra Id
        {
            var list = _computoService.GetCabeceraApi(id);
            var result = JsonConvert.SerializeObject(list);
            return Content(result);
        }

        [System.Web.Mvc.HttpPost]
#pragma warning disable CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        public async Task<ActionResult> DeleteComputoArbol(int id)
#pragma warning restore CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        {
            var r = _computoService.EliminarVigencia(id);

            if (r.Result)
            {

                return Content("Eliminado");
            }

            return Content("ErrorEliminado");

        }


        public ActionResult ItemsProcura()
        {
            var lsita = _itemservice.GetItemsHijos(".").Where(c => c.GrupoId == 3);

            var result = JsonConvert.SerializeObject(lsita,
                Newtonsoft.Json.Formatting.None,
                new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });
            return Content(result);
        }

        public ActionResult CatalogoUnidades()
        {
            var lsita = _catalogoService.ListarCatalogos(5);

            var result = JsonConvert.SerializeObject(lsita,
                Newtonsoft.Json.Formatting.None,
                new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });
            return Content(result);
        }


        public ActionResult ExportarExcel(int OfertaId)
        {
            var ofertac = _ofertaService.getdetalle(OfertaId);

            var wbs = _wbsService.Listar(OfertaId);
            var items = _itemservice.GetItemsporContratoActivo2(ofertac.Proyecto.contratoId, ofertac.fecha_oferta.Value);

            ExcelPackage excel = new ExcelPackage();
            var workSheet = excel.Workbook.Worksheets.Add("Carga Computo");

            workSheet.TabColor = System.Drawing.Color.Azure;
            /*workSheet.Protection.IsProtected = true;
              workSheet.Protection.SetPassword("pmdis");
              */
            workSheet.DefaultRowHeight = 12;

            //Header of table  
            //  
            workSheet.Row(1).Height = 20;
            workSheet.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            workSheet.Row(2).Style.Font.Bold = true;
            workSheet.Row(4).Style.Font.Bold = true;
            workSheet.Row(4).Height = 30;



            int columna = 5;
            foreach (var itemswbs in wbs)
            {


                workSheet.Cells[1, columna].Value = itemswbs.nombre_padre;
                workSheet.Cells[2, columna].Value = itemswbs.nivel_nombre;
                workSheet.Cells[3, columna].Value = itemswbs.Id;
                if (itemswbs.DisciplinaId >= 0)
                {
                    workSheet.Cells[4, columna].Value = itemswbs.Catalogo.nombre;
                }
                workSheet.Cells[1, 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                workSheet.Cells[1, 1].Style.Fill.BackgroundColor.SetColor(Color.Gainsboro);
                workSheet.Cells[1, 2].Style.Fill.PatternType = ExcelFillStyle.Solid;
                workSheet.Cells[1, 2].Style.Fill.BackgroundColor.SetColor(Color.Gainsboro);
                workSheet.Cells[1, 3].Style.Fill.PatternType = ExcelFillStyle.Solid;
                workSheet.Cells[1, 3].Style.Fill.BackgroundColor.SetColor(Color.Gainsboro);
                workSheet.Cells[2, 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                workSheet.Cells[2, 1].Style.Fill.BackgroundColor.SetColor(Color.Gainsboro);
                workSheet.Cells[2, 2].Style.Fill.PatternType = ExcelFillStyle.Solid;
                workSheet.Cells[2, 2].Style.Fill.BackgroundColor.SetColor(Color.Gainsboro);
                workSheet.Cells[2, 3].Style.Fill.PatternType = ExcelFillStyle.Solid;
                workSheet.Cells[2, 3].Style.Fill.BackgroundColor.SetColor(Color.Gainsboro);
                workSheet.Cells[3, 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                workSheet.Cells[3, 1].Style.Fill.BackgroundColor.SetColor(Color.Gainsboro);
                workSheet.Cells[3, 2].Style.Fill.PatternType = ExcelFillStyle.Solid;
                workSheet.Cells[3, 2].Style.Fill.BackgroundColor.SetColor(Color.Gainsboro);
                workSheet.Cells[3, 3].Style.Fill.PatternType = ExcelFillStyle.Solid;
                workSheet.Cells[3, 3].Style.Fill.BackgroundColor.SetColor(Color.Gainsboro);
                workSheet.Cells[1, 4].Style.Fill.PatternType = ExcelFillStyle.Solid;
                workSheet.Cells[1, 4].Style.Fill.BackgroundColor.SetColor(Color.Gainsboro);
                workSheet.Cells[2, 4].Style.Fill.PatternType = ExcelFillStyle.Solid;
                workSheet.Cells[2, 4].Style.Fill.BackgroundColor.SetColor(Color.Gainsboro);
                workSheet.Cells[3, 4].Style.Fill.PatternType = ExcelFillStyle.Solid;
                workSheet.Cells[3, 4].Style.Fill.BackgroundColor.SetColor(Color.Gainsboro);
                workSheet.Cells[4, 4].Style.Fill.PatternType = ExcelFillStyle.Solid;
                workSheet.Cells[4, 4].Style.Fill.BackgroundColor.SetColor(Color.Gainsboro);
                workSheet.Cells[1, columna].Style.Fill.PatternType = ExcelFillStyle.Solid;
                workSheet.Cells[1, columna].Style.Fill.BackgroundColor.SetColor(Color.Gainsboro);

                workSheet.Cells[2, columna].Style.Fill.PatternType = ExcelFillStyle.Solid;
                workSheet.Cells[2, columna].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);

                workSheet.Cells[3, columna].Style.Fill.PatternType = ExcelFillStyle.Solid;
                workSheet.Cells[3, columna].Style.Fill.BackgroundColor.SetColor(Color.Gainsboro);
                workSheet.Cells[4, columna].Style.Fill.PatternType = ExcelFillStyle.Solid;
                workSheet.Cells[4, columna].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                workSheet.Column(columna).Width = 30;

                columna = columna + 1;
            }

            workSheet.Cells[4, 1].Value = "Id";
            workSheet.Cells[4, 2].Value = "Código Item";
            workSheet.Cells[4, 3].Value = "Nombre Item";
            workSheet.Cells[4, 4].Value = "Unidad";
            workSheet.Cells["B4:D4"].AutoFilter = true;
            workSheet.Cells[4, 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
            workSheet.Cells[4, 1].Style.Fill.BackgroundColor.SetColor(Color.Gainsboro);
            workSheet.Cells[4, 2].Style.Fill.PatternType = ExcelFillStyle.Solid;
            workSheet.Cells[4, 2].Style.Fill.BackgroundColor.SetColor(Color.Gainsboro);
            workSheet.Cells[4, 3].Style.Fill.PatternType = ExcelFillStyle.Solid;
            workSheet.Cells[4, 3].Style.Fill.BackgroundColor.SetColor(Color.Gainsboro);
            workSheet.Cells[4, 3].Style.Fill.PatternType = ExcelFillStyle.Solid;
            workSheet.Cells[4, 3].Style.Fill.BackgroundColor.SetColor(Color.Gainsboro);
            workSheet.Column(1).Width = 20;
            workSheet.Column(2).Width = 20;
            workSheet.Column(3).Width = 40;
            workSheet.Column(1).Style.Font.Bold = true;
            workSheet.Column(1).Hidden = true;

            workSheet.Row(3).Hidden = true;

            int c = 5;
            workSheet.View.FreezePanes(5, 1);
            workSheet.View.FreezePanes(5, 5);
            foreach (var pitem in items)
            {
                workSheet.Cells[c, 1].Value = pitem.Id;
                workSheet.Cells[c, 2].Value = pitem.codigo;
                workSheet.Cells[c, 3].Value = pitem.nombre;
                workSheet.Cells[c, 4].Value = _computoService.nombrecatalogo(pitem.UnidadId);
                workSheet.Cells[c, 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                workSheet.Cells[c, 1].Style.Fill.BackgroundColor.SetColor(Color.Gainsboro);
                workSheet.Cells[c, 2].Style.Fill.PatternType = ExcelFillStyle.Solid;
                workSheet.Cells[c, 2].Style.Fill.BackgroundColor.SetColor(Color.Gainsboro);
                workSheet.Cells[c, 3].Style.Fill.PatternType = ExcelFillStyle.Solid;
                workSheet.Cells[c, 3].Style.Fill.BackgroundColor.SetColor(Color.Gainsboro);
                workSheet.Cells[c, 4].Style.Fill.PatternType = ExcelFillStyle.Solid;
                workSheet.Cells[c, 4].Style.Fill.BackgroundColor.SetColor(Color.Gainsboro);
                workSheet.Row(c).Style.Locked = false;
                c = c + 1;
            }



            string excelName = "FormatoCargaComputo";
            using (var memoryStream = new MemoryStream())
            {
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment; filename=" + excelName + ".xlsx");
                excel.SaveAs(memoryStream);
                memoryStream.WriteTo(Response.OutputStream);
                Response.Flush();
                Response.End();
                return Content("");
            }
        }

        public ActionResult SubirExcel(int id)
        {
            var ofertac = _ofertaService.getdetalle(id);
            ViewBag.OfertaId = ofertac.Id;
            ViewBag.Contratoid = ofertac.Proyecto.contratoId;
            ViewBag.Fechaoferta = ofertac.fecha_oferta.Value.ToShortDateString();
            ViewBag.ruta = new string[] { "Inicio", "Ingeniería", "Computo", ofertac.codigo + "-" + ofertac.version, "Carga Masiva" };
            return View();
        }



        [System.Web.Mvc.HttpPost]
        public async Task<ActionResult> SubirExcel(HttpPostedFileBase UploadedFile, int ofertaid)
        {
            string error = "";
            try
            {
                List<String[]> Observaciones = new List<string[]>();

                if (UploadedFile != null)
                {

                    // tdata.ExecuteCommand("truncate table OtherCompanyAssets");  
                    if (UploadedFile.ContentType == "application/vnd.ms-excel" || UploadedFile.ContentType ==
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                    {
                        string fileName = UploadedFile.FileName;
                        string fileContentType = UploadedFile.ContentType;
                        byte[] fileBytes = new byte[UploadedFile.ContentLength];
                        var data = UploadedFile.InputStream.Read(fileBytes, 0,
                            Convert.ToInt32(UploadedFile.ContentLength));


                        using (var package = new ExcelPackage(UploadedFile.InputStream))
                        {
                            var currentSheet = package.Workbook.Worksheets;
                            var workSheet = currentSheet.First();
                            var noOfCol = workSheet.Dimension.End.Column - 4;

                            var noOfRow = workSheet.Dimension.End.Row - 13;

                            int maximo_nivel = _wbsService.nivel_mas_alto(ofertaid);
                            for (int i = 6; i <= noOfCol; i++)
                            {
                                for (int rowIterator = maximo_nivel + 3; rowIterator <= noOfRow; rowIterator++)
                                {
                                    error = "Ocurrió un error en  columna :" + i + " fila:" + rowIterator+" del documento que intenta subir";
                                    var para_oferta = (workSheet.Cells[rowIterator, 1].Value ?? "").ToString();
                                    var itemid = (workSheet.Cells[rowIterator, 2].Value ?? "").ToString();

                                    var wbsid = (workSheet.Cells[maximo_nivel + 1, i].Value ?? "").ToString();

                                    var cantidad = (workSheet.Cells[rowIterator, i].Value ?? "").ToString();
                                    var valor = Decimal.Parse("0" + cantidad, NumberStyles.Float);
                                    var valoritem = Int32.Parse(itemid);
                                    var valorwbs = Int32.Parse(wbsid);

                                    if (itemid.Length > 0 && valoritem > 0 && valorwbs > 0 && wbsid.Length > 0 && cantidad.Length > 0 && valor >=0 && para_oferta.Length > 0 && para_oferta.Equals("True"))
                                    {

                                        decimal PU = _ServicePreciario.ObtenerPrecioUnitarioItem(valoritem, ofertaid);


                                        ComputoDto n = new ComputoDto
                                        {

                                            ItemId = Int32.Parse(itemid),
                                            WbsId = Int32.Parse(wbsid),
                                            cantidad = valor,
                                            precio_unitario = PU,
                                            costo_total = PU * valor,
                                            cantidad_eac = valor, //Eac = cantidad
                                            cantidad_acumulada_anterior = 0,
                                            vigente = true,
                                            codigo_primavera = "a",
                                            fecha_registro = DateTime.Now,
                                            precio_ajustado = 0,
                                            precio_base = 0,
                                            precio_incrementado = 0,
                                            estado = true,
                                        };

                                        Computo e = _computoService.editarcomputoexiste(Int32.Parse(wbsid), Int32.Parse(itemid));
                                        var valor_anterior = e.cantidad_eac;
                                        if (e.Id == 0)
                                        {
                                            if (valor > 0) {
                                            var r = await _computoService.Create(n);
                                            }
                                        }
                                        else

                                        {
                                            e.cantidad = valor;
                                            e.cantidad_eac = valor;

                                            // e.cantidad_eac = valor; // No se actualiza cuando existe edición
                                            var r = await _computoService.Update(AutoMapper.Mapper.Map<ComputoDto>(e));
                                       

                                        }





                                    }

                                }
                            }
                            if (Observaciones.Count > 0)
                            {

                                ExcelPackage excel = new ExcelPackage();
                                var errores = excel.Workbook.Worksheets.Add("Observaciones");
                                errores.Cells[1, 1].Value = "Codigo Item";
                                errores.Cells[1, 2].Value = "Observacion";
                                workSheet.Column(1).Width = 20;
                                workSheet.Column(2).Width = 60;
                                var row = 2;
                                foreach (var pitem in Observaciones)
                                {
                                    errores.Cells[row, 1].Value = pitem[0].ToString();
                                    errores.Cells[row, 2].Value = pitem[1].ToString();

                                    row = row + 1;
                                }
                                string excelName = "Observaciones";
                                using (var memoryStream = new MemoryStream())
                                {
                                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                                    Response.AddHeader("content-disposition", "attachment; filename=" + excelName + ".xlsx");
                                    excel.SaveAs(memoryStream);
                                    memoryStream.WriteTo(Response.OutputStream);
                                    Response.Flush();
                                    Response.End();
                                }
                                ViewBag.Error = "Tiene Observaciones Verifique";
                                ViewBag.OfertaId = ofertaid;
                                var ofertac = _ofertaService.getdetalle(ofertaid);
                                ViewBag.Contratoid = ofertac.Proyecto.contratoId;
                                ViewBag.Fechaoferta = ofertac.fecha_oferta.Value.ToShortDateString();
                                return View("SubirExcel");
                            }
                            else
                            {
                                ViewBag.Msg = "Cargado Correctamente";
                                ViewBag.OfertaId = ofertaid;
                                var ofertac = _ofertaService.getdetalle(ofertaid);
                                ViewBag.Contratoid = ofertac.Proyecto.contratoId;
                                ViewBag.Fechaoferta = ofertac.fecha_oferta.Value.ToShortDateString();
                                return View("SubirExcel");

                            }
                        }

                    }
                    else
                    {


                        ViewBag.Error = "El Formato del Archivo Subido debe ser en formato Excel";
                        ViewBag.OfertaId = ofertaid;
                        var ofertac = _ofertaService.getdetalle(ofertaid);
                        ViewBag.Contratoid = ofertac.Proyecto.contratoId;
                        ViewBag.Fechaoferta = ofertac.fecha_oferta.Value.ToShortDateString();
                        return View("SubirExcel");


                    }
                }
            }
            catch {
                ViewBag.Error = error;
                ViewBag.OfertaId = ofertaid;
                var ofertac = _ofertaService.getdetalle(ofertaid);
                ViewBag.Contratoid = ofertac.Proyecto.contratoId;
                ViewBag.Fechaoferta = ofertac.fecha_oferta.Value.ToShortDateString();
                ElmahExtension.LogToElmah(new Exception("Error: " + error));

                return View("SubirExcel");

            }
            return View("SubirExcel");

        }

        public ActionResult MontosPresupuestos(int id)
        {
            decimal[] Observaciones = new decimal[10];

            //Montos Presupuesto
            decimal montopconstrucion = _computoService.MontoPresupuestoConstruccion(id);
            decimal montopingenieria = _computoService.MontoPresupuestoIngenieria(id);
            decimal montopprocura = _computoService.MontoPresupuestoProcura(id);
            decimal totalp = montopconstrucion + montopingenieria + montopprocura;

            //Montos Certificados
            decimal montoa = montopconstrucion * (Convert.ToDecimal(0.4119));
            decimal montoi = montopconstrucion * (Convert.ToDecimal(0.03));
            decimal montou = montopconstrucion * (Convert.ToDecimal(0.12));
            decimal montopc = montopprocura * (Convert.ToDecimal(0.10));
            decimal total = montoa + montoi + montou + montopc;

            //Montos //saldos


            decimal totals = totalp + total;


            Observaciones[0] = montopconstrucion;
            Observaciones[1] = montopingenieria;
            Observaciones[2] = montopprocura;
            Observaciones[3] = totalp;
            Observaciones[4] = montoa;
            Observaciones[5] = montoi;
            Observaciones[6] = montou;
            Observaciones[7] = montopc;
            Observaciones[8] = total;
            Observaciones[9] = totals;

            var result = JsonConvert.SerializeObject(Observaciones,
                Newtonsoft.Json.Formatting.None,
                new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    NullValueHandling = NullValueHandling.Ignore
                });
            return Content(result);
        }

        public ActionResult GenerarPrespuesto(int OfertaId)
        {
            var ofertac = _ofertaService.getdetalle(OfertaId);

            var computos = _computoService.GetComputosPorOferta(OfertaId);
            var wbs = _wbsService.Listar(OfertaId);

            var excel = _computoService.GenerarExcelCabecera(ofertac);
            var workSheet = excel.Workbook.Worksheets[1];

            int columna = 5;
            int c = 10;
            workSheet.View.ShowGridLines = true;
            workSheet.View.FreezePanes(5, 1);
            workSheet.View.FreezePanes(10, 5);
            foreach (var itemswbs in wbs)
            {


                workSheet.Cells[6, columna].Value = itemswbs.nombre_padre;
                workSheet.Cells[7, columna].Value = itemswbs.nivel_nombre;
                workSheet.Cells[8, columna].Value = itemswbs.Id;
                workSheet.Cells[6, columna].Style.Fill.PatternType = ExcelFillStyle.Solid;
                workSheet.Cells[6, columna].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                workSheet.Cells[7, columna].Style.Fill.PatternType = ExcelFillStyle.Solid;
                workSheet.Cells[7, columna].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                workSheet.Cells[9, columna].Style.Fill.PatternType = ExcelFillStyle.Solid;
                workSheet.Cells[9, columna].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                workSheet.Cells[6, 2].Style.Fill.PatternType = ExcelFillStyle.Solid;
                workSheet.Cells[6, 2].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                workSheet.Cells[6, 3].Style.Fill.PatternType = ExcelFillStyle.Solid;
                workSheet.Cells[6, 3].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                workSheet.Cells[6, 4].Style.Fill.PatternType = ExcelFillStyle.Solid;
                workSheet.Cells[6, 4].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                workSheet.Cells[7, 2].Style.Fill.PatternType = ExcelFillStyle.Solid;
                workSheet.Cells[7, 2].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                workSheet.Cells[7, 3].Style.Fill.PatternType = ExcelFillStyle.Solid;
                workSheet.Cells[7, 3].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                workSheet.Cells[7, 4].Style.Fill.PatternType = ExcelFillStyle.Solid;
                workSheet.Cells[7, 4].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                workSheet.Cells[9, 2].Style.Fill.PatternType = ExcelFillStyle.Solid;
                workSheet.Cells[9, 2].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                workSheet.Cells[9, 3].Style.Fill.PatternType = ExcelFillStyle.Solid;
                workSheet.Cells[9, 3].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                workSheet.Cells[9, 4].Style.Fill.PatternType = ExcelFillStyle.Solid;
                workSheet.Cells[9, 4].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                workSheet.Column(columna).Width = 10;
                workSheet.Column(columna).Width = 30;
                columna = columna + 1;
            }

            //  var itemss = _itemservice.ArbolWbsExcel(ofertac.Proyecto.contratoId, ofertac.fecha_oferta.Value);

            var itemss = _itemservice.GetItems();
            workSheet.Cells[9, 1].Value = "ID";
            workSheet.Cells["B6:B9"].Merge = true;
            workSheet.Cells["B6:B9"].Value = "ITEM";
            workSheet.Cells["B6:B9"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            workSheet.Cells["B6:B9"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            workSheet.Cells["C6:C9"].Merge = true;
            workSheet.Cells["C6:C9"].Value = "DESCRIPCIÓN";
            workSheet.Cells["C6:C9"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            workSheet.Cells["C6:C9"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            workSheet.Cells["D6:D9"].Merge = true;
            workSheet.Cells["D6:D9"].Value = "UNIDAD";
            workSheet.Cells["B6:D9"].AutoFilter = true;
            workSheet.Cells["D6:D9"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            workSheet.Cells["D6:D9"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            workSheet.Cells["B6:D9"].Style.Font.SetFromFont(new System.Drawing.Font("Arial", 9, FontStyle.Bold));
            workSheet.Cells["B10:B1569"].Style.Font.SetFromFont(new System.Drawing.Font("Arial", 9, FontStyle.Bold));
            workSheet.Column(3).Width = 90;

            workSheet.Column(1).Style.Font.Color.SetColor(Color.White);

            workSheet.Row(8).Hidden = true;

            foreach (var pitem in itemss)


            {
                workSheet.Cells[c, 1].Value = pitem.Id;
                workSheet.Cells[c, 2].Value = pitem.codigo;
                workSheet.Cells[c, 3].Value = pitem.nombre;
                workSheet.Cells[c, 4].Value = _computoService.nombrecatalogo(pitem.UnidadId);



                c = c + 1;

            }
            var noOfCol = workSheet.Dimension.End.Column;
            var noOfRow = workSheet.Dimension.End.Row;



            for (int j = 5; j <= noOfCol; j++)
            {
                var wbsid = (workSheet.Cells[8, j].Value ?? "").ToString();
                for (int i = 10; i <= noOfRow; i++)
                {
                    var itemid = (workSheet.Cells[i, 1].Value ?? "").ToString();

                    foreach (var items in computos)
                    {
                        if (Convert.ToString(items.WbsId) == wbsid && Convert.ToString(items.ItemId) == itemid)
                        {

                            workSheet.Cells[i, j].Value = items.cantidad;

                        }

                    }


                }

            }


            string excelName = "Computos Cantidades";
            using (var memoryStream = new MemoryStream())
            {
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment; filename=" + excelName + ".xlsx");
                excel.SaveAs(memoryStream);
                memoryStream.WriteTo(Response.OutputStream);
                Response.Flush();
                Response.End();
                return Content("");
            }
        }



        [System.Web.Mvc.HttpPost]
#pragma warning disable CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        public async Task<ActionResult> CargaMasiva(HttpPostedFileBase UploadedFile, int ofertaid)
#pragma warning restore CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        {
            var doc = _computoService.CargaMasiva(UploadedFile, ofertaid);
            if (doc == null)
            {
                ViewBag.Msg = "Cargado Correctamente";
                ViewBag.OfertaId = ofertaid;
                var ofertac = _ofertaService.getdetalle(ofertaid);
                ViewBag.Contratoid = ofertac.Proyecto.contratoId;
                ViewBag.Fechaoferta = ofertac.fecha_oferta.Value.ToShortDateString();
                return View("SubirExcel");
            }
            else
            {

                ViewBag.Error = "Tiene Observaciones Verifique";
                ViewBag.OfertaId = ofertaid;
                var ofertac = _ofertaService.getdetalle(ofertaid);
                ViewBag.Contratoid = ofertac.Proyecto.contratoId;
                ViewBag.Fechaoferta = ofertac.fecha_oferta.Value.ToShortDateString();
                return View("SubirExcel");
            }

        }

#pragma warning disable CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        public async Task<ActionResult> ExportarE(int id) //Pasar pametros oferta 
#pragma warning restore CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        {
            var brdo = _ofertaService.getdetalle(id);

            var presupuesto = _presupuestoService.ObtenerPresupuestoDefinitivo(brdo.RequerimientoId);

            int maximo_nivel_presupuesto = _wbspresupuestoService.nivel_mas_alto(presupuesto.Id);
            ExcelPackage paquetepresupuesto = _presupuestoService.GenerarExcelCargaPresupuestoRdo(presupuesto, maximo_nivel_presupuesto, true);

            int maximo_nivel = _wbsService.nivel_mas_alto(brdo.Id);
            ExcelPackage paqueterdo = _computoService.GenerarExcelCarga(brdo, maximo_nivel);

            paqueterdo.Workbook.Worksheets.Add("Presupuesto", paquetepresupuesto.Workbook.Worksheets[1]);


            string excelName = "Carga Rdo-Presupuesto";
            using (var memoryStream = new MemoryStream())
            {
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment; filename=" + excelName + ".xlsx");
                paqueterdo.SaveAs(memoryStream);
                memoryStream.WriteTo(Response.OutputStream);
                Response.Flush();
                Response.End();
                return Content("");
            }
        }


        public ActionResult ComputosOfertaApi(int id) // OfertaId
        {
            var computos = _computoService.GetComputosPorOferta(id);
            var result = JsonConvert.SerializeObject(computos);
            return Content(result);
        }


#pragma warning disable CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        public async Task<ActionResult> ExportarEAC(int id) //Pasar pametros oferta 
#pragma warning restore CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        {
            var brdo = _ofertaService.getdetalle(id);


            int maximo_nivel = _wbsService.nivel_mas_alto(brdo.Id);
            ExcelPackage paqueterdo = _computoService.GenerarExcelCargaEAC(brdo, maximo_nivel);

            string excelName = "Carga Masiva EAC" + brdo.codigo;
            using (var memoryStream = new MemoryStream())
            {
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment; filename=" + excelName + ".xlsx");
                paqueterdo.SaveAs(memoryStream);
                memoryStream.WriteTo(Response.OutputStream);
                Response.Flush();
                Response.End();
                return Content("OK");
            }
        }


        [System.Web.Mvc.HttpPost]
        public ActionResult SubirExcelEAC(HttpPostedFileBase UploadedFile, int Id) //Base Rdo
        {
            var brdo = _ofertaService.getdetalle(Id);
            int maximo_nivel = _wbsService.nivel_mas_alto(brdo.Id);
            string resultado = _computoService.CargaMasivaEAC(UploadedFile, Id, maximo_nivel);

            return Content(resultado);

        }

        public ActionResult ComputoGetInfo(int id) // ComputoId
        {
            var computo = _computoService.GetInfo(id);
            var result = JsonConvert.SerializeObject(computo);
            return Content(result);
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult ComputoActiveTemporal(int id,bool es_temporal) // ComputoId
        {
            var result = _computoService.ActiveEsTemporal(id,es_temporal);
            return Content(result?"OK":"ERROR");
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult ComputoActiveAjustado(int id, bool cantidadAjustada,string tipo) // ComputoId
        {
            var result = _computoService.ChangeCantidadAjustada(id,cantidadAjustada,tipo);
            return Content(result ? "OK" : "ERROR");
        }

    }
}

