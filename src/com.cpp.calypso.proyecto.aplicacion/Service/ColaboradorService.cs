using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.dominio;
using com.cpp.calypso.seguridad.aplicacion;

namespace com.cpp.calypso.proyecto.aplicacion
{
    public class ColaboradorAsyncBaseCrudAppService : AsyncBaseCrudAppService<Colaborador, ColaboradorDto, PagedAndFilteredResultRequestDto>, IColaboradorAsyncBaseCrudAppService
    {
        private readonly IBaseRepository<Usuario> _usuariorepository;
        private readonly IBaseRepository<DetallePreciario> _detallepreciario;
        private readonly IBaseRepository<Catalogo> _catalogo;

        public ColaboradorAsyncBaseCrudAppService(
            IBaseRepository<Colaborador> repository,
            IBaseRepository<Usuario> usuariorepository,
            IBaseRepository<DetallePreciario> detallepreciario,
            IBaseRepository<Catalogo> catalogo

            ) : base(repository)
        {
            _usuariorepository = usuariorepository;
            _detallepreciario = detallepreciario;
            _catalogo = catalogo;
        }

        public bool buscarcolaborador(string cedula)
        {
            var query = Repository.GetAll()
                .Where(o => o.vigente == true);

            var c = (from co in query
                     where co.cedula == cedula
                     select new ColaboradorDto()
                     {
                         Id = co.Id,
                         nombres = co.nombres,
                         apellidos = co.apellidos,
                         correo = co.correo,
                     }).FirstOrDefault();
            if (c.Id > 0)
            {
                return true;
            }

            return false;
        }

        public Colaborador buscarcolaboradortcu(string tcu)
        {
            var query = Repository.GetAll()
               .Where(o => o.vigente == true).Where(o => o.cedula == tcu).FirstOrDefault();

            if (query != null && query.Id > 0)
            {
                return query;
            }
            else
            {
                return new Colaborador();
            }
        }

        public UsuarioDto buscarusuarioporcedula(string cedula)
        {
            var usuario = _usuariorepository.GetAll().Where(c => c.Identificacion == cedula).FirstOrDefault();

            if (usuario != null)
            {
                var usuarioDto = new UsuarioDto
                {
                    Id = usuario.Id,
                    Cuenta = usuario.Cuenta,
                    Apellidos = usuario.Apellidos,
                    Nombres = usuario.Nombres,
                    Identificacion = usuario.Identificacion

                };
                return usuarioDto;
            }
            else
            {
                return null;
            }
        }



        public List<DetallePreciario> items_ingenieria_contrato(int contratoid)
        {
            var lista_items = _detallepreciario.GetAll()
                                                  .Where(c => c.Preciario.fecha_desde <= DateTime.Now)
                                                  .Where(c => c.Preciario.fecha_hasta >= DateTime.Now)
                                                  .Where(c => c.Preciario.ContratoId == contratoid)
                                                  .Where(c => c.Preciario.vigente)
                                                  .Where(c => c.Item.GrupoId == 1)
                                                  .Where(c => c.vigente)
                                                  .Select(c => c).ToList();
            return lista_items;

        }

        public List<ColaboradorDto> Listar()
        {
            var query = Repository.GetAll()
                .Where(o => o.vigente == true);

            var c = (from co in query
                     select new ColaboradorDto()
                     {
                         Id = co.Id,
                         nombres = co.nombres,
                         apellidos = co.apellidos,
                         correo = co.correo,
                         cedula = co.cedula,
                         nombre_cliente = co.Cliente != null ? co.Cliente.nombre : ""

                     }).ToList();
            return c;
        }

        public int CrearColaborador(Colaborador colaborador)
        {
            var e = Repository.GetAll().Where(c => c.cedula == colaborador.cedula).FirstOrDefault();
            if (e == null)
            {
                var id = Repository.InsertAndGetId(colaborador);
                return id;
            }
            else
            {
                return -1;
            }


        }

        public int EditColaborador(Colaborador e)
        {
            var en = Repository.GetAll().Where(c => c.cedula == e.cedula).Where(c => c.Id != e.Id).FirstOrDefault();
            if (en == null)
            {
                var u = Repository.Get(e.Id);
                u.cedula = e.cedula;
                u.nombres = e.nombres;
                u.apellidos = e.apellidos;
                u.correo = e.correo;
                u.ClienteId = e.ClienteId;
                u.vigente = e.vigente;
                var up = Repository.Update(u);
                return up.Id;
            }
            else
            {
                return -1;
            }

        }

        public bool DeleteColaborador(int id)
        {
            var d = Repository.Get(id);
            Repository.Delete(d);
            return true;
        }

        public List<ColaboradorDto> ListAll()
        {
            var query = Repository.GetAllIncluding(c => c.Cliente).ToList();
            var list = (from c in query
                        select new ColaboradorDto()
                        {
                            Id = c.Id,
                            apellidos = c.apellidos,
                            nombres = c.nombres,
                            cedula = c.cedula,
                            ClienteId = c.ClienteId,
                            correo = c.correo,
                            vigente = c.vigente,
                            nombre_cliente = c.Cliente.nombre
                        }).ToList();
            return list;

        }

        public List<CatalogoDto> ListTypesUser()
        {
            var query = _catalogo.GetAll().Where(c => c.vigente)
                                         .Where(c => c.TipoCatalogo.codigo == "TUSERTRANSMITTAL")
                                         .ToList();
            var list = (from q in query
                        select new CatalogoDto()
                        {
                            Id=q.Id,
                            nombre=q.nombre,
                            codigo=q.codigo,
                            ordinal=q.ordinal,
                            descripcion=q.descripcion,
                            vigente=q.vigente,
                            TipoCatalogoId=q.TipoCatalogoId,
                            predeterminado=q.predeterminado
                            
                        }).ToList();
            return list;
        }
    }
}
