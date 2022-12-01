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
    public interface IColaboradorCertificacionIngenieriaAsyncBaseCrudAppService : IAsyncBaseCrudAppService<ColaboradorCertificacionIngenieria, ColaboradorCertificacionIngenieriaDto, PagedAndFilteredResultRequestDto>
    {
        List<ColaboradorCertificacionIngenieriaDto> GetParametrizacionPorColaboradorId(int colaboradorId);

        Task<ResultadoColaboradorRubro> CrearAsync(ColaboradorCertificacionIngenieriaDto dto);

        Task<ResultadoColaboradorRubro> ActualizarAsync(ColaboradorCertificacionIngenieriaDto dto);

        bool Eliminar(int id);


        List<ColaboradorListadoIngenieriaDto> GetColaboradores();

        CatalogosCertificacionIngenieriaDto ObtenerCatalogos();
    }
}
