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
    public interface IColaboradorIngenieriaAsyncBaseCrudAppService : IAsyncBaseCrudAppService<ColaboradorIngenieria, ColaboradorIngenieriaDto, PagedAndFilteredResultRequestDto>
    {

        int InsertColaborador(ColaboradorIngenieria e);
        int EditColaborador(ColaboradorIngenieria e);
        bool DeleteColaborador(int id);

        List<ColaboradorIngenieriaDto> Listado();
        List<ColaboradorIngenieriaDto> ListByContrato(int id);
        List<ColaboradorIngenieriaDto> ListByOferta(int id);

        ColaboradorIngenieria Search(string numero_identificacion);

        List<DetallePreciario> ListarCargosByContrato(int id);

        List<Contrato> ListarContratos();


    }
}
