using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.aplicacion.Dto;
using com.cpp.calypso.proyecto.dominio;

namespace com.cpp.calypso.proyecto.aplicacion.Service
{
    public class CorreoExternoAsyncBaseCrudAppService : AsyncBaseCrudAppService<CorreoExterno, CorreoExternoDto, PagedAndFilteredResultRequestDto>, ICorreoExternoAsyncBaseCrudAppService
    {
        public CorreoExternoAsyncBaseCrudAppService(
            IBaseRepository<CorreoExterno> repository
        ) : base(repository)
        {
        }

        public List<CorreoExternoDto> listar()
        {
            var query = Repository.GetAll().Where(o => o.vigente == true);

            var correos = (from c in query
                select new CorreoExternoDto()
                {
                    Id = c.Id,
                    vigente = c.vigente,
                    correo = c.correo,
                    nombre = c.nombre,
                    institucion = c.institucion
                }).ToList();
            return correos;
        }
    }
}
