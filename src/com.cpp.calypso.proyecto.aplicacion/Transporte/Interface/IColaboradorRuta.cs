using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.proyecto.aplicacion.Dto;
using com.cpp.calypso.proyecto.aplicacion.Transporte.Dto;
using com.cpp.calypso.proyecto.aplicacion.Transporte.Models;
using com.cpp.calypso.proyecto.dominio.Transporte;

namespace com.cpp.calypso.proyecto.aplicacion.Transporte.Interface
{
    public interface IColaboradorRutaAsyncBaseCrudAppService : IAsyncBaseCrudAppService<ColaboradorRuta, ColaboradorRutaDto, PagedAndFilteredResultRequestDto>
    {
        List<ColaboradorRutaDto> Listar();
        List<ColaboradorRutaDto> ListarbyColaborador(int id);//ColaboradorId
        int Ingresar(ColaboradorRuta ruta);
        int Editar(ColaboradorRuta ruta);
        int Eliminar(int id);
        ColaboradorRuta GetDetalles(int id);

        ColaboradoresDetallesDto BuscarColaborador(string NumeroIdentificacion);

        ColaboradorRuta BuscarColaboradoRuta(int ColaboradorId, int RutaHorarioId,int Id);
       
        List<RutaHorarioDto> ListaRutasHorario(int id);
        Ruta GetDetallesRuta(int id);
        Task EnviarMensajeAsync(int id);

        #region Reportes Transporte
        //Reportes Transportes//

        List<PersonasTransportadasDto> ReportePersonasTransportadas(InputReporteTransporte i);
        List<DiarioViajeVehiculoDto> ReporteDiarioViajesProveedor(InputReporteTransporte i);
        List<TrabajoDiarioDto> ReporteDiarioTrabajo(InputReporteTransporte i);

        #endregion


    }
}
