using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.aplicacion.Dto;
using com.cpp.calypso.proyecto.aplicacion.Interfaces;
using com.cpp.calypso.proyecto.dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion.Service
{
    public class ParroquiaAsyncBaseCrudAppService : AsyncBaseCrudAppService<Parroquia, ParroquiaDto, PagedAndFilteredResultRequestDto>, IParroquiaAsyncBaseCrudAppService
    {
        public ParroquiaAsyncBaseCrudAppService(
            IBaseRepository<Parroquia> repository
            ) : base(repository)
        {
        }

		public List<ParroquiaDto> ObtenerParroquiaPorCanton(int cantonId)
		{
			var parroquiaQuery = Repository.GetAll().Where(c=> c.CiudadId == cantonId && c.vigente == true).OrderBy(c => c.nombre);
			var parroquia = (from p in parroquiaQuery
						  select new ParroquiaDto()
						  {
							  Id = p.Id,
							  codigo = p.codigo,
							  nombre = p.nombre,
							  CiudadId = p.CiudadId,
							  vigente = p.vigente,
                              codigo_postal = p.codigo_postal
						  }).ToList();
			return parroquia;
		}

        public ParroquiaDto GetParroquia(int Id)
        {
            var p = Repository.Get(Id);

            ParroquiaDto parroquia = new ParroquiaDto()
            {
                Id = p.Id,
                codigo = p.codigo,
                nombre = p.nombre,
                CiudadId = p.CiudadId,
                Ciudad = p.Ciudad,
                vigente = p.vigente,
                codigo_postal = p.codigo_postal
            };


            return parroquia;
        }

        public List<ParroquiaDto> GetParroquias()
        {
            var parroquiaQuery = Repository.GetAll().Where(c => c.vigente);

            var parroquia = (from p in parroquiaQuery
                             select new ParroquiaDto()
                             {
                                 Id = p.Id,
                                 codigo = p.codigo,
                                 nombre = p.nombre,
                                 CiudadId = p.CiudadId,
                                 vigente = p.vigente,
                                 codigo_postal = p.codigo_postal
                             }).ToList();


            return parroquia;
        }
    }
}
