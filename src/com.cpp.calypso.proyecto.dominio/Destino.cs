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
    public class Destino : Entity
    {
		[Obligado]
		[DisplayName("Colaborador")]
		public int ColaboradorId { get; set; }
		public virtual Colaborador Colaborador { get; set; }

		[Obligado]
		[DisplayName("Nombre")]
		[LongitudMayorAttribute(100)]
		public string nombre { get; set; }

		[Obligado]
		[DisplayName("Descripcion")]
		[LongitudMayorAttribute(200)]
		public string descripcion { get; set; }

		[Obligado]
		[DisplayName("Vigente")]
		public bool vigente { get; set; } = true;
	}
}
