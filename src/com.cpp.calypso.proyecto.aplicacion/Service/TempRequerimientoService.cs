using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.ModelBinding;
using AutoMapper;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.dominio;
using com.cpp.calypso.proyecto.dominio.Constantes;
using OfficeOpenXml;

namespace com.cpp.calypso.proyecto.aplicacion
{
    public class TempRequerimientoAsyncBaseCrudAppService : AsyncBaseCrudAppService<TempRequerimiento, TempRequerimientoDto, PagedAndFilteredResultRequestDto>, ITempRequerimientoAsyncBaseCrudAppService
    {
        private readonly IBaseRepository<Proyecto> _proyectoRepository;
        private readonly IBaseRepository<Requerimiento> _requerimientoRepository;
        private readonly IBaseRepository<OfertaComercialPresupuesto> _ofertaComercialPresupuestoRepository;
        private readonly IBaseRepository<OfertaComercial> _ofertaComercialRepository;
        private readonly IBaseRepository<TempProyecto> _tempProyectoRepository;
        private readonly IBaseRepository<TempTransmittal> _tempTransmittal;
        private readonly IBaseRepository<TempCartas> _tempCartas;
        private readonly IBaseRepository<TempOs> _tempOs;
        private readonly IdentityEmailMessageService _correoservice;
        private readonly IBaseRepository<OrdenServicio> _ordenServicio;
        private readonly IBaseRepository<DetalleOrdenServicio> _detalleOrden;
        private readonly IBaseRepository<TransmitalCabecera> _transmitalRepository;
        private readonly IBaseRepository<Carta> _cartaRepository;
        private readonly IBaseRepository<Catalogo> _catalogoRepository;
        private readonly IBaseRepository<Colaborador> _tuserRepository;
        private readonly IBaseRepository<SeguimientoComercial> _segComercial;
        public TempRequerimientoAsyncBaseCrudAppService(
            IBaseRepository<TempRequerimiento> repository,
            IBaseRepository<Proyecto> proyectoRepository,
            IBaseRepository<Requerimiento> requerimientoRepository,
            IBaseRepository<OfertaComercialPresupuesto> ofertaComercialPresupuestoRepository,
            IBaseRepository<OfertaComercial> ofertaComercialRepository,
            IBaseRepository<TempProyecto> tempProyectoRepository,
            IBaseRepository<TempTransmittal> tempTransmittal,
          IBaseRepository<TempCartas> tempCartas,
         IBaseRepository<TempOs> tempOs,
        IdentityEmailMessageService correoservice,
        IBaseRepository<OrdenServicio> ordenServicio,
        IBaseRepository<TransmitalCabecera> transmitalRepository,
          IBaseRepository<Carta> cartaRepository,
          IBaseRepository<Catalogo> catalogoRepository,
          IBaseRepository<Colaborador> tuserRepository,
          IBaseRepository<DetalleOrdenServicio> detalleOrden,
          IBaseRepository<SeguimientoComercial> segComercial
            ) : base(repository)
        {
            _proyectoRepository = proyectoRepository;
            _requerimientoRepository = requerimientoRepository;
            _ofertaComercialPresupuestoRepository = ofertaComercialPresupuestoRepository;
            _ofertaComercialRepository = ofertaComercialRepository;
            _tempProyectoRepository = tempProyectoRepository;
            _correoservice = correoservice;
            _tempTransmittal = tempTransmittal;
            _tempCartas = tempCartas;
            _tempOs = tempOs;
            _ordenServicio = ordenServicio;
            _transmitalRepository = transmitalRepository;
            _cartaRepository = cartaRepository;
            _catalogoRepository = catalogoRepository;
            _tuserRepository = tuserRepository;
            _detalleOrden = detalleOrden;
            _segComercial = segComercial;
        }


        public void CargarRequerimientos()
        {
            var items = Repository.GetAll().ToList();

            for (var i = 0; i < 1; i++)
            {
                var temp = items[i];
                var proyecto = this.BuscarProyectoId(temp.ProyectoPrincipal);
                var codigo = "";
                var tipo_requerimiento = new TipoRequerimiento();

                if (temp.Codigo[0] == '0')
                {
                    tipo_requerimiento = TipoRequerimiento.Principal;
                    codigo = temp.Codigo.Substring(1, temp.Codigo.Length - 1);
                }
                else
                {
                    tipo_requerimiento = TipoRequerimiento.Adicional;
                    codigo = temp.Codigo;
                }


                var requerimiento = new RequerimientoDto()
                {

                    ProyectoId = proyecto,
                    tipo_requerimiento = tipo_requerimiento,
                    codigo = codigo,
                    fecha_recepcion = temp.FechaRecepcion,
                    descripcion = temp.Descripcion,
                    solicitante = temp.Solicitante,
                    vigente = true,
                    estado = true,
                    monto_ingenieria = temp.MontoIngenieria,
                    monto_construccion = temp.MontoConstruccion,
                    monto_procura = temp.MontoSuministro,
                    monto_total = temp.MontoIngenieria + temp.MontoConstruccion + temp.MontoSuministro,
                    requiere_cronograma = true
                };
                try
                {
                    var id = _requerimientoRepository.InsertAndGetId(Mapper.Map<Requerimiento>(requerimiento));

                    temp.RequerimeintoId = id;
                    Repository.Update(temp);
                }
                catch (Exception e)
                {
                    var msg = e.Message;

                }
            }

        }

        public void CargarTablaRelacion(int desde, int hasta)
        {
            var items = Repository.GetAll().Where(c => c.Id >= desde).Where(c => c.Id <= hasta).ToList();
            List<TempRequerimiento> listadoSinOfertaAsociada = new List<TempRequerimiento>();
            List<TempRequerimiento> ofertaComercialNoEncontrada = new List<TempRequerimiento>();


            foreach (var temp in items)
            {

                if (temp.CodigoOfertaAsiciada != null)
                {
                    var ofertaComercial = _ofertaComercialRepository
                        .GetAll().FirstOrDefault(o => o.codigo == temp.CodigoOfertaAsiciada);

                    var reqencontrado = _requerimientoRepository.GetAll().Where(c => c.Id == temp.RequerimeintoId).FirstOrDefault();
                    if (ofertaComercial != null)
                    {
                        if (reqencontrado != null && reqencontrado.Id > 0)
                        {
                            var ofertaPresupuesto = new OfertaComercialPresupuesto()
                            {
                                OfertaComercialId = ofertaComercial.Id,
                                //PresupuestoId = 0,
                                RequerimientoId = reqencontrado.Id,
                                vigente = true,
                            };

                            _ofertaComercialPresupuestoRepository.Insert(ofertaPresupuesto);
                        }
                    }
                    else
                    {
                        ofertaComercialNoEncontrada.Add(temp);
                    }
                }
                else
                {
                    listadoSinOfertaAsociada.Add(temp);
                }
            }

            var x = listadoSinOfertaAsociada;
            var y = ofertaComercialNoEncontrada;
        }

        public int BuscarProyectoId(string codigo)
        {
            string codigo_nuevo = "";

            if (codigo[0] == '0')
            {
                codigo_nuevo = codigo.Substring(1, codigo.Length - 1);
            }
            else
            {
                codigo_nuevo = codigo;
            }
            var proyecto = _proyectoRepository
                .GetAll()
                .Where(o => o.vigente).FirstOrDefault(o => o.codigo == codigo_nuevo);

            if (proyecto != null)
            {
                return proyecto.Id;
            }
            else
            {
                var proyecto_nuevo = new ProyectoDto()
                {
                    codigo = codigo_nuevo,
                    nombre_proyecto = "Generado migracion",
                    descripcion_proyecto = "Generado migracion",
                    contratoId = 1,
                    estado_proyecto = true,
                    vigente = true,
                    responsable = Proyecto.Reponsable.Campo
                };

                var proyectoId = _proyectoRepository.InsertAndGetId(Mapper.Map<Proyecto>(proyecto_nuevo));
                return proyectoId;
            }
        }

        public async Task<string> CargarProyectoAsync()
        {
            List<String> migrados = new List<string>();
            var proyectos = _tempProyectoRepository.GetAll().ToList();
            string codigo_nuevo = "";
            foreach (var p in proyectos)
            {
                var Proyecto = this.BuscarProyecto(p.codigo);

                if (Proyecto != null)
                {
                    Proyecto.presupuesto = p.budget;
                    Proyecto.comentarios = p.comentarios;
                    Proyecto.monto_aprobado_orden_trabajo = p.total_trabajos;
                    Proyecto.monto_aprobado_os = p.totalos;

                    _proyectoRepository.Update(Proyecto);
                    string proyecto = Proyecto.codigo + " -Encontrado - Actualizado" + Proyecto.nombre_proyecto + DateTime.Now.Date.ToShortDateString();
                    codigo_nuevo = Proyecto.codigo;
                    migrados.Add(proyecto);
                }
                else
                {
                    codigo_nuevo = p.codigo;
                    var proyecto_nuevo = new ProyectoDto()
                    {
                        codigo = codigo_nuevo,
                        nombre_proyecto = p.descripcion,
                        descripcion_proyecto = p.descripcion,
                        contratoId = 1,
                        estado_proyecto = true,
                        vigente = true,
                        responsable = Proyecto.Reponsable.Campo,
                        monto_aprobado_orden_trabajo = p.total_trabajos,
                        monto_aprobado_os = p.totalos,
                        comentarios = p.comentarios,
                        periodo_garantia = 12
                    };

                    var proyectoId = _proyectoRepository.InsertAndGetId(Mapper.Map<Proyecto>(proyecto_nuevo));
                    string proyecto = codigo_nuevo + " -Creado -" + p.descripcion + DateTime.Now.Date.ToShortDateString();
                    migrados.Add(proyecto);
                }
            }
            await this.EnviarArchivos(migrados, "PROYECTOS");
            return "OK";
        }

        public Proyecto BuscarProyecto(string codigo)
        {
            string codigo_nuevo = "";

            if (codigo[0] == '0')
            {
                codigo_nuevo = codigo.Substring(1, codigo.Length - 1);
            }
            else
            {
                codigo_nuevo = codigo;
            }
            var proyecto = _proyectoRepository
                .GetAll()
                .Where(o => o.vigente).FirstOrDefault(o => o.codigo == codigo_nuevo);

            if (proyecto != null)
            {
                return proyecto;
            }
            else { return null; }

        }


        public async Task<string> EnviarArchivos(List<String> datos, string tipo)
        {
            Random a = new Random();
            var valor = a.Next(1, 1000);
            string fileName = System.Web.HttpContext.Current.Server.MapPath("~/Views/ArchivosMigraciones/" + tipo + "" + DateTime.Now.Date.Year + "" + valor + ".txt");

            using (StreamWriter sw = File.CreateText(fileName))
            {

                foreach (var b in datos)
                {
                    sw.WriteLine(b);
                }
            }

            MailMessage message = new MailMessage();
            message.Subject = "PMDIS: MIGRACIONES";

            message.To.Add("efrain.saransig@atikasoft.com");
            //message.To.Add("paola.soria@atikasoft.com");
            if (File.Exists((string)fileName))
            {
                message.Attachments.Add(new Attachment(fileName));
            }
            /*********/
            try
            {
                await _correoservice.SendWithFilesAsync(message);
                return "OK";
            }
            catch (Exception e)
            {
                return e.Message;

            }





        }

        public async Task<string> CargarRequerimientossAsync(int desde, int hasta)
        {
            List<String> migrados = new List<string>();
            var items = Repository.GetAll().Where(c => c.Id >= desde).Where(c => c.Id <= hasta).ToList();
            foreach (var temp in items)
            {

                var proyecto = this.BuscarProyectoId(temp.ProyectoPrincipal);
                var codigo = "";
                var tipo_requerimiento = new TipoRequerimiento();

                if (temp.Codigo[0] == '0')
                {
                    tipo_requerimiento = TipoRequerimiento.Principal;
                    codigo = temp.Codigo.Substring(1, temp.Codigo.Length - 1);
                }
                else
                {
                    tipo_requerimiento = TipoRequerimiento.Adicional;
                    codigo = temp.Codigo;
                }



                var requerimiento = new RequerimientoDto()
                {

                    ProyectoId = proyecto,
                    tipo_requerimiento = tipo_requerimiento,
                    codigo = codigo,
                    fecha_recepcion = temp.FechaRecepcion,
                    descripcion = temp.Descripcion,
                    solicitante = temp.Solicitante,
                    vigente = true,
                    estado = true,
                    monto_ingenieria = temp.MontoIngenieria,
                    monto_construccion = temp.MontoConstruccion,
                    monto_procura = temp.MontoSuministro,
                    monto_total = temp.MontoIngenieria + temp.MontoConstruccion + temp.MontoSuministro,
                    requiere_cronograma = true
                };

                var re = _requerimientoRepository.GetAll().Where(c => c.codigo == requerimiento.codigo).FirstOrDefault();

                if (re != null && re.Id > 0)
                {
                    var en = _requerimientoRepository.Get(re.Id);
                    en.monto_construccion = temp.MontoConstruccion;
                    en.monto_ingenieria = temp.MontoIngenieria;
                    en.monto_procura = temp.MontoSuministro;
                    en.monto_total = temp.MontoIngenieria + temp.MontoIngenieria + temp.MontoConstruccion;
                    en.solicitante = temp.Solicitante;
                    en.descripcion = temp.Descripcion;
                    var up = _requerimientoRepository.Update(en);
                    temp.RequerimeintoId = re.Id;
                    Repository.Update(temp);
                    string data = "ACTUALIZADA-" + re.codigo + re.descripcion + DateTime.Now.Date.ToShortDateString();
                    string temporal = "ACTUALIZADA TEMP-" + temp.Codigo + temp.Id + DateTime.Now.Date.ToShortDateString();

                    migrados.Add(data);
                    migrados.Add(temporal);
                }
                else
                {
                    try
                    {
                        var id = _requerimientoRepository.InsertAndGetId(Mapper.Map<Requerimiento>(requerimiento));

                        temp.RequerimeintoId = id;
                        Repository.Update(temp);
                        string data = "CREADA-" + requerimiento.codigo + requerimiento.descripcion + DateTime.Now.Date.ToShortDateString();
                        string temporal = "ACTUALIZADA TEMP-" + temp.Codigo + temp.Id + DateTime.Now.Date.ToShortDateString();

                        migrados.Add(data);
                        migrados.Add(temporal);
                    }
                    catch (Exception e)
                    {
                        var msg = e.Message;
                        string data = "CREADA-ERROR" + requerimiento.codigo + requerimiento.descripcion + DateTime.Now.Date.ToShortDateString() + "" + e.Message.ToString();
                        migrados.Add(data);

                    }
                }
            }
            await this.EnviarArchivos(migrados, "REQUERIMIENTOS");
            return "OK";
        }


        public async Task<string> CargarTransmittalsAsync(int desde, int hasta)
        {
            var trans = _tempTransmittal.GetAll().Where(c => c.Id >= desde)
                                                .Where(c => c.Id <= hasta).ToList();

            List<String> migrados = new List<string>();

            foreach (var item in trans)
            {

                var oferta = _ofertaComercialRepository.GetAll().Where(c => c.codigo == item.codigoOferta).FirstOrDefault();

                var e = _transmitalRepository.GetAll().Where(c => c.codigo_transmital == item.codigoTransmital).FirstOrDefault();
                if (e != null && e.Id > 0)
                {
                    e.descripcion = item.descripciondeltransmital;
                    e.fecha_emision = item.fechaEmision;
                    try
                    {
                        _transmitalRepository.Update(e);
                        string data = "ACTUALIZADA-" + e.codigo_transmital + e.descripcion + DateTime.Now.Date.ToShortDateString();
                        migrados.Add(data);
                    }
                    catch (Exception v)
                    {
                        string data = "ACTUALIZADA-" + e.codigo_transmital + e.descripcion + DateTime.Now.Date.ToShortDateString() + "" + v.Message.ToString();
                        migrados.Add(data);
                    }


                }
                else
                {




                    TransmitalCabecera n = new TransmitalCabecera()
                    {
                        ClienteId = 1,
                        EmpresaId = 1,
                        codigo_carta = ".",
                        codigo_transmital = item.codigoTransmital,
                        ContratoId = 1,
                        copia_a = "0",
                        descripcion = item.descripciondeltransmital,
                        dirigido_a = "0",
                        enviado_por = "0",
                        estado = 1,
                        fecha_emision = item.fechaEmision,
                        fecha_recepcion = null,
                        fecha_ultima_modificacion = null,
                        tipo = "CO",
                        tipo_formato = "I",
                        tipo_proposito = "PA",
                        vigente = true,


                    };

                    if (item.codigoOferta == "n/a")
                    {

                        n.OfertaComercialId = null;
                    }
                    else
                    {

                        if (oferta != null && oferta.Id > 0)
                        {
                            n.OfertaComercialId = oferta.Id;
                        }
                        else
                        {
                            n.OfertaComercialId = null;
                        }
                    }

                    int idNuevo = 0;
                    try
                    {
                        idNuevo = _transmitalRepository.InsertAndGetId(n);
                        string data = "CREADA-" + n.codigo_transmital + n.descripcion + DateTime.Now.Date.ToShortDateString();
                        migrados.Add(data);
                        if (idNuevo > 0)
                        {

                            if (oferta != null && oferta.Id > 0)
                            {
                                oferta.TransmitalId = idNuevo;
                                _ofertaComercialRepository.Update(oferta);
                            }
                        }
                    }
                    catch (Exception x)
                    {
                        string data = "CREADA- ERROR" + n.codigo_transmital + n.descripcion + DateTime.Now.Date.ToShortDateString() + "" + x.Message.ToString();
                        migrados.Add(data);

                    }

                }


            }
            await this.EnviarArchivos(migrados, "TRANSMITTAL");
            return "OK";
        }

        public async Task<string> CargarCartas(int desde, int hasta)
        {
            List<String> migrados = new List<string>();
            var temp = _tempCartas.GetAll().Where(c => c.Id >= desde).Where(c => c.Id <= hasta).ToList();



            foreach (var item in temp)
            {

                try
                {

                    int ClasificacionId = 0;
                    int tipoCarta = 0;
                    int tipoDestinatario = 0;

                    if (item.cliente == "CPP")
                    {
                        var clasificacion = _catalogoRepository.GetAll().Where(c => c.codigo == "CL_CARTA_CPP").FirstOrDefault();
                        if (clasificacion != null)
                        {
                            ClasificacionId = clasificacion.Id;
                        }
                    }
                    if (item.cliente == "SHA")
                    {
                        var clasificacion = _catalogoRepository.GetAll().Where(c => c.codigo == "CL_CARTA_SHA").FirstOrDefault();
                        if (clasificacion != null)
                        {
                            ClasificacionId = clasificacion.Id;
                        }
                    }
                    if (item.cliente == "CGA")
                    {
                        var clasificacion = _catalogoRepository.GetAll().Where(c => c.codigo == "CL_CARTA_CGA").FirstOrDefault();
                        if (clasificacion != null)
                        {
                            ClasificacionId = clasificacion.Id;
                        }
                    }

                    var ctDestinatario = _catalogoRepository.GetAll().Where(c => c.codigo == "TIPO_CLIENTE").FirstOrDefault();
                    if (ctDestinatario != null)
                    {
                        tipoDestinatario = ctDestinatario.Id;
                    }

                    if (item.numerocarta.Contains("3808-B-LT"))
                    {
                        var tipa = _catalogoRepository.GetAll().Where(c => c.codigo == "CARTA_ENVIADA").FirstOrDefault();
                        if (tipa != null)
                        {
                            tipoCarta = tipa.Id;
                        }
                    }
                    if (item.numerocarta.Contains("SHY"))
                    {
                        var tipa = _catalogoRepository.GetAll().Where(c => c.codigo == "CARTA_RECIBIDA").FirstOrDefault();
                        if (tipa != null)
                        {
                            tipoCarta = tipa.Id;
                        }
                    }

                    if (item.fecharecepcion == new DateTime(1900, 1, 1) && item.fechasello == null)
                    {
                        var tipa = _catalogoRepository.GetAll().Where(c => c.codigo == "CARTA_NOUTILIZADA").FirstOrDefault();
                        if (tipa != null)
                        {
                            tipoCarta = tipa.Id;
                        }
                    }



                    Carta n = new Carta()
                    {
                        asunto = item.asunto,
                        fecha = item.fecharecepcion,
                        fechaSello = item.fechasello,
                        TipoCartaId = tipoCarta,
                        TipoDestinatarioId = tipoDestinatario,
                        ClasificacionId = ClasificacionId,
                        dirigidoA = item.dirigido,
                        enviadoPor = item.enviadopor,
                        numeroCarta = item.numerocarta,
                        numeroCartaEnviada = item.nrocartaenviada,
                        numeroCartaRecibida = item.nrocartarecibida,
                        linkCarta = item.commentarios,
                        referencia = "",
                        requiereRespuesta = item.requiererespuesta != null && item.requiererespuesta.ToUpper() == "SI" ? true : false,




                    };


                    var id = _cartaRepository.InsertAndGetId(n);
                    string data = "CREADA" + n.numeroCarta + DateTime.Now.Date.ToShortDateString() + "";
                    migrados.Add(data);
                }
                catch (DbEntityValidationException x)
                {

                    string data = "CREADA-ERROR" + "-" + x.Message.ToString();
                    migrados.Add(data);
                }

            }
            await this.EnviarArchivos(migrados, "Cartas");
            return "OK";

        }

        public async Task<string> CargarOsAsync(int desde, int hasta)
        {
            var so = _tempOs.GetAll().Where(c => c.Id >= desde).Where(c => c.Id <= hasta).ToList();

            List<String> migrados = new List<string>();
            int fila = 0;
            foreach (var item in so)
            {
                var proyecto = _proyectoRepository.GetAll().Where(c => c.codigo == item.codigoProyecto).Where(c => c.vigente).FirstOrDefault();

                var oferta = _ofertaComercialRepository.GetAll().Where(c => c.codigo == item.codigoOferta).Where(c => c.version == item.revOferta).FirstOrDefault();

                var en = _ordenServicio.GetAll().Where(c => c.codigo_orden_servicio == item.codigoOrden).Where(c => c.vigente).FirstOrDefault();
                if (en != null && en.Id > 0)
                {
                    en.monto_aprobado_os = item.MontoAprobado;
                    en.fecha_orden_servicio = item.fechaOrden;
                    en.version_os = item.version;
                    en.EstadoId = this.GetPOESTADO(item.status);

                    _ordenServicio.Update(en);
                    string data = "ACTUALIZADAS-" + en.codigo_orden_servicio + en.version_os + DateTime.Now.Date.ToShortDateString() + "";
                    migrados.Add(data);



                    if (proyecto != null && proyecto.Id > 0)
                    {
                        if (oferta != null && oferta.Id > 0)
                        {

                            if (item.montoIngenieria > 0)
                            {
                                var detalle = _detalleOrden.GetAll()
                     .Where(c => c.ProyectoId == proyecto.Id)
                     .Where(c => c.OfertaComercialId == oferta.Id)
                     .Where(c => c.OrdenServicioId == en.Id)
                     .Where(c => c.GrupoItemId == DetalleOrdenServicio.GrupoItems.Ingeniería)
                     .Where(c => c.vigente)
                     .FirstOrDefault();
                                if (detalle != null && detalle.Id > 0)
                                {
                                    detalle.valor_os = item.montoIngenieria;
                                    _detalleOrden.Update(detalle);
                                    string s = "Detalle Actualizado- fila" + fila + "";
                                    migrados.Add(s);
                                }
                                else
                                {
                                    DetalleOrdenServicio d = new DetalleOrdenServicio()
                                    {
                                        Id = 0,
                                        GrupoItemId = DetalleOrdenServicio.GrupoItems.Ingeniería,
                                        OfertaComercialId = oferta.Id,
                                        OrdenServicioId = en.Id,
                                        ProyectoId = proyecto.Id,
                                        valor_os = item.montoIngenieria,
                                        vigente = true

                                    };
                                    _detalleOrden.Insert(d);
                                    string s = "Detalle Creado- fila" + fila + "";
                                    migrados.Add(s);
                                }

                            }
                            if (item.montoContruccion > 0)
                            {
                                var detalle = _detalleOrden.GetAll()
                    .Where(c => c.ProyectoId == proyecto.Id)
                    .Where(c => c.OfertaComercialId == oferta.Id)
                    .Where(c => c.OrdenServicioId == en.Id)
                    .Where(c => c.GrupoItemId == DetalleOrdenServicio.GrupoItems.Construcción)
                    .Where(c => c.vigente)
                    .FirstOrDefault();
                                if (detalle != null && detalle.Id > 0)
                                {
                                    detalle.valor_os = item.montoContruccion;
                                    string s = "Detalle Actualizado- fila" + fila + "";
                                    migrados.Add(s);
                                    _detalleOrden.Update(detalle);

                                }
                                else
                                {
                                    DetalleOrdenServicio d = new DetalleOrdenServicio()
                                    {
                                        Id = 0,
                                        GrupoItemId = DetalleOrdenServicio.GrupoItems.Construcción,
                                        OfertaComercialId = oferta.Id,
                                        OrdenServicioId = en.Id,
                                        ProyectoId = proyecto.Id,
                                        valor_os = item.montoContruccion,
                                        vigente = true

                                    };
                                    string s = "Detalle Creado- fila" + fila + "";
                                    migrados.Add(s);
                                    _detalleOrden.Insert(d);

                                }
                            }
                            if (item.montoProcura > 0)
                            {
                                var detalle = _detalleOrden.GetAll()
                                                        .Where(c => c.ProyectoId == proyecto.Id)
                                                        .Where(c => c.OfertaComercialId == oferta.Id)
                                                        .Where(c => c.OrdenServicioId == en.Id)
                                                        .Where(c => c.GrupoItemId == DetalleOrdenServicio.GrupoItems.Suministros)
                                                        .Where(c => c.vigente)
                                                        .FirstOrDefault();
                                if (detalle != null && detalle.Id > 0)
                                {
                                    detalle.valor_os = item.montoProcura;
                                    _detalleOrden.Update(detalle);
                                    string s = "Detalle Actualizado- fila" + fila + "";
                                    migrados.Add(s);
                                }
                                else
                                {
                                    DetalleOrdenServicio d = new DetalleOrdenServicio()
                                    {
                                        Id = 0,
                                        GrupoItemId = DetalleOrdenServicio.GrupoItems.Suministros,
                                        OfertaComercialId = oferta.Id,
                                        OrdenServicioId = en.Id,
                                        ProyectoId = proyecto.Id,
                                        valor_os = item.montoProcura,
                                        vigente = true

                                    };
                                    _detalleOrden.Insert(d);
                                    string s = "Detalle Creado- fila" + fila + "";
                                    migrados.Add(s);

                                }
                            }








                        }
                        else
                        {
                            string x = "Fila " + fila + " - " + item.codigoOferta + " " + item.revOferta + " Oferta no Encontrado";
                            migrados.Add(x);
                        }

                    }
                    else
                    {
                        string w = "Fila " + fila + " - " + item.codigoProyecto + " Proyecto no Encontrado";
                        migrados.Add(w);
                    }



                }
                else
                {


                    OrdenServicio a = new OrdenServicio();
                    a.codigo_orden_servicio = item.codigoOrden;
                    a.fecha_orden_servicio = item.fechaOrden;
                    a.monto_aprobado_os = item.MontoAprobado;
                    a.monto_aprobado_construccion = 0;
                    a.monto_aprobado_ingeniería = 0;
                    a.monto_aprobado_suministros = 0;
                    a.monto_aprobado_subcontrato = 0;
                    a.vigente = true;
                    a.ArchivoId = null;
                    a.version_os = item.version;
                    a.EstadoId = this.GetPOESTADO(item.status);

                    int idOrden = 0;
                    try
                    {
                        idOrden = _ordenServicio.InsertAndGetId(a);
                        string data = "CREADA-" + a.codigo_orden_servicio + a.version_os + DateTime.Now.Date.ToShortDateString() + "";
                        migrados.Add(data);
                    }
                    catch (Exception x)
                    {
                        string data = "CREADA- ERROR" + a.codigo_orden_servicio + a.version_os + DateTime.Now.Date.ToShortDateString() + "" + x.Message.ToString();
                        migrados.Add(data);
                    }

                    if (idOrden > 0)
                    {
                        if (proyecto != null && proyecto.Id > 0)
                        {
                            if (oferta != null && oferta.Id > 0)
                            {

                                if (item.montoIngenieria > 0)
                                {
                                    var detalle = _detalleOrden.GetAll()
                         .Where(c => c.ProyectoId == proyecto.Id)
                         .Where(c => c.OfertaComercialId == oferta.Id)
                         .Where(c => c.OrdenServicioId == idOrden)
                         .Where(c => c.GrupoItemId == DetalleOrdenServicio.GrupoItems.Ingeniería)
                         .Where(c => c.vigente)
                         .FirstOrDefault();
                                    if (detalle != null && detalle.Id > 0)
                                    {
                                        detalle.valor_os = item.montoIngenieria;
                                        _detalleOrden.Update(detalle);

                                    }
                                    else
                                    {
                                        DetalleOrdenServicio d = new DetalleOrdenServicio()
                                        {
                                            Id = 0,
                                            GrupoItemId = DetalleOrdenServicio.GrupoItems.Ingeniería,
                                            OfertaComercialId = oferta.Id,
                                            OrdenServicioId = idOrden,
                                            ProyectoId = proyecto.Id,
                                            valor_os = item.montoIngenieria,
                                            vigente = true

                                        };
                                        _detalleOrden.Insert(d);

                                    }

                                }
                                if (item.montoContruccion > 0)
                                {
                                    var detalle = _detalleOrden.GetAll()
                        .Where(c => c.ProyectoId == proyecto.Id)
                        .Where(c => c.OfertaComercialId == oferta.Id)
                        .Where(c => c.OrdenServicioId == idOrden)
                        .Where(c => c.GrupoItemId == DetalleOrdenServicio.GrupoItems.Construcción)
                        .Where(c => c.vigente)
                        .FirstOrDefault();
                                    if (detalle != null && detalle.Id > 0)
                                    {
                                        detalle.valor_os = item.montoContruccion;
                                        _detalleOrden.Update(detalle);

                                    }
                                    else
                                    {
                                        DetalleOrdenServicio d = new DetalleOrdenServicio()
                                        {
                                            Id = 0,
                                            GrupoItemId = DetalleOrdenServicio.GrupoItems.Construcción,
                                            OfertaComercialId = oferta.Id,
                                            OrdenServicioId = idOrden,
                                            ProyectoId = proyecto.Id,
                                            valor_os = item.montoContruccion,
                                            vigente = true

                                        };
                                        _detalleOrden.Insert(d);

                                    }
                                }
                                if (item.montoProcura > 0)
                                {
                                    var detalle = _detalleOrden.GetAll()
                                                            .Where(c => c.ProyectoId == proyecto.Id)
                                                            .Where(c => c.OfertaComercialId == oferta.Id)
                                                            .Where(c => c.OrdenServicioId == idOrden)
                                                            .Where(c => c.GrupoItemId == DetalleOrdenServicio.GrupoItems.Suministros)
                                                            .Where(c => c.vigente)
                                                            .FirstOrDefault();
                                    if (detalle != null && detalle.Id > 0)
                                    {
                                        detalle.valor_os = item.montoProcura;
                                        _detalleOrden.Update(detalle);

                                    }
                                    else
                                    {
                                        DetalleOrdenServicio d = new DetalleOrdenServicio()
                                        {
                                            Id = 0,
                                            GrupoItemId = DetalleOrdenServicio.GrupoItems.Suministros,
                                            OfertaComercialId = oferta.Id,
                                            OrdenServicioId = idOrden,
                                            ProyectoId = proyecto.Id,
                                            valor_os = item.montoProcura,
                                            vigente = true

                                        };
                                        _detalleOrden.Insert(d);

                                    }
                                }








                            }
                            else
                            {
                                string data = "Fila " + fila + " - " + item.codigoOferta + " " + item.revOferta + " Oferta no Encontrado";
                                migrados.Add(data);
                            }

                        }
                        else
                        {
                            string data = "Fila " + fila + " - " + item.codigoProyecto + " Proyecto no Encontrado";
                            migrados.Add(data);
                        }
                    }



                }

                fila++;



            }
            await this.EnviarArchivos(migrados, "OS");
            return "OK";
        }

        public async Task<string> ActualizarReferenciaAsync(int desde, int hasta)
        {
            List<String> migrados = new List<string>();
            var items = Repository.GetAll().Where(c => c.Id >= desde).Where(c => c.Id <= hasta).ToList();
            foreach (var temp in items)
            {
                var oferta = _ofertaComercialRepository.GetAll().Where(c => c.codigo == temp.CodigoOfertaAsiciada).FirstOrDefault();
                if (oferta != null && oferta.Id > 0)
                {

                    oferta.estado_oferta = this.GetEstadoOferta(temp.estadoRequerimiento);
                    _ofertaComercialRepository.Update(oferta);
                }

                if (temp.RequerimeintoId.HasValue)
                {
                    var requerimiento = _requerimientoRepository.GetAll().Where(c => c.Id == temp.RequerimeintoId).FirstOrDefault();
                    if (requerimiento != null && requerimiento.Id > 0)
                    {
                        requerimiento.EstadoOfertaId = this.GetEstadoOferta(temp.estadoRequerimiento);

                        _requerimientoRepository.Update(requerimiento);
                        string data = "ACTUALIZADA  ESTADO OFERTA TO REQUERIMIENTO-" + requerimiento.codigo;
                        migrados.Add(data);
                    }
                }

            }
            await this.EnviarArchivos(migrados, "REQUERIMIENTOSID");
            return "OK";
        }
        public int GetEstadoOferta(string nombre)
        {
            switch (nombre)
            {
                case "Aprobado":
                    return 2029;
                case "Anulado":
                    return 2031;
                case "Cancelado":
                    return 2024;
                case "En Revisión":
                    return 2027;
                case "Por Presentar":
                    return 2027;
                case "Presentado":
                    return 2028;
                case "":
                    return 2027;

                default:
                    return 2027;

            }
        }

        public async Task<string> ActualizarMontosRequerimientosAsync(string listrequerimiento)
        {
            var codigoRequerimientos = listrequerimiento.Split(',');
            List<String> migrados = new List<string>();

            foreach (var code in codigoRequerimientos)
            {



                var tempRequerimiento = Repository.GetAll().Where(c => c.Codigo == code).FirstOrDefault();

                var codigo = "";
                if (code[0] == '0')
                {
                    codigo = code.Substring(1, code.Length - 1);
                }
                else
                {
                    codigo = code;
                }


                if (tempRequerimiento != null && tempRequerimiento.Codigo.Length > 0)
                {

                    var requerimientoOriginal = _requerimientoRepository.GetAll().Where(c => c.codigo == codigo).FirstOrDefault();

                    if (requerimientoOriginal != null && requerimientoOriginal.Id > 0)
                    {
                        string line = "-----------------------------------------------------";
                        migrados.Add(line);
                        var en = _requerimientoRepository.Get(requerimientoOriginal.Id);
                        string original = "REQUERIMIENTO ORIGINAL: " + en.codigo + " MONTOS: i: " + en.monto_ingenieria + " ,c: " + en.monto_construccion + ", s: " + en.monto_procura + ", sub: " + en.monto_subcontrato + ", T: " + en.monto_total;
                        migrados.Add(original);


                        en.monto_construccion = tempRequerimiento.MontoConstruccion;
                        en.monto_ingenieria = tempRequerimiento.MontoIngenieria;
                        en.monto_procura = tempRequerimiento.MontoSuministro;
                        en.monto_total = tempRequerimiento.MontoIngenieria + tempRequerimiento.MontoIngenieria + tempRequerimiento.MontoConstruccion;
                        try
                        {

                            string TO = "------------------TO-----------------------";
                            migrados.Add(TO);
                            var up = _requerimientoRepository.Update(en);
                            string actualizado = "REQUERIMIENTO ACTUALIZADO : " + tempRequerimiento.Codigo + " MONTOS: i: " + tempRequerimiento.MontoIngenieria + " ,c: " + tempRequerimiento.MontoConstruccion + ", s: " + tempRequerimiento.MontoSuministro + ", sub: " + "SN" + ", T: " + (tempRequerimiento.MontoIngenieria + tempRequerimiento.MontoConstruccion + tempRequerimiento.MontoSuministro);
                            migrados.Add(actualizado);
                        }
                        catch (Exception e)
                        {
                            string error = "ERROR:" + en.codigo + " " + e.Message.ToString();
                            migrados.Add(error);

                        }


                    }
                    else
                    {
                        string line = "------------------NO ENCONTRADO ORIGINAL :" + tempRequerimiento.Codigo + "-------------------------";
                        migrados.Add(line);
                    }

                }
                else
                {
                    string line = "------------------NO ENCONTRADO TEMPORAL :" + code + "-------------------------";
                    migrados.Add(line);
                }
            }
            await this.EnviarArchivos(migrados, "UPDATEMONTOSREQUERIMIENTOS");
            return "OK";

        }

        public int GetPOESTADO(string nombre)
        {
            var estadoPresentado = _catalogoRepository.GetAll().Where(c => c.codigo == "POPRESENTADO").FirstOrDefault();
            var estadoAprobado = _catalogoRepository.GetAll().Where(c => c.codigo == "POAPROBADO").FirstOrDefault();
            var estadoCancelado = _catalogoRepository.GetAll().Where(c => c.codigo == "POCANCELADO").FirstOrDefault();

            switch (nombre)
            {
                case "Aprobado":
                    return estadoAprobado != null && estadoAprobado.Id > 0 ? estadoAprobado.Id : 0;

                case "Presentado":
                    return estadoPresentado != null && estadoPresentado.Id > 0 ? estadoPresentado.Id : 0;
                case "Cancelado":
                    return estadoCancelado != null && estadoCancelado.Id > 0 ? estadoCancelado.Id : 0;

                default:
                    return 0;

            }
        }

        public void ActualizarFechasOfertasComerciales()
        {
            var estadopresentado = _catalogoRepository.GetAll().Where(c => c.codigo == CatalogosCodigos.OFERTA_ESTADO_PRESENTADO).FirstOrDefault();

            var ofertas = _ofertaComercialRepository.GetAll().Where(c => c.vigente)
                                                .Where(c => c.es_final == 1)
                                                .Where(c => c.estado_oferta == estadopresentado.Id)
                                                .Where(c => !c.fecha_ultimo_envio.HasValue)
                                                .ToList();
            foreach (var of in ofertas)
            {
                of.fecha_ultimo_envio = of.fecha_oferta;
                _ofertaComercialRepository.Update(of);



            }
        }

        public void ActualizarClaseRequerimientoOfertaComercial()
        {

            var query = _segComercial.GetAll().ToList();
            foreach (var seg in query)
            {
                var oferta = _ofertaComercialRepository.GetAll().Where(c => c.vigente)
                                                              .Where(c => c.codigo == seg.codigo)
                                                              .Where(c => c.version == seg.version)
                                                              .Where(c => c.es_final == 1)
                                                              .FirstOrDefault();
                if (oferta != null && oferta.Id > 0)
                {
                    var req_relacionados = _ofertaComercialPresupuestoRepository.GetAll().Where(c => c.OfertaComercialId == oferta.Id)
                                                                                       .Where(c => c.vigente)
                                                                                       .Select(c => c.RequerimientoId)
                                                                                       .ToList().Distinct().ToList();
                    foreach (var req_id in req_relacionados)
                    {
                        var requerimiento = _requerimientoRepository.Get(req_id);
                        requerimiento.ultima_clase = seg.claseAACE;

                    }

                }


            }
        }

        public string ActualizarNombresProyectos(HttpPostedFileBase UploadedFile)
        {

            if (UploadedFile != null)
            {
                using (System.IO.StreamWriter u =
                          new System.IO.StreamWriter(@"C:\Migracion\ProyectosActualizados" + ".txt"))
                {


                    // PROYECTOS

                    var listaProyectos = _proyectoRepository.GetAll().Where(c => c.vigente).ToList();

                    // tdata.ExecuteCommand("truncate table OtherCompanyAssets");  
                    if (UploadedFile.ContentType == "application/vnd.ms-excel" || UploadedFile.ContentType ==
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                    {
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

                            for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                            {
                                var PORTAFOLIO = (workSheet.Cells[rowIterator, 1].Value ?? "").ToString();
                                var CODIGO = (workSheet.Cells[rowIterator, 2].Value ?? "").ToString();
                                var NOMBREDELPROYECTO = (workSheet.Cells[rowIterator, 3].Value ?? "").ToString();
                                var PROYECTOYDESCRIPCIÓN = (workSheet.Cells[rowIterator, 4].Value ?? "").ToString();
                                var CORRESPONDECIA = (workSheet.Cells[rowIterator, 6].Value ?? "").ToString();

                                if (PORTAFOLIO.Length > 0 && CODIGO.Length > 0 && NOMBREDELPROYECTO.Length > 0 && PROYECTOYDESCRIPCIÓN.Length > 0 && CORRESPONDECIA.Length>0)
                                {
                                    string codigoExcel = CORRESPONDECIA.TrimEnd().TrimStart().Trim();

                                    string code = "FC01.01";
                                    string code2 = code.Trim().Replace('0', ' ').Replace(" ","").Trim();
                                    var proyectoEncontrado = (from p in listaProyectos
                                                              where p.codigo.Trim() == codigoExcel
                                                              select p).FirstOrDefault();

                                    if (proyectoEncontrado != null)
                                    {
                                        
                                        u.WriteLine("ID " + proyectoEncontrado.Id + " " +
                                                    " CODA=> " + proyectoEncontrado.codigo +
                                                    " CODN=> " + CODIGO +
                                                    " NA=> " + proyectoEncontrado.nombre_proyecto +
                                                    " NN=> " + NOMBREDELPROYECTO
                                                    );

                                        var p = _proyectoRepository.Get(proyectoEncontrado.Id);
                                        p.codigo = CODIGO;
                                        p.nombre_proyecto = NOMBREDELPROYECTO;
                                        p.descripcion_proyecto = NOMBREDELPROYECTO;
                                        _proyectoRepository.Update(p);

                                    }
                                    else {
                                        u.WriteLine("NO ENCONTRADO Fila=> " + rowIterator +" "+ CODIGO);
                                    }



                                }



                            }

                        }
                        return "OK";

                    }
                }
            }
            else
            {
                return "";
            }
            return "OK";
        }
    }



}
