using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.proyecto.dominio;

namespace com.cpp.calypso.proyecto.aplicacion
{
    public interface ITempRequerimientoAsyncBaseCrudAppService : IAsyncBaseCrudAppService<TempRequerimiento, TempRequerimientoDto, PagedAndFilteredResultRequestDto>
    {
        Task<string> CargarProyectoAsync();
        void CargarRequerimientos();

        void CargarTablaRelacion(int desde, int hasta);
        Task<string> CargarRequerimientossAsync(int desde, int hasta);
        Task<string> CargarTransmittalsAsync(int desde, int hasta);
        Task<string> CargarCartas(int desde,int hasta);
        Task<string> CargarOsAsync(int desde, int hasta);

        Task<string> ActualizarReferenciaAsync(int desde, int hasta);

        Task<string> ActualizarMontosRequerimientosAsync(string listrequerimiento);

        void ActualizarFechasOfertasComerciales();
        void ActualizarClaseRequerimientoOfertaComercial();

        string ActualizarNombresProyectos(HttpPostedFileBase UploadedFile);
    }
}
