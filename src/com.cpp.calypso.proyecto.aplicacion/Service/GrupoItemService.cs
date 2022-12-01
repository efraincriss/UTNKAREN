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
    public class GrupoItemAsyncBaseCrudAppService : AsyncBaseCrudAppService<GrupoItem, GrupoItemDto, PagedAndFilteredResultRequestDto>, IGrupoItemAsyncBaseCrudAppService
    {
        public GrupoItemAsyncBaseCrudAppService(
            IBaseRepository<GrupoItem> repository
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

        public GrupoItemDto getdetalle(int Id)
        {
            var query = Repository.GetAll().Where(e => e.vigente == true);
            var detalle = (from d in query
                where d.Id == Id
                where d.vigente == true
                select new GrupoItemDto
                {
                    Id = d.Id,
                    vigente = d.vigente,
                    descripcion = d.descripcion,
                  

                }
            ).FirstOrDefault();
            return detalle;
        }

        public List<GrupoItem> lista()
        {
            var lista = Repository.GetAll().Where(e => e.vigente == true).ToList();
            return lista;
        }
    }
}
