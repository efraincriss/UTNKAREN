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
    [AutoMap(typeof(DetalleDistribucion))]
    [Serializable]
    public class DetalleDistribucionDto : EntityDto
    {
		[Obligado]
		[DisplayName("Distribucion")]
		public int DistribucionViandaId { get; set; }
	 
		[Obligado]
		[DisplayName("Solicitud")]
		public int SolicitudViandaId { get; set; }
		public virtual SolicitudViandaDto SolicitudVianda { get; set; }

		[Obligado]
		[DisplayName("Total Asignado")]
		public int total_asignado { get; set; }

		[Obligado]
		[DisplayName("Total Entregado")]
		public int total_entregado { get; set; }

	 	 
	}
}
