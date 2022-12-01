using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.aplicacion.CertificacionIngenieria.Dto;
using com.cpp.calypso.proyecto.aplicacion.CertificacionIngenieria.Interface;
using com.cpp.calypso.proyecto.aplicacion.CertificacionIngenieria.Models;
using com.cpp.calypso.proyecto.dominio;
using com.cpp.calypso.proyecto.dominio.CertificacionIngenieria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion.CertificacionIngenieria.Service
{
    public class DetalleDirectoE500AsyncBaseCrudAppService : comun.aplicacion.AsyncBaseCrudAppService<DetalleDirectoE500, DetalleDirectoE500Dto, PagedAndFilteredResultRequestDto>, IDetalleDirectoE500AsyncBaseCrudAppService
    {

        private readonly IBaseRepository<Proyecto> _proyectoRepository;
        private readonly IBaseRepository<Catalogo> _catalogoRepository;
        private readonly IBaseRepository<DetallesDirectosIngenieria> _directosRepository;
        private readonly IBaseRepository<ParametroSistema> _parametroRepository;


        public DetalleDirectoE500AsyncBaseCrudAppService(
            IBaseRepository<DetalleDirectoE500> repository,

            IBaseRepository<Proyecto> proyectoRepository,
            IBaseRepository<Catalogo> catalogoRepository,
            IBaseRepository<DetallesDirectosIngenieria> directosRepository,
            IBaseRepository<ParametroSistema> parametroRepository

        ) : base(repository)
        {
            _proyectoRepository = proyectoRepository;
            _catalogoRepository = catalogoRepository;
            _directosRepository = directosRepository;
            _parametroRepository = parametroRepository;

        }

        public string DistribuirHorasDirectasaProyecto(int Id, List<E500Distribucion> temporales)
        {
            try
            {
                      /*Lista Nuevo Directos a Registrar*/
            List<DetallesDirectosIngenieria> entidadesaIngresar = new List<DetallesDirectosIngenieria>();

            var Directo = _directosRepository.GetAll().Where(c => c.Id == Id).FirstOrDefault();
            if (Directo != null)
            {
                foreach (var temp in temporales)
                {
                    var proyecto = _proyectoRepository.GetAll().Where(c => c.Id == temp.ProyectoADistribuirId).FirstOrDefault();

                    if (proyecto != null)
                    {
                        var nuevo = new DetallesDirectosIngenieria()
                        {
                            Id = 0,
                            CargaAutomatica = false,
                            CertificadoId = null,
                            CodigoProyecto = proyecto.codigo,
                            ColaboradorId = Directo.ColaboradorId,
                            EsDirecto = Directo.EsDirecto,
                            EspecialidadId = Directo.EspecialidadId,
                            EstadoRegistroId = Directo.EstadoRegistroId,
                            EtapaId = Directo.EtapaId,
                            FechaCarga = Directo.FechaCarga,
                            FechaTrabajo = Directo.FechaTrabajo,
                            JustificacionActualizacion = Directo.JustificacionActualizacion,
                            Identificacion = Directo.Identificacion,
                            LocacionId = Directo.LocacionId,
                            ModalidadId = Directo.ModalidadId,
                            NombreEjecutante = Directo.NombreEjecutante,
                            NumeroHoras = temp.Horas,
                            ProyectoId = temp.ProyectoADistribuirId,
                            Observaciones = Directo.Observaciones,
                            TipoRegistroId = Directo.TipoRegistroId
                        };
                        entidadesaIngresar.Add(nuevo);
                    }
                }

            }
            foreach (var entity in entidadesaIngresar)
            {
                _directosRepository.Insert(entity);
            }

            _directosRepository.Delete(Directo.Id);
                return "OK";
            }
            catch (Exception e)
            {
                return e.Message;
            }

        }

        public string EnviaraDirectosaOtroProyecto(int ProyectoDestinoId, int[] Id)
        {
            var ProyectoDestino = _proyectoRepository.GetAll().Where(c => c.Id == ProyectoDestinoId).FirstOrDefault();

            foreach (var DirectoId in Id)
            {
                var entityDirecto = _directosRepository.Get(DirectoId);
                if (entityDirecto != null) {
                entityDirecto.ProyectoId = ProyectoDestino.Id;
                entityDirecto.CodigoProyecto = ProyectoDestino.codigo;
                _directosRepository.Update(entityDirecto);
                }
            }
            return "OK";
        }

        public string EnviaraE500(int[] Id)
        {
            foreach (var directoId in Id)
            {
                var directo = _directosRepository.GetAllIncluding(c => c.Proyecto.Contrato.Cliente).Where(c=>c.Id==directoId).FirstOrDefault();

                var e500 = new DetalleDirectoE500()
                {
                    Id = 0,
                    CertificadoId = null,
                    ClienteId = directo.Proyecto.Contrato.ClienteId,
                    ColaboradorId = directo.ColaboradorId,
                    EspecialidadId = directo.EspecialidadId,
                    EstadoRegistroId = directo.EstadoRegistroId,
                    EtapaId = directo.EtapaId,
                    FechaCarga = directo.FechaCarga,
                    FechaTrabajo = directo.FechaTrabajo,
                    Identificacion = directo.Identificacion,
                    LocacionId = directo.LocacionId,
                    ModalidadId = directo.ModalidadId,
                    NombreEjecutante = directo.NombreEjecutante,
                    NumeroHoras = directo.NumeroHoras,
                    Observaciones = directo.Observaciones,
                    TipoRegistroId = directo.TipoRegistroId,

                };
                Repository.Insert(e500);
                _directosRepository.Delete(directo.Id);
            }

            return "OK";
        }

        public List<DetalleDirectoE500Dto> ObtenerDetallesDirectosE500()
        {
            var queryE500 = Repository.GetAllIncluding(c => c.TipoRegistro, c => c.Colaborador, c => c.Cliente, c => c.Locacion, c => c.Modalidad, c => c.EstadoRegistro)

                                                   .Where(c => !c.CertificadoId.HasValue)//No este Certificado;
                                                    .ToList();
            var precisionDecimales = 2;
            var E500Lista = (from q in queryE500
                             select new DetalleDirectoE500Dto()
                             {
                                 Id = q.Id,
                                 CertificadoId = q.CertificadoId,
                                 nombreCliente = q.Cliente.razon_social,
                                 ColaboradorId = q.ColaboradorId,
                                 nombreColaborador = q.ColaboradorId > 0 ? q.Colaborador.nombres_apellidos : "",
                                 EspecialidadId = q.EspecialidadId,
                                 EstadoRegistroId = q.EstadoRegistroId,
                                 EtapaId = q.EtapaId,
                                 FechaTrabajo = q.FechaTrabajo,
                                 formatFechaTrabajo = q.FechaTrabajo.ToShortDateString(),
                                 NumeroHoras = Decimal.Round(q.NumeroHoras,precisionDecimales),
                                 NombreEjecutante = q.NombreEjecutante,
                                 ModalidadId = q.ModalidadId,
                                 Observaciones = q.Observaciones,
                                 Identificacion = q.Identificacion,
                                 LocacionId = q.LocacionId,
                                 nombreLocacion = q.LocacionId.HasValue ? q.Locacion.nombre : "",
                                 nombreModalidad = q.ModalidadId.HasValue ? q.Modalidad.nombre : "",
                                 FechaCarga = q.FechaCarga,
                                 TipoRegistroId = q.TipoRegistroId,
                                 formatFechaCarga = q.FechaCarga.ToShortDateString(),
                                 nombreEstado = q.EstadoRegistroId > 0 ? q.EstadoRegistro.nombre : "",
                             }).ToList();
            return E500Lista;

        }

        public List<DetallesDirectosIngenieriaDto> ObtenerDetallesDirectosProyecto(int ProyectoId)
        {
            var queryDirectos = _directosRepository.GetAllIncluding(c => c.TipoRegistro, c => c.Colaborador, c => c.Proyecto, c => c.Locacion, c => c.Modalidad, c => c.EstadoRegistro)
                                                   .Where(c => !c.CertificadoId.HasValue)//No este Certificado;
                                                   .Where(c => c.ProyectoId == ProyectoId)
                                                   .ToList();
            var precisionDecimales = 2;
            var directoSinCertificar = (from q in queryDirectos
                                        select new DetallesDirectosIngenieriaDto()
                                        {
                                            Id = q.Id,
                                            CertificadoId = q.CertificadoId,
                                            CodigoProyecto = q.CodigoProyecto,
                                            ColaboradorId = q.ColaboradorId,
                                            nombreColaborador = q.ColaboradorId > 0 ? q.Colaborador.nombres_apellidos : "",
                                            EsDirecto = q.EsDirecto,
                                            EspecialidadId = q.EspecialidadId,
                                            EstadoRegistroId = q.EstadoRegistroId,
                                            EtapaId = q.EtapaId,
                                            FechaTrabajo = q.FechaTrabajo,
                                            formatFechaTrabajo = q.FechaTrabajo.ToShortDateString(),
                                            nombreProyecto = q.ProyectoId > 0 ? q.Proyecto.codigo : "",
                                            NumeroHoras = Decimal.Round(q.NumeroHoras,precisionDecimales),
                                            NombreEjecutante = q.NombreEjecutante,
                                            ModalidadId = q.ModalidadId,
                                            Observaciones = q.Observaciones,
                                            ProyectoId = q.ProyectoId,
                                            Identificacion = q.Identificacion,
                                            LocacionId = q.LocacionId,
                                            nombreLocacion = q.LocacionId.HasValue ? q.Locacion.nombre : "",
                                            nombreModalidad = q.ModalidadId.HasValue ? q.Modalidad.nombre : "",
                                            CargaAutomatica = q.CargaAutomatica,
                                            FechaCarga = q.FechaCarga,
                                            JustificacionActualizacion = q.JustificacionActualizacion,
                                            TipoRegistroId = q.TipoRegistroId,
                                            esCargaAutomatica = q.CargaAutomatica ? "AUTOMATICA" : "MANUAL",
                                            formatFechaCarga = q.FechaCarga.ToShortDateString(),
                                            nombreEstado = q.EstadoRegistroId > 0 ? q.EstadoRegistro.nombre : "",
                                            contratoId = q.Proyecto.contratoId
                                        }).ToList();

            return directoSinCertificar;
        }

        public List<Proyecto> ObtenerProyectos()
        {
            return _proyectoRepository.GetAll().Where(c => c.vigente).ToList();
        }
    }
}
