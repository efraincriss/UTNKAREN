using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.proyecto.aplicacion.Proveedor.Dto;
using com.cpp.calypso.proyecto.dominio.Proveedor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion.Proveedor.Interfaces
{
    public interface ITarifaLavanderiaAsyncBaseCrudAppService : IAsyncBaseCrudAppService<TarifaLavanderia, TarifaLavanderiaDto, PagedAndFilteredResultRequestDto>

    {
        List<TarifaLavanderiaDto> ListarPorContrato(int ContratoId);
        bool TarifaUnica(int contratoProveedorId, int tipoServicioId);
        bool CrearTarifa(TarifaLavanderia entity);
         bool EditarTarifa(TarifaLavanderia entity);
        bool EliminarTarifa(int id);

        void DesactivarTarifa(int Id);

        void ActivarTarifa(int Id);
    }
}
