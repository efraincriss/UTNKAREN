using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.proyecto.aplicacion.RecursosHumanos.Dto;
using com.cpp.calypso.proyecto.dominio.RecursosHumanos;
using OfficeOpenXml;

namespace com.cpp.calypso.proyecto.aplicacion.RecursosHumanos.Interfaces
{
    public interface IActualizacionSueldoAsyncBaseCrudAppService : IAsyncBaseCrudAppService<ActualizacionSueldo, ActualizacionSueldoDto, PagedAndFilteredResultRequestDto>
    {
        ExcelPackage DescargarPlantillaCargaMasivaDeJornales();

        ExcelPackage CargaMasivaDeSueldosJornales(HttpPostedFileBase uploadedFile, string observaciones, string fecha);

        List<ActualizacionSueldoDto> ObtenerTodasLasActualizacionesDeSaldos();

        List<DetalleActualizacionSueldoDto> ObtenerDetallesDeUnaActualizacion(int actualizacionId);

        ExcelPackage DescargarPlantillaActualizacionMasivaDeColaboradores();

        ExcelPackage CargaMasivaDeActualizacionColaboradores(HttpPostedFileBase uploadedFile);

        ExcelPackage CargaMasivaReingresoColaboradores(HttpPostedFileBase uploadedFile);

        ExcelPackage ActualizacionMasivaReingresoColaboradores(HttpPostedFileBase uploadedFile);
    }
}
