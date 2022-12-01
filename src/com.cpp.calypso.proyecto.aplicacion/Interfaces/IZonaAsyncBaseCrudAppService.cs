using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.proyecto.dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion
{
    public interface IZonaAsyncBaseCrudAppService : IAsyncBaseCrudAppService<Zona, ZonaDto, PagedAndFilteredResultRequestDto>
    {
        List<ZonaDto> ObtenerTodos();
        List<TreeItem> GenerarArbolZonasFrente();
        TreeItem ExtraerHijos(Zona zona);
        void DeleteZona(int Id);
        void UpdateZona(int Id, string nombre, string descripcion);
        int CreateZona(string codigo, string nombre, string descripcion);
    }
}
