using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.aplicacion.Interfaces;
using com.cpp.calypso.proyecto.dominio;

namespace com.cpp.calypso.proyecto.aplicacion.Service
{
    public class TipoCatalogoAsyncBaseCrudAppService : AsyncBaseCrudAppService<TipoCatalogo, TipoCatalogoDto, PagedAndFilteredResultRequestDto>, ITipoCatalogoAsyncBaseCrudAppService
    {
        private readonly ICatalogoAsyncBaseCrudAppService _catalogoservice;
        private readonly  IBaseRepository<Usuario> _userrepository;
     
        public TipoCatalogoAsyncBaseCrudAppService(
            IBaseRepository<TipoCatalogo> repository,
            IBaseRepository<Usuario> userrepository,
        ICatalogoAsyncBaseCrudAppService catalogoservice
            ) : base(repository)
        {
            _catalogoservice = catalogoservice;
            _userrepository = userrepository;

        }

        public List<TipoCatalogoDto> ObtenerListaTipoCatalogos()
        {
            List<TipoCatalogoDto> catalogos_modulos = new List<TipoCatalogoDto>();
            var tipoCatalogoQuery = Repository.GetAllIncluding(c=>c.Modulo);

            string user = System.Web.HttpContext.Current.User.Identity.Name.ToString();

            var usuario = _userrepository.GetAllIncluding(c => c.Modulos).Where(c => c.Cuenta == user).FirstOrDefault();


            if (usuario != null && usuario.Id > 0) {
                foreach (var m in usuario.Modulos)
                {


                    var item = (from t in tipoCatalogoQuery
                                where t.editable == true
                                where t.vigente == true
                                where t.ModuloId!=null
                                where t.ModuloId==m.Id
                                select new TipoCatalogoDto()
                                {
                                    Id = t.Id,
                                    vigente = t.vigente,
                                    codigo = t.codigo,
                                    nombre = t.nombre,
                                    editable = t.editable,
                                    tipo_ordenamiento = t.tipo_ordenamiento
                                }).ToList();
                    catalogos_modulos.AddRange(item);

                }

            }

            return catalogos_modulos;
        }

        public List<TipoCatalogo> GetCatalogosPorCodigo(string[] codigo)
        {
            List<TipoCatalogo> tiposCatalogos = new List<TipoCatalogo>();
            foreach (var cod in codigo)
            {
                var tipo_catalogo = Repository.Single(c => c.codigo == cod && c.vigente == true);
                tiposCatalogos.Add(tipo_catalogo);
            }

            return tiposCatalogos;
        }

        public TipoCatalogo GetCatalogoPorCodigo(string codigo)
        {
            var tipo_catalogo = Repository.Single(c => c.codigo == codigo);

            //GetAll().Where(c => c.codigo == codigo && c.vigente == true);

            return tipo_catalogo;


        }

        public TipoCatalogo Detalles(int id)
        {
            var detalle = Repository.Get(id);
            return detalle;
        }
    }
}
