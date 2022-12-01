using com.cpp.calypso.comun.aplicacion;

using com.cpp.calypso.proyecto.dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace com.cpp.calypso.proyecto.aplicacion.Interfaces
{
    public interface IAdendaAsyncBaseCrudAppService : IAsyncBaseCrudAppService<Adenda, AdendaDto, PagedAndFilteredResultRequestDto>
    {
        AdendaDto GetDetalle(int AdendaId);
        AdendaDto EliminarVigencia(int AdendaId);

        bool comprobarfechaadenda(DateTime fa, DateTime fcontrato);

        int GuardarArchivo(int ContratoId, HttpPostedFileBase UploadedFile);
    }
}
