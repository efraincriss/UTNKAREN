using com.cpp.calypso.comun.aplicacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion
{
    public interface IZonaProvinciaAsyncBaseCrudAppService : IAsyncBaseCrudAppService<ZonaProvincia, ZonaProvinciaDto, PagedAndFilteredResultRequestDto>
    {
        void CreateMany(int zonaId, string[] provincias);
    }
}
