using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.proyecto.aplicacion.CertificacionIngenieria.Dto;
using com.cpp.calypso.proyecto.aplicacion.CertificacionIngenieria.Models;
using com.cpp.calypso.proyecto.dominio;
using com.cpp.calypso.proyecto.dominio.CertificacionIngenieria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion.CertificacionIngenieria.Interface
{
    public interface IDetalleDirectoE500AsyncBaseCrudAppService : IAsyncBaseCrudAppService<DetalleDirectoE500, DetalleDirectoE500Dto, PagedAndFilteredResultRequestDto>
    {

         List<Proyecto> ObtenerProyectos();

         List<DetallesDirectosIngenieriaDto> ObtenerDetallesDirectosProyecto(int ProyectoId);

         List<DetalleDirectoE500Dto> ObtenerDetallesDirectosE500();

        string DistribuirHorasDirectasaProyecto(int Id, List<E500Distribucion> temporales);

        string EnviaraE500(int[] Id);

        string EnviaraDirectosaOtroProyecto(int ProyectoDestinoId,int[] Id);



    }
}
