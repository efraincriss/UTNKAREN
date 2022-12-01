using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.dominio;

namespace com.cpp.calypso.proyecto.aplicacion
{
    public class ObraAdicionalAsyncBaseCrudAppService : AsyncBaseCrudAppService<ObraAdicional, ObraAdicionalDto, PagedAndFilteredResultRequestDto>, IObraAdicionalAsyncBaseCrudAppService
    {
        private readonly IBaseRepository<Catalogo> _catalogoRepository;
        private readonly IBaseRepository<Wbs> _wbsRepository;

        public ObraAdicionalAsyncBaseCrudAppService(
            IBaseRepository<ObraAdicional> repository,
            IBaseRepository<Catalogo> catalogoRepository,
            IBaseRepository<Wbs> wbsRepository
            ) : base(repository)
        {
            _catalogoRepository = catalogoRepository;
            _wbsRepository = wbsRepository;
        }

        public async Task<int> Eliminar(int id)
        {
            var item = Repository.Get(id);
            item.vigente = false;
            await Repository.UpdateAsync(item);
            return item.AvanceObraId;
        }

        public List<ObraAdicionalDto> listar(int AvanceObraId)
        {
            var query = Repository.GetAllIncluding(a => a.Item);
            var items = (from a in query
                where a.vigente == true
                where a.AvanceObraId == AvanceObraId
                select new ObraAdicionalDto()
                {
                    Id = a.Id,
                    AvanceObraId = a.AvanceObraId,
                    vigente = a.vigente,
                    ItemId = a.ItemId,
                    WbsId = a.WbsId,
                    cantidad = a.cantidad,
                    costo_total = a.costo_total,
                    estado = a.estado,
                    precio_ajustado = a.precio_ajustado,
                    precio_base = a.precio_base,
                    precio_incrementado = a.precio_incrementado,
                    precio_unitario = a.precio_unitario,
                    tipo_precio = a.tipo_precio,
                    Wbs = a.Wbs,
                    item_codigo = a.Item.codigo,
                    nombre_item = a.Wbs.nivel_nombre,
                }).ToList();

            foreach (var d in items)
            {
                var name = _wbsRepository
                    .GetAll()
                    .Where(o => o.vigente == true)
                    .Where(o => o.OfertaId == d.Wbs.OfertaId).SingleOrDefault(o => o.id_nivel_codigo == d.Wbs.id_nivel_padre_codigo);
                d.nombre_padre = name.nivel_nombre;
            }
            /*foreach (var a in items)
            {
                a.nombre_area = ObtenerNombreCatalogo(a.WbsOferta.AreaId);
                a.nombre_disciplina = ObtenerNombreCatalogo(a.WbsOferta.DisciplinaId);
                a.nombre_elemento = ObtenerNombreCatalogo(a.WbsOferta.ElementoId);
                a.nombre_actividad = ObtenerNombreCatalogo(a.WbsOferta.ActividadId);
            }*/

            return items;
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
    }
}
