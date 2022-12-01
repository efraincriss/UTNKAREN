using Abp.Domain.Entities;
using com.cpp.calypso.comun.dominio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.dominio
{
    [Serializable]
    public class RequisitoFrente : Entity
    {

		[Obligado]
		[DisplayName("Requisito")]
		public int RequisitoColaboradorId { get; set; }
		public virtual RequisitoColaborador RequisitoColaborador { get; set; }

		[Obligado]
		
		[DisplayName("Frente")]
		public int ZonaFrenteId { get; set; }
		public virtual ZonaFrente ZonaFrente { get; set; }

		[Obligado]
		[DisplayName("Estado")]
		public bool vigente { get; set; } = true;

	}
}
