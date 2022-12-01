using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.aplicacion.Proveedor.Dto;
using com.cpp.calypso.proyecto.aplicacion.Proveedor.Interfaces;
using com.cpp.calypso.proyecto.dominio.Proveedor;

namespace com.cpp.calypso.proyecto.aplicacion.Proveedor
{
    public class TarifaHotelAsyncBaseCrudAppService : AsyncBaseCrudAppService<TarifaHotel, TarifaHotelDto, PagedAndFilteredResultRequestDto>, ITarifaHotelAsyncBaseCrudAppService
    {
        public TarifaHotelAsyncBaseCrudAppService(
            IBaseRepository<TarifaHotel> repository
            ) : base(repository)
        {
        }



        public void DesactivarTarifa(int TarifaHotelId)
        {
            var entity = Repository.Get(TarifaHotelId);
            entity.estado = false;
            Repository.Update(entity);
        }

        public void ActivarTarifa(int tarifaId)
        {
            var entity = Repository.Get(tarifaId);
            entity.estado = true;
            Repository.Update(entity);
        }


        public List<TarifaHotelDto> ListarPorContrato(int ContratoId)
        {
            var entities = Repository.GetAll()
                .Include(o => o.TipoHabitacion)
                .Where(o => o.ContratoProveedorId == ContratoId)
                .ToList();

            var dtos = Mapper.Map<List<TarifaHotel>, List<TarifaHotelDto>>(entities);

            var count = 1;
            foreach (var dto in dtos)
            {
                dto.secuencial = count;
                count++;
            }
            return dtos;
        }

        public bool TarifaUnica(int contratoProveedorId, int tipoHabitacionId)
        {
            var count = Repository.GetAll()
                .Where(o => o.ContratoProveedorId == contratoProveedorId)
                .Count(o => o.TipoHabitacionId == tipoHabitacionId);

            if (count > 0)
                return false;
            return true;
        }
    }
}
