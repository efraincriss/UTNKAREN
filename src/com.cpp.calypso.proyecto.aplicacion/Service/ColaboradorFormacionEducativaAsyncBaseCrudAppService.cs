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
    public class ColaboradorFormacionEducativaAsyncBaseCrudAppService : AsyncBaseCrudAppService<ColaboradorFormacionEducativa, ColaboradorFormacionEducativaDto, PagedAndFilteredResultRequestDto>, IColaboradorFormacionEducativaAsyncBaseCrudAppService
    {
        public ColaboradorFormacionEducativaAsyncBaseCrudAppService(
            IBaseRepository<ColaboradorFormacionEducativa> repository
            ) : base(repository)
        {
        }

        public ColaboradorFormacionEducativaDto GetFormacion(int Id)
        {
            var d = Repository.GetAll().Where(c => c.ColaboradoresId == Id && c.vigente == true).FirstOrDefault();

            if (d != null) {
                ColaboradorFormacionEducativaDto f = new ColaboradorFormacionEducativaDto()
                {
                    Id = d.Id,
                    ColaboradoresId = d.ColaboradoresId,
                    formacion = d.formacion,
                    institucion_educativa = d.institucion_educativa,
                    catalogo_titulo_id = d.catalogo_titulo_id,
                    fecha_registro_senecyt = d.fecha_registro_senecyt,
                    Colaboradores = d.Colaboradores
                };

                return f;
            }

            return null;
        }
    }
}
