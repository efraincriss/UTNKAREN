using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.aplicacion.Transporte.Dto;
using com.cpp.calypso.proyecto.aplicacion.Transporte.Interface;
using com.cpp.calypso.proyecto.dominio.Transporte;

namespace com.cpp.calypso.proyecto.aplicacion.Transporte.Service
{
    public class VehiculoAsyncBaseCrudAppService : AsyncBaseCrudAppService<Vehiculo, VehiculoDto, PagedAndFilteredResultRequestDto>, IVehiculoAsyncBaseCrudAppService
    {
        private readonly IBaseRepository<VehiculoHistorico> _vehiculoHistoricoRepository;
        private readonly IBaseRepository<RutaHorarioVehiculo> _rutavehiculoRepository;

        public VehiculoAsyncBaseCrudAppService(
            IBaseRepository<Vehiculo> repository,
            IBaseRepository<VehiculoHistorico> vehiculoHistoricoRepository,
            IBaseRepository<RutaHorarioVehiculo> rutavehiculoRepository
            ) : base(repository)
        {
            _vehiculoHistoricoRepository = vehiculoHistoricoRepository;
            _rutavehiculoRepository = rutavehiculoRepository;
        }


        public ICollection<VehiculoDto> GetAllVehiculos()
        {
            var entities = Repository.GetAll()
                .Include(o => o.Proveedor)
                .Include(o => o.TipoVehiculo).ToList();

            var dtos = Mapper.Map<List<Vehiculo>, ICollection<VehiculoDto>>(entities);

            var count = 1;
            foreach (var vehiculoDto in dtos)
            {
                vehiculoDto.Secuencial = count;
                count++;
            }
            return dtos;
        }

        public string CanCreate(string codigo, string placa, int anioFabricacion, DateTime fechaMatricula)
        {
            var year = DateTime.Now.Year + 1;
            if (anioFabricacion < 1970)
            {
                return "El año no puede ser menor a 1970";
            }

            if (anioFabricacion > year)
            {
                return "El año no puede ser mayor a " + year;
            }

            var fechaActual = DateTime.Now;
            if (fechaMatricula < fechaActual)
            {
                return "La fecha de vencimiento de matrícula no puede ser menor a la fecha actual";
            }
            var count = Repository.GetAll()
                .Where(o => o.Codigo == codigo)
                .Count(o => o.NumeroPlaca == placa)
                ;
            
            if (count > 0)
            {
                return "La placa y codigo ingresados ya estan registradas";
            }

            return "PUEDE_CREAR";
        }

        public string CanUpdate(string codigo, string placa, int anioFabricacion, DateTime fechaMatricula, int proveedorId)
        {
            var year = DateTime.Now.Year + 1;
            if (anioFabricacion < 1970)
            {
                return "El año no puede ser menor a 1970";
            }

            if (anioFabricacion > year)
            {
                return "El año no puede ser mayor a " + year;
            }

            var fechaActual = DateTime.Now;
            if (fechaMatricula < fechaActual)
            {
                return "La fecha de vencimiento de matrícula no puede ser menor a la fecha actual";
            }
            var count = Repository.GetAll()
                    .Where(o => o.Codigo == codigo)
                    .Where(o => o.ProveedorId != proveedorId)
                    .Count(o => o.NumeroPlaca == placa)

                ;
            
            if (count > 0)
            {
                return "La placa y codigo ingresados ya estan registradas en otro proveedor";
            }


            return "PUEDE_ACTUALIZAR";
        }

        public bool RegistrarHistorico(int id, string nuevoEstado)
        {
            var input = Repository.Get(id);
            var entity = new VehiculoHistorico()
            {
                Estado = input.Estado,
                VehiculoId = input.Id,
                FechaEstado = input.FechaEstado,
                Id = 0,
            };
            _vehiculoHistoricoRepository.Insert(entity);

            if (nuevoEstado == input.Estado)
            {
                return false;
            }

            return true;
        }

        public string CanDelete(int id)
        {
            var lista = _rutavehiculoRepository.GetAll().Where(c => c.VehiculoId == id).ToList();
            if (lista.Count > 0)
            {
                return "EL Vehículo no se puede eliminar debido a que tiene rutas asignadas";
            }
            else {
                return "PUEDE_ELIMINAR";
            }
        }
        public string nextcode()
        {
            int sec_number = 1;
            var list_code = Repository.GetAll().Where(c => !c.IsDeleted).Select(c => c.Codigo).ToList();
            if (list_code.Count > 0)
            {
                List<int> numeracion = (from l in list_code
                                        where l.Length == 8
                                        select Convert.ToInt32(l.Substring(3, 5))).ToList();

                if (numeracion.Count > 0)
                {
                    sec_number = numeracion.Max() + 1;
                }


            }
            return "VEH" + String.Format("{0:00000}", sec_number);
        }

        public string GetTipoVehiculo(int id)
        {
            var input =Repository.GetAll()
                .Include(o => o.TipoVehiculo)
                .Where(c=>c.Id==id).FirstOrDefault();
            return input!=null?input.TipoVehiculo!=null?input.TipoVehiculo.nombre:"":"";
        }
    }
}
