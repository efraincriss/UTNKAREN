using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.proyecto.aplicacion.Dto;
using com.cpp.calypso.proyecto.dominio;
using com.cpp.calypso.proyecto.dominio.Models;
using com.cpp.calypso.proyecto.dominio.RecursosHumanos.Models;
using com.cpp.calypso.seguridad.aplicacion;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml;

namespace com.cpp.calypso.proyecto.aplicacion.Interfaces
{
    public interface IColaboradoresAsyncBaseCrudAppService : IAsyncBaseCrudAppService<Colaboradores, ColaboradoresDto, PagedAndFilteredResultRequestDto>
    {
        List<ColaboradoresDto> GetList();
        List<ColaboradoresDto> GetColaboradoresInfoCompleta();
        ColaboradoresDto GetColaboradorInfoCompleta(int Id);
        ColaboradoresDto GetColaborador(int Id);
		string GetLegajo();
		string UniqueIdentification(string nro);
		ColaboradoresDto GetColaboradorRequisito(int id);
        List<ColaboradoresDto> GetColaboradorBajas();
        Colaboradores GetColaboradorPorTipoIdentificacion(int? tipoIdentificacion, string numero);
        Colaboradores GetColaboradorPorTipoIdentificacionExcluidoExterno(int? tipoIdentificacion, string numero);

        Colaboradores GetColaboradorPorTipoIdentificacionExcluidoExternoActivacionMasiva(int? tipoIdentificacion, string numero);

        List<ColaboradoresDto> GetColaboradorPorIdentificacion(string numero);

        List<ColaboradoresDto> GetColaboradorPorFiltros(string numero, string nombres, int grupoPersonal);

        Task<string> GenerarExcelCarga(List<ColaboradoresDto> colaboradores, bool es_manual);

        string GenerarWord(int id,DateTime x,DateTime y);  //X=fecha desde , y fecha hasta
              
        /// <summary>
        /// Campusoft: Estos metodos deberia ser proporcionados por el Cliente
        /// </summary>
        /// <returns></returns>
        Task<List<ColaboradoresLookupDto>> GetLookupAll();

        List<ColaboradoresLookupDto> GetAnotadoresLookupAll();
        List<ColaboradoresLookupDto> GetTransportistasLookupAll();
               
        Task EnviarNotificacionQRAsync(string fechaCaducidad, string correo, string asunto, string Attach);

        string getParametroPorCodigo(string codigo);

        ColaboradoresDto GetInfoColaboradorWS(int tipoIdentificacion, string cedula, string huella_dactilar);
        Archivo GetArchivo(int id);

        List<string> ExcelCargaMasiva(HttpPostedFileBase UploadedFile);
        List<ColaboradoresDto> GetUsuariosExternos();
        ColaboradoresDto GetUsuarioExterno(int Id);
        string UniqueUsuarioExterno(string nro);

        List<ColaboradoresDto> GetListaResponsables(string nombre);
        List<ColaboradoresDto> ConsultaUsuarioExternoPorIdentificacion(int tipoIdentificacion, string numero);

        ColaboradoresDetallesDto Detalles(int colaboradorId);
        List<ColaboradorResponsabilidadDto> GetColaboradorUsuario(string numero);

        List<ColaboradoresDto> GetFiltrosBajas(string numero, string nombres, string estado);


        //GENERACION DE QR// 
        Dictionary<String,Object> GenerarQr(int id); // id= Colabador Id
        string GenerarQrCodigoSeguridad(int id);
        string GenerarQrExternos(int id);
        List<ColaboradoresDto> GetUusuarioFiltros(string numero, string nombres);
        string UniquePosicion(string posicion, int id);
        string UniqueCuentaBanco(string cuenta, int banco, int id);
        int colaboradortieneservicios_(int id);
        int colaboradortienereservas(int id);
        int UpdateValidacionCedula(int id);
        string ServiciosColaborado(int id);
        string colaboradortienereservasactivas(int id);
        List<Colaboradores> consultaFiltrosReporte(ColaboradorReporteDto colaborador);
        ExcelPackage reporteInformacionGeneral(ColaboradorReporteDto colaborador);
        ExcelPackage GenerarExcelAltaMasiva();
        bool VerificaIdentificacion(string identificacion);
        List<ColaboradoresDto> GetFiltrosColaboradoresTable(string numero, string nombres, string estado);

        //Cast WS to Object Registro Civil
        RegistroCivilDto ChangeResultXMLObject(XmlNode node);

        //Verificar Si tiene Principal
        bool VerificarHuellaPrincipalColaborador(int colaboradorid);


        bool ExisteColaborador(string NumeroIdentificacion);
        List<ColaboradoresDto> FiltrosColaboradoresEstado(string numero, string nombres, string estado);
        
        List<ColaboradoresDto> SearchColaboradores(string numero, string nombres, string estado);

        List<ColaboradoresDto> SearchAllColaboradores(string numero, string nombres, string estado);


        #region Colaborador Externo a Interno
        int SearchColaboradorExterno(string numero_identificacion);
        bool InactivarColaboradorExterno(int Id);

        string BuscarIdUnicoColaboradores(string numero_identificacion);
        #endregion

        List<ColaboradoresDto> SearchColaborador(string nro_identificacion, string nombres);

        ColaboradorModel SimpleDataColaborador(int Id);
        bool SimpleInsertServiceColaborador(ServiceModel c);

        Colaboradores existeColaboradorPrincipal(string numero_identificacion, bool externo);

        //Reingresos

        string ColaboradorReingresoAsync(int ColaboradorIdUltimo, ColaboradoresDto temp);


        ExcelPackage ReporteColaboradoresHistoricos();
        List<ReingresoModel> ListColaboradoresReingreso();

        bool ValidarPeriodoColaborador(int ColaboradorId, DateTime fechaActualizacion);
        bool ValidarPeriodoColaboradorReingreso(int ColaboradorId, DateTime fechaActualizacion);

        Task<bool> Desactivar(int id, string pass);
       bool ValidarPassFechaIngreso( string pass);
    }
}
