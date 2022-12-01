using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.proyecto.dominio;

namespace com.cpp.calypso.proyecto.aplicacion
{
    public interface ITempOfertaAsyncBaseCrudAppService : IAsyncBaseCrudAppService<TempOferta, TempOfertaDto, PagedAndFilteredResultRequestDto>
    {
        void CargarOfertas();
        Task<string> CargarOfertas2Async(int desde, int hasta);
    }
}
