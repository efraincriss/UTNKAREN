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
    public class CartaArchivoServiceAsyncBaseCrudAppService : AsyncBaseCrudAppService<CartaArchivo, CartaArchivoDto, PagedAndFilteredResultRequestDto>, ICartaArchivoAsyncBaseCrudAppService
    {

        public CartaArchivoServiceAsyncBaseCrudAppService(IBaseRepository<CartaArchivo> repository) : base(repository)
        {

        }

        public CartaArchivoDto getdetalle(int CartaArchivoId)
        {
            var query = Repository.GetAllIncluding(c => c.Carta);
            var items = (from r in query
                         where r.Id == CartaArchivoId
                         where r.vigente == true
                         select new CartaArchivoDto()
                         {
                             Id = r.Id,
                             CartaId = r.CartaId,
                             Carta = r.Carta,
                             vigente = r.vigente,
                             descripcion = r.descripcion
                         }
                                 ).FirstOrDefault();
            return items;
        }

        public List<CartaArchivoDto> ListaArchivosporCarta(int CartaId)
        {
            var query = Repository.GetAllIncluding(c => c.Carta, c => c.Archivo).Where(c => c.CartaId == CartaId).ToList();
            var items = (from r in query
                         where r.CartaId == CartaId
                         where r.vigente == true
                         select new CartaArchivoDto()
                         {
                             Id = r.Id,
                             CartaId = r.CartaId,
                             vigente = r.vigente,
                             ArchivoId = r.ArchivoId,
                             nombreArchivo = r.Archivo.nombre,
                             descripcion = r.descripcion
                         }
                                 ).ToList();
            return items;
        }
    }
}

