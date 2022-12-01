using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.framework;
using com.cpp.calypso.proyecto.dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion
{
    public interface IProyectoAsyncBaseCrudAppService: IAsyncBaseCrudAppService<Proyecto, ProyectoDto, 
        ConsultaProyectoPagedAndFilteredResultRequestDto>
    {
        List<ProyectoDto> Consultar(ConsultaProyectoPagedAndFilteredResultRequestDto consulta);
        bool FinalizarProyecto(ProyectoDto proyectoDto);
    }

    public class ProyectoAsyncBaseCrudAppService :
        AsyncBaseCrudAppService<Proyecto, ProyectoDto, ConsultaProyectoPagedAndFilteredResultRequestDto>, IProyectoAsyncBaseCrudAppService
    {
        public ProyectoAsyncBaseCrudAppService(
             IHandlerExcepciones manejadorExcepciones,
            IBaseRepository<Proyecto> repository) : base(manejadorExcepciones,repository)
        {

        }

        public override Task<PagedResultDto<ProyectoDto>> GetAll(ConsultaProyectoPagedAndFilteredResultRequestDto 
            input)
        {
            ///Linq.. (ret)
            return base.GetAll(input);
        }

        protected override IQueryable<Proyecto> ApplyFilter(IQueryable<Proyecto> query, 
            ConsultaProyectoPagedAndFilteredResultRequestDto input)
        {

          
            if (string.IsNullOrWhiteSpace(input.Codigo))
            {
                query.Where(p => p.Codigo == input.Codigo);
            }

            if (input.Estado.HasValue)
            {
                query.Where(p => p.Estado == input.Estado);
            }

            return query;

            //return base.ApplyFilter(query, input);
        }

        public List<ProyectoDto> Consultar(ConsultaProyectoPagedAndFilteredResultRequestDto consulta) {

            var query =  Repository.GetAll();
 
            if (string.IsNullOrWhiteSpace(consulta.Codigo)) {
                query.Where(p => p.Codigo == consulta.Codigo);
            }

            if (consulta.Estado.HasValue)
            {
                query.Where(p => p.Estado == consulta.Estado);
            }

            var list = query.ToList().Select(MapToEntityDto).ToList();

            return list;
        }

        public bool FinalizarProyecto(ProyectoDto proyectoDto) {
            //Llamado Dominio. (Reglas)
            //Lllamado de infraestructura. 
            return true;
        }
    }


    public class ConsultaProyectoPagedAndFilteredResultRequestDto : PagedAndFilteredResultRequestDto {

        public string Codigo { get; set; }

        public EstadoProyecto? Estado { get; set; }
    }

    [AutoMap(typeof(Proyecto))]
    [Serializable]
    public class ProyectoDto : EntityDto
    {
        public string Codigo { get; set; }

        [Obligado]
        public string Nombre { get; set; }

        public DateTime FechaInicio { get; set; }

        public DateTime FechaFinal { get; set; }

        public EstadoProyecto Estado { get; set; }

        public ICollection<ActividadDto> Actividades { get; set; }
    }

    [AutoMap(typeof(Actividad))]
    [Serializable]
    public class ActividadDto : EntityDto
    {
        public string Nombre { get; set; }

        public int ProyectoId { get; set; }

    }
}
