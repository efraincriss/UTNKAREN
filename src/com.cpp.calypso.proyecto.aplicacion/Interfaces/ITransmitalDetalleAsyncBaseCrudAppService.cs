using com.cpp.calypso.proyecto.dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.proyecto.aplicacion.Dto;

namespace com.cpp.calypso.proyecto.aplicacion.Interfaces
{
    public interface ITransmitalDetalleAsyncBaseCrudAppService : IAsyncBaseCrudAppService<TransmitalDetalle, TransmitalDetalleDto, PagedAndFilteredResultRequestDto>
    {

        TransmitalDetalleDto GetDetalle(int TransmitalDetalleId);
        List<TransmitalDetalleDto> GetTransmitalDetalles(int TransmitalId);
        bool EliminarVigencia(int Transmitadetalleid);

        bool existe_esoferta(int id);// Transmital CABECERA ID
    }
}
