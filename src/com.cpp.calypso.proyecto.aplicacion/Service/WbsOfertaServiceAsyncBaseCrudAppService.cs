using Abp.Application.Services.Dto;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.aplicacion.Interfaces;
using com.cpp.calypso.proyecto.dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using System.Threading.Tasks;
using Abp.Collections.Extensions;

namespace com.cpp.calypso.proyecto.aplicacion
{
    public class WbsOfertaServiceAsyncBaseCrudAppService :
        AsyncBaseCrudAppService<WbsOferta, WbsOfertaDto, PagedAndFilteredResultRequestDto>,
        IWbsOfertaAsyncBaseCrudAppService
    {
        private readonly IComputoAsyncBaseCrudAppService _computoService;
        private readonly IBaseRepository<Computo> repositorycomputo;
        private readonly IBaseRepository<Catalogo> repositorycatalogo;
        private readonly IBaseRepository<DetallePreciario> repositorydetallepreciario;
        private readonly IBaseRepository<DetalleAvanceObra> _detalleAvanceObraRepository;
        public readonly IBaseRepository<Item> itemrepository;
        private readonly IBaseRepository<Catalogo> _catalogoRepository;
        private readonly IBaseRepository<Oferta> _ofertaRepository;
        private readonly IBaseRepository<Wbs> _wbsRepository;
        private readonly  IBaseRepository<Preciario> repositorypreciario;
        private readonly IBaseRepository<Ganancia> _gananciarepository;
        public readonly IBaseRepository<DetalleGanancia> _detallegananciarepository;
        public WbsOfertaServiceAsyncBaseCrudAppService(IBaseRepository<WbsOferta> repository,

            IBaseRepository<Computo> repositorycomputo,
            IBaseRepository<Catalogo> repositorycatalogo,
            IBaseRepository<Item> itemrepository,
            IBaseRepository<Catalogo> catalogoRepository,
            IBaseRepository<Oferta> ofertaRepository,
            IBaseRepository<Preciario> repositorypreciario,
        IBaseRepository<DetallePreciario> repositorydetallepreciario,
        IBaseRepository<DetalleAvanceObra> detalleAvanceObraRepository,
        IBaseRepository<Wbs> wbsRepository,
                 IBaseRepository<Ganancia> gananciarepository,
            IBaseRepository<DetalleGanancia> detallegananciarepository
            ) : base(repository)
        {
            this.repositorypreciario = repositorypreciario;
            this.repositorycomputo = repositorycomputo;
            this.itemrepository = itemrepository;
            _catalogoRepository = catalogoRepository;
            _ofertaRepository = ofertaRepository;
            this.repositorydetallepreciario = repositorydetallepreciario;
            _detalleAvanceObraRepository = detalleAvanceObraRepository;
            this.repositorycatalogo = repositorycatalogo;
            _wbsRepository= wbsRepository;
            _gananciarepository = gananciarepository;
            _detallegananciarepository = detallegananciarepository;
           
        }

        public WbsOferta GetWbsOfertaporOferta(int OfertaId)
        {
            var wbsofertasQuery = Repository.GetAllIncluding(c => c.Oferta, c => c.Oferta.Requerimiento,
                c => c.Oferta.Requerimiento.Proyecto, c => c.Oferta.Requerimiento.Proyecto.Contrato,
                c => c.Oferta.Requerimiento.Proyecto.Contrato.Cliente);
            var item = (from r in wbsofertasQuery
                where r.OfertaId == OfertaId
                select new WbsOferta()
                {
                    Id = r.Id,
                    OfertaId = r.OfertaId,
                    AreaId = r.AreaId,
                    DisciplinaId = r.DisciplinaId,
                    ElementoId = r.ElementoId,
                    ActividadId = r.ActividadId,
                    estado = r.estado,
                    observaciones = r.observaciones,
                    vigente = r.vigente,


                }).SingleOrDefault();
            return item;
        }


        public WbsOferta Get(int wbsId)
        {
            var wbsofertasQuery = Repository.GetAll();
            var item = (from r in wbsofertasQuery
                where r.Id == wbsId
                select new WbsOferta()
                {
                    Id = r.Id,
                    OfertaId = r.OfertaId,
                    AreaId = r.AreaId,
                    DisciplinaId = r.DisciplinaId,
                    ElementoId = r.ElementoId,
                    ActividadId = r.ActividadId,
                    estado = r.estado,
                    observaciones = r.observaciones,
                    fecha_inicio = r.fecha_inicio,
                    fecha_fin = r.fecha_fin,
                    es_estructura = r.es_estructura,
                    vigente = r.vigente

                }).SingleOrDefault();
            return item;
        }



        public List<WbsOferta> GetWbsOfertas()
        {
            var wbsofertaQuery = Repository.GetAllIncluding(c => c.Oferta, c => c.Oferta.Requerimiento,
                    c => c.Oferta.Requerimiento.Proyecto, c => c.Oferta.Requerimiento.Proyecto.Contrato,
                    c => c.Oferta.Requerimiento.Proyecto.Contrato.Cliente)
                .Where(e => e.vigente == true).ToList();
            return wbsofertaQuery;

        }

        public List<WbsOfertaDto> GetWbsOfertas(int OfertaId)
        {
            var wbquery = Repository.GetAllIncluding(c => c.Oferta, c => c.Oferta.Requerimiento,
                c => c.Oferta.Requerimiento.Proyecto, c => c.Oferta.Requerimiento.Proyecto.Contrato,
                c => c.Oferta.Requerimiento.Proyecto.Contrato.Cliente);

            // wbquery = Repository.GetAllIncluding(c => c.Oferta, c => c.Oferta.Requerimiento, c => c.Oferta.Requerimiento.Proyecto, c => c.Oferta.Requerimiento.Proyecto.Contrato, c => c.Oferta.Requerimiento.Proyecto.Contrato.Cliente);
            var wbs = (from c in wbquery
                       where c.OfertaId == OfertaId && c.vigente == true
                       where c.es_estructura == false
                       select new WbsOfertaDto
                       {
                           Id = c.Id,
                           AreaId = c.AreaId,
                           DisciplinaId = c.DisciplinaId,
                           ElementoId = c.ElementoId,
                           ActividadId = c.ActividadId,
                           estado = c.estado,
                           observaciones = c.observaciones,
                           Oferta=c.Oferta,
                           vigente = c.vigente
                      
                       }).ToList();

            foreach (var e in wbs)
            {
                e.nombrearea = nombrecatalogo2(e.AreaId);
                e.nombreelemento = nombrecatalogo2(e.ElementoId);
                e.nombreactividad = nombrecatalogo2(e.ActividadId);
                e.nombrediciplina = nombrecatalogo2(e.DisciplinaId);

            }

            return wbs;
        }

        public List<WbsOfertaDto> ListarPorOferta(int ofertaId)
        {
            var wbsQuery = Repository.GetAll();

            var items = (from w in wbsQuery
                where w.OfertaId == ofertaId
                where w.vigente == true
                where w.es_estructura == false
                select new WbsOfertaDto()
                {
                    Id = w.Id,
                    ActividadId = w.ActividadId,
                    DisciplinaId = w.DisciplinaId,
                    AreaId = w.AreaId,
                    ElementoId = w.ElementoId,
                    estado = w.estado,
                    OfertaId = w.OfertaId,
                    observaciones = w.observaciones,
                    cliente = w.Oferta.Proyecto.Contrato.Cliente.razon_social,
                    proyecto = w.Oferta.Proyecto.nombre_proyecto,
                    fecha_oferta = w.Oferta.fecha_oferta,
                    vigente = w.vigente,
                    es_estructura = w.es_estructura,
                    fecha_inicio = w.fecha_inicio,
                    fecha_fin = w.fecha_fin,
                    
                }).ToList();

            foreach (var i in items)
            {
                i.nombre_actividad = this.ObtenerNombreArea(i.ActividadId);
                i.nombre_elemento = this.ObtenerNombreArea(i.ElementoId);
                i.nombre_disciplina = this.ObtenerNombreArea(i.DisciplinaId);
                i.nombre_area = this.ObtenerNombreArea(i.AreaId);
            }

            return items;
        }

        public List<WbsOfertaDto> ListarPorOfertaConEstructura(int ofertaId)
        {
            var wbsQuery = Repository.GetAll();

            var items = (from w in wbsQuery
                where w.OfertaId == ofertaId
                where w.vigente == true

                select new WbsOfertaDto()
                {
                    Id = w.Id,
                    ActividadId = w.ActividadId,
                    DisciplinaId = w.DisciplinaId,
                    AreaId = w.AreaId,
                    ElementoId = w.ElementoId,
                    estado = w.estado,
                    OfertaId = w.OfertaId,
                    observaciones = w.observaciones,
                    cliente = w.Oferta.Proyecto.Contrato.Cliente.razon_social,
                    proyecto = w.Oferta.Proyecto.nombre_proyecto,
                    fecha_oferta = w.Oferta.fecha_oferta,
                    vigente = w.vigente,
                    es_estructura = w.es_estructura,
                }).ToList();

            foreach (var i in items)
            {
                i.nombre_actividad = this.ObtenerNombreArea(i.ActividadId);
                i.nombre_elemento = this.ObtenerNombreArea(i.ElementoId);
                i.nombre_disciplina = this.ObtenerNombreArea(i.DisciplinaId);
                i.nombre_area = this.ObtenerNombreArea(i.AreaId);
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
                }).SingleOrDefault();


            return item;
        }

        public List<TreeWbs> GenerarArbol(int ofertaId)
        {
            var listaWbs = this.ListarPorOfertaConEstructura(ofertaId);
            var Lista = new List<TreeWbs>();


            foreach (var r in listaWbs)
            {

                if (r.es_estructura)
                {
                    if (Lista.IsNullOrEmpty())
                    {
                        if (r.DisciplinaId > 0)
                        {
                            if (r.ElementoId > 0)
                            {
                                var elemento = new TreeWbs()
                                {
                                    label = r.nombre_area,
                                    data = r.AreaId + "",
                                    expandedIcon = "fa fa-fw fa-folder-open",
                                    collapsedIcon = "fa fa-fw fa-folder",
                                    tipo = "area",
                                    
                                    nombres = "aaaa",
                                    children = new List<TreeWbs>()
                                    {
                                        new TreeWbs()
                                        {
                                            label = r.nombre_disciplina,
                                            data = r.AreaId + "," + r.DisciplinaId,
                                            expandedIcon = "fa fa-fw fa-folder-open",
                                            collapsedIcon = "fa fa-fw fa-folder",
                                            tipo = "disciplina",
                                          
                                            children = new List<TreeWbs>()
                                            {
                                                new TreeWbs()
                                                {
                                                    label = r.nombre_elemento,
                                                    data = r.AreaId + "," + r.DisciplinaId + "," + r.ElementoId,
                                                    expandedIcon = "fa fa-fw fa-folder-open",
                                                    collapsedIcon = "fa fa-fw fa-folder",
                                                    tipo = "elemento",
                                                   
                                                }
                                            }
                                        }

                                    }

                                };
                                Lista.Add(elemento);
                            }
                            else
                            {
                                var elemento = new TreeWbs()
                                {
                                    label = r.nombre_area,
                                    data = r.AreaId + "",
                                    expandedIcon = "fa fa-fw fa-folder-open",
                                    collapsedIcon = "fa fa-fw fa-folder",
                                  
                                    tipo = "area",
                                    children = new List<TreeWbs>()
                                    {
                                        new TreeWbs()
                                        {
                                            label = r.nombre_disciplina,
                                            data = r.AreaId + "," + r.DisciplinaId,
                                            expandedIcon = "fa fa-fw fa-folder-open",
                                            collapsedIcon = "fa fa-fw fa-folder",
                                            tipo = "disciplina",
                                           
                                            children = new List<TreeWbs>()
                                        }

                                    }

                                };
                                Lista.Add(elemento);
                            }
                        }
                        else
                        {
                            var elemento = new TreeWbs()
                            {
                                label = r.nombre_area,
                                data = r.AreaId + "",
                                expandedIcon = "fa fa-fw fa-folder-open",
                                collapsedIcon = "fa fa-fw fa-folder",
                             
                                tipo = "area",
                                children = new List<TreeWbs>()

                            };
                            Lista.Add(elemento);
                        }
                    }
                    else
                    {


                        if (r.ElementoId > 0)
                        {



                            var index = this.buscarExistenciaItem(Lista, r.nombre_area);
                            if (index >= 0)
                            {
                                var indexDisciplina = buscarExistenciaItem(Lista[index].children, r.nombre_disciplina);
                                if (indexDisciplina >= 0)
                                {
                                    var indexElemento = buscarExistenciaItem(
                                        Lista[index].children[indexDisciplina].children,
                                        r.nombre_elemento);
                                    if (indexElemento < 0)
                                    {
                                        var elemento = new TreeWbs()
                                        {
                                            label = r.nombre_elemento,
                                            data = r.AreaId + "," + r.DisciplinaId + "," + r.ElementoId,
                                            expandedIcon = "fa fa-fw fa-folder-open",
                                            collapsedIcon = "fa fa-fw fa-folder",
                                   
                                            tipo = "elemento",
                                        };
                                        Lista[index].children[indexDisciplina].children.Add(elemento);
                                    }

                                }
                                else
                                {
                                    var elemento = new TreeWbs()
                                    {
                                        label = r.nombre_disciplina,
                                        data = r.AreaId + "," + r.DisciplinaId,
                                        expandedIcon = "fa fa-fw fa-folder-open",
                                        collapsedIcon = "fa fa-fw fa-folder",
                                        tipo = "disciplina",
                                   
                                        children = new List<TreeWbs>()
                                        {
                                            new TreeWbs()
                                            {
                                                label = r.nombre_elemento,
                                                data = r.AreaId + "," + r.DisciplinaId + "," + r.ElementoId,
                                                expandedIcon = "fa fa-fw fa-folder-open",
                                                collapsedIcon = "fa fa-fw fa-folder",
                                                tipo = "elemento",
                                            
                                            }
                                        }

                                    };
                                    Lista[index].children.Add(elemento);
                                }
                            }
                            else
                            {

                                var elemento = new TreeWbs()
                                {
                                    label = r.nombre_area,
                                    data = r.AreaId + "",
                                    expandedIcon = "fa fa-fw fa-folder-open",
                                    collapsedIcon = "fa fa-fw fa-folder",
                                    tipo = "area",
                                 
                                    children = new List<TreeWbs>()
                                    {
                                        new TreeWbs()
                                        {
                                            label = r.nombre_disciplina,
                                            data = r.AreaId + "," + r.DisciplinaId,
                                            expandedIcon = "fa fa-fw fa-folder-open",
                                            collapsedIcon = "fa fa-fw fa-folder",
                                            tipo = "disciplina",
                                     
                                            children = new List<TreeWbs>()
                                            {
                                                new TreeWbs()
                                                {
                                                    label = r.nombre_elemento,
                                                    data = r.AreaId + "," + r.DisciplinaId + "," + r.ElementoId,
                                                    expandedIcon = "fa fa-fw fa-folder-open",
                                                    collapsedIcon = "fa fa-fw fa-folder",
                                                    tipo = "elemento",
                                                  
                                                }
                                            }
                                        }

                                    }

                                };
                                Lista.Add(elemento);

                            }


                        }
                        else if (r.DisciplinaId > 0)
                        {
                            var index = this.buscarExistenciaItem(Lista, r.nombre_area);
                            if (index >= 0)
                            {
                                var indexDisciplina = buscarExistenciaItem(Lista[index].children, r.nombre_disciplina);
                                if (indexDisciplina < 0)
                                {
                                    var elemento = new TreeWbs()
                                    {
                                        label = r.nombre_disciplina,
                                        data = r.AreaId + "," + r.DisciplinaId,
                                        expandedIcon = "fa fa-fw fa-folder-open",
                                        collapsedIcon = "fa fa-fw fa-folder",
                                        tipo = "disciplina",
                                   
                                        children = new List<TreeWbs>()
                                    };
                                    Lista[index].children.Add(elemento);
                                }
                            }
                            else
                            {
                                var elemento = new TreeWbs()
                                {
                                    label = r.nombre_area,
                                    data = r.AreaId + "",
                                    expandedIcon = "fa fa-fw fa-folder-open",
                                    collapsedIcon = "fa fa-fw fa-folder",
                                    tipo = "area",
                                
                                    children = new List<TreeWbs>()
                                    {
                                        new TreeWbs()
                                        {
                                            label = r.nombre_disciplina,
                                            data = r.AreaId + "," + r.DisciplinaId,
                                            expandedIcon = "fa fa-fw fa-folder-open",
                                            collapsedIcon = "fa fa-fw fa-folder",
                                            tipo = "disciplina",
                                       
                                            children = new List<TreeWbs>()
                                        }

                                    }

                                };
                                Lista.Add(elemento);
                            }
                        }
                        else
                        {
                            var index = this.buscarExistenciaItem(Lista, r.nombre_area);
                            if (index < 0)
                            {
                                var elemento = new TreeWbs()
                                {
                                    label = r.nombre_area,
                                    data = r.AreaId + "",
                                    expandedIcon = "fa fa-fw fa-folder-open",
                                    collapsedIcon = "fa fa-fw fa-folder",
                                    tipo = "area",
                                   
                                    children = new List<TreeWbs>()

                                };
                                Lista.Add(elemento);
                            }

                        }

                    }

                }
                else
                {

                    //desdeaqui...

                    if (Lista.IsNullOrEmpty())
                    {
                        var listacomputos = _computoService.GetComputosporWbsOferta(r.Id); // lista de computos por wbs
                        var elemento = new TreeWbs()
                        {
                            label = r.nombre_area,
                            data = r.AreaId + "",
                            expandedIcon = "fa fa-fw fa-folder-open",
                            collapsedIcon = "fa fa-fw fa-folder",
                            tipo = "area",
                            
                            children = new List<TreeWbs>()
                            {
                                new TreeWbs()
                                {
                                    label = r.nombre_disciplina,
                                    data = r.AreaId + "," + r.DisciplinaId,
                                    expandedIcon = "fa fa-fw fa-folder-open",
                                    collapsedIcon = "fa fa-fw fa-folder",
                                    tipo = "disciplina",
                                
                                    children = new List<TreeWbs>()
                                    {
                                        new TreeWbs()
                                        {
                                            label = r.nombre_elemento,
                                            data = r.AreaId + "," + r.DisciplinaId + "," + r.ElementoId,
                                            expandedIcon = "fa fa-fw fa-folder-open",
                                            collapsedIcon = "fa fa-fw fa-folder",
                                            tipo = "elemento",
                                           
                                            children = new List<TreeWbs>()
                                            {
                                                   
                                           new TreeWbs()
                                                {
                                                    label = r.nombre_actividad,
                                                    data = r.AreaId + "," + r.DisciplinaId + "," + r.ElementoId + "," +
                                                           r.ActividadId + "," + r.Id,
                                                    icon = "fa fa-fw fa-file-word-o",
                                                    tipo = "actividad",
                                                 
                                                    nombres = r.nombre_area + ", " + r.nombre_disciplina + ", " + r.nombre_elemento + ", " + r.nombre_actividad,
                                                                                                 
                                                 

                                                }
                                            }

                                        }
                                    }
                                }

                            }

                        };
                        Lista.Add(elemento);
                    }
                    else
                    {
                        var index = this.buscarExistenciaItem(Lista, r.nombre_area);
                        if (index >= 0)
                        {
                            var indexDisciplina = buscarExistenciaItem(Lista[index].children, r.nombre_disciplina);
                            if (indexDisciplina >= 0)
                            {
                                var indexElemento = buscarExistenciaItem(
                                    Lista[index].children[indexDisciplina].children,
                                    r.nombre_elemento);
                                if (indexElemento >= 0)
                                {
                                    var elemento = new TreeWbs()
                                    {
                                        label = r.nombre_actividad,
                                        data = r.AreaId + "," + r.DisciplinaId + "," + r.ElementoId + "," +
                                               r.ActividadId + "," + r.Id,
                                        icon = "fa fa-fw fa-file-word-o",
                                        tipo = "actividad",
                                        nombres = r.nombre_area + ", " + r.nombre_disciplina + ", " + r.nombre_elemento + ", " + r.nombre_actividad

                                    };
                                    if (Lista[index].children[indexDisciplina].children[indexElemento].children == null)
                                    {
                                        Lista[index].children[indexDisciplina].children[indexElemento].children =
                                            new List<TreeWbs>();
                                    }

                                    Lista[index].children[indexDisciplina].children[indexElemento].children
                                        .Add(elemento);
                                }
                                else
                                {
                                    var elemento = new TreeWbs()
                                    {
                                        label = r.nombre_elemento,
                                        data = r.AreaId + "," + r.DisciplinaId + "," + r.ElementoId,
                                        expandedIcon = "fa fa-fw fa-folder-open",
                                        collapsedIcon = "fa fa-fw fa-folder",
                                        tipo = "elemento",
                                      
                                        children = new List<TreeWbs>()
                                        {
                                            new TreeWbs()
                                            {
                                            label = r.nombre_actividad,
                                            data = r.AreaId + "," + r.DisciplinaId + "," + r.ElementoId + "," + r.ActividadId + "," + r.Id,
                                            icon = "fa fa-fw fa-file-word-o",
                                            tipo = "actividad",
                                            nombres = r.nombre_area + ", " + r.nombre_disciplina + ", " + r.nombre_elemento + ", " + r.nombre_actividad
                                        }

                                        }


                                    };
                                    Lista[index].children[indexDisciplina].children.Add(elemento);
                                }
                            }
                            else
                            {
                                var elemento = new TreeWbs()
                                {
                                    label = r.nombre_disciplina,
                                    data = r.AreaId + "," + r.DisciplinaId,
                                    expandedIcon = "fa fa-fw fa-folder-open",
                                    collapsedIcon = "fa fa-fw fa-folder",
                                    tipo = "disciplina",
                                  
                                    children = new List<TreeWbs>()
                                    {
                                        new TreeWbs()
                                        {
                                            label = r.nombre_elemento,
                                            data = r.AreaId + "," + r.DisciplinaId + "," + r.ElementoId,
                                            expandedIcon = "fa fa-fw fa-folder-open",
                                            collapsedIcon = "fa fa-fw fa-folder",
                                            tipo = "elemento",
                                         
                                            children = new List<TreeWbs>()
                                            {
                                                new TreeWbs()
                                                {
                                                label = r.nombre_actividad,
                                                data = r.AreaId + "," + r.DisciplinaId + "," + r.ElementoId + "," + r.ActividadId + "," + r.Id,
                                                icon = "fa fa-fw fa-file-word-o",
                                                tipo = "actividad",
                                                nombres = r.nombre_area + ", " + r.nombre_disciplina + ", " + r.nombre_elemento + ", " + r.nombre_actividad
                                            }

                                            }

                                        }
                                    }

                                };
                                Lista[index].children.Add(elemento);
                            }
                        }
                        else
                        {
                            var elemento = new TreeWbs()
                            {
                                label = r.nombre_area,
                                data = r.AreaId + "",
                                expandedIcon = "fa fa-fw fa-folder-open",
                                collapsedIcon = "fa fa-fw fa-folder",
                                tipo = "area",
                                
                                children = new List<TreeWbs>()
                                {
                                    new TreeWbs()
                                    {
                                        label = r.nombre_disciplina,
                                        data = r.AreaId + "," + r.DisciplinaId,
                                        expandedIcon = "fa fa-fw fa-folder-open",
                                        collapsedIcon = "fa fa-fw fa-folder",
                                        tipo = "disciplina",
                                     
                                        children = new List<TreeWbs>()
                                        {
                                            new TreeWbs()
                                            {
                                                label = r.nombre_elemento,
                                                data = r.AreaId + "," + r.DisciplinaId + "," + r.ElementoId,
                                                expandedIcon = "fa fa-fw fa-folder-open",
                                                collapsedIcon = "fa fa-fw fa-folder",
                                                tipo = "elemento",
                                         
                                                children = new List<TreeWbs>()
                                                {
                                                    new TreeWbs()
                                                    {
                                                    label = r.nombre_actividad,
                                                    data = r.AreaId + "," + r.DisciplinaId + "," + r.ElementoId + "," + r.ActividadId + "," + r.Id,
                                                    icon = "fa fa-fw fa-file-word-o",
                                                    tipo = "actividad",
                                                    nombres = r.nombre_area + ", " + r.nombre_disciplina + ", " + r.nombre_elemento + ", " + r.nombre_actividad
                                                }}

                                            }
                                        }
                                    }

                                }

                            };
                            Lista.Add(elemento);
                        }
                    }



                }



            }

            return Lista;
        }

        public List<JerarquiaWbs> GenerarJerarquia(int ofertaId)
        {
            var listaWbs = this.ListarPorOfertaConEstructura(ofertaId);
            var Lista = new List<JerarquiaWbs>();


            foreach (var r in listaWbs)
            {

                if (r.es_estructura)
                {
                    if (Lista.IsNullOrEmpty())
                    {
                        if (r.DisciplinaId > 0)
                        {
                            if (r.ElementoId > 0)
                            {
                                var elemento = new JerarquiaWbs()
                                {
                                    label = "Area",
                                    className = "ui-area",
                                    expanded = true,
                                    type = "area",
                                    data = r.nombre_area,
                                    children = new List<JerarquiaWbs>()
                                    {
                                        new JerarquiaWbs()
                                        {
                                            label = "Disciplina",
                                            className = "ui-disciplina",
                                            expanded = false,
                                            type = "disciplina",
                                            data = r.nombre_disciplina,
                                            children = new List<JerarquiaWbs>()
                                            {
                                                new JerarquiaWbs()
                                                {
                                                    label = "Elemento",
                                                    className = "ui-elemento",
                                                    expanded = false,
                                                    type = "elemento",
                                                    data = r.nombre_elemento,
                                                }
                                            }
                                        }

                                    }

                                };
                                Lista.Add(elemento);
                            }
                            else
                            {

                                var elemento = new JerarquiaWbs()
                                {
                                    label = "Area",
                                    className = "ui-area",
                                    expanded = false,
                                    type = "area",
                                    data = r.nombre_area,
                                    children = new List<JerarquiaWbs>()
                                    {
                                        new JerarquiaWbs()
                                        {
                                            label = "Disciplina",
                                            className = "ui-disciplina",
                                            expanded = false,
                                            type = "disciplina",
                                            data = r.nombre_disciplina,
                                            children = new List<JerarquiaWbs>()

                                        }

                                    }

                                };
                                Lista.Add(elemento);
                            }
                        }
                        else
                        {
                            var elemento = new JerarquiaWbs()
                            {
                                label = "Area",
                                className = "ui-area",
                                expanded = false,
                                type = "area",
                                data = r.nombre_area,
                                children = new List<JerarquiaWbs>()

                            };
                            Lista.Add(elemento);
                        }
                    }
                    else
                    {


                        if (r.ElementoId > 0)
                        {



                            var index = this.buscarExistenciaItemJerarquia(Lista, r.nombre_area);
                            if (index >= 0)
                            {
                                var indexDisciplina =
                                    buscarExistenciaItemJerarquia(Lista[index].children, r.nombre_disciplina);
                                if (indexDisciplina >= 0)
                                {
                                    var indexElemento = buscarExistenciaItemJerarquia(
                                        Lista[index].children[indexDisciplina].children,
                                        r.nombre_elemento);
                                    if (indexElemento < 0)
                                    {
                                        var elemento = new JerarquiaWbs()
                                        {
                                            label = "Elemento",
                                            className = "ui-elemento",
                                            expanded = false,
                                            type = "elemento",
                                            data = r.nombre_elemento,
                                        };
                                        Lista[index].children[indexDisciplina].children.Add(elemento);
                                    }

                                }
                                else
                                {
                                    var elemento = new JerarquiaWbs()
                                    {
                                        label = "Disciplina",
                                        className = "ui-disciplina",
                                        expanded = false,
                                        type = "disciplina",
                                        data = r.nombre_disciplina,
                                        children = new List<JerarquiaWbs>()
                                        {
                                            new JerarquiaWbs()
                                            {
                                                label = "Elemento",
                                                className = "ui-elemento",
                                                expanded = false,
                                                type = "elemento",
                                                data = r.nombre_elemento,

                                            }
                                        }

                                    };
                                    Lista[index].children.Add(elemento);
                                }
                            }
                            else
                            {

                                var elemento = new JerarquiaWbs()
                                {
                                    label = "Area",
                                    className = "ui-area",
                                    expanded = false,
                                    type = "area",
                                    data = r.nombre_area,
                                    children = new List<JerarquiaWbs>()
                                    {
                                        new JerarquiaWbs()
                                        {
                                            label = "Disciplina",
                                            className = "ui-disciplina",
                                            expanded = false,
                                            type = "disciplina",
                                            data = r.nombre_disciplina,
                                            children = new List<JerarquiaWbs>()
                                            {
                                                new JerarquiaWbs()
                                                {
                                                    label = "Elemento",
                                                    className = "ui-elemento",
                                                    expanded = false,
                                                    type = "elemento",
                                                    data = r.nombre_elemento,

                                                }
                                            }
                                        }

                                    }

                                };
                                Lista.Add(elemento);

                            }


                        }
                        else if (r.DisciplinaId > 0)
                        {
                            var index = this.buscarExistenciaItemJerarquia(Lista, r.nombre_area);
                            if (index >= 0)
                            {
                                var indexDisciplina =
                                    buscarExistenciaItemJerarquia(Lista[index].children, r.nombre_disciplina);
                                if (indexDisciplina < 0)
                                {
                                    var elemento = new JerarquiaWbs()
                                    {
                                        label = "Disciplina",
                                        className = "ui-disciplina",
                                        expanded = false,
                                        type = "disciplina",
                                        data = r.nombre_disciplina,
                                        children = new List<JerarquiaWbs>()
                                    };
                                    Lista[index].children.Add(elemento);
                                }
                            }
                            else
                            {
                                var elemento = new JerarquiaWbs()
                                {
                                    label = "Area",
                                    className = "ui-area",
                                    expanded = false,
                                    type = "area",
                                    data = r.nombre_area,
                                    children = new List<JerarquiaWbs>()
                                    {
                                        new JerarquiaWbs()
                                        {
                                            label = "Disciplina",
                                            className = "ui-disciplina",
                                            expanded = false,
                                            type = "disciplina",
                                            data = r.nombre_disciplina,
                                            children = new List<JerarquiaWbs>()
                                        }

                                    }

                                };
                                Lista.Add(elemento);
                            }
                        }
                        else
                        {
                            var index = this.buscarExistenciaItemJerarquia(Lista, r.nombre_area);
                            if (index < 0)
                            {
                                var elemento = new JerarquiaWbs()
                                {
                                    label = "Area",
                                    className = "ui-area",
                                    expanded = false,
                                    type = "area",
                                    data = r.nombre_area,
                                    children = new List<JerarquiaWbs>()

                                };
                                Lista.Add(elemento);
                            }

                        }

                    }

                }
                else
                {



                    if (Lista.IsNullOrEmpty())
                    {

                        var elemento = new JerarquiaWbs()
                        {
                            label = "Area",
                            className = "ui-area",
                            expanded = false,
                            type = "area",
                            data = r.nombre_area,
                            children = new List<JerarquiaWbs>()
                            {
                                new JerarquiaWbs()
                                {
                                    label = "Disciplina",
                                    className = "ui-disciplina",
                                    expanded = false,
                                    type = "disciplina",
                                    data = r.nombre_disciplina,
                                    children = new List<JerarquiaWbs>()
                                    {
                                        new JerarquiaWbs()
                                        {
                                            label = "Elemento",
                                            className = "ui-elemento",
                                            expanded = false,
                                            type = "elemento",
                                            data = r.nombre_elemento,
                                            children = new List<JerarquiaWbs>()
                                            {
                                                new JerarquiaWbs()
                                                {
                                                    label = "Actividad",
                                                    className = "ui-actividad",
                                                    expanded = false,
                                                    type = "actividad",
                                                    data = r.nombre_actividad,

                                                }
                                            }

                                        }
                                    }
                                }

                            }

                        };
                        Lista.Add(elemento);
                    }
                    else
                    {
                        var index = this.buscarExistenciaItemJerarquia(Lista, r.nombre_area);
                        if (index >= 0)
                        {
                            var indexDisciplina =
                                buscarExistenciaItemJerarquia(Lista[index].children, r.nombre_disciplina);
                            if (indexDisciplina >= 0)
                            {
                                var indexElemento = buscarExistenciaItemJerarquia(
                                    Lista[index].children[indexDisciplina].children,
                                    r.nombre_elemento);
                                if (indexElemento >= 0)
                                {
                                    var elemento = new JerarquiaWbs()
                                    {
                                        label = "Actividad",
                                        className = "ui-actividad",
                                        expanded = false,
                                        type = "actividad",
                                        data = r.nombre_actividad,

                                    };
                                    if (Lista[index].children[indexDisciplina].children[indexElemento].children == null)
                                    {
                                        Lista[index].children[indexDisciplina].children[indexElemento].children =
                                            new List<JerarquiaWbs>();
                                    }

                                    Lista[index].children[indexDisciplina].children[indexElemento].children
                                        .Add(elemento);
                                }
                                else
                                {
                                    var elemento = new JerarquiaWbs()
                                    {
                                        label = "Elemento",
                                        className = "ui-elemento",
                                        expanded = false,
                                        type = "elemento",
                                        data = r.nombre_elemento,
                                        children = new List<JerarquiaWbs>()
                                        {
                                            new JerarquiaWbs()
                                            {
                                                label = "Actividad",
                                                className = "ui-actividad",
                                                expanded = false,
                                                type = "actividad",
                                                data = r.nombre_actividad,
                                            }
                                        }


                                    };
                                    Lista[index].children[indexDisciplina].children.Add(elemento);
                                }
                            }
                            else
                            {
                                var elemento = new JerarquiaWbs()
                                {
                                    label = "Disciplina",
                                    className = "ui-disciplina",
                                    expanded = false,
                                    type = "disciplina",
                                    data = r.nombre_disciplina,
                                    children = new List<JerarquiaWbs>()
                                    {
                                        new JerarquiaWbs()
                                        {
                                            label = "Elemento",
                                            className = "ui-elemento",
                                            expanded = false,
                                            type = "elemento",
                                            data = r.nombre_elemento,
                                            children = new List<JerarquiaWbs>()
                                            {
                                                new JerarquiaWbs()
                                                {
                                                    label = "Actividad",
                                                    className = "ui-actividad",
                                                    expanded = false,
                                                    type = "actividad",
                                                    data = r.nombre_actividad,
                                                }
                                            }

                                        }
                                    }

                                };
                                Lista[index].children.Add(elemento);
                            }
                        }
                        else
                        {
                            var elemento = new JerarquiaWbs()
                            {
                                label = "Area",
                                className = "ui-area",
                                expanded = false,
                                type = "area",
                                data = r.nombre_area,
                                children = new List<JerarquiaWbs>()
                                {
                                    new JerarquiaWbs()
                                    {
                                        label = "Disciplina",
                                        className = "ui-disciplina",
                                        expanded = false,
                                        type = "disciplina",
                                        data = r.nombre_disciplina,

                                        children = new List<JerarquiaWbs>()
                                        {
                                            new JerarquiaWbs()
                                            {
                                                label = "Elemento",
                                                className = "ui-elemento",
                                                expanded = false,
                                                type = "elemento",
                                                data = r.nombre_elemento,
                                                children = new List<JerarquiaWbs>()
                                                {
                                                    new JerarquiaWbs()
                                                    {
                                                        label = "Actividad",
                                                        className = "ui-actividad",
                                                        expanded = false,
                                                        type = "actividad",
                                                        data = r.nombre_actividad,
                                                    }
                                                }

                                            }
                                        }
                                    }

                                }

                            };
                            Lista.Add(elemento);
                        }
                    }



                }



            }

            return Lista;
        }


        public int buscarExistenciaItem(List<TreeWbs> lista, string nombre)
        {
            var r = -1;
            for (int j = 0; j < lista.Count; j++)
            {
                if (lista[j].label.Equals(nombre))
                {
                    return j;
                }
            }

            return r;
        }

        public int buscarExistenciaItemJerarquia(List<JerarquiaWbs> lista, string nombre)
        {
            var r = -1;
            for (int j = 0; j < lista.Count; j++)
            {
                if (lista[j].data.Equals(nombre))
                {
                    return j;
                }
            }

            return r;
        }

        public int EliminarVigencia(int WbsOfertaId)
        {
            var wbs = Repository.Get(WbsOfertaId);
            wbs.vigente = false;
            Repository.Update(wbs);
            return wbs.OfertaId;
        }

        public List<string[]> ObtenerWbsDistintos(int ofertaId)
        {
            var wbsQuery = Repository.GetAll();
            var items = (from w in wbsQuery
                where w.OfertaId == ofertaId
                where w.vigente == true
                where w.es_estructura == false
                select new WbsOfertaDto()
                {
                    DisciplinaId = w.DisciplinaId,
                    AreaId = w.AreaId,
                    ElementoId = w.ElementoId,
                }).Distinct().ToList();

            List<string[]> wbs = new List<string[]>();
            var tempArea = "";
            var tempDiciplina = "";
            var tempElemento = "";

            foreach (var i in items)
            {
                if (!tempArea.Equals(this.ObtenerNombreArea(i.AreaId)))
                {
                    tempArea = this.ObtenerNombreArea(i.AreaId);
                    wbs.Add(new string[]
                    {
                        tempArea,
                        "list-group-item list-group-item-primary",
                        ""
                    });
                    tempDiciplina = "";
                    tempElemento = "";
                }


                if (!tempDiciplina.Equals(this.ObtenerNombreDiciplina(i.DisciplinaId)))
                {
                    tempDiciplina = this.ObtenerNombreDiciplina(i.DisciplinaId);
                    wbs.Add(new string[]
                    {
                        tempDiciplina,
                        "list-group-item list-group-item-dark",
                        "padding-left: 2em"
                    });
                }

                if (!tempElemento.Equals(this.ObtenerNombreElemento(i.ElementoId)))
                {
                    tempElemento = this.ObtenerNombreElemento(i.ElementoId);
                    wbs.Add(new string[]
                    {
                        tempElemento,
                        "list-group-item list-group-item-secondary",
                        "padding-left: 4em"
                    });
                }

                var actividades = (from a in wbsQuery
                    where a.OfertaId == ofertaId
                    where a.vigente == true
                    where a.AreaId == i.AreaId
                    where a.DisciplinaId == i.DisciplinaId
                    where a.ElementoId == i.ElementoId
                    where a.es_estructura == false
                    select new WbsOfertaDto()
                    {
                        ActividadId = a.ActividadId
                    }).ToList();

                foreach (var a in actividades)
                {
                    wbs.Add(new string[]
                    {
                        this.ObtenerNombreActividad(a.ActividadId),
                        "list-group-item list-group-item-light",
                        "padding-left: 6em"
                    });
                }
            }

            return wbs;
        }

        public string nombrecatalogo2(int tipocatagoid)
        {
            var a = repositorycatalogo.Get(tipocatagoid);

            return a.nombre;

        }

        public List<CatalogoDto> GetAreas()
        {
            var areasQ = _catalogoRepository.GetAll();
            var items = (from w in areasQ
                where w.TipoCatalogoId == 1
                where w.vigente == true
                select new CatalogoDto()
                {
                    Id = w.Id,
                    nombre = w.nombre
                }).Distinct().ToList();
            return items;
        }

        public List<CatalogoDto> GetDisciplinas()
        {
            var areasQ = _catalogoRepository.GetAll();
            var items = (from w in areasQ
                where w.TipoCatalogoId == 2
                where w.vigente == true
                select new CatalogoDto()
                {
                    Id = w.Id,
                    nombre = w.nombre
                }).Distinct().ToList();
            return items;
        }

        public List<CatalogoDto> GetElementos()
        {
            var areasQ = _catalogoRepository.GetAll();
            var items = (from w in areasQ
                where w.TipoCatalogoId == 3
                where w.vigente == true
                select new CatalogoDto()
                {
                    Id = w.Id,
                    nombre = w.nombre
                }).Distinct().ToList();
            return items;
        }

        public List<CatalogoDto> GetActtividades()
        {
            var areasQ = _catalogoRepository.GetAll();
            var items = (from w in areasQ
                where w.TipoCatalogoId == 4
                where w.vigente == true
                select new CatalogoDto()
                {
                    Id = w.Id,
                    nombre = w.nombre
                }).Distinct().ToList();
            return items;
        }

        public string ObtenerNombreArea(int id)
        {
            var areasQ = _catalogoRepository.GetAll();
            var item = (from w in areasQ
                where w.Id == id
                where w.vigente == true
                select new CatalogoDto()
                {
                    Id = w.Id,
                    nombre = w.nombre
                }).FirstOrDefault();
            return item.nombre;
        }

        public string ObtenerNombreDiciplina(int id)
        {
            var areasQ = _catalogoRepository.GetAll();
            var item = (from w in areasQ
                where w.Id == id
                where w.vigente == true
                select new CatalogoDto()
                {
                    Id = w.Id,
                    nombre = w.nombre
                }).FirstOrDefault();
            return item.nombre;
        }

        public string ObtenerNombreElemento(int id)
        {
            var areasQ = _catalogoRepository.GetAll();
            var item = (from w in areasQ
                where w.Id == id
                where w.vigente == true
                select new CatalogoDto()
                {
                    Id = w.Id,
                    nombre = w.nombre
                }).FirstOrDefault();
            return item.nombre;
        }

        public string ObtenerNombreActividad(int id)
        {
            var areasQ = _catalogoRepository.GetAll();
            var item = (from w in areasQ
                where w.Id == id
                where w.vigente == true
                select new CatalogoDto()
                {
                    Id = w.Id,
                    nombre = w.nombre
                }).FirstOrDefault();
            return item.nombre;
        }


        public bool ComprobarExistenciaWbs(int Area, int Disciplina, int Elemento, int Actividad, int Oferta)
        {
            var wbs = Repository.GetAll()
                .Where(o => o.AreaId == Area)
                .Where(o => o.DisciplinaId == Disciplina)
                .Where(o => o.ElementoId == Elemento)
                .Where(o => o.ActividadId == Actividad)
                .Where(o => o.OfertaId == Oferta)
                .Where(o => o.vigente == true)
                .ToList();

            if (wbs.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }



        public WbsOfertaDto GetDetalle(int WbsOfertaId)
        {
            var wbsofertasQuery = Repository.GetAll();
            var item = (from r in wbsofertasQuery
                        where r.Id == WbsOfertaId
                        select new WbsOfertaDto()
                        {
                            Id = r.Id,
                            OfertaId = r.OfertaId,
                            AreaId = r.AreaId,
                            DisciplinaId = r.DisciplinaId,
                            ElementoId = r.ElementoId,
                            ActividadId = r.ActividadId,
                            estado = r.estado,
                            observaciones = r.observaciones,
                            fecha_inicio = r.fecha_inicio,
                            fecha_fin = r.fecha_fin,
                            es_estructura = r.es_estructura,
                            vigente = r.vigente

                        }).FirstOrDefault();
            return item;
        }

        public List<Catalogo> GetAreasWbsRegistrado(int OfertaId)
        {
            List<Catalogo> Areas = new List<Catalogo>();

            var listawbs = GetWbsOfertas(OfertaId);
            foreach (var wbs in listawbs)
            {
                Areas.Add(repositorycatalogo.Get(wbs.AreaId));
            }

            return Areas.Distinct().ToList();

        }

        public List<Catalogo> GetDisciplinasWbsRegistrado(int OfertaId, int AreaId)
        {
            List<Catalogo> Disciplinas = new List<Catalogo>();

            var listawbs = GetWbsOfertas(OfertaId).Where(x => x.AreaId == AreaId);
            foreach (var wbs in listawbs)
            {
                Disciplinas.Add(repositorycatalogo.Get(wbs.DisciplinaId));
            }

            return Disciplinas.Distinct().ToList();

        }

        public List<Catalogo> GetElementosWbsRegistrado(int OfertaId, int AreaId, int DisciplinaId)
        {
            List<Catalogo> Elementos = new List<Catalogo>();

            var listawbs = GetWbsOfertas(OfertaId).Where(x => x.AreaId == AreaId)
                .Where(x => x.DisciplinaId == DisciplinaId);
            foreach (var wbs in listawbs)
            {
                Elementos.Add(repositorycatalogo.Get(wbs.ElementoId));
            }

            return Elementos.Distinct().ToList();

        }

        public List<Catalogo> GetActividadesWbsRegistrado(int OfertaId, int AreaId, int DisciplinaId, int ElementoId)
        {
            List<Catalogo> Actividades = new List<Catalogo>();

            var listawbs = GetWbsOfertas(OfertaId).Where(x => x.AreaId == AreaId)
                .Where(x => x.DisciplinaId == DisciplinaId).Where(x => x.ElementoId == ElementoId);
            foreach (var wbs in listawbs)
            {
                Actividades.Add(repositorycatalogo.Get(wbs.ActividadId));
            }

            return Actividades.Distinct().ToList();
        }

        public WbsOfertaDto GetWbsOfertaIdpor(int OfertaId, int Area, int Disc, int Elem, int Act)
        {
            var wbquery = Repository.GetAllIncluding(c => c.Oferta, c => c.Oferta.Requerimiento,
                    c => c.Oferta.Requerimiento.Proyecto, c => c.Oferta.Requerimiento.Proyecto.Contrato,
                    c => c.Oferta.Requerimiento.Proyecto.Contrato.Cliente)
                .Where(x => x.AreaId == Area).Where(x => x.DisciplinaId == Disc).Where(x => x.ElementoId == Elem)
                .Where(x => x.ActividadId == Act);


            // wbquery = Repository.GetAllIncluding(c => c.Oferta, c => c.Oferta.Requerimiento, c => c.Oferta.Requerimiento.Proyecto, c => c.Oferta.Requerimiento.Proyecto.Contrato, c => c.Oferta.Requerimiento.Proyecto.Contrato.Cliente);
            var wbs = (from c in wbquery
                where c.OfertaId == OfertaId && c.vigente == true
                select new WbsOfertaDto
                {
                    Id = c.Id,
                    AreaId = c.AreaId,
                    DisciplinaId = c.DisciplinaId,
                    ElementoId = c.ElementoId,
                    ActividadId = c.ActividadId,
                    estado = c.estado,
                    observaciones = c.observaciones,
                    Oferta = c.Oferta,
                    vigente = c.vigente

                }).SingleOrDefault();


            wbs.nombrearea = nombrecatalogo2(wbs.AreaId);
            wbs.nombreelemento = nombrecatalogo2(wbs.ElementoId);
            wbs.nombreactividad = nombrecatalogo2(wbs.ActividadId);
            wbs.nombrediciplina = nombrecatalogo2(wbs.DisciplinaId);


            return wbs;
        }

        public List<TreeWbs> GenerarArbolComputo(int ofertaId)
        {
            var listaWbs = this.ListarPorOfertaConEstructura(ofertaId);
            var Lista = new List<TreeWbs>();


            foreach (var r in listaWbs)
            {

                if (r.es_estructura)
                {
                    if (Lista.IsNullOrEmpty())
                    {
                        if (r.DisciplinaId > 0)
                        {
                            if (r.ElementoId > 0)
                            {
                                var elemento = new TreeWbs()
                                {
                                    label = r.nombre_area,
                                    data = r.AreaId + "",
                                    expandedIcon = "fa fa-fw fa-folder-open",
                                    collapsedIcon = "fa fa-fw fa-folder",
                                    tipo = "area",
                                
                                    nombres = "aaaa",
                                    children = new List<TreeWbs>()
                                    {
                                        new TreeWbs()
                                        {
                                            label = r.nombre_disciplina,
                                            data = r.AreaId + "," + r.DisciplinaId,
                                            expandedIcon = "fa fa-fw fa-folder-open",
                                            collapsedIcon = "fa fa-fw fa-folder",
                                            tipo = "disciplina",
                                       
                                            children = new List<TreeWbs>()
                                            {
                                                new TreeWbs()
                                                {
                                                    label = r.nombre_elemento,
                                                    data = r.AreaId + "," + r.DisciplinaId + "," + r.ElementoId,
                                                    expandedIcon = "fa fa-fw fa-folder-open",
                                                    collapsedIcon = "fa fa-fw fa-folder",
                                                    tipo = "elemento",
                                               
                                                }
                                            }
                                        }

                                    }

                                };
                                Lista.Add(elemento);
                            }
                            else
                            {
                                var elemento = new TreeWbs()
                                {
                                    label = r.nombre_area,
                                    data = r.AreaId + "",
                                    expandedIcon = "fa fa-fw fa-folder-open",
                                    collapsedIcon = "fa fa-fw fa-folder",
                                    tipo = "area",
                                  
                                    children = new List<TreeWbs>()
                                    {
                                        new TreeWbs()
                                        {
                                            label = r.nombre_disciplina,
                                            data = r.AreaId + "," + r.DisciplinaId,
                                            expandedIcon = "fa fa-fw fa-folder-open",
                                            collapsedIcon = "fa fa-fw fa-folder",
                                            tipo = "disciplina",
                                      
                                            children = new List<TreeWbs>()
                                        }

                                    }

                                };
                                Lista.Add(elemento);
                            }
                        }
                        else
                        {
                            var elemento = new TreeWbs()
                            {
                                label = r.nombre_area,
                                data = r.AreaId + "",
                                expandedIcon = "fa fa-fw fa-folder-open",
                                collapsedIcon = "fa fa-fw fa-folder",
                                tipo = "area",
                           
                                children = new List<TreeWbs>()

                            };
                            Lista.Add(elemento);
                        }
                    }
                    else
                    {


                        if (r.ElementoId > 0)
                        {



                            var index = this.buscarExistenciaItem(Lista, r.nombre_area);
                            if (index >= 0)
                            {
                                var indexDisciplina = buscarExistenciaItem(Lista[index].children, r.nombre_disciplina);
                                if (indexDisciplina >= 0)
                                {
                                    var indexElemento = buscarExistenciaItem(
                                        Lista[index].children[indexDisciplina].children,
                                        r.nombre_elemento);
                                    if (indexElemento < 0)
                                    {
                                        var elemento = new TreeWbs()
                                        {
                                            label = r.nombre_elemento,
                                            data = r.AreaId + "," + r.DisciplinaId + "," + r.ElementoId,
                                            expandedIcon = "fa fa-fw fa-folder-open",
                                            collapsedIcon = "fa fa-fw fa-folder",
                                            tipo = "elemento",
                                      
                                        };
                                        Lista[index].children[indexDisciplina].children.Add(elemento);
                                    }

                                }
                                else
                                {
                                    var elemento = new TreeWbs()
                                    {
                                        label = r.nombre_disciplina,
                                        data = r.AreaId + "," + r.DisciplinaId,
                                        expandedIcon = "fa fa-fw fa-folder-open",
                                        collapsedIcon = "fa fa-fw fa-folder",
                                        tipo = "disciplina",
                                     
                                        children = new List<TreeWbs>()
                                        {
                                            new TreeWbs()
                                            {
                                                label = r.nombre_elemento,
                                                data = r.AreaId + "," + r.DisciplinaId + "," + r.ElementoId,
                                                expandedIcon = "fa fa-fw fa-folder-open",
                                                collapsedIcon = "fa fa-fw fa-folder",
                                                tipo = "elemento",
                                           
                                            }
                                        }

                                    };
                                    Lista[index].children.Add(elemento);
                                }
                            }
                            else
                            {

                                var elemento = new TreeWbs()
                                {
                                    label = r.nombre_area,
                                    data = r.AreaId + "",
                                    expandedIcon = "fa fa-fw fa-folder-open",
                                    collapsedIcon = "fa fa-fw fa-folder",
                                    tipo = "area",
                           
                                    children = new List<TreeWbs>()
                                    {
                                        new TreeWbs()
                                        {
                                            label = r.nombre_disciplina,
                                            data = r.AreaId + "," + r.DisciplinaId,
                                            expandedIcon = "fa fa-fw fa-folder-open",
                                            collapsedIcon = "fa fa-fw fa-folder",
                                            tipo = "disciplina",
                                  
                                            children = new List<TreeWbs>()
                                            {
                                                new TreeWbs()
                                                {
                                                    label = r.nombre_elemento,
                                                    data = r.AreaId + "," + r.DisciplinaId + "," + r.ElementoId,
                                                    expandedIcon = "fa fa-fw fa-folder-open",
                                                    collapsedIcon = "fa fa-fw fa-folder",
                                                    tipo = "elemento",
                                                   
                                                }
                                            }
                                        }

                                    }

                                };
                                Lista.Add(elemento);

                            }


                        }
                        else if (r.DisciplinaId > 0)
                        {
                            var index = this.buscarExistenciaItem(Lista, r.nombre_area);
                            if (index >= 0)
                            {
                                var indexDisciplina = buscarExistenciaItem(Lista[index].children, r.nombre_disciplina);
                                if (indexDisciplina < 0)
                                {
                                    var elemento = new TreeWbs()
                                    {
                                        label = r.nombre_disciplina,
                                        data = r.AreaId + "," + r.DisciplinaId,
                                        expandedIcon = "fa fa-fw fa-folder-open",
                                        collapsedIcon = "fa fa-fw fa-folder",
                                        tipo = "disciplina",
                                  
                                        children = new List<TreeWbs>()
                                    };
                                    Lista[index].children.Add(elemento);
                                }
                            }
                            else
                            {
                                var elemento = new TreeWbs()
                                {
                                    label = r.nombre_area,
                                    data = r.AreaId + "",
                                    expandedIcon = "fa fa-fw fa-folder-open",
                                    collapsedIcon = "fa fa-fw fa-folder",
                                    tipo = "area",
                                   
                                    children = new List<TreeWbs>()
                                    {
                                        new TreeWbs()
                                        {
                                            label = r.nombre_disciplina,
                                            data = r.AreaId + "," + r.DisciplinaId,
                                            expandedIcon = "fa fa-fw fa-folder-open",
                                            collapsedIcon = "fa fa-fw fa-folder",
                                            tipo = "disciplina",
                                          
                                            children = new List<TreeWbs>()
                                        }

                                    }

                                };
                                Lista.Add(elemento);
                            }
                        }
                        else
                        {
                            var index = this.buscarExistenciaItem(Lista, r.nombre_area);
                            if (index < 0)
                            {
                                var elemento = new TreeWbs()
                                {
                                    label = r.nombre_area,
                                    data = r.AreaId + "",
                                    expandedIcon = "fa fa-fw fa-folder-open",
                                    collapsedIcon = "fa fa-fw fa-folder",
                                    tipo = "area",
                                   
                                    children = new List<TreeWbs>()

                                };
                                Lista.Add(elemento);
                            }

                        }

                    }

                }
                else
                {

                    //desdeaqui...

                    if (Lista.IsNullOrEmpty())
                    {

                        var elemento = new TreeWbs()
                        {
                            label = r.nombre_area,
                            data = r.AreaId + "",
                            expandedIcon = "fa fa-fw fa-folder-open",
                            collapsedIcon = "fa fa-fw fa-folder",
                            tipo = "area",
                           
                            children = new List<TreeWbs>()
                            {
                                new TreeWbs()
                                {
                                    label = r.nombre_disciplina,
                                    data = r.AreaId + "," + r.DisciplinaId,
                                    expandedIcon = "fa fa-fw fa-folder-open",
                                    collapsedIcon = "fa fa-fw fa-folder",
                                    tipo = "disciplina",
                                 
                                    children = new List<TreeWbs>()
                                    {
                                        new TreeWbs()
                                        {
                                            label = r.nombre_elemento,
                                            data = r.AreaId + "," + r.DisciplinaId + "," + r.ElementoId,
                                            expandedIcon = "fa fa-fw fa-folder-open",
                                            collapsedIcon = "fa fa-fw fa-folder",
                                            tipo = "elemento",
                                          
                                            children = new List<TreeWbs>()
                                            {
                                                new TreeWbs()
                                                {
                                                    label = r.nombre_actividad,
                                                    data = r.AreaId + "," + r.DisciplinaId + "," + r.ElementoId + "," +
                                                           r.ActividadId + "," + r.Id,
                                                            expandedIcon = "fa fa-fw fa-folder-open",
                                            collapsedIcon = "fa fa-fw fa-folder",
                                                    tipo = "actividad",
                                               
                                                    nombres = r.nombre_area + ", " + r.nombre_disciplina + ", " + r.nombre_elemento + ", " + r.nombre_actividad,
                                                   // children= _computoService.TreeComputo(r.Id) //Aumente computo

                                                }
                                            }

                                        }
                                    }
                                }

                            }

                        };
                        Lista.Add(elemento);
                    }
                    else
                    {
                        var index = this.buscarExistenciaItem(Lista, r.nombre_area);
                        if (index >= 0)
                        {
                            var indexDisciplina = buscarExistenciaItem(Lista[index].children, r.nombre_disciplina);
                            if (indexDisciplina >= 0)
                            {
                                var indexElemento = buscarExistenciaItem(
                                    Lista[index].children[indexDisciplina].children,
                                    r.nombre_elemento);
                                if (indexElemento >= 0)
                                {
                                    var elemento = new TreeWbs()
                                    {
                                        label = r.nombre_actividad,
                                        data = r.AreaId + "," + r.DisciplinaId + "," + r.ElementoId + "," +
                                               r.ActividadId + "," + r.Id,
                                        expandedIcon = "fa fa-fw fa-folder-open",
                                        collapsedIcon = "fa fa-fw fa-folder",
                                        tipo = "actividad",
                                       
                                        nombres = r.nombre_area + ", " + r.nombre_disciplina + ", " + r.nombre_elemento + ", " + r.nombre_actividad,
                                      //  children = _computoService.TreeComputo(r.Id) //Aumente computo
                                    };
                                    if (Lista[index].children[indexDisciplina].children[indexElemento].children == null)
                                    {
                                        Lista[index].children[indexDisciplina].children[indexElemento].children =
                                            new List<TreeWbs>();
                                    }

                                    Lista[index].children[indexDisciplina].children[indexElemento].children
                                        .Add(elemento);
                                }
                                else
                                {
                                    var elemento = new TreeWbs()
                                    {
                                        label = r.nombre_elemento,
                                        data = r.AreaId + "," + r.DisciplinaId + "," + r.ElementoId,
                                        expandedIcon = "fa fa-fw fa-folder-open",
                                        collapsedIcon = "fa fa-fw fa-folder",
                                        tipo = "elemento",
                                      
                                        children = new List<TreeWbs>()
                                        {
                                            new TreeWbs()
                                            {
                                            label = r.nombre_actividad,
                                            data = r.AreaId + "," + r.DisciplinaId + "," + r.ElementoId + "," + r.ActividadId + "," + r.Id,
                                           expandedIcon = "fa fa-fw fa-folder-open",
                                    collapsedIcon = "fa fa-fw fa-folder",
                                            tipo = "actividad",
                                              
                                            nombres = r.nombre_area + ", " + r.nombre_disciplina + ", " + r.nombre_elemento + ", " + r.nombre_actividad,
                                            //children= _computoService.TreeComputo(r.Id) //Aumente computo

                                            }

                                        }


                                    };
                                    Lista[index].children[indexDisciplina].children.Add(elemento);
                                }
                            }
                            else
                            {
                                var elemento = new TreeWbs()
                                {
                                    label = r.nombre_disciplina,
                                    data = r.AreaId + "," + r.DisciplinaId,
                                    expandedIcon = "fa fa-fw fa-folder-open",
                                    collapsedIcon = "fa fa-fw fa-folder",
                                    tipo = "disciplina",
                                  
                                    children = new List<TreeWbs>()
                                    {
                                        new TreeWbs()
                                        {
                                            label = r.nombre_elemento,
                                            data = r.AreaId + "," + r.DisciplinaId + "," + r.ElementoId,
                                            expandedIcon = "fa fa-fw fa-folder-open",
                                            collapsedIcon = "fa fa-fw fa-folder",
                                            tipo = "elemento",
                                          
                                            children = new List<TreeWbs>()
                                            {
                                                new TreeWbs()
                                                {
                                                label = r.nombre_actividad,
                                                data = r.AreaId + "," + r.DisciplinaId + "," + r.ElementoId + "," + r.ActividadId + "," + r.Id,
                                                   expandedIcon = "fa fa-fw fa-folder-open",
                                            collapsedIcon = "fa fa-fw fa-folder",
                                                tipo = "actividad",
                                             
                                                nombres = r.nombre_area + ", " + r.nombre_disciplina + ", " + r.nombre_elemento + ", " + r.nombre_actividad,
                                                // children= _computoService.TreeComputo(r.Id) //Aumente computo

                                                }

                                            }

                                        }
                                    }

                                };
                                Lista[index].children.Add(elemento);
                            }
                        }
                        else
                        {
                            var elemento = new TreeWbs()
                            {
                                label = r.nombre_area,
                                data = r.AreaId + "",
                                expandedIcon = "fa fa-fw fa-folder-open",
                                collapsedIcon = "fa fa-fw fa-folder",
                                tipo = "area",
                              
                                children = new List<TreeWbs>()
                                {
                                    new TreeWbs()
                                    {
                                        label = r.nombre_disciplina,
                                        data = r.AreaId + "," + r.DisciplinaId,
                                        expandedIcon = "fa fa-fw fa-folder-open",
                                        collapsedIcon = "fa fa-fw fa-folder",
                                        tipo = "disciplina",
                                     
                                        children = new List<TreeWbs>()
                                        {
                                            new TreeWbs()
                                            {
                                                label = r.nombre_elemento,
                                                data = r.AreaId + "," + r.DisciplinaId + "," + r.ElementoId,
                                                expandedIcon = "fa fa-fw fa-folder-open",
                                                collapsedIcon = "fa fa-fw fa-folder",
                                                tipo = "elemento",
                                                
                                                children = new List<TreeWbs>()
                                                {
                                                    new TreeWbs()
                                                    {
                                                    label = r.nombre_actividad,
                                                    data = r.AreaId + "," + r.DisciplinaId + "," + r.ElementoId + "," + r.ActividadId + "," + r.Id,
                                                      expandedIcon = "fa fa-fw fa-folder-open",
                                            collapsedIcon = "fa fa-fw fa-folder",
                                                    tipo = "actividad",
                                                    
                                                    nombres = r.nombre_area + ", " + r.nombre_disciplina + ", " + r.nombre_elemento + ", " + r.nombre_actividad,
                                                     //children= _computoService.TreeComputo(r.Id) //Aumente computo

                                                    } }

                                            }
                                        }
                                    }

                                }

                            };
                            Lista.Add(elemento);
                        }
                    }



                }



            }

            return Lista;
        
    }

        public WbsOfertaDto GetWbsOfertaIngenieria(int OfertaId)
        {
            var wbsofertasQuery = Repository.GetAllIncluding(c => c.Oferta, c => c.Oferta.Requerimiento,
                c => c.Oferta.Requerimiento.Proyecto, c => c.Oferta.Requerimiento.Proyecto.Contrato,
                c => c.Oferta.Requerimiento.Proyecto.Contrato.Cliente);
            var item = (from r in wbsofertasQuery
                where r.OfertaId == OfertaId
                where r.AreaId == 2
                where r.DisciplinaId == 4030
                where r.ElementoId == 4031
                where r.ActividadId == 4032
                where r.es_estructura ==false
                select new WbsOfertaDto()
                {
                    Id = r.Id,
                    OfertaId = r.OfertaId,
                    AreaId = r.AreaId,
                    DisciplinaId = r.DisciplinaId,
                    ElementoId = r.ElementoId,
                    ActividadId = r.ActividadId,
                    estado = r.estado,
                    observaciones = r.observaciones,
                    vigente = r.vigente,


                }).SingleOrDefault();
            return item;
        }
    }

}

