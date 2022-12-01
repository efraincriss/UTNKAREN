using System;
using System.IO;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Abp.Application.Services.Dto;
using com.cpp.calypso.framework;
using com.cpp.calypso.proyecto.aplicacion;
using com.cpp.calypso.proyecto.aplicacion.Interfaces;
using com.cpp.calypso.proyecto.dominio;
using Newtonsoft.Json;
using OfficeOpenXml;

namespace com.cpp.calypso.web.Areas.Proyecto.Controllers
{


    public class AvanceObraController : BaseController
    {
        private readonly IAvanceObraAsyncBaseCrudAppService _avanceObraService;
        private readonly IObraAdicionalAsyncBaseCrudAppService _obraAdicionalService;
        private readonly IContratoAsyncBaseCrudAppService _contratoService;
        private readonly IProyectoAsyncBaseCrudAppService _proyectpService;
        private readonly IOfertaAsyncBaseCrudAppService _ofertaService;
        private readonly IArchivoAsyncBaseCrudAppService _archivoService;

        public AvanceObraController(
            IHandlerExcepciones manejadorExcepciones,
            IAvanceObraAsyncBaseCrudAppService avanceObraService,
            IObraAdicionalAsyncBaseCrudAppService obraAdicionalService,
            IContratoAsyncBaseCrudAppService contratoService,
            IProyectoAsyncBaseCrudAppService proyectpService,
            IOfertaAsyncBaseCrudAppService ofertaService,
            IArchivoAsyncBaseCrudAppService archivoService
        ) : base(manejadorExcepciones)
        {
            _avanceObraService = avanceObraService;
            _obraAdicionalService = obraAdicionalService;
            _contratoService = contratoService;
            _proyectpService = proyectpService;
            _ofertaService = ofertaService;
            _archivoService = archivoService;

        }

        public async Task<ActionResult> Create(int? id) // OfertaId
        {
            if (id.HasValue)
            {
                var oferta = await _ofertaService.Get(new EntityDto<int>(id.Value));
                AvanceObraDto avance = new AvanceObraDto()
                {

                    vigente = true,
                    OfertaId = id.Value,
                    fecha_presentacion = DateTime.Now,
                    fecha_hasta = DateTime.Now,
                    fecha_desde = DateTime.Now,
                    //
                    descripcion = oferta.Proyecto.nombre_proyecto,
                    alcance = oferta.Proyecto.nombre_proyecto
                };
                ViewBag.ruta = new string[] { "Inicio", "Construcción", "Avance Obra", oferta.codigo, "Crear" };
                return View(avance);
            }
            return RedirectToAction("Index", "Inicio", new { area = "" });
        }


        [HttpPost]
        public async Task<ActionResult> Create(AvanceObraDto avance)
        {
            avance.vigente = true;
            if (ModelState.IsValid)
            {
                var avanceObra = await _avanceObraService.Create(avance);
                return RedirectToAction("Index", new { id = avanceObra.OfertaId });
            }

            return View("Create", avance);
        }

        public ActionResult IndexOfertas(int? id) // Proyecto Id
        {
            if (id.HasValue)
            {
                var ofertas = _avanceObraService.ListarOfertasDeProyecto(id.Value);
                return View(ofertas);
            }
            return RedirectToAction("Index", "Inicio", new { area = "" });
        }

        public async Task<ActionResult> Index(int? id, string error = "") // Oferta Id
        {
           

            if (id.HasValue)
            {
                ViewBag.Id = id.Value;
                ViewBag.IdOferta = id.Value;
                var oferta = await _ofertaService.Get(new EntityDto<int>(id.Value));
                ViewBag.proyecto = oferta.Proyecto.codigo + " - " + oferta.Proyecto.nombre_proyecto;
                ViewBag.oferta = oferta.codigo + " - " + oferta.version;
                var ofertas = _avanceObraService.ListarAvancesDeOferta(id.Value);
                ViewBag.ruta = new string[] { "Inicio", "Construcción", "Avance Obra", oferta.codigo };
                return View(ofertas);
            }
            return RedirectToAction("Index", "Inicio", new { area = "" });
        }


        public async Task<ActionResult> Edit(int? id) // AvanceObra id
        {
            if (!id.HasValue)
            {
                return RedirectToAction("Index", "Inicio", new { area = "" });
            }

            var oferta = await _avanceObraService.Get(new EntityDto<int>(id.Value));
            ViewBag.ruta = new string[] { "Inicio", "Construcción", "Avance Obra", oferta.Oferta.codigo, "Editar" };
            return View(oferta);
        }


        [HttpPost]
        public async Task<ActionResult> Edit(AvanceObraDto avance)
        {
            if (ModelState.IsValid)
            {
                var avanceObra = await _avanceObraService.Update(avance);
                return RedirectToAction("Details", new { id = avanceObra.Id });
            }

            return View("Edit", avance);
        }


        public async Task<ActionResult> Details(int? id, string flag = "",string error="") // AvanceObra Id
        {
            if (!id.HasValue)
            {
                return RedirectToAction("Index", "Inicio", new { area = "" });
            }

            if (flag != "")
            {
                ViewBag.msg = flag;
            }

            if (error != "")
            {
                ViewBag.MsgError = error;
            }

            //var detalles = _avanceObraService.ListarDetallesAvanceObra(id.Value);
            var avance = await _avanceObraService.Get(new EntityDto<int>(id.Value));
            var oferta = await _ofertaService.Get(new EntityDto<int>(avance.OfertaId));

            var proyecto = await _proyectpService.Get(new EntityDto<int>(avance.Oferta.ProyectoId));
            var montos = _avanceObraService.MontoPresupuestadoIncrementado(oferta.Id);
            ViewBag.montos = montos;
            ViewBag.ContratoId = proyecto.contratoId;
            ViewBag.Fecha = avance.Oferta.fecha_oferta;
            //var obrasAdicionales = _obraAdicionalService.listar(id.Value);

            ViewBag.AvanceObraId = id.Value;

            //Archivos Avances Obra

            var archivos = _avanceObraService.ListaArchivos(id.Value);
            AvanceObraDetallesViewModel viewModel = new AvanceObraDetallesViewModel()
            {
                AvanceObra = avance,
                proyecto = proyecto,
                Archivos = archivos,
                //DetallesAvanceObra = detalles,
                //ObrasAdicionales = obrasAdicionales,

            };
            ViewBag.ruta = new string[] { "Inicio", "Construcción", "Avance Obra", oferta.codigo, "Detalles Avance Obra" };
            return View(viewModel);
        }


        [HttpPost]
        public ActionResult AprobarAvance(int? id) // AvanceObraId
        {
            if (id.HasValue)
            {
                _avanceObraService.AprobarAvanceObra(id.Value);
                return RedirectToAction("Details", "AvanceObra", new { id = id, flag = "Avance de Obra Aprobado" });
            }
            return RedirectToAction("Index", "Inicio", new { area = "" });
        }

        [HttpPost]
        public ActionResult DesaprobarAvance(int? id) // AvanceObraId
        {
            if (id.HasValue)
            {
              bool resultado=  _avanceObraService.DesaprobarAvanceObra(id.Value);

                if (!resultado)
                {
                    return RedirectToAction("Details", "AvanceObra", new { id = id, error = "No se puede Desaprobar existe un RDO generado con fecha superior" });
                }


                return RedirectToAction("Details", "AvanceObra", new { id = id, flag = "Avance de Obra Desaprobado" });


            }
            return RedirectToAction("Index", "Inicio", new { area = "" });
        }

        public ActionResult Delete(int? id) // AvanceObra Id
        {
            if (!id.HasValue)
            {
                return RedirectToAction("Index", "Inicio", new { area = "" });
            }

            int ofertaId = _avanceObraService.EliminarVigencia(id.Value);
            if (ofertaId == -1) {

            }
           
            return RedirectToAction("Index", new { id = ofertaId });
        }


        // Api de la tabla temportal para ingreso de detalles con grid editable
        [HttpPost]
        public ActionResult GetComputosDetalles([System.Web.Http.FromBody] int ofertaId, [System.Web.Http.FromBody] DateTime fecha, [System.Web.Http.FromBody] int AvanceObraId) //OfertaId
        {
            var data = _avanceObraService.ObtenerComputosAvanceObra(ofertaId, fecha, AvanceObraId);
            var result = JsonConvert.SerializeObject(data);
            return Content(result);
        }


        [HttpPost]
        public ActionResult GetDetallesAvanceObra(int? id) // AvanceObraId
        {
            if (id.HasValue)
            {
                var detalles = _avanceObraService.ListarDetallesAvanceObra(id.Value);
                var result = JsonConvert.SerializeObject(detalles,
                    Newtonsoft.Json.Formatting.None,
                    new JsonSerializerSettings
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                        NullValueHandling = NullValueHandling.Ignore
                    });
                return Content(result);

            }
            return Content("BadRequest");
        }

        [HttpPost]
        public ActionResult CreateArchivo(ArchivosAvanceObraDto archivo)
        {
            var dataid = _avanceObraService.GuardarArchivo(archivo);
            if (dataid > 0) {
                return Content("OK");

            }
            else
            {
                return Content("ERROR");
            }
        }

        [HttpPost]
        public async Task<ActionResult> EditarArchivo(int id, string descripcion)
        {
            var a = _avanceObraService.EditFile(id, descripcion);
            if (a > 0)
            {
                return Content("OK");

            }
            else
            {
                return Content("ERROR");
            }



        }
        [HttpPost]
        public ActionResult DeleteArchivo(int? id) // ArchivoAvanceObraId
        {
            if (!id.HasValue)
            {
                return Content("ERROR");
            }

            int avanceid = _avanceObraService.EliminarVigenciaArchivo(id.Value);
            return Content("OK");
        }

        public async System.Threading.Tasks.Task<ActionResult> descargararchivo(int id)
        {
            var a = await _archivoService.Get(new EntityDto<int>(id));
            return File(a.hash, a.tipo_contenido, a.nombre);
        }

        public async Task<ActionResult> GetExcelCarga(int? id) // AvanceObraId
        {
            var avance = await _avanceObraService.Get(new Abp.Application.Services.Dto.EntityDto<int>(id.Value));
            ExcelPackage excel = _avanceObraService.CargaMasivaAvanceObra(id.Value);


            string excelName = "Formato Avance de Obra-" + avance.fecha_presentacion.GetValueOrDefault().ToShortDateString();
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


        public ActionResult UploadFileList( int id) //lISTA DE COLABORADORES
        {
            var list = _avanceObraService.ListaArchivos(id);
                 var result = JsonConvert.SerializeObject(list,
               Newtonsoft.Json.Formatting.None,

               new JsonSerializerSettings
               {
                   ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                   NullValueHandling = NullValueHandling.Ignore,


               });
            return Content(result);
        }

        public async Task<ActionResult> Descargar(int id)
        {
            var entity = await _archivoService.Get(new EntityDto<int>(id));

            if (entity == null)
            {
                var msg = string.Format("El Archivo con identificacion {0} no existe",
                    id);

                return HttpNotFound(msg);
            }

            return File(entity.hash, entity.tipo_contenido, entity.nombre);
        }
    }
}
