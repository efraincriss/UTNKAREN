using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.proyecto.aplicacion.Dto;
using com.cpp.calypso.proyecto.aplicacion.Proveedor.Dto;
using com.cpp.calypso.proyecto.dominio;
using com.cpp.calypso.proyecto.dominio.Proveedor;

namespace com.cpp.calypso.proyecto.aplicacion.Proveedor.Interfaces
{
    public interface IReservaHotelAsyncBaseCrudAppService : IAsyncBaseCrudAppService<ReservaHotel, ReservaHotelDto, PagedAndFilteredResultRequestDto>
    {
        List<EspacioHabitacionDto> BuscarEspaciosLibres(DateTime fechaInicio, DateTime fechaFin);

        void CrearDetallesReserva(int ReservaHotelId, DateTime fechaInicio, DateTime fechaFin);

        List<ColaboradorNombresDto> BuscarPorIdentificacionNombre(string identificacion = "", string nombre = "");

        List<ColaboradorNombresDto> BuscarColaboradoresHospsdaje(string identificacion = "", string nombre = "");

        List<ReservaHotelDto> ListarReservas(DateTime fechaInicio, DateTime fechaFin);

        bool EliminarReserva(int reservaId);

        bool EditarReserva(int reservaId, DateTime fechaHasta);

        List<HotelHabitacionDto> ListarHoteles(DateTime fecha);

        string ReservaValidada(int colaboradorId, DateTime fechaInicio, DateTime fechaFin, int espacioId);

        List<TarifaTipoHabitacionDto> ListarTarifasTipoHabitacion(int proveedorId);

        //RESERVAS EXTEMPORANEAS
        string CambiarEstadoDetallesConsumidoE(List<DetalleReservaDto> data);
        string CambiarEstadoDetallesNoConsumidoE(List<DetalleReservaDto> data);

        string CambiarEstadoDetallesLavanderia(List<DetalleReservaDto> data);
        string CambiarEstadoDetallesNoLavanderia(List<DetalleReservaDto> data);

        string ReservaExtemporaneaValidada(int colaboradorId, DateTime fechaInicio, DateTime fechaFin, int espacioId);

        int GuardarArchivo(HttpPostedFileBase archivo);

        int CrearReservaExtemporanea(ReservaHotelDto dto, HttpPostedFileBase archivo);
        string UpdateServicioHospedajeColaborador(int id); //Id is Colaborador Id

        Task<bool> SendMessageAsync(int id,string tipo);

        string DiasJornadaCampo();


        bool Activar(string pass);


        string Iniciar_FinalizarConsumo(int id,bool inicio, DateTime fecha, string justificacion);
        string EditarFecha(int id, bool inicio, DateTime fecha, string justificacion);
    }
}
