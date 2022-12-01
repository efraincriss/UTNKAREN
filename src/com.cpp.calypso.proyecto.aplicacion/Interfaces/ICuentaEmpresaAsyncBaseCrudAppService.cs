using com.cpp.calypso.comun.aplicacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.cpp.calypso.proyecto.dominio;

namespace com.cpp.calypso.proyecto.aplicacion
{
    public interface ICuentaEmpresaAsyncBaseCrudAppService : IAsyncBaseCrudAppService<CuentaEmpresa, CuentaEmpresaDto, PagedAndFilteredResultRequestDto>
    {

        CuentaEmpresaDto GetDetalles(int id);

        int EliminarVigencia(int representanteId);
    }
}
