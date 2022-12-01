using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.framework;
using com.cpp.calypso.proyecto.aplicacion.Acceso.Dto;
using com.cpp.calypso.proyecto.dominio;
using com.cpp.calypso.proyecto.dominio.Accesos;
using com.cpp.calypso.seguridad.aplicacion;
using OfficeOpenXml;

namespace com.cpp.calypso.proyecto.aplicacion.Acceso.Interface
{
    public interface IConsultaPublicaAsyncBaseCrudAppService : IAsyncBaseCrudAppService<ConsultaPublica, ConsultaPublicaDto, PagedAndFilteredResultRequestDto>
    {
        List<ConsultaPublicaDto> BuscarPorIdentificacionNombre(string identificacion = "", string nombre = "");


        string GenerarWord(int consultaPublicaId);

        void SubirPdf(int consultaPublicaId, ArchivoDto archivo);

        ConsultaPublicaDto ExisteCandidato(string identificacion);



        bool EnviarCorreoElectronico(int id, string asunto, string cuerpo, bool IsBodyHtml, HttpPostedFileBase UploadedFile, string lista_distribucion_id);
        List<ListaDistribucion> ListaDistribucion(int Tipo = 0);


     

        byte[] ConsultarFotoUsuario();
        byte[] GuardarFotoUsuario(HttpPostedFileBase UploadedFile);


        //EJECUCION DE QUERY SERVICIOS
        string RealizarConsulta(string queryString);
        List<Object> RealizarMultiplesConsultas(string json);
        ExcelPackage Reporte(DateTime? desde, DateTime? hasta);

    }
}
