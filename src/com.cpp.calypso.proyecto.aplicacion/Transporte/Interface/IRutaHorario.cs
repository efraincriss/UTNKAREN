using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.proyecto.aplicacion.Transporte.Dto;
using com.cpp.calypso.proyecto.dominio.Transporte;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion.Transporte.Interface
{
    public interface IRutaHorarioAsyncBaseCrudAppService : IAsyncBaseCrudAppService<RutaHorario, RutaHorarioDto, PagedAndFilteredResultRequestDto>
    {
        int Ingresar(RutaHorario rutaHorrario);
        int Editar(RutaHorario rutaHorrario);
        int Eliminar(int id);

        bool mismohorario(int rutaid, TimeSpan hora);
    }
}