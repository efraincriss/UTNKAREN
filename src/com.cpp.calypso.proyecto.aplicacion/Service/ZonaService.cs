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
    public class ZonaAsyncBaseCrudAppService :
        AsyncBaseCrudAppService<Zona, ZonaDto, PagedAndFilteredResultRequestDto>,
        IZonaAsyncBaseCrudAppService
    {
        private readonly IZonaFrenteAsyncBaseCrudAppService _zonaFrente;
        public ZonaAsyncBaseCrudAppService(
            IBaseRepository<Zona> repository,
            IZonaFrenteAsyncBaseCrudAppService zonaFrente
            ) : base(repository)
        {
            _zonaFrente = zonaFrente;
        }

        public List<ZonaDto> ObtenerTodos()
        {
            var ZonaQuery = Repository.GetAll();
            var items = (from a in ZonaQuery
                         where a.vigente == true
                         select new ZonaDto()
                         {
                             Id = a.Id,
                             codigo = a.codigo,
                             nombre = a.nombre,
                             descripcion = a.descripcion,
                             vigente = a.vigente
                         }).ToList();
            return items;
        }

        public List<TreeItem> GenerarArbolZonasFrente()
        {
            var zonas = Repository.GetAll();
            var hijos = new List<TreeItem>();

            foreach (var x in zonas.ToList())
            {
                var item = ExtraerHijos(x);
                hijos.Add(item);
            }
            return hijos;
        }

        public TreeItem ExtraerHijos(Zona zona)
        {
            List<ZonaFrenteDto> hijos = _zonaFrente.GetFrentesPorZona(zona.Id);
            if (hijos.Count > 0)
            {
                var listahijos = new List<TreeItem>();
                foreach (var h in hijos)
                {
                    TreeItem arbol = new TreeItem()
                    {
                        label = h.codigo + " " + h.nombre,
                        labelcompleto = "FRENTE: " + h.codigo + " " + h.nombre,
                        data = "" + h.codigo,
                        id = h.Id,
                        expandedIcon = "fa fa-map-o",
                        collapsedIcon = "fa fa-map",
                    };
                    listahijos.Add(arbol);
                }
                return new TreeItem()
                {
                    label = zona.codigo + " " + zona.nombre,
                    labelcompleto = "ZONA: " + zona.codigo + " " + zona.nombre,
                    data = "" + zona.codigo,
                    id = zona.Id,
                    expandedIcon = "fa fa-map-o",
                    collapsedIcon = "fa fa-map",
                    children = listahijos
                };
            }
            else
            {
                return new TreeItem()
                {
                    label = zona.codigo + " " + zona.nombre,
                    labelcompleto = "ZONA: " + zona.codigo + " " + zona.nombre,
                    id = zona.Id,
                    data = "" + zona.codigo,
                    icon = "fa fa-map-o",
                };
            }
        }

        public void DeleteZona(int Id)
        {
            var zona = Repository.Get(Id);
            zona.vigente = false;
            Repository.Update(zona);
        }

        public void UpdateZona(int Id, string nombre, string descripcion)
        {
            var zona = Repository.Get(Id);
            zona.nombre = nombre;
            zona.descripcion = descripcion;
            Repository.Update(zona);
        }

        public int CreateZona(string codigo, string nombre, string descripcion)
        {
            Zona z = new Zona()
            {
                codigo = codigo,
                nombre = nombre,
                descripcion = descripcion,
                vigente = true
            };
            return Repository.InsertAndGetId(z);
        }

    }
}
