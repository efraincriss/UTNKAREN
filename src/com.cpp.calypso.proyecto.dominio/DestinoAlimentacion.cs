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
    public class DestinoAlimentacion : Entity
    {
		[Obligado]
		[DisplayName("Destino")]
		public int DestinoId { get; set; }
		public virtual Destino Destino { get; set; }

		[Obligado]
		[DisplayName("Tipo Comida")]
		public int TipoOpcionComidaId { get; set; }
		public virtual TipoOpcionComida TipoComida { get; set; }

		[Obligado]
		[DisplayName("Vigente")]
		public bool vigente { get; set; } = true;
	}
}
