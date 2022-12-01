using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.proyecto.dominio;

namespace com.cpp.calypso.proyecto.aplicacion 
{
    public interface IProyectoAsyncBaseCrudAppService : IAsyncBaseCrudAppService<Proyecto, ProyectoDto, PagedAndFilteredResultRequestDto>
    {

        ProyectoDto GetDetalles(int proyectoId);
        List<Proyecto> GetProyectos();
        List<RequerimientoDto> RequerimientosDelProyecto(int proyectoid);
        bool EliminarVigencia(int proyectoId);
        bool CambiarSecuencial(int proyectoId,string tipo);


        List<ProyectoDto> ObtenerProyectosPorContrato(int contratoId);

        void ActualizarMontosProyecto(int ProyectoId);

        bool comprobacionfechainiciofinin(DateTime fechai, DateTime fechafin);

        bool comprobacionfechaacta(DateTime fechaa, DateTime fechai, DateTime fechafin);

        bool existeproyecto(string codigoproyecto);
        bool existeproyectoc(ProyectoDto p);
        

        List<ProyectoDto> Listar();
        List<ProyectoDto> ListarCambiarProyectoRequerimiento();
    }
}
