using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.aplicacion.Dto;
using com.cpp.calypso.proyecto.aplicacion.Interfaces;
using com.cpp.calypso.proyecto.dominio;

namespace com.cpp.calypso.proyecto.aplicacion.Service
{
    public class MenuProveedorAsyncBaseCrudAppService : AsyncBaseCrudAppService<MenuProveedor, MenuProveedorDto, PagedAndFilteredResultRequestDto>, IMenuProveedorAsyncBaseCrudAppService
    {
        public MenuProveedorAsyncBaseCrudAppService(
            IBaseRepository<MenuProveedor> repository
            ) : base(repository)
        {
        }

        public async Task<IList<MenuProveedorDto>> GetMenuProveedor(int proveedorId)
        {

           var list = await Repository.GetAllListAsync(m => m.ProveedorId == proveedorId);

           var listDto = from menu in list
                    select MapToEntityDto(menu);

            return listDto.ToList();

        }

        public async Task UpdateApproved(bool activar, int[] ids)
        {

            var list = await Repository.GetAllListAsync(
                m => ids.Contains(m.Id));

            foreach (var item in list)
            {
                if (activar)
                    item.aprobado = MenuEstadoAprobacion.Aprobado;
                else
                    item.aprobado = MenuEstadoAprobacion.PendienteAprobacion;

                item.fecha_aprobacion = DateTime.Now;

                Repository.Update(item);
            }
 

             
        }
    }
}
