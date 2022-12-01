using AutoMapper;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.aplicacion.CertificacionIngenieria.Dto;
using com.cpp.calypso.proyecto.aplicacion.CertificacionIngenieria.Interface;
using com.cpp.calypso.proyecto.dominio;
using com.cpp.calypso.proyecto.dominio.CertificacionIngenieria;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion.CertificacionIngenieria.Service
{
    public class PorcentajeAvanceIngenieriaAsyncBaseCrudAppService : AsyncBaseCrudAppService<PorcentajeAvanceIngenieria, PorcentajeAvanceIngenieriaDto, PagedAndFilteredResultRequestDto>, IPorcentajeAvanceIngenieriaAsyncBaseCrudAppService
    {
        private readonly IBaseRepository<PorcentajeAvanceIngenieria> repository;
        private readonly IBaseRepository<Proyecto> _proyectoRepository;
        private readonly IBaseRepository<Catalogo> _catalogoRepository;

        public PorcentajeAvanceIngenieriaAsyncBaseCrudAppService(
            IBaseRepository<PorcentajeAvanceIngenieria> repository,
            IBaseRepository<Proyecto> proyectoRepository,
            IBaseRepository<Catalogo> catalogoRepository
        ) : base(repository)
        {
            this.repository = repository;
            _proyectoRepository = proyectoRepository;
            _catalogoRepository = catalogoRepository;
        }


        public List<PorcentajeAvanceIngenieriaDto> ObtenerAvancesIngenieriaPorFecha(DateTime fechaDesde)
        {
            var list = Repository.GetAll()
                .Include(o => o.CatalogoPorcentaje)
                .Include(o => o.Proyecto)
                .Where(o => o.FechaAvance >= fechaDesde)
                .ToList();

            var entity = Mapper.Map<List<PorcentajeAvanceIngenieriaDto>>(list);
            return entity;
        }

        public async Task<ResultadoColaboradorRubro> ActualizarAsync(PorcentajeAvanceIngenieriaDto dto)
        {
            var fechaValida = ValidarFechaMayorAlUltimoAvanceRegistrado(dto.FechaAvance, dto.ProyectoId, dto.Id, dto.CatalogoProcentajeId);

            if (!fechaValida)
            {
                return new ResultadoColaboradorRubro
                {
                    Success = false,
                    Message = "La Fecha Avance no puede ser menor a la última registrada"
                };
            }
            var entity = Mapper.Map<PorcentajeAvanceIngenieria>(dto);
            await Repository.UpdateAsync(entity);
            return new ResultadoColaboradorRubro
            {
                Success = true,
                Message = ""
            };
        }

        public async Task<ResultadoColaboradorRubro> CrearAsync(PorcentajeAvanceIngenieriaDto dto)
        {
            var fechaValida = ValidarFechaMayorAlUltimoAvanceRegistrado(dto.FechaAvance, dto.ProyectoId, dto.Id, dto.CatalogoProcentajeId);

            if (!fechaValida)
            {
                return new ResultadoColaboradorRubro
                {
                    Success = false,
                    Message = "La Fecha Avance no puede ser menor a la última registrada"
                };
            }
            var entity = Mapper.Map<PorcentajeAvanceIngenieria>(dto);
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

        public bool ValidarFechaMayorAlUltimoAvanceRegistrado(DateTime fechaAvance, int proyectoId, int avanceId, int catalogoPorcentajeId)
        {
            var ultimoAvance = Repository.GetAll()
                .Where(o => o.ProyectoId == proyectoId)
                .Where(o => o.CatalogoProcentajeId == catalogoPorcentajeId)
                .OrderByDescending(o => o.FechaAvance)
                .Where(o => o.Id != avanceId)
                .FirstOrDefault();

            if (ultimoAvance == null) return true;

            return ultimoAvance.FechaAvance < fechaAvance;
        }

        public List<ProyectoIngenieriaDto> ObtenerProyectos()
        {
            var list = _proyectoRepository.GetAll()
                .Where(o => o.vigente)
                .OrderBy(o => o.nombre_proyecto)
                .ToList();

            var dtos = Mapper.Map<List<ProyectoIngenieriaDto>>(list);
            return dtos;
        }

        public List<CatalogoDto> ObtenerCatalogos()
        {
            var porecentajes = _catalogoRepository.GetAll()
                .Include(o => o.TipoCatalogo)
                .Where(o => o.vigente)
                .Where(o => o.TipoCatalogo.codigo == "PORCENTAJE_AVANCE_IN")
                .ToList();

            return Mapper.Map<List<Catalogo>, List<CatalogoDto>>(porecentajes);
        }
    }
}
