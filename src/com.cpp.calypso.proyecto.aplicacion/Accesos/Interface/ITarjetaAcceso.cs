using System.Collections.Generic;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.proyecto.dominio;

namespace com.cpp.calypso.proyecto.aplicacion
{
    public interface ITarjetaAccesoAsyncBaseCrudAppService : IAsyncBaseCrudAppService<TarjetaAcceso, TarjetaAccesoDto, PagedAndFilteredResultRequestDto>
    {
        List<TarjetaAccesoDto> GetByColaborador(int colaboradorId);

        void SwitchEntregado(int tarjetaId);

        void AnularTarjeta(ActualizarTarjetaDto input, int archivoId = 0);

        void SubirPdf(int tarjetaId, ArchivoDto archivo);

        bool PuedeCrear(int colaboradorId);

    }
}
