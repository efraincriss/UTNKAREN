using AutoMapper;
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
    public class RequisitosAsyncBaseCrudAppService : AsyncBaseCrudAppService<Requisitos, RequisitosDto, PagedAndFilteredResultRequestDto>, IRequisitosAsyncBaseCrudAppService
    {

        private readonly ICatalogoAsyncBaseCrudAppService _catalogoRepository;
        private readonly IBaseRepository<RequisitoColaborador> _requisitoColaboradorRepository;
        private readonly IBaseRepository<ColaboradorRequisito> _colaboradorRequisitoRepository;

        public RequisitosAsyncBaseCrudAppService(
            IBaseRepository<Requisitos> repository,
            ICatalogoAsyncBaseCrudAppService catalogoRepository,
            IBaseRepository<RequisitoColaborador> requisitoColaboradorRepository,
            IBaseRepository<ColaboradorRequisito> colaboradorRequisitoRepository
            ) : base(repository)
        {
            _catalogoRepository = catalogoRepository;
            _requisitoColaboradorRepository = requisitoColaboradorRepository;
            _colaboradorRequisitoRepository = colaboradorRequisitoRepository;
        }

        public List<RequisitosDto> GetList()
        {
            var query = Repository.GetAll().Where(c=> c.vigente == true && c.activo == true).OrderBy(c => c.nombre);

            var requisitos = (from d in query
                              select new RequisitosDto
                              {
                                  Id = d.Id,
                                  codigo = d.codigo,
                                  nombre = d.nombre,
                                  requisitoId = d.requisitoId,
                                  descripcion = d.descripcion,
                                  responsableId = d.responsableId,
                                  caducidad = d.caducidad,
                                  tiempo_vigencia = d.tiempo_vigencia,
                                  dias_inicio_alerta = d.dias_inicio_alerta,
                                  nombre_estado = d.Responsable.nombre,
                                  vigente = d.vigente,
                                  nombre_requisito = d.TipoRequisito.codigo,
                                  nombre_caducidad = d.TipoRequisito.nombre,
                                  nombre_activo = d.activo == true ? "SI" : "NO",
                              }).ToList();

			return requisitos;
        }

        public RequisitosDto GetRequisito(int Id)
        {
            var c = Repository.Get(Id);

            RequisitosDto requisito = new RequisitosDto()
            {
                Id = c.Id,
                codigo = c.codigo,
                nombre = c.nombre,
                requisitoId = c.requisitoId,
                descripcion = c.descripcion,
                responsableId = c.responsableId,
                caducidad = c.caducidad,
                tiempo_vigencia = c.tiempo_vigencia,
                dias_inicio_alerta = c.dias_inicio_alerta,
                vigente = c.vigente
            };


            return requisito;
        }

        public string UniqueCodigo(string codigo)
        {
            var c = Repository.GetAll().Where(d => d.codigo == codigo && d.vigente == true).FirstOrDefault();
            if (c != null)
            {
                return "SI";
            }
            else
            {
                return "NO";
            }
        }

        public string ActualizaActivo(int id, bool activo)
        {
            if(activo == true)
            {
                //Actualiza ColaboradorRequisito
                var listaColReq = _colaboradorRequisitoRepository.GetAll().Where(x => x.RequisitosId == id && x.activo == false && x.IsDeleted == false);
                var requisitosColReq = Mapper.Map<IQueryable<ColaboradorRequisito>, List<ColaboradorRequisito>>(listaColReq);
                foreach (var r in requisitosColReq) {
                    r.activo = true;
                    _colaboradorRequisitoRepository.Update(r);
                }

                //ActualizaRequisitoColaborador
                var listaReq = _requisitoColaboradorRepository.GetAll().Where(x => x.RequisitosId == id && x.activo == false && x.IsDeleted == false);
                var requisitosReq = Mapper.Map<IQueryable<RequisitoColaborador>, List<RequisitoColaborador>>(listaReq);
                foreach (var r in requisitosReq)
                {
                    r.activo = true;
                    _requisitoColaboradorRepository.Update(r);
                }
            }
            else
            {
                //Actualiza ColaboradorRequisito
                var listaColReq = _colaboradorRequisitoRepository.GetAll().Where(x => x.RequisitosId == id && x.activo == true && x.IsDeleted == false);
                var requisitosColReq = Mapper.Map<IQueryable<ColaboradorRequisito>, List<ColaboradorRequisito>>(listaColReq);
                foreach (var r in requisitosColReq)
                {
                    r.activo = false;
                    _colaboradorRequisitoRepository.Update(r);
                }

                //ActualizaRequisitoColaborador
                var listaReq = _requisitoColaboradorRepository.GetAll().Where(x => x.RequisitosId == id && x.activo == true && x.IsDeleted == false);
                var requisitosReq = Mapper.Map<IQueryable<RequisitoColaborador>, List<RequisitoColaborador>>(listaReq);
                foreach (var r in requisitosReq)
                {
                    r.activo = false;
                    _requisitoColaboradorRepository.Update(r);
                }
            }

            return "OK";
        }

        public string nextcode()
        {
            int sec_number = 1;
            var list_code = Repository.GetAll().Where(c => !c.IsDeleted).Select(c => c.codigo).ToList();
            if (list_code.Count > 0)
            {
                List<int> numeracion = (from l in list_code
                                        where l.Length == 8
                                        select Convert.ToInt32(l.Substring(3, 5))).ToList();

                if (numeracion.Count > 0)
                {
                    sec_number = numeracion.Max() + 1;
                }


            }

            return "REQ" + String.Format("{0:00000}", sec_number);
        }

    }
}
