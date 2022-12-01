using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.proyecto.aplicacion.Dto;
using com.cpp.calypso.proyecto.dominio;

namespace com.cpp.calypso.proyecto.aplicacion.Interfaces
{

    public interface IDetalleCertificadoIngenieriaAsyncBaseCrudAppService : IAsyncBaseCrudAppService<DetalleCertificadoIngenieria, DetalleCertificadoIngenieriaDto, PagedAndFilteredResultRequestDto>
    {
        List<DetalleCertificadoIngenieriaDto> Listar(int CertificadoId);
        DetalleCertificadoIngenieriaDto getdetalle(int Id);
        bool Eliminar(int Id);

        bool InsertarDetallesObra(List<DetalleAvanceObraDto> data, int CertificadoId);
        bool InsertarDetallesIngenieria(List<DetalleAvanceIngenieriaDto> data, int CertificadoId);

        bool InsertarDetallesProcura(List<DetalleAvanceProcuraDto> data, int CertificadoId);
        List<DetalleCertificadoIngenieriaDto> ListarI(int CertificadoId);
        List<DetalleCertificadoIngenieriaDto> ListarP(int CertificadoId);

    }
}
