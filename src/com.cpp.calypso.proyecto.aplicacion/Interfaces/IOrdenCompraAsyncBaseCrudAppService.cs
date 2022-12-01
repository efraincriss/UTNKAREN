using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.proyecto.dominio;

namespace com.cpp.calypso.proyecto.aplicacion.Interfaces
{
   public  interface IOrdenCompraAsyncBaseCrudAppService : IAsyncBaseCrudAppService<OrdenCompra, OrdenCompraDto, PagedAndFilteredResultRequestDto>
   {

       List<OrdenCompraDto> listar(int ofertaId);

       int EliminarVigencia(int ordenCompraId);

       void ActualizarMontosOrdenCompra(int ordenCompraId);

       List<OrdenCompraDto> ListarPorProyecto(int proyectoId);

     OrdenCompraDto getdetalles(int OrdenCompraId);

    }
}

