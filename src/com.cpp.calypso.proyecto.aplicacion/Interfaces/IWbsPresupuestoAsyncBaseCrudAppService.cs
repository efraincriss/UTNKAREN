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
    public interface IWbsPresupuestoAsyncBaseCrudAppService : IAsyncBaseCrudAppService<WbsPresupuesto, WbsPresupuestoDto, PagedAndFilteredResultRequestDto>
    {
        List<TreeWbs> GenerarArbol(int PresupuestoId);

        List<WbsPresupuestoDto> ObtenerKeysArbol(int OfertaId);

        List<JerarquiaWbs> GenerarDiagrama(int ofertaId);

        List<TreeWbs> GenerarArbolDrag(int PresupuestoId);

        string GuardarArbolDrag(List<TreeWbs> data);

        int ContarComputosPorWbs(int WbsId);

        WbsPresupuestoDto CrearPadre(WbsPresupuestoDto wbs);

        bool EliminarNivel(int WbsId);

        void Editar(int WbsId, string nombre);

        void CrearActividades(WbsPresupuestoDto wbs, string[] ActividadesIds);

        List<TreeWbsComputo> GenerarArbolComputo(int PresupuestoId);

        List<WbsPresupuestoDto> Listar(int ofertaId);

        TreeWbs CopiarNodosAWbs(int WbsPresupuestoId, Wbs wbsOferta);

        TreeWbs CopiarNodosAWbsPresupuesto(int WbsPresupuestoId, WbsPresupuesto wbspresupuesto);

        //
        //carga fechas y excel
        WbsPresupuestoDto ObtenerPadre(string id_nivel_padre_codigo, int presupuesoId);
        List<WbsPresupuestoDto> Jerarquiawbs(int id);
        WbsPresupuestoDto DatosWbs(int id);

        ExcelPackage GenerarExcelCargaFechas(PresupuestoDto oferta);

        List<WbsPresupuestoDto> ArbolWbsExcel(int OfertaId);

        int nivel_mas_alto(int Id);
        int contarnivel(int id,int presupuestoid);


        //COPIAR NODOS

        bool CopiarWBS(int presupuestoid, int wbsdestino, int wbsorigen);


        //ESTRUCTURA

        // NUEVO ARBOL WBS COMO SE CONSTRUYE

        List<WbsPresupuesto> EstructuraWbs(int PresupuestoId);
        List<WbsPresupuesto> ObtenerWbsHijos(int PresupuestoId, string codigo_padre, List<WbsPresupuesto> estructura);


        string VerficarExcelFechas(HttpPostedFileBase UploadedFile);


    }
}
