using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.proyecto.dominio;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion.Interfaces
{
    public interface IPreciarioAsyncBaseCrudAppService : IAsyncBaseCrudAppService<Preciario, PreciarioDto, PagedAndFilteredResultRequestDto>
    {

        List<Preciario> GetPreciarios();
       PreciarioDto GetDetalle(int PreciarioId);
        Preciario GetPreciarioContrato(int ContratoId);
        bool ComprobarExistenciaPreciarioContrato(DateTime fechainicio,DateTime fechafin,int ContratoId);
        bool ComprobarExistenciaPreciarioContratoEdit(int idpreciario,DateTime fechainicio, DateTime fechafin, int ContratoId);
        int EliminarVigenciaAsync(int preciarioId);
        PreciarioDto preciarioporcontratofecha(int ContratoId, DateTime? FechaOferta);

        int ClonaPreciario(int preciarioid);



        decimal ObtenerPrecioUnitarioItem(int ItemId, int OfertaId);

    }
}
