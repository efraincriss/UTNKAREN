using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.aplicacion.Interfaces;
using com.cpp.calypso.proyecto.dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion.Service
{
    public class CobroAsyncBaseCrudAppService : AsyncBaseCrudAppService<Cobro, CobroDto, PagedAndFilteredResultRequestDto>, ICobroAsyncBaseCrudAppService
    {
        public CobroAsyncBaseCrudAppService(
            IBaseRepository<Cobro> repository
        ) : base(repository)
        {
        }

        public bool Eliminar(int Id)
        {
            var r = Repository.Get(Id);
            r.vigente = false;
            var resultado = Repository.Update(r);
            if (r.Id > 0 && r != null)
            {
                return true;
            }
            else {
                return false;
            }
        }

        public CobroDto getdetalle(int Id)
        {
            throw new NotImplementedException();
        }

        public List<Cobro> Listar()
        {
            var cobros = Repository.GetAll().Where(c => c.vigente == true).ToList();
            return cobros;
        }
    }
}