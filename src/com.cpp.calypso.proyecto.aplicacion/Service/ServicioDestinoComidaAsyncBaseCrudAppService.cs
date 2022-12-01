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
	public class ServicioDestinoComidaAsyncBaseCrudAppService : AsyncBaseCrudAppService<ServicioDestinoComida, ServicioDestinoComidaDto, PagedAndFilteredResultRequestDto>, IServicioDestinoComidaAsyncBaseCrudAppService
	{
		public ServicioDestinoComidaAsyncBaseCrudAppService(
			IBaseRepository<ServicioDestinoComida> repository
			) : base(repository)
		{
		}

		public List<ServicioDestinoComidaDto> GetComidas(int Id)
		{
			var query = Repository.GetAll().Where(c => c.ServicioDestinoId == Id);

			List<ServicioDestinoComidaDto> comidas =
							  (from d in query
							   where d.vigente == true
							   select new ServicioDestinoComidaDto
							   {
								   Id = d.Id,
								   ServicioDestinoId = d.ServicioDestinoId,
								   tipo_comida = d.tipo_comida
							   }).ToList();

			return comidas;
		}
	}
}
