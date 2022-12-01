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
    public interface IColaboradoresHuellaDigitalAsyncBaseCrudAppService : IAsyncBaseCrudAppService<ColaboradoresHuellaDigital, ColaboradoresHuellaDigitalDto, PagedAndFilteredResultRequestDto>
    {
        List<ColaboradoresHuellaDigitalDto> GetHuellasPorColaborador(int Id);
        ColaboradoresHuellaDigital GetHuellaDigital(int Id);

        ColaboradoresHuellaDigital GetHuellaPorDedoColaborador(int IdColoaborador, int IdDedo);
        Task<string> CrearHuellasPorColaboradorAsync(ColaboradoresHuellaDigitalDto colaboradoresHuellaDigital);

        Task<string> ActualizarHuellasPorColaboradorAsync(ColaboradoresHuellaDigitalDto colaboradoresHuellaDigital);
        bool EliminarHuella(int Id);
    }
}
