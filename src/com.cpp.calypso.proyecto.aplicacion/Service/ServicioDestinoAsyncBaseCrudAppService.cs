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
	public class ServicioDestinoAsyncBaseCrudAppService : AsyncBaseCrudAppService<ServicioDestino, ServicioDestinoDto, PagedAndFilteredResultRequestDto>, IServicioDestinoAsyncBaseCrudAppService
	{
		private readonly ICatalogoAsyncBaseCrudAppService _catalogoService;
		public ServicioDestinoAsyncBaseCrudAppService(
			IBaseRepository<ServicioDestino> repository,
			ICatalogoAsyncBaseCrudAppService catalogoService
			) : base(repository)
		{
			_catalogoService = catalogoService;
		}

		public List<ServicioDestinoDto> GetList()
		{
			var i = 1;
			var query = Repository.GetAll();

			var servicios = (from d in query
							  where d.vigente == true
							  select new ServicioDestinoDto
							  {
								  Id = d.Id,
								  destinoId = d.destinoId,
								  alojamiento = d.alojamiento,
								  lavanderia = d.lavanderia,
								  movilizacion = d.movilizacion,
								  alimentacion = d.alimentacion,
								  vigente = d.vigente
							  }).ToList();

			foreach (var e in servicios)
			{
				e.nro = i++;

				var cat_destino= _catalogoService.GetCatalogo(e.destinoId);
				e.destino = cat_destino.nombre;

				e.nombre_alimentacion= (e.alimentacion) ? "SI" : "NO";
				e.nombre_alojamiento = (e.alojamiento) ? "SI" : "NO";
				e.nombre_lavanderia = (e.lavanderia) ? "SI" : "NO";
				e.nombre_movilizacion= (e.movilizacion) ? "SI" : "NO";
			}

			return servicios;
		}

		public ServicioDestinoDto GetServicio(int Id)
		{
			var d = Repository.Get(Id);

			ServicioDestinoDto servicio = new ServicioDestinoDto()
			{
				Id = d.Id,
				destinoId = d.destinoId,
				alojamiento = d.alojamiento,
				lavanderia = d.lavanderia,
				movilizacion = d.movilizacion,
				alimentacion = d.alimentacion,
				vigente = d.vigente
			};
			
			return servicio;
		}

        public string UniqueServicio(int destino)
        {
            var servicio = Repository.GetAll().Where(d => d.destinoId == destino && d.vigente == true).FirstOrDefault();
            if (servicio != null)
            {
                return "SI";
            }
            else
            {
                return "NO";
            }
        }
    }
}
