using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.proyecto.aplicacion;
using com.cpp.calypso.proyecto.dominio;
using OfficeOpenXml;

namespace com.cpp.calypso.proyecto.aplicacion.Interfaces
{
    public interface IDetalleAvanceObraAsyncBaseCrudAppService : IAsyncBaseCrudAppService<DetalleAvanceObra, DetalleAvanceObraDto, PagedAndFilteredResultRequestDto>
    {
        List<ComputoDto> ListaComputosPorOferta(int ofertaId);

        Task<int> CreateDetalleAvance(DetalleAvanceObraDto detalle, decimal cantidad_eac);

        decimal calcularvalor(int AvanceObraId);

        Task<int> GuardarDetalles(List<ComputoAvanceObra> lista, int AvanceObraId);
        Task<int> GuardarDetallesNegativos(ComputoAvanceObra lista, int AvanceObraId);

        bool cambiaracertificado(int id);
       

        int Eliminar(int DetalleAvanceObra);

        Task<string> SubirExcelAvanceObra(HttpPostedFileBase UploadedFile,int avanceobraid);
        Task<string> CargarArchivoIDS(HttpPostedFileBase UploadedFile, int proyectoId);
        List<string> ValidarSubirExcelAvanceObra(HttpPostedFileBase UploadedFile,int AvanceObraId);

        ExcelPackage CargaMasivaIDS(int proyectoId); //Avance de Obra ID;
    }
}
