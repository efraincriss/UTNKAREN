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
    public class ProyectoColaborador : Entity
    {
		[Obligado]
		[DisplayName("Colaborador")]
		public int ColaboradoresId { get; set; }
		public virtual Colaboradores Colaboradores { get; set; }

		[Obligado]
		[DisplayName("Proyecto")]
		public int ProyectoId { get; set; }
		public virtual Proyecto Proyecto { get; set; }

		[Obligado]
        [DisplayName("Vigente")]
        public bool vigente { get; set; } = true;
    }
}
