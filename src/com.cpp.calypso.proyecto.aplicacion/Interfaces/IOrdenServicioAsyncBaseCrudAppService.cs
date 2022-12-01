using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.proyecto.dominio;
using com.cpp.calypso.proyecto.dominio.Models;
using OfficeOpenXml;

namespace com.cpp.calypso.proyecto.aplicacion
{
    public interface IOrdenServicioAsyncBaseCrudAppService : IAsyncBaseCrudAppService<OrdenServicio, OrdenServicioDto, PagedAndFilteredResultRequestDto>
    {
        OrdenServicioDto llenarCabecera(int ofertaId);
        List<OrdenServicioDto> listar(int ofertaId);

        int EliminarVigencia(int ordenServicioId);

        void ActualizarMontosOrdenServicio(int ordenServicioId);

        List<OrdenServicioDto> ListarPorProyecto(int proyectoId);
        List<OrdenServicioDto> ListarOsByOferta(int OfertaId);
        List<OrdenServicioDto> ListarProyectoDetalles(int proyectoId);

        List<OrdenServicio> ListaOSOfertaComercial(int Id);//OfertaComercial;

        int CrearOs(OrdenServicio ordenservicio);
        OrdenServicio Detalles(int Id);
        List<OrdenServicio> Listar();
        /* Real Debug*/
        List<OrdenServicioModel> GetLista();
        string InsertOrden(OrdenServicio o);
        string EditOrden(OrdenServicio o);
        string DeleteOrden(int id);
         List<ModelClassReact> ListProyectos();
         List<ModelClassReact> ListOfertas();
         List<ModelClassReact> ListGrupoItem();

        List<DetalleOrdenServicioDto> ListDetallesByOrden(int Id);

        string InsertDetalleOrden(DetalleOrdenServicio o);
        string EditDetalleOrden(DetalleOrdenServicio o);

        string DeleteDetalleOrden(int id);

        bool UpdateMontosOs(int Id);
        OrdenServicioModel GetOSDetalle(int Id);

        ExcelPackage ReportePOS(ReportDto r);

    }
}
