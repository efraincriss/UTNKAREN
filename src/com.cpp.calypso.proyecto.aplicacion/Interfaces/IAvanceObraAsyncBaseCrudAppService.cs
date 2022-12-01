using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.proyecto.dominio;
using com.cpp.calypso.proyecto.dominio.Models;
using OfficeOpenXml;

namespace com.cpp.calypso.proyecto.aplicacion.Interfaces
{
    public interface IAvanceObraAsyncBaseCrudAppService : IAsyncBaseCrudAppService<AvanceObra, AvanceObraDto, PagedAndFilteredResultRequestDto>
    {
        List<OfertaDto> ListarOfertasDeProyecto(int ProyectoId);

        List<AvanceObraDto> ListarAvancesDeOferta(int OfertaId);

        int EliminarVigencia(int avanceObraId);

        List<DetalleAvanceObraDto> ListarDetallesAvanceObra(int avanceObraId);

        List<ComputoAvanceObra> ObtenerComputosAvanceObra(int IdOferta, DateTime fecha, int AvanceObraId);

        ProyectoDto GetProyecto(int avanceObraId);

        List<AvanceObraDto> ListarAvancesResumen(int OfertaId);

        decimal[] MontoPresupuestadoIncrementado(int OfertaId);

        //Certificados
        List<AvanceObraDto> ListarAvancesDeOfertaSinCertificar(int OfertaId, DateTime fechaCorte);

        List<DetalleAvanceObraDto> ListarDetallesAvanceObraProyecto(int ProyectoId, DateTime fechaCorte);

        List<DetalleAvanceObraDto> ListarDetallesAvanceObraProyectoFast(int ProyectoId, DateTime fechaCorte);

        //Archivos

        int GuardarArchivo(int AvanceObraId, HttpPostedFileBase[] UploadedFile);

        List<ArchivosAvanceObraDto> ListaArchivos(int AvanceObraId);
        int EditarArchivo(int ArchivoAvanceObraId, HttpPostedFileBase UploadedFile);

        int EliminarVigenciaArchivo(int id);

        ArchivosAvanceObraDto getdetallesarchivo(int id);

        bool AprobarAvanceObra(int id);

        bool DesaprobarAvanceObra(int id);


        //Mas Niveles

        string NivelPadre(Wbs wbs, int OfertaId, int nivel);

        //carga Masiva

        ExcelPackage CargaMasivaAvanceObra(int Id); //Avance de Obra ID;
        List<AvanceObraExcel> GenerarArbol(int OfertaId,int AvanceObraId);


        List<AvanceObraExcel> GenerarArbolCargaIds(int OfertaId);
        ExcelPackage CargaMasivaIDSWBS(int ofertaId);


        int GuardarArchivo(ArchivosAvanceObraDto entityDto);
        int EditFile(int id, string descripcion);


    }
}
