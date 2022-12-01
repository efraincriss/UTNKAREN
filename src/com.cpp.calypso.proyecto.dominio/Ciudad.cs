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
    public class Ciudad : Entity
    {
        [Obligado]
        [DisplayName("Provincia")]
        public int ProvinciaId { get; set; }

        public virtual Provincia Provincia { get; set; }

        [Obligado]
        [DisplayName("Codigo")]
        [LongitudMayorAttribute(5)]
        public string codigo { get; set; }

        [Obligado]
        [DisplayName("Nombre")]
        [LongitudMayorAttribute(200)]
        public string nombre { get; set; }

        [Obligado]
        public bool vigente { get; set; } = true;
    }
}
