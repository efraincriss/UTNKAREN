using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.dominio;
using static com.cpp.calypso.proyecto.dominio.DetalleOrdenServicio;

namespace com.cpp.calypso.proyecto.aplicacion.Service
{
    public class DetalleOrdenServicioAsyncBaseCrudAppService : AsyncBaseCrudAppService<DetalleOrdenServicio, DetalleOrdenServicioDto, PagedAndFilteredResultRequestDto>, IDetalleOrdenServicioAsyncBaseCrudAppService
    {
        private readonly IBaseRepository<OrdenServicio> _ordenServicioRepository;
        private readonly IBaseRepository<Proyecto> _proyectoService;

        public DetalleOrdenServicioAsyncBaseCrudAppService(
            IBaseRepository<DetalleOrdenServicio> repository,
            IBaseRepository<OrdenServicio> ordenServicioRepository,
            IBaseRepository<Proyecto> proyectoService
        ) : base(repository)
        {
            _ordenServicioRepository = ordenServicioRepository;
            _proyectoService = proyectoService;
        }

        public List<DetalleOrdenServicioDto> listar(int ordenServicioId)
        {
            var query = Repository.GetAll();
            var items = (from d in query
                         where d.vigente == true
                         where d.OrdenServicioId == ordenServicioId
                         select new DetalleOrdenServicioDto()
                         {
                             Id = d.Id,
                             ProyectoId = d.ProyectoId,
                             GrupoItemId = d.GrupoItemId,
                             OrdenServicioId = d.OrdenServicioId,
                             valor_os = d.valor_os,
                             vigente = d.vigente,
                             nombre_proyecto = d.Proyecto.nombre_proyecto,
                            codigo_proyecto=d.Proyecto.codigo
                            
                }).ToList();

            return items.OrderBy(c=>c.codigo_proyecto).ToList();
        }

        public DetalleOrdenServicioDto GetDetalles(int detalleOrdenServicioId)
        {
            var query = Repository.GetAll();
            var item = (from d in query
                where d.vigente == true
                where d.Id == detalleOrdenServicioId
                select new DetalleOrdenServicioDto()
                {
                    Id = d.Id,
                    GrupoItemId = d.GrupoItemId,
                    OrdenServicioId = d.OrdenServicioId,
                    valor_os = d.valor_os,
                    vigente = d.vigente,
                    OrdenServicio = d.OrdenServicio
                }).SingleOrDefault();

            return item;
        }


        public int Eliminar(int detalleOrdenServicioId)
        {
            var query = Repository.GetAll();
            var item = (from d in query
                where d.vigente == true
                where d.Id == detalleOrdenServicioId
                select new DetalleOrdenServicioDto()
                {
                    Id = d.Id,
                    GrupoItemId = d.GrupoItemId,
                    OrdenServicioId = d.OrdenServicioId,
                    valor_os = d.valor_os,
                    vigente = d.vigente,
                    OrdenServicio = d.OrdenServicio
                }).SingleOrDefault();

            item.vigente = false;

            Repository.Update(MapToEntity(item));

            return item.OrdenServicioId;
        }

        public int creardetallos(DetalleOrdenServicio detalleorden)
        {
            var nuevo = Repository.InsertAndGetId(detalleorden);
            return nuevo;
        }
    }
}
