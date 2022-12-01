using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.proyecto.dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion.Interfaces
{
    public interface IComputosTemporalAsyncBaseCrudAppService : IAsyncBaseCrudAppService<ComputosTemporal, ComputosTemporalDto, PagedAndFilteredResultRequestDto>
    {
        Task NuevaVersionPresupuestoAsync(int OfertaId);
        List<ComputosTemporalDto> GenerarNuevaVersion(int OfertaId);
        List<ComputosTemporalDto> GetComputosTempPorOferta(int OfertaId);
        void Actualizar(int id, decimal cantidad_eac, decimal precio_ajustado);
        bool ActualizarValorEac(int idComputo, decimal valorEac);
        Task<ComputosTemporalDto> GetDetalle(int ComputoId);
        List<ComputosTemporalDto> GetComputosporOferta(int OfertaId);
        List<Item> GetComputosporOfertaNovalidos(int OfertaId);
        //List<ComputoDto> GetComputosporWbsOferta(int WbsOfertaId, DateTime? fecha = null);
        bool comprobarexistecomputo(int WbsOfertaId);
        bool comprobarexistenciaitem(int WbsOfertaId, int ItemId);
        List<ComputoDto> GetComputosPorOferta(int id);
        void CalcularPresupuesto(int ContratoId, DateTime FechaOferta);
        ComputoDto ActualizarprecioAjustado(int PreciarioId, ComputoDto seleccionado);
        String ActualizarCostoTotal(int ofertaid, int ContratoId, int PreciarioId, DateTime FechaOferta, Boolean Validado);
        string nombrecatalogo(int tipocatagoid);
        List<TreeWbs> TreeComputo(int WbsOfertaId);
        Task<bool> EliminarVigencia(int ComputoId);
        bool EditarCantidadComputoTemporal(int id, decimal cantidad, decimal cantidad_eac);
        ComputoDto GetCabeceraApi(int id);
        List<ComputoDto> GrupoComputosporOferta(int OfertaId);
        List<ComputoDto> GetComputosPorOfertaList(int[] computos);
    }
}
