using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.framework;
using com.cpp.calypso.proyecto.aplicacion.CertificacionIngenieria.Dto;
using com.cpp.calypso.proyecto.aplicacion.CertificacionIngenieria.Interface;
using com.cpp.calypso.proyecto.aplicacion.CertificacionIngenieria.Models;
using com.cpp.calypso.proyecto.aplicacion.Interfaces;
using com.cpp.calypso.proyecto.dominio.CertificacionIngenieria;
using com.cpp.calypso.web.Areas.Accesos.Controllers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;
using JsonResult = System.Web.Mvc.JsonResult;


namespace com.cpp.calypso.web.Areas.CertificacionIngenieria.Controllers
{
    public class CertificadoController : BaseAccesoSpaController<DetallesDirectosIngenieria, DetallesDirectosIngenieriaDto, PagedAndFilteredResultRequestDto>
    {
        private readonly IDetallesDirectosIngenieriaAsyncBaseCrudAppService _detallesIngenieriaService;
        private readonly ICatalogoAsyncBaseCrudAppService _catalogoService;
        private readonly IGrupoCertificadoIngenieriaAsyncBaseCrudAppService _certificadoService;

        public CertificadoController(
            IHandlerExcepciones manejadorExcepciones,
            IViewService viewService,
            IDetallesDirectosIngenieriaAsyncBaseCrudAppService detallesIngenieriaService,
            ICatalogoAsyncBaseCrudAppService catalogoService,
             IGrupoCertificadoIngenieriaAsyncBaseCrudAppService certificadoService
        ) : base(manejadorExcepciones, viewService)
        {
            _detallesIngenieriaService = detallesIngenieriaService;
            _catalogoService = catalogoService;
            _certificadoService = certificadoService;
        }

        // GET: CertificacionIngenieria/Certificado
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult IndexProyectos()
        {
            return View();
        }

        public ActionResult SearchByCodeApi(string code)
        {
            var result = _catalogoService.APIObtenerCatalogos(code);
            return new JsonResult
            {
                Data = new { success = true, result },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };

        }

        [HttpGet]
        public ActionResult ObtenerGruposPorFechas(DateTime? fechaInicio, DateTime? fechaFin)
        {
            var list = _certificadoService.GetCertificados(fechaInicio, fechaFin);
            return WrapperResponseGetApi(ModelState, () => list);
        }

        [HttpGet]
        public ActionResult ListaProyectos()
        {
            var list = _certificadoService.ObtenerProyectos();
            return WrapperResponseGetApi(ModelState, () => list);
        }

        [HttpGet]
        public ActionResult ComprobarUltimoCertificadiEstaAprobado()
        {
            var result = _certificadoService.UltimoCertificadiGenerado();
            return WrapperResponseGetApi(ModelState, () => result);
        }


        [HttpGet]
        public ActionResult ObtenerDetallesSinCertificarPorFechas(DateTime? fechaInicio, DateTime? fechaFin,int ClienteId)
        {

            var list = _certificadoService.GetDetallesSinCertificar(fechaInicio, fechaFin, ClienteId);
            var result = JsonConvert.SerializeObject(list,
              Newtonsoft.Json.Formatting.None,
              new JsonSerializerSettings
              {
                  ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                  NullValueHandling = NullValueHandling.Ignore

              });
            return Content(result);
            // return WrapperResponseGetApi(ModelState, () => list);
        }

        [HttpGet]
        public ActionResult ObtenerCertificadosPorGrupo(int id)
        {
            var list = _certificadoService.CertificadosPorGrupo(id);
            return WrapperResponseGetApi(ModelState, () => list);
        }

        [HttpGet]
        public ActionResult ObtenerParametrizacionPorGrupo(int id)
        {
            var list = _certificadoService.ObtenerParametrizacion(id);
            return WrapperResponseGetApi(ModelState, () => list);
        }

        [HttpGet]
        public ActionResult ObtenerGastosPorCertificado(int id)
        {
            var list = _certificadoService.GastosDirectosCertificado(id);
            return WrapperResponseGetApi(ModelState, () => list);
        }

        [HttpPost]
        public ActionResult ValidarFechasCertificacion(DateTime fechaInicio, DateTime fechaFin, DateTime fechaCertificado, int clienteId)
        {
            var validacion = _certificadoService.validarFechasCertificado(fechaInicio, fechaFin, fechaCertificado, clienteId);
            return new JsonResult
            {
                Data = new { success = validacion.Success, message = validacion.Message }
            };
        }


        [HttpPost]
        public ActionResult Crear(GrupoCertificadoIngenieria input, int[] Directos, int[] Indirectos, int[] E500)
        {
     
            var result = _certificadoService.Crear(input, Directos, Indirectos, E500);

            return new JsonResult
            {
                Data = new { success = result.Success, message = result.Message }
            };
        }

        [HttpPost]
        public ActionResult ActualizarProyecto(ProyectosCertificacion input)
        {

            var result = _certificadoService.ActualizarCampoCertificacion(input);

            return new JsonResult
            {
                Data = new { success = result, message ="OK" }
            };
        }


        /*Transacciones Separadas*/

        [HttpPost]
        public ActionResult ActualizarCampoGrupo(GrupoCertificadoIngenieria input)
        {

          _certificadoService.ActualizarCampoLocacionConUbicacionParametrizacion(input.FechaInicio,input.FechaFin);
            return Content("");

        }


        [HttpPost]
        public ActionResult CrearGrupo(GrupoCertificadoIngenieria input, List<ProyectoDistribucionModel> distribucionProyectos)
        {
            _certificadoService.ActualizarCampoLocacionConUbicacionParametrizacion(input.FechaInicio, input.FechaFin);
            var GrupoCertificadoId = _certificadoService.CrearGrupoCertificados(input, distribucionProyectos);
            return Content(""+GrupoCertificadoId);
       
        }

        [HttpPost]
        public ActionResult GenerarCertificadosPorProyectosDirectos(int GrupoCertificadoId, int ProyectoId,int[] Directos)
        {

            var result = _certificadoService.CrearCertificadosDirectos(GrupoCertificadoId, ProyectoId, Directos);

            return new JsonResult
            {
                Data = new { success = result, message = "OK" }
            };
        }
        [HttpPost]
        public ActionResult GenerarDistribucionE500Principal(int GrupoCertificadoId, int[] Indirectos,int [] E500, List<ProyectoDistribucionModel> distribucionProyectos)
        {
            var result2 = _certificadoService.AñadirDistribucionE500(GrupoCertificadoId, E500,distribucionProyectos);

         

             


            return new JsonResult
            {
                Data = new { success = result2, message = "OK"}
            };
        }
        [HttpPost]
        public ActionResult GenerarDistribucionIndirectosPrincipal(int GrupoCertificadoId, int[] Indirectos, int[] E500, List<ProyectoDistribucionModel> distribucionProyectos)
        {
      

            var result = _certificadoService.AñadirDistribucionIndirectos(GrupoCertificadoId, Indirectos, distribucionProyectos);





            return new JsonResult
            {
                Data = new { success = result, message = "OK" }
            };
        }
        [HttpPost]
        public ActionResult AprobarGrupoCertificado(int Id)
        {
         
            return new JsonResult
            {
                Data = new { success = true, message = "OK" }
            };

        }
        [HttpPost]
        public ActionResult AgregarViaticos(int GrupoCertificadoId,List<ProyectoDistribucionModel> distribucionProyectos)
        {
            //  _certificadoService.GenerarViaticos(GrupoCertificadoId);
            _certificadoService.ViaticosVersion2(GrupoCertificadoId, distribucionProyectos);

            return new JsonResult
            {
                Data = new { success = true, message = "OK" }
            };

        }

       // [HttpPost]
        public ActionResult ActualizarCabecerasGrupo(int GrupoCertificadoId)
        {

            _certificadoService.ActualizarCabeceras(GrupoCertificadoId);
   
            return new JsonResult
            {
                Data = new { success = true, message = "OK"}
            };

        }

        [HttpPost]
        public ActionResult GenerarDistribucionE500(int GrupoCertificadoId,int[] E500)
        {

            var result = _certificadoService.AñadirDistribucionE500(GrupoCertificadoId,E500,new List<ProyectoDistribucionModel>());

            return new JsonResult
            {
                Data = new { success = result, message = "OK" }
            };
        }
        [HttpPost]
        public ActionResult Delete(int Id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var dto = _certificadoService.Eliminar(Id);
                    return new JsonResult
                    {
                        Data = new { success = dto, result = dto }
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
        public ActionResult GuardarIndirecto(int Id, string proyectos)
        {
            var result = _certificadoService.guardarProyectoNodistribucion(Id, proyectos);
            return Content(result);

        }

        [HttpPost]
        public ActionResult AprobarCertificado(int Id)
        {
            var result = _certificadoService.AprobarCertificado(Id);
            return Content(result ? "OK" : "ERROR");
        }

        public ActionResult APICliente()
        {
            var data = _certificadoService.GetListCliente();
            var result = JsonConvert.SerializeObject(data,
                Newtonsoft.Json.Formatting.None,
                new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    NullValueHandling = NullValueHandling.Ignore
                });
            return Content(result);
        }
        public ActionResult APIProyectosDistribuibles(int[] Directos)
        {
            var data = _certificadoService.ProyectoDistribuibles(Directos);
            var result = JsonConvert.SerializeObject(data,
                Newtonsoft.Json.Formatting.None,
                new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    NullValueHandling = NullValueHandling.Ignore
                });
            return Content(result);
        }

        public ActionResult DescargarGrupoCertificados(int Id)//GrupoCertificadoId
        {

            try
            {

           
            var excel = _certificadoService.GrupoCertificadosCompleto(Id);
               
                string excelName = _certificadoService.nombreExcelGrupoCertificado(Id);
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
            catch (ArgumentNullException e)
            {
                ElmahExtension.LogToElmah(new Exception(" " + e.Message));
                ElmahExtension.LogToElmah(new Exception("Log Error Certificado" + e.InnerException));
                ElmahExtension.LogToElmah(new Exception("Log Error Certificado Result" + e.HResult));

                ElmahExtension.LogToElmah(new Exception("Log Error Certificado" + e.Source));
                return Content("");

            }
        }




        public ActionResult DescargarGrupoCertificados2(int Id)//GrupoCertificadoId
        {

            try
            {


                var excel = _certificadoService.GrupoCertificadosCompleto2(Id);

                string excelName = _certificadoService.nombreExcelGrupoCertificado(Id);
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
            catch (ArgumentNullException e)
            {
                ElmahExtension.LogToElmah(new Exception(" " + e.Message));
                ElmahExtension.LogToElmah(new Exception("Log Error Certificado" + e.InnerException));
                ElmahExtension.LogToElmah(new Exception("Log Error Certificado Result" + e.HResult));

                ElmahExtension.LogToElmah(new Exception("Log Error Certificado" + e.Source));
                return Content("");

            }
        }


        [HttpPost]
        public ActionResult CrearMasivo(GrupoCertificadoIngenieria input)
        {
           
            var result = _certificadoService.GenerarCertificadosMasivos(input.FechaInicio,input.FechaFin,input.ClienteId);

            return new JsonResult
            {
                Data = new { success = true, message = "OK" }
            };


        }


        public ActionResult Resumen(GrupoCertificadoIngenieria input, int[] Directos, int[] Indirectos, int[] E500)
        {

            try
            {
                var validacion = _certificadoService.ValidarRegistros(input, Directos, Indirectos, E500);

                return Content("OK");
                if (validacion.puedeCertificar)
                {
                    return Content("OK");
                }
                else
                {
                    var excel = _certificadoService.Resumen(validacion);
                    string excelName = "VALIDACIONES" + DateTime.Now.Year;
                    using (var memoryStream = new MemoryStream())
                    {
                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.AddHeader("content-disposition", "attachment; filename=" + excelName + ".xlsx");
                        excel.SaveAs(memoryStream);
                        memoryStream.WriteTo(Response.OutputStream);
                        Response.Flush();
                        Response.End();
                        return Content("ERROR");
                    }
                }

            }
            catch (ArgumentNullException e)
            {
                ElmahExtension.LogToElmah(new Exception(" " + e.Message));
                ElmahExtension.LogToElmah(new Exception("Log Error Certificado" + e.InnerException));
                ElmahExtension.LogToElmah(new Exception("Log Error Certificado Result" + e.HResult));

                ElmahExtension.LogToElmah(new Exception("Log Error Certificado" + e.Source));
                return Content("");

            }
        }

        [HttpGet]
        public ActionResult ObtenerMontos(int Id)
        {

            var list = _certificadoService.TotalesRedistribucion(Id);
            var result = JsonConvert.SerializeObject(list,
              Newtonsoft.Json.Formatting.None,
              new JsonSerializerSettings
              {
                  ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                  NullValueHandling = NullValueHandling.Ignore

              });
            return Content(result);
            // return WrapperResponseGetApi(ModelState, () => list);
        }


    }
}