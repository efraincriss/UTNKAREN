using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.proyecto.aplicacion.Dto;
using com.cpp.calypso.proyecto.dominio;

namespace com.cpp.calypso.proyecto.aplicacion.Interfaces
{
    public interface IDetalleAvanceProcuraAsyncBaseCrudAppService : IAsyncBaseCrudAppService<DetalleAvanceProcura, DetalleAvanceProcuraDto, PagedAndFilteredResultRequestDto>
    {
        DetalleAvanceProcuraDto getdetalles(int DetalleAvanceProcuraId);
        List<DetalleAvanceProcuraDto> listarporavanceprocura(int AvanceProcuraId);
        List<ComputoDto> GetComputos(int ofertaId);

        decimal calcularvalor(int AvanceprocuraId);

        int EliminarVigencia(int AvanceprocuraId);
        decimal obtenercalculoanterior(int ComputoId);

        decimal montoanteriores(int AvanceprocuraId);
        decimal montoacumulados(int AvanceprocuraId);
        decimal montoactual(int AvanceprocuraId);
        decimal montopresupuesto(int AvanceprocuraId);
        bool cambiaracertificado(int id);

    }
}
