using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.dominio;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion.Dto
{
    [AutoMap(typeof(Parroquia))]
    [Serializable]
    public class ParroquiaDto : EntityDto
    {
		[Obligado]
		[DisplayName("Ciudad")]
		public int CiudadId { get; set; }

		public virtual Ciudad Ciudad { get; set; }

		[Obligado]
		[DisplayName("Codigo")]
		[LongitudMayorAttribute(5)]
		public string codigo { get; set; }

		[Obligado]
		[DisplayName("Nombre")]
		[LongitudMayorAttribute(200)]
		public string nombre { get; set; }

		[Obligado]
		public bool vigente { get; set; } = true;

        [CanBeNull]
        [DisplayName("Codigo Postal")]
        public string codigo_postal { get; set; }
    }
}
