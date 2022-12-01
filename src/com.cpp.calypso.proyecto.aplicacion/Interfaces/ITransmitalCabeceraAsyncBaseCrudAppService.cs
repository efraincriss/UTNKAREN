using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.proyecto.dominio;
using Xceed.Words.NET;

namespace com.cpp.calypso.proyecto.aplicacion.Interfaces
{
   public  interface ITransmitalCabeceraAsyncBaseCrudAppService : IAsyncBaseCrudAppService<TransmitalCabecera, TransmitalCabeceraDto, PagedAndFilteredResultRequestDto>
    {

       TransmitalCabeceraDto GetDetalle(int TransmitalId);
        List<TransmitalCabeceraDto> GetTransmitalCabeceras(int OfertaId);
        List<TransmitalCabeceraDto> GetAllTransmitalCabeceras();
        bool EliminarVigencia(int Transmitalid);

        String CrearTransmital(TransmitalCabecera transmital);
        String EditarTransmital(TransmitalCabecera transmital);
        String DeleteTransmital(int Id);

        String CrearTransmitalOfertaComercial(int id, TransmitalCabecera transmistal); // OfertaComercial

        List<Colaborador> ListaColaboradoresTransmital();

        //
        string GenerarWord(int id);
        string GenerarWordTransmittal(int OfertaId);
        //Obtener Transmitl por oferta

        TransmitalCabecera IdOfertaComercialTransmital(int id);


        //Transmital Detalles.
        bool CrearDetalle(TransmitalDetalle d);
        string EditDetalle(TransmitalDetalle d);
        int CrearArchivo(HttpPostedFileBase e);
        bool DeleteDetalle(int Id);
        bool SearchDefinitiva(TransmitalDetalle d);

        bool tieneTransmital(int Id);
        string nombresTransmital(int Id);
        int secuencialTransmital(int ClienteId);
    }
}
