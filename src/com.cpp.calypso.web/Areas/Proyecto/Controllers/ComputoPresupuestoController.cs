using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Abp.Application.Services.Dto;
using com.cpp.calypso.framework;
using com.cpp.calypso.proyecto.aplicacion;
using com.cpp.calypso.proyecto.aplicacion.Interfaces;
using com.cpp.calypso.proyecto.dominio;
using com.cpp.calypso.web.Areas.Proyecto.Models;
using com.cpp.calypso.web.Models;
using Newtonsoft.Json;

namespace com.cpp.calypso.web.Areas.Proyecto
{
    public class ComputoPresupuestoController : BaseController
    {
        private readonly IComputoPresupuestoAsyncBaseCrudAppService _computoPresupuestoService;
        private readonly IPresupuestoAsyncBaseCrudAppService _presupuestoService;
        private readonly IWbsPresupuestoAsyncBaseCrudAppService _wbsPresupuestoService;
        private readonly IItemAsyncBaseCrudAppService _itemService;
        private readonly IPreciarioAsyncBaseCrudAppService _preciarioService;
        private readonly IDetallePreciarioAsyncBaseCrudAppService _detallePreciarioService;
        private readonly IRequerimientoAsyncBaseCrudAppService _RequerimientoService;

        // GET: Proyecto/ComputoPresupuesto
        public ComputoPresupuestoController(
            IHandlerExcepciones manejadorExcepciones,
            IComputoPresupuestoAsyncBaseCrudAppService computoPresupuestoService,
            IPresupuestoAsyncBaseCrudAppService presupuestoService,
            IWbsPresupuestoAsyncBaseCrudAppService wbsPresupuestoService,
            IItemAsyncBaseCrudAppService itemService,
            IPreciarioAsyncBaseCrudAppService preciarioService,
            IDetallePreciarioAsyncBaseCrudAppService detallePreciarioService,
            IRequerimientoAsyncBaseCrudAppService RequerimientoService
            ) : base(manejadorExcepciones)
        {
            _computoPresupuestoService = computoPresupuestoService;
            _presupuestoService = presupuestoService;
            _wbsPresupuestoService = wbsPresupuestoService;
            _itemService = itemService;
            _preciarioService = preciarioService;
            _detallePreciarioService = detallePreciarioService;
            _RequerimientoService = RequerimientoService;
        }


        public ActionResult IndexPresupuesto()
        {
            ViewBag.ruta = new string[] { "Inicio", "Ingeniería", "Computos Presupuesto" };


            var ofertas = _presupuestoService.ListaPresupuestoDefinitivos();
            return View(ofertas);
        }

        public async Task<ActionResult> Index(int? id, string mensaje, int principal = 0)
        {
            if (id.HasValue)
            {
                if (mensaje != null)
                {
                    ViewBag.msg = mensaje;
                }
                ViewBag.Principal = principal;
                var presu = await _presupuestoService.Get(new EntityDto<int>(id.Value));
                ViewBag.ruta = new string[] { "Inicio", "Ingeniería", "Computo", presu.codigo + "-" + presu.version, "Gestionar" };
                var computos = new List<ComputoPresupuestoDto>();
                OfertaPresupuestoComputoViewModel viewModel = new OfertaPresupuestoComputoViewModel()
                {
                    Oferta = presu,
                    Contrato = presu.Proyecto.Contrato
                };
                ViewBag.Formato = presu.Proyecto.Contrato.Formato.Value;

                return View(viewModel);
            }

            return RedirectToAction("Index", "Inicio", new { area = "" });
        }

        public ActionResult ComputosPresupuesto(int id) // PresupuestoId
        {
            var computos = _computoPresupuestoService.GetComputosPorPresupuesto(id);
            var result = JsonConvert.SerializeObject(computos);
            return Content(result);
        }



        public ActionResult ApiComputo(int id)
        {
            var lsita = _wbsPresupuestoService.GenerarArbolComputo(id);
            var result = JsonConvert.SerializeObject(lsita,
                Newtonsoft.Json.Formatting.None,
                new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });
            return Content(result);
        }

        [System.Web.Mvc.HttpPost]
#pragma warning disable CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        public async Task<ActionResult> DeleteComputoArbol(int id)
#pragma warning restore CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        {
            var r = _computoPresupuestoService.EliminarVigencia(id);

            if (r)
            {
                return Content("Eliminado");
            }
            return Content("ErrorEliminado");

        }


        public ActionResult ItemsparaOfertaC(int id, [FromBody]DateTime f)
        {
            var lsita = _itemService.GetItemsporContratoActivo2(id, f);
            var result = JsonConvert.SerializeObject(lsita,
                Newtonsoft.Json.Formatting.None,
                new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });
            return Content(result);
        }

        public ActionResult ItemsProcura()
        {
            var lsita = _itemService.GetItemsHijos(".").Where(c => c.GrupoId == 3);
            var result = JsonConvert.SerializeObject(lsita,
                Newtonsoft.Json.Formatting.None,
                new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });
            return Content(result);
        }


        [System.Web.Mvc.HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> CreateComputoArbol(ComputoPresupuestoDto computoDto, [FromBody]string nuevonombrei)
        {

            var wbs = await _wbsPresupuestoService.Get(new EntityDto<int>(computoDto.WbsPresupuestoId));
            var ofertac = await _presupuestoService.Get(new EntityDto<int>(wbs.PresupuestoId));
            var Item = _itemService.DatosItem(computoDto.ItemId);
            computoDto.fecha_registro = DateTime.Now;
            computoDto.estado = true;
            computoDto.vigente = true;
            computoDto.Cambio = ComputoPresupuesto.TipoCambioComputo.Nuevo;
            if (Item != null && Item.Id > 0)
            {
                if (Item.GrupoId == 3)
                {
                    string[] data = Item.codigo.Split('.');
                    computoDto.codigo_item_alterno = data[0] + "." + ofertac.codigo + "." + (_computoPresupuestoService.GetComputosporOfertaProcura(ofertac.Id) + 1);
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
            bool e = _computoPresupuestoService.comprobarexistenciaitem(computoDto.WbsPresupuestoId, computoDto.ItemId);
            if (!e)
            {
                var computo = await _computoPresupuestoService.InsertOrUpdateAsync(computoDto);
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
            [FromBody]string codigo,
            [FromBody]decimal precio,
            [FromBody]int cantidad,
            [FromBody]int unidadid,
            [FromBody]int npadre)
        {
            string mensaje = "";

            var wbs = await _wbsPresupuestoService.Get(new EntityDto<int>(WbsId));
            var ofertac = await _presupuestoService.Get(new EntityDto<int>(wbs.PresupuestoId));
            var contador = _computoPresupuestoService.GetComputosporOfertaProcura(wbs.PresupuestoId);
            var ipadre = await _itemService.GetDetalle(npadre);

            if (ipadre.Id > 0)
            {
                var ihijos = _itemService.GetItemsHijos(ipadre.codigo);
                ItemDto iadicional = new ItemDto
                {
                    Id = 0,
                    codigo = ipadre.codigo + (ihijos.Count + 1),
                    item_padre = ipadre.codigo,
                    GrupoId = 3,
                    descripcion = nombre,
                    UnidadId = unidadid,
                    nombre = nombre,
                    vigente = true,
                    para_oferta = true
                };
                var itemnuevo = await _itemService.Create(iadicional);
                if (itemnuevo.Id > 0)
                {
                    var adicional = new ComputoPresupuestoDto
                    {
                        Id = 0,
                        WbsPresupuestoId = WbsId,
                        cantidad = cantidad,
                        ItemId = itemnuevo.Id,
                        estado = true,
                        vigente = true,
                        fecha_registro = DateTime.Now,
                        cantidad_eac = cantidad,
                        Cambio = ComputoPresupuesto.TipoCambioComputo.Nuevo,
                        codigo_item_alterno = codigo
                    };
                    var rcomputo = await _computoPresupuestoService.Create(adicional);
                    if (rcomputo.Id > 0)
                    {
                        var c = await _computoPresupuestoService.Get(new EntityDto<int>(rcomputo.Id));
                        var wbsoferta = await _wbsPresupuestoService.Get(new EntityDto<int>(c.WbsPresupuestoId));
                        var oferta = await _presupuestoService.Get(new EntityDto<int>(wbsoferta.PresupuestoId));
                        var fecha = oferta.fecha_registro.Value;
                        var preciario = _preciarioService.preciarioporcontratofecha(oferta.Proyecto.contratoId, fecha);

                        if (preciario.Id > 0)
                        {
                            DetallePreciarioDto a = new DetallePreciarioDto
                            {
                                Id = 0,
                                ItemId = rcomputo.ItemId,
                                precio_unitario = precio,
                                vigente = true,
                                PreciarioId = preciario.Id
                            };
                            var dp = await _detallePreciarioService.Create(a);
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

        [System.Web.Mvc.HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> EditComputo(ComputoPresupuestoDto computoDto, [FromBody] decimal cantidad_eac = 0)
        {
            var e = await _computoPresupuestoService.Get(new EntityDto<int>(computoDto.Id));
            bool r = _computoPresupuestoService.EditarCantidadComputo(e.Id, computoDto.cantidad, cantidad_eac);
            return Content(r == true ? "OK" : "Error");
        }


        [System.Web.Mvc.HttpPost]
        public async Task<ActionResult> EditPrecioAjustado(ComputoPresupuesto computo)
        {
            var r = _computoPresupuestoService.ActualizarprecioAjustado(computo);
            var wbs = await _wbsPresupuestoService.Get(new EntityDto<int>(r.WbsPresupuestoId));
            _presupuestoService.CalcularMontosPresupuesto(wbs.PresupuestoId);
            return Content(r.Id > 0 ? "OK" : "Error");
        }

        public ActionResult ItemsSecondFormat(int id)
        {
            var data = _presupuestoService.TotalesSecondFormatPresupuesto(id);
            bool req = _RequerimientoService.actualizarmontosrequerimiento(id, data.A_VALOR_COSTO_TOTAL_INGENIERÍA_BASICA_YDETALLE_AIU_ANEXO1, data.D_VALOR_COSTO_DIRECTO_CONSTRUCCIÓN, data.B_VALOR_COSTO_DIRECTO_PROCURA_CONTRATISTA_ANEXO2, data.C_VALOR_COSTO_DIRECTO_SUBCONTRATOS_CONTRATISTA, data.COSTO_TOTAL_DEL_PROYECTO_ABCDE);

            var result = JsonConvert.SerializeObject(data);
            return Content(result);
        }
    }
}