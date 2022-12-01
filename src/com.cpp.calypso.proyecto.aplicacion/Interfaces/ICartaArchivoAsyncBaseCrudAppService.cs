using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.proyecto.dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion.Interfaces
{
    public interface ICartaArchivoAsyncBaseCrudAppService : IAsyncBaseCrudAppService<CartaArchivo, CartaArchivoDto, PagedAndFilteredResultRequestDto>
    {
        List<CartaArchivoDto> ListaArchivosporCarta(int CartaId);

        CartaArchivoDto getdetalle(int CartaArchivoId);


    }
}
