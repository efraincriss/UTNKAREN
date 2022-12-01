using Abp.Application.Services.Dto;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.proyecto.aplicacion.Dto;
using com.cpp.calypso.proyecto.dominio;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion.Interfaces
{
    public interface IContratoProveedorAsyncBaseCrudAppService : 
        IAsyncBaseCrudAppService<ContratoProveedor, ContratoProveedorDto, PagedAndFilteredResultRequestDto>
    {

        Task<ContratoProveedorTipoOpcionesDto> GetInfo(EntityDto<int> input);

        Task<ContratoProveedorTipoOpcionesDto> Create(ContratoProveedorTipoOpcionesDto input);

        Task<ContratoProveedorTipoOpcionesDto> Update(ContratoProveedorTipoOpcionesDto input);

        Task<ContratoProveedorTipoOpcionesDto> UpdateAndDelete(ContratoProveedorTipoOpcionesDto entity);

        bool CanDeleteTipoOpcionComida(int TipoOpcionComidaId);
    }
}
