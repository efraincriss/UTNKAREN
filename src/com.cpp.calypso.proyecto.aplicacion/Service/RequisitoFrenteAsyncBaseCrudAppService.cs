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
    public class RequisitoFrenteAsyncBaseCrudAppService : AsyncBaseCrudAppService<RequisitoFrente, RequisitoFrenteDto, PagedAndFilteredResultRequestDto>, IRequisitoFrenteAsyncBaseCrudAppService
    {
        public RequisitoFrenteAsyncBaseCrudAppService(
            IBaseRepository<RequisitoFrente> repository
            ) : base(repository)
        {
        }

        public List<RequisitoFrenteDto> GetFrentesPorRequisito(int Id)
        {
            var query = Repository.GetAll().Where(c => c.RequisitoColaboradorId == Id);

            List<RequisitoFrenteDto> frentes = 
                              (from d in query
                              where d.vigente == true
                              select new RequisitoFrenteDto
                              {
                                  Id = d.Id,
                                  RequisitoColaboradorId = d.RequisitoColaboradorId,
                                  ZonaFrenteId = d.ZonaFrenteId
							  }).ToList();

            return frentes;
        }
        
    }
}
