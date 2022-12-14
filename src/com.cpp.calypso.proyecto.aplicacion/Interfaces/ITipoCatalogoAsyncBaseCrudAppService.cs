using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.proyecto.dominio;

namespace com.cpp.calypso.proyecto.aplicacion.Interfaces
{
    public interface ITipoCatalogoAsyncBaseCrudAppService : IAsyncBaseCrudAppService<TipoCatalogo, TipoCatalogoDto, PagedAndFilteredResultRequestDto>
    {
        List<TipoCatalogoDto> ObtenerListaTipoCatalogos();
        List<TipoCatalogo> GetCatalogosPorCodigo(string[] codigo);
        TipoCatalogo GetCatalogoPorCodigo(string codigo);
        TipoCatalogo Detalles(int id);

    }
}
