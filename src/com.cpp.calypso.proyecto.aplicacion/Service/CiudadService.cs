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
    public class CiudadAsyncBaseCrudAppService :
           AsyncBaseCrudAppService<Ciudad, CiudadDto, PagedAndFilteredResultRequestDto>,
           ICiudadAsyncBaseCrudAppService
    {
        public CiudadAsyncBaseCrudAppService(
            IBaseRepository<Ciudad> repository
            ) : base(repository)
        {
        }

        public List<CiudadDto> ObtenerCantonPorProvincia(int provId)
        {
            var ciudadQuery = Repository.GetAll().Where(c => c.ProvinciaId == provId && c.vigente == true).OrderBy(c => c.nombre);
            var ciudad = (from p in ciudadQuery
                          select new CiudadDto()
                          {
                              Id = p.Id,
                              codigo = p.codigo,
                              nombre = p.nombre,
                              ProvinciaId = p.ProvinciaId,
                              vigente = p.vigente

                          }).ToList();
            return ciudad;
        }

        public CiudadDto GetCiudad(int Id)
        {
            var p = Repository.Get(Id);

            CiudadDto ciudad = new CiudadDto()
            {
                Id = p.Id,
                codigo = p.codigo,
                nombre = p.nombre,
                ProvinciaId = p.ProvinciaId,
                Provincia = p.Provincia,
                vigente = p.vigente

            };


            return ciudad;
        }

        public List<CiudadDto> GetCiudades()
        {
            var ciudadQuery = Repository.GetAll().Where(c => c.vigente == true).OrderBy(c => c.nombre);
            var ciudad = (from p in ciudadQuery
                          select new CiudadDto()
                          {
                              Id = p.Id,
                              codigo = p.codigo,
                              nombre = p.nombre,
                              ProvinciaId = p.ProvinciaId,
                              vigente = p.vigente

                          }).ToList();
            return ciudad;
        }

        public string ObtenerCodigoPostal(int id)
        {

            var ciudad = Repository.GetAll().Where(c => c.Id == id).Where(c => c.vigente).FirstOrDefault();

            if (ciudad != null && ciudad.Id > 0)
            {
                return ciudad.codigo;
            }
            return " ";

        }
    }
}