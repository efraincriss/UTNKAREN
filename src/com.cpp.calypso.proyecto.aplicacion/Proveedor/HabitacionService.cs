using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Uow;
using AutoMapper;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.aplicacion.Proveedor.Dto;
using com.cpp.calypso.proyecto.aplicacion.Proveedor.Interfaces;
using com.cpp.calypso.proyecto.dominio;
using com.cpp.calypso.proyecto.dominio.Proveedor;

namespace com.cpp.calypso.proyecto.aplicacion.Proveedor
{
    public class HabitacionAsyncBaseCrudAppService : AsyncBaseCrudAppService<Habitacion, HabitacionDto, PagedAndFilteredResultRequestDto>, IHabitacionAsyncBaseCrudAppService
    {
        private readonly IBaseRepository<EspacioHabitacion> _espaciosHabitacionRepository;

        public HabitacionAsyncBaseCrudAppService(
            IBaseRepository<Habitacion> repository,
            IBaseRepository<EspacioHabitacion> espaciosHabitacionRepository
            ) : base(repository)
        {
            _espaciosHabitacionRepository = espaciosHabitacionRepository;
        }


        public List<HabitacionDto> GetHabitacionesPorProveedor(int ProveedorId)
        {
            var habitaciones = Repository.GetAll()
                .Include(o => o.TipoHabitacion)
                .Where(o => o.ProveedorId == ProveedorId)
                .OrderBy(o => o.numero_habitacion)
                .ToList();

            var habitacionesDto = Mapper.Map<List<Habitacion>, List<HabitacionDto>>(habitaciones);

            var count = 1;
            foreach (var dto in habitacionesDto)
            {
                dto.secuencial = count;
                count++;
            }
            return habitacionesDto;
        }


        public async Task<HabitacionDto> GetDetalle(int habitacionId)
        {
            var query = Repository.GetAll()
                .Where(o => o.Id == habitacionId);

            var entity = await query.SingleOrDefaultAsync();
            var dto = Mapper.Map<Habitacion, HabitacionDto>(entity);
            return dto;
        }

        public bool ExisteNumeroHabitacion(string nroHabitacion, int proveedorId)
        {
            var count = Repository.GetAll()
                .Where(o => o.ProveedorId == proveedorId)
                .Count(o => o.numero_habitacion == nroHabitacion);

            if (count > 0)
                return true;
            return false;

        }

        [UnitOfWork]
        public void SwitchEstadoHabitacion(int habitacionId, bool value)
        {
            var entity = Repository.Get(habitacionId);
            entity.estado = value;

            var espacios = _espaciosHabitacionRepository.GetAll()
                    .Where(o => o.HabitacionId == habitacionId)
                    .ToList()
                ;

            foreach (var e in espacios)
            {
                e.activo = value;
            }

        }


        #region Proveedor / Habitacion

        public List<HabitacionTree> GenerarArbolHabitaciones(int proveedorId)
        {
            var habitaciones = Repository.GetAll()
                .Where(o => o.estado)
                .Where(o => o.ProveedorId == proveedorId)
                .OrderBy(o => o.numero_habitacion);

            var list = new List<HabitacionTree>();

            foreach (var habitacion in habitaciones)
            {
                var node = new HabitacionTree()
                {
                    data = habitacion.Id,
                    
                    icon = "fa fa-fw fa-folder",
                    key = habitacion.Id + "_key",
                    label = habitacion.numero_habitacion
                };

                list.Add(node);
            }

            return list;


        }

        #endregion


    }
}
