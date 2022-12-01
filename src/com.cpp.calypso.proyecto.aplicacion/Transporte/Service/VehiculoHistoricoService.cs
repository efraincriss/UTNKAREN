using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.aplicacion.Transporte.Dto;
using com.cpp.calypso.proyecto.aplicacion.Transporte.Interface;
using com.cpp.calypso.proyecto.dominio.Transporte;

namespace com.cpp.calypso.proyecto.aplicacion.Transporte.Service
{
    public class VehiculoHistoricoAsyncBaseCrudAppService : AsyncBaseCrudAppService<VehiculoHistorico, VehiculoHistoricoDto, PagedAndFilteredResultRequestDto>, IVehiculoHistoricoAsyncBaseCrudAppService
    {
        public VehiculoHistoricoAsyncBaseCrudAppService(
            IBaseRepository<VehiculoHistorico> repository
            ) : base(repository)
        {
        }
    }
}
