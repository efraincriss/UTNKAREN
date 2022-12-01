using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.aplicacion.Proveedor.Dto;
using com.cpp.calypso.proyecto.aplicacion.Proveedor.Interfaces;
using com.cpp.calypso.proyecto.dominio.Proveedor;

namespace com.cpp.calypso.proyecto.aplicacion.Proveedor
{
    public class EspacioHabitacionAsyncBaseCrudAppService : AsyncBaseCrudAppService<EspacioHabitacion, EspacioHabitacionDto, PagedAndFilteredResultRequestDto>, IEspacioHabitacionAsyncBaseCrudAppService
    {
        private readonly IBaseRepository<Habitacion> _habitacionRepository;
        private readonly IBaseRepository<DetalleReserva> _detalleReservaRepository;

        public EspacioHabitacionAsyncBaseCrudAppService(
            IBaseRepository<EspacioHabitacion> repository,
            IBaseRepository<Habitacion> habitacionRepository,
            IBaseRepository<DetalleReserva> detalleReservaRepository
            ) : base(repository)
        {
            _habitacionRepository = habitacionRepository;
            _detalleReservaRepository = detalleReservaRepository;
        }

        public List<EspacioHabitacionDto> GetEspaciosHabitacionPorProveedore(int ProveedorId)
        {
            var espacios = Repository.GetAll()
                .Include(o => o.Habitacion.TipoHabitacion)
                .Where(o => o.Habitacion.ProveedorId == ProveedorId)
                .ToList();
            var espaciosDto = Mapper.Map<List<EspacioHabitacion>, List<EspacioHabitacionDto>>(espacios);

            var count = 1;
            foreach (var dto in espaciosDto)
            {
                dto.secuencial = count;
                dto.EspaciosHabitacionConfig = (from e in espaciosDto where e.HabitacionId == dto.HabitacionId where e.activo select e).Count();
                count++;
            }
            return espaciosDto;
        }

        public bool ActivarDesactivarEspacio(int espacioId)
        {
            var espacio = Repository.Get(espacioId);
            var habitacion = _habitacionRepository.Get(espacio.HabitacionId);
            if (!habitacion.estado)
            {
                return false;
            }
            espacio.activo = !espacio.activo;
            Repository.Update(espacio);
            return true;
        }

        public void CrearEspacios(int HabitacionId, int espacios)
        {
            try
            {
                var CodigoEspacio = 'A';
                for (int i = 0; i < espacios; i++)
                {
                    var entity = new EspacioHabitacion()
                    {
                        HabitacionId = HabitacionId,
                        codigo_espacio = CodigoEspacio + "",
                        estado = true,
                        activo = true,

                    };
                    Repository.Insert(entity);
                    CodigoEspacio++;
                }
            }
            catch (Exception e)
            {
                var msg = "Ocurrió un error inesperado";
                throw new GenericException(msg, msg);
            }
        }

        public void EliminarEspaciosDeHabitacion(int habitacionId)
        {
            try
            {
                var espacios = Repository.GetAll()
                    .Where(o => o.HabitacionId == habitacionId).ToList();

                foreach (var e in espacios)
                {
                    //Repository.Delete(e);
                    e.activo = false;
                    Repository.Update(e);
                }
            }
            catch (Exception e)
            {
                var msg = "Ocurrió un error inesperado";
                throw new GenericException(msg, msg);
            }
        }

        public List<EspacioLibreDto> EspaciosLibresConDatos(int habitacionId, DateTime fecha)
        {
            var espacios = Repository.GetAll()
                .Include(o => o.Habitacion.Proveedor)
                .Where(o => o.activo)
                .Where(o => o.HabitacionId == habitacionId)
                .ToList();

            var list = new List<EspacioLibreDto>();

            foreach (var espacio in espacios)
            {
                var reservasCount = _detalleReservaRepository
                    .GetAll()
                    .Include(o => o.ReservaHotel)
                    .Where(o => o.ReservaHotel.EspacioHabitacionId == espacio.Id)
                    .Count(o => o.fecha_reserva == fecha);




                if (reservasCount > 0)
                {
                    var colaborador = _detalleReservaRepository
             .GetAll()
             .Include(o => o.ReservaHotel.Colaborador)
             .Where(o => o.ReservaHotel.EspacioHabitacionId == espacio.Id).Select(o => o.ReservaHotel.Colaborador).FirstOrDefault();
                    var dto = new EspacioLibreDto()
                    {
                        Id = espacio.Id,
                        codigo_espacio = espacio.codigo_espacio,
                        ocupado = true,
                        nombres_colaborador = colaborador!=null?colaborador.nombres_apellidos.ToUpper():""

                    };
                    list.Add(dto);
                }
                else
                {
                    var dto = new EspacioLibreDto()
                    {
                        Id = espacio.Id,
                        codigo_espacio = espacio.codigo_espacio,
                        ocupado = false,
                        nombres_colaborador = ""
                    };
                    list.Add(dto);
                }
            }

            return list;
        }


        public void CrearNuevosEspacios(int HabitacionId, int capacidad, int capacidadAnterior)
        {
           

            try
            {
                var letras = Repository.GetAll().Where(x => x.HabitacionId == HabitacionId).Select(x => x.codigo_espacio).ToList();
                if (capacidad >= letras.Count)
                {
                    var CodigoEspacio = 'A';                
             

                    if (letras.Count > 0) {
                        var charLetras = new List<Char>();
                        foreach (var item in letras)
                        {
                            charLetras.Add(Convert.ToChar(item));
                        }

                        CodigoEspacio = charLetras.Max();
                        CodigoEspacio++;
                        int espaciosnuevos = capacidad - letras.Count;

                        for (int i = 0; i < espaciosnuevos; i++)
                        {
                            var entity = new EspacioHabitacion()
                            {
                                HabitacionId = HabitacionId,
                                codigo_espacio = CodigoEspacio + "",
                                estado = true,
                                activo = true,

                            };
                            Repository.Insert(entity);
                            CodigoEspacio++;
                        }

                    }

                }

                var updateEstado = Repository.GetAll().Where(x => x.HabitacionId == HabitacionId).ToList().OrderBy(c => c.codigo_espacio).ToList();

                    int index = 1;
                    foreach (var espacio in updateEstado)
                    {
                       
                        if (index <= capacidad) {
                            var e = Repository.Get(espacio.Id);
                            e.activo = true ;
                        }

                        index++;
                    }

                }

              
            
            catch (Exception e)
            {
                var msg = "Ocurrió un error inesperado";
                throw new GenericException(msg, msg);
            }
        }

    }
}
