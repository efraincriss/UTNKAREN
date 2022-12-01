using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.dominio;
using com.cpp.calypso.proyecto.dominio.Models;

namespace com.cpp.calypso.proyecto.aplicacion
{
    public class ListaDistribucionAsyncBaseCrudAppService : AsyncBaseCrudAppService<ListaDistribucion, ListaDistribucionDto, PagedAndFilteredResultRequestDto>, IListaDistribucionAsyncBaseCrudAppService
    {
        private readonly IBaseRepository<CorreoLista> _correoListarepository;
        private readonly IBaseRepository<Usuario> _usuarioRepository;
        private readonly IBaseRepository<CorreoExterno> _correoExternoRepository;

        public ListaDistribucionAsyncBaseCrudAppService(
            IBaseRepository<ListaDistribucion> repository,
            IBaseRepository<CorreoLista> correoListarepository,
            IBaseRepository<Usuario> usuarioRepository,
            IBaseRepository<CorreoExterno> correoExternoRepository
        ) : base(repository)
        {
            _correoListarepository = correoListarepository;
            _usuarioRepository = usuarioRepository;
            _correoExternoRepository = correoExternoRepository;
        }

        public List<ListaDistribucionDto> listar()
        {
            var query = Repository.GetAll();

            var lista = (from l in query
                         where l.vigente == true
                         select new ListaDistribucionDto()
                         {
                             Id = l.Id,
                             vigente = l.vigente,
                             estado = l.estado,
                             nombre = l.nombre
                         }).ToList();

            return lista;
        }

        public List<CorreoListaDto> GetCorreosInternos(int listaId)
        {
            var query = _correoListarepository.GetAll()
                .Where(o => o.externo == false)
                .Where(o => o.ListaDistribucionId == listaId)
                .Where(o => o.vigente == true)
                .OrderBy(o => o.orden)
                .ToList() ;

            var correos = (from c in query
                           select new CorreoListaDto()
                           {
                               Id = c.Id,
                               orden = c.orden,
                               correo = c.correo,
                               nombres = c.nombres,
                               externo = c.externo,
                               seccion = c.seccion,
                               nombre_seccion = Enum.GetName(typeof(SeccionCorreo), c.seccion)
                           })
                           .OrderBy(o => o.orden)
                           .ToList();

            return correos;
        }

        public List<CorreoListaDto> GetCorreosExternos(int listaId)
        {
            var query = _correoListarepository.GetAll()
                .Where(o => o.externo == true)
                .Where(o => o.ListaDistribucionId == listaId)
                .Where(o => o.vigente == true)
                .OrderBy(o => o.orden)
                .ToList();

            var correos = (from c in query
                           select new CorreoListaDto()
                           {
                               Id = c.Id,
                               orden = c.orden,
                               correo = c.correo,
                               nombres = c.nombres,
                               externo = c.externo,
                               seccion=c.seccion,
                               nombre_seccion = Enum.GetName(typeof(SeccionCorreo),c.seccion)
                           })
                           .OrderBy(o => o.orden)
                           .ToList();

            return correos;
        }

        public List<CorreoListaDto> GetCorreosInternosParaIngresar(int listaId)
        {
            var query = _usuarioRepository.GetAll();

            var queryLista = _correoListarepository.GetAll()
                .Where(o => o.externo == false)
                .Where(o => o.ListaDistribucionId == listaId)
                .Where(o => o.vigente == true);

            var correos = (from c in query
                           where !(from l in queryLista
                                   select l.correo).Contains(c.Correo)
                           select new CorreoListaDto()
                           {
                               Id = c.Id,
                               correo = c.Correo,
                               nombres =c.Apellidos+"_"+c.Nombres,

                           }).ToList();

            return correos;
        }

        public List<CorreoListaDto> GetCorreosExternosParaIngresar(int listaId)
        {
            var query = _correoExternoRepository.GetAll()
                .Where(o => o.vigente == true);

            var queryLista = _correoListarepository.GetAll()
                .Where(o => o.externo == true)
                .Where(o => o.ListaDistribucionId == listaId)
                .Where(o => o.vigente == true);

            var correso = (from c in query
                           where !(from l in queryLista
                                   select l.correo).Contains(c.correo)
                           select new CorreoListaDto()
                           {
                               Id = c.Id,
                               correo = c.correo,
                               nombres = c.nombre,
                           }).ToList();

            return correso;
        }

        public bool EliminarCorreoLista(int Id)
        {
            try
            {
                var correo = _correoListarepository.Get(Id);
                _correoListarepository.Delete(correo);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }

        }

        public List<UserCorreos> GetCorreosListos(string codigo)
        {
            var correos = _correoListarepository.GetAllIncluding(c => c.ListaDistribucion)
                                                .Where(c=>c.vigente)
                                                .Where(c=>c.ListaDistribucion.codigo==codigo)
                                                .ToList();
            var list = (from l in correos
                        select new UserCorreos()
                        {
                            nombres=l.nombres,
                            correo=l.correo

                        }).ToList();
            return list;
        
        }

        public bool ActualizarLista(CorreoListaDto correo)
        {
            var e = _correoListarepository.Get(correo.Id);
            e.seccion = correo.seccion;
            _correoListarepository.Update(e);
            return true;
        }

        public bool OrdenarCorreos(List<CorreoListaDto> correos)
        {
            int count = 1;
            foreach (var correo in correos)
            {
                var e = _correoListarepository.Get(correo.Id);
                e.orden = count;
                count++;
                _correoListarepository.Update(e);
            }
            return true;
        }
    }
}
