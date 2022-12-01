using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.proyecto.aplicacion.Dto;
using com.cpp.calypso.proyecto.dominio;
using com.cpp.calypso.proyecto.dominio.Models;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace com.cpp.calypso.proyecto.aplicacion.Interfaces
{
    public interface IRsoCabeceraAsyncBaseCrudAppService : IAsyncBaseCrudAppService<RsoCabecera, Dto.RsoCabeceraDto, PagedAndFilteredResultRequestDto>
    {
        List<RsoCabecera> GetRdoCabeceras(int ProyectoId);
        List<RsoCabeceraDto> GetRdoCabecerasTable(int ProyectoId);
        RsoCabecera GetDetalles(int RdoCabeceraId);

        ExcelPackage GenerarExcelRdo(int RdoCabeceraId, string TipoReporte);

        Task EmitirRdoAsync(int RdoCabeceraId);

        int CreateRdoCabecera(int ProyectoId, DateTime fecha,int Id);

        string CreateRsoLast(int ProyectoId, DateTime fecha, int Id);



        // Generación de la Curva Rdo

        ExcelPackage GenerarCurvaRDO(int ProyectoId, int RdoCabeceraId);


        List<RsoCurva> ActualizarCurva(int ProyectoId, int RdoCabeceraId);

        ExcelPackage MontoTotales(int Id, string TipoReporte); //Id Rdo Cabecera.

        List<RdoDatos> GetRdo(int RdoCabeceraId, string TipoReporte);
        List<RdoDatos> GetRdoAdicionales(int RdoCabeceraId, string TipoReporte);

        int EliminarRdoCabecera(int RdoCabeceraId);

        bool EliminarPeriodoRdoCabecera(int Id,DateTime fecha_registro);

        //
        List<ProyectoObservacionDto> ListarPorProyectoTipo(int ProyectoId, TipoComentario Tipo);

        decimal ObtenerCantidadAcumuladaAnterior(int computoId, DateTime fecha_reporte, int proyectoId);

        decimal ObtenerCantidadActual(int computoId, DateTime fecha_reporte, int proyectoId);

        TotalesDescuentoRdo ObtenerTotalesEspecialidad(List<RdoDatos> actividades, string col, decimal total);

        int nivel_mas_alto(int Id);
        int contarnivel(int id, int ofertaid);

        List<Contrato> GetContratos();
        List<Proyecto> GetProyectos(int ContratoId);

        ExcelPackage GenerarExcelCurva(ModelHistoricoCurva model);
        string ActualizarFechasHistoricos(HttpPostedFileBase UploadedFile);

        DateTime FechaMinimaAvanceObra(int ProyectoId);
    }
}