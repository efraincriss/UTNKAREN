using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.proyecto.aplicacion.RecursosHumanos.Dto;
using com.cpp.calypso.proyecto.dominio.RecursosHumanos;

namespace com.cpp.calypso.proyecto.aplicacion.RecursosHumanos.Interfaces
{
    public interface IDetalleActualizacionSueldoAsyncBaseCrudAppService : IAsyncBaseCrudAppService<DetalleActualizacionSueldo, DetalleActualizacionSueldoDto, PagedAndFilteredResultRequestDto>
    {
    }
}
