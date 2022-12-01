using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion
{
    public class ProvinciaAsyncBaseCrudAppService :
        AsyncBaseCrudAppService<Provincia, ProvinciaDto, PagedAndFilteredResultRequestDto>,
        IProvinciaAsyncBaseCrudAppService
    {
        public ProvinciaAsyncBaseCrudAppService(
            IBaseRepository<Provincia> repository
            ) : base(repository)
        {

        }

        public List<ProvinciaDto> ObtenerProvinciaPorPais(int paisId)
        {
            var provinciaQuery = Repository.GetAll().Where(c => c.PaisId == paisId && c.vigente == true).OrderBy(c => c.nombre);
            var provincia = (from p in provinciaQuery
                             select new ProvinciaDto()
                             {
                                 Id = p.Id,
                                 codigo = p.codigo,
                                 nombre = p.nombre,
                                 PaisId = p.PaisId,
                                 vigente = p.vigente,
								 region_amazonica = p.region_amazonica
                             }).ToList();
            return provincia;
        }

        public ProvinciaDto GetProvincia(int Id)
        {
            var p = Repository.Get(Id);

            ProvinciaDto provincia = new ProvinciaDto()
            {
                Id = p.Id,
                codigo = p.codigo,
                nombre = p.nombre,
                PaisId = p.PaisId,
                Pais = p.Pais,
                region_amazonica = p.region_amazonica,
                vigente = p.vigente
            };


            return provincia;
        }

        public List<ProvinciaDto> GetProvincias()
        {
            var provinciaQuery = Repository.GetAll().Where(c => c.vigente == true).OrderBy(c => c.nombre);
            var provincia = (from p in provinciaQuery
                             select new ProvinciaDto()
                             {
                                 Id = p.Id,
                                 codigo = p.codigo,
                                 nombre = p.nombre,
                                 PaisId = p.PaisId,
                                 vigente = p.vigente,
                                 region_amazonica = p.region_amazonica
                             }).ToList();
            return provincia;
        }

    }
}
