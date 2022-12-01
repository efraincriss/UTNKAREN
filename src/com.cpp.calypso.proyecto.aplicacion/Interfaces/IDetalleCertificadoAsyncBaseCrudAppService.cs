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
   public interface IDetalleCertificadoAsyncBaseCrudAppService : IAsyncBaseCrudAppService<DetalleCertificado, DetalleCertificadoDto, PagedAndFilteredResultRequestDto>
    {
        List<DetalleCertificadoDto> Listar(int CertificadoId, int tipo);
        DetalleCertificadoDto getdetalle(int Id);
        bool Eliminar(int Id);

        bool InsertarDetallesObra(List<DetalleAvanceObraDto> data,int CertificadoId);

        bool InsertarDetallesObraFast(int []data, int CertificadoId);


        bool InsertarDetallesIngenieria(List<DetalleAvanceIngenieriaDto> data, int CertificadoId);

        bool InsertarDetallesProcura(List<DetalleAvanceProcuraDto> data, int CertificadoId);
        List<DetalleCertificadoDto> ListarI(int CertificadoId);
        List<DetalleCertificadoDto> ListarP(int CertificadoId);
        decimal montocertificadototal(int CertificadoId, int tipo);

        bool actualizarmontoscertificados();
        bool actualizarmontoscertificadosCerficado(int certificadoId);


    }
}
