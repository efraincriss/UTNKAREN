using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.dominio;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace com.cpp.calypso.proyecto.aplicacion.Dto
{
    [AutoMap(typeof(JustificacionVianda))]
    [Serializable]
    public class JustificacionViandaDto : EntityDto
    {
        public int total_pedido { get; set; }
        public int total_consumido { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Fecha Solicitud")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime fecha_solicitud { get; set; }

        [Obligado]
		[DisplayName("Solicitud")]
		public int SolicitudViandaId { get; set; }
     
        [DisplayName("Solicitante")]
        public int solicitante_id { get; set; }

        [DisplayName("Solicitante")]
        public string solicitante_nombre { get; set; }

        [DisplayName("Conductor Asignado")]
        public int? conductor_asignado_id { get; set; }
        public string conductor_asignado_nombre { get; set; }

        [DisplayName("Anotador")]
        public int? anotador_id { get; set; }
        public string anotador_nombre { get; set; }





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
        public string estado_nombre { get; set; }
    }
}
