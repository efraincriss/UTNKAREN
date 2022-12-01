using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.dominio;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace com.cpp.calypso.proyecto.aplicacion.Dto
{
     [Serializable]
    public class DistribucionTransporteUpdateDto : EntityDto
    {
		[Obligado]
		[DisplayName("Proveedor")]
		public int ProveedorId { get; set; }
        public string proveedor_nombre { get; set; }
  
        [Obligado]
        [DisplayName("Tipo Comida")]
        public int tipo_comida_id { get; set; }
        public virtual string tipo_comida_nombre { get; set; }

        [DataType(DataType.Date)]
		[DisplayName("Fecha")]
		[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
		public DateTime fecha { get; set; }
         
		[DisplayName("Conductor Asignado")]
		public int? conductor_asignado_id { get; set; } 

  
    }
}
