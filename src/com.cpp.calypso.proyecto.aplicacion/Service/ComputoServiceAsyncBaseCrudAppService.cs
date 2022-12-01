
using AutoMapper;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.aplicacion.Interfaces;
using com.cpp.calypso.proyecto.dominio;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using OfficeOpenXml.Style;
using System.Drawing;
using System.Web;
namespace com.cpp.calypso.proyecto.aplicacion
{
    public class ComputoServiceAsyncBaseCrudAppService :
        AsyncBaseCrudAppService<Computo, ComputoDto, PagedAndFilteredResultRequestDto>, IComputoAsyncBaseCrudAppService
    {

        private readonly IItemAsyncBaseCrudAppService _itemservice;
        private readonly IBaseRepository<Catalogo> repositorycatalogo;
        private readonly IBaseRepository<Preciario> repositorypreciario;
        public readonly IBaseRepository<Item> itemrepository;
        private readonly IDetallePreciarioAsyncBaseCrudAppService detallepreciarioService;
        private readonly IBaseRepository<DetalleAvanceObra> _detalleAvanceObraRepository;
        private readonly IBaseRepository<DetallePreciario> repositorydetallepreciario;
        private readonly IBaseRepository<Wbs> _wbsrepository;
        private readonly IBaseRepository<ComputoComercial> _computocomercialrepository;

        private readonly IBaseRepository<Ganancia> _gananciarepository;
        public readonly IBaseRepository<DetalleGanancia> _detallegananciarepository;
        public readonly IBaseRepository<ComputoPresupuesto> _computoPresupuestorepository;
        public ComputoServiceAsyncBaseCrudAppService(
            IBaseRepository<Item> itemrepository,
            IBaseRepository<Computo> repository,
            IBaseRepository<Catalogo> repositorycatalogo,

            IBaseRepository<DetallePreciario> repositorydetallepreciario,
            IBaseRepository<Preciario> repositorypreciario,
            IBaseRepository<DetalleAvanceObra> detalleAvanceObraRepository,
            IBaseRepository<Wbs> wbsrepository,
            IBaseRepository<Ganancia> gananciarepository,
            IBaseRepository<DetalleGanancia> detallegananciarepository,
            IBaseRepository<ComputoComercial> computocomercialrepository,
             IBaseRepository<ComputoPresupuesto> computoPresupuestorepository
        ) : base(repository)
        {
            this.repositorypreciario = repositorypreciario;
            _detalleAvanceObraRepository = detalleAvanceObraRepository;
            this.itemrepository = itemrepository;
            this.repositorydetallepreciario = repositorydetallepreciario;
            this.repositorycatalogo = repositorycatalogo;
            _computocomercialrepository = computocomercialrepository;
            _computoPresupuestorepository = computoPresupuestorepository;
            _itemservice = new ItemServiceAsyncBaseCrudAppService(itemrepository, repositorydetallepreciario,
                repositorypreciario, repositorycatalogo, detalleAvanceObraRepository, _computocomercialrepository, _computoPresupuestorepository);
            detallepreciarioService =
                new DetallePreciarioServiceAsyncBaseCrudAppService(repositorydetallepreciario, repositorypreciario, itemrepository, repositorycatalogo);

            _wbsrepository = wbsrepository;
            _gananciarepository = gananciarepository;
            _detallegananciarepository = detallegananciarepository;
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

            var lista = Repository.GetAll().Where(c => c.WbsId == WbsOfertaId).Where(c => c.ItemId == ItemId).Where(
                c => c.vigente == true).ToList();
            if (lista.Count > 0)
            {
                encontrado = true;
            }

            return encontrado;
        }

        public List<ComputoDto> GetComputosporOferta(int OfertaId)
        {
            var computosQuery = Repository.GetAllIncluding(o => o.Wbs.Oferta, o => o.Item);
            var computos = (from c in computosQuery
                            where c.Wbs.OfertaId == OfertaId
                            where c.vigente == true
                            select new ComputoDto
                            {
                                Id = c.Id,
                                WbsId = c.WbsId,

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
                                Item = c.Item,
                                cantidad_eac = c.cantidad_eac,
                                fecha_eac = c.fecha_eac,
                                fecha_registro = c.fecha_registro,
                                fecha_actualizacion = c.fecha_actualizacion,
                                presupuestado = c.presupuestado,
                                codigo_item_alterno = c.codigo_item_alterno

                            }).ToList();
            foreach (var e in computos)
            {
                var name = itemrepository.GetAll().Where(o => o.vigente == true).Where(o => o.codigo == e.Item.item_padre).FirstOrDefault();

                if (name != null && name.Id > 0)
                {
                    e.item_padre_nombre = name.nombre;
                    e.nombre_unidad = nombrecatalogo2(e.Item.UnidadId);

                }
            }

            return computos;
        }

        public List<ComputoDto> GetComputosporWbsOferta(int WbsOfertaId, DateTime? fecha = null) // Fecha apra 
        {
            var computosQuery = Repository.GetAll().Where(c=>c.WbsId==WbsOfertaId).Where(c=>c.vigente);
            var computos = (from c in computosQuery
                            where c.WbsId == WbsOfertaId
                            where c.vigente == true
                            select new ComputoDto
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
                                fecha_eac = c.fecha_eac,
                                codigo_item_alterno = c.codigo_item_alterno,
                                es_temporal=c.es_temporal,
                                cantidadAjustada=c.cantidadAjustada,
                                tipo=c.tipo
                            }).ToList();

            foreach (var e in computos)
            {
                if (fecha != null)
                {
                    e.cantidad_acumulada_anterior = this.ObtenerCantidadAcumulada(e.Id, fecha.Value);
                }

                if (e.Item != null && e.Id > 0)
                {
                    var name = itemrepository.GetAll().Where(o => o.vigente == true).Where(o => o.codigo == e.Item.item_padre).FirstOrDefault();

                    if (name != null && name.Id > 0)
                    {
                        e.item_padre_nombre = name.nombre;
                        e.nombre_unidad = nombrecatalogo2(e.Item.UnidadId);

                    }

                    /*
                    int padreid = _itemservice.buscaridentificadorpadre(e.Item.item_padre);
                    if (padreid != 0)
                    {
                        var I = _itemservice.GetDetalle(padreid);
                        e.item_padre_nombre = I.Result.nombre;
                        e.nombre_unidad = nombrecatalogo2(I.Result.UnidadId);

                    }*/

                }
            }

            return computos;
        }

        decimal ObtenerCantidadAcumulada(int computoId, DateTime fecha_reporte)
        {
            decimal cantidad_acumulada = 0;
            var query = _detalleAvanceObraRepository.GetAllIncluding(o => o.AvanceObra)
                .Where(o => o.vigente == true)
                .Where(o => o.ComputoId == computoId)
                .Where(o => o.AvanceObra.aprobado == true)
                .Where(o => o.AvanceObra.fecha_presentacion < fecha_reporte);

            var detalles = (from d in query
                            select new DetalleAvanceObraDto()
                            {
                                cantidad_diaria = d.cantidad_diaria,
                                fecha_registro = d.fecha_registro,
                                total = d.total
                            }).ToList();

            foreach (var d in detalles)
            {
                cantidad_acumulada += d.cantidad_diaria;
            }

            return cantidad_acumulada;
        }

        public async Task<ComputoDto> GetDetalle(int ComputoId)
        {
            var computo = Repository.GetAll();
            ComputoDto item = (from c in computo

                               where c.Id == ComputoId
                               select new ComputoDto
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

                               }).FirstOrDefault();



            return item;
        }

        public string nombrecatalogo(int tipocatagoid)
        {
            var a = repositorycatalogo.Get(tipocatagoid);
            if (a != null && a.Id > 0)
            {
                return a.nombre;
            }
            else
            {
                return "";
            }

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

                Repository.Update(MapToEntity(seleccionado));

            }
            else if (seleccionado.precio_ajustado <= 0)
            {
                if (seleccionado.precio_base > 0)
                {
                    seleccionado.precio_aplicarse = "precio base";
                }
                else
                {
                    seleccionado.precio_aplicarse = "";

                }

                seleccionado.precio_unitario = seleccionado.precio_base;
                seleccionado.costo_total = 0;
                Repository.Update(MapToEntity(seleccionado));
            }


            return seleccionado;
        }

        public string ActualizarCostoTotal(int ofertaid, int ContratoId, int PreciarioId, DateTime FechaOferta,
            Boolean Validado)
        {
            String resultado = "";
            //

            decimal gananciacontruccion = 1;
            decimal gananciaprocura = 1;
            if (Validado)
            {

                //calculo de ganancia
                var ganancia = _gananciarepository.GetAll().Where(c => c.ContratoId == ContratoId)
                    .Where(c => c.fecha_inicio <= FechaOferta)
                    .Where(c => c.fecha_fin >= FechaOferta).Where(c => c.vigente == true).FirstOrDefault();

                if (ganancia != null && ganancia.Id > 0)
                {
                    var detalleganancia = _detallegananciarepository.GetAll().Where(c => c.GananciaId == ganancia.Id).
                         Where(c => c.vigente == true).ToList();

                    if (detalleganancia.Count > 0)
                    {

                        gananciacontruccion = (from e in detalleganancia where e.GrupoItemId == 2 select e.valor).Sum();
                        gananciaprocura = (from e in detalleganancia where e.GrupoItemId == 3 select e.valor).Sum();

                    }

                }


                var computos = this.GetComputosporOferta(ofertaid);

                if (computos.Count > 0 && PreciarioId > 0)
                {

                    //detallespreciario

                    var itemspreciario = repositorydetallepreciario.GetAll().Where(c => c.PreciarioId == PreciarioId).Where(e => e.vigente == true).ToList();

                    foreach (var item in computos)
                    {

                        var preciounitario = (from e in itemspreciario
                                              where e.ItemId == item.ItemId
                                              where e.vigente == true
                                              select e.precio_unitario).FirstOrDefault();

                        if (preciounitario > 0)
                        {




                            item.precio_base = preciounitario;

                            if (item.Item.GrupoId == 1)
                            {

                            }

                            if (item.Item.GrupoId == 2)
                            {
                                if (gananciacontruccion > 1)
                                {
                                    item.precio_incrementado = item.precio_unitario * (gananciacontruccion / 100);
                                }
                            }

                            if (item.Item.GrupoId == 3)
                            {
                                if (gananciaprocura > 1)
                                {
                                    item.precio_incrementado = item.precio_unitario * (gananciaprocura / 100);
                                }
                            }

                            Decimal costototal = 0;

                            if (item.precio_ajustado > 0)
                            {

                                costototal = item.cantidad * item.precio_ajustado;
                                item.precio_unitario = item.precio_ajustado;
                                item.precio_aplicarse = "precio ajus";
                            }
                            else
                            {
                                costototal = item.cantidad * item.precio_base; // multiplicas por precio ba
                                //costototal = icomp.cantidad * icomp.precio_incrementado;
                                item.precio_aplicarse = "precio base";
                                item.precio_unitario = item.precio_base;
                            }

                            item.costo_total = costototal;
                            item.fecha_registro = item.fecha_registro;
                            item.fecha_actualizacion = item.fecha_actualizacion;
                            item.precio_incrementado = item.precio_incrementado;
                            Repository.Update(MapToEntity(item));

                        }

                        else
                        {
                            resultado = "EL precio unitario del Item: " + item.Item.codigo + " " +
                                        item.Item.nombre +
                                        " es cero verifique su preciario o incluya un precio ajustado";

                        }
                    }

                }

            }

            return resultado;

        }

        public string nombrecatalogo2(int tipocatagoid)
        {
            var a = repositorycatalogo.Get(tipocatagoid);
            if (a != null && a.Id > 0)
            {
                return a.nombre;
            }
            else
            {
                return "";
            }
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

        public List<TreeWbsComputo> TreeComputo(int WbsOfertaId)
        {
            //listacomputo//
            List<ComputoDto> lc = this.GetComputosporWbsOferta(WbsOfertaId);
            var Lista = new List<TreeWbsComputo>();


            foreach (var r in lc)
            {
                string lab = "";
                string alterno = "";
                if (r.Item.GrupoId == 3)
                {
                    lab = r.codigo_item_alterno + " " + r.Item.nombre;
                }
                else
                {
                    lab = r.Item.codigo + " " + r.Item.nombre;
                }
                if (r.codigo_item_alterno != null && r.codigo_item_alterno.Length > 0)
                {
                    alterno = r.codigo_item_alterno;
                }
                else
                {
                    alterno = r.Item.codigo;
                }
                var jcomputo = new TreeWbsComputo()
                {
                    key = r.Id,
                    label = lab,
                    data = r.Id + "!" + r.cantidad_eac + "!" + r.precio_unitario + "!" + r.costo_total + "!" +
                           r.item_padre_nombre + "!" + alterno + "!" + r.Item.nombre + "!" +
                           r.fecha_registro.GetValueOrDefault().ToShortDateString() + "!" +
                           r.fecha_actualizacion.GetValueOrDefault().ToShortDateString() + "!" + r.cantidad_eac + "!" +
                           nombrecatalogo2(r.Item.UnidadId)+ "!" +r.cantidadAjustada+ "!" +r.tipo,
                    icon = r.es_temporal ? "fa fa-fw fa-clock-o" : "fa fa-fw fa-file-word-o",
                    tipo = "computo",
                    expanded = true,
                    nombres = r.Item.codigo + ", " + r.Item.nombre,
                    selectable = true,


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
                Repository.InsertOrUpdate(MapToEntity(proyecto));
                resul = true;
            }

            return resul;
        }

        public bool EditarCantidadComputo(int id, decimal cantidad, decimal cantidad_eac)
        {
            Computo a = Repository.Get(id);
            a.cantidad_eac = cantidad;
            /*
            a.cantidad = a.cantidad;
            a.costo_total = 0;
            if (cantidad_eac == 0)
            {
                a.cantidad_eac = cantidad;
            }
            else
            {
                a.cantidad_eac = cantidad_eac;
            }
            */
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
                                WbsId = c.WbsId,
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

        public decimal MontoPresupuestoIngenieria(int OfertaId)
        {
            decimal montoingenieria = 0;
            var listacomputos = Repository.GetAllIncluding(c => c.Wbs.Oferta, c => c.Item)
                .Where(c => c.vigente == true).Where(c => c.Wbs.OfertaId == OfertaId).Where(c => c.Item.GrupoId == 1)
                .ToList();

            if (listacomputos != null && listacomputos.Count >= 0)
            {
                montoingenieria = (from x in listacomputos select x.costo_total).Sum(); //Presupuesto Total

            }

            return montoingenieria;
        }

        public decimal MontoPresupuestoConstruccion(int OfertaId)
        {

            decimal montoingenieria = 0;
            var listacomputos = Repository.GetAllIncluding(c => c.Wbs.Oferta, c => c.Item)
                .Where(c => c.vigente == true).Where(c => c.Wbs.OfertaId == OfertaId).Where(c => c.Item.GrupoId == 2)
                .ToList();

            if (listacomputos != null && listacomputos.Count >= 0)
            {
                montoingenieria = (from x in listacomputos select x.costo_total).Sum(); //Presupuesto Total

            }

            return montoingenieria;
        }

        public decimal MontoPresupuestoProcura(int OfertaId)
        {
            decimal montoingenieria = 0;
            var listacomputos = Repository.GetAllIncluding(c => c.Wbs.Oferta, c => c.Item)
                .Where(c => c.vigente == true).Where(c => c.Wbs.OfertaId == OfertaId).Where(c => c.Item.GrupoId == 3)
                .ToList();

            if (listacomputos != null && listacomputos.Count >= 0)
            {
                montoingenieria = (from x in listacomputos select x.costo_total).Sum(); //Presupuesto Total

            }

            return montoingenieria;
        }
        public decimal sumacantidades(int OfertaId, int ItemId)
        {

            var listacomputos = Repository.GetAllIncluding(c => c.Wbs.Oferta.Proyecto, c => c.Item)
                .Where(c => c.vigente == true).Where(c => c.Wbs.OfertaId == OfertaId).Where(c => c.ItemId == ItemId);
            var items = listacomputos.Select(c => c.cantidad).Sum();
            return items;
        }
        public IEnumerable<Item> GetItemDistintosComputos(int OfertaId)
        {

            var listacomputos = Repository.GetAllIncluding(c => c.Wbs.Oferta.Proyecto, c => c.Item)
                .Where(c => c.vigente == true).Where(c => c.Wbs.OfertaId == OfertaId).ToList();
            var items = listacomputos.Select(c => c.Item).Distinct();
            return items.OrderBy(x => x.codigo);
        }




        public ExcelPackage GenerarExcelCabecera(OfertaDto oferta)
        {

            // Datos básicos del documento
            ExcelPackage excelPackage = new ExcelPackage();
            excelPackage.Workbook.Properties.Author = "CPP";
            excelPackage.Workbook.Properties.Title = "Prespuestos";
            excelPackage.Workbook.Properties.Subject = "Generación de Presupuesto";
            excelPackage.Workbook.Properties.Created = DateTime.Now;


            //Crea una hoja de trabajo
            ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("Presupuesto");

            // Lineas de divición
            worksheet.View.ShowGridLines = false;

            // Cabecera de nombres 
            worksheet.Cells["B2:BH2"].Style.Font.SetFromFont(new System.Drawing.Font("Arial", 12, FontStyle.Bold));
            worksheet.Cells["B2:BH2"].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
            worksheet.Cells["B2:BH2"].Style.Border.Bottom.Color.SetColor(Color.Blue);


            worksheet.Cells["B3:B1569"].Style.Font.SetFromFont(new System.Drawing.Font("Arial", 12, FontStyle.Bold));
            worksheet.Cells["B3:B1569"].Style.Border.Left.Style = ExcelBorderStyle.Medium;
            worksheet.Cells["B3:B1569"].Style.Border.Left.Color.SetColor(Color.Blue);
            worksheet.Cells["BH3:BH1569"].Style.Font.SetFromFont(new System.Drawing.Font("Arial", 12, FontStyle.Bold));
            worksheet.Cells["BH3:BH1569"].Style.Border.Right.Style = ExcelBorderStyle.Medium;
            worksheet.Cells["BH3:BH569"].Style.Border.Right.Color.SetColor(Color.Blue);

            worksheet.Cells["B4:D4"].Merge = true;
            worksheet.Cells["B4:D4"].Value =
                " Locación Proyecto #: " + this.nombrecatalogo2(oferta.Proyecto.LocacionId);
            worksheet.Cells["B4:D4"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells["B4:D4"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            worksheet.Cells["B4:D4"].Style.Font.Size = 12;

            worksheet.Cells["C5:D5"].Merge = true;
            worksheet.Cells["C5:D5"].Value = "(" + oferta.Proyecto.codigo + ") " +
                                             oferta.Proyecto.descripcion_proyecto;
            worksheet.Cells["C5:D5"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells["C5:D5"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            worksheet.Cells["C5:D5"].Style.Font.Size = 12;
            worksheet.Cells["BD5:BF5"].Merge = true;
            worksheet.Cells["BD5:BF5"].Value = oferta.codigo;
            worksheet.Cells["BD5:BF5"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells["BD5:BF5"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            worksheet.Cells["BD5:BF5"].Style.Numberformat.Format = DateTimeFormatInfo.CurrentInfo.ShortDatePattern;
            worksheet.Cells["BG5:BH5"].Merge = true;
            worksheet.Cells["BG5:BH5"].Value = "Revisión " + oferta.version;
            worksheet.Cells["BG5:BH5"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells["BG5:BH5"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            worksheet.Cells["BG5:BH5"].Style.Numberformat.Format = DateTimeFormatInfo.CurrentInfo.ShortDatePattern;



            return excelPackage;
        }

        public ExcelPackage CargaMasiva(HttpPostedFileBase UploadedFile, int OfertaId)
        {
            List<String[]> Observaciones = new List<string[]>();

            if (UploadedFile != null)
            {

                // tdata.ExecuteCommand("truncate table OtherCompanyAssets");  
                if (UploadedFile.ContentType == "application/vnd.ms-excel" || UploadedFile.ContentType ==
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                {
                    string fileName = UploadedFile.FileName;
                    string fileContentType = UploadedFile.ContentType;
                    byte[] fileBytes = new byte[UploadedFile.ContentLength];
                    var data = UploadedFile.InputStream.Read(fileBytes, 0,
                        Convert.ToInt32(UploadedFile.ContentLength));

                    using (var package = new ExcelPackage(UploadedFile.InputStream))
                    {
                        var currentSheet = package.Workbook.Worksheets;
                        var workSheet = currentSheet.First();
                        var noOfCol = workSheet.Dimension.End.Column;

                        var noOfRow = workSheet.Dimension.End.Row;
                        for (int i = 5; i <= noOfCol; i++)
                        {
                            for (int rowIterator = 5; rowIterator <= noOfRow; rowIterator++)
                            {
                                var itemid = (workSheet.Cells[rowIterator, 1].Value ?? "").ToString();
                                var wbsid = (workSheet.Cells[3, i].Value ?? "").ToString();
                                var cantidad = (workSheet.Cells[rowIterator, i].Value ?? "").ToString();

                                if (itemid.Length > 0 && wbsid.Length > 0 && cantidad.Length > 0)
                                {


                                    ComputoDto n = new ComputoDto
                                    {

                                        Id = 0,
                                        ItemId = Int32.Parse(itemid),
                                        WbsId = Int32.Parse(wbsid),
                                        cantidad = Decimal.Parse("0" + cantidad),
                                        precio_unitario = 0,
                                        costo_total = 0,
                                        cantidad_eac = 0,
                                        cantidad_acumulada_anterior = 0,
                                        vigente = true,
                                        codigo_primavera = "a",
                                        fecha_registro = DateTime.Now,
                                        precio_ajustado = 0,
                                        precio_base = 0,
                                        precio_incrementado = 0,
                                        estado = true,
                                    };
                                    bool e = this.comprobarexistenciaitem(Int32.Parse(wbsid), Int32.Parse(itemid));
                                    if (!e)
                                    {
                                        var r = Repository.Insert(MapToEntity(n));

                                    }
                                    else
                                    {

                                        var codigo = (workSheet.Cells[rowIterator, 2].Value ?? "").ToString();
                                        String[] error =
                                            {codigo, " Ya existe el registro de ese item  en el Wbs" + wbsid};

                                        Observaciones.Add(error);
                                    }




                                }

                            }
                        }

                        if (Observaciones.Count > 0)
                        {

                            ExcelPackage excel = new ExcelPackage();
                            var errores = excel.Workbook.Worksheets.Add("Observaciones");
                            errores.Cells[1, 1].Value = "Codigo Item";
                            errores.Cells[1, 2].Value = "Observacion";
                            workSheet.Column(1).Width = 20;
                            workSheet.Column(2).Width = 60;
                            var row = 2;
                            foreach (var pitem in Observaciones)
                            {
                                errores.Cells[row, 1].Value = pitem[0].ToString();
                                errores.Cells[row, 2].Value = pitem[1].ToString();

                                row = row + 1;
                            }

                            return excel;

                        }



                    }
                }
            }

            return null;
        }

        public ExcelPackage GenerarExcelCarga(OfertaDto oferta, int nivel_maximo)
        {
            //Lista Wbs
            var query = _wbsrepository.GetAllIncluding(x => x.Catalogo)
                .Where(o => o.vigente == true)
                .Where(o => o.OfertaId == oferta.Id)
                .Where(o => o.es_actividad == true).ToList();

            var wbs = (from w in query
                       select new WbsDto()
                       {
                           Id = w.Id,
                           OfertaId = w.OfertaId,
                           fecha_inicial = w.fecha_inicial,
                           fecha_final = w.fecha_final,
                           id_nivel_padre_codigo = w.id_nivel_padre_codigo,
                           id_nivel_codigo = w.id_nivel_codigo,
                           nivel_nombre = w.nivel_nombre,
                           observaciones = w.observaciones,
                           DisciplinaId = w.DisciplinaId,
                           revision = w.revision,
                           Catalogo = w.Catalogo

                       }).ToList();


            foreach (var w in wbs)
            {
                var name = _wbsrepository
                    .GetAll()
                    .Where(o => o.vigente == true)
                    .Where(o => o.OfertaId == w.OfertaId).SingleOrDefault(o => o.id_nivel_codigo == w.id_nivel_padre_codigo);
                if (name != null)
                {
                    w.nombre_padre = name.nivel_nombre;
                }
            }




            var listacomputos = Repository.GetAllIncluding(x => x.Wbs.Oferta.Proyecto, x => x.Item).Where(x => x.vigente == true).ToList();
            var computos = (from z in listacomputos
                            where z.Wbs.OfertaId == oferta.Id && z.vigente == true
                            select new ComputoDto
                            {
                                Id = z.Id,
                                ItemId = z.ItemId,
                                WbsId = z.WbsId,
                                item_codigo = z.Item.codigo,
                                item_nombre = z.Item.nombre,
                                cantidad = z.cantidad,
                                cantidad_eac = z.cantidad_eac,
                                costo_total = z.costo_total,
                                precio_unitario = z.precio_unitario,
                                Item = z.Item,
                                precio_incrementado = z.precio_incrementado,
                                precio_ajustado = z.precio_ajustado,
                                diferente = z.cantidad_eac != z.cantidad,
                                total_pu = (z.cantidad * z.precio_unitario),
                                total_pu_aui = (z.cantidad * z.precio_incrementado)

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
            // var items = _itemservice.GetItemsporContratoActivo(oferta.Proyecto.contratoId, oferta.fecha_oferta.Value);
            var items = _itemservice.ArbolWbsExcel(oferta.Proyecto.contratoId, oferta.fecha_oferta.GetValueOrDefault());
            ExcelPackage excel = new ExcelPackage();
            var workSheet = excel.Workbook.Worksheets.Add("Rdo");

            workSheet.TabColor = System.Drawing.Color.Azure;
            /*workSheet.Protection.IsProtected = true;
              workSheet.Protection.SetPassword("pmdis");
              */
            workSheet.DefaultRowHeight = 12;

            //Header of table  
            //  

            workSheet.View.ZoomScale = 80;
            int row = nivel_maximo;
            for (int i = 1; i <= row; i++)
            {

                workSheet.Row(i).Style.Font.Bold = true;
            }

            int columna = 6;

            foreach (var itemswbs in wbs.OrderBy(l => l.id_nivel_codigo))
            {


                int fila = nivel_maximo + 4;
                List<Wbs> Jerarquia = new List<Wbs>();

                Wbs item = _wbsrepository.Get(itemswbs.Id);
                Jerarquia.Add(item);
                while (item.id_nivel_padre_codigo != ".")
                {
                    //
                    item = _wbsrepository.GetAll()
                        .Where(X => X.id_nivel_codigo == item.id_nivel_padre_codigo)
                        .Where(C => C.vigente)
                         .Where(C => C.OfertaId == oferta.Id)
                        .FirstOrDefault();

                    Jerarquia.Add(item);
                }
                int a = Jerarquia.Count();
                int rowtyle = nivel_maximo - 1;
                foreach (var wbsj in Jerarquia)
                {
                    if (rowtyle > 0)
                    {
                        workSheet.Cells[rowtyle, columna].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        workSheet.Cells[rowtyle, columna].Style.Fill.BackgroundColor.SetColor(Color.LightSteelBlue);

                        workSheet.Cells[rowtyle, columna].Style.Border.Left.Style =
                        workSheet.Cells[rowtyle, columna].Style.Border.Right.Style = ExcelBorderStyle.Medium;
                        rowtyle--;
                    }
                    if (a > 0)
                    {
                        if (wbsj.es_actividad)
                        {
                            workSheet.Cells[fila - 4, columna].Value = wbsj.nivel_nombre;
                            workSheet.Cells[fila - 4, columna].Value = wbsj.nivel_nombre;
                            workSheet.Column(columna).Width = 25;
                            workSheet.Cells[fila - 4, columna].Style.WrapText = true;
                            workSheet.Cells[fila - 4, columna].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            workSheet.Cells[fila - 4, columna].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            workSheet.Cells[fila - 4, columna].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            workSheet.Cells[fila - 4, columna].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                            workSheet.Cells[fila - 4, columna].Style.Border.Top.Style = workSheet.Cells[fila - 4, columna].Style.Border.Left.Style = workSheet.Cells[fila - 4, columna].Style.Border.Right.Style = workSheet.Cells[fila - 4, columna].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;

                            workSheet.Cells[fila - 2, columna].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            workSheet.Cells[fila - 2, columna].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                        }
                        else
                        {
                            workSheet.Cells[a, columna].Value = wbsj.nivel_nombre;

                            workSheet.Cells[a, columna].Style.WrapText = true;
                            workSheet.Cells[a, columna].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            workSheet.Cells[a, columna].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            workSheet.Cells[a, columna].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            workSheet.Cells[a, columna].Style.Fill.BackgroundColor.SetColor(Color.LightSteelBlue);
                            workSheet.Cells[a, columna].Style.Border.Left.Style =
                            workSheet.Cells[a, columna].Style.Border.Right.Style = ExcelBorderStyle.Medium;
                            /* 
                             var valornuevo = workSheet.Cells[a, columna].Text;
                             var valorantiguo = workSheet.Cells[a, (columna-1)].Text;
                             if (valornuevo == valorantiguo) {
                                 string rango1 = workSheet.Cells[a, columna - 1].Address;
                                 string rango2= workSheet.Cells[a, columna].Address;
                                 string range = rango1 + ":" + rango2;

                                 if(!workSheet.Cells[a, (columna - 1)].Merge) {
                                 workSheet.Cells[range].Merge =true;
                                 workSheet.Cells[range].Value=valornuevo.ToString();

                                 workSheet.Cells[range].Style.Border.Top.Style = 
                                 workSheet.Cells[range].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
                                 }
                             }
                             */
                        }
                    }

                    a--;
                }
                workSheet.Row(fila - 3).Hidden = true;
                workSheet.Cells[fila - 3, columna].Value = itemswbs.Id;
                workSheet.Cells[fila - 3, columna].Style.Fill.PatternType = ExcelFillStyle.Solid;
                workSheet.Cells[fila - 3, columna].Style.Fill.BackgroundColor.SetColor(Color.LightSteelBlue);
                workSheet.Cells[fila - 3, columna].Style.Border.Left.Style =
                       workSheet.Cells[fila - 3, columna].Style.Border.Right.Style = ExcelBorderStyle.Medium;


                if (itemswbs.DisciplinaId >= 0)
                {
                    workSheet.Cells[fila - 2, columna].Value = itemswbs.Catalogo.nombre;
                    workSheet.Cells[fila - 2, columna].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    workSheet.Cells[fila - 2, columna].Style.Fill.BackgroundColor.SetColor(Color.LightSteelBlue);
                    workSheet.Cells[fila - 2, columna].Style.Border.Left.Style =
                       workSheet.Cells[fila - 2, columna].Style.Border.Right.Style = ExcelBorderStyle.Medium;
                    workSheet.Cells[fila - 2, columna].Style.Border.Top.Style = workSheet.Cells[fila - 2, columna].Style.Border.Left.Style = workSheet.Cells[fila - 2, columna].Style.Border.Right.Style = workSheet.Cells[fila - 2, columna].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
                }
                else
                {
                    workSheet.Row(fila - 2).Hidden = true;
                }

                columna = columna + 1;
            }
            workSheet.Cells[nivel_maximo, 1].Value = "Para Oferta";
            workSheet.Cells[nivel_maximo, 2].Value = "Id";
            workSheet.Cells[nivel_maximo, 3].Value = "ITEM";
            workSheet.Cells[nivel_maximo, 4].Value = "DESCRIPCIÓN";
            workSheet.Cells[nivel_maximo, 5].Value = "UNIDAD";


            string rango = workSheet.Cells[nivel_maximo, 2].Address + ":" + workSheet.Cells[nivel_maximo, 5].Address;
            workSheet.Cells[rango].AutoFilter = true;
            for (int i = 2; i <= 5; i++)
            {
                workSheet.Cells[nivel_maximo, i].Style.WrapText = true;
                workSheet.Cells[nivel_maximo, i].Style.Fill.PatternType = ExcelFillStyle.Solid;
                workSheet.Cells[nivel_maximo, i].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                workSheet.Cells[nivel_maximo + 2, i].Style.Fill.PatternType = ExcelFillStyle.Solid;
                workSheet.Cells[nivel_maximo + 2, i].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                workSheet.Cells[nivel_maximo, i].Style.Border.Top.Style = workSheet.Cells[nivel_maximo, i].Style.Border.Left.Style = workSheet.Cells[nivel_maximo, i].Style.Border.Right.Style = workSheet.Cells[nivel_maximo, i].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;

                workSheet.Cells[nivel_maximo, i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Cells[nivel_maximo, i].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            }

            workSheet.Column(3).Width = 15;
            workSheet.Column(4).Width = 80;
            workSheet.Column(5).Width = 15;
            workSheet.Column(1).Style.Font.Bold = true;
            workSheet.Column(1).Hidden = true;
            workSheet.Column(2).Hidden = true;
            workSheet.Row(nivel_maximo).Height = 60;

            //int inicio de las filas
            //int c = 5;
            int c = nivel_maximo + 3;
            workSheet.View.FreezePanes(6, 1);
            workSheet.View.FreezePanes(c, 6);
            foreach (var pitem in items)
            {

                workSheet.Cells[c, 1].Value = pitem.para_oferta;
                workSheet.Cells[c, 2].Value = pitem.Id;
                workSheet.Cells[c, 3].Value = pitem.codigo;
                workSheet.Cells[c, 4].Value = pitem.nombre;
                workSheet.Cells[c, 4].Style.WrapText = true;
                if (pitem.UnidadId != 0)
                {
                    workSheet.Cells[c, 5].Value = this.nombrecatalogo(pitem.UnidadId);

                }

                if (pitem.item_padre == ".")
                {
                    int padre = workSheet.Dimension.End.Column;
                    while (padre > 0)
                    {
                        workSheet.Cells[c, padre].Style.Font.Bold = true;
                        workSheet.Cells[c, padre].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        workSheet.Cells[c, padre].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                        workSheet.Cells[c, padre].Style.Border.Top.Style = workSheet.Cells[c, padre].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
                        padre--;

                    }

                }
                if (pitem.item_padre != "." && pitem.para_oferta == false)
                {
                    int padre = workSheet.Dimension.End.Column;
                    while (padre > 0)
                    {
                        workSheet.Cells[c, padre].Style.Font.Bold = true;
                        workSheet.Cells[c, padre].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        workSheet.Cells[c, padre].Style.Fill.BackgroundColor.SetColor(Color.WhiteSmoke);
                        workSheet.Cells[c, padre].Style.Border.Top.Style = workSheet.Cells[c, padre].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        padre--;

                    }

                }

                workSheet.Cells[c, 5].Style.Border.Right.Style = ExcelBorderStyle.Medium;
                workSheet.Cells[c, 2].Style.Border.Left.Style = ExcelBorderStyle.Medium;
                workSheet.Cells[c, workSheet.Dimension.End.Column].Style.Border.Right.Style = ExcelBorderStyle.Medium;




                workSheet.Row(c).Style.Locked = false;
                c = c + 1;
            }



            //EMPIEZA PARTE DE LA DERECHA CALCULOS Y TODO

            var noOfCol = workSheet.Dimension.End.Column;
            var noOfRow = workSheet.Dimension.End.Row;

            workSheet.Cells[nivel_maximo, noOfCol + 1].Value = "CANTIDAD ESTIMADA";
            workSheet.Cells[nivel_maximo, noOfCol + 2].Value = "PRECIO UNITARIO";
            workSheet.Cells[nivel_maximo, noOfCol + 3].Value = "COSTO TOTAL ESTIMADO";




            // Estilos a la Parte Derecha de Calculos
            for (int a = noOfCol; a <= noOfCol + 3; a++)
            {
                workSheet.Column(a).Width = 25;
                workSheet.Cells[nivel_maximo, a].Style.WrapText = true;
                workSheet.Cells[nivel_maximo, a].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Cells[nivel_maximo, a].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                workSheet.Cells[nivel_maximo, a].Style.Fill.PatternType = ExcelFillStyle.Solid;
                workSheet.Cells[nivel_maximo, a].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                workSheet.Cells[nivel_maximo, a].Style.Border.Top.Style =
                workSheet.Cells[nivel_maximo, a].Style.Border.Left.Style =
                workSheet.Cells[nivel_maximo, a].Style.Border.Right.Style =
                workSheet.Cells[nivel_maximo, a].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
            }
            for (int j = 6; j <= noOfCol; j++)
            {

                //Valores Parte Derecha





                var wbsid = (workSheet.Cells[nivel_maximo + 1, j].Value ?? "").ToString();
                for (int i = nivel_maximo + 3; i <= noOfRow; i++)
                {
                    var itemid = (workSheet.Cells[i, 2].Value ?? "").ToString();

                    foreach (var itemss in computos)
                    {

                        if (Convert.ToString(itemss.WbsId) == wbsid && Convert.ToString(itemss.ItemId) == itemid)
                        {

                            //


                            workSheet.Cells[i, j].Value = itemss.cantidad;
                            workSheet.Cells[i, noOfCol + 2].Value = itemss.precio_unitario;
                            workSheet.Cells[i, j].Style.Numberformat.Format = "#,##0.00";
                            workSheet.Cells[i, noOfCol + 1].Style.Numberformat.Format = "#,##0.00";
                            workSheet.Cells[i, noOfCol + 2].Style.Numberformat.Format = "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";
                            workSheet.Cells[i, noOfCol + 3].Style.Numberformat.Format = "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";

                            workSheet.Cells[i, noOfCol + 4].Value = itemss.Item.GrupoId;
                            workSheet.Cells[i, noOfCol + 4].Style.Font.Color.SetColor(Color.White);
                        }

                    }


                }




            }

            int ncol = workSheet.Dimension.End.Column - 1;

            for (int i = nivel_maximo + 3; i <= noOfRow; i++)
            {
                var rango_incio = workSheet.Cells[i, 6].Address;
                var rango_final = workSheet.Cells[i, ncol - 3].Address;
                var rangosumar = rango_incio + ":" + rango_final;
                workSheet.Cells[i, ncol - 2].Formula = "=SUM(" + rangosumar + ")";

                var cantidad = workSheet.Cells[i, ncol - 2].Value;
                var precio = workSheet.Cells[i, ncol - 2].Value;

                var dcantidad = workSheet.Cells[i, ncol - 2].Address;
                var dprecio = workSheet.Cells[i, ncol - 1].Address;
                workSheet.Cells[i, ncol].FormulaR1C1 = "=ROUND(" + dcantidad + "*" + dprecio + ", 2)";

            }

            int coln = workSheet.Dimension.End.Column;
            while (coln > 0)
            {
                workSheet.Cells[workSheet.Dimension.End.Row, coln].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
                coln--;

            }

            // rango total
            string costoi = workSheet.Cells[nivel_maximo + 3, ncol].Address;
            string costoif = workSheet.Cells[noOfRow, ncol].Address;
            string rangovalortotal = "$" + costoi + ":$" + costoif;


            int bfila = noOfRow;
            bfila = bfila + 2;
            //CALCULOS BASE
            //Ingenieria
            workSheet.Cells[bfila, 4].Value = "Sub - total Ingeniería";
            workSheet.Cells[bfila, 4].Style.WrapText = true;
            workSheet.Cells[bfila, 4].Style.Border.Top.Style =
            workSheet.Cells[bfila, 4].Style.Border.Left.Style =
            workSheet.Cells[bfila, 4].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

            workSheet.Cells[bfila, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            workSheet.Cells[bfila, 4].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            workSheet.Cells[bfila, 4].Style.Font.Bold = true;


            workSheet.Cells[bfila, 5].Style.Border.Top.Style =
            workSheet.Cells[bfila, 5].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[bfila, 5].Style.Border.Right.Style = ExcelBorderStyle.Medium;


            //suma ingenieria

            string dinicio = workSheet.Cells[nivel_maximo + 3, noOfCol + 4].Address;
            string dfinal = workSheet.Cells[noOfRow, noOfCol + 4].Address;
            string rangogrupo = "$" + dinicio + ":" + "$" + dfinal;
            string grupo = "" + 1 + "";

            var formula = "=SUMIF(" + rangogrupo + ", " + grupo + ", " + rangovalortotal + ")";
            workSheet.Cells[bfila, noOfCol + 3].FormulaR1C1 = formula;
            workSheet.Cells[bfila, noOfCol + 3].Style.Numberformat.Format =
                "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";
            workSheet.Cells[bfila, noOfCol + 3].Style.WrapText = true;

            string sumaingenieria = "$" + workSheet.Cells[bfila, noOfCol + 3].Address;


            workSheet.Cells[bfila, noOfCol + 3].Style.Border.Top.Style =
            workSheet.Cells[bfila, noOfCol + 3].Style.Border.Left.Style =
            workSheet.Cells[bfila, noOfCol + 3].Style.Border.Right.Style =
            workSheet.Cells[bfila, noOfCol + 3].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[bfila, noOfCol + 3].Style.Font.Bold = true;

            bfila++;
            //Procura
            workSheet.Cells[bfila, 4].Value = "Sub-total Procura";
            workSheet.Cells[bfila, 4].Style.WrapText = true;
            workSheet.Cells[bfila, 4].Style.Border.Top.Style =
            workSheet.Cells[bfila, 4].Style.Border.Left.Style =

            workSheet.Cells[bfila, 4].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[bfila, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            workSheet.Cells[bfila, 4].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            workSheet.Cells[bfila, 4].Style.Font.Bold = true;

            workSheet.Cells[bfila, 5].Style.Border.Top.Style =
               workSheet.Cells[bfila, 5].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[bfila, 5].Style.Border.Right.Style = ExcelBorderStyle.Medium;


            //suma procura

            dinicio = workSheet.Cells[nivel_maximo + 3, noOfCol + 4].Address;
            dfinal = workSheet.Cells[noOfRow, noOfCol + 4].Address;
            rangogrupo = "$" + dinicio + ":" + "$" + dfinal;
            grupo = "" + 3 + "";

            formula = "=SUMIF(" + rangogrupo + ", " + grupo + ", " + rangovalortotal + ")";
            workSheet.Cells[bfila, noOfCol + 3].FormulaR1C1 = formula;
            workSheet.Cells[bfila, noOfCol + 3].Style.Numberformat.Format =
                "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";
            workSheet.Cells[bfila, noOfCol + 3].Style.WrapText = true;


            string suma_procura = "$" + workSheet.Cells[bfila, noOfCol + 3].Address;
            workSheet.Cells[bfila, noOfCol + 3].Style.Border.Top.Style =
         workSheet.Cells[bfila, noOfCol + 3].Style.Border.Left.Style =
         workSheet.Cells[bfila, noOfCol + 3].Style.Border.Right.Style =
         workSheet.Cells[bfila, noOfCol + 3].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[bfila, noOfCol + 3].Style.Font.Bold = true;

            //CALCULOS BASE
            //Reembolsables
            bfila++;
            workSheet.Cells[bfila, 4].Value = "Sub-total Reembolsables";
            workSheet.Cells[bfila, 4].Style.WrapText = true;
            workSheet.Cells[bfila, 4].Style.Border.Top.Style =
            workSheet.Cells[bfila, 4].Style.Border.Left.Style =

            workSheet.Cells[bfila, 4].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[bfila, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            workSheet.Cells[bfila, 4].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            workSheet.Cells[bfila, 4].Style.Font.Bold = true;

            workSheet.Cells[bfila, 5].Style.Border.Top.Style =
            workSheet.Cells[bfila, 5].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[bfila, 5].Style.Border.Right.Style = ExcelBorderStyle.Medium;

            workSheet.Cells[bfila, noOfCol + 3].Value = 0;
            workSheet.Cells[bfila, noOfCol + 3].Style.Numberformat.Format =
                "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";
            workSheet.Cells[bfila, noOfCol + 3].Style.WrapText = true;

            string sumareembolsables = "$" + workSheet.Cells[bfila, noOfCol + 3].Address;

            workSheet.Cells[bfila, noOfCol + 3].Style.Border.Top.Style =
         workSheet.Cells[bfila, noOfCol + 3].Style.Border.Left.Style =
         workSheet.Cells[bfila, noOfCol + 3].Style.Border.Right.Style =
         workSheet.Cells[bfila, noOfCol + 3].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[bfila, noOfCol + 3].Style.Font.Bold = true;

            //Consturccion
            bfila++;
            workSheet.Cells[bfila, 4].Value = "Sub-total Construcción";
            workSheet.Cells[bfila, 4].Style.WrapText = true;
            workSheet.Cells[bfila, 4].Style.Border.Top.Style =
            workSheet.Cells[bfila, 4].Style.Border.Left.Style =

            workSheet.Cells[bfila, 4].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[bfila, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            workSheet.Cells[bfila, 4].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            workSheet.Cells[bfila, 4].Style.Font.Bold = true;

            workSheet.Cells[bfila, 5].Style.Border.Top.Style =
             workSheet.Cells[bfila, 5].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[bfila, 5].Style.Border.Right.Style = ExcelBorderStyle.Medium;

            dinicio = workSheet.Cells[nivel_maximo + 3, noOfCol + 4].Address;
            dfinal = workSheet.Cells[noOfRow, noOfCol + 4].Address;
            rangogrupo = "$" + dinicio + ":" + "$" + dfinal;
            grupo = "" + 2 + "";

            formula = "=SUMIF(" + rangogrupo + ", " + grupo + ", " + rangovalortotal + ")";
            workSheet.Cells[bfila, noOfCol + 3].FormulaR1C1 = formula;
            workSheet.Cells[bfila, noOfCol + 3].Style.Numberformat.Format =
                "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";
            workSheet.Cells[bfila, noOfCol + 3].Style.WrapText = true;

            string suma_contruccion = "$" + workSheet.Cells[bfila, noOfCol + 3].Address;
            workSheet.Cells[bfila, noOfCol + 3].Style.Border.Top.Style =
         workSheet.Cells[bfila, noOfCol + 3].Style.Border.Left.Style =
         workSheet.Cells[bfila, noOfCol + 3].Style.Border.Right.Style =
         workSheet.Cells[bfila, noOfCol + 3].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[bfila, noOfCol + 3].Style.Font.Bold = true;

            //Descuento
            bfila++;
            workSheet.Cells[bfila, 4].Value = "Descuento por <Descripción del concepto>";
            workSheet.Cells[bfila, 4].Style.WrapText = true;
            workSheet.Cells[bfila, 4].Style.Border.Top.Style =
            workSheet.Cells[bfila, 4].Style.Border.Left.Style =

            workSheet.Cells[bfila, 4].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[bfila, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            workSheet.Cells[bfila, 4].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            workSheet.Cells[bfila, 4].Style.Font.Bold = true;

            workSheet.Cells[bfila, 5].Style.Border.Top.Style =
             workSheet.Cells[bfila, 5].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[bfila, 5].Style.Border.Right.Style = ExcelBorderStyle.Medium;


            workSheet.Cells[bfila, noOfCol + 3].Value = 0;
            workSheet.Cells[bfila, noOfCol + 3].Style.Numberformat.Format =
                "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";
            workSheet.Cells[bfila, noOfCol + 3].Style.WrapText = true;

            workSheet.Cells[bfila, noOfCol + 3].Style.Border.Top.Style =
         workSheet.Cells[bfila, noOfCol + 3].Style.Border.Left.Style =
         workSheet.Cells[bfila, noOfCol + 3].Style.Border.Right.Style =
         workSheet.Cells[bfila, noOfCol + 3].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[bfila, noOfCol + 3].Style.Font.Bold = true;


            //Descuento
            bfila++;
            workSheet.Cells[bfila, 4].Value = "Administración";
            workSheet.Cells[bfila, 4].Style.WrapText = true;
            workSheet.Cells[bfila, 4].Style.Border.Top.Style =
            workSheet.Cells[bfila, 4].Style.Border.Left.Style =

            workSheet.Cells[bfila, 4].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[bfila, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            workSheet.Cells[bfila, 4].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            workSheet.Cells[bfila, 4].Style.Font.Bold = true;

            workSheet.Cells[bfila, 5].Style.Border.Top.Style =
                        workSheet.Cells[bfila, 5].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[bfila, 5].Style.Border.Right.Style = ExcelBorderStyle.Medium;

            string administracion = "$" + workSheet.Cells[bfila, noOfCol + 3].Address;
            workSheet.Cells[bfila, noOfCol + 3].Style.Border.Top.Style =
         workSheet.Cells[bfila, noOfCol + 3].Style.Border.Left.Style =
         workSheet.Cells[bfila, noOfCol + 3].Style.Border.Right.Style =
         workSheet.Cells[bfila, noOfCol + 3].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[bfila, noOfCol + 3].Style.Font.Bold = true;



            workSheet.Cells[bfila, noOfCol + 3].Style.Border.Top.Style =
         workSheet.Cells[bfila, noOfCol + 3].Style.Border.Left.Style =
         workSheet.Cells[bfila, noOfCol + 3].Style.Border.Right.Style =
         workSheet.Cells[bfila, noOfCol + 3].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[bfila, noOfCol + 3].Style.Font.Bold = true;

            //
            bfila++;
            workSheet.Cells[bfila, 4].Value = "Administracion sobre Obra (%)";
            workSheet.Cells[bfila, 4].Style.WrapText = true;
            workSheet.Cells[bfila, 4].Style.Border.Top.Style =
            workSheet.Cells[bfila, 4].Style.Border.Left.Style =

            workSheet.Cells[bfila, 4].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[bfila, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            workSheet.Cells[bfila, 4].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            workSheet.Cells[bfila, 4].Style.Font.Bold = true;

            workSheet.Cells[bfila, 5].Value = 0.4119;
            workSheet.Cells[bfila, 5].Style.WrapText = true;

            workSheet.Cells[bfila, 5].Style.Border.Top.Style =
            workSheet.Cells[bfila, 5].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[bfila, 5].Style.Border.Right.Style = ExcelBorderStyle.Medium;

            workSheet.Cells[bfila, 5].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            workSheet.Cells[bfila, 5].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            workSheet.Cells[bfila, 5].Style.Font.Bold = true;
            workSheet.Cells[bfila, 5].Style.Numberformat.Format = "0.0%";

            //contruccion

            string formula_calculo = "=" + workSheet.Cells[bfila, 5].Address + "*" + suma_contruccion;
            workSheet.Cells[bfila, noOfCol + 3].FormulaR1C1 = formula_calculo;
            workSheet.Cells[bfila, noOfCol + 3].Style.Numberformat.Format =
                "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";
            workSheet.Cells[bfila, noOfCol + 3].Style.WrapText = true;

            string valoradministracion_obra = workSheet.Cells[bfila, noOfCol + 3].Address;

            workSheet.Cells[bfila, noOfCol + 3].Style.Border.Top.Style =
         workSheet.Cells[bfila, noOfCol + 3].Style.Border.Left.Style =
         workSheet.Cells[bfila, noOfCol + 3].Style.Border.Right.Style =
         workSheet.Cells[bfila, noOfCol + 3].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[bfila, noOfCol + 3].Style.Font.Bold = true;


            //
            bfila++;
            workSheet.Cells[bfila, 4].Value = "Imprevistos sobre Obra (%)";
            workSheet.Cells[bfila, 4].Style.WrapText = true;
            workSheet.Cells[bfila, 4].Style.Border.Top.Style =
            workSheet.Cells[bfila, 4].Style.Border.Left.Style =

            workSheet.Cells[bfila, 4].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[bfila, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            workSheet.Cells[bfila, 4].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            workSheet.Cells[bfila, 4].Style.Font.Bold = true;

            workSheet.Cells[bfila, 5].Value = 0.03;
            workSheet.Cells[bfila, 5].Style.WrapText = true;

            workSheet.Cells[bfila, 5].Style.Border.Top.Style =
            workSheet.Cells[bfila, 5].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[bfila, 5].Style.Border.Right.Style = ExcelBorderStyle.Medium;

            workSheet.Cells[bfila, 5].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            workSheet.Cells[bfila, 5].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            workSheet.Cells[bfila, 5].Style.Font.Bold = true;
            workSheet.Cells[bfila, 5].Style.Numberformat.Format = "0.0%";

            //caculo
            formula_calculo = "=" + workSheet.Cells[bfila, 5].Address + "*" + suma_contruccion;
            workSheet.Cells[bfila, noOfCol + 3].FormulaR1C1 = formula_calculo;
            workSheet.Cells[bfila, noOfCol + 3].Style.Numberformat.Format =
                "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";
            workSheet.Cells[bfila, noOfCol + 3].Style.WrapText = true;


            string valorimprevistos_obra = workSheet.Cells[bfila, noOfCol + 3].Address;
            workSheet.Cells[bfila, noOfCol + 3].Style.Border.Top.Style =
         workSheet.Cells[bfila, noOfCol + 3].Style.Border.Left.Style =
         workSheet.Cells[bfila, noOfCol + 3].Style.Border.Right.Style =
         workSheet.Cells[bfila, noOfCol + 3].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[bfila, noOfCol + 3].Style.Font.Bold = true;
            //
            bfila++;
            workSheet.Cells[bfila, 4].Value = "Utilidad sobre Obra (%)";
            workSheet.Cells[bfila, 4].Style.WrapText = true;
            workSheet.Cells[bfila, 4].Style.Border.Top.Style =
            workSheet.Cells[bfila, 4].Style.Border.Left.Style =

            workSheet.Cells[bfila, 4].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[bfila, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            workSheet.Cells[bfila, 4].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            workSheet.Cells[bfila, 4].Style.Font.Bold = true;

            workSheet.Cells[bfila, 5].Value = 0.12;
            workSheet.Cells[bfila, 5].Style.WrapText = true;

            workSheet.Cells[bfila, 5].Style.Border.Top.Style =
            workSheet.Cells[bfila, 5].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[bfila, 5].Style.Border.Right.Style = ExcelBorderStyle.Medium;

            workSheet.Cells[bfila, 5].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            workSheet.Cells[bfila, 5].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            workSheet.Cells[bfila, 5].Style.Font.Bold = true;
            workSheet.Cells[bfila, 5].Style.Numberformat.Format = "0.0%";


            //caculo
            formula_calculo = "=" + workSheet.Cells[bfila, 5].Address + "*" + suma_contruccion;
            workSheet.Cells[bfila, noOfCol + 3].FormulaR1C1 = formula_calculo;
            workSheet.Cells[bfila, noOfCol + 3].Style.Numberformat.Format =
                "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";
            workSheet.Cells[bfila, noOfCol + 3].Style.WrapText = true;



            string valor_utilidadObra = workSheet.Cells[bfila, noOfCol + 3].Address;
            workSheet.Cells[bfila, noOfCol + 3].Style.Border.Top.Style =
         workSheet.Cells[bfila, noOfCol + 3].Style.Border.Left.Style =
         workSheet.Cells[bfila, noOfCol + 3].Style.Border.Right.Style =
         workSheet.Cells[bfila, noOfCol + 3].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[bfila, noOfCol + 3].Style.Font.Bold = true;
            //
            bfila++;
            workSheet.Cells[bfila, 4].Value = "Administracion sobre Procura Contratista (%)";
            workSheet.Cells[bfila, 4].Style.WrapText = true;
            workSheet.Cells[bfila, 4].Style.Border.Top.Style =
            workSheet.Cells[bfila, 4].Style.Border.Left.Style =

            workSheet.Cells[bfila, 4].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[bfila, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            workSheet.Cells[bfila, 4].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            workSheet.Cells[bfila, 4].Style.Font.Bold = true;

            workSheet.Cells[bfila, 5].Value = 0.1;
            workSheet.Cells[bfila, 5].Style.WrapText = true;

            workSheet.Cells[bfila, 5].Style.Border.Top.Style =
            workSheet.Cells[bfila, 5].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[bfila, 5].Style.Border.Right.Style = ExcelBorderStyle.Medium;

            workSheet.Cells[bfila, 5].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            workSheet.Cells[bfila, 5].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            workSheet.Cells[bfila, 5].Style.Font.Bold = true;
            workSheet.Cells[bfila, 5].Style.Numberformat.Format = "0.0%";




            //caculo
            formula_calculo = "=" + workSheet.Cells[bfila, 5].Address + "*" + suma_procura;
            workSheet.Cells[bfila, noOfCol + 3].FormulaR1C1 = formula_calculo;
            workSheet.Cells[bfila, noOfCol + 3].Style.Numberformat.Format =
                "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";
            workSheet.Cells[bfila, noOfCol + 3].Style.WrapText = true;


            string valor_procura = workSheet.Cells[bfila, noOfCol + 3].Address;
            workSheet.Cells[bfila, noOfCol + 3].Style.Border.Top.Style =
         workSheet.Cells[bfila, noOfCol + 3].Style.Border.Left.Style =
         workSheet.Cells[bfila, noOfCol + 3].Style.Border.Right.Style =
         workSheet.Cells[bfila, noOfCol + 3].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[bfila, noOfCol + 3].Style.Font.Bold = true;


            bfila++;
            workSheet.Cells[bfila, 4].Value = "Administracion sobre Reembolsables (%)";
            workSheet.Cells[bfila, 4].Style.WrapText = true;
            workSheet.Cells[bfila, 4].Style.Border.Top.Style =
            workSheet.Cells[bfila, 4].Style.Border.Left.Style =

            workSheet.Cells[bfila, 4].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[bfila, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            workSheet.Cells[bfila, 4].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            workSheet.Cells[bfila, 4].Style.Font.Bold = true;

            workSheet.Cells[bfila, 5].Value = 0.1;
            workSheet.Cells[bfila, 5].Style.WrapText = true;

            workSheet.Cells[bfila, 5].Style.Border.Top.Style =
            workSheet.Cells[bfila, 5].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[bfila, 5].Style.Border.Right.Style = ExcelBorderStyle.Medium;

            workSheet.Cells[bfila, 5].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            workSheet.Cells[bfila, 5].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            workSheet.Cells[bfila, 5].Style.Font.Bold = true;
            workSheet.Cells[bfila, 5].Style.Numberformat.Format = "0.0%";


            //caculo
            formula_calculo = "=" + workSheet.Cells[bfila, 5].Address + "*" + sumareembolsables;
            workSheet.Cells[bfila, noOfCol + 3].FormulaR1C1 = formula_calculo;
            workSheet.Cells[bfila, noOfCol + 3].Style.Numberformat.Format =
                "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";
            workSheet.Cells[bfila, noOfCol + 3].Style.WrapText = true;


            string valor_reembolsables = workSheet.Cells[bfila, noOfCol + 3].Address;
            workSheet.Cells[bfila, noOfCol + 3].Style.Border.Top.Style =
         workSheet.Cells[bfila, noOfCol + 3].Style.Border.Left.Style =
         workSheet.Cells[bfila, noOfCol + 3].Style.Border.Right.Style =
         workSheet.Cells[bfila, noOfCol + 3].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[bfila, noOfCol + 3].Style.Font.Bold = true;
            //envio de datos a administracion

            formula_calculo = "=SUM(" + valoradministracion_obra + ":" + valor_reembolsables + ")";
            workSheet.Cells[administracion].Formula = formula_calculo;
            workSheet.Cells[administracion].Style.Numberformat.Format =
                "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";
            workSheet.Cells[administracion].Style.WrapText = true;


            bfila++;
            workSheet.Cells[bfila, 4].Value = "TOTAL";
            workSheet.Cells[bfila, 4].Style.WrapText = true;
            workSheet.Cells[bfila, 4].Style.Border.Top.Style =
            workSheet.Cells[bfila, 4].Style.Border.Left.Style =

            workSheet.Cells[bfila, 4].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[bfila, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            workSheet.Cells[bfila, 4].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            workSheet.Cells[bfila, 4].Style.Font.Bold = true;
            workSheet.Cells[bfila, 5].Style.Border.Top.Style =
            workSheet.Cells[bfila, 5].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[bfila, 5].Style.Border.Right.Style = ExcelBorderStyle.Medium;

            //TOTAL

            formula_calculo = "=SUM(" + sumaingenieria + "+" + suma_procura + "+" + suma_contruccion + "+" + sumareembolsables + "+" + administracion + ")";
            workSheet.Cells[bfila, noOfCol + 3].Formula = formula_calculo;
            workSheet.Cells[bfila, noOfCol + 3].Style.Numberformat.Format =
                "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";
            workSheet.Cells[bfila, noOfCol + 3].Style.WrapText = true;
            string valortotal = workSheet.Cells[bfila, noOfCol + 3].Address;
            workSheet.Cells[bfila, noOfCol + 3].Style.Border.Top.Style =
         workSheet.Cells[bfila, noOfCol + 3].Style.Border.Left.Style =
         workSheet.Cells[bfila, noOfCol + 3].Style.Border.Right.Style =
         workSheet.Cells[bfila, noOfCol + 3].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[bfila, noOfCol + 3].Style.Font.Bold = true;



            return excel;
        }

        public int GetComputosporOfertaProcura(int OfertaId)
        {
            var computosQuery = Repository.GetAllIncluding(o => o.Wbs, o => o.Item).Where(o => o.Wbs.OfertaId == OfertaId).
                Where(o => o.vigente == true).Where(o => o.Item.GrupoId == 3).ToList();
            return computosQuery.Count;
        }

        public Computo editarcomputoexiste(int WbsOfertaId, int ItemId)
        {

            var computo = Repository.GetAll().Where(c => c.WbsId == WbsOfertaId).Where(c => c.ItemId == ItemId).Where(
                c => c.vigente == true).FirstOrDefault();
            if (computo != null && computo.Id > 0)
            {
                return computo;
            }

            return new Computo();
        }

        public ExcelPackage GenerarExcelCargaEAC(OfertaDto oferta, int nivel_maximo)
        {
            {
                //Lista Wbs
                var query = _wbsrepository.GetAllIncluding(x => x.Catalogo)
                    .Where(o => o.vigente == true)
                    .Where(o => o.OfertaId == oferta.Id)
                    .Where(o => o.es_actividad == true).ToList();

                var wbs = (from w in query
                           select new WbsDto()
                           {
                               Id = w.Id,
                               OfertaId = w.OfertaId,
                               fecha_inicial = w.fecha_inicial,
                               fecha_final = w.fecha_final,
                               id_nivel_padre_codigo = w.id_nivel_padre_codigo,
                               id_nivel_codigo = w.id_nivel_codigo,
                               nivel_nombre = w.nivel_nombre,
                               observaciones = w.observaciones,
                               DisciplinaId = w.DisciplinaId,
                               revision = w.revision,
                               Catalogo = w.Catalogo

                           }).ToList();


                foreach (var w in wbs)
                {
                    var name = _wbsrepository
                        .GetAll()
                        .Where(o => o.vigente == true)
                        .Where(o => o.OfertaId == w.OfertaId).SingleOrDefault(o => o.id_nivel_codigo == w.id_nivel_padre_codigo);
                    if (name != null)
                    {
                        w.nombre_padre = name.nivel_nombre;
                    }
                }




                var listacomputos = Repository.GetAllIncluding(x => x.Wbs.Oferta.Proyecto, x => x.Item).Where(x => x.vigente == true).ToList();
                var computos = (from z in listacomputos
                                where z.Wbs.OfertaId == oferta.Id && z.vigente == true
                                select new ComputoDto
                                {
                                    Id = z.Id,
                                    ItemId = z.ItemId,
                                    WbsId = z.WbsId,
                                    item_codigo = z.Item.codigo,
                                    item_nombre = z.Item.nombre,
                                    cantidad = z.cantidad,
                                    cantidad_eac = z.cantidad_eac,
                                    costo_total = z.costo_total,
                                    precio_unitario = z.precio_unitario,
                                    Item = z.Item,
                                    precio_incrementado = z.precio_incrementado,
                                    precio_ajustado = z.precio_ajustado,
                                    diferente = z.cantidad_eac != z.cantidad,
                                    total_pu = (z.cantidad * z.precio_unitario),
                                    total_pu_aui = (z.cantidad * z.precio_incrementado)

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
                // var items = _itemservice.GetItemsporContratoActivo(oferta.Proyecto.contratoId, oferta.fecha_oferta.Value);
                var items = _itemservice.ArbolWbsExcel(oferta.Proyecto.contratoId, oferta.fecha_oferta.GetValueOrDefault());
                ExcelPackage excel = new ExcelPackage();
                var workSheet = excel.Workbook.Worksheets.Add("EAC");

                workSheet.TabColor = System.Drawing.Color.Azure;
                /*workSheet.Protection.IsProtected = true;
                  workSheet.Protection.SetPassword("pmdis");
                  */
                workSheet.DefaultRowHeight = 12;

                //Header of table  
                //  

                workSheet.View.ZoomScale = 80;
                int row = nivel_maximo;
                for (int i = 1; i <= row; i++)
                {

                    workSheet.Row(i).Style.Font.Bold = true;
                }

                int columna = 6;

                foreach (var itemswbs in wbs.OrderBy(l => l.id_nivel_codigo))
                {


                    int fila = nivel_maximo + 4;
                    List<Wbs> Jerarquia = new List<Wbs>();

                    Wbs item = _wbsrepository.Get(itemswbs.Id);
                    Jerarquia.Add(item);
                    while (item.id_nivel_padre_codigo != ".")
                    {
                        //
                        item = _wbsrepository.GetAll()
                            .Where(X => X.id_nivel_codigo == item.id_nivel_padre_codigo)
                            .Where(C => C.vigente)
                             .Where(C => C.OfertaId == oferta.Id)
                            .FirstOrDefault();

                        Jerarquia.Add(item);
                    }
                    int a = Jerarquia.Count();
                    int rowtyle = nivel_maximo - 1;
                    foreach (var wbsj in Jerarquia)
                    {
                        if (rowtyle > 0)
                        {
                            workSheet.Cells[rowtyle, columna].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            workSheet.Cells[rowtyle, columna].Style.Fill.BackgroundColor.SetColor(Color.LightSteelBlue);

                            workSheet.Cells[rowtyle, columna].Style.Border.Left.Style =
                            workSheet.Cells[rowtyle, columna].Style.Border.Right.Style = ExcelBorderStyle.Medium;
                            rowtyle--;
                        }
                        if (a > 0)
                        {
                            if (wbsj.es_actividad)
                            {
                                workSheet.Cells[fila - 4, columna].Value = wbsj.nivel_nombre;
                                workSheet.Cells[fila - 4, columna].Value = wbsj.nivel_nombre;
                                workSheet.Column(columna).Width = 25;
                                workSheet.Cells[fila - 4, columna].Style.WrapText = true;
                                workSheet.Cells[fila - 4, columna].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                workSheet.Cells[fila - 4, columna].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                workSheet.Cells[fila - 4, columna].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                workSheet.Cells[fila - 4, columna].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                                workSheet.Cells[fila - 4, columna].Style.Border.Top.Style = workSheet.Cells[fila - 4, columna].Style.Border.Left.Style = workSheet.Cells[fila - 4, columna].Style.Border.Right.Style = workSheet.Cells[fila - 4, columna].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;

                                workSheet.Cells[fila - 2, columna].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                workSheet.Cells[fila - 2, columna].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                            }
                            else
                            {
                                workSheet.Cells[a, columna].Value = wbsj.nivel_nombre;


                                workSheet.Cells[a, columna].Style.WrapText = true;
                                workSheet.Cells[a, columna].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                workSheet.Cells[a, columna].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                workSheet.Cells[a, columna].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                workSheet.Cells[a, columna].Style.Fill.BackgroundColor.SetColor(Color.LightSteelBlue);
                                workSheet.Cells[a, columna].Style.Border.Left.Style =
                                workSheet.Cells[a, columna].Style.Border.Right.Style = ExcelBorderStyle.Medium;
                                /* 
                                 var valornuevo = workSheet.Cells[a, columna].Text;
                                 var valorantiguo = workSheet.Cells[a, (columna-1)].Text;
                                 if (valornuevo == valorantiguo) {
                                     string rango1 = workSheet.Cells[a, columna - 1].Address;
                                     string rango2= workSheet.Cells[a, columna].Address;
                                     string range = rango1 + ":" + rango2;

                                     if(!workSheet.Cells[a, (columna - 1)].Merge) {
                                     workSheet.Cells[range].Merge =true;
                                     workSheet.Cells[range].Value=valornuevo.ToString();

                                     workSheet.Cells[range].Style.Border.Top.Style = 
                                     workSheet.Cells[range].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
                                     }
                                 }
                                 */
                            }
                        }

                        a--;
                    }
                    workSheet.Row(fila - 3).Hidden = true;
                    workSheet.Cells[fila - 3, columna].Value = itemswbs.Id;
                    workSheet.Cells[fila - 3, columna].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    workSheet.Cells[fila - 3, columna].Style.Fill.BackgroundColor.SetColor(Color.LightSteelBlue);
                    workSheet.Cells[fila - 3, columna].Style.Border.Left.Style =
                           workSheet.Cells[fila - 3, columna].Style.Border.Right.Style = ExcelBorderStyle.Medium;


                    if (itemswbs.DisciplinaId >= 0)
                    {
                        workSheet.Cells[fila - 2, columna].Value = itemswbs.Catalogo.nombre;
                        workSheet.Cells[fila - 2, columna].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        workSheet.Cells[fila - 2, columna].Style.Fill.BackgroundColor.SetColor(Color.LightSteelBlue);
                        workSheet.Cells[fila - 2, columna].Style.Border.Left.Style =
                           workSheet.Cells[fila - 2, columna].Style.Border.Right.Style = ExcelBorderStyle.Medium;
                        workSheet.Cells[fila - 2, columna].Style.Border.Top.Style = workSheet.Cells[fila - 2, columna].Style.Border.Left.Style = workSheet.Cells[fila - 2, columna].Style.Border.Right.Style = workSheet.Cells[fila - 2, columna].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
                    }
                    else
                    {
                        workSheet.Row(fila - 2).Hidden = true;
                    }

                    columna = columna + 1;
                }
                workSheet.Cells[nivel_maximo, 1].Value = "Para Oferta";
                workSheet.Cells[nivel_maximo, 2].Value = "Id";
                workSheet.Cells[nivel_maximo, 3].Value = "ITEM";
                workSheet.Cells[nivel_maximo, 4].Value = "DESCRIPCIÓN";
                workSheet.Cells[nivel_maximo, 5].Value = "UNIDAD";


                string rango = workSheet.Cells[nivel_maximo, 2].Address + ":" + workSheet.Cells[nivel_maximo, 5].Address;
                workSheet.Cells[rango].AutoFilter = true;
                for (int i = 2; i <= 5; i++)
                {
                    workSheet.Cells[nivel_maximo, i].Style.WrapText = true;
                    workSheet.Cells[nivel_maximo, i].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    workSheet.Cells[nivel_maximo, i].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                    workSheet.Cells[nivel_maximo + 2, i].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    workSheet.Cells[nivel_maximo + 2, i].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                    workSheet.Cells[nivel_maximo, i].Style.Border.Top.Style = workSheet.Cells[nivel_maximo, i].Style.Border.Left.Style = workSheet.Cells[nivel_maximo, i].Style.Border.Right.Style = workSheet.Cells[nivel_maximo, i].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;

                    workSheet.Cells[nivel_maximo, i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    workSheet.Cells[nivel_maximo, i].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                }

                workSheet.Column(3).Width = 15;
                workSheet.Column(4).Width = 80;
                workSheet.Column(5).Width = 15;
                workSheet.Column(1).Style.Font.Bold = true;
                workSheet.Column(1).Hidden = true;
                workSheet.Column(2).Hidden = true;
                workSheet.Row(nivel_maximo).Height = 60;

                //int inicio de las filas
                //int c = 5;
                int c = nivel_maximo + 3;
                workSheet.View.FreezePanes(6, 1);
                workSheet.View.FreezePanes(c, 6);
                foreach (var pitem in items)
                {

                    workSheet.Cells[c, 1].Value = pitem.para_oferta;
                    workSheet.Cells[c, 2].Value = pitem.Id;
                    workSheet.Cells[c, 3].Value = pitem.codigo;
                    workSheet.Cells[c, 4].Value = pitem.nombre;
                    workSheet.Cells[c, 4].Style.WrapText = true;
                    if (pitem.UnidadId != 0)
                    {
                        workSheet.Cells[c, 5].Value = this.nombrecatalogo(pitem.UnidadId);

                    }

                    if (pitem.item_padre == ".")
                    {
                        int padre = workSheet.Dimension.End.Column;
                        while (padre > 0)
                        {
                            workSheet.Cells[c, padre].Style.Font.Bold = true;
                            workSheet.Cells[c, padre].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            workSheet.Cells[c, padre].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                            workSheet.Cells[c, padre].Style.Border.Top.Style = workSheet.Cells[c, padre].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
                            padre--;

                        }

                    }
                    if (pitem.item_padre != "." && pitem.para_oferta == false)
                    {
                        int padre = workSheet.Dimension.End.Column;
                        while (padre > 0)
                        {
                            workSheet.Cells[c, padre].Style.Font.Bold = true;
                            workSheet.Cells[c, padre].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            workSheet.Cells[c, padre].Style.Fill.BackgroundColor.SetColor(Color.WhiteSmoke);
                            workSheet.Cells[c, padre].Style.Border.Top.Style = workSheet.Cells[c, padre].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            padre--;

                        }

                    }

                    workSheet.Cells[c, 5].Style.Border.Right.Style = ExcelBorderStyle.Medium;
                    workSheet.Cells[c, 2].Style.Border.Left.Style = ExcelBorderStyle.Medium;
                    workSheet.Cells[c, workSheet.Dimension.End.Column].Style.Border.Right.Style = ExcelBorderStyle.Medium;




                    workSheet.Row(c).Style.Locked = false;
                    c = c + 1;
                }



                var noOfCol = workSheet.Dimension.End.Column;
                var noOfRow = workSheet.Dimension.End.Row;




                for (int j = 6; j <= noOfCol; j++)
                {

                    //Valores Parte Derecha





                    var wbsid = (workSheet.Cells[nivel_maximo + 1, j].Value ?? "").ToString();
                    for (int i = nivel_maximo + 3; i <= noOfRow; i++)
                    {
                        var itemid = (workSheet.Cells[i, 2].Value ?? "").ToString();

                        foreach (var itemss in computos)
                        {

                            if (Convert.ToString(itemss.WbsId) == wbsid && Convert.ToString(itemss.ItemId) == itemid)
                            {

                                //


                                workSheet.Cells[i, j].Value = itemss.cantidad_eac;


                                workSheet.Cells[i, j].Style.Numberformat.Format = "#,##0.00";


                            }

                        }


                    }




                }





                return excel;
            }
        }

        public String CargaMasivaEAC(HttpPostedFileBase UploadedFile, int OfertaId, int maximo_nivel_wbs)
        {

            if (UploadedFile != null)
            {

                // tdata.ExecuteCommand("truncate table OtherCompanyAssets");  
                if (UploadedFile.ContentType == "application/vnd.ms-excel" || UploadedFile.ContentType ==
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                {
                    string fileName = UploadedFile.FileName;
                    string fileContentType = UploadedFile.ContentType;
                    byte[] fileBytes = new byte[UploadedFile.ContentLength];
                    var data = UploadedFile.InputStream.Read(fileBytes, 0,
                        Convert.ToInt32(UploadedFile.ContentLength));


                    using (var package = new ExcelPackage(UploadedFile.InputStream))
                    {
                        var currentSheet = package.Workbook.Worksheets;
                        var workSheet = currentSheet.First();
                        var noOfCol = workSheet.Dimension.End.Column - 4;

                        var noOfRow = workSheet.Dimension.End.Row - 13;

                        int maximo_nivel = maximo_nivel_wbs;
                        for (int i = 6; i <= noOfCol; i++)
                        {
                            for (int rowIterator = maximo_nivel + 3; rowIterator <= noOfRow; rowIterator++)
                            {
                                var para_oferta = (workSheet.Cells[rowIterator, 1].Value ?? "").ToString();
                                var itemid = (workSheet.Cells[rowIterator, 2].Value ?? "").ToString();

                                var wbsid = (workSheet.Cells[maximo_nivel + 1, i].Value ?? "").ToString();

                                var cantidad = (workSheet.Cells[rowIterator, i].Value ?? "").ToString();
                                var valor = Decimal.Parse("0" + cantidad, NumberStyles.Float);
                                var valoritem = Int32.Parse(itemid);
                                var valorwbs = Int32.Parse(wbsid);

                                if (itemid.Length > 0 && valoritem > 0 && valorwbs > 0 && wbsid.Length > 0 && cantidad.Length > 0 && valor > 0 && para_oferta.Length > 0 && para_oferta.Equals("True"))
                                {


                                    int WBSID = Int32.Parse(wbsid);
                                    int ITEMID = Int32.Parse(itemid);

                                    Computo e = Repository.GetAll().Where(c => c.vigente)
                                                                   .Where(c => c.WbsId == WBSID)
                                                                   .Where(c => c.ItemId == ITEMID).FirstOrDefault();

                                    if (e != null && e.Id > 0)
                                    {
                                        e.cantidad_eac = valor;

                                        var r = Repository.Update(e);

                                    }


                                }

                            }
                        }
                        return "OK";

                    }

                }
                else
                {


                    return "NO_FORMATO";


                }
            }
            else
            {
                return "ARCHIVO_INCORRECTO";
            }




        }

        public bool ActiveEsTemporal(int Id, bool data)
        {
            var computo = Repository.Get(Id);
            computo.es_temporal = data;
            Repository.Update(computo);
            return computo.Id > 0 ? true : false;

        }

        public Computo GetInfo(int Id)
        {
            var computo = Repository.Get(Id);
            return computo;
        }

        public bool ChangeCantidadAjustada(int ComputoId,bool cantidadAjustada, string tipo)
        {
            var c = Repository.Get(ComputoId);
            c.cantidadAjustada = cantidadAjustada;
            c.tipo = cantidadAjustada ? tipo : "";
            Repository.Update(c);
            return true;
        }


    }
}
