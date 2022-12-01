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
    public class ColaboradorMovilizacionAsyncBaseCrudAppService : AsyncBaseCrudAppService<ColaboradorMovilizacion, ColaboradorMovilizacionDto, PagedAndFilteredResultRequestDto>, IColaboradorMovilizacionAsyncBaseCrudAppService
    {
        public ColaboradorMovilizacionAsyncBaseCrudAppService(
            IBaseRepository<ColaboradorMovilizacion> repository
            ) : base(repository)
        {
        }

        public ColaboradorMovilizacionDto GetMovilizacion(int Id)
        {
            var m = Repository.GetAll().Where(c => c.ColaboradorServicioId == Id).FirstOrDefault();

            if (m == null)
            {
                return null;
            }
            else {
                ColaboradorMovilizacionDto movilizacion = new ColaboradorMovilizacionDto()

                {
                    Id = m.Id,
                    ColaboradorServicioId = m.ColaboradorServicioId,
                    ComunidadId = m.ComunidadId,
                    ParroquiaId = m.ParroquiaId,
                    catalogo_tipo_movilizacion_id = m.catalogo_tipo_movilizacion_id
                };

                return movilizacion;
            }


            

           
        }
    }
}
