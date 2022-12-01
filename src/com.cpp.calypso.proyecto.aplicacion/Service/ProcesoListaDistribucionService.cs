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
    public class ProcesoListaDistribucionAsyncBaseCrudAppService : AsyncBaseCrudAppService<ProcesoListaDistribucion, ProcesoListaDistribucionDto, PagedAndFilteredResultRequestDto>, IProcesoListaDistribucionAsyncBaseCrudAppService
    {
        private readonly IBaseRepository<CorreoLista> _repositoryCorreoLista;
        private readonly IBaseRepository<ListaDistribucion> _repositoryListaDistribucion;

        public ProcesoListaDistribucionAsyncBaseCrudAppService(
            IBaseRepository<ProcesoListaDistribucion> repository,
            IBaseRepository<CorreoLista> repositoryCorreoLista,
            IBaseRepository<ListaDistribucion> repositoryListaDistribucion
        ) : base(repository)
        {
            _repositoryCorreoLista = repositoryCorreoLista;
            _repositoryListaDistribucion = repositoryListaDistribucion;
        }

        public List<ProcesoListaDistribucionDto> listar()
        {
            var query = Repository.GetAllIncluding(o => o.ListaDistribucion, o => o.ProcesoNotificacion).Where(o => o.vigente == true);

            var procesos = (from p in query
                select new ProcesoListaDistribucionDto()
                {
                    Id = p.Id,
                    vigente = p.vigente,
                    ListaDistribucionId = p.ListaDistribucionId,
                    ProcesoNotificacionId = p.ProcesoNotificacionId,
                    nombre_lista = p.ListaDistribucion.nombre,
                    nombre_proceso = p.ProcesoNotificacion.nombre
                }).ToList();
            return procesos;
        }

        public int Crear(ProcesoListaDistribucionDto proceso)
        {
            var count = Repository.GetAll()
                .Where(o => o.vigente == true)
                .Where(o => o.ListaDistribucionId == proceso.ListaDistribucionId)
                .Where(o => o.ProcesoNotificacionId == proceso.ProcesoNotificacionId).Count();

            if (count > 0)
            {
                return 0;
            }
            else
            {
                var id = Repository.InsertAndGetId(MapToEntity(proceso));
                return id;
                
            }

        }

        public List<CorreoListaDto> CorreosDeProceso(int procesoId)
        {
            var query = _repositoryCorreoLista.GetAllIncluding(o => o.ListaDistribucion)
                .Where(o => o.vigente == true);

            var queryProceso = Repository.GetAll().Where(o => o.vigente == true)
                .Where(o => o.ProcesoNotificacionId == procesoId);

            var correos = (from c in query
                where (from p in queryProceso
                    select p.ListaDistribucionId).Contains(c.ListaDistribucionId)
                select new CorreoListaDto()
                {
                    Id = c.Id,
                    correo = c.correo,
                    externo = c.externo,
                    nombres = c.nombres,
                    nombre_lista = c.ListaDistribucion.nombre
                }).ToList();

            return correos;
        }
    }
}
