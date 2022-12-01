using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.proyecto.dominio;

namespace com.cpp.calypso.proyecto.aplicacion.Interfaces
{
    public interface IDetalleOrdenCompraAsyncBaseCrudAppService : IAsyncBaseCrudAppService<DetalleOrdenCompra, DetalleOrdenCompraDto, PagedAndFilteredResultRequestDto>
    {

        List<DetalleOrdenCompraDto> listar(int ordenCompraId);

        DetalleOrdenCompraDto GetDetalles(int detalleOrdenCompraId);


        int Eliminar(int detalleOrdenCompraId);
        List<Item> Itemsoferta(int OfertaId);

        decimal calcularvalor(int ordenCompraId);

        List<ComputoDto> GetComputos(int ofertaId);

        bool comprobarfechaitem(int OrdenCompraId, int ItemId, DateTime fecha);

        List<DetalleOrdenCompraDto> GetComputosOrdenescompra(int OrdenCompraId);

        bool PasarOrdenAprobado(int id);
        bool PasarOrdenRegistrado(int id);
        List<DetalleOrdenCompraDto> listarporoferta(int OfertaId);

        decimal porcentaje(DetalleOrdenCompraDto detalle);
        decimal comprobarporcentaje(int ComputoId);

    }
}
