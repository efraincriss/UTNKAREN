using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.proyecto.aplicacion.Dto;
using com.cpp.calypso.proyecto.dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace com.cpp.calypso.proyecto.aplicacion.Interfaces
{
    public interface  IColaboradoresFotografiaAsyncBaseCrudAppService : IAsyncBaseCrudAppService<ColaboradoresFotografia, ColaboradoresFotografiaDto, PagedAndFilteredResultRequestDto>
    {
        List<ColaboradoresFotografia> GetFotografiaPorOrigen(int Idcolaborador, string origen);
        ColaboradoresFotografia GetFotografia(int Id);
        Archivo GetArchivoFotografia(int Idcolaborador, string origen);
        Task<string> CrearActualizarFotografiaPorColaboradorAsync(ColaboradoresFotografiaDto colaboradoresFotografia, HttpPostedFileBase[] UploadedFile);
        bool EliminarFotografiaPorOrigen(int Idcolaborador, string origen);
    }
}
