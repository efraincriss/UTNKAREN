using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.proyecto.dominio;

namespace com.cpp.calypso.proyecto.aplicacion.Interfaces
{
    public interface IOfertaAsyncBaseCrudAppService : IAsyncBaseCrudAppService<Oferta, OfertaDto, PagedAndFilteredResultRequestDto>
    {
        
        void ActualizarVersion(int idOferta);

        void Aprobar(int idOferta);
        bool EliminarVigencia(int ofertaId);
        List<Oferta> GetOfertas();
        int ClonarOferta(int ofertaId, int proyectoId, int requerimientoId);
        int ObtenerIdProyecto(int requerimientoId);
        List<OfertaDto> listarPorProyectoId(int proyectoId);
        List<OfertaDto> ListarPorRequerimiento(int RequerimientoId);
        string GetCodigoClienteYProyecto(int OfertaId);
        OfertaDto getdetalle(int OfertaId);
        List<OfertaDto> ListarPorContrato(int ContratoId);
        Task<string> FormatoCorreoOferta(int procesoId, int ofertaId, string[] correos);
        string ObtenerSecuencial(int proyectoId);
        OfertaDto GetOfertaDefinitiva(int requerimientoId);
        Task ActualizarFechaPresupuestoAsync(int ofertaId);
        void ActualizarComputoCompleto(int ofertaId);
        List<OfertaDto> ListarPorRequerimientoDefinitivas(int RequerimientoId);
        List<OfertaDto> listarPorProyectoDefinitivaId(int reqId);
        List<Oferta> GetOfertasDefinitivas();

        List<OfertaDto> TodasOfertasDefiniticas();

        Task<int> CrearPresupuesto(OfertaDto presupuesto);

        OfertaDto DetallePresupuestoConEnumerable(int OfertaId);

        Task ActualizarPresupuesto(OfertaDto p);

        Task<bool> AprobarPresupuesto(int OfertaId);

        Task<bool> DesaprobarPresupuesto(int OfertaId);

        // Base Rdo

        string ClonarOfertaPresupuesto(int requerimientoId);

        string CargarPresupuestoInicial(int RequerimientoId);
        string CargarPresupuestoBaseRdo(int RequerimientoId);
        string ActualizarCantidadesPresupuestoActual(int RequerimientoId);


        int EliminarOferta(int id);
    }
}
