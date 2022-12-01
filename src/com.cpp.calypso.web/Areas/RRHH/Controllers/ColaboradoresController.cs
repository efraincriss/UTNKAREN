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
using com.cpp.calypso.proyecto.dominio.Models;
using com.cpp.calypso.proyecto.dominio.RecursosHumanos.Models;

namespace com.cpp.calypso.web.Areas.RRHH.Controllers
{
    public class ColaboradoresController : BaseController
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
        public ColaboradoresController(
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
            _discapacidadService = discapacidadService;            _bajaColaboradorService = bajaColaboradorService;
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

        // GET: Proyecto/Colaboradores
        public ActionResult Vista()
        {
            return View();
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult GetColaboradoresApi()
        {
            var list = _colaboradoresService.GetList();
            var result = JsonConvert.SerializeObject(list);
            return Content(result);
        }

        public ActionResult Create()
        {
            ColaboradoresDto colaborador = new ColaboradoresDto();
            return View(colaborador);
        }

        public ActionResult Edit()
        {
            ColaboradoresDto colaborador = new ColaboradoresDto();
            return View(colaborador);
        }
        public ActionResult Reingreso(int Id)
        {
            ViewBag.ColaboradorId = Id;
            return View();
        }


        public ActionResult ReingresoHistorico()
        {
      
            return View();
        }

        public ActionResult Servicios()
        {
            return View();
        }

        public ActionResult CrearServicios()
        {
            ColaboradoresDto colaborador = new ColaboradoresDto();
            return View(colaborador);
        }
        public ActionResult CrearServiciosColaborador(int Id)
        {
            ViewBag.Id = Id;
            return View();
        }
        public ActionResult IndexBajas()
        {
            return View();
        }

        public ActionResult CrearCodigoQR()
        {
            ColaboradoresDto colaborador = new ColaboradoresDto();
            return View(colaborador);
        }

        public ActionResult Bajas()
        {
            return View();
        }

        public ActionResult CrearUsuarioExterno()
        {
            return View();
        }


        [System.Web.Mvc.HttpPost]
#pragma warning disable CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        public async System.Threading.Tasks.Task<ActionResult> CrearServiciosApi(ColaboradorServicio[] servicios)
#pragma warning restore CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        {
            if (ModelState.IsValid)
            {
                _colaboradorServicioService.CreateServicios(servicios);

                return Content("OK");
            }
            return Content("NO");

        }

        [System.Web.Mvc.HttpPost]
        public async System.Threading.Tasks.Task<int> CreateApiAsync(ColaboradoresDto colaborador)
        {
            if (colaborador.Id > 0)
            {
                ColaboradoresDto c = await _colaboradoresService.Get(new EntityDto<int>(colaborador.Id));
                ColaboradoresHistoricoDto historico = new ColaboradoresHistoricoDto();
                historico = c;
                historico.fecha_creacion = DateTime.Now;
                historico.usuario_creacion = User.Identity.Name;
                await _historicoService.InsertOrUpdateAsync(historico);
                var contacto = c.Contacto;
                colaborador.Pais = c.Pais;
                colaborador.AdminRotacion = c.AdminRotacion;
                colaborador.CreationTime = c.CreationTime;
                colaborador.CreatorUserId = c.CreatorUserId;
                colaborador.viene_registro_civil = false;

                c = colaborador;

                c.Contacto = contacto;
                c.ContactoId = colaborador.ContactoId;



                if (colaborador.discapacidad == true)
                {
                    ColaboradorDiscapacidadDto dis = _discapacidadService.GetDiscapacidadColaborador(c.Id);
                    if (dis != null)
                    {
                        //dis.ColaboradoresId = c.Id;
                        dis.catalogo_porcentaje_id = colaborador.catalogo_porcentaje_id.Value;
                        dis.catalogo_tipo_discapacidad_id = colaborador.catalogo_tipo_discapacidad_id.Value;
                        await _discapacidadService.Update(dis);
                    }
                    else
                    {
                        ColaboradorDiscapacidadDto d = new ColaboradorDiscapacidadDto();
                        d.ColaboradoresId = c.Id;
                        d.catalogo_porcentaje_id = colaborador.catalogo_porcentaje_id.Value;
                        d.catalogo_tipo_discapacidad_id = colaborador.catalogo_tipo_discapacidad_id.Value;
                        await _discapacidadService.InsertOrUpdateAsync(d);
                    }

                }
                else
                {
                    colaborador.catalogo_codigo_siniestro_id = _catalogoservice.GetCatalogoPorCodigo(RRHHCodigos.CODIGO_SINIESTRO_NOINCAPACITADO).Id;
                    colaborador.catalogo_codigo_incapacidad_id = _catalogoservice.GetCatalogoPorCodigo(RRHHCodigos.CODIGO_INCAPACIDAD_NOINCAPACITADO).Id;
                }

                ColaboradorFormacionEducativaDto formacion = _formacionEducativaService.GetFormacion(c.Id);
                formacion.formacion = colaborador.formacion.ToString();
                formacion.institucion_educativa = colaborador.institucion_educativa;
                formacion.catalogo_titulo_id = colaborador.catalogo_titulo_id;
                formacion.fecha_registro_senecyt = colaborador.fecha_registro_senecyt;
                await _formacionEducativaService.Update(formacion);
                c.nombres_apellidos = c.primer_apellido + " " + c.segundo_apellido + " " + c.nombres;
                await _colaboradoresService.Update(c);
                return colaborador.Id;

            }
            else
            {

                var ExternoId = _colaboradoresService.SearchColaboradorExterno(colaborador.numero_identificacion);
                if (ExternoId > 0)
                {
                    var update = _colaboradoresService.InactivarColaboradorExterno(ExternoId);

                }
                string result = "";
                if (ExternoId == 0)
                {
                    result = _colaboradoresService.BuscarIdUnicoColaboradores(colaborador.numero_identificacion);
                }
                else
                {
                    result = "FUE_EXTERNO";
                }
                if (result == "NO" || result == "FUE_EXTERNO")
                {
                    var legajo = _colaboradoresService.GetLegajo();

                    if (legajo != "NO")
                    {
                        var nro = int.Parse(legajo) + 1;
                        colaborador.numero_legajo_temporal = (nro).ToString();
                    }
                    else
                    {
                        colaborador.numero_legajo_temporal = (00001).ToString();
                    }

                    if (colaborador.catalogo_codigo_siniestro_id != null && colaborador.catalogo_codigo_incapacidad_id != null)
                    {
                        colaborador.catalogo_codigo_siniestro_id = _catalogoservice.GetCatalogoPorCodigo(RRHHCodigos.CODIGO_SINIESTRO_NOINCAPACITADO).Id;
                        colaborador.catalogo_codigo_incapacidad_id = _catalogoservice.GetCatalogoPorCodigo(RRHHCodigos.CODIGO_INCAPACIDAD_NOINCAPACITADO).Id;
                    }
                    colaborador.viene_registro_civil = false;
                    colaborador.nombres_apellidos = colaborador.primer_apellido + " " + colaborador.segundo_apellido + " " + colaborador.nombres;
                    var c = await _colaboradoresService.Create(colaborador);


                    ColaboradorFormacionEducativaDto formacion = new ColaboradorFormacionEducativaDto();
                    formacion.formacion = colaborador.formacion.ToString();
                    formacion.ColaboradoresId = c.Id;
                    formacion.institucion_educativa = colaborador.institucion_educativa;
                    formacion.catalogo_titulo_id = colaborador.catalogo_titulo_id;
                    formacion.fecha_registro_senecyt = colaborador.fecha_registro_senecyt;

                    await _formacionEducativaService.InsertOrUpdateAsync(formacion);

                    if (colaborador.discapacidad == true)
                    {
                        ColaboradorDiscapacidadDto dis = new ColaboradorDiscapacidadDto();
                        dis.ColaboradoresId = c.Id;
                        dis.catalogo_porcentaje_id = colaborador.catalogo_porcentaje_id.Value;
                        dis.catalogo_tipo_discapacidad_id = colaborador.catalogo_tipo_discapacidad_id.Value;
                        await _discapacidadService.InsertOrUpdateAsync(dis);
                    }

                    return c.Id;

                }
                else
                {
                    return -1;
                }
            }

            //}
            //return 0;
        }

        [System.Web.Mvc.HttpPost]
        public async System.Threading.Tasks.Task<int> CreateContactoApi(int ColaboradoresId, ContactoDto c)
        {
            c.referencia = null;
            c.vigente = true;

            if (c.Id > 0)
            {
                ContactoDto contacto = await _contactoService.Get(new EntityDto<int>(c.Id));
                c.CreationTime = contacto.CreationTime;
                c.CreatorUserId = contacto.CreatorUserId;
                c.Parroquia = contacto.Parroquia;
                c.Comunidad = contacto.Comunidad;

                await _contactoService.Update(c);

                return c.Id;

            }
            else
            {
                var id = await _contactoService.InsertOrUpdateAsync(c);

                ColaboradoresDto colaborador = await _colaboradoresService.Get(new EntityDto<int>(ColaboradoresId));
                colaborador.ContactoId = id.Id;
                await _colaboradoresService.Update(colaborador);

                return id.Id;
            }

        }

        [System.Web.Mvc.HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> CreateEmpleoApi(ColaboradoresDto colaborador)
        {
            var guardar = true;
            if (ModelState.IsValid)
            {
                if (colaborador.posicion != null)
                {
                    var existe = _colaboradoresService.UniquePosicion(colaborador.posicion, colaborador.Id);
                    if (existe == "SI")
                    {
                        guardar = false;
                        return Content("La posición ha sido asignada a otro colaborador");
                    }
                }
                if (colaborador.numero_cuenta != null && colaborador.catalogo_banco_id != null)
                {
                    var infobanco = _colaboradoresService.UniqueCuentaBanco(colaborador.numero_cuenta, colaborador.catalogo_banco_id.Value, colaborador.Id);
                    if (infobanco != "NO")
                    {
                        guardar = false;
                        return Content("La cuenta ingresada pertenece a " + infobanco);
                    }
                }

                if (guardar == true)
                {
                    ColaboradoresDto c = await _colaboradoresService.Get(new EntityDto<int>(colaborador.Id));
                    c.nombres_apellidos = colaborador.primer_apellido != null ? colaborador.primer_apellido : "" + " " + colaborador.segundo_apellido != null ? colaborador.segundo_apellido : "" + " " + colaborador.nombres != null ? colaborador.nombres : "";
                    c.empleado_id_sap_local = colaborador.empleado_id_sap_local;
                    ColaboradoresHistoricoDto historico = new ColaboradoresHistoricoDto();
                    historico = c;
                    historico.fecha_creacion = DateTime.Now;
                    historico.usuario_creacion = User.Identity.Name;
                    await _historicoService.InsertOrUpdateAsync(historico);
                    var contacto = c.Contacto;
                    colaborador.Pais = c.Pais;
                    colaborador.AdminRotacion = c.AdminRotacion;
                    colaborador.CreationTime = c.CreationTime;
                    colaborador.CreatorUserId = c.CreatorUserId;
                    //colaborador.horario_desde = TimeSpan.Parse(colaborador.h_desde);
                    //colaborador.horario_hasta = TimeSpan.Parse(colaborador.h_hasta);

                    c = colaborador;

                    c.Contacto = contacto;
                    c.ContactoId = colaborador.ContactoId;



                    if (colaborador.discapacidad == true)
                    {
                        ColaboradorDiscapacidadDto dis = _discapacidadService.GetDiscapacidadColaborador(c.Id);
                        if (dis != null)
                        {
                            //dis.ColaboradoresId = c.Id;
                            dis.catalogo_porcentaje_id = colaborador.catalogo_porcentaje_id.Value;
                            dis.catalogo_tipo_discapacidad_id = colaborador.catalogo_tipo_discapacidad_id.Value;
                            await _discapacidadService.Update(dis);
                        }
                        else
                        {
                            ColaboradorDiscapacidadDto d = new ColaboradorDiscapacidadDto();
                            d.ColaboradoresId = c.Id;
                            d.catalogo_porcentaje_id = colaborador.catalogo_porcentaje_id.Value;
                            d.catalogo_tipo_discapacidad_id = colaborador.catalogo_tipo_discapacidad_id.Value;
                            await _discapacidadService.InsertOrUpdateAsync(d);
                        }

                    }
                    else
                    {
                        colaborador.catalogo_codigo_siniestro_id = _catalogoservice.GetCatalogoPorCodigo(RRHHCodigos.CODIGO_SINIESTRO_NOINCAPACITADO).Id;
                        colaborador.catalogo_codigo_incapacidad_id = _catalogoservice.GetCatalogoPorCodigo(RRHHCodigos.CODIGO_INCAPACIDAD_NOINCAPACITADO).Id;
                    }

                    if (colaborador.formacion > 0 || colaborador.institucion_educativa != null || colaborador.catalogo_titulo_id > 0 || colaborador.fecha_registro_senecyt != null)
                    {
                        ColaboradorFormacionEducativaDto formacion = _formacionEducativaService.GetFormacion(c.Id);
                        if (formacion != null)
                        {
                            formacion.formacion = colaborador.formacion.ToString();
                            formacion.institucion_educativa = colaborador.institucion_educativa;
                            formacion.catalogo_titulo_id = colaborador.catalogo_titulo_id;
                            formacion.fecha_registro_senecyt = colaborador.fecha_registro_senecyt;
                            await _formacionEducativaService.Update(formacion);
                        }
                        else {
                            ColaboradorFormacionEducativaDto nueva = new ColaboradorFormacionEducativaDto()
                            {
                                catalogo_titulo_id = colaborador.catalogo_titulo_id,
                                ColaboradoresId = c.Id,
                                fecha_registro_senecyt = colaborador.fecha_registro_senecyt,
                                formacion = colaborador.formacion.ToString(),
                                institucion_educativa = colaborador.institucion_educativa,
                                vigente = true

                            };
                            await _formacionEducativaService.Create(nueva);

                        }
                    }


                    await _colaboradoresService.Update(c);

                    return Content("OK");
                }

            }
            return Content("NO");

        }

        [System.Web.Mvc.HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> CreateReingresoApi(ColaboradoresDto colaborador, int IdAntiguo)
        {
            string message = "OK";

            var guardar = true;
            if (ModelState.IsValid)
            {
                if (guardar == true)
                {
                    var apellidosString = String.Concat(colaborador.primer_apellido.Trim()," " +colaborador.segundo_apellido);
                    var completeString = String.Concat(apellidosString.Trim()," "+colaborador.nombres);


                    colaborador.nombres_apellidos = completeString.Trim();
                    var nuevoColaboradorId = _colaboradoresService.ColaboradorReingresoAsync(IdAntiguo,colaborador);
                    if (nuevoColaboradorId == "EXISTE_COLABORADOR_INTERNO_ACTIVO") {
                        message = "Ya se encuentra vigente un colaborador con el mismo número de identificación, verifique en el Listado Principal";
                        return Content(JsonConvert.SerializeObject(new { success = message, nuevoReingresoId = 0 }));
                    }else
                  
                    {
                        ColaboradoresDto c = await _colaboradoresService.Get(new EntityDto<int>(Int32.Parse(nuevoColaboradorId) ));
                        c.nombres_apellidos = completeString;

                        ColaboradoresHistoricoDto historico = new ColaboradoresHistoricoDto();
                        historico = c;
                        historico.fecha_creacion = DateTime.Now;
                        historico.usuario_creacion = User.Identity.Name;
                        await _historicoService.InsertOrUpdateAsync(historico);

                        if (colaborador.discapacidad == true)
                        {
                            ColaboradorDiscapacidadDto dis = _discapacidadService.GetDiscapacidadColaborador(c.Id);
                            if (dis != null)
                            {
                                dis.catalogo_porcentaje_id = colaborador.catalogo_porcentaje_id.Value;
                                dis.catalogo_tipo_discapacidad_id = colaborador.catalogo_tipo_discapacidad_id.Value;
                                await _discapacidadService.Update(dis);
                            }
                            else
                            {
                                ColaboradorDiscapacidadDto d = new ColaboradorDiscapacidadDto();
                                d.ColaboradoresId = c.Id;
                                d.catalogo_porcentaje_id = colaborador.catalogo_porcentaje_id.Value;
                                d.catalogo_tipo_discapacidad_id = colaborador.catalogo_tipo_discapacidad_id.Value;
                                await _discapacidadService.InsertOrUpdateAsync(d);
                            }

                        }
                        else
                        {
                            colaborador.catalogo_codigo_siniestro_id = _catalogoservice.GetCatalogoPorCodigo(RRHHCodigos.CODIGO_SINIESTRO_NOINCAPACITADO).Id;
                            colaborador.catalogo_codigo_incapacidad_id = _catalogoservice.GetCatalogoPorCodigo(RRHHCodigos.CODIGO_INCAPACIDAD_NOINCAPACITADO).Id;
                        }

                        if (colaborador.formacion > 0 || colaborador.institucion_educativa != null || colaborador.catalogo_titulo_id > 0 || colaborador.fecha_registro_senecyt != null)
                        {
                            ColaboradorFormacionEducativaDto formacion = _formacionEducativaService.GetFormacion(c.Id);
                            if (formacion != null)
                            {
                                formacion.formacion = colaborador.formacion.ToString();
                                formacion.institucion_educativa = colaborador.institucion_educativa;
                                formacion.catalogo_titulo_id = colaborador.catalogo_titulo_id;
                                formacion.fecha_registro_senecyt = colaborador.fecha_registro_senecyt;
                                await _formacionEducativaService.Update(formacion);
                            }
                            else
                            {
                                ColaboradorFormacionEducativaDto nueva = new ColaboradorFormacionEducativaDto()
                                {
                                    catalogo_titulo_id = colaborador.catalogo_titulo_id,
                                    ColaboradoresId = c.Id,
                                    fecha_registro_senecyt = colaborador.fecha_registro_senecyt,
                                    formacion = colaborador.formacion.ToString(),
                                    institucion_educativa = colaborador.institucion_educativa,
                                    vigente = true

                                };
                                await _formacionEducativaService.Create(nueva);

                            }
                        }


                        await _colaboradoresService.Update(c);
                    }


                    return Content(JsonConvert.SerializeObject(new { success = message, nuevoReingresoId = nuevoColaboradorId }));
                }

            }
            message = "ERROR";
            return Content(JsonConvert.SerializeObject(new { success = message, nuevoReingresoId = 0 }));

        }



        [System.Web.Mvc.HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> CreateServiciosApi(List<ColaboradorServicioDto> servicios, List<int> idComidas, int? movilizacion)
        {
       

            foreach (var e in servicios)
            {
                if (e.nombre == "alimentacion")
                {
                    var servicio = await _colaboradorServicioService.Create(e);

                    var list = _comidaService.GetComidas(servicio.Id);

                    if (list != null)
                    {
                        foreach (var item in list)
                        {
                            await _comidaService.Delete(item);

                        }
                    }

                    ColaboradoresComidaDto comida = new ColaboradoresComidaDto();

                    foreach (var item in idComidas)
                    {
                        comida.tipo_alimentacion_id = item; //comidas
                        comida.ColaboradorServicioId = servicio.Id;

                        await _comidaService.Create(comida);

                    }

                }
                else if (e.nombre == "movilizacion")
                {
                    var servicio = await _colaboradorServicioService.Create(e);

                    var m = _colaboradorMovilizacionService.GetMovilizacion(servicio.Id);

                    if (m == null)
                    {
                        //await _colaboradorMovilizacionService.Delete(m);
                    }
                    else
                    {
                        await _colaboradorMovilizacionService.Delete(m);
                    }

                    if (movilizacion.HasValue)
                    {
                        ColaboradorMovilizacionDto mov = new ColaboradorMovilizacionDto();
                        var colaborador = _colaboradoresService.GetColaborador(e.ColaboradoresId);

                        mov.catalogo_tipo_movilizacion_id = movilizacion.Value;
                        mov.ColaboradorServicioId = servicio.Id;

                        if (colaborador.ContactoId > 0)
                        {
                            var contacto = _contactoService.GetContacto(colaborador.ContactoId.Value);
                            mov.ComunidadId = contacto.ComunidadId > 0 ? contacto.ComunidadId : null;
                            mov.ParroquiaId = contacto.ParroquiaId > 0 ? contacto.ParroquiaId : null;
                        }

                        await _colaboradorMovilizacionService.Create(mov);
                    }

                }
                else
                {
                    await _colaboradorServicioService.Create(e);
                }

            }


            return Content("OK");

        }

        [System.Web.Mvc.HttpPost]
        public ActionResult CreateBajaApi(ColaboradorBajaModel model)
        {

            bool InsertarBaja = _bajaColaboradorService.InsertarBajaColaborador(model);

            if (InsertarBaja)
            {
                return Content("OK");
            }
            else
            {
                return Content("Error");
            }
            /*
                if (UploadedFile==null )
                {
                    await _bajaColaboradorService.InsertOrUpdateAsync(baja);
                }
                else
                {
                    _bajaColaboradorService.CargarArchivoBaja(baja, UploadedFile);
                }


            if (ModelState.IsValid)
            {
                ColaboradoresDto c = await _colaboradoresService.Get(new EntityDto<int>(baja.ColaboradoresId));

                c.estado = RRHHCodigos.ESTADO_INACTIVO;

                await _colaboradoresService.Update(c);

                return Content("OK");
            }
            else
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return Content("Error");
            }
            */

        }

        [System.Web.Mvc.HttpPost]
        public ActionResult GetColaboradorFiltros(string numeroIdentificacion, string nombres, int grupoPersonal)
        {
            var lista = _colaboradoresService.GetColaboradorPorFiltros(numeroIdentificacion, nombres, grupoPersonal);
            var result = JsonConvert.SerializeObject(lista);
            return Content(result);
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult GetFiltrosBajas(string numeroIdentificacion, string nombres, string estado)
        {
            var lista = _colaboradoresService.GetFiltrosBajas(numeroIdentificacion, nombres, estado);
            var result = JsonConvert.SerializeObject(lista);
            return Content(result);
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult GetFiltrosAusentismo(string numeroIdentificacion, string nombres)
        {

            var lista = _colaboradoresService.SearchColaborador(numeroIdentificacion, nombres);
            /* if (lista != null)
             {
                 foreach (var e in lista)
                 {
                     e.ausentismos = _ausentismoService.GetAusentismosColaborador(e.Id);
                 }
             }
             */
            var result = JsonConvert.SerializeObject(lista,
                Newtonsoft.Json.Formatting.None,
                new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    NullValueHandling = NullValueHandling.Ignore
                });
            return Content(result);
        }

        [System.Web.Mvc.HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> CreateCargaSocialApi(ColaboradorCargaSocialDto cargas)
        {
            ColaboradorCargaSocialDto result = new ColaboradorCargaSocialDto();

            if (ModelState.IsValid)
            {
                if (cargas.tipoIdentificacion != null && cargas.tipoIdentificacion != "0")
                {
                    if (int.Parse(cargas.tipoIdentificacion) > 0)
                    {
                        cargas.Id = int.Parse(cargas.tipoIdentificacion);
                        result = await _cargaSocialService.InsertOrUpdateAsync(cargas);
                    }
                    else
                    {
                        result = await _cargaSocialService.InsertOrUpdateAsync(cargas);
                    }
                }
                else
                {
                    var unique = _cargaSocialService.UniqueIdentification(cargas.ColaboradoresId, cargas.nro_identificacion);

                    if (unique != "SI")
                    {
                        result = await _cargaSocialService.InsertOrUpdateAsync(cargas);
                    }
                    else
                    {
                        return Content("FAMILIAR_REGISTRADO");
                    }

                }

                if (cargas.discapacidad == true)
                {
                    ColaboradorDiscapacidadDto dis = _discapacidadService.GetDiscapacidadCargaSocial(result.Id);
                    if (dis != null)
                    {
                        //dis.ColaboradoresId = c.Id;
                        dis.catalogo_porcentaje_id = cargas.catalogo_porcentaje_id.Value;
                        dis.catalogo_tipo_discapacidad_id = cargas.catalogo_tipo_discapacidad_id.Value;
                        await _discapacidadService.Update(dis);
                    }
                    else
                    {
                        ColaboradorDiscapacidadDto d = new ColaboradorDiscapacidadDto();
                        d.ColaboradorCargaSocialId = result.Id;
                        d.catalogo_porcentaje_id = cargas.catalogo_porcentaje_id.Value;
                        d.catalogo_tipo_discapacidad_id = cargas.catalogo_tipo_discapacidad_id.Value;
                        await _discapacidadService.InsertOrUpdateAsync(d);
                    }

                }
                return Content(result.Id.ToString());

            }


            return Content("");
        }

        [System.Web.Mvc.HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> DeleteCargaSocialApi(int Id)
        {
            ColaboradorCargaSocialDto carga = await _cargaSocialService.Get(new EntityDto<int>(Id));

            carga.vigente = false;
            await _cargaSocialService.Update(carga);

            return Content("OK");
        }

        [System.Web.Mvc.HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> DeleteApiAsync(int id)
        {

            ColaboradoresDto colaborador = await _colaboradoresService.Get(new EntityDto<int>(id));
            colaborador.vigente = false;
            //colaborador.IsDeleted = true;

            ColaboradoresHistoricoDto historico = new ColaboradoresHistoricoDto();
            historico = colaborador;
            await _historicoService.InsertOrUpdateAsync(historico);

            ColaboradorFormacionEducativaDto formacion = _formacionEducativaService.GetFormacion(colaborador.Id);
            if (formacion != null)
            {
                formacion.vigente = false;
                await _formacionEducativaService.Update(formacion);
            }


            ColaboradorDiscapacidadDto discapacidad = _discapacidadService.GetDiscapacidadColaborador(colaborador.Id);
            if (discapacidad != null)
            {
                discapacidad.vigente = false;
                await _discapacidadService.Update(discapacidad);
            }


            List<ColaboradorServicioDto> servicios = _colaboradorServicioService.GetServicios(colaborador.Id);
            foreach (var item in servicios)
            {
                var comidas = _comidaService.GetComidas(item.Id);
                if (comidas != null)
                {
                    foreach (var c in comidas)
                    {
                        await _comidaService.Delete(c);
                    }
                }

                var movilizacion = _colaboradorMovilizacionService.GetMovilizacion(item.Id);
                if (movilizacion != null)
                {
                    await _colaboradorMovilizacionService.Delete(movilizacion);
                }

                item.vigente = false;
                await _colaboradorServicioService.Update(item);
            }

            /*if (colaborador.ContactoId != null)
            {
                ContactoDto contacto = await _contactoService.Get(new EntityDto<int>(colaborador.ContactoId.Value));
                if (contacto != null)
                {
                    contacto.vigente = false;
                    //contacto.IsDeleted = true;
                    await _contactoService.Update(contacto);
                }
            }*/

            List<ColaboradorCargaSocialDto> cargas = _cargaSocialService.GetCargas(colaborador.Id);
            foreach (var item in cargas)
            {
                item.vigente = false;
                await _cargaSocialService.Update(item);
            }


            await _colaboradoresService.Update(colaborador);

            return Content("OK");
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
                    if (f.formacion != null && f.formacion.Length > 0)
                    {
                        list.formacion = f.formacion == "" ? 0 : int.Parse(f.formacion);
                    }
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
        public ActionResult GetColaboradorInfoBasicaApi(int Id)
        {
            var list = _colaboradoresService.GetColaborador(Id);
            var result = JsonConvert.SerializeObject(list);
            return Content(result);
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult GetPaisesApi()
        {
            var paises = _paisservice.GetPaises();
            var result = JsonConvert.SerializeObject(paises);
            return Content(result);
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult GetProvinciasApi(int id)
        {
            var provincias = _provinciaService.ObtenerProvinciaPorPais(id);
            var result = JsonConvert.SerializeObject(provincias);
            return Content(result);
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult GetCantonesApi(int id)
        {
            var cantones = _ciudadService.ObtenerCantonPorProvincia(id);
            var result = JsonConvert.SerializeObject(cantones);
            return Content(result);
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult GetParroquiasApi(int id)
        {
            var parroquias = _parroquiaService.ObtenerParroquiaPorCanton(id);
            var result = JsonConvert.SerializeObject(parroquias);
            return Content(result);
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult GetComunidadesApi(int id)
        {
            var comunidades = _comunidadService.ObtenerComunidadPorParroquia(id);
            var result = JsonConvert.SerializeObject(comunidades);
            return Content(result);
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult GetCatalogosPorTipoApi(int Id)
        {
            //Obtiene los Catlogos del Tipo Identificacion
            var lista = _catalogoservice.ListarCatalogos(Id);//Revisar ID
            var result = JsonConvert.SerializeObject(lista);
            return Content(result);
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult GetCatalogosPorCodigoApi(string codigo)
        {

            //Obtiene el codigo de Tipo de Catalogo
            var tipoCatalogo = _tipocatalogoservice.GetCatalogoPorCodigo(codigo);
            //Obtiene los Catlogos del Tipo Identificacion
            var lista = _catalogoservice.ListarCatalogos(tipoCatalogo.Id);//Revisar ID
            var result = JsonConvert.SerializeObject(lista,
               Newtonsoft.Json.Formatting.None,
               new JsonSerializerSettings
               {
                   ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                   NullValueHandling = NullValueHandling.Ignore

               });
            return Content(result);
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult GetCatalogosPorCodigoApiSinAltaAnulada(string codigo)
        {

            //Obtiene el codigo de Tipo de Catalogo
            var tipoCatalogo = _tipocatalogoservice.GetCatalogoPorCodigo(codigo);
            //Obtiene los Catlogos del Tipo Identificacion
            var lista = _catalogoservice.ListarCatalogos(tipoCatalogo.Id);//Revisar ID
            if (codigo == "ESTADOSCOL") {
                lista = lista.Where(x => x.codigo != "ALTA_ANULADA").ToList();
            }
            var result = JsonConvert.SerializeObject(lista,
               Newtonsoft.Json.Formatting.None,
               new JsonSerializerSettings
               {
                   ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                   NullValueHandling = NullValueHandling.Ignore

               });
            return Content(result);
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult GetComidasApi(int Id)
        {
            var list = _comidaService.GetComidas(Id);
            var result = JsonConvert.SerializeObject(list);
            return Content(result);
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult GetContactoApi(int Id)
        {
            var list = _contactoService.GetContacto(Id);
            if (list.ParroquiaId > 0)
            {
                var p = _parroquiaService.GetParroquia(list.ParroquiaId.Value);
                list.CiudadId = p.CiudadId;
                list.ProvinciaId = p.Ciudad.ProvinciaId;
                var prov = _provinciaService.GetProvincia(list.ProvinciaId.Value);
                list.PaisId = prov.PaisId;
                if (prov.region_amazonica == true)
                {
                    list.amazonica = true;
                }
            }
            var result = JsonConvert.SerializeObject(list);
            return Content(result);
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult GetCargasApi(int Id)
        {
            var list = _cargaSocialService.GetCargas(Id);

            foreach (var c in list)
            {
                var d = _discapacidadService.GetDiscapacidadCargaSocial(c.Id);
                if (d != null)
                {
                    c.catalogo_porcentaje_id = d.catalogo_porcentaje_id;
                    c.catalogo_tipo_discapacidad_id = d.catalogo_tipo_discapacidad_id;
                    c.discapacidad = true;
                    c.nombre_dis = "SI";
                }
                else
                {
                    c.discapacidad = false;
                    c.nombre_dis = "NO";
                }
            }
            var result = JsonConvert.SerializeObject(list);
            return Content(result);
        }

        [System.Web.Mvc.HttpPost]
        public async Task<ActionResult> GetColaboradorRequisito(int id)
        {
            var list = _colaboradoresService.GetColaboradorRequisito(id);

            if (list != null)
            {
                //Get Usuario Loggeado
                list.nombre_estado = User.Identity.GetUserId();
                var idUser = User.Identity.GetUserId();

                list.Usuario = await _usuarioService.Get(new EntityDto<int>(int.Parse(idUser)));

                list.responsabilidades = _colaboradoresService.GetColaboradorUsuario(list.Usuario.Identificacion);

                var req_cumple = _colaboradoresRequisitoService.GetList(list.Id);
                if (req_cumple.Count > 0)
                {
                    list.req_cumple = req_cumple;
                }
                if (list.catalogo_grupo_personal_id != null)
                {
                    var req = _requisitosServicioService.GetRequisitoPorGrupo(list.catalogo_grupo_personal_id.Value);
                    list.requisitos = req;
                }

                if (list.ContactoId > 0)
                {
                    var contacto = _contactoService.GetContacto(list.ContactoId.Value);
                    list.telefono = contacto.celular;
                }

                //var result = JsonConvert.SerializeObject(list);
                var result = JsonConvert.SerializeObject(list,
                Newtonsoft.Json.Formatting.None,
                new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    NullValueHandling = NullValueHandling.Ignore
                });
                return Content(result);
            }
            return Content("NO");
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult GetColaboradorAusentismos(int id)
        {
            #region Bad Code Jess
            /*
            var list = _colaboradoresService.GetColaboradorRequisito(id);

            if (list != null)
            {
                var ausentismos = _ausentismoService.GetAusentismosColaborador(list.Id);
                if (ausentismos.Count > 0)
                {
                    list.ausentismos = ausentismos;
                }
                if (list.catalogo_grupo_personal_id != null)
                {
                    var req = _requisitosServicioService.GetRequisitoPorGrupo(list.catalogo_grupo_personal_id.Value);
                    list.requisitos = req;
                }

                if (list.ContactoId > 0)
                {
                    var contacto = _contactoService.GetContacto(list.ContactoId.Value);
                    list.telefono = contacto.celular;
                }
              
                //var result = JsonConvert.SerializeObject(list);
                  */
            #endregion

            var list = _ausentismoService.GetAusentismosColaborador(id);
            var result = JsonConvert.SerializeObject(list,
                Newtonsoft.Json.Formatting.None,
                new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    NullValueHandling = NullValueHandling.Ignore
                });
            return Content(result);

        }

        [System.Web.Mvc.HttpPost]
        public ActionResult GetDataColaboradorAusentismos(int id)
        {
            #region Bad Code Jess

            var list = _colaboradoresService.GetColaboradorRequisito(id);

            if (list != null)
            {
                var ausentismos = _ausentismoService.GetAusentismosColaborador(list.Id);
                if (ausentismos.Count > 0)
                {
                    list.ausentismos = ausentismos;
                }
                if (list.catalogo_grupo_personal_id != null)
                {
                    var req = _requisitosServicioService.GetRequisitoPorGrupo(list.catalogo_grupo_personal_id.Value);
                    list.requisitos = req;
                }

                if (list.ContactoId > 0)
                {
                    var contacto = _contactoService.GetContacto(list.ContactoId.Value);
                    list.telefono = contacto.celular;
                }

                //var result = JsonConvert.SerializeObject(list);

                #endregion

                /* var list = _ausentismoService.GetAusentismosColaborador(id);*/
                var result = JsonConvert.SerializeObject(list,
                    Newtonsoft.Json.Formatting.None,
                    new JsonSerializerSettings
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                        NullValueHandling = NullValueHandling.Ignore
                    });
                return Content(result);

            }
            return Content("ERROR");
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult GetBajaApi(int Id)
        {
            var list = _bajaColaboradorService.GetBaja(Id);
            var result = JsonConvert.SerializeObject(list);
            return Content(result);
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult GetColaboradoresBajasApi()
        {
            //var colaboradores = _colaboradoresService.GetColaboradorBajas();
            var colaboradores = _bajaColaboradorService.GetBajas();
            var result = JsonConvert.SerializeObject(colaboradores);
            return Content(result);
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult GetServiciosColaboradorApi(int Id)
        {
            var servicios = _colaboradorServicioService.GetServicios(Id);
            var result = JsonConvert.SerializeObject(servicios);
            return Content(result);
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult GetMovilizacionApi(int Id)
        {
            var movilizacion = _colaboradorMovilizacionService.GetMovilizacion(Id);
            var result = JsonConvert.SerializeObject(movilizacion);
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

        [System.Web.Mvc.HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> GetHuellasPorColaboradorApi(int Id)
        {
            var list = _colaboradoresHuellaDigitalService.GetHuellasPorColaborador(Id);
            var result = JsonConvert.SerializeObject(list);
            return Content(result);
        }

        public ActionResult Huella() //CreateHuella
        {
            return View();
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
                ch.principal = principal;
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
#pragma warning disable CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        public async System.Threading.Tasks.Task<ActionResult> DeleteFotografiaApi(int Idcolaborador, string origen)
#pragma warning restore CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        {
            _colaboradoresFotografiaService.EliminarFotografiaPorOrigen(Idcolaborador, origen);

            return Content("OK");
        }

        public ActionResult IndexConsumoWS()
        {
            return View();
        }



        public ActionResult CreateValidacionCedula(int id)

        {
            var Id = _colaboradoresService.UpdateValidacionCedula(id);
            return Content(Id > 0 ? "OK" : "Error");
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
        public async System.Threading.Tasks.Task<ActionResult> GetUsuario()
        {
            var usuario = await _usuarioService.Get(User.Identity.Name);
            var result = JsonConvert.SerializeObject(usuario);
            return Content(result);
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
        public async Task<ActionResult> EditEstadoColaboradorApi(int id, string estado)
        {
            ColaboradoresDto c = await _colaboradoresService.Get(new EntityDto<int>(id));

            c.estado = estado;

            await _colaboradoresService.Update(c);

            return Content("OK");

        }

        [System.Web.Mvc.HttpPost]
        public async Task<ActionResult> GetAltaManualApi(int[] ids)
        {
            List<ColaboradoresDto> colaboradores = new List<ColaboradoresDto>();
            if (ids != null)
            {
                foreach (var e in ids)
                {
                    var c = _colaboradoresService.GetColaboradorInfoCompleta(e);
                    colaboradores.Add(c);
                }


                if (colaboradores != null)
                {
                    var excel = await _colaboradoresService.GenerarExcelCarga(colaboradores, true);

                    if (excel == "OK")
                    {
                        foreach (var e in colaboradores)
                        {
                            ColaboradoresDto c = await _colaboradoresService.Get(new EntityDto<int>(e.Id));
                            c.estado = "ENVIADO SAP";
                            c.fecha_alta = DateTime.Now;

                            await _colaboradoresService.Update(c);
                        }
                        return Content("OK");
                    }
                }
            }

            return Content("NO");
        }

        [System.Web.Http.HttpPost]
        public ActionResult GetCertificadoApi(int id, DateTime a, DateTime b)
        {
            var word = _colaboradoresService.GenerarWord(id, a, b);
            if (word.Length > 0)
            {
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
            else
            {
                return Content("");
            }


        }
        //CONSUMO DE WEB SERVICE
        public ActionResult Consumir(string cedula)
        {
            try
            {

                var objeto = _webService.BusquedaPorCedulaRegistroCivil(cedula);
                var insertConsultaPublica = _webService.GuardaInformacionWebServiceenConsultaPublica(objeto, false);
                //var dto = _colaboradoresService.ChangeResultXMLObject(objeto);

                var result = JsonConvert.SerializeObject(objeto,
                    Newtonsoft.Json.Formatting.None,
                    new JsonSerializerSettings
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                        NullValueHandling = NullValueHandling.Ignore
                    });
                return Content(result);
            }
            catch (Exception e)
            {
                ElmahExtension.LogToElmah(new Exception("Error WS " + e.Message));
                return Content("SIN_RESPUESTA");
            }
        }
        //CONSUMO DE WEB SERVICE HUELLA
        public ActionResult ConsumirHuella(string cedula, string huella_dactilar)
        {
            try
            {

                var objeto = _webService.BusquedaPorCedulaRegistroCivilHuellaDigital(cedula, huella_dactilar);
                var insertConsultaPublica = _webService.GuardaInformacionWebServiceenConsultaPublica(objeto, true);


                var result = JsonConvert.SerializeObject(objeto,
                    Newtonsoft.Json.Formatting.None,
                    new JsonSerializerSettings
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                        NullValueHandling = NullValueHandling.Ignore
                    });
                return Content(result);
            }
            catch (Exception e)
            {
                ElmahExtension.LogToElmah(new Exception("Error WS " + e.Message));
                return Content("SIN_RESPUESTA");
            }
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult GetParametroAPI(string codigo)
        {

            var parametro = _colaboradoresService.getParametroPorCodigo(codigo);
            var result = JsonConvert.SerializeObject(parametro);

            return Content(result);
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult GetInfoColaboradorWSApi(int tipoIdentificacion, string cedula, string huella_dactilar)
        {
            if (ModelState.IsValid)
            {
                var colaborador = _colaboradoresService.GetInfoColaboradorWS(tipoIdentificacion, cedula, huella_dactilar);

                if (colaborador != null)
                {
                    if (colaborador.vigente == true && colaborador.estado != RRHHCodigos.ESTADO_INACTIVO)
                    {
                        return Content("Número de Identificación ingresada ya existe");
                    }
                    else
                    {
                        var result = JsonConvert.SerializeObject(colaborador);
                        return Content(result);
                    }

                }
                else
                {
                    return Content("NO");
                }

            }
            return Content("");

        }

        [System.Web.Mvc.HttpPost]
        public ActionResult GetInfoCargaSocialWSApi(int tipoIdentificacion, string cedula)
        {
            if (ModelState.IsValid)
            {
                var carga = _cargaSocialService.GetInfoCargaSocialWS(tipoIdentificacion, cedula);

                if (carga != null)
                {
                    var result = JsonConvert.SerializeObject(carga);
                    return Content(result);
                }
                else
                {
                    return Content("NO");
                }

            }
            return Content("");

        }

        public ActionResult GetListaProvinciasApi()
        {
            var provincias = _provinciaService.GetProvincias();
            var result = JsonConvert.SerializeObject(provincias);
            return Content(result);
        }

        public ActionResult GetListaCiudadesApi()
        {
            var ciudades = _ciudadService.GetCiudades();
            var result = JsonConvert.SerializeObject(ciudades);
            return Content(result);
        }

        public ActionResult GetListaParroquiasApi()
        {
            var parroquias = _parroquiaService.GetParroquias();
            var result = JsonConvert.SerializeObject(parroquias);
            return Content(result);
        }

        public ActionResult GetArchivoApi(int id)
        {
            var archivo = _colaboradoresService.GetArchivo(id);
            var result = JsonConvert.SerializeObject(archivo);
            return Content(result);
        }

        public ActionResult GetUsuariosExternosApi()
        {
            var usuarios = _colaboradoresService.GetUsuariosExternos();
            var result = JsonConvert.SerializeObject(usuarios);
            return Content(result);
        }

        public ActionResult GetUsuarioExternoApi(int id)
        {
            var usuarios = _colaboradoresService.GetUsuarioExterno(id);
            usuarios.visita = _visitasService.GetVisitasPorColaborador(id);
            var result = JsonConvert.SerializeObject(usuarios);
            return Content(result);
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult GetUsuarioExternoBusquedaApi(string numeroIdentificacion, string nombres)
        {
            var usuarios = _colaboradoresService.GetUusuarioFiltros(numeroIdentificacion, nombres);
            foreach (var u in usuarios)
            {
                var visita = _visitasService.GetVisitasPorColaborador(u.Id);
                if (visita != null)
                {
                    u.visita = visita;
                }
            }

            // var result = JsonConvert.SerializeObject(usuarios);
            var result = JsonConvert.SerializeObject(usuarios,
                Newtonsoft.Json.Formatting.None,
                new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    NullValueHandling = NullValueHandling.Ignore

                });
            return Content(result);
        }

        public async Task<ActionResult> CreateUsuarioExterno(ColaboradoresDto usuario)
        {
            if (ModelState.IsValid)
            {
                var result = _colaboradoresService.UniqueIdentification(usuario.numero_identificacion);
                if (result == "INACTIVO" || result == "NO")
                {
                    ContactoDto contacto = new ContactoDto();
                    contacto.correo_electronico = usuario.email;
                    contacto.celular = usuario.telefono;

                    var c = await _contactoService.InsertOrUpdateAsync(contacto);

                    if (c.Id > 0)
                    {
                        var destinos = _catalogoservice.ListarCatalogos(RRHHCodigos.DESTINO);
                        var destino = destinos.Find(x => x.codigo == RRHHCodigos.DESTINO_FORANEO);
                        var grupos = _catalogoservice.ListarCatalogos(RRHHCodigos.GRUPO_PERSONAL);
                        var grupo_personal = grupos.Find(x => x.codigo == RRHHCodigos.GRUPO_PERSONAL_VISITA);
                        usuario.es_externo = true;
                        usuario.ContactoId = c.Id;
                        usuario.catalogo_destino_estancia_id = destino.Id;
                        usuario.catalogo_grupo_personal_id = grupo_personal.Id;
                        //usuario.empleado_id_sap =Int32.Parse(usuario.numero_identificacion);
                        var col = await _colaboradoresService.InsertOrUpdateAsync(usuario);
                        return Content(col.Id.ToString());
                    }
                    else
                    {
                        return Content("No se ha podido guardar !");
                    }
                }
                else
                {
                    return Content("Existe");
                }




            }
            return Content("NO");
        }

        public async Task<ActionResult> EditUsuarioExterno(ColaboradoresDto usuario)
        {
            if (ModelState.IsValid)
            {
                ContactoDto contacto = await _contactoService.Get(new EntityDto<int>(usuario.ContactoId.Value));
                contacto.Id = usuario.ContactoId.Value;
                contacto.correo_electronico = usuario.email;
                contacto.celular = usuario.telefono;
                var c = await _contactoService.Update(contacto);

                ColaboradoresDto colaborador = await _colaboradoresService.Get(new EntityDto<int>(usuario.Id));

                usuario.es_externo = true;
                usuario.Pais = colaborador.Pais;
                usuario.Contacto = colaborador.Contacto;
                usuario.CreationTime = colaborador.CreationTime;
                usuario.CreatorUserId = colaborador.CreatorUserId;
                usuario.catalogo_destino_estancia_id = colaborador.catalogo_destino_estancia_id;
                //usuario.empleado_id_sap = Int32.Parse(usuario.numero_identificacion);
                usuario.catalogo_grupo_personal_id = colaborador.catalogo_grupo_personal_id;
                var col = await _colaboradoresService.Update(usuario);

                return Content(col.Id.ToString());
            }
            return Content("NO");
        }

        public ActionResult GetCompruebaUsuario(string numero)
        {
            var result = _colaboradoresService.UniqueUsuarioExterno(numero);
            return Content(result);
        }

        public ActionResult GetResponsable(string nombre)
        {
            var list = _colaboradoresService.GetListaResponsables(nombre);
            var result = JsonConvert.SerializeObject(list);
            return Content(result);
        }

        public async Task<ActionResult> GetExcelCargaMasiva(HttpPostedFileBase UploadedFile)
        {
            if (UploadedFile != null)
            {

                // tdata.ExecuteCommand("truncate table OtherCompanyAssets");  
                if (UploadedFile.ContentType == "application/vnd.ms-excel" || UploadedFile.ContentType ==
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                {
                    var CatalogoTiposIdentificacion = _catalogoservice.ListarCatalogosporcodigo(RRHHCodigos.TIPO_IDENTIFICACION);

                    var CatalogosGenero = _catalogoservice.ListarCatalogosporcodigo(RRHHCodigos.GENERO);

                    var CatalogosEtnia = _catalogoservice.ListarCatalogosporcodigo(RRHHCodigos.ETNIA);

                    var CatalogosNacionalidades = _catalogoservice.ListarCatalogosporcodigo(RRHHCodigos.NACIONALIDAD);

                    var CatalogosDestino = _catalogoservice.ListarCatalogosporcodigo(RRHHCodigos.DESTINO);

                    var CatalogosEncargadoPersonal = _catalogoservice.ListarCatalogosporcodigo(RRHHCodigos.ENCARGADO_PERSONAL);

                    var CatalogosSector = _catalogoservice.ListarCatalogosporcodigo(RRHHCodigos.SECTOR);

                    var CatalogosCargo = _catalogoservice.ListarCatalogosporcodigo(RRHHCodigos.CARGO);

                    var CatalogosProyectos = _catalogoservice.ListarCatalogosporcodigo(RRHHCodigos.PROYECTO);

                    var CatalogosGrupoPersonal = _catalogoservice.ListarCatalogosporcodigo(RRHHCodigos.GRUPOPERSONAL);


                    var paises = _paisservice.GetPaises();

                    var parroquias = _parroquiaService.GetParroquias();
                    List<string> errores = new List<string>();
                    List<ColaboradoresDto> colaboradores = new List<ColaboradoresDto>();

                    string fileName = UploadedFile.FileName;
                    string fileContentType = UploadedFile.ContentType;
                    byte[] fileBytes = new byte[UploadedFile.ContentLength];
                    var data = UploadedFile.InputStream.Read(fileBytes, 0,
                        Convert.ToInt32(UploadedFile.ContentLength));

                    using (var package = new ExcelPackage(UploadedFile.InputStream))
                    {
                        var currentSheet = package.Workbook.Worksheets;
                        var workSheet = currentSheet.First();
                        var noOfCol = workSheet.Dimension.End.Column;

                        if (noOfCol == 0)
                        {
                            errores.Add("El formato del archivo no es correcto");
                        }
                        else
                        {
                            var noOfRow = workSheet.Dimension.End.Row;

                            for (int rowIterator = 3; rowIterator <= noOfRow; rowIterator++)
                            {
                                ColaboradoresDto colaborador = new ColaboradoresDto();

                                /* TIPO IDENTIFICACION */
                                if (workSheet.Cells[rowIterator, 2].Value == null)
                                {
                                    errores.Add("Obligatorio Tipo Identificación fila: " + rowIterator);
                                }
                                else
                                {

                                    var tipoIdentificacion = (workSheet.Cells[rowIterator, 2].Value).ToString();
                                    var existe = false;
                                    var ExisteCatalogo = (from x in CatalogoTiposIdentificacion
                                                          where x.nombre == tipoIdentificacion
                                                          select x
                                                  ).FirstOrDefault();


                                    if (ExisteCatalogo != null && ExisteCatalogo.Id > 0)
                                    {
                                        existe = true;
                                        colaborador.catalogo_tipo_identificacion_id = ExisteCatalogo.Id;
                                        if (tipoIdentificacion == RRHHCodigos.TIPO_IDENTIFICACION_CEDULA)
                                        {
                                            colaborador.validacion_cedula = true;
                                        }

                                    }
                                    if (existe == false)
                                    {
                                        errores.Add("Error Tipo Identificación fila: " + rowIterator + " no existe");
                                    }
                                }

                                /* NUMERO DE IDENTIFICACION*/
                                if (workSheet.Cells[rowIterator, 3].Value == null)
                                {
                                    errores.Add("Obligatorio Número Identificación fila: " + rowIterator);
                                }
                                else
                                {
                                    var numeroIdentificacion = (workSheet.Cells[rowIterator, 3].Value).ToString();

                                    var existe = _colaboradoresService.UniqueIdentification(numeroIdentificacion);

                                    if (existe == "NO")
                                    {
                                        if (numeroIdentificacion.Length == 10)
                                        {
                                            var cedula = _colaboradoresService.VerificaIdentificacion(numeroIdentificacion);
                                            if (cedula == true)
                                            {
                                                colaborador.numero_identificacion = numeroIdentificacion;
                                            }
                                            else
                                            {
                                                errores.Add("Número de identificación " + numeroIdentificacion + " es inválido");
                                            }
                                        }
                                        else if (numeroIdentificacion.Length > 10 && numeroIdentificacion.Length <= 25)
                                        {
                                            colaborador.numero_identificacion = numeroIdentificacion;
                                        }
                                        else
                                        {
                                            errores.Add("Número de identificación " + numeroIdentificacion + " es inválido");
                                        }

                                    }
                                    else if (existe == "SI")
                                    {
                                        errores.Add("Número de identificación " + numeroIdentificacion + " ya existe");
                                    }

                                }

                                /* CÓDIGO DACTILAR*/
                                if (workSheet.Cells[rowIterator, 4].Value == null)
                                {
                                    errores.Add("Obligatorio Código Dactilar fila: " + rowIterator);
                                }
                                else
                                {
                                    var codigoDactilar = (workSheet.Cells[rowIterator, 4].Value).ToString();
                                    colaborador.codigo_dactilar = codigoDactilar;
                                }

                                /* PRIMER APELLIDO */
                                if (workSheet.Cells[rowIterator, 5].Value == null)
                                {
                                    errores.Add("Obligatorio Primer Apellido fila: " + rowIterator);
                                }
                                else
                                {
                                    var primerApellido = (workSheet.Cells[rowIterator, 5].Value).ToString();
                                    colaborador.primer_apellido = primerApellido;
                                }

                                /* SEGUNDO APELLIDO */
                                if (workSheet.Cells[rowIterator, 6].Value == null)
                                {
                                    errores.Add("Obligatorio Segundo Apellido fila: " + rowIterator);
                                }
                                else
                                {
                                    var segundoApellido = (workSheet.Cells[rowIterator, 6].Value).ToString();
                                    colaborador.segundo_apellido = segundoApellido;
                                }

                                /* NOMBRES */
                                if (workSheet.Cells[rowIterator, 7].Value == null)
                                {
                                    errores.Add("Obligatorio Nombres fila: " + rowIterator);
                                }
                                else
                                {
                                    var nombres = (workSheet.Cells[rowIterator, 7].Value).ToString();
                                    colaborador.nombres = nombres;
                                }

                                /* FECHA NACIMIENTO */
                                if (workSheet.Cells[rowIterator, 8].Value == null)
                                {
                                    errores.Add("Obligatorio Fecha Nacimiento fila: " + rowIterator);
                                }
                                else
                                {
                                    var fechaNacimiento = (workSheet.Cells[rowIterator, 8].Value).ToString();
                                    colaborador.fecha_nacimiento = Convert.ToDateTime(fechaNacimiento);
                                }

                                /* GENERO */
                                if (workSheet.Cells[rowIterator, 9].Value == null)
                                {
                                    errores.Add("Obligatorio Género fila: " + rowIterator);
                                }
                                else
                                {
                                    var genero = (workSheet.Cells[rowIterator, 9].Value).ToString();
                                    var existe = false;

                                    var ExisteCatalogo = (from x in CatalogosGenero
                                                          where x.nombre == genero
                                                          select x
                                        ).FirstOrDefault();


                                    if (ExisteCatalogo != null && ExisteCatalogo.Id > 0)
                                    {
                                        existe = true;
                                        colaborador.catalogo_genero_id = ExisteCatalogo.Id;
                                    }

                                    if (existe == false)
                                    {
                                        errores.Add("Error Género fila: " + rowIterator + " no existe");
                                    }
                                }

                                /* ETNIA */
                                var etnia = (workSheet.Cells[rowIterator, 10].Value ?? "").ToString();
                                if (etnia != "")
                                {
                                    var existe = false;
                                    var ExisteCatalogo = (from x in CatalogosEtnia
                                                          where x.nombre == etnia
                                                          select x
                                       ).FirstOrDefault();
                                    if (ExisteCatalogo != null && ExisteCatalogo.Id > 0)
                                    {
                                        existe = true;
                                        colaborador.catalogo_etnia_id = ExisteCatalogo.Id;
                                    }

                                    if (existe == false)
                                    {
                                        errores.Add("Error Etnia fila: " + rowIterator + " no existe");
                                    }
                                }

                                /* PAIS DE NACIMIENTO */
                                if (workSheet.Cells[rowIterator, 11].Value == null)
                                {
                                    errores.Add("Obligatorio Pais Nacimiento fila: " + rowIterator);
                                }
                                else
                                {
                                    var paisNacimiento = (workSheet.Cells[rowIterator, 11].Value).ToString();
                                    var existe = false;

                                    var ExisteCatalogo = (from x in paises
                                                          where x.nombre == paisNacimiento
                                                          select x
                                     ).FirstOrDefault();


                                    if (ExisteCatalogo != null && ExisteCatalogo.Id > 0)
                                    {
                                        existe = true;
                                        colaborador.PaisId = ExisteCatalogo.Id;
                                    }
                                    if (existe == false)
                                    {
                                        errores.Add("Error Pais de Nacimiento fila: " + rowIterator + " no existe");
                                    }
                                }

                                /* NACIONALIDAD */
                                var nacionalidad = (workSheet.Cells[rowIterator, 12].Value ?? "").ToString();
                                if (nacionalidad != "")
                                {
                                    var existe = false;

                                    var ExisteCatalogo = (from x in CatalogosNacionalidades
                                                          where x.nombre == nacionalidad
                                                          select x
                                    ).FirstOrDefault();


                                    if (ExisteCatalogo != null && ExisteCatalogo.Id > 0)
                                    {
                                        existe = true;
                                        colaborador.pais_pais_nacimiento_id = ExisteCatalogo.Id;
                                    }


                                    if (existe == false)
                                    {
                                        errores.Add("Error Nacionalidad fila: " + rowIterator + " no existe");
                                    }
                                }

                                /* VALIDAR FECHA DE INGRESO */
                                if (workSheet.Cells[rowIterator, 13].Value == null)
                                {
                                    errores.Add("Obligatorio Fecha Ingreso fila: " + rowIterator);
                                }
                                else
                                {
                                    var fechaIngreso = (workSheet.Cells[rowIterator, 13].Value).ToString();
                                    colaborador.fecha_ingreso = Convert.ToDateTime(fechaIngreso);
                                }

                                /* VALIDAR DESTINO */
                                if (workSheet.Cells[rowIterator, 14].Value == null)
                                {
                                    errores.Add("Obligatorio Destino fila: " + rowIterator);
                                }
                                else
                                {
                                    var destino = (workSheet.Cells[rowIterator, 14].Value).ToString();
                                    var existe = false;

                                    var ExisteCatalogo = (from x in CatalogosDestino
                                                          where x.nombre == destino
                                                          select x
                                     ).FirstOrDefault();

                                    if (ExisteCatalogo != null && ExisteCatalogo.Id > 0)
                                    {
                                        existe = true;
                                        colaborador.catalogo_destino_estancia_id = ExisteCatalogo.Id;
                                    }

                                    if (existe == false)
                                    {
                                        errores.Add("Error Destino fila: " + rowIterator + " no existe");
                                    }
                                }

                                /* ENCARGADO DE PERSONAL */
                                var encargadoPersonal = (workSheet.Cells[rowIterator, 15].Value ?? "").ToString();
                                if (encargadoPersonal != "")
                                {
                                    var existe = false;
                                    var ExisteCatalogo = (from x in CatalogosEncargadoPersonal
                                                          where x.nombre == encargadoPersonal
                                                          select x
                                  ).FirstOrDefault();

                                    if (ExisteCatalogo != null && ExisteCatalogo.Id > 0)
                                    {
                                        existe = true;
                                        colaborador.catalogo_encargado_personal_id = ExisteCatalogo.Id;
                                    }
                                    if (existe == false)
                                    {
                                        errores.Add("Error Encargado Personal fila: " + rowIterator + " no existe");
                                    }
                                }

                                /* AREA/ SECTOR */
                                if (workSheet.Cells[rowIterator, 16].Value == null)
                                {
                                    errores.Add("Obligatorio Área/Sector fila: " + rowIterator);
                                }
                                else
                                {
                                    var sector = (workSheet.Cells[rowIterator, 16].Value).ToString();
                                    var existe = false;

                                    var ExisteCatalogo = (from x in CatalogosSector
                                                          where x.nombre == sector
                                                          select x
                                     ).FirstOrDefault();

                                    if (ExisteCatalogo != null && ExisteCatalogo.Id > 0)
                                    {
                                        existe = true;
                                        colaborador.catalogo_sector_id = ExisteCatalogo.Id;
                                    }

                                    if (existe == false)
                                    {
                                        errores.Add("Error Área/Sector fila: " + rowIterator + " no existe");
                                    }
                                }

                                /* CARGO */
                                if (workSheet.Cells[rowIterator, 17].Value == null)
                                {
                                    errores.Add("Obligatorio Cargo fila: " + rowIterator);
                                }
                                else
                                {
                                    var cargo = (workSheet.Cells[rowIterator, 17].Value).ToString();
                                    var existe = false;

                                    var ExisteCatalogo = (from x in CatalogosCargo
                                                          where x.nombre == cargo
                                                          select x
                                ).FirstOrDefault();

                                    if (ExisteCatalogo != null && ExisteCatalogo.Id > 0)
                                    {
                                        existe = true;
                                        colaborador.catalogo_cargo_id = ExisteCatalogo.Id;
                                    }

                                    if (existe == false)
                                    {
                                        errores.Add("Error Cargo fila: " + rowIterator + " no existe");
                                    }
                                }

                                /* PARROQUIA */
                                if (workSheet.Cells[rowIterator, 18].Value == null)
                                {
                                    errores.Add("Obligatorio Parroquia fila: " + rowIterator);
                                }
                                else
                                {
                                    var parroquia = (workSheet.Cells[rowIterator, 18].Value).ToString();
                                    var existe = false;

                                    var ExisteCatalogo = (from x in parroquias
                                                          where x.nombre == parroquia
                                                          select x
                                    ).FirstOrDefault();

                                    if (ExisteCatalogo != null && ExisteCatalogo.Id > 0)
                                    {
                                        existe = true;
                                        colaborador.parroquia = ExisteCatalogo.Id;
                                    }

                                    if (existe == false)
                                    {
                                        errores.Add("Error Parroquia fila: " + rowIterator + " no existe");
                                    }
                                }

                                /* COMUNIDAD */
                                var comunidad = (workSheet.Cells[rowIterator, 19].Value ?? "").ToString();

                                /* PROYECTO */
                                if (workSheet.Cells[rowIterator, 20].Value == null)
                                {
                                    errores.Add("Campo Proyecto Obligatorio fila: " + rowIterator);
                                }
                                else
                                {
                                    var proyecto = (workSheet.Cells[rowIterator, 20].Value).ToString();
                                    var existe = false;

                                    var ExisteCatalogo = (from x in CatalogosProyectos
                                                          where x.nombre == proyecto
                                                          select x
                                ).FirstOrDefault();

                                    if (ExisteCatalogo != null && ExisteCatalogo.Id > 0)
                                    {
                                        existe = true;
                                        colaborador.ContratoId = ExisteCatalogo.Id;
                                    }

                                    if (existe == false)
                                    {
                                        errores.Add("Error fila: " + rowIterator + " El Nombre del Proyecto no existe");
                                    }
                                }


                                /* GRUPO PERSONAL */
                                if (workSheet.Cells[rowIterator, 21].Value == null)
                                {
                                    errores.Add("Campo Grupo Personal Obligatorio fila: " + rowIterator);
                                }
                                else
                                {
                                    var grupopersonal = (workSheet.Cells[rowIterator, 20].Value).ToString();
                                    var existe = false;

                                    var ExisteCatalogo = (from x in CatalogosGrupoPersonal
                                                          where x.nombre == grupopersonal
                                                          select x
                                ).FirstOrDefault();

                                    if (ExisteCatalogo != null && ExisteCatalogo.Id > 0)
                                    {
                                        existe = true;
                                        colaborador.catalogo_grupo_personal_id = ExisteCatalogo.Id;
                                    }

                                    if (existe == false)
                                    {
                                        errores.Add("Error fila: " + rowIterator + " El Nombre del Grupo Personal no existe");
                                    }
                                }

                                colaboradores.Add(colaborador);


                            }
                        }


                    }

                    if (errores.ToList().Count == 0)
                    {

                        foreach (var i in colaboradores)
                        {
                            var unique = _colaboradoresService.UniqueIdentification(i.numero_identificacion);
                            i.nombres_apellidos = i.primer_apellido + " " + i.segundo_apellido + " " + i.nombres;
                            i.nombres_apellidos = i.nombres_apellidos;

                            var consultaBDD = _colaboradoresService.GetInfoColaboradorWS(i.catalogo_tipo_identificacion_id.Value, i.numero_identificacion, i.codigo_dactilar);
                            if (consultaBDD != null)
                            {
                                i.codigo_dactilar = consultaBDD.codigo_dactilar;
                                i.primer_apellido = consultaBDD.primer_apellido;
                                i.segundo_apellido = consultaBDD.segundo_apellido;
                                i.nombres = consultaBDD.nombres;
                                i.fecha_nacimiento = consultaBDD.fecha_nacimiento;
                                i.catalogo_genero_id = consultaBDD.catalogo_genero_id;
                                i.catalogo_etnia_id = consultaBDD.catalogo_etnia_id;
                                i.PaisId = consultaBDD.PaisId;
                                i.pais_pais_nacimiento_id = consultaBDD.pais_pais_nacimiento_id;
                                i.empleado_id_sap = consultaBDD.empleado_id_sap;
                            }
                            else
                            {

                                var consultaCandidatos = _consultaPublicaService.ExisteCandidato(i.numero_identificacion);
                                if (consultaCandidatos.Id > 0 && consultaCandidatos.fecha_consulta.HasValue)
                                {
                                    i.nombres_apellidos = consultaCandidatos.nombres_completos;
                                    i.fecha_nacimiento = consultaCandidatos.fecha_nacimiento;
                                    i.viene_registro_civil = true;
                                    i.fecha_registro_civil = consultaCandidatos.fecha_consulta;
                                    //Valida Genero
                                    if (consultaCandidatos.sexo == RRHHCodigos.SEXO_HOMBRE)
                                    {
                                        var sexo = CatalogosGenero.Where(x => x.codigo == RRHHCodigos.GENERO_VARON).FirstOrDefault();
                                        i.catalogo_genero_id = sexo.Id;
                                    }
                                    else if (consultaCandidatos.sexo == RRHHCodigos.SEXO_MUJER)
                                    {
                                        var sexo = CatalogosGenero.Where(x => x.codigo == RRHHCodigos.GENERO_MUJER).FirstOrDefault();
                                        i.catalogo_genero_id = sexo.Id;
                                    }

                                    //Valida Nacionalidad
                                    if (consultaCandidatos.nacionalidad == RRHHCodigos.REGISTRO_CIVIL_ECUATORIANA)
                                    {
                                        var nacionalidad = CatalogosNacionalidades.Where(x => x.codigo == RRHHCodigos.NACIONALIDAD_ECUATORIANA).FirstOrDefault();
                                        i.pais_pais_nacimiento_id = nacionalidad.Id;
                                    }
                                }
                                else if (i.validacion_cedula == true)
                                {
                                    var ws = _webService.BusquedaPorCedulaRegistroCivilHuellaDigital(i.numero_identificacion, i.codigo_dactilar);

                                    var objeto = _colaboradoresService.ChangeResultXMLObject(ws);
                                    if (objeto.Error != "000")
                                    {
                                        i.nombres_apellidos = objeto.Nombre;
                                        i.fecha_nacimiento = objeto.FechaNacimiento;//
                                        i.viene_registro_civil = true;
                                        i.fecha_registro_civil = DateTime.Now;

                                        if (objeto.Sexo == RRHHCodigos.SEXO_HOMBRE)
                                        {
                                            var sexo = CatalogosGenero.Where(x => x.codigo == RRHHCodigos.GENERO_VARON).FirstOrDefault();
                                            i.catalogo_genero_id = sexo.Id;
                                        }
                                        else if (objeto.Sexo == RRHHCodigos.SEXO_MUJER)
                                        {
                                            var sexo = CatalogosGenero.Where(x => x.codigo == RRHHCodigos.GENERO_MUJER).FirstOrDefault();
                                            i.catalogo_genero_id = sexo.Id;
                                        }

                                        if (objeto.Nacionalidad == RRHHCodigos.REGISTRO_CIVIL_ECUATORIANA)
                                        {
                                            var nacionalidad = CatalogosNacionalidades.Where(x => x.codigo == RRHHCodigos.NACIONALIDAD_ECUATORIANA).FirstOrDefault();
                                            i.pais_pais_nacimiento_id = nacionalidad.Id;
                                        }
                                    }
                                }
                            }

                            ContactoDto contacto = new ContactoDto();
                            contacto.ParroquiaId = i.parroquia;
                            var c = await _contactoService.InsertOrUpdateAsync(contacto);

                            i.ContactoId = c.Id;
                            i.es_carga_masiva = true;
                            i.fecha_carga_masiva = DateTime.Now;
                            i.estado = RRHHCodigos.ESTADO_TEMPORAL;

                            var legajo = _colaboradoresService.GetLegajo();
                            if (legajo == "NO")
                            {
                                i.numero_legajo_temporal = (00001).ToString();
                            }
                            else
                            {
                                var nro = int.Parse(legajo) + 1;
                                i.numero_legajo_temporal = (nro).ToString();
                            }

                            var insertado = await _colaboradoresService.InsertOrUpdateAsync(i);


                        }
                        string result = "OK";
                        return new JsonResult
                        {
                            Data = new { success = true, result },
                            JsonRequestBehavior = JsonRequestBehavior.AllowGet
                        };
                    }
                    else
                    {
                        return new JsonResult
                        {
                            Data = new { success = false, errores },
                            JsonRequestBehavior = JsonRequestBehavior.AllowGet
                        };
                    }


                }
                else
                {
                    List<string> errores = new List<string>();
                    errores.Add("El Formato del Archivo debe ser Excel");
                    return new JsonResult
                    {
                        Data = new { success = false, errores },
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet
                    };
                }
            }
            return Content("");
        }


        /// <summary>
        /// Campusoft: Servicios que deberia ser proporcionados por el CLiente
        /// </summary>
        /// <returns></returns>

        public async Task<ActionResult> GetColaboradoresLookupApi()
        {
            var entityDto = await _colaboradoresService.GetLookupAll();
            return WrapperResponseGetApi(ModelState, () => entityDto);
        }
        public ActionResult GetAnotadoresLookupApi()
        {
            var entityDto = _colaboradoresService.GetAnotadoresLookupAll();
            return WrapperResponseGetApi(ModelState, () => entityDto);
        }



        #region Api

        public ActionResult DetallesApi(int id)
        {
            var pagedResultDto = _colaboradoresService.Detalles(id);
            return WrapperResponseGetApi(ModelState, () => pagedResultDto);
        }

        #endregion

        //Reportes
        public ActionResult Reportes()
        {
            return View();
        }

        public async Task<ActionResult> GetReporteInformacionGeneralApi(ColaboradorReporteDto colaborador)
        {
            ExcelPackage excel = _colaboradoresService.reporteInformacionGeneral(colaborador);
            if (excel != null)
            {
                var usuario = "";
                var idUser = User.Identity.GetUserId();
                var usuarioencontrado = await _usuarioService.Get(new EntityDto<int>(int.Parse(idUser)));
                if (usuarioencontrado != null && usuarioencontrado.Id > 0)
                {
                    usuario = usuarioencontrado.Nombres + usuarioencontrado.Apellidos + "_";
                }

                string excelName = "ReporteColaboradores_" + usuario + DateTime.Now.ToString("dd_MM_yyyy_hh_mm");
                using (var memoryStream = new MemoryStream())
                {
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment; filename=" + excelName + ".xlsx");
                    excel.SaveAs(memoryStream);
                    memoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                    return Content("OK");
                }
            }
            else
            {
                return Content("ERROR");

            }


        }

        public async Task<ActionResult> GetReporteCargasFamiliaresApi(ColaboradorReporteDto colaborador)
        {
            ExcelPackage excel = _cargaSocialService.reporteInformacionGeneral(colaborador);
            if (excel != null)
            {
                var usuario = "";
                var idUser = User.Identity.GetUserId();
                var usuarioencontrado = await _usuarioService.Get(new EntityDto<int>(int.Parse(idUser)));
                if (usuarioencontrado != null && usuarioencontrado.Id > 0)
                {
                    usuario = usuarioencontrado.Nombres + usuarioencontrado.Apellidos + "_";
                }

                string excelName = "ReporteCargasFamiliares_" + usuario + DateTime.Now.ToString("dd_MM_yyyy_hh_mm");
                using (var memoryStream = new MemoryStream())
                {
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment; filename=" + excelName + ".xlsx");
                    excel.SaveAs(memoryStream);
                    memoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                    return Content("OK");
                }
            }
            else
            {

                return Content("ERROR");
            }
        }

        public async Task<ActionResult> CreateAltaColaboradorador(int id, int id_empleado, string meta4 = "")
        {
            var colaborador = await _colaboradoresService.Get(new EntityDto<int>(id));
            colaborador.empleado_id_sap = id_empleado;
            colaborador.estado = "ACTIVO";
            if (meta4 != null)
            {
                colaborador.meta4 = meta4;
            }

            var c = await _colaboradoresService.Update(colaborador);
            return Content("OK ");
        }

        public ActionResult CreateAltaMasiva()
        {
            ExcelPackage excel = _colaboradoresService.GenerarExcelAltaMasiva();
            if (excel != null)
            {
                string excelName = "FormatoAltaMasivaColaboradores_" + DateTime.Now.ToShortDateString();
                using (var memoryStream = new MemoryStream())
                {
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment; filename=" + excelName + ".xlsx");
                    excel.SaveAs(memoryStream);
                    memoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                    return Content("OK");
                }
            }
            return Content("");


        }

        public async Task<ActionResult> GetExcelAltaMasiva(HttpPostedFileBase UploadedFile)
        {
            if (UploadedFile != null)
            {
                var CatalogoTiposIdentificacion = _catalogoservice.ListarCatalogosporcodigo(RRHHCodigos.TIPO_IDENTIFICACION);
                // tdata.ExecuteCommand("truncate table OtherCompanyAssets");  
                if (UploadedFile.ContentType == "application/vnd.ms-excel" || UploadedFile.ContentType ==
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                {

                    List<string> errores = new List<string>();
                    List<ColaboradoresDto> colaboradores = new List<ColaboradoresDto>();

                    string fileName = UploadedFile.FileName;
                    string fileContentType = UploadedFile.ContentType;
                    byte[] fileBytes = new byte[UploadedFile.ContentLength];
                    var data = UploadedFile.InputStream.Read(fileBytes, 0,
                        Convert.ToInt32(UploadedFile.ContentLength));

                    using (var package = new ExcelPackage(UploadedFile.InputStream))
                    {
                        var currentSheet = package.Workbook.Worksheets;
                        var workSheet = currentSheet.First();
                        var noOfCol = workSheet.Dimension.End.Column;

                        if (noOfCol == 0)
                        {
                            errores.Add("El formato del archivo no es correcto");
                        }
                        else
                        {
                            var noOfRow = workSheet.Dimension.End.Row;

                            for (int rowIterator = 15; rowIterator <= noOfRow; rowIterator++)
                            {
                                ColaboradoresDto colaborador = new ColaboradoresDto();

                                if (workSheet.Cells[rowIterator, 2].Value == null)
                                {
                                    errores.Add("Obligatorio Tipo Identificación fila: " + rowIterator);
                                }
                                else
                                {
                                    var tipoIdentificacion = (workSheet.Cells[rowIterator, 2].Value).ToString();
                                    var existe = false;

                                    //  var catalogo= (from c in Ca  )


                                    foreach (var e in CatalogoTiposIdentificacion)
                                    {
                                        if (e.nombre == tipoIdentificacion)
                                        {
                                            existe = true;
                                            colaborador.catalogo_tipo_identificacion_id = e.Id;
                                            if (tipoIdentificacion == RRHHCodigos.TIPO_IDENTIFICACION_CEDULA)
                                            {
                                                colaborador.validacion_cedula = true;
                                            }
                                            break;
                                        }
                                    }
                                    if (existe == false)
                                    {
                                        errores.Add("Error Tipo Identificación fila: " + rowIterator + " no existe");
                                    }
                                }
                                //Validar Número de Identificación
                                if (workSheet.Cells[rowIterator, 3].Value == null)
                                {
                                    errores.Add("Obligatorio Número Identificación fila: " + rowIterator);
                                }
                                else
                                {
                                    var numeroIdentificacion = (workSheet.Cells[rowIterator, 3].Value).ToString();
                                    colaborador.numero_identificacion = numeroIdentificacion;

                                }

                                //Validar ID SAP LOCAL
                                if (workSheet.Cells[rowIterator, 9].Value == null)
                                {
                                    errores.Add("Obligatorio ID SAP Local fila: " + rowIterator);
                                }
                                else {
                                    var id_sap_local = (workSheet.Cells[rowIterator, 9].Value).ToString();
                                    colaborador.empleado_id_sap_local = int.Parse(id_sap_local);
                                }
                                //Validar ID SAP
                                if (workSheet.Cells[rowIterator, 7].Value == null)
                                {
                                    errores.Add("Obligatorio ID SAP fila: " + rowIterator);
                                }
                                else
                                {
                                    var id_sap = (workSheet.Cells[rowIterator, 7].Value).ToString();
                                    colaborador.empleado_id_sap = int.Parse(id_sap);
                                }
                                //Validar META 4
                                if (workSheet.Cells[rowIterator, 8].Value != null)
                                {
                                    var meta4 = (workSheet.Cells[rowIterator, 8].Value).ToString();
                                    colaborador.meta4 = meta4;
                                }

                                colaboradores.Add(colaborador);


                            }
                        }


                    }

                    if ((errores != null) && (!errores.Any()))
                    {
                        // Add new item
                        System.Diagnostics.Debug.WriteLine("Vacio !!!!!");

                        foreach (var i in colaboradores)
                        {
                            //GetColaboradorPorTipoIdentificacionExcluidoExterno ES
                            Colaboradores query = _colaboradoresService.GetColaboradorPorTipoIdentificacionExcluidoExterno(i.catalogo_tipo_identificacion_id, i.numero_identificacion);
                            var colaborador = Mapper.Map<Colaboradores, ColaboradoresDto>(query);
                            if (colaborador != null)
                            {
                                colaborador.empleado_id_sap = i.empleado_id_sap;
                                colaborador.meta4 = i.meta4;
                                colaborador.empleado_id_sap_local = i.empleado_id_sap_local;
                                colaborador.estado = RRHHCodigos.ESTADO_ACTIVO;

                                await _colaboradoresService.Update(colaborador);
                            }
                        }

                        errores.Add("OK");
                        return Content(JsonConvert.SerializeObject(errores));
                    }
                    else
                    {
                        return Content(JsonConvert.SerializeObject(errores));
                    }

                }
                else
                {
                    return Content("Formato de archivo incorrecto !");
                }
            }
            return Content("");
        }

        public ActionResult GetIdentificacionEsUnica(string numero)
        {
            var result = _colaboradoresService.UniqueIdentification(numero);
            return Content(result);
        }

        public ActionResult GetFiltrosColaboradoresTableApi(string numeroIdentificacion, string nombres, string estado)
        {
            var lista = _colaboradoresService.GetFiltrosColaboradoresTable(numeroIdentificacion, nombres, estado);
            var result = JsonConvert.SerializeObject(lista);
            return Content(result);
        }

        public ActionResult GetEmpresasApi()
        {
            var lista = _empresaService.GetEmpresas();
            var result = JsonConvert.SerializeObject(lista);
            return Content(result);
        }

        public ActionResult GetValidarFechaPeriodoRegistrosAnteriores(int Id, DateTime fecha) {
            var existePeriodo = _colaboradoresService.ValidarPeriodoColaborador(Id, fecha);
            return Content(existePeriodo ? "INCLUIDO_EN_PERIODO_ANTERIOR" : "OK");
        }
        public ActionResult GetValidarFechaPeriodoRegistrosAnterioresReingreso(int Id, DateTime fecha)
        {
            var existePeriodo = _colaboradoresService.ValidarPeriodoColaboradorReingreso(Id, fecha);
            return Content(existePeriodo ? "INCLUIDO_EN_PERIODO_ANTERIOR" : "OK");
        }



        #region ES: MAPEO CORRECTO
        public ActionResult FirstList() //lISTA DE COLABORADORES
        {
            var list = _colaboradoresService.GetList();
            var result = JsonConvert.SerializeObject(list);
            return Content(result);
        }
        public ActionResult FilterSearch(string numeroIdentificacion = "", string nombres = "", string estado = "") // FILTROS COLABORADORES
        {
            var lista = _colaboradoresService.FiltrosColaboradoresEstado(numeroIdentificacion, nombres, estado);
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

            var result = JsonConvert.SerializeObject(catalogos,
              Newtonsoft.Json.Formatting.None,

              new JsonSerializerSettings
              {
                  ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                  NullValueHandling = NullValueHandling.Ignore,


              });

            return Content(result);
        }


        [System.Web.Mvc.HttpPost]
        public ActionResult ObtainFullInfo()//ObtainFullInfo GetColaboradoresInfoCompletaApi
        {
            var list = _colaboradoresService.GetColaboradoresInfoCompleta();

            var result = JsonConvert.SerializeObject(list);
            return Content(result);
        }



        #endregion


        public JsonResult GetConsulta(string identificacion)
        {
            var dto = _consultaPublicaService.ExisteCandidato(identificacion);

            if (dto.Id > 0 && dto.fecha_consulta.HasValue)
            {
                return new JsonResult
                {
                    Data = new { success = true, result = dto }
                };
            }
            return new JsonResult
            {
                Data = new { success = false }
            };
        }

        /// <summary>
        /// Exclusive to use in M'odules that need compare the name of exist catalogos in real time /dropdown
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult GetNamebyId(int id)
        {
            var name = _catalogoservice.GetNamebyId(id);
            return Content(name);

        }
        public ActionResult GetCodePostalByCiudad(int id)
        {
            var codigopostal = _ciudadService.ObtenerCodigoPostal(id);
            return Content(codigopostal);

        }


        [System.Web.Mvc.HttpPost]
        public ActionResult GetDataQr(int Id)
        {
            //Dictionary<String, Object> result = _colaboradoresService.GenerarQr(Id);
            string result = _colaboradoresService.GenerarQrCodigoSeguridad(Id);
            return new JsonResult
            {
                Data = new { success = true, result },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        [System.Web.Mvc.HttpPost]
        public ActionResult GetDataQrE(int Id)
        {
            //Dictionary<String, Object> result = _colaboradoresService.GenerarQr(Id);
            string result = _colaboradoresService.GenerarQrCodigoSeguridad(Id);
            return Content(result);

        }

        [System.Web.Mvc.HttpPost]
        public ActionResult GetDataQrExternos(int Id)
        {
            string result = _colaboradoresService.GenerarQrExternos(Id);
            return Content(result);

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
        [System.Web.Mvc.HttpPost]
        public ActionResult GetExternoActivo(string Id)
        {
            var externoId = _colaboradoresService.SearchColaboradorExterno(Id);
            return Content(externoId > 0 ? externoId.ToString() : "NO_EXTERNO");
        }
        [System.Web.Mvc.HttpPost]
        public ActionResult GetIdUnico(string numero)
        {
            var result = _colaboradoresService.BuscarIdUnicoColaboradores(numero);
            return Content(result);
        }

        public ActionResult GetSimpleColaborador(int id)
        {
            var data = _colaboradoresService.SimpleDataColaborador(id);
            var result = JsonConvert.SerializeObject(data);
            return Content(result);

        }
        [System.Web.Mvc.HttpPost]
        public ActionResult GetInsertServices(ServiceModel c)
        {
            var result = _colaboradoresService.SimpleInsertServiceColaborador(c);

            return Content(result ? "OK" : "ERROR");
        }

        /*Validar si existe Colaborador Principal al crear un nuevo Colaborador Externo*/
        public ActionResult GetColaboradorExistente(string numero_identificacion)
        {
            var result = _colaboradoresService.existeColaboradorPrincipal(numero_identificacion, false);
            return Content(JsonConvert.SerializeObject(result));

        }
        public ActionResult GetColaboradoresReingres()
        {
            var excel = _colaboradoresService.ReporteColaboradoresHistoricos();
            string excelName = "ReingresoColaboradores";
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


        [System.Web.Mvc.HttpPost]
        public async Task<JsonResult> EnableDisableApi(int id,string pass)
        {

            try
            {
                if (ModelState.IsValid)
                {
               


                   var result = await _colaboradoresService.Desactivar(id,pass);


                    return new JsonResult
                    {
                        Data = new { success = true, result }
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


        [System.Web.Mvc.HttpPost]
        public JsonResult ValidarPassApi(string pass)
        {

            try
            {
                if (ModelState.IsValid)
                {



                    var result =  _colaboradoresService.ValidarPassFechaIngreso( pass);


                    return new JsonResult
                    {
                        Data = new { success = true, result }
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

    }

}