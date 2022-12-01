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
	public class ColaboradoresComidaAsyncBaseCrudAppService : AsyncBaseCrudAppService<ColaboradoresComida, ColaboradoresComidaDto, PagedAndFilteredResultRequestDto>, IColaboradoresComidaAsyncBaseCrudAppService
	{
		public ColaboradoresComidaAsyncBaseCrudAppService(
			IBaseRepository<ColaboradoresComida> repository
			) : base(repository)
		{
		}

		public List<ColaboradoresComidaDto> GetComidas(int Id)
		{
			var query = Repository.GetAll().Where(c => c.ColaboradorServicioId == Id);

			List<ColaboradoresComidaDto> comidas =
							  (from d in query
							   select new ColaboradoresComidaDto
							   {
								   Id = d.Id,
                                   ColaboradorServicioId = d.ColaboradorServicioId,
								   tipo_alimentacion_id = d.tipo_alimentacion_id
                               }).ToList();

			return comidas;
		}
	}
}
