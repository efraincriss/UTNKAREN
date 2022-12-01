using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.proyecto.aplicacion.Dto;
using com.cpp.calypso.proyecto.dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion.Interfaces
{
    public interface IClienteAsyncBaseCrudAppService: IAsyncBaseCrudAppService<Cliente, ClienteDto, PagedAndFilteredResultRequestDto>
    {
        List<Cliente> GetClientes();
        List<ClienteDto> GetClientesApi();
        Task<ClienteDto> GetDetalle(int ClienteId);
        Task<string> CrearClienteAsync(ClienteDto cliente);
        Task<string> ActualizarClienteAsync(ClienteDto cliente);
        List<ContratoDto> GetContratosporCliente(int ClienteId);

        bool EliminarCliente(int Id);
    }
}
