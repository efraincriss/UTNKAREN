using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.proyecto.aplicacion.CertificacionIngenieria.Dto;
using com.cpp.calypso.proyecto.aplicacion.CertificacionIngenieria.Models;
using com.cpp.calypso.proyecto.dominio;
using com.cpp.calypso.proyecto.dominio.CertificacionIngenieria;
using com.cpp.calypso.proyecto.dominio.Models;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace com.cpp.calypso.proyecto.aplicacion.CertificacionIngenieria.Interface
{
    public interface IDetallesDirectosIngenieriaAsyncBaseCrudAppService : IAsyncBaseCrudAppService<DetallesDirectosIngenieria, DetallesDirectosIngenieriaDto, PagedAndFilteredResultRequestDto>
    {

        List<DetallesDirectosIngenieriaDto> ObtenerDetallesIngenieria(DateTime? FechaInicial, DateTime? FechaFinal);

        DetallesDirectos ObtenerDetallesDirectosIngenieria(DateTime? FechaInicial, DateTime? FechaFinal);


        ExcelPackage CargaMasivaDetallesIngenieria(HttpPostedFileBase uploadedFile);
        ExcelPackage DescargarPlantillaCargaMasivaDetallesIngenieria();

        CatalogosIngenieria CatalogosIngenieria();

        List<SimpleColaborador> ObtenerColaboradores(string search);
        List<ModelClassReact> ObtenerProyectos();

        bool CrearDetalle(DetallesDirectosIngenieria input);
        bool ActualizarDetalle(DetallesDirectosIngenieria input);
        string DeleteDetalle(int Id);

        bool ActualizarEstadoValidadoIngenieria(int Id);

        CargaTimesheetDto ObtenerUltimaCargaTimesheet();

        Task<bool> ValidarCargaTimesheetAsync(int cargaTimesheetId);

        int SecuencialCargaDirectos();

    }
}
