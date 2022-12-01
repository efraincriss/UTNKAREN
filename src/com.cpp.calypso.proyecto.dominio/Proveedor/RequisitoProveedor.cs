using Abp.Domain.Entities;
using com.cpp.calypso.comun.dominio;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace com.cpp.calypso.proyecto.dominio
{
    [Serializable]
    public class RequisitoProveedor : Entity
    {

		[Obligado]
		[DisplayName("Requisito")]
		public int RequisitosId { get; set; }
		public virtual Requisitos Requisitos { get; set; }

		[Obligado]
		[DisplayName("Proveedor")]
		public int ProveedorId { get; set; }
		public virtual dominio.Proveedor.Proveedor Proveedor { get; set; }

		[Obligado]
		[DisplayName("Cumple")]
		public RequisitoEstado cumple { get; set; }

		[Obligado]
		[StringLength(500)]
		[DisplayName("Observaciones")]
		public string observaciones { get; set; }


	}

    public enum RequisitoEstado
    {
        [Description("No Cumple")]
        NoCumple = 0,

        [Description("Cumple")]
        Cumple = 1,


    }
}
