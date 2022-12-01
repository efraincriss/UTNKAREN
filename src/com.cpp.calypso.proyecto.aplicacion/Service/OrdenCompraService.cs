using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.aplicacion.Interfaces;
using com.cpp.calypso.proyecto.dominio;
using Microsoft.ClearScript;

namespace com.cpp.calypso.proyecto.aplicacion.Service
{
    public class OrdenCompraAsyncBaseCrudAppService : AsyncBaseCrudAppService<OrdenCompra, OrdenCompraDto, PagedAndFilteredResultRequestDto>, IOrdenCompraAsyncBaseCrudAppService
    {
        private readonly IBaseRepository<DetalleOrdenCompra> _detalleOrdenCompraRepository;
        private readonly IBaseRepository<Proyecto> _proyectoRepository;

        public OrdenCompraAsyncBaseCrudAppService(
            IBaseRepository<OrdenCompra> repository,
            IBaseRepository<DetalleOrdenCompra> detalleOrdenCompraRepository,
            IBaseRepository<Proyecto> proyectoRepository
        ) : base(repository)
        {
            _detalleOrdenCompraRepository = detalleOrdenCompraRepository;
            _proyectoRepository = proyectoRepository;
        }

     

        public List<OrdenCompraDto> listar(int ofertaId)
        {
            var ordenQuery = Repository.GetAllIncluding(o => o.Oferta);
            var item = (from o in ordenQuery
                        where o.OfertaId == ofertaId
                        where o.vigente == true
                        select new OrdenCompraDto()
                        {
                            Id = o.Id,
                            Oferta = o.Oferta,
                            vigente = o.vigente,
                            OfertaId = o.OfertaId,
                            fecha_presentacion = o.fecha_presentacion,
                            nro_pedido_compra = o.nro_pedido_compra,
                            valor_pedido_compra = o.valor_pedido_compra,
                            referencia = o.referencia
                        }).ToList();
            return item;
        }


        public List<OrdenCompraDto> ListarPorProyecto(int proyectoId)
        {
            var query = Repository.GetAllIncluding(o => o.Oferta)
                .Where(o => o.vigente == true)
                .Where(o => o.Oferta.ProyectoId == proyectoId);

            var items = (from o in query
                         select new OrdenCompraDto()
                         {
                             Id = o.Id,
                             Oferta = o.Oferta,
                             vigente = o.vigente,
                             OfertaId = o.OfertaId,
                             fecha_presentacion = o.fecha_presentacion,
                             nro_pedido_compra = o.nro_pedido_compra,
                             valor_pedido_compra = o.valor_pedido_compra,
                             referencia = o.referencia
                         }).ToList();

            return items;
        }

        public int EliminarVigencia(int ordenCompraId)
        {
            var orden = Repository.Get(ordenCompraId);
            if (orden != null)
            {
                orden.vigente = false;
                Repository.Update(orden);
                return orden.OfertaId;
            }

            return 0;
        }

        public void ActualizarMontosOrdenCompra(int ordenCompraId)
        {
            var orden = Repository.Get(ordenCompraId);

            // Obtengo todos los detalles de la orden de servicio
            var query = _detalleOrdenCompraRepository.GetAll();
            var detalles = (from d in query
                            where d.vigente == true
                            where d.OrdenCompraId == ordenCompraId
                            select new DetalleOrdenCompraDto()
                            {
                             
                            }).ToList();

            // Sumo los diferentes montos de los detalles
            decimal monto_os = 0;
            decimal ingenieria = 0;
            decimal suministros = 0;
            decimal construccion = 0;
           /* foreach (var d in detalles)
            {
                if (d.GrupoItemId == DetalleOrdenServicio.GrupoItem.Ingeniería)
                {
                    ingenieria += d.valor_os;
                    monto_os += d.valor_os;
                }
                else if (d.GrupoItemId == DetalleOrdenServicio.GrupoItem.Construcción)
                {
                    construccion += d.valor_os;
                    monto_os += d.valor_os;
                }
                else if (d.GrupoItemId == DetalleOrdenServicio.GrupoItem.Suministros)
                {
                    suministros += d.valor_os;
                    monto_os += d.valor_os;
                }
            }


            // Actualizo la orden de servicio
            orden.monto_aprobado_os = monto_os;
            orden.monto_aprobado_construccion = construccion;
            orden.monto_aprobado_ingeniería = ingenieria;
            orden.monto_aprobado_suministros = suministros;


            Repository.Update(orden);

            this.ActualizarMontosProyecto(orden.Oferta.ProyectoId);*/

        }

        public OrdenCompraDto getdetalles(int OrdenCompraId)
        {
            var query = Repository.GetAllIncluding(c => c.Oferta,c=>c.Oferta.Proyecto);
            var item = (from d in query
                where d.vigente == true
                where d.Id == OrdenCompraId
                        select new OrdenCompraDto()
                {
                    Id = d.Id,
                    OfertaId = d.OfertaId,
                    vigente = d.vigente,
                    estado = d.estado,
                   fecha_presentacion = d.fecha_presentacion,
                    nro_pedido_compra = d.nro_pedido_compra,
                    valor_pedido_compra = d.valor_pedido_compra,
                    referencia = d.referencia
                    
                }).SingleOrDefault();

            return item;
        }
    }
}
