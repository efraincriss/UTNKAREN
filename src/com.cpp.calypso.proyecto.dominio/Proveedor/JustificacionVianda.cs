using Abp.Domain.Entities.Auditing;
using com.cpp.calypso.comun.dominio;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace com.cpp.calypso.proyecto.dominio
{
    /// <summary>
    /// Justificacion de viandas 
    /// </summary>
    [Serializable]
    public class JustificacionVianda : AuditedEntity
    {
		[Obligado]
		[DisplayName("Solicitud")]
		public int SolicitudViandaId { get; set; }
		public virtual SolicitudVianda SolicitudVianda { get; set; }

		[Obligado]
		[DisplayName("Numero Viandas")]
		public int numero_viandas { get; set; }

		[Obligado]
		[StringLength(500)]
		[DisplayName("Justificacion")]
		public string justificacion { get; set; }
 

		[Obligado]
		[DisplayName("Estado")]
		public JustificacionViandaEstado estado { get; set; }
	}

    public enum JustificacionViandaEstado
    {
        [Description("Denegado")]
        Denegado = 0,

        [Description("Aprobado")]
        Aprobado = 1

    }
}
