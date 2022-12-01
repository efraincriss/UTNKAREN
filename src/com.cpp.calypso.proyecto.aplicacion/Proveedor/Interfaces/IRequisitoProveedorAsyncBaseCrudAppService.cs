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
    public interface IRequisitoProveedorAsyncBaseCrudAppService : 
        IAsyncBaseCrudAppService<RequisitoProveedor, RequisitoProveedorDto, PagedAndFilteredResultRequestDto>
    {

        Task<IList<RequisitoProveedorDto>> AddOrUpdate(IList<RequisitoProveedorDto> listEntity);

        /// <summary>
        /// Cumple /  No cumple
        /// </summary>
        /// <param name="cumple">True: Cumple, False: No cumple</param>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task UpdateApproved(bool cumple, int[] ids);
    }
}
