using com.cpp.calypso.comun.aplicacion;
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
    public interface IFacturaAsyncBaseCrudAppService : IAsyncBaseCrudAppService<Factura, FacturaDto, PagedAndFilteredResultRequestDto>
    {
        List<FacturaDto> GetFacturas();
        FacturaDto GetDetalle(int FacturaId);

        List<FacturaExcel> FiltrarExcel(List<FacturaExcel> Lista);
        List<FacturaExcel> FiltrarExcelNumeroFactura(List<FacturaExcel> Lista, string numerofactura);

        bool comprobarfactura(String numerofactura, DateTime fechadocumento);
        int comprobarcliente(String codigosapcliente);
        int comprobarempresa(String codigosapempresa);

        Facturas  BuscarFacturasDB(List<FacturaExcel> Lista);
        RetencionDB BuscarRetencionesAB(List<FacturaExcel> Lista,string referencia);
        Facturas BuscarCobrosDZ(List<FacturaExcel> Lista);


        List<FacturaExcel> CrearFacturas(Facturas Lista);

        List<FacturaExcel> CrearCobros(List<FacturaExcel> Lista);

        Cobro ObtenerCobro(string documento_compensacion);


        FacturasDB  RepartirFacturasCuenta(Facturas Lista);

        Factura AnularFactura(int Id);

        string ActualizarCobros(int Id);


        //NUEVOS CAMBIOS

      TiposFacturas CargarArchivosFacturas(HttpPostedFileBase UploadedFile);

        ExcelPackage ReporteFacturas();

    }
}
