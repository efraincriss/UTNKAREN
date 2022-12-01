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
    public interface IDetalleCertificadoProcuraAsyncBaseCrudAppService : IAsyncBaseCrudAppService<DetalleCertificadoProcura, DetalleCertificadoProcuraDto, PagedAndFilteredResultRequestDto>
    {
        List<DetalleCertificadoProcuraDto> Listar(int CertificadoId);
        DetalleCertificadoProcuraDto getdetalle(int Id);
        bool Eliminar(int Id);

        bool InsertarDetallesObra(List<DetalleAvanceObraDto> data, int CertificadoId);
        bool InsertarDetallesIngenieria(List<DetalleAvanceIngenieriaDto> data, int CertificadoId);

        bool InsertarDetallesProcura(List<DetalleAvanceProcuraDto> data, int CertificadoId);
        List<DetalleCertificadoProcuraDto> ListarI(int CertificadoId);
        List<DetalleCertificadoProcuraDto> ListarP(int CertificadoId);

    }
}