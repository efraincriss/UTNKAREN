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
    public interface IColaboradorRubroIngenieriaAsyncBaseCrudAppService : IAsyncBaseCrudAppService<ColaboradorRubroIngenieria, ColaboradorRubroIngenieriaDto, PagedAndFilteredResultRequestDto>
    {

        Task<ResultadoColaboradorRubro> CrearColaboradorRubroAsync(ColaboradorRubroIngenieriaDto dto);

        List<ColaboradorRubroIngenieriaDto> ObtenerColaboresRubros();

        ResultadoColaboradorRubro EditarColaboradorRubroAsync(ColaboradorRubroIngenieriaDto dto);

        ResultadoColaboradorRubro Eliminar(int id);

        List<DetallePreciarioDto> GetPreciariosPorContrato(int contratoId);

        ExcelPackage CargaMasivaDeTarifas(HttpPostedFileBase uploadedFile);

        ExcelPackage DescargarPlantillaCargaMasivaTarifas(int contratoId);

        List<ColaboradorRubroIngenieriaDto> ObtenerColaboresRubrosConFiltros(DateTime? fechaInicio, DateTime? fechaFin);

        List<ItemDto> GetItems();

        ResponseColaboradorItemIngenieriaDto ComprobarExistenciaItemEnContrato(int itemId);
    }
}
