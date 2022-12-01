using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.dominio;

namespace com.cpp.calypso.proyecto.aplicacion
{
    public class CorreoListaAsyncBaseCrudAppService : AsyncBaseCrudAppService<CorreoLista, CorreoListaDto, PagedAndFilteredResultRequestDto>, ICorreoListaAsyncBaseCrudAppService
    {
        public CorreoListaAsyncBaseCrudAppService(
            IBaseRepository<CorreoLista> repository
        ) : base(repository)
        {
        }

        public List<CorreoListaDto> listar()
        {
            var query = Repository.GetAllIncluding(o => o.ListaDistribucion);
            var correos = (from c in query
                where c.vigente == true
                select new CorreoListaDto()
                {
                    Id = c.Id,
                    ListaDistribucionId = c.ListaDistribucionId,
                    ListaDistribucion = c.ListaDistribucion,
                    nombres = c.nombres,
                    UsuarioId = c.UsuarioId,
                    correo = c.correo,
                    externo = c.externo,
                    identificacion = c.identificacion
                }).ToList();
            return correos;
        }
    }
}
