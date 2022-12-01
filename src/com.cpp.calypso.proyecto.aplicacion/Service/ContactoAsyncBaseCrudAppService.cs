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
    public class ContactoAsyncBaseCrudAppService : AsyncBaseCrudAppService<Contacto, ContactoDto, PagedAndFilteredResultRequestDto>, IContactoAsyncBaseCrudAppService
    {
        public ContactoAsyncBaseCrudAppService(
            IBaseRepository<Contacto> repository
            ) : base(repository)
        {

        }

		public ContactoDto GetContacto(int Id)
		{
			var c = Repository.Get(Id);

			ContactoDto contacto = new ContactoDto()
			{
				Id = c.Id,
				calle_principal = c.calle_principal,
				numero = c.numero,
				calle_secundaria = c.calle_secundaria,
				telefono_convencional = c.telefono_convencional,
				celular = c.celular,
				correo_electronico = c.correo_electronico,
				ComunidadId = c.ComunidadId,
                comunidad_comunidad_laboral_id = c.comunidad_comunidad_laboral_id,
				detalle_comunidad = c.detalle_comunidad,
				ParroquiaId = c.ParroquiaId,
                parroquia_parroquia_laboral_id = c.parroquia_parroquia_laboral_id,
				detalle_parroquia = c.detalle_parroquia,
				vigente = c.vigente,
                referencia = c.referencia,
                detalle_comunidad_laboral = c.detalle_comunidad_laboral,
                detalle_parroquia_laboral = c.detalle_parroquia_laboral,
                codigo_postal = c.codigo_postal,
                Parroquia = c.Parroquia
			};


			return contacto;
		}
	}
}
