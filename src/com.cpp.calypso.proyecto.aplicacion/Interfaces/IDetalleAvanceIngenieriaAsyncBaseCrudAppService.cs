using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.proyecto.aplicacion.Dto;
using com.cpp.calypso.proyecto.dominio;

namespace com.cpp.calypso.proyecto.aplicacion
{
    public interface IDetalleAvanceIngenieriaAsyncBaseCrudAppService : IAsyncBaseCrudAppService<DetalleAvanceIngenieria, DetalleAvanceIngenieriaDto, PagedAndFilteredResultRequestDto>
    {

        List<DetalleAvanceIngenieriaDto> ListarPorAvanceIngenieria(int avanceIngenieriaId);

        List<ComputoDto> GetComputos(int ofertaId);

        decimal ObtenerCantidadAcumulada(int computoId, DateTime fecha_reporte, int ofertaId);

        //calculo de monto de ingenieria
        decimal calcularvalor(int AvanceIngenieriaid);
        bool cambiaracertificado(int id);
    }
}
