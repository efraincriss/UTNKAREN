using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.dominio;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace com.cpp.calypso.proyecto.aplicacion.Dto
{
    [AutoMap(typeof(TipoOpcionComida))]
    [Serializable]
    public class TipoOpcionComidaDto : EntityDto
    {
        [Obligado]
        [DisplayName("Contrato")]
        public int ContratoId { get; set; }

        [Obligado]
        [DisplayName("Opciones Comida")]
        public int opcion_comida_id { get; set; }
        public string opcion_comida_nombre { get; set; }

        

        [Obligado]
        [DisplayName("Costo")]
        public decimal costo { get; set; }

		
		 

		[Obligado]
		[DisplayName("Tipo Comida")]
		public int tipo_comida_id { get; set; }
        public string tipo_comida_nombre { get; set; }


        [Obligado]
        [DataType(DataType.Date)]
        [DisplayName("Hora Inicio")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime hora_inicio { get; set; }


        [Obligado]
        [DataType(DataType.Date)]
        [DisplayName("Hora Fin")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime hora_fin { get; set; }


    }
}
