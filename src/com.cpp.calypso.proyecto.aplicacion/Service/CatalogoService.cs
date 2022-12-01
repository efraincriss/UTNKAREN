using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.aplicacion.Interfaces;
using com.cpp.calypso.proyecto.dominio;
using com.cpp.calypso.proyecto.dominio.Models;
using Newtonsoft.Json;

namespace com.cpp.calypso.proyecto.aplicacion.Service
{
    public class CatalogoAsyncBaseCrudAppService : AsyncBaseCrudAppService<Catalogo, CatalogoDto, PagedAndFilteredResultRequestDto>, ICatalogoAsyncBaseCrudAppService
    {
        public CatalogoAsyncBaseCrudAppService(
            IBaseRepository<Catalogo> repository
            ) : base(repository)
        {
        }
        #region Catalogos
        public Catalogo GetCatalogo(int IdCatalogo)
        {
            Catalogo catalogo = Repository.Get(IdCatalogo);
            if (catalogo != null && catalogo.Id > 0)
            {
                return catalogo;

            }
            else
            {
                
                return new Catalogo();
            }

        }

        public Catalogo GetCatalogoPorCodigo(string codigo)
        {
            Catalogo catalogo = Repository.Single(c => c.codigo == codigo);
            return catalogo;
        }

        public List<string> GetCatalogosPorCodigo(List<TipoCatalogo> tiposCatalogos)
        {
            var catalogos = new List<string>();
            foreach (var cod in tiposCatalogos)
            {
                List<CatalogoDto> cat = ListarCatalogos(cod.Id);
                var result = JsonConvert.SerializeObject(cat,
               Newtonsoft.Json.Formatting.None,
               new JsonSerializerSettings
               {
                   ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                   NullValueHandling = NullValueHandling.Ignore
               });
                catalogos.Add(result);

            }

            //List<string> SortedList = catalogos.OrderBy(o => o.).ToList();

            return catalogos;
        }

        public List<CatalogoDto> ListarCatalogos(int tipoCatalogoId)

        {

            var catalogoQuery = Repository.GetAll().Include(x => x.TipoCatalogo).Where(c => c.vigente).ToList();
            var item = (from c in catalogoQuery
                        where c.TipoCatalogoId == tipoCatalogoId
                        where c.vigente == true
                        select new CatalogoDto()
                        {
                            Id = c.Id,
                            vigente = c.vigente,
                            TipoCatalogoId = c.TipoCatalogoId,
                            codigo = c.codigo,
                            descripcion = c.descripcion,
                            nombre = c.nombre,
                            ordinal = c.ordinal,
                            predeterminado = c.predeterminado,
                            TipoCatalogo = c.TipoCatalogo
                        }).ToList();
            return item;
        }

        public List<CatalogoDto> ListarCatalogos(string code)
        {
            var catalogoQuery = Repository.GetAllIncluding(t => t.TipoCatalogo).OrderByDescending(r => r.predeterminado);
            var item = (from c in catalogoQuery
                        where c.TipoCatalogo.codigo.ToUpper() == code.ToUpper()
                        where c.vigente == true
                        select new CatalogoDto()
                        {
                            Id = c.Id,
                            vigente = c.vigente,
                            TipoCatalogoId = c.TipoCatalogoId,
                            codigo = c.codigo,
                            descripcion = c.descripcion,
                            nombre = c.nombre,
                            ordinal = c.ordinal,
                            predeterminado = c.predeterminado,
                            TipoCatalogo = c.TipoCatalogo
                        }).OrderBy(c=>c.ordinal).ToList();
            return item;
        }

        public int EliminarVigencia(int catalogoId)
        {
            var cat = Repository.Get(catalogoId);
            if (cat != null)
            {
                cat.vigente = false;
                Repository.Update(cat);
                return cat.TipoCatalogoId;
            }

            return 0;
        }

        public List<Catalogo> ListarTodosCatalogos()
        {
            List<Catalogo> nuevo = Repository.GetAllIncluding(c => c.TipoCatalogo).Where(c => c.vigente == true).ToList();

            return nuevo;
        }

        public List<CatalogoDto> ListarCatalogosporcodigo(string codigo)
        {
            var catalogoQuery = Repository.GetAllIncluding(t => t.TipoCatalogo).OrderByDescending(r => r.predeterminado);
            var item = (from c in catalogoQuery
                        where c.TipoCatalogo.codigo == codigo
                        where c.vigente == true
                        select new CatalogoDto()
                        {
                            Id = c.Id,
                            vigente = c.vigente,
                            TipoCatalogoId = c.TipoCatalogoId,
                            codigo = c.codigo,
                            descripcion = c.descripcion,
                            nombre = c.nombre,
                            ordinal = c.ordinal,
                            predeterminado = c.predeterminado,
                            TipoCatalogo = c.TipoCatalogo
                        }).ToList();

            if (item != null && item.Count > 0)
            {
                return item;
            }
            else
            {
                return new List<CatalogoDto>();
            }

        }

        public bool existecatalogo(string nombre)
        {
            var existe = Repository.GetAll()
                                   .Where(c => c.vigente)
                                   .Where(c => c.nombre.ToUpper() == nombre.ToUpper())
                                   .Where(c => c.TipoCatalogoId == 4)
                                   .FirstOrDefault();
            if (existe != null && existe.Id > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public string GetNamebyId(int id)
        {
            var name = Repository.GetAll()
                                   .Where(c => c.vigente)
                                   .Where(c => c.Id == id)
                                   .Select(c => c.nombre)
                                   .FirstOrDefault();
            return name != null ? name : "";

        }


        public List<Catalogo> ObtenerCatalogos(string code)
        {
            var lista = Repository.GetAll().Where(c => c.vigente).Where(c => c.TipoCatalogo.vigente)
                                                    .Where(c => c.TipoCatalogo.codigo == code).ToList();

            if (lista.Count > 0)
            {
                return lista;
            }
            else
            {
                return new List<Catalogo>();
            }

        }
        #endregion
        public List<CatalogoDto> APIObtenerCatalogos(string code)
        {
            var query = Repository.GetAll().Where(c => c.TipoCatalogo.vigente)
                                           .Where(c => c.TipoCatalogo.codigo == code)
                                           .Where(c => c.vigente)
                                           .OrderBy(c=>c.nombre);
            var lista = (from c in query
                         select new CatalogoDto()
                         {
                             Id = c.Id,
                             NombreTipoCatalogo = c.TipoCatalogo.nombre,
                             nombre = c.nombre,
                             codigo = c.codigo,
                             valor_texto = c.valor_texto
                         }).ToList();
            return lista;
        }

        public List<ModelClassReact> APIObtenerCatalogosReact(string code)
        {
            var query = Repository.GetAll().Where(c => c.TipoCatalogo.vigente)
                                             .Where(c => c.TipoCatalogo.codigo == code)
                                             .Where(c => c.vigente)
                                             .OrderBy(c => c.nombre);
            var lista = (from c in query
                         select new ModelClassReact()
                         {
                             dataKey = c.Id,
                             label=c.nombre,
                             value=c.Id
                         }).ToList();
            return lista;
        }
    }
}
