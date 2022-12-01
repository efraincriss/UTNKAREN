using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.proyecto.aplicacion.Dto;
using com.cpp.calypso.proyecto.dominio;
using com.cpp.calypso.proyecto.dominio.Models;
using OfficeOpenXml;

namespace com.cpp.calypso.proyecto.aplicacion
{
    public interface IPresupuestoAsyncBaseCrudAppService : IAsyncBaseCrudAppService<Presupuesto, PresupuestoDto, PagedAndFilteredResultRequestDto>
    {
        Task<int> CrearPresupuesto(PresupuestoDto presupuesto);

        PresupuestoDto DetallePresupuestoConEnumerable(int PresupuestoId);

        Task ActualizarPresupuesto(PresupuestoDto p);
        Task ActualizarPresupuestoEmail(PresupuestoDto p);
        Task<bool> AprobarPresupuesto(int PresupuestoId);

        Task<bool> DesaprobarPresupuesto(int PresupuestoId);

        List<PresupuestoDto> ListarPorRequerimiento(int RequerimientoId);

        string CrearNuevaVersion(PresupuestoDto pre);

        bool CambiarComputoCompleto(int PresupuestoId);

        PresupuestoDto ObtenerPresupuestoDefinitivo(int RequerimientoId);

        ExcelPackage GenerarExcelCarga(PresupuestoDto oferta,int nivel_maximo);

        string nombrecatalogo(int tipocatagoid);

        List<PresupuestoDto> ListarPresupuestosDefinitivosAprobados(int RequerimientoId);
  
   
        void CalcularMontosPresupuesto(int Id); //Id Presupuesto Id

        // Generacion Nuevo Excel  Rdo y Presupuesto

        ExcelPackage GenerarExcelCargaPresupuesto(PresupuestoDto oferta,int nivel_maximo,bool reporte);
        ExcelPackage GenerarExcelCargaPresupuestoRdo(PresupuestoDto oferta, int nivel_maximo, bool reporte);

        void EstadoCambioNullPresupuestos(int Id);


        PresupuestoMensaje ActualizarCostos(int Id);

        //Validar si esta generado computo

        bool estageneradopresupuesto(int Id);

        List<Presupuesto> ListaPresupuestoDefinitivos();

        Contrato ObtenerContratoFromPresupuesto(int Id);


        //EXCEL CARGA PRESUPUESTO VERSION 2



        List<String> ValidarNumerosNegativos(HttpPostedFileBase UploadedFile, int maximo_nivel, int PresupuestoId);
        string IngresarItemsProcuraExce(HttpPostedFileBase UploadedFile, int maximo_nivel, int PresupuestoId);
        string IngresarItemsSubContratosExce(HttpPostedFileBase UploadedFile, int maximo_nivel, int PresupuestoId);


        //VALIDACIÓN ITEMS PROCURA



        string ValidarItemsProcuraExcel(HttpPostedFileBase UploadedFile, int PresupuestoId,int maximo_nivel,List<Item>lista_procura);
        string ValidarItemsSubExcel(HttpPostedFileBase UploadedFile, int PresupuestoId, int maximo_nivel, List<Item> lista_procura);

        string IngresarItemsPresupuestoExcel(HttpPostedFileBase UploadedFile, int maximo_nivel, int PresupuestoId);


        //Presupuestos Liberados

        List<PresupuestoDto> ListaPresupuestosLiberados();

        List<RequerimientoDto> ListadoRequerimientosCola();



        //

        // NUEVO ARBOL WBS COMO SE CONSTRUYE

        List<WbsPresupuesto> EstructuraWbs(int PresupuestoId);
        List<WbsPresupuesto> ObtenerWbsHijos(int PresupuestoId, string codigo_padre, List<WbsPresupuesto> estructura);


        void EnviarMontosPresupuestoReq(int id);


        #region Oferta Economica Segundo Formato
        ExcelPackage MatrizPresupuestoSecondFormat(PresupuestoDto oferta, int nivel_maximo, bool reporte);

        decimal ObtenerPrecioUnitarioItem(int ItemId, int PresupuestoId);
        #endregion

        TotalesSegundoContrato TotalesSecondFormat(List<ComputoPresupuestoDto>computos);
        TotalesSegundoContrato TotalesSecondFormatPresupuesto(int Id);



        int GuardarArchivo(int PresupuestoId, HttpPostedFileBase UploadedFile);
        List<ArchivoPresupuestoDto> ListaArchivos(int PresupuestoId);
        int EditarArchivo(int Id, HttpPostedFileBase UploadedFile);
        int EliminarArchivo(int id);

        ArchivoPresupuesto DetalleArchivo(int id);


        string hrefoutlook(int id);

    }
}
