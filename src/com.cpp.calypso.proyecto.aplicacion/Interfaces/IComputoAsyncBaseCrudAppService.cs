using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.proyecto.dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using OfficeOpenXml;

namespace com.cpp.calypso.proyecto.aplicacion.Interfaces
{
    public interface IComputoAsyncBaseCrudAppService : IAsyncBaseCrudAppService<Computo, ComputoDto, PagedAndFilteredResultRequestDto>
    {
        bool ActualizarValorEac(int idComputo, decimal valorEac);
        Task<ComputoDto> GetDetalle(int ComputoId);

        List<Item> GetComputosporOfertaNovalidos(int OfertaId);
        List<ComputoDto> GetComputosporWbsOferta(int WbsOfertaId, DateTime? fecha = null);
        bool comprobarexistecomputo(int WbsOfertaId);
       
        List<ComputoDto> GetComputosPorOferta(int id);
        void CalcularPresupuesto(int ContratoId,DateTime FechaOferta);
          
        ComputoDto ActualizarprecioAjustado(int PreciarioId, ComputoDto seleccionado);
        String ActualizarCostoTotal(int ofertaid, int ContratoId, int PreciarioId,DateTime FechaOferta, Boolean Validado);
        string nombrecatalogo(int tipocatagoid);

        List<TreeWbsComputo> TreeComputo(int WbsOfertaId);

        Task<bool> EliminarVigencia(int ComputoId);
        bool EditarCantidadComputo(int id, decimal cantidad, decimal cantidad_eac);

        ComputoDto GetCabeceraApi(int id);
        List<ComputoDto> GrupoComputosporOferta(int OfertaId);
        List<ComputoDto> GetComputosPorOfertaList(int[] computos);

        decimal MontoPresupuestoIngenieria(int OfertaId);
        decimal MontoPresupuestoConstruccion(int OfertaId);
        decimal MontoPresupuestoProcura(int OfertaId);
        decimal sumacantidades(int OfertaId, int Itemid);
        IEnumerable<Item> GetItemDistintosComputos(int OfertaId);
       

        ExcelPackage GenerarExcelCabecera(OfertaDto oferta);

        ExcelPackage CargaMasiva(HttpPostedFileBase UploadedFile,int OfertaId);

        ExcelPackage GenerarExcelCarga(OfertaDto oferta, int nivel_maximo);

        int GetComputosporOfertaProcura(int OfertaId);

        bool comprobarexistenciaitem(int WbsOfertaId, int ItemId);

        Computo editarcomputoexiste(int WbsOfertaId, int ItemId);

        //Carga de EAC
        ExcelPackage GenerarExcelCargaEAC(OfertaDto oferta, int nivel_maximo);

        String CargaMasivaEAC(HttpPostedFileBase UploadedFile, int OfertaId,int maximo_nivel_Wbs);

        bool ActiveEsTemporal(int Id,bool data);
        Computo GetInfo(int Id);




        bool ChangeCantidadAjustada(int ComputoId,bool cantidadAjustada, string tipo);
    }
}
