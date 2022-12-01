using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.dominio;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace com.cpp.calypso.proyecto.aplicacion.Dto
{
    [AutoMap(typeof(DistribucionVianda))]
    [Serializable]
    public class DistribucionViandaDto : EntityDto
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

		[Obligado]
		[DisplayName("Total Pedido")]
		public int total_pedido { get; set; }

		[Obligado]
		[DisplayName("Total Entregado Transporte")]
		public int total_entregado_transporte { get; set; }

		[Obligado]
		[DisplayName("Total Justificado")]
		public int total_justificado { get; set; }

		[Obligado]
		[DisplayName("Total Liquidado")]
		public int total_liquidado { get; set; }

		[Obligado]
		[DisplayName("Liquidado")]
		public int liquidado { get; set; }

		[DisplayName("Conductor Asignado")]
		public int? conductor_asignado_id { get; set; }
        public string conductor_asignado_nombre { get; set; }

 

        [DataType(DataType.Time)]
		[DisplayName("Hora Asignación Transporte")]
		[DisplayFormat(DataFormatString = "{0:HH-mm}", ApplyFormatInEditMode = true)]
		public DateTime? hora_asignacion_transporte { get; set; }


        [Obligado]
        [DisplayName("Estado")]
        public DistribucionViandaEstado estado { get; set; }
        public string estado_nombre { get; set; }

    }
}
