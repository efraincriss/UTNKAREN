using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.aplicacion.Transporte.Dto;
using com.cpp.calypso.proyecto.aplicacion.Transporte.Interface;
using com.cpp.calypso.proyecto.dominio;
using com.cpp.calypso.proyecto.dominio.Transporte;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion.Transporte.Service
{
    public class RutaHorarioVehiculoAsyncBaseCrudAppService : AsyncBaseCrudAppService<RutaHorarioVehiculo, RutaHorarioVehiculoDto, PagedAndFilteredResultRequestDto>, IRutaHorarioVehiculoAsyncBaseCrudAppService
    {

        private readonly IBaseRepository<Vehiculo> _vehiculoRepository;
        private readonly IBaseRepository<Ruta> _rutaRepository;
        private readonly IBaseRepository<RutaHorario> _horarioRepository;
        private readonly IBaseRepository<Parada> _paradaRepository;
        IBaseRepository<OperacionDiariaRuta> _operacionRutaRepository;
        public RutaHorarioVehiculoAsyncBaseCrudAppService(
            IBaseRepository<RutaHorarioVehiculo> repository,
            IBaseRepository<Vehiculo> vehiculoRepository,
            IBaseRepository<Ruta> rutaRepository,
            IBaseRepository<RutaHorario> horarioRepository,
            IBaseRepository<Parada> paradaRepository,
            IBaseRepository<OperacionDiariaRuta> operacionRutaRepository
            ) : base(repository)
        {
            _vehiculoRepository = vehiculoRepository;
            _rutaRepository = rutaRepository;
            _horarioRepository = horarioRepository;
            _paradaRepository = paradaRepository;
            _operacionRutaRepository = operacionRutaRepository;

        }

        public int Editar(RutaHorarioVehiculo r)
        {
            var ruta = Repository.Get(r.Id);
            ruta.VehiculoId = r.VehiculoId;
            ruta.FechaDesde = r.FechaDesde;
            ruta.FechaHasta = r.FechaHasta;
            ruta.Duracion = r.Duracion;
            ruta.HoraLlegada = r.HoraLlegada;
            ruta.Observacion = r.Observacion;
            var id = Repository.Update(ruta);
            return id.Id;
        }

        public int Eliminar(int id)
        {
            var count = _operacionRutaRepository.GetAllIncluding(c => c.RutaHorarioVehiculo).Where(c => c.RutaHorarioVehiculoId == id).ToList();
            if (count.Count > 0)
            {
                return -1;
            }
            else
            {

                var rutap = Repository.Get(id);
                Repository.Delete(rutap);
                return rutap.Id;
            }
        }

        public bool existecode(string code)
        {
            throw new NotImplementedException();
        }

        public RutaHorarioVehiculoDto GetDetalles(int id)
        {
            var query = Repository.GetAllIncluding(c => c.RutaHorario, c => c.Vehiculo).ToList();
            var lista = (from l in query
                         select new RutaHorarioVehiculoDto
                         {
                             Id = l.Id,
                             RutaHorarioId = l.RutaHorarioId,
                             VehiculoId = l.VehiculoId,
                             CodigoVehiculo = l.Vehiculo.Codigo,
                             CapacidadVehiculo = l.Vehiculo.Capacidad,
                             TipoVehiculo = l.Vehiculo.TipoVehiculo.nombre,
                             NombreEstado = Enum.GetName(typeof(EstadoAsignacionVehiculo), l.Estado),
                             FechaDesde = l.FechaDesde,
                             FechaHasta = l.FechaHasta,
                         }).FirstOrDefault();

            return lista;
        }

        public TimeSpan HoraLLegada(int rutaid, int horarioid)
        {
            TimeSpan resultado = new TimeSpan();
            var estado = _horarioRepository.GetAllIncluding(c => c.Ruta).Where(c => c.RutaId == rutaid).Where(c => c.Id == horarioid).FirstOrDefault();
            if (estado != null) {
                resultado = estado.Horario.Add(TimeSpan.FromMinutes(estado.Ruta.Duracion));
                return resultado;
            }
            return resultado;
           
        }

        public int Ingresar(RutaHorarioVehiculo ruta,int horarioid)
        {
            var puede = Repository.GetAll().Where(c => c.VehiculoId == ruta.VehiculoId).Where(c => c.RutaHorarioId == ruta.RutaHorarioId).ToList();

            if (puede.Count > 0) {
                return -1;
            }
            else {
            var horario = _horarioRepository.Get(horarioid);
                     

            ruta.HorarioSalida = horario.Horario;
            var nuevo = Repository.InsertAndGetId(ruta);
            return nuevo;
            }
        }

        public List<RutaHorarioVehiculoDto> Listar()
        {
            var query = Repository.GetAllIncluding(c => c.RutaHorario, c => c.Vehiculo.TipoVehiculo).Where(c => !c.IsDeleted).ToList();
            var lista = (from l in query
                         select new RutaHorarioVehiculoDto
                         {
                          Id=l.Id,
                          RutaHorarioId=l.RutaHorarioId,
                          VehiculoId=l.VehiculoId,
                          CodigoVehiculo = l.Vehiculo.Codigo,
                          CapacidadVehiculo =l.Vehiculo.Capacidad,
                          TipoVehiculo=l.Vehiculo.TipoVehiculo.nombre,
                          NombreEstado= Enum.GetName(typeof(EstadoAsignacionVehiculo),l.Estado),
                          FechaDesde=l.FechaDesde,
                          FechaHasta=l.FechaHasta,
                          HoraLlegada=l.HoraLlegada,
                          HorarioSalida=l.HorarioSalida,
                             Duracion = l.Duracion,
                             Observacion = l.Observacion,
                             Estado = l.Estado
                         }).ToList();

            return lista;

        }

        public List<RutaHorarioVehiculoDto> ListarbyRutaHorario(int rutaid, int horarioid)
        {
            var query = Repository.GetAllIncluding(c => c.RutaHorario, c => c.Vehiculo.TipoVehiculo)
                .Where(c => c.RutaHorario.RutaId==rutaid)
                .Where(c => c.RutaHorarioId==horarioid)
                .Where(c => !c.IsDeleted)
                .Where(c => !c.Vehiculo.IsDeleted).ToList();
            var lista = (from l in query
                         select new RutaHorarioVehiculoDto
                         {
                             Id = l.Id,
                             RutaHorarioId = l.RutaHorarioId,
                             VehiculoId = l.VehiculoId,
                             CodigoVehiculo = l.Vehiculo.CodigoEquipoInventario,
                             CapacidadVehiculo = l.Vehiculo.Capacidad,
                             PlacaVehiculo = l.Vehiculo.NumeroPlaca,
                             TipoVehiculo = l.Vehiculo.TipoVehiculo.nombre,
                             NombreEstado = Enum.GetName(typeof(EstadoAsignacionVehiculo), l.Estado),
                             FechaDesde = l.FechaDesde,
                             FechaHasta = l.FechaHasta,
                             HoraLlegada = l.HoraLlegada,
                             HorarioSalida = l.HorarioSalida,
                             Duracion=l.Duracion,
                             Observacion=l.Observacion,
                             Estado=l.Estado,
                             FechaDesdeTexto=l.FechaDesde.ToShortDateString(),
                             FechaHastaTexto=l.FechaHasta.ToShortDateString()
                         }).ToList();

            return lista;
        }
    }
}