using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.proyecto.dominio;

namespace com.cpp.calypso.proyecto.aplicacion
{
    public interface IInstitucionFinancieraAsyncBaseCrudAppService : IAsyncBaseCrudAppService<InstitucionFinanciera, InstitucionFinancieraDto, PagedAndFilteredResultRequestDto>
    {
        List<InstitucionFinanciera> GetInstitucionesFinancieras();
        bool Eliminar(int institucionId);
        List<InstitucionFinancieraDto> GetInstitucionesFinancierasDto();
    }
}
