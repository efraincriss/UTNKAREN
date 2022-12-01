using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.proyecto.dominio;

namespace com.cpp.calypso.proyecto.aplicacion.Interfaces
{
   public  interface IArchivoAsyncBaseCrudAppService :IAsyncBaseCrudAppService<Archivo, ArchivoDto, PagedAndFilteredResultRequestDto>
    {

        int InsertArchivo(HttpPostedFileBase x);
    }
}
