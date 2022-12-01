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
    [AutoMap(typeof(GrupoPersonalRequisito))]
    [Serializable]
    public class GrupoPersonalRequisitoDto : EntityDto
    {
		[Obligado]
		[DisplayName("Requisito")]
		public int RequisitosId { get; set; }
		public virtual Requisitos Requisitos { get; set; }

		[Obligado]
		[DisplayName("Grupo Personal")]
		public int GrupoPersonalId { get; set; }
		public virtual GrupoPersonal GrupoPersonal { get; set; }

		[Obligado]
		[DisplayName("Vigente")]
		public bool vigente { get; set; } = true;
	}
}
