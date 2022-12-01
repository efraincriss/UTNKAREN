using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.proyecto.aplicacion.Documentos.Dto;
using com.cpp.calypso.proyecto.dominio.Documentos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion.Documentos.Interface
{
    public interface IImagenSeccionAsyncBaseCrudAppService : IAsyncBaseCrudAppService<ImagenSeccion, ImagenSeccionDto, PagedAndFilteredResultRequestDto>
    {
        Task<bool> CrearImagenSeccionAsync(ImagenSeccionDto dto);

        Task<string> CrearImagenesSeccionAsync(List<ImagenSeccionDto> imagenes);

        ResultadoEliminacionResponse EliminarImagen(int id);

        List<ImagenSeccionDto> ObtenerImagensPorSeccion(int seccionId);


    }
}
