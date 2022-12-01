using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.dominio;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace com.cpp.calypso.proyecto.aplicacion.Dto
{
    [AutoMap(typeof(NovedadProveedor))]
    [Serializable]
    public class NovedadProveedorDto : EntityDto
    {
 
        [Obligado]
        [DisplayName("Proveedor")]
        public int ProveedorId { get; set; }
         

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

        public int? documentacion_id { get; set; }

        /// <summary>
        /// Asociar un archivo. 
        /// </summary>
        public virtual ArchivoDto documentacion_subida { get; set; }
    }
}
