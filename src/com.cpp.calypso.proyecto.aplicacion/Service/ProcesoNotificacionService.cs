using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.dominio;

namespace com.cpp.calypso.proyecto.aplicacion
{
    public class ProcesoNotificacionAsyncBaseCrudAppService : AsyncBaseCrudAppService<ProcesoNotificacion, ProcesoNotificacionDto, PagedAndFilteredResultRequestDto>, IProcesoNotificacionAsyncBaseCrudAppService
    {


        public ProcesoNotificacionAsyncBaseCrudAppService(
            IBaseRepository<ProcesoNotificacion> repository

        ) : base(repository)
        {
    
        }

    
        public List<ProcesoNotificacionDto> Listar()
        {
            var query = Repository.GetAll().Where(o => o.vigente == true);
            var procesos = (from p in query
                select new ProcesoNotificacionDto()
                {
                    Id = p.Id,
                    vigente = p.vigente,
                    nombre = p.nombre,
                    estado = p.estado,
                    formato = p.formato,
                    Tipo = p.Tipo
                }).ToList();
            return procesos;
        }

        public List<ProcesoNotificacionDto> ListarPorTipo(ProcesoNotificacion.TipoProceso tipo)
        {
            var query = Repository.GetAll()
                .Where(o => o.vigente)
                .Where(o => o.Tipo == tipo);

            var items = (from p in query
                select new ProcesoNotificacionDto()
                {
                    Id = p.Id,
                    nombre = p.nombre,
                    formato = p.formato
                }).ToList();

            return items;
        }

    }
}
