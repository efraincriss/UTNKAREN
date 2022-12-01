using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Abp.Application.Services.Dto;
using AutoMapper;
using com.cpp.calypso.framework;
using com.cpp.calypso.proyecto.aplicacion;
using com.cpp.calypso.proyecto.aplicacion.Dto;
using com.cpp.calypso.proyecto.aplicacion.Interfaces;
using com.cpp.calypso.proyecto.dominio;

namespace com.cpp.calypso.web.Areas.Proyecto.Controllers
{
   
    public class TransmitalDetalleController : BaseController
    {
        private readonly IOfertaComercialAsyncBaseCrudAppService _ofertaService;
        private readonly IArchivoAsyncBaseCrudAppService _archivoService;
        private readonly ITransmitalCabeceraAsyncBaseCrudAppService _transmitalCabeceraService;
        private readonly ITransmitalDetalleAsyncBaseCrudAppService _transmitalDetalleService;
        public TransmitalDetalleController(IHandlerExcepciones manejadorExcepciones,
            ITransmitalCabeceraAsyncBaseCrudAppService transmitalCabeceraService,
            ITransmitalDetalleAsyncBaseCrudAppService transmitalDetalleService,
            IArchivoAsyncBaseCrudAppService archivoService,
        IOfertaComercialAsyncBaseCrudAppService ofertaService) : base(manejadorExcepciones)
        {
           _transmitalCabeceraService = transmitalCabeceraService;
            _transmitalDetalleService = transmitalDetalleService;
            _ofertaService = ofertaService;
            _archivoService = archivoService;

        }
        // GET: Proyecto/TransmitalDetalle
        public ActionResult Index()
        {
            return View();
        }

        // GET: Proyecto/TransmitalDetalle/Details/5
        public ActionResult Details(int id)
        {
            TransmitalDetalleDto e = _transmitalDetalleService.GetDetalle(id);
            return View(e);
        }
        //sin oferta
        // GET: Proyecto/TransmitalDetalle/Create
#pragma warning disable CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        public async System.Threading.Tasks.Task<ActionResult> Createso(int id)
#pragma warning restore CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        {
            TransmitalDetalleDto td = new TransmitalDetalleDto();
            td.TransmitalId = id;
            td.codigo_detalle = "Anexo #" + "-" + (_transmitalDetalleService.GetTransmitalDetalles(id).Count() + 1);
            td.descripcion = "N/A";
            td.version= "N/A";

            ViewBag.SO =1;
            return View("Create",td);
        }

        // POST: Proyecto/TransmitalDetalle/Create
        [HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> Create(TransmitalDetalleDto td, HttpPostedFileBase UploadedFile)

        {
            try
            {
                if (ModelState.IsValid)
                {
                    td.vigente = true;

                    if (UploadedFile != null)
                    {
                        string fileName = UploadedFile.FileName;
                        string fileContentType = UploadedFile.ContentType;
                        byte[] fileBytes = new byte[UploadedFile.ContentLength];
                        var data = UploadedFile.InputStream.Read(fileBytes, 0,
                            Convert.ToInt32(UploadedFile.ContentLength));

                        ArchivoDto n = new ArchivoDto
                        {
                            Id = 0,
                            codigo = "archivo-prueba",
                            nombre = fileName,
                            vigente = true,
                            fecha_registro = DateTime.Now,
                            hash = fileBytes,
                            tipo_contenido = fileContentType,
                        };
                        var archivo = await _archivoService.Create(n);
                   

                    td.ArchivoId = archivo.Id;
                }

                    var transmitalcabecera=await _transmitalCabeceraService.Get(new EntityDto<int>(td.TransmitalId));

                    if (td.es_oferta && transmitalcabecera.OfertaComercialId.HasValue) {

                        bool existeesoferta = _transmitalDetalleService.existe_esoferta(transmitalcabecera.Id);
                        if (existeesoferta)
                        {
                            ViewBag.Msg = "Ya Existe un Archivo con el  código de la Oferta";
                            return View("Create", Mapper.Map<TransmitalDetalleDto>(td));

                        }

                        var oferta = await _ofertaService.Get(new EntityDto<int>(transmitalcabecera.OfertaComercialId.Value));
                        td.codigo_detalle=oferta.codigo;


                    }

                var result = await _transmitalDetalleService.Create(td);
                    return RedirectToAction("Details", "TransmitalCabecera", new { id = result.TransmitalId });
                }

                return View("Create", Mapper.Map<TransmitalDetalleDto>(td));
            }
            catch
            {
                return View();
            }
        }

        // GET: Proyecto/TransmitalDetalle/Create
        public async System.Threading.Tasks.Task<ActionResult> Create(int id ,int ofertaid)
        {
            TransmitalDetalleDto td= new TransmitalDetalleDto();
            var oferta = await _ofertaService.Get(new EntityDto<int>(ofertaid));
            td.TransmitalId = id;
            td.version = oferta.version;
            string codigo_oferta = oferta.codigo.Split('-')[oferta.codigo.Split('-').Length-1];

            td.codigo_detalle = "Anexo Oferta # " + codigo_oferta;
            td.descripcion = oferta.descripcion;
            td.nro_hojas = 1;
            td.nro_copias = 1;

            bool existeesoferta = _transmitalDetalleService.existe_esoferta(id);
            if (existeesoferta)
            {
                ViewBag.hayoferta = true;

            }
            else {
                ViewBag.hayoferta = false;
            }
            return View(td);
        }

        // POST: Proyecto/TransmitalDetalle/Create
        [HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> Createso(TransmitalDetalleDto td, HttpPostedFileBase UploadedFile)

        {
            try
            {
                if (ModelState.IsValid)
                {
                    td.vigente = true;

                    if (UploadedFile != null)
                    {
                        string fileName = UploadedFile.FileName;
                        string fileContentType = UploadedFile.ContentType;
                        byte[] fileBytes = new byte[UploadedFile.ContentLength];
                        var data = UploadedFile.InputStream.Read(fileBytes, 0,
                            Convert.ToInt32(UploadedFile.ContentLength));

                        ArchivoDto n = new ArchivoDto
                        {
                            Id = 0,
                            codigo = "archivo-prueba",
                            nombre = fileName,
                            vigente = true,
                            fecha_registro = DateTime.Now,
                            hash = fileBytes,
                            tipo_contenido = fileContentType,
                        };
                        var archivo = await _archivoService.Create(n);


                        td.ArchivoId = archivo.Id;
                    }

                    var result = await _transmitalDetalleService.Create(td);
                    return RedirectToAction("Details", "TransmitalCabecera", new { id = result.TransmitalId});
                }

                return View("Create", Mapper.Map<TransmitalDetalleDto>(td));
            }
            catch
            {
                return View();
            }
        }

        // GET: Proyecto/TransmitalDetalle/Edit/5
        public ActionResult Edit(int id)
        {
            TransmitalDetalleDto e = _transmitalDetalleService.GetDetalle(id);

            return View(e);
        }

        // POST: Proyecto/TransmitalDetalle/Edit/5
        [HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> Edit(TransmitalDetalleDto td, HttpPostedFileBase UploadedFile)
        {

            try
            {
                if (ModelState.IsValid)
                {
                    if (UploadedFile != null)
                    {
                        string fileName = UploadedFile.FileName;
                        string fileContentType = UploadedFile.ContentType;
                        byte[] fileBytes = new byte[UploadedFile.ContentLength];
                        var data = UploadedFile.InputStream.Read(fileBytes, 0,
                            Convert.ToInt32(UploadedFile.ContentLength));

                        ArchivoDto n = new ArchivoDto
                        {
                            Id = 0,
                            codigo = "archivo-prueba",
                            nombre = fileName,
                            vigente = true,
                            fecha_registro = DateTime.Now,
                            hash = fileBytes,
                            tipo_contenido = fileContentType,
                        };
                        var archivo = await _archivoService.Create(n);


                        td.ArchivoId = archivo.Id;
                    }

                    var result = await _transmitalDetalleService.Update(td);
                    return RedirectToAction("Details", "TransmitalCabecera", new { id = result.TransmitalId });
                }
                return View("Edit", Mapper.Map<TransmitalDetalleDto>(td));
            }
            catch
            {
                return View();
            }
        }

        // POST: Proyecto/TransmitalDetalle/Delete/5
        [HttpPost]
#pragma warning disable CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        public async System.Threading.Tasks.Task<ActionResult> Delete(int id)
#pragma warning restore CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        {
            try
            {
                TransmitalDetalleDto tde =  _transmitalDetalleService.GetDetalle(id);
                var td = _transmitalDetalleService.EliminarVigencia(id);
             
                if (td) {

                    return RedirectToAction("Details", "TransmitalCabecera", new { id = tde.TransmitalId });
                }
                return View();
            }
            catch
            {
                return View();
            }
        }
     
        public async System.Threading.Tasks.Task<ActionResult> descargararchivo(int id)
        {
            var a =await  _archivoService.Get(new EntityDto<int>(id));
            return File(a.hash, a.tipo_contenido,a.nombre);
        }

        [HttpPost]
        public void Subir(HttpPostedFileBase file)
        {
            if (file == null) return;

            string archivo = (DateTime.Now.ToString("yyyyMMddHHmmss") + "-" + file.FileName).ToLower();

            file.SaveAs(Server.MapPath("~/Uploads/" + archivo));
        }
    }
}
