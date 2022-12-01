using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.proyecto.dominio;
using com.cpp.calypso.proyecto.dominio.Models;
using OfficeOpenXml;

namespace com.cpp.calypso.proyecto.aplicacion
{
   public interface IRdoCabeceraAsyncBaseCrudAppService: IAsyncBaseCrudAppService<RdoCabecera, RdoCabeceraDto, PagedAndFilteredResultRequestDto>
    {
        List<RdoCabecera> GetRdoCabeceras(int ProyectoId);
        List<RdoCabeceraDto> GetRdoCabecerasTable(int ProyectoId);
        RdoCabecera GetDetalles(int RdoCabeceraId);

        ExcelPackage GenerarExcelRdo(int RdoCabeceraId, string TipoReporte);

        Task EmitirRdoAsync(int RdoCabeceraId);

        int CreateRdoCabecera(int ProyectoId, DateTime fecha);

        // Generación de la Curva Rdo

        ExcelPackage GenerarCurvaRDO(int ProyectoId, int RdoCabeceraId);


        List<RdoCurva> ActualizarCurva(int ProyectoId, int RdoCabeceraId);

        ExcelPackage MontoTotales(int Id, string TipoReporte); //Id Rdo Cabecera.

        List<RdoDatos> GetRdo(int RdoCabeceraId, string TipoReporte);
        List<RdoDatos> GetRdoAdicionales(int RdoCabeceraId, string TipoReporte);

        int EliminarRdoCabecera(int RdoCabeceraId);
   


        //
        List<ProyectoObservacionDto> ListarPorProyectoTipo(int ProyectoId, TipoComentario Tipo);

        decimal ObtenerCantidadAcumuladaAnterior(int computoId, DateTime fecha_reporte, int proyectoId);

        decimal ObtenerCantidadActual(int computoId, DateTime fecha_reporte, int proyectoId);

        TotalesDescuentoRdo ObtenerTotalesEspecialidad(List<RdoDatos> actividades,string col,decimal total);

        int nivel_mas_alto(int Id);
        int contarnivel(int id, int ofertaid);

        List<Contrato> GetContratos();
        List<Proyecto> GetProyectos(int ContratoId);

        ExcelPackage GenerarExcelCurva(ModelHistoricoCurva model);
        string ActualizarFechasHistoricos(HttpPostedFileBase UploadedFile);

        DateTime FechaMinimaAvanceObra(int ProyectoId);


        ExcelPackage GenerarErroresCantidadesRDOS();
        ExcelPackage GenerarErroresCantidadesCero();
        bool ExisteRdoGeneradoSuperior(int ProyectoId,DateTime fechageneracion);
    }
}
