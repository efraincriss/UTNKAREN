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
    public class CargosSectorAsyncBaseCrudAppService : AsyncBaseCrudAppService<CargosSector, CargosSectorDto, PagedAndFilteredResultRequestDto>, ICargosSectorAsyncBaseCrudAppService
    {
        private readonly IBaseRepository<Catalogo> _catalogoRepository;
        private readonly IBaseRepository<TipoCatalogo> _tipocatalogoRepository;

        public CargosSectorAsyncBaseCrudAppService(
            IBaseRepository<CargosSector> repository,
            IBaseRepository<Catalogo> catalogoRepository,
            IBaseRepository<TipoCatalogo> tipocatalogoRepository
            ) : base(repository)
        {
            _catalogoRepository = catalogoRepository;
            _tipocatalogoRepository = tipocatalogoRepository;
        }

        public string ActualizarCargosSectorAsync(CargosSectorDto cargosSector)
        {
            int idCargosSector = cargosSector.Id;
            cargosSector.vigente = true;
            var result = Repository.InsertOrUpdateAndGetId(MapToEntity(cargosSector));
            return result.ToString();
        }

        public string CrearCargosSectorAsync(CargosSectorDto cargosSector)
        {
            cargosSector.vigente = true;

            var result = Repository.InsertAndGetId(MapToEntity(cargosSector));
            return result.ToString();
        }

        public bool EliminarCargosSector(int Id)
        {
            var cargoSector = Repository.Get(Id);
            if (cargoSector != null)
            {
                cargoSector.vigente = false;
                cargoSector.IsDeleted = true;
                Repository.Update(cargoSector);
                return true;
            }

            return false;
        }

        public bool ActualizarCargosSector(int Id, String username)
        {
            var cargoSector = Repository.Get(Id);
            if (cargoSector != null)
            {
                cargoSector.vigente = true;
                Repository.Update(cargoSector);
                return true;
            }

            return false;
        }

        public CargosSector GetCargosSector(int Id)
        {
            CargosSector cargosSector = Repository.Get(Id);
            return cargosSector;
        }

        public CargosSector BuscarCargoSector(int IdCargo, int IdSector)
        {
            CargosSector cargosSector = Repository.GetAll().Where(e => e.CargoId == IdCargo && e.SectorId == IdSector).FirstOrDefault();
            return cargosSector;
        }

        public List<CargosSectorDto> GetList()
        {
            List<CargosSector> cargosSectorList = Repository.GetAll()           
                .Where(e => e.vigente == true).ToList();
            List<CargosSectorDto> listDto = new List<CargosSectorDto>();
            CargosSectorDto cargosSector;

            var nr = 1;

            foreach (var i in cargosSectorList)
            {
                cargosSector = new CargosSectorDto();
                cargosSector.Id = i.Id;
                cargosSector.SectorId = i.SectorId;
                cargosSector.vigente = i.vigente;
                cargosSector.CargoId = i.CargoId;
                cargosSector.nombre_sector = i.Sector.nombre; //asi 
                cargosSector.nombre_cargo = i.Cargo.nombre; //asi 
                cargosSector.nro = nr;
               listDto.Add(cargosSector);
                nr++;
            }
                return listDto;
        }

        public List<CargosSector> GetListCargosPorSector(int IdSector)
        {
            List<CargosSector> cargosSectorList = Repository.GetAll().Where(e => e.vigente == true && e.SectorId == IdSector).ToList();
            return cargosSectorList;
        }

        public String CatalogosPorSector(int Id)// Id Sector
        {

            List<Catalogo> listaTarget = new List<Catalogo>();

            List<CargosSector> cargoSearch = Repository.GetAll()
                .Where(e => e.vigente == true && e.SectorId == Id).ToList();

            var tipo_catalogo = _tipocatalogoRepository.GetAll()
                .Where(e => e.vigente)
                .Where(e => e.codigo == "CARGO").FirstOrDefault();

            var catalogos = _catalogoRepository.GetAll()
                .Where(e => e.vigente).Where(e => e.TipoCatalogoId == tipo_catalogo.Id).ToList();


            foreach (var cargo in cargoSearch)
            {
                foreach (var cat in catalogos)
                {
                    if (cargo.CargoId == cat.Id)
                    {
                        listaTarget.Add(cat);
                    }
                }
            }

            List<string> lscatalogos = new List<string>();
            lscatalogos.Add(JsonConvert.SerializeObject(listaTarget));

            return JsonConvert.SerializeObject(lscatalogos);
        }

        public String CargosDisponibles(int Id)// Id Sector
        {

            List<Catalogo> listaTarget = new List<Catalogo>();

            List<CargosSector> cargoSearch = Repository.GetAll()
                .Where(e => e.vigente == true && e.SectorId == Id).ToList();

            var tipo_catalogo = _tipocatalogoRepository.GetAll()
                .Where(e => e.vigente)
                .Where(e => e.codigo == "CARGO").FirstOrDefault();

            var catalogos = _catalogoRepository.GetAll()
                .Where(e => e.vigente).Where(e => e.TipoCatalogoId == tipo_catalogo.Id).ToList();


            foreach (var cargo in cargoSearch)
            {
                foreach (var cat in catalogos)
                {
                    if (cargo.CargoId == cat.Id)
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
