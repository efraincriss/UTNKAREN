using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.proyecto.aplicacion.Dto;
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
    public interface IOfertaComercialAsyncBaseCrudAppService : IAsyncBaseCrudAppService<OfertaComercial, OfertaComercialDto, PagedAndFilteredResultRequestDto>
    {

        void ActualizarVersion(int Id);
        List<OfertaComercialDto> Lista();
        List<OfertaComercialDto> ListaContrato(int Id);
        List<OfertaComercial> ListaVersiones(int OfertaPadreId);

        int CrearOfertaComercia(OfertaComercial oferta);

        int EditarOfertaComercial(OfertaComercial oferta);
        int CrearNuevaVersion(OfertaComercial oferta);

        OfertaComercial GetDetalles(int Id);
        OfertaComercial BuscarPadre(int OfertaPadreId);


        MontosTotalesOrdenesServicio monto_ordenes_servicio(int OfertaId);

        void CalcularMontosOfertaComercial(int Id); //Id Presupuesto Id

        bool CambiarEstadoOferta(int Id, int CatalogoId);

        List<ArchivosOfertaDto> ListaArchivos(int Id);//Id OferttaComercial

        bool GuardarArchivo(HttpPostedFileBase UploadedFile, int Id);


        //Transmital Repository

        List<TransmitalCabeceraDto> ListarTransmitals();
        TransmitalCabeceraDto ListarTransmitalsId(int Id);
        List<TransmitalCabeceraDto> ListarTransmitalsPorContrato(int id); //id ContratoID


        string GenerarWordOfertaComercial(int id);

        // Obtener Proyecto ligados a una Oferta

        string ProyectosLigadosOfertaComercial(int id, String[] proyectos);
        //

        int secuencialofertacomercial();
        //

        string enviarMensaje(int id);// int OfertaComercial Id

        Task EnviarMail(int id);

        /*ENVIAR OFERTA COMERCIAL*/

        Task<string> Send_Files_OfertaComercial(int Id, bool user_transmittal, string asunto = "", string body = "", string urltransmittal = "");
        Task<string> Send_Files_OfertaComercialList(int Id, bool user_transmittal, List<UserCorreos> list, string body = "");

        ExcelPackage ReporteAdicionales(ReportDto r);

        ExcelPackage ReporteDetalladosAdicionales(ReportDto r);
        ExcelPackage SeguimientoComercial(ReportDto r);

        Task<String> SendMailAdministracionContratosAsync();

        MontosItem ObtenerMontosRequerimientosOfertComercial(int OfertaComercialId);

        bool ActualizarMontoOfertaComercial(int Id, decimal monto_ofertado, decimal monto_total_os);

        string hrefoutlook(int id, string to = "", string cc = "", string subject = "");

        string hrefoutlookOrdenProceder(int id, string to = "", string cc = "", string subject = "");
        List<DatosAdicionales> GetDatosAdicionales(ReportDto r);

        /*Oferta Comercial 2020*/
        OfertaComercialData ObtenerDataOferta(int Id);
        bool ActualizarMontoAprobado();

        string ActualizarMontoAprobadoOferta(int id, decimal monto_aprobado);

        bool tienePresupuestosAdicionales(int id);

        List<Contrato> ObtenerContratos();

        string ActualizarMontoAprobadoSegunEstadoOferta();

        /*Orden Proceder*/
        int GuardarArchivoOrden(int OfertaComercialId, HttpPostedFileBase UploadedFile);
        List<ArchivoOrdenProcederDto> ListaArchivosOrden(int OfertaComercialId);
        int EditarArchivoOrdenProceder(int Id, HttpPostedFileBase UploadedFile);
        int EliminarArchivoOrdenProceder(int id);

        ArchivoOrdenProceder DetalleArchivo(int id);


        string Actualizarmonto_ofertado_migracion_actual(int id, decimal monto_ofertado_migracion_actual);
        string Actualizarmonto_so_aprobado_migracion_actual(int id, decimal monto_so_aprobado_migracion_actual);
        string Actualizarmonto_so_aprobado_migracion_anterior(int id, decimal monto_so_aprobado_migracion_anterior);

    }
}
