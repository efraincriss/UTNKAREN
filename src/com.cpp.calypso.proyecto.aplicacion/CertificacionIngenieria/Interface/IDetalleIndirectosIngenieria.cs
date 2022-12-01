using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.proyecto.aplicacion.CertificacionIngenieria.Dto;
using com.cpp.calypso.proyecto.dominio.CertificacionIngenieria;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace com.cpp.calypso.proyecto.aplicacion.CertificacionIngenieria.Interface
{
    public interface IDetalleIndirectosIngenieriaAsyncBaseCrudAppService : IAsyncBaseCrudAppService<DetalleIndirectosIngenieria, DetalleIndirectosIngenieriaDto, PagedAndFilteredResultRequestDto>
    {

        ResultadoColaboradorRubro Eliminar(int id);

        Task<ResultadoColaboradorRubro> ActualizarAsync(DetalleIndirectosIngenieriaDto dto);

        Task<ResultadoColaboradorRubro> CrearIndirectoAsync(DetalleIndirectosIngenieriaDto dto);

        List<DetalleIndirectosIngenieriaDto> ObtenerIndirectosIngenieriaPorFechas(DateTime? fechaDesde, DateTime? fechaHasta);

        ResultadoColaboradorRubro CalcularDiasLaborados(int colaboradorId, DateTime fechaDesde, DateTime fechaHasta);

        ExcelPackage DescargarPlantillaCargaMasivaGastosIndirectos();

        Task<ExcelPackage> CargaMasivaDeGastosIndirectosAsync(HttpPostedFileBase uploadedFile);
    }
}
