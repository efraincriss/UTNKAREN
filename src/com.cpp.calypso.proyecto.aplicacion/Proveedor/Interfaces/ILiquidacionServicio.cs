using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.proyecto.aplicacion.Acceso.Dto;
using com.cpp.calypso.proyecto.aplicacion.Proveedor.Dto;
using com.cpp.calypso.proyecto.dominio;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion.Proveedor.Interfaces
{
    public interface ILiquidacionServicioAsyncBaseCrudAppService : IAsyncBaseCrudAppService<LiquidacionServicio, LiquidacionServicioDto, PagedAndFilteredResultRequestDto>
    {
        List<FormatLiquidacionReserva> ListaReservasPendientesdeLiquidacion(InputLiquidacionDto input);
        List<FormatLiquidacionReserva> ListaReservasLiquidadas(InputLiquidacionDto input);

        List<FormatLiquidacionSolicitudVianda> ListaSolicitudesViandasPendientesdeLiquidacion(InputLiquidacionDto input);
        List<FormatLiquidacionSolicitudVianda> ListaSolicitudesViandasLiquidadas(InputLiquidacionDto input);

        List<FormatLiquidacionConsumo> ListaConsumosPendientesdeLiquidacion(InputLiquidacionDto input);
        List<FormatLiquidacionConsumo> ListaConsumosLiquidadas(InputLiquidacionDto input);
        List<LiquidacionServicioDto> ListadoLiquidaciones();


        // Secuencial
        string Nextcode();
        // Contrato Proveedor
        int ObtenerContratoProveedor(int ProveedorId);
        decimal ObtenerTarifaHotel(int ContratoProveedorId, int TipoHabitacionId);
       LiquidacionServicioDto GetDetalles(int id);

        #region Liquidación Hospedaje 
        string GenerarLiquidacionHospedaje(int ContratoProveedorId, InputLiquidacionDto input, List<FormatLiquidacionReserva> pendientes);
        string AgregarLiquidacionHospedaje(int LiquidacionId,List<FormatLiquidacionReserva> pendientes);
        string RemoverLiquidacionHospedaje(int LiquidacionId, List<FormatLiquidacionReserva> pendientes);

        #endregion

        #region Liquidación Consumo 
        string GenerarLiquidacionConsumo(int ContratoProveedorId, InputLiquidacionDto input, List<FormatLiquidacionConsumo> pendientes);


        string AgregarLiquidacionConsumo(int LiquidacionId, List<FormatLiquidacionConsumo> pendientes);
        string RemoverLiquidacionConsumo(int LiquidacionId, List<FormatLiquidacionConsumo> pendientes);
        #endregion

        #region Liquidación Solicitud Viandas 
        string GenerarLiquidacionVianda(int ContratoProveedorId,InputLiquidacionDto input, List<FormatLiquidacionSolicitudVianda> pendientes);


        string AgregarLiquidacionVianda(int LiquidacionId, List<FormatLiquidacionSolicitudVianda> pendientes);
        string RemoverLiquidacionVianda(int LiquidacionId, List<FormatLiquidacionSolicitudVianda> pendientes);
        #endregion

        bool ChangeEstadoPagadoLiquidacion(int Id);
        bool ChangeEliminado(int Id);


        ExcelPackage ObtenerExcelHospedaje(int Id);
        ExcelPackage ObtenerExcelAlimentacion(int Id);
        ExcelPackage ObtenerExcelViandas(int Id);
    }
}
