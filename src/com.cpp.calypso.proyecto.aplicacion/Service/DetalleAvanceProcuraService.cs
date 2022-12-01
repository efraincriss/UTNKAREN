using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.aplicacion.Interfaces;
using com.cpp.calypso.proyecto.dominio;

namespace com.cpp.calypso.proyecto.aplicacion.Service
{
    public class DetalleAvanceProcuraServiceAsyncBaseCrudAppService :AsyncBaseCrudAppService<DetalleAvanceProcura, DetalleAvanceProcuraDto, PagedAndFilteredResultRequestDto>,IDetalleAvanceProcuraAsyncBaseCrudAppService
    {
        private readonly IBaseRepository<Computo> _computoRepository;
        private readonly IBaseRepository<AvanceProcura> _avanceprocuraRepository;
        public DetalleAvanceProcuraServiceAsyncBaseCrudAppService(
            IBaseRepository<DetalleAvanceProcura> repository,
            IBaseRepository<Computo> computoRepository, IBaseRepository<AvanceProcura> avanceprocuraRepository
        ) : base(repository)
        {
            _computoRepository = computoRepository;
            _avanceprocuraRepository = avanceprocuraRepository;
        }

        public DetalleAvanceProcuraDto getdetalles(int DetalleAvanceProcuraId)
        {
            var query = Repository.GetAllIncluding(o => o.AvanceProcura, o=>o.AvanceProcura.Oferta,o => o.AvanceProcura.Oferta.Proyecto, o => o.DetalleOrdenCompra,o=>o.DetalleOrdenCompra.Computo , o=>o.DetalleOrdenCompra.OrdenCompra);

            var items = (from a in query
                where a.vigente == true
                where a.Id == DetalleAvanceProcuraId
                select new DetalleAvanceProcuraDto()
                {
                    Id = a.Id,
                    DetalleOrdenCompraId = a.DetalleOrdenCompraId,
                    DetalleOrdenCompra = a.DetalleOrdenCompra,
                    vigente = a.vigente,
                    AvanceProcuraId = a.AvanceProcuraId,
                  AvanceProcura = a.AvanceProcura,
                    fecha_real = a.fecha_real,
                   estado = a.estado,
                    valor_real = a.valor_real,
                    cantidad = a.cantidad,
                    precio_unitario = a.precio_unitario,
                    OrdenCompra = a.DetalleOrdenCompra.OrdenCompra,
                    Computo = a.DetalleOrdenCompra.Computo
                }).FirstOrDefault();

            return items;
        }

        public List<DetalleAvanceProcuraDto> listarporavanceprocura(int AvanceProcuraId)
        {

            var query = Repository.GetAllIncluding(o => o.AvanceProcura,o=>o.DetalleOrdenCompra,o=>o.DetalleOrdenCompra.OrdenCompra, o => o.AvanceProcura.Oferta, o => o.AvanceProcura.Oferta.Proyecto, o => o.DetalleOrdenCompra.Computo.Item);
            var items = (from a in query
                where a.AvanceProcuraId==AvanceProcuraId
                         where a.vigente==true
                select new DetalleAvanceProcuraDto()
                {
                    Id = a.Id,
                  vigente = a.vigente,
                    AvanceProcuraId = a.AvanceProcuraId,
                    AvanceProcura = a.AvanceProcura,
                    DetalleOrdenCompraId = a.DetalleOrdenCompraId,
                    DetalleOrdenCompra = a.DetalleOrdenCompra,
                    Item = a.DetalleOrdenCompra.Computo.Item,
                    fecha_real =a.fecha_real,
                 estado = a.estado,
                    valor_real = a.valor_real,
                    cantidad = a.cantidad,
                    precio_unitario = a.precio_unitario,
                    calculo_anterior = a.calculo_anterior,
                    calculo_diario = a.calculo_diario,
                    ingreso_acumulado = a.ingreso_acumulado,
                    OrdenCompra = a.DetalleOrdenCompra.OrdenCompra,
                    Computo = a.DetalleOrdenCompra.Computo
                }).ToList();

            return items;
        }

        public List<ComputoDto> GetComputos(int ofertaId)
        {
            var query = _computoRepository.GetAllIncluding(o => o.Wbs.Oferta, o => o.Item)
                .Where(o => o.Wbs.OfertaId == ofertaId)
                .Where(o => o.vigente == true);

            var computos = (from c in query
                where c.Item.item_padre=="7.1."
                select new ComputoDto()
                {
                    Id = c.Id,
                    item_codigo = c.Item.codigo,
                    item_nombre = c.Item.nombre,
                    Item = c.Item,
                    ItemId = c.ItemId
                }).ToList();

            foreach (var item in computos)
            {
                if (item.Item != null)
                {
                    item.nombreitem = item.Id + " - " + item.Item.codigo + "-" + item.Item.nombre;
                }
            }
            return computos;
        }

        public decimal calcularvalor(int AvanceprocuraId)
        {
            decimal total = 0;
            var query = Repository.GetAllIncluding(c => c.AvanceProcura, c => c.AvanceProcura.Oferta);
            var items = (from a in query
                where a.vigente == true
                where a.AvanceProcuraId == AvanceprocuraId
                select new DetalleAvanceProcuraDto()
                {
                    Id = a.Id,
                    vigente = a.vigente,
                    AvanceProcuraId = a.AvanceProcuraId,
                    AvanceProcura = a.AvanceProcura,
                    DetalleOrdenCompraId = a.DetalleOrdenCompraId,
                    DetalleOrdenCompra = a.DetalleOrdenCompra,
                    Item = a.DetalleOrdenCompra.Computo.Item,
                    fecha_real = a.fecha_real,
                    estado = a.estado,
                    valor_real = a.valor_real,
                    cantidad = a.cantidad,
                    precio_unitario = a.precio_unitario,
                    calculo_anterior = a.calculo_anterior,
                    calculo_diario = a.calculo_diario,
                    ingreso_acumulado = a.ingreso_acumulado,
                    Computo = a.DetalleOrdenCompra.Computo
                }).ToList();


            foreach (var i in items)
            {
                total = total + i.valor_real;

            }
            var avance = _avanceprocuraRepository.Get(AvanceprocuraId);
            avance.monto_procura = total;
            var resultado = _avanceprocuraRepository.Update(avance);
            return total;
        }

        public int EliminarVigencia(int AvanceprocuraId)
        {
            var orden = Repository.Get(AvanceprocuraId);
            if (orden != null)
            {
                orden.vigente = false;
                Repository.Update(orden);
                return orden.AvanceProcuraId;
            }

            return 0;
        }

        public decimal obtenercalculoanterior(int ComputoId)
        {
            var query = Repository.GetAllIncluding(c => c.DetalleOrdenCompra, c => c.DetalleOrdenCompra.Computo).
                Where(c=>c.DetalleOrdenCompra.ComputoId==ComputoId).Where(c=>c.vigente==true).ToList();
            decimal sumaporcentaje = 0;

            if (query.Count > 0)
            {
                sumaporcentaje = (from x in query select x.calculo_diario).Sum();  // sum: 19
                return sumaporcentaje;
            }

            return sumaporcentaje;

        }
        public decimal montoanteriores(int AvanceprocuraId)
        {
            var query = Repository.GetAllIncluding(c => c.DetalleOrdenCompra, c => c.DetalleOrdenCompra.Computo)
               .Where(c => c.vigente == true).Where(c=>c.AvanceProcuraId==AvanceprocuraId).ToList();
            
            decimal sumaporcentaje = 0;

            if (query.Count > 0)
            {
                sumaporcentaje = (from x in query select x.calculo_anterior).Sum();  // sum: 19
                return sumaporcentaje;
            }

            return sumaporcentaje;

        }
        public decimal montopresupuesto(int AvanceprocuraId)
        {
            var query = Repository.GetAllIncluding(c => c.DetalleOrdenCompra, c => c.DetalleOrdenCompra.Computo)
                .Where(c => c.vigente == true).Where(c => c.AvanceProcuraId == AvanceprocuraId).ToList();
            decimal sumaporcentaje = 0;

            if (query.Count > 0)
            {
                sumaporcentaje = (from x in query select x.cantidad).Sum();  // sum: 19
                return sumaporcentaje;
            }

            return sumaporcentaje;

        }
        public decimal montoacumulados(int AvanceprocuraId)
        {
            var query = Repository.GetAllIncluding(c => c.DetalleOrdenCompra, c => c.DetalleOrdenCompra.Computo)
                .Where(c => c.vigente == true).Where(c => c.AvanceProcuraId == AvanceprocuraId).ToList();
            decimal sumaporcentaje = 0;

            if (query.Count > 0)
            {
                sumaporcentaje = (from x in query select x.ingreso_acumulado).Sum();  // sum: 19
                return sumaporcentaje;
            }

            return sumaporcentaje;

        }

        
        public decimal montoactual(int AvanceprocuraId)
        {
            var query = Repository.GetAllIncluding(c => c.DetalleOrdenCompra, c => c.DetalleOrdenCompra.Computo)
                .Where(c => c.vigente == true).Where(c => c.AvanceProcuraId == AvanceprocuraId).ToList();
            decimal sumaporcentaje = 0;

            if (query.Count > 0)
            {
                sumaporcentaje = (from x in query select x.calculo_diario).Sum();  // sum: 19
                return sumaporcentaje;
            }

            return sumaporcentaje;

        }
        public bool cambiaracertificado(int id)
        {
            var r = Repository.Get(id);
            r.estacertificado = false;
            var d = Repository.Update(r);
            return true;
        }
    }
    }
