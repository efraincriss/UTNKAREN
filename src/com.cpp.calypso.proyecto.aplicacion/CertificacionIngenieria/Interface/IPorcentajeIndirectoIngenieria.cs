using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.proyecto.aplicacion.CertificacionIngenieria.Dto;
using com.cpp.calypso.proyecto.dominio.CertificacionIngenieria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion.CertificacionIngenieria.Interface
{
    public interface IPorcentajeIndirectoIngenieriaAsyncBaseCrudAppService : IAsyncBaseCrudAppService<PorcentajeIndirectoIngenieria, PorcentajeIndirectoIngenieriaDto, PagedAndFilteredResultRequestDto>
    {
        Task<ResultadoColaboradorRubro> ActualizarAsync(PorcentajeIndirectoIngenieriaDto dto);

        Task<ResultadoColaboradorRubro> CrearAsync(PorcentajeIndirectoIngenieriaDto dto);

        ResultadoColaboradorRubro Eliminar(int id);

        List<PorcentajeIndirectoIngenieriaDto> ObtenerPorcentajesDelDetalleIndirecto(int detalleIndirectoId);
    }
}
