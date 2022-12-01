using Abp.Domain.Entities;
using com.cpp.calypso.comun.dominio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace com.cpp.calypso.proyecto.dominio
{
    [Serializable]
    public class DistribucionVianda : Entity
    {
        public DistribucionVianda()
        {
            Detalle = new List<DetalleDistribucion>();
        }

        [Obligado]
		[DisplayName("Proveedor")]
		public int ProveedorId { get; set; }
		public virtual dominio.Proveedor.Proveedor Proveedor { get; set; }


        [Obligado]
        [DisplayName("Tipo Comida")]
        public int tipo_comida_id { get; set; }
        public virtual Catalogo tipo_comida { get; set; }

        [Obligado]
        [DisplayName("Estado")]
        public DistribucionViandaEstado estado { get; set; }

        /// <summary>
        /// Detalle de distribucion (Lista de Solicitudes)
        /// </summary>
        public virtual ICollection<DetalleDistribucion> Detalle { get; set; }


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
        public Colaboradores conductor_asignado { get; set; } //TIENE QUE IR A COLABORADOR

        [DataType(DataType.Time)]
	  	[DisplayName("Hora Asignación Transporte")]
		[DisplayFormat(DataFormatString = "{0:HH-mm}", ApplyFormatInEditMode = true)]
		public DateTime? hora_asignacion_transporte { get; set; }

		 
	}

    public enum DistribucionViandaEstado
    {
        [Description("Registrado")]
        Registrado = 1,

        [Description("Aprobado")]
        Aprobado = 2,

        [Description("Asignado Transporte")]
        AsignadoTransporte = 3,

        [Description("Despachada transporte")]
        DespachadaTransporte = 4,

        [Description("Entregada Anotador")]
        EntregadaAnotador = 5
    }
}
