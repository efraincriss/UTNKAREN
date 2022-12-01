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
    public interface IZonaProveedorAsyncBaseCrudAppService : IAsyncBaseCrudAppService<ZonaProveedor, ZonaProveedorDto, PagedAndFilteredResultRequestDto>
    {

        /// <summary>
        /// Obtener las zonas, con informacion adicional "proveedores"
        /// </summary>
        /// <returns></returns>
        Task<IList<ZonaProveedorInfoDto>>
            GetInfoAll(int TipoComidaId);
    }
}
