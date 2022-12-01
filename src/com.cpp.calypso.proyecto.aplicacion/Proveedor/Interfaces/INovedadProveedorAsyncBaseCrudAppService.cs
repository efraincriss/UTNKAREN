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
    public interface INovedadProveedorAsyncBaseCrudAppService : IAsyncBaseCrudAppService<NovedadProveedor, NovedadProveedorDto, PagedAndFilteredResultRequestDto>
    {


        /// <summary>
        /// Obtener listado de novedades asociados a un proveedor
        /// </summary>
        /// <param name="proveedorId"></param>
        /// <returns></returns>
        Task<IList<NovedadProveedorDto>> GetNovedadProveedor(int proveedorId);

    }
}
