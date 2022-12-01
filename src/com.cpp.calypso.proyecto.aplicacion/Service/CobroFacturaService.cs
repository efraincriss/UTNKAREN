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
   public class CobroFacturaAsyncBaseCrudAppService : AsyncBaseCrudAppService<CobroFactura, CobroFacturaDto, PagedAndFilteredResultRequestDto>, ICobroFacturaAsyncBaseCrudAppService
    {
        IBaseRepository<Cobro> _cobrorepository;
        public CobroFacturaAsyncBaseCrudAppService(
            IBaseRepository<CobroFactura> repository,
            IBaseRepository<Cobro> cobrorepository
        ) : base(repository)
        {
            _cobrorepository = cobrorepository;
        }

        public bool Eliminar(int Id)
        {
            var x = Repository.Get(Id);
            if (x != null && x.Id > 0) {
                x.vigente = false;
                var actualizado = Repository.Update(x);
                return true;
            }
            return false;
        }

        public CobroFacturaDto getdetalle(int Id)
        {
            var x = Repository.GetAllIncluding(c => c.Cobro, c => c.Factura,c=>c.Factura.Empresa,c=>c.Factura.Cliente)
                .Where(c => c.vigente == true).ToList();
            var lista = (from co in x
                         where co.CobroId == Id
                         where co.vigente == true
                         select new CobroFacturaDto {
                             Id = co.Id,
                             CobroId = co.CobroId,
                             Cobro = co.Cobro,
                             FacturaId = co.FacturaId,
                             Factura = co.Factura,
                             monto = co.monto,
                             vigente = co.vigente
                         }).FirstOrDefault();
            return lista;
        }

        public List<Cobro> ListaCobrosUnicos()
        {
            var lista = _cobrorepository.GetAll().Where(y => y.vigente == true).ToList();
            return lista;
        }

        public List<CobroFactura> ListadeCobros()
        {
            var x = Repository.GetAllIncluding(c => c.Cobro, c => c.Factura)
                .Where(c => c.vigente == true).ToList();
            //var y = (from z in x select z).ToList().Distinct();               
            return x;
        }

        public List<CobroFactura> ListadeCobrosFactura(int Id)// FacturaId
        {
            var x = Repository.GetAllIncluding(c => c.Cobro, c => c.Factura)
                .Where(c => c.vigente == true)
                .Where(c=>c.FacturaId==Id)
                .ToList();
            return x;
        }

        public List<CobroFactura> ListaFacturaCobros(int Id)
        {

            var x = Repository.GetAllIncluding(c => c.Cobro, c => c.Factura)
                .Where(c => c.vigente == true)
                .Where(c => c.CobroId == Id)
                .ToList();
            return x;
        }
    }
}