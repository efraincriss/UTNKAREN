using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.aplicacion.Dto;
using com.cpp.calypso.proyecto.aplicacion.Interfaces;
using com.cpp.calypso.proyecto.dominio;
using com.cpp.calypso.proyecto.dominio.Constantes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion.Service
{
    public class ColaboradoresVisitaAsyncBaseCrudAppService : AsyncBaseCrudAppService<ColaboradoresVisita, ColaboradoresVisitaDto, PagedAndFilteredResultRequestDto>, IColaboradoresVisitaAsyncBaseCrudAppService
    {
        private readonly ICatalogoAsyncBaseCrudAppService _catalogoRepository;
        public readonly IBaseRepository<Colaboradores> _colaboradoresRepository;
        public readonly IBaseRepository<ColaboradorRequisito> _colRequisitoRepository;
        public ColaboradoresVisitaAsyncBaseCrudAppService(
            ICatalogoAsyncBaseCrudAppService catalogoRepository,
            IBaseRepository<ColaboradoresVisita> repository,
            IBaseRepository<Colaboradores> colaboradoresRepository,
            IBaseRepository<ColaboradorRequisito> colRequisitoRepository
            ) : base(repository)
        {
            _catalogoRepository = catalogoRepository;
            _colaboradoresRepository = colaboradoresRepository;
            _colRequisitoRepository = colRequisitoRepository;
        }

        public ColaboradoresVisitaDto GetVisitasPorColaborador(int id)
        {
            var d = Repository.GetAll().Where(c => c.vigente == true && c.ColaboradoresId == id && c.estado == RRHHCodigos.ESTADO_ACTIVO).FirstOrDefault();

            if (d != null)
            {
                ColaboradoresVisitaDto visita = new ColaboradoresVisitaDto()
                {
                    Id = d.Id,
                    ColaboradoresId = d.ColaboradoresId,
                    Colaboradores = d.Colaboradores,
                    CreationTime = d.CreationTime,
                    CreatorUserId = d.CreatorUserId,
                    fecha_desde = d.fecha_desde,
                    fecha_hasta = d.fecha_hasta,
                    colaborador_responsable_id = d.colaborador_responsable_id,
                    ColaboradoresResponsable = d.ColaboradoresResponsable,
                    estado = d.estado,
                    motivo = d.motivo,
                    empresa = d.empresa
                };
                return visita;
            }

            return null;
        }

        public List<ColaboradoresVisitaDto> GetVisitaUsuariosExternos()
        {
            var e = 1;
            var query = Repository.GetAll().Where(c => c.vigente == true && c.estado == RRHHCodigos.ESTADO_ACTIVO);

            if (query != null)
            {
                var visitas = (from d in query
                               select new ColaboradoresVisitaDto
                               {
                                   Id = d.Id,
                                   ColaboradoresId = d.ColaboradoresId,
                                   Colaboradores = d.Colaboradores,
                                   CreationTime = d.CreationTime,
                                   CreatorUserId = d.CreatorUserId,
                                   fecha_desde = d.fecha_desde,
                                   fecha_hasta = d.fecha_hasta,
                                   colaborador_responsable_id = d.colaborador_responsable_id,
                                   ColaboradoresResponsable = d.ColaboradoresResponsable,
                                   estado = d.estado,
                                   motivo = d.motivo,
                                   nombre_identificacion = d.Colaboradores.TipoIdentificacion.nombre,
                                   numero_identificacion = d.Colaboradores.numero_identificacion,
                                   nombres = d.Colaboradores.nombres,
                                   nombres_apellidos = d.Colaboradores.nombres_apellidos,
                                   estado_colaborador = d.Colaboradores.estado,

                                   
                               }).ToList();

                foreach (var i in visitas)
                {
                    i.nro = e++;

                    i.apellidos_nombres = i.Colaboradores.primer_apellido + ' ' + i.Colaboradores.segundo_apellido;
                    i.nombreTipo = i.Colaboradores.es_visita ? "VISITA" : "TERCERO";
                }
                return visitas;
            }

            return null;
        }

        public bool SwitchEstadoColaboradorExterno(int Id)
        {
            var externo = _colaboradoresRepository.GetAll().Where(c => c.vigente).Where(c => c.Id == Id).FirstOrDefault();
            if (externo.estado == "ACTIVO")
            {
                externo.estado = "INACTIVO";
            }
            else
            {
                externo.estado = "ACTIVO";
            }
            var update = _colaboradoresRepository.Update(externo);
            return update.Id > 0 ? true : false;

        }

        public string DeleteColaboradorExterno(int Id)
        {

            var update = _colaboradoresRepository.Get(Id);
            var visitas = Repository.GetAll().Where(c => c.ColaboradoresId == update.Id)
                                             .Where(c => c.vigente)
                                             .ToList();
            var requisitos = _colRequisitoRepository.GetAll().Where(c => c.ColaboradoresId == update.Id)
                                                    .Where(c => c.vigente)
                                                    .ToList();
            if (visitas.Count == 0 && requisitos.Count == 0)
            {
                update.vigente = false;
                update.IsDeleted = true;
                _colaboradoresRepository.Update(update);
                return "OK";

            }
            if (visitas.Count > 0)
            {
                return "VISITAS";
            }
            if (requisitos.Count > 0)
            {
                return "REQUISITOS";
            }
            return "ERROR";

        }
    }
}
