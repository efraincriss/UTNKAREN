using AutoMapper;
using com.cpp.calypso.framework;
using com.cpp.calypso.proyecto.aplicacion;
using com.cpp.calypso.proyecto.aplicacion.Interfaces;
using com.cpp.calypso.web.Areas.Proyecto.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using com.cpp.calypso.proyecto.dominio;
using Newtonsoft.Json;
using com.cpp.calypso.proyecto.dominio.Constantes;
using com.cpp.calypso.proyecto.dominio.Models;
using System.IO;

namespace com.cpp.calypso.web.Areas.Proyecto.Controllers
{

    public class CartaController : BaseController
    {
        // GET: Proyecto/Carta
        private readonly IEmpresaAsyncBaseCrudAppService _empresaService;
        private readonly IClienteAsyncBaseCrudAppService _clienteService;
        private readonly IDestinatarioAsyncBaseCrudAppService _destinatarioService;
        private readonly IDestinatarioCartaAsyncBaseCrudAppService _dcartaService;
        private readonly ICartaAsyncBaseCrudAppService _cartaService;
        private readonly ICartaArchivoAsyncBaseCrudAppService _cartaarchivosService;
        private readonly IListaDistribucionAsyncBaseCrudAppService _listaService;


        private readonly ICatalogoAsyncBaseCrudAppService _catalogoService;
        public CartaController(IHandlerExcepciones manejadorExcepciones,
            IEmpresaAsyncBaseCrudAppService empresaService,
            ICartaAsyncBaseCrudAppService cartaService,
            IClienteAsyncBaseCrudAppService clienteService,
            IDestinatarioAsyncBaseCrudAppService destinatarioService,
            IDestinatarioCartaAsyncBaseCrudAppService dcartaService,
            ICartaArchivoAsyncBaseCrudAppService cartaarchivosService,
            IListaDistribucionAsyncBaseCrudAppService listaService,
            ICatalogoAsyncBaseCrudAppService catalogoService
            ) : base(manejadorExcepciones)
        {

            _empresaService = empresaService;
            _cartaService = cartaService;
            _clienteService = clienteService;
            _destinatarioService = destinatarioService;
            _dcartaService = dcartaService;
            _cartaarchivosService = cartaarchivosService;
            _listaService = listaService;

            _catalogoService = catalogoService;
        }
        public ActionResult Index()
        {
            return View();

        }

        // GET: Proyecto/Carta/Details/5
        public ActionResult Details(int id)
        {
            var carta = _cartaService.getdetalle(id);
            /*var cartaarchivos = _cartaarchivosService.ListaArchivosporCarta(id);
            var track = _dcartaService.GetDestinatarioCartas(id);

            CartaArchivosModelView c = new CartaArchivosModelView
            {
                Carta = carta,
                ListaArchivos = cartaarchivos,
                Track = track
            };*/
            ViewBag.CartaId = carta.Id;

            return View(carta);
        }

        // GET: Proyecto/Carta/Create
        public ActionResult Create()
        {
            CartaDto n = new CartaDto();
            /* n.fecha_envio = DateTime.Now;
             n.estado = true;
             n.numero_carta = "3808-BT-LTE" + (_cartaService.GetCartaporTipo(TipoCarta.Enviada).Count() + 1);

             n.dirigido_a = "pendiente";

             n.enviado_por = "" + System.Web.HttpContext.Current.Session["usuario"];*/
            return View(n);
        }

        public ActionResult CreateFecha(int id, DateTime a, DateTime b)


        {

            return null;
        }

        // POST: Proyecto/Carta/Create
        [HttpPost]
        public async Task<ActionResult> Create(CartaDto c)
        {

            return View();

        }

        public ActionResult CreateR()
        {
            CartaDto n = new CartaDto();
            /* n.fecha_envio = DateTime.Now;
             n.estado = true;
             n.numero_carta = "3808-BT-LTR";
             //    + (_cartaService.GetCartaporTipo(2).Count() + 1);

             n.dirigido_a = "pendiente";
             n.enviado_por = "Session Usuario";*/
            return View(n);
        }

        // POST: Proyecto/Carta/Create
        [HttpPost]
        public async Task<ActionResult> CreateR(CartaDto c)
        {

            return View();
        }


        // GET: Proyecto/Carta/Edit/5
        public ActionResult Edit(int id)
        {
            var c = _cartaService.getdetalle(id);


            return View(c);
        }

        // POST: Proyecto/Carta/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(CartaDto c)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var r = await _cartaService.Update(c);
                    return RedirectToAction("Index");
                }
                else
                {
                    return View("Edit", Mapper.Map<CartaDto>(c));
                }

            }
            catch
            {
                return View();
            }
        }

        // POST: Proyecto/Carta/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                var result = _cartaService.EliminarVigencia(id);
                if (result)
                {
                    String message = "x";

                    return RedirectToAction("Index", new { r = message });
                }
                else
                {
                    String message = "e";


                    return RedirectToAction("Index", new { r = message });

                }

            }
            catch
            {
                return View();
            }
        }

        public ActionResult _ListaCartas(int id, int id2, int id3)
        {
            var ListaCartas = _cartaService.ListaCartasEmTi(id, id2, id3);

            return PartialView(ListaCartas);
        }

        public ActionResult ObtenerCrear(Carta c)
        {
            var id = _cartaService.InsertCarta(c);
            return Content(id > 0 ? "OK" : "ERROR");
        }
        public ActionResult ObtenerEditar(Carta c)
        {
            if (ModelState.IsValid)
            {
                var id = _cartaService.EditCarta(c);
                return Content(id > 0 ? "OK" : "ERROR");
            }
            else
            {
                return Content("Error");
            }
        }
        public ActionResult ObtenerEliminar(int id)
        {
            bool r = _cartaService.EliminarCarta(id);
            return Content(r ? "OK" : "ERROR");
        }
        public ActionResult ObtenerEmpresas()
        {
            var list = _cartaService.ListEmpresa();
            var result = JsonConvert.SerializeObject(list,
            Newtonsoft.Json.Formatting.None,

            new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore,


            });
            return Content(result);
        }
        public ActionResult ObtenerClientes()
        {
            var list = _cartaService.ListClientes();
            var result = JsonConvert.SerializeObject(list,
            Newtonsoft.Json.Formatting.None,

            new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore,


            });
            return Content(result);
        }




        public ActionResult ObtenerCrearDetalle(CartaArchivo e, HttpPostedFileBase UploadedFile = null)
        {
            if (UploadedFile != null)
            {
                int archivoId = _cartaService.CrearArchivo(UploadedFile);
                if (archivoId > 0)
                {
                    e.ArchivoId = archivoId;
                }
                else
                {
                }

            }
            else
            {
                e.ArchivoId = 0;
            }
            var result = _cartaService.CrearDetalle(e);
            return Content(result ? "OK" : "ERROR");

        }
        public ActionResult ObtenerEditarDetalle(CartaArchivo e, HttpPostedFileBase UploadedFile = null)
        {

            if (UploadedFile != null)
            {
                int archivoId = _cartaService.CrearArchivo(UploadedFile);
                if (archivoId > 0)
                {
                    e.ArchivoId = archivoId;
                }

            }

            var data = _cartaService.EditDetalle(e);
            return Content(data);


        }

        public ActionResult ObtenerEliminarDetalle(int Id)
        {
            var result = _cartaService.DeleteDetalle(Id);
            return Content(result ? "OK" : "ERROR");

        }
        public ActionResult ObtenerAdjuntos(int Id)
        {
            var cartaarchivos = _cartaarchivosService.ListaArchivosporCarta(Id);
            var result = JsonConvert.SerializeObject(cartaarchivos);
            return Content(result);
        }
        public ActionResult ObtenerDetail(int id) //Listado de Transmitals
        {
            var data = _cartaService.getdetalle(id);
            var result = JsonConvert.SerializeObject(data);
            return Content(result);
        }
        [System.Web.Http.HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> GetEnviar(int Id, int[] ListIds, string body = "")
        {
            var send = await _cartaService.Send_Files_Cartas(Id, ListIds, body);
            return Content(send);

        }
        public ActionResult ObtenerCorreosLista() //Listado de Transmitals
        {
            var data = _listaService.GetCorreosListos(CatalogosCodigos.LISTADISTRIBUCION_CARTAS);
            var result = JsonConvert.SerializeObject(data);
            return Content(result);
        }
        public ActionResult ObtenerListCarta()
        {
            var list = _cartaService.ListadoCartas();
            var result = JsonConvert.SerializeObject(list,
            Newtonsoft.Json.Formatting.None,

            new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore,


            });
            return Content(result);
        }
        public ActionResult ObtenerSecuencialCliente(int Id) //Cliente Id
        {
            string data = "";
            var numero_carta = _cartaService.secuencialCarta();

            data = "3808-B-LT-" + String.Format("{0:000000}", numero_carta);
            var result = JsonConvert.SerializeObject(data);
            return Content(result);
        }
        public ActionResult GetMailto(int Id, List<int> Ids)
        {
            var result = _cartaService.hrefoutlook(Id, Ids);
            return Content(result);
        }



        /*ES Cartas */

        public ActionResult GetByCodeApi(string code)
        {
            var entityDto = _catalogoService.ListarCatalogos(code);
            return WrapperResponseGetApi(ModelState, () => entityDto);

        }

        public ActionResult ObtenerList(int TipoCartaId)
        {
            var list = _cartaService.ListCarta(TipoCartaId);
            var numero_carta = _cartaService.secuencialCarta();

            var model = new ModelCartaList()
            {
                list = list,
                numero_carta = "3808-B-LT-" + String.Format("{0:000000}", numero_carta)
            };
            var result = JsonConvert.SerializeObject(model,
            Newtonsoft.Json.Formatting.None,

            new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore,


            });
            return Content(result);
        }
        public ActionResult ObtenerListExistentes()
        {
            //svar liscartas = _cartaService.ListCartasExistentes();
            var listdirigidos = _cartaService.ListaDistribucionCartas();
            var usuario = _cartaService.UsuarioActual();
            var result = JsonConvert.SerializeObject(new { Dirigidos = listdirigidos, Usuario = usuario },
            Newtonsoft.Json.Formatting.None,

            new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore,
            });
            return Content(result);
        }


        public ActionResult GetReporte()
        {
            var excel = _cartaService.Reporte();
            string excelName = "Reporte_CARTAS_" + DateTime.Now.ToShortDateString();
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

    }
}
