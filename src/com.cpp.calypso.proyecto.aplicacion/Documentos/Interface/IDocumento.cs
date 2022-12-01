using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.proyecto.aplicacion.Documentos.Dto;
using com.cpp.calypso.proyecto.dominio.Documentos;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion.Documentos.Interface
{
    public interface IDocumentoAsyncBaseCrudAppService : IAsyncBaseCrudAppService<Documento, DocumentoDto, PagedAndFilteredResultRequestDto>
    {

        List<DocumentoDto> ObtenerDocumentosDeCarpeta(int carpetaId);

        Task<bool> CrearDocumentoAsync(DocumentoDto dto);

        bool ActualizarDocumento(DocumentoDto dto);

        ResultadoEliminacionResponse EliminarDocumento(int id);

        DocumentoDto ObtenerDocumento(int documentoId);

        List<DocumentoDto> ObtenerDocumentosTipoAnexoPorCarpeta(int carpetaId);

        List<Documento> ObtenerDocumentosporTipo(string TipoDocumento, int carpetaId);
        List<Documento> ObtenerDocumentosporCarpeta(int carpetaId);
        List<Seccion> ObtenerSeccionporCarpeta(int carpetaId);
        List<Seccion> ObtenerSeccionporDocumento(int DocumentoId);

        ExcelPackage ListadoDocumentos();
        ExcelPackage ListadoUsuariosAutorizados();

    }
}
