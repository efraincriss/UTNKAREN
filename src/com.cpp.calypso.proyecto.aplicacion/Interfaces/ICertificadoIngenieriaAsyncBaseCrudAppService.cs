using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.proyecto.aplicacion.Dto;
using com.cpp.calypso.proyecto.dominio;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion.Interfaces
{
    public interface ICertificadoIngenieriaAsyncBaseCrudAppService : IAsyncBaseCrudAppService<CertificadoIngenieria, CertificadoIngenieriaDto, PagedAndFilteredResultRequestDto>
    {
        List<CertificadoIngenieriaDto> ListAll(int Id);
        CertificadoIngenieriaDto GetDetalle(int Id);
        int GenerarCertificado(int Id, DateTime fechaCorte, DateTime fechaEmision);
        int DeleteCertificado(int id);
        int Anular(int id);
        int Aprobar(int id);
        ExcelPackage ObtenerCertificadoIngenieria(int Id);
    }
}
