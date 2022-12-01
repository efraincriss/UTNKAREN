using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using com.cpp.calypso.framework;
using com.cpp.calypso.proyecto.aplicacion.Interfaces;
using System.Web.ModelBinding;
using AutoMapper;

namespace com.cpp.calypso.proyecto.aplicacion
{

    public class ProyectoAsyncBaseCrudAppService :
        AsyncBaseCrudAppService<Proyecto, ProyectoDto, PagedAndFilteredResultRequestDto>,IProyectoAsyncBaseCrudAppService
    {
        private readonly IBaseRepository<OrdenServicio> _ordenServicioRepository;
        private readonly IBaseRepository<Catalogo> _catalogoRepository;
        private readonly IBaseRepository<Oferta> _ofertaRepository;

        private readonly IBaseRepository<OfertaComercialPresupuesto> _repositoryOCP;

        public ProyectoAsyncBaseCrudAppService(
            IBaseRepository<Proyecto> repository,
            IBaseRepository<OrdenServicio> ordenServicioRepository,
            IBaseRepository<Catalogo> catalogoRepository,
            IBaseRepository<Oferta> ofertaRepository,
            IBaseRepository<OfertaComercialPresupuesto> repositoryOCP
        ) : base(repository)
        {
            _ordenServicioRepository = ordenServicioRepository;
            _catalogoRepository = catalogoRepository;
            _ofertaRepository = ofertaRepository;
            _repositoryOCP=repositoryOCP;
    }

        public ProyectoDto GetDetalles(int proyectoId)
        {
            var ofertaComercialPresupuestos = _repositoryOCP.GetAllIncluding(c => c.OfertaComercial).Where(c => c.vigente)
                                                                 .Where(c => c.OfertaComercial != null)
                                                                 .ToList();

            var empresaQuery = Repository.GetAllIncluding(r => r.Requerimientos, r=>r.Contrato,r=>r.Contrato.Cliente);
            var item = (from p in empresaQuery
                where p.Id == proyectoId
                where p.vigente == true                    
                select new ProyectoDto()
                {
                    Id = p.Id,
                    vigente = p.vigente,
                    alcance_basico = p.alcance_basico,
                    centroCostosId = p.centroCostosId,
                    codigo = p.codigo,
                    comentarios = p.comentarios,
                    descripcion_proyecto = p.descripcion_proyecto,
                    estado_proyecto = p.estado_proyecto,
                    fecha_estimada_fin = p.fecha_estimada_fin,
                    fecha_estimada_inicio = p.fecha_estimada_inicio,
                    fecha_acta_entrega = p.fecha_acta_entrega,
                    fecha_recepcion_definitiva = p.fecha_recepcion_definitiva,
                    fecha_recepcion_provisoria = p.fecha_recepcion_provisoria,
                    monto_aprobado_orden_trabajo = p.monto_aprobado_orden_trabajo,
                    monto_certificado_orden_trabajo = p.monto_certificado_orden_trabajo,
                    monto_cobrado = p.monto_cobrado,
                    monto_facturado = p.monto_facturado,
                    monto_ofertado = p.monto_ofertado,
                    nombre_proyecto = p.nombre_proyecto,
                    presupuesto = p.presupuesto,
                    contratoId = p.contratoId,
                    Requerimientos = p.Requerimientos.Where(r => r.vigente == true).OrderBy(r => (int)(r.tipo_requerimiento)).ToList(),
                    Contrato = p.Contrato,
                    Ofertas = p.Ofertas.Where(r=>r.vigente==true).ToList(),
                    monto_aprobado_os_suministros = p.monto_aprobado_os_suministros,
                    monto_aprobado_os = p.monto_aprobado_os,
                    monto_aprobado_os_construccion = p.monto_aprobado_os_construccion,
                    monto_aprobado_os_ingenieria = p.monto_aprobado_os_ingenieria,
                    codigo_generado=p.codigo_generado,
                    responsable = p.responsable,
                    codigo_ingenieria=p.codigo_ingenieria,
                    codigo_reporte_RDO=p.codigo_reporte_RDO,
                    es_RSO=p.es_RSO,
                    locacion=p.locacion,
                    codigo_reporte_certificacion=p.codigo_reporte_certificacion,
                    codigo_cliente=p.codigo_cliente,
                    codigo_interno=p.codigo_interno,
                    orden_timesheet=p.orden_timesheet,
                    certificable_ingenieria=p.certificable_ingenieria,
                    PortafolioId=p.PortafolioId,
                    UbicacionId=p.UbicacionId,
                    ProyectoCerrado=p.ProyectoCerrado,
                    anio_certificacion_ingenieria=p.anio_certificacion_ingenieria,
                    usar_logo_prederminado=p.usar_logo_prederminado

                }).SingleOrDefault();
            decimal ingenieria = 0;
            decimal construccion = 0;
            decimal procura = 0;

            List<RequerimientoDto> query = new List<RequerimientoDto>();
            foreach (var r in item.Requerimientos)
            {
                var rdto = Mapper.Map<RequerimientoDto>(r);
                var ofertaLigada = (from ocp in ofertaComercialPresupuestos
                                    where ocp.RequerimientoId == r.Id
                                    select ocp.OfertaComercial).FirstOrDefault();

                if (ofertaLigada != null)
                {
                    rdto.ofertaComercialLigada = ofertaLigada;
                }
                query.Add(rdto);

                ingenieria += r.monto_ingenieria;
                construccion += r.monto_construccion;
                procura += r.monto_procura;
            }
            item.RequerimientosLigados = query;

            item.monto_ingenieria = ingenieria;
            item.monto_procura = procura;
            item.monto_construccion = construccion;
            item.monto_total = ingenieria + construccion + procura;
            return item;
        }

        public string GetNombreCatalogo(int CatalogoId)
        {
            var catalogo = _catalogoRepository.Get(CatalogoId).nombre;
            return catalogo;
        }

        public List<Proyecto> GetProyectos()
        {
            var proyectoQuery = Repository.GetAllIncluding(c => c.Contrato, c => c.Contrato.Cliente)
                .Where(e => e.vigente == true).ToList();
            return proyectoQuery;
        }

        public List<RequerimientoDto> RequerimientosDelProyecto(int proyectoid)
        {
            throw new NotImplementedException();
        }

        public List<ProyectoDto> Listar()
        {
            var query = Repository.GetAll()
                .Where(o => o.vigente);
            var proyectos = (from p in query
                select new ProyectoDto()
                {
                    Id = p.Id,
                    codigo = p.codigo,
                    nombre_proyecto = p.nombre_proyecto
                }).ToList();
            return proyectos;
        }

        public List<ProyectoDto> ListarCambiarProyectoRequerimiento()
        {
            var query = Repository.GetAll()
                .Where(o => o.vigente);
            var proyectos = (from p in query
                             select new ProyectoDto()
                             {
                                 Id = p.Id,
                                 codigo = p.codigo,
                                 nombre_proyecto = p.codigo+" - "+ p.nombre_proyecto
                             }).ToList();
            return proyectos;
        }


        public bool EliminarVigencia(int proyectoId)
        {
            bool resul = false;
            var proyecto = this.GetDetalles(proyectoId);
            if (proyecto != null)
            {
                if (proyecto.Requerimientos.Count > 0)
                {
                    resul = true;
                }
                else
                {
                    proyecto.vigente = false;
                    Repository.InsertOrUpdate(MapToEntity(proyecto));
                }

            }

            return resul;
        }


        public List<ProyectoDto> ObtenerProyectosPorContrato(int contratoId)
        {
            var proyectoQuery = Repository.GetAll();
            var proyectos = (from p in proyectoQuery
                where p.vigente == true
                where p.contratoId == contratoId
                select new ProyectoDto()
                {
                    Id = p.Id,
                    codigo = p.codigo,
                    descripcion_proyecto = p.descripcion_proyecto,
                    nombre_proyecto = p.nombre_proyecto
                }).ToList();
            return proyectos;
        }

        public void ActualizarMontosProyecto(int ProyectoId)
        {/*
            var proyecto = Repository.Get(ProyectoId);


            var query = _ordenServicioRepository.GetAllIncluding(o => o.OfertaComercial);
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

            Repository.Update(proyecto);*/
        }

        public bool comprobacionfechainiciofinin(DateTime fechai, DateTime fechafin)
        { if (fechai != null && fechafin != null) {
                if (fechafin > fechai)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return true;
            }

        }

        public bool comprobacionfechaacta(DateTime fechaa, DateTime fechai, DateTime fechafin)
        {
            if(fechaa!=null && fechai!=null && fechafin != null) {
            if (fechaa > fechai && fechaa > fechafin)
            {
                return true;
            }
            else {
                return false;
            }
            }
            else { 
            return true;
            }
        }

        public bool existeproyecto(string codigoproyecto)
        {
            var listaproyecto = Repository.GetAllIncluding(c => c.Contrato).Where(c => c.vigente == true)
                .Where(e => e.codigo == codigoproyecto).ToList();
            if (listaproyecto.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool existeproyectoc(ProyectoDto p)
        {
            var listaproyecto = Repository.GetAllIncluding(c => c.Contrato).Where(c => c.vigente == true)
                .Where(e => e.codigo == p.codigo && e.contratoId==p.contratoId).Where(e=>e.Id!=p.Id).ToList();
            if (listaproyecto.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool CambiarSecuencial(int proyectoId,string tipo="")
        {
          
            return true;
        }
    }
}