using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.proyecto.dominio;
using OfficeOpenXml;

namespace com.cpp.calypso.proyecto.aplicacion
{
    public interface IWbsAsyncBaseCrudAppService : IAsyncBaseCrudAppService<Wbs, WbsDto, PagedAndFilteredResultRequestDto>
    {
        List<WbsDto> Listar(int ofertaId);
        List<TreeWbs> GenerarArbol(int ofertaId);

        TreeWbs GenerarNodos(Wbs wbs);
        WbsDto CrearPadre(WbsDto wbs);

        void CrearActividades(WbsDto wbs, string[] ActividadesIds);

        OfertaDto GetClienteProyectoFecha(int ofertaId);

        List<JerarquiaWbs> GenerarDiagrama(int ofertaId);

        bool EliminarNivel(int WbsId);

        int ContarComputosPorWbs(int WbsId);

        void Editar(int WbsId, string nombre);

        bool ClonarWbs(int OfertaDestinoId, int OfertaOrigen);

        //computos recursivo
        List<TreeWbsComputo> GenerarArbolComputo(int ofertaId);
        TreeWbsComputo GenerarNodosComputo(Wbs wbs);
        List<WbsDto> GetWbsOfertas(int OfertaId);
        List<TreeWbs> GenerarArbolComputosTemporal(int ofertaId);

        TreeWbs GenerarNodosComputosTemporal(Wbs wbs);


        //carga fechas y excel
        WbsDto ObtenerPadre(string id_nivel_padre_codigo,int OfertaId);
        List<WbsDto> Jerarquiawbs(int id);
        WbsDto DatosWbs(int id);

        ExcelPackage GenerarExcelCargaFechas(OfertaDto oferta);

        List<WbsDto> ArbolWbsExcel(int OfertaId);

        // Keys Arbols
        List<WbsDto> ObtenerKeysArbol(int OfertaId);

        List<TreeWbs> GenerarArbolDrag(int ofertaId);

        TreeWbs GenerarNodosDrag(Wbs wbs);//, int? indice,string orden="");

        String GuardarArbolDrag(List<TreeWbs> data);
        String GuardarHijoDrag(List<TreeWbs> data, string padre);

        bool CopiarWBS(int OfertaId, int PresupuestoId, int WbsId, int WbsPresupuestoId);

        int nivel_mas_alto(int Id);
        
        int contarnivel(int id,int ofertaid);

        // NUEVO ARBOL WBS COMO SE CONSTRUYE

        List<Wbs> EstructuraWbs(int OfertaId);
        List<Wbs> ObtenerWbsHijos(int OfertaId,string codigo_padre, List<Wbs> estructura);

        string VerficarExcelFechas(HttpPostedFileBase UploadedFile);
    }
}
