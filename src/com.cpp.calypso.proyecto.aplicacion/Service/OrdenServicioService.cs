using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.dominio;
using com.cpp.calypso.proyecto.dominio.Models;
using OfficeOpenXml;
using static com.cpp.calypso.proyecto.dominio.DetalleOrdenServicio;

namespace com.cpp.calypso.proyecto.aplicacion
{
    public class OrdenServicioAsyncBaseCrudAppService : AsyncBaseCrudAppService<OrdenServicio, OrdenServicioDto, PagedAndFilteredResultRequestDto>, IOrdenServicioAsyncBaseCrudAppService
    {
        private readonly IBaseRepository<DetalleOrdenServicio> _detalleOrdenServicioRepository;
        private readonly IBaseRepository<Proyecto> _proyectoRepository;
        private readonly IBaseRepository<GrupoItem> _grupoiRepository;
        private readonly IBaseRepository<OfertaComercial> _ofertacomercial;
        private readonly IDetalleOrdenServicioAsyncBaseCrudAppService _ordenServicioDetalle;

        public OrdenServicioAsyncBaseCrudAppService(
            IBaseRepository<OrdenServicio> repository,
            IBaseRepository<DetalleOrdenServicio> detalleOrdenServicioRepository,
            IBaseRepository<Proyecto> proyectoRepository,
            IDetalleOrdenServicioAsyncBaseCrudAppService ordenServicioDetalle,
            IBaseRepository<OfertaComercial> ofertacomercial,
            IBaseRepository<GrupoItem> grupoiRepository
            ) : base(repository)
        {
            _detalleOrdenServicioRepository = detalleOrdenServicioRepository;
            _proyectoRepository = proyectoRepository;
            _ordenServicioDetalle = ordenServicioDetalle;
            _ofertacomercial = ofertacomercial;
            _grupoiRepository = grupoiRepository;
        }

        //genera datos para la cabecera de visualizacion de OS
        public OrdenServicioDto llenarCabecera(int ofertaId)
        {
            OrdenServicioDto orden = new OrdenServicioDto();
            var ordenQuery = Repository.GetAllIncluding(o => o.Estado);
            var item = (from o in ordenQuery
                            //  where o.OfertaComercialId == ofertaId
                        where o.vigente == true
                        select new OrdenServicioDto()
                        {
                            Id = o.Id,
                            //   OfertaComercial = o.OfertaComercial,
                            vigente = o.vigente,
                            // OfertaComercialId = o.OfertaComercialId,
                            fecha_orden_servicio = o.fecha_orden_servicio,
                            monto_aprobado_os = o.monto_aprobado_os,
                            codigo_orden_servicio = o.codigo_orden_servicio,
                            version_os = o.version_os,
                            monto_aprobado_suministros = o.monto_aprobado_suministros,
                            monto_aprobado_construccion = o.monto_aprobado_construccion,
                            monto_aprobado_ingeniería = o.monto_aprobado_ingeniería,
                            ValorTotal = (o.monto_aprobado_suministros + o.monto_aprobado_construccion + o.monto_aprobado_ingeniería)
                        }).ToList();

            foreach (var i in item)
            {
                orden.monto_presupuestado += i.monto_aprobado_suministros;
                orden.monto_ingenieria += i.monto_aprobado_ingeniería;
                orden.monto_construccion += i.monto_aprobado_construccion;
                orden.total += (i.monto_aprobado_suministros + i.monto_aprobado_ingeniería + i.monto_aprobado_construccion);
                List<DetalleOrdenServicioDto> od = _ordenServicioDetalle.listar(i.Id);
                foreach (var id in od)
                {
                    if (id.GrupoItemId.ToString().Equals("Ingeniería"))
                        orden.oSmonto_ingenieria += id.valor_os;
                    if (id.GrupoItemId.ToString().Equals("Construcción"))
                        orden.oSmonto_construccion += id.valor_os;
                    if (id.GrupoItemId.ToString().Equals("Suministros"))
                        orden.oSmonto_presupuestado += id.valor_os;
                    orden.oStotal = (orden.oSmonto_ingenieria + orden.oSmonto_construccion + orden.oSmonto_presupuestado);
                }
                if (orden.monto_presupuestado != 0)
                    orden.Pmonto_presupuestado = (orden.oSmonto_presupuestado / orden.monto_presupuestado);
                if (orden.monto_ingenieria != 0)
                    orden.Pmonto_ingenieria = (orden.oSmonto_ingenieria / orden.monto_ingenieria);
                if (orden.monto_construccion != 0)
                    orden.Pmonto_construccion = (orden.oSmonto_construccion / orden.monto_construccion);
                orden.Ptotal = (orden.Pmonto_presupuestado + orden.Pmonto_ingenieria + orden.Pmonto_construccion);
            }
            return orden;
        }

        public List<OrdenServicioDto> listar(int ofertaId)
        {
            var ordenQuery = Repository.GetAllIncluding(o => o.Estado);
            var item = (from o in ordenQuery
                            // where o.OfertaComercialId == ofertaId
                        where o.vigente == true
                        select new OrdenServicioDto()
                        {
                            Id = o.Id,
                            //  OfertaComercial = o.OfertaComercial,
                            vigente = o.vigente,
                            // OfertaComercialId = o.OfertaComercialId,
                            fecha_orden_servicio = o.fecha_orden_servicio,
                            monto_aprobado_os = o.monto_aprobado_os,
                            codigo_orden_servicio = o.codigo_orden_servicio,
                            version_os = o.version_os,
                            monto_aprobado_suministros = o.monto_aprobado_suministros,
                            monto_aprobado_construccion = o.monto_aprobado_construccion,
                            monto_aprobado_ingeniería = o.monto_aprobado_ingeniería,
                            ValorTotal = (o.monto_aprobado_suministros + o.monto_aprobado_construccion + o.monto_aprobado_ingeniería)
                        }).ToList();


            return item;
        }


        public List<OrdenServicioDto> ListarPorProyecto(int proyectoId)
        {

            var query = _detalleOrdenServicioRepository.GetAllIncluding(o => o.OrdenServicio)
                .Where(o => o.vigente == true)
                .Where(o => o.OrdenServicio.vigente)
                .Where(o => o.ProyectoId == proyectoId).ToList();

            List<OrdenServicioDto> result = query
            .GroupBy(l => l.OrdenServicio.codigo_orden_servicio)
            .Select(cl => new OrdenServicioDto
            {
                Id = cl.First().Id,
                codigo_orden_servicio = cl.First().OrdenServicio.codigo_orden_servicio,
                fecha_orden_servicio = cl.First().OrdenServicio.fecha_orden_servicio,
                monto_aprobado_ingeniería = cl.Where(c => c.GrupoItemId == GrupoItems.Ingeniería).Sum(c => c.valor_os),
                monto_aprobado_construccion = cl.Where(c => c.GrupoItemId == GrupoItems.Construcción).Sum(c => c.valor_os),
                monto_aprobado_suministros = cl.Where(c => c.GrupoItemId == GrupoItems.Suministros).Sum(c => c.valor_os),
                monto_aprobado_subcontrato = cl.Where(c => c.GrupoItemId == GrupoItems.SubContratos).Sum(c => c.valor_os),
                monto_aprobado_os = cl.Where(c => c.GrupoItemId == GrupoItems.Ingeniería).Sum(c => c.valor_os) + cl.Where(c => c.GrupoItemId == GrupoItems.Construcción).Sum(c => c.valor_os) + cl.Where(c => c.GrupoItemId == GrupoItems.Suministros).Sum(c => c.valor_os) + cl.Where(c => c.GrupoItemId == GrupoItems.SubContratos).Sum(c => c.valor_os),
            }).ToList();

            /*var items = (from o in query
                         select new OrdenServicioDto()
                         {
                             Id = o.Id,
                             codigo_orden_servicio = o.codigo_orden_servicio,
                             monto_aprobado_construccion = o.monto_aprobado_construccion,
                             monto_aprobado_ingeniería = o.monto_aprobado_ingeniería,
                             fecha_orden_servicio = o.fecha_orden_servicio,
                             monto_aprobado_os = o.monto_aprobado_os,
                             monto_aprobado_suministros = o.monto_aprobado_suministros
                         }).ToList();
                         */
            return result;

        }



        public List<OrdenServicioDto> ListarProyectoDetalles(int proyectoId)
        {

            var query = _detalleOrdenServicioRepository.GetAllIncluding(o => o.OrdenServicio)
                .Where(o => o.vigente == true)
                .Where(o => o.ProyectoId == proyectoId).Select(c => c.OrdenServicio).Distinct().ToList();

            var montos = _detalleOrdenServicioRepository.GetAllIncluding(o => o.OrdenServicio)
               .Where(o => o.vigente == true)
               .Where(o => o.ProyectoId == proyectoId).ToList();
            var items = (from o in query
                         select new OrdenServicioDto()
                         {
                             Id = o.Id,
                             codigo_orden_servicio = o.codigo_orden_servicio,
                             monto_aprobado_construccion = (from m in montos
                                                            where m.GrupoItemId == DetalleOrdenServicio.GrupoItems.Construcción
                                                            where m.OrdenServicioId == o.Id
                                                            select m.valor_os).ToList().Sum(),
                             monto_aprobado_ingeniería = (from m in montos
                                                          where m.GrupoItemId == DetalleOrdenServicio.GrupoItems.Ingeniería
                                                          where m.OrdenServicioId == o.Id
                                                          select m.valor_os).ToList().Sum(),
                             fecha_orden_servicio = o.fecha_orden_servicio,

                             monto_aprobado_suministros = (from m in montos
                                                           where m.GrupoItemId == DetalleOrdenServicio.GrupoItems.Suministros
                                                           where m.OrdenServicioId == o.Id
                                                           select m.valor_os).ToList().Sum(),

                         }).ToList();
            foreach (var item in items)
            {
                item.monto_aprobado_os = item.monto_aprobado_suministros + item.monto_aprobado_construccion + item.monto_aprobado_ingeniería;
            }

            return items.Distinct().ToList();

        }

        public int EliminarVigencia(int ordenServicioId)
        {
            var orden = Repository.Get(ordenServicioId);
            if (orden != null)
            {
                orden.vigente = false;
                Repository.Update(orden);
                return orden.Id;
            }

            return 0;
        }

        public void ActualizarMontosOrdenServicio(int ordenServicioId)
        {
            var orden = Repository.Get(ordenServicioId);

            // Obtengo todos los detalles de la orden de servicio
            var query = _detalleOrdenServicioRepository.GetAll();
            var detalles = (from d in query
                            where d.vigente == true
                            where d.OrdenServicioId == ordenServicioId
                            select new DetalleOrdenServicioDto()
                            {
                                GrupoItemId = d.GrupoItemId,
                                valor_os = d.valor_os
                            }).ToList();

            // Sumo los diferentes montos de los detalles
            decimal monto_os = 0;
            decimal ingenieria = 0;
            decimal suministros = 0;
            decimal construccion = 0;
            foreach (var d in detalles)
            {
                if (d.GrupoItemId == DetalleOrdenServicio.GrupoItems.Ingeniería)
                {
                    ingenieria += d.valor_os;
                    monto_os += d.valor_os;
                }
                else if (d.GrupoItemId == DetalleOrdenServicio.GrupoItems.Construcción)
                {
                    construccion += d.valor_os;
                    monto_os += d.valor_os;
                }
                else if (d.GrupoItemId == DetalleOrdenServicio.GrupoItems.Suministros)
                {
                    suministros += d.valor_os;
                    monto_os += d.valor_os;
                }
            }


            // Actualizo la orden de servicio
            orden.monto_aprobado_os = monto_os;
            orden.monto_aprobado_construccion = construccion;
            orden.monto_aprobado_ingeniería = ingenieria;
            orden.monto_aprobado_suministros = suministros;


            Repository.Update(orden);

            /* this.ActualizarMontosProyecto(orden.OfertaComercial.ProyectoId);*/

        }

        public void ActualizarMontosProyecto(int ProyectoId)
        {/*
            var proyecto = _proyectoRepository.Get(ProyectoId);


            var query = Repository.GetAllIncluding(o => o.Oferta);
            var ordenes = (from o in query
                where o.vigente == true
                where o.Oferta.ProyectoId == ProyectoId
                select new OrdenServicioDto()
                {
                    Id = o.Id,
                    monto_aprobado_construccion = o.monto_aprobado_construccion,
                    monto_aprobado_ingeniería = o.monto_aprobado_ingeniería,
                    monto_aprobado_os = o.monto_aprobado_os,
                    monto_aprobado_suministros = o.monto_aprobado_suministros,

                }).ToList();

            decimal monto_os = 0;
            decimal ingenieria = 0;
            decimal suministros = 0;
            decimal construccion = 0;
            foreach (var d in ordenes)
            {
                monto_os += d.monto_aprobado_os;
                ingenieria += d.monto_aprobado_ingeniería;
                suministros += d.monto_aprobado_suministros;
                construccion += d.monto_aprobado_construccion;
            }


            // Actualizo la orden de servicio
            proyecto.monto_aprobado_os = monto_os;
            proyecto.monto_aprobado_os_construccion = construccion;
            proyecto.monto_aprobado_os_ingenieria = ingenieria;
            proyecto.monto_aprobado_os_suministros = suministros;

            _proyectoRepository.Update(proyecto);
            */
        }

        public List<OrdenServicio> ListaOSOfertaComercial(int Id)
        {
            var lista = Repository.GetAllIncluding(c => c.Estado)

                .Where(c => c.vigente == true).Where(c => c.EstadoId == Id)
                .ToList();
            return lista;
        }

        public int CrearOs(OrdenServicio ordenservicio)
        {
            var nuevo = Repository.InsertAndGetId(ordenservicio);



            return nuevo;
        }

        public List<OrdenServicio> Listar()
        {
            var orden = Repository.GetAllIncluding(c => c.Estado)
                                .Where(c => c.vigente)
                                .ToList();

            return orden;
        }

        public OrdenServicio Detalles(int Id)
        {
            var os = Repository.Get(Id);
            return os;
        }

        public string InsertOrden(OrdenServicio o)
        {
            var e = Repository.GetAll().Where(c => c.vigente)
                                     .Where(c => c.codigo_orden_servicio == o.codigo_orden_servicio)
                                     .ToList();
            if (e.Count == 0)
            {
                o.version_os = "A";
                var i = Repository.Insert(o);
                return "OK";

            }
            else
            {
                return "CODE_EXIST";
            }

        }

        public string EditOrden(OrdenServicio o)
        {
            var e = Repository.GetAll().Where(c => c.vigente)
                                  .Where(c => c.codigo_orden_servicio == o.codigo_orden_servicio)
                                  .Where(c => c.Id != o.Id)
                                  .ToList();
            if (e.Count == 0)
            {
                var x = Repository.Get(o.Id);
                x.codigo_orden_servicio = o.codigo_orden_servicio;
                x.fecha_orden_servicio = o.fecha_orden_servicio;
                x.anio = o.anio;
                x.comentarios = o.comentarios;
                x.referencias_po = o.referencias_po;
                x.EstadoId = o.EstadoId;
                if (o.ArchivoId != null)
                {
                    x.ArchivoId = o.ArchivoId;
                }
                var i = Repository.Update(x);
                return "OK";

            }
            else
            {
                return "CODE_EXIST";
            }

        }

        public string DeleteOrden(int id)
        {
            var e = Repository.Get(id);
            Repository.Delete(e);
            return "OK";
        }

        public List<OrdenServicioModel> GetLista()
        {
            var detalles = _detalleOrdenServicioRepository.GetAllList();

            var query = Repository.GetAllIncluding(c => c.Estado).Where(c => c.vigente).ToList();
            var list = (from q in query
                        select new OrdenServicioModel()
                        {
                            Id = q.Id,
                            codigo_orden_servicio = q.codigo_orden_servicio,
                            EstadoId = q.EstadoId,
                            nombreEstado = q.Estado.nombre,
                            version_os = q.version_os,
                            monto_aprobado_construccion = q.monto_aprobado_construccion,
                            monto_aprobado_ingeniería = q.monto_aprobado_ingeniería,
                            monto_aprobado_os = q.monto_aprobado_os,
                            monto_aprobado_subcontrato = q.monto_aprobado_subcontrato,
                            monto_aprobado_suministros = q.monto_aprobado_suministros,
                            fecha_orden_servicio = q.fecha_orden_servicio,
                            formatFechaOs = q.fecha_orden_servicio.ToShortDateString(),
                            ofertasComerciales = String.Join(";", detalles.Where(c => c.OrdenServicioId == q.Id).Select(c => c.OfertaComercial.codigo)),
                            tieneArchivo = q.ArchivoId.HasValue ? true : false,
                            ArchivoId = q.ArchivoId,
                            anio=q.anio,
                            referencias_po=q.referencias_po,
                            comentarios=q.comentarios,
                            
                        }).OrderByDescending(c => c.fecha_orden_servicio).ToList();

            return list;
        }

        public List<ModelClassReact> ListProyectos()
        {
            var query = _proyectoRepository.GetAll().Where(c => c.vigente).ToList();
            var list = (from q in query
                        select new ModelClassReact
                        {
                            dataKey = q.Id,
                            label = q.codigo + " - " + q.nombre_proyecto,
                            value = q.Id

                        }).ToList();
            return list;
        }

        public List<ModelClassReact> ListOfertas()
        {
            var query = _ofertacomercial.GetAll().Where(c => c.vigente).OrderByDescending(c => c.codigo)
                                        .ToList();

            var list = (from q in query
                        select new ModelClassReact
                        {
                            dataKey = q.Id,
                            label = q.codigo + " - " + q.version,
                            value = q.Id

                        }).ToList();
            return list;
        }

        public List<ModelClassReact> ListGrupoItem()
        {
            var query = _grupoiRepository.GetAll().Where(c => c.vigente).ToList();

            var list = (from q in query
                        select new ModelClassReact
                        {
                            dataKey = q.Id,
                            label = q.descripcion,
                            value = q.Id

                        }).ToList();
            return list;
        }

        public List<DetalleOrdenServicioDto> ListDetallesByOrden(int Id)
        {
            var query = _detalleOrdenServicioRepository
                                   .GetAllIncluding(c => c.OfertaComercial,
                                                    c => c.Proyecto)
                                   .Where(c => c.vigente)
                                   .Where(c => c.OrdenServicioId == Id)
                                   .ToList();
            var list = (from q in query
                        select new DetalleOrdenServicioDto()
                        {
                            Id = q.Id,
                            OfertaComercialId = q.OfertaComercialId,
                            codigoOferta = q.OfertaComercial.codigo+"_"+q.OfertaComercial.version,
                            ProyectoId = q.ProyectoId,
                            codigo_proyecto = q.Proyecto.codigo,
                            nombre_proyecto = q.Proyecto.nombre_proyecto,
                            OrdenServicioId = q.OrdenServicioId,
                            valor_os = q.valor_os,
                            GrupoItemId = q.GrupoItemId,
                            
                            nombre_grupo = Enum.GetName(typeof(DetalleOrdenServicio.GrupoItems), q.GrupoItemId)
                        }).ToList();

            return list;
        }

        public string InsertDetalleOrden(DetalleOrdenServicio o)
        {
            try
            {
                var iddetalle = _detalleOrdenServicioRepository.InsertAndGetId(o);
                //var update = this.UpdateMontosOs(o.OrdenServicioId);

                return "OK";
            }
            catch (Exception e)
            {
                return e.Message.ToString();
            }
        }

        public string EditDetalleOrden(DetalleOrdenServicio o)
        {
            var e = _detalleOrdenServicioRepository.Get(o.Id);
            e.ProyectoId = o.ProyectoId;
            e.OfertaComercialId = o.OfertaComercialId;
            e.valor_os = o.valor_os;
            e.GrupoItemId = o.GrupoItemId;
            try
            {
                _detalleOrdenServicioRepository.Update(e);
                //var update = this.UpdateMontosOs(o.OrdenServicioId);
                return "OK";
            }
            catch (Exception x)
            {

                return x.Message;
            }


        }

        public string DeleteDetalleOrden(int id)
        {

            _detalleOrdenServicioRepository.Delete(id);

            return "OK";
        }

        public bool UpdateMontosOs(int Id)
        {
            var o = Repository.Get(Id);
            var detalles = this.ListDetallesByOrden(Id);
            decimal i = (from d in detalles
                         where d.GrupoItemId == GrupoItems.Ingeniería
                         select d.valor_os).Sum();
            decimal c = (from d in detalles
                         where d.GrupoItemId == GrupoItems.Construcción
                         select d.valor_os).Sum();
            decimal s = (from d in detalles
                         where d.GrupoItemId == GrupoItems.Suministros
                         select d.valor_os).Sum();
            decimal sub = (from d in detalles
                           where d.GrupoItemId == GrupoItems.SubContratos
                           select d.valor_os).Sum();

            decimal total = i + c + s + sub;

            o.monto_aprobado_ingeniería = i;
            o.monto_aprobado_construccion = c;
            o.monto_aprobado_subcontrato = sub;
            o.monto_aprobado_suministros = s;
            o.monto_aprobado_os = total;
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

        public OrdenServicioModel GetOSDetalle(int Id)
        {
            var detalles = _detalleOrdenServicioRepository.GetAllIncluding(c => c.OfertaComercial).Where(c => c.OrdenServicioId == Id).ToList();

            var q = Repository.GetAllIncluding(c => c.Estado).Where(c => c.vigente)
               .Where(c => c.Id == Id).FirstOrDefault();

            var list = new OrdenServicioModel()
            {
                Id = q.Id,
                codigo_orden_servicio = q.codigo_orden_servicio,
                EstadoId = q.EstadoId,
                nombreEstado = q.Estado.nombre,
                version_os = q.version_os,
                monto_aprobado_construccion = q.monto_aprobado_construccion,
                monto_aprobado_ingeniería = q.monto_aprobado_ingeniería,
                monto_aprobado_os = q.monto_aprobado_os,
                monto_aprobado_subcontrato = q.monto_aprobado_subcontrato,
                monto_aprobado_suministros = q.monto_aprobado_suministros,
                fecha_orden_servicio = q.fecha_orden_servicio,
                formatFechaOs = q.fecha_orden_servicio.ToShortDateString(),
                ofertasComerciales = String.Join(";", detalles.Where(c => c.OrdenServicioId == q.Id).Select(c => c.OfertaComercial.codigo)),
                tieneArchivo = q.ArchivoId.HasValue ? true : false,
                ArchivoId = q.ArchivoId,
                anio=q.anio,
                referencias_po=q.referencias_po,
                comentarios=q.comentarios
            };

            return list;
        }

        public List<OrdenServicioDto> ListarOsByOferta(int OfertaId)
        {

            var query = _detalleOrdenServicioRepository.GetAllIncluding(o => o.OrdenServicio)
                .Where(o => o.vigente == true)
                .Where(o => o.OrdenServicio.vigente)
                .Where(o => o.OfertaComercialId == OfertaId).ToList();

            List<OrdenServicioDto> result = query
            .GroupBy(l => l.OrdenServicio.codigo_orden_servicio)
            .Select(cl => new OrdenServicioDto
            {
                Id = cl.First().Id,
                codigo_orden_servicio = cl.First().OrdenServicio.codigo_orden_servicio,
                fecha_orden_servicio = cl.First().OrdenServicio.fecha_orden_servicio,
                monto_aprobado_ingeniería = cl.Where(c => c.GrupoItemId == GrupoItems.Ingeniería).Sum(c => c.valor_os),
                monto_aprobado_construccion = cl.Where(c => c.GrupoItemId == GrupoItems.Construcción).Sum(c => c.valor_os),
                monto_aprobado_suministros = cl.Where(c => c.GrupoItemId == GrupoItems.Suministros).Sum(c => c.valor_os),
                monto_aprobado_subcontrato = cl.Where(c => c.GrupoItemId == GrupoItems.SubContratos).Sum(c => c.valor_os),
                monto_aprobado_os = cl.Where(c => c.GrupoItemId == GrupoItems.Ingeniería).Sum(c => c.valor_os) + cl.Where(c => c.GrupoItemId == GrupoItems.Construcción).Sum(c => c.valor_os) + cl.Where(c => c.GrupoItemId == GrupoItems.Suministros).Sum(c => c.valor_os) + cl.Where(c => c.GrupoItemId == GrupoItems.SubContratos).Sum(c => c.valor_os),
            }).ToList();

            return result;
        }

        public ExcelPackage ReportePOS(ReportDto r)
        {
            ExcelPackage excel = new ExcelPackage();

            string filename = System.Web.HttpContext.Current.Server.MapPath("~/Views/PlantillaWord/ReporteOS.xlsx");
            if (File.Exists((string)filename))
            {
                FileInfo newFile = new FileInfo(filename);
                ExcelPackage pck = new ExcelPackage(newFile);
                excel.Workbook.Worksheets.Add("Ordenes Servicio", pck.Workbook.Worksheets[1]);
            }
            ExcelWorksheet h = excel.Workbook.Worksheets[1];

            var proyectosquery = _detalleOrdenServicioRepository.GetAll().Where(c => c.vigente)
                                                                  .Where(c => c.Proyecto.vigente)
                                                                  .OrderBy(c => c.Proyecto.codigo)
                                                                  .Select(c => c.Proyecto).Distinct().ToList();
            int count = 3;
            string cell = "";
            foreach (var p in proyectosquery)
            {
                var query = _detalleOrdenServicioRepository.GetAllIncluding(c => c.OrdenServicio, c => c.OfertaComercial)
                                                                  .Where(c => c.vigente)
                                                                  .Where(c => c.OrdenServicio.vigente)
                                                                  .Where(c => c.OfertaComercial.vigente)
                                                                  //.Where(c => c.OfertaComercial.es_final == 1)
                                                                  .Where(c => c.ProyectoId == p.Id)
                                                                  .OrderBy(c => c.OrdenServicio.anio)
                                                                  .ToList();

                List<OrdenServicioDto> result = query
                .GroupBy(l => l.OrdenServicio.codigo_orden_servicio)
                .Select(cl => new OrdenServicioDto
                {
                    Id = cl.First().Id,
                    codigo_orden_servicio = cl.First().OrdenServicio.codigo_orden_servicio,
                    comentarios=cl.First().OrdenServicio.comentarios,
                    fecha_orden_servicio = cl.First().OrdenServicio.fecha_orden_servicio,
                    monto_aprobado_ingeniería = cl.Where(c => c.GrupoItemId == GrupoItems.Ingeniería).Sum(c => c.valor_os),
                    monto_aprobado_construccion = cl.Where(c => c.GrupoItemId == GrupoItems.Construcción).Sum(c => c.valor_os),
                    monto_aprobado_suministros = cl.Where(c => c.GrupoItemId == GrupoItems.Suministros).Sum(c => c.valor_os),
                    monto_aprobado_subcontrato = cl.Where(c => c.GrupoItemId == GrupoItems.SubContratos).Sum(c => c.valor_os),
                    monto_aprobado_os = cl.Where(c => c.GrupoItemId == GrupoItems.Ingeniería).Sum(c => c.valor_os) + cl.Where(c => c.GrupoItemId == GrupoItems.Construcción).Sum(c => c.valor_os) + cl.Where(c => c.GrupoItemId == GrupoItems.Suministros).Sum(c => c.valor_os) + cl.Where(c => c.GrupoItemId == GrupoItems.SubContratos).Sum(c => c.valor_os),
                    anio=cl.First().OrdenServicio.anio,
                    ref_oferta = String.Join(",", cl.Select(c => c.OfertaComercial.codigo + "_" + c.OfertaComercial.version).Distinct().ToList()),

                }).ToList();

                foreach (var d in result)
                {
                    cell = "A" + count;
                    h.Cells[cell].Value = p.codigo;
                    h.Cells[cell].Style.Font.Bold = true;
                    h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    h.Cells[cell].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                    h.Cells[cell].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Justify;
                    cell = "B" + count;
                    h.Cells[cell].Value = d.anio;
                    h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    h.Cells[cell].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                    h.Cells[cell].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Justify;
                    cell = "C" + count;
                    h.Cells[cell].Value = d.codigo_orden_servicio;
                    h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    h.Cells[cell].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                    h.Cells[cell].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Justify;
                    cell = "D" + count;
                    h.Cells[cell].Value = d.fecha_orden_servicio;
                    h.Cells[cell].Style.Numberformat.Format = "dd-mmm-yyyy";
                    h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    h.Cells[cell].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                    h.Cells[cell].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Justify;
                    cell = "E" + count;
                    h.Cells[cell].Value = d.monto_aprobado_os;
                    h.Cells[cell].Style.Font.Bold = true;
                    h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    h.Cells[cell].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                    h.Cells[cell].Style.Numberformat.Format = "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";
                    h.Cells[cell].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Justify;
                    cell = "F" + count;
                    h.Cells[cell].Value = d.monto_aprobado_ingeniería;
                    h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    h.Cells[cell].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                    h.Cells[cell].Style.Numberformat.Format = "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";
                    h.Cells[cell].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Justify;
                    cell = "G" + count;
                    h.Cells[cell].Value = d.monto_aprobado_construccion;
                    h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    h.Cells[cell].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                    h.Cells[cell].Style.Numberformat.Format = "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";
                    h.Cells[cell].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Justify;
                    cell = "H" + count;
                    h.Cells[cell].Value = d.monto_aprobado_suministros;
                    h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    h.Cells[cell].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                    h.Cells[cell].Style.Numberformat.Format = "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";
                    h.Cells[cell].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Justify;
                    cell = "I" + count;
                    h.Cells[cell].Value = d.monto_aprobado_subcontrato;
                    h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    h.Cells[cell].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                    h.Cells[cell].Style.Numberformat.Format = "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";
                    h.Cells[cell].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Justify;
                    cell = "J" + count;
                    h.Cells[cell].Value = d.ref_oferta;
                    h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    h.Cells[cell].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                    h.Cells[cell].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Justify;
                    cell = "K" + count;
                    h.Cells[cell].Value = d.comentarios;
                    h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    h.Cells[cell].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                    h.Cells[cell].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Justify;
                    count++;
                }
            }


            return excel;
        }

    }
}
