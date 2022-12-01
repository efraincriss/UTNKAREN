using Abp.Domain.Entities.Auditing;
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
    public class AdminRotacionAsyncBaseCrudAppService : AsyncBaseCrudAppService<AdminRotacion, AdminRotacionDto, PagedAndFilteredResultRequestDto>, IAdminRotacionAsyncBaseCrudAppService
	{

        private readonly ICatalogoAsyncBaseCrudAppService _catalogoRepository;

		public AdminRotacionAsyncBaseCrudAppService(
            IBaseRepository<AdminRotacion> repository,
            ICatalogoAsyncBaseCrudAppService catalogoRepository
			) : base(repository)
        {
            _catalogoRepository = catalogoRepository;

		}

        public List<AdminRotacionDto> GetList()
        {
			var i = 1;
            var query = Repository.GetAll();

            var rotaciones = (from d in query
                               where d.vigente == true
                               select new AdminRotacionDto
                               {
                                   Id = d.Id,
                                   codigo = d.codigo,
                                   nombre = d.nombre,
                                   dias_laborablesC = d.dias_laborablesC,
                                   dias_laborablesO = d.dias_laborablesO,
                                   dias_descanso = d.dias_descanso,
                                   vigente = d.vigente
                               }).ToList();

			foreach (var r in rotaciones)
			{
				r.nro = i++;

                if (r.vigente)
                {
                    r.nombre_estado = "Activo";
                }
                else
                {
                    r.nombre_estado = "Inactivo";
                }
            }

            return rotaciones;
        }

        public AdminRotacionDto GetRotacion(int Id)
        {
            var c = Repository.Get(Id);

            AdminRotacionDto rotacion = new AdminRotacionDto()
            {
                Id = c.Id,
                codigo = c.codigo,
                nombre = c.nombre,
                dias_laborablesC = c.dias_laborablesC,
                dias_laborablesO = c.dias_laborablesO,
                dias_descanso = c.dias_descanso,
                vigente = c.vigente
            };


            return rotacion;
        }

		public string UniqueCodigo(string codigo)
		{
			var c = Repository.GetAll().Where(d => d.codigo == codigo && d.vigente == true).FirstOrDefault();
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
