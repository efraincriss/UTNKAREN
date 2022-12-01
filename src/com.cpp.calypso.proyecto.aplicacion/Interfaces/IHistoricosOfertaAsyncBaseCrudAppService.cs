using com.cpp.calypso.comun.aplicacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion
{
    public interface IHistoricosOfertaAsyncBaseCrudAppService : IAsyncBaseCrudAppService<HistoricosOferta, 
                     HistoricosOfertaDto, PagedAndFilteredResultRequestDto>
    {
        void CreateHistoricoOferta(HistoricosOferta historicosOferta, int estado, int IdestadoTemp);
        List<HistoricosOfertaDto> ListarHistoricosOferta(int idOferta);
        int EliminarVigencia(int historicoId);
        HistoricosOfertaDto GetHistoricosOferta(int id);
    }
}
