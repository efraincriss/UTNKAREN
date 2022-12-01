using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.proyecto.aplicacion.CertificacionIngenieria.Dto;
using com.cpp.calypso.proyecto.aplicacion.CertificacionIngenieria.Models;
using com.cpp.calypso.proyecto.aplicacion.Dto;
using com.cpp.calypso.proyecto.dominio;
using com.cpp.calypso.proyecto.dominio.CertificacionIngenieria;
using com.cpp.calypso.proyecto.dominio.Models;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion.CertificacionIngenieria.Interface
{
    public interface IGrupoCertificadoIngenieriaAsyncBaseCrudAppService : IAsyncBaseCrudAppService<GrupoCertificadoIngenieria, GrupoCertificadoIngenieriaDto, PagedAndFilteredResultRequestDto>
    {


        bool GenerarCertificadosMasivos(DateTime? fechaInicio, DateTime? fechaFin, int ClienteId);
        /*Listado para seleccionar detalles de directos e indirectos a certificar*/
        DetallesSinCertificar GetDetallesSinCertificar(DateTime? fechaInicio, DateTime? fechaFin, int ClienteId);

        List<Proyecto> GetProyectosADistribuir(DateTime? fechaInicio, DateTime? fechaFin);


        /*Grupo Certificados*/
        List<GrupoCertificadoIngenieriaDto> GetCertificados(DateTime? fechaInicio, DateTime? fechaFin);
        List<ProyectosCertificacion> ObtenerProyectos();
        bool ActualizarCampoCertificacion(ProyectosCertificacion input);

        ResultadoCertificacion Crear(GrupoCertificadoIngenieria e, int[] Directos, int[] Indirectos, int[] E500);

        bool Actualizar(GrupoCertificadoIngenieria e);

        bool Eliminar(int id);

        bool AprobarGrupoCertificado(int id);

        List<Proyecto> ProyectosACertificar(int[] Directos);

        /*Certificados por Grupo*/

        List<CertificadoIngenieriaProyectoDto> CertificadosPorGrupo(int GrupoCertificadoId);

        List<ProyectoDistribucionModel> ObtenerParametrizacion(int GrupoCertificadoId);

        bool AprobarCertificado(int Id);

        ResultadoCertificacion UltimoCertificadiGenerado();



        /*Gastos Directos por Certificado*/

        List<GastoDirectoCertificadoDto> GastosDirectosCertificado(int CertificacionIngenieriaProyectoId);

        List<Cliente> GetListCliente();

        //   ExcelPackage ValidarCampos();

        string guardarProyectoNodistribucion(int id, string proyectos);


      //  ExcelPackage GrupoCertificados(int GrupoCertificadoId);

        ExcelPackage Resumen(ValidacionColaboradorRubro validacion);

        bool ResumenCertificacion(int GrupoCertificadoId, int ProyectoId);

        ExcelPackage CertificadoPorProyecto(int ProyectoId, ExcelPackage excel);

        string nombreExcelGrupoCertificado(int GrupoCertificadoId);


        RedistribucionTotales TotalesRedistribucion(int Id);

        ResultadoCertificacion validarFechasCertificado(DateTime fechaInicio, DateTime fechaFin, DateTime fechaCertificacion, int clienteId);

        List<TotalMensualCertificado> TotalMensualCertificadodelGrupo(List<CertificadoIngenieriaProyecto> certificados);





        /*Detalles Directos Ingenieria Dto Campo Migrado para Certificacion*/
        List<DetallesDirectosIngenieriaDto> DtoDetallesDirectosIncluidoTarifa(List<int> DirectosId);


        List<Item> RubrosDirectosCertificadosPorProyecto(int ProyectoId, DateTime fechaCertificado);
        List<Item> RubrosIndirectosCertificadosPorProyectoUIO(int ProyectoId, DateTime fechaCertificado);
        List<Item> RubrosIndirectosCertificadosPorProyectoOIT(int ProyectoId, DateTime fechaCertificado);
        ExcelPackage GrupoCertificadosCompleto(int GrupoCertificadoId);
        ExcelPackage GrupoCertificadosCompleto2(int GrupoCertificadoId);

        /*Generacion de Certificados En diferentes Transacciones*/
        int CrearGrupoCertificados(GrupoCertificadoIngenieria e,List<ProyectoDistribucionModel> distribucionProyectos);

        bool CrearCertificadosDirectos(int GrupoCertificadoId, int ProyectoId, int[] Directos);
        bool AñadirDistribucionIndirectos(int GrupoCertificadoId, int[] Indirectos, List<ProyectoDistribucionModel> distribucionProyectos);

        bool AñadirDistribucionE500(int GrupoCertificadoId, int[] E500, List<ProyectoDistribucionModel> distribucionProyectos);

        void ViaticosVersion2(int GrupoCertificadoId, List<ProyectoDistribucionModel> distribucionProyectos);
        void GenerarViaticos(int GrupoCertificadoId);
        void ActualizarCabeceras(int GrupoCertificadoId);

        string ActualizarProyectoCertificable(int id,bool certificable);


        void ActualizarCampoLocacionConUbicacionParametrizacion(DateTime fechaDesde, DateTime fechaHasta);

        DtoValoresRubro ObtenerValoresPresupuestoporCodigoItem(string codigo, int ContratoId,List<ComputoPresupuesto>computos);


        ValidacionColaboradorRubro ValidarRegistros(GrupoCertificadoIngenieria input, int[] Directos, int[] Indirectos, int[] E500);

        List<ProyectoDistribucionModel> ProyectoDistribuibles(int[] DirectosSeleccionadosId);
    }



}
