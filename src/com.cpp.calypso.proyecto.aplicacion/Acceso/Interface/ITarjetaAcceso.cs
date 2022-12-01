using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.proyecto.aplicacion.Acceso.Dto;
using com.cpp.calypso.proyecto.aplicacion.Dto;
using com.cpp.calypso.proyecto.dominio;
using com.cpp.calypso.proyecto.dominio.Acceso;

namespace com.cpp.calypso.proyecto.aplicacion.Acceso.Interface
{
    public interface ITarjetaAccesoAsyncBaseCrudAppService : IAsyncBaseCrudAppService<TarjetaAcceso, TarjetaAccesoDto, PagedAndFilteredResultRequestDto>
    {
        List<TarjetaAccesoDto> GetByColaborador(int colaboradorId);

        void SwitchEntregado(int tarjetaId);

        void AnularTarjeta(ActualizarTarjetaDto input, int archivoId = 0);

        void SubirPdf(int tarjetaId, ArchivoDto archivo);

        bool PuedeCrear(int colaboradorId);

        int obtenersecuencialtarjetas(int id);
        string obtenersolicitudpamactiva();

        

    }
}
