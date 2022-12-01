using Abp.Application.Services.Dto;
using com.cpp.calypso.framework;
using com.cpp.calypso.proyecto.aplicacion;
using com.cpp.calypso.proyecto.aplicacion.Dto;
using com.cpp.calypso.proyecto.aplicacion.Interfaces;
using com.cpp.calypso.proyecto.dominio;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace com.cpp.calypso.web.Areas.RRHH.Controllers
{
    public class CategoriasEncargadoController : BaseController
    {
        private readonly ICategoriasEncargadoAsyncBaseCrudAppService _categoriasEncargadoService;
        private readonly ICatalogoAsyncBaseCrudAppService _catalogoservice;
        private readonly ITipoCatalogoAsyncBaseCrudAppService _tipocatalogoservice;

        public CategoriasEncargadoController(
            IHandlerExcepciones manejadorExcepciones,
            ICategoriasEncargadoAsyncBaseCrudAppService categoriasEncargadoService,
            ICatalogoAsyncBaseCrudAppService catalogoservice,
            ITipoCatalogoAsyncBaseCrudAppService tipocatalogoservice
            ) : base(manejadorExcepciones)
        {
            _categoriasEncargadoService = categoriasEncargadoService;
            _catalogoservice = catalogoservice;
            _tipocatalogoservice = tipocatalogoservice;
        }

        public ActionResult Index()
        {
            return View();
        }

        // POST: RRHH/CategoriasEncargado/Create
        public ActionResult Create()
        {
            CategoriasEncargadoDto categoriasEncargado = new CategoriasEncargadoDto();
            return View(categoriasEncargado);
        }

        // POST: RRHH/CategoriasEncargado/Edit/5
        public ActionResult Edit()
        {
            CategoriasEncargadoDto categoriasEncargado = new CategoriasEncargadoDto();
            return View(categoriasEncargado);
        }

        // GET: RRHH/CategoriasEncargado/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // GET: RRHH/CategoriasEncargado/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        [HttpPost]
#pragma warning disable CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        public async System.Threading.Tasks.Task<ActionResult> DeleteApiAsync(int id)
#pragma warning restore CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        {

            var resultado = _categoriasEncargadoService.EliminarCategoriasEncargado(id);
            if (resultado)
            {
                return Content("OK");
            }
            else
            { return Content("E"); }
        }

        [HttpPost]
        public ActionResult GetCategoriaEncargadoApiAsync(int id)
        {
            CategoriasEncargado categoriaSearch = _categoriasEncargadoService.GetCategoriasEncargado(id);
            var result = JsonConvert.SerializeObject(categoriaSearch.EncargadoId);
            return Content(result);
        }

        [HttpPost]
        public ActionResult GetCategoriasPorEncargadoApiAsync(int id)
        {
            return Content(_categoriasEncargadoService.CatalogosporCategoria(id));
        }

        [HttpPost]
        public ActionResult GetCategoriasDisponiblesApiAsync(int id)
        {
            return Content(_categoriasEncargadoService.CategoriasDisponibles(id));
        }

        [HttpPost]
        public ActionResult GetCatalogosPorTipoApi(int Id)
        {
            //Obtiene los Catlogos del Tipo Identificacion
            var lista = _catalogoservice.ListarCatalogos(Id);//Revisar ID
            var result = JsonConvert.SerializeObject(lista);
            return Content(result);
        }

        [HttpPost]
        public ActionResult GetCategoriasEncargadoApi()
        {
            var list = _categoriasEncargadoService.GetList();
            var result = JsonConvert.SerializeObject(list);
            return Content(result);
        }

        [HttpPost]
        public ActionResult GetListaCatalogosPorCodigoApi(string[] codigo)
        {
            List<string> catalogos = new List<string>();

            foreach (var c in codigo)
            {
                var tipoCatalogo = _tipocatalogoservice.GetCatalogoPorCodigo(c);
                //Obtiene los Catlogos del Tipo Identificacion
                var lista = _catalogoservice.ListarCatalogos(tipoCatalogo.Id);//Revisar ID
                catalogos.Add(JsonConvert.SerializeObject(lista));
            }

            var result = JsonConvert.SerializeObject(catalogos);
            return Content(result);
        }

        [HttpPost]
        public ActionResult GetCatalogosPorCodigoApi(string codigo)
        {
            //Obtiene el codigo de Tipo de Catalogo
            var tipoCatalogo = _tipocatalogoservice.GetCatalogoPorCodigo(codigo);
            //Obtiene los Catalogos del Tipo Identificacion
            var lista = _catalogoservice.ListarCatalogos(tipoCatalogo.Id);//Revisar ID
            var result = JsonConvert.SerializeObject(lista);

            return Content(result);
        }

        [HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> EditarCategoriaEncargadoApi(int encargado, int[] idCategorias, int[] idCategoriasDisponibles)
        {
            if (ModelState.IsValid)
            {
                CategoriasEncargadoDto categoriaEncargado;

                /* Categorias Disponibles */
                if (idCategoriasDisponibles != null && idCategoriasDisponibles.Length != 0)
                {
                    foreach (var item in idCategoriasDisponibles)
                    {
                        /* Verifica si existe el cargo asociado al area y lo registra o actualiza */
                        CategoriasEncargado buscarCategoria = _categoriasEncargadoService.BuscarCategoriaEncargado(item, encargado);

                        if (buscarCategoria != null)
                        {
                            /* Si ya esta creado se actualiza la vigencia */
                            _categoriasEncargadoService.EliminarCategoriasEncargado(buscarCategoria.Id);
                        }
                    }
                }

                /* Cargos seleccionados */
                if (idCategorias != null && idCategorias.Length != 0)
                {
                    foreach (var item in idCategorias)
                    {
                        /* Verifica si existe el cargo asociado al area y lo registra o actualiza */
                        CategoriasEncargado buscarCategoria = _categoriasEncargadoService.BuscarCategoriaEncargado(item, encargado);

                        if (buscarCategoria == null)
                        {
                            categoriaEncargado = new CategoriasEncargadoDto();
                            categoriaEncargado.EncargadoId = encargado;
                            categoriaEncargado.vigente = true;
                            categoriaEncargado.CategoriaId = item;

                            await _categoriasEncargadoService.Create(categoriaEncargado);
                        }
                        else
                        {
                            /* Si ya esta creado se actualiza la vigencia */
                            _categoriasEncargadoService.ActualizarCategoriasEncargado(buscarCategoria.Id, User.Identity.Name);
                        }
                    }
                }

                return Content("OK");
            }
            return Content("NO");

        }


        [HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> CreateCategoriaEncargadoApi(int encargado, int[] idCategorias)
        {
            if (ModelState.IsValid)
            {
                CategoriasEncargadoDto categoriasEncargado;

                if (idCategorias != null && idCategorias.Length != 0)
                {
                    foreach (var item in idCategorias)
                    {
                        /* Verifica si existe el cargo asociado al area y lo registra o actualiza */
                        CategoriasEncargado encargadoSearch = _categoriasEncargadoService.BuscarCategoriaEncargado(item, encargado);

                        if (encargadoSearch == null)
                        {
                            categoriasEncargado = new CategoriasEncargadoDto();
                            categoriasEncargado.EncargadoId = encargado;
                            categoriasEncargado.vigente = true;
                            categoriasEncargado.CategoriaId = item;

                            await _categoriasEncargadoService.Create(categoriasEncargado);
                        }
                        else
                        {
                            /* Si ya esta creado se actualiza la vigencia */
                            _categoriasEncargadoService.ActualizarCategoriasEncargado(encargadoSearch.Id, User.Identity.Name);
                        }
                    }
                }

                return Content("OK");
            }
            return Content("NO");

        }

        [System.Web.Mvc.HttpPost]
        public ActionResult GetCategoriasPorEncargadoPersonalApi(int id)
        {
            var categorias = _categoriasEncargadoService.GetListCategoriasPorEncargado(id);
            var result = JsonConvert.SerializeObject(categorias);
            return Content(result);
        }

    }
}