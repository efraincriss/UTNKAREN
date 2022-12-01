using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.proyecto.dominio;
using com.cpp.calypso.proyecto.dominio.Models;

namespace com.cpp.calypso.proyecto.aplicacion.Interfaces
{
    public interface ICatalogoAsyncBaseCrudAppService : IAsyncBaseCrudAppService<Catalogo, CatalogoDto, PagedAndFilteredResultRequestDto>
    {
        #region Metodo Catalogos
        List<string> GetCatalogosPorCodigo(List<TipoCatalogo> tiposCatalogos);
        List<CatalogoDto> ListarCatalogos(int tipoCatalogoId);
        List<CatalogoDto> ListarCatalogosporcodigo(string codigo);
        
        List<Catalogo> ListarTodosCatalogos();
        int EliminarVigencia(int catalogoId);

        Catalogo GetCatalogo(int IdCatalogo);

        List<CatalogoDto> ListarCatalogos(string code);

        //Verificar si existe
        bool existecatalogo(string nombre);

        Catalogo GetCatalogoPorCodigo(string codigo);
        List<Catalogo> ObtenerCatalogos(string code);
        #endregion

        //API CATALOGO USE THIS
        string GetNamebyId(int id);
        List<CatalogoDto> APIObtenerCatalogos(string code);
        List<ModelClassReact> APIObtenerCatalogosReact(string code);

    }
}
