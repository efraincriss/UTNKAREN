using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.aplicacion.Interfaces;
using com.cpp.calypso.proyecto.dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion.Service
{
  public  class DestinatarioCartaServiceAsyncBaseCrudAppService : AsyncBaseCrudAppService<DestinatarioCarta, DestinatarioCartaDto, PagedAndFilteredResultRequestDto>, IDestinatarioCartaAsyncBaseCrudAppService
    {
        public readonly IBaseRepository<Carta> _repositorycarta;
        public DestinatarioCartaServiceAsyncBaseCrudAppService(IBaseRepository<DestinatarioCarta> repository,
            IBaseRepository<Carta> repositorycarta) : base(repository)
        {
            _repositorycarta = repositorycarta;
        }

        public List<DestinatarioCartaDto> GetDestinatarioCartas(int CartaId)
        {
            var query = Repository.GetAllIncluding(c => c.Carta, c => c.Destinatario);
            var items = (from r in query
                         where r.CartaId == CartaId
                         where r.vigente == true
                         select new DestinatarioCartaDto()
                         {
                             Id = r.Id,
                            Destinatario=r.Destinatario,
                            Carta=r.Carta,
                            CartaId=r.CartaId,
                            DestinatarioId=r.DestinatarioId,
                             estado = r.estado,
                             vigente = r.vigente,
                         
                         }).ToList();
            return items;
        }

        public DestinatarioCartaDto getdetalle(int DestinatarioCartaId)
        {
            var query = Repository.GetAllIncluding(c => c.Carta, c => c.Destinatario);
            var items = (from r in query
                         where r.Id == DestinatarioCartaId
                         where r.vigente == true
                         select new DestinatarioCartaDto()
                         {
                             Id = r.Id,
                             Destinatario = r.Destinatario,
                             Carta = r.Carta,
                             CartaId = r.CartaId,
                             DestinatarioId = r.DestinatarioId,
                             estado = r.estado,
                             vigente = r.vigente,

                         }).FirstOrDefault();
            return items;
        }

        public bool RegistrarCartasDestinatario(CartaDto c, int[] destinoseleccionados)
        {
            if (destinoseleccionados.Count() > 0)
            {
                foreach (var item in destinoseleccionados)
                {
                    DestinatarioCartaDto a = new DestinatarioCartaDto()
                    {
                        CartaId = c.Id,
                        DestinatarioId = item,
                        //estado = c.estado,
                        vigente = true
                    };
                    
                    Repository.Insert(MapToEntity(a));
                }
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}