using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Uow;
using AutoMapper;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.framework;
using com.cpp.calypso.proyecto.aplicacion.Acceso.Dto;
using com.cpp.calypso.proyecto.aplicacion.Acceso.Interface;
using com.cpp.calypso.proyecto.aplicacion.Dto;
using com.cpp.calypso.proyecto.dominio;
using com.cpp.calypso.proyecto.dominio.Acceso;
using com.cpp.calypso.seguridad.aplicacion;

namespace com.cpp.calypso.proyecto.aplicacion.Acceso.Service
{
    public class TarjetaAccesoAsyncBaseCrudAppService : AsyncBaseCrudAppService<TarjetaAcceso, TarjetaAccesoDto, PagedAndFilteredResultRequestDto>, ITarjetaAccesoAsyncBaseCrudAppService
    {
        private readonly IBaseRepository<Usuario> _usuarioRepository;
        private readonly IBaseRepository<Archivo> _archivoRepository;
        private readonly IBaseRepository<ColaboradorRequisito> _colaboradorRequisitosRepository;
        private readonly IBaseRepository<RequisitoColaborador> _grupoPersonalRepository;
        private readonly IBaseRepository<Colaboradores> _colaboradorRepository;


        public TarjetaAccesoAsyncBaseCrudAppService(
            IBaseRepository<TarjetaAcceso> repository,
            IBaseRepository<Usuario> usuarioRepository,
            IBaseRepository<Archivo> archivoRepository,
            IBaseRepository<ColaboradorRequisito> colaboradorRequisitosRepository,
            IBaseRepository<RequisitoColaborador> grupoPersonalRepository,
            IBaseRepository<Colaboradores> colaboradorRepository
            ) : base(repository)
        {
            _usuarioRepository = usuarioRepository;
            _archivoRepository = archivoRepository;
            _colaboradorRequisitosRepository = colaboradorRequisitosRepository;
            _grupoPersonalRepository = grupoPersonalRepository;
            _colaboradorRepository = colaboradorRepository;
        }

        public List<TarjetaAccesoDto> GetByColaborador(int colaboradorId)
        {
            var query = Repository.GetAll()
                .Where(o => o.ColaboradorId == colaboradorId)
                .OrderByDescending(o => o.fecha_emision)
                ;

            var entities = query.ToList();

            var dtos = Mapper.Map<List<TarjetaAcceso>, List<TarjetaAccesoDto>>(entities);//estab entities
            foreach (var dto in dtos)
            {
                if (dto.CreatorUserId.HasValue)
                {
                    var id = (int)dto.CreatorUserId.Value;
                    dto.usuario_nombres = _usuarioRepository.Get(id).NombresCompletos;
                }

            }
            return dtos;
        }

        [UnitOfWork]
        public void SwitchEntregado(int tarjetaId)
        {
            var entity = Repository.Get(tarjetaId);
            
            entity.entregada = !entity.entregada;
        }

        public void AnularTarjeta(ActualizarTarjetaDto input, int archivoId = 0)
        {
            var entity = Repository.Get(input.Id);
            entity.observaciones = input.observaciones;
            if (archivoId > 0)
            {
                entity.DocumentoRespaldoId = archivoId;
            }
            entity.fecha_vencimiento = input.fecha_vencimiento;
            entity.estado = TarjetaEstado.Inactivo;
            Repository.Update(entity);
        }

        public void SubirPdf(int tarjetaId, ArchivoDto archivo)
        {
            var consulta = Repository.Get(tarjetaId);
            if (consulta.DocumentoRespaldoId > 0)
            {
                var archivoId = consulta.DocumentoRespaldoId;
                var file = consulta.DocumentoRespaldo;

                consulta.DocumentoRespaldoId = null;
                Repository.Update(consulta);
                _archivoRepository.Delete(file);

            }
            var entity = _archivoRepository.Insert(Mapper.Map<ArchivoDto, Archivo>(archivo));

            consulta.DocumentoRespaldo = entity;
            Repository.Update(consulta);
        }

        public bool PuedeCrear(int colaboradorId)
        {
            var count = Repository.GetAll()
                .Where(o => o.ColaboradorId == colaboradorId)
                .Count(o => o.estado == TarjetaEstado.Activo);

            if (count > 0)
            {
                return false;
            }

            return true;
        }

        public int obtenersecuencialtarjetas(int id)
        {
            return Repository.GetAll().Where(c=>c.ColaboradorId==id).ToList().Count+1;
        }

        public string obtenersolicitudpamactiva()
        {
            var solicitud = Repository.GetAll().Where(c=>c.estado==TarjetaEstado.Activo).FirstOrDefault();
            if (solicitud != null && solicitud.Id > 0)
            {
                return solicitud.solicitud_pam;
            }
            else {
                return "";
            }


        }
    }
}
