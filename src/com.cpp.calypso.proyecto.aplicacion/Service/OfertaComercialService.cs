using Abp.Net.Mail;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.framework;
using com.cpp.calypso.proyecto.aplicacion.Dto;
using com.cpp.calypso.proyecto.aplicacion.Interfaces;
using com.cpp.calypso.proyecto.dominio;
using com.cpp.calypso.proyecto.dominio.Constantes;
using com.cpp.calypso.proyecto.dominio.Models;
using Microsoft.AspNet.Identity;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Xceed.Words.NET;
using Syncfusion.DocIO;
using Syncfusion.DocIO.DLS;
using Syncfusion.DocToPDFConverter;
using Syncfusion.Pdf;
using OfficeOpenXml.Drawing.Chart;
using Newtonsoft.Json;
using System.IO;
using System.IO.Compression;

namespace com.cpp.calypso.proyecto.aplicacion.Service
{
    public class OfertaComercialAsyncBaseCrudAppService : AsyncBaseCrudAppService<OfertaComercial, OfertaComercialDto, PagedAndFilteredResultRequestDto>, IOfertaComercialAsyncBaseCrudAppService
    {
        private readonly IBaseRepository<OfertaComercialPresupuesto> _ofertacomercialpresupuestorepository;
        private readonly IBaseRepository<Presupuesto> _presupuestorepository;
        private readonly IBaseRepository<ComputoPresupuesto> _computopresupuestoRepository;
        private readonly IBaseRepository<WbsComercial> _wbscomercialRepository;
        private readonly IBaseRepository<ComputoComercial> _computocomercialRepository;
        private readonly IBaseRepository<OrdenServicio> _ordenesService;
        private readonly IBaseRepository<ArchivosOferta> _archivooService;
        private readonly IBaseRepository<Archivo> _archivoRepository;
        private readonly IBaseRepository<WbsPresupuesto> _wbspresupuestoRepository;
        private readonly IBaseRepository<DetalleOrdenServicio> _detalleordenesService;
        private readonly IBaseRepository<Catalogo> _catalogoRepository;
        private readonly IBaseRepository<TransmitalCabecera> _transmitalRepository;
        private readonly IBaseRepository<TransmitalDetalle> _detalletransmitalRepository;

        private readonly IBaseRepository<Usuario> _usuarioRepository;
        private readonly IBaseRepository<Requerimiento> _requerimientoRepository;
        private readonly IBaseRepository<Proyecto> _proyectoRepository;
        private readonly IBaseRepository<DetalleGanancia> _detalleGananciaRepository;
        private readonly IEmailSender EmailService;

        private readonly IBaseRepository<Contrato> _contratoRepository;

        private readonly IdentityEmailMessageService _correoservice;
        private readonly IBaseRepository<CorreoLista> _correslistarepository;
        private readonly IBaseRepository<Colaborador> _colaboradorRepository;

        private readonly IBaseRepository<ArchivoOrdenProceder> _archivoOrdenProceder;
        private readonly IBaseRepository<ParametroSistema> _parametrorepository;

        public OfertaComercialAsyncBaseCrudAppService(

            IBaseRepository<OfertaComercial> repository,
            IBaseRepository<OfertaComercialPresupuesto> ofertacomercialpresupuestorepository,
            IBaseRepository<Presupuesto> presupuestorepository,
            IBaseRepository<WbsPresupuesto> wbspresupuestoRepository,
            IBaseRepository<ComputoPresupuesto> computopresupuestoRepository,
            IBaseRepository<WbsComercial> wbscomercialRepository,
            IBaseRepository<ComputoComercial> computocomercialRepository,
            IBaseRepository<OrdenServicio> ordenesService,
            IBaseRepository<ArchivosOferta> archivooService,
            IBaseRepository<Archivo> archivoRepository,
            IBaseRepository<TransmitalCabecera> transmitalRepository,
            IBaseRepository<TransmitalDetalle> detalletransmitalRepository,
            IBaseRepository<DetalleOrdenServicio> detalleordenesService,
            IBaseRepository<Usuario> usuarioRepository,
            IEmailSender emailService,
            IBaseRepository<Catalogo> catalogoRepository,
        IdentityEmailMessageService correoservice,
            IBaseRepository<CorreoLista> correslistarepository,
            IBaseRepository<Colaborador> colaboradorRepository,
             IBaseRepository<Requerimiento> requerimientoRepository,
             IBaseRepository<Proyecto> proyectoRepository,
            IBaseRepository<DetalleGanancia> detalleGananciaRepository,
            IBaseRepository<Contrato> contratoRepository,
            IBaseRepository<ArchivoOrdenProceder> archivoOrdenProceder,
             IBaseRepository<ParametroSistema> parametrorepository

            ) : base(repository)
        {
            _ofertacomercialpresupuestorepository = ofertacomercialpresupuestorepository;
            _presupuestorepository = presupuestorepository;
            _computopresupuestoRepository = computopresupuestoRepository;
            _wbscomercialRepository = wbscomercialRepository;
            _computocomercialRepository = computocomercialRepository;
            _ordenesService = ordenesService;
            _archivooService = archivooService;
            _wbspresupuestoRepository = wbspresupuestoRepository;
            _archivoRepository = archivoRepository;
            _transmitalRepository = transmitalRepository;
            _detalletransmitalRepository = detalletransmitalRepository;
            _detalleordenesService = detalleordenesService;
            _usuarioRepository = usuarioRepository;
            EmailService = emailService;
            _correoservice = correoservice;
            _correslistarepository = correslistarepository;
            _colaboradorRepository = colaboradorRepository;
            _requerimientoRepository = requerimientoRepository;
            _catalogoRepository = catalogoRepository;
            _proyectoRepository = proyectoRepository;
            _detalleGananciaRepository = detalleGananciaRepository;
            _contratoRepository = contratoRepository;
            _archivoOrdenProceder = archivoOrdenProceder;
            _parametrorepository = parametrorepository;
        }

        public void ActualizarVersion(int Id)
        {
            throw new NotImplementedException();
        }

        public int CrearNuevaVersion(OfertaComercial nueva)
        {
            int idversionantigua = nueva.Id;
            var oferta = Repository.Get(idversionantigua);
            var versionantigua = oferta;

            OfertaComercial nuevo = new OfertaComercial
            {

                Id = 0,
                estado_oferta = oferta.estado_oferta,
                version = oferta.version,
                acta_cierre = oferta.acta_cierre,
                alcance = oferta.alcance,
                centro_de_Costos_Id = oferta.centro_de_Costos_Id,
                codigo = oferta.codigo,
                codigo_shaya = oferta.codigo_shaya,
                computo_completo = oferta.computo_completo,
                ContratoId = oferta.ContratoId,
                descripcion = oferta.descripcion,
                dias_emision_oferta = oferta.dias_emision_oferta,
                dias_hasta_recepcion_so = oferta.dias_hasta_recepcion_so,
                estado = oferta.estado,
                estatus_de_Ejecucion = oferta.estatus_de_Ejecucion,
                es_final = 0,
                fecha_oferta = DateTime.Today,
                fecha_orden_proceder = oferta.fecha_orden_proceder,
                fecha_pliego = oferta.fecha_pliego,
                fecha_primer_envio = oferta.fecha_primer_envio,
                fecha_recepcion_so = oferta.fecha_recepcion_so,
                fecha_ultima_modificacion = oferta.fecha_ultima_modificacion,
                fecha_ultimo_envio = oferta.fecha_oferta,
                forma_contratacion = oferta.forma_contratacion,
                monto_certificado_aprobado_acumulado = oferta.monto_certificado_aprobado_acumulado,
                monto_ofertado = oferta.monto_ofertado,
                monto_so_aprobado = oferta.monto_so_aprobado,
                monto_ofertado_pendiente_aprobacion = oferta.monto_ofertado_pendiente_aprobacion,
                monto_so_referencia_total = oferta.monto_so_referencia_total,
                OfertaPadreId = oferta.OfertaPadreId,
                orden_proceder = oferta.orden_proceder,
                orden_proceder_enviada_por = oferta.orden_proceder_enviada_por,
                porcentaje_avance = oferta.porcentaje_avance,
                revision_Oferta = oferta.revision_Oferta,
                service_order = oferta.service_order,
                service_request = oferta.service_request,
                tipo_Trabajo_Id = oferta.tipo_Trabajo_Id,
                TransmitalId = oferta.TransmitalId,
                vigente = oferta.vigente,
                comentarios = versionantigua.comentarios
            };

            var resultado = Repository.InsertAndGetId(nuevo);

            //Presupuestos Ligados
            var relaciones = _ofertacomercialpresupuestorepository.GetAll()
                .Where(p => p.OfertaComercialId == idversionantigua)
                .Where(p => p.vigente == true).ToList();

            if (relaciones.Count > 0)
            {
                foreach (var r in relaciones)
                {
                    OfertaComercialPresupuesto a = new OfertaComercialPresupuesto();
                    a.Id = 0;
                    a.fecha_asignacion = r.fecha_asignacion;
                    a.OfertaComercialId = resultado;
                    a.PresupuestoId = r.PresupuestoId;
                    a.RequerimientoId = r.RequerimientoId;
                    a.vigente = true;

                    var idnuevo = _ofertacomercialpresupuestorepository.InsertAndGetId(a);
                }

            }

            //Ordenes Servicio

            var ordenes = _ordenesService.GetAll().Where(c => c.vigente == true).Where(c => c.EstadoId == idversionantigua).ToList();

            if (ordenes.Count > 0)
            {

                foreach (var or in ordenes)
                {


                    OrdenServicio o = new OrdenServicio
                    {
                        Id = 0,
                        fecha_orden_servicio = or.fecha_orden_servicio,
                        codigo_orden_servicio = or.codigo_orden_servicio,
                        monto_aprobado_construccion = or.monto_aprobado_construccion,
                        monto_aprobado_ingeniería = or.monto_aprobado_ingeniería,
                        monto_aprobado_os = or.monto_aprobado_os,
                        monto_aprobado_suministros = or.monto_aprobado_suministros,
                        //OfertaComercialId = resultado,
                        version_os = or.version_os,
                        vigente = true
                    };
                    var idorden = _ordenesService.InsertAndGetId(o);


                    var detallesordenservicio = _detalleordenesService.GetAll().Where(c => c.vigente).
                                                                              Where(c => c.OrdenServicioId == or.Id).ToList();
                    if (detallesordenservicio.Count > 0)
                    {

                        foreach (var de in detallesordenservicio)
                        {
                            DetalleOrdenServicio d = new DetalleOrdenServicio
                            {
                                Id = 0,
                                GrupoItemId = de.GrupoItemId,
                                OrdenServicioId = idorden,
                                valor_os = de.valor_os,
                                vigente = true
                            };
                            var detalleid = _ordenesService.InsertAndGetId(o);
                        }


                    }



                }
            }

            // tRANSMITALS
            var transmitals = _transmitalRepository.GetAllIncluding(c => c.Contrato).Where(c => c.vigente == true).Where(c => c.OfertaComercialId == idversionantigua).ToList();

            if (transmitals.Count > 0)
            {

                foreach (var o in transmitals)
                {
                    var transmittal = _transmitalRepository.Get(o.Id);
                    transmittal.OfertaComercialId = resultado;

                    var update = _transmitalRepository.Update(transmittal);

                }
            }



            if (resultado > 0)
            {
                if (idversionantigua > 0)
                {


                    if (versionantigua != null && versionantigua.Id > 0)
                    {
                        /*Principales*/
                        versionantigua.es_final = 1;
                        var version = nueva.version[0];
                        version++;
                        versionantigua.version = version.ToString();
                        versionantigua.comentarios = nueva.comentarios;
                        versionantigua.fecha_oferta = nueva.fecha_oferta;
                        versionantigua.link_documentum = nueva.link_documentum;
                        versionantigua.TransmitalId = null;
                        versionantigua.estado_oferta = nueva.estado_oferta;
                        versionantigua.acta_cierre = nueva.acta_cierre;
                        versionantigua.alcance = nueva.alcance;
                        versionantigua.centro_de_Costos_Id = nueva.centro_de_Costos_Id;
                        versionantigua.codigo_shaya = nueva.codigo_shaya;
                        versionantigua.descripcion = nueva.descripcion;
                        versionantigua.estado = nueva.estado;
                        versionantigua.estatus_de_Ejecucion = nueva.estatus_de_Ejecucion;
                        versionantigua.forma_contratacion = nueva.forma_contratacion;
                        versionantigua.service_order = nueva.service_order;
                        versionantigua.service_request = nueva.service_request;
                        versionantigua.tipo_Trabajo_Id = nueva.tipo_Trabajo_Id;
                        versionantigua.fecha_ultimo_envio = nueva.fecha_oferta;


                        var antigua = Repository.Update(versionantigua);

                    }
                }
                return resultado;
            }
            else
            {
                return 0;
            }
        }

        public int CrearOfertaComercia(OfertaComercial oferta)

        {
            var secuencia = this.secuencialofertacomercial();
            if (secuencia > 0)
            {
                oferta.codigo = "3808-B-SP-" + String.Format("{0:000000}", secuencia);
            }
            else
            {
                oferta.codigo = "3808-B-SP-"
                    + String.Format("{0:000000}", (Repository.GetAll().Where(c => c.vigente == true).ToList().Count() + 1));
            }
            // oferta.estado_oferta = 5182;

            var resultado = Repository.InsertAndGetId(oferta);
            if (resultado > 0)
            {
                var dos = Repository.Get(resultado);
                dos.OfertaPadreId = resultado;
                return resultado;
            }
            else
            {
                return 0;
            }
        }
        public int EditarOfertaComercial(OfertaComercial oferta)

        {

            var estadopresentado = _catalogoRepository.GetAll().Where(c => c.codigo == CatalogosCodigos.OFERTA_ESTADO_PRESENTADO).FirstOrDefault();
            if (estadopresentado != null && estadopresentado.Id > 0)
            {
                if (oferta.estado_oferta == estadopresentado.Id)
                {
                    oferta.fecha_primer_envio = oferta.fecha_oferta;
                    oferta.fecha_ultimo_envio = oferta.fecha_oferta;
                }
            }

            var resultado = Repository.Update(oferta);

            if (resultado.Id > 0)
            {
                /*Actualizacion Estado de todos los Requerimientos Ligados al Editar el Estado de una Oferta Comercial*/

                var requerimientos = _ofertacomercialpresupuestorepository.GetAllIncluding(c => c.Requerimiento).Where(c => c.OfertaComercialId == oferta.Id).Where(c => c.vigente).ToList();

                if (requerimientos.Count > 0)
                {
                    foreach (var item in requerimientos)
                    {

                        item.fecha_asignacion = DateTime.Now;
                        _ofertacomercialpresupuestorepository.Update(item);
                        var requerimiento = _requerimientoRepository.GetAll().Where(c => c.Id == item.RequerimientoId).FirstOrDefault();
                        if (requerimiento != null && requerimiento.Id > 0)
                        {
                            requerimiento.EstadoOfertaId = oferta.estado_oferta;
                            _requerimientoRepository.Update(requerimiento);
                        }
                    }
                }

                return resultado.Id;
            }
            else
            {
                return 0;
            }
        }

        public OfertaComercial GetDetalles(int Id)
        {
            var detalle = Repository.GetAllIncluding(c => c.Contrato).Where(c => c.vigente).Where(c => c.Id == Id).FirstOrDefault();

            var listdisitribucion = _correslistarepository.GetAllIncluding(c => c.ListaDistribucion).Where(c => c.vigente).Where(c => c.ListaDistribucion.codigo == CatalogosCodigos.LISTADISTRIBUCION_OFERTA_COMERCIAL).ToList();


            detalle.orden_proceder_enviada_por = String.Join(", ", listdisitribucion.Select(c => c.correo));
            return detalle;
        }

        public List<OfertaComercialDto> Lista()
        {
            var resultado = Repository.GetAll().Where(c => c.vigente == true).
                Where(c => c.es_final == 1).ToList();
            var OfertasFinalesIds = (from o in resultado select o.Id).ToList();
            var proyectos = _ofertacomercialpresupuestorepository.GetAll().Where(c => c.vigente)
                                                                 .Where(c => OfertasFinalesIds.Contains(c.OfertaComercialId))
                                                                 .ToList();

            var query = (from o in resultado
                         select new OfertaComercialDto
                         {
                             Id = o.Id,
                             codigo = o.codigo,
                             descripcion = o.descripcion,
                             estado_oferta = o.estado_oferta,
                             nombre_estado = o.Catalogo != null ? o.Catalogo.nombre : "",
                             version = o.version,
                             proyecto_ligados = this.ProyectosLigadosOfertaComercial(o.Id, proyectos.Where(c => c.OfertaComercialId == o.Id)
                         .Select(c => c.Requerimiento.Proyecto.codigo).Distinct().ToArray()),
                             nombre_estado_proceso = o.Catalogo != null ? o.Catalogo.nombre : "",
                             proyecto_ligados_id = this.ProyectosLigadosOfertaComercial(o.Id, proyectos.Where(c => c.OfertaComercialId == o.Id)
                         .Select(c => "" + c.Requerimiento.Proyecto.Id).Distinct().ToArray()),

                             monto_ofertado = o.monto_ofertado,
                             monto_so_aprobado = o.monto_so_aprobado,
                             monto_ofertado_pendiente_aprobacion = o.monto_ofertado_pendiente_aprobacion,
                             orden_proceder = o.orden_proceder,
                             fecha_orden_proceder = o.fecha_orden_proceder,
                             link_ordenProceder = o.link_ordenProceder,
                             tieneOrdenProceder = o.orden_proceder ? "SI" : "NO"


                         }).ToList();


            return query;
        }

        public List<OfertaComercial> ListaVersiones(int OfertaPadreId)
        {
            var Oferta = Repository.Get(OfertaPadreId);


            var versiones = Repository.GetAllIncluding(c => c.OfertaPadre, c => c.Catalogo).Where(c => c.vigente == true).
                Where(c => c.OfertaPadreId == Oferta.OfertaPadreId.Value).
                // .Where(c => c.es_final == 0).
                OrderByDescending(c => c.version).ToList();
            return versiones;
        }
        public OfertaComercial BuscarPadre(int OfertaPadreId)
        {
            var query = Repository.GetAllIncluding(c => c.OfertaPadre).
                  Where(c => c.vigente == true).
                  Where(c => c.Id == OfertaPadreId)
                  .FirstOrDefault();

            return query;
        }

        public MontosTotalesOrdenesServicio monto_ordenes_servicio(int OfertaId)
        {
            var lista_ordenes = _detalleordenesService.GetAll().Where(c => c.OfertaComercialId == OfertaId)
                                                        .Where(c => c.vigente).ToList();

            decimal construccion = (from e in lista_ordenes where e.GrupoItemId == DetalleOrdenServicio.GrupoItems.Construcción select e.valor_os).Sum();
            decimal ingenieria = (from e in lista_ordenes where e.GrupoItemId == DetalleOrdenServicio.GrupoItems.Ingeniería select e.valor_os).Sum();
            decimal subcontratos = (from e in lista_ordenes where e.GrupoItemId == DetalleOrdenServicio.GrupoItems.SubContratos select e.valor_os).Sum();
            decimal suministros = (from e in lista_ordenes where e.GrupoItemId == DetalleOrdenServicio.GrupoItems.Suministros select e.valor_os).Sum();

            decimal resultado = (construccion + ingenieria + subcontratos + suministros);
            MontosTotalesOrdenesServicio nuevo = new MontosTotalesOrdenesServicio()
            {
                construccion = construccion,
                ingenieria = ingenieria,
                suminitros = suministros,
                subcontratos = subcontratos,
                montototalos = resultado
            };

            return nuevo;
        }

        public void CalcularMontosOfertaComercial(int Id)
        {
            decimal montoingenieria = 0;
            decimal montoconstruccion = 0;
            decimal montosuministros = 0;
            var listacomputos = _computocomercialRepository.GetAllIncluding(c => c.WbsComercial.OfertaComercial, c => c.Item)
                .Where(c => c.vigente == true).Where(c => c.WbsComercial.OfertaComercialId == Id)
                .ToList();

            if (listacomputos != null && listacomputos.Count >= 0)
            {
                montoingenieria = (from x in listacomputos where x.Item.GrupoId == 1 select x.costo_total).Sum();
                montoconstruccion = (from x in listacomputos where x.Item.GrupoId == 2 select x.costo_total).Sum();
                montosuministros = (from x in listacomputos where x.Item.GrupoId == 3 select x.costo_total).Sum();

            }



            ///


            var lista_ordenes = _ordenesService.GetAll().Where(c => c.EstadoId == Id).ToList();

            decimal construccion = (from e in lista_ordenes select e.monto_aprobado_construccion).Sum();
            decimal ingenieria = (from e in lista_ordenes select e.monto_aprobado_ingeniería).Sum();
            decimal os = (from e in lista_ordenes select e.monto_aprobado_os).Sum();
            decimal suministros = (from e in lista_ordenes select e.monto_aprobado_suministros).Sum();

            decimal resultado = (construccion + ingenieria + suministros);


            var presupuesto = Repository.Get(Id);
            presupuesto.monto_ofertado = montoingenieria + montoconstruccion + montosuministros;
            presupuesto.monto_so_aprobado = resultado;


            var actualizado = Repository.Update(presupuesto);


        }

        public List<OfertaComercialDto> ListaContrato(int Id)
        {
            var resultado = Repository.GetAll().Where(c => c.vigente == true).
                Where(c => c.es_final == 1).
                Where(c => c.ContratoId == Id).ToList();

            var proyectos = _ofertacomercialpresupuestorepository
      .GetAll().Where(c => c.vigente).ToList();

            var query = (from o in resultado
                         select new OfertaComercialDto
                         {
                             Id = o.Id,
                             codigo = o.codigo,
                             descripcion = o.descripcion,
                             estado_oferta = o.estado_oferta,
                             nombre_estado = o.Catalogo != null ? o.Catalogo.nombre : "",
                             version = o.version,
                             proyecto_ligados = this.ProyectosLigadosOfertaComercial(o.Id, proyectos.Where(c => c.OfertaComercialId == o.Id)
                         .Select(c => c.Requerimiento.Proyecto.codigo).Distinct().ToArray()),
                             proyecto_ligados_id = this.ProyectosLigadosOfertaComercial(o.Id, proyectos.Where(c => c.OfertaComercialId == o.Id)
                         .Select(c => "" + c.Requerimiento.Proyecto.Id).Distinct().ToArray()),
                             monto_ofertado = o.monto_ofertado,
                             monto_so_aprobado = o.monto_so_aprobado,
                             monto_ofertado_pendiente_aprobacion = o.monto_ofertado_pendiente_aprobacion,
                             orden_proceder = o.orden_proceder,
                             fecha_orden_proceder = o.fecha_orden_proceder,
                             link_ordenProceder = o.link_ordenProceder,
                             tieneOrdenProceder = o.orden_proceder ? "SI" : "NO"

                         }).ToList();


            return query;
        }

        public bool CambiarEstadoOferta(int Id, int CatalogoId)
        {
            var o = Repository.Get(Id);
            o.estado_oferta = CatalogoId;
            var resultado = Repository.Update(o);

            if (resultado.Id > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public List<ArchivosOfertaDto> ListaArchivos(int Id)
        {
            var x = _archivooService.GetAll().Where(c => c.OfertaId == Id)
                .Where(c => c.vigente == true).ToList();

            var lista = (from e in x
                         select new ArchivosOfertaDto
                         {
                             Id = e.Id,
                             ArchivoId = e.ArchivoId,
                             Archivo = e.Archivo,
                             codigo = e.Archivo.codigo,
                             descripcion = e.Archivo.nombre,
                             OfertaId = e.OfertaId,
                             vigente = e.vigente
                         }).ToList();


            return lista;

        }

        public bool GuardarArchivo(HttpPostedFileBase UploadedFile, int Id)
        {
            if (UploadedFile != null)
            {

                string fileName = UploadedFile.FileName;
                string fileContentType = UploadedFile.ContentType;
                byte[] fileBytes = new byte[UploadedFile.ContentLength];
                var data = UploadedFile.InputStream.Read(fileBytes, 0,
                    Convert.ToInt32(UploadedFile.ContentLength));


                Archivo a = new Archivo()
                {
                    Id = 0,
                    codigo = "A" + (_archivooService.GetAll().Where(c => c.OfertaId == Id).Where(c => c.vigente == true).ToList().Count() + 1),
                    nombre = fileName,
                    hash = fileBytes,
                    tipo_contenido = fileContentType,
                    fecha_registro = DateTime.Now,
                    vigente = true


                };
                var ArchivoId = _archivoRepository.InsertAndGetId(a);

                ArchivosOferta ao = new ArchivosOferta()
                {
                    Id = 0,
                    ArchivoId = ArchivoId,
                    OfertaId = Id,
                    vigente = true
                };

                var archivooferta = _archivooService.Insert(ao);


                return true;


            }
            return false;
        }

        public List<TransmitalCabeceraDto> ListarTransmitals()
        {
            var transmitals = _transmitalRepository.GetAllIncluding(c => c.Cliente, c => c.Contrato, c => c.Empresa, c => c.OfertaComercial).Where(c => c.vigente).ToList();

            var listadto = (from t in transmitals
                            select new TransmitalCabeceraDto()
                            {
                                Id = t.Id,
                                cliente = t.Cliente != null ? t.Cliente.razon_social : "",
                                empresa = t.Empresa != null ? t.Empresa.razon_social : "",
                                contrato = t.Contrato.Codigo,
                                codigo_oferta_comercial = t.OfertaComercial != null ? t.OfertaComercial.codigo : "",
                                version_oferta_comercial = t.OfertaComercial != null ? t.OfertaComercial.version : "",
                                fecha_emision = t.fecha_emision,
                                estado = t.estado,
                                descripcion = t.descripcion,
                                codigo_transmital = t.codigo_transmital,
                                version = t.version,
                                EmpresaId = t.EmpresaId,
                                ClienteId = t.ClienteId,
                                ContratoId = t.ContratoId,
                                codigo_carta = t.codigo_carta,
                                copia_a = t.copia_a,
                                dirigido_a = t.dirigido_a,
                                enviado_por = t.enviado_por,
                                fecha_recepcion = t.fecha_recepcion,
                                fecha_ultima_modificacion = t.fecha_ultima_modificacion,
                                tipo = t.tipo,
                                tipo_formato = t.tipo_formato,
                                tipo_proposito = t.tipo_proposito,
                                vigente = t.vigente,
                                format_fecha_emision = t.fecha_emision.ToShortDateString(),
                                code = t.OfertaComercial != null ? t.OfertaComercial.codigo : "N/A"

                            }).ToList();


            foreach (var item in listadto)
            {
                if (item.dirigido_a != null && item.dirigido_a.Length > 0)
                {
                    List<Colaborador> e = new List<Colaborador>();
                    string[] dirigidos = item.dirigido_a.Split(',');
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
                        item.listdirigidos = e;
                    }
                }
                if (item.copia_a != null && item.copia_a.Length > 0)
                {
                    List<Colaborador> e = new List<Colaborador>();
                    string[] copia_a = item.copia_a.Split(',');
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
                        item.listcopia = e;
                    }
                }
            }
            return listadto;
        }


        public TransmitalCabeceraDto ListarTransmitalsId(int Id)
        {
            var t = _transmitalRepository.GetAllIncluding(c => c.Cliente, c => c.Contrato, c => c.Empresa, c => c.OfertaComercial).Where(c => c.vigente).Where(c => c.Id == Id).FirstOrDefault();

            var listadto = new TransmitalCabeceraDto()
            {
                Id = t.Id,
                cliente = t.Cliente != null ? t.Cliente.razon_social : "",
                empresa = t.Empresa != null ? t.Empresa.razon_social : "",
                contrato = t.Contrato.Codigo,
                codigo_oferta_comercial = t.OfertaComercial != null ? t.OfertaComercial.codigo : "",
                version_oferta_comercial = t.OfertaComercial != null ? t.OfertaComercial.version : "",
                fecha_emision = t.fecha_emision,
                estado = t.estado,
                descripcion = t.descripcion,
                codigo_transmital = t.codigo_transmital,
                version = t.version,
                EmpresaId = t.EmpresaId,
                ClienteId = t.ClienteId,
                ContratoId = t.ContratoId,
                codigo_carta = t.codigo_carta,
                copia_a = t.copia_a,
                dirigido_a = t.dirigido_a,
                enviado_por = t.enviado_por,
                fecha_recepcion = t.fecha_recepcion,
                fecha_ultima_modificacion = t.fecha_ultima_modificacion,
                tipo = t.tipo,
                tipo_formato = t.tipo_formato,
                tipo_proposito = t.tipo_proposito,
                vigente = t.vigente,
                format_fecha_emision = t.fecha_emision.ToShortDateString(),
                code = t.OfertaComercial != null ? t.OfertaComercial.codigo : "N/A"

            };


            if (listadto.dirigido_a != null && listadto.dirigido_a.Length > 0)
            {
                List<Colaborador> e = new List<Colaborador>();
                string[] dirigidos = listadto.dirigido_a.Split(',');
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
                    listadto.listdirigidos = e;
                }
            }
            if (listadto.copia_a != null && listadto.copia_a.Length > 0)
            {
                List<Colaborador> e = new List<Colaborador>();
                string[] copia_a = listadto.copia_a.Split(',');
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
                    listadto.listcopia = e;
                }
            }

            return listadto;
        }


        public List<TransmitalCabeceraDto> ListarTransmitalsPorContrato(int id)
        {
            var transmitals = _transmitalRepository.GetAllIncluding(c => c.Cliente, c => c.Contrato, c => c.Empresa, c => c.OfertaComercial)
                                                            .Where(c => c.vigente)
                                                            .Where(c => c.ContratoId == id)
                                                            .ToList();

            var listadto = (from t in transmitals
                            select new TransmitalCabeceraDto()
                            {
                                Id = t.Id,
                                cliente = t.Cliente != null ? t.Cliente.razon_social : " ",
                                empresa = t.Empresa != null ? t.Empresa.razon_social : " ",
                                contrato = t.Contrato != null ? t.Contrato.Codigo : " ",
                                codigo_oferta_comercial = t.OfertaComercial != null ? t.OfertaComercial.codigo : " ",
                                version_oferta_comercial = t.OfertaComercial != null ? t.OfertaComercial.version : "",
                                fecha_emision = t.fecha_emision,
                                descripcion = t.descripcion,
                                estado = t.estado,
                                codigo_transmital = t.codigo_transmital,
                                version = t.version,
                                EmpresaId = t.EmpresaId,
                                ClienteId = t.ClienteId,
                                ContratoId = t.ContratoId,
                                codigo_carta = t.codigo_carta,
                                copia_a = t.copia_a,
                                dirigido_a = t.dirigido_a,
                                enviado_por = t.enviado_por,
                                fecha_recepcion = t.fecha_recepcion,
                                fecha_ultima_modificacion = t.fecha_ultima_modificacion,
                                tipo = t.tipo,
                                tipo_formato = t.tipo_formato,
                                tipo_proposito = t.tipo_proposito,
                                vigente = t.vigente,
                                format_fecha_emision = t.fecha_emision.ToShortDateString(),
                                code = t.OfertaComercial != null ? t.OfertaComercial.codigo : "N/A"
                            }).ToList();
            foreach (var item in listadto)
            {
                if (item.dirigido_a != null && item.dirigido_a.Length > 0)
                {
                    List<Colaborador> e = new List<Colaborador>();
                    string[] dirigidos = item.dirigido_a.Split(',');
                    if (dirigidos.Length > 0)
                    {
                        foreach (var d in dirigidos)
                        {
                            var id2 = Int32.Parse(d);
                            var colaborador = _colaboradorRepository.GetAllIncluding(c => c.Cliente).Where(c => c.Id == id2).FirstOrDefault();
                            if (colaborador != null && colaborador.Id > 0)
                            {
                                e.Add(colaborador);
                            }
                        }
                        item.listdirigidos = e;
                    }
                }
                if (item.copia_a != null && item.copia_a.Length > 0)
                {
                    List<Colaborador> e = new List<Colaborador>();
                    string[] copia_a = item.copia_a.Split(',');
                    if (copia_a.Length > 0)
                    {
                        foreach (var d in copia_a)
                        {
                            var id3 = Int32.Parse(d);
                            var colaborador = _colaboradorRepository.GetAllIncluding(c => c.Cliente).Where(c => c.Id == id3).FirstOrDefault();
                            if (colaborador != null && colaborador.Id > 0)
                            {
                                e.Add(colaborador);
                            }
                        }
                        item.listcopia = e;
                    }
                }
            }
            return listadto;
        }

        public string GenerarWordOfertaComercial(int id)
        {
            string resultado = "";
            var oc = this.GetDetalles(id);
            var versiones = this.ListaVersiones(oc.Id).OrderBy(c => c.version).ToList();

            var requerimientos = _ofertacomercialpresupuestorepository.GetAll()
                                                                  .Where(c => c.vigente)
                                                                  .Where(c => c.OfertaComercialId == id)
                                                                  .Select(c => c.Requerimiento).ToList();

            if (requerimientos.Count == 1)
            {
                var req = requerimientos[0];
                if (req.tipo_requerimiento == TipoRequerimiento.Principal)
                {
                    resultado = "Propuesta de Trabajos para ";
                }
                else
                {
                    resultado = "Propuesta de Trabajos Adicionales para ";
                }

            }
            else if (requerimientos.Count > 1)
            {

                resultado = "Propuesta de Trabajos Adicionales para ";
            }


            var oferta_comercial = Repository.Get(id);

            var proyectosligados = _ofertacomercialpresupuestorepository.GetAll().Where(c => c.vigente)
                                                       .Where(c => c.OfertaComercialId == id)
                                                       .Select(c => c.Requerimiento.Proyecto.codigo).Distinct().ToArray();
            if (proyectosligados.Length > 0)
            {
                resultado = resultado + string.Join(" ; ", proyectosligados);

            }

            // resultado = resultado + " Trabajos comunitarios";

            string usuario = "";
            string user = System.Web.HttpContext.Current.User.Identity.Name.ToString();
            string userId = System.Web.HttpContext.Current.User.Identity.GetUserId().ToString();
            if (user.Length > 4)
            {
                String var = user;
                int tam_var = var.Length;
                String Var_Sub = var.Substring((tam_var - 3), 3);
                usuario = Var_Sub;
            }
            string filename = "";

            if (oc.Contrato.Formato.Value == FormatoContrato.Contrato_2016)
            {
                filename = System.Web.HttpContext.Current.Server.MapPath("~/Views/PlantillaWord/OfertaComercial2016.docx");
                //
            }
            else
            {
                filename = System.Web.HttpContext.Current.Server.MapPath("~/Views/PlantillaWord/OfertaComercial2019.docx");
            }
            if (File.Exists((string)filename))
            {
                Random a = new Random();
                var valor = a.Next(1, 100000);
                string salida = System.Web.HttpContext.Current.Server.MapPath("~/Views/PlantillaWord/" + oc.codigo + "_" + oc.version + " " + oc.descripcion + DateTime.Now.Day + "" + DateTime.Now.Month + "" + DateTime.Now.Year + ".docx");

                using (var plantilla = DocX.Load(filename))
                {
                    var document = DocX.Create(salida);
                    document.InsertDocument(plantilla);
                    document.ReplaceText("<SITIO_REFERENCIA>", oc.Contrato != null ? oc.Contrato.sitio_referencia : "<SITIO_REFERENCIA>");
                    document.ReplaceText("<Acuerdo_No>", oc.Contrato != null ? oc.Contrato.Codigo : "<Acuerdo_No>");
                    document.ReplaceText("<CODIGO_OFERTA>", oc.codigo);
                    document.ReplaceText("<VERSION_OFERTA>", oc.version);
                    document.ReplaceText("<DESCRIPCION_OFERTA>", resultado + " - " + oc.descripcion);
                    document.ReplaceText("<USER>", usuario.ToUpper());
                    document.ReplaceText("<FECHA_HOY>", DateTime.Now.Date.ToShortDateString());
                    document.ReplaceText("<FECHA_OFERTA>", DateTime.Now.Date.ToShortDateString());
                    var requerimiento = (from r in requerimientos where r.tipo_requerimiento == TipoRequerimiento.Principal select r).FirstOrDefault();

                    if (requerimiento != null && requerimiento.Id > 0)
                    {
                        document.ReplaceText("<TIPO_REQUERIMIENTO>", requerimiento.tipo_requerimiento == TipoRequerimiento.Principal ? "" : requerimiento.tipo_requerimiento.EnumDescription());
                        document.ReplaceText("<CODIGO_PROYECTO>", requerimiento.Proyecto != null ? requerimiento.Proyecto.codigo : "");
                        document.ReplaceText("<DESCRIPCION_REQUERIMIENTO>", requerimiento.descripcion);
                        document.ReplaceText("<ALCANCE_REQUERIMIENTO>", requerimiento.alcance != null ? requerimiento.alcance : "<ALCANCE_REQUERIMIENTO>");
                        document.ReplaceText("<FECHA_MINIMA_REQUERIMIENTO>", requerimiento.fecha_recepcion.ToShortDateString());
                        //document.ReplaceText("<PLAZO_SEMANAS_PROYECTO>", requerimiento.fecha_recepcion.);//verificar

                        var presupuesto = _presupuestorepository.GetAll().Where(c => c.vigente).Where(c => c.es_final).Where(c => c.RequerimientoId == requerimiento.Id).
                            FirstOrDefault();
                        if (presupuesto != null && presupuesto.Id > 0)
                        {
                            document.ReplaceText("<CLASE_PRESUPUESTO>", presupuesto.Clase.HasValue ? presupuesto.Clase.EnumDescription().ToString().ToUpper() : "<CLASE_PRESUPUESTO>");
                        }
                    }
                    else
                    {
                        var adicionales = (from r in requerimientos where r.tipo_requerimiento == TipoRequerimiento.Adicional select r).ToList();

                        var valormayor = 0;
                        foreach (var ad in adicionales)
                        {
                            var presupuesto = _presupuestorepository.GetAll().Where(c => c.vigente).Where(c => c.es_final).Where(c => c.RequerimientoId == ad.Id).
                            FirstOrDefault();

                            if (presupuesto != null && presupuesto.Id > 0)
                            {
                                var total = presupuesto.monto_ingenieria + presupuesto.monto_construccion + presupuesto.monto_suministros + presupuesto.monto_subcontratos;
                                if (total > valormayor)
                                {
                                    document.ReplaceText("<CLASE_PRESUPUESTO> ", presupuesto.Clase.HasValue ? presupuesto.Clase.EnumDescription().ToString().ToUpper() : "<CLASE_PRESUPUESTO>");
                                }
                            }
                        }

                    }


                    var tables = document.Tables.ToList();

                    int fila = 2;
                    int principal = 1;
                    foreach (var v in versiones)
                    {
                        if (principal == 1)
                        {

                            tables[2].Rows[principal].Cells[0].Paragraphs[0].Append("A");
                            tables[2].Rows[principal].Cells[1].Paragraphs[0].Append(v.fecha_primer_envio.HasValue ? v.fecha_primer_envio.Value.ToShortDateString() : DateTime.Now.Date.ToShortDateString());
                            tables[2].Rows[principal].Cells[2].Paragraphs[0].Append("Todas");
                            tables[2].Rows[principal].Cells[3].Paragraphs[0].Append("Emisión para Comentarios");
                        }
                        tables[2].Rows[fila].Cells[0].Paragraphs[0].Append(v.version);
                        tables[2].Rows[fila].Cells[1].Paragraphs[0].Append(v.fecha_primer_envio.HasValue ? v.fecha_primer_envio.Value.ToShortDateString() : DateTime.Now.Date.ToShortDateString());
                        tables[2].Rows[fila].Cells[2].Paragraphs[0].Append("Todas");
                        tables[2].Rows[fila].Cells[3].Paragraphs[0].Append("Emisión para Aprobación");
                        principal++;
                        fila++;
                    }
                    int row = 2;
                    int rowprincipal = 1;
                    foreach (var v in versiones)
                    {
                        if (rowprincipal == 1)
                        {

                            tables[1].Rows[rowprincipal].Cells[0].Paragraphs[0].Append("A");
                            tables[1].Rows[rowprincipal].Cells[1].Paragraphs[0].Append(v.fecha_primer_envio.HasValue ? v.fecha_primer_envio.Value.ToShortDateString() : DateTime.Now.Date.ToShortDateString());
                            tables[1].Rows[rowprincipal].Cells[3].Paragraphs[0].Append(usuario.ToUpper());
                            tables[1].Rows[rowprincipal].Cells[4].Paragraphs[0].Append("JIC");
                            tables[1].Rows[rowprincipal].Cells[5].Paragraphs[0].Append("QDU");
                        }
                        tables[1].Rows[row].Cells[0].Paragraphs[0].Append(v.version);
                        tables[1].Rows[row].Cells[1].Paragraphs[0].Append(v.fecha_primer_envio.HasValue ? v.fecha_primer_envio.Value.ToShortDateString() : DateTime.Now.Date.ToShortDateString());
                        tables[1].Rows[row].Cells[3].Paragraphs[0].Append(usuario.ToUpper());
                        tables[1].Rows[row].Cells[4].Paragraphs[0].Append("JIC");
                        tables[1].Rows[row].Cells[5].Paragraphs[0].Append("QDU");
                        row++;
                        rowprincipal++;
                    }
                    document.Save();

                    return salida;
                }

            }
            else
            {
                return "";
            }


        }

        public string ProyectosLigadosOfertaComercial(int id, String[] proyectos)
        {

            string resultado = "";
            if (proyectos.Length > 0)
            {
                resultado = string.Join(" ; ", proyectos);

            }

            return resultado;

        }

        public int secuencialofertacomercial()
        {
            int secuencia = 0;
            var listado_codigos = Repository.GetAll().Where(c => c.vigente).Select(c => c.codigo).ToList();
            if (listado_codigos.Count > 0)
            {

                List<int> numeracion = (from l in listado_codigos
                                        select Convert.ToInt32(l.Split('-')[l.Split('-').Length - 1])
                                    ).ToList();

                if (numeracion.Count > 0)
                {
                    secuencia = numeracion.Max() + 1;
                }
            }



            return secuencia;
        }

        public string enviarMensaje(int id)
        {
            var oferta = Repository.Get(id);
            string usuario_nombre = "";
            string user = System.Web.HttpContext.Current.User.Identity.Name.ToString();

            var usuario = _usuarioRepository.GetAll().Where(c => c.Cuenta == user).FirstOrDefault();
            if (usuario != null)
            {
                usuario_nombre = usuario.Nombres + " " + usuario.Apellidos.ToUpper() + " (CPP)";
            }

            try
            {
                //Usuario

                //Configuración del Mensaje
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp-mail.outlook.com");
                //Especificamos el correo desde el que se enviará el Email y el nombre de la persona que lo envía
                mail.From = new MailAddress("pmdis_cpp@outlook.com", "Efrain Saransig", Encoding.UTF8);
                //Aquí ponemos el asunto del correo
                mail.Subject = "Notificación PMDIS";
                //Aquí ponemos el mensaje que incluirá el correo
                mail.Body = "PMDIS le informa que el usuario: " + usuario_nombre + " generó la oferta " + oferta.codigo + " se adjunta";

                //Especificamos a quien enviaremos el Email, no es necesario que sea Gmail, puede ser cualquier otro proveedor
                mail.To.Add("wcarrasco@cpp.com.ec");
                mail.To.Add("efrain.saransig@atikasoft.com");
                //Si queremos enviar archivos adjuntos tenemos que especificar la ruta en donde se encuentran

                string archivo = System.Web.HttpContext.Current.Server.MapPath("~/Views/Archivos/3808-B-SP-000434.rar");
                if (File.Exists((string)archivo))
                {
                    mail.Attachments.Add(new Attachment(archivo));
                }


                //Configuracion del SMTP
                SmtpServer.Port = 587; //Puerto que utiliza Gmail para sus servicios
                                       //Especificamos las credenciales con las que enviaremos el mail
                SmtpServer.UseDefaultCredentials = false;
                SmtpServer.Credentials = new System.Net.NetworkCredential("pmdis_cpp@outlook.com", "pmdis.2019");
                SmtpServer.Port = 587;
                SmtpServer.UseDefaultCredentials = false;
                SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
                SmtpServer.EnableSsl = true;
                SmtpServer.Send(mail);

            }
            catch (Exception ex)
            {
                Logger.Debug(ex.Message);
                Logger.Error(ex.Message);
                Logger.Warn(ex.Message);
                ElmahExtension.LogToElmah(ex);

            }
            return "";
        }

        public async Task EnviarMail(int id)
        {
            var oferta = Repository.Get(id);
            string usuario_nombre = "";
            string user = System.Web.HttpContext.Current.User.Identity.Name.ToString();

            var usuario = _usuarioRepository.GetAll().Where(c => c.Cuenta == user).FirstOrDefault();
            if (usuario != null)
            {
                usuario_nombre = usuario.Nombres + " " + usuario.Apellidos.ToUpper() + " (CPP)";
            }

            try
            {
                //3. Enviar Mensaje..
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress("pmdis_cpp@outlook.com", "Efrain Saransig", Encoding.UTF8);
                //Aquí ponemos el asunto del correo
                mail.Subject = "Notificación PMDIS";
                //Aquí ponemos el mensaje que incluirá el correo
                mail.Body = "PMDIS le informa que el usuario: " + usuario_nombre + " generó la oferta " + oferta.codigo + " se adjunta";

                //Especificamos a quien enviaremos el Email, no es necesario que sea Gmail, puede ser cualquier otro proveedor
                mail.To.Add("efrain.saransig@atikasoft.com");
                mail.To.Add("simunoz@proveedores.techint.com");
                //Si queremos enviar archivos adjuntos tenemos que especificar la ruta en donde se encuentran

                string archivo = System.Web.HttpContext.Current.Server.MapPath("~/Views/Archivos/3808-B-SP-000434.rar");
                if (File.Exists((string)archivo))
                {
                    mail.Attachments.Add(new Attachment(archivo));
                }

                await EmailService.SendAsync(mail, true);


            }
            catch (Exception ex)
            {
                ManejadorExcepciones.HandleException(ex);
            }

        }

        public async Task<string> Send_Files_OfertaComercial(int Id, bool user_transmittal, string asunto = "", string body = "", string urltransmittal = "")
        {
            var oferta_comercial = Repository.Get(Id);

            string result = "";
            var transmital = _transmitalRepository.GetAll().Where(c => c.vigente)
                                                         .Where(c => c.OfertaComercialId.HasValue)
                                                         .Where(c => c.OfertaComercialId == Id)
                                                         .FirstOrDefault();
            if (transmital != null && transmital.Id > 0)
            {
                /* */
                var lista_archivos = _detalletransmitalRepository.GetAllIncluding(c => c.Transmital.OfertaComercial, c => c.Archivo)
                                             .Where(c => c.vigente)
                                             .Where(c => c.Transmital.OfertaComercialId.HasValue)
                                             .Where(c => c.Transmital.OfertaComercialId == Id)
                                             .Where(c => c.Transmital.vigente)
                                             .ToList();
                if (lista_archivos.Count > 0)
                {
                    var correos_lista = _correslistarepository.GetAll().Where(c => c.vigente)
                                            .Where(c => c.ListaDistribucion.vigente)
                                            .Where(c => c.ListaDistribucion.codigo == CatalogosCodigos.LISTADISTRIBUCION_OFERTA_COMERCIAL).ToList();

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
                        foreach (var item in correos_lista)
                        {
                            message.To.Add(item.correo);
                            ElmahExtension.LogToElmah(new Exception("Send Adjuntos Ofert-Transmital: " + item.correo));
                        }
                        if (user_transmittal)
                        {
                            string dirigido = "";
                            if (transmital != null && transmital.dirigido_a.Length > 0)
                            {
                                String[] copias = transmital.dirigido_a.Split(',');
                                if (copias.Length > 0)
                                {
                                    foreach (var c in copias)
                                    {
                                        var colaborador = _colaboradorRepository.Get(Int32.Parse(c));
                                        if (colaborador != null)
                                        {
                                            message.To.Add(colaborador.correo);
                                            ElmahExtension.LogToElmah(new Exception("Send Adjuntos Ofert-Transmital: " + colaborador.correo));
                                        }
                                    }
                                }

                            }
                            string copia = "";
                            if (transmital != null && transmital.copia_a.Length > 0)
                            {
                                String[] copias = transmital.copia_a.Split(',');
                                if (copias.Length > 0)
                                {
                                    foreach (var c in copias)
                                    {
                                        var colaborador = _colaboradorRepository.Get(Int32.Parse(c));
                                        if (colaborador != null)
                                        {
                                            message.To.Add(colaborador.correo);
                                            ElmahExtension.LogToElmah(new Exception("Send Adjuntos Ofert-Transmital: " + colaborador.correo));
                                        }
                                    }
                                }

                            }
                        }



                        Random ax = new Random();
                        var valorx = ax.Next(1, 100);
                        var zipFile = System.Web.HttpContext.Current.Server.MapPath("~/Views/ArchivosProyectos/AdjuntosTransmittals/" + oferta_comercial.codigo + "-" + oferta_comercial.version + "-" + valorx + ".zip");


                        using (var archive = ZipFile.Open(zipFile, ZipArchiveMode.Create))
                        {
                            foreach (var ar in lista_archivos)
                            {
                                var archivo = ar.Archivo;
                                if (archivo != null)
                                {
                                    //Save the Byte Array as File.
                                    Random a = new Random();
                                    var valor = a.Next(1, 100);
                                    string path = System.Web.HttpContext.Current.Server.MapPath("~/Views/ArchivosProyectos/AdjuntosTransmittals/" + archivo.nombre);
                                    File.WriteAllBytes(path, archivo.hash);
                                    if (File.Exists((string)path))
                                    {
                                        archive.CreateEntryFromFile(path, archivo.nombre);
                                    }

                                }
                            }

                        }
                        if (File.Exists((string)zipFile))
                        {

                            message.Attachments.Add(new Attachment(zipFile));

                        }
                        if (File.Exists((string)urltransmittal))
                        {
                            var urlpdf = this.wordtopdf(transmital.codigo_transmital, urltransmittal);
                            if (File.Exists((string)urlpdf))
                            {
                                message.Attachments.Add(new Attachment(urlpdf));
                            }
                        }
                        try
                        {
                            await _correoservice.SendWithFilesAsync(message);
                            var o = Repository.Get(Id);

                            var estadopresentado = _catalogoRepository.GetAll().Where(c => c.codigo == CatalogosCodigos.OFERTA_ESTADO_PRESENTADO).FirstOrDefault();
                            if (estadopresentado != null && estadopresentado.Id > 0)
                            {

                                o.estado_oferta = estadopresentado.Id;
                                var resultado = Repository.Update(o);

                                var requerimientos = _ofertacomercialpresupuestorepository.GetAllIncluding(c => c.Requerimiento).Where(c => c.OfertaComercialId == o.Id)
                                    .Where(c => c.vigente).ToList();

                                if (requerimientos.Count > 0)
                                {
                                    foreach (var item in requerimientos)
                                    {


                                        var requerimiento = _requerimientoRepository.GetAll().Where(c => c.Id == item.RequerimientoId).FirstOrDefault();
                                        if (requerimiento != null && requerimiento.Id > 0)
                                        {
                                            requerimiento.EstadoOfertaId = estadopresentado.Id;
                                            _requerimientoRepository.Update(requerimiento);
                                        }
                                    }
                                }



                            }

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

        public string wordtopdf(string transmittalcode, string urltransmittal)
        {
            string path = "";
            try
            {
                //Loads an existing Word document
                WordDocument wordDocument = new WordDocument(urltransmittal, FormatType.Docx);
                //wordDocument.Watermark = null;
                //Creates an instance of the DocToPDFConverter
                DocToPDFConverter converter = new DocToPDFConverter();


                //Converts Word document into PDF document
                PdfDocument pdfDocument = converter.ConvertToPDF(wordDocument);

                //Releases all resources used by DocToPDFConverter
                converter.Dispose();

                //Closes the instance of document objects
                wordDocument.Close();

                //Saves the PDF file 
                Random a = new Random();
                var valor = a.Next(1, 100);
                string url = System.Web.HttpContext.Current.Server.MapPath("~/Views/ArchivosProyectos/AdjuntosTransmittals/" + transmittalcode + "-" + valor + ".pdf");
                pdfDocument.Save(url);

                //Closes the instance of document objects
                pdfDocument.Close(true);

                if (File.Exists((string)url))
                {
                    return url;
                }
                else
                {
                    return "";
                }
            }
            catch (Exception x)
            {

                path = "";
            }
            return path;

        }

        public async Task<string> Send_Files_OfertaComercialList(int Id, bool user_transmittal, List<UserCorreos> list, string body = "")
        {
            var oferta_comercial = Repository.Get(Id);

            string result = "";
            var transmital = _transmitalRepository.GetAll().Where(c => c.vigente)
                                                         .Where(c => c.OfertaComercialId.HasValue)
                                                         .Where(c => c.OfertaComercialId == Id)
                                                         .FirstOrDefault();
            if (transmital != null && transmital.Id > 0)
            {
                /* */
                var lista_archivos = _detalletransmitalRepository.GetAllIncluding(c => c.Transmital.OfertaComercial, c => c.Archivo)
                                             .Where(c => c.vigente)
                                             .Where(c => c.Transmital.OfertaComercialId.HasValue)
                                             .Where(c => c.Transmital.OfertaComercialId == Id)
                                             .Where(c => c.Transmital.vigente)
                                             .ToList();
                if (lista_archivos.Count > 0)
                {


                    if (list.Count > 0)
                    {

                        /* ES: Envio de Archi*/
                        MailMessage message = new MailMessage();
                        SmtpClient SmtpServer = new SmtpClient("smtp.teic.techint.net");

                        SmtpServer.Port = 25;

                        SmtpServer.EnableSsl = false;

                        message.From = new MailAddress("adm_contractual.auca@cpp.com.ec");

                        message.Subject = "PMDIS: Envió Adjuntos Oferta Comercial-Transmittal " + oferta_comercial.codigo + " " + transmital.codigo_transmital;
                        message.Body = body;
                        foreach (var item in list)
                        {
                            message.To.Add(item.correo);
                            ElmahExtension.LogToElmah(new Exception("Send Adjuntos Ofert-Transmital: " + item.correo));
                        }
                        if (user_transmittal)
                        {
                            string dirigido = "";
                            if (transmital != null && transmital.dirigido_a.Length > 0)
                            {
                                String[] copias = transmital.dirigido_a.Split(',');
                                if (copias.Length > 0)
                                {
                                    foreach (var c in copias)
                                    {
                                        var colaborador = _colaboradorRepository.Get(Int32.Parse(c));
                                        if (colaborador != null)
                                        {
                                            message.To.Add(colaborador.correo);
                                            ElmahExtension.LogToElmah(new Exception("Send Adjuntos Ofert-Transmital: " + colaborador.correo));
                                        }
                                    }
                                }

                            }
                            string copia = "";
                            if (transmital != null && transmital.copia_a.Length > 0)
                            {
                                String[] copias = transmital.copia_a.Split(',');
                                if (copias.Length > 0)
                                {
                                    foreach (var c in copias)
                                    {
                                        var colaborador = _colaboradorRepository.Get(Int32.Parse(c));
                                        if (colaborador != null)
                                        {
                                            message.To.Add(colaborador.correo);
                                            ElmahExtension.LogToElmah(new Exception("Send Adjuntos Ofert-Transmital: " + colaborador.correo));
                                        }
                                    }
                                }

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
                            var o = Repository.Get(Id);
                            o.estado_oferta = 5184;
                            var resultado = Repository.Update(o);
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

        public ExcelPackage ReporteAdicionales(ReportDto r)
        {
            ExcelPackage excel = new ExcelPackage();

            string filename = System.Web.HttpContext.Current.Server.MapPath("~/Views/PlantillaWord/ReporteAdicionales.xlsx");
            if (File.Exists((string)filename))
            {
                FileInfo newFile = new FileInfo(filename);
                ExcelPackage pck = new ExcelPackage(newFile);
                excel.Workbook.Worksheets.Add("ADICIONALES", pck.Workbook.Worksheets[1]);
                excel.Workbook.Worksheets.Add("PENDIENTES DE EMISIÓN", pck.Workbook.Worksheets[2]);
                excel.Workbook.Worksheets.Add("PENDIENTES DE APROBACIÓN", pck.Workbook.Worksheets[3]);
                excel.Workbook.Worksheets.Add("GRAFICOS", pck.Workbook.Worksheets[4]);
            }
            var data = this.GetDatosAdicionales(r);


            ExcelWorksheet h = excel.Workbook.Worksheets[1];
            h.View.ZoomScale = 70;

            ExcelWorksheet h2 = excel.Workbook.Worksheets[2];
            h2.View.ZoomScale = 70;

            ExcelWorksheet h3 = excel.Workbook.Worksheets[3];
            h3.View.ZoomScale = 70;

            ExcelWorksheet worksheet = excel.Workbook.Worksheets[4];
            int count = 2;
            int rowdatagrafico = 9;
            foreach (var i in data)
            {
                string cell = "A" + count;
                h.Cells[cell].Value = i.contrato;
                h.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                h.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                cell = "A" + rowdatagrafico; //grap
                worksheet.Cells[cell].Value = i.codigoProyecto;


                cell = "B" + count;
                h.Cells[cell].Value = i.codigoProyecto;
                h.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                h.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                cell = "B" + rowdatagrafico; //grap
                worksheet.Cells[cell].Value = i.codigoRequerimiento;

                cell = "C" + count;
                h.Cells[cell].Value = i.descripcionProyecto;
                //h.Cells[cell].Style.WrapText = true;
                h.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                h.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                cell = "C" + rowdatagrafico; //grap
                worksheet.Cells[cell].Value = i.clasificacion;

                cell = "D" + count;
                h.Cells[cell].Value = i.codigoRequerimiento;
                h.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                h.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                cell = "D" + rowdatagrafico; //grap
                worksheet.Cells[cell].Value = i.descripcionRequerimiento;

                cell = "E" + count;
                h.Cells[cell].Value = i.descripcionRequerimiento;
                //h.Cells[cell].Style.WrapText = true;
                h.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                h.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                cell = "E" + rowdatagrafico; //grap
                worksheet.Cells[cell].Value = i.estadoOferta;

                /*cell = "F" + count;
                h.Cells[cell].Value = i.estadoRequerimiento;
                */
                cell = "F" + count;
                h.Cells[cell].Value = i.requiereCronograma;
                h.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                h.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                cell = "F" + rowdatagrafico; //grap
                worksheet.Cells[cell].Value = i.fechaOferta;

                cell = "G" + count;
                h.Cells[cell].Value = i.fechaRegistroRequerimiento;
                h.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                h.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                cell = "G" + rowdatagrafico; //grap
                worksheet.Cells[cell].Value = i.ofertaPresentada;

                cell = "H" + count;
                h.Cells[cell].Value = i.fechaCargaPresupuesto;
                h.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                h.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                cell = "H" + rowdatagrafico; //grap
                worksheet.Cells[cell].Value = i.montoIngenieria;
                worksheet.Cells[cell].Style.Numberformat.Format = "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";

                cell = "I" + count;
                h.Cells[cell].Value = i.estadoOferta;
                h.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                h.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                cell = "I" + rowdatagrafico; //grap
                worksheet.Cells[cell].Value = i.montoSuministros;
                worksheet.Cells[cell].Style.Numberformat.Format = "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";

                cell = "J" + count;

                cell = "J" + rowdatagrafico; //grap
                worksheet.Cells[cell].Value = i.montoConstruccion;
                worksheet.Cells[cell].Style.Numberformat.Format = "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";


                cell = "K" + count;
                h.Cells[cell].Value = i.ofertaPresentada;
                h.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                h.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                cell = "K" + rowdatagrafico; //grap
                worksheet.Cells[cell].Value = i.monto;
                worksheet.Cells[cell].Style.Numberformat.Format = "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";

                cell = "L" + count;
                h.Cells[cell].Value = i.versionOferta;
                h.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                h.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                cell = "M" + count;
                h.Cells[cell].Value = i.fechaEmisionOferta;
                h.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                h.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                cell = "N" + count;
                h.Cells[cell].Value = i.fechaUltimoEnvio;
                h.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                h.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                cell = "O" + count;
                h.Cells[cell].Value = i.montoTotalPresupuesto;// i.monto;
                h.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                h.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                h.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                cell = "P" + count;
                h.Cells[cell].Value = i.montoConstruccionPresupuesto;//*i.montoConstruccion;
                h.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                h.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                h.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                cell = "Q" + count;
                h.Cells[cell].Value = i.montoIngenieriaPresupuesto;//  i.montoIngenieria;
                h.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                h.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                h.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                cell = "R" + count;
                h.Cells[cell].Value = i.montoSuministrosPresupuesto;// i.montoSuministros;
                h.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                h.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                h.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                cell = "S" + count;
                h.Cells[cell].Value = i.montoSubcontratosPresupuesto;// i.montoSubcontratos;
                h.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                h.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                h.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                cell = "T" + count;
                h.Cells[cell].Value = i.fechaRecepcionSolicitud;
                h.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                h.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                cell = "U" + count;
                h.Cells[cell].Value = i.fechadePresupuesto;
                h.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                h.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                cell = "V" + count;
                h.Cells[cell].Value = i.responsable;
                h.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                h.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                cell = "W" + count;
                h.Cells[cell].Value = i.fechaOferta;
                h.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                h.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                cell = "X" + count;
                h.Cells[cell].Value = i.clasificacion;
                h.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                h.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                cell = "Y" + count;
                h.Cells[cell].Value = i.comentarios;
                h.Cells[cell].Style.WrapText = false;
                h.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                h.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                cell = "Z" + count;
                h.Cells[cell].Value = i.solicitante;
                h.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                h.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                cell = "AA" + count;
                h.Cells[cell].Value = i.medioSolicitud;
                h.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                h.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                cell = "AB" + count;
                h.Cells[cell].Value = i.monto_ofertado_migracion_actual;// i.Monto Migracion;
                h.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                h.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                h.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell = "AC" + count;
                h.Cells[cell].Value = (i.montoofertado + i.monto_ofertado_migracion_actual);// i.Monto Migracion;
                h.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                h.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                h.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                h.Row(count).Height = 60;


                count++;
                rowdatagrafico++;

            }

            h.DeleteColumn(8); //Fecha Carga Presupuesto H
            h.DeleteColumn(9);//Aprobado Shaya J
            h.DeleteColumn(19);//Adicionales fecha Prespuestacion U
            h.DeleteColumn(24);//Adicionales AA mEDIO sOLICITUD


            var pedientesemision = (from a in data where a.estadoOfertaCodigo == "TEnRevision" select a).ToList();
            count = 2;
            int number = 1;
            foreach (var i in pedientesemision)
            {
                string cell = "A" + count;
                h2.Cells[cell].Value = number;
                h2.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                h2.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                cell = "B" + count;
                h2.Cells[cell].Value = i.contrato;
                h2.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                h2.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                cell = "C" + count;
                h2.Cells[cell].Value = i.codigoProyecto;
                h2.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                h2.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                cell = "D" + count;
                h2.Cells[cell].Value = i.codigoRequerimiento;
                h2.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                h2.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                cell = "E" + count;
                h2.Cells[cell].Value = i.descripcionRequerimiento;
                h2.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                // h2.Cells[cell].Style.WrapText = true;
                h2.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                cell = "F" + count;
                h2.Cells[cell].Value = i.fechaRegistroRequerimiento;
                h2.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                h2.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                cell = "G" + count;
                h2.Cells[cell].Value = i.fechaCargaPresupuesto;
                h2.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                h2.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                cell = "H" + count;
                h2.Cells[cell].Value = i.monto;
                h2.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                h2.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                h2.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                cell = "I" + count;
                h2.Cells[cell].Value = i.montoConstruccion;
                h2.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                h2.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                h2.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                cell = "J" + count;
                h2.Cells[cell].Value = i.montoIngenieria;
                h2.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                h2.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                h2.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                cell = "K" + count;
                h2.Cells[cell].Value = i.montoSuministros;
                h2.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                cell = "L" + count;
                h2.Cells[cell].Value = i.montoSubcontratos;
                h2.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                h2.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                h2.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                cell = "M" + count;
                h2.Cells[cell].Value = i.ofertaPresentada;
                h2.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                h2.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                cell = "N" + count;
                h2.Cells[cell].Value = i.versionOferta;
                h2.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                h2.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                cell = "O" + count;
                h2.Cells[cell].Value = i.estadoOferta;
                h2.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                cell = "P" + count;
                h2.Cells[cell].Value = i.fechaOferta;
                h2.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                h2.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                cell = "Q" + count;
                h2.Cells[cell].Value = i.monto_ofertado_migracion_actual;// i.Monto Migracion;
                h2.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                h2.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                h2.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell = "R" + count;
                h2.Cells[cell].Value = (i.montoofertado + i.monto_ofertado_migracion_actual);// i.Monto Migracion;
                h2.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                h2.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                h2.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                h2.Row(count).Height = 60;
                count++;
                number++;
            }

            var pendientesaprobacion = (from a in data where a.estadoOfertaCodigo == "TPresentado" select a).ToList();

            count = 2;
            number = 1;
            foreach (var i in pendientesaprobacion)
            {
                string cell = "A" + count;
                h3.Cells[cell].Value = number;
                h3.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                h3.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                cell = "B" + count;
                h3.Cells[cell].Value = i.contrato;
                h3.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                h3.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                cell = "C" + count;
                h3.Cells[cell].Value = i.codigoProyecto;
                h3.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                h3.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                cell = "D" + count;
                h3.Cells[cell].Value = i.codigoRequerimiento;
                h3.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                h3.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                cell = "E" + count;
                h3.Cells[cell].Value = i.descripcionRequerimiento;

                //h3.Cells[cell].Style.WrapText = true;
                h3.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                h3.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                cell = "F" + count;
                h3.Cells[cell].Value = i.fechaRegistroRequerimiento;
                h3.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                h3.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                cell = "G" + count;
                h3.Cells[cell].Value = i.fechaCargaPresupuesto;
                h3.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                h3.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                cell = "H" + count;
                h3.Cells[cell].Value = i.monto;
                h3.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                h3.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                h3.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                cell = "I" + count;
                h3.Cells[cell].Value = i.montoConstruccion;
                h3.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                h3.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                h3.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                cell = "J" + count;
                h3.Cells[cell].Value = i.montoIngenieria;
                h3.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                h3.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                h3.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                cell = "K" + count;
                h3.Cells[cell].Value = i.montoSuministros;
                h3.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                h3.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                h3.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                cell = "L" + count;
                h3.Cells[cell].Value = i.montoSubcontratos;
                h3.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                h3.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                h3.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                cell = "M" + count;
                h3.Cells[cell].Value = i.ofertaPresentada;
                h3.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                h3.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                cell = "N" + count;
                h3.Cells[cell].Value = i.versionOferta;
                h3.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                h3.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                cell = "O" + count;
                h3.Cells[cell].Value = i.estadoOferta;
                h3.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                h3.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                cell = "P" + count;
                h3.Cells[cell].Value = i.fechaOferta;
                h3.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                h3.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;


                cell = "Q" + count;
                h3.Cells[cell].Value = i.monto_ofertado_migracion_actual;// i.Monto Migracion;
                h3.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                h3.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                h3.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell = "R" + count;
                h3.Cells[cell].Value = (i.montoofertado + i.monto_ofertado_migracion_actual);// i.Monto Migracion;
                h3.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                h3.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                h3.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;


                h3.Row(count).Height = 60;
                count++;
                number++;
            }

            int aprobados = (from ap in data where ap.estadoOfertaCodigo == "TAprobado" select ap).ToList().Count;
            int cancelados = (from ap in data where ap.estadoOfertaCodigo == "TCancelado" select ap).ToList().Count;
            int presentados = (from ap in data where ap.estadoOfertaCodigo == "TPresentado" select ap).ToList().Count;
            int pendientesemision = (from ap in data where ap.estadoOfertaCodigo == "TEnRevision" select ap).ToList().Count;


            /*Graficos*/
            //fill cell data with a loop, note that row and column indexes start at 1
            int pierow = 1;

            string celdapie = "S";

            worksheet.Cells[celdapie + pierow].Value = "Aprobado";
            worksheet.Cells[celdapie + (pierow + 1)].Value = aprobados;
            worksheet.Cells[celdapie + pierow].Style.Font.Color.SetColor(Color.White);
            worksheet.Cells[celdapie + (pierow + 1)].Style.Font.Color.SetColor(Color.White);

            celdapie = "T";

            worksheet.Cells[celdapie + pierow].Value = "Cancelado";
            worksheet.Cells[celdapie + (pierow + 1)].Value = cancelados;
            worksheet.Cells[celdapie + pierow].Style.Font.Color.SetColor(Color.White);
            worksheet.Cells[celdapie + (pierow + 1)].Style.Font.Color.SetColor(Color.White);

            celdapie = "U";
            worksheet.Cells[celdapie + pierow].Value = "Presentado";
            worksheet.Cells[celdapie + (pierow + 1)].Value = presentados;
            worksheet.Cells[celdapie + pierow].Style.Font.Color.SetColor(Color.White);
            worksheet.Cells[celdapie + (pierow + 1)].Style.Font.Color.SetColor(Color.White);

            celdapie = "V";
            worksheet.Cells[celdapie + pierow].Value = "Pendientes Emisión";
            worksheet.Cells[celdapie + (pierow + 1)].Value = pendientesemision;
            worksheet.Cells[celdapie + pierow].Style.Font.Color.SetColor(Color.White);
            worksheet.Cells[celdapie + (pierow + 1)].Style.Font.Color.SetColor(Color.White);

            pierow++;
            //create a new piechart of type Pie3D
            ExcelPieChart pieChart = worksheet.Drawings.AddChart("pieChart", eChartType.Pie3D) as ExcelPieChart;

            //set the title
            pieChart.Title.Text = "ADICIONALES 2019 - 2020";

            //select the ranges for the pie. First the values, then the header range
            pieChart.Series.Add(ExcelRange.GetAddress(2, 20, 2, 23), ExcelRange.GetAddress(1, 20, 1, 23)); //Rango de Columnas del los Valores


            //show the percentages in the pie
            pieChart.DataLabel.ShowPercent = true;
            pieChart.DataLabel.ShowCategory = true;
            //size of the chart
            pieChart.SetSize(500, 400);

            //add the chart at cell C5
            pieChart.SetPosition(1, 0, 1, 0);

            pierow++;
            int header = pierow;

            string celda = "T" + header; //Columna  Aprobado Requerimientos Reportes

            worksheet.Cells[celda].Value = "Aprobado";
            worksheet.Cells[celda].Style.Font.Color.SetColor(Color.White);

            celda = "U" + header;

            worksheet.Cells[celda].Value = "Cancelado";
            worksheet.Cells[celda].Style.Font.Color.SetColor(Color.White);

            celda = "V" + header;
            worksheet.Cells[celda].Value = "Presentado";
            worksheet.Cells[celda].Style.Font.Color.SetColor(Color.White);

            celda = "W" + header;
            worksheet.Cells[celda].Value = "Pendientes Emisión";
            worksheet.Cells[celda].Style.Font.Color.SetColor(Color.White);

            var proyectos = (from p in data orderby p.codigoProyecto select p.codigoProyecto).Distinct();

            pierow++;
            int row = pierow;
            int initial = pierow;

            foreach (var proyecto in proyectos)
            {
                celda = "S" + row;
                worksheet.Cells[celda].Value = proyecto;
                worksheet.Cells[celda].Style.Font.Color.SetColor(Color.White);
                aprobados = (from ap in data
                             where ap.estadoOfertaCodigo == "TAprobado"
                             where ap.codigoProyecto == proyecto
                             select ap).ToList().Count;
                cancelados = (from ap in data
                              where ap.estadoOfertaCodigo == "TCancelado"
                              where ap.codigoProyecto == proyecto
                              select ap).ToList().Count;
                presentados = (from ap in data
                               where ap.estadoOfertaCodigo == "TPresentado"
                               where ap.codigoProyecto == proyecto
                               select ap).ToList().Count;
                pendientesemision = (from ap in data
                                     where ap.estadoOfertaCodigo == "TEnRevision"
                                     where ap.codigoProyecto == proyecto
                                     select ap).ToList().Count;

                /*Valores Columnas Reportes*/

                celda = "T" + row;
                worksheet.Cells[celda].Value = aprobados;
                worksheet.Cells[celda].Style.Font.Color.SetColor(Color.White);

                celda = "U" + row;

                worksheet.Cells[celda].Value = cancelados;
                worksheet.Cells[celda].Style.Font.Color.SetColor(Color.White);

                celda = "V" + row;

                worksheet.Cells[celda].Value = presentados;
                worksheet.Cells[celda].Style.Font.Color.SetColor(Color.White);

                celda = "W" + row;

                worksheet.Cells[celda].Value = pendientesemision;
                worksheet.Cells[celda].Style.Font.Color.SetColor(Color.White);
                row++;

            }
            var chart = worksheet.Drawings.AddChart("chart", eChartType.ColumnStacked);
            chart.Legend.Position = eLegendPosition.Bottom;
            chart.SetPosition(1, 0, 5, 0);
            chart.SetSize(800, 400);


            var series = chart.Series.Add("T" + initial + ": T" + row, "S" + initial + ": S" + row);
            series.HeaderAddress = new ExcelAddress("'GRAFICOS'!T" + header);  //Columna Proyecto

            var series2 = chart.Series.Add("U" + initial + ":U" + row, "S" + initial + ":S" + row);
            series2.HeaderAddress = new ExcelAddress("'GRAFICOS'!U" + header); //Columna Aprobados


            var series3 = chart.Series.Add("V" + initial + ":V" + row, "S" + initial + ":S" + row);
            series3.HeaderAddress = new ExcelAddress("'GRAFICOS'!V" + header); // Columna Cancelados

            var series4 = chart.Series.Add("W" + initial + ":W" + row, "S" + initial + ":S" + row);
            series4.HeaderAddress = new ExcelAddress("'GRAFICOS'!W" + header); //Columna Pendientes de Aprobacion


            excel.Save();


            //**/

            h.InsertColumn(18, 2);
            h.Cells[1, 26, h.Dimension.End.Row, 27].Copy(h.Cells[1, 18, h.Dimension.End.Row, 19]);
            h.Column(18).Width = 25;
            h.Column(19).Width = 25;



            return excel;
        }

        public ExcelPackage ReporteDetalladosAdicionales(ReportDto r)
        {
            ExcelPackage excel = new ExcelPackage();
            string filename = System.Web.HttpContext.Current.Server.MapPath("~/Views/PlantillaWord/ReporteDetalladoProyecto.xlsx");
            if (File.Exists((string)filename))
            {
                FileInfo newFile = new FileInfo(filename);
                ExcelPackage pck = new ExcelPackage(newFile);
                excel.Workbook.Worksheets.Add("DETALLADO PROYECTO", pck.Workbook.Worksheets[2]);
            }
            ExcelWorksheet h = excel.Workbook.Worksheets[1];

            var proyectosquery = _proyectoRepository.GetAllIncluding(c => c.Contrato).Where(c => c.vigente).ToList();

            if (r.ContratoId > 0)
            {
                proyectosquery = proyectosquery.Where(c => c.contratoId == r.ContratoId).ToList();
            }
            if (r.ClienteId > 0)
            {
                proyectosquery = proyectosquery.Where(c => c.Contrato.ClienteId == r.ClienteId).ToList();

            }
            int count = 2;
            string cell = "";
            foreach (var p in proyectosquery)
            {
                var data = this.GetDatosDetallados(p.Id);

                foreach (var i in data)
                {
                    cell = "A" + count;
                    h.Cells[cell].Value = i.contrato;
                    cell = "B" + count;
                    h.Cells[cell].Value = i.clasificacion;
                    cell = "C" + count;
                    h.Cells[cell].Value = i.codigoProyecto;
                    cell = "D" + count;
                    h.Cells[cell].Value = i.codigoRequerimiento;
                    cell = "E" + count;
                    h.Cells[cell].Value = i.descripcionRequerimiento;
                    h.Cells[cell].Style.WrapText = true;
                    cell = "F" + count;
                    h.Cells[cell].Value = i.estadoRequerimiento;
                    cell = "G" + count;
                    h.Cells[cell].Value = i.monto;
                    h.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                    cell = "H" + count;
                    h.Cells[cell].Value = i.fechaSolicitud;
                    cell = "I" + count;
                    h.Cells[cell].Value = i.fechaPresentacion;
                    cell = "J" + count;
                    h.Cells[cell].Value = i.fechaSO;
                    cell = "K" + count;
                    h.Cells[cell].Value = i.numeroOferta;
                    count++;


                }

                var total = (from a in data select a.monto).ToList().Sum();
                decimal totalapropado = 0;
                decimal totalpresentado = 0;
                decimal totalporpresentar = 0;

                totalapropado = (from a in data where a.codigoEstadoRequerimiento == "TAprobado" select a.monto).ToList().Sum();
                totalpresentado = (from a in data where a.codigoEstadoRequerimiento == "TPresentado" select a.monto).ToList().Sum();
                totalporpresentar = (from a in data where a.codigoEstadoRequerimiento == "TEnRevision" select a.monto).ToList().Sum();


                cell = "A" + count;
                h.Cells[cell].Value = "Subtotales";

                cell = "C" + count;
                h.Cells[cell].Value = p.codigo;
                cell = "F" + count;
                h.Cells[cell].Value = "Total";
                cell = "G" + count;
                h.Cells[cell].Value = total;
                h.Cells[cell].Style.Numberformat.Format = "#,##0.00";

                string range = "A" + count + ":K" + count;
                h.Cells[range].Style.Fill.PatternType = ExcelFillStyle.Solid;
                h.Cells[range].Style.Fill.BackgroundColor.SetColor(Color.Black);
                h.Cells[range].Style.Font.Color.SetColor(Color.White);
                h.Cells[range].Style.Font.Bold = true;
                for (int i = 1; i <= 11; i++)
                {
                    h.Cells[count, i].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    h.Cells[count, i].Style.Border.Bottom.Color.SetColor(Color.White);
                    h.Cells[count, i].Style.Border.Top.Color.SetColor(Color.White);
                    h.Cells[count, i].Style.Border.Left.Color.SetColor(Color.White);
                    h.Cells[count, i].Style.Border.Right.Color.SetColor(Color.White);
                }

                count++;


                cell = "A" + count;
                h.Cells[cell].Value = "";
                cell = "C" + count;
                h.Cells[cell].Value = p.codigo;
                cell = "F" + count;
                h.Cells[cell].Value = "Aprobado";
                cell = "G" + count;
                h.Cells[cell].Value = totalapropado;
                h.Cells[cell].Style.Numberformat.Format = "#,##0.00";

                range = "A" + count + ":K" + count;
                h.Cells[range].Style.Fill.PatternType = ExcelFillStyle.Solid;
                h.Cells[range].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(128, 128, 128));
                h.Cells[range].Style.Font.Color.SetColor(Color.White);
                //h.Cells[range].Style.Font.Bold = true;
                for (int i = 1; i <= 11; i++)
                {
                    h.Cells[count, i].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    h.Cells[count, i].Style.Border.Bottom.Color.SetColor(Color.White);
                    h.Cells[count, i].Style.Border.Top.Color.SetColor(Color.White);
                    h.Cells[count, i].Style.Border.Left.Color.SetColor(Color.White);
                    h.Cells[count, i].Style.Border.Right.Color.SetColor(Color.White);
                }


                count++;


                cell = "A" + count;
                h.Cells[cell].Value = "";
                cell = "C" + count;
                h.Cells[cell].Value = p.codigo;
                cell = "F" + count;
                h.Cells[cell].Value = "Presentado";
                cell = "G" + count;
                h.Cells[cell].Value = totalpresentado;
                h.Cells[cell].Style.Numberformat.Format = "#,##0.00";

                range = "A" + count + ":K" + count;
                h.Cells[range].Style.Fill.PatternType = ExcelFillStyle.Solid;
                h.Cells[range].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(128, 128, 128));
                h.Cells[range].Style.Font.Color.SetColor(Color.White);
                //h.Cells[range].Style.Font.Bold = true;
                for (int i = 1; i <= 11; i++)
                {
                    h.Cells[count, i].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    h.Cells[count, i].Style.Border.Bottom.Color.SetColor(Color.White);
                    h.Cells[count, i].Style.Border.Top.Color.SetColor(Color.White);
                    h.Cells[count, i].Style.Border.Left.Color.SetColor(Color.White);
                    h.Cells[count, i].Style.Border.Right.Color.SetColor(Color.White);
                }

                count++;

                cell = "A" + count;
                h.Cells[cell].Value = "";
                cell = "C" + count;
                h.Cells[cell].Value = p.codigo;
                cell = "F" + count;
                h.Cells[cell].Value = "En Revisión";
                cell = "G" + count;
                h.Cells[cell].Value = totalporpresentar;
                h.Cells[cell].Style.Numberformat.Format = "#,##0.00";

                range = "A" + count + ":K" + count;
                h.Cells[range].Style.Fill.PatternType = ExcelFillStyle.Solid;
                h.Cells[range].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(128, 128, 128));
                h.Cells[range].Style.Font.Color.SetColor(Color.White);
                //h.Cells[range].Style.Font.Bold = true;
                for (int i = 1; i <= 11; i++)
                {
                    h.Cells[count, i].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    h.Cells[count, i].Style.Border.Bottom.Color.SetColor(Color.White);
                    h.Cells[count, i].Style.Border.Top.Color.SetColor(Color.White);
                    h.Cells[count, i].Style.Border.Left.Color.SetColor(Color.White);
                    h.Cells[count, i].Style.Border.Right.Color.SetColor(Color.White);
                }


                count++;




            }




            return excel;
        }

        public ExcelPackage SeguimientoComercial(ReportDto r)
        {
            ExcelPackage excel = new ExcelPackage();

            string filename = System.Web.HttpContext.Current.Server.MapPath("~/Views/PlantillaWord/SeguimientoComercial.xlsx");
            if (File.Exists((string)filename))
            {
                FileInfo newFile = new FileInfo(filename);
                ExcelPackage pck = new ExcelPackage(newFile);
                excel.Workbook.Worksheets.Add("Seguimiento Comercial", pck.Workbook.Worksheets[1]);
                excel.Workbook.Worksheets.Add("Reporte", pck.Workbook.Worksheets[2]);
            }
            ExcelWorksheet h = excel.Workbook.Worksheets[1];
            //h.View.ZoomScale = 40;



            ExcelWorksheet reporte = excel.Workbook.Worksheets[2];

            reporte.Cells["A1"].Value = $"SEGUIMIENTO COMERCIAL - ({DateTime.Now.ToShortDateString()})";


            var ofertaquery = Repository.GetAllIncluding(c => c.Contrato).Where(c => c.vigente).Where(o => o.es_final == 1).OrderBy(c => c.codigo).ToList();
            if (r.ContratoId > 0)
            {
                ofertaquery = ofertaquery.Where(c => c.ContratoId == r.ContratoId).ToList();
            }
            if (r.ClienteId > 0)
            {
                ofertaquery = ofertaquery.Where(c => c.Contrato.ClienteId == r.ClienteId).ToList();
            }

            int count = 2;
            int rCount = 7;
            foreach (var i in ofertaquery)
            {

                h.Row(count).Height = 60;
                decimal montoOsIngnieria = new decimal(0);
                decimal montoOsProcura = new decimal(0); ;
                decimal montoOsConstruccion = new decimal(0); ;
                decimal montoOsSubcontratos = new decimal(0); ;
                string referenciasPO = "";

                var requerimientos = _ofertacomercialpresupuestorepository.GetAllIncluding(c => c.Requerimiento.Proyecto, p => p.Presupuesto)
                    .Where(c => c.vigente)
                    .Where(c => c.OfertaComercialId == i.Id)
                    .ToList();

                // Monto OS Ingenieria
                var listaIngenieria = _detalleordenesService.GetAllIncluding(o => o.OrdenServicio).Where(o => o.OfertaComercialId == i.Id)
                    .Where(o => o.GrupoItemId == DetalleOrdenServicio.GrupoItems.Ingeniería)
                    //.FirstOrDefault(o => o.vigente);
                    .Where(o => o.OrdenServicio.Estado.codigo == "POAPROBADO")
                    .Where(o => o.vigente)
                    .ToList();
                foreach (var detalle in listaIngenieria)
                {
                    montoOsIngnieria += detalle.valor_os;
                }

                // Ganacia Ingenieria
                var listadoGanancias = _detalleGananciaRepository.GetAllIncluding(o => o.Ganancia, p => p.GrupoItem)
                    .Where(o => o.vigente)
                    .Where(o => o.GrupoItem.codigo == "INGENIERIA")
                    .Where(o => o.Ganancia.ContratoId == i.ContratoId)
                    .ToList();
                decimal gananciaIngenieria = new decimal(0);
                foreach (var ganancia in listadoGanancias)
                {
                    gananciaIngenieria += ganancia.valor;
                }


                // Monto OS Procura
                var listaProcura = _detalleordenesService.GetAllIncluding(o => o.OrdenServicio).Where(o => o.OfertaComercialId == i.Id)
                    .Where(o => o.GrupoItemId == DetalleOrdenServicio.GrupoItems.Suministros)
                    //.FirstOrDefault(o => o.vigente);
                    .Where(o => o.OrdenServicio.Estado.codigo == "POAPROBADO")
                    .Where(o => o.vigente)
                    .ToList();
                foreach (var detalle in listaProcura)
                {
                    montoOsProcura += detalle.valor_os;
                }
                // Ganacia Procura
                var listadoGananciasProcura = _detalleGananciaRepository.GetAllIncluding(o => o.Ganancia, p => p.GrupoItem)
                    .Where(o => o.vigente)
                    .Where(o => o.GrupoItem.codigo == "PROCURA")
                    .Where(o => o.Ganancia.ContratoId == i.ContratoId)
                    .ToList();
                decimal gananciaProcura = new decimal(0);
                foreach (var ganancia in listadoGananciasProcura)
                {
                    gananciaProcura += ganancia.valor;
                }

                // Monto OS Construccion
                var listaConstruccion = _detalleordenesService.GetAllIncluding(o => o.OrdenServicio).Where(o => o.OfertaComercialId == i.Id)
                    .Where(o => o.GrupoItemId == DetalleOrdenServicio.GrupoItems.Construcción)
                    //.FirstOrDefault(o => o.vigente);
                    .Where(o => o.OrdenServicio.Estado.codigo == "POAPROBADO")
                    .Where(o => o.vigente)
                    .ToList();
                foreach (var detalle in listaConstruccion)
                {
                    montoOsConstruccion += detalle.valor_os;
                }

                // Ganacia Construccion
                var listadoGananciasConstruccion = _detalleGananciaRepository.GetAllIncluding(o => o.Ganancia, p => p.GrupoItem)
                    .Where(o => o.vigente)
                    .Where(o => o.GrupoItem.codigo == "CONSTRUCCION")
                    .Where(o => o.Ganancia.ContratoId == i.ContratoId)
                    .ToList();
                decimal gananciaConstruccion = new decimal(0);
                foreach (var ganancia in listadoGananciasConstruccion)
                {
                    gananciaConstruccion += ganancia.valor;
                }

                // Monto OS Subcontratos
                var listaSubcontratos = _detalleordenesService.GetAllIncluding(o => o.OrdenServicio).Where(o => o.OfertaComercialId == i.Id)
                    .Where(o => o.GrupoItemId == DetalleOrdenServicio.GrupoItems.SubContratos)
                    //.FirstOrDefault(o => o.vigente);
                    .Where(o => o.OrdenServicio.Estado.codigo == "POAPROBADO")
                    .Where(o => o.vigente)
                    .ToList();
                foreach (var detalle in listaSubcontratos)
                {
                    montoOsSubcontratos += detalle.valor_os;
                }

                // Ganacia Subcontratos
                var listadoGananciasSubcontratos = _detalleGananciaRepository.GetAllIncluding(o => o.Ganancia, p => p.GrupoItem)
                    .Where(o => o.vigente)
                    .Where(o => o.GrupoItem.codigo == "SUBCONTRATOS")
                    .Where(o => o.Ganancia.ContratoId == i.ContratoId)
                    .ToList();
                decimal gananciaSubcontratos = new decimal(0);
                foreach (var ganancia in listadoGananciasSubcontratos)
                {
                    gananciaSubcontratos += ganancia.valor;
                }

                // Referencias PO
                var ordenesOs = _detalleordenesService.GetAllIncluding(o => o.OrdenServicio)
                    .Where(o => o.OfertaComercialId == i.Id)
                    .Where(o => o.vigente)
                    .Select(o => o.OrdenServicio)
                    .Distinct()
                    .ToList();

                foreach (var ordenes in ordenesOs)
                {
                    referenciasPO += ordenes.referencias_po;
                }

                var fechaUltimaSo = _detalleordenesService.GetAllIncluding(o => o.OrdenServicio)
                    .Where(o => o.OfertaComercialId == i.Id)
                    .Where(o => o.vigente)
                    .OrderByDescending(o => o.OrdenServicio.fecha_orden_servicio)
                    .FirstOrDefault();

                // Fecha primer envio
                string fechaPrimerEnvio = "";
                if (i.version == "B")
                {
                    fechaPrimerEnvio = i.fecha_primer_envio.HasValue
                        ? i.fecha_primer_envio.GetValueOrDefault().ToShortDateString()
                        : "";
                }
                else
                {
                    var ofertaVersionB = Repository.GetAll().Where(o => o.codigo == i.codigo)
                        .FirstOrDefault(o => o.version == "B");
                    fechaPrimerEnvio = ofertaVersionB != null && ofertaVersionB.fecha_primer_envio.HasValue
                        ? i.fecha_primer_envio.GetValueOrDefault().ToShortDateString()
                        : i.fecha_primer_envio.HasValue ? i.fecha_primer_envio.Value.ToShortDateString()
                            : i.fecha_oferta.HasValue ? i.fecha_oferta.Value.ToShortDateString() : "";
                }

                if (requerimientos.Count > 0)
                {


                    foreach (var req in requerimientos)
                    {

                        string cell = "A" + count;
                        h.Cells[cell].Value = i.codigo;
                        h.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        h.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;



                        // Hoja Reporte
                        cell = "A" + rCount;
                        reporte.Cells[cell].Value = i.codigo;
                        h.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        h.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;



                        cell = "B" + count;
                        h.Cells[cell].Value = i.version;
                        h.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        h.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                        // Hoja Reporte
                        cell = "B" + rCount;
                        reporte.Cells[cell].Value = i.version;
                        h.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        h.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                        cell = "J" + count;
                        h.Cells[cell].Value = req.Requerimiento.ultima_clase;
                        h.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        h.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                        var tipoTrabajo = _catalogoRepository.GetAll().Where(c => c.Id == i.tipo_Trabajo_Id).FirstOrDefault();
                        if (tipoTrabajo != null && tipoTrabajo.Id > 0)
                        {
                            cell = "D" + count;
                            h.Cells[cell].Value = tipoTrabajo.nombre;
                            h.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            h.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                        }
                        var Alcance = _catalogoRepository.GetAll().Where(c => c.Id == i.alcance).FirstOrDefault();
                        if (Alcance != null && Alcance.Id > 0)
                        {
                            cell = "E" + count;
                            h.Cells[cell].Value = Alcance.nombre;
                            h.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            h.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                            /*cell = "C" + rCount;
                            reporte.Cells[cell].Value = Alcance.nombre;*/
                        }
                        cell = "F" + count;
                        h.Cells[cell].Value = req.Requerimiento.Proyecto.codigo;
                        h.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        h.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                        // Reporte
                        cell = "D" + rCount;
                        reporte.Cells[cell].Value = req.Requerimiento.codigo;

                        cell = "G" + count;
                        h.Cells[cell].Value = i.codigo_shaya;
                        h.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        h.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                        cell = "H" + count;
                        h.Cells[cell].Value = i.descripcion;
                        h.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        h.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        //h.Cells[cell].Style.WrapText = true;

                        // Reporte
                        cell = "E" + rCount;
                        reporte.Cells[cell].Value = i.descripcion;
                        //h.Cells[cell].Style.WrapText = true;

                        var EstadoOferta = _catalogoRepository.GetAll().Where(c => c.Id == i.estado_oferta).FirstOrDefault();

                        if (EstadoOferta != null && EstadoOferta.Id > 0)
                        {
                            cell = "I" + count;
                            h.Cells[cell].Value = EstadoOferta.nombre;
                            h.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            h.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                            // Reporte
                            cell = "F" + rCount;
                            reporte.Cells[cell].Value = EstadoOferta.nombre;
                        }

                        // cell = "J" + count;
                        h.Cells[cell].Value = "";
                        h.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        h.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                        // Monto Ofertado Ingenieria
                        cell = "K" + count;
                        h.Cells[cell].Value = req.Presupuesto != null ? req.Presupuesto.monto_ingenieria : req.Requerimiento.monto_ingenieria;
                        h.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                        h.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        h.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                        // Monto Ofertado Procura
                        cell = "L" + count;
                        h.Cells[cell].Value = req.Presupuesto != null ? req.Presupuesto.monto_suministros : req.Requerimiento.monto_procura;
                        h.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                        h.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        h.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                        // Monto Ofertado Construccion
                        cell = "M" + count;
                        h.Cells[cell].Value = req.Presupuesto != null ? req.Presupuesto.monto_construccion : req.Requerimiento.monto_construccion;
                        h.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                        h.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        h.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;


                        // Monto Ofertado Subcontratos
                        cell = "N" + count;
                        h.Cells[cell].Value = req.Presupuesto != null ? req.Presupuesto.monto_subcontratos : req.Requerimiento.monto_subcontrato;
                        h.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                        h.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        h.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;


                        // Monto Oferta
                        cell = "O" + count;
                        h.Cells[cell].Value = i.monto_ofertado;//:(req.Requerimiento.monto_ingenieria + req.Requerimiento.monto_procura + req.Requerimiento.monto_construccion + req.Requerimiento.monto_subcontrato);
                        h.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                        h.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        h.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                        cell = "G" + rCount;
                        reporte.Cells[cell].Value = i.monto_ofertado;
                        h.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                        h.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        h.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;


                        // Monto OS Ingenieria                      
                        cell = "P" + count;
                        h.Cells[cell].Value = ((montoOsIngnieria * gananciaIngenieria) / 100) + montoOsIngnieria;
                        h.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                        h.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        h.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                        // Monto OS Procura
                        cell = "Q" + count;
                        h.Cells[cell].Value = ((montoOsProcura * gananciaProcura) / 100) + montoOsProcura;
                        h.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                        h.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        h.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                        // Monto OS Construccion
                        cell = "R" + count;
                        h.Cells[cell].Value = ((montoOsConstruccion * gananciaConstruccion) / 100) + montoOsConstruccion;
                        h.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                        h.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        h.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                        // Monto OS Subcontratos
                        cell = "S" + count;
                        h.Cells[cell].Value = ((montoOsSubcontratos * gananciaSubcontratos) / 100) + montoOsSubcontratos;
                        h.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                        h.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        h.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;


                        cell = "T" + count;
                        h.Cells[cell].Value = i.monto_so_aprobado;
                        h.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                        h.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        h.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                        cell = "H" + rCount;
                        reporte.Cells[cell].Value = i.monto_so_aprobado;
                        h.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                        h.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        h.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                        // Pendiente Ingenieria
                        cell = "U" + count;
                        h.Cells[cell].Formula = $"=K{count}-P{count}";
                        h.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                        h.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        h.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                        // Pendiente Procura
                        cell = "V" + count;
                        h.Cells[cell].Formula = $"=L{count}-Q{count}";
                        h.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                        h.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        h.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                        // Pendiente Construccion
                        cell = "W" + count;
                        h.Cells[cell].Formula = $"=M{count}-R{count}";
                        h.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                        h.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        h.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                        // Pendiente Subcontratos
                        cell = "X" + count;
                        h.Cells[cell].Formula = $"=N{count}-S{count}";
                        h.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                        h.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        h.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                        // Monto ofertado Pendiente de aprobacion
                        cell = "Y" + count;
                        h.Cells[cell].Value = i.monto_ofertado_pendiente_aprobacion;
                        h.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                        h.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        h.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                        // Fecha SR
                        cell = "Z" + count;
                        h.Cells[cell].Value = req.Requerimiento.fecha_recepcion.ToShortDateString();
                        h.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        h.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                        cell = "AA" + count;
                        h.Cells[cell].Value = fechaPrimerEnvio;
                        h.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        h.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                        cell = "I" + rCount;
                        reporte.Cells[cell].Value = i.fecha_primer_envio.HasValue ? i.fecha_primer_envio.Value.ToShortDateString() : "";
                        h.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        h.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                        cell = "J" + rCount;
                        reporte.Cells[cell].Value = i.fecha_primer_envio.HasValue ? i.fecha_primer_envio.Value.Year.ToString() : "";


                        cell = "AB" + count;
                        h.Cells[cell].Value = i.fecha_ultimo_envio.HasValue ? i.fecha_ultimo_envio.Value.ToShortDateString() : "";
                        h.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        h.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                        // Codigo SO
                        cell = "AC" + count;
                        h.Cells[cell].Value = referenciasPO;
                        h.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        h.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                        // Fecha ultima SO registrada
                        cell = "AD" + count;
                        h.Cells[cell].Value = fechaUltimaSo != null ? fechaUltimaSo.OrdenServicio.fecha_orden_servicio.ToShortDateString() : "";
                        h.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        h.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                        var transmittals = _transmitalRepository.GetAll().Where(c => c.OfertaComercialId == i.Id)
                                                                       .Where(c => c.OfertaComercialId.HasValue)
                                                                       .Where(c => c.vigente).ToList();
                        if (transmittals.Count > 0)
                        {
                            cell = "AE" + count;
                            h.Cells[cell].Value = String.Join(";", transmittals.Select(c => c.codigo_transmital).ToList());
                            h.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            h.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        }
                        var FormaContratacion = _catalogoRepository.GetAll().Where(c => c.Id == i.forma_contratacion).FirstOrDefault();
                        if (FormaContratacion != null && FormaContratacion.Id > 0)
                        {

                            cell = "AF" + count;
                            h.Cells[cell].Value = FormaContratacion.nombre;
                            h.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            h.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        }
                        var EstadoEjecucion = _catalogoRepository.GetAll().Where(c => c.Id == i.estatus_de_Ejecucion).FirstOrDefault();
                        if (EstadoEjecucion != null && EstadoEjecucion.Id > 0)
                        {
                            cell = "AG" + count;
                            h.Cells[cell].Value = EstadoEjecucion.nombre;
                            h.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            h.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        }

                        cell = "AH" + count;
                        h.Cells[cell].Value = req.Requerimiento.tipo_requerimiento.ToString();
                        h.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        h.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;



                        cell = "AJ" + count;
                        h.Cells[cell].Value = req.Requerimiento.codigo;
                        h.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        h.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                        cell = "AI" + count;
                        h.Cells[cell].Value = i.comentarios;
                        h.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        h.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        //h.Cells[cell].Style.WrapText = true;44

                        cell = "AK" + count;
                        h.Cells[cell].Value = i.monto_ofertado_migracion_actual;
                        h.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                        h.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        h.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;


                        cell = "AL" + count;
                        h.Cells[cell].Value = (i.monto_ofertado + i.monto_ofertado_migracion_actual);
                        h.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                        h.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        h.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                        cell = "AM" + count;
                        h.Cells[cell].Value = (i.monto_so_aprobado_migracion_actual + i.monto_so_aprobado_migracion_anterior);
                        h.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                        h.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        h.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;


                        cell = "AN" + count;
                        h.Cells[cell].Value = -100;
                        h.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                        h.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        h.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                        cell = "AO" + count;
                        h.Cells[cell].Value = (i.monto_so_aprobado + i.monto_so_aprobado_migracion_actual + i.monto_so_aprobado_migracion_anterior);
                        h.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                        h.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        h.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;


                        cell = "K" + rCount;
                        reporte.Cells[cell].Value = i.monto_ofertado_migracion_actual;
                        reporte.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                        reporte.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        reporte.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;


                        cell = "L" + rCount;
                        reporte.Cells[cell].Value = i.monto_so_aprobado_migracion_actual;
                        reporte.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                        reporte.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        reporte.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell = "M" + rCount;
                        reporte.Cells[cell].Value = i.monto_so_aprobado_migracion_anterior;
                        reporte.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                        reporte.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        reporte.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell = "N" + rCount;
                        reporte.Cells[cell].Value = (i.monto_ofertado + i.monto_ofertado_migracion_actual);
                        reporte.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                        reporte.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        reporte.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell = "O" + rCount;
                        reporte.Cells[cell].Value = (i.monto_so_aprobado + i.monto_so_aprobado_migracion_actual + i.monto_so_aprobado_migracion_anterior);
                        reporte.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                        reporte.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        reporte.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        count++;
                        rCount++;
                        h.Row(count).Height = 60;
                    }

                }
                else
                {
                    string cell = "A" + count;
                    h.Cells[cell].Value = i.codigo;
                    h.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    h.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                    // Hoja Reporte
                    cell = "A" + rCount;
                    reporte.Cells[cell].Value = i.codigo;

                    cell = "B" + count;
                    h.Cells[cell].Value = i.version;
                    h.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    h.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                    // Hoja Reporte
                    cell = "B" + rCount;
                    reporte.Cells[cell].Value = i.version;

                    var tipoTrabajo = _catalogoRepository.GetAll().Where(c => c.Id == i.tipo_Trabajo_Id).FirstOrDefault();
                    if (tipoTrabajo != null && tipoTrabajo.Id > 0)
                    {
                        cell = "D" + count;
                        h.Cells[cell].Value = tipoTrabajo.nombre;
                        h.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        h.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    }
                    var Alcance = _catalogoRepository.GetAll().Where(c => c.Id == i.alcance).FirstOrDefault();
                    if (Alcance != null && Alcance.Id > 0)
                    {
                        cell = "E" + count;
                        h.Cells[cell].Value = Alcance.nombre;
                        h.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        h.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                        /*cell = "C" + rCount;
                        reporte.Cells[cell].Value = Alcance.nombre;*/
                    }
                    cell = "F" + count;
                    h.Cells[cell].Value = "";
                    h.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    h.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                    // Reporte
                    cell = "D" + rCount;
                    reporte.Cells[cell].Value = "";

                    cell = "G" + count;
                    h.Cells[cell].Value = i.codigo_shaya;
                    h.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    h.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                    cell = "H" + count;
                    h.Cells[cell].Value = i.descripcion;

                    h.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    h.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                    // Reporte
                    cell = "E" + rCount;
                    reporte.Cells[cell].Value = i.descripcion;

                    var EstadoOferta = _catalogoRepository.GetAll().Where(c => c.Id == i.estado_oferta).FirstOrDefault();

                    if (EstadoOferta != null && EstadoOferta.Id > 0)
                    {
                        cell = "I" + count;
                        h.Cells[cell].Value = EstadoOferta.nombre;
                        h.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        h.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                        // Reporte
                        cell = "F" + rCount;
                        reporte.Cells[cell].Value = EstadoOferta.nombre;
                    }

                    cell = "J" + count;
                    h.Cells[cell].Value = "";

                    // Monto Ofertado Ingenieria
                    cell = "K" + count;
                    h.Cells[cell].Value = 0;
                    h.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                    h.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    h.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                    // Monto Ofertado Procura
                    cell = "L" + count;
                    h.Cells[cell].Value = 0;
                    h.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                    h.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    h.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                    // Monto Ofertado Construccion
                    cell = "M" + count;
                    h.Cells[cell].Value = 0;
                    h.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                    h.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    h.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                    // Monto Ofertado Subcontratos
                    cell = "N" + count;
                    h.Cells[cell].Value = 0;
                    h.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                    h.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    h.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                    // Monto Oferta
                    cell = "O" + count;
                    h.Cells[cell].Value = i.monto_ofertado;
                    h.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                    h.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    h.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                    cell = "G" + rCount;
                    reporte.Cells[cell].Value = i.monto_ofertado;
                    h.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                    h.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    h.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                    // Monto OS Ingenieria                      
                    cell = "P" + count;
                    h.Cells[cell].Value = ((montoOsIngnieria * gananciaIngenieria) / 100) + montoOsIngnieria;
                    h.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                    h.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    h.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                    // Monto OS Procura
                    cell = "Q" + count;
                    h.Cells[cell].Value = ((montoOsProcura * gananciaProcura) / 100) + montoOsProcura;
                    h.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                    h.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    h.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                    // Monto OS Construccion
                    cell = "R" + count;
                    h.Cells[cell].Value = ((montoOsConstruccion * gananciaConstruccion) / 100) + montoOsConstruccion;
                    h.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                    h.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    h.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                    // Monto OS Subcontratos
                    cell = "S" + count;
                    h.Cells[cell].Value = ((montoOsSubcontratos * gananciaSubcontratos) / 100) + montoOsSubcontratos;
                    h.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                    h.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    h.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                    cell = "T" + count;
                    h.Cells[cell].Value = i.monto_so_aprobado;
                    h.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                    h.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    h.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                    cell = "H" + rCount;
                    reporte.Cells[cell].Value = i.monto_so_aprobado;
                    reporte.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                    reporte.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    reporte.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                    // Pendiente Ingenieria
                    cell = "U" + count;
                    h.Cells[cell].Formula = $"=K{count}-P{count}";
                    h.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                    h.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    h.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                    // Pendiente Procura
                    cell = "V" + count;
                    h.Cells[cell].Formula = $"=L{count}-Q{count}";
                    h.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                    h.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    h.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                    // Pendiente Construccion
                    cell = "W" + count;
                    h.Cells[cell].Formula = $"=M{count}-R{count}";
                    h.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                    h.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    h.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                    // Pendiente Subcontratos
                    cell = "X" + count;
                    h.Cells[cell].Formula = $"=N{count}-S{count}";
                    h.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                    h.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    h.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                    // Monto ofertado Pendiente de aprobacion
                    cell = "Y" + count;
                    h.Cells[cell].Value = i.monto_ofertado_pendiente_aprobacion;
                    h.Cells[cell].Style.Numberformat.Format = "#,##0.00"; ;
                    h.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    h.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                    // Fecha SR
                    cell = "Z" + count;
                    h.Cells[cell].Value = i.fecha_oferta.HasValue ? i.fecha_oferta.Value.ToShortDateString() : "";
                    h.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    h.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                    cell = "AA" + count;
                    h.Cells[cell].Value = fechaPrimerEnvio;
                    h.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    h.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                    cell = "I" + rCount;
                    reporte.Cells[cell].Value = i.fecha_primer_envio.HasValue
                        ? i.fecha_primer_envio.Value.ToShortDateString()
                        : "";
                    cell = "J" + rCount;
                    reporte.Cells[cell].Value = i.fecha_primer_envio.HasValue
                        ? i.fecha_primer_envio.Value.Year.ToString() : "";
                    cell = "AB" + count;
                    h.Cells[cell].Value = i.fecha_ultimo_envio.HasValue ? i.fecha_ultimo_envio.Value.ToShortDateString() : "";
                    h.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    h.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                    // Codigo SO
                    cell = "AC" + count;
                    h.Cells[cell].Value = referenciasPO;
                    h.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    h.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                    // Fecha ultima SO registrada
                    cell = "AD" + count;
                    h.Cells[cell].Value = fechaUltimaSo != null ? fechaUltimaSo.OrdenServicio.fecha_orden_servicio.ToShortDateString() : "";
                    h.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    h.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                    var transmittals = _transmitalRepository.GetAll().Where(c => c.OfertaComercialId == i.Id)
                                                                   .Where(c => c.OfertaComercialId.HasValue)
                                                                   .Where(c => c.vigente).ToList();
                    if (transmittals.Count > 0)
                    {
                        cell = "AE" + count;
                        h.Cells[cell].Value = String.Join(";", transmittals.Select(c => c.codigo_transmital).ToList());
                        h.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        h.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    }
                    var FormaContratacion = _catalogoRepository.GetAll().Where(c => c.Id == i.forma_contratacion).FirstOrDefault();
                    if (FormaContratacion != null && FormaContratacion.Id > 0)
                    {

                        cell = "AF" + count;
                        h.Cells[cell].Value = FormaContratacion.nombre;
                        h.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        h.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    }
                    var EstadoEjecucion = _catalogoRepository.GetAll().Where(c => c.Id == i.estatus_de_Ejecucion).FirstOrDefault();
                    if (EstadoEjecucion != null && EstadoEjecucion.Id > 0)
                    {
                        cell = "AG" + count;
                        h.Cells[cell].Value = EstadoEjecucion.nombre;
                        h.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        h.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    }

                    cell = "AH" + count;
                    h.Cells[cell].Value = "";


                    cell = "AI" + count;
                    h.Cells[cell].Value = i.comentarios;

                    h.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    h.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                    cell = "AK" + count;
                    h.Cells[cell].Value = i.monto_ofertado_migracion_actual;
                    h.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                    h.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    h.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;


                    cell = "AL" + count;
                    h.Cells[cell].Value = (i.monto_ofertado + i.monto_ofertado_migracion_actual);
                    h.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                    h.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    h.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                    cell = "AM" + count;
                    h.Cells[cell].Value = (i.monto_so_aprobado_migracion_actual + i.monto_so_aprobado_migracion_anterior);
                    h.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                    h.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    h.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;


                    cell = "AN" + count;
                    h.Cells[cell].Value = -100;
                    h.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                    h.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    h.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                    cell = "AO" + count;
                    h.Cells[cell].Value = (i.monto_so_aprobado + i.monto_so_aprobado_migracion_actual + i.monto_so_aprobado_migracion_anterior);
                    h.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                    h.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    h.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;



                    cell = "K" + rCount;
                    reporte.Cells[cell].Value = i.monto_ofertado_migracion_actual;
                    reporte.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                    reporte.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    reporte.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;


                    cell = "L" + rCount;
                    reporte.Cells[cell].Value = i.monto_so_aprobado_migracion_actual;
                    reporte.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                    reporte.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    reporte.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell = "M" + rCount;
                    reporte.Cells[cell].Value = i.monto_so_aprobado_migracion_anterior;
                    reporte.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                    reporte.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    reporte.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell = "N" + rCount;
                    reporte.Cells[cell].Value = (i.monto_ofertado + i.monto_ofertado_migracion_actual);
                    reporte.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                    reporte.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    reporte.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell = "O" + rCount;
                    reporte.Cells[cell].Value = (i.monto_so_aprobado + i.monto_so_aprobado_migracion_actual + i.monto_so_aprobado_migracion_anterior);
                    reporte.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                    reporte.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    reporte.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                    count++;
                    rCount++;

                    h.Row(count).Height = 60;
                }
            }

            h.DeleteColumn(3); //remove Colum Clase ya esta incluida en Clase AACE

            // Datos grafica total
            int countDatosChart = 1;
            var estadosOfertas = ofertaquery.Select(o => o.estado_oferta).Distinct();
            foreach (var estado in estadosOfertas)
            {
                string cell = "V" + countDatosChart;
                reporte.Cells[cell].Value =
                    _catalogoRepository.GetAll().Where(c => c.Id == estado).FirstOrDefault().nombre;
                cell = "U" + countDatosChart;
                reporte.Cells[cell].Value = ofertaquery.Where(o => o.estado_oferta == estado).Count();

                countDatosChart++;
            }

            // Datos grafica del anio actual
            int countDatosAñoActualChart = 1;
            var inicioAnio = new DateTime(DateTime.Now.Year, 1, 1);
            var finAnio = new DateTime(DateTime.Now.Year, 12, 31);
            var estadosOfertasAñoActual = ofertaquery
                .Where(o => o.fecha_primer_envio >= inicioAnio)
                .Where(o => o.fecha_primer_envio <= finAnio)
                .Select(o => o.estado_oferta).Distinct();
            foreach (var estado in estadosOfertasAñoActual)
            {
                string cell = "Q" + countDatosAñoActualChart;
                reporte.Cells[cell].Value =
                    _catalogoRepository.GetAll().Where(c => c.Id == estado).FirstOrDefault().nombre;
                cell = "P" + countDatosAñoActualChart;
                reporte.Cells[cell].Value = ofertaquery
                    .Where(o => o.fecha_primer_envio >= inicioAnio)
                    .Where(o => o.fecha_primer_envio <= finAnio)
                    .Where(o => o.estado_oferta == estado).Count();

                countDatosAñoActualChart++;
            }

            // Grafica total
            ExcelPieChart pieChart = reporte.Drawings.AddChart("pieChart", eChartType.Pie3D) as ExcelPieChart;

            //set the title
            pieChart.Title.Text = "OFERTAS COMERCIALES 2016 - 2020" +
                                  "";

            //select the ranges for the pie. First the values, then the header range
            pieChart.Series.Add(ExcelRange.GetAddress(1, 21, countDatosChart - 1, 21), ExcelRange.GetAddress(1, 22, countDatosChart - 1, 22));

            //position of the legend
            pieChart.Legend.Position = eLegendPosition.Bottom;

            //show the percentages in the pie
            pieChart.DataLabel.ShowPercent = true;

            //size of the chart
            pieChart.SetSize(400, 300);

            //add the chart at cell C5
            pieChart.SetPosition(1, 0, 0, 0);

            // Grafica Anio Actual
            ExcelPieChart pieChartAnio =
                reporte.Drawings.AddChart("pieChartAnioActual", eChartType.Pie3D) as ExcelPieChart;
            pieChartAnio.Title.Text = $"OFERTAS COMERCIALES - {DateTime.Now.Year}";
            pieChartAnio.Series.Add(ExcelRange.GetAddress(1, 16, countDatosAñoActualChart - 1, 16),
                ExcelRange.GetAddress(1, 17, countDatosAñoActualChart - 1, 17));
            pieChartAnio.Legend.Position = eLegendPosition.Bottom;
            pieChartAnio.DataLabel.ShowPercent = true;
            pieChartAnio.SetSize(400, 300);
            pieChartAnio.SetPosition(1, 0, 5, 0);

            //reporte.Column(13).Style.Font.Color.SetColor(Color.White);
            //reporte.Column(14).Style.Font.Color.SetColor(Color.White);
            //reporte.Column(15).Style.Font.Color.SetColor(Color.White);
            reporte.Column(16).Style.Font.Color.SetColor(Color.White);
            reporte.Column(17).Style.Font.Color.SetColor(Color.White);
            reporte.Column(18).Style.Font.Color.SetColor(Color.White);
            reporte.Column(19).Style.Font.Color.SetColor(Color.White);
            reporte.Column(20).Style.Font.Color.SetColor(Color.White);
            reporte.Column(21).Style.Font.Color.SetColor(Color.White);

            h.InsertColumn(4, 1, 2);
            h.Column(4).Width = 20;
            h.Cells[1, 36, h.Dimension.End.Row, 36].Copy(h.Cells[1, 4, h.Dimension.End.Row, 4]);
            h.DeleteColumn(36);




            /**/

            h.InsertColumn(16, 2);

            h.Cells[1, 38, h.Dimension.End.Row, 39].Copy(h.Cells[1, 16, h.Dimension.End.Row, 17]);



            h.InsertColumn(23, 3);
            h.Cells[1, 43, h.Dimension.End.Row, 45].Copy(h.Cells[1, 23, h.Dimension.End.Row, 25]);

            for (int i = 1; i <= 5; i++)
            {
                h.DeleteColumn(41);
            }

            h.Column(16).Width = 30;
            h.Column(17).Width = 30;
            h.Column(23).Width = 30;
            h.Column(24).Width = 30;
            h.Column(25).Width = 30;
            for (int i = 1; i <= 3; i++)
            {
                h.DeleteColumn(23); //delete column actual,  i.monto_so_migrado_anterior  ,total
            }
            for (int i = 1; i <= 2; i++)
            {
                h.DeleteColumn(15); //delete ccolum monto oferta y monto oferta migrado
            }
            
            for (int i = 11; i <= 15; i++)
            {
                reporte.Column(i).Hidden=true;
            }
            
            return excel;
        }

        public List<DatosAdicionales> GetDatosAdicionales(ReportDto r)
        {
            List<DatosAdicionales> list = new List<DatosAdicionales>();


            //List todos los requerimientos adicionales
            var query = _requerimientoRepository.GetAllIncluding(c => c.Proyecto.Contrato, c => c.EstadoOferta)
                .Where(c => c.vigente)
                .Where(c => c.tipo_requerimiento == TipoRequerimiento.Adicional)
                .ToList();


            if (r.ContratoId > 0)
            {
                query = query.Where(c => c.Proyecto.contratoId == r.ContratoId).ToList();
            }
            if (r.ClienteId > 0)
            {
                query = query.Where(c => c.Proyecto.Contrato.ClienteId == r.ClienteId).ToList();

            }

            //dependencias
            //presupuestos

            var presupuestosquery = _presupuestorepository.GetAll().Where(c => c.vigente)
                                                                 .Where(c => c.es_final)
                                                                 // .Where(c => c.estado_emision == Presupuesto.EstadoAprobacion.Aprobado||c.estado_emision==Presupuesto.EstadoEmision.)
                                                                 .ToList();
            //ofertas
            var ofertasquery = _ofertacomercialpresupuestorepository.GetAllIncluding(c => c.OfertaComercial)
                                                                  .Where(c => c.vigente)
                                                                  .ToList();

            var data = (from l in query
                        select new DatosAdicionales()
                        {
                            requerimientoId = l.Id,
                            contrato = l.Proyecto.Contrato.Codigo,
                            codigoProyecto = l.Proyecto.codigo,
                            nombreProyecto = l.Proyecto.nombre_proyecto,
                            descripcionProyecto = l.Proyecto.descripcion_proyecto,
                            codigoRequerimiento = l.codigo,
                            descripcionRequerimiento = l.descripcion,
                            estadoRequerimiento = l.EstadoOferta.nombre,
                            estadoRequerimientoCodigo = l.EstadoOferta.codigo,
                            requiereCronograma = l.requiere_cronograma ? "SI" : "NO",
                            fechaRegistroRequerimiento = l.fecha_recepcion.ToShortDateString(),
                            monto = l.monto_total,
                            montoIngenieria = l.monto_ingenieria,
                            montoConstruccion = l.monto_construccion,
                            montoSuministros = l.monto_procura,
                            montoSubcontratos = l.monto_subcontrato,
                            fechaRecepcionSolicitud = l.fecha_recepcion.ToShortDateString(),
                            solicitante = l.solicitante,

                        }).ToList();

            foreach (var l in data)
            {
                var presupuesto = (from p in presupuestosquery
                                   where p.RequerimientoId == l.requerimientoId
                                   select p).FirstOrDefault();
                if (presupuesto != null && presupuesto.Id > 0)
                {
                    l.fechaCargaPresupuesto = presupuesto.fecha_registro.HasValue ? presupuesto.fecha_registro.Value.ToShortDateString() : "";
                    l.montoConstruccionPresupuesto = presupuesto.monto_construccion;
                    l.montoIngenieriaPresupuesto = presupuesto.monto_ingenieria;
                    l.montoSubcontratosPresupuesto = presupuesto.monto_subcontratos;
                    l.montoSuministrosPresupuesto = presupuesto.monto_suministros;
                    l.montoTotalPresupuesto = presupuesto.monto_total;
                }

                var oferta = (from o in ofertasquery
                              where o.RequerimientoId == l.requerimientoId
                              orderby o.OfertaComercial.version descending
                              select o.OfertaComercial).FirstOrDefault();
                if (oferta != null && oferta.Id > 0)
                {
                    var nombreEstado = _catalogoRepository.GetAll().Where(c => c.Id == oferta.estado_oferta)
                                                                  .FirstOrDefault();
                    var centroCostosEstado = _catalogoRepository.GetAll().Where(c => c.Id == oferta.centro_de_Costos_Id)
                                                                  .FirstOrDefault();
                    if (nombreEstado != null && nombreEstado.Id > 0)
                    {
                        l.estadoOferta = nombreEstado.nombre;
                        l.estadoOfertaCodigo = nombreEstado.codigo;
                    }
                    l.ofertaPresentada = oferta.codigo;
                    l.montoofertado = oferta.monto_ofertado;
                    l.fechaEmisionOferta = oferta.fecha_primer_envio.HasValue ? oferta.fecha_primer_envio.Value.ToShortDateString() : "";
                    l.comentarios = oferta.comentarios;
                    l.fechaOferta = oferta.fecha_oferta.HasValue ? oferta.fecha_oferta.Value.ToShortDateString() : "";
                    if (centroCostosEstado != null && centroCostosEstado.Id > 0)
                    {
                        l.clasificacion = centroCostosEstado.nombre;
                    }
                    l.versionOferta = oferta.version;
                    l.fechaUltimoEnvio = oferta.fecha_ultimo_envio.HasValue ? oferta.fecha_ultimo_envio.Value.ToShortDateString() : "";

                    /*Nuevas Columnas Migracion*/
                    l.monto_ofertado_migracion_actual = oferta.monto_ofertado_migracion_actual;
                    l.monto_so_aprobado_migracion_actual = oferta.monto_ofertado_migracion_actual;
                    l.monto_so_aprobado_migracion_anterior = oferta.monto_so_aprobado_migracion_anterior;

                }
                else
                {
                    l.estadoOferta = l.estadoRequerimiento;
                    l.estadoOfertaCodigo = l.estadoRequerimientoCodigo;
                }


            }




            return data;

        }

        public async Task<string> SendMailAdministracionContratosAsync()
        {
            try

            {

                MailMessage mail = new MailMessage();

                SmtpClient SmtpServer = new SmtpClient("smtp.teic.techint.net");

                SmtpServer.Port = 25;

                SmtpServer.EnableSsl = false;

                mail.From = new MailAddress("adm_contractual.auca@cpp.com.ec");

                mail.To.Add("efrain.saransig@atikasoft.com");
                mail.To.Add("saransigefrain@gmail.com");
                mail.Subject = "Test Mail";

                mail.Body = "This is for testing SMTP mail from smtp.teic.techint.net, port 25";



                SmtpServer.Port = 25;

                SmtpServer.EnableSsl = false;



                SmtpServer.Send(mail);

                System.Console.WriteLine("Se envio el mail");
                var s = "Send mail Contractual ok";
                return s;


            }

            catch (Exception ex)

            {
                ElmahExtension.LogToElmah(new Exception("Send Mail administracion" + ex.Message));
                ElmahExtension.LogToElmah(ex);
                System.Console.WriteLine(ex.ToString());
                return ex.Message;
            }




        }

        public List<DatosDetalladosProyectos> GetDatosDetallados(int ProyectoId)
        {

            //List todos los requerimientos 
            var query = _requerimientoRepository.GetAllIncluding(c => c.Proyecto.Contrato, c => c.EstadoOferta).Where(c => c.vigente)
                                                                                       .Where(c => c.ProyectoId == ProyectoId)
                                                                                       .ToList();



            var presupuestosquery = _presupuestorepository.GetAll().Where(c => c.vigente)
                                                                 .Where(c => c.es_final)
                                                                 .Where(c => c.estado_emision == Presupuesto.EstadoEmision.Emitido)
                                                                 .ToList();
            //ofertas
            var ofertasquery = _ofertacomercialpresupuestorepository.GetAllIncluding(c => c.OfertaComercial)
                                                                  .Where(c => c.vigente)
                                                                  .ToList();



            var data = (from l in query
                        select new DatosDetalladosProyectos()
                        {
                            proyectoId = l.ProyectoId,
                            requerimientoId = l.Id,
                            codigoProyecto = l.Proyecto.codigo,
                            nombreProyecto = l.Proyecto.nombre_proyecto,
                            descripcionProyecto = l.Proyecto.descripcion_proyecto,
                            codigoRequerimiento = l.codigo,
                            descripcionRequerimiento = l.descripcion,
                            estadoRequerimiento = l.EstadoOferta.nombre,
                            fechaSolicitud = l.fecha_recepcion.ToShortDateString(),
                            codigoEstadoRequerimiento = l.EstadoOferta.codigo,
                            contratoId = l.Proyecto.contratoId,
                            contrato = l.Proyecto.Contrato.Codigo,
                            monto = l.monto_total
                        }).ToList();


            foreach (var l in data)
            {

                var oferta = (from o in ofertasquery
                              where o.RequerimientoId == l.requerimientoId
                              orderby o.OfertaComercial.version descending
                              select o.OfertaComercial).FirstOrDefault();
                if (oferta != null && oferta.Id > 0)
                {
                    l.fechaPresentacion = oferta.fecha_oferta.HasValue ? oferta.fecha_oferta.Value.ToShortDateString() : "";
                    l.numeroOferta = oferta.codigo;

                    var nombreEstado = _catalogoRepository.GetAll().Where(c => c.Id == oferta.centro_de_Costos_Id)
                                                                  .FirstOrDefault();
                    if (nombreEstado != null && nombreEstado.Id > 0)
                    {
                        l.clasificacion = nombreEstado.nombre;
                    }

                    var so = _ordenesService.GetAll().Where(c => c.EstadoId == oferta.Id).Where(c => c.vigente).FirstOrDefault();
                    if (so != null && so.Id > 0)
                    {
                        l.fechaSO = so.fecha_orden_servicio.ToShortDateString();
                    }
                }


            }
            return data;
        }

        public List<DatosSeguimiento> GetDatosSeguimiento(int OfertaComercialId)
        {
            return new List<DatosSeguimiento>();
        }

        public MontosItem ObtenerMontosRequerimientosOfertComercial(int OfertaComercialId)
        {
            MontosItem result = new MontosItem();
            var requerimientosOferta = _ofertacomercialpresupuestorepository
                                                        .GetAllIncluding(c => c.Requerimiento, c => c.Presupuesto)
                                                        .Where(c => c.OfertaComercialId == OfertaComercialId)
                                                        .Where(c => c.vigente)
                                                        .ToList();
            if (requerimientosOferta.Count > 0)
            {
                foreach (var req in requerimientosOferta)
                {
                    var presupuesto = _presupuestorepository.GetAll().Where(c => c.vigente)
                                                                   .Where(c => c.es_final)
                                                                   .Where(c => c.estado_aprobacion == Presupuesto.EstadoAprobacion.Aprobado)
                                                                   .Where(c => c.RequerimientoId == req.RequerimientoId)
                                                                   .FirstOrDefault();
                    if (presupuesto != null && presupuesto.Id > 0)
                    {
                        /* Actualizado Tabla Presupuestos*/
                        req.PresupuestoId = presupuesto.Id;
                        _ofertacomercialpresupuestorepository.Update(req);
                        result.success = true;

                    }

                }
                result.monto_ingenieria = (from r in requerimientosOferta select r.Requerimiento.monto_ingenieria).ToList().Sum();
                result.monto_contruccion = (from r in requerimientosOferta select r.Requerimiento.monto_construccion).ToList().Sum();
                result.monto_suministros = (from r in requerimientosOferta select r.Requerimiento.monto_procura).ToList().Sum();
                result.monto_subcontratos = (from r in requerimientosOferta select r.Requerimiento.monto_subcontrato).ToList().Sum();
                result.monto_total = result.monto_ingenieria + result.monto_contruccion + result.monto_suministros + result.monto_subcontratos;
            }

            return result;
        }

        public bool ActualizarMontoOfertaComercial(int Id, decimal monto_ofertado, decimal monto_total_os)
        {
            var o = Repository.Get(Id);
            if (monto_ofertado != -1)
            { // Debe Mantener el registro de la cabecera
                o.monto_ofertado = monto_ofertado;

                //CUANDO TIENE PRESPUESTO MIGRADO PASA A CERO 0
                o.monto_ofertado_migracion_actual = 0;


            }
            if (!o.monto_editado)
            {

                // POR EL ACTUALIZACION DE COMPORTAMIENTOS DE POS NO VA

                /*
                if (monto_total_os > 0) //Solo Si tiene valor Ingresado en POS
                {
                    o.monto_so_aprobado = monto_total_os;

                    o.monto_ofertado_pendiente_aprobacion = monto_ofertado - monto_total_os;
                }
                else
                {
                    o.monto_ofertado_pendiente_aprobacion = o.monto_ofertado - o.monto_so_aprobado;
                }*/
            }

            try
            {
                Repository.Update(o);
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public string hrefoutlook(int id, string to = "", string cc = "", string subject = "")
        {
            var o = Repository.Get(id);
            var contrato = _contratoRepository.Get(o.ContratoId);

            var estadopresentado = _catalogoRepository.GetAll().Where(c => c.codigo == CatalogosCodigos.OFERTA_ESTADO_PRESENTADO).FirstOrDefault();
            if (estadopresentado != null && estadopresentado.Id > 0)
            {

                o.estado_oferta = estadopresentado.Id;
                o.fecha_ultimo_envio = DateTime.Now;

                var resultado = Repository.Update(o);

                var requerimientos = _ofertacomercialpresupuestorepository.GetAllIncluding(c => c.Requerimiento).Where(c => c.OfertaComercialId == o.Id)
                    .Where(c => c.vigente).ToList();

                if (requerimientos.Count > 0)
                {
                    foreach (var item in requerimientos)
                    {

                        item.fecha_asignacion = DateTime.Now;
                        _ofertacomercialpresupuestorepository.Update(item);
                        var requerimiento = _requerimientoRepository.GetAll().Where(c => c.Id == item.RequerimientoId).FirstOrDefault();
                        if (requerimiento != null && requerimiento.Id > 0)
                        {
                            requerimiento.EstadoOfertaId = estadopresentado.Id;
                            _requerimientoRepository.Update(requerimiento);
                        }
                    }
                }



            }






            string codetransmittal = "";
            var transmittal = _transmitalRepository.GetAll().Where(c => c.vigente).Where(c => c.OfertaComercialId == id).FirstOrDefault();


            var requerimientoligado = _ofertacomercialpresupuestorepository.GetAllIncluding(c => c.Requerimiento.Proyecto).Where(c => c.OfertaComercialId == o.Id).Where(c => c.vigente).FirstOrDefault();
            string asunto = "A-";
            string descripcion2 = "";

            if (requerimientoligado != null)
            {
                if (contrato.Formato == FormatoContrato.Contrato_2016)
                {
                    asunto = asunto + "2016-Service Proposal (Servicios): ";
                }
                else
                {
                    asunto = asunto + "2019-Service Proposal (Servicios): ";
                }

                asunto = asunto + "Propuesta de Trabajos Adicionales " + requerimientoligado.Requerimiento.Proyecto.codigo + " " + requerimientoligado.Requerimiento.descripcion + " " + "  (" + o.codigo + " - Rev. " + o.version + ")";

                descripcion2 = "Adicionales" + '"' + requerimientoligado.Requerimiento.descripcion + '"' + " (" + o.codigo + " - Rev. " + o.version + ")";
            }
            else
            {
                asunto = o.codigo + " - Rev. " + o.version;
            }

            if (transmittal != null && transmittal.Id > 0)
            {
                codetransmittal = transmittal.codigo_transmital;
                descripcion2 = descripcion2 + " [ Transmittal " + transmittal.codigo_transmital + " ]";
                string dirigido = "";
            }

            List<string> concopia = new List<string>();
            List<string> para = new List<string>();
            var correos_lista = _correslistarepository.GetAll().Where(c => c.vigente)
                                        .Where(c => c.ListaDistribucion.vigente)
                                        .Where(c => c.ListaDistribucion.codigo == CatalogosCodigos.LISTADISTRIBUCION_OFERTA_COMERCIAL)
                                        .OrderBy(c => c.orden)
                                        .ToList();


            foreach (var item in correos_lista.Where(c => c.seccion == SeccionCorreo.dirigido).ToList())
            {
                para.Add(item.correo);
            }


            foreach (var item in correos_lista.Where(c => c.seccion == SeccionCorreo.copia).ToList())
            {
                concopia.Add(item.correo);
            }

            string user = System.Web.HttpContext.Current.User.Identity.Name.ToString();

            var usuario = _usuarioRepository.GetAll().Where(c => c.Cuenta == user).FirstOrDefault();



            string href = "mailto:" + String.Join(";", para) + "?subject=" + asunto + "&" + (concopia.Count > 0 ? "CC=" + String.Join(";", concopia) + "&" : "") + "Body=%20%0A%0AEstimados%2C%0A%0AAdjunto%20a%20la%20presente%20la%20Propuesta%20de%20Trabajos%20" + descripcion2 + "%20%2C%20y%20sus%20anexos%3A%0A%0AAnexo%20-%20Propuesta%20Económica.%0AAnexo%20-%20Solicitud%20de%20Trabajos%20Adicionales.%0AAdicionalmente%20adjunto%20el%20transmittal%20de%20envío%20de%20Documentos%20%5BTransmittal%20" + codetransmittal + "%5D.%0A%0AQuedamos%20a%20vuestra%20disposición%20ante%20cualquier%20particular." + "%0A%0AGracias%20y%20saludos%2C%0A%0A" + (usuario != null ? usuario.Nombres + "%20" + usuario.Apellidos : "") + "%20%0AAdministraci%C3%B3n%20De%20Contratos";
            return href;
        }

        public string hrefoutlookOrdenProceder(int id, string to = "", string cc = "", string subject = "")
        {
            var o = Repository.Get(id);
            var contrato = _contratoRepository.Get(o.ContratoId);

            var estadopresentado = _catalogoRepository.GetAll().Where(c => c.codigo == CatalogosCodigos.OFERTA_ESTADO_PRESENTADO).FirstOrDefault();


            List<string> concopia = new List<string>();
            List<string> para = new List<string>();
            var correos_lista = _correslistarepository.GetAll().Where(c => c.vigente)
                                        .Where(c => c.ListaDistribucion.vigente)
                                        .Where(c => c.ListaDistribucion.codigo == CatalogosCodigos.LISTADISTRIBUCION_ORDEN_PROCEDER)
                                        .OrderBy(c => c.orden)
                                        .ToList();


            foreach (var item in correos_lista.Where(c => c.seccion == SeccionCorreo.dirigido).ToList())
            {
                para.Add(item.correo);
            }


            foreach (var item in correos_lista.Where(c => c.seccion == SeccionCorreo.copia).ToList())
            {
                concopia.Add(item.correo);
            }

            string codetransmittal = "";

            var requerimientoligado = _ofertacomercialpresupuestorepository.GetAllIncluding(c => c.Requerimiento.Proyecto).Where(c => c.OfertaComercialId == o.Id).Where(c => c.vigente).FirstOrDefault();
            string asunto = "A-";
            string cuerpoOferta = "";


            if (contrato.Formato == FormatoContrato.Contrato_2016)
            {
                asunto = asunto + "2016";
            }
            else
            {
                asunto = asunto + "2019";
            }
            if (requerimientoligado != null)
            {
                asunto = asunto + "-" + requerimientoligado.Requerimiento.Proyecto.codigo;
            }

            asunto = asunto + "-Orden de Proceder: " + o.codigo + "_" + o.version + " " + o.descripcion;

            cuerpoOferta = "- " + o.codigo + "_" + o.version + " " + (requerimientoligado != null && requerimientoligado.Requerimiento.tipo_requerimiento == TipoRequerimiento.Adicional ? "Adicional " : "") + o.descripcion;


            string user = System.Web.HttpContext.Current.User.Identity.Name.ToString();

            var usuario = _usuarioRepository.GetAll().Where(c => c.Cuenta == user).FirstOrDefault();



            string href = "mailto:" + String.Join(";", para) + "?subject=" + asunto + "&" + (concopia.Count > 0 ? "CC=" + String.Join(";", concopia) + "&" : "") + "Body=Estimados%2C%0A%0ASe%20recibi%C3%B3%20la%20siguiente%20Orden%20de%20Proceder%20firmada%20por%20el%20L%C3%ADder%20de%20Proyecto%20y%20la%20Gerencia%20FIC%2C%20ver%20adjuntos.%0A%0A" + cuerpoOferta + "%0A%0A%0AQuedo%20a%20las%20%C3%B3rdenes." + "%0A%0AGracias%20y%20saludos%2C%0A" + (usuario != null ? usuario.Nombres + "%20" + usuario.Apellidos : "") + "%20%0AAdministraci%C3%B3n%20De%20Contratos";
            return href;
        }


        public OfertaComercialData ObtenerDataOferta(int Id)
        {
            var estadoPresentado = _catalogoRepository.GetAll().Where(c => c.codigo == CatalogosCodigos.OFERTA_ESTADO_PRESENTADO).FirstOrDefault();
            var estadoAprobado = _catalogoRepository.GetAll().Where(c => c.codigo == CatalogosCodigos.OFERTA_ESTADO_APROBADO).FirstOrDefault();
            var estadoEnRevision = _catalogoRepository.GetAll().Where(c => c.codigo == CatalogosCodigos.OFERTA_ESTADO_ENREVISION).FirstOrDefault();

            var o = Repository.Get(Id);

            var data = new OfertaComercialData
            {
                Id = o.Id,
                codigoContrato = o.Contrato.Codigo,
                fechaPrimerEnvio = o.fecha_primer_envio.HasValue ?
                                       o.fecha_primer_envio.GetValueOrDefault().ToString("dd/MM/yyyy HH:mm:ss") : "",
                fechaOferta = o.fecha_oferta.HasValue ? o.fecha_oferta.Value.ToString("dd/MM/yyyy HH:mm:ss") : "",
                fechaUltimoEnvio = o.fecha_ultimo_envio.HasValue ? o.fecha_ultimo_envio.Value.ToString("dd/MM/yyyy HH:mm:ss") : "",
                nombreEstadoOferta = o.Catalogo.nombre,
                tieneTransmital = o.TransmitalId.HasValue && o.TransmitalId.Value > 0 ? true : false,
                comentarios = o.comentarios,
                ContratoId = o.ContratoId,
                acta_cierre = o.acta_cierre,
                alcance = o.alcance,
                centro_de_Costos_Id = o.centro_de_Costos_Id,
                codigo = o.codigo,
                version = o.version,
                codigo_shaya = o.codigo_shaya,
                computo_completo = o.computo_completo,
                descripcion = o.descripcion,
                dias_emision_oferta = o.dias_emision_oferta,
                dias_hasta_recepcion_so = o.dias_hasta_recepcion_so,
                estado = o.estado,
                estado_oferta = o.estado_oferta,
                estatus_de_Ejecucion = o.estatus_de_Ejecucion,
                es_final = o.es_final,
                fecha_oferta = o.fecha_oferta,
                fecha_orden_proceder = o.fecha_orden_proceder,
                fecha_pliego = o.fecha_pliego,
                fecha_primer_envio = o.fecha_primer_envio,
                fecha_recepcion_so = o.fecha_recepcion_so,
                fecha_ultima_modificacion = o.fecha_ultima_modificacion,
                fecha_ultimo_envio = o.fecha_ultimo_envio,
                monto_certificado_aprobado_acumulado = o.monto_certificado_aprobado_acumulado,
                forma_contratacion = o.forma_contratacion,
                link_documentum = o.link_documentum,
                monto_ofertado = o.monto_ofertado,
                monto_ofertado_pendiente_aprobacion = o.monto_ofertado - o.monto_so_aprobado,
                monto_so_aprobado = o.monto_so_aprobado,
                monto_so_referencia_total = o.monto_so_referencia_total,
                OfertaPadreId = o.OfertaPadreId,
                orden_proceder = o.orden_proceder,
                orden_proceder_enviada_por = o.orden_proceder_enviada_por,
                porcentaje_avance = o.porcentaje_avance,
                revision_Oferta = o.revision_Oferta,
                service_order = o.service_order,
                service_request = o.service_request,
                tipo_Trabajo_Id = o.tipo_Trabajo_Id,
                TransmitalId = o.TransmitalId,
                codigoTransmittal = o.TransmitalId.HasValue && o.TransmitalId.Value > 0 ? _transmitalRepository.Get(o.TransmitalId.Value).codigo_transmital : "",
                vigente = o.vigente,
                monto_editado = o.monto_editado,
                link_ordenProceder = o.link_ordenProceder,

                monto_ofertado_migracion_actual = o.monto_ofertado_migracion_actual,
                monto_so_aprobado_migracion_anterior = o.monto_so_aprobado_migracion_anterior,
                monto_so_aprobado_migracion_actual = o.monto_so_aprobado_migracion_actual,




            };
            data.tienePresupuestoLigado = _ofertacomercialpresupuestorepository.GetAll().Where(c => c.OfertaComercialId == o.Id).Where(c => c.vigente).Where(c => c.PresupuestoId.HasValue).Count() > 0 ? true : false;


            data.tienePresupuestosAdicionales = _ofertacomercialpresupuestorepository.GetAll()
                                                                                     .Where(c => c.OfertaComercialId == o.Id)
                                                                                     .Where(c => c.vigente)
                                                                                     .Where(c => c.Requerimiento.tipo_requerimiento == TipoRequerimiento.Adicional)
                                                                                     .Count() > 0 ?
                                                                                     true : false;
            data.tieneRequerimientoPrincipal = _ofertacomercialpresupuestorepository.GetAll()
                                                                                     .Where(c => c.OfertaComercialId == o.Id)
                                                                                     .Where(c => c.vigente)
                                                                                     .Where(c => c.Requerimiento.tipo_requerimiento == TipoRequerimiento.Principal)
                                                                                     .Count() > 0 ?
                                                                                     true : false;


            data.puedeEditarMontoAprobado = true; // 



            //ACTUALIZACIONES COMPORTAMIENTO SO 26-04-2022 TRAC 112.
            if (data.tieneRequerimientoPrincipal)
            { 
                decimal valorMontoEnDetallesPO = 0;
                var detallesPO = _detalleordenesService.GetAll().Where(po => po.vigente)
                                                                 .Where(po => po.OfertaComercialId == o.Id)
                                                                 .ToList();

                valorMontoEnDetallesPO = detallesPO.Count > 0 ? detallesPO.Sum(po => po.valor_os) : 0;

               

                var valorTotalMigracionPO = (data.monto_so_aprobado_migracion_actual + data.monto_so_aprobado_migracion_anterior);
                if (valorMontoEnDetallesPO == 0 && valorTotalMigracionPO != 0 && !data.monto_editado) { // SI NO SE EDITA MANUALMENTE EL MONTO APROBADO 
                    data.monto_so_aprobado = valorTotalMigracionPO;


                    //ACTUALIZA VALORES
                    o.monto_so_aprobado = valorTotalMigracionPO;
                    o.monto_ofertado_pendiente_aprobacion = (o.monto_ofertado + o.monto_ofertado_migracion_actual) - o.monto_so_aprobado;

                    /*o.monto_so_aprobado_migracion_actual = 0;
                    o.monto_so_aprobado_migracion_anterior = 0;*/

                }
                if (valorMontoEnDetallesPO > 0 && !data.monto_editado )
                { // SOLO SI EL USUARIO NO EDITA MANUALMENTE EL MONTO APROBADO 

                    if (valorMontoEnDetallesPO > valorTotalMigracionPO) { 
                    data.monto_so_aprobado = valorMontoEnDetallesPO;
                    o.monto_so_aprobado = valorMontoEnDetallesPO; //ACTUALIZA DIRECTAMENTE LA CABECERA DE LA OFERTA SIN NECESIDAD DE OTRO QUERY PARA UPDATE SOLO CON GET

                    o.monto_ofertado_pendiente_aprobacion = (o.monto_ofertado + o.monto_ofertado_migracion_actual) - valorMontoEnDetallesPO;
                    }
                }

                if (data.monto_editado) {

                    //DATOS PANTALLA
                    data.monto_so_aprobado_migracion_actual = 0;
                    data.monto_so_aprobado_migracion_anterior = 0;

                    //UPDATE CABECERA
                    o.monto_so_aprobado_migracion_actual = 0;
                    o.monto_so_aprobado_migracion_anterior = 0;
                }

            }

      

            //Petición Danes Reyes  el 01/10/2020 -> confirmado 05/10/2020
            /*Si la Oferta esta en estado Revision y tiene Presupuestos Adicionales Ligados*/
            if (data.tienePresupuestosAdicionales && !data.monto_editado && estadoEnRevision != null && data.estado_oferta == estadoEnRevision.Id)
            {
                data.puedeEditarMontoAprobado = false; // 
                data.monto_so_aprobado = 0;
                o.monto_so_aprobado = 0;


                //DATOS PANTALLA
                data.monto_so_aprobado_migracion_actual = 0;
                data.monto_so_aprobado_migracion_anterior = 0;

                //UPDATE CABECERA
                o.monto_so_aprobado_migracion_actual = 0;
                o.monto_so_aprobado_migracion_anterior = 0;
            }
            else
            /*Si la Oferta esta en estado Presentado y tiene Presupuestos Adicionales Ligados*/
            if (data.tienePresupuestosAdicionales && !data.monto_editado && estadoPresentado != null && data.estado_oferta == estadoPresentado.Id)
            {
                data.puedeEditarMontoAprobado = false; // 
                data.monto_so_aprobado = 0;
                o.monto_so_aprobado = 0;


                //DATOS PANTALLA
                data.monto_so_aprobado_migracion_actual = 0;
                data.monto_so_aprobado_migracion_anterior = 0;

                //UPDATE CABECERA
                o.monto_so_aprobado_migracion_actual = 0;
                o.monto_so_aprobado_migracion_anterior = 0;
            }
            else

            /*Si la Oferta esta en estado Aprobado y tiene Presupuestos Adicionales Ligados*/
            if (data.tienePresupuestosAdicionales && !data.monto_editado && estadoAprobado != null && data.estado_oferta == estadoAprobado.Id)
            {
                data.puedeEditarMontoAprobado = true; // 

                data.monto_so_aprobado = data.monto_ofertado;
                o.monto_so_aprobado = data.monto_ofertado;
            }



            return data;
        }

        public bool ActualizarMontoAprobado()
        {
            var ofertaComercialPresupuestos = _ofertacomercialpresupuestorepository.GetAllIncluding(c => c.Requerimiento.Proyecto, c => c.OfertaComercial)
                                                                                   .Where(c => c.Requerimiento.tipo_requerimiento == TipoRequerimiento.Principal)
                                                                                   .Where(c => c.vigente)
                                                                                   .Where(c => c.OfertaComercial.es_final == 1)
                                                                                   .ToList();
            foreach (var ol in ofertaComercialPresupuestos)
            {
                var posasignados = _detalleordenesService.GetAllIncluding(c => c.OrdenServicio)
                                                       .Where(c => c.OfertaComercialId == ol.Id)
                                                       .Where(c => c.vigente).Count();
                if (posasignados > 0)
                {

                    var ofertasreqadicionales = _ofertacomercialpresupuestorepository.GetAll().Where(c => c.Requerimiento.tipo_requerimiento == TipoRequerimiento.Adicional)
                                                                                              .Where(c => c.vigente)
                                                                                              .Where(c => c.Requerimiento.ProyectoId == ol.Requerimiento.ProyectoId)
                                                                                              .ToList();
                    foreach (var ad in ofertasreqadicionales)
                    {
                        var estadoaprobado = _catalogoRepository.GetAll().Where(c => c.codigo == CatalogosCodigos.OFERTA_ESTADO_APROBADO).FirstOrDefault();

                        var ofertaComercial = Repository.Get(ad.OfertaComercialId);
                        ofertaComercial.monto_so_aprobado = ofertaComercial.monto_ofertado;
                        ofertaComercial.estado_oferta = estadoaprobado != null ? estadoaprobado.Id : 0;
                        Repository.Update(ofertaComercial);

                    }


                }



            }
            return true;

        }

        public string ActualizarMontoAprobadoOferta(int id, decimal monto_aprobado)
        {
            var o = Repository.Get(id);
            if (o.monto_so_aprobado != monto_aprobado)
            {
                o.monto_so_aprobado = monto_aprobado;
                o.monto_ofertado_pendiente_aprobacion = o.monto_ofertado - monto_aprobado;
                o.monto_editado = true;
                Repository.Update(o);
            }
            return "OK";
        }

        public bool tienePresupuestosAdicionales(int id)
        {
            var query = _ofertacomercialpresupuestorepository.GetAll().Where(c => c.vigente).Where(c => c.OfertaComercialId == id).Where(c => c.Requerimiento.tipo_requerimiento == TipoRequerimiento.Adicional).ToList().Count();
            return query > 0 ? true : false;
        }

        public List<Contrato> ObtenerContratos()
        {
            return _contratoRepository.GetAll().Where(c => c.vigente).ToList();
        }

        public string ActualizarMontoAprobadoSegunEstadoOferta()
        {
            var estadoPresentado = _catalogoRepository.GetAll().Where(c => c.codigo == CatalogosCodigos.OFERTA_ESTADO_PRESENTADO).FirstOrDefault();
            var estadoAprobado = _catalogoRepository.GetAll().Where(c => c.codigo == CatalogosCodigos.OFERTA_ESTADO_APROBADO).FirstOrDefault();
            var estadoEnRevision = _catalogoRepository.GetAll().Where(c => c.codigo == CatalogosCodigos.OFERTA_ESTADO_ENREVISION).FirstOrDefault();


            var query = Repository.GetAll().Where(c => c.vigente).Where(c => c.es_final == 1).ToList();
            var queryOfertaPresupuesto = _ofertacomercialpresupuestorepository.GetAllIncluding(c => c.Requerimiento)
                                                                              .Where(c => c.vigente)
                                                                              .ToList();

            foreach (var o in query)
            {
                var ofertaUptate = Repository.Get(o.Id);
                var tienePresupuestosAdicionales = (from r in queryOfertaPresupuesto
                                                    where r.Requerimiento.tipo_requerimiento == TipoRequerimiento.Adicional
                                                    where r.OfertaComercialId == o.Id
                                                    select r
                                                   ).ToList().Count > 0 ? true : false;


                //Petición Danes Reyes  el 01/10/2020 -> confirmado 05/10/2020
                /*Si la Oferta esta en estado Revision y tiene Presupuestos Adicionales Ligados*/
                if (tienePresupuestosAdicionales && !o.monto_editado && estadoEnRevision != null && o.estado_oferta == estadoEnRevision.Id)
                {
                    o.monto_so_aprobado = 0;
                    ofertaUptate.monto_so_aprobado = 0;
                }
                else
                /*Si la Oferta esta en estado Presentado y tiene Presupuestos Adicionales Ligados*/
                if (tienePresupuestosAdicionales && !o.monto_editado && estadoPresentado != null && o.estado_oferta == estadoPresentado.Id)
                {
                    o.monto_so_aprobado = 0;
                    ofertaUptate.monto_so_aprobado = 0;
                }
                else

                /*Si la Oferta esta en estado Aprobado y tiene Presupuestos Adicionales Ligados*/
                if (tienePresupuestosAdicionales && !o.monto_editado && estadoAprobado != null && o.estado_oferta == estadoAprobado.Id)
                {

                    o.monto_so_aprobado = o.monto_ofertado;
                    ofertaUptate.monto_so_aprobado = o.monto_ofertado;
                }



            }



            return "OK";
        }


        public int GuardarArchivoOrden(int OfertaComercialId, HttpPostedFileBase UploadedFile)
        {

            if (UploadedFile != null)
            {


                var contador = this.ListaArchivosOrden(OfertaComercialId).Count + 1;
                string fileName = UploadedFile.FileName;
                string fileContentType = UploadedFile.ContentType;
                byte[] fileBytes = new byte[UploadedFile.ContentLength];
                var data = UploadedFile.InputStream.Read(fileBytes, 0,
                    Convert.ToInt32(UploadedFile.ContentLength));

                ArchivoOrdenProceder n = new ArchivoOrdenProceder
                {
                    Id = 0,
                    nombre = fileName,
                    fecha_registro = DateTime.Now,
                    hash = fileBytes,
                    tipo_contenido = fileContentType,
                    ofertaComercialId = OfertaComercialId
                };
                var archivoid = _archivoOrdenProceder.InsertAndGetId(n);

                if (archivoid > 0)
                {
                    return 1;

                }
                else
                {
                    return 0;

                }

            }
            return 0;
        }

        public List<ArchivoOrdenProcederDto> ListaArchivosOrden(int OfertaComercialId)
        {
            var listaarhivos = _archivoOrdenProceder.GetAll().Where(c => c.ofertaComercialId == OfertaComercialId).ToList();

            var items = (from a in listaarhivos
                         select new ArchivoOrdenProcederDto
                         {
                             Id = a.Id,
                             ofertaComercialId = a.ofertaComercialId,
                             fecha_registro = a.fecha_registro,
                             formatFechaRegistro = a.fecha_registro.ToShortDateString(),
                             hash = a.hash,
                             nombre = a.nombre,
                             tipo_contenido = a.tipo_contenido
                         }).ToList();

            foreach (var i in items)
            {
                i.filebase64 = Convert.ToBase64String(i.hash);
            }
            return items;
        }

        public int EditarArchivoOrdenProceder(int Id, HttpPostedFileBase UploadedFile)
        {
            var archivo = _archivoOrdenProceder.Get(Id);
            if (UploadedFile != null)
            {
                string fileName = UploadedFile.FileName;
                string fileContentType = UploadedFile.ContentType;
                byte[] fileBytes = new byte[UploadedFile.ContentLength];
                var data = UploadedFile.InputStream.Read(fileBytes, 0,
                    Convert.ToInt32(UploadedFile.ContentLength));

                archivo.hash = fileBytes;
                archivo.nombre = fileName;
                archivo.fecha_registro = DateTime.Now;
                archivo.tipo_contenido = fileContentType;

                var resultado = _archivoOrdenProceder.Update(archivo);
                return 1;
            }
            else
            {
                return 0;
            }

        }

        public int EliminarArchivoOrdenProceder(int id)
        {
            var archivo = _archivoOrdenProceder.Get(id);
            int ofertaComercialID = archivo.ofertaComercialId;
            _archivoOrdenProceder.Delete(archivo);
            return ofertaComercialID;
        }

        public ArchivoOrdenProceder DetalleArchivo(int id)
        {
            return _archivoOrdenProceder.Get(id);
        }



        public string Actualizarmonto_ofertado_migracion_actual(int id, decimal monto_ofertado_migracion_actual)
        {

            var o = Repository.Get(id);
            if (o.monto_ofertado_migracion_actual != monto_ofertado_migracion_actual)
            {
                o.monto_ofertado_migracion_actual = monto_ofertado_migracion_actual;
                Repository.Update(o);
            }
            return "OK";

        }

        public string Actualizarmonto_so_aprobado_migracion_actual(int id, decimal monto_so_aprobado_migracion_actual)
        {
            var o = Repository.Get(id);
            if (o.monto_so_aprobado_migracion_actual != monto_so_aprobado_migracion_actual)
            {
                o.monto_so_aprobado_migracion_actual = monto_so_aprobado_migracion_actual;
                Repository.Update(o);
            }
            return "OK";
        }

        public string Actualizarmonto_so_aprobado_migracion_anterior(int id, decimal monto_so_aprobado_migracion_anterior)
        {
            var o = Repository.Get(id);
            if (o.monto_so_aprobado_migracion_anterior != monto_so_aprobado_migracion_anterior)
            {
                o.monto_so_aprobado_migracion_anterior = monto_so_aprobado_migracion_anterior;
                Repository.Update(o);
            }
            return "OK";
        }
    }
}

