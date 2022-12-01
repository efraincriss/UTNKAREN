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
	public class ColaboradorDiscapacidadAsyncBaseCrudAppService : AsyncBaseCrudAppService<ColaboradorDiscapacidad, ColaboradorDiscapacidadDto, PagedAndFilteredResultRequestDto>, IColaboradorDiscapacidadAsyncBaseCrudAppService
	{
		public ColaboradorDiscapacidadAsyncBaseCrudAppService(
			IBaseRepository<ColaboradorDiscapacidad> repository
			) : base(repository)
		{
		}

        public ColaboradorDiscapacidadDto GetDiscapacidadColaborador(int Id)
        {
            var d = Repository.GetAll().Where(c => c.ColaboradoresId == Id && c.vigente == true).FirstOrDefault();

            if (d != null) {
                ColaboradorDiscapacidadDto discapacidad = new ColaboradorDiscapacidadDto()
                {
                    Id = d.Id,
                    ColaboradoresId = d.ColaboradoresId,
                    catalogo_porcentaje_id = d.catalogo_porcentaje_id,
                    catalogo_tipo_discapacidad_id = d.catalogo_tipo_discapacidad_id,
                    Colaboradores = d.Colaboradores
                };

                return discapacidad;
            }
            return null;
            
        }

        public ColaboradorDiscapacidadDto GetDiscapacidadCargaSocial(int Id)
        {
            var d = Repository.GetAll().Where(c => c.ColaboradorCargaSocialId == Id && c.vigente == true).FirstOrDefault();

            if (d != null) {
                ColaboradorDiscapacidadDto discapacidad = new ColaboradorDiscapacidadDto()
                {
                    Id = d.Id,
                    ColaboradoresId = d.ColaboradoresId,
                    catalogo_porcentaje_id = d.catalogo_porcentaje_id,
                    catalogo_tipo_discapacidad_id = d.catalogo_tipo_discapacidad_id,
                    ColaboradorCargaSocial = d.ColaboradorCargaSocial
                };

                return discapacidad;
            }

            return null;
        }
    }
}
