using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using AutoMapper;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.aplicacion.Interfaces;
using com.cpp.calypso.proyecto.dominio;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace com.cpp.calypso.proyecto.aplicacion
{


    public class WbsAsyncBaseCrudAppService : AsyncBaseCrudAppService<Wbs, WbsDto, PagedAndFilteredResultRequestDto>,
        IWbsAsyncBaseCrudAppService
    {
        private readonly IComputosTemporalAsyncBaseCrudAppService _computosTemporalService;
        private readonly IComputoAsyncBaseCrudAppService _computoService;
        private readonly IBaseRepository<Computo> repositorycomputo;
        private readonly IBaseRepository<Catalogo> repositorycatalogo;
        private readonly IBaseRepository<DetallePreciario> repositorydetallepreciario;
        private readonly IBaseRepository<DetalleAvanceObra> _detalleAvanceObraRepository;
        public readonly IBaseRepository<Item> itemrepository;
        private readonly IBaseRepository<Catalogo> _catalogoRepository;
        private readonly IBaseRepository<Oferta> _ofertaRepository;

        private readonly IBaseRepository<Preciario> repositorypreciario;
        private readonly IBaseRepository<Ganancia> _gananciarepository;
        public readonly IBaseRepository<DetalleGanancia> _detallegananciarepository;
        private readonly IBaseRepository<WbsPresupuesto> _wbsPresupuestoRepository;
        private readonly IBaseRepository<ComputoPresupuesto> _computoPresupuesto;
        private readonly IBaseRepository<Computo> _computoRepository;
        private readonly IWbsPresupuestoAsyncBaseCrudAppService _wbsPresupuestoService;
        private readonly IBaseRepository<ComputoComercial> _computocomercialrepository;

        public WbsAsyncBaseCrudAppService(
            IComputosTemporalAsyncBaseCrudAppService computosTemporalService,
            IBaseRepository<Wbs> repository,
            IBaseRepository<Computo> repositorycomputo,
            IBaseRepository<Catalogo> repositorycatalogo,
            IBaseRepository<Item> itemrepository,
            IBaseRepository<Catalogo> catalogoRepository,
            IBaseRepository<Oferta> ofertaRepository,
            IBaseRepository<Preciario> repositorypreciario,
            IBaseRepository<DetallePreciario> repositorydetallepreciario,
            IBaseRepository<DetalleAvanceObra> detalleAvanceObraRepository,
            IBaseRepository<Ganancia> gananciarepository,
            IBaseRepository<DetalleGanancia> detallegananciarepository,
            IBaseRepository<WbsPresupuesto> wbsPresupuestoRepository,
            IBaseRepository<ComputoPresupuesto> computoPresupuesto,
            IBaseRepository<Computo> computoRepository,
            IWbsPresupuestoAsyncBaseCrudAppService wbsPresupuestoService,
            IBaseRepository<ComputoComercial> computocomercialrepository
            ) : base(repository)
        {
            _computosTemporalService = computosTemporalService;
            this.repositorypreciario = repositorypreciario;
            this.repositorycomputo = repositorycomputo;
            this.itemrepository = itemrepository;
            _catalogoRepository = catalogoRepository;
            _ofertaRepository = ofertaRepository;
            this.repositorydetallepreciario = repositorydetallepreciario;
            _detalleAvanceObraRepository = detalleAvanceObraRepository;
            this.repositorycatalogo = repositorycatalogo;
            _gananciarepository = gananciarepository;
            _detallegananciarepository = detallegananciarepository;
            _wbsPresupuestoRepository = wbsPresupuestoRepository;
            _computoPresupuesto = computoPresupuesto;
            _computoRepository = computoRepository;
            _wbsPresupuestoService = wbsPresupuestoService;
            _computocomercialrepository = computocomercialrepository;
            _computoService = new ComputoServiceAsyncBaseCrudAppService(itemrepository, repositorycomputo, catalogoRepository, repositorydetallepreciario, repositorypreciario, detalleAvanceObraRepository, repository, gananciarepository, detallegananciarepository, _computocomercialrepository, _computoPresupuesto);
        }

        public List<Wbs> GetWbsPorOferta(int ofertaId)
        {
            var items = Repository.GetAll()
                .Where(o => o.vigente == true)
                .Where(o => o.OfertaId == ofertaId).ToList();
            return items;
        }

        public List<Wbs> GetWbsHijos(string codigo_padre, int ofertaId)
        {
            var items = Repository.GetAll()
                .Where(o => o.vigente == true)
                .Where(o => o.OfertaId == ofertaId)
                .Where(o => o.id_nivel_padre_codigo == codigo_padre).ToList();
            return items;
        }

        public List<WbsDto> Listar(int ofertaId)
        {
            var query = Repository.GetAllIncluding(c => c.Catalogo)
                .Where(o => o.vigente == true)
                .Where(o => o.OfertaId == ofertaId)
                .Where(o => o.es_actividad == true)
                .OrderBy(o => o.id_nivel_codigo);

            var items = (from w in query
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

            try
            {
                foreach (var w in items)
                {
                    var name = Repository
                        .GetAll()
                        .Where(o => o.vigente == true)
                        .Where(o => o.OfertaId == w.OfertaId)
                        .SingleOrDefault(o => o.id_nivel_codigo == w.id_nivel_padre_codigo)
                        ;
                    if (name != null)
                    {
                        if (name.Id > 0)
                        {
                            w.nombre_padre = name.nivel_nombre;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                var msmg = e.Message;
            }

            return items;

        }

        public OfertaDto GetClienteProyectoFecha(int ofertaId)
        {
            var wbsQuery = _ofertaRepository.GetAllIncluding(c => c.Proyecto.Contrato.Cliente);

            var item = (from w in wbsQuery
                        where w.Id == ofertaId
                        where w.vigente == true
                        select new OfertaDto()
                        {
                            Id = w.Id,
                            cliente_razon_social = w.Proyecto.Contrato.Cliente.razon_social,
                            proyecto_codigo = w.Proyecto.nombre_proyecto,
                            fecha_oferta = w.fecha_oferta,
                            codigo = w.codigo,
                            version = w.version,
                            es_final = w.es_final
                        }).SingleOrDefault();


            return item;
        }

        public List<TreeWbs> GenerarArbol(int ofertaId)
        {
            var i = Repository.GetAll().Where(c => c.id_nivel_padre_codigo == ".")
                .Where(o => o.OfertaId == ofertaId).Where(o => o.vigente == true);


            var Lista = new List<TreeWbs>();


            foreach (var x in i.ToList().OrderBy(c => c.id_nivel_codigo))
            {
                var item = GenerarNodos(x);
                Lista.Add(item);
            }

            return Lista;
        }

        public TreeWbs GenerarNodos(Wbs wbs)
        {
            List<Wbs> hijos = GetWbsHijos(wbs.id_nivel_codigo, wbs.OfertaId);
            if (hijos.Count > 0)
            {
                var lista_hijos = new List<TreeWbs>();
                foreach (var h in hijos)
                {
                    var lhijos = GenerarNodos(h);
                    lista_hijos.Add(lhijos);
                }
                return new TreeWbs()
                {
                    key = wbs.Id,
                    label = wbs.nivel_nombre,
                    data = wbs.id_nivel_codigo + "," + wbs.id_nivel_padre_codigo + "," + wbs.Id + "," + wbs.OfertaId
                    + "," + wbs.observaciones + "," + wbs.revision + "," + wbs.fecha_inicial.GetValueOrDefault().ToShortDateString() + ","
                        + wbs.fecha_final.GetValueOrDefault().ToShortDateString() + "," + wbs.es_actividad + "," + wbs.estado + "," + wbs.DisciplinaId,
                    expandedIcon = "fa fa-fw fa-folder-open",
                    collapsedIcon = "fa fa-fw fa-folder",
                    tipo = "padre",
                    //expanded = true,
                    children = lista_hijos,
                    selectable = true,
                    draggable = true,
                    droppable = true
                };
            }
            else
            {
                if (wbs.es_actividad)
                {
                    return new TreeWbs()
                    {
                        key = wbs.Id,
                        label = wbs.nivel_nombre,
                        data = wbs.id_nivel_codigo + "," + wbs.id_nivel_padre_codigo + "," + wbs.Id + "," + wbs.OfertaId
                        + "," + wbs.observaciones + "," + wbs.revision + "," + wbs.fecha_inicial.GetValueOrDefault().ToShortDateString() + ","
                        + wbs.fecha_final.GetValueOrDefault().ToShortDateString() + "," + wbs.es_actividad + "," + wbs.estado + "," + wbs.DisciplinaId,
                        icon = "fa fa-fw fa-file-word-o",
                        tipo = "actividad",
                        //expanded = true,
                        selectable = true,
                        draggable = true,
                        droppable = false
                    };
                }
                else
                {
                    return new TreeWbs()
                    {
                        key = wbs.Id,
                        label = wbs.nivel_nombre,
                        data = wbs.id_nivel_codigo + "," + wbs.id_nivel_padre_codigo + "," + wbs.Id + "," + wbs.OfertaId
                         + "," + wbs.observaciones + "," + wbs.revision + "," + wbs.fecha_inicial.GetValueOrDefault().ToShortDateString() + ","
                        + wbs.fecha_final.GetValueOrDefault().ToShortDateString() + "," + wbs.es_actividad + "," + wbs.estado + "," + wbs.DisciplinaId,
                        expandedIcon = "fa fa-fw fa-folder-open",
                        collapsedIcon = "fa fa-fw fa-folder",
                        tipo = "padre",
                        children = new List<TreeWbs>(),
                        selectable = true,
                        draggable = true,
                        droppable = true
                    };
                }

            }
        }


        public WbsDto CrearPadre(WbsDto wbs)
        {
            if (!this.ExisteWbs(wbs))
            {
                wbs.id_nivel_codigo = this.GenerarCodigo(wbs.id_nivel_padre_codigo, wbs.es_actividad, wbs.OfertaId, null);
                var wbsInserted = Repository.Insert(MapToEntity(wbs));
                return MapToEntityDto(wbsInserted);
            }

            return wbs;
        }

        public bool EliminarNivel(int WbsId)
        {
            var wbs = Repository.Get(WbsId);
            var actividades = this.ContarActividadesConComputos(wbs);
            if (actividades > 0)
            {
                return false;
            }
            else
            {
                this.EliminarNodos(wbs);
                return true;
            }
        }

        // Cuenta cuantas actividades hijas con computos tiene el nodo
        public int ContarActividadesConComputos(Wbs wbs)
        {
            List<Wbs> hijos = GetWbsHijos(wbs.id_nivel_codigo, wbs.OfertaId);
            if (hijos.Count > 0)
            {
                var count = 0;
                foreach (var h in hijos)
                {
                    var cactividades = ContarActividadesConComputos(h);
                    count += cactividades;
                }

                return count;
            }
            else
            {
                return ContarComputosPorWbs(wbs.Id);
            }
        }

        public void Eliminar(int id) //WbsId
        {
            var wbs = Repository.Get(id);
            wbs.vigente = false;
            Repository.Update(wbs);
        }

        public void Editar(int WbsId, string nombre)
        {
            var wbs = Repository.Get(WbsId);
            wbs.nivel_nombre = nombre;
            Repository.Update(wbs);
        }

        // Cuenta cuantas actividades hijas con computos tiene el nodo
        public void EliminarNodos(Wbs wbs)
        {
            List<Wbs> hijos = GetWbsHijos(wbs.id_nivel_codigo, wbs.OfertaId);
            if (hijos.Count > 0)
            {
                foreach (var h in hijos)
                {
                    this.EliminarNodos(h);
                }
                this.Eliminar(wbs.Id);
            }
            else
            {
                this.Eliminar(wbs.Id);
            }
        }

        public int ContarComputosPorWbs(int WbsId)
        {
            var count = repositorycomputo
                .GetAll()
                .Where(o => o.vigente)
                //     .Where(o=>o.cantidad>0)//solo si tiene cantidad mayor a cero puede eliminar
                .Count(o => o.WbsId == WbsId);
            return count > 0 ? 1 : 0;
        }


        public void CrearActividades(WbsDto wbs, string[] ActividadesIds)
        {
            foreach (var i in ActividadesIds)
            {
                var nombre = this.GetName(Int32.Parse(i));
                var wbsDto = new WbsDto()
                {
                    OfertaId = wbs.OfertaId,
                    es_actividad = true,
                    estado = true,
                    id_nivel_padre_codigo = wbs.id_nivel_padre_codigo,
                    id_nivel_codigo = this.GenerarCodigo(wbs.id_nivel_padre_codigo, true, wbs.OfertaId, null),
                    nivel_nombre = nombre,
                    observaciones = "",
                    vigente = true,
                };

                if (!ExisteWbs(wbsDto))
                {
                    Repository.Insert(MapToEntity(wbsDto));
                }

            }
        }

        public string GetName(int CatalogoId)
        {
            var query = _catalogoRepository.Get(CatalogoId);

            return query.nombre;
        }

        public string GenerarCodigo(string padre, bool es_actividad, int OfertaId, int? indice)
        {
            //Hijos
            var count = 0;
            if (padre == ".")
            {

                var NumHijosPadre = Repository
                    .GetAll()
                    .Where(o => o.vigente == true)
                    .Where(o => o.OfertaId == OfertaId)
                    .Where(o => o.id_nivel_padre_codigo == padre).ToList();

                var contadorMaximo = 0;


                //Contador Maximo
                var queryContador = (from e in NumHijosPadre
                                     orderby Convert.ToInt32(e.id_nivel_codigo.Replace(".", ""))
                                     select Convert.ToInt32(e.id_nivel_codigo.Replace(".", ""))).ToList();
                if (queryContador.Count > 0)
                {
                    contadorMaximo = queryContador.Max();
                }
                count = contadorMaximo + 1;

            }
            else
            {

                count = Repository
                    .GetAll()
                    .Where(o => o.vigente == true)
                    .Where(o => o.OfertaId == OfertaId)
                    .Count(o => o.id_nivel_padre_codigo == padre);
            }
            count = count + 1;
            if (!es_actividad)
            {
                return (padre == "." ? count + "." : padre + (indice.HasValue ? indice.Value : count) + ".");
            }
            else

            {
                return padre + (indice.HasValue ? indice.Value : count);
            }
        }

        public bool ExisteWbs(WbsDto wbs)
        {
            var count = Repository
                .GetAll()
                .Where(o => o.vigente == true)
                .Where(o => o.nivel_nombre == wbs.nivel_nombre).Count(o => o.id_nivel_padre_codigo == wbs.id_nivel_codigo);

            return (count > 0 ? true : false);
        }

        public bool ClonarWbs(int OfertaDestinoId, int OfertaOrigen)
        {
            var count = Repository
                .GetAll()
                .Where(o => o.vigente).Count(o => o.OfertaId == OfertaDestinoId);
            if (count > 0)
            {
                return false;
            }
            else
            {
                var listado = Repository.GetAll()
                    .Where(o => o.vigente)
                    .Where(o => o.OfertaId == OfertaOrigen).ToList();

                foreach (var wbs in listado)
                {
                    var w = new WbsDto()
                    {
                        OfertaId = OfertaDestinoId,
                        es_actividad = wbs.es_actividad,
                        estado = wbs.estado,
                        fecha_final = wbs.fecha_final,
                        fecha_inicial = wbs.fecha_inicial,
                        id_nivel_codigo = wbs.id_nivel_codigo,
                        id_nivel_padre_codigo = wbs.id_nivel_padre_codigo,
                        nivel_nombre = wbs.nivel_nombre,
                        observaciones = wbs.observaciones,
                        vigente = true
                    };

                    Repository.Insert(MapToEntity(w));
                }

                return true;
            }
        }



        public List<JerarquiaWbs> GenerarDiagrama(int ofertaId)
        {
            var i = Repository.GetAll().Where(c => c.id_nivel_padre_codigo == ".")
                .Where(o => o.OfertaId == ofertaId);


            var Lista = new List<JerarquiaWbs>();


            foreach (var x in i.ToList())
            {
                var item = GenerarNodosDiagrama(x);
                Lista.Add(item);
            }

            return Lista;
        }

        public JerarquiaWbs GenerarNodosDiagrama(Wbs wbs)
        {
            List<Wbs> hijos = GetWbsHijos(wbs.id_nivel_codigo, wbs.OfertaId);
            if (hijos.Count > 0)
            {
                var lista_hijos = new List<JerarquiaWbs>();
                foreach (var h in hijos)
                {
                    var lhijos = GenerarNodosDiagrama(h);
                    lista_hijos.Add(lhijos);
                }
                return new JerarquiaWbs()
                {
                    label = "Nivel",
                    className = "ui-area",
                    expanded = false,
                    type = "area",
                    data = wbs.nivel_nombre,
                    children = lista_hijos
                };
            }
            else
            {
                if (wbs.es_actividad)
                {
                    return new JerarquiaWbs()
                    {
                        label = "Actividad",
                        className = "ui-actividad",
                        expanded = false,
                        type = "actividad",
                        data = wbs.nivel_nombre,

                    };
                }
                else
                {
                    return new JerarquiaWbs()
                    {
                        label = "Nivel",
                        className = "ui-area",
                        expanded = false,
                        type = "area",
                        data = wbs.nivel_nombre,
                        children = new List<JerarquiaWbs>(),

                    };
                }

            }
        }



        public List<TreeWbsComputo> GenerarArbolComputo(int ofertaId)
        {

            var i = Repository.GetAll().Where(c => c.id_nivel_padre_codigo == ".")
                 .Where(o => o.OfertaId == ofertaId).Where(c => c.vigente == true);


            var Lista = new List<TreeWbsComputo>();


            foreach (var x in i.ToList())
            {
                var item = GenerarNodosComputo(x);
                Lista.Add(item);
            }

            return Lista;

        }

        public List<TreeWbs> GenerarArbolComputosTemporal(int ofertaId)
        {

            var i = Repository.GetAll().Where(c => c.id_nivel_padre_codigo == ".")
                 .Where(o => o.OfertaId == ofertaId);

            var Lista = new List<TreeWbs>();

            foreach (var x in i.ToList())
            {
                var item = GenerarNodosComputosTemporal(x);
                Lista.Add(item);
            }

            return Lista;

        }


        public TreeWbsComputo GenerarNodosComputo(Wbs wbs)
        {
            List<Wbs> hijos = GetWbsHijos(wbs.id_nivel_codigo, wbs.OfertaId);
            if (hijos.Count > 0)
            {
                var lista_hijos = new List<TreeWbsComputo>();
                foreach (var h in hijos)
                {
                    var lhijos = GenerarNodosComputo(h);
                    lista_hijos.Add(lhijos);
                }
                return new TreeWbsComputo()
                {
                    key = wbs.Id,
                    label = wbs.nivel_nombre,
                    data = wbs.id_nivel_codigo + "," + wbs.id_nivel_padre_codigo + "," + wbs.Id + "," + wbs.OfertaId,
                    expandedIcon = "fa fa-fw fa-folder-open",
                    collapsedIcon = "fa fa-fw fa-folder",
                    tipo = "padre",
                    expanded = true,
                    children = lista_hijos,
                    selectable = true
                };
            }
            else
            {
                if (wbs.es_actividad)
                {
                    return new TreeWbsComputo()
                    {
                        key = wbs.Id,
                        label = wbs.nivel_nombre,
                        data = wbs.id_nivel_codigo + "," + wbs.id_nivel_padre_codigo + "," + wbs.Id + "," + wbs.OfertaId,
                        expandedIcon = "fa fa-file-text",
                        collapsedIcon = "fa fa-file-text",
                        tipo = "actividad",
                        expanded = true,
                        children = _computoService.TreeComputo(wbs.Id),
                        selectable = true//Aumente computo
                    };
                }
                else
                {
                    return new TreeWbsComputo()
                    {
                        key = wbs.Id,
                        label = wbs.nivel_nombre,
                        data = wbs.id_nivel_codigo + "," + wbs.id_nivel_padre_codigo + "," + wbs.Id + "," + wbs.OfertaId,
                        expandedIcon = "fa fa-fw fa-folder-open",
                        collapsedIcon = "fa fa-fw fa-folder",
                        tipo = "padre",
                        expanded = true,
                        children = new List<TreeWbsComputo>(),

                    };
                }

            }
        }

        public TreeWbs GenerarNodosComputosTemporal(Wbs wbs)
        {
            List<Wbs> hijos = GetWbsHijos(wbs.id_nivel_codigo, wbs.OfertaId);
            if (hijos.Count > 0)
            {
                var lista_hijos = new List<TreeWbs>();
                foreach (var h in hijos)
                {
                    var lhijos = GenerarNodosComputosTemporal(h);
                    lista_hijos.Add(lhijos);
                }
                return new TreeWbs()
                {
                    label = wbs.nivel_nombre,
                    data = wbs.id_nivel_codigo + "," + wbs.id_nivel_padre_codigo + "," + wbs.Id + "," + wbs.OfertaId,
                    expandedIcon = "fa fa-fw fa-folder-open",
                    collapsedIcon = "fa fa-fw fa-folder",
                    tipo = "padre",

                    children = lista_hijos
                };
            }
            else
            {
                if (wbs.es_actividad)
                {
                    return new TreeWbs()
                    {
                        label = wbs.nivel_nombre,
                        data = wbs.id_nivel_codigo + "," + wbs.id_nivel_padre_codigo + "," + wbs.Id + "," + wbs.OfertaId
                        + "," + wbs.observaciones + "," + wbs.revision + "," + wbs.fecha_inicial.GetValueOrDefault().ToShortDateString() + ","
                        + wbs.fecha_final.GetValueOrDefault().ToShortDateString() + "," + wbs.es_actividad + "," + wbs.estado
                        ,

                        expandedIcon = "fa fa-fw fa-folder-open",
                        collapsedIcon = "fa fa-fw fa-folder",
                        tipo = "actividad",

                        children = _computosTemporalService.TreeComputo(wbs.Id) //Aumente computo
                    };
                }
                else
                {
                    return new TreeWbs()
                    {
                        label = wbs.nivel_nombre,
                        data = wbs.id_nivel_codigo + "," + wbs.id_nivel_padre_codigo + "," + wbs.Id + "," + wbs.OfertaId,
                        expandedIcon = "fa fa-fw fa-folder-open",
                        collapsedIcon = "fa fa-fw fa-folder",
                        tipo = "padre",

                        children = new List<TreeWbs>(),

                    };
                }

            }
        }

        public List<WbsDto> GetWbsOfertas(int OfertaId)
        {
            var wbquery = Repository.GetAllIncluding(c => c.Oferta, c => c.Oferta.Requerimiento,
                c => c.Oferta.Requerimiento.Proyecto, c => c.Oferta.Requerimiento.Proyecto.Contrato,
                c => c.Oferta.Requerimiento.Proyecto.Contrato.Cliente);

            // wbquery = Repository.GetAllIncluding(c => c.Oferta, c => c.Oferta.Requerimiento, c => c.Oferta.Requerimiento.Proyecto, c => c.Oferta.Requerimiento.Proyecto.Contrato, c => c.Oferta.Requerimiento.Proyecto.Contrato.Cliente);
            var wbs = (from c in wbquery
                       where c.OfertaId == OfertaId && c.vigente == true
                       where c.es_actividad == false
                       select new WbsDto
                       {
                           Id = c.Id,
                           observaciones = c.observaciones,
                           Oferta = c.Oferta,
                           vigente = c.vigente,
                           es_actividad = c.es_actividad,
                           estado = c.estado,
                           OfertaId = c.OfertaId,
                           fecha_final = c.fecha_final,
                           fecha_inicial = c.fecha_inicial,
                           id_nivel_codigo = c.id_nivel_codigo,
                           id_nivel_padre_codigo = c.id_nivel_padre_codigo,
                           nivel_nombre = c.nivel_nombre,
                           DisciplinaId = c.DisciplinaId,
                           revision = c.revision
                       }).ToList();

            return wbs;
        }

        public WbsDto ObtenerPadre(string id_nivel_padre_codigo, int OfertaId)
        {
            var items = Repository.GetAll();

            var ItemPadre = (from c in items
                             where c.vigente == true
                             where c.OfertaId == OfertaId
                             where c.id_nivel_codigo == id_nivel_padre_codigo
                             select new WbsDto
                             {
                                 Id = c.Id,
                                 fecha_inicial = c.fecha_inicial,
                                 id_nivel_codigo = c.id_nivel_codigo,
                                 id_nivel_padre_codigo = c.id_nivel_padre_codigo,
                                 nivel_nombre = c.nivel_nombre,
                                 es_actividad = c.es_actividad,
                                 OfertaId = c.OfertaId
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

        public List<WbsDto> Jerarquiawbs(int id)
        {
            List<WbsDto> Jerarquia = new List<WbsDto>();

            WbsDto item = this.DatosWbs(id);
            Jerarquia.Add(item);
            while (item.id_nivel_padre_codigo != ".")
            {

                item = this.ObtenerPadre(item.id_nivel_padre_codigo, item.OfertaId);
                Jerarquia.Add(item);
            }

            return Jerarquia.OrderBy(c => c.id_nivel_codigo).ToList();
        }

        public WbsDto DatosWbs(int id)
        {
            var items = Repository.GetAll();

            var ItemPadre = (from c in items
                             where c.vigente == true
                             where c.Id == id
                             select new WbsDto
                             {
                                 Id = c.Id,
                                 fecha_inicial = c.fecha_inicial,
                                 id_nivel_codigo = c.id_nivel_codigo,
                                 id_nivel_padre_codigo = c.id_nivel_padre_codigo,
                                 nivel_nombre = c.nivel_nombre,
                                 es_actividad = c.es_actividad,
                                 fecha_final = c.fecha_final != null ? c.fecha_final : null,
                                 OfertaId = c.OfertaId
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

        public ExcelPackage GenerarExcelCargaFechas(OfertaDto oferta)
        {
            // var wbs = this.ArbolWbsExcel(oferta.Id);
            var wbs = this.EstructuraWbs(oferta.Id);

            ExcelPackage excel = new ExcelPackage();
            var workSheet = excel.Workbook.Worksheets.Add("Croonograma");
            workSheet.Cells[1, 1].Value = "Id";

            workSheet.Cells[1, 2].Value = "CÓDIGO";
            workSheet.Cells[1, 2].Style.WrapText = true;
            workSheet.Cells[1, 2].Style.Font.Bold = true;
            workSheet.Cells[1, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            workSheet.Cells[1, 2].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            workSheet.Cells[1, 2].Style.Fill.PatternType = ExcelFillStyle.Solid;
            workSheet.Cells[1, 2].Style.Fill.BackgroundColor.SetColor(Color.DodgerBlue);
            workSheet.Cells[1, 2].Style.Font.Color.SetColor(Color.White);


            workSheet.Cells[1, 3].Value = "DESCRIPCIÓN";
            workSheet.Cells[1, 3].Style.WrapText = true;
            workSheet.Cells[1, 3].Style.Font.Bold = true;
            workSheet.Cells[1, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            workSheet.Cells[1, 3].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            workSheet.Cells[1, 3].Style.Fill.PatternType = ExcelFillStyle.Solid;
            workSheet.Cells[1, 3].Style.Fill.BackgroundColor.SetColor(Color.DodgerBlue);
            workSheet.Cells[1, 3].Style.Font.Color.SetColor(Color.White);

            workSheet.Cells[1, 4].Value = "FECHA INICIAL";

            workSheet.Cells[1, 4].Style.WrapText = true;
            workSheet.Cells[1, 4].Style.Font.Bold = true;
            workSheet.Cells[1, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            workSheet.Cells[1, 4].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            workSheet.Cells[1, 4].Style.Fill.PatternType = ExcelFillStyle.Solid;
            workSheet.Cells[1, 4].Style.Fill.BackgroundColor.SetColor(Color.DodgerBlue);
            workSheet.Cells[1, 4].Style.Font.Color.SetColor(Color.White);

            workSheet.Cells[1, 5].Value = "FECHA FINAL";
            workSheet.Cells[1, 5].Style.WrapText = true;
            workSheet.Cells[1, 5].Style.Font.Bold = true;
            workSheet.Cells[1, 5].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            workSheet.Cells[1, 5].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            workSheet.Cells[1, 5].Style.Fill.PatternType = ExcelFillStyle.Solid;
            workSheet.Cells[1, 5].Style.Fill.BackgroundColor.SetColor(Color.DodgerBlue);
            workSheet.Cells[1, 5].Style.Font.Color.SetColor(Color.White);


            //workSheet.Cells["B4:D4"].AutoFilter = true;

            workSheet.Column(1).Width = 20;
            workSheet.Column(2).Width = 20;
            workSheet.Column(3).Width = 50;
            workSheet.Column(4).Width = 20;
            workSheet.Column(5).Width = 20;
            workSheet.Column(1).Style.Font.Bold = true;
            workSheet.Column(1).Hidden = true;






            int row = 2;

            //foreach (var itemswbs in wbs.OrderBy(c => c.id_nivel_codigo))
            foreach (var itemswbs in wbs)
            {

                workSheet.Cells[row, 1].Value = itemswbs.Id;
                workSheet.Cells[row, 2].Value = itemswbs.id_nivel_codigo;
                workSheet.Cells[row, 3].Value = itemswbs.nivel_nombre;
                workSheet.Cells[row, 4].Style.Numberformat.Format = "DD/MM/YYYY";
                workSheet.Cells[row, 5].Style.Numberformat.Format = "DD/MM/YYYY";

                //COLORES
                if (itemswbs.es_actividad)
                {

                    workSheet.Cells[row, 2].Style.Font.Bold = true;
                    workSheet.Cells[row, 2].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    workSheet.Cells[row, 2].Style.Fill.BackgroundColor.SetColor(Color.WhiteSmoke);
                    workSheet.Cells[row, 2].Style.Border.Top.Style =
                    workSheet.Cells[row, 2].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                    workSheet.Cells[row, 3].Style.Font.Bold = true;
                    workSheet.Cells[row, 3].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    workSheet.Cells[row, 3].Style.Fill.BackgroundColor.SetColor(Color.WhiteSmoke);
                    workSheet.Cells[row, 3].Style.Border.Top.Style =
                    workSheet.Cells[row, 3].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                    workSheet.Cells[row, 4].Style.Font.Bold = true;
                    workSheet.Cells[row, 4].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    workSheet.Cells[row, 4].Style.Fill.BackgroundColor.SetColor(Color.WhiteSmoke);
                    workSheet.Cells[row, 4].Style.Border.Top.Style =
                         workSheet.Cells[row, 4].Style.Border.Left.Style =
                              workSheet.Cells[row, 4].Style.Border.Right.Style =
                    workSheet.Cells[row, 4].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                    workSheet.Cells[row, 5].Style.Font.Bold = true;
                    workSheet.Cells[row, 5].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    workSheet.Cells[row, 5].Style.Fill.BackgroundColor.SetColor(Color.WhiteSmoke);
                    workSheet.Cells[row, 5].Style.Border.Top.Style =
                           workSheet.Cells[row, 5].Style.Border.Left.Style =
                              workSheet.Cells[row, 5].Style.Border.Right.Style =
                    workSheet.Cells[row, 5].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;



                    //DATOS

                    if (itemswbs.fecha_inicial != null)
                    {


                        workSheet.Cells[row, 4].Value = itemswbs.fecha_inicial;
                        workSheet.Cells[row, 4].Style.Numberformat.Format = "DD/MM/YYYY";
                    }
                    if (itemswbs.fecha_final != null)
                    {
                        workSheet.Cells[row, 5].Value = itemswbs.fecha_final.Value;
                        workSheet.Cells[row, 5].Style.Numberformat.Format = "DD/MM/YYYY";
                    }


                }
                row++;



            }

            return excel;
        }

        public List<WbsDto> ArbolWbsExcel(int OfertaId)
        {
            List<WbsDto> all = new List<WbsDto>();
            var wbs = this.Listar(OfertaId);
            foreach (var item in wbs)
            {

                var x = this.Jerarquiawbs(item.Id);
                foreach (var item2 in x)
                {
                    all.Add(item2);
                }
                all.Add(item);

            }
            return all.GroupBy(p => p.Id)
                .Select(g => g.FirstOrDefault())
                .OrderBy(c => c.id_nivel_codigo
                ).ToList();
        }

        public List<WbsDto> ObtenerKeysArbol(int OfertaId)
        {
            var consulta = Repository.GetAll().Where(c => c.OfertaId == OfertaId).Where(c => c.vigente == true)
               .Where(c => c.es_actividad == false);

            if (consulta.ToList().Count > 0)
            {
                var llaves = (from c in consulta
                              select new WbsDto
                              {
                                  Id = c.Id
                              }).ToList();

                return llaves;

            }
            return new List<WbsDto>();
        }

        public List<TreeWbs> GenerarArbolDrag(int ofertaId)
        {
            var i = Repository.GetAll().Where(c => c.id_nivel_padre_codigo == ".")
                 .Where(o => o.OfertaId == ofertaId).Where(o => o.vigente == true);


            var Lista = new List<TreeWbs>();

            //  int indice = 1;
            foreach (var x in i.ToList().OrderBy(c => c.nivel_nombre))
            {

                // string orden =""+indice;

                var item = GenerarNodosDrag(x);//,indice,orden);
                                               //  var item = GenerarNodosDrag(x, indice, orden);
                Lista.Add(item);
                // indice++;
            }

            return Lista;
        }

        public TreeWbs GenerarNodosDrag(Wbs wbs) //int? indice, string orden = "")
        {
            // int indicehijo = 1;
            //string ordenhijo = orden + "." + indicehijo;

            List<Wbs> hijos = GetWbsHijosDrag(wbs.id_nivel_codigo, wbs.OfertaId);
            if (hijos.Count > 0)
            {
                var lista_hijos = new List<TreeWbs>();
                foreach (var h in hijos)
                {

                    //  ordenhijo = orden + "." + indicehijo;
                    var lhijos = GenerarNodosDrag(h);//,indicehijo,ordenhijo);
                    lista_hijos.Add(lhijos);
                    // indicehijo++;
                }

                return new TreeWbs()
                {
                    key = wbs.Id,
                    label = wbs.nivel_nombre, //orden + "  " + wbs.nivel_nombre,
                    data = wbs.id_nivel_codigo + "," + wbs.id_nivel_padre_codigo + "," + wbs.Id + "," + wbs.OfertaId
                    + "," + wbs.observaciones + "," + wbs.revision + "," + wbs.fecha_inicial.GetValueOrDefault().ToShortDateString() + ","
                        + wbs.fecha_final.GetValueOrDefault().ToShortDateString() + "," + wbs.es_actividad + "," + wbs.estado + "," + wbs.DisciplinaId,
                    expandedIcon = "fa fa-fw fa-folder-open",
                    collapsedIcon = "fa fa-fw fa-folder",
                    tipo = "padre",
                    //expanded = true,
                    children = lista_hijos,
                    selectable = true,
                    draggable = true,
                    droppable = true,

                    //orden = ordenhijo,
                    //indice = indicehijo,
                    nivel_nombre = wbs.nivel_nombre

                };
            }
            else
            {
                if (wbs.es_actividad)
                {

                    return new TreeWbs()
                    {
                        key = wbs.Id,
                        label = wbs.nivel_nombre,//,orden+"  "+ 
                        data = wbs.id_nivel_codigo + "," + wbs.id_nivel_padre_codigo + "," + wbs.Id + "," + wbs.OfertaId
                        + "," + wbs.observaciones + "," + wbs.revision + "," + wbs.fecha_inicial.GetValueOrDefault().ToShortDateString() + ","
                        + wbs.fecha_final.GetValueOrDefault().ToShortDateString() + "," + wbs.es_actividad + "," + wbs.estado + "," + wbs.DisciplinaId,
                        icon = "fa fa-fw fa-file-word-o",
                        tipo = "actividad",
                        // expanded = true,
                        selectable = true,
                        draggable = true,
                        droppable = false,

                        //orden = ordenhijo,
                        // indice=indicehijo,
                        nivel_nombre = wbs.nivel_nombre


                    };
                }
                else
                {

                    return new TreeWbs()
                    {
                        key = wbs.Id,
                        label = wbs.nivel_nombre, //orden + "  " + wbs.nivel_nombre,
                        data = wbs.id_nivel_codigo + "," + wbs.id_nivel_padre_codigo + "," + wbs.Id + "," + wbs.OfertaId
                         + "," + wbs.observaciones + "," + wbs.revision + "," + wbs.fecha_inicial.GetValueOrDefault().ToShortDateString() + ","
                        + wbs.fecha_final.GetValueOrDefault().ToShortDateString() + "," + wbs.es_actividad + "," + wbs.estado + "," + wbs.DisciplinaId,
                        expandedIcon = "fa fa-fw fa-folder-open",
                        collapsedIcon = "fa fa-fw fa-folder",
                        tipo = "padre",
                        children = new List<TreeWbs>(),
                        selectable = true,
                        draggable = true,
                        droppable = true,
                        // orden = ordenhijo,
                        // indice = indicehijo,

                        nivel_nombre = wbs.nivel_nombre
                    };
                }

            }

        }

        public List<Wbs> GetWbsHijosDrag(string codigo_padre, int ofertaId)
        {
            var items = Repository.GetAll()
                .Where(o => o.vigente == true)
                .Where(o => o.OfertaId == ofertaId)
                .Where(o => o.id_nivel_padre_codigo == codigo_padre)
                .OrderBy(c => c.nivel_nombre) //Nivel Nombre
                .ToList();
            return items;
        }

        public string GuardarArbolDrag(List<TreeWbs> data)
        {
            if (data != null && data.Count > 0)
            {
                int z = 1;
                foreach (var item in data)
                {
                    var padre = Repository.Get(item.key);
                    padre.id_nivel_padre_codigo = ".";
                    padre.id_nivel_codigo = z + ".";
                    var resultado = Repository.Update(padre);
                    if (item.children != null && item.children.Count > 0)
                    {
                        var x = this.GuardarHijoDrag(item.children, padre.id_nivel_codigo);
                    }
                    z++;
                }
            }
            return "OK";
        }

        public string GuardarHijoDrag(List<TreeWbs> data, string padre)
        {
            if (data != null && data.Count > 0)
            {

                var cont = 1;
                foreach (var item in data)
                {
                    var filawbs = Repository.Get(Int32.Parse("" + item.key));
                    filawbs.id_nivel_padre_codigo = padre;
                    filawbs.id_nivel_codigo = padre + cont + ".";
                    var resultado = Repository.Update(filawbs);


                    if (item.children != null && item.children.Count > 0)
                    {
                        var x = GuardarHijoDrag(item.children, filawbs.id_nivel_codigo);
                    }
                    cont++;
                }

            }

            return "OK";
        }

        public bool CopiarWBS(int OfertaId, int PresupuestoId, int WbsId, int WbsPresupuestoId)
        {
            var wbsPresupuesto = _wbsPresupuestoRepository.Get(WbsPresupuestoId);

            // Destino es Raiz
            if (WbsId < 0)
            {
                if (wbsPresupuesto.es_actividad)
                {
                    // Esta moviendo una actividad a la raiz
                    return false;
                }
                else
                {
                    // Esta moviendo un nivel a la raiz
                    var wbsOfertaTem = new Wbs()
                    {
                        id_nivel_codigo = ".",
                        OfertaId = OfertaId,
                        es_actividad = false,
                        estado = true,
                        id_nivel_padre_codigo = ".",
                        nivel_nombre = "Raiz",
                        revision = "A",
                        vigente = true,
                    };
                    var nodo = _wbsPresupuestoService.CopiarNodosAWbs(WbsPresupuestoId, wbsOfertaTem);
                    this.GuardarNodoCopia(nodo, wbsOfertaTem);
                    return true;
                }
            }
            else // Destino es un nivel
            {
                var wbsOferta = Repository.Get(WbsId);
                //Presupuesto es Actividad
                if (wbsPresupuesto.es_actividad)
                {
                    // Guardar en destino
                    var wbs = Repository.Get(WbsId);

                    var codigo = this.GenerarCodigo(wbs.id_nivel_codigo, true, wbs.OfertaId, null);
                    var newWbs = new Wbs()
                    {
                        DisciplinaId = wbsPresupuesto.DisciplinaId,
                        OfertaId = wbs.OfertaId,
                        es_actividad = true,
                        estado = wbsPresupuesto.estado,
                        fecha_final = wbsPresupuesto.fecha_final,
                        fecha_inicial = wbsPresupuesto.fecha_inicial,
                        id_nivel_codigo = codigo,
                        id_nivel_padre_codigo = wbs.id_nivel_codigo,
                        nivel_nombre = wbsPresupuesto.nivel_nombre,
                        observaciones = wbsPresupuesto.observaciones,
                        vigente = true,
                        Catalogo = wbsPresupuesto.Catalogo
                    };

                    var id = Repository.InsertAndGetId(newWbs);
                    this.CopiarComputos(WbsPresupuestoId, id);
                    return true;
                }
                else
                {
                    // Presupuesto es Nivel
                    var nodo = _wbsPresupuestoService.CopiarNodosAWbs(WbsPresupuestoId, wbsOferta);
                    this.GuardarNodoCopia(nodo, wbsOferta);
                    return true;
                }
            }
        }


        public void CopiarComputos(int WbsPresupuestoId, int WbsId)
        {
            var computosPresupuesto = _computoPresupuesto.GetAll()
                .Where(o => o.vigente)
                .Where(o => o.WbsPresupuestoId == WbsPresupuestoId).ToList();

            foreach (var c in computosPresupuesto)
            {
                var computo = new Computo()
                {
                    vigente = true,
                    ItemId = c.ItemId,
                    WbsId = WbsId,
                    cantidad = c.cantidad,
                    cantidad_eac = c.cantidad_eac,
                    codigo_primavera = c.codigo_primavera,
                    costo_total = c.costo_total,
                    estado = c.estado,
                    fecha_actualizacion = c.fecha_actualizacion,
                    codigo_item_alterno = c.codigo_item_alterno,
                    fecha_eac = c.fecha_eac,
                    fecha_registro = c.fecha_registro,
                    precio_ajustado = c.precio_ajustado,
                    precio_aplicarse = c.precio_aplicarse,
                    precio_base = c.precio_base,
                    precio_incrementado = c.precio_incrementado,
                    precio_unitario = c.precio_unitario,
                    presupuestado = c.presupuestado,
                };
                _computoRepository.Insert(computo);
            }
        }

        public string GuardarNodoCopia(TreeWbs data, Wbs WbsPadre)
        {
            int count = 1;
            var padre = Repository.Get(data.key);
            padre.id_nivel_padre_codigo = WbsPadre.id_nivel_codigo;
            var actividad = padre.es_actividad ? true : false;
            padre.id_nivel_codigo = this.GenerarCodigo(WbsPadre.id_nivel_codigo, actividad, WbsPadre.OfertaId, null);
            Repository.Update(padre);
            if (data.children != null && data.children.Count > 0)
            {
                var x = this.GuardarHijosCopia(data.children, padre.id_nivel_codigo);
            }
            return "OK";
        }

        public string GuardarHijosCopia(List<TreeWbs> data, string padre)
        {
            if (data != null && data.Count > 0)
            {
                int indice = 1;
                foreach (var item in data)
                {
                    var filawbs = Repository.Get(Int32.Parse("" + item.key));
                    filawbs.id_nivel_padre_codigo = padre;
                    var actividad = filawbs.es_actividad ? true : false;
                    filawbs.id_nivel_codigo = this.GenerarCodigo(padre, actividad, filawbs.OfertaId, indice);
                    Repository.Update(filawbs);
                    if (actividad)
                    {
                        var itemData = item.data.Split(',');
                        this.CopiarComputos(Int32.Parse(itemData[2]), filawbs.Id);
                    }

                    if (item.children != null && item.children.Count > 0)
                    {
                        var x = GuardarHijosCopia(item.children, filawbs.id_nivel_codigo);
                    }
                    indice++;
                }
            }
            return "OK";
        }


        public int nivel_mas_alto(int Id)
        {
            var lista = Repository.GetAllIncluding(x => x.Catalogo)
                .Where(o => o.vigente == true)
                .Where(o => o.OfertaId == Id)
                .Where(o => o.es_actividad == true)
                .ToList();
            int mayor = 0;


            foreach (var item in lista)
            {
                int contnivel = this.contarnivel(item.Id, item.OfertaId);
                if (contnivel >= mayor)
                {
                    mayor = contnivel;
                }


            }
            return mayor;
        }

        public int contarnivel(int id, int OfertaId)
        {
            List<Wbs> Jerarquia = new List<Wbs>();

            Wbs item = Repository.Get(id);
            Jerarquia.Add(item);
            while (item.id_nivel_padre_codigo != ".")
            {

                item = Repository.GetAll()
                    .Where(c => c.id_nivel_codigo == item.id_nivel_padre_codigo)
                    .Where(C => C.vigente)
                    .Where(c => c.OfertaId == OfertaId).FirstOrDefault();

                Jerarquia.Add(item);
            }
            return Jerarquia.Count();
        }

        public List<Wbs> EstructuraWbs(int OfertaId)
        {
            List<Wbs> all = new List<Wbs>();
            var wbs = Repository.GetAll().Where(c => c.vigente)
                                         .Where(c => c.id_nivel_padre_codigo == ".")
                                          .Where(c => c.OfertaId == OfertaId).ToList();
            if (wbs.Count > 0)
            {
                foreach (var item in wbs)
                {
                    all.Add(item);
                    var lista = this.ObtenerWbsHijos(OfertaId, item.id_nivel_codigo, all);
                }
            }
            return all;

        }

        public List<Wbs> ObtenerWbsHijos(int OfertaId, string id_nivel_codigo, List<Wbs> estructura)
        {
            var wbs = Repository.GetAll().Where(c => c.vigente)
                                       .Where(c => c.id_nivel_padre_codigo == id_nivel_codigo)
                                       .Where(c => c.OfertaId == OfertaId).ToList();
            if (wbs.Count > 0)
            {

                foreach (var item in wbs)
                {
                    estructura.Add(item);
                    var hijos = this.ObtenerWbsHijos(OfertaId, item.id_nivel_codigo, estructura);
                }
            }

            return estructura;
        }


        public string VerficarExcelFechas(HttpPostedFileBase UploadedFile)
        {
            string resultado = "OK/";
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

                        for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                        {

                            var disciplinaid = (workSheet.Cells[rowIterator, 1].Value ?? "").ToString();
                            var fechainicial = (workSheet.Cells[rowIterator, 4].Text ?? "01/01/1999").ToString();
                            var fechafinal = (workSheet.Cells[rowIterator, 5].Text ?? "01/01/1999").ToString();


                            if (disciplinaid.Length > 0 && fechainicial.Length > 0 && fechafinal.Length > 0 && fechafinal != "01/01/1999" && fechainicial != "01/01/1999")
                            {

                                var wbs = Repository.Get(Int32.Parse(disciplinaid));


                                if (wbs != null && wbs.es_actividad == true)
                                {

                                    DateTime tempfechainicial = DateTime.Parse(fechainicial);
                                    DateTime tempfechafinal = DateTime.Parse(fechafinal);

                                    if (tempfechafinal < tempfechainicial)
                                    {

                                        resultado = resultado + " , " + rowIterator + " ";
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    resultado = "";
                    resultado = "Subir Archivo Excel";
                }
            }
            else
            {
                resultado = "";
                resultado = "Sin Archivo";
            }
            return resultado;
        }
    }
}



