using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.aplicacion.Interfaces;
using com.cpp.calypso.proyecto.dominio;

namespace com.cpp.calypso.proyecto.aplicacion.Service
{
    public class PorcentajeIncrementoAsyncBaseCrudAppService : AsyncBaseCrudAppService<PorcentajeIncremento, PorcentajeIncrementoDto, PagedAndFilteredResultRequestDto>, IPorcentajeIncrementoAsyncBaseCrudAppService
    {
        public PorcentajeIncrementoAsyncBaseCrudAppService(
            IBaseRepository<PorcentajeIncremento> repository
        ) : base(repository)
        {
        }

        public bool Eliminar(int Id)
        {
            var porcentaje = Repository.Get(Id);
            porcentaje.vigente = false;

            var r = Repository.Update(porcentaje);
            if (r.Id > 0)
            {
                return true;
            }

            return false;
        }

        public PorcentajeIncrementoDto getdetalle(int Id)
        {
            var query = Repository.GetAll().Where(e => e.vigente == true);
            var detalle = (from d in query
                where d.Id == Id
                where d.vigente == true
                select new PorcentajeIncrementoDto
                {
                    Id = d.Id,
                    vigente = d.vigente,
                    descripcion = d.descripcion,
                    valor = d.valor

                }
            ).FirstOrDefault();
            return detalle;

        }

        public List<PorcentajeIncremento> Listar()
        {
            var lista = Repository.GetAll().Where(e => e.vigente == true).ToList();
            return lista;
        }
    }
}
