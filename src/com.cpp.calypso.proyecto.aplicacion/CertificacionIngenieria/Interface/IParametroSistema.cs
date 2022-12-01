using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.aplicacion.CertificacionIngenieria.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion.CertificacionIngenieria.Interface
{
    public interface IParametroSistemaAsyncBaseCrudAppService : IAsyncBaseCrudAppService<ParametroSistema, ParametroSistemaDto, PagedAndFilteredResultRequestDto>
    {
        List<ParametroSistemaDto> ObtenerParametrosPorModuloCertificacion();

        Task<bool> ActualizarParametroAsync(ParametroSistemaDto dto);
    }
}
