using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.aplicacion.Dto;
using com.cpp.calypso.proyecto.aplicacion.Interfaces;
using com.cpp.calypso.proyecto.dominio;
using com.cpp.calypso.proyecto.dominio.Models;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace com.cpp.calypso.proyecto.aplicacion
{
    public class AvanceObraAsyncBaseCrudAppService : AsyncBaseCrudAppService<AvanceObra, AvanceObraDto, PagedAndFilteredResultRequestDto>, IAvanceObraAsyncBaseCrudAppService
    {
        private readonly IBaseRepository<Oferta> _ofertaRepository;
        private readonly IBaseRepository<DetalleAvanceObra> _detalleAvanceObraRepository;
        private readonly IBaseRepository<Catalogo> _catalogoRepository;
        private readonly IBaseRepository<Wbs> _wbsRepository;
        private readonly IBaseRepository<Computo> _computoRepository;
        private readonly IBaseRepository<Item> _itemRepository;
        private readonly IBaseRepository<Proyecto> _proyectoRepository;
        private readonly IBaseRepository<RdoCabecera> _rdoCabeceraRepository;
        //Archivos avance obra
        private readonly IBaseRepository<Archivo> _archivoRepository;
        private readonly IBaseRepository<ArchivosAvanceObra> _archivoAvanceObraRepository;

        private readonly IBaseRepository<RdoDetalleEac> _eacrepository;

        public IRdoCabeceraAsyncBaseCrudAppService _rdoCabeceraService { get; }

        public AvanceObraAsyncBaseCrudAppService(
            IBaseRepository<AvanceObra> repository,
            IBaseRepository<Oferta> ofertaRepository,
            IBaseRepository<DetalleAvanceObra> detalleAvanceObraRepository,
            IBaseRepository<Catalogo> catalogoRepository,
            IBaseRepository<Wbs> wbsRepository,
            IBaseRepository<Computo> computoRepository,
            IBaseRepository<Archivo> archivoRepository,
            IBaseRepository<ArchivosAvanceObra> archivoAvanceObraRepository,
            IBaseRepository<Item> itemRepository,
            IBaseRepository<Proyecto> proyectoRepository,
            IBaseRepository<RdoCabecera> rdoCabeceraRepository,
            IBaseRepository<RdoDetalleEac> eacrepository,
        IRdoCabeceraAsyncBaseCrudAppService rdoCabeceraService
        ) : base(repository)
        {
            _ofertaRepository = ofertaRepository;
            _detalleAvanceObraRepository = detalleAvanceObraRepository;
            _catalogoRepository = catalogoRepository;
            _wbsRepository = wbsRepository;
            _computoRepository = computoRepository;
            _archivoRepository = archivoRepository;
            _archivoAvanceObraRepository = archivoAvanceObraRepository;
            _itemRepository = itemRepository;
            _proyectoRepository = proyectoRepository;
            _rdoCabeceraService = rdoCabeceraService;
            _rdoCabeceraRepository = rdoCabeceraRepository;
            _eacrepository = eacrepository;
        }

        public List<OfertaDto> ListarOfertasDeProyecto(int ProyectoId)
        {
            var OfertaQuery = _ofertaRepository.GetAllIncluding(c => c.Proyecto.Contrato.Cliente).Where(d=>d.ProyectoId == ProyectoId).Where(d=>d.es_final);

            var items = (from o in OfertaQuery
                         where o.vigente == true
                         where o.ProyectoId == ProyectoId
                         where o.es_final == true
                         select new OfertaDto()
                         {
                             Id = o.Id,
                             codigo = o.codigo,
                             fecha_oferta = o.fecha_oferta,
                             estado_oferta = o.estado_oferta,
                             cliente_razon_social = o.Proyecto.Contrato.Cliente.razon_social,
                             proyecto_codigo = o.Proyecto.codigo
                         }).ToList();
            return items;
        }

        public List<AvanceObraDto> ListarAvancesDeOferta(int OfertaId)
        {
            var AvanceObraQuery = Repository.GetAll();
            var items = (from a in AvanceObraQuery
                         where a.vigente == true
                         where a.OfertaId == OfertaId
                         select new AvanceObraDto()
                         {
                             Id = a.Id,
                             OfertaId = a.OfertaId,
                             alcance = a.alcance,
                             certificado = a.certificado,
                             comentario = a.comentario,
                             descripcion = a.descripcion,
                             fecha_desde = a.fecha_desde,
                             fecha_hasta = a.fecha_hasta,
                             fecha_presentacion = a.fecha_presentacion,
                             monto_construccion = a.monto_construccion,
                             monto_ingenieria = a.monto_ingenieria,
                             monto_suministros = a.monto_suministros,
                             monto_total = a.monto_total,
                             vigente = a.vigente,
                             aprobado = a.aprobado,
                             estado = a.estado
                         }).OrderByDescending(c => c.fecha_presentacion).ToList();
            return items;
        }
        public List<AvanceObraDto> ListarAvancesDeOfertaSinCertificar(int OfertaId, DateTime fechaCorte)
        {
            var AvanceObraQuery = Repository.GetAll().Where(c => c.OfertaId == OfertaId).Where(c => c.vigente)
                .Where(c=>c.OfertaId== OfertaId);
            var items = (from a in AvanceObraQuery
                         where a.vigente == true
                         where a.OfertaId == OfertaId
                         where a.certificado == 0
                         where a.fecha_presentacion <= fechaCorte
                         select new AvanceObraDto()
                         {
                             Id = a.Id,
                             OfertaId = a.OfertaId,
                             Oferta = a.Oferta,
                             alcance = a.alcance,
                             certificado = a.certificado,
                             comentario = a.comentario,
                             descripcion = a.descripcion,
                             fecha_desde = a.fecha_desde,
                             fecha_hasta = a.fecha_hasta,
                             fecha_presentacion = a.fecha_presentacion,
                             monto_construccion = a.monto_construccion,
                             monto_ingenieria = a.monto_ingenieria,
                             monto_suministros = a.monto_suministros,
                             monto_total = a.monto_total,
                             vigente = a.vigente,
                             aprobado = a.aprobado,
                             estado = a.estado,


                         }).ToList();
            return items;
        }
        public List<AvanceObraDto> ListarAvancesResumen(int OfertaId)
        {
            List<AvanceObraDto> list = new List<AvanceObraDto>();
            int itemsOferta = 0;
            int itemsNoApr = 0;
            decimal montoP = 0;
            decimal montoNP = 0;
            var avances = ListarAvancesDeOferta(OfertaId);

            foreach (var avance in avances)
            {

                var detalles = ListarDetallesAvanceObra(avance.Id);

                foreach (var detalle in detalles)
                {
                    if (detalle.Computo.presupuestado)
                    {
                        itemsOferta++;
                        montoNP += detalle.total;
                    }
                    if (!detalle.Computo.presupuestado)
                    {
                        itemsNoApr++;
                        montoP += detalle.total;
                    }
                }
                AvanceObraDto a = new AvanceObraDto()
                {
                    Id = avance.Id,
                    fecha_presentacion = avance.fecha_presentacion,
                    descripcion = avance.descripcion,
                    vItemsOferta = itemsOferta,
                    vItemsNoAprobados = itemsNoApr,
                    vMontoPendiente = montoP,
                    vMontoPresupuestado = montoNP,
                    vTotal = montoP + montoNP,
                    certificado = avance.certificado,
                    monto_total = avance.monto_total
                };

                list.Add(a);
                itemsOferta = 0;
                itemsNoApr = 0;
                montoP = 0;
                montoNP = 0;
            }

            return list;
        }

        public int EliminarVigencia(int avanceObraId)
        {
            var avance = Repository.Get(avanceObraId);
            var avanceOferta = Repository.GetAllIncluding(c => c.Oferta).Where(c => c.Id == avanceObraId).FirstOrDefault();


            var avanceDate = avance.fecha_presentacion.Value.Date;
            var ExisteRdoGenerado = _rdoCabeceraRepository.GetAll()
                                                          .Where(c => c.es_definitivo)
                                                          .Where(c => c.fecha_rdo > avanceDate)
                                                          .Where(c => c.vigente)
                                                          .Where(c => c.ProyectoId == avanceOferta.Oferta.ProyectoId)
                                                          .OrderByDescending(c => c.fecha_rdo)
                                                          .FirstOrDefault();
            if (ExisteRdoGenerado != null)
            {
                return avance.OfertaId;
            }




            avance.vigente = false;
            Repository.Update(avance);
            return avance.OfertaId;
        }

        public List<DetalleAvanceObraDto> ListarDetallesAvanceObra(int avanceObraId)
        {
            var DetalleAvanceObraQuery = _detalleAvanceObraRepository.GetAllIncluding(d => d.AvanceObra, d => d.Computo).Where(d => d.AvanceObraId == avanceObraId);
            var items = (from a in DetalleAvanceObraQuery
                         where a.vigente == true
                         where a.AvanceObraId == avanceObraId
                         //where a.estacertificado == false
                         select new DetalleAvanceObraDto()
                         {
                             Id = a.Id,
                             AvanceObraId = a.AvanceObraId,
                             AvanceObra = a.AvanceObra,
                             vigente = a.vigente,
                             Computo = a.Computo,
                             precio_unitario = a.precio_unitario,
                             total = a.total,
                             cantidad_diaria = a.cantidad_diaria,
                             Wbs = a.Computo.Wbs,
                             item_codigo = a.Computo.Item.codigo,
                             nombre_item = a.Computo.Wbs.nivel_nombre,
                             cantidad_acumulada_anterior = a.cantidad_acumulada_anterior,
                             cantidad_acumulada = a.cantidad_acumulada,
                             presupuestado = a.Computo.presupuestado,
                             budget = a.Computo.cantidad,
                             cantidad_eac = a.Computo.cantidad_eac,
                             fecha_registro = a.fecha_registro

                         }).ToList();

            foreach (var d in items)
            {
                var name = _wbsRepository
                    .GetAll()
                    .Where(o => o.vigente == true)
                    .Where(o => o.OfertaId == d.Wbs.OfertaId).SingleOrDefault(o => o.id_nivel_codigo == d.Wbs.id_nivel_padre_codigo);
                d.nombre_padre = name.nivel_nombre;

                d.fechar = d.fecha_registro.GetValueOrDefault().ToShortDateString();
            }
            /*
                        foreach (var a in items)
                        {
                            a.nombre_area = ObtenerNombreCatalogo(a.Wbs.AreaId);
                            a.nombre_disciplina = ObtenerNombreCatalogo(a.Wbs.DisciplinaId);
                            a.nombre_elemento = ObtenerNombreCatalogo(a.Wbs.ElementoId);
                            a.nombre_actividad = ObtenerNombreCatalogo(a.Wbs.ActividadId);
                        }
                        */
            return items;
        }

        public List<DetalleAvanceObraDto> ListarDetallesAvanceObraFast(int avanceObraId)
        {
            var DetalleAvanceObraQuery = _detalleAvanceObraRepository.GetAllIncluding(d => d.AvanceObra.Oferta, d => d.Computo).Where(d=>d.AvanceObraId== avanceObraId);
            var items = (from a in DetalleAvanceObraQuery
                         where a.vigente == true
                         where a.AvanceObraId == avanceObraId
                         //where a.estacertificado == false
                         select new DetalleAvanceObraDto()
                         {
                             Id = a.Id,
                             AvanceObraId = a.AvanceObraId,
                             //AvanceObra = a.AvanceObra,
                             codigoOferta=a.AvanceObra.Oferta.codigo,
                             vigente = a.vigente,
                            // Computo = a.Computo,
                             precio_unitario = a.precio_unitario,
                             total = a.total,
                             cantidad_diaria = a.cantidad_diaria,
                           //  Wbs = a.Computo.Wbs,
                             item_codigo = a.Computo.Item.codigo,
                             nombre_item = a.Computo.Wbs.nivel_nombre,
                             cantidad_acumulada_anterior = a.cantidad_acumulada_anterior,
                             cantidad_acumulada = a.cantidad_acumulada,
                             presupuestado = a.Computo.presupuestado,
                             budget = a.Computo.cantidad,
                             cantidad_eac = a.Computo.cantidad_eac,
                             fecha_registro = a.fecha_registro,
                             cantidad_presupuestada=a.Computo.cantidad,
                             

                         }).ToList();

            foreach (var d in items)
            {
              /*  var name = _wbsRepository
                    .GetAll()
                    .Where(o => o.vigente == true)
                    .Where(o => o.OfertaId == d.Wbs.OfertaId).SingleOrDefault(o => o.id_nivel_codigo == d.Wbs.id_nivel_padre_codigo);
                d.nombre_padre = name.nivel_nombre;*/

                d.fechar = d.fecha_registro.GetValueOrDefault().ToShortDateString();
            }
            /*
                        foreach (var a in items)
                        {
                            a.nombre_area = ObtenerNombreCatalogo(a.Wbs.AreaId);
                            a.nombre_disciplina = ObtenerNombreCatalogo(a.Wbs.DisciplinaId);
                            a.nombre_elemento = ObtenerNombreCatalogo(a.Wbs.ElementoId);
                            a.nombre_actividad = ObtenerNombreCatalogo(a.Wbs.ActividadId);
                        }
                        */
            return items;
        }

        public ProyectoDto GetProyecto(int avanceObraId)
        {
            var AvanceObraQuery = Repository.GetAllIncluding(o => o.Oferta.Proyecto);
            var item = (from w in AvanceObraQuery
                        where w.Id == avanceObraId
                        where w.vigente == true
                        select new AvanceObraDto()
                        {
                            Oferta = w.Oferta
                        }).FirstOrDefault();

            return new ProyectoDto()
            {
                presupuesto = item.Oferta.Proyecto.presupuesto,
                nombre_proyecto = item.Oferta.Proyecto.nombre_proyecto,
                codigo = item.Oferta.Proyecto.codigo
            };
        }

        public List<ComputoAvanceObra> ObtenerComputosAvanceObra(int IdOferta, DateTime fecha, int AvanceObraId)
        {
            var detallesQuery = _detalleAvanceObraRepository.GetAll()
                .Where(o => o.vigente == true)
                .Where(o => o.AvanceObraId == AvanceObraId).ToList();


            var query = _computoRepository.GetAllIncluding(o => o.Item, o => o.Wbs)
                .Where(o => o.vigente == true)
                .Where(o => o.Wbs.OfertaId == IdOferta).ToList();


            var data = (from c in query
                        where !(from o in detallesQuery
                                select o.ComputoId)
                            .Contains(c.Id)
                        select new ComputoAvanceObra()
                        {
                            Actividad = c.Wbs.nivel_nombre,
                            WbsActividad = c.Wbs,
                            CodigoItem = c.Item.codigo,
                            NombreItem = c.Item.nombre,
                            GrupoId = c.Item.GrupoId,
                            ComputoId = c.Id,
                            PrecioUnitario = c.precio_unitario,
                            CantidadEAC = c.cantidad_eac,
                            Budget = c.cantidad,
                            cantidadAjustada = c.cantidadAjustada,
                            tienecantidadAjustada = c.cantidadAjustada ? "SI" : "NO",
                            tipoCantidadAjustada = c.tipo


                        }).ToList();


            var detallesRDOHastaFecha = new List<RdoDetalleEac>();
            var oferta = _ofertaRepository.GetAll().Where(o => o.Id == IdOferta).FirstOrDefault(); //Oferta BaseRDO
            if (oferta != null) {
             var RdoHastaFechaActualAvance = _rdoCabeceraRepository.GetAll().Where(c => c.vigente)
                                                         .Where(c => c.ProyectoId == oferta.ProyectoId)
                                                         .Where(c => c.es_definitivo)
                                                         .Where(c => c.fecha_rdo < fecha)
                                                         .OrderByDescending(c => c.fecha_rdo)
                                                         .FirstOrDefault();
                if (RdoHastaFechaActualAvance != null)
                {

                    var detalles = _eacrepository.GetAll().Where(c => c.vigente)
                                                .Where(c => c.RdoCabeceraId == RdoHastaFechaActualAvance.Id)
                                                .ToList();
                    detallesRDOHastaFecha.AddRange(detalles);

                }

            }



            foreach (var i in data)
            {


                //i.CantidadAnterior = this.ObtenerCantidadAcumulada(i.ComputoId, fecha, IdOferta); //Anterior Avance de Obra

                //#AVANCE RDO ANTERIOR RDO
                i.CantidadAnterior = this.ObtenerCantidadAcumuladaAnteriorRDO(i.ComputoId, detallesRDOHastaFecha); //Cantidad Hasta el RDO Definitivo Menor a la fecha de Avance
                i.padre_superior = this.NivelPadre(i.WbsActividad, IdOferta, 1);
                i.padre_principal = this.NivelPadre(i.WbsActividad, IdOferta, 2);
            }
            return data.Where(c => c.GrupoId == 2).ToList();


        }


        decimal ObtenerCantidadAcumuladaAnteriorRDO(int computoId, List<RdoDetalleEac> detallesRdoHastaFechaActualAvance)
        {
            decimal cantidadAnterior = 0;
            var query = (from o in detallesRdoHastaFechaActualAvance
                         where o.vigente
                         where o.ComputoId == computoId
                         select o).ToList();

            foreach (var d in query)
            {
                cantidadAnterior += Decimal.Round(d.cantidad_acumulada, 8);
            }
            return Decimal.Round(cantidadAnterior, 8);
        }


        public decimal ObtenerCantidadAcumulada(int computoId, DateTime fecha_reporte, int ofertaId)
        {
            decimal cantidad_acumulada = 0;
            var query = _detalleAvanceObraRepository.GetAllIncluding(o => o.AvanceObra)
                .Where(o => o.vigente == true)
                .Where(o => o.ComputoId == computoId)
                .Where(o => o.AvanceObra.aprobado == true)
                .Where(o => o.AvanceObra.vigente)
                .Where(o => o.AvanceObra.fecha_presentacion < fecha_reporte)
                .Where(o => o.AvanceObra.OfertaId == ofertaId);

            var detalles = (from d in query
                            select new DetalleAvanceIngenieriaDto()
                            {
                                cantidad_horas = d.cantidad_diaria,
                            }).ToList();

            foreach (var d in detalles)
            {
                cantidad_acumulada += d.cantidad_horas;
            }
            return cantidad_acumulada;
        }

        public string ObtenerNombreCatalogo(int id)
        {
            var areasQ = _catalogoRepository.GetAll();
            var item = (from w in areasQ
                        where w.Id == id
                        where w.vigente == true
                        select new CatalogoDto()
                        {
                            Id = w.Id,
                            nombre = w.nombre
                        }).FirstOrDefault();
            return item.nombre;
        }

        public List<DetalleAvanceObraDto> ListarDetallesAvanceObraProyecto(int ProyectoId, DateTime fechaCorte)
        {
            List<DetalleAvanceObraDto> detallesavanceobra = new List<DetalleAvanceObraDto>();


            var ofertas = this.ListarOfertasDeProyecto(ProyectoId);
            if (ofertas.Count > 0)
            {
                foreach (var offinal in ofertas)
                {

                    var avances = this.ListarAvancesDeOfertaSinCertificar(offinal.Id, fechaCorte);

                    if (avances.Count > 0)
                    {
                        foreach (var ovance in avances)
                        {

                            var detalles = this.ListarDetallesAvanceObra(ovance.Id);

                            if (detalles.Count > 0)
                            {
                                detallesavanceobra.AddRange(detalles);

                              /*  foreach (var da in detalles)
                                {
                                    detallesavanceobra.Add(da);
                                }*/
                            }

                        }


                    }

                }

            }


            return detallesavanceobra;
        }


        public List<DetalleAvanceObraDto> ListarDetallesAvanceObraProyectoFast(int ProyectoId, DateTime fechaCorte)
        {
            List<DetalleAvanceObraDto> detallesavanceobra = new List<DetalleAvanceObraDto>();


            var ofertas = this.ListarOfertasDeProyecto(ProyectoId);
            if (ofertas.Count > 0)
            {
                foreach (var offinal in ofertas)
                {

                    var avances = this.ListarAvancesDeOfertaSinCertificar(offinal.Id, fechaCorte);

                    if (avances.Count > 0)
                    {
                        foreach (var ovance in avances)
                        {

                            var detalles = this.ListarDetallesAvanceObraFast(ovance.Id);

                            if (detalles.Count > 0)
                            {
                                detallesavanceobra.AddRange(detalles);

                                /*  foreach (var da in detalles)
                                  {
                                      detallesavanceobra.Add(da);
                                  }*/
                            }

                        }


                    }

                }

            }


            return detallesavanceobra;
        }

        public int GuardarArchivo(int AvanceObraId, HttpPostedFileBase[] UploadedFile)
        {

            if (UploadedFile != null && UploadedFile.Length > 0)
            {
                foreach (var archivo in UploadedFile)
                {


                    var contador = this.ListaArchivos(AvanceObraId).Count + 1;
                    string fileName = archivo.FileName;
                    string fileContentType = archivo.ContentType;
                    byte[] fileBytes = new byte[archivo.ContentLength];
                    var data = archivo.InputStream.Read(fileBytes, 0,
                        Convert.ToInt32(archivo.ContentLength));

                    Archivo n = new Archivo
                    {
                        Id = 0,
                        codigo = "AOBRA" + contador,
                        nombre = fileName,
                        vigente = true,
                        fecha_registro = DateTime.Now,
                        hash = fileBytes,
                        tipo_contenido = fileContentType,
                    };
                    var archivoid = _archivoRepository.InsertAndGetId(n);

                    if (archivoid > 0)
                    {
                        ArchivosAvanceObra ac = new ArchivosAvanceObra()
                        {
                            Id = 0,
                            AvanceObraId = AvanceObraId,
                            ArchivoId = archivoid,

                            vigente = true
                        };
                        var archivoAvanceObraId = _archivoAvanceObraRepository.InsertAndGetId(ac);

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

        public List<ArchivosAvanceObraDto> ListaArchivos(int AvanceObraId)
        {

            var listaarhivos = _archivoAvanceObraRepository.GetAllIncluding(a => a.Archivo).ToList();

            var items = (from a in listaarhivos
                         where a.vigente == true
                         where a.AvanceObraId == AvanceObraId
                         select new ArchivosAvanceObraDto
                         {
                             Id = a.Id,
                             ArchivoId = a.ArchivoId,
                             Archivo = a.Archivo,
                             AvanceObraId = a.AvanceObraId,
                             AvanceObra = a.AvanceObra,
                             vigente = a.vigente,
                             descripcion = a.descripcion,

                         }).ToList();

            foreach (var i in items)
            {
                i.filebase64 = Convert.ToBase64String(i.Archivo.hash);
            }

            return items;
        }

        public int EditarArchivo(int ArchivoAvanceObraId, HttpPostedFileBase UploadedFile)
        {
            var archivo = _archivoAvanceObraRepository.Get(ArchivoAvanceObraId);
            var contador = this.ListaArchivos(archivo.AvanceObraId).Count + 1;
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

                    var resultado = _archivoAvanceObraRepository.Update(archivo);

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
            var archivo = _archivoAvanceObraRepository.Get(id);

            archivo.vigente = false;

            var resultado = _archivoAvanceObraRepository.Update(archivo);

            return resultado.AvanceObraId;
        }

        public ArchivosAvanceObraDto getdetallesarchivo(int id)
        {
            var listaarhivos = _archivoAvanceObraRepository.GetAllIncluding(a => a.Archivo);
            var items = (from a in listaarhivos
                         where a.vigente == true
                         where a.Id == id
                         select new ArchivosAvanceObraDto
                         {
                             Id = a.Id,
                             ArchivoId = a.ArchivoId,
                             Archivo = a.Archivo,
                             AvanceObraId = a.AvanceObraId,
                             AvanceObra = a.AvanceObra,
                             vigente = a.vigente
                         }).FirstOrDefault();

            return items;
        }


        public decimal[] MontoPresupuestadoIncrementado(int OfertaId)
        {
            var computos = _computoRepository.GetAll()
                .Where(o => o.vigente)
                .Where(o => o.Wbs.OfertaId == OfertaId).ToList();

            decimal monto = 0;
            decimal aiu = 0;

            foreach (var c in computos)
            {
                monto += (c.precio_unitario * c.cantidad);
                aiu += (c.precio_incrementado * c.cantidad);
            }

            return new[] { monto, aiu };

        }

        public bool AprobarAvanceObra(int id)
        {
            var avance = Repository.Get(id);
            avance.aprobado = true;
            Repository.Update(avance);
            return true;
        }

        public bool DesaprobarAvanceObra(int id)
        {
           

            var avance = Repository.Get(id);
            var avanceOferta = Repository.GetAllIncluding(c => c.Oferta).Where(c => c.Id == id).FirstOrDefault();

            var avanceDate = avance.fecha_presentacion.Value.Date;
            var ExisteRdoGenerado = _rdoCabeceraRepository.GetAll()
                                                          .Where(c => c.es_definitivo)
                                                          .Where(c => c.fecha_rdo > avanceDate)
                                                          .Where(c => c.vigente)
                                                          .Where(c=>c.ProyectoId== avanceOferta.Oferta.ProyectoId)
                                                          .OrderByDescending(c=>c.fecha_rdo)
                                                          .FirstOrDefault();
            if (ExisteRdoGenerado != null) {
                return false;
            }
                                                




            avance.aprobado = false;
            Repository.Update(avance);
            return true;
        }

        public string NivelPadre(Wbs wbs, int OfertaId, int nivel)
        {
            int cont = 1;
            Wbs temporal = wbs;
            while (cont <= nivel && temporal != null)
            {

                var padre = _wbsRepository.GetAll()
                    .Where(c => c.id_nivel_codigo == temporal.id_nivel_padre_codigo)
                    .Where(c => c.vigente)
                    .Where(c => c.OfertaId == OfertaId)
                    .FirstOrDefault();

                if (padre != null)
                {
                    temporal = padre;
                }
                else
                {
                    temporal = null;
                }

                cont++;
            }


            return temporal != null ? temporal.nivel_nombre : "-";
        }

        public ExcelPackage CargaMasivaAvanceObra(int id)
        {

            var cabecera = Repository.Get(id);

            ExcelPackage excel = new ExcelPackage();
            string nombretab = "Carga Masiva " + cabecera.fecha_presentacion.GetValueOrDefault().ToShortDateString();
            var workSheet = excel.Workbook.Worksheets.Add(nombretab);
            var Estructura = this.GenerarArbol(cabecera.OfertaId, cabecera.Id);
            //PRIMERA HOJA
            workSheet.TabColor = System.Drawing.Color.Navy;
            workSheet.DefaultRowHeight = 15;
            //workSheet.View.ZoomScale = 80;



            workSheet.Cells[1, 1].Value = "Id";
            workSheet.Cells[1, 2].Value = "CÓDIGO";
            workSheet.Cells[1, 2].Style.WrapText = true;
            workSheet.Cells[1, 2].Style.Font.Bold = true;
            workSheet.Cells[1, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            workSheet.Cells[1, 2].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            workSheet.Cells[1, 2].Style.Fill.PatternType = ExcelFillStyle.Solid;
            workSheet.Cells[1, 2].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(47, 117, 181));
            workSheet.Cells[1, 2].Style.Font.Color.SetColor(Color.White);
            workSheet.Cells[1, 2].Style.Border.BorderAround(ExcelBorderStyle.Medium);

            workSheet.Cells[1, 3].Value = "DESCRIPCIÓN";
            workSheet.Cells[1, 3].Style.WrapText = true;
            workSheet.Cells[1, 3].Style.Font.Bold = true;
            workSheet.Cells[1, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            workSheet.Cells[1, 3].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            workSheet.Cells[1, 3].Style.Fill.PatternType = ExcelFillStyle.Solid;
            workSheet.Cells[1, 3].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(47, 117, 181));
            workSheet.Cells[1, 3].Style.Font.Color.SetColor(Color.White);
            workSheet.Cells[1, 3].Style.Border.BorderAround(ExcelBorderStyle.Medium);

            workSheet.Cells[1, 4].Value = "UNIDAD";
            workSheet.Cells[1, 4].Style.WrapText = true;
            workSheet.Cells[1, 4].Style.Font.Bold = true;
            workSheet.Cells[1, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            workSheet.Cells[1, 4].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            workSheet.Cells[1, 4].Style.Fill.PatternType = ExcelFillStyle.Solid;
            workSheet.Cells[1, 4].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(47, 117, 181));
            workSheet.Cells[1, 4].Style.Font.Color.SetColor(Color.White);
            workSheet.Cells[1, 4].Style.Border.BorderAround(ExcelBorderStyle.Medium);

            workSheet.Cells[1, 5].Value = "CANTIDAD EAC";
            workSheet.Cells[1, 5].Style.WrapText = true;
            workSheet.Cells[1, 5].Style.Font.Bold = true;
            workSheet.Cells[1, 5].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            workSheet.Cells[1, 5].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            workSheet.Cells[1, 5].Style.Fill.PatternType = ExcelFillStyle.Solid;
            workSheet.Cells[1, 5].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(47, 117, 181));
            workSheet.Cells[1, 5].Style.Font.Color.SetColor(Color.White);
            workSheet.Cells[1, 5].Style.Border.BorderAround(ExcelBorderStyle.Medium);

            workSheet.Cells[1, 6].Value = "CANTIDAD ANTERIOR";
            workSheet.Cells[1, 6].Style.WrapText = true;
            workSheet.Cells[1, 6].Style.Font.Bold = true;
            workSheet.Cells[1, 6].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            workSheet.Cells[1, 6].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            workSheet.Cells[1, 6].Style.Fill.PatternType = ExcelFillStyle.Solid;
            workSheet.Cells[1, 6].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(47, 117, 181));
            workSheet.Cells[1, 6].Style.Font.Color.SetColor(Color.White);
            workSheet.Cells[1, 6].Style.Border.BorderAround(ExcelBorderStyle.Medium);

            workSheet.Cells[1, 7].Value = "CANTIDAD ACUMULADA";
            workSheet.Cells[1, 7].Style.WrapText = true;
            workSheet.Cells[1, 7].Style.Font.Bold = true;
            workSheet.Cells[1, 7].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            workSheet.Cells[1, 7].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            workSheet.Cells[1, 7].Style.Fill.PatternType = ExcelFillStyle.Solid;
            workSheet.Cells[1, 7].Style.Fill.BackgroundColor.SetColor(Color.OrangeRed);
            workSheet.Cells[1, 7].Style.Font.Color.SetColor(Color.White);
            workSheet.Cells[1, 7].Style.Border.BorderAround(ExcelBorderStyle.Medium);

            workSheet.Cells[1, 8].Value = "CANTIDAD AJUSTADA (ING-TOP-RL)";
            workSheet.Cells[1, 8].Style.WrapText = true;
            workSheet.Cells[1, 8].Style.Font.Bold = true;
            workSheet.Cells[1, 8].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            workSheet.Cells[1, 8].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            workSheet.Cells[1, 8].Style.Fill.PatternType = ExcelFillStyle.Solid;
            workSheet.Cells[1, 8].Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
            workSheet.Cells[1, 8].Style.Font.Color.SetColor(Color.White);
            workSheet.Cells[1, 8].Style.Border.BorderAround(ExcelBorderStyle.Medium);


            workSheet.Row(1).Height = 35;

            workSheet.View.FreezePanes(2, 1);

            //workSheet.Cells["B4:D4"].AutoFilter = true;

            workSheet.Column(1).Width = 15;
            workSheet.Column(2).Width = 12;
            workSheet.Column(3).Width = 60;
            workSheet.Column(4).Width = 12;
            workSheet.Column(5).Width = 20;
            workSheet.Column(6).Width = 20;
            workSheet.Column(7).Width = 20;
            workSheet.Column(8).Width = 20;

            workSheet.Column(1).Style.Font.Bold = true;
            workSheet.Column(1).Hidden = true;

            int c = 2;
            int final = 7;
            foreach (var i in Estructura)
            {
                string rango = "A" + c + ":H" + c;
                if (i.Color <= 255)
                {
                    workSheet.Cells[rango].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    workSheet.Cells[rango].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(i.Color, i.Color, i.Color));
                }
                else
                {
                    workSheet.Cells[rango].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    workSheet.Cells[rango].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(242, 242, 242));

                }



                workSheet.Cells[c, 1].Value = i.ComputoId;
                workSheet.Cells[c, 1].Style.WrapText = true;


                workSheet.Cells[c, 2].Value = i.Codigo;
                workSheet.Cells[c, 2].Style.WrapText = true;



                workSheet.Cells[c, 3].Value = i.Nombre;
                workSheet.Cells[c, 3].Style.WrapText = true;

                workSheet.Cells[c, 4].Value = i.UnidadMedida;
                workSheet.Cells[c, 4].Style.WrapText = true;

                if (i.Tipo == "computo")
                {

                    workSheet.Cells[rango].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    workSheet.Cells[rango].Style.Fill.BackgroundColor.SetColor(Color.White);

                    workSheet.Cells[c, 5].Value = i.CantidadEAC;
                    workSheet.Cells[c, 5].Style.WrapText = true;
                    workSheet.Cells[c, 5].Style.Numberformat.Format = "#,##0.00";

                    workSheet.Cells[c, 6].Value = i.CantidadAnterior;
                    workSheet.Cells[c, 6].Style.WrapText = true;
                    workSheet.Cells[c, 6].Style.Numberformat.Format = "#,##0.00";


                    if (i.ComputoId > 0)
                    {
                        var detalle = _detalleAvanceObraRepository.GetAll().Where(x => x.vigente)
                                                                         .Where(x => x.AvanceObraId == cabecera.Id)
                                                                         .Where(x => x.AvanceObra.vigente)
                                                                         .Where(x => x.ComputoId == i.ComputoId)
                                                                         .FirstOrDefault();
                        if (detalle != null && detalle.Id > 0)
                        {
                            workSheet.Cells[c, 7].Value = detalle.cantidad_acumulada;
                        }


                    }


                    workSheet.Cells[c, 7].Style.WrapText = true;
                    workSheet.Cells[c, 7].Style.Numberformat.Format = "#,##0.00";



                }

                if (i.Tipo == "actividad")
                {

                    workSheet.Cells[rango].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    workSheet.Cells[rango].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(180, 198, 231));

                }
                c = c + 1;
            }
            //FORMATO A UNA PAGINA
            workSheet.View.PageBreakView = true;
            //workSheet.PrinterSettings.PrintArea = workSheet.Cells[1, 1, workSheet.Dimension.End.Row, workSheet.Dimension.End.Column];
            workSheet.PrinterSettings.FitToPage = true;
            workSheet.Cells[1, 1, workSheet.Dimension.End.Row, workSheet.Dimension.End.Column].AutoFilter = true;
            return excel;

        }

        public List<AvanceObraExcel> GenerarArbol(int Id, int AvanceObraId)
        {
            List<AvanceObraExcel> principal = new List<AvanceObraExcel>();
            var wbs = _wbsRepository.GetAll().Where(c => c.vigente)
                                              .Where(c => c.OfertaId == Id)
                                              .ToList();

            var computos = _computoRepository.GetAll().Where(c => c.vigente)
                                              .Where(c => c.Wbs.vigente)
                                              .Where(c => c.Wbs.OfertaId == Id)
                                              .ToList();
            var padres = (from w in wbs
                          where w.id_nivel_padre_codigo == "."
                          select w).ToList();
            if (padres.Count > 0)
            {
                foreach (var item in padres)
                {
                    var color = 105;
                    AvanceObraExcel ao = new AvanceObraExcel()
                    {
                        ComputoId = 0,
                        Codigo = "",
                        Nombre = item.nivel_nombre,
                        Tipo = "principal",
                        UnidadMedida = "",
                        Color = color

                    };
                    principal.Add(ao);
                    var hijos = this.ObtenerHijos(wbs, computos, item.id_nivel_codigo, AvanceObraId, color + 5);
                    if (hijos.Count > 0)
                    {
                        principal.AddRange(hijos);
                    }

                }

            }
            return principal;


        }

        public List<AvanceObraExcel> ObtenerHijos(List<Wbs> data, List<Computo> computos, string codigo, int AvanceObraId, int color)
        {
            List<AvanceObraExcel> child = new List<AvanceObraExcel>();

            var hijos = (from d in data
                         where d.id_nivel_padre_codigo == codigo
                         select d).ToList();
            if (hijos.Count > 0)
            {
                foreach (var item in hijos)
                {

                    AvanceObraExcel ao = new AvanceObraExcel()
                    {
                        ComputoId = 0,
                        Codigo = "",
                        Nombre = item.nivel_nombre,
                        UnidadMedida = "",
                        Tipo = item.es_actividad ? "actividad" : "padre"
                    };

                    if (item.es_actividad)
                    {
                        ao.Color = color;
                        child.Add(ao);
                        var computoobra = this.ObtenerHijosComputos(computos, item.Id, AvanceObraId);
                        if (computoobra.Count > 0)
                        {
                            child.AddRange(computoobra);
                        }
                    }
                    else
                    {
                        child.Add(ao);
                        ao.Color = color;
                        var wbshijos = this.ObtenerHijos(data, computos, item.id_nivel_codigo, AvanceObraId, color + 5);
                        if (wbshijos.Count > 0)
                        {
                            child.AddRange(wbshijos);
                        }
                    }


                }
            }
            return child;


        }




        public List<AvanceObraExcel> ObtenerHijosComputos(List<Computo> data, int WbsId, int AvanceObraId)
        {

            List<AvanceObraExcel> child = new List<AvanceObraExcel>();

            var hijos = (from d in data
                         where d.WbsId == WbsId
                         select d).ToList();
            if (hijos.Count > 0)
            {
                var avance = Repository.Get(AvanceObraId);
                var computos = (from c in hijos
                                select new AvanceObraExcel()
                                {
                                    ComputoId = c.Id,
                                    Codigo = c.Item.codigo,
                                    Nombre = c.Item.nombre,
                                    Tipo = "computo",
                                    UnidadMedida = c.Item.Catalogo.nombre,
                                    CantidadEAC = c.cantidad_eac,
                                    CantidadAnterior = this.ObtenerCantidadAcumulada(c.Id, avance.fecha_presentacion.Value, avance.OfertaId)
                                }).ToList();

                var items_reordenados = (from e in computos
                                         orderby Convert.ToInt32(e.Codigo.ToUpper().Replace("X", "").Replace("A", "").Replace(".", ""))
                                         select e).ToList();
                if (items_reordenados.Count > 0)
                {
                    child.AddRange(computos);
                }
            }
            return child;

        }

        public int GuardarArchivo(ArchivosAvanceObraDto entityDto)
        {
            var contador = this.ListaArchivos(entityDto.AvanceObraId).Count + 1;
            string fileName = entityDto.UploadedFile.FileName;
            string fileContentType = entityDto.UploadedFile.ContentType;
            byte[] fileBytes = new byte[entityDto.UploadedFile.ContentLength];
            var data = entityDto.UploadedFile.InputStream.Read(fileBytes, 0,
            Convert.ToInt32(entityDto.UploadedFile.ContentLength));

            Archivo n = new Archivo
            {
                Id = 0,
                codigo = "AOBRA" + contador,
                nombre = fileName,
                vigente = true,
                fecha_registro = DateTime.Now,
                hash = fileBytes,
                tipo_contenido = fileContentType,
            };
            var archivoid = _archivoRepository.InsertAndGetId(n);
            var Archivo = new ArchivosAvanceObra()
            {
                ArchivoId = archivoid,
                AvanceObraId = entityDto.AvanceObraId,
                descripcion = entityDto.descripcion,
                Id = 0,
                vigente = true

            };

            var dataid = _archivoAvanceObraRepository.InsertAndGetId(Archivo);
            if (dataid > 0)
            {
                return dataid;
            }
            else
            {
                return -1;
            }
        }

        public int EditFile(int id, string descripcion)
        {
            var e = _archivoAvanceObraRepository.Get(id);
            e.descripcion = descripcion;
            var a = _archivoAvanceObraRepository.Update(e);

            return a.Id;
        }

        public List<AvanceObraExcel> GenerarArbolCargaIds(int Id)
        {
            List<AvanceObraExcel> principal = new List<AvanceObraExcel>();
            var wbs = _wbsRepository.GetAll().Where(c => c.vigente)
                                              .Where(c => c.OfertaId == Id)
                                              .ToList();

            var computos = _computoRepository.GetAllIncluding(c => c.Item).Where(c => c.vigente)
                                              .Where(c => c.Wbs.vigente)
                                              .Where(c => c.Wbs.OfertaId == Id)
                                              .Where(c => c.Item.GrupoId == 2)
                                              .ToList();
            var padres = (from w in wbs
                          where w.id_nivel_padre_codigo == "."
                          select w).ToList();
            if (padres.Count > 0)
            {
                foreach (var item in padres)
                {
                    var color = 105;
                    AvanceObraExcel ao = new AvanceObraExcel()
                    {
                        ComputoId = 0,
                        Codigo = "",
                        Nombre = item.nivel_nombre,
                        Tipo = "principal",
                        UnidadMedida = "",
                        Color = color

                    };

                    var hijos = this.ObtenerHijosIDS(wbs, computos, item.id_nivel_codigo, color + 5);
                    if (hijos.Count > 0)
                    {
                        principal.Add(ao);
                        principal.AddRange(hijos);
                    }

                }

            }
            return principal;


        }

        public List<AvanceObraExcel> ObtenerHijosIDS(List<Wbs> data, List<Computo> computos, string codigo, int color)
        {
            List<AvanceObraExcel> child = new List<AvanceObraExcel>();

            var hijos = (from d in data
                         where d.id_nivel_padre_codigo == codigo
                         select d).ToList();
            if (hijos.Count > 0)
            {
                foreach (var item in hijos)
                {

                    AvanceObraExcel ao = new AvanceObraExcel()
                    {
                        ComputoId = 0,
                        Codigo = "",
                        Nombre = item.nivel_nombre,
                        UnidadMedida = "",
                        Tipo = item.es_actividad ? "actividad" : "padre"
                    };

                    if (item.es_actividad)
                    {
                        ao.Color = color;

                        var computoobra = this.ObtenerHijosComputosIDS(computos, item.Id);
                        if (computoobra.Count > 0)
                        {
                            child.Add(ao);
                            child.AddRange(computoobra);
                        }
                    }
                    else
                    {

                        ao.Color = color;
                        var wbshijos = this.ObtenerHijosIDS(data, computos, item.id_nivel_codigo, color + 5);
                        if (wbshijos.Count > 0)
                        {
                            child.Add(ao);
                            child.AddRange(wbshijos);
                        }
                    }


                }
            }
            return child;


        }

        public List<AvanceObraExcel> ObtenerHijosComputosIDS(List<Computo> data, int WbsId)
        {

            List<AvanceObraExcel> child = new List<AvanceObraExcel>();

            var hijos = (from d in data
                         where d.WbsId == WbsId
                         select d).ToList();
            if (hijos.Count > 0)
            {
                var computos = (from c in hijos
                                select new AvanceObraExcel()
                                {
                                    ComputoId = c.Id,
                                    Codigo = c.Item.codigo,
                                    Nombre = c.Item.nombre,
                                    Tipo = "computo",
                                    //  UnidadMedida = c.Item.Catalogo.nombre,
                                    CantidadEAC = c.cantidad_eac,
                                    CantidadAnterior = 0
                                }).ToList();

                var items_reordenados = (from e in computos
                                         orderby Convert.ToInt32(e.Codigo.ToUpper().Replace("X", "").Replace("A", "").Replace(".", ""))
                                         select e).ToList();
                if (items_reordenados.Count > 0)
                {
                    child.AddRange(computos);
                }
            }
            return child;

        }

        public ExcelPackage CargaMasivaIDSWBS(int ofertaId)
        {

            var baserdo = _ofertaRepository.Get(ofertaId);

            var rdocabecera = _rdoCabeceraRepository.GetAll().Where(c => c.ProyectoId == baserdo.ProyectoId).Where(c => c.es_definitivo).OrderByDescending(c => c.fecha_rdo).FirstOrDefault();
            if (rdocabecera != null)
            {
                var datos = _rdoCabeceraService.GetRdo(rdocabecera.Id, "EAC");
                var datosAdicionales = _rdoCabeceraService.GetRdoAdicionales(rdocabecera.Id, "EAC");
                datos.AddRange(datosAdicionales);


                ExcelPackage excel = new ExcelPackage();
                string nombretab = "Carga Masiva IDS_" + baserdo.descripcion;
                var hoja = excel.Workbook.Worksheets.Add(nombretab);

                var computos = _computoRepository.GetAllIncluding(c => c.Item).Where(c => c.Wbs.OfertaId == ofertaId)
                                  .Where(c => c.vigente).ToList();
                hoja.Column(1).Hidden = true;
                hoja.Column(3).Width = 100;


                hoja.Cells["A1"].Value = baserdo.ProyectoId;
                int fila = 1;

                var proyecto = _proyectoRepository.Get(baserdo.ProyectoId);
                string celda = "C1";
                hoja.Cells[celda].Value = "PROYECTO: " + proyecto.codigo + " " + proyecto.nombre_proyecto;
                //hoja.Cells[celda].Value = "PROYECTO: " + baserdo.descripcion;
                hoja.Cells[celda].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                hoja.Cells[celda].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                hoja.Cells[celda].Style.Fill.PatternType = ExcelFillStyle.Solid;
                hoja.Cells[celda].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(47, 117, 181));
                hoja.Cells[celda].Style.Font.Color.SetColor(Color.White);
                hoja.Cells[celda].Style.Border.BorderAround(ExcelBorderStyle.Medium);

                fila++;
                celda = "A" + fila;

                hoja.Cells[celda].Value = "ID";
                hoja.Cells[celda].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                hoja.Cells[celda].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                hoja.Cells[celda].Style.Fill.PatternType = ExcelFillStyle.Solid;
                hoja.Cells[celda].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(47, 117, 181));
                hoja.Cells[celda].Style.Font.Color.SetColor(Color.White);
                hoja.Cells[celda].Style.Border.BorderAround(ExcelBorderStyle.Medium);


                celda = "B" + fila;
                hoja.Cells[celda].Value = "CÓDIGO";
                hoja.Cells[celda].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                hoja.Cells[celda].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                hoja.Cells[celda].Style.Fill.PatternType = ExcelFillStyle.Solid;
                hoja.Cells[celda].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(47, 117, 181));
                hoja.Cells[celda].Style.Font.Color.SetColor(Color.White);
                hoja.Cells[celda].Style.Border.BorderAround(ExcelBorderStyle.Medium);

                celda = "C" + fila;
                hoja.Cells[celda].Value = "ITEM";

                hoja.Cells[celda].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                hoja.Cells[celda].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                hoja.Cells[celda].Style.Fill.PatternType = ExcelFillStyle.Solid;
                hoja.Cells[celda].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(47, 117, 181));
                hoja.Cells[celda].Style.Font.Color.SetColor(Color.White);
                hoja.Cells[celda].Style.Border.BorderAround(ExcelBorderStyle.Medium);

                celda = "D" + fila;
                hoja.Cells[celda].Value = "ID RDO";
                hoja.Cells[celda].Style.Font.Bold = true;
                hoja.Cells[celda].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                hoja.Cells[celda].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                hoja.Cells[celda].Style.Fill.PatternType = ExcelFillStyle.Solid;
                hoja.Cells[celda].Style.Fill.BackgroundColor.SetColor(Color.OrangeRed);
                hoja.Cells[celda].Style.Font.Color.SetColor(Color.White);
                hoja.Cells[celda].Style.Border.BorderAround(ExcelBorderStyle.Medium);

                fila++;
                foreach (var i in datos)
                {

                    if (i.tipo == "Padre")
                    {

                        var range = "A" + fila + ":D" + fila;
                        hoja.Cells[range].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        if (i.color.Length > 0)
                        {
                            hoja.Cells[range].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml(i.color));
                        }
                        else
                        {
                            hoja.Cells[range].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(242, 242, 242));
                        }

                        if (i.principal)
                        {
                            hoja.Cells[range].Style.Font.Color.SetColor(Color.White);
                            hoja.Cells[range].Style.Font.Bold = true;
                            hoja.Cells["C" + fila].Style.Font.SetFromFont(new Font("Arial", 14, FontStyle.Bold));
                        }
                    }
                    else if (i.tipo == "Actividad")
                    {

                        var range = "A" + fila + ":D" + fila;
                        hoja.Cells[range].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        if (i.color.Length > 0)
                        {
                            hoja.Cells[range].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml(i.color));
                        }
                        else
                        {
                            hoja.Cells[range].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(242, 242, 242));
                        }
                        hoja.Cells[range].Style.Font.Color.SetColor(Color.Black);

                    }
                    celda = "A" + fila;
                    hoja.Cells[celda].Value = i.computoId == 0 ? "" : i.computoId + "";
                    celda = "B" + fila;
                    hoja.Cells[celda].Value = i.codigo_preciario;
                    celda = "C" + fila;
                    hoja.Cells[celda].Value = i.nombre_actividad;
                    hoja.Cells[celda].Style.WrapText = true;
                    if (i.computoId > 0)
                    {
                        var computo = (from comp in computos where comp.Id == i.computoId select comp).FirstOrDefault();
                        if (computo != null && computo.id_rubro_RDO != null)
                        {
                            celda = "D" + fila;
                            hoja.Cells[celda].Value = computo.id_rubro_RDO;
                        }
                    }

                    fila++;
                }

                return excel;
            }
            else
            {
                return new ExcelPackage();
            }

        }



    }
}
