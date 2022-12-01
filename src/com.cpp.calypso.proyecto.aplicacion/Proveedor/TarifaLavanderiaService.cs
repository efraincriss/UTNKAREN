using AutoMapper;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.aplicacion.Proveedor.Dto;
using com.cpp.calypso.proyecto.aplicacion.Proveedor.Interfaces;
using com.cpp.calypso.proyecto.dominio.Proveedor;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion.Proveedor
{
   public class TarifaLavanderiaAsyncBaseCrudAppService : AsyncBaseCrudAppService<TarifaLavanderia, TarifaLavanderiaDto, PagedAndFilteredResultRequestDto>, ITarifaLavanderiaAsyncBaseCrudAppService
    {

        public TarifaLavanderiaAsyncBaseCrudAppService(
            IBaseRepository<TarifaLavanderia> repository
            ) : base(repository)
        {
        }

        public bool CrearTarifa(TarifaLavanderia entity)
        {
            Repository.Insert(entity);

            return true;
        }

        public bool EditarTarifa(TarifaLavanderia entity)
        {
            var e = Repository.Get(entity.Id);
            e.valor_servicio = entity.valor_servicio;
            return true;
        }

        public bool EliminarTarifa(int id)
        {
            var entity = Repository.Get(id);

            Repository.Delete(entity);

            return true;
        }
        public bool TarifaUnica(int contratoProveedorId, int tipoServicioId)
        {
            var count = Repository.GetAll()
                .Where(o => o.ContratoProveedorId == contratoProveedorId)
                .Count(o => o.TipoServicioId == tipoServicioId);

            if (count > 0)
                return false;
            return true;
        }
        public List<TarifaLavanderiaDto> ListarPorContrato(int ContratoId)
        {

            var entities = Repository.GetAll()
                .Include(o => o.TipoServicio)
                .Where(o => o.ContratoProveedorId == ContratoId)
                .ToList();

            var dtos = Mapper.Map<List<TarifaLavanderia>, List<TarifaLavanderiaDto>>(entities);

            var count = 1;
            foreach (var dto in dtos)
            {
                dto.secuencial = count;
                count++;
            }
            return dtos;
        }

        public void DesactivarTarifa(int TarifaId)
        {
            var entity = Repository.Get(TarifaId);
            entity.estado = false;
            Repository.Update(entity);
        }

        public void ActivarTarifa(int tarifaId)
        {
            var entity = Repository.Get(tarifaId);
            entity.estado = true;
            Repository.Update(entity);
        }
    }
}
