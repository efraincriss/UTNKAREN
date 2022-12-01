using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.proyecto.dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion.Interfaces
{
    public interface ICobroAsyncBaseCrudAppService : IAsyncBaseCrudAppService<Cobro, CobroDto, PagedAndFilteredResultRequestDto>
    {
        List<Cobro> Listar();
        CobroDto getdetalle(int Id);
        bool Eliminar(int Id);
    }
}