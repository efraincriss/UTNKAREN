using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.proyecto.aplicacion.Dto;
using com.cpp.calypso.proyecto.aplicacion.Transporte.Dto;
using com.cpp.calypso.proyecto.dominio.Proveedor;
using com.cpp.calypso.proyecto.dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace com.cpp.calypso.proyecto.aplicacion.Transporte.Interface
{
    public interface IChoferAsyncBaseCrudAppService : IAsyncBaseCrudAppService<Chofer, ChoferDto, PagedAndFilteredResultRequestDto>
    {

        List<ChoferDto> Listar();
        int IngresarChofer(Chofer chofer);
        int EditarChofer(Chofer chofer);
        int EliminarChofer(int id);
        Chofer GetDetalles(int id);


        Chofer BuscarChofer(string NumeroIdentificacion);
        Chofer BuscarChoferProveedor(string NumeroIdentificacion,int proveedorid);
        Chofer BuscarChoferProveedorEditar(string NumeroIdentificacion, int proveedorid,int id);

      
        List<ProveedorDto> ListaProveedoresTransporte();

        Task<List<ChoferDto>> GetLookupAll();

        Task<bool> EnviarCorreo();
    }
}
