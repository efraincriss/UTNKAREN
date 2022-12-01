using Abp.Application.Services.Dto;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.dominio;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace com.cpp.calypso.proyecto.aplicacion.Dto
{

    /// <summary>
    /// Total de consumos por solicitud de vianda.
    /// </summary>
    [Serializable]
    public class ConsumoViandaTotalesDto : EntityDto
    {
       ///Id. (Id de distriubuccion)

        [Obligado]
        [DisplayName("Detalle Distribuccion Id")]
        public int detalle_distribuccion_id { get; set; }

        [Obligado]
        [DisplayName("Solicitud")]
        public int solicitud_id { get; set; }
   
        [DisplayName("Solicitante")]
        public string solicitante_nombre { get; set; }

        [Obligado]
        [DisplayName("Locacion")]
        public int LocacionId { get; set; }
        public virtual string locacion_nombre { get; set; }

        [Obligado]
        [DisplayName("Disciplina")]
        public int disciplina_id { get; set; }
        public virtual string disciplina_nombre { get; set; }

        [Obligado]
        [DisplayName("Area")]
        public int AreaId { get; set; }
        public virtual string area_nombre { get; set; }


        [Obligado]
		[DisplayName("Proveedor")]
		public int ProveedorId { get; set; }
		public virtual string proveedor_nombre { get; set; }

        [Obligado]
        [DisplayName("Tipo Comida")]
        public int tipo_comida_id { get; set; }
        public virtual string tipo_comida_nombre { get; set; }

        [DataType(DataType.Date)]
		[DisplayName("Fecha")]
		[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
		public DateTime fecha_distribuccion { get; set; }

        [Obligado]
        [DisplayName("Estado")]
        public SolicitudViandaEstado estado_solicitud { get; set; }

        public string estado_solicitud_nombre { get; set; }


        public int pedido_viandas;
 
        public int alcance_viandas;

        [Obligado]
        [DisplayName("Total Pedido")]
        public int total_pedido { get; set; }


        [DisplayName("Consumido")]
        public int consumido { get; set; }


        [DisplayName("Consumo Justificado")]
        public int consumo_justificado { get; set; }


        [DisplayName("Total Consumido")]
        public int total_consumido { get; set; }

    }
}
