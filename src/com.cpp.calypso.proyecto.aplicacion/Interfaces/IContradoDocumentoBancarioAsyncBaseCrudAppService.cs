using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.proyecto.aplicacion;
using com.cpp.calypso.proyecto.dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace com.cpp.calypso.proyecto.aplicacion.Interfaces
{
    public interface IContratoDocumentoBancarioAsyncBaseCrudAppService : IAsyncBaseCrudAppService<ContratoDocumentoBancario, ContratoDocumentoBancarioDto, PagedAndFilteredResultRequestDto>
    {
      ContratoDocumentoBancarioDto GetDetalle(int ContratoDocumentoBancarioId);
        ContratoDocumentoBancarioDto EliminarVigencia(int ContratoDocumentoBancarioId);

        bool comprobarfecha(DateTime fi, DateTime fv);

        int GuardarArchivo(int ContratoId, HttpPostedFileBase UploadedFile);

    }
}