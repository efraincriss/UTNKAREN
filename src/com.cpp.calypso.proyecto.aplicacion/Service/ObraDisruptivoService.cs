using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.dominio;

namespace com.cpp.calypso.proyecto.aplicacion
{
    public class ObraDisruptivoAsyncBaseCrudAppService : AsyncBaseCrudAppService<ObraDisruptivo, ObraDisruptivoDto, PagedAndFilteredResultRequestDto>, IObraDisruptivoAsyncBaseCrudAppService
    {
        private readonly IBaseRepository<Catalogo> _catalogoRepository;

        public ObraDisruptivoAsyncBaseCrudAppService(
            IBaseRepository<ObraDisruptivo> repository,
            IBaseRepository<Catalogo> catalogoRepository
            ) : base(repository)
        {
            _catalogoRepository = catalogoRepository;
        }

        public int EliminarVigencia(int ObraDisruptivoId)
        {
            var query = Repository.GetAll();

            var item = (from r in query
                where r.Id == ObraDisruptivoId
                where r.vigente == true
                select new ObraDisruptivoDto()
                {
                    Id = r.Id,
                    vigente = r.vigente,
                    numero_horas_hombres = r.numero_horas_hombres,
                    numero_recursos = r.numero_recursos,
                    numero_horas = r.numero_horas,
                    Proyecto = r.Proyecto,
                    ProyectoId = r.ProyectoId,
                    observaciones = r.observaciones,
                    hora_fin = r.hora_fin,
                    hora_inicio = r.hora_inicio,
                    tipo_improductividad = r.tipo_improductividad
                }).SingleOrDefault();

            item.vigente = false;
            var id = Repository.InsertOrUpdateAndGetId(Mapper.Map<ObraDisruptivo>(item));
            return id;
        }

        public List<ObraDisruptivoDto> listar(int AvacenObraId)
        {
            var query = Repository.GetAllIncluding(c=>c.Proyecto,c=>c.Catalogo).ToList();

            var items = (from r in query
                         where r.ProyectoId == AvacenObraId
                         where r.vigente == true
                         select new ObraDisruptivoDto()
                         {
                             Id = r.Id,
                             vigente = r.vigente,
                             numero_horas_hombres = r.numero_horas_hombres,
                             numero_recursos = r.numero_recursos,
                             numero_horas = r.numero_horas,
                             Proyecto = r.Proyecto,
                             ProyectoId = r.ProyectoId,
                             observaciones = r.observaciones,
                             hora_fin = r.hora_fin,
                             hora_inicio = r.hora_inicio,
                             fecha_inicio = r.fecha_inicio,
                             fecha_fin = r.fecha_fin,
                             tipo_improductividad = r.tipo_improductividad,
                             TipoRecursoId = r.TipoRecursoId,
                             nombre_recurso = r.Catalogo.nombre,
                             numero_dias = r.fecha_fin.HasValue ? (r.fecha_fin.Value - r.fecha_inicio.Value).Days + 1 :
                             (DateTime.Now - r.fecha_inicio.Value).Days + 1,
                             porcentaje_disruptivo=r.porcentaje_disruptivo,



                }).ToList();
            foreach (var d in items)
            {
                d.nombre_improductividad = this.nombreCatalogo(d.tipo_improductividad);
               d.dias_real =d.numero_dias * Convert.ToDecimal(d.porcentaje_disruptivo * 0.01);
            }
            return items;
        }

        public List<CatalogoDto> getCatalogosImproductividad()
        {
            var query = _catalogoRepository.GetAll();
            var items = (from c in query
                where c.vigente == true
                where c.TipoCatalogoId == 1002
                select new CatalogoDto()
                {
                    Id = c.Id,
                    nombre = c.nombre
                }).ToList();

            return items;
        }

        public List<CatalogoDto> getCatalogosRecursos()
        {
            var query = _catalogoRepository.GetAll();
            var items = (from c in query
                where c.vigente == true
                where c.TipoCatalogoId == 3008
                select new CatalogoDto()
                {
                    Id = c.Id,
                    nombre = c.nombre
                }).ToList();

            return items;
        }

        public string nombreCatalogo(int id)
        {
            var query = _catalogoRepository.GetAll();
            var items = (from c in query
                where c.vigente == true
                where c.Id == id
                select new CatalogoDto()
                {
                    Id = c.Id,
                    nombre = c.nombre
                }).SingleOrDefault();

            return items.nombre;
        }
    }
}
