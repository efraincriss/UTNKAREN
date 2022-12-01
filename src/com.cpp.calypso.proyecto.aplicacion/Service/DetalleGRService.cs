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
    public class DetalleGRAsyncBaseCrudAppService : AsyncBaseCrudAppService<DetalleGR, DetalleGRDto, PagedAndFilteredResultRequestDto>, IDetalleGRAsyncBaseCrudAppService
    {
        private readonly IBaseRepository<Certificado> _certificadoRepository;
        private readonly IBaseRepository<GR> _grRepository;

        public DetalleGRAsyncBaseCrudAppService(
            IBaseRepository<DetalleGR> repository,
            IBaseRepository<Certificado> certificadoRepository,
            IBaseRepository<GR> grRepository
        ) : base(repository)
        {
            _certificadoRepository = certificadoRepository;
            _grRepository = grRepository;
        }

        public List<DetalleGRDto> ListarPorGr(int id)
        {
            var query = Repository.GetAll()
                .Where(o => o.vigente)
                .Where(o => o.GRId == id);

            var items = (from g in query
                select new DetalleGRDto()
                {
                    Certificado = g.Certificado,
                    CertificadoId = g.CertificadoId,
                    GR = g.GR,
                    GRId = g.GRId,
                    Id = g.Id
                }).ToList();

            return items;
        }

        public async Task<int> CrearDetalles(int[] idsCertificados, int GrId)
        {
            int cont = 0;
            foreach (var id in idsCertificados)
            {
                var detalle = new DetalleGR()
                {
                    CertificadoId = id,
                    GRId = GrId,
                    vigente = true
                };
                await Repository.InsertAndGetIdAsync(detalle);

                var certificado = _certificadoRepository.Get(id);
                certificado.tiene_GR = true;
                _certificadoRepository.Update(certificado);
                cont++;
            }

            var monto_total = Repository.GetAll()
                .Where(o => o.vigente)
                .Where(o => o.GRId == GrId)
                .Sum(o => o.Certificado.monto_certificado);

            var gr = _grRepository.Get(GrId);
            gr.monto_total = monto_total;
            _grRepository.Update(gr);

            return cont;
        }

        public async Task Eliminar(int id) //DetalleGrId
        {
            var detalle = Repository.Get(id);
            detalle.vigente = false;
            await Repository.UpdateAsync(detalle);

            var monto_total_anterior = Repository.GetAll()
                .Where(o => o.vigente)
                .Where(o => o.GRId == detalle.GRId)
                .Sum(o => o.Certificado.monto_certificado);

            var monto = monto_total_anterior - detalle.Certificado.monto_certificado;

            var  gr = _grRepository.Get(detalle.GRId);
            gr.monto_total = monto;
            _grRepository.Update(gr);

            var certificado = _certificadoRepository.Get(detalle.CertificadoId);
            certificado.tiene_GR = false;
            _certificadoRepository.Update(certificado);
        }

        
    }
}
