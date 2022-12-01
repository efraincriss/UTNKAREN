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
    public class DestinatarioServiceAsyncBaseCrudAppService : AsyncBaseCrudAppService<Destinatario, DestinatarioDto, PagedAndFilteredResultRequestDto>, IDestinatarioAsyncBaseCrudAppService
    {
        
        public DestinatarioServiceAsyncBaseCrudAppService(IBaseRepository<Destinatario> repository) : base(repository)
        {
        
        }

        public List<DestinatarioDto> getDestinatarios()
        {
            var Query = Repository.GetAll();
            var items = (from r in Query
                         where r.vigente == true
                          select new DestinatarioDto()
                         {
                              Id = r.Id,
                              intitucion = r.intitucion,
                              cargo = r.cargo,
                              nombre = r.nombre,
                              vigente = r.vigente

                          }).ToList();
            return items;
        }

        public DestinatarioDto getdetalle(int destId) {
            var Query = Repository.GetAll().Where(e => e.vigente == true).ToList();

            var item = (from r in Query
                        where r.Id == destId
                        where r.vigente == true
                        select new DestinatarioDto()
                        {
                            Id = r.Id,
                            intitucion = r.intitucion,
                            cargo = r.cargo,
                            nombre = r.nombre,
                            vigente = r.vigente

                        }).FirstOrDefault(); // Tercera
            return item;
        }
    }
}