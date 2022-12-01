using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.aplicacion.Interfaces;
using com.cpp.calypso.proyecto.dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Extensions;
using System.Linq.Dynamic;
using com.cpp.calypso.proyecto.dominio.Models;
using com.cpp.calypso.proyecto.dominio.Constantes;

namespace com.cpp.calypso.proyecto.aplicacion
{
    public class ItemServiceAsyncBaseCrudAppService :
        AsyncBaseCrudAppService<Item, ItemDto, PagedAndFilteredResultRequestDto>, IItemAsyncBaseCrudAppService
    {
        private readonly IBaseRepository<DetallePreciario> repositorydetallepreciario;
        private readonly IBaseRepository<Preciario> _repositorypreciario;
        private readonly IBaseRepository<Catalogo> _repositorycatalogo;
        private readonly IBaseRepository<DetalleAvanceObra> _detalleavancebrarepository;
        private readonly IBaseRepository<ComputoComercial> _computocomercialrepository;
        private readonly IBaseRepository<ComputoPresupuesto> _computopresupuestorepository;

        public ItemServiceAsyncBaseCrudAppService(IBaseRepository<Item> repository,
            IBaseRepository<DetallePreciario> repositorydetallepreciario,
            IBaseRepository<Preciario> repositorypreciario,
            IBaseRepository<Catalogo> repositorycatalogo,
            IBaseRepository<DetalleAvanceObra> detalleavancebrarepository,
            IBaseRepository<ComputoComercial> computocomercialrepository,
            IBaseRepository<ComputoPresupuesto> computopresupuestorepository
            ) : base(repository)
        {
            this.repositorydetallepreciario = repositorydetallepreciario;
            _repositorypreciario = repositorypreciario;
            _repositorycatalogo = repositorycatalogo;
            _detalleavancebrarepository = detalleavancebrarepository;
            _computocomercialrepository = computocomercialrepository;
            _computopresupuestorepository = computopresupuestorepository;
        }

        public int buscaridentificadorpadre(string item_padre)
        {
            var itemQuery = Repository.GetAll().Where(e => e.vigente == true).Where(c => c.codigo == item_padre)
                .FirstOrDefault();


            if (itemQuery != null && itemQuery.Id > 0)
            {

                return itemQuery.Id;

            }

            return 0;
        }

        public bool comprobaritempadre(string Codigo)
        {
            String padre = "";
            var listaitems = GetItems();

            foreach (var item in listaitems)
            {

            }

            return true;
        }

        public async Task<ItemDto> GetDetalle(int ItemId)
        {
            var itemQuery = Repository.GetAll().Where(c => c.vigente == true);
            ItemDto item = (from c in itemQuery

                            where c.Id == ItemId
                            select new ItemDto
                            {
                                Id = c.Id,
                                codigo = c.codigo,
                                descripcion = c.descripcion,
                                item_padre = c.item_padre,
                                nombre = c.nombre,
                                para_oferta = c.para_oferta,
                                UnidadId = c.UnidadId,
                                vigente = c.vigente,
                                GrupoId = c.GrupoId,
                                EspecialidadId = c.EspecialidadId,
                                PendienteAprobacion = c.PendienteAprobacion
                            }).FirstOrDefault();
            return item;
        }

        public List<Item> GetItemsparaOferta() // recupera todos los items
        {
            //  var contratoQuery = Repository.GetAll().Where(e => e.vigente == true).ToList();
            var itemQuery = Repository.GetAll().Where(e => e.vigente == true).Where(e => e.para_oferta == true)
                .ToList();
            return itemQuery;
        }

        public List<Item> GetItems() // recupera todos los items
        {
            //  var contratoQuery = Repository.GetAll().Where(e => e.vigente == true).ToList();
            var itemQuery = Repository.GetAll().Where(e => e.vigente == true).ToList();
            return itemQuery.OrderBy(c => c.codigo).ToList();
        }

        public List<Item> GetItemsHijos(string item_padre) // recupera los items de todo los hijos
        {
            var listaitems = Repository.GetAll().Where(e => e.vigente == true).Where(e => e.item_padre == item_padre).OrderBy(c => c.codigo).ToList();
            if (listaitems != null && listaitems.Count > 0)
            {
                return listaitems;
            }
            else
            {
                return new List<Item>();
            }

        }

        public List<Item> GetItemsporContratoActivo(int ContratoId, DateTime fechaoferta)
        {
            List<Item> listanueva = new List<Item>();

            var preciario = _repositorypreciario.GetAllIncluding(c => c.Contrato)
            .Where(c => c.ContratoId == ContratoId)
            .Where(c => c.fecha_desde <= fechaoferta)
            .Where(c => c.fecha_hasta >= fechaoferta)
            .Where(c => c.vigente == true).FirstOrDefault();

            if (preciario != null)
            {
                var listadetallepreciario = repositorydetallepreciario
                    .GetAllIncluding(c => c.Item, c => c.Preciario, c => c.Preciario.Contrato)
                    .Where(c => c.vigente == true).Where(c => c.PreciarioId == preciario.Id).OrderBy(c => c.Item.codigo)
                    .ToList();


                foreach (var detalleitem in listadetallepreciario)
                {

                    listanueva.Add(detalleitem.Item);

                }
            }



            return listanueva;
        }

        public bool siexisteid(string codigo)
        {
            List<Item> listanueva = Repository.GetAll().Where(e => e.vigente == true).Where(c => c.codigo == codigo).ToList();

            if (listanueva != null && listanueva.Count > 0)
            {
                return true;
            }
            else
            {

                return false;
            }

        }

        public bool siexisteidEdit(string codigo, int id)
        {
            List<Item> listanueva = Repository.GetAll().Where(c => c.Id != id).Where(c => c.codigo == codigo).ToList();

            if (listanueva != null && listanueva.Count > 0)
            {
                return true;
            }
            else
            {

                return false;
            }

        }

        public TreeItem ExtraerHijos(Item item)
        {
            List<Item> sihijos = GetItemsHijos(item.codigo);
            if (sihijos.Count > 0)
            {
                var listahijos = new List<TreeItem>();
                foreach (var h in sihijos)
                {
                    var lhijos = ExtraerHijos(h);
                    listahijos.Add(lhijos);
                }

                return new TreeItem()
                {

                    label = item.codigo + " " + item.nombre,
                    labelcompleto = item.codigo + " " + item.nombre,
                    data = "" + item.codigo,
                    id = item.Id,
                    expandedIcon = "fa fa-fw fa-folder-open",
                    collapsedIcon = "fa fa-fw fa-folder",
                    children = listahijos
                };
            }
            else
            {
                return new TreeItem()
                {
                    label = item.codigo + " " + item.nombre,
                    labelcompleto = item.codigo + " " + item.nombre,
                    id = item.Id,
                    data = "" + item.codigo,
                    icon = "fa fa-fw fa-file-word-o",

                };

            }


        }

        public List<TreeItem> GenerarArbol()
        {
            var i = Repository.GetAll().Where(c => c.item_padre == ".").Where(e => e.vigente == true);

            var Lista = new List<TreeItem>();
            if (i.ToList().Count > 0)
            {
                foreach (var x in i.ToList())
                {
                    var item = ExtraerHijos(x);
                    Lista.Add(item);
                }

                return Lista;
            }
            else
            {
                return Lista;
            }
        }

        public List<Item> GetItemsHijosContenido(string item_codigo)
        {
            List<Item> listanueva = new List<Item>();
            ;
            var listaitems = GetItems();

            foreach (var item in listaitems)
            {
                if (item.item_padre == item_codigo)
                {
                    List<Item> listahijos = GetItemsHijosContenido(item.codigo);

                    if (listahijos.Count > 0)
                    {
                        foreach (var h in listahijos)
                        {
                            listanueva.Add(h);
                        }
                    }

                    listanueva.Add(item);
                }
            }

            return listanueva;

        }

        public List<Item> GetItemsPor(string item_padre)
        {
            var itemQuery = Repository.GetAll().Where(e => e.vigente == true).Where(e => e.item_padre == item_padre)
                .ToList();
            return itemQuery;
        }

        public List<ItemDto> GetItemsParaOferta()
        {
            var itemQuery = Repository.GetAll().Where(e => e.vigente == true).Where(e => e.para_oferta == true);
            var items = (from i in itemQuery
                         select new ItemDto()
                         {
                             Id = i.Id,
                             vigente = i.vigente,
                             nombre = i.nombre,
                             GrupoId = i.GrupoId,
                             UnidadId = i.UnidadId,
                             codigo = i.codigo,
                             descripcion = i.descripcion,
                             item_padre = i.item_padre,
                             para_oferta = i.para_oferta,

                         }).ToList();

            foreach (var e in items)
            {
                if (e.Id != null)
                {
                    int padreid = buscaridentificadorpadre(e.item_padre);
                    if (padreid != 0)
                    {
                        var I = GetDetalle(padreid);
                        e.nombrepadre = I.Result.nombre;

                    }
                }

            }

            return items;
        }

        public bool comprobaritemmovimiento(string padre)
        {
            string punto = padre.Substring(padre.Length - 1, 1);
            if (punto == ".")
            {
                return true;
            }

            return false;
        }

        public List<Item> JerarquiaItem(int id)
        {
            List<Item> Jerarquia = new List<Item>();

            //ItemDto item = this.DatosItem(id);
            Item item = this.FastDetalle(id);
            Jerarquia.Add(item);
            while (item != null && item.item_padre != ".")
            {

                item = this.ObtenerPadre(item.item_padre);
                if (item != null && item.Id > 0)
                {
                    Jerarquia.Add(item);
                }
            }
            var items_reordenados = (from e in Jerarquia
                                     orderby Convert.ToInt32(e.codigo.ToUpper().Replace("X", "").Replace("A", "").Replace(".", ""))
                                     select e).ToList();
            return items_reordenados.ToList();
        }

        public Item ObtenerPadre(string codigopadre)
        {
            var item = Repository.GetAll().Where(e => e.vigente == true).Where(e => e.codigo == codigopadre).FirstOrDefault();

            if (item != null)
            {
                return item;

            }
            else
            {
                return null;
            }
        }

        public ItemDto DatosItem(int id)
        {
            var items = Repository.GetAll();

            var ItemPadre = (from i in items
                             where i.vigente == true
                             where i.Id == id
                             select new ItemDto
                             {
                                 Id = i.Id,
                                 codigo = i.codigo,
                                 item_padre = i.item_padre,
                                 vigente = i.vigente,
                                 GrupoId = i.GrupoId,
                                 UnidadId = i.UnidadId,
                                 nombre = i.nombre,
                                 para_oferta = i.para_oferta
                             }).FirstOrDefault();
            if (ItemPadre != null)
            {
                return ItemPadre;

            }
            else
            {
                return null;
            }
        }

        public List<Item> ArbolWbsExcel(int ContratoId, DateTime fechaoferta)
        {
            List<Item> all = new List<Item>();
            var wbs = this.GetItemsporContratoActivo2(ContratoId, fechaoferta);

            foreach (var item in wbs)
            {

                var x = this.JerarquiaItem(item.Id);
                foreach (var item2 in x)
                {
                    bool siesta = all.Any(z => z.Id == item2.Id);
                    if (!siesta)
                    {
                        all.Add(item2);
                    }
                }


            }

            return all.ToList();
        }

        public List<Item> GetItemsporContratoActivo2(int ContratoId, DateTime fechaoferta)
        {
            var lista_items = repositorydetallepreciario.GetAll().Where(c => c.vigente)
                                                                .Where(c => c.Preciario.ContratoId == ContratoId)
                                                                .Where(c => c.Preciario.fecha_desde <= fechaoferta)
                                                                .Where(c => c.Preciario.fecha_hasta >= fechaoferta)
                                                                .Where(c => c.Preciario.vigente)
                                                                .Select(c => c.Item)
                                                                .ToList();

            return lista_items;


            /*
            List<Item> listanueva = new List<Item>();
                var preciario = _repositorypreciario.GetAll()
                .Where(c => c.ContratoId == ContratoId)
                .Where(c => c.fecha_desde <= fechaoferta)
                .Where(c => c.fecha_hasta >= fechaoferta)
                .Where(c => c.vigente == true).FirstOrDefault();

            if (preciario != null)
            {
                var listadetallepreciario = repositorydetallepreciario
                    .GetAllIncluding(c => c.Item)
                    .Where(c => c.vigente == true).Where(c => c.PreciarioId == preciario.Id).OrderBy(c => c.Item.codigo)
                    .ToList();

                listanueva.AddRange(listadetallepreciario.Select(x => x.Item));
               
               
            }
            return listanueva;

    */

        }

        public Item FastDetalle(int id)
        {
            var item = Repository.GetAll().Where(i => i.Id == id).Where(i => i.vigente == true).FirstOrDefault();

            if (item != null)
            {
                return item;

            }
            else
            {
                return null;
            }
        }

        public List<Item> ItemsReporteExcel()
        {
            var wbs = Repository.GetAll().Where(c => c.vigente == true);
            return wbs.OrderBy(c => c.codigo).ToList();
        }

        public List<Item> ArbolWbsExcelPresupuesto(int ContratoId, DateTime fechaoferta)
        {
            List<Item> all = new List<Item>();

            var items = this.GetItemsporContratoActivo2(ContratoId, fechaoferta).Where(c => c.GrupoId != 3);

            foreach (var item in items)
            {


                var x = this.JerarquiaItem(item.Id);
                foreach (var item2 in x)
                {
                    bool siesta = all.Any(z => z.Id == item2.Id);
                    if (!siesta)
                    {
                        all.Add(item2);
                    }
                }


            }

            return all.OrderBy(c => c.codigo).ToList();

        }

        public List<Item> ObtenerProcuraHijos(string codigo, List<Item> items_procura)
        {
            List<Item> Estructura = new List<Item>();
            var items = (from i in items_procura where i.item_padre == codigo where i.vigente select i).ToList();

            var items_reordenados = (from e in items
                                     orderby Convert.ToInt32(e.codigo.ToUpper().Replace("X", "").Replace("A", "").Replace(".", ""))
                                     select e).ToList();
            if (items_reordenados.Count > 0)
            {
                foreach (var item in items_reordenados)
                {
                    Estructura.Add(item);
                    var lista = ObtenerProcuraHijos(item.codigo, items_procura);
                    Estructura.AddRange(lista);
                }
            }
            else
            {
                Estructura = (from e in items_procura
                              where e.item_padre == codigo
                              where e.para_oferta
                              orderby Convert.ToInt32(e.codigo.ToUpper().Replace("X", "").Replace("A", "").Replace(".", ""))
                              select e).ToList();
            }
            return Estructura;

        }

        public List<Item> ObtenerItemsProcura()
        {
            List<Item> Estructura = new List<Item>();
            var principales_procura = Repository.GetAll().Where(c => c.vigente)
                                                 /*.Where(c => c.item_padre == ".")
                                                 .Where(c => !c.para_oferta)
                                                 */.Where(c => c.Grupo.codigo == ProyectoCodigos.CODE_PROCURA_CONTRATISTA)
                                                 .ToList();
            var items_reordenados = (from e in principales_procura
                                     where e.item_padre == "."
                                     orderby Convert.ToInt32(e.codigo.ToUpper().Replace("X", "").Replace("A", "").Replace(".", ""))
                                     select e).ToList();
            if (items_reordenados.Count > 0)
            {
                foreach (var item in items_reordenados)
                {
                    Estructura.Add(item);
                    var listahijos = this.ObtenerProcuraHijos(item.codigo, principales_procura);
                    if (listahijos.Count > 0)
                    {
                        Estructura.AddRange(listahijos);
                    }

                }

            }



            return Estructura;
        }

        public List<Item> ArbolItemsCertificadoSinPendientes(int ProyectoId, DateTime fechaCorte)
        {
            List<Item> all = new List<Item>();


            var items = _detalleavancebrarepository.GetAllIncluding(x => x.AvanceObra.Oferta, x => x.Computo.Item)
                                                   .Where(x => x.AvanceObra.vigente)
                                                   .Where(x => x.AvanceObra.aprobado)
                                                   .Where(x => x.AvanceObra.Oferta.es_final)
                                                   .Where(x=>x.AvanceObra.fecha_presentacion<=fechaCorte)
                                                   .Where(x => x.Computo.vigente)
                                                   .Where(x => x.AvanceObra.Oferta.ProyectoId == ProyectoId)
                                                   .Where(x => x.vigente)
                                                   .Where(x => !x.Computo.Item.PendienteAprobacion)//Nuevo SIn Pendietes
                                                   .Where(x => !x.Computo.es_temporal)//temporales
                                                   .Select(c => c.Computo.Item)                                             
                                                                .ToList();
         
            foreach (var item in items)
            {

                var x = this.JerarquiaItem(item.Id);
                foreach (var item2 in x)
                {
                    bool siesta = all.Any(z => z.Id == item2.Id);
                    if (!siesta)
                    {
                        all.Add(item2);
                    }
                }


            }

            return all.ToList();

        }

        public List<Item> ArbolItemsCertificadoPendientesAprobacion(int ProyectoId,DateTime fechaCorte)
        {
            List<Item> all = new List<Item>();
            var items = _detalleavancebrarepository.GetAllIncluding(x => x.AvanceObra.Oferta, x => x.Computo.Item)
                                                   .Where(x => x.AvanceObra.vigente)
                                                     .Where(x => x.AvanceObra.aprobado)
                                                     .Where(x => x.AvanceObra.Oferta.es_final)
                                                           .Where(x => x.AvanceObra.fecha_presentacion <= fechaCorte)
                                                        .Where(x => x.Computo.vigente)

                                                   .Where(x => x.AvanceObra.Oferta.ProyectoId == ProyectoId)
                                                   .Where(x => x.vigente)

                                                   .Where(x => x.Computo.Item.PendienteAprobacion || x.Computo.es_temporal)//Nuevo Pendientes

                                                   .Select(c => c.Computo.Item).ToList().Distinct().ToList(); ;
            /*foreach (var item in items)
            {

                var x = this.JerarquiaItem(item.Id);
                foreach (var item2 in x)
                {
                    bool siesta = all.Any(z => z.Id == item2.Id);
                    if (!siesta)
                    {
                        all.Add(item2);
                    }
                }


            }

            return all.OrderBy(c => c.codigo).ToList();*/
            var items_reordenados = (from e in items
                                     orderby Convert.ToInt32(e.codigo.ToUpper().Replace("X", "").Replace("A", "").Replace(".", ""))
                                     select e).ToList();
            return items_reordenados;
        }

        public List<Item> ArbolItemsComputoComercial(int OfertaComercialId)
        {
            List<Item> all = new List<Item>();
            var items = _computocomercialrepository
                           .GetAllIncluding(x => x.WbsComercial, x => x.Item)
                           .Where(x => x.vigente)
                           .Where(x => x.WbsComercial.OfertaComercialId == OfertaComercialId)
                           .Select(x => x.Item).ToList();
            foreach (var item in items)
            {

                var x = this.JerarquiaItem(item.Id);
                foreach (var item2 in x)
                {
                    bool siesta = all.Any(z => z.Id == item2.Id);
                    if (!siesta)
                    {
                        all.Add(item2);
                    }
                }


            }

            return all.OrderBy(c => c.codigo).ToList();
        }

        public List<Item> EstructuraItems(int ContratoId, DateTime Fecha)
        {
            List<Item> Estructura = new List<Item>();
            var Items_Preciario = this.GetItemsporContratoActivo2(ContratoId, Fecha).Where(c => c.GrupoId != 3).ToList();//ver esto solo para 
            var principales = Repository.GetAll().Where(c => c.vigente)
                                                 .Where(c => c.item_padre == ".")
                                                 .Where(c => !c.para_oferta)
                                                 .Where(c => c.GrupoId != 3).ToList();
            var items_reordenados = (from e in principales
                                     orderby Convert.ToInt32(e.codigo.ToUpper().Replace("X", "").Replace("A", "").Replace(".", ""))
                                     select e).ToList();
            if (items_reordenados.Count > 0)
            {
                foreach (var item in items_reordenados)
                {
                    Estructura.Add(item);
                    var listahijos = this.EstructuraItemsHijos(item.codigo, Items_Preciario);
                    if (listahijos.Count > 0)
                    {
                        Estructura.AddRange(listahijos);
                    }

                }
            }



            return Estructura;
        }

        public List<Item> EstructuraItemsHijos(string codigo, List<Item> detalleitems)
        {
            List<Item> Estructura = new List<Item>();
            var items = Repository.GetAll().Where(c => c.item_padre == codigo)
                                           .Where(c => c.vigente)
                                           .Where(c => !c.para_oferta)
                                           .Where(c => c.GrupoId != 3).ToList();

            var items_reordenados = (from e in items
                                     orderby Convert.ToInt32(e.codigo.ToUpper().Replace("X", "").Replace("A", "").Replace(".", ""))
                                     select e).ToList();
            if (items_reordenados.Count > 0)
            {
                foreach (var item in items_reordenados)
                {
                    Estructura.Add(item);
                    var lista = EstructuraItemsHijos(item.codigo, detalleitems);
                    Estructura.AddRange(lista);
                }
            }
            else
            {
                Estructura = (from e in detalleitems
                              where e.item_padre == codigo
                              where e.para_oferta
                              orderby Convert.ToInt32(e.codigo.ToUpper().Replace("X", "").Replace("A", "").Replace(".", ""))
                              select e).ToList();
            }
            return Estructura;
        }


        public List<Item> EstructuraArbolItems(int ContratoId, DateTime Fecha)
        {
            List<Item> Estructura = new List<Item>();
            var Items_Preciario = this.GetItemsporContratoActivo2(ContratoId, Fecha).ToList();//ver esto solo para 
            var principales = Repository.GetAll().Where(c => c.vigente)
                                                 .Where(c => c.item_padre == ".")
                                                 .Where(c => !c.para_oferta)
                                                 .ToList();
            var items_reordenados = (from e in principales
                                     orderby Convert.ToInt32(e.codigo.ToUpper().Replace("X", "").Replace("A", "").Replace(".", ""))
                                     select e).ToList();
            if (items_reordenados.Count > 0)
            {
                foreach (var item in items_reordenados)
                {
                    Estructura.Add(item);
                    var listahijos = this.EstructuraItemsHijos(item.codigo, Items_Preciario);
                    if (listahijos.Count > 0)
                    {
                        Estructura.AddRange(listahijos);
                    }

                }
            }



            return Estructura;
        }

        public List<Item> EstructuraArbolItemsHijos(string codigo, List<Item> detalleitems)
        {
            List<Item> Estructura = new List<Item>();
            var items = Repository.GetAll().Where(c => c.item_padre == codigo)
                                           .Where(c => c.vigente)
                                           .Where(c => !c.para_oferta)
                                           .ToList();

            var items_reordenados = (from e in items
                                     orderby Convert.ToInt32(e.codigo.ToUpper().Replace("X", "").Replace("A", "").Replace(".", ""))
                                     select e).ToList();
            if (items_reordenados.Count > 0)
            {
                foreach (var item in items_reordenados)
                {
                    Estructura.Add(item);
                    var lista = EstructuraItemsHijos(item.codigo, detalleitems);
                    Estructura.AddRange(lista);
                }
            }
            else
            {
                Estructura = (from e in detalleitems
                              where e.item_padre == codigo
                              where e.para_oferta
                              orderby Convert.ToInt32(e.codigo.ToUpper().Replace("X", "").Replace("A", "").Replace(".", ""))
                              select e).ToList();
            }
            return Estructura;
        }

        public List<NodeItem> TreeDataArbol()
        {
            var Lista = new List<NodeItem>();
            List<Item> padres_ordenados = new List<Item>();

            var padres = Repository.GetAllIncluding(c => c.Especialidad).Where(c => c.item_padre == ".").Where(e => e.vigente == true).ToList();

            var listaitems = Repository.GetAllIncluding(c => c.Especialidad).Where(e => e.vigente == true).ToList();


            /*  Items Padres Ordenados */
            if (padres.Count > 0)
            {
                padres_ordenados = (from e in padres
                                    orderby Convert.ToInt32(e.codigo.ToUpper().Replace("X", "").Replace("A", "").Replace(".", ""))
                                    select e).ToList();
            }

            if (padres_ordenados.Count > 0)
            {
                foreach (var x in padres_ordenados)
                {
                    var item = NodeExtraerHijos(x, listaitems);
                    Lista.Add(item);
                }

                return Lista;
            }
            else
            {
                return Lista;
            }


        }

        public NodeItem NodeExtraerHijos(Item item, List<Item> listaitems)
        {
            List<Item> hijos_ordenados = new List<Item>();
            List<Item> sihijos = (from i in listaitems
                                  where i.item_padre == item.codigo
                                  where i.vigente
                                  select i).ToList();

            if (sihijos.Count > 0)
            {

                hijos_ordenados = (from e in sihijos
                                   orderby Convert.ToInt32(e.codigo.ToUpper().Replace("X", "").Replace("A", "").Replace(".", ""))
                                   select e).ToList();
            }
            if (hijos_ordenados.Count > 0)
            {
                var listahijos = new List<NodeItem>();
                foreach (var h in hijos_ordenados)
                {
                    var lhijos = NodeExtraerHijos(h, listaitems);
                    listahijos.Add(lhijos);
                }

                return new NodeItem()
                {
                    key = item.Id,
                    label = item.codigo + " " + item.nombre,
                    labelcompleto = item.codigo + " " + item.nombre,
                    data = "" + item.codigo,
                    id = item.Id,
                    expandedIcon = "fa fa-fw fa-folder-open",
                    collapsedIcon = "fa fa-fw fa-folder",
                    children = listahijos,
                    EspecialidadId = item.EspecialidadId.HasValue ? item.EspecialidadId.Value : 0,
                    NombreEspecialidad = item.Especialidad != null ? item.Especialidad.nombre : "",
                    selectable = true,
                    UnidadId = item.UnidadId,
                    GrupoId = item.GrupoId,
                    para_oferta = item.para_oferta


                };
            }
            else
            {
                return new NodeItem()
                {
                    key = item.Id,
                    label = item.codigo + " " + item.nombre,
                    labelcompleto = item.codigo + " " + item.nombre,
                    id = item.Id,
                    data = "" + item.codigo,
                    icon = "fa fa-fw fa-file-word-o",
                    EspecialidadId = item.EspecialidadId.HasValue ? item.EspecialidadId.Value : 0,
                    NombreEspecialidad = item.Especialidad != null ? item.Especialidad.nombre : "",
                    selectable = true,
                    UnidadId = item.UnidadId,
                    GrupoId = item.GrupoId,
                    para_oferta = item.para_oferta
                };

            }


        }

        public InfoItem DetailsAPIItem(int Id)
        {
            var q = Repository.GetAllIncluding(c => c.Catalogo, c => c.Especialidad).Where(c => c.Id == Id).Where(c => c.vigente).FirstOrDefault();

            if (q != null && q.Id > 0)
            {
                var hijos = Repository.GetAll().Where(c => c.vigente).Where(c => c.item_padre == q.codigo).ToList();
                /*ES */
                var apicodigo = q.codigo.TrimEnd('.').Split('.');

                InfoItem e = new InfoItem();
                e.Id = q.Id;
                e.codigo = q.codigo;
                e.item_padre = q.item_padre;
                e.nombre = q.nombre;
                e.EspecialidadId = q.EspecialidadId.HasValue ? q.EspecialidadId.Value : 0;
                e.UnidadId = q.UnidadId;
                e.descripcion = q.descripcion;
                e.para_oferta = q.para_oferta;
                e.GrupoId = q.GrupoId;
                e.PendienteAprobacion = q.PendienteAprobacion;
                e.vigente = q.PendienteAprobacion;
                e.apicodigo = apicodigo[apicodigo.Length - 1];
                e.tieneHijos = hijos.Count > 0 ? hijos.Count : 0;

                return e;
            }
            else
            {

                return new InfoItem();
            }
        }

        public bool Eliminar(int Id)
        {
            var item = Repository.Get(Id);
            item.vigente = false;
            var update = Repository.Update(item);
            if (update.Id > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public List<Item> EstructuraItemMatrizPresupuesto(int ContratoId, DateTime Fecha)
        {
            List<Item> Estructura = new List<Item>();

            var Items_Preciario = this.GetItemsporContratoActivo2(ContratoId, Fecha).ToList();
            var principales = Repository.GetAllIncluding(c => c.Grupo, c => c.Especialidad)
                                                 .Where(c => c.vigente)
                                                 .Where(c => c.item_padre == ".")
                                                 .Where(c => !c.para_oferta)
                                                 .ToList();

            var items_reordenados = (from e in principales
                                     orderby
               Convert.ToInt32(
               e.codigo.ToUpper()
                       .Replace("X", "")
                       .Replace("A", "")
                       .Replace(".", "")
                              )
                                     select e).ToList();
            if (items_reordenados.Count > 0)
            {
                foreach (var item in items_reordenados)
                {
                    Estructura.Add(item);
                    var listahijos = this.ItemsHijos(item.codigo, Items_Preciario);
                    if (listahijos.Count > 0)
                    {
                        Estructura.AddRange(listahijos);
                    }

                }
            }



            return Estructura;
        }

        public List<Item> ItemsHijos(string codigo, List<Item> detalleitems)
        {
            List<Item> Estructura = new List<Item>();
            var items = Repository.GetAllIncluding(c => c.Grupo, c => c.Especialidad)
                                           .Where(c => c.item_padre == codigo)
                                           .Where(c => c.vigente)
                                           .Where(c => !c.para_oferta)
                                           .ToList();

            var items_reordenados = (from e in items
                                     orderby Convert.ToInt32(
                         e.codigo.ToUpper()
                          .Replace("X", "")
                          .Replace("A", "")
                          .Replace(".", ""))
                                     select e).ToList();

            if (items_reordenados.Count > 0)
            {
                foreach (var item in items_reordenados)
                {
                    Estructura.Add(item);
                    var lista = ItemsHijos(item.codigo, detalleitems);
                    Estructura.AddRange(lista);
                }
            }
            else
            {
                Estructura = (from e in detalleitems
                              where e.item_padre == codigo
                              where e.para_oferta
                              orderby Convert.ToInt32(
                                  e.codigo.ToUpper()
                                   .Replace("X", "")
                                   .Replace("A", "")
                                   .Replace(".", ""))
                              select e).ToList();
            }
            return Estructura;
        }

        public List<Item> ItemsMatrizPresupuesto(int ContratoId, DateTime Fecha, List<ComputoPresupuestoDto> computos)
        {
            List<Item> Estructura = new List<Item>();

            var Items_Preciario = this.GetItemsporContratoActivo2(ContratoId, Fecha).ToList();//ver esto solo para 
            if (computos != null && computos.Count > 0)
            {
                var procura_contratista = (from p in computos where p.codigo_grupo == ProyectoCodigos.CODE_PROCURA_CONTRATISTA select p.ItemId).ToList().Distinct().ToList();
                if (procura_contratista.Count > 0)
                {
                    foreach (var Id in procura_contratista)
                    {
                        var i = Repository.GetAll().Where(c => c.Id == Id).FirstOrDefault();
                        if (!Items_Preciario.Select(c=>c.Id).ToList().Contains(Id)) {
                            Items_Preciario.Add(i);
                        }
                    
                    }

                }
                var subcontratos = (from p in computos where p.codigo_grupo == ProyectoCodigos.CODE_SUBCONTRATOS_CONTRATISTA select p.ItemId).ToList().Distinct().ToList();
                if (subcontratos.Count > 0)
                {
                    foreach (var Id in subcontratos)
                    {
                        var i = Repository.GetAll().Where(c => c.Id == Id).FirstOrDefault();
                        if (!Items_Preciario.Select(c => c.Id).ToList().Contains(Id))
                        {
                            Items_Preciario.Add(i);
                        }
                    
                    }
                }

            }
            var principales = Repository.GetAll().Where(c => c.vigente)
                                                 .Where(c => c.item_padre == ".")
                                                 .Where(c => !c.para_oferta)
                                                 .ToList();
            var items_reordenados = (from e in principales
                                     orderby Convert.ToInt32(e.codigo.ToUpper().Replace("X", "").Replace("A", "").Replace(".", ""))
                                     select e).ToList();
            if (items_reordenados.Count > 0)
            {
                foreach (var item in items_reordenados)
                {
                    Estructura.Add(item);
                    var listahijos = this.ItemsMatrizPresupuestoHijos(item.codigo, Items_Preciario);
                    if (listahijos.Count > 0)
                    {
                        Estructura.AddRange(listahijos);
                    }

                }
            }



            return Estructura;
        }

        public List<Item> ItemsMatrizPresupuestoHijos(string codigo, List<Item> detalleitems)
        {

            List<Item> Estructura = new List<Item>();
            var items = Repository.GetAll().Where(c => c.item_padre == codigo)
                                           .Where(c => c.vigente)
                                           .Where(c => !c.para_oferta).ToList();

            var items_reordenados = (from e in items
                                     orderby Convert.ToInt32(e.codigo.ToUpper().Replace("X", "").Replace("A", "").Replace(".", ""))
                                     select e).ToList();
            if (items_reordenados.Count > 0)
            {
                foreach (var item in items_reordenados)
                {
                    Estructura.Add(item);
                    var lista = ItemsMatrizPresupuestoHijos(item.codigo, detalleitems);
                    Estructura.AddRange(lista);
                }
            }
            else
            {
                Estructura = (from e in detalleitems
                              where e.item_padre == codigo
                              where e.para_oferta
                              orderby Convert.ToInt32(e.codigo.ToUpper().Replace("X", "").Replace("A", "").Replace(".", ""))
                              select e).ToList();
            }
            return Estructura;
        }



        public List<Item> ItemsMatrizComercial(int ContratoId, DateTime Fecha, List<ComputoComercialDto> computos)
        {
            List<Item> Estructura = new List<Item>();

            var Items_Preciario = this.GetItemsporContratoActivo2(ContratoId, Fecha).ToList();//ver esto solo para 
            if (computos != null && computos.Count > 0)
            {
                var procura_contratista = (from p in computos where p.codigo_grupo == ProyectoCodigos.CODE_PROCURA_CONTRATISTA select p.ItemId).ToList().Distinct().ToList();
                if (procura_contratista.Count > 0)
                {
                    foreach (var Id in procura_contratista)
                    {
                        var i = Repository.GetAll().Where(c => c.Id == Id).FirstOrDefault();
                        Items_Preciario.Add(i);
                    }

                }
                var subcontratos = (from p in computos where p.codigo_grupo == ProyectoCodigos.CODE_SUBCONTRATOS_CONTRATISTA select p.ItemId).ToList().Distinct().ToList();
                if (subcontratos.Count > 0)
                {
                    foreach (var Id in subcontratos)
                    {
                        var i = Repository.GetAll().Where(c => c.Id == Id).FirstOrDefault();
                        Items_Preciario.Add(i);
                    }
                }

            }
            var principales = Repository.GetAll().Where(c => c.vigente)
                                                 .Where(c => c.item_padre == ".")
                                                 .Where(c => !c.para_oferta)
                                                 .ToList();
            var items_reordenados = (from e in principales
                                     orderby Convert.ToInt32(e.codigo.ToUpper().Replace("X", "").Replace("A", "").Replace(".", ""))
                                     select e).ToList();
            if (items_reordenados.Count > 0)
            {
                foreach (var item in items_reordenados)
                {
                    Estructura.Add(item);
                    var listahijos = this.ItemsMatrizPresupuestoHijos(item.codigo, Items_Preciario);
                    if (listahijos.Count > 0)
                    {
                        Estructura.AddRange(listahijos);
                    }

                }
            }



            return Estructura;
        }

        public List<Item> ObtenerItemsProcuraporPresupuesto(int PresupuestoId)
        {
            var result = new List<Item>();
            var queryitemsprocura = _computopresupuestorepository.GetAllIncluding(c => c.WbsPresupuesto, c => c.Item.Grupo)
                                                         .Where(c => c.WbsPresupuesto.PresupuestoId == PresupuestoId)
                                                         .Where(c => c.vigente)
                                                         .Where(c => c.WbsPresupuesto.vigente)
                                                         .Where(c => c.Item.Grupo.codigo == ProyectoCodigos.CODE_PROCURA_CONTRATISTA)
                                                         .Select(c => c.ItemId).ToList().Distinct().ToList();
            if (queryitemsprocura.Count > 0)
            {
                foreach (var Id in queryitemsprocura)
                {
                    var i = Repository.GetAll().Where(c => c.Id == Id).FirstOrDefault();
                    if (i != null)
                    {
                        result.Add(i);
                    }

                }
                return result;
            }
            else
            {
                return new List<Item>();
            }
        }

        public List<Item> ArbolItemsCertificadoSinPendientesUltimoRdo(int ProyectoId, DateTime fechaCorte, List<RdoDetalleEac> detallesUltimoRdo)
        {
            List<Item> all = new List<Item>();

            var items = (from x in detallesUltimoRdo
                         where !x.Computo.Item.PendienteAprobacion
                         where !x.Computo.es_temporal
                         where x.ac_actual != 0
                         select x.Computo.Item)
                       .ToList();

          /*  var items = _detalleavancebrarepository.GetAllIncluding(x => x.AvanceObra.Oferta, x => x.Computo.Item)
                                                   .Where(x => x.AvanceObra.vigente)
                                                   .Where(x => x.AvanceObra.aprobado)
                                                   .Where(x => x.AvanceObra.Oferta.es_final)
                                                   .Where(x => x.AvanceObra.fecha_presentacion <= fechaCorte)
                                                   .Where(x => x.Computo.vigente)
                                                   .Where(x => x.AvanceObra.Oferta.ProyectoId == ProyectoId)
                                                   .Where(x => x.vigente)
                                                   .Where(x => !x.Computo.Item.PendienteAprobacion)//Nuevo SIn Pendietes
                                                   .Where(x => !x.Computo.es_temporal)//temporales
                                                   .Select(c => c.Computo.Item)
                                                                .ToList();
                                                                */
            foreach (var item in items)
            {

                var x = this.JerarquiaItem(item.Id);
                foreach (var item2 in x)
                {
                    bool siesta = all.Any(z => z.Id == item2.Id);
                    if (!siesta)
                    {
                        all.Add(item2);
                    }
                }


            }

            return all.ToList();
        }

        public List<Item> ArbolItemsCertificadoPendientesAprobacionUltimoRdo(int ProyectoId, DateTime fechaCorte, List<RdoDetalleEac> detallesUltimoRdo)
        {
            List<Item> all = new List<Item>();

            var items = (from x in detallesUltimoRdo
                         where x.Computo.Item.PendienteAprobacion || x.Computo.es_temporal
                         where x.ac_actual != 0
                         select x.Computo.Item)
                       .ToList().Distinct().ToList();

          /*  var items = _detalleavancebrarepository.GetAllIncluding(x => x.AvanceObra.Oferta, x => x.Computo.Item)
                                                   .Where(x => x.AvanceObra.vigente)
                                                     .Where(x => x.AvanceObra.aprobado)
                                                     .Where(x => x.AvanceObra.Oferta.es_final)
                                                           .Where(x => x.AvanceObra.fecha_presentacion <= fechaCorte)
                                                        .Where(x => x.Computo.vigente)

                                                   .Where(x => x.AvanceObra.Oferta.ProyectoId == ProyectoId)
                                                   .Where(x => x.vigente)

                                                   .Where(x => x.Computo.Item.PendienteAprobacion || x.Computo.es_temporal)//Nuevo Pendientes

                                                   .Select(c => c.Computo.Item).ToList().Distinct().ToList(); 

    */
 
            var items_reordenados = (from e in items
                                     orderby Convert.ToInt32(e.codigo.ToUpper().Replace("X", "").Replace("A", "").Replace(".", ""))
                                     select e).ToList();
            return items_reordenados;
        }
    }
}
