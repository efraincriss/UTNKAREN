using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Abp.Application.Services.Dto;
using AutoMapper;
using com.cpp.calypso.framework;
using com.cpp.calypso.proyecto.aplicacion;
using com.cpp.calypso.proyecto.aplicacion.Dto;
using com.cpp.calypso.proyecto.aplicacion.Interfaces;
using com.cpp.calypso.proyecto.dominio;
using com.cpp.calypso.proyecto.dominio.Constantes;
using com.cpp.calypso.web.Areas.Proyecto.Models;
using Newtonsoft.Json;

namespace com.cpp.calypso.web.Areas.Proyecto.Controllers
{

    public class TransmitalCabeceraController : BaseController
    {
        private readonly IEmpresaAsyncBaseCrudAppService _empresaService;
        private readonly IContratoAsyncBaseCrudAppService _contratoService;
        private readonly IProyectoAsyncBaseCrudAppService _proyectoService;
        private readonly IClienteAsyncBaseCrudAppService _clienteService;
        private readonly IOfertaAsyncBaseCrudAppService _ofertaService;
        private readonly ITransmitalCabeceraAsyncBaseCrudAppService _transmitalCabeceraService;
        private readonly ITransmitalDetalleAsyncBaseCrudAppService _transmitalDetalleService;
        private readonly IColaboradorAsyncBaseCrudAppService _colaboradorsService;
        private readonly IOfertaComercialAsyncBaseCrudAppService _Service;
        private readonly IListaDistribucionAsyncBaseCrudAppService _listaService;

        public IArchivoAsyncBaseCrudAppService ArchivoService { get; }

        public TransmitalCabeceraController(IHandlerExcepciones manejadorExcepciones,
            ITransmitalCabeceraAsyncBaseCrudAppService transmitalCabeceraService,
            IOfertaAsyncBaseCrudAppService ofertaService,
            ITransmitalDetalleAsyncBaseCrudAppService transmitalDetalleService,
            IEmpresaAsyncBaseCrudAppService empresaService,
            IClienteAsyncBaseCrudAppService clienteService,
            IProyectoAsyncBaseCrudAppService proyectoService,
             IContratoAsyncBaseCrudAppService contratoService,
             IColaboradorAsyncBaseCrudAppService colaboradorsService,
             IOfertaComercialAsyncBaseCrudAppService Service,
             IArchivoAsyncBaseCrudAppService archivoService,
              IListaDistribucionAsyncBaseCrudAppService listaService
            ) : base(manejadorExcepciones)
        {
            _transmitalCabeceraService = transmitalCabeceraService;
            _ofertaService = ofertaService;
            _transmitalDetalleService = transmitalDetalleService;
            _empresaService = empresaService;
            _clienteService = clienteService;
            _proyectoService = proyectoService;
            _contratoService = contratoService;
            _colaboradorsService = colaboradorsService;
            _Service = Service;
            ArchivoService = archivoService;
            _listaService = listaService;
        }
        // GET: Proyecto/TransmitalCabecera
        public async System.Threading.Tasks.Task<ActionResult> Index(int id)
        {
            var oferta = await _ofertaService.Get(new EntityDto<int>(id));
            var listacabeceras = _transmitalCabeceraService.GetTransmitalCabeceras(id);
            OfertaTranmitalCabeceraViewModel o = new OfertaTranmitalCabeceraViewModel
            {
                Oferta = oferta,
                TransmitalCabeceras = listacabeceras
            };
            return View(o);
        }

        public ActionResult IndexTransmital(string message)
        {
            if (message != null)
            {

                ViewBag.Msg = message;
            }


            return View();
        }
        public ActionResult IndexUsuarios()
        {
            return View();
        }

        // GET: Proyecto/TransmitalCabecera/Details/5
        public ActionResult Details(int id, int id2 = 0)
        {/*
            TransmitalCabeceraDto cabecera = _transmitalCabeceraService.GetDetalle(id);

            var listadetalles = _transmitalDetalleService.GetTransmitalDetalles(id);
            TransmitalCabeceraDetalleViewModel o = new TransmitalCabeceraDetalleViewModel
            {
                TransmitalCabecera = cabecera,
                DetallesTransmital = listadetalles

            };

            */
            ViewBag.TransmittalId = id;
            ViewBag.OfertaComercialId = id2;
            return View();
        }

        // GET: Proyecto/TransmitalCabecera/Details/5
        public ActionResult formato(int id)
        {
            TransmitalCabeceraDto cabecera = _transmitalCabeceraService.GetDetalle(id);
            var listadetalles = _transmitalDetalleService.GetTransmitalDetalles(id);
            TransmitalCabeceraDetalleViewModel o = new TransmitalCabeceraDetalleViewModel
            {
                TransmitalCabecera = cabecera,
                DetallesTransmital = listadetalles

            };
            return PartialView("formato", o);
        }
        //Create Sin Oferta
        // GET: Proyecto/TransmitalCabecera/Create
        public ActionResult Createso()
        {
            TransmitalCabeceraDto a = new TransmitalCabeceraDto();

            a.fecha_emision = DateTime.Now;


            return View(a);
        }

        // POST: Proyecto/TransmitalCabecera/Create
        [System.Web.Mvc.HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> Createso(TransmitalCabeceraDto t)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (t.OfertaComercialId == 0)
                    {
                        t.OfertaComercialId = null;
                        t.ContratoId = null;
                        t.ClienteId = null;
                    }
                    String copias = "";
                    if (t.SelectedValues != null && t.SelectedValues.Count() > 0)
                    {
                        foreach (var item in t.SelectedValues)
                        {
                            copias = item + " , ";
                        }
                        t.copia_a = copias;
                    }
                    t.vigente = true;
                    var result = await _transmitalCabeceraService.Create(t);
                    return RedirectToAction("IndexTransmital", "TransmitalCabecera");
                }
                return View("Create", Mapper.Map<TransmitalCabeceraDto>(t));
            }
            catch
            {
                return View();
            }
        }

        //oferta
        // GET: Proyecto/TransmitalCabecera/Create
        public ActionResult Create(int id)
        {
            OfertaDto o = _ofertaService.getdetalle(id);
            ProyectoDto p = _proyectoService.GetDetalles(o.Requerimiento.ProyectoId);
            TransmitalCabeceraDto a = new TransmitalCabeceraDto();
            a.OfertaComercialId = id;
            a.EmpresaId = o.Proyecto.Contrato.Empresa.Id;
            a.ClienteId = o.Proyecto.Contrato.Cliente.Id;
            a.ContratoId = o.Proyecto.Contrato.Id;
            a.codigo_transmital = o.Proyecto.Contrato.codigo_generado + "-B-TD-" + (_transmitalCabeceraService.GetTransmitalCabeceras(id).Count + 1);
            a.descripcion = p.codigo + " " + o.descripcion;
            a.fecha_emision = DateTime.Now;

            return View(a);
        }

        // POST: Proyecto/TransmitalCabecera/Create
        [System.Web.Mvc.HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> Create(TransmitalCabeceraDto t)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    t.vigente = true;
                    var result = await _transmitalCabeceraService.Create(t);
                    return RedirectToAction("Index", "TransmitalCabecera", new { id = result.OfertaComercialId });
                }
                return View("Create", Mapper.Map<TransmitalCabeceraDto>(t));
            }
            catch
            {
                return View();
            }
        }

        // GET: Proyecto/TransmitalCabecera/Edit/5
        public ActionResult Edit(int id)
        {
            TransmitalCabeceraDto e = _transmitalCabeceraService.GetDetalle(id);

            return View(e);
        }

        // POST: Proyecto/TransmitalCabecera/Edit/5
        [System.Web.Mvc.HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> Edit(TransmitalCabeceraDto t)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _transmitalCabeceraService.Update(t);
                    return RedirectToAction("IndexTransmital", "TransmitalCabecera", new { id = result.OfertaComercialId });
                }
                return View("Edit", Mapper.Map<TransmitalCabeceraDto>(t));
            }
            catch
            {
                return View();
            }
        }

        // POST: Proyecto/TransmitalCabecera/Delete/5
        [System.Web.Mvc.HttpPost]
#pragma warning disable CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        public async System.Threading.Tasks.Task<ActionResult> Delete(int id)
#pragma warning restore CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        {
            try
            {
                TransmitalCabeceraDto tde = _transmitalCabeceraService.GetDetalle(id);
                var td = _transmitalCabeceraService.EliminarVigencia(id);

                if (td)
                {

                    return RedirectToAction("IndexTransmital", "TransmitalCabecera");
                }
                else
                {
                    String m = "No se puede eliminar porque tiene datos relacionados.";
                    return RedirectToAction("IndexTransmital", "TransmitalCabecera", new { message = m });
                }

            }
            catch
            {
                return View();
            }
        }
        public ActionResult ObtenerContratosEC(int id, int id2)
        {
            List<ContratoDto> listacontratos = _contratoService.GetContratosporEC(id, id2);


            var resultado = JsonConvert.SerializeObject(listacontratos,
                Newtonsoft.Json.Formatting.None,
                new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,

                });

            return Content(resultado);
        }

        public ActionResult ObtenerOfertas(int id)
        {
            List<OfertaDto> listaofertas = _ofertaService.ListarPorContrato(id);


            var resultado = JsonConvert.SerializeObject(listaofertas,
                Newtonsoft.Json.Formatting.None,
                new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });
            return Content(resultado);
        }

#pragma warning disable CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        public async Task<ActionResult> DetalleOferta(int id)
#pragma warning restore CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        {
            OfertaDto oferta = _ofertaService.getdetalle(id);
            ProyectoDto p = _proyectoService.GetDetalles(oferta.Requerimiento.ProyectoId);
            string contrato = p.Contrato.codigo_generado;
            int cont = (_transmitalCabeceraService.GetTransmitalCabeceras(oferta.Id).Count() + 1);
            ItemModelo n = new ItemModelo
            {
                codigocliente = oferta.Proyecto.Contrato.Cliente.codigoasignado,
                descripcionproyecto = oferta.Proyecto.descripcion_proyecto,
                descripcionoferta = oferta.descripcion,
                contador = cont,
                contrato = contrato
            };

            var resultado = JsonConvert.SerializeObject(n,
                Newtonsoft.Json.Formatting.None,
                new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });
            return Content(resultado);
        }
        public ActionResult UrlAsPDF(int id)
        {


            return Content("");
        }

        [System.Web.Http.HttpPost]
        public ActionResult CrearTransmital([FromBody] TransmitalCabecera transmistal)
        {
            if (ModelState.IsValid)
            {
                var mensaje = _transmitalCabeceraService.CrearTransmital(transmistal);
                return Content(mensaje);
            }
            else
            {
                return Content("ERROR");
            }

        }

        [System.Web.Http.HttpPost]
        public ActionResult EditarTransmital([FromBody] TransmitalCabecera transmistal)
        {
            if (ModelState.IsValid)
            {
                var mensaje = _transmitalCabeceraService.EditarTransmital(transmistal);
                return Content(mensaje);
            }
            else
            {
                return Content("ERROR");
            }

        }

        public ActionResult CrearTransmitalOfertaComercial(int id, [FromBody] TransmitalCabecera transmistal)
        {
            if (ModelState.IsValid){

                var mensaje = _transmitalCabeceraService.CrearTransmitalOfertaComercial(id, transmistal);

                return Content(mensaje);
            }
            return Content("ERROR");

        }

        #region Api

        public ActionResult ObtenerEmpresaCliente(int id) //ContratoId
        {
            var listaclientesempresas = _contratoService.ListaEmpresaClienteporContrato(id);
            var result = JsonConvert.SerializeObject(listaclientesempresas);
            return Content(result);
        }

        public ActionResult ObtenerColaboradores() //ContratoId
        {
            var colaboradores = _transmitalCabeceraService.ListaColaboradoresTransmital();
            var resultado = JsonConvert.SerializeObject(colaboradores,
                 Newtonsoft.Json.Formatting.None,
                 new JsonSerializerSettings
                 {
                     NullValueHandling = NullValueHandling.Ignore
                 });
            return Content(resultado);
        }

        public ActionResult ObtenerWord(int id) //ContratoId
        {
            var word = _transmitalCabeceraService.GenerarWord(id);
            string path = (word);
            string name = Path.GetFileName(path);
            string ext = Path.GetExtension(path);
            string type = "";

            if (ext != null)
            {
                switch (ext.ToLower())
                {
                    case ".htm":
                    case ".html":
                        type = "text/HTML";
                        break;

                    case ".txt":
                        type = "text/plain";
                        break;

                    case ".GIF":
                        type = "image/GIF";
                        break;

                    case ".pdf":
                        type = "Application/pdf";
                        break;

                    case ".doc":
                    case ".docx":
                    case ".rtf":
                        type = "Application/msword";
                        break;
                }
            }

            Response.AppendHeader("content-disposition", "inline; filename=" + name);

            if (type != "")
                Response.ContentType = type;
            Response.WriteFile(path);

            Response.End();

            return Content("");
        }


        #endregion
        public ActionResult ObtenerTransmitalUsers()
        {
            var list = _colaboradorsService.ListAll();
            var result = JsonConvert.SerializeObject(list);
            return Content(result);
        }
        public ActionResult ObtenerTypes()
        {
            var list = _colaboradorsService.ListTypesUser();
            var result = JsonConvert.SerializeObject(list);
            return Content(result);
        }
        public ActionResult ObtenerCrear(Colaborador e)
        {
            var result = _colaboradorsService.CrearColaborador(e);
            if (result == -1)
            {
                return Content("MISMO");
            }
            else
            {
                return Content("OK");
            }

        }
        public ActionResult ObtenerEditar(Colaborador e)
        {
            var result = _colaboradorsService.EditColaborador(e);
            if (result == -1)
            {
                return Content("MISMO");
            }
            else
            {
                return Content("OK");
            }
        }
        public ActionResult ObtenerDelete(int Id)
        {
            var result = _colaboradorsService.DeleteColaborador(Id);
            return Content("OK");
        }
        //transmital
        public ActionResult ObtenerListTransmital() //Listado de Transmitals
        {
            var listatransmital = _Service.ListarTransmitals().OrderByDescending(c => c.fecha_emision).ToList();

            var codigo_transmital = _transmitalCabeceraService.secuencialTransmital(1);
            var model = new ModelTransmittal
            {
                list = listatransmital,
                codigo_transmital = "3808-B-TD-" + String.Format("{0:000000}", codigo_transmital)
            };

            var result = JsonConvert.SerializeObject(model);
            return Content(result);
        }
        public ActionResult ObtenerListTransmitalById(int Id) //Listado de Transmitals
        {
            var listatransmital = _Service.ListarTransmitals().OrderByDescending(c => c.fecha_emision).ToList();

            var result = JsonConvert.SerializeObject(listatransmital);
            return Content(result);
        }

        public ActionResult ObtenerListByContrato(int id) //Listado de Transmitals
        {
            var listatransmital = _Service.ListarTransmitalsPorContrato(id).OrderByDescending(c => c.fecha_emision).ToList();
            var codigo_transmital = _transmitalCabeceraService.secuencialTransmital(1);
            var model = new ModelTransmittal
            {
                list = listatransmital,
                codigo_transmital = "3808-B-TD-" + String.Format("{0:000000}", codigo_transmital)
            };

            var result = JsonConvert.SerializeObject(model);
            return Content(result);
        }
        public ActionResult ObtenerDetail(int id) //Listado de Transmitals
        {
            var data = _transmitalCabeceraService.GetDetalle(id);
            var result = JsonConvert.SerializeObject(data);
            return Content(result);
        }

        public ActionResult ObtenerAdjunto(int id) //Listado de Transmitals
        {
            var data = _transmitalDetalleService.GetTransmitalDetalles(id);
            var result = JsonConvert.SerializeObject(data);
            return Content(result);
        }

        public async Task<ActionResult> Descargar(int id)
        {
            var entity = await ArchivoService.Get(new EntityDto<int>(id));

            if (entity == null)
            {
                var msg = string.Format("El Archivo con identificacion {0} no existe",
                    id);

                return HttpNotFound(msg);
            }

            return File(entity.hash, entity.tipo_contenido, entity.nombre);
        }

        public ActionResult ObtenerCrearDetalle(TransmitalDetalle e, HttpPostedFileBase UploadedFile = null)
        {
            if (UploadedFile != null)
            {
                int archivoId = _transmitalCabeceraService.CrearArchivo(UploadedFile);
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
            var result = _transmitalCabeceraService.CrearDetalle(e);
            return Content(result ? "OK" : "ERROR");

        }
        public ActionResult ObtenerEditarDetalle(TransmitalDetalle e, HttpPostedFileBase UploadedFile = null)
        {

            if (UploadedFile != null)
            {
                int archivoId = _transmitalCabeceraService.CrearArchivo(UploadedFile);
                if (archivoId > 0)
                {
                    e.ArchivoId = archivoId;
                }

            }

            var data = _transmitalCabeceraService.EditDetalle(e);
            return Content(data);


        }

        public ActionResult ObtenerEliminarDetalle(int Id)
        {
            var result = _transmitalCabeceraService.DeleteDetalle(Id);
            return Content(result ? "OK" : "ERROR");

        }

        public ActionResult ObtenerOfertbyContrato(int Id) // Oferta Comercial por Contrato
        {
            var lsita = _Service.ListaContrato(Id);

            var result = JsonConvert.SerializeObject(lsita,
                Newtonsoft.Json.Formatting.None,
                new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    NullValueHandling = NullValueHandling.Ignore

                });
            return Content(result);
        }

        public ActionResult ObtenerDeleteTransmital(int Id)
        {
            var result = _transmitalCabeceraService.DeleteTransmital(Id);
            return Content(result);

        }
        public ActionResult ObtenerCorreosLista() //Listado de Transmitals
        {
            var data = _listaService.GetCorreosListos(CatalogosCodigos.LISTADISTRIBUCION_OFERTA_COMERCIAL);
            var result = JsonConvert.SerializeObject(data);
            return Content(result);
        }

        public ActionResult ObtenerSecuencialCliente(int Id) //Cliente Id
        {
            string data = "";
            var codigo_transmital = _transmitalCabeceraService.secuencialTransmital(Id);

            data = "3808-B-TD-" + String.Format("{0:000000}", codigo_transmital);
            var result = JsonConvert.SerializeObject(data);
            return Content(result);
        }
    }
}

