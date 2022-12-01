using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Extensions;
using AutoMapper;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.aplicacion.Dto;
using com.cpp.calypso.proyecto.aplicacion.Proveedor.Dto;
using com.cpp.calypso.proyecto.aplicacion.Proveedor.Interfaces;
using com.cpp.calypso.proyecto.dominio;
using com.cpp.calypso.proyecto.dominio.Proveedor;

namespace com.cpp.calypso.proyecto.aplicacion.Proveedor
{
    public class DetalleConsumoExtemporaneoAsyncBaseCrudAppService : AsyncBaseCrudAppService<DetalleConsumoExtemporaneo, DetalleConsumoExtemporaneoDto,  PagedAndFilteredResultRequestDto>, IDetalleConsumoExtemporaneoAsyncBaseCrudAppService
    {
        private readonly IBaseRepository<Colaboradores> _colaboradoRepository;

        public DetalleConsumoExtemporaneoAsyncBaseCrudAppService(
            IBaseRepository<DetalleConsumoExtemporaneo> repository,
            IBaseRepository<Colaboradores> colaboradoRepository
        ) : base(repository)
        {
            _colaboradoRepository = colaboradoRepository;
        }


        public List<DetalleConsumoExtemporaneoDto> BuscarDetallesPorCabecera(int consumoExtemporaneoId)
        {
            var list = Repository.GetAll().Include(o => o.ConsumoExtemporaneo).Include(o => o.Colaborador)
                .Where(o => o.ConsumoExtemporaneoId == consumoExtemporaneoId)
                .OrderBy(o => o.Colaborador.primer_apellido)
                .ToList();

            var dtos = Mapper.Map<List<DetalleConsumoExtemporaneo>, List<DetalleConsumoExtemporaneoDto>>(list);

            var count = 1;
            foreach (var proveedorDto in dtos)
            {
                proveedorDto.Secuencia = count;
                count++;
            }
            return dtos;
        }

        public List<ColaboradorNombresDto> BuscarPorIdentificacionNombre(string identificacion = "", string nombre = "")
        {
            var query = _colaboradoRepository.GetAll()
                    .Include(o => o.GrupoPersonal)
                ;

            if (!identificacion.IsNullOrEmpty())
            {
                query = query.Where(o => o.numero_identificacion.StartsWith(identificacion));
            }

            if (!nombre.IsNullOrEmpty())
            {
                query = query.Where(o =>
                    o.nombres_apellidos.Contains(nombre));
            }

            var entities = query.ToList();

            var dto = Mapper.Map<List<Colaboradores>, List<ColaboradorNombresDto>>(entities);

            return dto;
        }

        public string CrearDetalleConsumo(DetalleConsumoExtemporaneoDto detalle)
        {
            var entity = Mapper.Map<DetalleConsumoExtemporaneoDto, DetalleConsumoExtemporaneo>(detalle);

            var createdEntity = Repository.InsertAndGetId(entity);

            return "Consumo creado";
        }

        public bool VerificarDobleConsumo(int colaboradorId, int consumoExtemporaneoId)
        {
            var consumos = Repository.GetAll()
                .Where(o => o.ConsumoExtemporaneoId == consumoExtemporaneoId)
                .Count(o => o.ColaboradorId == colaboradorId);

            if (consumos > 0) return false;

            return true;
        }


    }
}
