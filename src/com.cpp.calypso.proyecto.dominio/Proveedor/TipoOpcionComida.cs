using Abp.Domain.Entities;
using com.cpp.calypso.comun.dominio;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace com.cpp.calypso.proyecto.dominio
{
    /// <summary>
    /// Tipo opcion de comida. (Asociado al contrato del proveedor)
    /// Tipo Comida / Opcion de Comida
    /// Costos 
    /// </summary>
    [Serializable]
    public class TipoOpcionComida : Entity
    {
        [Obligado]
        [DisplayName("Contrato")]
        public int ContratoId { get; set; }
        public virtual ContratoProveedor contrato { get; set; }


        [Obligado]
		[DisplayName("Opciones Comida")]
		public int opcion_comida_id { get; set; }
		
        public virtual Catalogo opcion_comida { get; set; }
 

		[Obligado]
		[DisplayName("Costo")]
		public decimal costo { get; set; }
 
		[Obligado]
		[DisplayName("Tipo Comida")]
		public int tipo_comida_id { get; set; }
        public virtual Catalogo tipo_comida { get; set; }


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
