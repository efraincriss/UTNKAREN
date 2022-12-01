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
using ColaboradorCertificacionIngenieria = com.cpp.calypso.proyecto.dominio.CertificacionIngenieria.ColaboradorCertificacionIngenieria;

namespace com.cpp.calypso.proyecto.aplicacion.CertificacionIngenieria.Service
{
    public class ColaboradorCertificacionIngenieriaAsyncBaseCrudAppService : AsyncBaseCrudAppService<ColaboradorCertificacionIngenieria, ColaboradorCertificacionIngenieriaDto, PagedAndFilteredResultRequestDto>, IColaboradorCertificacionIngenieriaAsyncBaseCrudAppService
    {
        private readonly IBaseRepository<Colaboradores> _colaboradorRepository;
        private readonly IBaseRepository<Catalogo> _catalogoRepository;

        public ColaboradorCertificacionIngenieriaAsyncBaseCrudAppService(
            IBaseRepository<ColaboradorCertificacionIngenieria> repository,
            IBaseRepository<Colaboradores> colaboradorRepository,
            IBaseRepository<Catalogo> catalogoRepository
        ) : base(repository)
        {
            _colaboradorRepository = colaboradorRepository;
            _catalogoRepository = catalogoRepository;
        }

        public List<ColaboradorCertificacionIngenieriaDto> GetParametrizacionPorColaboradorId(int colaboradorId)
        {
            var parametros = Repository.GetAll()
                .Include(o => o.Ubicacion)
                .Include(o => o.Modalidad)
                .Include(o => o.Disciplina)
                .Where(o => o.ColaboradorId == colaboradorId)
                .OrderByDescending(o => o.Id)
                .ToList();
            return Mapper.Map<List<ColaboradorCertificacionIngenieria>, List<ColaboradorCertificacionIngenieriaDto>>(parametros);
        }

        public List<ColaboradorListadoIngenieriaDto> GetColaboradores()
        {
            var colaboradores = _colaboradorRepository.GetAll()
                .Where(o => o.estado == "ACTIVO" || o.estado == "INACTIVO" || o.estado == "ENVIADO SAP" || o.estado == "TEMPORAL")
                .ToList();
            var dtos = Mapper.Map<List<Colaboradores>, List<ColaboradorListadoIngenieriaDto>>(colaboradores);
            return dtos;
        }

        public async Task<ResultadoColaboradorRubro> CrearAsync(ColaboradorCertificacionIngenieriaDto dto)
        {
            /* Comprobar fechas*/
            var fechaFin = dto.FechaHasta.HasValue ? dto.FechaHasta.Value : DateTime.Now;
            var existe = ComprobarExistenciaParametrizacion(dto.FechaDesde, fechaFin, dto.ColaboradorId, dto.Id);
            if (existe)
            {
                return new ResultadoColaboradorRubro
                {
                    Success = false,
                    Message = "Ya existe una parametrización para el colaborador en estas fechas"
                };
            }

            /* Buscar un registro anterior del colaborador  */
            var registroAnterior = Repository.GetAll()
                .Where(o => o.ColaboradorId == dto.ColaboradorId)
                .Where(o=>o.DisciplinaId.HasValue)
                .Where(o => o.DisciplinaId.Value == dto.DisciplinaId.Value)
                .OrderByDescending(o => o.Id)
                .FirstOrDefault();

            /* Se actualizará fecha fin del registro anterior con “fecha inicio nuevo periodo – 1*/
            if (registroAnterior != null)
            {
                registroAnterior.FechaHasta = DateTime.Now.AddDays(-1);
            }

            var entity = Mapper.Map<ColaboradorCertificacionIngenieria>(dto);
            await Repository.InsertAsync(entity);
            return new ResultadoColaboradorRubro
            {
                Success = true,
                Message = ""
            };
        }

        public async Task<ResultadoColaboradorRubro> ActualizarAsync(ColaboradorCertificacionIngenieriaDto dto)
        {
            /* Comprobar fechas*/
            var fechaFin = dto.FechaHasta.HasValue ? dto.FechaHasta.Value : DateTime.Now;
            var existe = ComprobarExistenciaParametrizacion(dto.FechaDesde, fechaFin, dto.ColaboradorId, dto.Id);
            if (existe)
            {
                return new ResultadoColaboradorRubro
                {
                    Success = false,
                    Message = "Ya existe una parametrización para el colaborador en estas fechas"
                };
            }

            var entity = Mapper.Map<ColaboradorCertificacionIngenieria>(dto);
            await Repository.UpdateAsync(entity);
            return new ResultadoColaboradorRubro
            {
                Success = true,
                Message = ""
            }; ;
        }

        public bool Eliminar(int id)
        {
            Repository.Delete(id);
            return true;
        }

        public CatalogosCertificacionIngenieriaDto ObtenerCatalogos()
        {
            var disciplinas = _catalogoRepository.GetAll()
                .Include(o => o.TipoCatalogo)
                .Where(o => o.vigente)
                .Where(o => o.TipoCatalogo.codigo == "DISCIPLINA")
                .ToList();

            var ubicacion = _catalogoRepository.GetAll()
                .Include(o => o.TipoCatalogo)
                .Where(o => o.vigente)
                .Where(o => o.TipoCatalogo.codigo == "LOCACION_INGENIERIA_TIMESHEET")
                .ToList();

            var modalidad = _catalogoRepository.GetAll()
                .Include(o => o.TipoCatalogo)
                .Where(o => o.vigente)
                .Where(o => o.TipoCatalogo.codigo == "MODALIDAD_INGENIERIA_TIMESHEET")
                .ToList();

            return new CatalogosCertificacionIngenieriaDto()
            {
                Disciplina = Mapper.Map<List<Catalogo>, List<CatalogoDto>>(disciplinas),
                Modalidad = Mapper.Map<List<Catalogo>, List<CatalogoDto>>(modalidad),
                Ubicacion = Mapper.Map<List<Catalogo>, List<CatalogoDto>>(ubicacion),
            };
        }

        public bool ComprobarExistenciaParametrizacion(DateTime fechainicio, DateTime fechafin, int colaboradorId, int parametrizacionId)
        {
            var parametrizacionQuery = Repository.GetAll()
                .Where(c => c.ColaboradorId == colaboradorId);


            if (parametrizacionId > 0)
            {
                parametrizacionQuery = parametrizacionQuery.Where(o => o.Id != parametrizacionId);
            }

            var parametrizacionColaborador = parametrizacionQuery.ToList();

            bool result = false;
            foreach (var item in parametrizacionColaborador)
            {
                if (fechainicio >= item.FechaDesde && fechafin <= item.FechaHasta)
                {
                    result = true;
                    ;
                    break;
                }

                if (item.FechaDesde > fechainicio && item.FechaHasta < fechafin)
                {
                    result = true;
                    ;
                    break;
                }

                if (fechainicio > item.FechaDesde && fechainicio < item.FechaHasta)
                {
                    result = true;
                    ;
                    break;
                }

                if (fechafin > item.FechaDesde && fechafin < item.FechaHasta)
                {
                    result = true;
                    ;
                    break;
                }
            }

            return result;
        }
    }
}
