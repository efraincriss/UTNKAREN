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
    public class GRAsyncBaseCrudAppService : AsyncBaseCrudAppService<GR, GRDto, PagedAndFilteredResultRequestDto>, IGRAsyncBaseCrudAppService
    {
        private readonly IBaseRepository<DetalleGR> _detalleGRepository;

        public GRAsyncBaseCrudAppService(
            IBaseRepository<GR> repository,
            IBaseRepository<DetalleGR> detalleGRepository
        ) : base(repository)
        {
            _detalleGRepository = detalleGRepository;
        }

        public List<GRDto> Listar()
        {
            var query = Repository.GetAllIncluding(o => o.Proyecto.Contrato.Cliente)
                .Where(o => o.vigente);

            var items = (from i in query
                select new GRDto()
                {
                    Id = i.Id,
                    Proyecto = i.Proyecto,
                    ProyectoId = i.ProyectoId,
                    Cliente = i.Proyecto.Contrato.Cliente,
                    fecha_registro = i.fecha_registro,
                    numero_gr = i.numero_gr,
                    Contrato = i.Proyecto.Contrato
                }).ToList();
            return items;
        }

        public GRDto GetGr(int id)
        {
            var query = Repository.GetAllIncluding(o => o.Proyecto.Contrato.Cliente)
                .Where(o => o.Id == id);

            var item = (from g in query
                select new GRDto()
                {
                    Id = g.Id,
                    numero_gr = g.numero_gr,
                    Cliente = g.Proyecto.Contrato.Cliente,
                    Contrato = g.Proyecto.Contrato,
                    Proyecto = g.Proyecto,
                    ProyectoId = g.ProyectoId,
                    fecha_registro = g.fecha_registro,

                }).SingleOrDefault();
            return item;

        }

        public decimal GetMontoTotal(int GrId)
        {
            var gr = Repository.Get(GrId);
            return gr.monto_total;
        }

        public bool Eliminar(int GrId)
        {
            var count = _detalleGRepository
                .GetAll()
                .Where(o => o.vigente).Count(o => o.GRId == GrId);

            if (count > 0)
            {
                return false;
            }
            else
            {
                var gr = Repository.Get(GrId);
                gr.vigente = false;
                Repository.Update(gr);
                return true;
            }
        }


    }
}
