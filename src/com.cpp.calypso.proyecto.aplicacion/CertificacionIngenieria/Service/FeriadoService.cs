using AutoMapper;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.aplicacion.CertificacionIngenieria.Dto;
using com.cpp.calypso.proyecto.aplicacion.CertificacionIngenieria.Interface;
using com.cpp.calypso.proyecto.dominio.CertificacionIngenieria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion.CertificacionIngenieria.Service
{
    public class FeriadoAsyncBaseCrudAppService : AsyncBaseCrudAppService<Feriado, FeriadoDto, PagedAndFilteredResultRequestDto>, IFeriadoAsyncBaseCrudAppService
    {
        public FeriadoAsyncBaseCrudAppService(
            IBaseRepository<Feriado> repository
        ) : base(repository)
        {
        }

        public List<FeriadoDto> GetFeriados()
        {
            var feriados = Repository.GetAll().ToList();
            return Mapper.Map<List<Feriado>, List<FeriadoDto>>(feriados);
        }

        public async Task<bool> CrearFeriadoAsync(FeriadoDto dto)
        {
            var entity = Mapper.Map<Feriado>(dto);
            await Repository.InsertAsync(entity);
            return true;
        }

        public async Task<bool> ActualizarFeriadoAsync(FeriadoDto dto)
        {
            var entity = Mapper.Map<Feriado>(dto);
            await Repository.UpdateAsync(entity);
            return true;
        }

        public bool EliminarFeriado(int id)
        {
            Repository.Delete(id);
            return true;
        }
    }
}
