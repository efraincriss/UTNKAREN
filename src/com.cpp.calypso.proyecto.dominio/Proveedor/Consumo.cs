using Abp.Domain.Entities;
using com.cpp.calypso.comun.dominio;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace com.cpp.calypso.proyecto.dominio
{
    /// <summary>
    /// TODO: Para que sirve esta tabla ???
    /// </summary>
    [Serializable]
    public class Consumo : Entity
    {
		[Obligado]
		[DisplayName("Proveedor")]
		public int ProveedorId { get; set; }
		public virtual dominio.Proveedor.Proveedor Proveedor { get; set; }

		 
        [Obligado]
        [DisplayName("Colaborador")]
        public int colaborador_id { get; set; }
        public Colaboradores colaborador { get; set; }

        /// <summary>
        /// Fecha del Consumo
        /// </summary>
        [DataType(DataType.Date)]
		[DisplayName("Fecha")]
		[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
		public DateTime fecha { get; set; }

		[Obligado]
       
        [DisplayName("Tipo Comida")]
		public int Tipo_Comida_Id { get; set; }
		public virtual Catalogo TipoComida { get; set; }


        /// <summary>
        /// TODO: Revision del Cliente, en el modelo. 
        /// 1. Opcion Comida, no debe ser tabla, se esta utilizando un catalogo. Codigo: OPCIONCOMIDA
        /// 2. Tiene un enlace a tipoOpcionComida, que ya posee opcion de comida. (Esta entidad esta atada al contrato del proveedor)
        /// </summary>
		[Obligado]
        [DisplayName("Opción Comida")]
		public int Opcion_Comida_Id { get; set; }
		public virtual Catalogo OpcionComida { get; set; }

		[DisplayName("Observación")]
		public string observacion { get; set; }

		[Obligado]
		[DisplayName("Vigente")]
		public bool vigente { get; set; } = true;

        [DisplayName("Origen Consumo")]
        public int? origen_consumo { get; set; }

        [DisplayName("liquidado")]
        public bool liquidado { get; set; } = false;

        [DisplayName("Detalle Liquidación")]
        public int liquidacion_detalle_id { get; set; } = 0;

        public string identificador { get; set; }
    }
}
