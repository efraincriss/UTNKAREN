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
    public interface IColaboradoresVisitaAsyncBaseCrudAppService : IAsyncBaseCrudAppService<ColaboradoresVisita, ColaboradoresVisitaDto, PagedAndFilteredResultRequestDto>
    {
        ColaboradoresVisitaDto GetVisitasPorColaborador(int id);
        List<ColaboradoresVisitaDto> GetVisitaUsuariosExternos();

        bool SwitchEstadoColaboradorExterno(int Id);

        string DeleteColaboradorExterno(int Id);
    }
}
