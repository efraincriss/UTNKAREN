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
    public class PaisAsyncBaseCrudAppService :
        AsyncBaseCrudAppService<Pais, PaisDto, PagedAndFilteredResultRequestDto>,
        IPaisAsyncBaseCrudAppService
    {
        public PaisAsyncBaseCrudAppService(
            IBaseRepository<Pais> repository
            ) : base(repository)
        {
        }

        public List<PaisDto> GetPaises()
        {
            var paisesQuery = Repository.GetAll().OrderBy(c => c.nombre);
            var paises = (from pais in paisesQuery
                          select new PaisDto
                          {
                              Id = pais.Id,
                              codigo = pais.codigo,
                              nombre = pais.nombre,
                              vigente = pais.vigente
                          }
            ).ToList();
            return paises;
        }

        public PaisDto GetPais(int Id)
        {
            var p = Repository.Get(Id);

            PaisDto pais = new PaisDto()
            {
                Id = p.Id,
                codigo = p.codigo,
                nombre = p.nombre,
                vigente = p.vigente

            };


            return pais;
        }
    }
}
