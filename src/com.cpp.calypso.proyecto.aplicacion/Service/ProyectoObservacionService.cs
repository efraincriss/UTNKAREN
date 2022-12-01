using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.aplicacion.Dto;
using com.cpp.calypso.proyecto.dominio;

namespace com.cpp.calypso.proyecto.aplicacion
{
    public class ProyectoObservacionAsyncBaseCrudAppService : AsyncBaseCrudAppService<ProyectoObservacion, ProyectoObservacionDto, PagedAndFilteredResultRequestDto>, IProyectoObservacionAsyncBaseCrudAppService
    {
        private readonly IBaseRepository<Proyecto> _proyectoRepository;
        private readonly IBaseRepository<Catalogo> _catalogoRepository;
        private readonly IBaseRepository<Precipitacion> _precipitacionRepository;
        public ProyectoObservacionAsyncBaseCrudAppService(
            IBaseRepository<ProyectoObservacion> repository,
                        IBaseRepository<Proyecto> proyectoRepository,
            IBaseRepository<Catalogo> catalogoRepository,
            IBaseRepository<Precipitacion> precipitacionRepository
            ) : base(repository)
        {
            _proyectoRepository = proyectoRepository;
            _catalogoRepository = catalogoRepository;
            _precipitacionRepository = precipitacionRepository;
        }

        public string CambiarRDOaRSO(int ProyectoId,bool esRSO)
        {
            var p = _proyectoRepository.Get(ProyectoId);
            p.es_RSO = esRSO;
            _proyectoRepository.Update(p);
            return "OK";
        }

        public Proyecto DetallesProyecto(int Id)
    {
        var proyecto = _proyectoRepository.Get(Id);
        return proyecto;
    }

        public int EditarPrecipitacion(Precipitacion precipitacion)
        {
            var cantidadanterior = _precipitacionRepository.GetAll().Where(c => c.ProyectoId == precipitacion.ProyectoId)
                                                                     .Where(c => c.Fecha <= precipitacion.Fecha)
                                                                     .Where(c=>c.vigente)
                                                                      .Where(c => c.Tipo == precipitacion.Tipo)
                                                                     .ToList().Select(c => c.CantidadDiaria).Sum();
            precipitacion.CantidadAnterior = cantidadanterior;
            precipitacion.CantidadAcumulada = cantidadanterior + precipitacion.CantidadDiaria;

            var e = _precipitacionRepository.Get(precipitacion.Id);
            e.Fecha = precipitacion.Fecha;
            e.Hora_inicio = precipitacion.Hora_inicio;
            e.Hora_fin = precipitacion.Hora_fin;
            e.Tipo = e.Tipo;
            e.CantidadDiaria = e.CantidadAcumulada;
            e.vigente = true;
            var update = _precipitacionRepository.Update(e);
            return update.Id;
        }

        public int Eliminar(int ObservacionId)
    {
        var observacion = Repository.Get(ObservacionId);
        if (observacion != null)
        {
            observacion.vigente = false;
            Repository.Update(observacion);
            return observacion.ProyectoId;
        }

        return 0;
    }

        public int EliminarPrecipitacion(int Id)
        {
            _precipitacionRepository.Delete(Id);
            return 1;
        }

        public List<ProyectoObservacionDto> ListarPorProyecto(int ProyectoId)
    {
        var query = Repository.GetAll()
            .Where(o => o.vigente)
            .Where(o => o.ProyectoId == ProyectoId);
        var items = (from i in query
                     select new ProyectoObservacionDto()
                     {
                         Id = i.Id,
                         Proyecto = i.Proyecto,
                         FechaObservacion = i.FechaObservacion,
                         Observacion = i.Observacion,
                         ProyectoId = i.ProyectoId
                     }).ToList();

        return items;
    }

    public List<ProyectoObservacionDto> ListarPorProyectoTipo(int ProyectoId, TipoComentario Tipo)
    {
        var query = Repository.GetAllIncluding(c=>c.TipoObservacion).Where(o => o.vigente).Where(o => o.ProyectoId == ProyectoId).Where(c => c.vigente)

                                                             .Where(o => o.Tipo == Tipo).ToList();
                                
        var items = (from i in query
                     select new ProyectoObservacionDto()
                     {
                         Id = i.Id,
                         ProyectoId = i.ProyectoId,
                         NombreProyecto = i.Proyecto.nombre_proyecto,
                         FechaObservacion = i.FechaObservacion,
                         Observacion = i.Observacion,
                         FormatFecha = i.FechaObservacion.ToShortDateString(),
                         NombreTipoObservacion = i.TipoObservacion.nombre,
                         TipoObservacionId = i.TipoObservacionId,
                         vigente = i.vigente,
                         Tipo=i.Tipo
                     }).ToList();

        return items;
    }

        public List<PrecipitacionDto> ListarPrecipiatacionesPorProyecto(int ProyectoId)
        {
            var query = _precipitacionRepository.GetAllIncluding(c=>c.Proyecto).Where(c => c.vigente).Where(c=>c.ProyectoId== ProyectoId).ToList();
            var Lista = (from l in query
                         select new PrecipitacionDto()
                         {
                             Id = l.Id,
                             nombreproyecto = l.Proyecto.nombre_proyecto,
                             CantidadDiaria = l.CantidadDiaria,
                             CantidadAnterior = l.CantidadAnterior,
                             CantidadAcumulada = l.CantidadAcumulada,
                             fechaformat = l.Fecha.ToShortDateString(),
                             horafinformat = l.Hora_fin.ToString(),
                             horainicioformat = l.Hora_inicio.ToString(),
                             nombretipo = Enum.GetName(typeof(TipoPrecipitacion), l.Tipo),
                             vigente = l.vigente

                         }).ToList();
                  
            return Lista;
        }

        public int NuevaPrecipitacion(Precipitacion nueva)
        {
           var cantidadanterior = _precipitacionRepository.GetAll().Where(c => c.ProyectoId == nueva.ProyectoId)
                                                                    .Where(c=>c.Fecha<=nueva.Fecha)
                                                                    .Where(c=>c.Tipo==nueva.Tipo)
                                                                    .ToList().Select(c => c.CantidadDiaria).Sum();
            nueva.CantidadAnterior = cantidadanterior;
            nueva.CantidadAcumulada = cantidadanterior + nueva.CantidadDiaria;
            var precipitacion = _precipitacionRepository.InsertAndGetId(nueva);
            return precipitacion;
        }

        public List<Catalogo> ObtenerCatalogos(string code)
    {
            var lista = _catalogoRepository.GetAll().Where(c => c.vigente).Where(c => c.TipoCatalogo.vigente)
                                                    .Where(c => c.TipoCatalogo.codigo == code).ToList();

            if (lista.Count > 0)
            {
                return lista;
            }
            else {
                return new List<Catalogo>();
            }

    }
}
}
