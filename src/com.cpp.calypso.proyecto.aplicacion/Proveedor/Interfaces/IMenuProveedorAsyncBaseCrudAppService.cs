using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.proyecto.aplicacion.Dto;
using com.cpp.calypso.proyecto.dominio;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion.Interfaces
{
    public interface IMenuProveedorAsyncBaseCrudAppService :
        IAsyncBaseCrudAppService<MenuProveedor, MenuProveedorDto, PagedAndFilteredResultRequestDto>
    {
         

        /// <summary>
        /// Obtener listado de menus asociados a un proveedor
        /// </summary>
        /// <param name="proveedorId"></param>
        /// <returns></returns>
        Task<IList<MenuProveedorDto>> GetMenuProveedor(int proveedorId);

        Task UpdateApproved(bool activar, int[] ids);
    }
}
