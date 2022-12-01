using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.dominio;

namespace com.cpp.calypso.proyecto.aplicacion
{
    public class InstitucionFinancieraAsyncBaseCrudAppService : AsyncBaseCrudAppService<InstitucionFinanciera,
        InstitucionFinancieraDto, PagedAndFilteredResultRequestDto>, IInstitucionFinancieraAsyncBaseCrudAppService
    {

        private readonly IBaseRepository<Oferta> _ofertaRepository;
        private readonly IBaseRepository<ContratoDocumentoBancario> _contratoDocumentoBancario;

        public InstitucionFinancieraAsyncBaseCrudAppService(
            IBaseRepository<InstitucionFinanciera> repository,
            IBaseRepository<ContratoDocumentoBancario> contratoDocumentoBancario
            ) : base(repository)
        {
            _contratoDocumentoBancario = contratoDocumentoBancario;
        }

        public List<InstitucionFinanciera> GetInstitucionesFinancieras()
        {
            var institucionQuery = Repository.GetAll().Where(e => e.vigente == true).ToList();
            return institucionQuery;
        }

        public bool Eliminar(int institucionId)
        {
            var documentos = _contratoDocumentoBancario.GetAll();

            var items = (from o in documentos
                         where o.vigente == true
                         where o.InstitucionFinancieraId == institucionId
                         select new ContratoDocumentoBancarioDto()
                         {
                             codigo = o.codigo

                         }).ToList();

            if (items.Count > 0)
            {
                return false;
            }
            else
            {
                var institucion = Repository.Get(institucionId);
                institucion.vigente = false;
                Repository.Update(institucion);
                return true;
            }
         }

        public List<InstitucionFinancieraDto> GetInstitucionesFinancierasDto()
        {
            var query = Repository.GetAll();
            var instituciondto = (from r in query
                                  where r.vigente == true
                                  select new InstitucionFinancieraDto()
                                  {
                                      Id = r.Id,
                                      concepto = r.concepto,
                                      direccion = r.direccion,
                                      nombre = r.nombre,
                                      persona_contrato = r.persona_contrato,
                                      telefono = r.telefono,
                                      vigente = r.vigente

                                  }).ToList();
            return instituciondto;

        }

    }
}
