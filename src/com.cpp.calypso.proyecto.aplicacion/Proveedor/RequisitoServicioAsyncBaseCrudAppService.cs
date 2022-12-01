using AutoMapper;
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
    public class RequisitoServicioAsyncBaseCrudAppService : AsyncBaseCrudAppService<RequisitoServicio, RequisitoServicioDto, PagedAndFilteredResultRequestDto>, IRequisitoServicioAsyncBaseCrudAppService
    {

        private readonly ICatalogoAsyncBaseCrudAppService _catalogoService;
		private readonly IRequisitosAsyncBaseCrudAppService _requisitosService;

		public RequisitoServicioAsyncBaseCrudAppService(
            IBaseRepository<RequisitoServicio> repository,
            ICatalogoAsyncBaseCrudAppService catalogoService,
			IRequisitosAsyncBaseCrudAppService requisitosService
            ) : base(repository)
        {
            _catalogoService = catalogoService;
			_requisitosService = requisitosService;
        }

        public List<RequisitoServicioDto> GetList()
        {
            int i = 1;
            var query = Repository.GetAll();

            var requisitos = (from d in query
                              where d.vigente == true
                              select new RequisitoServicioDto
                              {
                                  Id = d.Id,
								  RequisitosId = d.RequisitosId,
                                  catalogo_servicio_id = d.catalogo_servicio_id,
                                  obligatorio = d.obligatorio,
                                  vigente = d.vigente,
                              }).ToList();

            foreach (var e in requisitos)
            {
                e.nro = i++;
				
				var cat_requisito = _requisitosService.GetRequisito(e.RequisitosId);
				e.requisito = cat_requisito.nombre;
                

                var cat_servicio = _catalogoService.GetCatalogo(e.catalogo_servicio_id);
                e.servicio = cat_servicio.nombre;

                if (e.obligatorio == true)
                {
                    e.nombre_obligatorio = "SI";
                }
                else
                {
                    e.nombre_obligatorio = "NO";
                }

            }

                return requisitos;
        }

        public async Task<List<RequisitoServicioDto>> GetRequisitos(int servicioId)
        {
            var query = Repository.GetAll();
            var list = await(from item in query
                               where item.catalogo_servicio_id == servicioId
                               && item.vigente == true
                             select item).ToListAsync();

            var listDto = from item in list
                          select MapToEntityDto(item);

            return listDto.ToList();
        }

        public RequisitoServicioDto GetRequisitoServicio(int Id)
        {
            var d = Repository.Get(Id);

            RequisitoServicioDto requisito = new RequisitoServicioDto()
            {
				Id = d.Id,
				RequisitosId = d.RequisitosId,
				catalogo_servicio_id = d.catalogo_servicio_id,
				obligatorio = d.obligatorio,
				vigente = d.vigente,
			};


            return requisito;
        }

        public string UniqueServicio(int servicio, int requisito, int id)
        {
            var s = Repository.GetAll().Where(d => d.catalogo_servicio_id == servicio && d.RequisitosId == requisito && d.vigente == true).FirstOrDefault();
            if (s != null)
            {
                if (s.Id == id) {
                    return "UPDATE";
                }
                else {
                    return "SI";
                }
            }
            else
            {
                return "NO";
            }
        }

    }
}
