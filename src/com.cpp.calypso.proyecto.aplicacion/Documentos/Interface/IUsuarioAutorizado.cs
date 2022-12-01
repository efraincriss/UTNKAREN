using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.aplicacion.Documentos.Dto;
using com.cpp.calypso.proyecto.dominio.Documentos;
using com.cpp.calypso.seguridad.aplicacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion.Documentos.Interface
{
    public interface IUsuarioAutorizadoAsyncBaseCrudAppService : IAsyncBaseCrudAppService<UsuarioAutorizado, UsuarioAutorizadoDto, PagedAndFilteredResultRequestDto>
    {
        List<UsuarioDto> ObtenerUsuariosAutorizadosPorContratoId(int id);

        List<UsuarioDto> ObtenerUsuariosDisponiblesPorContrato(int id);

        void AgregarUsuarios(List<int> usuarios, int carpetaId);

        ResultadoEliminacionResponse EliminarUsuarioAutorizado(int usuario, int carpetaId);


    }
}
