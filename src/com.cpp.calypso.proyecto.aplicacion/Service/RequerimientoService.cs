using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using AutoMapper;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.framework;
using com.cpp.calypso.proyecto.aplicacion.Dto;
using com.cpp.calypso.proyecto.aplicacion.Interfaces;
using com.cpp.calypso.proyecto.dominio;
using com.cpp.calypso.proyecto.dominio.Constantes;
using com.cpp.calypso.proyecto.dominio.Models;

namespace com.cpp.calypso.proyecto.aplicacion
{
    public class RequerimientoAsyncBaseCrudAppService :
        AsyncBaseCrudAppService<Requerimiento, RequerimientoDto, PagedAndFilteredResultRequestDto>,
        IRequerimientoAsyncBaseCrudAppService
    {
        private readonly IBaseRepository<Oferta> _repositoryOferta;
        private readonly IBaseRepository<OfertaComercialPresupuesto> _repositoryOCP;
        private readonly IBaseRepository<Proyecto> _repositoryProyecto;
        private readonly IBaseRepository<Secuencial> _secuencialRepository;
        private readonly IBaseRepository<ProcesoNotificacion> _repositoryProcesoNotificacion;

        private readonly IBaseRepository<Presupuesto> _presupuestoRepository;
        private readonly IBaseRepository<Catalogo> _catalogoRepository;
        private readonly IBaseRepository<AvanceObra> _avanceobraRepository;
        private readonly IBaseRepository<CorreoLista> _correslistarepository;
        private readonly IdentityEmailMessageService _correoservice;

        //Archivos avance obra
        private readonly IBaseRepository<Archivo> _archivoRepository;
        private readonly IBaseRepository<ArchivosRequerimiento> _archivoRequerimientoRepository;

        public RequerimientoAsyncBaseCrudAppService(
            IBaseRepository<Requerimiento> repository,
            IBaseRepository<Oferta> repositoryOferta,
            IBaseRepository<Proyecto> repositoryProyecto,
            IBaseRepository<Secuencial> secuencialRepository,
            IBaseRepository<ProcesoNotificacion> repositoryProcesoNotificacion,
            IBaseRepository<Presupuesto> presupuestoRepository,
            IBaseRepository<Catalogo> catalogoRepository,
            IBaseRepository<AvanceObra> avanceobraRepository,
              IBaseRepository<Archivo> archivoRepository,
            IBaseRepository<ArchivosRequerimiento> archivoRequerimientoRepository,
            IdentityEmailMessageService correoservice,
            IBaseRepository<CorreoLista> correslistarepository,
             IBaseRepository<OfertaComercialPresupuesto> repositoryOCP
    ) : base(repository)
        {
            _repositoryOferta = repositoryOferta;
            _repositoryProyecto = repositoryProyecto;
            _secuencialRepository = secuencialRepository;
            _repositoryProcesoNotificacion = repositoryProcesoNotificacion;
            _presupuestoRepository = presupuestoRepository;
            _catalogoRepository = catalogoRepository;
            _avanceobraRepository = avanceobraRepository;
            _archivoRepository = archivoRepository;
            _archivoRequerimientoRepository = archivoRequerimientoRepository;
            _correoservice = correoservice;
            _correslistarepository = correslistarepository;
            _repositoryOCP = repositoryOCP;
        }

        public List<RequerimientoDto> Listar()
        {
            var query = Repository.GetAll()
                .Where(o => o.vigente);

            var lista = (from r in query
                         select new RequerimientoDto()
                         {
                             Id = r.Id,
                             Proyecto = r.Proyecto,
                             codigo = r.codigo,
                             fecha_recepcion = r.fecha_recepcion,
                             descripcion = r.descripcion,
                             cliente = r.Proyecto.Contrato.Cliente.razon_social,
                             proyecto_codigo = r.Proyecto.codigo,
                             contrato_descripcion = r.Proyecto.Contrato.descripcion,
                             estado_presupuesto_actual = r.Catalogo.nombre,
                             ultimo_origen = r.ultimo_origen,
                             ultima_clase = r.ultima_clase,
                             ultima_version = r.ultima_version,
                             alcance = r.alcance,
                             monto_ingenieria = r.monto_ingenieria,
                             monto_construccion = r.monto_construccion,
                             monto_procura = r.monto_procura,
                             fecha_maxima_oferta = r.fecha_maxima_oferta,
                             fecha_carga_cronograma = r.fecha_carga_cronograma,
                             fecha_maxima_presupuesto = r.fecha_maxima_presupuesto,
                         }).ToList();
            return lista;
        }

        public List<RequerimientoDto> ListarporContrato(int Id)
        {
            var query = Repository.GetAll()
                .Where(o => o.vigente).
                Where(o => o.Proyecto.contratoId == Id);

            var lista = (from r in query
                         select new RequerimientoDto()
                         {
                             Id = r.Id,
                             Proyecto = r.Proyecto,
                             codigo = r.codigo,
                             fecha_recepcion = r.fecha_recepcion,
                             descripcion = r.descripcion,
                             cliente = r.Proyecto.Contrato.Cliente.razon_social,
                             proyecto_codigo = r.Proyecto.codigo,
                             contrato_descripcion = r.Proyecto.Contrato.descripcion,
                             estado_presupuesto_actual = r.Catalogo.nombre,
                             ultimo_origen = r.ultimo_origen,
                             ultima_clase = r.ultima_clase,
                             ultima_version = r.ultima_version,
                             alcance = r.alcance,
                             fecha_maxima_oferta = r.fecha_maxima_oferta,
                             fecha_carga_cronograma = r.fecha_carga_cronograma,
                             fecha_maxima_presupuesto = r.fecha_maxima_presupuesto,


                         }).ToList();
            return lista;
        }
        public RequerimientoDto GetDetalles(int requerimientoId)
        {
            var listdisitribucion = _correslistarepository.GetAllIncluding(c => c.ListaDistribucion).Where(c => c.vigente).Where(c => c.ListaDistribucion.codigo == CatalogosCodigos.LISTADISTRIBUCION_REQUERIMIENTO).ToList();
            var requerimientoQuery = Repository.GetAllIncluding(
                o => o.Catalogo,
                o => o.Ofertas, n => n.Novedades, c => c.Proyecto.Contrato.Cliente).Where(c => c.Id == requerimientoId);
            var item = (from r in requerimientoQuery
                        where r.Id == requerimientoId
                        where r.vigente == true
                        select new RequerimientoDto()
                        {
                            Id = r.Id,
                            ProyectoId = r.ProyectoId,
                            Proyecto = r.Proyecto,
                            codigo = r.codigo,
                            descripcion = r.descripcion,
                            estado = r.estado,
                            fecha_recepcion = r.fecha_recepcion,
                            solicitante = r.solicitante,
                            tipo_requerimiento = r.tipo_requerimiento,
                            vigente = r.vigente,
                            cliente = r.Proyecto.Contrato.Cliente.razon_social,
                            dias_plazo_requerimiento = r.Proyecto.Contrato.dias_plazo_oferta_requerimiento,
                            Novedades = r.Novedades.Where(n => n.vigente == true).ToList(), // Primera consulta
                            Ofertas = r.Ofertas.Where(o => o.vigente == true).OrderByDescending(o => o.version).ToList(), // Segunda
                            monto_ingenieria = r.monto_ingenieria,
                            monto_procura = r.monto_procura,
                            monto_construccion = r.monto_construccion,
                            monto_total = r.monto_total,
                            requiere_cronograma = r.requiere_cronograma,
                            fecha_limite_cronograma = r.fecha_limite_cronograma,
                            proyecto_codigo = r.Proyecto.codigo,
                            contrato_descripcion = r.Proyecto.Contrato.descripcion,
                            estado_presupuesto = r.estado_presupuesto,
                            estado_presupuesto_actual = r.Catalogo.nombre,
                            ultima_clase = r.ultima_clase,
                            ultima_version = r.ultima_version,
                            ultimo_origen = r.ultimo_origen,
                            alcance = r.alcance,
                            dias_para_cronograma = r.dias_para_cronograma.Value,
                            dias_para_presupuesto = r.dias_para_presupuesto.Value,
                            dias_para_oferta = r.dias_para_oferta.Value,
                            EstadoOfertaId = r.EstadoOfertaId,



                        }).SingleOrDefault(); // Tercera

            if (item != null && item.Id > 0)
            {
                item.correos_lista_distribucion = String.Join(",", listdisitribucion.Select(c => c.correo));
            }
            return item;
        }

        public string ObtenerSecuencial(int proyectoId)
        {
            var proyecto = _repositoryProyecto.Get(proyectoId);
            var last = Repository.GetAllIncluding(o => o.Proyecto).OrderByDescending(o => o.Id)
                .Where(o => o.tipo_requerimiento == TipoRequerimiento.Adicional)
                .Where(o => o.codigo.StartsWith("ADC"))
                .Where(o => o.vigente)
                .FirstOrDefault();
            // .FirstOrDefault(o => o.Proyecto.contratoId == proyecto.contratoId);
            string codigo = "";
            if (last != null && last.Id > 0)
            {
                codigo = last.codigo;
            }
            else
            {
                codigo = "ADC0000";
            }
            var substring = Regex.Match(codigo, @"(.{4})\s*$").ToString();
            var count = Int32.Parse(substring);
            count++;
            var secuencial = String.Format("{0:0000}", count);
            return secuencial;
        }


        public ProyectoRequerimiento ObtenerMaximoSecuencial()
        {
            ProyectoRequerimiento result = new ProyectoRequerimiento();
            var last = Repository.GetAllIncluding(o => o.Proyecto).OrderByDescending(o => o.Id)
                .Where(o => o.tipo_requerimiento == TipoRequerimiento.Adicional)
                .Where(o => o.codigo.StartsWith("ADC"))
                .Where(o => o.vigente)
                .FirstOrDefault();
            string codigo = "";
            if (last != null && last.Id > 0)
            {
                result.codigoProyecto = last.Proyecto.codigo;
                result.codigoAdicional = last.codigo;

            }

            return result;
        }

        public bool ComprobarExisteCodigo(RequerimientoDto input)
        {
            var r = Repository.Get(input.Id);
            if (input.codigo.Equals(r.codigo))
            {
                return false;
            }
            else
            {
                var p = _repositoryProyecto.Get(input.ProyectoId);
                // Compruebo si existe un requerimiento
                // con el mismo codigo pero diferente id
                var count = Repository.GetAllIncluding(o => o.Proyecto)
                    .Where(o => o.vigente)
                    .Where(o => o.Proyecto.contratoId == p.contratoId)
                    .Where(o => o.codigo == input.codigo)
                    .Count(o => o.Id != input.Id);

                return (count > 0 ? true : false);
            }
        }

        public RequerimientoDto EliminarVigencia(int requerimientoId)
        {
            var requerimiento = this.GetDetalles(requerimientoId);
            if (requerimiento != null)
            {
                if (requerimiento.Novedades.Count > 0 || requerimiento.Ofertas.Count > 0)
                {
                    return new RequerimientoDto()
                    {
                        ProyectoId = requerimiento.ProyectoId
                    };
                }
                else
                {
                    requerimiento.vigente = false;
                    var reqActualizado = Repository.Update(MapToEntity(requerimiento));
                    return MapToEntityDto(reqActualizado);
                }
            }
            return new RequerimientoDto();
        }

        public OfertaDto CrearOfertaParaRequerimiento(int requerimeintoId, int proyectoId)
        {
            var proyecto = _repositoryProyecto.Get(proyectoId);
            OfertaDto oferta = new OfertaDto()
            {
                alcance = "",
                codigo = "Pendiente",
                descripcion = "Prendiente",
                version = "Version por generar",
                fecha_oferta = DateTime.Now,
                fecha_pliego = new DateTime(1990, 1, 1),
                fecha_ultimo_envio = new DateTime(1990, 1, 1),
                fecha_ultima_modificacion = new DateTime(1990, 1, 1),
                fecha_primer_envio = new DateTime(1990, 1, 1),
                fecha_recepcion_so = new DateTime(1990, 1, 1),
                fecha_orden_proceder = new DateTime(1990, 1, 1),
                vigente = true,
                ProyectoId = proyectoId,
                acta_cierre = 0,
                ClaseId = 0,
                codigo_shaya = "Pendiente",
                centro_de_Costos_Id = 0,
                es_final = false,
                dias_emision_oferta = 0,
                tipo_Trabajo_Id = 0,
                estatus_de_Ejecucion = 0,
                revision_Oferta = "Pendiente",
                forma_contratacion = 0,
                dias_hasta_recepcion_so = 0,
                estado = 0,
                estado_oferta = 0,
                monto_ofertado = 0,
                monto_certificado_aprobado_acumulado = 0,
                monto_so_aprobado = 0,
                monto_so_referencia_total = 0,
                monto_ofertado_pendiente_aprobacion = 0,
                orden_proceder = false,
                porcentaje_avance = 0,
                service_request = false,
                service_order = false,
                Id = 0,

            };
            oferta.RequerimientoId = requerimeintoId;

            var o = _repositoryOferta.Insert(Mapper.Map<Oferta>(oferta));

            return Mapper.Map<OfertaDto>(o);

        }

        public bool ExistePrincipal(int proyectoId)
        {
            var requerimientoQuery = Repository.GetAll();
            var item = (from r in requerimientoQuery
                        where r.vigente == true
                        where r.tipo_requerimiento == 0
                        where r.ProyectoId == proyectoId
                        select new RequerimientoDto()
                        {
                            Id = r.Id,
                        }).SingleOrDefault(); // Tercera
            if (item != null)
            {
                return true;
            }

            return false;
        }


        public List<RequerimientoDto> ObtenerRequerimientosDeProyecto(int proyectoId)
        {
            var requerimientoQuery = Repository.GetAll();
            var items = (from r in requerimientoQuery
                         where r.vigente == true
                         where r.ProyectoId == proyectoId
                         select new RequerimientoDto()
                         {
                             Id = r.Id,
                             codigo = r.codigo,
                             descripcion = r.descripcion

                         }).ToList();

            return items;
        }

        public void EnviarNotificaciones(string formato, string[] correos, string asunto)
        {
            MailMessage msg = new MailMessage();
            msg.From = new MailAddress(ConfigurationManager.AppSettings["Abp.Net.Mail.DefaultFromAddress"]);
            msg.Body = formato;
            msg.Subject = asunto;
            msg.IsBodyHtml = true;
            foreach (var c in correos)
            {
                msg.To.Add(c);
            }

            SmtpClient smtp = new SmtpClient();
            smtp.Host = ConfigurationManager.AppSettings["Abp.Net.Mail.Smtp.Host"];

            smtp.EnableSsl = true;
            NetworkCredential nc = new NetworkCredential();
            nc.UserName = msg.From.Address;
            nc.Password = ConfigurationManager.AppSettings["Abp.Net.Mail.Smtp.Password"];

            smtp.Credentials = nc;

            smtp.Port = Int32.Parse(ConfigurationManager.AppSettings["Abp.Net.Mail.Smtp.Port"]);
            //smtp.Send();
        }

        public string FormatoCorreoRequerimientoCreado(int procesoId, int requerimientoId)
        {
            string[] correos = new[]
            {
                "ivan.rojas@atikasoft.com",
                "navirojas06@gmail.com"
            };
            EnviarNotificaciones("Hola Mundo", correos, "Al crear un requerimiento se genere notificación");

            return "";
        }

        public bool cambiar_estado_requerimiento(PresupuestoDto presupuesto, int catalogo)
        {
            var x = Repository.Get(presupuesto.RequerimientoId);
            var p = _presupuestoRepository.Get(presupuesto.Id);
            x.estado_presupuesto = catalogo;
            x.ultima_version = p.version;
            x.ultimo_origen = _catalogoRepository.Get(p.origen.GetValueOrDefault()).nombre.ToString();
            x.ultima_clase = p.Clase.GetValueOrDefault().ToString();
            var resultado = Repository.Update(x);

            if (resultado.Id > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool cambiar_estado_cancelado(int Id)
        {
            var ofertadefinitiva = _repositoryOferta.GetAll().Where(c => c.vigente == true)
                 .Where(c => c.es_final == true)
                 .Where(C => C.RequerimientoId == Id)
                 .FirstOrDefault();

            if (ofertadefinitiva != null && ofertadefinitiva.Id > 0)
            {

                var avances = _avanceobraRepository.GetAll()
                    .Where(c => c.OfertaId == ofertadefinitiva.Id)
                    .Where(c => c.vigente == true)
                    .ToList();

                if (avances.Count > 0)
                {

                    return false;
                }
            }
            else
            {

                var requerimiento = Repository.Get(Id);
                requerimiento.estado = false;
                return true;
            }
            return false;
        }

        public bool cambiar_estado_activado(int Id)
        {
            var requerimiento = Repository.Get(Id);
            requerimiento.estado = true;
            var result = Repository.Update(requerimiento);
            if (result.Id > 0)
            {
                return true;
            }
            else
            {
                return false;

            }

        }


        public int GuardarArchivo(int RequerimientoId, HttpPostedFileBase[] UploadedFile, int tipo)
        {

            if (UploadedFile != null && UploadedFile.Length > 0)
            {
                foreach (var archivo in UploadedFile)
                {


                    var contador = this.ListaArchivos(RequerimientoId).Count + 1;
                    string fileName = archivo.FileName;
                    string fileContentType = archivo.ContentType;
                    byte[] fileBytes = new byte[archivo.ContentLength];
                    var data = archivo.InputStream.Read(fileBytes, 0,
                        Convert.ToInt32(archivo.ContentLength));

                    Archivo n = new Archivo
                    {
                        Id = 0,
                        codigo = "A-REQ-" + contador,
                        nombre = fileName,
                        vigente = true,
                        fecha_registro = DateTime.Now,
                        hash = fileBytes,
                        tipo_contenido = fileContentType,
                    };
                    var archivoid = _archivoRepository.InsertAndGetId(n);

                    if (archivoid > 0)
                    {
                        ArchivosRequerimiento ac = new ArchivosRequerimiento()
                        {
                            Id = 0,
                            RequerimientoId = RequerimientoId,
                            ArchivoId = archivoid,
                            vigente = true,
                            tipo = tipo == 1 ? true : false

                        };
                        var archivoRequerimientoId = _archivoRequerimientoRepository.InsertAndGetId(ac);

                    }
                    else
                    {
                        return 0;

                    }

                }

                return 1;
            }
            return 0;

        }

        public List<ArchivosRequerimientoDto> ListaArchivos(int RequerimientoId)
        {

            var listaarhivos = _archivoRequerimientoRepository.GetAllIncluding(a => a.Archivo);

            var items = (from a in listaarhivos
                         where a.vigente == true
                         where a.RequerimientoId == RequerimientoId
                         select new ArchivosRequerimientoDto
                         {
                             Id = a.Id,
                             ArchivoId = a.ArchivoId,
                             Archivo = a.Archivo,
                             RequerimientoId = a.RequerimientoId,
                             Requerimiento = a.Requerimiento,
                             vigente = a.vigente,
                             tipo = a.tipo
                         }).ToList();

            return items;
        }

        public int EditarArchivo(int ArchivoRequerimientoId, HttpPostedFileBase UploadedFile, int tipo)
        {
            var archivo = _archivoRequerimientoRepository.Get(ArchivoRequerimientoId);
            var contador = this.ListaArchivos(archivo.RequerimientoId).Count + 1;
            if (UploadedFile != null)
            {
                string fileName = UploadedFile.FileName;
                string fileContentType = UploadedFile.ContentType;
                byte[] fileBytes = new byte[UploadedFile.ContentLength];
                var data = UploadedFile.InputStream.Read(fileBytes, 0,
                    Convert.ToInt32(UploadedFile.ContentLength));

                Archivo n = new Archivo
                {
                    Id = 0,
                    codigo = archivo.Archivo.codigo,
                    nombre = fileName,
                    vigente = true,
                    fecha_registro = DateTime.Now,
                    hash = fileBytes,
                    tipo_contenido = fileContentType,
                };
                var archivoid = _archivoRepository.InsertAndGetId(n);

                if (archivoid > 0)
                {
                    archivo.ArchivoId = archivoid;

                    var resultado = _archivoRequerimientoRepository.Update(archivo);

                    return resultado.Id;
                }
                else
                {
                    return 0;

                }
            }
            else
            {
                return 0;
            }

        }

        public int EliminarVigenciaArchivo(int id)
        {
            var archivo = _archivoRequerimientoRepository.Get(id);

            archivo.vigente = false;

            var resultado = _archivoRequerimientoRepository.Update(archivo);

            return resultado.RequerimientoId;
        }

        public ArchivosRequerimientoDto getdetallesarchivo(int id)
        {
            var listaarhivos = _archivoRequerimientoRepository.GetAllIncluding(a => a.Archivo);
            var items = (from a in listaarhivos
                         where a.vigente == true
                         where a.Id == id
                         select new ArchivosRequerimientoDto
                         {
                             Id = a.Id,
                             ArchivoId = a.ArchivoId,
                             Archivo = a.Archivo,
                             RequerimientoId = a.RequerimientoId,
                             Requerimiento = a.Requerimiento,
                             vigente = a.vigente
                         }).FirstOrDefault();

            return items;
        }

        public bool actualizarmontosrequerimiento(int presupuestoId, decimal i = 0, decimal c = 0, decimal s = 0, decimal sub = 0, decimal total = 0)
        {
            var presupuesto = _presupuestoRepository.Get(presupuestoId);
            var requerimiento = Repository.GetAll().Where(x => x.Id == presupuesto.RequerimientoId).FirstOrDefault();

            if (requerimiento != null && requerimiento.Id > 0)
            {
                requerimiento.monto_ingenieria = i;
                requerimiento.monto_construccion = c;
                requerimiento.monto_procura = s;
                requerimiento.monto_subcontrato = sub;
                requerimiento.monto_total = total;

                try
                {
                    presupuesto.monto_ingenieria = i;
                    presupuesto.monto_construccion = c;
                    presupuesto.monto_suministros = s;
                    presupuesto.monto_subcontratos = sub;
                    presupuesto.monto_total = total;
                    _presupuestoRepository.Update(presupuesto);

                    Repository.Update(requerimiento);
                    return true;
                }
                catch (Exception)
                {

                    return false;
                }
            }

            return true;
        }

        public bool ExisteAdicional(string codigo)
        {
            var requerimiento = Repository.GetAll().Where(c => c.codigo == codigo).Where(c => c.vigente).FirstOrDefault();

            return requerimiento != null && requerimiento.Id > 0 ? true : false;
        }

        public async Task<string> Send_Files_Requerimiento(int Id, string asunto = "", string body = "")
        {


            string result = "";
            var requerimiento = this.GetDetalles(Id);
            if (requerimiento != null && requerimiento.Id > 0)
            {
                /* */
                var lista_archivos = _archivoRequerimientoRepository.GetAllIncluding(c => c.Requerimiento, c => c.Archivo)
                                             .Where(c => c.vigente)
                                            .Where(c => c.RequerimientoId == Id)
                                              .ToList();
                if (lista_archivos.Count > 0)
                {
                    var correos_lista = _correslistarepository.GetAll().Where(c => c.vigente)
                                            .Where(c => c.ListaDistribucion.vigente)
                                            .Where(c => c.ListaDistribucion.codigo == CatalogosCodigos.LISTADISTRIBUCION_REQUERIMIENTO).ToList();

                    if (correos_lista.Count > 0)
                    {

                        /* ES: Envio de Archi*/
                        MailMessage message = new MailMessage();
                        SmtpClient SmtpServer = new SmtpClient("smtp.teic.techint.net");

                        SmtpServer.Port = 25;

                        SmtpServer.EnableSsl = false;

                        message.From = new MailAddress("adm_contractual.auca@cpp.com.ec");

                        message.Subject = asunto;
                        message.Body = body;
                        /*string logo = System.Web.HttpContext.Current.Server.MapPath("~/Views/LogosCPP/firmacpp.jpg");

                      if (File.Exists((string)logo))
                       {

                           string messageText = "<p><strong>Danilo Mena F.</strong></p>" +
                                                "<p><strong> Administración De Contratos</strong ></p>" +
                                                "<p><strong>dmena@cpp.com.ec</ strong >" +
                                                "</p><p><strong>PBX: (593) 2 - 298 - 8700</strong></ p >"
                                                 ;
                           message.AlternateViews.Add(CreateHtmlMessage(messageText, logo));
                       }*/


                        message.IsBodyHtml = true;
                        foreach (var item in correos_lista)
                        {
                            message.To.Add(item.correo);
                            ElmahExtension.LogToElmah(new Exception("Send Adjuntos Requermiento: " + item.correo));
                        }


                        Random a = new Random();
                        var valor = a.Next(1, 1000);

                        foreach (var ar in lista_archivos)
                        {
                            var archivo = ar.Archivo;
                            if (archivo != null)
                            {
                                //Save the Byte Array as File.
                                string path = System.Web.HttpContext.Current.Server.MapPath("~/Views/ArchivosProyectos/Requerimientos/" + archivo.nombre);
                                File.WriteAllBytes(path, archivo.hash);
                                if (File.Exists((string)path))
                                {
                                    message.Attachments.Add(new Attachment(path));
                                }

                            }
                        }
                        try
                        {
                            await _correoservice.SendWithFilesAsync(message);

                            return "OK";
                        }
                        catch (Exception e)
                        {

                            return "ERROR";
                        }



                    }
                    else
                    {
                        result = "SIN_CORREOS";
                    }
                }
                else
                {
                    result = "SIN_ARCHIVOS";

                }

            }
            else
            {
                result = "SIN_TRANSMITAL";
            }

            return result;
        }

        public ProyectoRequerimiento CodigoAdicionalActualProyectos()
        {
            ProyectoRequerimiento result = this.ObtenerMaximoSecuencial();

            return result;


        }

        public AlternateView CreateHtmlMessage(string message, string logoPath)
        {
            var inline = new LinkedResource(logoPath);


            var alternateView = AlternateView.CreateAlternateViewFromString(
                                    message,
                                    Encoding.UTF8,
                                    MediaTypeNames.Text.Html);
            alternateView.LinkedResources.Add(inline);

            return alternateView;
        }

        public string hrefoutlook(int id, string to = "", string cc = "", string subject = "")
        {
            List<string> para = new List<string>();
            List<string> concopia = new List<string>();
            //concopia.Add("example@gmail.com");
            var o = Repository.Get(id);//Requerimiento
            var p = _repositoryProyecto.Get(o.ProyectoId);

            var correos_lista = _correslistarepository.GetAll().Where(c => c.vigente)
                                          .Where(c => c.ListaDistribucion.vigente)
                                          .Where(c => c.ListaDistribucion.codigo == CatalogosCodigos.LISTADISTRIBUCION_REQUERIMIENTO)
                                          .OrderBy(c=>c.orden)
                                          .ToList();

            if (correos_lista.Count > 0)
            {
                para.AddRange(correos_lista.Select(c => c.correo).ToList());
            }
            else
            {
                return "#";
            }


            string href = "mailto:" + String.Join(";", para) + "?" + (concopia.Count > 0 ? "CC=" + String.Join(";", concopia) + "&" : "") + "body=Estimados%2C%0D%0A%0D%0ARecibimos%20de%20" + o.solicitante + "%20la%20solicitud%20de%20Trabajos%20Adicionales%20" + o.codigo + "%20para%20el%20Proyecto%20" + p.codigo + " " + p.nombre_proyecto + ".%20El%20Alcance%20se%20resume%20en%3A%0D%0A%0D%0A%20"
                /*+ 
                /Uri.EscapeDataString((o.alcance != null ? o.alcance : ".")) */
                + "%20%0D%0AIngenier%C3%ADa%3A%20Por%20favor%20su%20apoyo%20con%20las%20cantidades%20de%20obra%20correspondientes.%0D%0APresupuestos%3A%20Por%20favor%20su%20apoyo%20con%20el%20presupuesto%20correspondiente.%0D%0APlanificaci%C3%B3n%3A%20Por%20favor%20su%20gentil%20ayuda%20indic%C3%A1ndonos%20el%20impacto%20en%20el%20cronograma.%0D%0A%0D%0A%0D%0AA%20disposici%C3%B3n%20ante%20cualquier%20duda.";


            return href;

        }

        public bool cambiarProyectoReferenciaPresupuesto(int RequerimientoId, int NuevoProyectoId)
        {
            var presupuesto = _presupuestoRepository.GetAll().Where(c => c.RequerimientoId == RequerimientoId).ToList();
            if (presupuesto.Count > 0)
            {
                foreach (var item in presupuesto)
                {
                    var e = _presupuestoRepository.Get(item.Id);
                    e.ProyectoId = NuevoProyectoId;
                    _presupuestoRepository.Update(e);

                }
                return true;

            }
            else
            {
                return false;
            }
        }

        public async Task<bool> ActualizarSolicitanteAsync(RequerimientoDto requerimiento)
        {

            var req = Mapper.Map<Requerimiento>(requerimiento)
            ;
            await Repository.UpdateAsync(req);
            // Repository.Update(req);
            return true;



        }

        public List<RequerimientoDto> RequerimientosyOfertasLigadas(List<Requerimiento> list)
        {
            List<RequerimientoDto> query = new List<RequerimientoDto>();
            var ofertaComercialPresupuestos = _repositoryOCP.GetAllIncluding(c=>c.OfertaComercial).Where(c => c.vigente)
                                                                    .Where(c=>c.OfertaComercial!=null)
                                                                    .ToList();
            foreach (var req in list)
            {
                var rdto = Mapper.Map<RequerimientoDto>(req);
                var ofertaLigada = (from r in ofertaComercialPresupuestos
                                    where r.RequerimientoId == req.Id
                                    select r.OfertaComercial).FirstOrDefault();

                if (ofertaLigada != null)
                {
                    rdto.ofertaComercialLigada = ofertaLigada;
                }
                query.Add(rdto);
            }
            return query;
        }
    }
}
