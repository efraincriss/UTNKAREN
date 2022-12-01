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
    public class ServicioAsyncBaseCrudAppService : AsyncBaseCrudAppService<Servicio, ServicioDto, PagedAndFilteredResultRequestDto>, IServicioAsyncBaseCrudAppService
    {
        public ServicioAsyncBaseCrudAppService(
            IBaseRepository<Servicio> repository
            ) : base(repository)
        {
        }

		public List<ServicioDto> GetList()
		{
			var i = 1;
			var query = Repository.GetAll();

			var servicios = (from d in query
							  select new ServicioDto
							  {
								  Id = d.Id,
								  codigo = d.codigo,
								  nombre = d.nombre,
							  }).ToList();

			foreach (var r in servicios)
			{
				r.nro = i++;
			}

			return servicios;
		}

		public ServicioDto GetServicio(int Id)
		{
			var c = Repository.Get(Id);

			ServicioDto servicio = new ServicioDto()
			{
				Id = c.Id,
				codigo = c.codigo,
				nombre = c.nombre
			};


			return servicio;
		}

		public string UniqueCodigo(string codigo)
		{
			var c = Repository.GetAll().Where(d => d.codigo == codigo).FirstOrDefault();
			if (c != null)
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
