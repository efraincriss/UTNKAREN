using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.proyecto.aplicacion.CertificacionIngenieria.Dto;
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
    public interface IAvancePorcentajeProyectoAsyncBaseCrudAppService : IAsyncBaseCrudAppService<AvancePorcentajeProyecto, AvancePorcentajeProyectoDto, PagedAndFilteredResultRequestDto>

    {

        List<AvancePorcentajeProyectoDto> ObtenerDetalles(DateTime? FechaCarga);
        ExcelPackage CargaMasiva(HttpPostedFileBase uploadedFile);
        ExcelPackage DescargarPlantillaCargaMasiva();

        List<ModelClassReact> ObtenerProyectos();

        AvancePorcentajeProyectoDto ObtenerDato(int ProyectoId,DateTime fecha);

        bool CrearDetalle(AvancePorcentajeProyecto input);
        bool ActualizarDetalle(AvancePorcentajeProyecto input);
        string DeleteDetalle(int Id);

    }
}
