using AutoMapper;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.aplicacion.CertificacionIngenieria.Dto;
using com.cpp.calypso.proyecto.aplicacion.CertificacionIngenieria.Interface;
using com.cpp.calypso.proyecto.dominio.CertificacionIngenieria;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion.CertificacionIngenieria.Service
{
    public class PorcentajeIndirectoIngenieriaAsyncBaseCrudAppService : AsyncBaseCrudAppService<PorcentajeIndirectoIngenieria, PorcentajeIndirectoIngenieriaDto, PagedAndFilteredResultRequestDto>, IPorcentajeIndirectoIngenieriaAsyncBaseCrudAppService
    {
        private readonly IBaseRepository<DetalleIndirectosIngenieria> _detalleIndirectosRepository;

        public PorcentajeIndirectoIngenieriaAsyncBaseCrudAppService(
            IBaseRepository<PorcentajeIndirectoIngenieria> repository,
            IBaseRepository<DetalleIndirectosIngenieria> detalleIndirectosRepository
        ) : base(repository)
        {
            _detalleIndirectosRepository = detalleIndirectosRepository;
        }

        public List<PorcentajeIndirectoIngenieriaDto> ObtenerPorcentajesDelDetalleIndirecto(int detalleIndirectoId)
        {
            var list = Repository.GetAll()
                .Include(o => o.Contrato)
                .Where(o => o.DetalleIndirectosIngenieriaId == detalleIndirectoId)
                .ToList();

            var entity = Mapper.Map<List<PorcentajeIndirectoIngenieriaDto>>(list);
            return entity;
        }

        public async Task<ResultadoColaboradorRubro> ActualizarAsync(PorcentajeIndirectoIngenieriaDto dto)
        {
            var detalleIndirecto = _detalleIndirectosRepository.Get(dto.DetalleIndirectosIngenieriaId);
            var entity = Mapper.Map<PorcentajeIndirectoIngenieria>(dto);
            entity.Horas = (detalleIndirecto.HorasLaboradas * entity.PorcentajeIndirecto) / 100;

            await Repository.UpdateAsync(entity);
            return new ResultadoColaboradorRubro
            {
                Success = true,
                Message = ""
            }; ;
        }

        public async Task<ResultadoColaboradorRubro> CrearAsync(PorcentajeIndirectoIngenieriaDto dto)
        {
            var detalleIndirecto = _detalleIndirectosRepository.Get(dto.DetalleIndirectosIngenieriaId);
            var entity = Mapper.Map<PorcentajeIndirectoIngenieria>(dto);
            entity.Horas = (detalleIndirecto.HorasLaboradas * entity.PorcentajeIndirecto) / 100;
            await Repository.InsertAsync(entity);

            return new ResultadoColaboradorRubro
            {
                Success = true,
                Message = ""
            };
        }

        public ResultadoColaboradorRubro Eliminar(int id)
        {
            Repository.Delete(id);
            return new ResultadoColaboradorRubro
            {
                Success = true,
                Message = "Porcentaje eliminado correctamente"
            };
        }
    }
}
