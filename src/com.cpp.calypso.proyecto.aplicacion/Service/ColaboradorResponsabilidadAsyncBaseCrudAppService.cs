using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.aplicacion.Dto;
using com.cpp.calypso.proyecto.aplicacion.Interfaces;
using com.cpp.calypso.proyecto.dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion.Service
{
    public class ColaboradorResponsabilidadAsyncBaseCrudAppService : AsyncBaseCrudAppService<ColaboradorResponsabilidad, ColaboradorResponsabilidadDto, PagedAndFilteredResultRequestDto>, IColaboradorResponsabilidadAsyncBaseCrudAppService
    {
        public ColaboradorResponsabilidadAsyncBaseCrudAppService(
            IBaseRepository<ColaboradorResponsabilidad> repository
            ) : base(repository)
        {

        }

        public List<ColaboradorResponsabilidadDto> GetList()
        {
            var query = Repository.GetAll();

            var res = (from d in query
                              select new ColaboradorResponsabilidadDto
                              {
                                  Id = d.Id,
                                  colaborador_id = d.colaborador_id,
                                  Colaboradores = d.Colaboradores,
                                  catalogo_responsable_id = d.catalogo_responsable_id,
                                  acceso = d.acceso,
                              }).ToList();

          
            return res;
        }

        public List<ColaboradorResponsabilidadDto> GetResponsabilidadesPorColaborador(int Id)
        {
            var query = Repository.GetAll().Where(c => c.colaborador_id == Id);

            var res = (from d in query
                       select new ColaboradorResponsabilidadDto
                       {
                           Id = d.Id,
                           colaborador_id = d.colaborador_id,
                           Colaboradores = d.Colaboradores,
                           catalogo_responsable_id = d.catalogo_responsable_id,
                           acceso = d.acceso,
                       }).ToList();


            return res;
        }
    }
}
