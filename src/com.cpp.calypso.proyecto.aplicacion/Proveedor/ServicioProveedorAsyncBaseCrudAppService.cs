using System.Collections.Generic;
using System.Threading.Tasks;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.aplicacion.Dto;
using com.cpp.calypso.proyecto.aplicacion.Interfaces;
using com.cpp.calypso.proyecto.dominio;

namespace com.cpp.calypso.proyecto.aplicacion.Service
{
    public class ServicioProveedorAsyncBaseCrudAppService :
        AsyncBaseCrudAppService<ServicioProveedor, ServicioProveedorDto, PagedAndFilteredResultRequestDto>,
        IServicioProveedorAsyncBaseCrudAppService
	{
        public IRequisitoProveedorAsyncBaseCrudAppService RequisitoProveedorService { get; }
        public IRequisitosAsyncBaseCrudAppService RequisitoService { get; }
        public IRequisitoServicioAsyncBaseCrudAppService RequisitoServicioService { get; }


        public ServicioProveedorAsyncBaseCrudAppService(
			IBaseRepository<ServicioProveedor> repository,
            IRequisitoProveedorAsyncBaseCrudAppService requisitoProveedorService,
            IRequisitosAsyncBaseCrudAppService requisitoService,
            IRequisitoServicioAsyncBaseCrudAppService requisitoServicioService

            ) : base(repository)
		{
            RequisitoProveedorService = requisitoProveedorService;
            RequisitoService = requisitoService;
            RequisitoServicioService = requisitoServicioService;
        }

      
        public async override Task<ServicioProveedorDto> Create(ServicioProveedorDto input)
        {
            //TODO: Las reglas deben ser parte de las clases de Negocios. (Domino-Manager)
            //Rules
            //CheckDuplicate
            var CheckDuplicate = await Repository.LongCountAsync(item =>
                                 item.ServicioId == input.ServicioId
                                && item.ProveedorId == input.ProveedorId);

            if (CheckDuplicate > 0)
            {
                var msg = string.Format("Ya existe el servicio asociado al Proveedor");
                throw new GenericException(msg, msg);
            }

            //2. Guardar
            var result =  await base.Create(input);

            //3. Crear los requisitos asociados al servicio en proveedor
            //3.1 Obtener los requisitos asociados al servicio...
            var requisitos = await RequisitoServicioService.GetRequisitos(input.ServicioId);
            var requisitosProveedor = new List<RequisitoProveedorDto>();
            foreach (var item in requisitos)
            {
                var requisitoDto = new RequisitoProveedorDto();
                requisitoDto.ProveedorId = input.ProveedorId;
                requisitoDto.RequisitosId = item.RequisitosId;
                requisitoDto.cumple = RequisitoEstado.NoCumple;
                requisitoDto.observaciones = "Requisito asociado,  al momento de un Servicio al proveedor";

                requisitosProveedor.Add(requisitoDto);
            }

            var requisitosUpdate = await RequisitoProveedorService.AddOrUpdate(requisitosProveedor);

            return result; 
        }

        public async override Task<ServicioProveedorDto> Update(ServicioProveedorDto input)
        {
            //Rules
            //CheckDuplicate
            var CheckDuplicate = await Repository.LongCountAsync(item =>
                                 item.ServicioId == input.ServicioId
                                && item.ProveedorId == input.ProveedorId
                                && item.Id != input.Id);

            if (CheckDuplicate > 0)
            {
                var msg = string.Format("Ya existe el servicio asociado al Proveedor");
                throw new GenericException(msg, msg);
            }



            var result =  await base.Update(input);

            //3. Crear los requisitos asociados al servicio en proveedor
            //3.1 Obtener los requisitos asociados al servicio...
            var requisitos = await RequisitoServicioService.GetRequisitos(input.ServicioId);
            var requisitosProveedor = new List<RequisitoProveedorDto>();
            foreach (var item in requisitos)
            {
                var requisitoDto = new RequisitoProveedorDto();
                requisitoDto.ProveedorId = input.ProveedorId;
                requisitoDto.RequisitosId = item.RequisitosId;
                requisitoDto.cumple = RequisitoEstado.NoCumple;
                requisitoDto.observaciones = "Requisito asociado,  por asociar un Servicio al proveedor";
            }

            var requisitosUpdate = await RequisitoProveedorService.AddOrUpdate(requisitosProveedor);

            return result;
        }
    }
}
