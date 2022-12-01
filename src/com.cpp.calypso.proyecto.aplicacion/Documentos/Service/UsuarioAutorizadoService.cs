using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.aplicacion.Documentos.Dto;
using com.cpp.calypso.proyecto.aplicacion.Documentos.Interface;
using com.cpp.calypso.proyecto.dominio.Documentos;
using com.cpp.calypso.seguridad.aplicacion;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion.Documentos.Service
{
    public class UsuarioAutorizadoAsyncBaseCrudAppService : AsyncBaseCrudAppService<UsuarioAutorizado, UsuarioAutorizadoDto, PagedAndFilteredResultRequestDto>, IUsuarioAutorizadoAsyncBaseCrudAppService
    {
        private readonly IBaseRepository<Usuario> _userRepository;

        public UsuarioAutorizadoAsyncBaseCrudAppService(
            IBaseRepository<UsuarioAutorizado> repository,
            IBaseRepository<Usuario> userRepository
        ) : base(repository)
        {
            _userRepository = userRepository;
        }

        public List<UsuarioDto> ObtenerUsuariosAutorizadosPorContratoId(int id)
        {
            var usuarios = Repository.GetAll()
                .Include(o => o.Usuario)
                .Where(o => o.CarpetaId == id)
                .Select(o => o.Usuario)
                .ToList();
            var dtos = AutoMapper.Mapper.Map<List<UsuarioDto>>(usuarios);
            return dtos;
        }

        public List<UsuarioDto> ObtenerUsuariosDisponiblesPorContrato(int id)
        {
            var usuariosAsignados = Repository.GetAll()
                .Include(o => o.Usuario)
                .Where(o => o.CarpetaId == id)
                .Select(o => o.Usuario)
                .ToList();

            var usuariosDisponibles = _userRepository.GetAll()
                .Include(o => o.Modulos)
                .Where(u => u.Roles.Any(o => o.Codigo == "LECTOR_CONTRATO"))
                .ToList();

            foreach (var user in usuariosAsignados)
            {
                usuariosDisponibles.Remove(user);
            }

            //usuariosAsignados.AddRange(usuariosDisponibles);
            //var unique = usuariosAsignados.Distinct().ToList();

            var dtos = AutoMapper.Mapper.Map<List<UsuarioDto>>(usuariosDisponibles);
            return dtos;
        }

        public void AgregarUsuarios(List<int> usuarios, int carpetaId)
        {
            foreach (var user in usuarios)
            {
                var exist = Repository.GetAll()
                .Where(o => o.UsuarioId == user)
                .Where(o => o.CarpetaId == carpetaId)
                .Any();

                if (!exist)
                {
                    var entity = new UsuarioAutorizado
                    {
                        CarpetaId = carpetaId,
                        UsuarioId = user
                    };
                    Repository.Insert(entity);
                }
            }
        }

        public ResultadoEliminacionResponse EliminarUsuarioAutorizado(int usuario, int carpetaId)
        {
            var exist = Repository.GetAll()
                .Where(o => o.UsuarioId == usuario)
                .Where(o => o.CarpetaId == carpetaId)
                .Any();

            if (exist)
            {
                var entity = Repository.GetAll()
                .Where(o => o.UsuarioId == usuario)
                .Where(o => o.CarpetaId == carpetaId)
                .OrderByDescending(q => q.Id)
                .FirstOrDefault();

                Repository.Delete(entity);

                return new ResultadoEliminacionResponse
                {
                    Eliminado = true
                };
            }

            return new ResultadoEliminacionResponse
            {
                Eliminado = false,
                Error = "No se encontró el usuario"
            };
        }
    }
}
