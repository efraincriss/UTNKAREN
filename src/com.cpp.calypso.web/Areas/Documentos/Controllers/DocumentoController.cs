using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.framework;
using com.cpp.calypso.proyecto.aplicacion.CertificacionIngenieria.Interface;
using com.cpp.calypso.proyecto.aplicacion.Documentos.Dto;
using com.cpp.calypso.proyecto.aplicacion.Documentos.Interface;
using com.cpp.calypso.proyecto.aplicacion.Interfaces;
using com.cpp.calypso.proyecto.dominio;
using com.cpp.calypso.proyecto.dominio.Documentos;
using com.cpp.calypso.web.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using JsonResult = System.Web.Mvc.JsonResult;

namespace com.cpp.calypso.web.Areas.Documentos.Controllers
{
    public class DocumentoController : BaseAccesoSpaController<Documento, DocumentoDto, PagedAndFilteredResultRequestDto>
    {
        private readonly IDocumentoAsyncBaseCrudAppService _documentoService;
        private readonly ICatalogoAsyncBaseCrudAppService _catalogoService;
        private readonly ISeccionAsyncBaseCrudAppService _seccionService;

        private readonly IPacienteAsyncBaseCrudAppService _paciente;
        public DocumentoController(
            IHandlerExcepciones manejadorExcepciones,
            IViewService viewService,
            IDocumentoAsyncBaseCrudAppService documentoService,
            ICatalogoAsyncBaseCrudAppService catalogoService,
            ISeccionAsyncBaseCrudAppService seccionService,
            IPacienteAsyncBaseCrudAppService paciente
         ) : base(manejadorExcepciones, viewService)
        {
            _documentoService = documentoService;
            _catalogoService = catalogoService;
            _seccionService = seccionService;
            _paciente = paciente;
        }

        public ActionResult Detalles(int contratoId)
        {
            var model = new FormReactModelView();
            model.Id = "gestion_documentos_contratos";
            model.ReactComponent = "~/Scripts/build/gestion_documentos_contratos.js";

            if (!string.IsNullOrEmpty(Title))
            {
                model.Title = Title;
            }


            ViewBag.ruta = new string[] { "Inicio", "Gestión de Contratos", "Listado Contratos", "Gestión de Documentos" };

            return View(model);
        }

        public ActionResult IndexRegistro()
        {
            return View();
        }

        public ActionResult FGetCatalogos()
        {
            var list = new MNAModels();
            list.pr1 = _catalogoService.APIObtenerCatalogos("MNA_1");
            list.pr2 = _catalogoService.APIObtenerCatalogos("MNA_2");
            list.pr3 = _catalogoService.APIObtenerCatalogos("MNA_3");
            list.pr4 = _catalogoService.APIObtenerCatalogos("MNA_4");
            list.pr5 = _catalogoService.APIObtenerCatalogos("MNA_5");
            list.pr6 = _catalogoService.APIObtenerCatalogos("MNA_6");
            list.pr7 = _catalogoService.APIObtenerCatalogos("MNA_7");
            list.pr8 = _catalogoService.APIObtenerCatalogos("MNA_8");
            list.pr9 = _catalogoService.APIObtenerCatalogos("MNA_9");
            list.pr10 = _catalogoService.APIObtenerCatalogos("MNA_10");
            list.pr11 = _catalogoService.APIObtenerCatalogos("MNA_11");
            list.pr12 = _catalogoService.APIObtenerCatalogos("MNA_12");
            list.pr13 = _catalogoService.APIObtenerCatalogos("MNA_13");
            list.pr14 = _catalogoService.APIObtenerCatalogos("MNA_14");
            list.pr15 = _catalogoService.APIObtenerCatalogos("MNA_15");
            list.pr16 = _catalogoService.APIObtenerCatalogos("MNA_16");
            list.pr17 = _catalogoService.APIObtenerCatalogos("MNA_17");
            list.pr18 = _catalogoService.APIObtenerCatalogos("MNA_18");
            list.centros = _catalogoService.APIObtenerCatalogos("CENTRO_GE");
            var result = JsonConvert.SerializeObject(list);
            return Content(result);
        }

        public ActionResult FGetCatalogosSexo()
        {

            var list = _catalogoService.APIObtenerCatalogos("PACIENTESEXO");
            var result = JsonConvert.SerializeObject(list);
            return Content(result);
        }

        [HttpPost]
        public ActionResult CreateP(Paciente input)
        {

            var result = _paciente.insertarEntidad(input);
            return Content(result ? "OK" : "FALSE");
        }
        [HttpPost]
        public ActionResult FGetEdit(Paciente input)
        {

            var result = _paciente.Editar(input);
            return Content(result ? "OK" : "FALSE");
        }
        [HttpPost]
        public ActionResult FGetDelete(int id)
        {

            var result = _paciente.eliminarPaciente(id);
            return Content(result ? "OK" : "FALSE");
        }




        public ActionResult FGetList()
        {

            var list = _paciente.pacientes();
            var result = JsonConvert.SerializeObject(list);
            return Content(result);
        }


        public ActionResult FGetListPacie(int id)
        {

            var list = _paciente.detallePaciente(id);
            var result = JsonConvert.SerializeObject(list);
            return Content(result);
        }

        public ActionResult GetTotal1(int PerdidaApetitoId, int PerdidaPesoId, int MovilidadId, int EnfermedadAgudaId, int ProblemasNeuroId, int IndiceMasaId, int ViveDomicilioId
            , int MedicamentoDiaId, int UlceraLesionId, int ComidaDiariaId, int ConsumoPersonaId, int ConsumoFrutasVerdurasId, int NumeroVasosAguaId, int ModoAlimentarseId, int ConsideracionEnfermoId, int EstadoSaludId,

       int CircunferenciaBraquialId, int CircunferenciaPiernaId)
        {

            var list = _paciente.ObtenerTotales(PerdidaApetitoId, PerdidaPesoId, MovilidadId, EnfermedadAgudaId, ProblemasNeuroId, IndiceMasaId, ViveDomicilioId
            , MedicamentoDiaId, UlceraLesionId, ComidaDiariaId, ConsumoPersonaId, ConsumoFrutasVerdurasId, NumeroVasosAguaId, ModoAlimentarseId, ConsideracionEnfermoId, EstadoSaludId,

        CircunferenciaBraquialId, CircunferenciaPiernaId);
            var result = JsonConvert.SerializeObject(list);
            return Content(result);
        }

        public ActionResult GetTotal2(int PerdidaApetitoId, int PerdidaPesoId, int MovilidadId, int EnfermedadAgudaId, int ProblemasNeuroId, int IndiceMasaId, int ViveDomicilioId
          , int MedicamentoDiaId, int UlceraLesionId, int ComidaDiariaId, int ConsumoPersonaId, int ConsumoFrutasVerdurasId, int NumeroVasosAguaId, int ModoAlimentarseId, int ConsideracionEnfermoId, int EstadoSaludId,

     int CircunferenciaBraquialId, int CircunferenciaPiernaId)
        {

            var list = _paciente.ObtenerTotales2(PerdidaApetitoId, PerdidaPesoId, MovilidadId, EnfermedadAgudaId, ProblemasNeuroId, IndiceMasaId, ViveDomicilioId
            , MedicamentoDiaId, UlceraLesionId, ComidaDiariaId, ConsumoPersonaId, ConsumoFrutasVerdurasId, NumeroVasosAguaId, ModoAlimentarseId, ConsideracionEnfermoId, EstadoSaludId,

        CircunferenciaBraquialId, CircunferenciaPiernaId);
            var result = JsonConvert.SerializeObject(list);
            return Content(result);
        }





        [HttpPost]
        public ActionResult CreatePMNA(MNA input)
        {

            var result = _paciente.insertarMNA(input);
            return Content(result ? "OK" : "FALSE");
        }
        [HttpPost]
        public ActionResult FGetEditMNA(MNA input)
        {

            var result = _paciente.EditarMNA(input);
            return Content(result ? "OK" : "FALSE");
        }
        [HttpPost]
        public ActionResult FGetDeleteMNA(int id)
        {

            var result = _paciente.eliminarMNA(id);
            return Content(result ? "OK" : "FALSE");
        }

        [HttpPost]
        public ActionResult CreatePK(Katz input)
        {

            var result = _paciente.insertarKat(input);
            return Content(result ? "OK" : "FALSE");
        }
        [HttpPost]
        public ActionResult FGetEditK(Katz input)
        {

            var result = _paciente.EditarKat(input);
            return Content(result ? "OK" : "FALSE");
        }
        [HttpPost]
        public ActionResult FGetDeleteK(int id)
        {

            var result = _paciente.eliminarKat(id);
            return Content(result ? "OK" : "FALSE");
        }

        public ActionResult GetReporteSeguimientoMovil()
        {
            var excel = _paciente.Reporte();
            string excelName = "GENERAL" + DateTime.Now.ToShortDateString();
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

        #region Api

        [HttpGet]
        public ActionResult ObtenerDocumentosDeCarpeta(int id)
        {
            var documentos = _documentoService.ObtenerDocumentosDeCarpeta(id);
            return WrapperResponseGetApi(ModelState, () => documentos);
        }

        [HttpDelete]
        public ActionResult EliminarDocumento(int id)
        {
            var result = _documentoService.EliminarDocumento(id);
            return new JsonResult
            {
                Data = new { success = result.Eliminado, result = result.Error }
            };
        }

        [HttpPost]
        public async Task<ActionResult> CrearDocumentoAsync(DocumentoDto input)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var dto = await _documentoService.CrearDocumentoAsync(input);
                    return new JsonResult
                    {
                        Data = new { success = true }
                    };
                }
            }
            catch (Exception ex)
            {
                var result = ManejadorExcepciones.HandleException(ex);
                ModelState.AddModelError("", result.Message);
            }
            return new JsonResult
            {
                Data = new { success = false, errors = ModelState.ToSerializedDictionary() }
            };
        }


        [HttpPost]
        public ActionResult EditarDocumentoAsync(DocumentoDto input)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var dto = _documentoService.ActualizarDocumento(input);
                    return new JsonResult
                    {
                        Data = new { success = true }
                    };
                }
            }
            catch (Exception ex)
            {
                var result = ManejadorExcepciones.HandleException(ex);
                ModelState.AddModelError("", result.Message);
            }
            return new JsonResult
            {
                Data = new { success = false, errors = ModelState.ToSerializedDictionary() }
            };
        }

        [HttpGet]
        public ActionResult ObtenerCatalogoTipoDocumento()
        {
            var catalogos = _catalogoService.APIObtenerCatalogos("TIPO_DOCUMENTO_CONTR");
            return WrapperResponseGetApi(ModelState, () => catalogos);
        }

        [HttpGet]
        public ActionResult ObtenerDetallesDocumento(int id)
        {
            var documento = _documentoService.ObtenerDocumento(id);
            return WrapperResponseGetApi(ModelState, () => documento);
        }

        [HttpGet]
        public ActionResult ObtenerDocumentosTipoAnexo(int id)
        {
            var documentos = _documentoService.ObtenerDocumentosTipoAnexoPorCarpeta(id);
            return WrapperResponseGetApi(ModelState, () => documentos);
        }

        public ActionResult GetByCodeApi(string code)
        {
            var entityDto = _catalogoService.ListarCatalogos(code);
            return WrapperResponseGetApi(ModelState, () => entityDto);

        }
        public ActionResult ObtenerDocumentosporCarpeta(int carpetaId)
        {

            var documentos = _documentoService.ObtenerDocumentosDeCarpeta(carpetaId);
            return WrapperResponseGetApi(ModelState, () => documentos);
        }
        public ActionResult ObtenerDocumentosporTipo(string tipoDocumento, int carpetaId)
        {

            var documentos = _documentoService.ObtenerDocumentosporTipo(tipoDocumento, carpetaId);
            var result = JsonConvert.SerializeObject(documentos,
                 Newtonsoft.Json.Formatting.None,
                 new JsonSerializerSettings
                 {
                     ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                     NullValueHandling = NullValueHandling.Ignore

                 });
            return Content(result);
        }
        public ActionResult ObtenerSeccionesporDocumentoId(int documentoId)
        {

            var secciones = _documentoService.ObtenerSeccionporDocumento(documentoId);
            var result = JsonConvert.SerializeObject(secciones,
                 Newtonsoft.Json.Formatting.None,
                 new JsonSerializerSettings
                 {
                     ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                     NullValueHandling = NullValueHandling.Ignore

                 });
            return Content(result);
        }
        public ActionResult ObtenerSeccionesporCarpeta(int carpetaId)
        {

            var secciones = _documentoService.ObtenerSeccionporCarpeta(carpetaId);
            var result = JsonConvert.SerializeObject(secciones,
                 Newtonsoft.Json.Formatting.None,
                 new JsonSerializerSettings
                 {
                     ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                     NullValueHandling = NullValueHandling.Ignore

                 });
            return Content(result);
        }

        public ActionResult ObtenerDocporCarpeta(int carpetaId)
        {

            var documentos = _documentoService.ObtenerDocumentosporCarpeta(carpetaId);
            var result = JsonConvert.SerializeObject(documentos,
                      Newtonsoft.Json.Formatting.None,
                      new JsonSerializerSettings
                      {
                          ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                          NullValueHandling = NullValueHandling.Ignore

                      });
            return Content(result);
        }

        public ActionResult ObtenerSeccionesporFiltros(int carpetaId = 0, string tipoDocumento = "", int documentoId = 0, int seccionId = 0, string palabra = "", bool soloTitulos = false)
        {

            var secciones = _seccionService.ObtenerSeccionesFiltros(carpetaId, tipoDocumento, documentoId, seccionId, palabra, soloTitulos);
            var result = JsonConvert.SerializeObject(secciones,
               Newtonsoft.Json.Formatting.None,
               new JsonSerializerSettings
               {
                   ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                   NullValueHandling = NullValueHandling.Ignore

               });
            return Content(result);
        }


        #endregion
    }
}