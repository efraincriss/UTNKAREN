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
    public class DetalleGananciaAsyncBaseCrudAppService : AsyncBaseCrudAppService<DetalleGanancia, DetalleGananciaDto, PagedAndFilteredResultRequestDto>, IDetalleGananciaAsyncBaseCrudAppService
    {
        public DetalleGananciaAsyncBaseCrudAppService(
            IBaseRepository<DetalleGanancia> repository
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

        public DetalleGananciaDto getdetalle(int Id)
        {
            var query = Repository.GetAllIncluding(c=>c.Ganancia, c=>c.GrupoItem, c=>c.PorcentajeIncremento).Where(e => e.vigente == true);
            var detalle = (from d in query
                where d.Id == Id
                where d.vigente == true
                select new DetalleGananciaDto
                {
                    Id = d.Id,
                    GananciaId = d.GananciaId,
                    Ganancia = d.Ganancia,
                    GrupoItemId = d.GrupoItemId,
                    GrupoItem = d.GrupoItem,
                    PorcentajeIncrementoId = d.PorcentajeIncrementoId,
                    PorcentajeIncremento = d.PorcentajeIncremento,
                    vigente = d.vigente,
                    valor = d.valor

                }
            ).FirstOrDefault();
            return detalle;

        }

        public List<DetalleGanancia> Listar()
        {
            var lista = Repository.GetAllIncluding(c=>c.Ganancia,c=>c.GrupoItem,c=>c.PorcentajeIncremento).Where(e => e.vigente == true).ToList();
            return lista;
        }

        public List<DetalleGananciaDto> ListarporGanancia(int Id)
        {
            var list = Repository.GetAllIncluding(c => c.Ganancia, c => c.GrupoItem, c => c.PorcentajeIncremento).Where(e => e.vigente == true).ToList();
            var lista = (from d in list
                where d.GananciaId == Id
                where d.vigente == true
                select new DetalleGananciaDto
                {
                    Id = d.Id,
                    GananciaId = d.GananciaId,
                    Ganancia = d.Ganancia,
                    GrupoItemId = d.GrupoItemId,
                    GrupoItem = d.GrupoItem,
                    PorcentajeIncrementoId = d.PorcentajeIncrementoId,
                    PorcentajeIncremento = d.PorcentajeIncremento,
                    vigente = d.vigente,
                    valor = d.valor

                }).ToList();

            return lista;
        }
    }
}
