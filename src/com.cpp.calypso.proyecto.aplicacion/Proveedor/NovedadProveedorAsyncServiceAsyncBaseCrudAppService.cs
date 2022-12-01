using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.aplicacion.Dto;
using com.cpp.calypso.proyecto.aplicacion.Interfaces;
using com.cpp.calypso.proyecto.dominio;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion.Service
{
    public class NovedadProveedorAsyncBaseCrudAppService :
        AsyncBaseCrudAppService<NovedadProveedor, NovedadProveedorDto, PagedAndFilteredResultRequestDto>,
        INovedadProveedorAsyncBaseCrudAppService
    {
        public IArchivoAsyncBaseCrudAppService ArchivoService { get; }

        public NovedadProveedorAsyncBaseCrudAppService(
            IBaseRepository<NovedadProveedor> repository,
            IArchivoAsyncBaseCrudAppService archivoService
            ) : base(repository)
        {
            ArchivoService = archivoService;
        }



        public async Task<IList<NovedadProveedorDto>> GetNovedadProveedor(int proveedorId)
        {
            var list = await Repository.GetAllListAsync(m => m.ProveedorId == proveedorId);

            var listDto = from item in list
                          select MapToEntityDto(item);

            return listDto.ToList();
        }


        public override async Task<NovedadProveedorDto> Create(NovedadProveedorDto input)
        {
            //Crear Archivo si existe
            if (input.documentacion_subida != null)
            {
                var archivoResult = await ArchivoService.Create(input.documentacion_subida);
                input.documentacion_id = archivoResult.Id;
            }
            return await base.Create(input);
        }

        public override async Task<NovedadProveedorDto> Update(NovedadProveedorDto input)
        {
            //Crear Archivo si existe
            if (input.documentacion_subida != null)
            {
                var archivoResult = await ArchivoService.Create(input.documentacion_subida);
                input.documentacion_id = archivoResult.Id;
            }
            return await base.Update(input);
        }
    }
}
