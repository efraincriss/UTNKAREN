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
    [AutoMap(typeof(DestinoAlimentacion))]
    [Serializable]
    public class DestinoAlimentacionDto : EntityDto
    {
		[Obligado]
		[DisplayName("Destino")]
		public int DestinoId { get; set; }
		public virtual Destino Destino { get; set; }

		[Obligado]
		[DisplayName("Tipo Comida")]
		public int TipoOpcionComidaId { get; set; }
		public virtual TipoOpcionComida TipoComida { get; set; }

		[Obligado]
		[DisplayName("Vigente")]
		public bool vigente { get; set; } = true;
	}
}
