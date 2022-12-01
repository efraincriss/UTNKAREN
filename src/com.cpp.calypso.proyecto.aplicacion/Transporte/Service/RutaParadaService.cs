using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.aplicacion.Transporte.Dto;
using com.cpp.calypso.proyecto.aplicacion.Transporte.Interface;
using com.cpp.calypso.proyecto.dominio.Transporte;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion.Transporte.Service
{
    public class RutaParadaAsyncBaseCrudAppService : AsyncBaseCrudAppService<RutaParada, RutaParadaDto, PagedAndFilteredResultRequestDto>, IRutaParadaAsyncBaseCrudAppService
    {
        private readonly IBaseRepository<Ruta> _rutarepository;
        private readonly IBaseRepository<RutaHorario> _rutahorariorepository;
        private readonly IBaseRepository<Parada> _paradarepository;
        public RutaParadaAsyncBaseCrudAppService(
            IBaseRepository<RutaParada> repository,
               IBaseRepository<Ruta> rutarepository,
        IBaseRepository<RutaHorario> rutahorariorepository,
        IBaseRepository<Parada> paradarepository
            ) : base(repository)
        {
            _rutahorariorepository = rutahorariorepository;
            _rutarepository = rutarepository;
            _paradarepository = paradarepository;
        }

        public int Editar(RutaParada rutaparada)
        {
            var ruta = Repository.Get(rutaparada.Id);
            ruta.ParadaId = rutaparada.ParadaId;
            ruta.ordinal = rutaparada.ordinal;
            ruta.Distancia = rutaparada.Distancia;
            var id = Repository.Update(ruta);
            return id.Id;
        }

        public int Eliminar(int id)
        {
            var rutap = Repository.Get(id);
            Repository.Delete(rutap);
            return rutap.Id;
        }

        public int Ingresar(RutaParada rutaparada)
        {
            var nuevo = Repository.InsertAndGetId(rutaparada);
            return nuevo;
        }

        public List<RutaHorario> ListaHorariosporRuta(int id)
        {
            var query = _rutahorariorepository.GetAll().Include(c => c.Ruta).Where(c => c.RutaId == id).ToList();
            return query;


        }

        public List<RutaParadaDto> ListaParadaporRuta(int id)
        {
            var query = Repository.GetAllIncluding(c => c.Parada).Include(c => c.Ruta)
                    .Where(c=>c.RutaId==id)
                    .Where(c => !c.Parada.IsDeleted)
                .ToList();

            var lista = (from l in query
                         select new RutaParadaDto()
                         {
                             Id = l.Id,
                             RutaId=l.RutaId,
                             ParadaId=l.ParadaId,
                             NombreParada=l.Parada.Nombre,
                             Latitud = l.Parada.Latitud,
                             Longitud = l.Parada.Longitud,
                             Referencia = l.Parada.Referencia,
                             ordinal=l.ordinal,
                             Distancia=l.Distancia,
                         }).OrderBy(c=>c.ordinal).ToList();

            return lista;
        }

        public List<Parada> ListaParadas()
        {
            var lista = _paradarepository.GetAll().ToList();
            return lista;
        }
    }
}
