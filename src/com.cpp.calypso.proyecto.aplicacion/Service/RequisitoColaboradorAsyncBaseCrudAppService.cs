using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.aplicacion.Dto;
using com.cpp.calypso.proyecto.aplicacion.Interfaces;
using com.cpp.calypso.proyecto.dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion.Service
{
    public class RequisitoColaboradorAsyncBaseCrudAppService : AsyncBaseCrudAppService<RequisitoColaborador, RequisitoColaboradorDto, PagedAndFilteredResultRequestDto>, IRequisitoColaboradorAsyncBaseCrudAppService
    {
        private readonly ICatalogoAsyncBaseCrudAppService _catalogoRepository;

        public RequisitoColaboradorAsyncBaseCrudAppService(
            IBaseRepository<RequisitoColaborador> repository,
            ICatalogoAsyncBaseCrudAppService catalogoRepository
            ) : base(repository)
        {
            _catalogoRepository = catalogoRepository;
        }

        public List<RequisitoColaboradorDto> GetList()
        {
            var i = 1;
            var query = Repository.GetAllIncluding(c => c.Requisitos);

            var requisitos = (from d in query
                              where d.vigente == true
                              select new RequisitoColaboradorDto
                              {
                                  Id = d.Id,
                                  tipo_usuarioId = d.tipo_usuarioId,
                                  RequisitosId = d.RequisitosId,
                                  descripcion = d.descripcion,
                                  rolId = d.rolId,
                                  obligatorio = d.obligatorio,
                                  requiere_archivo = d.requiere_archivo,
                                  Requisitos= d.Requisitos,
                                  vigente = d.vigente,
                                  catalogo_tipo_ausentismo_id = d.catalogo_tipo_ausentismo_id
                              }).ToList();

            foreach(var e in requisitos)
            {
                e.nro = i++;

                var cat_u = _catalogoRepository.GetCatalogo(e.tipo_usuarioId);
                e.usuario = cat_u.nombre;
                
                e.requisito = e.Requisitos.nombre;

                var r = _catalogoRepository.GetCatalogo(e.rolId);
                e.nombre_rol = r.nombre;

                if (e.vigente)
                {
                    e.nombre_estado = "Activo";
                }
                else
                {
                    e.nombre_estado = "Inactivo";
                }

                if (e.obligatorio)
                {
                    e.nombre_obligatorio = "SI";
                }
                else
                {
                    e.nombre_obligatorio = "NO";
                }

                if (e.requiere_archivo)
                {
                    e.nombre_requiere = "SI";
                }
                else
                {
                    e.nombre_requiere = "NO";
                }
            }

            return requisitos;
        }

        public List<RequisitoColaboradorDto> GetListAsync()
        {
            throw new NotImplementedException();
        }

        public RequisitoColaboradorDto GetRequisito(int Id)
        {
            var d = Repository.Get(Id);

            RequisitoColaboradorDto requisito = new RequisitoColaboradorDto()
            {
                Id = d.Id,
                tipo_usuarioId = d.tipo_usuarioId,
				RequisitosId = d.RequisitosId,
                descripcion = d.descripcion,
				rolId = d.rolId,
                obligatorio = d.obligatorio,
                requiere_archivo = d.requiere_archivo,
                vigente = d.vigente,
                catalogo_tipo_ausentismo_id = d.catalogo_tipo_ausentismo_id
            };

            if (requisito.vigente)
            {
                requisito.nombre_estado = "Activo";
            }
            else
            {
                requisito.nombre_estado = "Inactivo";
            }

            return requisito;
        }

		public List<RequisitoColaboradorDto> GetRequisitoPorGrupo(int grupo)
		{
            var aux = 1;
			var query = Repository.GetAllIncluding(o => o.Requisitos).Where(c => c.tipo_usuarioId == grupo && c.vigente == true).OrderBy(c=> c.Requisitos.nombre);

			var requisitos = (from d in query
							  select new RequisitoColaboradorDto
							  {
								  Id = d.Id,
								  tipo_usuarioId = d.tipo_usuarioId,
								  RequisitosId = d.RequisitosId,
								  descripcion = d.descripcion,
								  rolId = d.rolId,
								  obligatorio = d.obligatorio,
								  requiere_archivo = d.requiere_archivo,
								  vigente = d.vigente,
								  Requisitos = d.Requisitos,
                                  catalogo_tipo_ausentismo_id = d.catalogo_tipo_ausentismo_id,
                                  nombre_accion = d.Accion.nombre
                              }).ToList();
            foreach (var e in requisitos)
            {
                e.nro = aux++;
                var rol = _catalogoRepository.GetCatalogo(e.rolId);
                e.nombre_rol = rol.codigo;
            }

                return requisitos;
		}

        public List<RequisitoColaboradorDto> GetRequisitosPorAccionApi(int idAccion, int idGrupoPersonal)
        {
            var query = Repository.GetAll().Where(o => o.vigente == true && o.tipo_usuarioId == idGrupoPersonal && o.catalogo_tipo_ausentismo_id == idAccion);

            List<RequisitoColaboradorDto> requisitos = (from d in query
                              select new RequisitoColaboradorDto
                              {
                                  Id = d.Id,
                                  tipo_usuarioId = d.tipo_usuarioId,
                                  RequisitosId = d.RequisitosId,
                                  descripcion = d.descripcion,
                                  rolId = d.rolId,
                                  obligatorio = d.obligatorio,
                                  requiere_archivo = d.requiere_archivo,
                                  vigente = d.vigente,
                                  Requisitos = d.Requisitos,
                                  catalogo_tipo_ausentismo_id = d.catalogo_tipo_ausentismo_id
                              }).ToList();

            var numero = 1;

            foreach (var e in requisitos)
            {
                e.nro = numero;
                numero++;
            }

            return requisitos;
        }

        public string UniqueRequisito(int idAccion, int idGrupoPersonal, int idRequisito, int id)
        {
            var query = Repository.GetAll().Where(o => o.vigente == true && o.tipo_usuarioId == idGrupoPersonal && o.rolId == idAccion && o.RequisitosId == idRequisito).FirstOrDefault();

            if (query != null)
            {
                if (query.Id == id)
                {
                    return "UPDATE";
                }
                else {
                    return "SI";
                }
            }
            else {
                return "NO";
            }
            
            
        }

        public List<RequisitoColaboradorDto> GetRequisitosPorFiltros(int? tipo_usuario, int? accion, string requisitos)
        {
            var query = Repository.GetAll().Where(a => a.vigente == true).Where(c=>c.activo);

            if (tipo_usuario != null)
            {
                query = query.Where(x => x.tipo_usuarioId == tipo_usuario);
            }

            if (accion != null)
            {
                query = query.Where(x => x.rolId == accion);
            }

            if (requisitos != "")
            {
                query = query.Where(x => x.Requisitos.nombre.StartsWith(requisitos));
            }

            var result = (from d in query
                              where d.vigente == true
                              select new RequisitoColaboradorDto
                              {
                                  Id = d.Id,
                                  tipo_usuarioId = d.tipo_usuarioId,
                                  RequisitosId = d.RequisitosId,
                                  descripcion = d.descripcion,
                                  rolId = d.rolId,
                                  obligatorio = d.obligatorio,
                                  requiere_archivo = d.requiere_archivo,
                                  Requisitos = d.Requisitos,
                                  vigente = d.vigente,
                                  catalogo_tipo_ausentismo_id = d.catalogo_tipo_ausentismo_id,
                                  usuario = d.TipoGrupoPersonal.nombre,
                                  requisito = d.Requisitos.nombre,
                                  nombre_rol = d.Accion.nombre,
                                  nombre_estado = d.vigente == true ? "Activo" : "Inactivo",
                                  nombre_obligatorio = d.obligatorio == true ? "SI" : "NO",
                                  nombre_requiere = d.requiere_archivo == true ? "SI" : "NO",
                              }).ToList();

            return result;

        }
    }
}
