using Abp.Domain.Entities;
using com.cpp.calypso.comun.dominio;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace com.cpp.calypso.proyecto.dominio
{
    [Serializable]
    public class MenuProveedor : Entity
    {
		[Obligado]
		[DisplayName("Proveedor")]
		public int ProveedorId { get; set; }
		public virtual dominio.Proveedor.Proveedor Proveedor { get; set; }

		[DataType(DataType.Date)]
		[DisplayName("Fecha Inicial")]
		[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
		public DateTime fecha_inicial { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Fecha Inicial")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime fecha_final { get; set; }

      

		[Obligado]
		[DisplayName("Aprobado")]
		public MenuEstadoAprobacion aprobado { get; set; }

	

		[DataType(DataType.Date)]
		[DisplayName("Fecha Aprobación")]
		[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
		public DateTime fecha_aprobacion { get; set; }

		[Obligado]
		[DisplayName("Descripción")]
		[LongitudMayorAttribute(200)]
		public string descripcion { get; set; }


        [DisplayName("Documentación")]
        public int? documentacion_id { get; set; }
        public virtual Archivo documentacion { get; set; }
    }

    public enum MenuEstadoAprobacion
    {
        [Description("Pendiente de Aprobación")]
        PendienteAprobacion = 0,

        [Description("Aprobado")]
        Aprobado = 1,
  
    }
}
