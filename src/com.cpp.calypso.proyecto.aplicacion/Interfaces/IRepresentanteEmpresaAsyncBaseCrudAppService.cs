using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.proyecto.aplicacion;
using com.cpp.calypso.proyecto.dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion
{
    public interface IRepresentanteEmpresaAsyncBaseCrudAppService : IAsyncBaseCrudAppService<RepresentanteEmpresa, RepresentanteEmpresaDto, PagedAndFilteredResultRequestDto>
    {
        RepresentanteEmpresaDto GetDetalles(int id);

        int EliminarVigencia(int representanteId);

        string CrearRepresentante(RepresentanteEmpresaDto representanteEmpresa);
    }
}
