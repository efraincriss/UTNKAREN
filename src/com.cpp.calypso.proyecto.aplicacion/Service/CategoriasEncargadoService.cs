using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.aplicacion.Dto;
using com.cpp.calypso.proyecto.aplicacion.Interfaces;
using com.cpp.calypso.proyecto.dominio;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion.Service
{
    public class CategoriasEncargadoAsyncBaseCrudAppService : AsyncBaseCrudAppService<CategoriasEncargado, 
        CategoriasEncargadoDto, PagedAndFilteredResultRequestDto>, ICategoriasEncargadoAsyncBaseCrudAppService
    {
        private readonly IBaseRepository<Catalogo>  _catalogoRepository;
        private readonly IBaseRepository<TipoCatalogo> _tipocatalogoRepository;

        public CategoriasEncargadoAsyncBaseCrudAppService(
            IBaseRepository<CategoriasEncargado> repository,
            IBaseRepository<Catalogo> catalogoRepository,
            IBaseRepository<TipoCatalogo>  tipocatalogoRepository
            ) : base(repository)
        {
            _catalogoRepository = catalogoRepository;
            _tipocatalogoRepository = tipocatalogoRepository;
        }

        public async Task<string> ActualizarCategoriasEncargadoAsync(CategoriasEncargadoDto categoriasEncargado)
        {
            categoriasEncargado.vigente = true;
            var result = Repository.InsertOrUpdateAndGetId(MapToEntity(categoriasEncargado));
            return result.ToString();
        }

        public async Task<string> CrearCategoriasEncargadoAsync(CategoriasEncargadoDto categoriasEncargado)
        {
            categoriasEncargado.vigente = true;

            var result = Repository.InsertAndGetId(MapToEntity(categoriasEncargado));
            return result.ToString();
        }

        public bool EliminarCategoriasEncargado(int Id)
        {
            var categoriasEncargado = Repository.Get(Id);
            if (categoriasEncargado != null)
            {
                categoriasEncargado.vigente = false;
                categoriasEncargado.IsDeleted = true;
                Repository.Update(categoriasEncargado);
                return true;
            }

            return false;
        }

        public bool ActualizarCategoriasEncargado(int Id, String username)
        {
            var categoriasEncargado = Repository.Get(Id);
            if (categoriasEncargado != null)
            {
                categoriasEncargado.vigente = true;
                Repository.Update(categoriasEncargado);
                return true;
            }

            return false;
        }

        public CategoriasEncargado GetCategoriasEncargado(int Id)
        {
            CategoriasEncargado categoriasEncargado = Repository.Get(Id);
            return categoriasEncargado;
        }

        public CategoriasEncargado BuscarCategoriaEncargado(int IdCategoria, int IdEncargado)
        {
            CategoriasEncargado categoriasEncargado = Repository.GetAll().Where(e => e.CategoriaId == IdCategoria && e.EncargadoId == IdEncargado).FirstOrDefault();
            return categoriasEncargado;
        }

        public List<CategoriasEncargadoDto> GetList()
        {
            List<CategoriasEncargado> categoriasEncargadoList = Repository.GetAll().Where(e => e.vigente == true).ToList();
            List<CategoriasEncargadoDto> listDto = new List<CategoriasEncargadoDto>();
            CategoriasEncargadoDto categoriasEncargadoArea;

            var nr = 1;

            foreach (var i in categoriasEncargadoList)
            {
                categoriasEncargadoArea = new CategoriasEncargadoDto();
                categoriasEncargadoArea.Id = i.Id;
                categoriasEncargadoArea.EncargadoId = i.EncargadoId;
                categoriasEncargadoArea.vigente = i.vigente;
                categoriasEncargadoArea.CategoriaId = i.CategoriaId;
                categoriasEncargadoArea.nombre_categoria = i.Categoria.nombre;
                categoriasEncargadoArea.nombre_encargado = i.Encargado.nombre;
                categoriasEncargadoArea.nro = nr; 
                listDto.Add(categoriasEncargadoArea);
                nr++;
            }
            return listDto;
        }

        public List<CategoriasEncargado> GetListCategoriasPorEncargado(int IdEncargado)
        {
            List<CategoriasEncargado> categoriasEncargadoList = Repository.GetAll().Where(e => e.vigente == true && e.EncargadoId == IdEncargado).ToList();
            return categoriasEncargadoList;
        }

        public String CatalogosporCategoria(int Id)// Id Encargado
        {

            List<Catalogo> listaTarget = new List<Catalogo>();

            List<CategoriasEncargado> categoriasEncargadoList = Repository.GetAll()
                .Where(e => e.vigente == true && e.EncargadoId == Id).ToList();

            var tipo_catalogo = _tipocatalogoRepository.GetAll()
                .Where(e => e.vigente)
                .Where(e => e.codigo == "CATEGORIA").FirstOrDefault();

            var catalogos = _catalogoRepository.GetAll()
                .Where(e => e.vigente).Where(e => e.TipoCatalogoId == tipo_catalogo.Id).ToList();


            foreach (var categoria in categoriasEncargadoList)
            {
                foreach (var cat in catalogos)
                {
                    if (categoria.CategoriaId == cat.Id)
                    {
                        listaTarget.Add(cat);
                    }
                }
            }

            List<string> lscatalogos = new List<string>();
            lscatalogos.Add(JsonConvert.SerializeObject(listaTarget));

            return JsonConvert.SerializeObject(lscatalogos);
        }

        public String CategoriasDisponibles(int Id)// Id Encargado
        {

            List<Catalogo> listaTarget = new List<Catalogo>();

            List<CategoriasEncargado> categoriasEncargadoList = Repository.GetAll()
                .Where(e => e.vigente == true && e.EncargadoId == Id).ToList();

            var tipo_catalogo = _tipocatalogoRepository.GetAll()
                .Where(e => e.vigente)
                .Where(e => e.codigo == "CATEGORIA").FirstOrDefault();

            var catalogos = _catalogoRepository.GetAll()
                .Where(e => e.vigente).Where(e => e.TipoCatalogoId == tipo_catalogo.Id).ToList();


            foreach (var categoria in categoriasEncargadoList)
            {
                foreach (var cat in catalogos)
                {
                    if (categoria.CategoriaId == cat.Id)
                    {
                        listaTarget.Add(cat);
                    }
                }
            }

            var differences = catalogos.Except(listaTarget);

            List<string> lscatalogos = new List<string>();
            lscatalogos.Add(JsonConvert.SerializeObject(differences));
            return JsonConvert.SerializeObject(lscatalogos);
        }
    }
}
