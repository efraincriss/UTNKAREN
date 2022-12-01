using System.Collections.Generic;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.proyecto.aplicacion.Dto;
using com.cpp.calypso.proyecto.dominio;
using System.Threading.Tasks;
using OfficeOpenXml;
using System;
using com.cpp.calypso.proyecto.aplicacion.Proveedor.Models;

namespace com.cpp.calypso.proyecto.aplicacion.Interfaces
{
    public interface IProveedorAsyncBaseCrudAppService : 
        IAsyncBaseCrudAppService<dominio.Proveedor.Proveedor, ProveedorDto, PagedAndFilteredResultRequestDto>
    {
        /// <summary>
        /// Obtener detalle de un proveedor
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ProveedorDetalleDto> GetDetalle(int id);

        /// <summary>
        /// Obtener un proveedor simplificado (Info)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ProveedorInfoDto> GetInfo(int id);
 
        /// <summary>
        /// Activar o Desactivar proveedor
        /// </summary>
        /// <param name="id"></param>
        /// <param name="opcionActivar">True: Activar. False: Desactivar</param>
        /// <returns></returns>
        Task<bool> Activar(int id,bool opcionActivar,string pass);



        #region Proveedor / Hospedaje
        List<ProveedorDto> ListProveedorHospedaje();


        #endregion

        #region Proveedor / Transporte

        List<ProveedorDto> ListProveedorTransporte();

        #endregion

        #region Proveedor / Alimentacion
        List<ProveedorDto> ListProveedorAlimentacion();

        List<Zona> Zonas();
        #endregion
        List<ProveedorDto> ListProveedorLiquidacionServicios();

        int EditarProveedor(ProveedorDto p);

        bool ExisteProveedor(string NumeroIdentificacion);

        //VALIDACIÓN DE PROVEEDOR

        bool ValidarEmailUnico(string email);
        string ProvedorEmailUnico(string email);
        bool ValidarEmailUnicoEdit(string email,int Id);
        string ProvedorEmailUnicoEdit(string email, int Id);

        ExcelPackage ReporteDiarioVianda(int proveedorId, DateTime fecha);
        ExcelPackage ReporteDiarioViandaMensula(int proveedorIdM, DateTime fechaInicio, DateTime fechaFin);

        ExcelPackage ReporteDiarioConsumo(List<int> Ids, DateTime fecha);
        ExcelPackage ReporteDiarioConsumoMensual (List<int> Ids, DateTime fechaInicio, DateTime fechaFin);

        List<int> ProveedoresConsolidadosporZona(List<int> ZonaId);

        List<int> ProveedoresConsolidadosporZonaHospedaje(List<int> ZonaId);
        ExcelPackage ConsumoMensualConsolidado(List<int> Ids, DateTime fechaInicio, DateTime fechaFin, List<int> ZonaId);







        ExcelPackage ReporteDiarioConsumoDuplicado(List<int> Ids, DateTime fechaInicio, DateTime fechaFin);

        ExcelPackage ReporteDuplicados(List<int> Ids,DateTime fechaInicio, DateTime fechaFin);

        ExcelPackage ReporteDiarioHospedaje(List<int> Ids, DateTime fecha);
        ExcelPackage ReporteHospedajeMensual(List<int> Ids, DateTime fechaInicio, DateTime fechaFin);


        ExcelPackage HospedajeMensualConsolidado(List<int> Ids, DateTime fechaInicio, DateTime fechaFin, List<int> ZonaId);

        bool ValidarDuplicadosConsumo();

        bool ActualizarCamposNuevos();

        ExcelPackage ReporteUsuarios();
        ExcelPackage ReporteUsuariosSincronizacion();
        ExcelPackage AplicaCedulaColaboradores();
        List<Catalogo> TipoOpcionComida();


        List<Zona> GetZonas();
        List<TipoOpcionComidaHorario> GetList(int ZonaId);

        string ActualizarHorarios(int TipoComidaId, TimeSpan HoraInicio, TimeSpan HoraFin, int zonaId);


        ExcelPackage ReporteHospedajeSerge(List<int> Ids, DateTime fechaInicio, DateTime fechaFin);

        ExcelPackage ReporteHospedajeFinalizados(List<int> Ids, DateTime fechaInicio, DateTime fechaFin);


        ExcelPackage ReporteVencimientoContratosProveedor();


        ExcelPackage ReporteUsuariosSincronizacionTiempo(DateTime fechaInicio, DateTime fechaFin, List<int>proveedorIds);
    }
}
