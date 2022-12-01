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
    [Authorize]
    public class CargosSectorController : BaseController
    {
        private readonly ICargosSectorAsyncBaseCrudAppService _cargosSectorService;
        private readonly ICatalogoAsyncBaseCrudAppService _catalogoservice;
        private readonly ITipoCatalogoAsyncBaseCrudAppService _tipocatalogoservice;

        public CargosSectorController(
            IHandlerExcepciones manejadorExcepciones,
            ICargosSectorAsyncBaseCrudAppService cargosSectorService,
            ICatalogoAsyncBaseCrudAppService catalogoservice,
            ITipoCatalogoAsyncBaseCrudAppService tipocatalogoservice
            ) : base(manejadorExcepciones)
        {
            _cargosSectorService = cargosSectorService;
            _catalogoservice = catalogoservice;
            _tipocatalogoservice = tipocatalogoservice;
        }

        public ActionResult Index()
        {
            return View();
        }

        // POST: RRHH/CargosSector/Create
        public ActionResult Create()
        {
            CargosSectorDto cargosSector = new CargosSectorDto();
            return View(cargosSector);
        }

        // POST: RRHH/CargosSector/Edit/5
        public ActionResult Edit()
        {
            CargosSectorDto cargosSector = new CargosSectorDto();
            return View(cargosSector);
        }

        // GET: RRHH/CargosSector/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // GET: RRHH/CargosSector/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        [HttpPost]
#pragma warning disable CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        public async System.Threading.Tasks.Task<ActionResult> DeleteApiAsync(int id)
#pragma warning restore CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        {
            var resultado = _cargosSectorService.EliminarCargosSector(id);
            if (resultado)
            {
                return Content("OK");
            }
            else
            { return Content("E"); }

        }

        [HttpPost]
        public ActionResult GetCargoSectorApiAsync(int id)
        {
            CargosSector cargoSearch = _cargosSectorService.GetCargosSector(id);
            var result = JsonConvert.SerializeObject(cargoSearch.SectorId);
            return Content(result);
        }

        [HttpPost]
        public ActionResult GetCargosPorSectorApiAsync(int id)
        {
            return Content(_cargosSectorService.CatalogosPorSector(id));
        }

        [HttpPost]
        public ActionResult GetCargosDisponiblesApiAsync(int id)
        {
            return Content(_cargosSectorService.CargosDisponibles(id));
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
        public ActionResult GetCargosSectorApi()
        {
            return Content(JsonConvert.SerializeObject(_cargosSectorService.GetList()));
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
        public async System.Threading.Tasks.Task<ActionResult> EditarCargoSectorApi(int Sector, int[] idCargos, int[] idCargosDisponibles)
        {
            if (ModelState.IsValid)
            {
                CargosSectorDto cargosSector;

                /* Cargos Disponibles */
                if (idCargosDisponibles != null && idCargosDisponibles.Length != 0)
                {
                    foreach (var item in idCargosDisponibles)
                    {
                        /* Verifica si existe el cargo asociado al Sector y lo registra o actualiza */
                        CargosSector buscarCargo = _cargosSectorService.BuscarCargoSector(item, Sector);

                        if (buscarCargo != null)
                        {
                            /* Si ya esta creado se actualiza la vigencia */
                            _cargosSectorService.EliminarCargosSector(buscarCargo.Id);
                        }
                    }
                }

                /* Cargos seleccionados */
                if (idCargos != null && idCargos.Length != 0)
                {
                    foreach (var item in idCargos)
                    {
                        /* Verifica si existe el cargo asociado al Sector y lo registra o actualiza */
                        CargosSector cargoSearch = _cargosSectorService.BuscarCargoSector(item, Sector);

                        if (cargoSearch == null)
                        {
                            cargosSector = new CargosSectorDto();
                            cargosSector.SectorId = Sector;
                            cargosSector.vigente = true;
                            cargosSector.CargoId = item;

                            await _cargosSectorService.Create(cargosSector);
                        }
                        else
                        {
                            /* Si ya esta creado se actualiza la vigencia */
                            _cargosSectorService.ActualizarCargosSector(cargoSearch.Id, User.Identity.Name);
                        }
                    }
                }

                return Content("OK");
            }
            return Content("NO");

        }


        [HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> CreateCargoSectorApi(int Sector, int[] idCargos)
        {
            if (ModelState.IsValid)
            {
                CargosSectorDto cargosSector;

                if (idCargos != null && idCargos.Length != 0)
                {
                    foreach (var item in idCargos)
                    {
                        /* Verifica si existe el cargo asociado al Sector y lo registra o actualiza */
                        CargosSector cargoSearch = _cargosSectorService.BuscarCargoSector(item, Sector);

                        if (cargoSearch == null)
                        {
                            cargosSector = new CargosSectorDto();
                            cargosSector.SectorId = Sector;
                            cargosSector.vigente = true;
                            cargosSector.CargoId = item;

                            await _cargosSectorService.Create(cargosSector);
                        }
                        else
                        {
                            /* Si ya esta creado se actualiza la vigencia */
                            _cargosSectorService.ActualizarCargosSector(cargoSearch.Id, User.Identity.Name);
                        }
                    }
                }

                return Content("OK");
            }
            return Content("NO");

        }

        [System.Web.Mvc.HttpPost]
        public ActionResult GetCargosPorSectorApi(int id)
        {
            var cargos = _cargosSectorService.GetListCargosPorSector(id);
            var result = JsonConvert.SerializeObject(cargos);
            return Content(result);
        }

    }
}