using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper.Mappers.Internal;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.aplicacion.Interfaces;
using com.cpp.calypso.proyecto.dominio;

namespace com.cpp.calypso.proyecto.aplicacion.Service
{
    public class DetalleOrdenCompraAsyncBaseCrudAppService :
        AsyncBaseCrudAppService<DetalleOrdenCompra, DetalleOrdenCompraDto, PagedAndFilteredResultRequestDto>,
        IDetalleOrdenCompraAsyncBaseCrudAppService
    {
        private readonly IBaseRepository<OrdenCompra> _ordenCompraRepository;
        private readonly IBaseRepository<Proyecto> _proyectoService;
        private readonly IBaseRepository<Item> _itemService;
        private readonly IBaseRepository<Computo> _repositorycomputo;

        public DetalleOrdenCompraAsyncBaseCrudAppService(
            IBaseRepository<Computo> repositorycomputo,
            IBaseRepository<DetalleOrdenCompra> repository,
            IBaseRepository<OrdenCompra> ordenCompraRepository,
            IBaseRepository<Proyecto> proyectoService,
            IBaseRepository<Item> itemService
        ) : base(repository)
        {
            _ordenCompraRepository = ordenCompraRepository;
            _proyectoService = proyectoService;
            _itemService = itemService;
            _repositorycomputo = repositorycomputo;
        }


        public List<DetalleOrdenCompraDto> listar(int ordenCompraId)
        {
            var query = Repository.GetAllIncluding(c => c.OrdenCompra, c => c.OrdenCompra.Oferta, c => c.Computo,
                c => c.Computo.Item);
            var items = (from d in query
                where d.vigente == true
                where d.OrdenCompraId == ordenCompraId
                select new DetalleOrdenCompraDto()
                {
                    Id = d.Id,
                    ComputoId = d.ComputoId,
                    Computo = d.Computo,
                    OrdenCompraId = d.OrdenCompraId,
                    OrdenCompra = d.OrdenCompra,
                    vigente = d.vigente,
                    fecha = d.fecha,
                    porcentaje = d.porcentaje,
                    tipoFecha = d.tipoFecha,
                    estado = d.estado,
                    valor = d.valor,
                    Item = d.Computo.Item,
                    

                }).ToList();

            return items;
        }

        public DetalleOrdenCompraDto GetDetalles(int detalleOrdenCompraId)
        {
            var query = Repository.GetAllIncluding(c => c.OrdenCompra, c => c.OrdenCompra.Oferta);
            var item = (from d in query
                where d.vigente == true
                where d.Id == detalleOrdenCompraId
                select new DetalleOrdenCompraDto()
                {
                    Id = d.Id,
                    OrdenCompraId = d.OrdenCompraId,
                    OrdenCompra = d.OrdenCompra,
                    ComputoId = d.ComputoId,
                    Computo = d.Computo,
                    vigente = d.vigente,
                    fecha = d.fecha,
                    porcentaje = d.porcentaje,
                    tipoFecha = d.tipoFecha,
                    estado = d.estado,
                    valor = d.valor,

                }).SingleOrDefault();

            return item;
        }


        public int Eliminar(int detalleOrdenCompraId)
        {
            var query = Repository.GetAll();
            var item = (from d in query
                where d.vigente == true
                where d.Id == detalleOrdenCompraId
                select new DetalleOrdenCompraDto()
                {
                    Id = d.Id,
                    ComputoId = d.ComputoId,
                    Computo = d.Computo,
                    OrdenCompraId = d.OrdenCompraId,
                    OrdenCompra = d.OrdenCompra,
                    vigente = d.vigente,
                    fecha = d.fecha,
                    porcentaje = d.porcentaje,
                    tipoFecha = d.tipoFecha,
                    estado = d.estado,
                    valor = d.valor,
                }).SingleOrDefault();

            item.vigente = false;

            Repository.Update(MapToEntity(item));

            return item.OrdenCompraId;
        }

        public List<Item> Itemsoferta(int Ofertaid)
        {

            var computosQuery = _repositorycomputo.GetAll();
            var computos = (from c in computosQuery
                where c.Wbs.OfertaId == Ofertaid && c.vigente == true
                select new ComputoDto
                {
                    Id = c.Id,
                    WbsId = c.WbsId,
                    ItemId = c.ItemId,
                    Item = c.Item,
                    cantidad = c.cantidad,
                    precio_unitario = c.precio_unitario,
                    costo_total = c.costo_total,
                    estado = c.estado,
                    vigente = c.vigente,
                    precio_base = c.precio_base,
                    precio_ajustado = c.precio_ajustado,
                    precio_incrementado = c.precio_incrementado,
                    precio_aplicarse = c.precio_aplicarse,
                    Wbs = c.Wbs


                }).ToList();

            List<Item> r = new List<Item>();

            foreach (var c in computosQuery)
            {
                var i = _itemService.Get(c.ItemId);



                r.Add(i);


            }

            return r;
        }

        public decimal calcularvalor(int ordenCompraId)
        {
            decimal total = 0;
            var query = Repository.GetAllIncluding(c => c.OrdenCompra, c => c.OrdenCompra.Oferta);
            var items = (from d in query
                where d.vigente == true
                where d.OrdenCompraId == ordenCompraId
                select new DetalleOrdenCompraDto()
                {
                    Id = d.Id,
                    ComputoId = d.ComputoId,
                    Computo = d.Computo,
                    OrdenCompraId = d.OrdenCompraId,
                    OrdenCompra = d.OrdenCompra,

                    vigente = d.vigente,
                    fecha = d.fecha,
                    porcentaje = d.porcentaje,
                    tipoFecha = d.tipoFecha,
                    estado = d.estado,
                    valor = d.valor

                }).ToList();


            foreach (var i in items)
            {
                total = total + i.valor;

            }

            var ordencompra = _ordenCompraRepository.Get(ordenCompraId);
            ordencompra.valor_pedido_compra = total;
            var resultado = _ordenCompraRepository.Update(ordencompra);
            return total;
        }

        public List<ComputoDto> GetComputos(int ofertaId)
        {
            List<ComputoDto> porcentaje = new List<ComputoDto>();
            var query = _repositorycomputo.GetAllIncluding(o => o.Wbs.Oferta, o => o.Item, o => o.Wbs)
                .Where(o => o.Wbs.OfertaId == ofertaId)
                .Where(o => o.vigente == true).Where(c=>c.Item.GrupoId==3);

            var computos = (from c in query

                select new ComputoDto()
                {
                    Id = c.Id,
                    item_codigo = c.Item.codigo,
                    item_nombre = c.Item.nombre,
                    ItemId = c.ItemId,
                    Item = c.Item,
                    WbsId = c.WbsId,
                    Wbs = c.Wbs,
                    cantidad = c.cantidad,
                    codigo_item_alterno=c.codigo_item_alterno
                }).ToList();

            
            foreach (var item in computos)
            {


                if (item.Item != null)
                {
                    if (item.codigo_item_alterno!=null && item.codigo_item_alterno.Length > 0)
                    {

                        item.nombreitem = item.Wbs.nivel_nombre + " - " + item.codigo_item_alterno + " - " + item.Item.nombre;
                    }
                    else {
                        item.nombreitem = item.Wbs.nivel_nombre + " - " + item.Item.codigo + " - " + item.Item.nombre;
                    }
                  

                    decimal resultado = comprobarporcentaje(item.Id);

                    if (resultado <100)
                    {
                        porcentaje.Add(item);
                    }
                }

              
            }


            return porcentaje;
        }

        public bool comprobarfechaitem(int OrdeCompraId, int ItemId, DateTime fecha)
        {
            var query = Repository.GetAllIncluding(c => c.OrdenCompra, c => c.OrdenCompra.Oferta, c => c.Computo,
                    c => c.Computo.Item)
                .Where(c => c.vigente == true).Where(c => c.ComputoId == ItemId).Where(c => c.fecha == fecha)
                .Where(c => c.OrdenCompraId == OrdeCompraId).ToList();

            if (query.Count > 0)

            {
                return true;
            }

            return false;
        }

        public List<DetalleOrdenCompraDto> GetComputosOrdenescompra(int OrdenCompraId)
        {
            
            var query = Repository.GetAllIncluding(c => c.Computo, c => c.Computo.Item);
            var items = (from d in query
                where d.vigente == true
                where d.estado == DetalleOrdenCompra.EstadoDetalleOrdenCompra.Registrado
                where d.OrdenCompraId == OrdenCompraId
                select new DetalleOrdenCompraDto()
                {
                    Id = d.Id,
                    ComputoId = d.ComputoId,
                    Computo = d.Computo,
                    OrdenCompraId = d.OrdenCompraId,
                    vigente = d.vigente,
                    fecha = d.fecha,
                    porcentaje = d.porcentaje,
                    tipoFecha = d.tipoFecha,
                    estado = d.estado,
                    valor = d.valor,
                    Item = d.Computo.Item

                }).ToList();
            foreach (var item in items)
            {
                if (item.Item != null)
                {
                    item.nombreitem = item.Computo.Wbs.nivel_nombre + " - " + item.Item.codigo + " - " +
                                      item.Item.nombre;
                }
            }

          
            return items;

        }

        public bool PasarOrdenAprobado(int id)
        {
            DetalleOrdenCompra a = Repository.Get(id);
            a.estado = DetalleOrdenCompra.EstadoDetalleOrdenCompra.Aprobado;
            var resultado = Repository.Update(a);
            if (resultado.Id > 0)
            {
                return true;
            }

            return false;
        }

        public bool PasarOrdenRegistrado(int id)
        {
            DetalleOrdenCompra a = Repository.Get(id);
            a.estado = DetalleOrdenCompra.EstadoDetalleOrdenCompra.Registrado;
            var resultado = Repository.Update(a);
            if (resultado.Id > 0)
            {
                return true;
            }

            return false;
        }

        public List<DetalleOrdenCompraDto> listarporoferta(int OfertaId)
        {
            var query = Repository.GetAllIncluding(c => c.OrdenCompra.Oferta, c => c.Computo);
            var items = (from d in query
                where d.vigente == true
                where d.OrdenCompra.OfertaId == OfertaId
                where d.estado == DetalleOrdenCompra.EstadoDetalleOrdenCompra.Registrado
                select new DetalleOrdenCompraDto()
                {
                    Id = d.Id,
                    ComputoId = d.ComputoId,
                    Computo = d.Computo,
                    OrdenCompraId = d.OrdenCompraId,
                    OrdenCompra = d.OrdenCompra,
                    vigente = d.vigente,
                    fecha = d.fecha,
                    porcentaje = d.porcentaje,
                    tipoFecha = d.tipoFecha,
                    estado = d.estado,
                    valor = d.valor,
                    Item = d.Computo.Item,
                   

                }).ToList();
            foreach (var i in items)
            {
                if (i.tipoFecha == DetalleOrdenCompra.TipoFecha.Compras)
                {
                    i.tiporegistro = "Compras";
                }
                else if (i.tipoFecha == DetalleOrdenCompra.TipoFecha.Pruebas)
                {
                    i.tiporegistro = "Pruebas";
                }
                else
                {
                    i.tiporegistro = "Final";
                }

               
                i.nombreestado= "Registrado";
                i.fechas = i.fecha.ToString("dd/MM/yyyy");
                i.porcentajes = i.porcentaje + "%";
                i.valores = String.Format(CultureInfo.InvariantCulture, "{0:0,0.00}", i.valor);
            }

            return items;
        }

        public decimal porcentaje(DetalleOrdenCompraDto detalle)
        {
            var datos = Repository.GetAllIncluding(c => c.OrdenCompra, c => c.Computo);
            decimal sumaporcentaje = 0;
            var item = (from i in datos
                where i.ComputoId == detalle.ComputoId
                where i.OrdenCompra.vigente == true
                where i.vigente==true
                select new DetalleOrdenCompraDto
                {
                    Id = i.Id,
                    ComputoId = i.ComputoId,
                    OrdenCompraId = i.OrdenCompraId,
                    porcentaje = i.porcentaje,
                    vigente = i.vigente

                }
            ).ToList();

            if (item != null && item.Count >= 0)
            {
               sumaporcentaje = (from x in item select x.porcentaje).Sum();  // sum: 19
                
            }

            return sumaporcentaje;
        }

        public decimal comprobarporcentaje(int ComputoId)
        {
            var datos = Repository.GetAllIncluding(c => c.OrdenCompra, c => c.Computo);
            decimal sumaporcentaje = 0;
            var item = (from i in datos
                where i.ComputoId == ComputoId
                where i.vigente==true
                select new DetalleOrdenCompraDto
                {
                    Id = i.Id,
                    ComputoId = i.ComputoId,
                    OrdenCompraId = i.OrdenCompraId,
                    porcentaje = i.porcentaje,
                    vigente = i.vigente

                }
            ).ToList();

            if (item != null && item.Count >= 0)
            {
                sumaporcentaje = (from x in item select x.porcentaje).Sum();  // sum: 19

            }

            return sumaporcentaje;
        }
    }
}

    
