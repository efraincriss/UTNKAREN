using com.cpp.calypso.comun.aplicacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion
{
    public interface IZonaFrenteAsyncBaseCrudAppService : 
        IAsyncBaseCrudAppService<ZonaFrente, ZonaFrenteDto, PagedAndFilteredResultRequestDto>
    {
        List<ZonaFrenteDto> GetFrentesPorZona(int ZonaId);

        List<ZonaFrenteDto> GetFrentes();
        void CrearFrente(ZonaFrenteDto zonaFrente);
        void DeleteFrente(int Id);
        void UpdateFrente(int Id, string nombre, string descripcion, string cordenadax, string cordenaday);
    }
}
