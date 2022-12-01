using System;
using System.Collections.Generic;
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
    public class DetalleReservaAsyncBaseCrudAppService : AsyncBaseCrudAppService<DetalleReserva, DetalleReservaDto, PagedAndFilteredResultRequestDto>, IDetalleReservaAsyncBaseCrudAppService
    {
        public DetalleReservaAsyncBaseCrudAppService(
            IBaseRepository<DetalleReserva> repository
            ) : base(repository)
        {
        }


        public bool EliminarDetallesPorReservaId(int reservaId)
        {
            var count = Repository.GetAll()
                .Where(o => o.ReservaHotelId == reservaId)
                .Count(o => o.consumido);

            if (count > 0)
            {
                return false;
            }

            var entities = Repository.GetAll()
                .Where(o => o.ReservaHotelId == reservaId)
                .ToList();

            foreach (var detalleReserva in entities)
            {
                Repository.Delete(detalleReserva);
                
            }

            return true;
        }


        public bool EliminarReservasPosterioresA(int reservaId, DateTime fecha)
        {
            var count = Repository.GetAll()
                .Where(o => o.ReservaHotelId == reservaId)
                .Where(o => o.fecha_reserva > fecha)
                .Count(o => o.consumido);

            if (count > 0)
            {
                return false;
            }

            var entities = Repository.GetAll()
                .Where(o => o.ReservaHotelId == reservaId)
                .Where(o => o.fecha_reserva > fecha)
                .ToList();

            foreach (var detalleReserva in entities)
            {
                Repository.Delete(detalleReserva);
            }

            return true;
        }

        public List<DetalleReservaDto> ListarPorReservaId(int reservaId)
        {
            var entities = Repository.GetAll()
                .Where(o => o.ReservaHotelId == reservaId)
                .ToList();

            var dtos = Mapper.Map<List<DetalleReserva>, List<DetalleReservaDto>>(entities);

            return dtos;
        }
    }
}
