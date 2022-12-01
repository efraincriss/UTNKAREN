using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.aplicacion.Dto;
using com.cpp.calypso.proyecto.aplicacion.Interfaces;
using com.cpp.calypso.proyecto.dominio;

namespace com.cpp.calypso.proyecto.aplicacion.Service
{
    public class SecuencialAsyncBaseCrudAppService : AsyncBaseCrudAppService<Secuencial, SecuencialDto, PagedAndFilteredResultRequestDto>, ISecuencialAsyncBaseCrudAppService
    {
        public SecuencialAsyncBaseCrudAppService(
            IBaseRepository<Secuencial> repository
        ) : base(repository)
        {
        }

        public int ObtenerIncrementarSecuencial(string nombre)
        {
            var query = Repository.GetAll();
            var sec = (from s in query
                where s.nombre == nombre
                select new SecuencialDto()
                {
                    Id = s.Id,
                    nombre = s.nombre,
                    secuencia = s.secuencia
                }).SingleOrDefault();

            var secuencia = sec.secuencia;

            sec.secuencia = secuencia + 1;
            Repository.UpdateAsync(MapToEntity(sec));
            return secuencia;
        }
    }
}
