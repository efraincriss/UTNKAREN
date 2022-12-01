using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.proyecto.aplicacion.Dto;
using com.cpp.calypso.proyecto.dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion.Interfaces
{
    public interface ICategoriasEncargadoAsyncBaseCrudAppService : IAsyncBaseCrudAppService<CategoriasEncargado, CategoriasEncargadoDto, PagedAndFilteredResultRequestDto>
    {
        List<CategoriasEncargadoDto> GetList();
        CategoriasEncargado GetCategoriasEncargado(int Id); //Id Encargado
        Task<string> CrearCategoriasEncargadoAsync(CategoriasEncargadoDto categoriasEncargado);
        Task<string> ActualizarCategoriasEncargadoAsync(CategoriasEncargadoDto categoriasEncargado);
        bool EliminarCategoriasEncargado(int Id); //Id Encargado
        CategoriasEncargado BuscarCategoriaEncargado(int IdCategoria, int IdEncargado);
        List<CategoriasEncargado> GetListCategoriasPorEncargado(int IdEncargado);

        String CatalogosporCategoria(int Id); //Id Encargado

        String CategoriasDisponibles(int Id); //Id Encargado

        bool ActualizarCategoriasEncargado(int Id, String username);

    }
}
