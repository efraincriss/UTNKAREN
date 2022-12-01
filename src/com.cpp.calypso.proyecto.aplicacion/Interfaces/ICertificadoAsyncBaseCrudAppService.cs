using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.proyecto.aplicacion.Dto;
using com.cpp.calypso.proyecto.dominio;
using com.cpp.calypso.proyecto.dominio.Models;
using OfficeOpenXml;

namespace com.cpp.calypso.proyecto.aplicacion.Interfaces
{
    public interface ICertificadoAsyncBaseCrudAppService : IAsyncBaseCrudAppService<Certificado, CertificadoDto, PagedAndFilteredResultRequestDto>
    {
        List<CertificadoDto> Listar();
        CertificadoDto getdetalle(int Id);
        bool Eliminar(int Id);

        List<CertificadoDto> GetCretificadosGr(int ProyectoId);
        decimal MontoPresupuestoIngenieria(int ProyectoId);
        decimal MontoPresupuestoConstruccion(int ProyecotoId);
        decimal MontoPresupuestoProcura(int ProyectoId);
        decimal MontoPresupuestoSubcontratos(int ProyectoId);
        decimal MontoCertificadoIngenieria(int ProyectoId);
        decimal MontoCertificadoConstruccion(int ProyectoId);
        decimal MontoCertificadoProcura(int ProyectoId);

        bool cambiarestadocertificado(int id);
        bool cancelarestadocertificado(int id);

        ExcelPackage GenerarExcelCertificado(int Id, int proyectoid); // Id para Pasar parametros a la Cabecera
        List<Contrato> GetListContratos();
        List<Proyecto> GetListProyecto(int ContratoId);
        Certificado GetDetalleCertificado(int Id);
        bool desaprobar(int id, string pass);
        MontosCabecerasCertificado ObtenerMontosCertificadosCabeceras(int Id, int proyectoid); //ObtenerMontosCabeceras

        string NombreCertificado(int Id,int proyectoId);
    }
}
