using AutoMapper;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.aplicacion.CertificacionIngenieria.Dto;
using com.cpp.calypso.proyecto.aplicacion.CertificacionIngenieria.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion.CertificacionIngenieria.Service
{
    public class ParametroSistemaAsyncBaseCrudAppService : AsyncBaseCrudAppService<ParametroSistema, ParametroSistemaDto, PagedAndFilteredResultRequestDto>, IParametroSistemaAsyncBaseCrudAppService
    {
        private readonly IBaseRepository<Modulo> _moduloRepository;

        public ParametroSistemaAsyncBaseCrudAppService(
            IBaseRepository<ParametroSistema> repository,
            IBaseRepository<Modulo> moduloRepository
        ) : base(repository)
        {
            _moduloRepository = moduloRepository;
        }


        public List<ParametroSistemaDto> ObtenerParametrosPorModuloCertificacion()
        {
            var moduloCertificacion = _moduloRepository.GetAll()
                .Where(o => o.Codigo == "mod_ingenieria")
                .FirstOrDefault();

            var list = Repository.GetAll()
                .Where(o => o.ModuloId == moduloCertificacion.Id)
                .ToList();

            return Mapper.Map<List<ParametroSistema>, List<ParametroSistemaDto>>(list);
        }

        public async Task<bool> ActualizarParametroAsync(ParametroSistemaDto dto)
        {
            var entity = Mapper.Map<ParametroSistema>(dto);
            await Repository.UpdateAsync(entity);
            return true;
        }
    }
}
