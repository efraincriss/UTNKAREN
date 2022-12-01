using System.Collections.Generic;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.proyecto.dominio;

namespace com.cpp.calypso.proyecto.aplicacion
{
    public interface IConsultaPublicaAsyncBaseCrudAppService : IAsyncBaseCrudAppService<ConsultaPublica, ConsultaPublicaDto, PagedAndFilteredResultRequestDto>
    {
        List<ConsultaPublicaDto> BuscarPorIdentificacionNombre(string identificacion = "", string nombre = "");


        string GenerarWord(int consultaPublicaId);

        void SubirPdf(int consultaPublicaId, ArchivoDto archivo);
    }
}
