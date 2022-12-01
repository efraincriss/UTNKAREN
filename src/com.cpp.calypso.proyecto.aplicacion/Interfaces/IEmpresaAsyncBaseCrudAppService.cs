using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.proyecto.aplicacion;
using com.cpp.calypso.proyecto.dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion.Interfaces
 
{
    public interface IEmpresaAsyncBaseCrudAppService : IAsyncBaseCrudAppService<Empresa, EmpresaDto, PagedAndFilteredResultRequestDto>
    {

        Task<EmpresaDto> GetDetalle(int empresaId);

        Task CancelarVigencia(int empresaId);

        List<Empresa> GetEmpresas();

        List<EmpresaDto> GetEmpresasApi();

        List<RepresentanteEmpresaDto> GetRepresentanteEmpresa(int empresaId);

        List<CuentaEmpresaDto> GetCuentasEmpresa(int empresaId);

        bool ComprobarYBorrarEmpresa(int empresaId);

        Task<string> CrearEmpresaAsync(EmpresaDto empresa);

    }
}
