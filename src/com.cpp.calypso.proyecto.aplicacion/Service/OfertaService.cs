using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.aplicacion.Interfaces;
using com.cpp.calypso.proyecto.dominio;
using Microsoft.AspNet.Identity;
using System.Net.Mail;
using System.Net;
using System.Text.RegularExpressions;
using com.cpp.calypso.proyecto.aplicacion.Dto;


namespace com.cpp.calypso.proyecto.aplicacion.Service
{
    public class OfertaAsyncBaseCrudAppService : AsyncBaseCrudAppService<Oferta,
        OfertaDto, PagedAndFilteredResultRequestDto>, IOfertaAsyncBaseCrudAppService
    {
        private readonly IBaseRepository<Wbs> _repositoryWbs;
        private readonly IBaseRepository<AvanceObra> _avancerepo;
        private readonly IBaseRepository<Computo> _repositoryComputo;
        private readonly IBaseRepository<Proyecto> _repositoryProyecto;
        private readonly IBaseRepository<Requerimiento> _repositoryRequerimeinto;
        private readonly IBaseRepository<ProcesoNotificacion> _repositoryProcesoNotificacion;
        private readonly IBaseRepository<Secuencial> _repositorySecuencial;
        private readonly IBaseRepository<Presupuesto> _repositoryPresupuesto;
        private readonly IBaseRepository<WbsPresupuesto> _wbsPresupuestoRepository;
        private readonly IBaseRepository<ComputoPresupuesto> _computoPresupuestoRepository;
        private readonly IdentityEmailMessageService _emailService;

        public OfertaAsyncBaseCrudAppService(
            IBaseRepository<Oferta> repository,
            IBaseRepository<Wbs> repositoryWbs,
            IBaseRepository<Computo> repositoryComputo,
            IBaseRepository<Proyecto> repositoryProyecto,
            IBaseRepository<Requerimiento> repositoryRequerimeinto,
            IBaseRepository<ProcesoNotificacion> repositoryProcesoNotificacion,
            IBaseRepository<Secuencial> repositorySecuencial,
            IBaseRepository<Presupuesto> repositoryPresupuesto,
            IBaseRepository<WbsPresupuesto> wbsPresupuestoRepository,
            IBaseRepository<ComputoPresupuesto> computoPresupuestoRepository,
            IdentityEmailMessageService emailService,
            IBaseRepository<AvanceObra> avancerepo
            ) : base(repository)
        {
            _repositoryWbs = repositoryWbs;
            _repositoryComputo = repositoryComputo;
            _repositoryProyecto = repositoryProyecto;
            _repositoryRequerimeinto = repositoryRequerimeinto;
            _repositoryProcesoNotificacion = repositoryProcesoNotificacion;
            _repositorySecuencial = repositorySecuencial;
            _repositoryPresupuesto = repositoryPresupuesto;
            _wbsPresupuestoRepository = wbsPresupuestoRepository;
            _computoPresupuestoRepository = computoPresupuestoRepository;
            _emailService = emailService;
            _avancerepo = avancerepo;
        }

        public void Aprobar(int idOferta)
        {
            Oferta o = Repository.Get(idOferta);
            o.presupuesto_emitido = true;
            Repository.Update(o);
        }

        public void ActualizarVersion(int idOferta)
        {
            Oferta o = Repository.Get(idOferta);
            var version = o.version[0];
            version++;
            o.version = version.ToString();
            Repository.Update(o);
        }

        public bool EliminarVigencia(int ofertaId)
        {
            var oferta = Repository.Get(ofertaId);
            if (oferta != null)
            {
                oferta.vigente = false;
                Repository.Update(oferta);
                return true;
            }

            return false;

        }

        public List<Oferta> GetOfertas()
        {
            var ofertaQuery = Repository.GetAllIncluding(c => c.Requerimiento, c => c.Requerimiento.Proyecto, c => c.Requerimiento.Proyecto.Contrato, c => c.Requerimiento.Proyecto.Contrato.Cliente, c => c.Requerimiento.Proyecto.Contrato.Empresa).Where(e => e.vigente == true).ToList();
            return ofertaQuery;

        }
        public List<Oferta> GetOfertasDefinitivas()
        {
            var query = Repository.GetAllIncluding(c => c.Requerimiento.Proyecto.Contrato.Cliente)
                .Where(e => e.vigente == true)
                .Where(e => e.es_final == true).ToList();

            return query;
        }

        public List<OfertaDto> TodasOfertasDefiniticas()
        {
            var query = Repository.GetAllIncluding(c => c.Requerimiento.Proyecto.Contrato)
                .Where(e => e.vigente == true)
                .Where(e => e.es_final == true);

            var items = (from o in query
                         select new OfertaDto()
                         {
                             Id = o.Id,
                             contrato_descripcion = o.Requerimiento.Proyecto.Contrato.descripcion,
                             proyecto_codigo = o.Requerimiento.Proyecto.codigo,
                             codigo_requerimiento = o.Requerimiento.codigo,
                             codigo = o.codigo
                         }).ToList();

            return items;
        }
        public int ClonarOferta(int ofertaId, int proyectoId, int requerimientoId)
        {

            var o = Mapper.Map<Oferta>(Repository.Get(ofertaId));

            var Oferta = new OfertaDto()
            {
                Id = 0,
                ProyectoId = proyectoId,
                RequerimientoId = requerimientoId,
                codigo = o.codigo,
                fecha_orden_proceder = o.fecha_orden_proceder,
                version = o.version,
                vigente = o.vigente,
                alcance = o.alcance,
                descripcion = o.descripcion,
                dias_emision_oferta = o.dias_emision_oferta,
                dias_hasta_recepcion_so = o.dias_hasta_recepcion_so,
                es_final = false,
                estado = o.estado,
                estado_oferta = o.estado_oferta,
                fecha_oferta = o.fecha_oferta,
                fecha_pliego = o.fecha_pliego,
                fecha_primer_envio = o.fecha_primer_envio,
                fecha_recepcion_so = o.fecha_recepcion_so,
                fecha_ultima_modificacion = o.fecha_ultima_modificacion,
                fecha_ultimo_envio = o.fecha_ultimo_envio,
                monto_certificado_aprobado_acumulado = o.monto_certificado_aprobado_acumulado,
                monto_ofertado = o.monto_ofertado,
                monto_ofertado_pendiente_aprobacion = o.monto_ofertado_pendiente_aprobacion,
                monto_so_aprobado = o.monto_so_aprobado,
                monto_so_referencia_total = o.monto_so_referencia_total,
                porcentaje_avance = o.porcentaje_avance,
                service_order = o.service_order,
                service_request = o.service_request,
                acta_cierre = o.acta_cierre,
                centro_de_Costos_Id = o.centro_de_Costos_Id,
                codigo_shaya = o.codigo_shaya,
                estatus_de_Ejecucion = o.estatus_de_Ejecucion,
                forma_contratacion = o.forma_contratacion,
                tipo_Trabajo_Id = o.tipo_Trabajo_Id,
                orden_proceder = o.orden_proceder,
                orden_proceder_enviada_por = o.orden_proceder_enviada_por,
                revision_Oferta = o.revision_Oferta,
                Ingenieria = o.Ingenieria,

            };
            var newOferta = Repository.InsertAndGetId(MapToEntity(Oferta));


            var wbsQuery = _repositoryWbs.GetAll();
            var wbs = (from w in wbsQuery
                       where w.vigente == true
                       where w.OfertaId == ofertaId
                       select new WbsDto()
                       {

                           OfertaId = newOferta,
                           es_actividad = w.es_actividad,
                           estado = w.estado,
                           observaciones = w.observaciones,
                           vigente = w.vigente,
                           Id = w.Id,
                           fecha_final = w.fecha_final,
                           fecha_inicial = w.fecha_inicial,
                           id_nivel_codigo = w.id_nivel_codigo,
                           id_nivel_padre_codigo = w.id_nivel_padre_codigo,
                           nivel_nombre = w.nivel_nombre

                       }).ToList();

            foreach (var w in wbs)
            {
                var newWbs = _repositoryWbs.InsertAndGetId(Mapper.Map<Wbs>(w));
                var queryComputo = _repositoryComputo.GetAll();
                var computos = (from c in queryComputo
                                where c.vigente == true
                                where c.WbsId == w.Id
                                select new ComputoDto()
                                {
                                    ItemId = c.ItemId,
                                    WbsId = newWbs,
                                    cantidad = c.cantidad,
                                    costo_total = c.costo_total,
                                    estado = c.estado,
                                    precio_unitario = c.precio_unitario,
                                    fecha_registro = c.fecha_registro,
                                    fecha_actualizacion = c.fecha_actualizacion,

                                    vigente = c.vigente,
                                    codigo_primavera = c.codigo_primavera,
                                }).ToList();
                foreach (var c in computos)
                {
                    var newComputo = _repositoryComputo.InsertAndGetId(Mapper.Map<Computo>(c));
                }
            }
            return newOferta;
        }

        public string ObtenerSecuencial(int proyectoId)
        {
            var proyecto = _repositoryProyecto.Get(proyectoId);
            var count = Repository.GetAllIncluding(o => o.Proyecto)
                .Count(o => o.Proyecto.contratoId == proyecto.contratoId);
            count++;
            var secuencial = String.Format("{0:000000}", count);
            return secuencial;
        }

        public OfertaDto GetOfertaDefinitiva(int requerimientoId)
        {
            var item = Repository.GetAll()
                .Where(o => o.vigente == true)
                .Where(o => o.es_final == true)
                .Where(o => o.RequerimientoId == requerimientoId).SingleOrDefault();

            return MapToEntityDto(item);
        }

        public int ObtenerIdProyecto(int requerimientoId)
        {
            var reqQuery = _repositoryRequerimeinto.GetAll();
            var item = (from r in reqQuery
                        where r.Id == requerimientoId
                        where r.vigente == true
                        select new RequerimientoDto()
                        {
                            ProyectoId = r.ProyectoId
                        }).SingleOrDefault();
            return item.ProyectoId;
        }

        public List<OfertaDto> listarPorProyectoId(int proyectoId)
        {
            var ofertaQuery = Repository.GetAllIncluding(p => p.Proyecto.Contrato.Cliente);
            var items =
            (from o in ofertaQuery
             where o.ProyectoId == proyectoId
             where o.vigente == true
             select new OfertaDto
             {
                 Id = o.Id,
                 Proyecto = o.Proyecto,
                 codigo = o.codigo,
                 fecha_oferta = o.fecha_oferta,
                 estado_oferta = o.estado_oferta,
                 cliente_razon_social = o.Proyecto.Contrato.Cliente.razon_social
             }).ToList();
            return items;
        }

        public List<OfertaDto> ListarPorRequerimiento(int RequerimientoId) // RequerimientoId
        {
            var query = Repository.GetAllIncluding(o => o.Proyecto.codigo);

            var items = (from o in query
                         where o.vigente == true
                         where o.RequerimientoId == RequerimientoId
                         select new OfertaDto()
                         {
                             Id = o.Id,
                             codigo = o.codigo,
                             proyecto_codigo = o.Proyecto.codigo,
                             version = o.version
                         }).ToList();

            return items;
        }

        public List<OfertaDto> ListarPorRequerimientoDefinitivas(int RequerimientoId) // RequerimientoId
        {
            var query = Repository.GetAllIncluding(o => o.Proyecto.codigo);

            var items = (from o in query
                         where o.vigente == true
                         where o.RequerimientoId == RequerimientoId
                         where o.es_final == true
                         select new OfertaDto()
                         {
                             Id = o.Id,
                             codigo = o.codigo,
                             proyecto_codigo = o.Proyecto.codigo,
                             version = o.version,
                             estatus_de_Ejecucion = o.estatus_de_Ejecucion,
                         }).ToList();

            return items;
        }

        public string GetCodigoClienteYProyecto(int OfertaId)
        {
            var oferta = Repository.GetAllIncluding(o => o.Proyecto, c => c.Proyecto.Contrato.Cliente).Where(x => x.Id == OfertaId).Where(x => x.vigente == true).SingleOrDefault();
            var codigo = oferta.Proyecto.Contrato.Cliente.codigoasignado + "-" + oferta.Proyecto.codigo + "-";
            return codigo;
        }

        public OfertaDto getdetalle(int OfertaId)
        {
            var Query = Repository.GetAllIncluding(o => o.Requerimiento, n => n.Proyecto.Contrato.Empresa, c => c.Proyecto.Contrato.Cliente, p => p.Proyecto, co => co.Proyecto.Contrato);
            var item = (from r in Query
                        where r.Id == OfertaId
                        where r.vigente == true
                        select new OfertaDto()
                        {
                            Id = r.Id,
                            ProyectoId = r.ProyectoId,
                            Proyecto = r.Proyecto,
                            codigo = r.codigo,
                            descripcion = r.descripcion,
                            estado = r.estado,
                            vigente = r.vigente,
                            acta_cierre = r.acta_cierre,
                            Requerimiento = r.Requerimiento,
                            alcance = r.alcance,
                            centro_de_Costos_Id = r.centro_de_Costos_Id,
                            codigo_shaya = r.codigo_shaya,
                            dias_emision_oferta = r.dias_emision_oferta,
                            es_final = r.es_final,
                            dias_hasta_recepcion_so = r.dias_hasta_recepcion_so,
                            estado_oferta = r.estado_oferta,
                            estatus_de_Ejecucion = r.estatus_de_Ejecucion,
                            fecha_oferta = r.fecha_oferta,
                            fecha_orden_proceder = r.fecha_orden_proceder,
                            fecha_primer_envio = r.fecha_primer_envio,
                            fecha_pliego = r.fecha_pliego,
                            fecha_recepcion_so = r.fecha_recepcion_so,
                            fecha_ultima_modificacion = r.fecha_ultima_modificacion,
                            fecha_ultimo_envio = r.fecha_ultimo_envio,
                            forma_contratacion = r.forma_contratacion,
                            monto_certificado_aprobado_acumulado = r.monto_certificado_aprobado_acumulado,
                            monto_ofertado = r.monto_ofertado,
                            monto_ofertado_pendiente_aprobacion = r.monto_ofertado_pendiente_aprobacion,
                            monto_so_aprobado = r.monto_so_aprobado,
                            monto_so_referencia_total = r.monto_so_referencia_total,
                            orden_proceder = r.orden_proceder,
                            porcentaje_avance = r.porcentaje_avance,
                            revision_Oferta = r.revision_Oferta,
                            service_order = r.service_order,
                            orden_proceder_enviada_por = r.orden_proceder_enviada_por,
                            version = r.version,
                            service_request = r.service_request,
                            RequerimientoId = r.RequerimientoId,
                            computo_completo = r.computo_completo,
                            tipo_Trabajo_Id = r.tipo_Trabajo_Id,
                            fecha_presupuesto = r.fecha_presupuesto,
                            presupuesto_emitido = r.presupuesto_emitido,
                            Ingenieria = r.Ingenieria
                        }).FirstOrDefault();
            return item;
        }

        public List<OfertaDto> ListarPorContrato(int ContratoId)
        {
            var query = Repository.GetAllIncluding(o => o.Proyecto, o => o.Proyecto.Contrato);

            var items = (from o in query
                         where o.vigente == true
                         where o.Proyecto.contratoId == ContratoId
                         select new OfertaDto()
                         {
                             Id = o.Id,
                             codigo = o.codigo,
                             proyecto_codigo = o.Proyecto.codigo
                         }).ToList();
            return items;
        }

        public async Task<string> FormatoCorreoOferta(int procesoId, int ofertaId, string[] correos)
        {
            var oferta = Repository.Get(ofertaId);
            var proceso = _repositoryProcesoNotificacion.Get(procesoId);

            var descripcion = oferta.descripcion;
            var codigo = oferta.codigo;
            var fecha_oferta = oferta.fecha_oferta;

            var formato = string.Format(proceso.formato, codigo, descripcion, fecha_oferta);
            formato = formato.Replace("\r\r\n", Environment.NewLine);

            /*var msg = new IdentityMessage();
            msg.Body = formato;
            msg.Destination = "patamon1632@gmail.com";

            msg.Subject = "Envío de oferta";
            await _emailService.SendAsync(msg);*/

            // Cambiar a Eval Shaya cuando se envía al cliente
            oferta.estado_oferta = 2028;
            Repository.Update(oferta);

            EnviarNotificacionesAsync(formato, correos, proceso.nombre);

            return "Ok";
        }

        public async Task ActualizarFechaPresupuestoAsync(int ofertaId)
        {
            var oferta = Repository.Get(ofertaId);
            oferta.presupuesto_emitido = true;
            string[] a = new string[2];
            oferta.fecha_presupuesto = DateTime.Today;
            await EnviarNotificacionesAsync("", a, "Envio de oferta");
            Repository.Update(oferta);
        }

        public void ActualizarComputoCompleto(int ofertaId)
        {
            var oferta = Repository.Get(ofertaId);

            oferta.computo_completo = true;
            Repository.Update(oferta);
        }

        public async Task EnviarNotificacionesAsync(string formato, string[] correos, string asunto)
        {
            /*
            // Create the Outlook application by using inline initialization.
            Outlook.Application oApp = new Outlook.Application();

            //Create the new message by using the simplest approach.
            Outlook.MailItem oMsg = (Outlook.MailItem)oApp.CreateItem(Outlook.OlItemType.olMailItem);

            //Add a recipient.
            // TODO: Change the following recipient where appropriate.
            //Microsoft.Office.Interop.Outlook.Recipient oRecip = (Microsoft.Office.Interop.Outlook.Recipient)oMsg.Recipients.Add("patamon1632@gmail.com");
            foreach (var c in correos)
            {
                oMsg.Recipients.Add(c);
            }

            oMsg.Recipients.ResolveAll();

            Outlook.Accounts accounts = oApp.Session.Accounts;
            foreach (Outlook.Account account in accounts)
            {
                // When the e-mail address matches, send the mail.
                if (account.SmtpAddress == "santy_lopez_wow@hotmail.com")
                {
                    oMsg.SendUsingAccount = account;
                    break;
                }
            }
            //Set the basic properties.
            oMsg.Subject = "Envío de oferta";
            oMsg.Body = formato;
            //Microsoft.Office.Interop.Outlook.Recipient recipient = oApp.Session.CreateRecipient("santy_lopez_wow@hotmail.com");
            //oMsg.Sender = recipient.AddressEntry;


            //Add an attachment.
            // TODO: change file path where appropriate
            //String sSource = "C:\\setupxlg.txt";
            //String sDisplayName = "MyFirstAttachment";
            //int iPosition = (int)oMsg.Body.Length + 1;
            //int iAttachType = (int)Microsoft.Office.Interop.Outlook.OlAttachmentType.olByValue;
            //Microsoft.Office.Interop.Outlook.Attachment oAttach = oMsg.Attachments.Add(sSource, iAttachType, iPosition, sDisplayName);

            // If you want to, display the message.
            oMsg.Display(true);  //modal

            //Send the message.
            //oMsg.Save();
            //oMsg.Send();

            //Explicitly release objects.
            //oRecip = null;
            //oAttach = null;
            oMsg = null;
            oApp = null;
            /*MailMessage msg = new MailMessage();
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
            smtp.Send(msg);*/
           
        }

        public List<OfertaDto> listarPorProyectoDefinitivaId(int reqId)
        {
            var ofertaQuery = Repository.GetAll();
            var items =
            (from o in ofertaQuery
             where o.RequerimientoId == reqId
             where o.es_final == true
             where o.vigente == true
             select new OfertaDto
             {
                 Id = o.Id,
                 Proyecto = o.Proyecto,
                 codigo = o.codigo,
                 version = o.version,
                 descripcion = o.descripcion,
                 fecha_oferta = o.fecha_oferta,
                 estado_oferta = o.estado_oferta
             }).ToList();
            return items;
        }

        // Presupuesto

        public string ClonarOfertaPresupuesto(int requerimientoId)
        {
            var requerimiento = _repositoryRequerimeinto.Get(requerimientoId);

            var o = Repository.GetAll()
                .Where(of => of.vigente)
                .Where(of => of.es_final)
                .FirstOrDefault(of => of.RequerimientoId == requerimientoId);

            //var o = Mapper.Map<Oferta>(Repository.Get(ofertaId));

            var Oferta = new OfertaDto()
            {
                Id = 0,
                ProyectoId = o.ProyectoId,
                RequerimientoId = requerimientoId,
                codigo = o.codigo,
                fecha_orden_proceder = o.fecha_orden_proceder,
                version = o.version,
                vigente = o.vigente,
                alcance = o.alcance,
                fecha_registro = o.fecha_registro,
                descripcion = o.descripcion,
                dias_emision_oferta = o.dias_emision_oferta,
                dias_hasta_recepcion_so = o.dias_hasta_recepcion_so,
                es_final = false,
                estado = o.estado,
                estado_oferta = o.estado_oferta,
                fecha_oferta = o.fecha_oferta,
                fecha_pliego = o.fecha_pliego,
                fecha_primer_envio = o.fecha_primer_envio,
                fecha_recepcion_so = o.fecha_recepcion_so,
                fecha_ultima_modificacion = o.fecha_ultima_modificacion,
                fecha_ultimo_envio = o.fecha_ultimo_envio,
                monto_certificado_aprobado_acumulado = o.monto_certificado_aprobado_acumulado,
                monto_ofertado = o.monto_ofertado,
                monto_ofertado_pendiente_aprobacion = o.monto_ofertado_pendiente_aprobacion,
                monto_so_aprobado = o.monto_so_aprobado,
                monto_so_referencia_total = o.monto_so_referencia_total,
                porcentaje_avance = o.porcentaje_avance,
                service_order = o.service_order,
                service_request = o.service_request,
                acta_cierre = o.acta_cierre,
                centro_de_Costos_Id = o.centro_de_Costos_Id,
                codigo_shaya = o.codigo_shaya,
                estatus_de_Ejecucion = o.estatus_de_Ejecucion,
                forma_contratacion = o.forma_contratacion,
                tipo_Trabajo_Id = o.tipo_Trabajo_Id,
                orden_proceder = o.orden_proceder,
                orden_proceder_enviada_por = o.orden_proceder_enviada_por,
                revision_Oferta = o.revision_Oferta,
                Ingenieria = o.Ingenieria,

            };
            var newOferta = Repository.InsertAndGetId(MapToEntity(Oferta));


            var wbsQuery = _repositoryWbs.GetAll();
            var wbs = (from w in wbsQuery
                       where w.vigente == true
                       where w.OfertaId == o.Id
                       select new WbsDto()
                       {
                           OfertaId = newOferta,
                           es_actividad = w.es_actividad,
                           estado = w.estado,
                           observaciones = w.observaciones,
                           vigente = w.vigente,
                           Id = w.Id,
                           fecha_final = w.fecha_final,
                           fecha_inicial = w.fecha_inicial,
                           id_nivel_codigo = w.id_nivel_codigo,
                           id_nivel_padre_codigo = w.id_nivel_padre_codigo,
                           nivel_nombre = w.nivel_nombre

                       }).ToList();

            foreach (var w in wbs)
            {
                var wbId = w.Id;
                w.Id = 0;
                var newWbs = _repositoryWbs.InsertAndGetId(Mapper.Map<Wbs>(w));
                var queryComputo = _repositoryComputo.GetAll();
                var computos = (from c in queryComputo
                                where c.vigente == true
                                where c.WbsId == wbId
                                select new ComputoDto()
                                {
                                    ItemId = c.ItemId,
                                    WbsId = newWbs,
                                    cantidad = c.cantidad,
                                    costo_total = c.costo_total,
                                    estado = c.estado,
                                    precio_unitario = c.precio_unitario,
                                    fecha_registro = c.fecha_registro,
                                    fecha_actualizacion = c.fecha_actualizacion,
                                    vigente = c.vigente,
                                    codigo_primavera = c.codigo_primavera,
                                }).ToList();
                foreach (var c in computos)
                {
                    c.Id = 0;
                    var newComputo = _repositoryComputo.InsertAndGetId(Mapper.Map<Computo>(c));
                }
            }

            var version = o.version[0];
            version++;
            o.version = version.ToString();
            Repository.Update(o);

            return newOferta + "";
        }

        public async Task<int> CrearPresupuesto(OfertaDto presupuesto)
        {
            var p = MapToEntity(presupuesto);
            p.estado_aprobacion = Oferta.Aprobacion.PendienteAprobacion;
            p.estado_emision = Oferta.Emision.EnPreparacion;
            p.version = "A";
            var cod = String.Format("{0:0000}", this.SecuencialPresupuesto(presupuesto.ProyectoId));
            p.codigo = cod;
            p.vigente = true;


            var queryDefinitiva = Repository
                .GetAll()
                .Where(o => o.vigente)
                .Where(o => o.RequerimientoId == presupuesto.RequerimientoId)
                .Count(o => o.es_final);

            p.es_final = queryDefinitiva <= 0;

            var id = await Repository.InsertAndGetIdAsync(p);
            return id;
        }

        public OfertaDto DetallePresupuestoConEnumerable(int OfertaId)
        {
            var presupuesto = Repository.Get(OfertaId);
            var dto = MapToEntityDto(presupuesto);
            dto.NombreEstadoAprobacion = dto.GetDisplayName(dto.estado_aprobacion);
            dto.NombreEstadoEmision = dto.GetDisplayName(dto.estado_emision);
            dto.NombreClase = dto.GetDisplayName(dto.ClaseId.GetValueOrDefault());
            return dto;
        }

        public int SecuencialPresupuesto(int ProyectoId)
        {
            var proyecto = _repositoryProyecto.Get(ProyectoId);
            var query = Repository
                .GetAll()
                .Where(o => o.vigente).Count(o => o.Proyecto.contratoId == proyecto.contratoId);
            query++;
            return query;
        }

        public async Task ActualizarPresupuesto(OfertaDto p)
        {
            var oferta = Repository.Get(p.Id);
            oferta.ClaseId = p.ClaseId;
            oferta.descripcion = p.descripcion;
            oferta.alcance = p.alcance;
            oferta.fecha_registro = p.fecha_registro;
            await Repository.UpdateAsync(oferta);
        }

        public async Task<bool> AprobarPresupuesto(int OfertaId)
        {
            var oferta = Repository.Get(OfertaId);
            if (oferta == null) return false;
            oferta.estado_aprobacion = Oferta.Aprobacion.Aprobado;
            await Repository.UpdateAsync(oferta);
            return true;
        }

        public async Task<bool> DesaprobarPresupuesto(int OfertaId)
        {
            var oferta = Repository.Get(OfertaId);
            if (oferta == null) return false;
            oferta.estado_aprobacion = Oferta.Aprobacion.PendienteAprobacion;
            await Repository.UpdateAsync(oferta);
            return true;
        }



        // Base RDO
        public string CargarPresupuestoInicial(int RequerimientoId)
        {
            var presupuesto = _repositoryPresupuesto.GetAll()
                .Where(o => o.vigente)
                .Where(o => o.RequerimientoId == RequerimientoId)
                .Where(o => o.estado_aprobacion == Presupuesto.EstadoAprobacion.Aprobado)
                .FirstOrDefault(o => o.es_final);

            if (presupuesto == null) return "NO_EXISTE_PRESUPUESTO_DEFINITIVO";

            var presupuestos = _repositoryPresupuesto.GetAll()
                .Where(o => o.vigente)
                .Where(o => o.RequerimientoId == RequerimientoId)
                .Where(o => o.es_final)
                .Where(o => o.estado_aprobacion == Presupuesto.EstadoAprobacion.Aprobado)
                .ToList();

            if (presupuestos.Count > 1) return "EXISTE_MAS_DE_UN_PRESUPUESTO_DEFINITIVO";

            var requerimiento = _repositoryRequerimeinto.Get(RequerimientoId);
            var codigo = this.ObtenerSecuencial(requerimiento.ProyectoId);
            var baseRDO = new Oferta()
            {
                RequerimientoId = RequerimientoId,
                codigo = codigo,
                ProyectoId = requerimiento.ProyectoId,
                es_final = true,
                vigente = true,
                fecha_registro = DateTime.Now,

                estado_oferta = 1,
                estado = 1,
                service_request = false,
                service_order = false,
                monto_so_referencia_total = 0,
                monto_ofertado = 0,
                monto_so_aprobado = 0,
                monto_ofertado_pendiente_aprobacion = 0,
                monto_certificado_aprobado_acumulado = 0,
                dias_emision_oferta = 0,
                porcentaje_avance = 0,
                version = "A",
                dias_hasta_recepcion_so = 0,
                orden_proceder = false,
                tipo_Trabajo_Id = 0,
                centro_de_Costos_Id = 0,
                estatus_de_Ejecucion = 0,
                forma_contratacion = 0,
                acta_cierre = 0,
                fecha_oferta = presupuesto.fecha_registro
            };

            var existe = Repository.GetAll().Where(c => c.es_final)
                                            .Where(c => c.RequerimientoId == requerimiento.Id)
                                            .Where(c => c.vigente).FirstOrDefault();
            if (existe != null)
            {
                return "BASERDOEXISTE";
            }
            else
            {

                var baseId = Repository.InsertAndGetId(baseRDO);


                var wbsQuery = _wbsPresupuestoRepository.GetAll()
                    .Where(o => o.vigente)
                    .Where(o => o.PresupuestoId == presupuesto.Id);
                var wbs = (from w in wbsQuery
                           select new WbsDto()
                           {
                               OfertaId = baseId,
                               es_actividad = w.es_actividad,
                               estado = w.estado,
                               observaciones = w.observaciones,
                               vigente = w.vigente,
                               Id = w.Id,
                               fecha_final = w.fecha_final,
                               fecha_inicial = w.fecha_inicial,
                               id_nivel_codigo = w.id_nivel_codigo,
                               id_nivel_padre_codigo = w.id_nivel_padre_codigo,
                               nivel_nombre = w.nivel_nombre,
                               DisciplinaId = w.DisciplinaId
                           }).ToList();

                foreach (var w in wbs)
                {
                    var antiguoWbsId = w.Id;
                    var newWbs = _repositoryWbs.InsertAndGetId(Mapper.Map<Wbs>(w));
                    var queryComputo = _computoPresupuestoRepository.GetAll()
                        .Where(o => o.vigente)
                        .Where(o => o.WbsPresupuestoId == antiguoWbsId);
                    var computos = (from c in queryComputo
                                    select new ComputoDto()
                                    {
                                        ItemId = c.ItemId,
                                        WbsId = newWbs,
                                        cantidad = c.cantidad,
                                        costo_total = c.costo_total,
                                        estado = c.estado,
                                        precio_unitario = c.precio_unitario,
                                        fecha_registro = c.fecha_registro,
                                        fecha_actualizacion = c.fecha_actualizacion,
                                        vigente = c.vigente,
                                        codigo_primavera = c.codigo_primavera,
                                        cantidad_eac = c.cantidad,
                                        precio_base = c.precio_base,
                                        precio_ajustado = c.precio_ajustado,
                                        precio_aplicarse = c.precio_aplicarse,
                                        precio_incrementado = c.precio_incrementado,
                                        ref_computo_presupuesto_id = c.Id // referencial al computo de presupuesto

                                    }).ToList();
                    foreach (var c in computos)
                    {
                        c.Id = 0;
                        _repositoryComputo.Insert(Mapper.Map<Computo>(c));
                    }
                }
                //Cancelar Cambios
                var computospresupuesto = _computoPresupuestoRepository.GetAll()
                    .Where(c => c.vigente == true)
                    .Where(c => c.WbsPresupuesto.PresupuestoId == presupuesto.Id).ToList();

                foreach (var item in computospresupuesto)
                {
                    item.Cambio = null;
                    _computoPresupuestoRepository.Update(item);
                }
                return "CREADO";
            }
        }

        // Base RDO
        public string CargarPresupuestoBaseRdo(int RequerimientoId)
        {
            var presupuesto = _repositoryPresupuesto.GetAll()
                .Where(o => o.vigente)
                .Where(o => o.RequerimientoId == RequerimientoId)
                .Where(o => o.estado_aprobacion == Presupuesto.EstadoAprobacion.Aprobado)
                .FirstOrDefault(o => o.es_final);

            if (presupuesto == null) return "NO_EXISTE_PRESUPUESTO_DEFINITIVO";

            var presupuestos = _repositoryPresupuesto.GetAll()
                .Where(o => o.vigente)
                .Where(o => o.RequerimientoId == RequerimientoId)
                .Where(o => o.es_final)
                .Where(o => o.estado_aprobacion == Presupuesto.EstadoAprobacion.Aprobado)
                .ToList();

            if (presupuestos.Count > 1) return "EXISTE_MAS_DE_UN_PRESUPUESTO_DEFINITIVO";

            var requerimiento = _repositoryRequerimeinto.Get(RequerimientoId);
            var codigo = this.ObtenerSecuencial(requerimiento.ProyectoId);
            var baseRDO = new Oferta()
            {
                RequerimientoId = RequerimientoId,
                codigo = codigo,
                ProyectoId = requerimiento.ProyectoId,
                es_final = true,
                vigente = true,
                fecha_registro = DateTime.Now,

                estado_oferta = 1,
                estado = 1,
                service_request = false,
                service_order = false,
                monto_so_referencia_total = 0,
                monto_ofertado = 0,
                monto_so_aprobado = 0,
                monto_ofertado_pendiente_aprobacion = 0,
                monto_certificado_aprobado_acumulado = 0,
                dias_emision_oferta = 0,
                porcentaje_avance = 0,
                version = "A",
                dias_hasta_recepcion_so = 0,
                orden_proceder = false,
                tipo_Trabajo_Id = 0,
                centro_de_Costos_Id = 0,
                estatus_de_Ejecucion = 0,
                forma_contratacion = 0,
                acta_cierre = 0,
                fecha_oferta = presupuesto.fecha_registro
            };

            var existe = Repository.GetAll().Where(c => c.es_final)
                                            .Where(c => c.RequerimientoId == requerimiento.Id)
                                            .Where(c => c.vigente).FirstOrDefault();
            if (existe != null)
            {
                var avances = _avancerepo.GetAll().Where(c => c.OfertaId == existe.Id).ToList();
                if (avances.Count > 0)
                {
                    return "AVANCES";
                }
                else
                {
                    existe.es_final = false;
                    existe.vigente = false;
                    Repository.Update(existe);
                }
               
            }
            else
            {

                var baseId = Repository.InsertAndGetId(baseRDO);


                var wbsQuery = _wbsPresupuestoRepository.GetAll()
                    .Where(o => o.vigente)
                    .Where(o => o.PresupuestoId == presupuesto.Id);
                var wbs = (from w in wbsQuery
                           select new WbsDto()
                           {
                               OfertaId = baseId,
                               es_actividad = w.es_actividad,
                               estado = w.estado,
                               observaciones = w.observaciones,
                               vigente = w.vigente,
                               Id = w.Id,
                               fecha_final = w.fecha_final,
                               fecha_inicial = w.fecha_inicial,
                               id_nivel_codigo = w.id_nivel_codigo,
                               id_nivel_padre_codigo = w.id_nivel_padre_codigo,
                               nivel_nombre = w.nivel_nombre,
                               DisciplinaId = w.DisciplinaId
                           }).ToList();

                foreach (var w in wbs)
                {
                    var antiguoWbsId = w.Id;
                    var newWbs = _repositoryWbs.InsertAndGetId(Mapper.Map<Wbs>(w));
                    var queryComputo = _computoPresupuestoRepository.GetAll()
                        .Where(o => o.vigente)
                        .Where(o => o.WbsPresupuestoId == antiguoWbsId);
                    var computos = (from c in queryComputo
                                    select new ComputoDto()
                                    {
                                        ItemId = c.ItemId,
                                        WbsId = newWbs,
                                        cantidad = c.cantidad,
                                        costo_total = c.costo_total,
                                        estado = c.estado,
                                        precio_unitario = c.precio_unitario,
                                        fecha_registro = c.fecha_registro,
                                        fecha_actualizacion = c.fecha_actualizacion,
                                        vigente = c.vigente,
                                        codigo_primavera = c.codigo_primavera,
                                        cantidad_eac = c.cantidad,
                                        precio_base = c.precio_base,
                                        precio_ajustado = c.precio_ajustado,
                                        precio_aplicarse = c.precio_aplicarse,
                                        precio_incrementado = c.precio_incrementado,
                                        ref_computo_presupuesto_id = c.Id // referencial al computo de presupuesto

                                    }).ToList();
                    foreach (var c in computos)
                    {
                        c.Id = 0;
                        _repositoryComputo.Insert(Mapper.Map<Computo>(c));
                    }
                }
                //Cancelar Cambios
                var computospresupuesto = _computoPresupuestoRepository.GetAll()
                    .Where(c => c.vigente == true)
                    .Where(c => c.WbsPresupuesto.PresupuestoId == presupuesto.Id).ToList();

                foreach (var item in computospresupuesto)
                {
                    item.Cambio = null;
                    _computoPresupuestoRepository.Update(item);
                }
                return "CREADO";
            }
            return "CREADO";
        }

        public string ActualizarCantidadesPresupuestoActual(int RequerimientoId)
        {
            //saco presupuesto
            var presupuesto = _repositoryPresupuesto.GetAll()
                 .Where(o => o.vigente)
                 .Where(o => o.RequerimientoId == RequerimientoId)
                 .Where(o => o.estado_aprobacion == Presupuesto.EstadoAprobacion.Aprobado)
                 .FirstOrDefault(o => o.es_final);

            if (presupuesto == null) return "NO_EXISTE_PRESUPUESTO_DEFINITIVO";

            var presupuestos = _repositoryPresupuesto.GetAll()
                .Where(o => o.vigente)
                .Where(o => o.RequerimientoId == RequerimientoId)
                .Where(o => o.es_final)
                .Where(o => o.estado_aprobacion == Presupuesto.EstadoAprobacion.Aprobado)
                .ToList();

            if (presupuestos.Count > 1) return "EXISTE_MAS_DE_UN_PRESUPUESTO_DEFINITIVO";

            var requerimiento = _repositoryRequerimeinto.Get(RequerimientoId);

            //saco la base rdo definitivo

            var baseRDOdefinitivo = Repository.GetAll()
                .Where(c => c.vigente == true)
                .Where(c => c.es_final == true)
                .Where(c => c.RequerimientoId == requerimiento.Id)
                .FirstOrDefault();

            if (baseRDOdefinitivo == null)
            {
                return "NO_EXISTE_RDO_DEFINITIVO";
            }
            else
            {
                //OBTENER COMPUTOS PRESUPUESTO DEFINITIVO

                var computos_presupuesto = _computoPresupuestoRepository.GetAll()
                     .Where(c => c.vigente == true)
                     .Where(c => c.WbsPresupuesto.PresupuestoId == presupuesto.Id)
                     .ToList();

                //OBTENER COMPUTOS DEL BASE RDO DEFINITIVO ACTUAL

                var computos_rdo = _repositoryComputo.GetAll()
                    .Where(c => c.vigente == true)
                    .Where(c => c.Wbs.OfertaId == baseRDOdefinitivo.Id)
                    .ToList();

                // RECORREO Y ACTUALIZO DATOS
                if (computos_presupuesto.Count > 0)
                {
                    //cp=computos presupuesto
                    foreach (var cp in computos_presupuesto)
                    {

                        var computordo = (from e in computos_rdo
                                          where e.Id == cp.ComputoId
                                          select e).FirstOrDefault();


                        if (computordo != null && computordo.Id > 0)
                        {

                            computordo.cantidad = cp.cantidad;
                            computordo.precio_unitario = cp.precio_unitario;
                            computordo.costo_total = cp.costo_total;
                            computordo.precio_base = cp.precio_base;
                            computordo.precio_incrementado = cp.precio_incrementado;
                            computordo.presupuestado = cp.presupuestado;
                            var actualizado = _repositoryComputo.Update(computordo);


                        }

                    }
                }
                else
                {
                    return "EL_PRESUPUESTO_DEFINITIVO_NO_TIENE_COMPUTOS";
                }

            }
            return "CANTIDADES_ACTUALIZADAS";
        }

        public int EliminarOferta(int id)
        {
            var base_rdo = Repository.Get(id);
            base_rdo.es_final = false;
            base_rdo.vigente = false;
            var update = Repository.Update(base_rdo);
            return base_rdo.RequerimientoId;
        }
    }
}
