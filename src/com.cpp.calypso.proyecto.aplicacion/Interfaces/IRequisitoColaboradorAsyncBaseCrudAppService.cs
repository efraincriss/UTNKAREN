using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.proyecto.aplicacion.Dto;
using com.cpp.calypso.proyecto.dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion.Interfaces
{
    public interface IRequisitoColaboradorAsyncBaseCrudAppService : IAsyncBaseCrudAppService<RequisitoColaborador, RequisitoColaboradorDto, PagedAndFilteredResultRequestDto>
    {

        List<RequisitoColaboradorDto> GetList();
        RequisitoColaboradorDto GetRequisito(int Id);
		List<RequisitoColaboradorDto> GetRequisitoPorGrupo(int grupo);

        List<RequisitoColaboradorDto> GetRequisitosPorAccionApi(int idAccion, int idGrupoPersonal);
        string UniqueRequisito(int idAccion, int idGrupoPersonal, int idRequisito, int id);
        List<RequisitoColaboradorDto> GetRequisitosPorFiltros(int? tipo_usuario, int? accion, string requisitos);

    }
}
