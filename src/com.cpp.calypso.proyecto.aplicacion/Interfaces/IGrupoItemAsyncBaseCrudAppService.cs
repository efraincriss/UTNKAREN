using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.proyecto.aplicacion.Dto;
using com.cpp.calypso.proyecto.dominio;

namespace com.cpp.calypso.proyecto.aplicacion.Interfaces
{
   public interface IGrupoItemAsyncBaseCrudAppService : IAsyncBaseCrudAppService<GrupoItem, GrupoItemDto, PagedAndFilteredResultRequestDto>
   {
       List<GrupoItem> lista();

       GrupoItemDto getdetalle(int Id);
       bool Eliminar(int Id);
   }
}
