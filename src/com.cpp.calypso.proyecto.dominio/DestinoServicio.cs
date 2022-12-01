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
    public class DestinoServicio : Entity
    {
		[Obligado]
		[DisplayName("Servicio")]
		public int ServicioId { get; set; }
		public virtual Servicio Servicio { get; set; }

		[Obligado]
		[DisplayName("Destino")]
		public int DestinoId { get; set; }
		public virtual Destino Destino { get; set; }

		[Obligado]
		[DisplayName("Vigente")]
		public bool vigente { get; set; } = true;
	}
}
