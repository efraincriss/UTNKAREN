using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.dominio;

namespace com.cpp.calypso.proyecto.aplicacion
{
    public class DetalleItemIngenieriaAsyncBaseCrudAppService : AsyncBaseCrudAppService<DetalleItemIngenieria, DetalleItemIngenieriaDto, PagedAndFilteredResultRequestDto>, IDetalleItemIngenieriaAsyncBaseCrudAppService
    {
        private readonly IBaseRepository<Catalogo> _catalogoRepository;

        public DetalleItemIngenieriaAsyncBaseCrudAppService(
            IBaseRepository<DetalleItemIngenieria> repository,
            IBaseRepository<Catalogo> catalogoRepository
            ) : base(repository)
        {
            _catalogoRepository = catalogoRepository;
        }

        public List<DetalleItemIngenieriaDto> ListarPorDetalleAvance(int detalleAvanceId)
        {
            var query = Repository.GetAllIncluding(o => o.etapa)
                .Where(o => o.vigente == true)
                .Where(o => o.DetalleAvanceIngenieriaId == detalleAvanceId);

            var items = (from i in query
                select new DetalleItemIngenieriaDto()
                {
                    Id = i.Id,
                    ColaboradorId = i.ColaboradorId,
                    cantidad_horas = i.cantidad_horas,
                    fecha_registro = i.fecha_registro,
                    vigente = i.vigente,
                    DetalleAvanceIngenieriaId = i.DetalleAvanceIngenieriaId,
                    etapa = i.etapa,
                    nombre_colaborador = i.Colaborador.nombres,
                    especialidad = i.especialidad,
                    tipo_registro = i.tipo_registro,
                }).ToList();

            foreach (var i in items)
            {
                i.nombre_especialidad = ObtenerNombreCatalogo(i.especialidad);
                
            }
            

            return items;
        }

        public string ObtenerNombreCatalogo(int id)
        {
            var catalogo = _catalogoRepository.GetAll();
            var item = (from w in catalogo
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
