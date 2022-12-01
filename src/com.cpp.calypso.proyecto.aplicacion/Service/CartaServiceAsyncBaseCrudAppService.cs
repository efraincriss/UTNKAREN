using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.framework;
using com.cpp.calypso.proyecto.aplicacion.Interfaces;
using com.cpp.calypso.proyecto.dominio;
using com.cpp.calypso.proyecto.dominio.Constantes;
using com.cpp.calypso.proyecto.dominio.Models;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace com.cpp.calypso.proyecto.aplicacion.Service
{
    public class CartaServiceAsyncBaseCrudAppService : AsyncBaseCrudAppService<Carta, CartaDto, PagedAndFilteredResultRequestDto>, ICartaAsyncBaseCrudAppService
    {
        public readonly IBaseRepository<DestinatarioCarta> _repositorydcarta;
        public readonly IBaseRepository<Empresa> _Empresa;
        public readonly IBaseRepository<CartaArchivo> _CartaArchivo;
        public readonly IBaseRepository<Archivo> _archivosRepository;
        public readonly IBaseRepository<Cliente> _Cliente;
        private readonly IdentityEmailMessageService _correoservice;
        private readonly IBaseRepository<CorreoLista> _correslistarepository;
        private readonly IBaseRepository<ListaDistribucion> _listarepository;
        private readonly IBaseRepository<Colaborador> _colaboradorRepository;
        private readonly IBaseRepository<Usuario> _usuarioRepository;
        public CartaServiceAsyncBaseCrudAppService(IBaseRepository<Carta> repository,
            IBaseRepository<DestinatarioCarta> repositorydcarta,
            IBaseRepository<Empresa> Empresa,
            IBaseRepository<CartaArchivo> CartaArchivo,
            IBaseRepository<Archivo> archivosRepository,
            IdentityEmailMessageService correoservice,
            IBaseRepository<CorreoLista> correslistarepository,
       IBaseRepository<Colaborador> colaboradorRepository,
       IBaseRepository<Usuario> usuarioRepository,
       IBaseRepository<ListaDistribucion> listarepository,
        IBaseRepository<Cliente> Cliente) : base(repository)
        {
            _repositorydcarta = repositorydcarta;
            _Empresa = Empresa;
            _Cliente = Cliente;
            _CartaArchivo = CartaArchivo;
            _archivosRepository = archivosRepository;
            _correoservice = correoservice;
            _correslistarepository = correslistarepository;
            _usuarioRepository = usuarioRepository;
            _colaboradorRepository = colaboradorRepository;
            _listarepository = listarepository;
        }

        public int EditCarta(Carta c)
        {
            var e = Repository.Get(c.Id);
            e.dirigidoA = c.dirigidoA;
            e.fecha = c.fecha;
            e.fechaSello = c.fechaSello;
            e.asunto = c.asunto;
            e.descripcion = c.descripcion;
            e.TipoCartaId = c.TipoCartaId;
            e.TipoDestinatarioId = c.TipoDestinatarioId;
            e.enviadoPor = c.enviadoPor;

            if (c.numeroCarta != null && c.numeroCarta.Length > 0)
            {
                e.numeroCarta = c.numeroCarta;
            }
            e.numeroCartaEnviada = c.numeroCartaEnviada;
            e.numeroCartaRecibida = c.numeroCartaRecibida;
            e.requiereRespuesta = c.requiereRespuesta;

            var Id = Repository.Update(e);
            return e.Id;

        }

        public bool EliminarCarta(int id)
        {
            var detalles = _CartaArchivo.GetAll().Where(c => c.CartaId == id).ToList();
            if (detalles.Count > 0)
            {
                _CartaArchivo.Delete(detalles);
            }
            var e = Repository.Get(id);
            Repository.Delete(e);
            return true;
        }

        public bool EliminarVigencia(int CartaId)
        {
            bool resul = false;
            var carta = this.getdetalle(CartaId);
            if (carta != null)
            {
                var lista = _repositorydcarta.GetAll().Where(c => c.CartaId == CartaId).Where(c => c.vigente == true).ToList();
                if (lista.Count > 0)
                {
                    resul = true;
                }
                else
                {
                    carta.vigente = false;
                    Repository.InsertOrUpdate(MapToEntity(carta));
                }

            }

            return resul;
        }

        public List<CartaDto> GetCartaporTipo(int tipo)
        {
            var query = Repository.GetAllIncluding(c => c.TipoCarta, c => c.TipoDestinatario, c => c.Clasificacion);
            var items = (from r in query
                             //where r.TipoCarta_Id == tipo
                         where r.vigente == true
                         select new CartaDto()
                         {
                             Id = r.Id,
                             /* Empresa = r.Empresa,
                              Cliente = r.Cliente,
                              EmpresaId = r.EmpresaId,
                              ClienteId = r.ClienteId,
                              asunto = r.asunto,
                              carta_origen = r.carta_origen,
                              carta_respuesta = r.carta_respuesta,
                              categoria = r.categoria,
                              copia_a = r.copia_a,
                              copia_oculta = r.copia_oculta,
                              descripcion = r.descripcion,
                              dirigido_a = r.dirigido_a,
                              enviado_por = r.enviado_por,
                              fecha_envio = r.fecha_envio,
                              fecha_recepcion = r.fecha_recepcion,
                              id_area_responsable = r.id_area_responsable,
                              numero_carta = r.numero_carta,
                              requiere_respuesta = r.requiere_respuesta,
                              tipo_destinatario = r.tipo_destinatario,
                              estado = r.estado,
                              vigente = r.vigente,
                              tipo = r.tipo*/

                         }).ToList();
            return items;
        }

        public CartaDto getdetalle(int CartaId)
        {
            var list = _listarepository.GetAll().Where(x => x.vigente)
                                      .Where(x => x.codigo == CatalogosCodigos.LISTADISTRIBUCION_CARTAS)
                                      .ToList();

            var c = Repository.GetAllIncluding(x => x.TipoCarta, x => x.TipoDestinatario, x => x.Clasificacion)
                                  .Where(x => x.Id == CartaId).Where(x => x.vigente).FirstOrDefault();
            var carta = new CartaDto()
            {
                Id = c.Id,
                TipoCartaId = c.TipoCartaId,
                numeroCarta = c.numeroCarta,
                TipoDestinatarioId = c.TipoDestinatarioId,
                ClasificacionId = c.ClasificacionId,
                fecha = c.fecha,
                fechaSello = c.fechaSello,
                asunto = c.asunto,
                enviadoPor = c.enviadoPor,
                dirigidoA = c.dirigidoA,
                requiereRespuesta = c.requiereRespuesta,
                numeroCartaRecibida = c.numeroCartaRecibida,
                numeroCartaEnviada = c.numeroCartaEnviada,
                descripcion = c.descripcion,
                linkCarta = c.linkCarta,
                referencia = c.referencia,
                vigente = c.vigente,
                formatFecha = c.fecha.HasValue ? c.fecha.Value.ToShortDateString() : "",
                formatFechaSello = c.fechaSello.HasValue ? c.fechaSello.Value.ToShortDateString() : "",
                nombretipoCarta = c.TipoCarta != null ? c.TipoCarta.nombre : "",
                nombretipoDestinatario = c.TipoDestinatario != null ? c.TipoDestinatario.nombre : "",
                nombreClasificacion = c.Clasificacion != null ? c.Clasificacion.nombre : ""
            };

            if (carta != null && carta.Id > 0)
            {
                carta.listDistribuciones = list;
            }
            /*
                 if (carta.dirigido_a.Length > 0)
                 {
                     List<Colaborador> e = new List<Colaborador>();
                     string[] dirigidos = carta.dirigido_a.Split(',');
                     if (dirigidos.Length > 0)
                     {
                         foreach (var d in dirigidos)
                         {
                             var id = Int32.Parse(d);
                             var colaborador = _colaboradorRepository.GetAllIncluding(c => c.Cliente).Where(c => c.Id == id).FirstOrDefault();
                             if (colaborador != null && colaborador.Id > 0)
                             {
                                 e.Add(colaborador);
                             }
                         }
                         carta.listdirigidos = e;
                         var lista = (from x in e select x.apellidos + " " + x.nombres + " (" + x.correo + ")").ToArray();

                         if (e.Count > 0)
                         {
                             carta.formatdiridigos = String.Join(", ", lista);
                         }
                     }
                 }
                 if (carta.copia_a.Length > 0)
                 {
                     List<Colaborador> e = new List<Colaborador>();
                     string[] copia_a = carta.copia_a.Split(',');
                     if (copia_a.Length > 0)
                     {
                         foreach (var d in copia_a)
                         {
                             var id = Int32.Parse(d);
                             var colaborador = _colaboradorRepository.GetAllIncluding(c => c.Cliente).Where(c => c.Id == id).FirstOrDefault();
                             if (colaborador != null && colaborador.Id > 0)
                             {
                                 e.Add(colaborador);
                             }
                         }
                         carta.listcopia = e;
                         var lista = (from x in e select x.apellidos + " " + x.nombres + " (" + x.correo + ")").ToArray();
                         if (e.Count > 0)
                         {
                             carta.formatcopia = String.Join(", ", lista);
                         }
                     }
                 }
                 if (carta.copia_oculta.Length > 0)
                 {
                     List<Colaborador> e = new List<Colaborador>();
                     string[] copia_oculta = carta.copia_oculta.Split(',');
                     if (copia_oculta.Length > 0)
                     {
                         foreach (var d in copia_oculta)
                         {
                             var id = Int32.Parse(d);
                             var colaborador = _colaboradorRepository.GetAllIncluding(c => c.Cliente).Where(c => c.Id == id).FirstOrDefault();
                             if (colaborador != null && colaborador.Id > 0)
                             {
                                 e.Add(colaborador);
                             }
                         }
                         carta.listcopiaoculta = e;
                         var lista = (from x in e select x.apellidos + " " + x.nombres + " (" + x.correo + ")").ToArray();
                         if (e.Count > 0)
                         {
                             carta.formatcopiaoculta = String.Join(", ", lista);
                         }
                     }
                 }
                 carta.cuerpocarta = this.cuerpocarta(carta.Id);
                 carta.asunto = "Carta " + carta.numero_carta + "-" + carta.descripcion;
             }*/
            return carta;

        }
        public int secuencialCarta()
        {
            int secuencia = 0;
            var listado_codigos = Repository.GetAll().Where(c => c.vigente)
                                                     .Where(c => c.numeroCarta.StartsWith("3808-B-LT-"))
                                                     .Select(c => c.numeroCarta)
                                                     .ToList();
            if (listado_codigos.Count > 0)
            {

                List<int> numeracion = (from l in listado_codigos
                                        select Convert.ToInt32(l.Split('-')[l.Split('-').Length - 1])
                                       ).ToList();

                if (numeracion.Count > 0)
                {
                    secuencia = numeracion.Max() + 1;
                }
                else
                {
                    return 1;
                }
            }
            return secuencia;
        }
        public int InsertCarta(Carta c)
        {

            var Id = Repository.InsertAndGetId(c);
            return Id;

        }

        public List<CartaDto> ListaCartasEmTi(int EmpresaId, int tipo_destinatario, int tipo)
        {
            /*
            var query = Repository.GetAllIncluding(c => c.Empresa, c => c.Cliente);
            var items = (from r in query
                         where r.EmpresaId == EmpresaId
                         where r.tipo_destinatario == tipo_destinatario
                         where r.tipo == tipo
                         where r.vigente == true
                         select new CartaDto()
                         {
                             Id = r.Id,
                             Empresa = r.Empresa,
                             Cliente = r.Cliente,
                             EmpresaId = r.EmpresaId,
                             ClienteId = r.ClienteId,
                             asunto = r.asunto,
                             carta_origen = r.carta_origen,
                             carta_respuesta = r.carta_respuesta,
                             categoria = r.categoria,
                             copia_a = r.copia_a,
                             copia_oculta = r.copia_oculta,
                             descripcion = r.descripcion,
                             dirigido_a = r.dirigido_a,
                             enviado_por = r.enviado_por,
                             fecha_envio = r.fecha_envio,
                             fecha_recepcion = r.fecha_recepcion,
                             id_area_responsable = r.id_area_responsable,
                             numero_carta = r.numero_carta,
                             requiere_respuesta = r.requiere_respuesta,
                             tipo_destinatario = r.tipo_destinatario,
                             estado = r.estado,
                             vigente = r.vigente,
                             tipo = r.tipo

                         }).ToList();

            return items;
            */
            return null;
        }

        public List<CartaDto> ListCarta(int TipoCartaId)
        {
            if (TipoCartaId == 0)
            {
                return new List<CartaDto>();
            }
            else
            {
                var query = Repository.GetAllIncluding(c => c.TipoCarta, c => c.TipoDestinatario, c => c.Clasificacion)
                                                 .Where(c => c.vigente)
                                                 .Where(c => c.TipoCartaId == TipoCartaId)

                                                 .ToList();

                var list = (from c in query
                            select new CartaDto()
                            {
                                Id = c.Id,
                                TipoCartaId = c.TipoCartaId,
                                numeroCarta = c.numeroCarta,
                                TipoDestinatarioId = c.TipoDestinatarioId,
                                ClasificacionId = c.ClasificacionId,
                                fecha = c.fecha,
                                fechaSello = c.fechaSello,
                                asunto = c.asunto,
                                enviadoPor = c.enviadoPor,
                                dirigidoA = c.dirigidoA,
                                requiereRespuesta = c.requiereRespuesta,
                                numeroCartaRecibida = c.numeroCartaRecibida,
                                numeroCartaEnviada = c.numeroCartaEnviada,
                                descripcion = c.descripcion,
                                linkCarta = c.linkCarta,
                                referencia = c.referencia,
                                vigente = c.vigente,
                                formatFecha = c.fecha.HasValue ? c.fecha.Value.ToShortDateString() : "",
                                formatFechaSello = c.fechaSello.HasValue ? c.fechaSello.Value.ToShortDateString() : "",
                                nombretipoCarta = c.TipoCarta != null ? c.TipoCarta.nombre : "",
                                nombretipoDestinatario = c.TipoDestinatario != null ? c.TipoDestinatario.nombre : "",
                                nombreClasificacion = c.Clasificacion != null ? c.Clasificacion.nombre : ""
                            }).OrderByDescending(c=>c.fecha).ToList();

                return list;
            }
        }

        public List<Cliente> ListClientes()
        {
            var list = _Cliente.GetAll().Where(c => c.vigente).ToList();
            return list;

        }

        public List<Empresa> ListEmpresa()
        {
            var list = _Empresa.GetAll().Where(c => c.vigente).ToList();
            return list;
        }

        public bool RegistrarCartasporDestinatario(CartaDto c, int[] destinoseleccionados)
        {
            if (destinoseleccionados.Count() > 0)
            {
                foreach (var item in destinoseleccionados)
                {
                    // c.dirigido_a = "" + item;
                    Repository.Insert(MapToEntity(c));
                }
                return true;
            }
            else
            {
                return false;
            }
        }



        public bool CrearDetalle(CartaArchivo d)
        {

            d.vigente = true;
            var e = _CartaArchivo.InsertAndGetId(d);
            return e > 0 ? true : false;
        }

        public string EditDetalle(CartaArchivo d)
        {


            var e = _CartaArchivo.Get(d.Id);
            e.descripcion = d.descripcion;
            if (e.ArchivoId != d.ArchivoId)
            {
                e.ArchivoId = d.ArchivoId;
            }

            var u = _CartaArchivo.Update(e);
            return u != null && u.Id > 0 ? "OK" : "ERROR";
        }

        public bool DeleteDetalle(int Id)
        {
            var e = _CartaArchivo.Get(Id);
            _CartaArchivo.Delete(e);
            return true;
        }

        public int CrearArchivo(HttpPostedFileBase x)
        {
            if (x != null)
            {
                string fileName = x.FileName;
                string fileContentType = x.ContentType;
                byte[] fileBytes = new byte[x.ContentLength];
                var data = x.InputStream.Read(fileBytes, 0,
                Convert.ToInt32(x.ContentLength));

                Archivo n = new Archivo
                {
                    Id = 0,
                    codigo = "TMITTAL" + 1,
                    nombre = fileName,
                    vigente = true,
                    fecha_registro = DateTime.Now,
                    hash = fileBytes,
                    tipo_contenido = fileContentType,
                };

                var archivoid = _archivosRepository.InsertAndGetId(n);
                return archivoid;
            }
            else
            {
                return 0;
            }
        }


        public async Task<string> Send_Files_Cartas(int Id, int[] ListIds, string body = "")
        {
            /* List<CorreoLista> correoslista = new List<CorreoLista>();

             if (ListIds.Length > 0)
             {

                 foreach (var LId in ListIds)
                 {
                     var correos = _correslistarepository.GetAll().Where(c => c.vigente)
                                             .Where(c => c.ListaDistribucionId == LId)
                                             .ToList();
                     if (correos.Count > 0)
                     {
                         correoslista.AddRange(correos);
                     }

                 }

             }

             string result = "";
             var carta = this.getdetalle(Id);
             if (carta != null && carta.Id > 0)
             {

                 var lista_archivos = _CartaArchivo.GetAllIncluding(c => c.Carta, c => c.Archivo)
                                              .Where(c => c.vigente)
                                             .Where(c => c.CartaId == Id)
                                               .ToList();
                 if (lista_archivos.Count > 0)
                 {


                     if (carta.listdirigidos.Count > 0)
                     {


                         MailMessage message = new MailMessage();
                         SmtpClient SmtpServer = new SmtpClient("smtp.teic.techint.net");

                         SmtpServer.Port = 25;

                         SmtpServer.EnableSsl = false;

                         message.From = new MailAddress("adm_contractual.auca@cpp.com.ec");

                         message.Subject = "PMDIS: Envió Adjuntos Carta " + carta.numero_carta + " " + carta.fecha_envio.ToShortDateString();
                         message.Body = body;
                         foreach (var item in carta.listdirigidos)
                         {
                             message.To.Add(item.correo);
                             ElmahExtension.LogToElmah(new Exception("Send Adjuntos Carta: " + item.correo));
                         }
                         foreach (var item in carta.listcopia)
                         {
                             message.CC.Add(item.correo);
                             ElmahExtension.LogToElmah(new Exception("Send Adjuntos Carta: " + item.correo));
                         }
                         foreach (var item in carta.listcopiaoculta)
                         {
                             message.CC.Add(item.correo);
                             ElmahExtension.LogToElmah(new Exception("Send Adjuntos Carta: " + item.correo));
                         }
                         if (correoslista.Count > 0)
                         {
                             foreach (var item in correoslista)
                             {
                                 message.CC.Add(item.correo);
                                 ElmahExtension.LogToElmah(new Exception("Send Adjuntos Carta: " + item.correo));
                             }
                         }

                         Random a = new Random();
                         var valor = a.Next(1, 1000);

                         foreach (var ar in lista_archivos)
                         {
                             var archivo = ar.Archivo;
                             if (archivo != null)
                             {
                                 //Save the Byte Array as File.
                                 string path = System.Web.HttpContext.Current.Server.MapPath("~/Views/ArchivosProyectos/AdjuntosTransmittals/" + archivo.nombre);
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

             return result;*/
            return null;
        }

        public string cuerpocarta(int Id)
        {
            var carta = Repository.GetAll().Where(c => c.Id == Id).FirstOrDefault();

            //* Nombre Dirigido*//
            var userdirigido = "";
            var dirigido = carta.dirigidoA.Split(',');
            if (dirigido.Length > 0)
            {
                var userid = Int32.Parse(dirigido[0].ToString());
                var colaborador = _colaboradorRepository.GetAll().Where(c => c.Id == userid).FirstOrDefault();
                if (colaborador != null && colaborador.Id > 0)
                {
                    userdirigido = colaborador.nombres;
                }

            }

            string cuerpo = "Estimad@ " + userdirigido + ",\n\n";
            if (carta != null && carta.Id > 0)
            {
                cuerpo = cuerpo + "Adjunto envío copia de la carta N° " + carta.numeroCarta + " ' " + carta.descripcion + " ' la cual recibirán a la brevedad en vuestras oficinas.\n\n" + "Agradezco vuestra gentil atención.";
            }
            return cuerpo;

        }

        public List<Carta> ListadoCartas()
        {
            var list = Repository.GetAll().Where(c => c.vigente).ToList();
            return list;
        }

        public string hrefoutlook(int id, List<int> ListIds)
        {

            List<string> concopia = new List<string>();
            if (ListIds.Count > 0)
            {

                foreach (var LId in ListIds)
                {
                    var correos = _correslistarepository.GetAll().Where(c => c.vigente)
                                            .Where(c => c.ListaDistribucionId == LId)
                                            .OrderBy(c=>c.orden)
                                            .ToList();
                    if (correos.Count > 0)
                    {
                        foreach (var item in correos)
                        {
                            concopia.Add(item.correo);
                        }
                    }

                }

            }

            List<string> para = new List<string>();

            var carta = Repository.GetAll().Where(c => c.Id == id).FirstOrDefault();

            var userdirigido = "";
            var dirigido = carta.dirigidoA.Split(',');


            if (dirigido.Length > 0)
            {
                foreach (var item in dirigido)
                {
                    var data= _correslistarepository.GetAllIncluding().Where(c => c.nombres == item).Where(c => c.ListaDistribucion.codigo == CatalogosCodigos.LISTADISTRIBUCION_CARTAS).Where(c=>c.vigente).FirstOrDefault();
                    if (data != null && data.Id > 0)
                    {
                        para.Add(data.correo);
                        userdirigido = data.nombres;
                    }
                }
            }
            string asunto = "Oficio " + carta.numeroCarta + " - " + '"' + carta.asunto + "" +'"';
            string descrip = "debidamente firmado por nuestra Gerencia de Servicio.";
            string user = System.Web.HttpContext.Current.User.Identity.Name.ToString();

            var usuario = _usuarioRepository.GetAll().Where(c => c.Cuenta == user).FirstOrDefault();

            string href = "mailto:" + String.Join(";", para) + "?subject=" + asunto + "&" + (concopia.Count > 0 ? "CC=" + String.Join(";", concopia) + "&" : "") + "body=Estimado%20" + userdirigido + "%2C%0D%0A%0D%0AAdjunto%20env%C3%ADo%20el%20Oficio%20N%C2%B0%20" + carta.numeroCarta + "%20'" + carta.descripcion + "'%2C%20" + descrip + "%0D%0A%0D%0AAgradezco%20vuestra%20gentil%20atenci%C3%B3n." + "%0A%0ASaludos%20cordiales%2C%0A%0A" + (usuario != null ? usuario.Nombres + "%20" + usuario.Apellidos : "") + "%20%0AAdministraci%C3%B3n%20De%20Contratos"; ;
            return href;
        }

        public List<CartaDto> ListByTipo(int tipo)
        {
            var query = Repository.GetAllIncluding(c => c.TipoCarta, c => c.TipoDestinatario, c => c.Clasificacion)
                                                  // .Where(c => c.TipoCarta_Id == tipo)
                                                  .Where(c => c.vigente)
                                                  .ToList();

            var list = (from c in query
                        select new CartaDto()
                        {
                            Id = c.Id,
                            asunto = c.asunto,
                            /*carta_origen = c.carta_origen,
                            carta_respuesta = c.carta_respuesta,
                            categoria = c.categoria,
                            ClienteId = c.ClienteId,
                            EmpresaId = c.EmpresaId,
                            copia_a = c.copia_a,
                            copia_oculta = c.copia_oculta,
                            descripcion = c.descripcion,
                            dirigido_a = c.dirigido_a,
                            enviado_por = c.enviado_por,
                            estado = c.estado,
                            fecha_envio = c.fecha_envio,
                            fecha_recepcion = c.fecha_recepcion,
                            id_area_responsable = c.id_area_responsable,
                            numero_carta = c.numero_carta,
                            requiere_respuesta = c.requiere_respuesta,
                            tipo = c.tipo,
                            tipo_destinatario = c.tipo_destinatario,
                            vigente = c.vigente,
                            formatFechaEnvio = c.fecha_envio.ToShortDateString(),
                            formatFechaRecepcion = c.fecha_recepcion.HasValue ? c.fecha_recepcion.GetValueOrDefault().ToShortDateString() : " ",
                            nombreEmpresa = c.Empresa.razon_social,
                            nombreCliente = c.Cliente.razon_social,
                            nombreTipo = Enum.GetName(typeof(TipoCarta), c.tipo),
                            nombreTipoDestinatario = Enum.GetName(typeof(TipoDestinatario), c.tipo_destinatario)*/

                        }).ToList().OrderByDescending(c => c.fecha).ToList();

            return list;

        }

        public List<string> ListCartasExistentes()
        {
            var query = Repository.GetAll().Where(c => c.vigente).Select(c => c.numeroCarta).ToList();
            return query;
        }

        public List<ModelClassReactString> ListaDistribucionCartas()
        {
            var query = _correslistarepository.GetAllIncluding()
                                              .Where(c => c.ListaDistribucion.codigo == CatalogosCodigos.LISTADISTRIBUCION_CARTAS)
                                              .Where(c => c.vigente)
                                              .Where(c => c.ListaDistribucion.vigente).ToList();

            var list = (from l in query
                        select new ModelClassReactString()
                        {
                            dataKey = l.Id,
                            label = l.nombres + " (" + l.correo + ")",
                            value = l.nombres

                        }).ToList();
            return list;
        }

        public string UsuarioActual()
        {
            string user = System.Web.HttpContext.Current.User.Identity.Name.ToString();
            var usuario = _usuarioRepository.GetAll().Where(c => c.Cuenta == user).FirstOrDefault();
            return usuario != null ? usuario.NombresCompletos : "";
        }

        public ExcelPackage Reporte()
        {
            throw new NotImplementedException();
        }
    }
}

