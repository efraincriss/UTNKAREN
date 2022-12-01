using com.cpp.calypso.comun.aplicacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.cpp.calypso.proyecto.aplicacion.Proveedor.Dto;
using com.cpp.calypso.proyecto.dominio.Proveedor;

namespace com.cpp.calypso.proyecto.aplicacion.Proveedor.Interfaces
{
    public interface ITarifaHotelAsyncBaseCrudAppService : IAsyncBaseCrudAppService<TarifaHotel, TarifaHotelDto, PagedAndFilteredResultRequestDto>
    {
        void DesactivarTarifa(int TarifaHotelId);

        void ActivarTarifa(int tarifaId);

        List<TarifaHotelDto> ListarPorContrato(int ContratoId);

        bool TarifaUnica(int contratoProveedorId, int tipoHabitacionId);
    }
}
