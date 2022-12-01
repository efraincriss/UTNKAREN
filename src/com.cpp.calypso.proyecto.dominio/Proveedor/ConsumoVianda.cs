using Abp.Domain.Entities;
using com.cpp.calypso.comun.dominio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.dominio
{
    /// <summary>
    /// Consumo de viandas por los colaboradores
    /// </summary>
    [Serializable]
    public class ConsumoVianda : Entity
    {
		[Obligado]
		[DisplayName("Solicitud")]
		public int SolicitudViandaId { get; set; }
		public virtual SolicitudVianda SolicitudVianda { get; set; }

        [Obligado]
        [DisplayName("Colaborador")]
        public int colaborador_id { get; set; }
        public Colaboradores colaborador { get; set; }

        [DataType(DataType.Date)]
		[DisplayName("Fecha Consumo Vianda")]
		[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
		public DateTime fecha_consumo_vianda { get; set; }

		[Obligado]
		[DisplayName("Tipo Comida")]
		public int TipoOpcionComidaId { get; set; }
		public virtual TipoOpcionComida TipoComida { get; set; }

        /// <summary>
        /// TODO: Revision por el  Cliente. 
        /// 1. Opcion Comida, no debe ser tabla, se esta utilizando un catalogo. Codigo: OPCIONCOMIDA
        /// 2. Tiene un enlace a tipoOpcionComida, que ya posee opcion de comida. (Esta entidad esta atada al contrato del proveedor)
        /// </summary>
        [Obligado]
		[DisplayName("Opción Comida")]
		public int OpcionComidaId { get; set; }
		public virtual OpcionComida OpcionComida { get; set; }

		[Obligado]
		[DisplayName("En sitio?")]
		public int en_sitio { get; set; }

		[Obligado]
		[StringLength(500)]
		[DisplayName("Observaciones")]
		public string observaciones { get; set; }
 
	}
}
