using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.aplicacion.Interfaces;
using com.cpp.calypso.proyecto.dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion
{
    public class ZonaProvinciaAsyncBaseCrudAppService :
        AsyncBaseCrudAppService<ZonaProvincia, ZonaProvinciaDto, PagedAndFilteredResultRequestDto>,
        IZonaProvinciaAsyncBaseCrudAppService
    {
        public ZonaProvinciaAsyncBaseCrudAppService(
            IBaseRepository<ZonaProvincia> repository
            ) : base(repository)
        {
        }

        public void CreateMany(int zonaId, string[]provincias)
        {
            foreach (var i in provincias)
            {
                ZonaProvincia p = new ZonaProvincia()
                {
                    ZonaId = zonaId,
                    ProvinciaId = Int32.Parse(i)
                };
                Repository.Insert(p);
            }
        }
    }
}
