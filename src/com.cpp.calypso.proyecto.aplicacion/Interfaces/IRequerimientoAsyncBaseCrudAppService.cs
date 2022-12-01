using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.proyecto.aplicacion.Dto;
using com.cpp.calypso.proyecto.dominio;
using com.cpp.calypso.proyecto.dominio.Models;

namespace com.cpp.calypso.proyecto.aplicacion.Interfaces
{
    public interface IRequerimientoAsyncBaseCrudAppService : IAsyncBaseCrudAppService<Requerimiento, RequerimientoDto, PagedAndFilteredResultRequestDto>
    {
        List<RequerimientoDto> Listar();
        List<RequerimientoDto> ListarporContrato(int Id);
        RequerimientoDto GetDetalles(int requerimientoId);

        RequerimientoDto EliminarVigencia(int requerimientoId);

        OfertaDto CrearOfertaParaRequerimiento(int requerimeintoId, int proyectoId);

        bool ExistePrincipal(int proyectoId);
        bool ExisteAdicional(string codigo);
        List<RequerimientoDto> ObtenerRequerimientosDeProyecto(int proyectoId);

        string ObtenerSecuencial(int proyectoId);

        string FormatoCorreoRequerimientoCreado(int procesoId, int ofertaId);

        bool ComprobarExisteCodigo(RequerimientoDto input);
        bool cambiarProyectoReferenciaPresupuesto(int RequerimientoId, int NuevoProyectoIdd);
        Task<bool> ActualizarSolicitanteAsync(RequerimientoDto requerimiento);
        bool cambiar_estado_requerimiento(PresupuestoDto presupuesto, int catalogo);

        bool cambiar_estado_cancelado(int Id);

        bool cambiar_estado_activado(int Id);


        // Archivos Requerimiento
        //Archivos

        int GuardarArchivo(int RequerimientosId, HttpPostedFileBase[] UploadedFile,int tipo);

        List<ArchivosRequerimientoDto> ListaArchivos(int RequerimientoId);
        int EditarArchivo(int RequerimientoId, HttpPostedFileBase UploadedFile,int tipo);

        int EliminarVigenciaArchivo(int id);

        ArchivosRequerimientoDto getdetallesarchivo(int id);

        bool actualizarmontosrequerimiento(int presupuestoId,decimal i = 0, decimal c = 0, decimal s = 0, decimal sub = 0,decimal total=0);

        Task<string> Send_Files_Requerimiento(int Id, string asunto="",string body = "");

        ProyectoRequerimiento CodigoAdicionalActualProyectos();
        string hrefoutlook(int id, string to = "", string cc = "", string subject = "");

        List<RequerimientoDto> RequerimientosyOfertasLigadas(List<Requerimiento> list);
    }
}
