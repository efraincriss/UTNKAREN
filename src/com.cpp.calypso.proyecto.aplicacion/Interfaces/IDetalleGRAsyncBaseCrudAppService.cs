using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.proyecto.dominio;

namespace com.cpp.calypso.proyecto.aplicacion
{
    public interface IDetalleGRAsyncBaseCrudAppService : IAsyncBaseCrudAppService<DetalleGR, DetalleGRDto, PagedAndFilteredResultRequestDto>
    {
        List<DetalleGRDto> ListarPorGr(int id); // GrId

        Task<int> CrearDetalles(int[] idsCertificados, int GrId);

        Task Eliminar(int id);
    }
}
