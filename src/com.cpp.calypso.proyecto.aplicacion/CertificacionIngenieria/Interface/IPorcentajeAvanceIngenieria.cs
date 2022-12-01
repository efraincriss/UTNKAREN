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
    public interface IPorcentajeAvanceIngenieriaAsyncBaseCrudAppService : IAsyncBaseCrudAppService<PorcentajeAvanceIngenieria, PorcentajeAvanceIngenieriaDto, PagedAndFilteredResultRequestDto>
    {

        List<PorcentajeAvanceIngenieriaDto> ObtenerAvancesIngenieriaPorFecha(DateTime fechaDesde);

        Task<ResultadoColaboradorRubro> ActualizarAsync(PorcentajeAvanceIngenieriaDto dto);

        Task<ResultadoColaboradorRubro> CrearAsync(PorcentajeAvanceIngenieriaDto dto);

        ResultadoColaboradorRubro Eliminar(int id);

        List<ProyectoIngenieriaDto> ObtenerProyectos();

        List<CatalogoDto> ObtenerCatalogos();
    }
}
