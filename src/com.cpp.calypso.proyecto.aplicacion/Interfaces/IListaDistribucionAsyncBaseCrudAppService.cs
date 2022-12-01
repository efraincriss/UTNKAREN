using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.proyecto.dominio;
using com.cpp.calypso.proyecto.dominio.Models;

namespace com.cpp.calypso.proyecto.aplicacion
{
    public interface IListaDistribucionAsyncBaseCrudAppService : IAsyncBaseCrudAppService<ListaDistribucion, ListaDistribucionDto, PagedAndFilteredResultRequestDto>
    {

        List<ListaDistribucionDto> listar();


        List<CorreoListaDto> GetCorreosInternos(int listaId);


        List<CorreoListaDto> GetCorreosExternos(int listaId);


        // Correos internos excepto los que ya estan ingresados en la lista
        List<CorreoListaDto> GetCorreosInternosParaIngresar(int listaId);

        // Correos externos excepto los que ya estan ingresados en la lista
        List<CorreoListaDto> GetCorreosExternosParaIngresar(int listaId);

        bool EliminarCorreoLista(int Id);

        List<UserCorreos> GetCorreosListos(string codigo);

        bool ActualizarLista(CorreoListaDto correo);

        bool OrdenarCorreos(List<CorreoListaDto> correos);

    }
}
