using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.aplicacion.RecursosHumanos.Dto;
using com.cpp.calypso.proyecto.aplicacion.RecursosHumanos.Interfaces;
using com.cpp.calypso.proyecto.dominio.RecursosHumanos;

namespace com.cpp.calypso.proyecto.aplicacion.RecursosHumanos.Services
{
    public class DetalleActualizacionSueldoAsyncBaseCrudAppService : AsyncBaseCrudAppService<DetalleActualizacionSueldo, DetalleActualizacionSueldoDto, PagedAndFilteredResultRequestDto>, IDetalleActualizacionSueldoAsyncBaseCrudAppService
    {
        public DetalleActualizacionSueldoAsyncBaseCrudAppService(
            IBaseRepository<DetalleActualizacionSueldo> repository
        ) : base(repository)
        {
        }
    }
}
