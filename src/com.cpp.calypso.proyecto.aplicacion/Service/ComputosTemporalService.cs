using Abp.Application.Services.Dto;
using AutoMapper;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.aplicacion.Interfaces;
using com.cpp.calypso.proyecto.dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion
{
    public class ComputosTemporalAsyncBaseCrudAppService :
        AsyncBaseCrudAppService<ComputosTemporal, ComputosTemporalDto, PagedAndFilteredResultRequestDto>, IComputosTemporalAsyncBaseCrudAppService
    {
        private readonly IDetallePreciarioAsyncBaseCrudAppService detallepreciarioService;
        private readonly IComputoAsyncBaseCrudAppService _computoService;
        private readonly IItemAsyncBaseCrudAppService _itemservice;
        private readonly IBaseRepository<Catalogo> repositorycatalogo;
        private readonly IBaseRepository<Preciario> repositorypreciario;
        private readonly IBaseRepository<DetalleAvanceObra> _detalleAvanceObraRepository;
        private readonly IBaseRepository<DetallePreciario> repositorydetallepreciario;
        public readonly IBaseRepository<Item> itemrepository;
        private readonly IBaseRepository<ComputoComercial> _computocomercialrepository;
        private readonly IBaseRepository<ComputoPresupuesto> _computoPresupuestoRepository;
        public ComputosTemporalAsyncBaseCrudAppService(
            IComputoAsyncBaseCrudAppService computoService,
            IBaseRepository<ComputosTemporal> repository,
            IBaseRepository<Catalogo> repositorycatalogo,
            IBaseRepository<Item> itemrepository,
            IBaseRepository<DetallePreciario> repositorydetallepreciario,
            IBaseRepository<Preciario> repositorypreciario,
            IBaseRepository<DetalleAvanceObra> detalleAvanceObraRepository,
            IBaseRepository<ComputoComercial> computocomercialrepository,
             IBaseRepository<ComputoPresupuesto> computoPresupuestoRepository
            ) : base(repository)
        {
            _computoService = computoService;
            this.repositorypreciario = repositorypreciario;
            _detalleAvanceObraRepository = detalleAvanceObraRepository;
            this.itemrepository = itemrepository;
            this.repositorydetallepreciario = repositorydetallepreciario;
            this.repositorycatalogo = repositorycatalogo;
            _computocomercialrepository = computocomercialrepository;
            _computoPresupuestoRepository = computoPresupuestoRepository;
            _itemservice = new ItemServiceAsyncBaseCrudAppService(itemrepository, repositorydetallepreciario, repositorypreciario,repositorycatalogo, _detalleAvanceObraRepository, _computocomercialrepository, _computoPresupuestoRepository);
            detallepreciarioService = new DetallePreciarioServiceAsyncBaseCrudAppService(repositorydetallepreciario, repositorypreciario,itemrepository,repositorycatalogo);
        }

        public List<ComputosTemporalDto> GetComputosTempPorOferta(int OfertaId)
        {
            var computosQuery = Repository.GetAll();
            var computos = (from c in computosQuery
                            where c.Wbs.OfertaId == OfertaId && c.vigente == true
                            select new ComputosTemporalDto
                            {
                                Id = c.Id,
                                WbsId = c.WbsId,
                                Item = c.Item,
                                actividad_nombre = c.Wbs.nivel_nombre,
                                cantidad = c.cantidad,
                                precio_unitario = c.precio_unitario,
                                costo_total = c.costo_total,
                                estado = c.estado,
                                vigente = c.vigente,
                                precio_base = c.precio_base,
                                precio_ajustado = c.precio_ajustado,
                                precio_incrementado = c.precio_incrementado,
                                precio_aplicarse = c.precio_aplicarse,
                                codigo_primavera = c.codigo_primavera,
                                Wbs = c.Wbs,
                                ItemId = c.ItemId,
                                cantidad_eac = c.cantidad_eac,
                                fecha_eac = c.fecha_eac,
                                fecha_registro = c.fecha_registro,
                                fecha_actualizacion = c.fecha_actualizacion,
                                presupuestado = c.presupuestado,
                                diferente = c.cantidad_eac != c.cantidad,
                                total_pu = (c.cantidad * c.precio_unitario),
                                total_pu_aui = (c.cantidad * c.precio_incrementado),
                                item_codigo = c.Item.codigo,
                                item_nombre = c.Item.nombre,
                            }).ToList();

            foreach (var e in computos)
            {
                int padreid = _itemservice.buscaridentificadorpadre(e.Item.item_padre);
                if (padreid != 0)
                {
                    var I = _itemservice.GetDetalle(padreid);
                    e.item_padre_nombre = I.Result.nombre;
                    e.item_padre_codigo = I.Result.codigo;
                }
            }

            return computos;
        }

        public void Actualizar(int id, decimal cantidad_eac, decimal precio_ajustado)
        {
            var computo = Repository.Get(id);
            computo.cantidad = cantidad_eac;
            computo.cantidad_eac = cantidad_eac;
            computo.precio_ajustado = precio_ajustado;
            Repository.Update(computo);
        }

        public async Task NuevaVersionPresupuestoAsync(int OfertaId)
        {

            List<ComputosTemporal> computosTemporales = await Repository.GetAllListAsync();
            foreach(var c in computosTemporales)
            {
                ComputoDto computo = await _computoService.GetDetalle(c.id_computo);

                if (computo == null)
                {
                    ComputoDto compDto = new ComputoDto()
                    {
                        precio_ajustado = c.precio_ajustado,
                        precio_aplicarse = c.precio_aplicarse,
                        precio_base = c.precio_base,
                        cantidad_eac = c.cantidad_eac,
                        codigo_primavera = c.codigo_primavera,
                        cantidad = c.cantidad,
                        WbsId = c.WbsId,
                        ItemId = c.ItemId,
                        precio_unitario = c.precio_unitario,
                        costo_total =c.costo_total,
                        estado = c.estado,
                        vigente = c.vigente,
                        precio_incrementado =c.precio_incrementado,
                        fecha_registro = c.fecha_registro,
                        fecha_actualizacion = c.fecha_actualizacion,
                        fecha_eac = c.fecha_eac,
                        presupuestado = c.presupuestado

                    };
                     await _computoService.Create(compDto);
                }
                else
                {
                    computo.precio_ajustado = c.precio_ajustado;
                    computo.precio_aplicarse = c.precio_aplicarse;
                    computo.precio_base = c.precio_base;
                    computo.cantidad_eac = c.cantidad_eac;
                    computo.codigo_primavera = c.codigo_primavera;
                    computo.cantidad = c.cantidad;
                    computo.WbsId = c.WbsId;
                    computo.ItemId = c.ItemId;
                    computo.precio_unitario = c.precio_unitario;
                    computo.costo_total = c.costo_total;
                    computo.estado = c.estado;
                    computo.vigente = c.vigente;
                    computo.precio_incrementado = c.precio_incrementado;
                    computo.fecha_registro = c.fecha_registro;
                    computo.fecha_actualizacion = c.fecha_actualizacion;
                    computo.fecha_eac = c.fecha_eac;
                    computo.presupuestado = c.presupuestado;
                    await _computoService.Update(computo);
                }
                Repository.Delete(c.Id);
            }
        }

        public List<ComputosTemporalDto> GenerarNuevaVersion(int OfertaId)
        {
               var computos = _computoService.GetComputosPorOferta(OfertaId);
            foreach (var i in computos)
            {
                ComputosTemporal compTemp = new ComputosTemporal()
                {
                    OfertaId = OfertaId,
                    id_computo = i.Id,
                    WbsId = i.WbsId,
                    ItemId = i.ItemId,
                    cantidad = i.cantidad,
                    precio_unitario = i.precio_unitario,
                    costo_total = i.costo_total,
                    estado = i.estado,
                    vigente = i.vigente,
                    precio_base = i.precio_base,
                    precio_incrementado = i.precio_incrementado,
                    precio_ajustado = i.precio_ajustado,
                    precio_aplicarse = i.precio_aplicarse,
                    codigo_primavera = i.codigo_primavera,
                    fecha_registro = i.fecha_registro,
                    fecha_actualizacion = i.fecha_actualizacion,
                    cantidad_eac = i.cantidad_eac,
                    fecha_eac = i.fecha_eac,
                    presupuestado = i.presupuestado
                };
                Repository.Insert(compTemp);
            }

            return GetComputosTempPorOferta(OfertaId);
        }

        public void CalcularPresupuesto(int ContratoId, DateTime FechaOferta)
        {
            //Recupero Items del Contrato y Fecha Oferta
            var Items = _itemservice.GetItemsporContratoActivo(ContratoId, FechaOferta);

            // Saco el Listado de los Computos
            var listacomputos = Repository.GetAllIncluding(c => c.Item, c => c.Wbs, c => c.Wbs.Oferta,
                c => c.Wbs.Oferta.Requerimiento, c => c.Wbs.Oferta.Requerimiento.Proyecto,
                c => c.Wbs.Oferta.Requerimiento.Proyecto.Contrato).Where(c => c.vigente == true).ToList();

            foreach (var computo in listacomputos) //recorro la lista de computos
            {
                //comparo contrato Id de un solo contrato
                if (computo.Wbs.Oferta.Requerimiento.Proyecto.Contrato.Id == ContratoId)
                {
                    // recorro items
                    foreach (var item in Items)
                    {
                        //comparo si el item en computo es igual a un item de la lista de items por contrato y fecha.
                        if (computo.ItemId == item.Id)
                        {
                            //recupero pu del preciario
                            Decimal pu = detallepreciarioService.ObtenerPrecioUnitario(Mapper.Map<Item>(item));

                            // precio base es igual al preciario.
                            Decimal pbase = pu;

                            // Por Hacer Suma de Porcentajes
                            Decimal sumaporcentajes = Convert.ToDecimal(0.05 + 0.03);

                            //Cálculo del Precio Incrementado
                            Decimal pincrementado =
                                detallepreciarioService.ObtenerPrecioIncrementado(item, pbase, sumaporcentajes);

                            computo.precio_incrementado = pincrementado;
                            computo.precio_base = pbase;
                            Repository.Update(computo);
                        }
                    }
                }
            }
        }


        public bool comprobarexistecomputo(int WbsOfertaId)
        {
            bool si = false;
            var registro = Repository.GetAll().ToList();
            foreach (var item in registro)
            {
                if (item.WbsId == WbsOfertaId && item.vigente == true)
                {
                    si = true;
                }
            }

            return si;
        }

        public bool comprobarexistenciaitem(int WbsOfertaId, int ItemId)
        {
            bool encontrado = false;
            var lista = GetComputosporWbsOferta(WbsOfertaId);
            foreach (var item in lista)
            {
                if (item.ItemId == ItemId)
                {
                    encontrado = true;
                }
            }

            return encontrado;
        }

        public List<ComputosTemporalDto> GetComputosporOferta(int OfertaId)
        {
            var computosQuery = Repository.GetAllIncluding(o => o.Wbs.Oferta, o => o.Item);
            var computos = (from c in computosQuery
                            where c.Wbs.OfertaId == OfertaId && c.vigente == true
                            select new ComputosTemporalDto
                            {
                                Id = c.Id,
                                WbsId = c.WbsId,
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
                                codigo_primavera = c.codigo_primavera,
                                Wbs = c.Wbs,
                                ItemId = c.ItemId,
                                cantidad_eac = c.cantidad_eac,
                                fecha_eac = c.fecha_eac,
                                fecha_registro = c.fecha_registro,
                                fecha_actualizacion = c.fecha_actualizacion,
                                presupuestado = c.presupuestado

                            }).ToList();
            
            return computos;
        }

        public List<ComputosTemporalDto> GetComputosporWbsOferta(int WbsOfertaId, DateTime? fecha = null) // Fecha apra 
        {
            var computosQuery = Repository.GetAll();
            var computos = (from c in computosQuery
                            where c.WbsId == WbsOfertaId
                            where c.vigente == true
                            select new ComputosTemporalDto
                            {
                                Id = c.Id,
                                WbsId = c.WbsId,
                                cantidad = c.cantidad,
                                precio_unitario = c.precio_unitario,
                                costo_total = c.costo_total,
                                estado = c.estado,
                                vigente = c.vigente,
                                Wbs = c.Wbs,
                                precio_base = c.precio_base, //Nuevos//
                                precio_ajustado = c.precio_ajustado,
                                precio_aplicarse = c.precio_aplicarse,
                                precio_incrementado = c.precio_incrementado,
                                ItemId = c.ItemId,
                                Item = c.Item,
                                item_codigo = c.Item.codigo,
                                item_nombre = c.Item.nombre,
                                fecha_registro = c.fecha_registro.Value,
                                fecha_actualizacion = c.fecha_actualizacion.Value,
                                codigo_primavera = c.codigo_primavera,
                                cantidad_eac = c.cantidad_eac,
                                fecha_eac = c.fecha_eac
                            }).ToList();

            foreach (var e in computos)
            {
                int padreid = _itemservice.buscaridentificadorpadre(e.Item.item_padre);
                if (padreid != 0)
                {
                    var I = _itemservice.GetDetalle(padreid);
                    e.item_padre_nombre = I.Result.nombre;
                }
            }
            return computos;
        }

        public async Task<ComputosTemporalDto> GetDetalle(int ComputoId)
        {
            var computo = Repository.GetAll();
            ComputosTemporalDto item = (from c in computo

                               where c.Id == ComputoId
                               select new ComputosTemporalDto
                               {
                                   Id = c.Id,
                                   WbsId = c.WbsId,
                                   Wbs = c.Wbs,
                                   Item = c.Item,
                                   ItemId = c.ItemId,
                                   precio_unitario = c.precio_unitario,
                                   cantidad = c.cantidad,
                                   costo_total = c.costo_total,
                                   estado = c.estado,
                                   vigente = c.vigente,
                                   precio_base = c.precio_base, //Nuevos//
                                   precio_ajustado = c.precio_ajustado,
                                   precio_aplicarse = c.precio_aplicarse,
                                   precio_incrementado = c.precio_incrementado,
                                   codigo_primavera = c.codigo_primavera,
                                   fecha_registro = c.fecha_registro,
                                   fecha_actualizacion = c.fecha_actualizacion,
                                   cantidad_eac = c.cantidad_eac,
                                   fecha_eac = c.fecha_eac,
                                   OfertaId = c.OfertaId

                               }).FirstOrDefault();
            return item;
        }

        public string nombrecatalogo(int tipocatagoid)
        {
            string nombre = "";
            var lista = repositorycatalogo.GetAllIncluding(c => c.TipoCatalogo).Where(c => c.vigente == true);

            foreach (var item in lista)
            {
                if (item.Id == tipocatagoid)
                {
                    nombre = item.nombre;

                }
            }

            return nombre;

        }

        public ComputoDto ActualizarprecioAjustado(int PreciarioId, ComputoDto seleccionado)
        {
            var detallesQuery = repositorydetallepreciario.GetAllIncluding(c => c.Preciario, c => c.Item);
            var items = (from c in detallesQuery
                         where c.PreciarioId == PreciarioId
                         where c.vigente == true
                         select new DetallePreciarioDto
                         {
                             Id = c.Id,
                             Preciario = c.Preciario,
                             PreciarioId = c.PreciarioId,
                             Item = c.Item,
                             ItemId = c.ItemId,
                             comentario = c.comentario,
                             precio_unitario = c.precio_unitario,
                             vigente = c.vigente,


                         }).ToList();

            var item = (from c in items
                        where c.ItemId == seleccionado.ItemId
                        where c.vigente == true
                        select new DetallePreciarioDto
                        {

                            Id = c.Id,
                            ItemId = c.ItemId,
                            precio_unitario = c.precio_unitario,
                            PreciarioId = c.PreciarioId,
                            vigente = true

                        }).FirstOrDefault();


            if (seleccionado.precio_ajustado > 0)
            {


                Decimal precioaj = seleccionado.precio_ajustado;



                seleccionado.precio_unitario = precioaj;
                seleccionado.precio_aplicarse = "precio ajus";
                seleccionado.costo_total = 0;
                if (item != null && item.precio_unitario <= 0)
                {
                    item.precio_unitario = precioaj;
                    repositorydetallepreciario.Update(AutoMapper.Mapper.Map<DetallePreciario>(item));
                }
                // Repository.Update(MapToEntity(seleccionado));

            }
            else if (seleccionado.precio_ajustado <= 0)
            {
                if (seleccionado.precio_incrementado > 0)
                {
                    seleccionado.precio_aplicarse = "precio AIU";
                }
                else
                {
                    seleccionado.precio_aplicarse = "";

                }
                seleccionado.precio_unitario = seleccionado.precio_incrementado;
                seleccionado.costo_total = 0;
                //Repository.Update(MapToEntity(seleccionado));
            }

            return seleccionado;
        }

        public String ActualizarCostoTotal(int ofertaid, int ContratoId, int PreciarioId, DateTime FechaOferta, Boolean Validado)
        {

            var detallesQuery = repositorydetallepreciario.GetAllIncluding(c => c.Preciario, c => c.Item);
            var items = (from c in detallesQuery
                         where c.PreciarioId == PreciarioId
                         where c.vigente == true
                         select new DetallePreciarioDto
                         {
                             Id = c.Id,
                             Preciario = c.Preciario,
                             PreciarioId = c.PreciarioId,
                             Item = c.Item,
                             ItemId = c.ItemId,
                             comentario = c.comentario,
                             precio_unitario = c.precio_unitario,
                             vigente = c.vigente,


                         }).ToList();

            String resultado = "";
            if (Validado)
            {
                var computos = this.GetComputosporOferta(ofertaid);

                if (items.Count > 0)
                {
                    foreach (var icomp in computos)
                    {
                        var item = (from c in items
                                    where c.ItemId == icomp.ItemId
                                    where c.vigente == true
                                    select new DetallePreciarioDto
                                    {

                                        Id = c.Id,
                                        ItemId = c.ItemId,
                                        precio_unitario = c.precio_unitario,
                                        PreciarioId = c.PreciarioId,
                                        vigente = true

                                    }).FirstOrDefault();

                        if (item != null && item.precio_unitario > 0)
                        {
                            //Decimal sumaporcentajes = Convert.ToDecimal(0.05 + 0.03);
                            icomp.precio_incrementado = item.precio_unitario;
                            icomp.precio_base = icomp.precio_incrementado;
                            //Cálculo del Costo Total
                            Decimal costototal = 0;

                            if (icomp.precio_ajustado > 0)
                            {

                                costototal = icomp.cantidad * icomp.precio_ajustado;
                                icomp.precio_unitario = icomp.precio_ajustado;
                                icomp.precio_aplicarse = "precio ajus";
                            }
                            else
                            {
                                costototal = icomp.cantidad * icomp.precio_incrementado;
                                icomp.precio_aplicarse = "precio AIU";
                                icomp.precio_unitario = icomp.precio_incrementado;
                            }

                            icomp.costo_total = costototal;
                            //  Repository.Update(MapToEntity(icomp));

                        }
                        else
                        {

                            resultado = "EL precio unitario del Item: " + icomp.Item.codigo + " " +
                                        icomp.Item.nombre +
                                        " es cero verifique su preciario o incluya un precio ajustado";
                            return resultado;
                        }
                    }


                }
            }

            return resultado;

        }
        public string nombrecatalogo2(int tipocatagoid)
        {
            var a = repositorycatalogo.Get(tipocatagoid);

            return a.nombre;

        }

        public List<Item> GetComputosporOfertaNovalidos(int OfertaId)
        {
            List<ComputoDto> novalidos = new List<ComputoDto>();
            var computosQuery = Repository.GetAll();
            var computos = (from c in computosQuery
                            where c.Wbs.OfertaId == OfertaId && c.vigente == true
                            select new ComputoDto
                            {
                                Id = c.Id,
                                WbsId = c.WbsId,
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
                                Wbs = c.Wbs,
                                cantidad_eac = c.cantidad_eac,
                                fecha_eac = c.fecha_eac,
                            }).ToList();
            /* foreach (var e in computos)
             {
                 e.nombrearea = nombrecatalogo2(e.Wbs.AreaId);
                 e.nombreelemento = nombrecatalogo2(e.Wbs.ElementoId);
                 e.nombreactividad = nombrecatalogo2(e.Wbs.ActividadId);
                 e.nombrediciplina = nombrecatalogo2(e.Wbs.DisciplinaId);

             }
             */
            foreach (var novalido in computos)
            {
                if (novalido.Item.item_padre == "validar" || novalido.Item.codigo == "validar")
                {
                    novalidos.Add(novalido);
                }


            }
            List<Item> a = new List<Item>();

            foreach (var item in novalidos)
            {
                a.Add(item.Item);
            }


            return a;
        }

        public List<TreeWbs> TreeComputo(int WbsOfertaId)
        {
            //listacomputo//
            List<ComputosTemporalDto> lc = this.GetComputosporWbsOferta(WbsOfertaId);
            var Lista = new List<TreeWbs>();

            foreach (var r in lc)
            {
                var jcomputo = new TreeWbs()
                {
                    label = r.Item.codigo + " " + r.Item.nombre,
                    data = r.Id + "!" + r.cantidad + "!" + r.precio_unitario + "!" + r.costo_total + "!" +
                               r.item_padre_nombre + "!" + r.Item.codigo + "!" + r.Item.nombre + "!" +
                               "na" + "!" + "na" + "!" + r.cantidad_eac,
                    icon = "fa fa-fw fa-file-word-o",
                    tipo = "computo",
                    
                    nombres = r.Item.codigo + ", " + r.Item.nombre

                };


                Lista.Add(jcomputo);
            }
            return Lista;

        }

        public async Task<bool> EliminarVigencia(int ComputoId)

        {
            bool resul = false;
            var proyecto = await GetDetalle(ComputoId);
            if (proyecto != null)
            {
                proyecto.vigente = false;
                Repository.Update(MapToEntity(proyecto));
                resul = true;
            }

            return resul;
        }

        public bool EditarCantidadComputoTemporal(int id, decimal cantidad, decimal cantidad_eac)
        {
            ComputosTemporal a = Repository.Get(id);
            a.cantidad = cantidad;
            a.costo_total = 0;
            if (cantidad_eac == 0)
            {
                a.cantidad_eac = cantidad;
            }
            else
            {
                a.cantidad_eac = cantidad_eac;
            }
            a.fecha_actualizacion = DateTime.Now;
            var resultado = Repository.Update(a);
            if (resultado.Id > 0)
            {
                return true;
            }

            return false;
        }

        public List<ComputoDto> GetComputosPorOferta(int id)
        {
            var listacomputos = Repository.GetAllIncluding(c => c.Wbs, c => c.Wbs.Oferta,
                c => c.Wbs.Oferta.Proyecto, c => c.Item).Where(c => c.vigente == true).ToList();
            var computos = (from c in listacomputos
                            where c.Wbs.Oferta.Id == id && c.vigente == true
                            select new ComputoDto
                            {
                                Id = c.Id,
                                actividad_nombre = c.Wbs.nivel_nombre,
                                oferta = c.Wbs.Oferta.codigo,
                                item_codigo = c.Item.codigo,
                                item_nombre = c.Item.nombre,
                                cantidad = c.cantidad,
                                cantidad_eac = c.cantidad_eac,
                                costo_total = c.costo_total,
                                precio_unitario = c.precio_unitario,
                                Item = c.Item,
                                precio_incrementado = c.precio_incrementado,
                                precio_ajustado = c.precio_ajustado,
                                diferente = c.cantidad_eac != c.cantidad,
                                total_pu = (c.cantidad * c.precio_unitario),
                                total_pu_aui = (c.cantidad * c.precio_incrementado)

                            }).ToList();

            foreach (var e in computos)
            {
                int padreid = _itemservice.buscaridentificadorpadre(e.Item.item_padre);
                if (padreid != 0)
                {
                    var I = _itemservice.GetDetalle(padreid);
                    e.item_padre_nombre = I.Result.nombre;
                    e.item_padre_codigo = I.Result.codigo;
                }
            }

            return computos;
        }

        public List<ComputoDto> GetComputosPorOfertaList(int[] computos)
        {
            var listacomputos = Repository.GetAllIncluding(c => c.Wbs, c => c.Wbs.Oferta,
                c => c.Wbs.Oferta.Proyecto, c => c.Item).Where(c => c.vigente == true).ToList();
            List<ComputoDto> computosList = new List<ComputoDto>();

            var list = (from c in listacomputos
                        where computos.Contains(c.Id)
                        select new ComputoDto
                        {
                            Id = c.Id,
                            actividad_nombre = c.Wbs.nivel_nombre,
                            oferta = c.Wbs.Oferta.codigo,
                            item_codigo = c.Item.codigo,
                            item_nombre = c.Item.nombre,
                            cantidad = c.cantidad_eac,
                            cantidad_eac = c.cantidad_eac,
                            costo_total = c.costo_total,
                            precio_unitario = c.precio_unitario,
                            Item = c.Item,
                            precio_incrementado = c.precio_incrementado,
                            precio_ajustado = c.precio_ajustado,
                            diferente = c.cantidad_eac != c.cantidad,
                            total_pu = (c.cantidad * c.precio_unitario),
                            total_pu_aui = (c.cantidad * c.precio_incrementado)

                        }).ToList();

            foreach (var e in list)
            {
                int padreid = _itemservice.buscaridentificadorpadre(e.Item.item_padre);
                if (padreid != 0)
                {
                    var I = _itemservice.GetDetalle(padreid);
                    e.item_padre_nombre = I.Result.nombre;
                    e.item_padre_codigo = I.Result.codigo;
                }
            }

            return list;
        }

        public ComputoDto GetCabeceraApi(int id) //OfertaId
        {
            decimal monto_construccion = 0;
            decimal monto_ingenieria = 0;
            decimal monto_procura = 0;
            string monto_total = "";
            string clase = "";

            var computo = Repository.GetAllIncluding(c => c.Wbs, c => c.Wbs.Oferta,
                c => c.Wbs.Oferta.Proyecto, c => c.Item).Where(c => c.vigente == true).ToList();

            var computos = (from c in computo
                            where c.Wbs.Oferta.Id == id && c.vigente == true
                            select new ComputoDto
                            {
                                Id = c.Id,
                                actividad_nombre = c.Wbs.nivel_nombre,
                                oferta = c.Wbs.Oferta.codigo,
                                item_codigo = c.Item.codigo,
                                item_nombre = c.Item.nombre,
                                cantidad = c.cantidad,
                                cantidad_eac = c.cantidad_eac,
                                costo_total = c.costo_total,
                                precio_unitario = c.precio_unitario,
                                Item = c.Item,
                                clase = c.Wbs.Oferta.ClaseId.ToString(),
                                monto_total = c.Wbs.Oferta.monto_ofertado.ToString()
                            }).ToList();

            foreach (var e in computos)
            {
                clase = "Clase " + e.clase;
                monto_total = e.monto_total;
                if (e.Item.GrupoId >= 2 && e.Item.GrupoId <= 6)
                {
                    monto_construccion += e.costo_total;
                }
                else
                {
                    if (e.Item.GrupoId == 1)
                    {
                        monto_ingenieria += e.costo_total;
                    }
                    else
                    {
                        if (e.Item.GrupoId == 7)
                        {
                            monto_procura += e.costo_total;
                        }
                    }
                }
            }

            ComputoDto comp = new ComputoDto()
            {
                monto_total = monto_total,
                clase = clase,
                monto_construccion = monto_construccion + "",
                monto_procura = monto_procura + "",
                monto_ingenieria = monto_ingenieria + ""
            };

            return comp;
        }


        public bool ActualizarValorEac(int idComputo, decimal valorEac)
        {
            bool resul = false;
            var computo = Repository.Get(idComputo);
            if (computo != null)
            {
                computo.cantidad_eac = valorEac;
                computo.fecha_actualizacion = DateTime.Now;
                Repository.Update(computo);
                resul = true;
            }
            return resul;
        }

        public List<ComputoDto> GrupoComputosporOferta(int OfertaId)
        {
            var computosQuery = Repository.GetAll();
            var computos = (from c in computosQuery
                            where c.Wbs.OfertaId == OfertaId && c.vigente == true
                            select new ComputoDto
                            {
                                Id = c.Id,
                                WbsId = c.WbsId,
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
                                Wbs = c.Wbs,
                                ItemId = c.ItemId,
                                cantidad_eac = c.cantidad_eac,
                                fecha_eac = c.fecha_eac,

                            }).ToList();
            foreach (var e in computos)
            {
                if (e.Item.UnidadId != 0)
                {
                    e.nombre_unidad = nombrecatalogo2(e.Item.UnidadId);

                }
            }

            return computos;
        }

    }
}
