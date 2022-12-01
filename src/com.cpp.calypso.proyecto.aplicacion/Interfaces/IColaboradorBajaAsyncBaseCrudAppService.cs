using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.proyecto.aplicacion.Dto;
using com.cpp.calypso.proyecto.dominio;
using com.cpp.calypso.proyecto.dominio.Models;
using com.cpp.calypso.seguridad.aplicacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace com.cpp.calypso.proyecto.aplicacion.Interfaces
{
    public interface IColaboradorBajaAsyncBaseCrudAppService: IAsyncBaseCrudAppService<ColaboradorBaja, ColaboradorBajaDto, PagedAndFilteredResultRequestDto>
    {
        ColaboradorBajaDto GetBaja(int Id);
        List<ColaboradorBajaDto> GetBajas();
        string CargarArchivoBaja(ColaboradorBajaDto baja, HttpPostedFileBase UploadedFile);
        List<ColaboradorBajaDto> GetBajasGenerarArchivo(BajaEstado estado);
        Task<string> GenerarExcelBajas(List<ColaboradorBajaDto> bajas, bool es_manual);
        int GuardarLiquidacionArchivoAsync(int baja, HttpPostedFileBase[] UploadedFile);
        ColaboradorBajaDto GetBajasEnviarSap(int Id);
        ColaboradorBajaTemp GetBajasEnviarSapTemp(int Id);
        bool UpdateColaboradorBaja(int Id);
        List<ColaboradorBajaDto> GetBajasArchivoIESS();
        Task<string> GenerarArchivoIESS(List<ColaboradorBajaDto> bajas, bool es_manual);
        Task<string> GenerarExcelBajasTemp(List<ColaboradorBajaTemp> bajas, bool es_manual);
        void SubirPdf(int ColaboradorBajaId, ArchivoDto archivo);
        void SubirPago(int ColaboradorBajaId, ArchivoDto archivo);

        bool InsertarBajaColaborador(ColaboradorBajaModel baja);
    }
}
