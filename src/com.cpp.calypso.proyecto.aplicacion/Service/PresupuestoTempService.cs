using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.dominio;

namespace com.cpp.calypso.proyecto.aplicacion
{
    public class PresupuestoTempAsyncBaseCrudAppService : AsyncBaseCrudAppService<PresupuestoTemp, PresupuestoTempDto, PagedAndFilteredResultRequestDto> , IPresupuestoTempAsyncBaseCrudAppService
    {
        public PresupuestoTempAsyncBaseCrudAppService(
            IBaseRepository<PresupuestoTemp> repository
        ) : base(repository)
        {
        }
    }
}
