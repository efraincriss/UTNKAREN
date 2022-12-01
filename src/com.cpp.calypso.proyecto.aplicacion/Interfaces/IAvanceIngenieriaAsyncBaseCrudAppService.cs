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

namespace com.cpp.calypso.proyecto.aplicacion
{
    public interface IAvanceIngenieriaAsyncBaseCrudAppService : IAsyncBaseCrudAppService<AvanceIngenieria, AvanceIngenieriaDto, PagedAndFilteredResultRequestDto>
    {
        decimal GetMontoPresupuestado(int ofertaId);
        List<AvanceIngenieriaDto> ListarPorOferta(int ofertaId);

        bool Eliminar(int avanceIngenieriaId);

        bool CargarAvanceIngenieria(int OfertaId);

        List<Oferta> ListaOfertasDefinitivas(List<AvanceIngenieriaExcel> Lista);

        List<AvanceIngenieriaExcel>FiltrarAvancesExcel(List<AvanceIngenieriaExcel> Lista,string codigoproyecto);
        List<AvanceIngenieriaExcel> FiltrarAvancesExcelFechas(List<AvanceIngenieriaExcel> Lista, DateTime fechadesde,DateTime fechahasta);
        bool CrearDetalle(List<Oferta> ofertas,List<AvanceIngenieriaExcel> Lista, DateTime presentacion, DateTime fechadesde, DateTime fechahasta);

        bool Detalles(List<AvanceIngenieriaExcel> Lista, DateTime presentacion, DateTime fechadesde, DateTime fechahasta);
        List<OfertaDto> ListarOfertasDeProyecto(int ProyectoId);
        List<AvanceIngenieriaDto> ListarAvancesDeOfertaSinCertificar(int OfertaId);
        List<DetalleAvanceIngenieriaDto> ListarPorAvanceIngenieria(int avanceIngenieriaId);
        List<DetalleAvanceIngenieriaDto> ListarDetallesAvanceIngenieriaProyecto(int ProyectoId);



        ExcelPackage ObtenerCertificadoIngenieria(int Id);

        List<IngenieriaDatos> Datos(int id);

        string UploadAvanceMasivo(AvanceUpload e);

       
    }
}
