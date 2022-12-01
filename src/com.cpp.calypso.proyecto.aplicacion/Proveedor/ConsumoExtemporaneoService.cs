using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.aplicacion.Dto;
using com.cpp.calypso.proyecto.aplicacion.Proveedor.Dto;
using com.cpp.calypso.proyecto.aplicacion.Proveedor.Interfaces;
using com.cpp.calypso.proyecto.aplicacion.Transporte.Dto;
using com.cpp.calypso.proyecto.dominio;
using com.cpp.calypso.proyecto.dominio.Constantes;
using com.cpp.calypso.proyecto.dominio.Proveedor;
using com.cpp.calypso.proyecto.dominio.Transporte;

namespace com.cpp.calypso.proyecto.aplicacion.Proveedor
{
    public class ConsumoExtemporaneoAsyncBaseCrudAppService : AsyncBaseCrudAppService<ConsumoExtemporaneo, ConsumoExtemporaneoDto,  PagedAndFilteredResultRequestDto>, IConsumoExtemporaneoAsyncBaseCrudAppService
    {
        private readonly IBaseRepository<ServicioProveedor> _servicioProveedorRepository;
        private readonly IBaseRepository<ContratoProveedor> _contratoProveedorRepository;
        private readonly IBaseRepository<TipoOpcionComida> _tipoOpcionComidaRepository;

        public ConsumoExtemporaneoAsyncBaseCrudAppService(
            IBaseRepository<ConsumoExtemporaneo> repository,
            IBaseRepository<ServicioProveedor> servicioProveedorRepository,
            IBaseRepository<ContratoProveedor> contratoProveedorRepository,
            IBaseRepository<TipoOpcionComida> tipoOpcionComidaRepository
            ) : base(repository)
        {
            _servicioProveedorRepository = servicioProveedorRepository;
            _contratoProveedorRepository = contratoProveedorRepository;
            _tipoOpcionComidaRepository = tipoOpcionComidaRepository;
        }



        public List<ConsumoExtemporaneoDto> ObtenerTodos()
        {
            var consumos = Repository.GetAll().Include(o => o.Proveedor).Include(o => o.TipoComida).OrderByDescending(o => o.Fecha).ToList();

            var dtos = Mapper.Map<List<ConsumoExtemporaneo>, List<ConsumoExtemporaneoDto>>(consumos);

            var count = 1;
            foreach (var paradaDto in dtos)
            {
                paradaDto.Secuencia = count;
                count++;
            }

            return dtos;

        }

        public List<ProveedorDto> ObtenerProveedoresAlimentacion()
        {
            var query = _servicioProveedorRepository.GetAll()
                .Include(o => o.Proveedor)
                .Where(o => o.Servicio.codigo == CatalogosCodigos.SERVICIO_ALMUERZO).ToList();

            var proveedores = (from sp in query
                select sp.Proveedor).ToList();

            var proveedoresDto = Mapper.Map<List<dominio.Proveedor.Proveedor>, List<ProveedorDto>>(proveedores);

            var count = 1;
            foreach (var proveedorDto in proveedoresDto)
            {
                proveedorDto.secuencial = count;
                count++;
            }
            return proveedoresDto;
        }

        public List<TipoOpcionComidaDto> ObtenerTiposComida(int proveedorId)
        {
            var contratoVigente = _contratoProveedorRepository
                .GetAll()
                .Where(o => o.ProveedorId == proveedorId)
                .FirstOrDefault(o => o.estado == ContratoEstado.Activo);

            if (contratoVigente != null)
            {
                var tiposComida = _tipoOpcionComidaRepository.GetAll().Include(o => o.tipo_comida).Include(o => o.opcion_comida)
                    .Where(o => o.ContratoId == contratoVigente.Id).ToList();

                var dtos = Mapper.Map<List<TipoOpcionComida>, List<TipoOpcionComidaDto>>(tiposComida);

                return dtos;
            }

            return new List<TipoOpcionComidaDto>();
        }


        public bool ValidarRepetidos(int proveedorId, DateTime date)
        {
            var consumos = Repository
                .GetAll()
                .Where(o => o.ProveedorId == proveedorId)
                .Count(o => o.Fecha == date);

            if (consumos > 0) return false;

            return true;

        }
    }
}
