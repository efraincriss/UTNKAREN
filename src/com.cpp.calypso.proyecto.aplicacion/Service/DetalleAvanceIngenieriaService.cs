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
    public class DetalleAvanceIngenieriaAsyncBaseCrudAppService : AsyncBaseCrudAppService<DetalleAvanceIngenieria, DetalleAvanceIngenieriaDto, PagedAndFilteredResultRequestDto>, IDetalleAvanceIngenieriaAsyncBaseCrudAppService
    {
        private readonly IBaseRepository<DetalleItemIngenieria> _itemIngenieriaRepository;
        private readonly IBaseRepository<Computo> _computoRepository;
        private readonly IBaseRepository<AvanceIngenieria> _avanceingenieriaRepository;
        public DetalleAvanceIngenieriaAsyncBaseCrudAppService(
            IBaseRepository<DetalleAvanceIngenieria> repository,
            IBaseRepository<DetalleItemIngenieria> itemIngenieriaRepository, 
            IBaseRepository<Computo> computoRepository,
            IBaseRepository<AvanceIngenieria> avanceingenieriaRepository
            ) : base(repository)
        {
            _itemIngenieriaRepository = itemIngenieriaRepository;
            _computoRepository = computoRepository;
            _avanceingenieriaRepository = avanceingenieriaRepository;
        }

        public List<DetalleAvanceIngenieriaDto> ListarPorAvanceIngenieria(int avanceIngenieriaId)
        {
            var query = Repository.GetAllIncluding(o => o.Computo.Item).Where(o => o.vigente == true)
                .Where(o => o.AvanceIngenieriaId == avanceIngenieriaId);

            var items = (from a in query
                select new DetalleAvanceIngenieriaDto()
                {
                    Id = a.Id,
                    AvanceIngenieriaId = a.AvanceIngenieriaId,
                    ComputoId = a.ComputoId,
                    cantidad_horas = a.cantidad_horas,
                   Computo = a.Computo,
                    codigo_item = a.Computo.Item.codigo,
                    descripcion_item = a.Computo.Item.descripcion,
                    vigente = a.vigente,
                    fecha_real = a.fecha_real,
                    precio_unitario = a.precio_unitario,
                    valor_real = a.valor_real,
                    calculo_anterior = a.calculo_anterior,
                    calculo_diario = a.calculo_diario,
                    ingreso_acumulado = a.ingreso_acumulado,
                    cantidad_acumulada = a.cantidad_acumulada,
                    cantidad_acumulada_anterior = a.cantidad_acumulada_anterior,
                    horas_presupuestadas = a.Computo.cantidad
        }).ToList();
            return items;
        }

        public void Eliminar(int DetalleAvanceId)
        {
            var detalle = Repository.Get(DetalleAvanceId);
            detalle.vigente = false;
            Repository.Update(detalle);

            var items = _itemIngenieriaRepository.GetAll()
                .Where(o => o.vigente == true)
                .Where(o => o.DetalleAvanceIngenieriaId == detalle.Id);

            foreach (var i in items)
            {
                i.vigente = false;
                _itemIngenieriaRepository.Update(i);
            }

        }

        public decimal ObtenerCantidadAcumulada(int computoId, DateTime fecha_reporte, int ofertaId)
        {
            decimal cantidad_acumulada = 0;
            var query = Repository.GetAllIncluding(o => o.AvanceIngenieria)
                .Where(o => o.vigente == true)
                .Where(o => o.ComputoId == computoId)
                .Where(o => o.AvanceIngenieria.aprobado == true)
                .Where(o => o.AvanceIngenieria.fecha_presentacion < fecha_reporte)
                .Where(o => o.AvanceIngenieria.OfertaId == ofertaId);

            var detalles = (from d in query
                select new DetalleAvanceIngenieriaDto()
                {
                    cantidad_acumulada_anterior = d.cantidad_acumulada_anterior,
                    cantidad_horas = d.cantidad_horas,
                    cantidad_acumulada = d.cantidad_acumulada
                }).ToList();

            foreach (var d in detalles)
            {
                cantidad_acumulada += d.cantidad_horas;
            }

            return cantidad_acumulada;
        }

        public List<ComputoDto> GetComputos(int ofertaId)
        {
            var query = _computoRepository.GetAllIncluding(o => o.Wbs.Oferta, o => o.Item)
                .Where(o => o.Wbs.OfertaId == ofertaId)
                //.Where(o => o.WbsOferta.AreaId == 1)
                //.Where(o => o.WbsOferta.DisciplinaId == 1)
                //.Where(o => o.WbsOferta.ElementoId == 1)
                //.Where(o => o.WbsOferta.ActividadId == 1)
                .Where(o => o.vigente == true);

            var computos = (from c in query
                select new ComputoDto()
                {
                    Id = c.Id,
                    item_codigo = c.Item.codigo,
                    item_nombre = c.Item.nombre,
                    Item =c.Item,
                    ItemId = c.ItemId,
                    precio_unitario = c.precio_unitario,
                }).ToList();

            return computos;
        }

        public decimal calcularvalor(int AvanceIngenieriaId)
        {

            decimal total = 0;
            var query = Repository.GetAllIncluding(c => c.AvanceIngenieria, c => c.AvanceIngenieria.Oferta);
            var items = (from a in query
                where a.vigente == true
                where a.AvanceIngenieriaId == AvanceIngenieriaId
                select new DetalleAvanceIngenieriaDto()
                {
                    Id = a.Id,
                    vigente = a.vigente,
                    AvanceIngenieriaId = a.AvanceIngenieriaId,
                    ComputoId = a.ComputoId,
                    Computo = a.Computo,
                    precio_unitario = a.precio_unitario,
                    calculo_anterior = a.calculo_anterior,
                    calculo_diario = a.calculo_diario,
                    ingreso_acumulado = a.ingreso_acumulado,
                    valor_real = a.valor_real,
                    fecha_real = a.fecha_real
                }).ToList();


            foreach (var i in items)
            {
                total = total + i.valor_real;

            }
            var avance = _avanceingenieriaRepository.Get(AvanceIngenieriaId);
            avance.monto_ingenieria = total;
            var resultado = _avanceingenieriaRepository.Update(avance);
            return total;

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
