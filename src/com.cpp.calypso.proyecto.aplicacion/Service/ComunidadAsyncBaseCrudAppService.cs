using Abp.Application.Services.Dto;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.framework;
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
    public class ComunidadAsyncBaseCrudAppService : AsyncBaseCrudAppService<Comunidad, ComunidadDto, PagedAndFilteredResultRequestDto>, IComunidadAsyncBaseCrudAppService
    {
        public ComunidadAsyncBaseCrudAppService(
            IBaseRepository<Comunidad> repository
            ) : base(repository)
        {
        }

		public List<ComunidadDto> ObtenerComunidadPorParroquia(int parroquiaId)
		{
			var comunidadQuery = Repository.GetAll().Where(c => c.ParroquiaId == parroquiaId && c.vigente == true).OrderBy(c => c.nombre);
			var comunidad = (from p in comunidadQuery
							 select new ComunidadDto()
							 {
								 Id = p.Id,
								 codigo = p.codigo,
								 nombre = p.nombre,
								 ParroquiaId = p.ParroquiaId,
                                 Parroquia = p.Parroquia,
								 vigente = p.vigente

							 }).ToList();
			return comunidad;
		}

        public ComunidadDto GetComunidad(int Id)
        {
            var p = Repository.Get(Id);

            ComunidadDto comunidad = new ComunidadDto()
            {
                Id = p.Id,
                codigo = p.codigo,
                nombre = p.nombre,
                ParroquiaId = p.ParroquiaId,
                vigente = p.vigente

            };


            return comunidad;
        }
    }
}
