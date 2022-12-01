using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.dominio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion.Dto
{
    [AutoMap(typeof(Comunidad))]
    [Serializable]
    public class ComunidadDto : EntityDto
    {
		[Obligado]
		[DisplayName("Parroquia")]
		public int ParroquiaId { get; set; }
		public virtual Parroquia Parroquia { get; set; }

		[Obligado]
		[DisplayName("Codigo")]
		[LongitudMayorAttribute(5)]
		public string codigo { get; set; }

		[Obligado]
		[DisplayName("Nombre")]
		[LongitudMayorAttribute(200)]
		public string nombre { get; set; }

		[Obligado]
		[DisplayName("Vigente")]
		public bool vigente { get; set; } = true;
	}
}
