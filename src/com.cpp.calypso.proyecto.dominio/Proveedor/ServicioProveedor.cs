using Abp.Domain.Entities;
using com.cpp.calypso.comun.dominio;
using System;
using System.ComponentModel;

namespace com.cpp.calypso.proyecto.dominio
{
    [Serializable]
    public class ServicioProveedor : Entity, ISoftDelete
	{
        public ServicioProveedor()
        {
            
        }


        [Obligado]
		[DisplayName("Servicio")]
		public int ServicioId { get; set; }
	    public virtual Catalogo Servicio { get; set; }

        [Obligado]
		[DisplayName("Proveedor")]
		public int ProveedorId { get; set; }
		public virtual dominio.Proveedor.Proveedor Proveedor { get; set; }

		[Obligado]
		[DisplayName("Estado")]
		public ServicioEstado estado { get; set; }


        public bool IsDeleted { get ; set ; }
    }

    public enum ServicioEstado
    {
        [Description("Inactivo")]
        Inactivo = 0,

        [Description("Activo")]
        Activo = 1,


    }
}
