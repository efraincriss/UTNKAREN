using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.aplicacion.Dto;
using com.cpp.calypso.proyecto.aplicacion.Interfaces;
using com.cpp.calypso.proyecto.dominio;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xceed.Words.NET;
using System.Web;
using AutoMapper;
using com.cpp.calypso.seguridad.aplicacion;
using com.cpp.calypso.proyecto.aplicacion.WebService;
using System.Globalization;
using com.cpp.calypso.proyecto.dominio.Constantes;
using Microsoft.AspNet.Identity;
using com.cpp.calypso.proyecto.dominio.Proveedor;
using com.cpp.calypso.proyecto.aplicacion.Proveedor.Dto;
using Newtonsoft.Json;
using com.cpp.calypso.proyecto.dominio.Acceso;
using System.Xml.Serialization;
using Newtonsoft.Json.Linq;
using System.Xml;
using System.Net.Mail;
using com.cpp.calypso.framework;
using com.cpp.calypso.proyecto.dominio.RecursosHumanos.Models;
using com.cpp.calypso.proyecto.dominio.RecursosHumanos;
using System.Data.Entity.Validation;
using com.cpp.calypso.proyecto.dominio.Models;
using System.Text;
using com.cpp.calypso.proyecto.dominio.CertificacionIngenieria;

namespace com.cpp.calypso.proyecto.aplicacion.Service
{
    public class ColaboradoresAsyncBaseCrudAppService : AsyncBaseCrudAppService<Colaboradores, ColaboradoresDto, PagedAndFilteredResultRequestDto>, IColaboradoresAsyncBaseCrudAppService
    {
        private readonly ICatalogoAsyncBaseCrudAppService _catalogoRepository;
        private readonly IColaboradorFormacionEducativaAsyncBaseCrudAppService _formacionEducativaService;
        private readonly IColaboradorDiscapacidadAsyncBaseCrudAppService _discapacidadService;
        private readonly IColaboradorBajaAsyncBaseCrudAppService _bajasService;
        private readonly IContactoAsyncBaseCrudAppService _contactoRepository;
        private readonly IdentityEmailMessageService EmailService;
        private readonly IBaseRepository<ParametroSistema> _parametroRepository;
        private readonly IParroquiaAsyncBaseCrudAppService _parroquiaService;
        private readonly IProvinciaAsyncBaseCrudAppService _provinciaService;
        private readonly IColaboradorResponsabilidadAsyncBaseCrudAppService _responsabilidadService;
        private readonly IdentityEmailMessageService _correoservice;
        private readonly IBaseRepository<Colaboradores> _colaboradoresRepository;
        private readonly IBaseRepository<ColaboradorBaja> _colaboradoresBajaRepository;
        private readonly IBaseRepository<ParametroSistema> _parametrorepository;
        private readonly IBaseRepository<Catalogo> _ccatalogorepository;
        private readonly IBaseRepository<TipoCatalogo> _tipoCatalogoService;
        private readonly IBaseRepository<ColaboradorCargaSocial> _cargaSocialRepository;
        private readonly IBaseRepository<Archivo> _archivoRepository;
        private readonly IBaseRepository<Pais> _paisrepository;
        private readonly IBaseRepository<Comunidad> _comunidadrepository;
        private readonly IBaseRepository<Rol> _rolrepository;
        private readonly IBaseRepository<ColaboradoresComida> _colcomidarepository;
        private readonly IBaseRepository<ColaboradorMovilizacion> _colmovilizacionrepository;
        private readonly IBaseRepository<CorreoLista> _correslistarepository;
        private readonly IBaseRepository<Catalogo> _catarepository;


        //Web service -QR
        private readonly IWebServiceAsyncBaseCrudAppService _webService;
        private readonly IBaseRepository<Usuario> _usuarioRepository;
        private readonly IBaseRepository<ColaboradorServicio> _colaboradorServicioRepository;
        private readonly IBaseRepository<ContactoEmergencia> _contactoEmergencia;
        private readonly IBaseRepository<ReservaHotel> _reservahotel;
        private readonly IBaseRepository<DetalleReserva> _detallereservahotel;
        private readonly IBaseRepository<ColaboradorDiscapacidad> _colaboradorDiscapacidadRepository;
        private readonly IBaseRepository<TarjetaAcceso> _tarjetaRepository;
        private readonly IBaseRepository<ColaboradoresHuellaDigital> _ColaboradorHuellaRepository;
        public IBaseRepository<ColaboradoresAusentismo> _colaboradorausentismo;

        private readonly IBaseRepository<Capacitacion> _capacitacionesRepository;
        private readonly IBaseRepository<Contacto> _contactoColaboradoresRepository;


        //REINGRESO CERTIFICACION INGENIERIA

        private readonly IBaseRepository<ColaboradorRubroIngenieria> _colaboradorRubroIngenieriarepository;
        private readonly IBaseRepository<ColaboradorCertificacionIngenieria> _colaboradorCertificacionIngenieriarepository;

        public ColaboradoresAsyncBaseCrudAppService(
            IBaseRepository<Colaboradores> repository,
            ICatalogoAsyncBaseCrudAppService catalogoRepository,
            IColaboradorFormacionEducativaAsyncBaseCrudAppService formacionEducativaService,
            IColaboradorDiscapacidadAsyncBaseCrudAppService discapacidadService,
            IColaboradorBajaAsyncBaseCrudAppService bajasService,
            IContactoAsyncBaseCrudAppService contactoRepository,
            IdentityEmailMessageService emailService,
            IBaseRepository<ParametroSistema> parametroRepository,
            IParroquiaAsyncBaseCrudAppService parroquiaService,
            IProvinciaAsyncBaseCrudAppService provinciaService,
            IBaseRepository<TipoCatalogo> tipoCatalogoService,
            IColaboradorResponsabilidadAsyncBaseCrudAppService responsabilidadService,
            IBaseRepository<ColaboradoresComida> colcomidarepository,
            IBaseRepository<ColaboradorMovilizacion> colmovilizacionrepository,

        IBaseRepository<ParametroSistema> parametrorepository,
           IBaseRepository<Catalogo> ccatalogorepository,
           IBaseRepository<ColaboradorCargaSocial> cargaSocialRepository,
           IBaseRepository<Archivo> archivoRepository,
           IBaseRepository<Pais> paisrepository,
           IBaseRepository<Comunidad> comunidadrepository,
           IBaseRepository<Rol> rolrepository,
            IBaseRepository<ColaboradorBaja> colaboradoresBajaRepository,
            IBaseRepository<ContactoEmergencia> contactoEmergencia,
        IWebServiceAsyncBaseCrudAppService webService,
           IBaseRepository<Usuario> usuarioRepository,
           IBaseRepository<ColaboradorServicio> colaboradorServicioRepository,
           IBaseRepository<ReservaHotel> reservahotel,
           IBaseRepository<DetalleReserva> detallereservahotel,
           IBaseRepository<ColaboradorDiscapacidad> colaboradorDiscapacidadRepository,
           IBaseRepository<TarjetaAcceso> tarjetaRepository,
             IdentityEmailMessageService correoservice,
             IBaseRepository<CorreoLista> correslistarepository,
             IBaseRepository<ColaboradoresHuellaDigital> ColaboradorHuellaRepository,
             IBaseRepository<ColaboradoresAusentismo> colaboradorausentismo,
             IBaseRepository<Catalogo> catarepository,
            IBaseRepository<Colaboradores> colaboradoresRepository,
             IBaseRepository<Capacitacion> capacitacionesRepository,
             IBaseRepository<Contacto> contactoColaboradoresRepository,




       IBaseRepository<ColaboradorRubroIngenieria> colaboradorRubroIngenieriarepository,
       IBaseRepository<ColaboradorCertificacionIngenieria> colaboradorCertificacionIngenieriarepository

            ) : base(repository)
        {
            _colaboradorRubroIngenieriarepository = colaboradorRubroIngenieriarepository;
            _colaboradorCertificacionIngenieriarepository = colaboradorCertificacionIngenieriarepository;

            _catalogoRepository = catalogoRepository;
            _formacionEducativaService = formacionEducativaService;
            _discapacidadService = discapacidadService;
            _bajasService = bajasService;
            _contactoRepository = contactoRepository;
            EmailService = emailService;
            _parametroRepository = parametroRepository;
            _parroquiaService = parroquiaService;
            _provinciaService = provinciaService;
            _parametrorepository = parametrorepository;
            _ccatalogorepository = ccatalogorepository;
            _tipoCatalogoService = tipoCatalogoService;
            _cargaSocialRepository = cargaSocialRepository;
            _colaboradoresBajaRepository = colaboradoresBajaRepository;
            _archivoRepository = archivoRepository;
            _responsabilidadService = responsabilidadService;
            _paisrepository = paisrepository;
            _comunidadrepository = comunidadrepository;
            _webService = webService;
            _usuarioRepository = usuarioRepository;
            _colaboradorServicioRepository = colaboradorServicioRepository;
            _reservahotel = reservahotel;
            _detallereservahotel = detallereservahotel;
            _colaboradorDiscapacidadRepository = colaboradorDiscapacidadRepository;
            _tarjetaRepository = tarjetaRepository;
            _correoservice = correoservice;
            _correslistarepository = correslistarepository;
            _ColaboradorHuellaRepository = ColaboradorHuellaRepository;
            _rolrepository = rolrepository;
            _colaboradorausentismo = colaboradorausentismo;
            _colcomidarepository = colcomidarepository;
            _colmovilizacionrepository = colmovilizacionrepository;
            _catarepository = catarepository;
            _colaboradoresRepository = colaboradoresRepository;
            _contactoEmergencia = contactoEmergencia;
            _capacitacionesRepository = capacitacionesRepository;
            _contactoColaboradoresRepository = contactoColaboradoresRepository;
        }


        public List<ColaboradoresDto> GetList()
        {
            var e = 1;
            var query = Repository.GetAll().Where(c => c.vigente == true && c.estado == RRHHCodigos.ESTADO_TEMPORAL && c.es_externo == false);

            var colaborador = (from d in query
                               select new ColaboradoresDto
                               {
                                   Id = d.Id,
                                   fecha_ingreso = d.fecha_ingreso,
                                   catalogo_tipo_identificacion_id = d.catalogo_tipo_identificacion_id,
                                   numero_identificacion = d.numero_identificacion,
                                   primer_apellido = d.primer_apellido,
                                   segundo_apellido = d.segundo_apellido,
                                   nombres = d.nombres,
                                   fecha_nacimiento = d.fecha_nacimiento,
                                   catalogo_genero_id = d.catalogo_genero_id,
                                   PaisId = d.PaisId,
                                   pais_pais_nacimiento_id = d.pais_pais_nacimiento_id,
                                   ContactoId = d.ContactoId,
                                   catalogo_destino_estancia_id = d.catalogo_destino_estancia_id,
                                   vigente = d.vigente,
                                   validacion_cedula = d.validacion_cedula,
                                   numero_legajo_temporal = d.numero_legajo_temporal,
                                   catalogo_grupo_personal_id = d.catalogo_grupo_personal_id,
                                   estado = d.estado,
                                   codigo_dactilar = d.codigo_dactilar,
                                   nombre_identificacion = d.TipoIdentificacion.nombre,
                                   nombre_destino = d.DestinoEstancia.nombre,
                                   fechavigenciacolaboradorqr = "",
                                   serviciosvigentes = "",
                                   tienereservaactiva = "",
                                   nombres_apellidos = d.nombres_apellidos,
                                   nombre_grupo_personal = d.GrupoPersonal != null ? d.GrupoPersonal.nombre : "",
                                   nombreestancia = "",
                               }).ToList();

            foreach (var i in colaborador)
            {
                i.nro = e++;

                i.apellidos_nombres = i.primer_apellido + ' ' + i.segundo_apellido;

                if (i.nombre_grupo_personal == "inicial")
                {
                    i.nombre_grupo_personal = "";
                }
                //Catalogo Destino
                if (i.catalogo_destino_estancia_id > 0)
                {
                    i.nombreestancia = _catalogoRepository.GetCatalogo(i.catalogo_destino_estancia_id.Value) != null ? _catalogoRepository.GetCatalogo(i.catalogo_destino_estancia_id.Value).nombre : "";
                }
                i.fechavigenciacolaboradorqr = this.getParametroPorCodigo("PARAMETRO.FECHA.CADUCIDAD.QR").Length > 0 ?

                    DateTime.Now.AddDays(Int32.Parse(this.getParametroPorCodigo("PARAMETRO.FECHA.CADUCIDAD.QR"))).ToString("dd/MM/yyyy HH:mm") : "";

                i.serviciosvigentes = this.ServiciosColaborado(i.Id);
                i.tienereservaactiva = this.colaboradortienereservasactivas(i.Id);
                i.numeroHuellas = this.NumeroHuellas(i.Id);
            }

            return colaborador;
        }

        public List<ColaboradoresDto> GetColaboradoresInfoCompleta()
        {
            var e = 1;
            var query = Repository.GetAllIncluding(d => d.Pais).Where(d => d.vigente == true && d.estado == "INFORMACION COMPLETA");

            var colaborador = (from d in query
                               where d.vigente == true
                               select new ColaboradoresDto
                               {
                                   Id = d.Id,
                                   fecha_ingreso = d.fecha_ingreso,
                                   candidato_id_sap = d.candidato_id_sap,
                                   posicion = d.posicion,
                                   catalogo_division_personal_id = d.catalogo_division_personal_id,
                                   catalogo_subdivision_personal_id = d.catalogo_subdivision_personal_id,
                                   catalogo_encargado_personal_id = d.catalogo_encargado_personal_id,
                                   catalogo_vinculo_laboral_id = d.catalogo_vinculo_laboral_id,
                                   catalogo_encuadre_id = d.catalogo_encuadre_id,
                                   catalogo_tipo_identificacion_id = d.catalogo_tipo_identificacion_id,
                                   numero_identificacion = d.numero_identificacion,
                                   PaisId = d.PaisId,
                                   fecha_nacimiento = d.fecha_nacimiento,
                                   catalogo_estado_civil_id = d.catalogo_estado_civil_id,
                                   fecha_matrimonio = d.fecha_matrimonio,
                                   pais_pais_nacimiento_id = d.pais_pais_nacimiento_id,
                                   numero_hijos = d.numero_hijos,
                                   catalogo_genero_id = d.catalogo_genero_id,
                                   catalogo_clase_contrato_id = d.catalogo_clase_contrato_id,
                                   catalogo_plan_beneficios_id = d.catalogo_plan_beneficios_id,
                                   catalogo_plan_salud_id = d.catalogo_plan_salud_id,
                                   catalogo_cobertura_dependiente_id = d.catalogo_cobertura_dependiente_id,
                                   catalogo_planes_beneficios_id = d.catalogo_planes_beneficios_id,
                                   catalogo_banco_id = d.catalogo_banco_id,
                                   numero_cuenta = d.numero_cuenta,
                                   catalogo_formacion_educativa_id = d.catalogo_formacion_educativa_id,
                                   catalogo_clase_id = d.catalogo_clase_id,
                                   catalogo_area_id = d.catalogo_area_id,
                                   catalogo_grupo_id = d.catalogo_grupo_id,
                                   catalogo_subgrupo_id = d.catalogo_subgrupo_id,
                                   remuneracion_mensual = d.remuneracion_mensual,
                                   catalogo_asociacion_id = d.catalogo_asociacion_id,
                                   catalogo_funcion_id = d.catalogo_funcion_id,
                                   catalogo_codigo_siniestro_id = d.catalogo_codigo_siniestro_id,
                                   catalogo_periodo_nomina_id = d.catalogo_periodo_nomina_id,
                                   catalogo_apto_medico_id = d.catalogo_apto_medico_id,
                                   validacion_cedula = d.validacion_cedula,
                                   primer_apellido = d.primer_apellido,
                                   segundo_apellido = d.segundo_apellido,
                                   nombres = d.nombres,
                                   catalogo_codigo_incapacidad_id = d.catalogo_codigo_incapacidad_id,
                                   catalogo_via_pago_id = d.catalogo_via_pago_id,
                                   Pais = d.Pais,
                                   estado = d.estado,
                                   ContactoId = d.ContactoId,
                                   codigo_dactilar = d.codigo_dactilar,
                                   nombre_identificacion = d.TipoIdentificacion.nombre,
                                   fechavigenciacolaboradorqr = "",
                                   serviciosvigentes = "",
                                   tienereservaactiva = "",
                                   nombreestancia = "",
                                   nombres_apellidos = d.nombres_apellidos,
                                   apellidos_nombres = d.primer_apellido + " " + d.segundo_apellido + " " + d.nombres
                               }).ToList();

            foreach (var i in colaborador)
            {
                i.nro = e++;

                i.apellidos_nombres = i.primer_apellido + ' ' + i.segundo_apellido;

                var formacion = _formacionEducativaService.GetFormacion(i.Id);
                if (formacion != null)
                {
                    i.formacion = formacion.formacion == "" ? 0 : int.Parse(formacion.formacion);
                    i.institucion_educativa = formacion.institucion_educativa;
                    i.catalogo_titulo_id = formacion.catalogo_titulo_id == null ? 0 : formacion.catalogo_titulo_id.Value;
                }


                //Consulta contacto de colaborador
                if (i.ContactoId.HasValue)
                {
                    var contacto = _contactoRepository.GetContacto(i.ContactoId.Value);
                    i.calle = contacto.calle_principal;
                    i.numero = contacto.numero;
                    i.telefono = contacto.celular;
                    i.email = contacto.correo_electronico;
                    i.codigo_postal = contacto.codigo_postal;

                    if (contacto.ParroquiaId > 0)
                    {
                        var pa = _parroquiaService.GetParroquia(contacto.ParroquiaId.Value);
                        var prov = _provinciaService.GetProvincia(pa.Ciudad.ProvinciaId);
                        i.region = prov.nombre;
                        i.subregion = pa.Ciudad.nombre;
                    }
                }
                //Catalogo Destino
                if (i.catalogo_destino_estancia_id > 0)
                {
                    i.nombreestancia = _catalogoRepository.GetCatalogo(i.catalogo_destino_estancia_id.Value) != null ? _catalogoRepository.GetCatalogo(i.catalogo_destino_estancia_id.Value).nombre : "";
                }
                i.fechavigenciacolaboradorqr = this.getParametroPorCodigo("PARAMETRO.FECHA.CADUCIDAD.QR").Length > 0 ?

                    DateTime.Now.AddDays(Int32.Parse(this.getParametroPorCodigo("PARAMETRO.FECHA.CADUCIDAD.QR"))).ToString("dd/MM/yyyy HH:mm") : "";

                i.serviciosvigentes = this.ServiciosColaborado(i.Id);
                i.tienereservaactiva = this.colaboradortienereservasactivas(i.Id);
                i.cargas = _cargaSocialRepository.GetAllIncluding(c => c.Pais).Where(c => c.ColaboradoresId == i.Id && c.vigente == true).ToList();
            }

            return colaborador;
        }

        public ColaboradoresDto GetColaboradorInfoCompleta(int Id)
        {
            var e = 1;
            var d = Repository.Get(Id);

            ColaboradoresDto i = new ColaboradoresDto()
            {
                Id = d.Id,
                fecha_ingreso = d.fecha_ingreso,
                candidato_id_sap = d.candidato_id_sap,
                posicion = d.posicion,
                catalogo_division_personal_id = d.catalogo_division_personal_id,
                catalogo_subdivision_personal_id = d.catalogo_subdivision_personal_id,
                catalogo_encargado_personal_id = d.catalogo_encargado_personal_id,
                catalogo_vinculo_laboral_id = d.catalogo_vinculo_laboral_id,
                catalogo_encuadre_id = d.catalogo_encuadre_id,
                catalogo_tipo_identificacion_id = d.catalogo_tipo_identificacion_id,
                numero_identificacion = d.numero_identificacion,
                PaisId = d.PaisId,
                fecha_nacimiento = d.fecha_nacimiento,
                catalogo_estado_civil_id = d.catalogo_estado_civil_id,
                fecha_matrimonio = d.fecha_matrimonio,
                pais_pais_nacimiento_id = d.pais_pais_nacimiento_id,
                numero_hijos = d.numero_hijos,
                catalogo_genero_id = d.catalogo_genero_id,
                catalogo_clase_contrato_id = d.catalogo_clase_contrato_id,
                catalogo_plan_beneficios_id = d.catalogo_plan_beneficios_id,
                catalogo_plan_salud_id = d.catalogo_plan_salud_id,
                catalogo_cobertura_dependiente_id = d.catalogo_cobertura_dependiente_id,
                catalogo_planes_beneficios_id = d.catalogo_planes_beneficios_id,
                catalogo_banco_id = d.catalogo_banco_id,
                numero_cuenta = d.numero_cuenta,
                catalogo_formacion_educativa_id = d.catalogo_formacion_educativa_id,
                catalogo_clase_id = d.catalogo_clase_id,
                catalogo_area_id = d.catalogo_area_id,
                catalogo_grupo_id = d.catalogo_grupo_id,
                catalogo_subgrupo_id = d.catalogo_subgrupo_id,
                remuneracion_mensual = d.remuneracion_mensual,
                catalogo_asociacion_id = d.catalogo_asociacion_id,
                catalogo_funcion_id = d.catalogo_funcion_id,
                catalogo_codigo_siniestro_id = d.catalogo_codigo_siniestro_id,
                catalogo_periodo_nomina_id = d.catalogo_periodo_nomina_id,
                catalogo_apto_medico_id = d.catalogo_apto_medico_id,
                primer_apellido = d.primer_apellido,
                segundo_apellido = d.segundo_apellido,
                nombres = d.nombres,
                catalogo_codigo_incapacidad_id = d.catalogo_codigo_incapacidad_id,
                catalogo_via_pago_id = d.catalogo_via_pago_id,
                Pais = d.Pais,
                estado = d.estado,
                ContactoId = d.ContactoId,
                codigo_dactilar = d.codigo_dactilar,
                nombre_identificacion = d.TipoIdentificacion.nombre,
                nombres_apellidos = d.nombres_apellidos,
                empleado_id_sap = d.empleado_id_sap,
                validacion_cedula = d.validacion_cedula,

            };

            i.nro = e++;

            i.apellidos_nombres = i.primer_apellido + ' ' + i.segundo_apellido;

            var formacion = _formacionEducativaService.GetFormacion(i.Id);
            if (formacion != null)
            {
                i.formacion = formacion.formacion == "" ? 0 : int.Parse(formacion.formacion);
                i.institucion_educativa = formacion.institucion_educativa;
                i.catalogo_titulo_id = formacion.catalogo_titulo_id == null ? 0 : formacion.catalogo_titulo_id.Value;
            }


            //Consulta contacto de colaborador
            if (i.ContactoId.HasValue)
            {
                var contacto = _contactoRepository.GetContacto(i.ContactoId.Value);
                i.calle = contacto.calle_principal;
                i.numero = contacto.numero;
                i.telefono = contacto.celular;
                i.email = contacto.correo_electronico;
                i.codigo_postal = contacto.codigo_postal;

                if (contacto.ParroquiaId > 0)
                {
                    var pa = _parroquiaService.GetParroquia(contacto.ParroquiaId.Value);
                    var prov = _provinciaService.GetProvincia(pa.Ciudad.ProvinciaId);
                    i.region = prov.nombre;
                    i.subregion = pa.nombre; // pa.Ciudad.nombre 
                }
            }


            //COnsulta Cargas Sociales
            i.cargas = _cargaSocialRepository.GetAllIncluding(c => c.Pais).Where(c => c.ColaboradoresId == i.Id && c.vigente == true).ToList();

            return i;
        }

        public ColaboradoresDto GetColaborador(int Id)
        {
            var d = Repository.Get(Id);

            if (d != null)
            {
                ColaboradoresDto colaborador = new ColaboradoresDto()
                {
                    Id = d.Id,
                    fecha_ingreso = d.fecha_ingreso,
                    catalogo_tipo_identificacion_id = d.catalogo_tipo_identificacion_id,
                    numero_identificacion = d.numero_identificacion,
                    primer_apellido = d.primer_apellido,
                    segundo_apellido = d.segundo_apellido,
                    nombres = d.nombres,
                    fecha_nacimiento = d.fecha_nacimiento,
                    catalogo_genero_id = d.catalogo_genero_id,
                    PaisId = d.PaisId,
                    pais_pais_nacimiento_id = d.pais_pais_nacimiento_id,
                    catalogo_etnia_id = d.catalogo_etnia_id,
                    catalogo_estado_civil_id = d.catalogo_estado_civil_id,
                    fecha_matrimonio = d.fecha_matrimonio,
                    catalogo_codigo_siniestro_id = d.catalogo_codigo_siniestro_id,
                    vigente = d.vigente,
                    candidato_id_sap = d.candidato_id_sap,
                    meta4 = d.meta4,
                    catalogo_grupo_personal_id = d.catalogo_grupo_personal_id,
                    catalogo_destino_estancia_id = d.catalogo_destino_estancia_id,
                    catalogo_sitio_trabajo_id = d.catalogo_sitio_trabajo_id,
                    AdminRotacion = d.AdminRotacion,
                    AdminRotacionId = d.AdminRotacionId,
                    catalogo_area_id = d.catalogo_area_id,
                    catalogo_cargo_id = d.catalogo_cargo_id,
                    catalogo_vinculo_laboral_id = d.catalogo_vinculo_laboral_id,
                    catalogo_clase_id = d.catalogo_clase_id,
                    catalogo_encuadre_id = d.catalogo_encuadre_id,
                    catalogo_plan_beneficios_id = d.catalogo_plan_beneficios_id,
                    catalogo_plan_salud_id = d.catalogo_plan_salud_id,
                    catalogo_cobertura_dependiente_id = d.catalogo_cobertura_dependiente_id,
                    catalogo_planes_beneficios_id = d.catalogo_planes_beneficios_id,
                    catalogo_asociacion_id = d.catalogo_asociacion_id,
                    catalogo_apto_medico_id = d.catalogo_apto_medico_id,
                    empresa_id = d.empresa_id,
                    posicion = d.posicion,
                    catalogo_division_personal_id = d.catalogo_division_personal_id,
                    catalogo_subdivision_personal_id = d.catalogo_subdivision_personal_id,
                    catalogo_funcion_id = d.catalogo_funcion_id,
                    catalogo_tipo_contrato_id = d.catalogo_tipo_contrato_id,
                    catalogo_clase_contrato_id = d.catalogo_clase_contrato_id,
                    fecha_caducidad_contrato = d.fecha_caducidad_contrato,
                    ejecutor_obra = d.ejecutor_obra,
                    catalogo_tipo_nomina_id = d.catalogo_tipo_nomina_id,
                    catalogo_periodo_nomina_id = d.catalogo_periodo_nomina_id,
                    ContratoId = d.ContratoId,
                    catalogo_forma_pago_id = d.catalogo_forma_pago_id,
                    catalogo_grupo_id = d.catalogo_grupo_id,
                    catalogo_subgrupo_id = d.catalogo_subgrupo_id,
                    remuneracion_mensual = d.remuneracion_mensual,
                    catalogo_banco_id = d.catalogo_banco_id,
                    catalogo_tipo_cuenta_id = d.catalogo_tipo_cuenta_id,
                    numero_cuenta = d.numero_cuenta,
                    numero_legajo_temporal = d.numero_legajo_temporal,
                    numero_legajo_definitivo = d.numero_legajo_definitivo,
                    ContactoId = d.ContactoId,
                    numero_hijos = d.numero_hijos,
                    empleado_id_sap = d.empleado_id_sap,
                    catalogo_encargado_personal_id = d.catalogo_encargado_personal_id,
                    catalogo_formacion_educativa_id = d.catalogo_formacion_educativa_id,
                    estado = d.estado,
                    validacion_cedula = d.validacion_cedula,
                    nombres_apellidos = d.nombres_apellidos,
                    catalogo_codigo_incapacidad_id = d.catalogo_codigo_incapacidad_id,
                    catalogo_sector_id = d.catalogo_sector_id,
                    catalogo_via_pago_id = d.catalogo_via_pago_id,
                    codigo_dactilar = d.codigo_dactilar,
                    viene_registro_civil = d.viene_registro_civil,
                    fecha_registro_civil = d.fecha_registro_civil,
                    es_sustituto = d.es_sustituto,
                    fecha_sustituto_desde = d.fecha_sustituto_desde,
                    nombre_identificacion = d.TipoIdentificacion.nombre,
                    nombre_destino = d.DestinoEstancia.nombre,
                    nombre_genero = d.Genero.nombre,
                    nombre_grupo_personal = d.GrupoPersonal == null ? null : d.GrupoPersonal.nombre,
                    fecha_baja = _bajasService.GetBaja(d.Id) != null ? _bajasService.GetBaja(d.Id).fecha_baja : null
                    ,
                    empleado_id_sap_local = d.empleado_id_sap_local,
                    fechaActualizacionFormat = d.LastModificationTime.HasValue ? d.LastModificationTime.Value : DateTime.Now,
                };


                return colaborador;
            }
            return null;

        }

        public string GetLegajo()
        {
            var c = Repository.GetAll().Where(d => d.es_externo == false);

            var colaboradores = Mapper.Map<IQueryable<Colaboradores>, List<Colaboradores>>(c);

            int item = 1;
            if (colaboradores.Count > 0)
            {
                item = colaboradores.Max(x => x.numero_legajo_temporal == null ? 0 : int.Parse(x.numero_legajo_temporal));
            }
            if (item != 0)
            {
                return item.ToString();
            }
            else
            {
                return "NO";
            }



        }

        public string UniqueIdentification(string nro)
        {
            var identificacion = Repository.GetAll().Where(d => d.numero_identificacion == nro && d.vigente == true).FirstOrDefault();
            if (identificacion != null && identificacion.estado != RRHHCodigos.ESTADO_INACTIVO)
            {
                return "SI";
            }
            if (identificacion != null && identificacion.estado == RRHHCodigos.ESTADO_INACTIVO)
            {
                return "INACTIVO";
            }
            return "NO";
        }

        public ColaboradoresDto GetColaboradorRequisito(int id)
        {
            var d = Repository.Get(id);

            if (d != null)
            {
                ColaboradoresDto colaborador = new ColaboradoresDto()
                {
                    Id = d.Id,
                    numero_identificacion = d.numero_identificacion,
                    primer_apellido = d.primer_apellido,
                    segundo_apellido = d.segundo_apellido,
                    nombres = d.nombres,
                    nombres_apellidos = d.nombres_apellidos,
                    catalogo_genero_id = d.catalogo_genero_id,
                    pais_pais_nacimiento_id = d.pais_pais_nacimiento_id,
                    catalogo_grupo_personal_id = d.catalogo_grupo_personal_id,
                    ContactoId = d.ContactoId,
                    catalogo_tipo_identificacion_id = d.catalogo_tipo_identificacion_id,
                    nombre_identificacion = d.TipoIdentificacion.nombre,
                    nombre_grupo_personal = d.catalogo_grupo_personal_id != null ? d.GrupoPersonal.nombre : null,
                    nombre_genero = d.Genero.nombre,
                    es_externo = d.es_externo,
                    codigo_genero = d.Genero.codigo,
                };

                return colaborador;
            }

            return null;
        }

        public List<ColaboradoresDto> GetColaboradorBajas()
        {
            var aux = 1;

            var query = Repository.GetAll().Where(c => c.vigente == true && c.estado == RRHHCodigos.ESTADO_ACTIVO || c.estado == RRHHCodigos.ESTADO_INACTIVO && c.es_externo == false);

            List<ColaboradoresDto> colaboradores = (from d in query
                                                    select new ColaboradoresDto
                                                    {
                                                        Id = d.Id,
                                                        fecha_ingreso = d.fecha_ingreso,
                                                        catalogo_tipo_identificacion_id = d.catalogo_tipo_identificacion_id,
                                                        numero_identificacion = d.numero_identificacion,
                                                        primer_apellido = d.primer_apellido,
                                                        segundo_apellido = d.segundo_apellido,
                                                        nombres = d.nombres,
                                                        numero_legajo_temporal = d.numero_legajo_temporal,
                                                        numero_legajo_definitivo = d.numero_legajo_definitivo,
                                                        estado = d.estado,
                                                        nombre_identificacion = d.TipoIdentificacion.nombre,
                                                        catalogo_encargado_personal_id = d.catalogo_encargado_personal_id,
                                                        nombre_grupo_personal = d.EncargadoPersonal.nombre,
                                                    }).ToList();

            foreach (var i in colaboradores)
            {
                i.nro = aux++;
                i.apellidos_nombres = i.primer_apellido + ' ' + i.segundo_apellido;


                if (i.estado == "INACTIVO")
                {
                    var b = _bajasService.GetBaja(i.Id);
                    if (b != null)
                    {
                        i.baja_id = b.Id;
                        i.catalogo_motivo_baja_id = b.catalogo_motivo_baja_id;
                        i.fecha_baja = b.fecha_baja;
                        i.estado_baja = b.estado;
                        i.motivo_baja = b.motivo_baja;
                        i.liquidado = b.fecha_pago_liquidacion;
                    }

                }
            }



            return colaboradores;
        }


        public Colaboradores GetColaboradorPorTipoIdentificacion(int? tipoIdentificacion, string numero)
        {
            var e = 1;
            Colaboradores query = null;

            if (tipoIdentificacion != 0)
            {
                query = Repository.GetAll()
                .Where(a => a.numero_identificacion == numero && a.catalogo_tipo_identificacion_id == tipoIdentificacion
                && a.vigente == true).FirstOrDefault();
            }
            else
            {
                query = Repository.GetAll()
                .Where(a => a.numero_identificacion == numero && a.vigente == true).FirstOrDefault();
            }


            return query;
        }

        public Colaboradores GetColaboradorPorTipoIdentificacionExcluidoExterno(int? tipoIdentificacion, string numero)
        {
            var e = 1;
            Colaboradores query = null;

            if (tipoIdentificacion != 0)
            {
                query = Repository.GetAll()
                .Where(a => a.numero_identificacion == numero && a.catalogo_tipo_identificacion_id == tipoIdentificacion
                && a.vigente == true).Where(c => c.es_externo == false).OrderByDescending(c => c.fecha_ingreso
                ).FirstOrDefault();
            }
            else
            {
                query = Repository.GetAll()
                .Where(a => a.numero_identificacion == numero && a.vigente == true).Where(c => c.es_externo == false).OrderByDescending(c => c.fecha_ingreso
                ).FirstOrDefault();
            }


            return query;
        }



        public Colaboradores GetColaboradorPorTipoIdentificacionExcluidoExternoActivacionMasiva(int? tipoIdentificacion, string numero)
        {
            var e = 1;
            Colaboradores query = null;

            if (tipoIdentificacion != 0)
            {
                query = Repository.GetAll()
                .Where(a => a.numero_identificacion == numero && a.catalogo_tipo_identificacion_id == tipoIdentificacion
                && a.vigente == true).Where(c => c.es_externo == false).OrderByDescending(c => c.fecha_ingreso
                ).Where(c => c.estado != RRHHCodigos.ESTADO_INACTIVO)
                .FirstOrDefault();
            }
            else
            {
                query = Repository.GetAll()
                .Where(a => a.numero_identificacion == numero && a.vigente == true).Where(c => c.es_externo == false).OrderByDescending(c => c.fecha_ingreso
                ).Where(c => c.estado != RRHHCodigos.ESTADO_INACTIVO).FirstOrDefault();
            }


            return query;
        }

        public List<ColaboradoresDto> GetColaboradorPorIdentificacion(string numero)
        {
            var e = 1;
            var query = Repository.GetAll().Where(a => a.numero_identificacion == numero && a.vigente == true);

            var colaborador = (from d in query
                               where d.vigente == true
                               select new ColaboradoresDto
                               {
                                   Id = d.Id,
                                   fecha_ingreso = d.fecha_ingreso,
                                   catalogo_tipo_identificacion_id = d.catalogo_tipo_identificacion_id,
                                   numero_identificacion = d.numero_identificacion,
                                   primer_apellido = d.primer_apellido,
                                   segundo_apellido = d.segundo_apellido,
                                   nombres = d.nombres,
                                   fecha_nacimiento = d.fecha_nacimiento,
                                   catalogo_genero_id = d.catalogo_genero_id,
                                   PaisId = d.PaisId,
                                   pais_pais_nacimiento_id = d.pais_pais_nacimiento_id,
                                   ContactoId = d.ContactoId,
                                   catalogo_destino_estancia_id = d.catalogo_destino_estancia_id,
                                   vigente = d.vigente,
                                   validacion_cedula = d.validacion_cedula,
                                   nombre_identificacion = d.TipoIdentificacion.nombre,
                               }).ToList();

            foreach (var i in colaborador)
            {
                i.nro = e++;

                i.apellidos_nombres = i.primer_apellido + ' ' + i.segundo_apellido;
            }

            return colaborador;
        }

        public async Task<List<ColaboradoresLookupDto>> GetLookupAll()
        {
            var query = Repository.GetAll();

            var lookupDtos = await (from d in query
                                    where d.vigente == true
                                    select new ColaboradoresLookupDto
                                    {
                                        Id = d.Id,
                                        nro_identificacion = d.numero_identificacion,
                                        nombres = d.nombres_apellidos,
                                        apellidos = d.primer_apellido + " " + d.segundo_apellido,
                                        nombres_completos = d.nombres_apellidos
                                    }
                               ).ToListAsync();
            return lookupDtos;
        }
        public List<ColaboradoresLookupDto> GetAnotadoresLookupAll()
        {
            var Colaboradores = new List<Colaboradores>();
            var rolesanotadores = _rolrepository.GetAllIncluding(c => c.Usuarios)
                                      .Where(c => c.Codigo == "ANO")
                                      .Select(c => c.Usuarios).ToList();

            ;
            foreach (var anotador in rolesanotadores)
            {
                foreach (var a in anotador)
                {
                    var colaborador = Repository.GetAll().Where(c => c.numero_identificacion == a.Identificacion).FirstOrDefault();
                    if (colaborador != null && !Colaboradores.Any(c => c.Id == colaborador.Id))
                    {
                        Colaboradores.Add(colaborador);
                    }
                }


            }


            var lookupDtos = (from d in Colaboradores
                              where d.vigente == true
                              select new ColaboradoresLookupDto
                              {
                                  Id = d.Id,
                                  nro_identificacion = d.numero_identificacion,
                                  nombres = d.nombres + " " + d.primer_apellido + " " + d.segundo_apellido
                              }
                               ).ToList();
            return lookupDtos;
        }

        public List<ColaboradoresLookupDto> GetTransportistasLookupAll()
        {
            var Colaboradores = new List<Colaboradores>();
            var rolesanotadores = _rolrepository.GetAllIncluding(c => c.Usuarios)
                                      .Where(c => c.Codigo == "TRA")
                                      .Select(c => c.Usuarios).ToList();

            ;
            foreach (var anotador in rolesanotadores)
            {
                foreach (var a in anotador)
                {
                    var colaborador = Repository.GetAll().Where(c => c.numero_identificacion == a.Identificacion).FirstOrDefault();
                    if (colaborador != null && !Colaboradores.Any(c => c.Id == colaborador.Id))
                    {
                        Colaboradores.Add(colaborador);
                    }
                }


            }


            var lookupDtos = (from d in Colaboradores
                              where d.vigente == true
                              select new ColaboradoresLookupDto
                              {
                                  Id = d.Id,
                                  nro_identificacion = d.numero_identificacion,
                                  nombres = d.nombres + " " + d.primer_apellido + " " + d.segundo_apellido
                              }
                               ).ToList();
            return lookupDtos;
        }

        public async Task<string> GenerarExcelCarga(List<ColaboradoresDto> colaboradores, bool es_manual)
        {
            DateTime fechaActual = DateTime.Now;

            var aux = fechaActual.ToString("ddMMyyyyhhmm");
            var fecha = fechaActual.ToString("dd/MM/yyyy");

            var usuario = "";
            string user = System.Web.HttpContext.Current.User.Identity.Name.ToString();
            var usuarioencontrado = _usuarioRepository.GetAll().Where(c => c.Cuenta == user).FirstOrDefault();
            if (usuarioencontrado != null && usuarioencontrado.Id > 0)
            {
                usuario = usuarioencontrado.NombresCompletos;
            }

            var motivos_medida = _catalogoRepository.ListarCatalogosporcodigo(RRHHCodigos.MOTIVO_MEDIDA);

            FileInfo prueba = new FileInfo("prueba.xlsx");
            using (ExcelPackage excel = new ExcelPackage(prueba))
            {
                //Crear hoja en archivo excel
                var hoja = excel.Workbook.Worksheets.Add("Alta de Colaboradores");
                var row = 16;
                //Width de Columnas
                hoja.Column(1).Width = 1.45;
                hoja.Column(2).Width = 10;
                hoja.Column(3).Width = 11;
                hoja.Column(4).Width = 11;
                hoja.Column(5).Width = 8;
                hoja.Column(6).Width = 12;
                hoja.Column(7).Width = 11;
                hoja.Column(8).Width = 16;
                hoja.Column(9).Width = 11;
                hoja.Column(10).Width = 11;
                hoja.Column(11).Width = 11;
                hoja.Column(12).Width = 10;
                hoja.Column(13).Width = 12;
                hoja.Column(14).Width = 11;
                hoja.Column(15).Width = 37;
                hoja.Column(16).Width = 11;
                hoja.Column(17).Width = 12;
                hoja.Column(18).Width = 11;
                hoja.Column(19).Width = 12;
                hoja.Column(20).Width = 10;
                hoja.Column(21).Width = 9;
                hoja.Column(22).Width = 22;
                hoja.Column(23).Width = 8;
                hoja.Column(24).Width = 9;
                hoja.Column(25).Width = 26;
                hoja.Column(26).Width = 13;
                hoja.Column(27).Width = 12;
                hoja.Column(28).Width = 22;
                hoja.Column(29).Width = 33;
                hoja.Column(30).Width = 28;
                hoja.Column(31).Width = 26;
                hoja.Column(32).Width = 15;
                hoja.Column(33).Width = 27;
                hoja.Column(34).Width = 19;
                hoja.Column(35).Width = 22;
                hoja.Column(36).Width = 22;
                hoja.Column(37).Width = 22;
                hoja.Column(38).Width = 23;
                hoja.Column(39).Width = 43;
                hoja.Column(40).Width = 16;
                hoja.Column(41).Width = 21;
                hoja.Column(42).Width = 29;
                hoja.Column(43).Width = 11;
                hoja.Column(44).Width = 11;
                hoja.Column(45).Width = 13;
                hoja.Column(46).Width = 25;
                hoja.Column(47).Width = 46;
                hoja.Column(48).Width = 43;
                hoja.Column(49).Width = 18;
                hoja.Column(50).Width = 19;
                hoja.Column(51).Width = 17;

                //height filas de titulos
                hoja.Row(3).Height = 30;
                hoja.Row(4).Height = 30;
                hoja.Row(5).Height = 30;
                hoja.Row(6).Height = 30;
                hoja.Row(7).Height = 30;
                hoja.Row(8).Height = 30;
                hoja.Row(9).Height = 30;
                hoja.Row(10).Height = 30;
                hoja.Row(11).Height = 30;
                hoja.Row(12).Height = 30;
                hoja.Row(13).Height = 30;
                hoja.Row(14).Height = 30;
                hoja.Row(15).Height = 30;


                //Cabecera de Documento
                hoja.Cells["A1:AY1"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                hoja.Cells["A1:AY1"].Style.Border.Bottom.Color.SetColor(Color.Black);

                hoja.Cells["B3:AY3"].Style.Font.Bold = true;
                hoja.Cells["B3:AY3"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                hoja.Cells["B3:AY3"].Style.Fill.BackgroundColor.SetColor(Color.DarkGray);
                hoja.Cells["B3:AY3"].Style.Border.BorderAround(ExcelBorderStyle.Thin, System.Drawing.Color.White);
                hoja.Cells["B3:AY3"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                hoja.Cells["E3"].Value = "Alta de Empleado";
                hoja.Cells["E3"].Style.Font.Color.SetColor(Color.White);
                hoja.Cells["E3"].Style.Font.Name = "Arial";
                hoja.Cells["E3"].Style.Font.Size = 20;

                hoja.Cells["A4:AY4"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                hoja.Cells["A4:AY4"].Style.Border.Bottom.Color.SetColor(Color.Black);


                hoja.Cells["A5:AY5"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                hoja.Cells["A5:AY5"].Style.Border.Bottom.Color.SetColor(Color.Black);


                hoja.Cells["B7:AY7"].Style.Font.Bold = true;
                hoja.Cells["B7:AY7"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                hoja.Cells["B7:AY7"].Style.Fill.BackgroundColor.SetColor(Color.DarkGray);
                hoja.Cells["B7:AY7"].Style.Border.BorderAround(ExcelBorderStyle.Thin, System.Drawing.Color.White);
                hoja.Cells["B7:AY7"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                hoja.Cells["B7"].Value = "Una vez completado cargar en el CSC Ecuador";
                hoja.Cells["B7"].Style.Font.Color.SetColor(Color.White);
                hoja.Cells["B7"].Style.Font.Name = "Calibri";
                hoja.Cells["B7"].Style.Font.Size = 12;

                hoja.Cells["B9:AY9"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                hoja.Cells["B9:AY9"].Style.Fill.BackgroundColor.SetColor(Color.DarkGray);
                hoja.Cells["B9:AY9"].Style.Border.BorderAround(ExcelBorderStyle.Thin, System.Drawing.Color.White);
                hoja.Cells["B9:AY9"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                hoja.Cells["B9"].Value = "Proyecto:";
                hoja.Cells["B9"].Style.Font.Name = "Calibri";
                hoja.Cells["B9"].Style.Font.Size = 12;
                hoja.Cells["D9"].Value = "ECUADOR";
                hoja.Cells["D9"].Style.Font.Name = "Calibri";
                hoja.Cells["D9"].Style.Font.Size = 10;

                hoja.Cells["B10:AY10"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                hoja.Cells["B10:AY10"].Style.Fill.BackgroundColor.SetColor(Color.DarkGray);
                hoja.Cells["B10:AY10"].Style.Border.BorderAround(ExcelBorderStyle.Thin, System.Drawing.Color.White);
                hoja.Cells["B10:AY10"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                hoja.Cells["B10"].Value = "Cargado por:";
                hoja.Cells["B10"].Style.Font.Name = "Calibri";
                hoja.Cells["B10"].Style.Font.Size = 12;
                hoja.Cells["D10"].Value = usuario;
                hoja.Cells["D10"].Style.Font.Name = "Calibri";
                hoja.Cells["D10"].Style.Font.Size = 10;

                hoja.Cells["B11:AY11"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                hoja.Cells["B11:AY11"].Style.Fill.BackgroundColor.SetColor(Color.DarkGray);
                hoja.Cells["B11:AY11"].Style.Border.BorderAround(ExcelBorderStyle.Thin, System.Drawing.Color.White);
                hoja.Cells["B11:AY11"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                hoja.Cells["B11"].Value = "Fecha:";
                hoja.Cells["B11"].Style.Font.Name = "Calibri";
                hoja.Cells["B11"].Style.Font.Size = 12;
                hoja.Cells["D11"].Value = fecha;
                hoja.Cells["D11"].Style.Font.Name = "Calibri";
                hoja.Cells["D11"].Style.Font.Size = 10;

                //Cabecera de la tabla de Alta de Colaboradores
                var titleCell = hoja.Cells["B14:AY14"]; // Celdas de títulos

                titleCell.Style.Font.Bold = true;
                titleCell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                titleCell.Style.Fill.BackgroundColor.SetColor(Color.DarkGray);
                titleCell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                titleCell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                titleCell.Style.WrapText = true;

                titleCell.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                titleCell.Style.Border.Right.Color.SetColor(Color.White);
                hoja.Cells["B15:AY15"].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                hoja.Cells["B15:AY15"].Style.Border.Right.Color.SetColor(Color.White);

                //titulos de formacion educativa
                hoja.Cells["AL15:AN15"].Style.Font.Bold = true;
                hoja.Cells["AL15:AN15"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                hoja.Cells["AL15:AN15"].Style.Fill.BackgroundColor.SetColor(Color.DarkGray);
                hoja.Cells["AL15:AN15"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                hoja.Cells["AL15:AN15"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                hoja.Cells["AL15:AN15"].Style.WrapText = true;

                hoja.Cells["AL15:AN15"].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                hoja.Cells["AL15:AN15"].Style.Border.Right.Color.SetColor(Color.White);
                hoja.Cells["AL15:AN15"].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                hoja.Cells["AL15:AN15"].Style.Border.Top.Color.SetColor(Color.White);

                hoja.Cells["B14:B15"].Merge = true;
                hoja.Cells["B14:B15"].Value = "Fecha de Alta";

                hoja.Cells["C14:C15"].Merge = true;
                hoja.Cells["C14:C15"].Value = "ID_CANDIDATO";

                hoja.Cells["D14:D15"].Merge = true;
                hoja.Cells["D14:D15"].Value = "ID_POSICION";

                hoja.Cells["E14:E15"].Merge = true;
                hoja.Cells["E14:E15"].Value = "ID_EMPLEADO";

                hoja.Cells["F14:F15"].Merge = true;
                hoja.Cells["F14:F15"].Value = "Motivo de Medida";

                hoja.Cells["G14:G15"].Merge = true;
                hoja.Cells["G14:G15"].Value = "Division de Personal";

                hoja.Cells["H14:H15"].Merge = true;
                hoja.Cells["H14:H15"].Value = "Subdivision de Personal (para donde va)";

                hoja.Cells["I14:I15"].Merge = true;
                hoja.Cells["I14:I15"].Value = "Encargado de personal (Nombre del Jefe Inmediato)";

                hoja.Cells["J14:J15"].Merge = true;
                hoja.Cells["J14:J15"].Value = "Vinculo laboral (Permanente / Temporal)";

                hoja.Cells["K14:K15"].Merge = true;
                hoja.Cells["K14:K15"].Value = "Encuadre FC";

                hoja.Cells["L14:L15"].Merge = true;
                hoja.Cells["L14:L15"].Value = "Tipo de Documento";

                hoja.Cells["M14:M15"].Merge = true;
                hoja.Cells["M14:M15"].Value = "Nro de Documento";

                hoja.Cells["N14:N15"].Merge = true;
                hoja.Cells["N14:N15"].Value = "País de Nacimiento";

                hoja.Cells["O14:O15"].Merge = true;
                hoja.Cells["O14:O15"].Value = "Nombres";

                hoja.Cells["P14:P15"].Merge = true;
                hoja.Cells["P14:P15"].Value = "Fecha de Nacimiento";

                hoja.Cells["Q14:Q15"].Merge = true;
                hoja.Cells["Q14:Q15"].Value = "Estado civil";

                hoja.Cells["R14:R15"].Merge = true;
                hoja.Cells["R14:R15"].Value = "Fecha casamiento";

                hoja.Cells["S14:S15"].Merge = true;
                hoja.Cells["S14:S15"].Value = "Nacionalidad";

                hoja.Cells["T14:T15"].Merge = true;
                hoja.Cells["T14:T15"].Value = "N° Hijos";

                hoja.Cells["U14:U15"].Merge = true;
                hoja.Cells["U14:U15"].Value = "Sexo";

                hoja.Cells["V14:V15"].Merge = true;
                hoja.Cells["V14:V15"].Value = "Calle";

                hoja.Cells["W14:W15"].Merge = true;
                hoja.Cells["W14:W15"].Value = "Número";

                hoja.Cells["X14:X15"].Merge = true;
                hoja.Cells["X14:X15"].Value = "Código Postal";

                hoja.Cells["Y14:Y15"].Merge = true;
                hoja.Cells["Y14:Y15"].Value = "Región";

                hoja.Cells["Z14:Z15"].Merge = true;
                hoja.Cells["Z14:Z15"].Value = "Teléfono de contacto";

                hoja.Cells["AA14:AA15"].Merge = true;
                hoja.Cells["AA14:AA15"].Value = "Sub región";

                hoja.Cells["AB14:AB15"].Merge = true;
                hoja.Cells["AB14:AB15"].Value = "Clase de Contrato";

                hoja.Cells["AC14:AC15"].Merge = true;
                hoja.Cells["AC14:AC15"].Value = "Plan de Beneficios";

                hoja.Cells["AD14:AD15"].Merge = true;
                hoja.Cells["AD14:AD15"].Value = "Opción plan salud";

                hoja.Cells["AE14:AE15"].Merge = true;
                hoja.Cells["AE14:AE15"].Value = "Cobertura dependiente";

                hoja.Cells["AF14:AF15"].Merge = true;
                hoja.Cells["AF14:AF15"].Value = "Planes de beneficios";

                hoja.Cells["AG14:AG15"].Merge = true;
                hoja.Cells["AG14:AG15"].Value = "Banco";

                hoja.Cells["AH14:AH15"].Merge = true;
                hoja.Cells["AH14:AH15"].Value = "Vía de pago";

                hoja.Cells["AI14:AI15"].Merge = true;
                hoja.Cells["AI14:AI15"].Value = "N° Cuenta";

                hoja.Cells["AJ14:AJ15"].Merge = true;
                hoja.Cells["AJ14:AJ15"].Value = "Mail Personal";

                hoja.Cells["AK14:AK15"].Merge = true;
                hoja.Cells["AK14:AK15"].Value = "Formación Educativa";

                //
                hoja.Cells["AL14:AN14"].Merge = true;
                hoja.Cells["AL14:AN14"].Value = "Llenar solo en caso de estudios Superiores o Universitarios Completos";

                //
                hoja.Cells["AL15"].Value = "Formación";
                hoja.Cells["AM15"].Value = "Nombre de la Institución Educativa (Superior o Universitaria)";
                hoja.Cells["AN15"].Value = "Título";
                //

                hoja.Cells["AO14:AO15"].Merge = true;
                hoja.Cells["AO14:AO15"].Value = "Clase";

                hoja.Cells["AP14:AP15"].Merge = true;
                hoja.Cells["AP14:AP15"].Value = "Área";

                hoja.Cells["AQ14:AQ15"].Merge = true;
                hoja.Cells["AQ14:AQ15"].Value = "Grupo (Categoría O PC)";

                hoja.Cells["AR14:AR15"].Merge = true;
                hoja.Cells["AR14:AR15"].Value = "Sub Grupo (Cuartil)";

                hoja.Cells["AS14:AS15"].Merge = true;
                hoja.Cells["AS14:AS15"].Value = "Remuneracion Mensual";

                hoja.Cells["AT14:AT15"].Merge = true;
                hoja.Cells["AT14:AT15"].Value = "Asociacion (Afiliado a Sindicato)";

                hoja.Cells["AU14:AU15"].Merge = true;
                hoja.Cells["AU14:AU15"].Value = "Funcion";

                hoja.Cells["AV14:AV15"].Merge = true;
                hoja.Cells["AV14:AV15"].Value = "Cod. Siniestro";

                hoja.Cells["AW14:AW15"].Merge = true;
                hoja.Cells["AW14:AW15"].Value = "Cod. Incapacidad";

                hoja.Cells["AX14:AX15"].Merge = true;
                hoja.Cells["AX14:AX15"].Value = "Período Nómina";

                hoja.Cells["AY14:AY15"].Merge = true;
                hoja.Cells["AY14:AY15"].Value = "Tipo Apto Medico";


                //HOJA DE CARGAS FAMILIARES

                var worksheet = excel.Workbook.Worksheets.Add("FamiliarEcu");

                worksheet.Column(2).Width = 10;
                worksheet.Column(3).Width = 15;
                worksheet.Column(4).Width = 38;
                worksheet.Column(5).Width = 15;
                worksheet.Column(6).Width = 20;
                worksheet.Column(7).Width = 17;
                worksheet.Column(8).Width = 14;
                worksheet.Column(9).Width = 9;
                worksheet.Column(10).Width = 10;
                worksheet.Column(11).Width = 16;
                worksheet.Column(12).Width = 13;
                worksheet.Column(13).Width = 9;
                worksheet.Column(14).Width = 12;
                worksheet.Column(15).Width = 9;
                worksheet.Column(16).Width = 12;
                worksheet.Column(17).Width = 19;

                var titleCarga = worksheet.Cells["B5:Q5"]; // Celdas de títulos

                titleCarga.Style.Font.Bold = true;
                titleCarga.Style.Fill.PatternType = ExcelFillStyle.Solid;
                titleCarga.Style.Fill.BackgroundColor.SetColor(Color.DarkGray);
                titleCarga.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                titleCarga.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                titleCarga.Style.WrapText = true;

                titleCarga.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                titleCarga.Style.Border.Right.Color.SetColor(Color.White);

                worksheet.Cells["B5:B6"].Merge = true;
                worksheet.Cells["B5:B6"].Value = "ID Empleado";

                worksheet.Cells["C5:C6"].Merge = true;
                worksheet.Cells["C5:C6"].Value = "Cédula/Pasaporte del Colaborador";

                worksheet.Cells["D5:D6"].Merge = true;
                worksheet.Cells["D5:D6"].Value = "Nombre del Colaborador";

                worksheet.Cells["E5:E6"].Merge = true;
                worksheet.Cells["E5:E6"].Value = "Parentesco";

                worksheet.Cells["F5:F6"].Merge = true;
                worksheet.Cells["F5:F6"].Value = "Apellido Paterno";

                worksheet.Cells["G5:G6"].Merge = true;
                worksheet.Cells["G5:G6"].Value = "Apellido Materno";

                worksheet.Cells["H5:H6"].Merge = true;
                worksheet.Cells["H5:H6"].Value = "Nombres";

                worksheet.Cells["I5:I6"].Merge = true;
                worksheet.Cells["I5:I6"].Value = "Sexo";

                worksheet.Cells["J5:J6"].Merge = true;
                worksheet.Cells["J5:J6"].Value = "Fecha de Nacimiento";

                worksheet.Cells["K5:K6"].Merge = true;
                worksheet.Cells["K5:K6"].Value = "País de Nacimiento";

                worksheet.Cells["L5:L6"].Merge = true;
                worksheet.Cells["L5:L6"].Value = "Nacionalidad";

                worksheet.Cells["M5:M6"].Merge = true;
                worksheet.Cells["M5:M6"].Value = "Estado civil";

                worksheet.Cells["N5:N6"].Merge = true;
                worksheet.Cells["N5:N6"].Value = "Fecha casamiento";

                worksheet.Cells["O5:O6"].Merge = true;
                worksheet.Cells["O5:O6"].Value = "Tipo de Documento";

                worksheet.Cells["P5:P6"].Merge = true;
                worksheet.Cells["P5:P6"].Value = "Nro de Documento";

                worksheet.Cells["Q5:Q6"].Merge = true;
                worksheet.Cells["Q5:Q6"].Value = "Persona con Discapacidad";
                var rowc = 7;


                foreach (var i in colaboradores)
                {

                    var body = hoja.Cells["B" + row + ":AY" + row];
                    body.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    body.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    body.Style.Font.Name = "Calibri";
                    body.Style.Font.Size = 9;
                    body.Style.WrapText = true;
                    body.Style.Border.BorderAround(ExcelBorderStyle.Dotted, System.Drawing.Color.Black);

                    body.Style.Border.Right.Style = ExcelBorderStyle.Dotted;
                    body.Style.Border.Right.Color.SetColor(Color.Black);

                    hoja.Row(row).Height = 24;



                    hoja.Cells["B" + row].Value = String.Format("{0:dd/MM/yyyy}", i.fecha_ingreso);
                    hoja.Cells["C" + row].Value = i.candidato_id_sap;
                    hoja.Cells["D" + row].Value = i.posicion;
                    if (i.empleado_id_sap.HasValue && i.empleado_id_sap.Value.ToString().Length > 0)
                    {
                        foreach (var m in motivos_medida)
                        {
                            if (m.codigo == RRHHCodigos.MOTIVO_MEDIDA_REINGRESO)
                            {
                                hoja.Cells["E" + row].Value = i.empleado_id_sap;
                                hoja.Cells["F" + row].Value = m.nombre;
                            }
                        }

                    }
                    else
                    {
                        foreach (var m in motivos_medida)
                        {
                            if (m.codigo == RRHHCodigos.MOTIVO_MEDIDA_ALTA)
                            {
                                hoja.Cells["E" + row].Value = "";
                                hoja.Cells["F" + row].Value = m.nombre;
                            }
                        }

                    }

                    hoja.Cells["G" + row].Value = i.catalogo_division_personal_id != null ? _catalogoRepository.GetCatalogo(i.catalogo_division_personal_id.Value).nombre : "";
                    hoja.Cells["H" + row].Value = i.catalogo_subdivision_personal_id != null ? _catalogoRepository.GetCatalogo(i.catalogo_subdivision_personal_id.Value).nombre : "";
                    hoja.Cells["I" + row].Value = i.catalogo_encargado_personal_id != null ? _catalogoRepository.GetCatalogo(i.catalogo_encargado_personal_id.Value).nombre : "";
                    hoja.Cells["J" + row].Value = i.catalogo_vinculo_laboral_id != null ? _catalogoRepository.GetCatalogo(i.catalogo_vinculo_laboral_id.Value).nombre : "";
                    hoja.Cells["K" + row].Value = i.catalogo_encuadre_id != null ? _catalogoRepository.GetCatalogo(i.catalogo_encuadre_id.Value).nombre : "";
                    hoja.Cells["L" + row].Value = i.catalogo_tipo_identificacion_id != null ? _catalogoRepository.GetCatalogo(i.catalogo_tipo_identificacion_id.Value).nombre : "";
                    hoja.Cells["M" + row].Value = i.numero_identificacion;
                    hoja.Cells["N" + row].Value = i.PaisId.HasValue ? i.Pais.nombre : "";

                    String nombresCapitalizado = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(i.nombres_apellidos.ToLower());
                    hoja.Cells["O" + row].Value = nombresCapitalizado.Normalize(NormalizationForm.FormD);


                    hoja.Cells["P" + row].Value = String.Format("{0:dd/MM/yyyy}", i.fecha_nacimiento);
                    hoja.Cells["Q" + row].Value = i.catalogo_estado_civil_id.HasValue ? _catalogoRepository.GetCatalogo(i.catalogo_estado_civil_id.Value).nombre : "";
                    hoja.Cells["R" + row].Value = String.Format("{0:dd/MM/yyyy}", i.fecha_matrimonio);
                    hoja.Cells["S" + row].Value = i.pais_pais_nacimiento_id > 0 ? _catalogoRepository.GetCatalogo(i.pais_pais_nacimiento_id).nombre : "";
                    hoja.Cells["T" + row].Value = i.numero_hijos;
                    hoja.Cells["U" + row].Value = i.catalogo_genero_id.HasValue ? _catalogoRepository.GetCatalogo(i.catalogo_genero_id.Value).nombre : "";
                    hoja.Cells["V" + row].Value = i.calle;
                    hoja.Cells["W" + row].Value = i.numero;
                    hoja.Cells["X" + row].Value = i.codigo_postal;
                    hoja.Cells["Y" + row].Value = i.region;
                    hoja.Cells["Z" + row].Value = i.telefono;
                    hoja.Cells["AA" + row].Value = i.subregion;
                    hoja.Cells["AB" + row].Value = i.catalogo_clase_contrato_id.HasValue ? _catalogoRepository.GetCatalogo(i.catalogo_clase_contrato_id.Value).nombre : "";
                    hoja.Cells["AC" + row].Value = i.catalogo_plan_beneficios_id.HasValue ? _catalogoRepository.GetCatalogo(i.catalogo_plan_beneficios_id.Value).nombre : "";
                    hoja.Cells["AD" + row].Value = i.catalogo_plan_salud_id.HasValue ? _catalogoRepository.GetCatalogo(i.catalogo_plan_salud_id.Value).nombre : "";
                    hoja.Cells["AE" + row].Value = i.catalogo_cobertura_dependiente_id.HasValue ? _catalogoRepository.GetCatalogo(i.catalogo_cobertura_dependiente_id.Value).nombre : "";
                    hoja.Cells["AF" + row].Value = i.catalogo_planes_beneficios_id.HasValue ? _catalogoRepository.GetCatalogo(i.catalogo_planes_beneficios_id.Value).nombre : "";
                    hoja.Cells["AG" + row].Value = i.catalogo_banco_id == null ? "" : _catalogoRepository.GetCatalogo(i.catalogo_banco_id.Value).nombre;
                    hoja.Cells["AH" + row].Value = i.catalogo_via_pago_id.HasValue ? _catalogoRepository.GetCatalogo(i.catalogo_via_pago_id.Value).nombre : "";
                    hoja.Cells["AI" + row].Value = i.numero_cuenta;
                    hoja.Cells["AJ" + row].Value = i.email;
                    hoja.Cells["AK" + row].Value = i.catalogo_formacion_educativa_id == null ? "" : _catalogoRepository.GetCatalogo(i.catalogo_formacion_educativa_id.Value).nombre;
                    //
                    hoja.Cells["AL" + row].Value = i.formacion == null || i.formacion == 0 ? "" : i.formacion.HasValue ? _catalogoRepository.GetCatalogo(i.formacion.Value).nombre : "";
                    hoja.Cells["AM" + row].Value = i.institucion_educativa;
                    hoja.Cells["AN" + row].Value = i.catalogo_titulo_id == 0 ? "" : i.catalogo_titulo_id.HasValue ? _catalogoRepository.GetCatalogo(i.catalogo_titulo_id.Value).nombre : "";
                    //
                    hoja.Cells["AO" + row].Value = i.catalogo_clase_id.HasValue ? _catalogoRepository.GetCatalogo(i.catalogo_clase_id.Value).nombre : "";
                    hoja.Cells["AP" + row].Value = i.catalogo_area_id.HasValue ? _catalogoRepository.GetCatalogo(i.catalogo_area_id.Value).nombre : "";
                    hoja.Cells["AQ" + row].Value = i.catalogo_grupo_id == null ? "" : _catalogoRepository.GetCatalogo(i.catalogo_grupo_id.Value).nombre;
                    hoja.Cells["AR" + row].Value = i.catalogo_subgrupo_id == null ? "" : _catalogoRepository.GetCatalogo(i.catalogo_subgrupo_id.Value).nombre;
                    hoja.Cells["AS" + row].Value = String.Format("{0:n}", i.remuneracion_mensual);
                    hoja.Cells["AT" + row].Value = i.catalogo_asociacion_id.HasValue ? _catalogoRepository.GetCatalogo(i.catalogo_asociacion_id.Value).nombre : "";
                    hoja.Cells["AU" + row].Value = i.catalogo_funcion_id.HasValue ? _catalogoRepository.GetCatalogo(i.catalogo_funcion_id.Value).nombre : "";
                    hoja.Cells["AV" + row].Value = i.catalogo_codigo_siniestro_id == null ? "42 - No incapacitado" : _catalogoRepository.GetCatalogo(i.catalogo_codigo_siniestro_id.Value).nombre;
                    hoja.Cells["AW" + row].Value = i.catalogo_codigo_incapacidad_id == null ? "9 - No Incap." : _catalogoRepository.GetCatalogo(i.catalogo_codigo_incapacidad_id.Value).nombre;
                    hoja.Cells["AX" + row].Value = i.catalogo_periodo_nomina_id.HasValue ? _catalogoRepository.GetCatalogo(i.catalogo_periodo_nomina_id.Value).nombre : "";
                    hoja.Cells["AY" + row].Value = i.catalogo_apto_medico_id.HasValue ? _catalogoRepository.GetCatalogo(i.catalogo_apto_medico_id.Value).nombre : "";

                    row++;

                    //LLENAR DATOS DE CARGAS FAMILIARES

                    if (i.cargas != null)
                    {


                        foreach (var c in i.cargas)
                        {
                            var bodyc = worksheet.Cells["B" + rowc + ":Q" + rowc];
                            bodyc.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            bodyc.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            bodyc.Style.Font.Name = "Calibri";
                            bodyc.Style.Font.Size = 9;
                            bodyc.Style.WrapText = true;
                            bodyc.Style.Border.BorderAround(ExcelBorderStyle.Dotted, System.Drawing.Color.Black);

                            bodyc.Style.Border.Right.Style = ExcelBorderStyle.Dotted;
                            bodyc.Style.Border.Right.Color.SetColor(Color.Black);

                            if (i.empleado_id_sap == 0)
                            {
                                worksheet.Cells["B" + rowc].Value = null;
                            }
                            else
                            {
                                worksheet.Cells["B" + rowc].Value = i.empleado_id_sap;
                            }

                            worksheet.Cells["C" + rowc].Value = i.numero_identificacion;
                            worksheet.Cells["D" + rowc].Value = i.nombres_apellidos;
                            worksheet.Cells["E" + rowc].Value = _catalogoRepository.GetCatalogo(c.parentesco_id).nombre;
                            worksheet.Cells["F" + rowc].Value = c.primer_apellido;
                            worksheet.Cells["G" + rowc].Value = c.segundo_apellido;
                            worksheet.Cells["H" + rowc].Value = c.nombres;
                            worksheet.Cells["I" + rowc].Value = _catalogoRepository.GetCatalogo(c.idGenero).nombre;
                            worksheet.Cells["J" + rowc].Value = String.Format("{0:dd/MM/yyyy}", c.fecha_nacimiento);
                            worksheet.Cells["K" + rowc].Value = c.Pais.nombre;
                            worksheet.Cells["L" + rowc].Value = _catalogoRepository.GetCatalogo(c.pais_nacimiento).nombre;
                            worksheet.Cells["M" + rowc].Value = _catalogoRepository.GetCatalogo(c.estado_civil).nombre;
                            worksheet.Cells["N" + rowc].Value = String.Format("{0:dd/MM/yyyy}", c.fecha_matrimonio);
                            worksheet.Cells["O" + rowc].Value = _catalogoRepository.GetCatalogo(c.idTipoIdentificacion).nombre;
                            worksheet.Cells["P" + rowc].Value = c.nro_identificacion;

                            var d = _discapacidadService.GetDiscapacidadCargaSocial(c.Id);
                            if (d != null)
                            {
                                worksheet.Cells["Q" + rowc].Value = "SI";
                            }
                            else
                            {
                                worksheet.Cells["Q" + rowc].Value = "NO";
                            }
                            rowc++;

                        }

                    }
                }



                if (es_manual == true)
                {
                    var correos = _correslistarepository.GetAll().Where(c => c.vigente)
                                                                .Where(c => c.ListaDistribucion.vigente)
                                                                .Where(c => c.ListaDistribucion.nombre == CatalogosCodigos.DEFAULT_LISTADISTRIBUCION).ToList();
                    if (correos.Count > 0)
                    {

                        /* ES: Envio de Excel al Correo*/
                        MailMessage message = new MailMessage();
                        message.Subject = "PMDIS: Datos Colaboradores";


                        foreach (var item in correos)
                        {
                            message.To.Add(item.correo);
                            ElmahExtension.LogToElmah(new Exception("Send to: " + item.correo));
                        }

                        excel.SaveAs(new FileInfo(System.Web.HttpContext.Current.Server.MapPath("~/Views/ArchivosColaboradores/AltaEmergentesColaboradores" + aux + ".xlsx")));

                        string url = System.Web.HttpContext.Current.Server.MapPath("~/Views/ArchivosColaboradores/AltaEmergentesColaboradores" + aux + ".xlsx");
                        if (File.Exists((string)url))
                        {
                            message.Attachments.Add(new Attachment(url));
                        }
                        await _correoservice.SendWithFilesAsync(message);
                        /*********/



                    }

                    /* System.IO.FileInfo filename = new System.IO.FileInfo(@"C:\CPP\Colaboradores\AltasEmergentes\AltaEmergentesColaboradores" + aux + ".xlsx");
                    excel.SaveAs(filename);*/
                }
                else
                {
                    var correos = _correslistarepository.GetAll().Where(c => c.vigente)
                                                               .Where(c => c.ListaDistribucion.vigente)
                                                               .Where(c => c.ListaDistribucion.nombre == CatalogosCodigos.DEFAULT_LISTADISTRIBUCION).ToList();
                    if (correos.Count > 0)
                    {


                        /* ES: Envio de Excel al Correo*/
                        MailMessage message = new MailMessage();
                        message.Subject = "PMDIS: Datos Colaboradores";
                        foreach (var item in correos)
                        {
                            message.To.Add(item.correo);


                        }

                        excel.SaveAs(new FileInfo(System.Web.HttpContext.Current.Server.MapPath("~/Views/ArchivosColaboradores/AltaEmergentesColaboradores" + aux + ".xlsx")));

                        string url = System.Web.HttpContext.Current.Server.MapPath("~/Views/ArchivosColaboradores/AltaEmergentesColaboradores" + aux + ".xlsx");
                        if (File.Exists((string)url))
                        {
                            message.Attachments.Add(new Attachment(url));
                        }
                        /*********/
                        await _correoservice.SendWithFilesAsync(message);


                    }
                    /* System.IO.FileInfo filename = new System.IO.FileInfo(@"C:\CPP\Colaboradores\Altas\AltaColaboradores" + aux + ".xlsx");
                    excel.SaveAs(filename);
                    */
                }

                return "OK";
            }

        }

        #region ES: Generacion Word
        public string GenerarWord(int id, DateTime x, DateTime y)
        {

            var empresaparametro = _parametrorepository.GetAll().Where(c => c.Codigo == "_empresa_cpp_certificado").Select(c => c.Nombre).FirstOrDefault();

            string fullMonthName = DateTime.Now.ToString("MMMM", CultureInfo.CreateSpecificCulture("es"));
            var fecha = DateTime.Now.Day + " de " + fullMonthName + " de " + DateTime.Now.Year;

            string fullMonthNameDesde = x.ToString("MMMM", CultureInfo.CreateSpecificCulture("es"));


            string fullMonthNameHasta = y.ToString("MMMM", CultureInfo.CreateSpecificCulture("es"));

            ///COLABORADOR
            var colaborador = this.GetColaborador(id);

            /// 
            ///

            var usuario = "";
            decimal sueldo = 0;

            var empresa = "";
            if (empresaparametro != null)
            {
                empresa = empresaparametro;
            }
            var nombres = "";
            var tipoidentificacion = "";
            var identificacion = "";
            var cargo = "";
            string genero = "";
            var fechadesde = x.Day + " de " + fullMonthNameDesde + " de " + x.Year;
            var fechahasta = y.Day + " de " + fullMonthNameHasta + " de " + y.Year;
            if (colaborador != null)
            {
                nombres = colaborador.primer_apellido + " " + colaborador.segundo_apellido + " " + colaborador.nombres;
                if (colaborador.catalogo_tipo_identificacion_id != null)
                {
                    tipoidentificacion = _ccatalogorepository.Get(colaborador.catalogo_tipo_identificacion_id.Value).nombre;
                }

                identificacion = colaborador.numero_identificacion;
                if (colaborador.catalogo_cargo_id != null)
                {
                    cargo = _ccatalogorepository.Get(colaborador.catalogo_cargo_id.Value).nombre;
                }
                if (colaborador.remuneracion_mensual > 0)
                {
                    sueldo = colaborador.remuneracion_mensual.Value;
                }
                if (colaborador.catalogo_genero_id > 0)
                {
                    genero = _catalogoRepository.GetCatalogo(colaborador.catalogo_genero_id.Value) != null && _catalogoRepository.GetCatalogo(colaborador.catalogo_genero_id.Value).codigo == "VAR" ? "El señor" : "La señora";
                }
            }
            string user = System.Web.HttpContext.Current.User.Identity.Name.ToString();

            var usuarioencontrado = _usuarioRepository.GetAll().Where(c => c.Cuenta == user).FirstOrDefault();
            if (usuarioencontrado != null && usuarioencontrado.Id > 0)
            {
                usuario = usuarioencontrado.NombresCompletos;
            }




            // VERIFICAR SI ES ACTIVO E INACTIVO

            if (colaborador != null && colaborador.estado == "ACTIVO")
            {
                string filename = System.Web.HttpContext.Current.Server.MapPath("~/Views/PlantillaWord/Certificado_Trabajo_2019_Activos.docx");
                if (File.Exists((string)filename))
                {
                    Random a = new Random();
                    var valor = a.Next(1, 100000);
                    string salida = System.Web.HttpContext.Current.Server.MapPath("~/Views/PlantillaWord/Certificados/Certificado-" + nombres + "-" + valor + ".docx");
                    using (var plantilla = DocX.Load(filename))
                    {
                        var document = DocX.Create(salida);
                        document.InsertDocument(plantilla);
                        document.ReplaceText("{fecha}", fecha);
                        document.ReplaceText("{tipoidentificacion}", tipoidentificacion.ToLower());

                        document.ReplaceText("{identificacion}", identificacion);
                        document.ReplaceText("{generocuerpo}", genero.ToLower());
                        document.ReplaceText("{generoinicial}", genero);
                        document.ReplaceText("{nombresyapellidoscolaborador}", nombres.ToUpper());
                        document.ReplaceText("{nombreempresa}", empresa);
                        document.ReplaceText("{cargo}", cargo.Length > 0 ? cargo.ToUpper() : "XXXXXX");
                        document.ReplaceText("{fechadesde}", fechadesde);
                        document.ReplaceText("{fechahasta}", fechahasta);
                        document.ReplaceText("{valorenletras}", this.ConvertirNumerosALetras(Convert.ToDouble(sueldo)));
                        document.ReplaceText("{valorennumeros}", String.Format(CultureInfo.InvariantCulture, "{0:0,0.00}", sueldo));
                        document.ReplaceText("{nombreapellidousuario}", usuario);
                        document.ReplaceText("{tcuser}", user);

                        document.Save();
                        return salida;
                    }

                }
                else
                {
                    return "";
                }

            }
            if (colaborador != null && colaborador.estado == "INACTIVO")
            {
                string filename = System.Web.HttpContext.Current.Server.MapPath("~/Views/PlantillaWord/Certificado_Trabajo_2019_Inactivos.docx");
                if (File.Exists((string)filename))
                {
                    Random a = new Random();
                    var valor = a.Next(1, 100000);
                    string salida = System.Web.HttpContext.Current.Server.MapPath("~/Views/PlantillaWord/Certificados/Certificado-" + nombres + "-" + valor + ".docx");
                    using (var plantilla = DocX.Load(filename))
                    {
                        var document = DocX.Create(salida);
                        document.InsertDocument(plantilla);
                        document.ReplaceText("{fecha}", fecha);
                        document.ReplaceText("{tipoidentificacion}", tipoidentificacion.ToLower());

                        document.ReplaceText("{identificacion}", identificacion);
                        document.ReplaceText("{generocuerpo}", genero.ToLower());
                        document.ReplaceText("{generoinicial}", genero);
                        document.ReplaceText("{nombresyapellidoscolaborador}", nombres.ToUpper());
                        document.ReplaceText("{nombreempresa}", empresa);
                        document.ReplaceText("{cargo}", cargo.Length > 0 ? cargo.ToUpper() : "pendiente");
                        document.ReplaceText("{fechadesde}", fechadesde);
                        document.ReplaceText("{fechahasta}", fechahasta);
                        document.ReplaceText("{valorenletras}", this.ConvertirNumerosALetras(Convert.ToDouble(sueldo)));
                        document.ReplaceText("{valorennumeros}", String.Format(CultureInfo.InvariantCulture, "{0:0,0.00}", sueldo));
                        document.ReplaceText("{nombreapellidousuario}", usuario);
                        document.ReplaceText("{tcuser}", user);

                        document.Save();
                        return salida;
                    }

                }
                else
                {
                    return "";
                }

            }
            return "OK";

        }

        #endregion
        public List<ColaboradoresDto> GetColaboradorPorFiltros(string numero, string nombres, int grupoPersonal)
        {


            var e = 1;
            var query = Repository.GetAll().Where(a => a.vigente == true && a.es_externo == false && a.estado != RRHHCodigos.ESTADO_INACTIVO && a.estado != RRHHCodigos.ESTADO_ALTAANULADA);

            if (numero != "")
            {
                query = query.Where(x => x.numero_identificacion.StartsWith(numero));
            }

            if (grupoPersonal != 0)
            {
                query = query.Where(x => x.catalogo_grupo_personal_id == grupoPersonal);
            }

            if (nombres != "")
            {
                query = query.Where(x =>
                                    x.nombres_apellidos.ToUpper().Contains(nombres) ||
                                    x.nombres.ToUpper().Contains(nombres) ||
                                    x.primer_apellido.ToUpper().Contains(nombres) ||
                                    x.segundo_apellido.ToUpper().Contains(nombres) ||
                                    (x.primer_apellido + " " + x.segundo_apellido).ToUpper().Contains(nombres)
                                    );
            }


            var colaborador = (from d in query
                               select new ColaboradoresDto
                               {
                                   Id = d.Id,
                                   fecha_ingreso = d.fecha_ingreso,
                             
                                   catalogo_tipo_identificacion_id = d.catalogo_tipo_identificacion_id,
                                   numero_identificacion = d.numero_identificacion,
                                   primer_apellido = d.primer_apellido,
                                   segundo_apellido = d.segundo_apellido,
                                   nombres = d.nombres,
                                   fecha_nacimiento = d.fecha_nacimiento,
                                   catalogo_genero_id = d.catalogo_genero_id,
                                   PaisId = d.PaisId,
                                   pais_pais_nacimiento_id = d.pais_pais_nacimiento_id,
                                   ContactoId = d.ContactoId,
                                   catalogo_destino_estancia_id = d.catalogo_destino_estancia_id,
                                   vigente = d.vigente,
                                   validacion_cedula = d.validacion_cedula,
                                   nombres_apellidos = d.primer_apellido + " " + d.segundo_apellido + " " + d.nombres,
                                   nombre_grupo_personal = d.GrupoPersonal != null ? d.GrupoPersonal.nombre : "",
                                   numero_legajo_temporal = d.numero_legajo_temporal,
                                   numero_legajo_definitivo = d.numero_legajo_definitivo,
                                   estado = d.estado,
                                   nombreestancia = "",
                                   fechavigenciacolaboradorqr = "",
                                   serviciosvigentes = "",
                                   tienereservaactiva = "",
                                   empleado_id_sap_local = d.empleado_id_sap_local,
                                   empleado_id_sap = d.empleado_id_sap
                               }).ToList();

            foreach (var i in colaborador)
            {
                i.nro = e++;

                i.apellidos_nombres = i.primer_apellido + ' ' + i.segundo_apellido;

                var catalogoIdentificacion = _catalogoRepository.GetCatalogo(i.catalogo_tipo_identificacion_id.Value);
                i.nombre_identificacion = catalogoIdentificacion.nombre;

                if (i.vigente)
                {
                    i.nombre_estado = "Activo";
                }
                else
                {
                    i.nombre_estado = "Inactivo";
                }
                //Catalogo Destino
                if (i.catalogo_destino_estancia_id > 0)
                {
                    i.nombreestancia = _catalogoRepository.GetCatalogo(i.catalogo_destino_estancia_id.Value) != null ? _catalogoRepository.GetCatalogo(i.catalogo_destino_estancia_id.Value).nombre : "";
                }
                i.fechavigenciacolaboradorqr = this.getParametroPorCodigo("PARAMETRO.FECHA.CADUCIDAD.QR").Length > 0 ?

                    DateTime.Now.AddDays(Int32.Parse(this.getParametroPorCodigo("PARAMETRO.FECHA.CADUCIDAD.QR"))).ToString("dd/MM/yyyy HH:mm") : "";

                i.serviciosvigentes = this.ServiciosColaborado(i.Id);
                i.tienereservaactiva = this.colaboradortienereservasactivas(i.Id);
                i.fechaIngresoFormat = i.fecha_ingreso.HasValue ? i.fecha_ingreso.GetValueOrDefault().ToShortDateString() : "";
            }

            return colaborador;

        }

        public List<ColaboradoresDto> GetFiltrosBajas(string numero, string nombres, string estado)
        {


            var e = 1;
            var query = Repository.GetAll().Where(a => a.vigente == true && a.es_externo == false);

            if (numero != "")
            {
                query = query.Where(x => x.numero_identificacion.Contains(numero));
            }

            if (estado != "")
            {
                query = query.Where(x => x.estado == estado && x.estado!=RRHHCodigos.ESTADO_ALTAANULADA);
            }

            if (nombres != "")
            {
                query = query.Where(x => x.nombres.ToUpper().Contains(nombres.ToUpper()) || x.primer_apellido.ToUpper().Contains(nombres.ToUpper()) || x.segundo_apellido.ToUpper().Contains(nombres.ToUpper()) || (x.primer_apellido + " " + x.segundo_apellido).ToUpper().Contains(nombres));
            }


            var colaborador = (from d in query
                               select new ColaboradoresDto
                               {
                                   Id = d.Id,
                                   fecha_ingreso = d.fecha_ingreso,
                                   catalogo_tipo_identificacion_id = d.catalogo_tipo_identificacion_id,
                                   numero_identificacion = d.numero_identificacion,
                                   primer_apellido = d.primer_apellido,
                                   segundo_apellido = d.segundo_apellido,
                                   nombres = d.nombres,
                                   fecha_nacimiento = d.fecha_nacimiento,
                                   catalogo_genero_id = d.catalogo_genero_id,
                                   PaisId = d.PaisId,
                                   pais_pais_nacimiento_id = d.pais_pais_nacimiento_id,
                                   ContactoId = d.ContactoId,
                                   catalogo_destino_estancia_id = d.catalogo_destino_estancia_id,
                                   vigente = d.vigente,
                                   validacion_cedula = d.validacion_cedula,
                                   nombres_apellidos = d.nombres_apellidos,
                                   nombre_grupo_personal = d.EncargadoPersonal.nombre,
                                   numero_legajo_temporal = d.numero_legajo_temporal,
                                   estado = d.estado,
                                   nombre_identificacion = d.TipoIdentificacion.nombre,

                                   catalogo_encargado_personal_id = d.catalogo_encargado_personal_id
                               }).ToList();

            foreach (var i in colaborador)
            {
                i.nro = e++;

                i.apellidos_nombres = i.primer_apellido + ' ' + i.segundo_apellido;

                if (i.estado == "INACTIVO")
                {
                    var b = _bajasService.GetBaja(i.Id);
                    if (b != null)
                    {
                        i.baja_id = b.Id;
                        i.catalogo_motivo_baja_id = b.catalogo_motivo_baja_id;
                        i.fecha_baja = b.fecha_baja;
                        i.estado_baja = b.estado;
                        i.motivo_baja = b.motivo_baja;
                        i.liquidado = b.fecha_pago_liquidacion;
                        i.estaLiquidado = b.fecha_pago_liquidacion == null ? "NO" : "SI";


                    }


                }
            }

            return colaborador;

        }

        public List<ColaboradoresDto> GetUusuarioFiltros(string numero, string nombres)
        {


            var e = 1;
            var query = Repository.GetAll().Where(a => a.vigente == true && a.es_externo == true);

            if (numero != "")
            {
                query = query.Where(x => x.numero_identificacion.StartsWith(numero) || x.numero_identificacion == numero);
            }

            if (nombres != "")
            {
                query = query.Where(x => x.nombres.ToUpper().Contains(nombres.ToUpper()) || x.primer_apellido.ToUpper().Contains(nombres.ToUpper()) || x.segundo_apellido.ToUpper().Contains(nombres.ToUpper()) || (x.primer_apellido + " " + x.segundo_apellido).ToUpper().Contains(nombres));
            }


            var colaborador = (from d in query
                               select new ColaboradoresDto
                               {
                                   Id = d.Id,
                                   fecha_ingreso = d.fecha_ingreso,
                                   catalogo_tipo_identificacion_id = d.catalogo_tipo_identificacion_id,
                                   numero_identificacion = d.numero_identificacion,
                                   primer_apellido = d.primer_apellido,
                                   segundo_apellido = d.segundo_apellido,
                                   nombres = d.nombres,
                                   fecha_nacimiento = d.fecha_nacimiento,
                                   catalogo_genero_id = d.catalogo_genero_id,
                                   PaisId = d.PaisId,
                                   pais_pais_nacimiento_id = d.pais_pais_nacimiento_id,
                                   ContactoId = d.ContactoId,
                                   catalogo_destino_estancia_id = d.catalogo_destino_estancia_id,
                                   vigente = d.vigente,
                                   validacion_cedula = d.validacion_cedula,
                                   nombres_apellidos = d.nombres_apellidos,
                                   nombre_grupo_personal = d.GrupoPersonal.nombre,
                                   numero_legajo_temporal = d.numero_legajo_temporal,
                                   nombre_identificacion = d.TipoIdentificacion.nombre,
                                   estado = d.estado,
                                   es_visita = d.es_visita
                               }).ToList();

            foreach (var i in colaborador)
            {
                i.nro = e++;
                i.nombreTipo = i.es_visita ? "VISITA" : "TERCERO";

                i.apellidos_nombres = i.primer_apellido + ' ' + i.segundo_apellido;
            }

            return colaborador;

        }

        public async Task EnviarNotificacionQRAsync(string fechaCaducidad, string correo, string asunto, string Attach)
        {
            try
            {

                /* GENERAR TAG IMAGEN */
                var img = "<img src='data: image / jpeg; base64," + Attach + "' />";

                var body = "<p>Se ha generado el c&oacute;digo QR considerar que ser&aacute; vigente hasta:&nbsp; " + fechaCaducidad + "</p> <p>&nbsp;</p> <p>" + img + "</p> ";

                var msg = new IdentityMessage();
                msg.Destination = correo;
                msg.Subject = asunto;
                msg.Body = body;
                await EmailService.SendAsync(msg);
            }
            catch (Exception ex)
            {
                var result = ManejadorExcepciones.HandleException(ex);
            }

        }

        public string getParametroPorCodigo(string codigo)
        {
            var result = _parametroRepository.GetAll().Where(d => d.Codigo == codigo).Select(d => d.Valor).FirstOrDefault();

            return result != null ? result : " ";
        }

        public ColaboradoresDto GetInfoColaboradorWS(int tipoIdentificacion, string cedula, string huella_dactilar)
        {
            var d = Repository.GetAll().Where(c => c.catalogo_tipo_identificacion_id == tipoIdentificacion
            && c.numero_identificacion == cedula).FirstOrDefault();

            if (d != null)
            {
                ColaboradoresDto colaborador = new ColaboradoresDto()
                {
                    Id = d.Id,
                    catalogo_tipo_identificacion_id = d.catalogo_tipo_identificacion_id,
                    numero_identificacion = d.numero_identificacion,
                    primer_apellido = d.primer_apellido,
                    segundo_apellido = d.segundo_apellido,
                    nombres = d.nombres,
                    nombres_apellidos = d.nombres_apellidos,
                    fecha_nacimiento = d.fecha_nacimiento,
                    catalogo_genero_id = d.catalogo_genero_id,
                    PaisId = d.PaisId,
                    pais_pais_nacimiento_id = d.pais_pais_nacimiento_id,
                    ContactoId = d.ContactoId,
                    catalogo_destino_estancia_id = d.catalogo_destino_estancia_id,
                    vigente = d.vigente,
                    catalogo_etnia_id = d.catalogo_etnia_id,
                    numero_hijos = d.numero_hijos,
                    catalogo_estado_civil_id = d.catalogo_estado_civil_id,
                    fecha_matrimonio = d.fecha_matrimonio,
                    viene_registro_civil = d.viene_registro_civil,
                    fecha_registro_civil = d.fecha_registro_civil,
                    catalogo_formacion_educativa_id = d.catalogo_formacion_educativa_id,
                    codigo_dactilar = d.codigo_dactilar,
                    empleado_id_sap = d.empleado_id_sap,
                    estado = d.estado
                };


                var f = _formacionEducativaService.GetFormacion(d.Id);
                if (f != null)
                {
                    colaborador.institucion_educativa = f.institucion_educativa;
                    colaborador.catalogo_titulo_id = f.catalogo_titulo_id;
                    if (f.formacion.Length > 0)
                    {
                        colaborador.formacion = int.Parse(f.formacion);
                    }
                    colaborador.fecha_registro_senecyt = f.fecha_registro_senecyt;
                }


                return colaborador;

            }

            return null;
        }

        public Archivo GetArchivo(int id)
        {
            var archivo = _archivoRepository.GetAll().Where(e => e.Id == id).FirstOrDefault();
            return archivo;
        }

        public List<ColaboradoresDto> GetUsuariosExternos()
        {

            var e = 1;
            var week = DateTime.Now.AddDays(-7);
            var date = DateTime.Now;
            var query = Repository.GetAll().Where(c => c.vigente == true && c.es_externo == true && c.estado == "ACTIVO");

            var colaborador = (from d in query
                               where d.CreationTime >= week && d.CreationTime <= date
                               select new ColaboradoresDto
                               {
                                   Id = d.Id,
                                   fecha_ingreso = d.fecha_ingreso,
                                   catalogo_tipo_identificacion_id = d.catalogo_tipo_identificacion_id,
                                   numero_identificacion = d.numero_identificacion,
                                   primer_apellido = d.primer_apellido,
                                   segundo_apellido = d.segundo_apellido,
                                   nombres = d.nombres,
                                   fecha_nacimiento = d.fecha_nacimiento,
                                   catalogo_genero_id = d.catalogo_genero_id,
                                   PaisId = d.PaisId,
                                   pais_pais_nacimiento_id = d.pais_pais_nacimiento_id,
                                   ContactoId = d.ContactoId,
                                   catalogo_destino_estancia_id = d.catalogo_destino_estancia_id,
                                   vigente = d.vigente,
                                   estado = d.estado,
                                   codigo_dactilar = d.codigo_dactilar,
                                   nombre_identificacion = d.TipoIdentificacion.nombre,
                                   nombre_destino = d.DestinoEstancia.nombre,
                                   estado_colaborador = d.estado
                               }).ToList();

            foreach (var i in colaborador)
            {
                i.nro = e++;

                i.apellidos_nombres = i.primer_apellido + ' ' + i.segundo_apellido;

            }

            return colaborador;
        }

        public string UniqueUsuarioExterno(string nro)
        {
            var user = Repository.GetAll().Where(d => d.numero_identificacion == nro).FirstOrDefault();
            if (user != null)
            {
                if (user.es_externo == true && user.vigente == true && user.estado != RRHHCodigos.ESTADO_INACTIVO)
                {
                    return "Usuario Externo ya existe";
                }
                else if (user.vigente == true && user.estado != RRHHCodigos.ESTADO_INACTIVO)
                {
                    return "Se encuentra registrado como Colaborador";
                }
                else
                {
                    var result = JsonConvert.SerializeObject(user);
                    return result;
                }
            }
            else
            {
                return "NO";
            }
        }

        public ColaboradoresDto GetUsuarioExterno(int Id)
        {
            var d = Repository.Get(Id);

            ColaboradoresDto colaborador = new ColaboradoresDto()
            {
                Id = d.Id,
                catalogo_tipo_identificacion_id = d.catalogo_tipo_identificacion_id,
                numero_identificacion = d.numero_identificacion,
                primer_apellido = d.primer_apellido,
                segundo_apellido = d.segundo_apellido,
                nombres = d.nombres,
                fecha_nacimiento = d.fecha_nacimiento,
                catalogo_genero_id = d.catalogo_genero_id,
                PaisId = d.PaisId,
                pais_pais_nacimiento_id = d.pais_pais_nacimiento_id,
                catalogo_estado_civil_id = d.catalogo_estado_civil_id,
                fecha_matrimonio = d.fecha_matrimonio,
                vigente = d.vigente,
                ContactoId = d.ContactoId,
                Contacto = d.Contacto,
                estado = d.estado,
                nombres_apellidos = d.nombres_apellidos,
                codigo_dactilar = d.codigo_dactilar,
                viene_registro_civil = d.viene_registro_civil,
                fecha_registro_civil = d.fecha_registro_civil,
                es_externo = d.es_externo,
                catalogo_destino_estancia_id = d.catalogo_destino_estancia_id,
                catalogo_grupo_personal_id = d.catalogo_grupo_personal_id,
                nombre_identificacion = d.TipoIdentificacion.nombre,
                validacion_cedula = d.validacion_cedula,
                nombreestancia = d.DestinoEstancia.nombre,
            };

            colaborador.fechavigenciacolaboradorqr = this.getParametroPorCodigo("PARAMETRO.FECHA.CADUCIDAD.QR").Length > 0 ?

                    DateTime.Now.AddDays(Int32.Parse(this.getParametroPorCodigo("PARAMETRO.FECHA.CADUCIDAD.QR"))).ToString("dd/MM/yyyy HH:mm") : "";

            colaborador.serviciosvigentes = this.ServiciosColaborado(colaborador.Id);
            colaborador.tienereservaactiva = this.colaboradortienereservasactivas(colaborador.Id);

            return colaborador;
        }

        public List<ColaboradoresDto> GetListaResponsables(string nombre)
        {
            var e = 1;
            var query = Repository.GetAll().Where(c => c.vigente == true && c.es_externo == false && c.estado != RRHHCodigos.ESTADO_INACTIVO);

            query = query.Where(x => x.nombres.ToUpper().StartsWith(nombre.ToUpper()) || x.primer_apellido.ToUpper().StartsWith(nombre.ToUpper()) || x.segundo_apellido.ToUpper().StartsWith(nombre.ToUpper()) || x.nombres_apellidos.StartsWith(nombre));

            if (query != null)
            {
                var colaborador = (from d in query
                                   select new ColaboradoresDto
                                   {
                                       Id = d.Id,
                                       nombres_apellidos = d.nombres_apellidos,
                                       primer_apellido = d.primer_apellido,
                                       segundo_apellido = d.segundo_apellido,
                                       nombres = d.nombres,
                                       vigente = d.vigente,
                                       estado = d.estado,
                                   }).ToList();

                foreach (var i in colaborador)
                {
                    i.nro = e++;
                }

                return colaborador;
            }

            return null;
        }

        public List<ColaboradoresDto> ConsultaUsuarioExternoPorIdentificacion(int tipoIdentificacion, string numero)
        {
            var e = 1;
            var query = Repository.GetAll().Where(c => c.vigente == true && c.catalogo_tipo_identificacion_id == tipoIdentificacion && c.numero_identificacion == numero && c.es_externo == true);


            if (query != null)
            {
                var colaborador = (from d in query
                                   select new ColaboradoresDto
                                   {
                                       Id = d.Id,
                                       catalogo_tipo_identificacion_id = d.catalogo_tipo_identificacion_id,
                                       numero_identificacion = d.numero_identificacion,
                                       nombres_apellidos = d.nombres_apellidos,
                                       primer_apellido = d.primer_apellido,
                                       segundo_apellido = d.segundo_apellido,
                                       nombres = d.nombres,
                                       vigente = d.vigente,
                                       estado = d.estado,
                                   }).ToList();

                foreach (var c in colaborador)
                {
                    c.nro = e++;
                    c.apellidos_nombres = c.primer_apellido + ' ' + c.segundo_apellido;
                    var catalogoIdentificacion = _catalogoRepository.GetCatalogo(c.catalogo_tipo_identificacion_id.Value);
                    c.nombre_identificacion = catalogoIdentificacion.nombre;
                }


                return colaborador;
            }

            return null;
        }

        public List<ColaboradorResponsabilidadDto> GetColaboradorUsuario(string numero)
        {
            var colaborador = Repository.GetAll().Where(a => a.numero_identificacion == numero && a.vigente == true).FirstOrDefault();
            if (colaborador != null)
            {
                var responsabilidades = _responsabilidadService.GetResponsabilidadesPorColaborador(colaborador.Id);

                return responsabilidades;
            }
            return null;
        }


        public List<string> ExcelCargaMasiva(HttpPostedFileBase UploadedFile)
        {
            var TCTiposIdentificacion = _tipoCatalogoService.Single(c => c.codigo == RRHHCodigos.TIPO_IDENTIFICACION && c.vigente == true);
            var CatalogoTiposIdentificacion = _ccatalogorepository.GetAll().Where(c => c.TipoCatalogoId == TCTiposIdentificacion.Id && c.vigente == true);

            var TCGenero = _tipoCatalogoService.Single(c => c.codigo == RRHHCodigos.GENERO && c.vigente == true);
            var CatalogosGenero = _ccatalogorepository.GetAll().Where(c => c.TipoCatalogoId == TCGenero.Id && c.vigente == true);

            var TCEtnia = _tipoCatalogoService.Single(c => c.codigo == RRHHCodigos.ETNIA && c.vigente == true);
            var CatalogosEtnia = _ccatalogorepository.GetAll().Where(c => c.TipoCatalogoId == TCEtnia.Id && c.vigente == true);

            var TCNacionalidades = _tipoCatalogoService.Single(c => c.codigo == RRHHCodigos.NACIONALIDAD && c.vigente == true);
            var CatalogosNacionalidades = _ccatalogorepository.GetAll().Where(c => c.TipoCatalogoId == TCNacionalidades.Id && c.vigente == true);

            var TCDestino = _tipoCatalogoService.Single(c => c.codigo == RRHHCodigos.DESTINO && c.vigente == true);
            var CatalogosDestino = _ccatalogorepository.GetAll().Where(c => c.TipoCatalogoId == TCDestino.Id && c.vigente == true);

            var TCEncargadoPersonal = _tipoCatalogoService.Single(c => c.codigo == RRHHCodigos.ENCARGADO_PERSONAL && c.vigente == true);
            var CatalogosEncargadoPersonal = _ccatalogorepository.GetAll().Where(c => c.TipoCatalogoId == TCEncargadoPersonal.Id && c.vigente == true);

            var TCSector = _tipoCatalogoService.Single(c => c.codigo == RRHHCodigos.SECTOR && c.vigente == true);
            var CatalogosSector = _ccatalogorepository.GetAll().Where(c => c.TipoCatalogoId == TCSector.Id && c.vigente == true);

            var TCCargo = _tipoCatalogoService.Single(c => c.codigo == RRHHCodigos.CARGO && c.vigente == true);
            var CatalogosCargo = _ccatalogorepository.GetAll().Where(c => c.TipoCatalogoId == TCCargo.Id && c.vigente == true);

            var paises = _paisrepository.GetAll().Where(c => c.vigente == true);

            var parroquias = _parroquiaService.GetParroquias();
            var comunidades = _comunidadrepository.GetAll().Where(c => c.vigente == true);

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

                var noOfRow = workSheet.Dimension.End.Row;

                for (int rowIterator = 3; rowIterator <= noOfRow; rowIterator++)
                {
                    ColaboradoresDto colaborador = new ColaboradoresDto();
                    //Validar Tipo de Identificación
                    if (workSheet.Cells[rowIterator, 2].Value == null)
                    {
                        errores.Add("Obligatorio Tipo Identificación fila: " + rowIterator);
                    }
                    else
                    {
                        var tipoIdentificacion = (workSheet.Cells[rowIterator, 2].Value).ToString();
                        var existe = false;
                        foreach (var e in CatalogoTiposIdentificacion)
                        {
                            if (e.nombre == tipoIdentificacion)
                            {
                                existe = true;
                                colaborador.catalogo_tipo_identificacion_id = e.Id;
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
                    //Validar Código Dactilar
                    if (workSheet.Cells[rowIterator, 4].Value == null)
                    {
                        errores.Add("Obligatorio Código Dactilar fila: " + rowIterator);
                    }
                    else
                    {
                        var codigoDactilar = (workSheet.Cells[rowIterator, 4].Value).ToString();
                        colaborador.codigo_dactilar = codigoDactilar;
                    }
                    //Validar Primer Apellido
                    if (workSheet.Cells[rowIterator, 5].Value == null)
                    {
                        errores.Add("Obligatorio Primer Apellido fila: " + rowIterator);
                    }
                    else
                    {
                        var primerApellido = (workSheet.Cells[rowIterator, 5].Value).ToString();
                        colaborador.primer_apellido = primerApellido;
                    }
                    //Validar Segundo Apellido
                    if (workSheet.Cells[rowIterator, 6].Value == null)
                    {
                        errores.Add("Obligatorio Segundo Apellido fila: " + rowIterator);
                    }
                    else
                    {
                        var segundoApellido = (workSheet.Cells[rowIterator, 6].Value).ToString();
                        colaborador.segundo_apellido = segundoApellido;
                    }
                    //Validar Nombres
                    if (workSheet.Cells[rowIterator, 7].Value == null)
                    {
                        errores.Add("Obligatorio Nombres fila: " + rowIterator);
                    }
                    else
                    {
                        var nombres = (workSheet.Cells[rowIterator, 7].Value).ToString();
                        colaborador.nombres = nombres;
                    }
                    //Validar Fecha de Nacimiento
                    if (workSheet.Cells[rowIterator, 8].Value == null)
                    {
                        errores.Add("Obligatorio Fecha Nacimiento fila: " + rowIterator);
                    }
                    else
                    {
                        var fechaNacimiento = (workSheet.Cells[rowIterator, 8].Value).ToString();
                        colaborador.fecha_nacimiento = Convert.ToDateTime(fechaNacimiento);
                        //var formatFechaNacimiento = String.Format("{0:dd/MM/yyyy}", fechaNacimiento);
                        //System.Diagnostics.Debug.WriteLine(formatFechaNacimiento, fechaNacimiento);
                    }
                    //Validar Género
                    if (workSheet.Cells[rowIterator, 9].Value == null)
                    {
                        errores.Add("Obligatorio Género fila: " + rowIterator);
                    }
                    else
                    {
                        var genero = (workSheet.Cells[rowIterator, 9].Value).ToString();
                        var existe = false;
                        foreach (var e in CatalogosGenero)
                        {
                            if (e.nombre == genero)
                            {
                                existe = true;
                                colaborador.catalogo_genero_id = e.Id;
                                break;
                            }
                        }
                        if (existe == false)
                        {
                            errores.Add("Error Género fila: " + rowIterator + " no existe");
                        }
                    }
                    //Validar Etnia
                    var etnia = (workSheet.Cells[rowIterator, 10].Value ?? "").ToString();
                    if (etnia != "")
                    {
                        var existe = false;
                        foreach (var e in CatalogosEtnia)
                        {
                            if (e.nombre == etnia)
                            {
                                existe = true;
                                colaborador.catalogo_etnia_id = e.Id;
                                break;
                            }
                        }
                        if (existe == false)
                        {
                            errores.Add("Error Etnia fila: " + rowIterator + " no existe");
                        }
                    }

                    //Validar Pais de Nacimiento
                    if (workSheet.Cells[rowIterator, 11].Value == null)
                    {
                        errores.Add("Obligatorio Pais Nacimiento fila: " + rowIterator);
                    }
                    else
                    {
                        var paisNacimiento = (workSheet.Cells[rowIterator, 11].Value).ToString();
                        var existe = false;
                        foreach (var e in paises)
                        {
                            if (e.nombre == paisNacimiento)
                            {
                                existe = true;
                                colaborador.PaisId = e.Id;
                                break;
                            }
                        }
                        if (existe == false)
                        {
                            errores.Add("Error Pais de Nacimiento fila: " + rowIterator + " no existe");
                        }
                    }
                    //Validar Nacionalidad
                    var nacionalidad = (workSheet.Cells[rowIterator, 12].Value ?? "").ToString();
                    if (nacionalidad != "")
                    {
                        var existe = false;
                        foreach (var e in CatalogosNacionalidades)
                        {
                            if (e.nombre == nacionalidad)
                            {
                                existe = true;
                                colaborador.pais_pais_nacimiento_id = e.Id;
                                break;
                            }
                        }
                        if (existe == false)
                        {
                            errores.Add("Error Nacionalidad fila: " + rowIterator + " no existe");
                        }
                    }

                    //Validar Fecha de Ingreso
                    if (workSheet.Cells[rowIterator, 13].Value == null)
                    {
                        errores.Add("Obligatorio Fecha Ingreso fila: " + rowIterator);
                    }
                    else
                    {
                        var fechaIngreso = (workSheet.Cells[rowIterator, 13].Value).ToString();
                        colaborador.fecha_ingreso = Convert.ToDateTime(fechaIngreso);
                    }

                    //Validar Destino
                    if (workSheet.Cells[rowIterator, 14].Value == null)
                    {
                        errores.Add("Obligatorio Destino fila: " + rowIterator);
                    }
                    else
                    {
                        var destino = (workSheet.Cells[rowIterator, 14].Value).ToString();
                        var existe = false;
                        foreach (var e in CatalogosDestino)
                        {
                            if (e.nombre == destino)
                            {
                                existe = true;
                                colaborador.catalogo_destino_estancia_id = e.Id;
                                break;
                            }
                        }
                        if (existe == false)
                        {
                            errores.Add("Error Destino fila: " + rowIterator + " no existe");
                        }
                    }

                    var encargadoPersonal = (workSheet.Cells[rowIterator, 15].Value ?? "").ToString();
                    if (encargadoPersonal != "")
                    {
                        var existe = false;
                        foreach (var e in CatalogosEncargadoPersonal)
                        {
                            if (e.nombre == encargadoPersonal)
                            {
                                existe = true;
                                colaborador.catalogo_encargado_personal_id = e.Id;
                                break;
                            }
                        }
                        if (existe == false)
                        {
                            errores.Add("Error Encargado Personal fila: " + rowIterator + " no existe");
                        }
                    }

                    //Validar Sector
                    if (workSheet.Cells[rowIterator, 16].Value == null)
                    {
                        errores.Add("Obligatorio Área/Sector fila: " + rowIterator);
                    }
                    else
                    {
                        var sector = (workSheet.Cells[rowIterator, 16].Value).ToString();
                        var existe = false;
                        foreach (var e in CatalogosSector)
                        {
                            if (e.nombre == sector)
                            {
                                existe = true;
                                colaborador.catalogo_sector_id = e.Id;
                                break;
                            }
                        }
                        if (existe == false)
                        {
                            errores.Add("Error Área/Sector fila: " + rowIterator + " no existe");
                        }
                    }

                    //Validar Cargo
                    if (workSheet.Cells[rowIterator, 17].Value == null)
                    {
                        errores.Add("Obligatorio Cargo fila: " + rowIterator);
                    }
                    else
                    {
                        var cargo = (workSheet.Cells[rowIterator, 17].Value).ToString();
                        var existe = false;
                        foreach (var e in CatalogosCargo)
                        {
                            if (e.nombre == cargo)
                            {
                                existe = true;
                                colaborador.catalogo_cargo_id = e.Id;
                                break;
                            }
                        }
                        if (existe == false)
                        {
                            errores.Add("Error Cargo fila: " + rowIterator + " no existe");
                        }
                    }
                    //Validar Parroquia
                    if (workSheet.Cells[rowIterator, 18].Value == null)
                    {
                        errores.Add("Obligatorio Parroquia fila: " + rowIterator);
                    }
                    else
                    {
                        var parroquia = (workSheet.Cells[rowIterator, 18].Value).ToString();
                        var existe = false;
                        foreach (var e in parroquias)
                        {
                            if (e.nombre == parroquia)
                            {
                                existe = true;
                                colaborador.parroquia = e.Id;
                                break;
                            }
                        }
                        if (existe == false)
                        {
                            errores.Add("Error Parroquia fila: " + rowIterator + " no existe");
                        }
                    }
                    //Validar Comunidad
                    var comunidad = (workSheet.Cells[rowIterator, 19].Value ?? "").ToString();
                    if (comunidad != "")
                    {
                        var existe = false;
                        foreach (var e in comunidades)
                        {
                            if (e.nombre == comunidad)
                            {
                                existe = true;
                                colaborador.comunidad = e.Id;
                                break;
                            }
                        }
                        if (existe == false)
                        {
                            errores.Add("Error Comunidad fila: " + rowIterator + " no existe");
                        }
                    }
                    colaboradores.Add(colaborador);

                    //System.Diagnostics.Debug.WriteLine(numeroIdentificacion, primerApellido, segundoApellido, nombres);
                }
            }

            if ((errores != null) && (!errores.Any()))
            {
                // Add new item
                System.Diagnostics.Debug.WriteLine("Vacio !!!!!");

                foreach (var i in colaboradores)
                {

                    var unique = UniqueIdentification(i.numero_identificacion);
                    if (unique == "SI")
                    {
                        errores.Add("Número de identificación " + i.numero_identificacion + " ya existe!");
                    }
                    else if (unique == "NO")
                    {

                        ContactoDto contacto = new ContactoDto();
                        contacto.ParroquiaId = i.parroquia;
                        contacto.ComunidadId = i.comunidad;

                        var c = _contactoRepository.InsertOrUpdateAsync(contacto);
                        i.ContactoId = c.Id;

                        Colaboradores col = new Colaboradores();
                        col.catalogo_tipo_identificacion_id = i.catalogo_tipo_identificacion_id;
                        col.numero_identificacion = i.numero_identificacion;
                        col.codigo_dactilar = i.codigo_dactilar;
                        col.primer_apellido = i.primer_apellido;
                        col.segundo_apellido = i.segundo_apellido;
                        col.nombres = i.nombres;
                        col.fecha_nacimiento = i.fecha_nacimiento;
                        col.catalogo_genero_id = i.catalogo_genero_id;
                        col.catalogo_etnia_id = i.catalogo_etnia_id;
                        col.PaisId = i.PaisId;
                        col.pais_pais_nacimiento_id = i.pais_pais_nacimiento_id;
                        col.fecha_ingreso = i.fecha_ingreso;
                        col.catalogo_destino_estancia_id = i.catalogo_destino_estancia_id;
                        col.catalogo_encargado_personal_id = i.catalogo_encargado_personal_id;
                        col.catalogo_sector_id = i.catalogo_sector_id;
                        col.catalogo_cargo_id = i.catalogo_cargo_id;
                        col.es_carga_masiva = true;
                        col.fecha_carga_masiva = DateTime.Now;

                        var legajo = GetLegajo();
                        if (legajo == "NO")
                        {
                            col.numero_legajo_temporal = (00001).ToString();
                        }
                        else
                        {
                            var nro = int.Parse(legajo) + 1;
                            col.numero_legajo_temporal = (nro).ToString();
                        }

                        //var ws = _webService.BusquedaPorCedulaRegistroCivilHuellaDigital(col.numero_cuenta, col.codigo_dactilar);

                        Repository.InsertAndGetIdAsync(col);

                    }

                    errores.Add("OK");

                }

                return errores;
            }
            else
            {
                return errores;
            }


        }

        #region SL: Colaboradores
        public ColaboradoresDetallesDto Detalles(int colaboradorId)
        {
            var entity = Repository.Get(colaboradorId);
            var dto = Mapper.Map<Colaboradores, ColaboradoresDetallesDto>(entity);
            //Nacionalidad
            if (entity.pais_pais_nacimiento_id > 0)
            {
                dto.Nacionalidad = _catalogoRepository.GetCatalogo(entity.pais_pais_nacimiento_id) != null ?
                                    _catalogoRepository.GetCatalogo(entity.pais_pais_nacimiento_id).nombre : "";
            }

            var discapacidad = _colaboradorDiscapacidadRepository.GetAll()
                .Include(o => o.CatalogoTipoDiscapacidad)
                .Where(o => o.ColaboradoresId.HasValue)
                .Where(o => o.ColaboradoresId == colaboradorId)
                .FirstOrDefault(o => !o.ColaboradorCargaSocialId.HasValue);

            if (discapacidad != null)
            {
                dto.Discapacidad = "SI";
                dto.TipoDiscapacidad = discapacidad.CatalogoTipoDiscapacidad.nombre;
            }
            else
            {
                dto.Discapacidad = "NO";
            }
            var solicitud = _tarjetaRepository.GetAll().Where(c => c.estado == TarjetaEstado.Activo).Where(c => c.ColaboradorId == dto.Id).FirstOrDefault();

            if (solicitud != null && solicitud.Id > 0)
            {
                dto.IngresoBpm = solicitud.solicitud_pam;
            }

            return dto;
        }


        #endregion

        #region ES: Generacion QR

        public Dictionary<string, object> GenerarQr(int id) //id Colaborador Id
        {
            //OBJECTO SERIALIZABLE PARA QR
            Dictionary<string, object> json = new Dictionary<string, object>();
            Dictionary<string, object> reservas = new Dictionary<string, object>();

            //USUARIO LOGEADO
            string userlogin = System.Web.HttpContext.Current.User.Identity.Name.ToString();
            var usuario = _usuarioRepository.GetAll().Where(c => c.Cuenta == userlogin).FirstOrDefault();

            if (usuario != null)
            {
                //COLABORADOR DATOS
                var colaborador = this.GetColaborador(id);
                if (colaborador != null)
                {   //INFORMACION PERSONAL COLABORADOR
                    // var infocontacto = _contactoRepository.GetContacto(colaborador.Id);
                    //PARAMETRO QR FECHA CADUCIDAD
                    var fechavigenciaqr = this.getParametroPorCodigo("PARAMETRO.FECHA.CADUCIDAD.QR");

                    //SERVICIOS COLABORADOR
                    List<string> services = new List<string>();
                    services.Add("Acceso");
                    var consulta = _colaboradorServicioRepository.GetAll().Include(c => c.Catalogo).Where(c => c.ColaboradoresId == colaborador.Id).Where(c => c.vigente).ToList();

                    if (consulta.Count > 0)
                    {
                        foreach (var item in consulta)
                        {
                            if (item.Catalogo.codigo == "SALMUERZO")
                            {
                                services.Add("Alimentacion");
                            }
                            if (item.Catalogo.codigo == "STRASPORTE")
                            {
                                services.Add("Transporte");
                            }
                            if (item.Catalogo.codigo == "SHOSPEDAJE")
                            {
                                services.Add("Hospedaje");
                            }
                            if (item.Catalogo.codigo == "SLAVANDERIA")
                            {
                                services.Add("Lavanderia");
                            }
                        }

                    }
                    var hospedaje = (from h in consulta where h.Catalogo.codigo == "SHOSPEDAJE" select h).FirstOrDefault();

                    //DATOS AGREGADOS AL JSON QR

                    var FechaTemporal = DateTime.Now.AddDays(Int32.Parse(fechavigenciaqr));
                    json.Add("cedula", colaborador.numero_identificacion);
                    if (fechavigenciaqr.Length > 0)
                    {
                        json.Add("fechaVigencia", FechaTemporal.Year + "-" + FechaTemporal.Month + "-" + FechaTemporal.Day);
                    }
                    else
                    {
                        json.Add("fechaVigencia", "Verifique Parametro Vigencia en la DB");

                    }
                    json.Add("nombres", colaborador.nombres_apellidos != null ? colaborador.nombres_apellidos : colaborador.nombres + " " + colaborador.primer_apellido != null ? colaborador.primer_apellido : colaborador.segundo_apellido != null ? colaborador.segundo_apellido : "");
                    //json.Add("nap", colaborador.primer_apellido + " " + colaborador.segundo_apellido + " " + colaborador.nombres);
                    json.Add("servicios", services.ToArray());
                    json.Add("usuarioGenerador", usuario.Id);
                    if (hospedaje != null)
                    {
                        var reservas_hotel = _reservahotel.GetAllIncluding(c => c.EspacioHabitacion.Habitacion.Proveedor).Where(c => c.ColaboradorId == colaborador.Id).ToList();
                        if (reservas_hotel.Count > 0)
                        {
                            var reservasqr = Mapper.Map<List<ReservaHotel>, List<ReservaQR>>(reservas_hotel);

                            if (reservasqr.Count > 0)
                            {
                                json.Add("hospedaje", reservasqr.FirstOrDefault() != null ? reservasqr.FirstOrDefault() : new ReservaQR());
                            }
                        }
                    }



                }

            }

            return json;
        }

        public string GenerarQrCodigoSeguridad(int id) //id Colaborador Id
        {

            var colaborador = this.GetColaborador(id);
            var data = _colaboradoresRepository.Get(id);

            JsonConvert.SerializeObject(colaborador.empleado_id_sap_local);

            return colaborador.empleado_id_sap_local.ToString();
        }
        public string GenerarQrExternos(int id) //id Colaborador Id
        {

            var colaborador = this.GetColaborador(id);
            var data = _colaboradoresRepository.Get(id);
            JsonConvert.SerializeObject(colaborador.numero_identificacion);

            return colaborador.numero_identificacion.ToString();
        }

        public string UniquePosicion(string posicion, int id)
        {
            var query = Repository.GetAll().Where(d => d.posicion == posicion && d.vigente == true && d.estado != RRHHCodigos.ESTADO_INACTIVO).FirstOrDefault();
            if (query != null)
            {
                if (query.Id == id)
                {
                    return "NO";
                }
                else
                {
                    return "SI";
                }

            }
            else
            {
                return "NO";
            }
        }

        public string UniqueCuentaBanco(string cuenta, int banco, int id)
        {
            var query = Repository.GetAll().Where(d => d.numero_cuenta == cuenta && d.catalogo_banco_id == banco && d.vigente == true && d.estado != RRHHCodigos.ESTADO_INACTIVO).FirstOrDefault();
            if (query != null)
            {
                if (query.Id == id)
                {
                    return "NO";
                }
                else
                {
                    return query.nombres_apellidos;
                }

            }
            else
            {
                return "NO";
            }
        }
        public int colaboradortieneservicios_(int id)
        {
            var servicios = _colaboradorServicioRepository.GetAll().Where(c => c.ColaboradoresId == id).Where(c => c.vigente).ToList();

            return servicios.Count > 0 ? 1 : 0;
        }

        public int colaboradortienereservas(int id)
        {
            var reservas = _reservahotel.GetAll().Where(c => c.ColaboradorId == id).ToList();
            return reservas.Count > 0 ? 1 : 0;
        }

        public int UpdateValidacionCedula(int id)
        {
            var colaborador = Repository.Get(id);
            if (colaborador.validacion_cedula)
            {
                colaborador.validacion_cedula = false;
            }
            else
            {
                colaborador.validacion_cedula = true;
            }
            return colaborador.Id;
        }

        #endregion

        #region ES: Convertir Letras  Numeros

        private string ConvertirNumerosALetras(double value)
        {
            string dec = " ";

            Int64 entero = Convert.ToInt64(Math.Truncate(value));
            int decimales = Convert.ToInt32(Math.Round((value - entero) * 100, 2));
            if (decimales > 0)
            {
                dec = " ";
                dec = " CON " + decimales.ToString() + "/100";
            }
            string Num2Text = "";
            value = Math.Truncate(value);
            if (value == 0) Num2Text = "CERO";
            else if (value == 1) Num2Text = "UNO";
            else if (value == 2) Num2Text = "DOS";
            else if (value == 3) Num2Text = "TRES";
            else if (value == 4) Num2Text = "CUATRO";
            else if (value == 5) Num2Text = "CINCO";
            else if (value == 6) Num2Text = "SEIS";
            else if (value == 7) Num2Text = "SIETE";
            else if (value == 8) Num2Text = "OCHO";
            else if (value == 9) Num2Text = "NUEVE";
            else if (value == 10) Num2Text = "DIEZ";
            else if (value == 11) Num2Text = "ONCE";
            else if (value == 12) Num2Text = "DOCE";
            else if (value == 13) Num2Text = "TRECE";
            else if (value == 14) Num2Text = "CATORCE";
            else if (value == 15) Num2Text = "QUINCE";
            else if (value < 20) Num2Text = "DIECI" + ConvertirNumerosALetras(value - 10);
            else if (value == 20) Num2Text = "VEINTE";
            else if (value < 30) Num2Text = "VEINTI" + ConvertirNumerosALetras(value - 20);
            else if (value == 30) Num2Text = "TREINTA";
            else if (value == 40) Num2Text = "CUARENTA";
            else if (value == 50) Num2Text = "CINCUENTA";
            else if (value == 60) Num2Text = "SESENTA";
            else if (value == 70) Num2Text = "SETENTA";
            else if (value == 80) Num2Text = "OCHENTA";
            else if (value == 90) Num2Text = "NOVENTA";
            else if (value < 100) Num2Text = ConvertirNumerosALetras(Math.Truncate(value / 10) * 10) + " Y " + ConvertirNumerosALetras(value % 10);
            else if (value == 100) Num2Text = "CIEN";
            else if (value < 200) Num2Text = "CIENTO " + ConvertirNumerosALetras(value - 100);
            else if ((value == 200) || (value == 300) || (value == 400) || (value == 600) || (value == 800)) Num2Text = ConvertirNumerosALetras(Math.Truncate(value / 100)) + "CIENTOS";
            else if (value == 500) Num2Text = "QUINIENTOS";
            else if (value == 700) Num2Text = "SETECIENTOS";
            else if (value == 900) Num2Text = "NOVECIENTOS";
            else if (value < 1000) Num2Text = ConvertirNumerosALetras(Math.Truncate(value / 100) * 100) + " " + ConvertirNumerosALetras(value % 100);
            else if (value == 1000) Num2Text = "MIL";
            else if (value < 2000) Num2Text = "MIL " + ConvertirNumerosALetras(value % 1000);
            else if (value < 1000000)
            {
                Num2Text = ConvertirNumerosALetras(Math.Truncate(value / 1000)) + " MIL";
                if ((value % 1000) > 0) Num2Text = Num2Text + " " + ConvertirNumerosALetras(value % 1000);
            }

            else if (value == 1000000) Num2Text = "UN MILLON";
            else if (value < 2000000) Num2Text = "UN MILLON " + ConvertirNumerosALetras(value % 1000000);
            else if (value < 1000000000000)
            {
                Num2Text = ConvertirNumerosALetras(Math.Truncate(value / 1000000)) + " MILLONES ";
                if ((value - Math.Truncate(value / 1000000) * 1000000) > 0) Num2Text = Num2Text + " " + ConvertirNumerosALetras(value - Math.Truncate(value / 1000000) * 1000000);
            }

            else if (value == 1000000000000) Num2Text = "UN BILLON";
            else if (value < 2000000000000) Num2Text = "UN BILLON " + ConvertirNumerosALetras(value - Math.Truncate(value / 1000000000000) * 1000000000000);

            else
            {
                Num2Text = ConvertirNumerosALetras(Math.Truncate(value / 1000000000000)) + " BILLONES";
                if ((value - Math.Truncate(value / 1000000000000) * 1000000000000) > 0) Num2Text = Num2Text + " " + ConvertirNumerosALetras(value - Math.Truncate(value / 1000000000000) * 1000000000000);
            }


            return Num2Text + dec;

        }

        public string ServiciosColaborado(int id)
        {
            string servicios = "";
            var consulta = _colaboradorServicioRepository.GetAll().Include(c => c.Catalogo).Where(c => c.ColaboradoresId == id).Where(c => c.vigente).Select(c => c.Catalogo.nombre).ToArray();
            if (consulta.Length > 0)
            {
                servicios = String.Join(",", consulta);
            }

            return servicios;

        }

        public string colaboradortienereservasactivas(int id)
        {
            string resultado = "NO";
            var listadetalles = _detallereservahotel.GetAll().Where(c => c.ReservaHotel.ColaboradorId == id)
                                                            .ToList();
            string hoy = DateTime.Now.ToShortDateString();
            string manana = DateTime.Now.AddDays(1).ToShortDateString();
            string pasadomanana = DateTime.Now.AddDays(2).ToShortDateString();
            List<ReservaQRDetalle> seleccionados = new List<ReservaQRDetalle>();

            var detallesQr = Mapper.Map<List<DetalleReserva>, List<ReservaQRDetalle>>(listadetalles);
            var dhoy = (from d in detallesQr
                        where d.fre == hoy
                        select d).FirstOrDefault();

            if (dhoy != null)
            {
                seleccionados.Add(dhoy);
            }
            var dmanana = (from d in detallesQr
                           where d.fre == manana
                           select d).FirstOrDefault();
            if (dmanana != null)
            {
                seleccionados.Add(dmanana);
            }
            var dpasado = (from d in detallesQr
                           where d.fre == pasadomanana
                           select d).FirstOrDefault();
            if (dpasado != null)
            {
                seleccionados.Add(dpasado);
            }

            if (seleccionados.Count > 0)
            {
                resultado = "SI";
            }

            return resultado;
        }
        #endregion


        public List<Colaboradores> consultaFiltrosReporte(ColaboradorReporteDto colaborador)
        {
            var query = Repository.GetAll().Where(c => c.vigente == true && c.es_externo == false);

            if (colaborador.tipo_identificacion > 0)
            {
                query = query.Where(x => x.catalogo_tipo_identificacion_id == colaborador.tipo_identificacion);
            }
            if (colaborador.numero_identificacion != null)
            {
                query = query.Where(x => x.numero_identificacion.StartsWith(colaborador.numero_identificacion));
            }
            if (colaborador.nombres_apellidos != null)
            {
                query = query.Where(x => x.nombres.ToUpper().Contains(colaborador.nombres_apellidos) || x.primer_apellido.ToUpper().Contains(colaborador.nombres_apellidos) || x.segundo_apellido.ToUpper().Contains(colaborador.nombres_apellidos) || (x.primer_apellido + " " + x.segundo_apellido).ToUpper().Contains(colaborador.nombres_apellidos));
            }
            if (colaborador.id_sap > 0)
            {
                query = query.Where(x => x.empleado_id_sap == colaborador.id_sap || x.candidato_id_sap == colaborador.id_sap);
            }
            if (colaborador.posicion != null)
            {
                query = query.Where(x => x.posicion.StartsWith(colaborador.posicion));
            }
            if (colaborador.estado != null)
            {
                query = query.Where(x => x.estado.StartsWith(colaborador.estado));
            }
            if (colaborador.grupo_personal > 0)
            {
                query = query.Where(x => x.catalogo_grupo_personal_id == colaborador.grupo_personal);
            }
            if (colaborador.encargado_personal > 0)
            {
                query = query.Where(x => x.catalogo_encargado_personal_id == colaborador.encargado_personal);
            }
            if (colaborador.fecha_ingreso_desde != null)
            {
                query = query.Where(x => x.fecha_ingreso >= colaborador.fecha_ingreso_desde);
            }
            if (colaborador.fecha_ingreso_hasta != null)
            {
                query = query.Where(x => x.fecha_ingreso <= colaborador.fecha_ingreso_hasta);
            }

            var colaboradores = Mapper.Map<IQueryable<Colaboradores>, List<Colaboradores>>(query);

            return colaboradores;
        }

        public ExcelPackage reporteInformacionGeneral(ColaboradorReporteDto colaborador)
        {


            var query = Repository.GetAllIncluding(c => c.TipoIdentificacion,
                                                   c => c.Genero,
                                                   c => c.Etnia,
                                                   c => c.Pais,
                                                   c => c.EstadoCivil,
                                                   c => c.Contacto.Parroquia,
                                                   c => c.Contacto.Comunidad,
                                                   c => c.FormacionEducativa,
                                                   c => c.Cargo,
                                                   c => c.CodigoSiniestro,
                                                   c => c.GrupoPersonal,
                                                   c => c.DestinoEstancia,
                                                   c => c.Area,
                                                   c => c.VinculoLaboral,
                                                   c => c.Clase,
                                                   c => c.PlanBeneficios,
                                                   c => c.PlanSalud,
                                                   c => c.CoberturaDependiente,
                                                   c => c.PlanesBeneficios,
                                                   c => c.Asociacion,
                                                   c => c.AptoMedico,
                                                   c => c.DivisionPersonal,
                                                   c => c.SubdivisionPersonal,
                                                   c => c.TipoContrato,
                                                    c => c.ClaseContrato,
                                                    c => c.Funcion,
                                                    c => c.TipoNomina,
                                                    c => c.PeriodoNomina,
                                                    c => c.FormaPago,
                                                    c => c.Grupo,
                                                    c => c.SubGrupo,
                                                    c => c.Banco,
                                                    c => c.TipoCuenta,
                                                    c => c.AdminRotacion,
                                                    c => c.Encuadre,
                                                    c => c.EncargadoPersonal,
                                                    c => c.CodigoIncapacidad,
                                                    c => c.Sector,
                                                    c => c.ViaPago
                                                   ).Where(c => c.vigente == true && c.es_externo == false)
                                                   .ToList();

            if (colaborador.tipo_identificacion != null && colaborador.tipo_identificacion > 0)
            {
                query = query.Where(x => x.catalogo_tipo_identificacion_id == colaborador.tipo_identificacion).ToList();
            }
            if (colaborador.numero_identificacion != null && colaborador.numero_identificacion != "")
            {
                query = query.Where(x => x.numero_identificacion.StartsWith(colaborador.numero_identificacion)).ToList();
            }
            if (colaborador.nombres_apellidos != null && colaborador.nombres_apellidos != "")
            {
                query = query.Where(x => x.nombres.ToUpper().Contains(colaborador.nombres_apellidos) || x.primer_apellido.ToUpper().Contains(colaborador.nombres_apellidos) || x.segundo_apellido.ToUpper().Contains(colaborador.nombres_apellidos) || (x.primer_apellido + " " + x.segundo_apellido).ToUpper().Contains(colaborador.nombres_apellidos)).ToList();
            }
            if (colaborador.id_sap > 0)
            {
                query = query.Where(x => x.empleado_id_sap == colaborador.id_sap || x.candidato_id_sap == colaborador.id_sap).ToList();
            }
            if (colaborador.posicion != null && colaborador.posicion != "")
            {
                query = query.Where(x => x.posicion.StartsWith(colaborador.posicion)).ToList();
            }
            if (colaborador.estado != null && colaborador.estado != "")
            {
                query = query.Where(x => x.estado.StartsWith(colaborador.estado)).ToList();
            }
            if (colaborador.grupo_personal > 0)
            {
                query = query.Where(x => x.catalogo_grupo_personal_id == colaborador.grupo_personal).ToList();
            }
            if (colaborador.encargado_personal > 0)
            {
                query = query.Where(x => x.catalogo_encargado_personal_id == colaborador.encargado_personal).ToList();
            }
            if (colaborador.fecha_ingreso_desde != null && colaborador.fecha_ingreso_desde != new DateTime(1, 1, 0001))
            {
                query = query.Where(x => x.fecha_ingreso >= colaborador.fecha_ingreso_desde).ToList();
            }
            if (colaborador.fecha_ingreso_hasta != null && colaborador.fecha_ingreso_desde != new DateTime(1, 1, 0001))
            {
                query = query.Where(x => x.fecha_ingreso <= colaborador.fecha_ingreso_hasta).ToList();
            }





            if (query.ToList().Count > 0)
            {

                DateTime fechaActual = DateTime.Now;

                var aux = fechaActual.ToString("ddMMyyyyhhmm");
                var fecha = fechaActual.ToString("dd/MM/yyyy");

                var usuario = "";
                string user = System.Web.HttpContext.Current.User.Identity.Name.ToString();
                var usuarioencontrado = _usuarioRepository.GetAll().Where(c => c.Cuenta == user).FirstOrDefault();
                if (usuarioencontrado != null && usuarioencontrado.Id > 0)
                {
                    usuario = usuarioencontrado.NombresCompletos;
                }

                ExcelPackage excel = new ExcelPackage();
                //Crear hoja en archivo excel
                var hoja = excel.Workbook.Worksheets.Add("Información General de Colaboradores");
                var row = 10;

                #region Alto y Ancho de la Columnas Excel
                hoja.Column(1).Width = 1.45;
                hoja.Column(2).Width = 10;
                hoja.Column(3).Width = 11;
                hoja.Column(4).Width = 11;
                hoja.Column(5).Width = 8;
                hoja.Column(6).Width = 12;
                hoja.Column(7).Width = 11;
                hoja.Column(8).Width = 16;
                hoja.Column(9).Width = 11;
                hoja.Column(10).Width = 11;
                hoja.Column(11).Width = 11;
                hoja.Column(12).Width = 10;
                hoja.Column(13).Width = 18;
                hoja.Column(14).Width = 18;
                hoja.Column(15).Width = 37;
                hoja.Column(16).Width = 11;
                hoja.Column(17).Width = 12;
                hoja.Column(18).Width = 11;
                hoja.Column(19).Width = 12;
                hoja.Column(20).Width = 10;
                hoja.Column(21).Width = 9;
                hoja.Column(22).Width = 22;
                hoja.Column(23).Width = 8;
                hoja.Column(24).Width = 9;
                hoja.Column(25).Width = 26;
                hoja.Column(26).Width = 13;
                hoja.Column(27).Width = 12;
                hoja.Column(28).Width = 22;
                hoja.Column(29).Width = 33;
                hoja.Column(30).Width = 28;
                hoja.Column(31).Width = 26;
                hoja.Column(32).Width = 15;
                hoja.Column(33).Width = 27;
                hoja.Column(34).Width = 19;
                hoja.Column(35).Width = 22;
                hoja.Column(36).Width = 22;
                hoja.Column(37).Width = 22;
                hoja.Column(38).Width = 23;
                hoja.Column(39).Width = 43;
                hoja.Column(40).Width = 16;
                hoja.Column(41).Width = 21;
                hoja.Column(42).Width = 29;
                hoja.Column(43).Width = 11;
                hoja.Column(44).Width = 11;
                hoja.Column(45).Width = 13;
                hoja.Column(46).Width = 25;
                hoja.Column(47).Width = 46;
                hoja.Column(48).Width = 43;
                hoja.Column(49).Width = 18;
                hoja.Column(50).Width = 19;
                hoja.Column(51).Width = 17;
                hoja.Column(52).Width = 20;
                hoja.Column(53).Width = 13;
                hoja.Column(54).Width = 23;
                hoja.Column(55).Width = 15;
                hoja.Column(56).Width = 18;
                hoja.Column(57).Width = 15;
                hoja.Column(58).Width = 20;
                hoja.Column(59).Width = 20;
                hoja.Column(60).Width = 15;
                hoja.Column(61).Width = 9;
                hoja.Column(62).Width = 11;
                hoja.Column(63).Width = 11;
                hoja.Column(64).Width = 15;
                hoja.Column(65).Width = 15;
                hoja.Column(66).Width = 15;
                hoja.Column(67).Width = 25;
                hoja.Column(68).Width = 18;
                hoja.Column(69).Width = 22;
                hoja.Column(70).Width = 15;
                hoja.Column(71).Width = 11;
                hoja.Column(72).Width = 11;
                hoja.Column(73).Width = 11;
                hoja.Column(74).Width = 15;
                hoja.Column(75).Width = 11;

                //height filas de titulos
                hoja.Row(1).Height = 30;
                hoja.Row(2).Height = 15;
                hoja.Row(3).Height = 30;
                hoja.Row(4).Height = 15;
                hoja.Row(5).Height = 30;
                hoja.Row(6).Height = 30;
                hoja.Row(7).Height = 30;
                hoja.Row(8).Height = 15;
                hoja.Row(9).Height = 30;
                #endregion

                #region Cabecera de Documento

                hoja.Cells["B1:BX1"].Style.Font.Bold = true;
                hoja.Cells["B1:BX1"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                hoja.Cells["B1:BX1"].Style.Fill.BackgroundColor.SetColor(Color.DarkGray);
                hoja.Cells["B1:BX1"].Style.Border.BorderAround(ExcelBorderStyle.Thin, System.Drawing.Color.White);
                hoja.Cells["B1:BX1"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                hoja.Cells["E1"].Value = "Reporte de Información General de Colaboradores";
                hoja.Cells["E1"].Style.Font.Color.SetColor(Color.White);
                hoja.Cells["E1"].Style.Font.Name = "Arial";
                hoja.Cells["E1"].Style.Font.Size = 20;

                hoja.Cells["B3:BX3"].Style.Font.Bold = true;
                hoja.Cells["B3:BX3"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                hoja.Cells["B3:BX3"].Style.Fill.BackgroundColor.SetColor(Color.DarkGray);
                hoja.Cells["B3:BX3"].Style.Border.BorderAround(ExcelBorderStyle.Thin, System.Drawing.Color.White);
                hoja.Cells["B3:BX3"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                hoja.Cells["B3"].Value = "Una vez completado cargar en el CSC Ecuador";
                hoja.Cells["B3"].Style.Font.Color.SetColor(Color.White);
                hoja.Cells["B3"].Style.Font.Name = "Calibri";
                hoja.Cells["B3"].Style.Font.Size = 12;

                hoja.Cells["B5:BX5"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                hoja.Cells["B5:BX5"].Style.Fill.BackgroundColor.SetColor(Color.DarkGray);
                hoja.Cells["B5:BX5"].Style.Border.BorderAround(ExcelBorderStyle.Thin, System.Drawing.Color.White);
                hoja.Cells["B5:BX5"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                hoja.Cells["B5"].Value = "Proyecto:";
                hoja.Cells["B5"].Style.Font.Name = "Calibri";
                hoja.Cells["B5"].Style.Font.Size = 12;
                hoja.Cells["D5"].Value = null;
                hoja.Cells["D5"].Style.Font.Name = "Calibri";
                hoja.Cells["D5"].Style.Font.Size = 10;

                hoja.Cells["B6:BX6"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                hoja.Cells["B6:BX6"].Style.Fill.BackgroundColor.SetColor(Color.DarkGray);
                hoja.Cells["B6:BX6"].Style.Border.BorderAround(ExcelBorderStyle.Thin, System.Drawing.Color.White);
                hoja.Cells["B6:BX6"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                hoja.Cells["B6"].Value = "Cargado por:";
                hoja.Cells["B6"].Style.Font.Name = "Calibri";
                hoja.Cells["B6"].Style.Font.Size = 12;
                hoja.Cells["D6"].Value = usuario;
                hoja.Cells["D6"].Style.Font.Name = "Calibri";
                hoja.Cells["D6"].Style.Font.Size = 10;

                hoja.Cells["B7:BX7"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                hoja.Cells["B7:BX7"].Style.Fill.BackgroundColor.SetColor(Color.DarkGray);
                hoja.Cells["B7:BX7"].Style.Border.BorderAround(ExcelBorderStyle.Thin, System.Drawing.Color.White);
                hoja.Cells["B7:BX7"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                hoja.Cells["B7"].Value = "Fecha:";
                hoja.Cells["B7"].Style.Font.Name = "Calibri";
                hoja.Cells["B7"].Style.Font.Size = 12;
                hoja.Cells["D7"].Value = fecha;
                hoja.Cells["D7"].Style.Font.Name = "Calibri";
                hoja.Cells["D7"].Style.Font.Size = 10;

                #endregion

                #region Cabecera de los Datos por columna
                var titleCell = hoja.Cells["B9:BX9"]; // Celdas de títulos

                titleCell.Style.Font.Bold = true;
                titleCell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                titleCell.Style.Fill.BackgroundColor.SetColor(Color.DarkGray);
                titleCell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                titleCell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                titleCell.Style.WrapText = true;

                titleCell.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                titleCell.Style.Border.Right.Color.SetColor(Color.White);
                hoja.Cells["B9:BX9"].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                hoja.Cells["B9:BX9"].Style.Border.Right.Color.SetColor(Color.White);


                hoja.Cells["B9"].Value = "FECHA INGRESO";
                hoja.Cells["C9"].Value = "ID_CANDIDATO";
                hoja.Cells["D9"].Value = "ID_SAP GLOBAL";
                hoja.Cells["E9"].Value = "POSICIÓN";
                hoja.Cells["F9"].Value = "ESTADO";
                hoja.Cells["G9"].Value = "META4";
                hoja.Cells["H9"].Value = "LEGAJO TEMPORAL";
                hoja.Cells["I9"].Value = "LEGAJO DEFINITIVO";
                hoja.Cells["J9"].Value = "TIPO DE IDENTIFICACIÓN";
                hoja.Cells["K9"].Value = "NÚMERO DE IDENTIFICACIÓN";
                hoja.Cells["L9"].Value = "CÓDIGO DACTILAR";
                hoja.Cells["M9"].Value = "APELLIDOS";
                hoja.Cells["N9"].Value = "NOMBRES";
                hoja.Cells["O9"].Value = "APELLIDOS NOMBRES";
                hoja.Cells["P9"].Value = "GÉNERO";
                hoja.Cells["Q9"].Value = "ETNIA";
                hoja.Cells["R9"].Value = "FECHA NACIMIENTO";
                hoja.Cells["S9"].Value = "PAIS NACIMIENTO";
                hoja.Cells["T9"].Value = "NACIONALIDAD";
                hoja.Cells["U9"].Value = "ESTADO CIVIL";
                hoja.Cells["V9"].Value = "FECHA MATRIMONIO";
                hoja.Cells["W9"].Value = "NUMERO DE HIJOS";
                hoja.Cells["X9"].Value = "TELÉFONO CONVENCIONAL";
                hoja.Cells["Y9"].Value = "TELÉFONO CELULAR";
                hoja.Cells["Z9"].Value = "PROVINCIA";
                hoja.Cells["AA9"].Value = "ES PROVINCIA AMAZONICA";
                hoja.Cells["AB9"].Value = "PARROQUIA";
                hoja.Cells["AC9"].Value = "COMUNIDAD";
                hoja.Cells["AD9"].Value = "DIRECCION";
                hoja.Cells["AE9"].Value = "SUELDO";
                hoja.Cells["AF9"].Value = "ESCOLARIDAD";
                hoja.Cells["AG9"].Value = "PUESTO";
                hoja.Cells["AH9"].Value = "PROYECTO";
                hoja.Cells["AI9"].Value = "CÓDIGO SINIESTRO";
                hoja.Cells["AJ9"].Value = "AGRUPACIÓN PARA REQUISITOS";
                hoja.Cells["AK9"].Value = "DESTINO DE ESTANCIA";
                hoja.Cells["AL9"].Value = "SITIO DE TRABAJO";
                hoja.Cells["AM9"].Value = "ÁREA";
                hoja.Cells["AN9"].Value = "VÍNCULO LABORAL (PERMANENTE / TEMPORAL)";
                hoja.Cells["AO9"].Value = "CLASE";
                hoja.Cells["AP9"].Value = "PLAN DE BENEFICIOS";
                hoja.Cells["AQ9"].Value = "PLAN DE SALUD";
                hoja.Cells["AR9"].Value = "COBERTURA DEPENDIENTE";
                hoja.Cells["AS9"].Value = "PLANES DE BENEFICIOS";
                hoja.Cells["AT9"].Value = "ASOCIACIÓN";
                hoja.Cells["AU9"].Value = "APTO MÉDICO";
                hoja.Cells["AV9"].Value = "DIVISION DE PERSONAL";
                hoja.Cells["AW9"].Value = "SUBDIVISION DE PERSONAL";
                hoja.Cells["AX9"].Value = "TIPO DE CONTRATO";
                hoja.Cells["AY9"].Value = "CLASE DE CONTRATO";
                hoja.Cells["AZ9"].Value = "FUNCIÓN";
                hoja.Cells["BA9"].Value = "TIPO DE NOMINA";
                hoja.Cells["BB9"].Value = "PERIODO DE NOMINA";
                hoja.Cells["BC9"].Value = "FORMA DE PAGO";
                hoja.Cells["BD9"].Value = "GRUPO (CATEGORÍA O PC)";
                hoja.Cells["BE9"].Value = "SUB GRUPO (CUARTIL)";
                hoja.Cells["BF9"].Value = "BANCO";
                hoja.Cells["BG9"].Value = "TIPO DE CUENTA";
                hoja.Cells["BH9"].Value = "EMPRESA";
                hoja.Cells["BI9"].Value = "RÉGIMEN";
                hoja.Cells["BJ9"].Value = "FECHA CADUCIDAD CONTRATO";
                hoja.Cells["BK9"].Value = "OBRA DE EJECUTOR";
                hoja.Cells["BL9"].Value = "REMUNERACION MENSUAL";
                hoja.Cells["BM9"].Value = "NÚMERO DE CUENTA";
                hoja.Cells["BN9"].Value = "ENCUADRE FC";
                hoja.Cells["BO9"].Value = "ENCARGADO DE PERSONAL";
                hoja.Cells["BP9"].Value = "CODIGO DE INCAPACIDAD";
                hoja.Cells["BQ9"].Value = "SECTOR";
                hoja.Cells["BR9"].Value = "VIA DE PAGO";
                hoja.Cells["BS9"].Value = "ES SUSTITUTO";
                hoja.Cells["BT9"].Value = "FECHA SUSTITUTO DESDE";
                hoja.Cells["BU9"].Value = "FECHA DE SALIDA";
                hoja.Cells["BV9"].Value = "MOTIVO DE SALIDA";
                hoja.Cells["BW9"].Value = "FECHA PAGO LIQUIDACION";
                hoja.Cells["BX9"].Value = "ID SAP LOCAL";
                #endregion


                foreach (var i in query.ToList())
                {

                    /*  var body = hoja.Cells["B" + row + ":BW" + row];
                    body.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    body.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    body.Style.Font.Name = "Calibri";
                    body.Style.Font.Size = 9;
                   //body.Style.WrapText = true;
                    body.Style.Border.BorderAround(ExcelBorderStyle.Dotted, System.Drawing.Color.Black);

                   // body.Style.Border.Right.Style = ExcelBorderStyle.Dotted;
                   // body.Style.Border.Right.Color.SetColor(Color.Black);*/

                    hoja.Row(row).Height = 24;



                    hoja.Cells["B" + row].Value = i.fecha_ingreso.HasValue ? i.fecha_ingreso.Value.ToShortDateString() : "";
                    hoja.Cells["C" + row].Value = i.candidato_id_sap.HasValue ? i.candidato_id_sap.Value.ToString() : "";
                    hoja.Cells["D" + row].Value = i.empleado_id_sap.HasValue ? i.empleado_id_sap.Value.ToString() : "";
                    hoja.Cells["BX" + row].Value = i.empleado_id_sap_local.HasValue ? i.empleado_id_sap_local.Value.ToString() : "";
                    hoja.Cells["E" + row].Value = i.posicion;
                    hoja.Cells["F" + row].Value = i.estado;
                    hoja.Cells["G" + row].Value = i.meta4;
                    hoja.Cells["H" + row].Value = i.numero_legajo_temporal;
                    hoja.Cells["I" + row].Value = i.numero_legajo_definitivo;
                    hoja.Cells["J" + row].Value = i.catalogo_tipo_identificacion_id == null ? "" : i.TipoIdentificacion.nombre;
                    hoja.Cells["K" + row].Value = i.numero_identificacion;
                    hoja.Cells["L" + row].Value = i.codigo_dactilar;
                    hoja.Cells["M" + row].Value = i.segundo_apellido == null ? i.primer_apellido : i.primer_apellido + " " + i.segundo_apellido;
                    hoja.Cells["N" + row].Value = i.nombres;
                    hoja.Cells["O" + row].Value = i.nombres_apellidos;
                    hoja.Cells["P" + row].Value = i.catalogo_genero_id == null ? "" : i.Genero.nombre;
                    hoja.Cells["Q" + row].Value = i.catalogo_etnia_id == null ? "" : i.Etnia.nombre;
                    hoja.Cells["R" + row].Value = i.fecha_nacimiento.HasValue ? i.fecha_nacimiento.GetValueOrDefault().ToShortDateString() : "";
                    hoja.Cells["S" + row].Value = i.PaisId.HasValue ? i.Pais.nombre : " ";
                    hoja.Cells["T" + row].Value = i.pais_pais_nacimiento_id > 0 ? "" : _catalogoRepository.GetCatalogo(i.pais_pais_nacimiento_id).nombre;
                    hoja.Cells["U" + row].Value = i.catalogo_estado_civil_id == null ? "" : i.EstadoCivil.nombre;
                    hoja.Cells["V" + row].Value = i.fecha_matrimonio == null ? "" : i.fecha_matrimonio.GetValueOrDefault().ToShortDateString();
                    hoja.Cells["W" + row].Value = i.numero_hijos.HasValue ? i.numero_hijos.Value.ToString() : " ";
                    if (i != null && i.Contacto != null)
                    {
                        hoja.Cells["X" + row].Value = i.Contacto.telefono_convencional;
                        hoja.Cells["Y" + row].Value = i.Contacto.celular;
                        hoja.Cells["Z" + row].Value = "";
                        hoja.Cells["AA" + row].Value = "";
                        hoja.Cells["AB" + row].Value = i.Contacto.Parroquia == null ? "" : i.Contacto.Parroquia.nombre;
                        hoja.Cells["AC" + row].Value = i.Contacto.ComunidadId == null ? "" : i.Contacto.Comunidad.nombre;
                        hoja.Cells["AD" + row].Value = i.Contacto.calle_principal + " " + i.Contacto.numero;
                    }
                    /*   else
                       {
                           hoja.Cells["X" + row].Value = "";
                           hoja.Cells["Y" + row].Value = "";
                           hoja.Cells["Z" + row].Value = "";
                           hoja.Cells["AA" + row].Value = "";
                           hoja.Cells["AB" + row].Value = "";
                           hoja.Cells["AC" + row].Value = "";
                           hoja.Cells["AD" + row].Value = "";
                       }*/

                    hoja.Cells["AE" + row].Value = i.remuneracion_mensual.HasValue ? i.remuneracion_mensual.Value : 0;
                    hoja.Cells["AF" + row].Value = i.catalogo_formacion_educativa_id == null ? "" : i.FormacionEducativa != null ? i.FormacionEducativa.nombre : "";
                    hoja.Cells["AG" + row].Value = i.catalogo_cargo_id == null ? "" : i.Cargo != null ? i.Cargo.nombre : "";
                    hoja.Cells["AH" + row].Value = i.ContratoId == null ? "" : _catalogoRepository.GetCatalogo(i.ContratoId.Value).nombre;
                    hoja.Cells["AI" + row].Value = i.catalogo_codigo_siniestro_id == null ? "" : i.CodigoSiniestro.nombre;
                    hoja.Cells["AJ" + row].Value = i.catalogo_grupo_personal_id == null ? "" : i.GrupoPersonal.nombre;
                    hoja.Cells["AK" + row].Value = i.catalogo_destino_estancia_id == null ? "" : i.DestinoEstancia.nombre;
                    hoja.Cells["AL" + row].Value = i.catalogo_sitio_trabajo_id == null ? "" : _catalogoRepository.GetCatalogo(int.Parse(i.catalogo_sitio_trabajo_id)).nombre;
                    hoja.Cells["AM" + row].Value = i.catalogo_area_id == null ? "" : i.Area.nombre;
                    hoja.Cells["AN" + row].Value = i.catalogo_vinculo_laboral_id == null ? "" : i.VinculoLaboral.nombre;
                    hoja.Cells["AO" + row].Value = i.catalogo_clase_id == null ? "" : i.Clase.nombre;
                    hoja.Cells["AP" + row].Value = i.catalogo_plan_beneficios_id == null ? "" : i.PlanBeneficios.nombre;
                    hoja.Cells["AQ" + row].Value = i.catalogo_plan_salud_id == null ? "" : i.PlanSalud.nombre;
                    hoja.Cells["AR" + row].Value = i.catalogo_cobertura_dependiente_id == null ? "" : i.CoberturaDependiente.nombre;
                    hoja.Cells["AS" + row].Value = i.catalogo_planes_beneficios_id == null ? "" : i.PlanesBeneficios.nombre;
                    hoja.Cells["AT" + row].Value = i.catalogo_asociacion_id == null ? "" : i.Asociacion.nombre;
                    hoja.Cells["AU" + row].Value = i.catalogo_apto_medico_id == null ? "" : i.AptoMedico.nombre;
                    hoja.Cells["AV" + row].Value = i.catalogo_division_personal_id == null ? "" : i.DivisionPersonal.nombre;
                    hoja.Cells["AW" + row].Value = i.catalogo_subdivision_personal_id == null ? "" : i.SubdivisionPersonal.nombre;
                    hoja.Cells["AX" + row].Value = i.catalogo_tipo_contrato_id == null ? "" : i.TipoContrato.nombre;
                    hoja.Cells["AY" + row].Value = i.catalogo_clase_contrato_id == null ? "" : i.ClaseContrato.nombre;
                    hoja.Cells["AZ" + row].Value = i.catalogo_funcion_id == null ? "" : i.Funcion.nombre;
                    hoja.Cells["BA" + row].Value = i.catalogo_tipo_nomina_id == null ? "" : i.TipoNomina.nombre;
                    hoja.Cells["BB" + row].Value = i.catalogo_periodo_nomina_id == null ? "" : i.PeriodoNomina.nombre;
                    hoja.Cells["BC" + row].Value = i.catalogo_forma_pago_id == null ? "" : i.FormaPago.nombre;
                    hoja.Cells["BD" + row].Value = i.catalogo_grupo_id == null ? "" : i.Grupo.nombre;
                    hoja.Cells["BE" + row].Value = i.catalogo_subgrupo_id == null ? "" : i.SubGrupo.nombre;
                    hoja.Cells["BF" + row].Value = i.catalogo_banco_id == null ? "" : i.Banco.nombre;
                    hoja.Cells["BG" + row].Value = i.catalogo_tipo_cuenta_id == null ? "" : i.TipoCuenta.nombre;
                    hoja.Cells["BH" + row].Value = i.empresa_id;
                    hoja.Cells["BI" + row].Value = i.AdminRotacionId == null ? null : i.AdminRotacion.nombre;
                    hoja.Cells["BJ" + row].Value = i.fecha_caducidad_contrato == null ? "" : i.fecha_caducidad_contrato.HasValue ? i.fecha_caducidad_contrato.GetValueOrDefault().ToString() : "";
                    hoja.Cells["BK" + row].Value = i.ejecutor_obra == true ? "SI" : "NO";
                    hoja.Cells["BL" + row].Value = i.remuneracion_mensual.HasValue ? i.remuneracion_mensual.GetValueOrDefault().ToString() : "";
                    hoja.Cells["BM" + row].Value = i.numero_cuenta;
                    hoja.Cells["BN" + row].Value = i.catalogo_encuadre_id == null ? "" : i.Encuadre.nombre;
                    hoja.Cells["BO" + row].Value = i.catalogo_encargado_personal_id == null ? "" : i.EncargadoPersonal.nombre;
                    hoja.Cells["BP" + row].Value = i.catalogo_codigo_incapacidad_id == null ? "" : i.CodigoIncapacidad.nombre;
                    hoja.Cells["BQ" + row].Value = i.catalogo_sector_id == null ? "" : i.Sector.nombre;
                    hoja.Cells["BR" + row].Value = i.catalogo_via_pago_id == null ? "" : i.ViaPago.nombre;
                    hoja.Cells["BS" + row].Value = i.es_sustituto == true ? "SI" : "NO";
                    hoja.Cells["BT" + row].Value = i.fecha_sustituto_desde == null ? "" : i.fecha_sustituto_desde.HasValue ? i.fecha_sustituto_desde.GetValueOrDefault().ToShortDateString() : "";

                    var bajas = _bajasService.GetBaja(i.Id);
                    if (bajas != null)
                    {
                        var filtro = false;

                        if (colaborador.motivo_baja != null)
                        {
                            if (bajas.motivo_baja == colaborador.motivo_baja)
                            {
                                filtro = true;
                            }
                        }
                        if (colaborador.fecha_baja_desde != null && colaborador.fecha_baja_desde != new DateTime(1, 1, 0001))
                        {
                            if (bajas.fecha_baja >= colaborador.fecha_baja_desde)
                            {
                                filtro = true;
                            }
                        }
                        if (colaborador.fecha_baja_hasta != null && colaborador.fecha_baja_desde != new DateTime(1, 1, 0001))
                        {
                            if (bajas.fecha_baja <= colaborador.fecha_baja_hasta)
                            {
                                filtro = true;
                            }
                        }
                        if (colaborador.motivo_baja == null && colaborador.fecha_baja_desde == null && colaborador.fecha_baja_hasta == null)
                        {
                            filtro = true;
                        }
                        if (filtro == true)
                        {
                            hoja.Cells["BU" + row].Value = bajas.fecha_baja.HasValue ? bajas.fecha_baja.GetValueOrDefault().ToShortDateString() : "";
                            hoja.Cells["BV" + row].Value = bajas.motivo_baja;
                            hoja.Cells["BW" + row].Value = bajas.fecha_pago_liquidacion.HasValue ? bajas.fecha_pago_liquidacion.GetValueOrDefault().ToShortDateString() : "";
                        }
                        else
                        {
                            hoja.Cells["BU" + row].Value = "";
                            hoja.Cells["BV" + row].Value = "";
                            hoja.Cells["BW" + row].Value = "";
                        }
                    }
                    else
                    {
                        hoja.Cells["BU" + row].Value = "";
                        hoja.Cells["BV" + row].Value = "";
                        hoja.Cells["BW" + row].Value = "";
                    }

                    row++;


                }

                //System.IO.FileInfo filename = new System.IO.FileInfo(@"C:\CPP\Colaboradores\Reportes\ReporteColaboradores" + aux + usuario + ".xlsx");
                //excel.SaveAs(filename);


                return excel;
            }
            else
            {
                return null;
            }

        }

        public ExcelPackage GenerarExcelAltaMasiva()
        {
            var query = Repository.GetAll().Where(c => c.estado == RRHHCodigos.ESTADO_ENVIADO_SAP && c.vigente == true && c.es_externo == false);
            var colaboradores = Mapper.Map<IQueryable<Colaboradores>, List<Colaboradores>>(query);

            if (colaboradores.Count > 0)
            {
                var motivos_medida = _catalogoRepository.ListarCatalogosporcodigo(RRHHCodigos.MOTIVO_MEDIDA);

                var usuario = "";
                string user = System.Web.HttpContext.Current.User.Identity.Name.ToString();
                var usuarioencontrado = _usuarioRepository.GetAll().Where(c => c.Cuenta == user).FirstOrDefault();
                if (usuarioencontrado != null && usuarioencontrado.Id > 0)
                {
                    usuario = usuarioencontrado.NombresCompletos;
                }

                DateTime fechaActual = DateTime.Now;

                var aux = fechaActual.ToString("ddMMyyyyhhmm");
                var fecha = fechaActual.ToString("dd/MM/yyyy");

                ExcelPackage excel = new ExcelPackage();
                //Crear hoja en archivo excel
                var hoja = excel.Workbook.Worksheets.Add("Alta de Colaboradores");

                hoja.View.ZoomScale = 90;
                var row = 15;
                //Width de Columnas
                hoja.Column(1).Width = 2;
                hoja.Column(2).Width = 15;
                hoja.Column(3).Width = 15;
                hoja.Column(4).Width = 20;
                hoja.Column(5).Width = 11;
                hoja.Column(6).Width = 10;
                hoja.Column(7).Width = 12;
                hoja.Column(8).Width = 12;

                //height filas de titulos
                hoja.Row(3).Height = 30;
                hoja.Row(4).Height = 15;
                hoja.Row(5).Height = 15;
                hoja.Row(6).Height = 15;
                hoja.Row(7).Height = 30;
                hoja.Row(8).Height = 15;
                hoja.Row(9).Height = 30;
                hoja.Row(10).Height = 15;
                hoja.Row(11).Height = 15;
                hoja.Row(12).Height = 15;
                hoja.Row(13).Height = 15;
                hoja.Row(14).Height = 30;


                //Cabecera de Documento
                hoja.Cells["A1:H1"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                hoja.Cells["A1:H1"].Style.Border.Bottom.Color.SetColor(Color.Black);

                hoja.Cells["B3:H3"].Style.Font.Bold = true;
                hoja.Cells["B3:H3"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                hoja.Cells["B3:H3"].Style.Fill.BackgroundColor.SetColor(Color.DarkGray);
                hoja.Cells["B3:H3"].Style.Border.BorderAround(ExcelBorderStyle.Thin, System.Drawing.Color.White);
                hoja.Cells["B3:H3"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                hoja.Cells["C3"].Value = "Información de Alta de Colaboradores";
                hoja.Cells["C3"].Style.Font.Color.SetColor(Color.White);
                hoja.Cells["C3"].Style.Font.Name = "Arial";
                hoja.Cells["C3"].Style.Font.Size = 16;

                hoja.Cells["A4:H4"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                hoja.Cells["A4:H4"].Style.Border.Bottom.Color.SetColor(Color.Black);


                hoja.Cells["A5:H5"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                hoja.Cells["A5:H5"].Style.Border.Bottom.Color.SetColor(Color.Black);


                hoja.Cells["B7:H7"].Style.Font.Bold = true;
                hoja.Cells["B7:H7"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                hoja.Cells["B7:H7"].Style.Fill.BackgroundColor.SetColor(Color.DarkGray);
                hoja.Cells["B7:H7"].Style.Border.BorderAround(ExcelBorderStyle.Thin, System.Drawing.Color.White);
                hoja.Cells["B7:H7"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                hoja.Cells["B7"].Value = "Una vez completado cargar en el CSC Ecuador";
                hoja.Cells["B7"].Style.Font.Color.SetColor(Color.White);
                hoja.Cells["B7"].Style.Font.Name = "Calibri";
                hoja.Cells["B7"].Style.Font.Size = 12;

                hoja.Cells["B9:H9"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                hoja.Cells["B9:H9"].Style.Fill.BackgroundColor.SetColor(Color.DarkGray);
                hoja.Cells["B9:H9"].Style.Border.BorderAround(ExcelBorderStyle.Thin, System.Drawing.Color.White);
                hoja.Cells["B9:H9"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                hoja.Cells["B9"].Value = "Proyecto:";
                hoja.Cells["B9"].Style.Font.Name = "Calibri";
                hoja.Cells["B9"].Style.Font.Size = 12;
                hoja.Cells["D9"].Value = "ECUADOR";
                hoja.Cells["D9"].Style.Font.Name = "Calibri";
                hoja.Cells["D9"].Style.Font.Size = 10;

                hoja.Cells["B10:H10"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                hoja.Cells["B10:H10"].Style.Fill.BackgroundColor.SetColor(Color.DarkGray);
                hoja.Cells["B10:H10"].Style.Border.BorderAround(ExcelBorderStyle.Thin, System.Drawing.Color.White);
                hoja.Cells["B10:H10"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                hoja.Cells["B10"].Value = "Cargado por:";
                hoja.Cells["B10"].Style.Font.Name = "Calibri";
                hoja.Cells["B10"].Style.Font.Size = 12;
                hoja.Cells["D10"].Value = usuario;
                hoja.Cells["D10"].Style.Font.Name = "Calibri";
                hoja.Cells["D10"].Style.Font.Size = 10;

                hoja.Cells["B11:H11"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                hoja.Cells["B11:H11"].Style.Fill.BackgroundColor.SetColor(Color.DarkGray);
                hoja.Cells["B11:H11"].Style.Border.BorderAround(ExcelBorderStyle.Thin, System.Drawing.Color.White);
                hoja.Cells["B11:H11"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                hoja.Cells["B11"].Value = "Fecha:";
                hoja.Cells["B11"].Style.Font.Name = "Calibri";
                hoja.Cells["B11"].Style.Font.Size = 12;
                hoja.Cells["D11"].Value = fecha;
                hoja.Cells["D11"].Style.Font.Name = "Calibri";
                hoja.Cells["D11"].Style.Font.Size = 10;

                //Cabecera de la tabla de Alta de Colaboradores
                var titleCell = hoja.Cells["B14:I14"]; // Celdas de títulos

                titleCell.Style.Font.Bold = true;
                titleCell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                titleCell.Style.Fill.BackgroundColor.SetColor(Color.DarkGray);
                titleCell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                titleCell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                titleCell.Style.WrapText = true;

                titleCell.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                titleCell.Style.Border.Right.Color.SetColor(Color.White);
                hoja.Cells["B14:H14"].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                hoja.Cells["B14:H14"].Style.Border.Right.Color.SetColor(Color.White);


                hoja.Cells["B14"].Value = "TIPO DE IDENTIFICACIÓN";
                hoja.Cells["C14"].Value = "No. DE IDENTIFICACIÓN";
                hoja.Cells["D14"].Value = "APELLIDOS Y NOMBRES";
                hoja.Cells["E14"].Value = "FECHA INGRESO";
                hoja.Cells["F14"].Value = "MOTIVO DE MEDIDAD";
                hoja.Cells["G14"].Value = "ID SAP";
                hoja.Cells["H14"].Value = "META 4";
                hoja.Cells["I14"].Value = "ID SAP LOCAL";

                foreach (var i in colaboradores)
                {
                    var body = hoja.Cells["B" + row + ":I" + row];
                    body.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    body.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    body.Style.Font.Name = "Calibri";
                    body.Style.Font.Size = 9;
                    body.Style.WrapText = true;
                    body.Style.Border.BorderAround(ExcelBorderStyle.Dotted, System.Drawing.Color.Black);

                    body.Style.Border.Right.Style = ExcelBorderStyle.Dotted;
                    body.Style.Border.Right.Color.SetColor(Color.Black);

                    hoja.Row(row).Height = 24;

                    hoja.Cells["B" + row].Value = i.catalogo_tipo_identificacion_id == null ? null : i.TipoIdentificacion.nombre;
                    hoja.Cells["C" + row].Value = i.numero_identificacion;
                    hoja.Cells["D" + row].Value = i.nombres_apellidos;
                    hoja.Cells["E" + row].Value = String.Format("{0:dd/MM/yyyy}", i.fecha_ingreso);

                    if (i.empleado_id_sap == 0)
                    {
                        foreach (var m in motivos_medida)
                        {
                            if (m.codigo == RRHHCodigos.MOTIVO_MEDIDA_ALTA)
                            {
                                hoja.Cells["F" + row].Value = m.nombre;
                            }
                        }

                    }
                    else
                    {
                        foreach (var m in motivos_medida)
                        {
                            if (m.codigo == RRHHCodigos.MOTIVO_MEDIDA_REINGRESO)
                            {
                                hoja.Cells["F" + row].Value = m.nombre;
                            }
                        }
                    }

                    hoja.Cells["G" + row].Value = null;
                    hoja.Cells["H" + row].Value = null;


                    row++;

                }

                //System.IO.FileInfo filename = new System.IO.FileInfo(@"C:\CPP\Colaboradores\AltaMasiva\Formato" + aux + ".xlsx");
                //excel.SaveAs(filename);

                return excel;
            }
            else
            {
                return null;
            }


        }

        public bool VerificaIdentificacion(string identificacion)
        {
            bool estado = false;
            char[] valced = new char[13];
            int provincia;
            if (identificacion.Length >= 10)
            {
                valced = identificacion.Trim().ToCharArray();
                provincia = int.Parse((valced[0].ToString() + valced[1].ToString()));
                if (provincia > 0 && provincia < 25)
                {
                    if (int.Parse(valced[2].ToString()) < 6)
                    {
                        estado = VerificaCedula(valced);
                    }
                }
            }
            return estado;
        }

        public static bool VerificaCedula(char[] validarCedula)
        {
            int aux = 0, par = 0, impar = 0, verifi;
            for (int i = 0; i < 9; i += 2)
            {
                aux = 2 * int.Parse(validarCedula[i].ToString());
                if (aux > 9)
                    aux -= 9;
                par += aux;
            }
            for (int i = 1; i < 9; i += 2)
            {
                impar += int.Parse(validarCedula[i].ToString());
            }

            aux = par + impar;
            if (aux % 10 != 0)
            {
                verifi = 10 - (aux % 10);
            }
            else
                verifi = 0;
            if (verifi == int.Parse(validarCedula[9].ToString()))
                return true;
            else
                return false;
        }

        public RegistroCivilDto ChangeResultXMLObject(XmlNode node)
        {
            var jsonText = JsonConvert.SerializeXmlNode(node);
            var jo = JObject.Parse(jsonText);
            var result = jo["return"].ToString();
            var RegistroCivilDto = JsonConvert.DeserializeObject<RegistroCivilDto>(result);
            return RegistroCivilDto;
        }

        public List<ColaboradoresDto> GetFiltrosColaboradoresTable(string numero, string nombres, string estado)
        {


            var e = 1;
            var query = Repository.GetAll().Where(a => a.vigente == true && a.es_externo == false);

            if (numero != "")
            {
                query = query.Where(x => x.numero_identificacion.StartsWith(numero));
            }

            if (estado != "")
            {
                query = query.Where(x => x.estado == estado);
            }

            if (nombres != "")
            {
                query = query.Where(x => x.nombres.ToUpper().Contains(nombres.ToUpper()) || x.primer_apellido.ToUpper().Contains(nombres.ToUpper()) || x.segundo_apellido.ToUpper().Contains(nombres.ToUpper()) || (x.primer_apellido + " " + x.segundo_apellido).ToUpper().Contains(nombres));
            }

            var colaborador = (from d in query
                               select new ColaboradoresDto
                               {
                                   Id = d.Id,
                                   fecha_ingreso = d.fecha_ingreso,
                                   catalogo_tipo_identificacion_id = d.catalogo_tipo_identificacion_id,
                                   numero_identificacion = d.numero_identificacion,
                                   primer_apellido = d.primer_apellido,
                                   segundo_apellido = d.segundo_apellido,
                                   nombres = d.nombres,
                                   fecha_nacimiento = d.fecha_nacimiento,
                                   catalogo_genero_id = d.catalogo_genero_id,
                                   PaisId = d.PaisId,
                                   pais_pais_nacimiento_id = d.pais_pais_nacimiento_id,
                                   ContactoId = d.ContactoId,
                                   catalogo_destino_estancia_id = d.catalogo_destino_estancia_id,
                                   vigente = d.vigente,
                                   validacion_cedula = d.validacion_cedula,
                                   numero_legajo_temporal = d.numero_legajo_temporal,
                                   catalogo_grupo_personal_id = d.catalogo_grupo_personal_id,
                                   estado = d.estado,
                                   codigo_dactilar = d.codigo_dactilar,
                                   nombre_identificacion = d.TipoIdentificacion.nombre,
                                   nombre_destino = d.DestinoEstancia.nombre,
                                   nombre_grupo_personal = d.GrupoPersonal.nombre,

                               }).ToList();

            foreach (var i in colaborador)
            {
                i.nro = e++;

                i.apellidos_nombres = i.primer_apellido + ' ' + i.segundo_apellido;
                i.numeroHuellas = this.NumeroHuellas(i.Id);

                if (i.nombre_grupo_personal == "inicial")
                {
                    i.nombre_grupo_personal = "";
                }
                //Catalogo Destino
                if (i.catalogo_destino_estancia_id > 0)
                {
                    i.nombreestancia = _catalogoRepository.GetCatalogo(i.catalogo_destino_estancia_id.Value) != null ? _catalogoRepository.GetCatalogo(i.catalogo_destino_estancia_id.Value).nombre : "";
                }
                i.fechavigenciacolaboradorqr = this.getParametroPorCodigo("PARAMETRO.FECHA.CADUCIDAD.QR").Length > 0 ?

                    DateTime.Now.AddDays(Int32.Parse(this.getParametroPorCodigo("PARAMETRO.FECHA.CADUCIDAD.QR"))).ToString("dd/MM/yyyy HH:mm") : "";

                i.serviciosvigentes = this.ServiciosColaborado(i.Id);
                i.tienereservaactiva = this.colaboradortienereservasactivas(i.Id);
            }

            return colaborador;

        }
        //Numero Huellas
        public int NumeroHuellas(int ColaboradorId)
        {
            var numero = _ColaboradorHuellaRepository.GetAll().Where(c => c.vigente)
                                                             .Where(c => c.colaborador_id == ColaboradorId)
                                                             .ToList()
                                                             .Count();
            return numero;
        }

        public bool VerificarHuellaPrincipalColaborador(int colaboradorid)
        {
            var principal = _ColaboradorHuellaRepository.GetAll().Where(c => c.vigente)
                                                             .Where(c => c.colaborador_id == colaboradorid)
                                                             .Where(c => c.principal)
                                                             .ToList()
                                                             .Count();
            return principal > 0 ? true : false;
        }

        public bool ExisteColaborador(string NumeroIdentificacion)
        {
            var e = Repository.GetAll().Where(c => c.numero_identificacion == NumeroIdentificacion).Where(c => c.estado != "INACTIVO").Where(c => c.vigente).FirstOrDefault();

            if (e != null && e.Id > 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public List<ColaboradoresDto> FiltrosColaboradoresEstado(string numero, string nombres, string estado)
        {


            var e = 1;
            var query = Repository.GetAll().Where(a => a.vigente == true && a.es_externo == false);

            if (numero != "")
            {
                query = query.Where(x => x.numero_identificacion.StartsWith(numero));
            }

            if (estado != "")
            {
                query = query.Where(x => x.estado == estado);
            }

            if (nombres != "")
            {
                query = query.Where(x => x.nombres.ToUpper().Contains(nombres.ToUpper()) || x.primer_apellido.ToUpper().Contains(nombres.ToUpper()) || x.segundo_apellido.ToUpper().Contains(nombres.ToUpper()) || (x.primer_apellido + " " + x.segundo_apellido).ToUpper().Contains(nombres));
            }

            var colaborador = (from d in query
                               select new ColaboradoresDto
                               {
                                   Id = d.Id,
                                   fecha_ingreso = d.fecha_ingreso,
                                   catalogo_tipo_identificacion_id = d.catalogo_tipo_identificacion_id,
                                   numero_identificacion = d.numero_identificacion,
                                   primer_apellido = d.primer_apellido,
                                   segundo_apellido = d.segundo_apellido,
                                   nombres = d.nombres,
                                   fecha_nacimiento = d.fecha_nacimiento,
                                   catalogo_genero_id = d.catalogo_genero_id,
                                   PaisId = d.PaisId,
                                   pais_pais_nacimiento_id = d.pais_pais_nacimiento_id,
                                   ContactoId = d.ContactoId,
                                   catalogo_destino_estancia_id = d.catalogo_destino_estancia_id,
                                   vigente = d.vigente,
                                   validacion_cedula = d.validacion_cedula,
                                   numero_legajo_temporal = d.numero_legajo_temporal,
                                   catalogo_grupo_personal_id = d.catalogo_grupo_personal_id,
                                   estado = d.estado,
                                   codigo_dactilar = d.codigo_dactilar,
                                   nombre_identificacion = d.TipoIdentificacion.nombre,
                                   nombre_destino = d.DestinoEstancia.nombre,
                                   nombre_grupo_personal = d.GrupoPersonal.codigo != "inicial" ? d.GrupoPersonal.nombre : "",
                                   apellidos_nombres = d.primer_apellido + " " + d.segundo_apellido

                               }).OrderByDescending(c => c.fecha_ingreso).ToList();

            foreach (var i in colaborador)
            {
                i.nro = e++;
                i.numeroHuellas = this.NumeroHuellas(i.Id);

            }

            return colaborador;

        }




        public List<ColaboradoresDto> SearchColaboradores(string numero = "", string nombres = "", string estado = "")
        {
            var e = 1;
            var query = Repository.GetAll().Where(a => a.vigente == true && a.es_externo == false);
            if (numero != "")
            {
                query = query.Where(x => x.numero_identificacion.StartsWith(numero));
            }

            if (estado != "")
            {
                query = query.Where(x => x.estado == estado);
            }

            if (nombres != "")
            {
                query = query.Where(x => x.nombres_apellidos.ToUpper().Contains(nombres));
            }

            var colaborador = (from d in query
                               select new ColaboradoresDto
                               {
                                   Id = d.Id,
                                   fecha_ingreso = d.fecha_ingreso,
                                   catalogo_tipo_identificacion_id = d.catalogo_tipo_identificacion_id,
                                   numero_identificacion = d.numero_identificacion,
                                   primer_apellido = d.primer_apellido,
                                   segundo_apellido = d.segundo_apellido,
                                   nombres = d.nombres,
                                   catalogo_destino_estancia_id = d.catalogo_destino_estancia_id,
                                   vigente = d.vigente,
                                   validacion_cedula = d.validacion_cedula,
                                   numero_legajo_temporal = d.numero_legajo_temporal,
                                   catalogo_grupo_personal_id = d.catalogo_grupo_personal_id,
                                   estado = d.estado,
                                   codigo_dactilar = d.codigo_dactilar,
                                   nombre_identificacion = d.TipoIdentificacion.nombre,
                                   nombre_destino = d.DestinoEstancia.nombre,



                               }).ToList();

            foreach (var i in colaborador)
            {
                i.nro = e++;
                i.apellidos_nombres = i.primer_apellido + " " + i.segundo_apellido + " " + i.nombres;
                i.numeroHuellas = this.NumeroHuellas(i.Id);
                i.serviciosvigentes = this.ServiciosColaborado(i.Id);
                //Catalogo Destino
                if (i.catalogo_destino_estancia_id > 0)
                {
                    i.nombreestancia = _catalogoRepository.GetCatalogo(i.catalogo_destino_estancia_id.Value) != null ? _catalogoRepository.GetCatalogo(i.catalogo_destino_estancia_id.Value).nombre : "";
                }
            }

            return colaborador;


        }



        public List<ColaboradoresDto> SearchAllColaboradores(string numero = "", string nombres = "", string estado = "")
        {

            //Huellas

            var Huellas = _ColaboradorHuellaRepository.GetAll().Where(c => c.vigente).ToList();

            //Servicios

            var Servicios = _colaboradorServicioRepository.GetAll().Include(c => c.Catalogo).Where(c => c.vigente).ToList();


            //Data Fil
            var e = 1;
            var query = Repository.GetAll().Where(a => a.vigente == true);
            if (numero != "")
            {
                query = query.Where(x => x.numero_identificacion.StartsWith(numero));
            }

            if (estado != "")
            {
                query = query.Where(x => x.estado == estado);
            }

            if (nombres != "")
            {
                query = query.Where(x => x.nombres_apellidos.ToUpper().Contains(nombres));
            }

            var colaborador = (from d in query
                               select new ColaboradoresDto
                               {
                                   Id = d.Id,
                                   fecha_ingreso = d.fecha_ingreso,
                                   catalogo_tipo_identificacion_id = d.catalogo_tipo_identificacion_id,
                                   numero_identificacion = d.numero_identificacion,
                                   primer_apellido = d.primer_apellido,
                                   segundo_apellido = d.segundo_apellido,
                                   nombres = d.nombres,
                                   catalogo_destino_estancia_id = d.catalogo_destino_estancia_id,
                                   vigente = d.vigente,
                                   validacion_cedula = d.validacion_cedula,
                                   numero_legajo_temporal = d.numero_legajo_temporal,
                                   catalogo_grupo_personal_id = d.catalogo_grupo_personal_id,
                                   estado = d.estado,
                                   codigo_dactilar = d.codigo_dactilar,
                                   nombre_identificacion = d.TipoIdentificacion.nombre,
                                   nombre_destino = d.DestinoEstancia.nombre,
                                   MergeApellidos = d.primer_apellido + " " + d.segundo_apellido,
                                   nombreestancia = d.DestinoEstancia != null ? d.DestinoEstancia.nombre : "",


                               }).ToList();

            foreach (var i in colaborador)
            {
                i.nro = e++;
                i.numeroHuellas = (from h in Huellas where h.colaborador_id == i.Id select h).ToList().Count();
                i.serviciosvigentes = (from s in Servicios where s.ColaboradoresId == i.Id select s).ToList().Count > 0 ?
                String.Join(",", (from s in Servicios where s.ColaboradoresId == i.Id select s.Catalogo.nombre).ToArray()) : "";
            }

            return colaborador;


        }

        public int SearchColaboradorExterno(string numero_identificacion)
        {
            var externo = Repository.GetAll().Where(c => c.vigente)
                                           .Where(c => c.numero_identificacion == numero_identificacion)
                                           .Where(c => c.es_externo.HasValue)
                                           .Where(c => c.es_externo.Value)
                                           .Where(c => c.estado == RRHHCodigos.ESTADO_ACTIVO)
                                           .FirstOrDefault();
            return externo != null && externo.Id > 0 ? externo.Id : 0;
        }

        public bool InactivarColaboradorExterno(int Id)
        {
            var externo = Repository.Get(Id);
            externo.estado = "INACTIVO";
            /* Personal externo inactivado, se registró como colaborador de CPP el FECHA_REGISTRO*/
            var update = Repository.Update(externo);
            return update.Id > 0 ? true : false;
        }

        public string BuscarIdUnicoColaboradores(string numero_identificacion)
        {
            var colaborador = Repository.GetAll()
                                      .Where(c => c.vigente)
                                      .Where(c => c.numero_identificacion == numero_identificacion)
                                      .Where(c => c.es_externo.HasValue)
                                      .Where(c => !c.es_externo.Value)
                                      .FirstOrDefault();

            if (colaborador != null && colaborador.estado == RRHHCodigos.ESTADO_INACTIVO)
            {
                return "INACTIVO";
            }
            if (colaborador != null && colaborador.estado != RRHHCodigos.ESTADO_INACTIVO)
            {
                return "SI";
            }

            return "NO";
        }

        public List<ColaboradoresDto> SearchColaborador(string nro_identificacion, string nombres)
        {
            var ausentismos = _colaboradorausentismo.GetAll()
                                                   .Where(c => c.vigente)
                                                   .ToList();

            var e = 1;
            var query = Repository.GetAll().Where(a => a.vigente)
                                           .Where(a => a.es_externo.HasValue)
                                           .Where(a => !a.es_externo.Value)
                                           .Where(a => a.estado != RRHHCodigos.ESTADO_INACTIVO)
                                             .Where(a => a.estado != RRHHCodigos.ESTADO_ALTAANULADA)
                                           .ToList();
            if (nro_identificacion != null && nro_identificacion != "")
            {
                query = query.Where(x => x.numero_identificacion.StartsWith(nro_identificacion)).ToList();
            }

            if (nombres != null && nombres != "")
            {
                query = query.Where(c => c.segundo_apellido != null)
                             .Where(c => c.nombres_apellidos != null)
                              .Where(x =>
                                    x.nombres_apellidos.ToUpper().Contains(nombres) ||
                                    x.nombres.ToUpper().Contains(nombres) ||
                                    x.primer_apellido.ToUpper().Contains(nombres) ||
                                    x.segundo_apellido.ToUpper().Contains(nombres) ||
                                    (x.primer_apellido + " " + x.segundo_apellido).ToUpper().Contains(nombres)
                                    ).ToList();
            }

            var colaborador = (from d in query
                               select new ColaboradoresDto
                               {
                                   Id = d.Id,
                                   fecha_ingreso = d.fecha_ingreso,
                                   catalogo_tipo_identificacion_id = d.catalogo_tipo_identificacion_id,
                                   numero_identificacion = d.numero_identificacion,
                                   primer_apellido = d.primer_apellido,
                                   segundo_apellido = d.segundo_apellido,
                                   nombres = d.nombres,
                                   fecha_nacimiento = d.fecha_nacimiento,
                                   catalogo_genero_id = d.catalogo_genero_id,
                                   PaisId = d.PaisId,
                                   pais_pais_nacimiento_id = d.pais_pais_nacimiento_id,
                                   ContactoId = d.ContactoId,
                                   catalogo_destino_estancia_id = d.catalogo_destino_estancia_id,
                                   vigente = d.vigente,
                                   validacion_cedula = d.validacion_cedula,
                                   nombres_apellidos = d.nombres_apellidos,
                                   nombre_grupo_personal = d.GrupoPersonal != null ? d.GrupoPersonal.nombre : "",
                                   numero_legajo_temporal = d.numero_legajo_temporal,
                                   estado = d.estado,
                                   nombreestancia = "",
                                   fechavigenciacolaboradorqr = "",
                                   serviciosvigentes = "",
                                   tienereservaactiva = "",
                                   numero_legajo_definitivo = d.numero_legajo_definitivo,
                                   telefono = d.ContactoId.HasValue ? d.Contacto != null ? d.Contacto.celular : "" : "",
                                   nombre_genero = d.Genero.nombre

                               }).ToList();

            foreach (var i in colaborador)
            {
                i.nro = e++;

                i.apellidos_nombres = i.primer_apellido + ' ' + i.segundo_apellido + ' ' + i.nombres;

                var catalogoIdentificacion = _catalogoRepository.GetCatalogo(i.catalogo_tipo_identificacion_id.Value);
                i.nombre_identificacion = catalogoIdentificacion.nombre;

                if (i.vigente)
                {
                    i.nombre_estado = "Activo";
                }
                else
                {
                    i.nombre_estado = "Inactivo";
                }
                i.posee_ausentismos = (from a in ausentismos where a.colaborador_id == i.Id select a).ToList().Count > 0 ? true : false;
                i.posee_ausentismos_vigentes = (from a in ausentismos
                                                where a.colaborador_id == i.Id
                                                where a.estado == "ACTIVO"
                                                select a).ToList().Count > 0 ? true : false;
            }

            return colaborador;

        }

        public ColaboradorModel SimpleDataColaborador(int Id)
        {
            var colaborador = Repository.GetAllIncluding(c => c.TipoIdentificacion, c => c.DestinoEstancia)
                                      .Where(c => c.Id == Id)
                                      .FirstOrDefault();



            if (colaborador != null && colaborador.Id > 0)
            {
                bool Alimentacion = false;
                bool Hospedaje = false;
                bool Transporte = false;
                List<Catalogo> comidas = new List<Catalogo>();
                List<Catalogo> movilizaciones = new List<Catalogo>();

                var servicios = _colaboradorServicioRepository.GetAllIncluding(x => x.Catalogo)
                                                            .Where(x => x.vigente)
                                                            .Where(x => x.ColaboradoresId == colaborador.Id)
                                                            .ToList();
                if (servicios.Count > 0)
                {
                    var a = (from e in servicios where e.Catalogo.codigo == CatalogosCodigos.SERVICIO_ALMUERZO select e).FirstOrDefault();
                    var h = (from e in servicios where e.Catalogo.codigo == CatalogosCodigos.SERVICIO_HOSPEDAJE select e).FirstOrDefault();
                    var t = (from e in servicios where e.Catalogo.codigo == CatalogosCodigos.SERVICIO_TRANSPORTE select e).FirstOrDefault();
                    if (a != null && a.Id > 0)
                    {
                        Alimentacion = true;

                        var tiposcomida = _colcomidarepository.GetAllIncluding(l => l.ColaboradorServicio).Where(l => l.ColaboradorServicioId == a.Id).
                             Select(l => l.tipo_alimentacion_id).ToList();
                        foreach (var co in tiposcomida)
                        {
                            var catalogo = _catarepository.Get(co);
                            if (catalogo != null && catalogo.Id > 0)
                            {
                                comidas.Add(catalogo);
                            }

                        }
                    }
                    if (h != null && h.Id > 0)
                    {
                        Hospedaje = true;
                    }
                    if (t != null && t.Id > 0)
                    {
                        Transporte = true;
                        var transportes = _colmovilizacionrepository.GetAllIncluding(l => l.ColaboradorServicio).Where(l => l.ColaboradorServicioId == t.Id).Select(l => l.catalogo_tipo_movilizacion_id).ToList();
                        foreach (var tr in transportes)
                        {
                            var catalogo = _catarepository.Get(tr);
                            if (catalogo != null && catalogo.Id > 0)
                            {
                                movilizaciones.Add(catalogo);
                            }
                        }
                    }
                }

                var c = new ColaboradorModel()
                {
                    Id = colaborador.Id,
                    tipoIdentificacion = colaborador.TipoIdentificacion != null ? colaborador.TipoIdentificacion.nombre : "",
                    identificacion = colaborador.numero_identificacion,
                    estado = colaborador.estado,
                    idLegajo = colaborador.numero_legajo_definitivo != null ? colaborador.numero_legajo_definitivo : "",
                    idSap = colaborador.candidato_id_sap.HasValue ? "" + colaborador.candidato_id_sap.Value : "",
                    idSapLocal = colaborador.empleado_id_sap_local.HasValue ? "" + colaborador.empleado_id_sap_local.Value : "",
                    nombresCompletos = colaborador.primer_apellido + " " + colaborador.segundo_apellido + " " + colaborador.nombres,
                    tipoColaborador = colaborador.DestinoEstancia != null ? colaborador.DestinoEstancia.nombre : "",
                    Alimentacion = Alimentacion,
                    Hospedaje = Hospedaje,
                    Transporte = Transporte,
                    selectComidas = comidas,
                    selectTansporte = movilizaciones
                };
                return c;

            }
            else
            {
                return new ColaboradorModel();
            }



        }

        public bool SimpleInsertServiceColaborador(ServiceModel c)
        {
            var servicios = _colaboradorServicioRepository.GetAllIncluding(x => x.Catalogo)
                                                            .Where(x => x.ColaboradoresId == c.Id).ToList();
            /*Eliminamos*/
            if (servicios.Count > 0)
            {
                foreach (var s in servicios)
                {
                    var tiposcomida = _colcomidarepository.GetAllIncluding(l => l.ColaboradorServicio).Where(l => l.ColaboradorServicioId == s.Id).ToList();
                    if (tiposcomida.Count > 0)
                    {
                        _colcomidarepository.Delete(tiposcomida);
                    }
                    var transportes = _colmovilizacionrepository.GetAllIncluding(l => l.ColaboradorServicio).Where(l => l.ColaboradorServicioId == s.Id).ToList();
                    if (transportes.Count > 0)
                    {
                        _colmovilizacionrepository.Delete(transportes);
                    }
                }
                _colaboradorServicioRepository.Delete(servicios);
            }
            /*Insertamos*/

            if (c.Alimentacion)
            {
                var catalog_alimentacion = _catarepository.GetAll().Where(z => z.vigente).Where(z => z.codigo == CatalogosCodigos.SERVICIO_ALMUERZO).FirstOrDefault();
                if (catalog_alimentacion != null && catalog_alimentacion.Id > 0)
                {
                    var a = new ColaboradorServicio() { Id = 0, ColaboradoresId = c.Id, ServicioId = catalog_alimentacion.Id, vigente = true };
                    var aid = _colaboradorServicioRepository.InsertAndGetId(a);
                    foreach (var tipoc in c.selectComidas.Distinct())
                    {
                        var comida = new ColaboradoresComida()
                        {
                            Id = 0,
                            ColaboradorServicioId = aid,
                            tipo_alimentacion_id = tipoc
                        };
                        _colcomidarepository.Insert(comida);
                    }
                }

            }
            if (c.Hospedaje)
            {
                var catalog_hospedaje = _catarepository.GetAll().Where(z => z.vigente).Where(z => z.codigo == CatalogosCodigos.SERVICIO_HOSPEDAJE).FirstOrDefault();
                if (catalog_hospedaje != null && catalog_hospedaje.Id > 0)
                {
                    var a = new ColaboradorServicio() { Id = 0, ColaboradoresId = c.Id, ServicioId = catalog_hospedaje.Id, vigente = true };
                    var hid = _colaboradorServicioRepository.InsertAndGetId(a);
                }
            }
            if (c.Transporte)
            {
                var catalog_transporte = _catarepository.GetAll().Where(z => z.vigente).Where(z => z.codigo == CatalogosCodigos.SERVICIO_TRANSPORTE).FirstOrDefault();
                if (catalog_transporte != null && catalog_transporte.Id > 0)
                {
                    var a = new ColaboradorServicio() { Id = 0, ColaboradoresId = c.Id, ServicioId = catalog_transporte.Id, vigente = true };
                    var tid = _colaboradorServicioRepository.InsertAndGetId(a);
                    foreach (var tipoc in c.selectTransporte.Distinct())
                    {
                        var transporte = new ColaboradorMovilizacion()
                        {
                            Id = 0,
                            ColaboradorServicioId = tid,
                            catalogo_tipo_movilizacion_id = tipoc
                        };
                        _colmovilizacionrepository.Insert(transporte);
                    }
                }
            }
            return true;
        }

        public Colaboradores existeColaboradorPrincipal(string numero_identificacion, bool externo)
        {
            var query = _colaboradoresRepository.GetAll().Where(c => c.numero_identificacion == numero_identificacion)
                 .Where(c => c.vigente)
                 .Where(c => c.es_externo == externo)
                 .Where(c => c.estado != RRHHCodigos.ESTADO_INACTIVO).ToList().FirstOrDefault();//Validar para reintegro de Colaboradores
            return query != null ? query : new Colaboradores();
        }

        public string ColaboradorReingresoAsync(int ColaboradorIdUltimo, ColaboradoresDto temp)
        {
            //Copy Colaborador Antiguo

            var e = Repository.GetAll().Where(c => c.Id == ColaboradorIdUltimo).FirstOrDefault();
            string fullname = temp.nombres_apellidos;
            var colaboradorTemporal = new Colaboradores()
            {
                fecha_ingreso = temp.fecha_ingreso,
                estado = RRHHCodigos.ESTADO_TEMPORAL,
                candidato_id_sap = temp.candidato_id_sap,
                primer_apellido = temp.primer_apellido,
                segundo_apellido = temp.segundo_apellido,
                nombres = temp.nombres,
                catalogo_genero_id = temp.catalogo_genero_id,
                catalogo_etnia_id = temp.catalogo_etnia_id,
                PaisId = temp.PaisId,
                pais_pais_nacimiento_id = temp.pais_pais_nacimiento_id,
                catalogo_estado_civil_id = temp.catalogo_estado_civil_id,
                numero_hijos = temp.numero_hijos,
                catalogo_formacion_educativa_id = temp.catalogo_formacion_educativa_id,
                es_sustituto = temp.es_sustituto,
                fecha_sustituto_desde = temp.fecha_sustituto_desde,
                catalogo_encargado_personal_id = temp.catalogo_encargado_personal_id,
                ContratoId = temp.ContratoId,
                catalogo_destino_estancia_id = temp.catalogo_destino_estancia_id,
                catalogo_area_id = null,
                catalogo_sector_id = null,
                catalogo_cargo_id = null,
                catalogo_clase_id = null,
                catalogo_vinculo_laboral_id = null,
                catalogo_asociacion_id = null,
                catalogo_encuadre_id = null,
                catalogo_planes_beneficios_id = null,
                catalogo_plan_beneficios_id = null,
                catalogo_plan_salud_id = null,
                catalogo_cobertura_dependiente_id = null,
                catalogo_apto_medico_id = null,
                catalogo_division_personal_id = null,
                catalogo_subdivision_personal_id = null,
                catalogo_funcion_id = null,
                catalogo_tipo_contrato_id = null,
                catalogo_clase_contrato_id = null,
                fecha_caducidad_contrato = null,
                ejecutor_obra = false,
                catalogo_tipo_nomina_id = null,
                catalogo_periodo_nomina_id = null,
                catalogo_grupo_personal_id = null,
                catalogo_grupo_id = null,
                catalogo_subgrupo_id = null,
                remuneracion_mensual = null,
                ContactoId = e.ContactoId,
                codigo_seguridad_qr = e.codigo_seguridad_qr,
                codigo_dactilar = e.codigo_dactilar,
                AdminRotacionId = e.AdminRotacionId,
                catalogo_banco_id = e.catalogo_banco_id,
                catalogo_codigo_incapacidad_id = e.catalogo_codigo_incapacidad_id,
                catalogo_codigo_siniestro_id = e.catalogo_codigo_siniestro_id,
                catalogo_forma_pago_id = e.catalogo_forma_pago_id,

                catalogo_sitio_trabajo_id = e.catalogo_sitio_trabajo_id,
                catalogo_tipo_cuenta_id = e.catalogo_tipo_cuenta_id,
                catalogo_tipo_identificacion_id = e.catalogo_tipo_identificacion_id,
                catalogo_via_pago_id = e.catalogo_via_pago_id,
                empleado_id_sap = e.empleado_id_sap,
                empresa_id = e.empresa_id,
                es_carga_masiva = e.es_carga_masiva,
                es_externo = e.es_externo,
                empresa_tercero = e.empresa_tercero,
                es_visita = e.es_visita,
                meta4 = e.meta4,
                fecha_alta = e.fecha_alta,
                fecha_carga_masiva = e.fecha_carga_masiva,
                fecha_matrimonio = e.fecha_matrimonio,
                fecha_nacimiento = e.fecha_nacimiento,
                fecha_registro_civil = e.fecha_registro_civil,

                nombres_apellidos = temp.nombres_apellidos,


                validacion_cedula = e.validacion_cedula,
                viene_registro_civil = e.viene_registro_civil,
                vigente = true,
                tiene_ausentismo = e.tiene_ausentismo,
                posicion = e.posicion,
                numero_legajo_temporal = e.numero_legajo_temporal,
                numero_cuenta = e.numero_cuenta,
                numero_identificacion = e.numero_identificacion,
                numero_legajo_definitivo = e.numero_legajo_definitivo,
                es_reingreso = true,
                empleado_id_sap_local = e.empleado_id_sap_local,



            };



            //InactivarColaborador Externo

            var colaboradorInterno = Repository.GetAll().Where(c => c.es_externo.HasValue)
                                                       .Where(c => !c.es_externo.Value)
                                                       .Where(c => c.vigente)
                                                       .Where(c => c.numero_identificacion == colaboradorTemporal.numero_identificacion)
                                                       .Where(c => c.estado != RRHHCodigos.ESTADO_INACTIVO)
                                                       .FirstOrDefault();
            if (colaboradorInterno != null)
            {
                return "EXISTE_COLABORADOR_INTERNO_ACTIVO";
            }
            var colaboradorExterno = Repository.GetAll().Where(c => c.es_externo.HasValue)
                                                        .Where(c => c.es_externo.Value)
                                                        .Where(c => c.vigente)
                                                        .Where(c => c.numero_identificacion == colaboradorTemporal.numero_identificacion)
                                                        .Where(c => c.estado == RRHHCodigos.ESTADO_ACTIVO)
                                                        .FirstOrDefault();


            if (colaboradorExterno != null)
            {
                var colexterno = Repository.Get(colaboradorExterno.Id);
                colexterno.estado = RRHHCodigos.BAJA_INACTIVO;
                colexterno.vigente = false;
                Repository.Update(colexterno);
            }


            colaboradorTemporal.nombres_apellidos = fullname;
            int colaboradorReingresoId = Repository.InsertAndGetId(colaboradorTemporal);


            ///---------------- Clonar Tablas Existentes--------------///

            //ColaboradoresCargasSociales
            var CargasSocialesExistentes = _cargaSocialRepository.GetAll().Where(c => c.vigente).Where(c => c.ColaboradoresId == ColaboradorIdUltimo).ToList();
            if (CargasSocialesExistentes.Count > 0)
            {
                foreach (var cargaSocial in CargasSocialesExistentes)
                {
                    var nuevaCargaSocial = new ColaboradorCargaSocial()
                    {
                        ColaboradoresId = colaboradorReingresoId,
                        estado_civil = cargaSocial.estado_civil,
                        fecha_matrimonio = cargaSocial.fecha_matrimonio,
                        fecha_nacimiento = cargaSocial.fecha_nacimiento,
                        idGenero = cargaSocial.idGenero,
                        idTipoIdentificacion = cargaSocial.idTipoIdentificacion,
                        nombres = cargaSocial.nombres,
                        nombres_apellidos = cargaSocial.nombres_apellidos,
                        nro_identificacion = cargaSocial.nro_identificacion,
                        PaisId = cargaSocial.PaisId,
                        pais_nacimiento = cargaSocial.pais_nacimiento,
                        parentesco_id = cargaSocial.parentesco_id,
                        por_sustitucion = cargaSocial.por_sustitucion,
                        primer_apellido = cargaSocial.primer_apellido,
                        segundo_apellido = cargaSocial.segundo_apellido,
                        viene_registro_civil = cargaSocial.viene_registro_civil,
                        vigente = cargaSocial.vigente

                    };


                    _cargaSocialRepository.Insert(nuevaCargaSocial);
                }
            }
            //ColaboradoresDiscapacidades
            var ColaboradoresDiscapacidades = _colaboradorDiscapacidadRepository.GetAll().Where(c => c.vigente).Where(c => c.ColaboradoresId == ColaboradorIdUltimo).ToList();
            if (ColaboradoresDiscapacidades.Count > 0)
            {
                foreach (var discap in ColaboradoresDiscapacidades)
                {
                    var nuevDiscapacidad = new ColaboradorDiscapacidad()
                    {
                        vigente = discap.vigente,
                        catalogo_porcentaje_id = discap.catalogo_porcentaje_id,
                        catalogo_tipo_discapacidad_id = discap.catalogo_tipo_discapacidad_id,
                        ColaboradorCargaSocialId = discap.ColaboradorCargaSocialId,
                        ColaboradoresId = colaboradorReingresoId,

                    };

                    _colaboradorDiscapacidadRepository.Insert(nuevDiscapacidad);
                }
            }
            //Colaboradores Huellas 
            var ColaboradoresHuellas = _ColaboradorHuellaRepository.GetAll().Where(c => c.vigente).Where(c => c.colaborador_id == ColaboradorIdUltimo).ToList();
            if (ColaboradoresHuellas.Count > 0)
            {
                foreach (var huella in ColaboradoresHuellas)
                {
                    var nuevaHuella = new ColaboradoresHuellaDigital()
                    {
                        vigente = huella.vigente,
                        huella = huella.huella,
                        catalogo_dedo_id = huella.catalogo_dedo_id,
                        colaborador_id = colaboradorReingresoId,
                        fecha_registro = huella.fecha_registro,
                        IsDeleted = huella.IsDeleted,
                        plantilla_base64 = huella.plantilla_base64,
                        principal = huella.principal,

                    };
                    _ColaboradorHuellaRepository.Insert(nuevaHuella);
                }
            }
            // Colaboradores Requisitos No se Clonará
            // Colaboradores Servicios No se Clonará cada contratacion son nuevas condiciones

            // Contactos Emergencias
            var ContactosEmergencias = _contactoEmergencia.GetAll().Where(c => c.ColaboradorId == ColaboradorIdUltimo).ToList();
            if (ContactosEmergencias.Count > 0)
            {
                foreach (var contactoe in ContactosEmergencias)
                {
                    var nContactoEmergencia = new ContactoEmergencia()
                    {
                        Identificacion = contactoe.Identificacion,
                        Direccion = contactoe.Direccion,
                        Nombres = contactoe.Nombres,
                        Celular = contactoe.Celular,
                        ColaboradorId = colaboradorReingresoId,
                        Telefono = contactoe.Telefono,
                        Relacion = contactoe.Relacion,
                        UrbanizacionComuna = contactoe.UrbanizacionComuna,


                    };

                    _contactoEmergencia.Insert(nContactoEmergencia);
                }
            }
            // Capacitaciones
            var Capacitaciones = _capacitacionesRepository.GetAll().Where(c => c.ColaboradoresId == ColaboradorIdUltimo).ToList();
            if (Capacitaciones.Count > 0)
            {
                foreach (var capacitacion in Capacitaciones)
                {
                    var nuevaCapacitacion = new Capacitacion()
                    {
                        ColaboradoresId = capacitacion.ColaboradoresId,
                        CatalogoNombreCapacitacionId = capacitacion.CatalogoNombreCapacitacionId,
                        CatalogoTipoCapacitacionId = capacitacion.CatalogoTipoCapacitacionId,
                        Fecha = capacitacion.Fecha,
                        Fuente = capacitacion.Fuente,
                        Horas = capacitacion.Horas,
                        Observaciones = capacitacion.Observaciones,

                    };

                    _capacitacionesRepository.Insert(nuevaCapacitacion);
                }
            }


            // COLABORADORES RUBRO PARA REINGRESO

            var colaboradorRubroAsignados = _colaboradorRubroIngenieriarepository.GetAll()
                                                                                 .Where(c => c.ColaboradorId == e.Id)
                                                                                 .ToList();



            //colaboradorReingresoId

            if (colaboradorRubroAsignados.Count > 0)
            {
                foreach (var colaboradorRubroAntiguo in colaboradorRubroAsignados)
                {
                    var nuevoColaborarRubro = new ColaboradorRubroIngenieria()
                    {
                        Id = 0,
                        ColaboradorId = colaboradorReingresoId,
                        ContratoId = colaboradorRubroAntiguo.ContratoId,
                        FechaInicio = colaboradorRubroAntiguo.FechaInicio,
                        FechaFin = colaboradorRubroAntiguo.FechaFin,
                        RubroId = colaboradorRubroAntiguo.RubroId,
                        Tarifa = colaboradorRubroAntiguo.Tarifa,

                    };
                    _colaboradorRubroIngenieriarepository.Insert(nuevoColaborarRubro);
                }

            }



            //COLABORADORES CERTIFICACION INGENIERIA REINGRESO

            var colaboradorCertificacionIngenieriaDatos = _colaboradorCertificacionIngenieriarepository.GetAll()
                                                                                                       .Where(c => c.ColaboradorId == e.Id)
                                                                                                       .ToList();
            if (colaboradorCertificacionIngenieriaDatos.Count > 0)
            {
                foreach (var colaboradorCertificacionIngenieria in colaboradorCertificacionIngenieriaDatos)
                {
                    var nuevoColaboradorCertificacion = new ColaboradorCertificacionIngenieria()
                    {
                        Id = 0,
                        AplicaViatico = colaboradorCertificacionIngenieria.AplicaViatico,
                        AplicaViaticoDirecto = colaboradorCertificacionIngenieria.AplicaViaticoDirecto,
                        AreaId = colaboradorCertificacionIngenieria.AreaId,
                        CategoriaID = colaboradorCertificacionIngenieria.CategoriaID,
                        ColaboradorId = colaboradorReingresoId,
                        DisciplinaId = colaboradorCertificacionIngenieria.DisciplinaId,
                        EsGastoDirecto = colaboradorCertificacionIngenieria.EsGastoDirecto,
                        EsJornal = colaboradorCertificacionIngenieria.EsJornal,
                        FechaDesde = colaboradorCertificacionIngenieria.FechaDesde,
                        FechaHasta = colaboradorCertificacionIngenieria.FechaHasta,
                        HorasPorDia = colaboradorCertificacionIngenieria.HorasPorDia,
                        ModalidadId = colaboradorCertificacionIngenieria.ModalidadId,
                        UbicacionId = colaboradorCertificacionIngenieria.UbicacionId
                    };
                    _colaboradorCertificacionIngenieriarepository.Insert(nuevoColaboradorCertificacion);
                }
            }


            return colaboradorReingresoId.ToString();



        }


        public int ContactoIdNuevo(ColaboradoresDto colaboradorTemporal)
        {
            var contactoColaborador = _contactoColaboradoresRepository.GetAll().Where(C => C.Id == colaboradorTemporal.ContactoId.Value).FirstOrDefault();
            if (contactoColaborador != null)
            {

                var nuevo = new Contacto()
                {
                    Id = 0,
                    calle_principal = contactoColaborador.calle_principal,
                    calle_secundaria = contactoColaborador.calle_secundaria,
                    celular = contactoColaborador.celular,
                    codigo_postal = contactoColaborador.codigo_postal,
                    ComunidadId = contactoColaborador.ComunidadId,
                    comunidad_comunidad_laboral_id = contactoColaborador.comunidad_comunidad_laboral_id,
                    correo_electronico = contactoColaborador.correo_electronico,
                    detalle_comunidad = contactoColaborador.detalle_comunidad,
                    detalle_comunidad_laboral = contactoColaborador.detalle_comunidad_laboral,
                    detalle_parroquia = contactoColaborador.detalle_parroquia,
                    detalle_parroquia_laboral = contactoColaborador.detalle_parroquia_laboral,
                    numero = contactoColaborador.numero,
                    ParroquiaId = contactoColaborador.ParroquiaId,
                    parroquia_parroquia_laboral_id = contactoColaborador.parroquia_parroquia_laboral_id,
                    referencia = contactoColaborador.referencia,
                    telefono_convencional = contactoColaborador.telefono_convencional.Trim(),
                    vigente = true

                };



                try
                {
                    int nuevoContactoId = _contactoColaboradoresRepository.InsertAndGetId(nuevo);
                    return nuevoContactoId;
                }
                catch (DbEntityValidationException e)
                {
                    return 0;
                }
            }
            return 0;
        }

        public ExcelPackage ReporteColaboradoresHistoricos()
        {
            ExcelPackage package = new ExcelPackage();
            var workbook = package.Workbook;
            var worksheet = workbook.Worksheets.Add("Reingresos");
            ExcelWorksheet h = package.Workbook.Worksheets[1];


            int count = 1;
            string cell = "A" + count;
            h.Cells[cell].Value = "ID SAP";
            cell = "B" + count;
            h.Cells[cell].Value = "LEGAJO";
            cell = "C" + count;
            h.Cells[cell].Value = "NOMBRES";
            cell = "D" + count;
            h.Cells[cell].Value = "CARGO";
            cell = "E" + count;
            h.Cells[cell].Value = "FECHA NACIMIENTO";
            cell = "F" + count;
            h.Cells[cell].Value = "FECHA INGRESO";
            cell = "G" + count;
            h.Cells[cell].Value = "FECHA SALIDA";
            cell = "H" + count;
            h.Cells[cell].Value = "PROYECTO";
            cell = "I" + count;
            h.Cells[cell].Value = "REINGRESO(SI / NO)";
            cell = "J" + count;
            h.Cells[cell].Value = "MOTIVO DE BAJA(ÚLTIMO)";
            cell = "K" + count;
            h.Cells[cell].Value = "STATUS";

            count++;

            var list = this.ListColaboradoresReingreso();
            cell = "";
            foreach (var i in list)
            {
                cell = "A" + count;
                h.Cells[cell].Value = i.CodigoSap;
                cell = "B" + count;
                h.Cells[cell].Value = i.NumeroLejajo;
                cell = "C" + count;
                h.Cells[cell].Value = i.NombreCompleto;
                cell = "D" + count;
                h.Cells[cell].Value = i.Cargo;
                cell = "E" + count;
                h.Cells[cell].Value = i.FechaNacimiento;
                cell = "F" + count;
                h.Cells[cell].Value = i.FechaUltimoIngreso;
                cell = "G" + count;
                h.Cells[cell].Value = i.Reingresos.Count > 0 ? i.Reingresos.FirstOrDefault().FechaSalida : "";
                cell = "H" + count;
                h.Cells[cell].Value = i.Proyecto;
                cell = "I" + count;
                h.Cells[cell].Value = "SI";
                cell = "J" + count;
                h.Cells[cell].Value = i.Reingresos.Count > 0 ? i.Reingresos.FirstOrDefault().MotivoBaja : "";
                cell = "K" + count;
                h.Cells[cell].Value = i.Estado;

                count++;
            }

            return package;
        }

        public List<ReingresoModel> ListColaboradoresReingreso()
        {
            var query = _colaboradoresRepository.GetAllIncluding(c => c.Area, c => c.Cargo, c => c.TipoIdentificacion, c => c.Proyecto)
                                               .Where(c => c.vigente)
                                               .Where(c => c.es_externo == false)
                                               .Where(c => c.es_reingreso)
                                               .Where(c => c.estado != RRHHCodigos.ESTADO_INACTIVO)
                                               .OrderByDescending(c => c.fecha_ingreso.Value)


                                               .ToList();
            var list = new List<ReingresoModel>();
            foreach (var q in query)
            {
                var data = new ReingresoModel()
                {
                    Area = q.catalogo_area_id.HasValue ? q.Area.nombre : "",
                    Cargo = q.catalogo_cargo_id.HasValue ? q.Cargo.nombre : "",
                    CodigoSap = q.es_externo.HasValue && q.es_externo.Value ? q.numero_identificacion : q.empleado_id_sap.HasValue ? q.empleado_id_sap.Value.ToString() : "",
                    Id = q.Id,
                    Identificacion = q.numero_identificacion,
                    NombreCompleto = q.nombres_apellidos,
                    TipoUsuario = q.es_externo.HasValue ? q.es_externo.Value ? "EXTERNO" : "INTERNO" : "",
                    PrimerApellido = q.primer_apellido,
                    SegundoApellido = q.segundo_apellido,
                    Nombres = q.nombres,
                    esExterno = q.es_externo.HasValue ? q.es_externo.Value : false,
                    Estado = q.estado,
                    FechaUltimoIngreso = q.fecha_ingreso.HasValue ? q.fecha_ingreso.Value.ToShortDateString() : "",
                    NumeroLejajo = q.numero_legajo_temporal,
                    TipoIdentificacion = q.TipoIdentificacion.nombre,
                    NumeroReingresos = 0,
                    Proyecto = q.ContratoId.HasValue ? q.Proyecto != null ? q.Proyecto.nombre : "" : "",
                    FechaNacimiento = q.fecha_nacimiento.HasValue ? q.fecha_nacimiento.Value.ToShortDateString() : "",
                    Reingresos = new List<ReingresoHistoricoModel>()
                };


                list.Add(data);

            }

            foreach (var h in list)
            {
                var querybaja = _colaboradoresBajaRepository.GetAllIncluding(c => c.Colaboradores.Cargo, c => c.MotivoBaja)
                                              .Where(c => c.vigente)
                                              .Where(c => c.Colaboradores.numero_identificacion == h.Identificacion)

              .OrderByDescending(c => c.fecha_baja.Value)
                                              .ToList();

                var listhistorico = new List<ReingresoHistoricoModel>();
                foreach (var q in querybaja)
                {
                    var data = new ReingresoHistoricoModel()
                    {

                        Cargo = q.Colaboradores.catalogo_cargo_id.HasValue ? q.Colaboradores.Cargo.nombre : "",
                        ColaboradorId = q.ColaboradoresId,
                        Estado = Enum.GetName(typeof(BajaEstado), q.estado),
                        FechaSalida = q.fecha_baja.HasValue ? q.fecha_baja.Value.ToShortDateString() : "",
                        FechaUltimoIngreso = q.Colaboradores.fecha_ingreso.HasValue ? q.Colaboradores.fecha_ingreso.Value.ToShortDateString() : "",
                        MotivoBaja = q.catalogo_motivo_baja_id.HasValue && q.MotivoBaja != null ? q.MotivoBaja.nombre : ""
                    };


                    listhistorico.Add(data);

                }

                if (listhistorico.Count > 0)
                {
                    h.Reingresos.AddRange(listhistorico);
                    h.NumeroReingresos = listhistorico.Count;
                }


            }

            return list;
        }


        public bool ValidarPeriodoColaborador(int ColaboradorId, DateTime fechaActualizacion)
        {

            var entidadColaborador = Repository.GetAll().Where(c => c.Id == ColaboradorId).FirstOrDefault();

            if (entidadColaborador != null)
            {         

                var fechaActualizacionDate = fechaActualizacion.Date;
                var bajas = _colaboradoresBajaRepository.GetAllIncluding(x => x.Colaboradores)
                                                      .Where(x => x.Colaboradores.numero_identificacion == entidadColaborador.numero_identificacion)
                                                      .Where(x => x.Colaboradores.fecha_ingreso < entidadColaborador.fecha_ingreso)
                                                      .OrderByDescending(x => x.Colaboradores.fecha_ingreso)
                                                      .ToList();

                var entrePeriodo = (from p in bajas
                                    where fechaActualizacionDate >= p.Colaboradores.fecha_ingreso.Value.Date
                                    where fechaActualizacionDate <= p.fecha_baja.Value.Date
                                    select p).FirstOrDefault();
                if (entrePeriodo != null) {
                    return true;
                }


                
            }
            return false;
        }



        public bool ValidarPeriodoColaboradorReingreso(int ColaboradorId, DateTime fechaActualizacion)
        {

            var entidadColaborador = Repository.GetAll().Where(c => c.Id == ColaboradorId).FirstOrDefault();

            if (entidadColaborador != null)
            {

                var fechaActualizacionDate = fechaActualizacion.Date;
                var bajas = _colaboradoresBajaRepository.GetAllIncluding(x => x.Colaboradores)
                                                      .Where(x => x.Colaboradores.numero_identificacion == entidadColaborador.numero_identificacion)
                                                    //  .Where(x => x.Colaboradores.fecha_ingreso < entidadColaborador.fecha_ingreso)
                                                      .OrderByDescending(x => x.Colaboradores.fecha_ingreso)
                                                      .ToList();

                var entrePeriodo = (from p in bajas
                                    where fechaActualizacionDate >= p.Colaboradores.fecha_ingreso.Value.Date
                                    where fechaActualizacionDate <= p.fecha_baja.Value.Date
                                    select p).FirstOrDefault();
                if (entrePeriodo != null)
                {
                    return true;
                }



            }
            return false;
        }

        public  async  Task<bool> Desactivar(int id, string pass)
        {
            var pass_param = _parametrorepository.GetAll().Where(c => c.Codigo == "ANULAR.ALTA.COLABORADOR").Select(c => c.Valor).FirstOrDefault();

            if (pass_param == pass)
            {

                var query = Repository.GetAll();
                var entity = await query.Where(p => p.Id == id).SingleOrDefaultAsync();

                if (entity == null)
                {
                    var msg = string.Format("No existe colaborador con Id : {0}", id);
                    throw new GenericException(msg, msg);
                }


                    entity.estado ="ALTA ANULADA";
                    entity.fecha_anulacion_alta = DateTime.Now;
            

                var result = await Repository.UpdateAsync(entity);

                return true;

            }
            else
            {
                return false;
            }
        }
        public bool ValidarPassFechaIngreso(string pass)
        {
            var pass_param = _parametrorepository.GetAll().Where(c => c.Codigo == "EDITAR.FECHA.INGRESO.COLABORADOR").Select(c => c.Valor).FirstOrDefault();

            if (pass_param == pass)
            {


                return true;

            }
            else
            {
                return false;
            }
        }


    }
}

