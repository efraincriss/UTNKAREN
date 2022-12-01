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
    public interface ISeccionAsyncBaseCrudAppService : IAsyncBaseCrudAppService<Seccion, SeccionDto, PagedAndFilteredResultRequestDto>
    {
        List<EstructuraArbol> GenerarArbol(int documentoId);

        Task<bool> CrearSeccionAsync(SeccionDto dto);

        bool ActualizarSeccion(SeccionDto dto);

        ResultadoEliminacionResponse EliminarSeccion(int id);
        ResultadoEliminacionResponse EliminarImagen(int id);

        SeccionDto ObtenerSeccionPorId(int id);

        bool GuardarArbolDragDrop(List<EstructuraArbol> data);

        List<SeccionDto> ObtenerSeccionesFiltros(int carpetaid, string tipoDocumento, int DocumentoId, int SeccionId, string palabra, bool soloTitulos);

        void actualizarSeccion();

   

    }
}
