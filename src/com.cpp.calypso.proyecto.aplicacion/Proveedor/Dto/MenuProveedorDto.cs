using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using com.cpp.calypso.comun.dominio;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace com.cpp.calypso.proyecto.aplicacion.Dto
{
    [AutoMap(typeof(dominio.MenuProveedor))]
    [Serializable]
    public class MenuProveedorDto : EntityDto
    {
		[Obligado]
		[DisplayName("Proveedor")]
		public int ProveedorId { get; set; }

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
		public int aprobado { get; set; }

	

		[DataType(DataType.Date)]
		[DisplayName("Fecha Aprobación")]
		[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
		public DateTime fecha_aprobacion { get; set; }

		[Obligado]
		[DisplayName("Descripción")]
		[LongitudMayorAttribute(200)]
		public string descripcion { get; set; }

        public int? documentacion_id { get; set; }
       
    }
}
