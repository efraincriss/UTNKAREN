using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using com.cpp.calypso.comun.dominio;

using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace com.cpp.calypso.proyecto.dominio
{
    [Serializable]
    public class NovedadProveedor : AuditedEntity
    {
        public NovedadProveedor()
        {
          
        }

        [Obligado]
		[DisplayName("Proveedor")]
		public int ProveedorId { get; set; }

		public virtual dominio.Proveedor.Proveedor Proveedor { get; set; }

		[Obligado]
		[StringLength(500)]
		[DisplayName("Descripción")]
		public string descripcion { get; set; }

		[Obligado]
		[DisplayName("Resuelta")]
		public NovedadResuelto resuelta { get; set; }



		[DisplayName("Fecha Registro")]
		[DataType(DataType.Date)]
		[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
		public DateTime? fecha_registro { get; set; }

		[DisplayName("Fecha Solución")]
		[DataType(DataType.Date)]
		[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
		public DateTime? fecha_solucion { get; set; }

        [DisplayName("Documentación")]
        public int? documentacion_id { get; set; }
        public virtual Archivo documentacion { get; set; }
    }

    public enum NovedadResuelto
    {
        [Description("Pendiente")]
        Pendiente = 0,

        [Description("Resuelto")]
        Resuelto = 1,


    }
}
