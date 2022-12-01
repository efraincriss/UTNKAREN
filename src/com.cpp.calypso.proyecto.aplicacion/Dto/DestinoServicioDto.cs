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
    [AutoMap(typeof(DestinoServicio))]
    [Serializable]
    public class DestinoServicioDto : EntityDto
    {
		[Obligado]
		[DisplayName("Servicio")]
		public int ServicioId { get; set; }
		public virtual Servicio Servicio { get; set; }

		[Obligado]
		[DisplayName("Destino")]
		public int DestinoId { get; set; }
		public virtual Destino Destino { get; set; }

		[Obligado]
		[DisplayName("Vigente")]
		public bool vigente { get; set; } = true;
	}
}
