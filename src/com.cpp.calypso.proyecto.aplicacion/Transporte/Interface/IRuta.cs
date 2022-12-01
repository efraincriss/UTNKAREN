using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.proyecto.aplicacion.Transporte.Dto;
using com.cpp.calypso.proyecto.aplicacion.Transporte.Models;
using com.cpp.calypso.proyecto.dominio.Transporte;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.cpp.calypso.proyecto.aplicacion.Dto;

namespace com.cpp.calypso.proyecto.aplicacion.Transporte.Interface
{
    public interface IRutaAsyncBaseCrudAppService : IAsyncBaseCrudAppService<Ruta, RutaDto, PagedAndFilteredResultRequestDto>
    {

        List<RutaDto> Listar();
        int IngresarRuta(Ruta chofer);
        int EditarRuta(Ruta chofer);
        int EliminarRuta(int id);
        Ruta GetDetalles(int id);

        List<Lugar> ListarLugares();

        bool existecode(string code, int id);

        string nextcode();

        ExcelPackage ExcelPersonasTransportadas(InputReporteTransporte input);
        ExcelPackage ExcelPersonasTransportadasNombres(InputReporteTransporte input);

        ExcelPackage ExcelViajes(InputReporteTransporte input);

        ExcelPackage ExcelTrabajosDiarios(InputReporteTransporte input);

        List<Ruta> ListadeRutasporProveedor(int ProveedorId);

        List<Vehiculo> ListadeVehiculosporProveedor(int ProveedorId);


        List<TransportistasDatos> ObtenerTransportistas();

        List<CatalogoDto> ObtenerTiposComidaViandas();

        ExcelPackage ObtenerReporteRetiroViandas(InputRetiroTransportista input);
    }
}
