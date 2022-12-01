using Abp.Domain.Entities;
using com.cpp.calypso.comun.dominio;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.dominio
{
    [Serializable]
    public class Provincia: Entity
    {
        [Obligado]
        [DisplayName("Pais")]
        public int PaisId { get; set; }

        public virtual Pais Pais { get; set; }

        [Obligado]
        [DisplayName("Codigo")]
        [LongitudMayorAttribute(5)]
        public string codigo { get; set; }

        [Obligado]
        [DisplayName("Nombre")]
        [LongitudMayorAttribute(100)]
        public string nombre { get; set; }

        [Obligado]
        public bool vigente { get; set; } = true;

		[Obligado]
		public bool? region_amazonica { get; set; }
    }
}
