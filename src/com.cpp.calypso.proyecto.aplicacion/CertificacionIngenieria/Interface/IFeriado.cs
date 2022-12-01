using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.proyecto.aplicacion.CertificacionIngenieria.Dto;
using com.cpp.calypso.proyecto.dominio.CertificacionIngenieria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion.CertificacionIngenieria.Interface
{
    public interface  IFeriadoAsyncBaseCrudAppService : IAsyncBaseCrudAppService<Feriado, FeriadoDto, PagedAndFilteredResultRequestDto>
    {
        List<FeriadoDto> GetFeriados();

        Task<bool> CrearFeriadoAsync(FeriadoDto dto);

        Task<bool> ActualizarFeriadoAsync(FeriadoDto dto);

        bool EliminarFeriado(int id);
    }
}
