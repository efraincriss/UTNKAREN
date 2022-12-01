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
    public interface ICapacitacionAsyncBaseCrudAppService : IAsyncBaseCrudAppService<Capacitacion, CapacitacionDto, PagedAndFilteredResultRequestDto>
    {
        CapacitacionesColaboradorDto ObtenerCapacitacionesPorColaborador(int colaboradorId);

        bool CrearCapacitacion(CapacitacionDto capacitacion);

        bool ActualizarCapacitacion(CapacitacionDto capacitacion);

        bool EliminarCapacitacion(int capacitacionId);

        List<Dto.ColaboradorDto> BuscarColaboradores(string filtro, string estado);

        string DescargarCertificados(int[] colaboradoresId);

        ExcelPackage DescargarPlantillaCargaMasivaDeCapacitaciones();

        ExcelPackage CargarCapacitaciones(HttpPostedFileBase UploadedFile);

        CapacitacionesColaboradorDto ObtenerCapacitaciones(string filtroColaborador, string tipoCapacitacion, string nombreCapacitacion, string fechaDesde, string fechaHasta);

        CapacitacionesColaboradorDto ObtenerCatalogosDeCapacitaciones();

        ExcelPackage DescargarReporteDeCapacitaciones(int capacitacion, int tipoCapacitacion, DateTime? fechaDesde, DateTime? fechaHasta);
    }
}
