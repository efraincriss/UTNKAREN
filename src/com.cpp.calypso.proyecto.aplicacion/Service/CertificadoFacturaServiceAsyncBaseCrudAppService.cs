using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.aplicacion.Interfaces;
using com.cpp.calypso.proyecto.dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion.Service
{
    public class CertificadoFacturaServiceAsyncBaseCrudAppService : AsyncBaseCrudAppService<CertificadoFactura, CertificadoFacturaDto, PagedAndFilteredResultRequestDto>, ICertificadoFacturaAsyncBaseCrudAppService
    {
        // private readonly IBaseRepository<Contrato> _repositorycontrato;
        public CertificadoFacturaServiceAsyncBaseCrudAppService(IBaseRepository<CertificadoFactura> repository) : base(repository)
        {

        }

        public List<CertificadoFacturaDto> certificadosporfactura(int FacturaId)
        {
            var ordenQuery = Repository.GetAllIncluding(o => o.Factura);
            var item = (from o in ordenQuery
                        where o.FacturaId == FacturaId
                        where o.vigente == true
                        select new CertificadoFacturaDto()
                        {
                            Id = o.Id,
                            Factura = o.Factura,
                            FacturaId = o.FacturaId,
                            vigente = o.vigente,
                            CertificadoId=o.CertificadoId,
                            }).ToList();
            return item;
        }
    }
}

