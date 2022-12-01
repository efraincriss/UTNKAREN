using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.aplicacion.Dto;
using com.cpp.calypso.proyecto.aplicacion.Interfaces;
using com.cpp.calypso.proyecto.dominio;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion.Service
{
    public class ZonaProveedorAsyncBaseCrudAppService : AsyncBaseCrudAppService<ZonaProveedor, ZonaProveedorDto, PagedAndFilteredResultRequestDto>,
        IZonaProveedorAsyncBaseCrudAppService
    {
        private readonly IBaseRepository<ContratoProveedor> _contratorepository;
        private readonly IBaseRepository<TipoOpcionComida> _tipoopioncomidarepository;
        public ZonaProveedorAsyncBaseCrudAppService(
            IBaseRepository<ZonaProveedor> repository,
            IBaseRepository<ContratoProveedor> contratorepository,
            IBaseRepository<TipoOpcionComida> tipoopioncomidarepository
            ) : base(repository)
        {
            _contratorepository = contratorepository;
            _tipoopioncomidarepository = tipoopioncomidarepository;
        }

        public async Task<IList<ZonaProveedorInfoDto>> GetInfoAll(int TipoComidaId) //Parametro vacio
        {
            List<ZonaProveedor> query = new List<ZonaProveedor>();
            var all = Repository.GetAll().ToList();

            var currentDate = DateTime.Today;
            //VERIFICAR SI TIENE CONTRATO
            foreach (var p in all)
            {
                var contratoVigente = _contratorepository.GetAll()
               .Where(o => o.ProveedorId == p.ProveedorId)
               .Where(o => o.estado == ContratoEstado.Activo)
               .Where(o => currentDate >= o.fecha_inicio && currentDate <= o.fecha_fin)
               .FirstOrDefault();

                if (contratoVigente != null && contratoVigente.Id > 0)
                {
                    var TipoComida = _tipoopioncomidarepository.GetAll()
                                                              .Where(c => c.tipo_comida_id == TipoComidaId)
                                                              .Where(c => c.contrato.Id == contratoVigente.Id)
                                                              .FirstOrDefault();
                    if (TipoComida != null && TipoComida.Id > 0)
                    {
                        query.Add(p);
                    }
                }

            }

            var queryDto = (from item in query
                            where
                            item.Proveedor.estado == proyecto.dominio.Proveedor.ProveedorEstado.Activo
                            select new ZonaProveedorInfoDto()
                            {
                                Id = item.Id,
                                ZonaId = item.ZonaId,
                                identificacion = item.Proveedor.identificacion,
                                ProveedorId = item.ProveedorId,
                                razon_social = item.Proveedor.razon_social,
                                zona_nombre = item.Zona.nombre
                            }
                      );

            return  queryDto.OrderBy(c=>c.razon_social).ToList();
        }
    }
}
