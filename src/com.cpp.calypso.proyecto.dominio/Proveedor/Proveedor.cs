using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using com.cpp.calypso.comun.dominio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace com.cpp.calypso.proyecto.dominio.Proveedor
{
    [Serializable]
    public class Proveedor : AuditedEntity, ISoftDelete
    {
        public Proveedor()
        {
             
        }
 

        [Obligado]
		[DisplayName("Tipo Identificación")]
		public ProveedorTipoIdentificacion tipo_identificacion { get; set; }

		[Obligado]
		[StringLength(20)]
		[DisplayName("Identificación")]
		public string identificacion { get; set; }

		[Obligado]
		[StringLength(100)]
		[DisplayName("Razón Social")]
		public string razon_social { get; set; }

		 
        [Obligado]
        public int contacto_id { get; set; }

        [DisplayName("Contacto")]
        public virtual Contacto contacto { get; set; }
         
  

        [Obligado]
		[DisplayName("Estado")]
		public ProveedorEstado estado { get; set; }

        [Obligado]
        [DisplayName("Es externo?")]
        public int es_externo { get; set; } = 1;

		 
		[StringLength(50)]
		[DisplayName("Coordenadas")]
		public string coordenadas { get; set; }

          
		[Obligado]
		[DisplayName("Tipo Proveedor")]
		public int tipo_proveedor_id { get; set; }
        public virtual Catalogo tipo_proveedor { get; set; }

        [StringLength(30)]
        [DisplayName("Código SAP")]
        public string codigo_sap { get; set; }

        [DisplayName("Usuario")]
        [StringLength(60)]
        public string usuario { get; set; }

        [DisplayName("Documentación")]
        public int? documentacion_id { get; set; }
        public virtual Archivo documentacion { get; set; }


        public virtual ICollection<ContratoProveedor> contratos { get; set; }

        public virtual ICollection<ServicioProveedor> servicios { get; set; }

        public virtual ICollection<NovedadProveedor> novedades { get; set; }

        public virtual ICollection<ZonaProveedor> zonas { get; set; }

        public virtual ICollection<RequisitoProveedor> requisitos { get; set; }

        public virtual ICollection<MenuProveedor> menus { get; set; }

        /// <summary>
        /// Is this entity deleted?
        /// </summary>
        public bool IsDeleted { get; set; }
    }
    

    /// <summary>
    /// TODO: Se busco un enumerable para el tipo de identificacion, existe en Application, no domain.
    /// </summary>
    public enum ProveedorTipoIdentificacion
    {
        Cedula =1,
        Ruc = 2,
        //Passaporte =3,
    }

    public enum ProveedorEstado
    {
        [Description("Activo")]
        Activo = 1,
        
        [Description("Inactivo")]
        Inactivo = 0
    }
}
