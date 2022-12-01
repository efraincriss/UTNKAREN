using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.aplicacion.Dto;
using com.cpp.calypso.proyecto.aplicacion.Interfaces;
using com.cpp.calypso.proyecto.dominio;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion.Service
{
    public class RequisitoProveedorAsyncBaseCrudAppService : 
        AsyncBaseCrudAppService<RequisitoProveedor, RequisitoProveedorDto, PagedAndFilteredResultRequestDto>, 
        IRequisitoProveedorAsyncBaseCrudAppService
    {
        public RequisitoProveedorAsyncBaseCrudAppService(
            IBaseRepository<RequisitoProveedor> repository
            ) : base(repository)
        {

        }

        public async Task<IList<RequisitoProveedorDto>> AddOrUpdate(IList<RequisitoProveedorDto> listEntityDto)
        {

            var baseRepository = Repository as IBaseRepository<RequisitoProveedor>;
            if (baseRepository == null)
            {
                throw new ArgumentException(string.Format("The repository should be type IBaseRepository<TEntity>. {0}", Repository.GetType()), "input");
            }

            var proveedorIds = new List<int>();
            var listEntity = new List<RequisitoProveedor>();
            foreach (var item in listEntityDto)
            {
                listEntity.Add(MapToEntity(item));
                proveedorIds.Add(item.ProveedorId);
            }
            proveedorIds = proveedorIds.Distinct().ToList();
    
            //Si existe el requisito, excluir de los items agregar/modificar..
            var query = Repository.GetAll();
            var existentes = await (from item in query
                             where proveedorIds.Contains(item.ProveedorId)
                             select item).ToListAsync();

            var listEntityFinal = new List<RequisitoProveedor>();
            foreach (var item in listEntity)
            {
                if (!existentes.Where(req => req.ProveedorId == item.ProveedorId 
                && req.RequisitosId == item.RequisitosId).Any()) {

                    listEntityFinal.Add(item);
                }
            }

            var listUpdate =  baseRepository.SaveOrUpdate(listEntityFinal);

            await CurrentUnitOfWork.SaveChangesAsync();

            var listUpdateDto = new List<RequisitoProveedorDto>();
            foreach (var item in listUpdate)
            {
                listUpdateDto.Add(MapToEntityDto(item));
            }
            return listUpdateDto;

        }

        public async Task UpdateApproved(bool cumple, int[] ids)
        {
            var list = await Repository.GetAllListAsync(
                m => ids.Contains(m.Id));

            foreach (var item in list)
            {
                if (cumple)
                    item.cumple = RequisitoEstado.Cumple;
                else
                    item.cumple = RequisitoEstado.NoCumple;
 
                Repository.Update(item);
            }

        }
    }
}
