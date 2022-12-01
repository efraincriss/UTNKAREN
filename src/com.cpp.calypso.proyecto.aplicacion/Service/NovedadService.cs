using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.aplicacion.Dto;
using com.cpp.calypso.proyecto.aplicacion.Interfaces;
using com.cpp.calypso.proyecto.dominio;

namespace com.cpp.calypso.proyecto.aplicacion.Service
{
    public class NovedadAsyncBaseCrudAppService : AsyncBaseCrudAppService<Novedad, NovedadDto, PagedAndFilteredResultRequestDto>, INovedadAsyncBaseCrudAppService

    {
        public NovedadAsyncBaseCrudAppService(
            IBaseRepository<Novedad> repository
            ) : base(repository)
        {
        }

        public NovedadDto GetDetalles(int novedadId)
        {
            var novedadQuery = Repository.GetAllIncluding(n => n.Requerimiento);
            var item = (from n in novedadQuery
                where n.Id == novedadId
                where n.vigente == true
                select new NovedadDto()
                {
                    Id = n.Id,
                    descripcion = n.descripcion,
                    RequerimientoId = n.RequerimientoId,
                    version = n.version,
                    fecha_novedad = n.fecha_novedad,
                    fecha_solucion = n.fecha_solucion,
                    solucion = n.solucion,
                    solucionada = n.solucionada,
                    vigente = n.vigente,
                    Requerimiento = n.Requerimiento                   
                }).SingleOrDefault();
            return item;
        }

        public int EliminarVigencia(int novedadId)
        {
            var novedad = Repository.Get(novedadId);
            novedad.vigente = false;
            Repository.Update(novedad);
            return novedad.RequerimientoId;
        }
    }
}
