using Abp.Application.Services.Dto;
using com.cpp.calypso.comun.dominio;
using AutoMapper;
using com.cpp.calypso.framework;
using com.cpp.calypso.proyecto.dominio;
using com.cpp.calypso.seguridad.aplicacion;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using com.cpp.calypso.proyecto.aplicacion.Interfaces;
using com.cpp.calypso.proyecto.aplicacion;
using com.cpp.calypso.proyecto.aplicacion.Dto;
using System.Xml;
using com.cpp.calypso.proyecto.aplicacion.WebService;
using QRCoder;
using System.Drawing;
using OfficeOpenXml;
using System.Text;
using System.Globalization;
using com.cpp.calypso.comun.aplicacion;
using Xceed.Words.NET;
using System.Xml.Serialization;
using com.cpp.calypso.proyecto.dominio.Constantes;
using System.Web.Http;
using com.cpp.calypso.proyecto.aplicacion.Acceso.Interface;
using Newtonsoft.Json.Linq;
using JsonResult = com.cpp.calypso.framework.JsonResult;

namespace com.cpp.calypso.web.Areas.Accesos.Controllers
{
    public class HuellaController : BaseController
    {

        private readonly IUsuarioService _usuarioService;
        private readonly IColaboradoresAsyncBaseCrudAppService _colaboradoresService;
        private readonly IColaboradorFormacionEducativaAsyncBaseCrudAppService _formacionEducativaService;
        private readonly IColaboradorDiscapacidadAsyncBaseCrudAppService _discapacidadService;
        private readonly IColaboradorBajaAsyncBaseCrudAppService _bajaColaboradorService;
        private readonly ICatalogoAsyncBaseCrudAppService _catalogoservice;
        private readonly ITipoCatalogoAsyncBaseCrudAppService _tipocatalogoservice;
        private readonly IContactoAsyncBaseCrudAppService _contactoService;
        private readonly IPaisAsyncBaseCrudAppService _paisservice;
        private readonly IProvinciaAsyncBaseCrudAppService _provinciaService;
        private readonly ICiudadAsyncBaseCrudAppService _ciudadService;
        private readonly IParroquiaAsyncBaseCrudAppService _parroquiaService;
        private readonly IComunidadAsyncBaseCrudAppService _comunidadService;
        private readonly IColaboradorCargaSocialAsyncBaseCrudAppService _cargaSocialService;
        private readonly IColaboradoresComidaAsyncBaseCrudAppService _comidaService;
        private readonly IColaboradorServicioAsyncBaseCrudAppService _colaboradorServicioService;
        private readonly IRequisitoColaboradorAsyncBaseCrudAppService _requisitosServicioService;
        private readonly IColaboradoresHuellaDigitalAsyncBaseCrudAppService _colaboradoresHuellaDigitalService;
        private readonly IColaboradoresFotografiaAsyncBaseCrudAppService _colaboradoresFotografiaService;
        private readonly IColaboradorMovilizacionAsyncBaseCrudAppService _colaboradorMovilizacionService;
        private readonly IOfertaAsyncBaseCrudAppService _ofertaService;
        private readonly IParametroService _parametroService;
        private readonly IColaboradorRequisitoAsyncBaseCrudAppService _colaboradoresRequisitoService;
        private readonly IColaboradoresHistoricoAsyncBaseCrudAppService _historicoService;
        private readonly IColaboradoresVisitaAsyncBaseCrudAppService _visitasService;
        private readonly IColaboradoresAusentismoAsyncBaseCrudAppService _ausentismoService;
        private readonly IContactoEmergenciaAsyncBaseCrudAppService _contactoEmergenciaService;
        private readonly IConsultaPublicaAsyncBaseCrudAppService _consultaPublicaService;

        //Empresa Colaborador
        private readonly IEmpresaAsyncBaseCrudAppService _empresaService;

        //Web service
        private readonly IWebServiceAsyncBaseCrudAppService _webService;
        public HuellaController(
        IHandlerExcepciones manejadorExcepciones,
            IParametroService parametroService,
            IColaboradoresAsyncBaseCrudAppService colaboradoresService,
            IColaboradorFormacionEducativaAsyncBaseCrudAppService formacionEducativaService,
            IColaboradorDiscapacidadAsyncBaseCrudAppService discapacidadService,
            IColaboradorBajaAsyncBaseCrudAppService bajaColaboradorService,
            ICatalogoAsyncBaseCrudAppService catalogoservice,
            ITipoCatalogoAsyncBaseCrudAppService tipocatalogoservice,
            IPaisAsyncBaseCrudAppService paisservice,
            IContactoAsyncBaseCrudAppService contactoService,
            IProvinciaAsyncBaseCrudAppService provinciaService,
            ICiudadAsyncBaseCrudAppService ciudadService,
            IParroquiaAsyncBaseCrudAppService parroquiaService,
            IComunidadAsyncBaseCrudAppService comunidadService,
            IColaboradorCargaSocialAsyncBaseCrudAppService cargaSocialService,
            IColaboradoresComidaAsyncBaseCrudAppService comidaService,
            IColaboradorServicioAsyncBaseCrudAppService colaboradorServicioService,
            IRequisitoColaboradorAsyncBaseCrudAppService requisitosServicioService,
            IColaboradoresHuellaDigitalAsyncBaseCrudAppService colaboradoresHuellaDigitalService,
            IColaboradoresFotografiaAsyncBaseCrudAppService colaboradoresFotografiaService,
            IColaboradorMovilizacionAsyncBaseCrudAppService colaboradorMovilizacionService,
            IUsuarioService usuarioService,
            IWebServiceAsyncBaseCrudAppService webService,
            IOfertaAsyncBaseCrudAppService ofertaService,
            IColaboradorRequisitoAsyncBaseCrudAppService colaboradoresRequisitoService,
            IColaboradoresHistoricoAsyncBaseCrudAppService historicoService,
            IColaboradoresVisitaAsyncBaseCrudAppService visitasService,
            IColaboradoresAusentismoAsyncBaseCrudAppService ausentismoService,
            IContactoEmergenciaAsyncBaseCrudAppService contactoEmergenciaService,
            IConsultaPublicaAsyncBaseCrudAppService consultaPublicaService,
            //ParametroService parametroService

            IEmpresaAsyncBaseCrudAppService empresaService
            ) : base(manejadorExcepciones)
        {
            _colaboradoresService = colaboradoresService;
            _formacionEducativaService = formacionEducativaService;
            _discapacidadService = discapacidadService;
            _bajaColaboradorService = bajaColaboradorService;
            _catalogoservice = catalogoservice;
            _tipocatalogoservice = tipocatalogoservice;
            _paisservice = paisservice;
            _contactoService = contactoService;
            _provinciaService = provinciaService;
            _ciudadService = ciudadService;
            _parroquiaService = parroquiaService;
            _comunidadService = comunidadService;
            _cargaSocialService = cargaSocialService;
            _comidaService = comidaService;
            _colaboradorServicioService = colaboradorServicioService;
            _requisitosServicioService = requisitosServicioService;
            _colaboradoresHuellaDigitalService = colaboradoresHuellaDigitalService;
            _colaboradoresFotografiaService = colaboradoresFotografiaService;
            _webService = webService;
            _colaboradorMovilizacionService = colaboradorMovilizacionService;
            _usuarioService = usuarioService;
            _ofertaService = ofertaService;
            _parametroService = parametroService;
            _colaboradoresRequisitoService = colaboradoresRequisitoService;
            _historicoService = historicoService;
            _visitasService = visitasService;
            _ausentismoService = ausentismoService;
            _contactoEmergenciaService = contactoEmergenciaService;
            _consultaPublicaService = consultaPublicaService;

            _empresaService = empresaService;
        }

        // GET: Accesos/Huella
        public ActionResult Vista()
        {
            return View();
        }

        public ActionResult Captura()
        {
            return View();
        }



        #region ES: MAPEO CORRECTO
        public ActionResult FirstList() //lISTA DE COLABORADORES
        {
            var list = _colaboradoresService.GetList();
            var result = JsonConvert.SerializeObject(list);
            return Content(result);
        }
        public ActionResult FilterSearch(string numeroIdentificacion, string nombres, string estado) // FILTROS COLABORADORES
        {
            var lista = _colaboradoresService.SearchAllColaboradores(numeroIdentificacion, nombres, estado);
            var result = JsonConvert.SerializeObject(lista);
            return Content(result);
        }
        public ActionResult GetByCodeApi(string code)
        {
            var result = _catalogoservice.APIObtenerCatalogos(code);
            return new JsonResult
            {
                Data = new { success = true, result },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };

        }

        [System.Web.Mvc.HttpPost]
        public ActionResult GetByCodeApiList(string[] codigo)//Obtener Lista de Catalogos por Codigo
        {
            var tiposCatalogos = _tipocatalogoservice.GetCatalogosPorCodigo(codigo);
            var catalogos = _catalogoservice.GetCatalogosPorCodigo(tiposCatalogos);
            var result = JsonConvert.SerializeObject(catalogos);
            return Content(result);
        }


        [System.Web.Mvc.HttpPost]
        public ActionResult ObtainFullInfo()//ObtainFullInfo GetColaboradoresInfoCompletaApi
        {
            var list = _colaboradoresService.GetColaboradoresInfoCompleta();

            var result = JsonConvert.SerializeObject(list);
            return Content(result);
        }

        [System.Web.Mvc.HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> DeleteHuellaApi(int Id)
        {
            var huella = await _colaboradoresHuellaDigitalService.Get(new EntityDto<int>(Id));
            huella.vigente = false;
            huella.principal = false;
            huella.IsDeleted = true;

            await _colaboradoresHuellaDigitalService.Update(huella);

            return Content("OK");
        }

        #endregion



        [System.Web.Mvc.HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> GetHuellasPorColaboradorApi(int Id)
        {
            var list = _colaboradoresHuellaDigitalService.GetHuellasPorColaborador(Id);
            var result = JsonConvert.SerializeObject(list);
            return Content(result);
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult GetColaboradorApi(int Id)
        {
            var list = _colaboradoresService.GetColaborador(Id);

            if (list != null)
            {
                var f = _formacionEducativaService.GetFormacion(list.Id);
                if (f != null)
                {
                    list.formacion = f.formacion == "" ? 0 : int.Parse(f.formacion);
                    list.institucion_educativa = f.institucion_educativa;
                    list.catalogo_titulo_id = f.catalogo_titulo_id == null ? 0 : f.catalogo_titulo_id.Value;
                    list.fecha_registro_senecyt = f.fecha_registro_senecyt == null ? null : f.fecha_registro_senecyt;
                }
                var d = _discapacidadService.GetDiscapacidadColaborador(list.Id);
                if (d != null)
                {
                    list.discapacidad = true;
                    list.catalogo_porcentaje_id = d.catalogo_porcentaje_id;
                    list.catalogo_tipo_discapacidad_id = d.catalogo_tipo_discapacidad_id;
                }

                if (list.ContactoId > 0)
                {
                    var contacto = _contactoService.GetContacto(list.ContactoId.Value);
                    list.telefono = contacto.celular;
                }
                var result = JsonConvert.SerializeObject(list);
                return Content(result);
            }
            return Content("NO");
        }

        [System.Web.Mvc.HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> GetHuellaApi(int Id)
        {
            var huella = _colaboradoresHuellaDigitalService.GetHuellaDigital(Id);
            return Content(JsonConvert.SerializeObject(huella));
        }

        [System.Web.Mvc.HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> CrearHuellaApiAsync(
      [FromBody] int Id, int dedo, int colaborador, string huella, bool principal, string plantilla_base64)
        {

            if (ModelState.IsValid)
            {

                var existeprincipal = _colaboradoresService.VerificarHuellaPrincipalColaborador(colaborador);



                ColaboradoresHuellaDigitalDto ch;

                /* si Principal es true buscamos que no exista un principal ya seleccionado */
                if (principal == true)
                {
                    List<ColaboradoresHuellaDigitalDto> lsHuella = _colaboradoresHuellaDigitalService.GetHuellasPorColaborador(colaborador);

                    foreach (var item in lsHuella)
                    {
                        if (item.principal == true
                            && dedo != item.catalogo_dedo_id)
                        {
                            return Content("Ya existe el registro de un Dedo Principal!");
                        }
                    }
                }
                else
                {
                    if (!existeprincipal)
                    {
                        return Content("_PRIMEROPRINCIPAL");
                    }

                }

                /* buscamos si existe el registro para actualizar en ez de crearlo nuevamente */
                ColaboradoresHuellaDigital huellaColaborador = _colaboradoresHuellaDigitalService.GetHuellaPorDedoColaborador(colaborador, dedo);

                if (huellaColaborador != null)
                {
                    if (huellaColaborador.vigente == true)
                    {
                        return Content("Ya se encuentra registrada una huella para el dedo seleccionado!");
                    }
                    else
                    {
                        ch = new ColaboradoresHuellaDigitalDto();
                        ch.Id = huellaColaborador.Id;
                        ch.colaborador_id = colaborador;
                        ch.catalogo_dedo_id = dedo;
                        ch.huella = huella;
                        ch.fecha_registro = DateTime.Now;
                        ch.vigente = true;
                        ch.plantilla_base64 = plantilla_base64;
                        ch.principal = principal;
                        ch.CreationTime = huellaColaborador.CreationTime;
                        ch.CreatorUserId = huellaColaborador.CreatorUserId;

                        /* Actualizamos */
                        await _colaboradoresHuellaDigitalService.Update(ch);
                    }
                }
                else
                {
                    ch = new ColaboradoresHuellaDigitalDto();
                    ch.Id = Id;
                    ch.colaborador_id = colaborador;
                    ch.catalogo_dedo_id = dedo;
                    ch.huella = huella;
                    ch.plantilla_base64 = plantilla_base64;
                    ch.principal = principal;

                    /* Se ejecuta el servicio */
                    await _colaboradoresHuellaDigitalService.CrearHuellasPorColaboradorAsync(ch);
                }
                return Content("OK");
            }

            return Content("No");
        }

        [System.Web.Mvc.HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> UpdateHuellaApiAsync(
     [FromBody] int Id, int dedo, int colaborador, string huella, bool principal, string plantilla_base64)
        {
            if (ModelState.IsValid)
            {
                /* si Principal es true buscamos que no exista un principal ya seleccionado */
                if (principal == true)
                {
                    List<ColaboradoresHuellaDigitalDto> lsHuella = _colaboradoresHuellaDigitalService.GetHuellasPorColaborador(colaborador);

                    foreach (var item in lsHuella)
                    {
                        if (item.principal == true
                            && dedo != item.catalogo_dedo_id)
                        {
                            return Content("Solo puede haber un principal!");
                        }
                    }
                }

                ColaboradoresHuellaDigitalDto ch = await _colaboradoresHuellaDigitalService.Get(new EntityDto<int>(Id));
                ch.principal  = principal;
                ch.colaborador_id = colaborador;
                ch.catalogo_dedo_id = dedo;
                ch.huella = huella;
                ch.fecha_registro = DateTime.Now;
                ch.vigente = true;
               
                ch.plantilla_base64 = plantilla_base64;

                /* Actualizamos */
                await _colaboradoresHuellaDigitalService.Update(ch);
                return Content("OK");
            }
            return Content("No");
        }


        [System.Web.Mvc.HttpPost]
#pragma warning disable CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        public async System.Threading.Tasks.Task<ActionResult> DeleteFotografiaApi(int Idcolaborador, string origen)
#pragma warning restore CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        {
            _colaboradoresFotografiaService.EliminarFotografiaPorOrigen(Idcolaborador, origen);

            return Content("OK");
        }

        [System.Web.Mvc.HttpPost]
#pragma warning disable CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        public async System.Threading.Tasks.Task<ActionResult> GetArchivoFotografiaApi(int Idcolaborador, string origen)
#pragma warning restore CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        {
            var huella = _colaboradoresFotografiaService.GetArchivoFotografia(Idcolaborador, origen);

            if (huella != null)
            {
                return Content(JsonConvert.SerializeObject(huella));
            }

            return Content("NO");
        }



        [System.Web.Mvc.HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> CreateArchivoFotografia([FromBody] int idColaborador, HttpPostedFileBase[] UploadedFile)
        {

            if (ModelState.IsValid)
            {
                ColaboradoresFotografiaDto colaboradoresFotografia = new ColaboradoresFotografiaDto();
                colaboradoresFotografia.colaborador_id = idColaborador;
                colaboradoresFotografia.origen = "CAR_ARC";
                colaboradoresFotografia.fecha_registro = DateTime.Now;

                var resultado = _colaboradoresFotografiaService.CrearActualizarFotografiaPorColaboradorAsync(colaboradoresFotografia, UploadedFile);
                return Content("OK");
            }
            return Content("NO");
        }

   


    public ActionResult ChangeValidacionCedula(int id)

    {
        var Id = _colaboradoresService.UpdateValidacionCedula(id);
        return Content(Id > 0 ? "OK" : "Error");
    }


        [System.Web.Mvc.HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> UpdateColaboradorQR(int id, bool validacion)
        {
            ColaboradoresDto colaborador = await _colaboradoresService.Get(new EntityDto<int>(id));

            if (validacion == true)
            {
                colaborador.validacion_cedula = true;
            }
            else
            {
                colaborador.validacion_cedula = false;
            }

            await _colaboradoresService.Update(colaborador);

            return Content("OK");
        }


        [System.Web.Mvc.HttpPost]
        public async Task<ActionResult> CreateQR(int id)

        {
            Dictionary<String, Object> json = _colaboradoresService.GenerarQr(id);

            var jsonresultado = JsonConvert.SerializeObject(json,
                Newtonsoft.Json.Formatting.Indented,
                new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    NullValueHandling = NullValueHandling.Ignore

                });

            int tiene_servicios = _colaboradoresService.colaboradortieneservicios_(id);
            int tienereservas = _colaboradoresService.colaboradortienereservas(id);

            if (tiene_servicios == 0)
            {

                /* Generar QR */
                QRCodeGenerator qrGeneratora = new QRCodeGenerator();
                QRCodeData qrCodeDataa = qrGeneratora.CreateQrCode(jsonresultado, QRCodeGenerator.ECCLevel.Q);
                QRCode qrCodea = new QRCode(qrCodeDataa);

                using (Bitmap bitMap = qrCodea.GetGraphic(20))
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        bitMap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                        var qr = ms.ToArray();
                        string base64String = Convert.ToBase64String(qr, 0, qr.Length);

                        String[] resultado = { "s_s", base64String };
                        var result = JsonConvert.SerializeObject(resultado);
                        return Content(result);
                    }
                }




            }


            if (tienereservas == 0)
            {
                /* Generar QR */
                QRCodeGenerator qrGenerators = new QRCodeGenerator();
                QRCodeData qrCodeDatas = qrGenerators.CreateQrCode(jsonresultado, QRCodeGenerator.ECCLevel.Q);
                QRCode qrCodes = new QRCode(qrCodeDatas);

                using (Bitmap bitMap = qrCodes.GetGraphic(20))
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        bitMap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                        var qr = ms.ToArray();
                        string base64String = Convert.ToBase64String(qr, 0, qr.Length);

                        String[] resultados = { "s_r", base64String };
                        var result = JsonConvert.SerializeObject(resultados);
                        return Content(result);
                    }
                }

            }



            /* Generar QR */
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(jsonresultado, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);

            using (Bitmap bitMap = qrCode.GetGraphic(20))
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    bitMap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    var qr = ms.ToArray();
                    string base64String = Convert.ToBase64String(qr, 0, qr.Length);
                    String[] resultados = { "OK", base64String };
                    var result = JsonConvert.SerializeObject(resultados);
                    return Content(result);
                }
            }
        }


        [System.Web.Mvc.HttpPost]
        public ActionResult GetNamesHuella(int Id) //Colboradores Ido
        {
            var colaborador = _colaboradoresService.Detalles(Id);
            if (colaborador != null && colaborador.Id > 0)
            {
                var result = JsonConvert.SerializeObject(colaborador,
                  Newtonsoft.Json.Formatting.None,
                  new JsonSerializerSettings
                  {
                      ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                      NullValueHandling = NullValueHandling.Ignore

                  });

                return Content(result);
            }
            else
            {

                return Content("NO");
            }

        }
    }
}
