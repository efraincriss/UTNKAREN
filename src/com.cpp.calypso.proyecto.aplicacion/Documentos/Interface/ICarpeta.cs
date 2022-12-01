using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.aplicacion.Documentos.Dto;
using com.cpp.calypso.proyecto.aplicacion.Documentos.Models;
using com.cpp.calypso.proyecto.aplicacion.RecursosHumanos.Dto;
using com.cpp.calypso.proyecto.dominio.Documentos;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion.Documentos.Interface
{
    public interface ICarpetaAsyncBaseCrudAppService : IAsyncBaseCrudAppService<Carpeta, CarpetaDto, PagedAndFilteredResultRequestDto>
    {
        CarpetaDto ObtenerCarpetaPorId(int id);

        Task<bool> CrearCarpetaAsync(CarpetaDto dto);

        List<CarpetaDto> ObtenerCarpetas();

        bool EditarCarpetaAsync(CarpetaDto dto);

        ResultadoEliminacionResponse Eliminar(int id);

        List<EstructuraArbol> ObtenerCarpetasAutorizadas();
        String ActualizarDocumentos();
        

        List<SeccionDto> SeccionesDES();
        List<ImagenSeccionDto> ImagenesDES();


        String SyncCarpetas(string azureTableName);
        String SyncDocumentos(string azureTableName);
        String SyncUsuarios(string azureTableName);
        String SyncSecciones(string azureTableName);

        int EncryptTest(string text);

        string VerificarContraseña(string pass);

        String SyncImagenesSecciones(string azureTableName);
        String SyncImagenesSeccionesLista(List<int>id);
        ExcelPackage SyncImagenesExcel();

        List<List<int>> ParticionadaImagenes();

        FechasSincronizacion UltimasFechasSincronizacion();
        int superaCaracteres(string seccion);

    }
}
