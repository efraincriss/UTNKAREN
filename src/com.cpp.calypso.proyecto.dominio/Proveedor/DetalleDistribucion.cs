using Abp.Domain.Entities;
using com.cpp.calypso.comun.dominio;
using System;
using System.ComponentModel;

namespace com.cpp.calypso.proyecto.dominio
{
    [Serializable]
    public class DetalleDistribucion : Entity
    {
		[Obligado]
		[DisplayName("Distribucion")]
		public int DistribucionViandaId { get; set; }
		public virtual DistribucionVianda DistribucionVianda { get; set; }

		[Obligado]
		[DisplayName("Solicitud")]
		public int SolicitudViandaId { get; set; }
		public virtual SolicitudVianda SolicitudVianda { get; set; }

		[Obligado]
		[DisplayName("Total Asignado")]
		public int total_asignado { get; set; }

		[Obligado]
		[DisplayName("Total Entregado")]
		public int total_entregado { get; set; }
 
	}
}
