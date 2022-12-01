using com.cpp.calypso.framework;
using com.cpp.calypso.proyecto.aplicacion;
using com.cpp.calypso.proyecto.aplicacion.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Abp.Application.Services.Dto;

namespace com.cpp.calypso.web.Areas.Proyecto.Controllers
{
       public class AdendaController :BaseController
    {

        private readonly IArchivoAsyncBaseCrudAppService _archivoService;
        private readonly IAdendaAsyncBaseCrudAppService adendaService;
        private readonly IContratoAsyncBaseCrudAppService _contratoService;
        public AdendaController(IHandlerExcepciones manejadorExcepciones, IAdendaAsyncBaseCrudAppService adendaService,
            IContratoAsyncBaseCrudAppService contratoService,
            IArchivoAsyncBaseCrudAppService archivoService) :
             base(manejadorExcepciones)
        {
            this.adendaService = adendaService;
            _contratoService = contratoService;
            _archivoService = archivoService;
        }

        // GET: Proyecto/Adenda
        public ActionResult Index()
        {
            return View();
        }

        // GET: Proyecto/Adenda/Details/5

  

        public ActionResult Details(int? id)
        {
            if (id.HasValue)
            {
                var adenda = adendaService.GetDetalle(id.Value);
                if (adenda != null)
                {
                    return View(adenda);
                }
            }
            return RedirectToAction("Index", "Contrato");
        }
       
        // GET: Proyecto/Adenda/Create
        public ActionResult Create(int? id)
        {
            if (id.HasValue)
            {
                AdendaDto adenda = new AdendaDto();
                adenda.fecha = DateTime.Now;
              adenda.ContratoId = id.Value;
                return View(adenda);
            }
            else
            {
                return RedirectToAction("Index", "Contrato");
            }

        }

        // POST: Proyecto/Adenda/Create
        [HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> Create(AdendaDto adenda, HttpPostedFileBase UploadedFile)
        {
            try
            {

                if (ModelState.IsValid)
                {
                   adenda.vigente = true;
                    var contrato = await _contratoService.GetDetalle(adenda.ContratoId); 
                   bool r = adendaService.comprobarfechaadenda(adenda.fecha, contrato.fecha_inicio);

                    if (r)
                    {
                        var arhivoid =
                            adendaService.GuardarArchivo(adenda.ContratoId, UploadedFile);
                        adenda.ArchivosContratoId = arhivoid;
                        var adenda_contrato = await adendaService.Create(adenda);

                        return RedirectToAction("Details", "Contrato", new { id = adenda.ContratoId });
                    }
                    else {
                        ViewBag.Error = "La fecha de la adenda no puede ser menor a la fecha del contrato";
                        return View("Create", adenda);
                    }
                }

                return View("Create", adenda);
               

            }
            catch
            {
                return View();
            }
        }

        // GET: Proyecto/Adenda/Edit/5
        public ActionResult Edit(int? id)
        {
            if (!id.HasValue)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                //var input = new comun.aplicacion.PagedAndFilteredResultRequestDto();
                //input.Filter.FirstOrDefault()
                var centrocostocontratoDto = adendaService.GetDetalle(id.Value);

                if (centrocostocontratoDto == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.NotFound);
                }
                else
                {
                    return View(centrocostocontratoDto);
                }
            }
        }

        // POST: Proyecto/Adenda/Edit/5
        [HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> Edit(int id, AdendaDto adendaDto)
        {
            try
            {
                //empresaService.InsertOrUpdateAsync(empresaDto);
                var contratos = await _contratoService.GetDetalle(adendaDto.ContratoId);
                bool r = adendaService.comprobarfechaadenda(adendaDto.fecha, contratos.fecha_inicio);

                if (r)
                {
                    var contrato = await adendaService.InsertOrUpdateAsync(adendaDto);
                return RedirectToAction("Details","Contrato",new {id=adendaDto.ContratoId });
                }
                else
                {
                    ViewBag.Error = "La fecha de la adenda no puede ser menor a la fecha del contrato";
                    return View("Edit", adendaDto);
                }
            }
            catch
            {
                return View();
            }
        }

     

        // POST: Proyecto/Adenda/Delete/5
       [HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> Delete(int? id)
        {
            int _id_contrato = 0;
            try
            {
                
                if (id.HasValue)
                {
                    AdendaDto adenda = adendaService.GetDetalle(id.Value);
                    adenda.vigente = false;
                    _id_contrato = adenda.ContratoId;
                   await adendaService.InsertOrUpdateAsync(adenda);

                    return RedirectToAction("Details", "Contrato", new { id = _id_contrato });
                    //empresaService.CancelarVigencia(id.Value);
                }
                return RedirectToAction("Details", "Contrato", new { id = _id_contrato });

            }
            catch
            {
                return RedirectToAction("Details", "Contrato", new { id = _id_contrato });
            }
        }
        public ActionResult SubirArchivo()
        {
         
            return View();
        }

        [System.Web.Mvc.HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> SubirArchivo( HttpPostedFileBase UploadedFile)
        {
           if (UploadedFile != null)
            {
                    string fileName = UploadedFile.FileName;
                    string fileContentType = UploadedFile.ContentType;
                    byte[] fileBytes = new byte[UploadedFile.ContentLength];
                    var data = UploadedFile.InputStream.Read(fileBytes, 0, Convert.ToInt32(UploadedFile.ContentLength));


                ArchivoDto n = new ArchivoDto
                {
                     Id =0,
                    codigo = "archivo-prueba",
                    nombre = fileName,
                    vigente = true,
                    fecha_registro = DateTime.Now,
                    hash = fileBytes,
                    tipo_contenido = fileContentType,
                    };
                var result =await _archivoService.Create(n);
                string direccion = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)+"\\" + fileName;
                System.IO.File.WriteAllBytes(direccion, fileBytes);
                ViewBag.Error = "Ser descargo al Escritorio";
                return File(fileBytes, fileContentType);

            }
            return View();

        }
    }
}
