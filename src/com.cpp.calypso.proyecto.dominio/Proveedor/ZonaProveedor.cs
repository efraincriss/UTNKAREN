using Abp.Domain.Entities;
using com.cpp.calypso.comun.dominio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.dominio
{
    [Serializable]
    public class ZonaProveedor : Entity
    {
		[Obligado]
		[DisplayName("Zona")]
		public int ZonaId { get; set; }
		public virtual Zona Zona { get; set; }

		[Obligado]
		[DisplayName("Proveedor")]
		public int ProveedorId { get; set; }
		public virtual dominio.Proveedor.Proveedor Proveedor { get; set; }

		[Obligado]
		[DisplayName("Estado")]
		public int estado { get; set; }

        [Obligado]
        [DefaultValue(true)]
        [DisplayName("Estado")]
        public bool vigente { get; set; } = true;

	}
}
