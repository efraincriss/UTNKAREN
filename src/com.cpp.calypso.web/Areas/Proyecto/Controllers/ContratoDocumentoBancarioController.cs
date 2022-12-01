using AutoMapper;
using com.cpp.calypso.framework;
using com.cpp.calypso.proyecto.aplicacion;
using com.cpp.calypso.proyecto.aplicacion.Interfaces;
using com.cpp.calypso.proyecto.dominio;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Abp.Application.Services.Dto;

namespace com.cpp.calypso.web.Areas.Proyecto.Controllers
{
    
    public class ContratoDocumentoBancarioController : BaseController
    {
        private readonly IContratoDocumentoBancarioAsyncBaseCrudAppService contratodocumentobancarioService;
        private readonly IInstitucionFinancieraAsyncBaseCrudAppService ifinancieraService;
        private readonly IArchivoAsyncBaseCrudAppService _archivoService;

        public ContratoDocumentoBancarioController(IHandlerExcepciones manejadorExcepciones, IContratoDocumentoBancarioAsyncBaseCrudAppService contratodocumentobancarioService, ICentrocostoContratoAsyncBaseCrudAppService centrocostocontratoService,
            IInstitucionFinancieraAsyncBaseCrudAppService ifinancieraService,
            IArchivoAsyncBaseCrudAppService archivoService) :
           base(manejadorExcepciones)
        {
            this.ifinancieraService = ifinancieraService;
            this.contratodocumentobancarioService = contratodocumentobancarioService;
            _archivoService = archivoService;
        }




        // GET: Proyecto/ContratoDocumentoBancario
        public async System.Threading.Tasks.Task<ActionResult> Index()
        {
            var input = new comun.aplicacion.PagedAndFilteredResultRequestDto();
            input.MaxResultCount = 50;
            var result = await contratodocumentobancarioService.GetAll(input);
            return View(result);
        }

        // GET: Proyecto/ContratoDocumentoBancario/Details/5
        public ActionResult Details(int? id)
        {
            if (id.HasValue)
            {
                var contratodocumento = contratodocumentobancarioService.GetDetalle(id.Value);
                if (contratodocumento != null)
                {
                    return View(contratodocumento);
                }
            }
            return RedirectToAction("Index", "Contrato");
        }

        // GET: Proyecto/ContratoDocumentoBancario/Create
        public ActionResult Create(int? id, String message)
        {

            if (message != null) {
                ViewBag.Msg = message;
            }
            if (id.HasValue)
            {
                ContratoDocumentoBancarioDto contratodoc = new ContratoDocumentoBancarioDto();
                contratodoc.ContratoId = id.Value;
                contratodoc.fecha_emision=DateTime.Now;
                contratodoc.InstitucionesFinancieras = ifinancieraService.GetInstitucionesFinancieras();
                return View(contratodoc);
            }
            else
            {
                return RedirectToAction("Index", "Contrato");
            }
        }

        // POST: Proyecto/ContratoDocumentoBancario/Create
        [HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> Create(ContratoDocumentoBancarioDto contratodoc, 
            HttpPostedFileBase UploadedFile)
        {
            try
            {

                if (ModelState.IsValid)
                {
                 
                    bool fecha = contratodocumentobancarioService.comprobarfecha(contratodoc.fecha_emision, contratodoc.fecha_vencimiento.Value);
                    if (fecha)
                    {
                        var arhivoid =
                            contratodocumentobancarioService.GuardarArchivo(contratodoc.ContratoId, UploadedFile);

                        contratodoc.vigente = true;
                        contratodoc.ArchivosContratoId = arhivoid;
                        var contrato = await contratodocumentobancarioService.Create(contratodoc);
                        return RedirectToAction("Details", "Contrato", new { id = contratodoc.ContratoId });

                        
                    }
                   ViewBag.Error = "La fecha de Vencimiento no puede ser menor a la fecha de emisión";
                    var dto = Mapper.Map<ContratoDocumentoBancarioDto>(contratodoc);
                    dto.InstitucionesFinancieras = ifinancieraService.GetInstitucionesFinancieras();
                    return View("Create",dto);

                }
                else
                {

                    var dto = Mapper.Map<ContratoDocumentoBancarioDto>(contratodoc);
                   dto.InstitucionesFinancieras = ifinancieraService.GetInstitucionesFinancieras();
                    return View("Create", dto);

                }


            }
            catch
            {
                return View();
            }
        }

        // GET: Proyecto/ContratoDocumentoBancario/Edit/5
        public ActionResult Edit(int? id)
        {
            if (!id.HasValue)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
               
              var contratoDocumentoBancarioDto = contratodocumentobancarioService.GetDetalle(id.Value);
                contratoDocumentoBancarioDto.InstitucionesFinancieras = ifinancieraService.GetInstitucionesFinancieras();

                if (contratoDocumentoBancarioDto == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.NotFound);
                }
                else
                {
                    return View(contratoDocumentoBancarioDto);
                }
            }
        }

        // POST: Proyecto/ContratoDocumentoBancario/Edit/5
        [HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> Edit(int id, ContratoDocumentoBancarioDto contratoDocumentoBancarioDto)
        {
            try
            {
                //empresaService.InsertOrUpdateAsync(empresaDto);
                bool fecha = contratodocumentobancarioService.comprobarfecha(contratoDocumentoBancarioDto.fecha_emision, contratoDocumentoBancarioDto.fecha_vencimiento.Value);
                if (fecha)
                {
                    var contrato = await contratodocumentobancarioService.InsertOrUpdateAsync(contratoDocumentoBancarioDto);
                    return RedirectToAction("Details", "Contrato", new { id = contratoDocumentoBancarioDto.ContratoId });
              


                }
                ViewBag.Error = "La fecha de Vencimiento no puede ser menor a la fecha de emisión";
                var dto = Mapper.Map<ContratoDocumentoBancarioDto>(contratoDocumentoBancarioDto);
                dto.InstitucionesFinancieras = ifinancieraService.GetInstitucionesFinancieras();
                return View("Edit", contratoDocumentoBancarioDto);
            }
            catch
            {
                return View();
            }
        }

    

        // POST: Proyecto/ContratoDocumentoBancario/Delete/5
        [HttpPost]
        public ActionResult Delete(int? id)
        {
            try
            {
                if (id.HasValue)
                {
                    ContratoDocumentoBancarioDto req = contratodocumentobancarioService.EliminarVigencia(id.Value);
                    return RedirectToAction("Details", "Contrato", new { id = req.Contrato.Id });
                }
                else
                {
                    return RedirectToAction("Index", "Contrato", new { message = "" });
                }

            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        System.Console.WriteLine("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }
            }


            return View();
        }
        public async System.Threading.Tasks.Task<ActionResult> descargararchivo(int id)
        {
            var a = await _archivoService.Get(new EntityDto<int>(id));
            return File(a.hash, a.tipo_contenido, a.nombre);
        }

    }
}
