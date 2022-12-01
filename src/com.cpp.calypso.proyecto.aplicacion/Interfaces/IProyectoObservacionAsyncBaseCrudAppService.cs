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
    public interface IProyectoObservacionAsyncBaseCrudAppService : IAsyncBaseCrudAppService<ProyectoObservacion, ProyectoObservacionDto, PagedAndFilteredResultRequestDto>
    {
        int Eliminar(int ObservacionId);

        List<ProyectoObservacionDto> ListarPorProyecto(int ProyectoId);
        List<ProyectoObservacionDto> ListarPorProyectoTipo(int ProyectoId, TipoComentario Tipo);

        Proyecto DetallesProyecto(int Id);

        List<Catalogo> ObtenerCatalogos(string code);


        List<PrecipitacionDto> ListarPrecipiatacionesPorProyecto(int ProyectoId);

        //PRECIPITACIONES

        int NuevaPrecipitacion(Precipitacion nueva);
        int EditarPrecipitacion(Precipitacion precipitacion);
        int EliminarPrecipitacion(int Id);

        string CambiarRDOaRSO(int ProyectoId,bool esRSO);
    }
}
