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
    public class Destinatario : Entity
    {
        [Obligado]
        [LongitudMayorAttribute(500)]
        [DisplayName("Institución")]
        public string intitucion{ get; set; }
        [Obligado]
        [LongitudMayorAttribute(100)]
        [DisplayName("Cargo")]
        public string cargo {get; set; }
        [Obligado]
        [LongitudMayorAttribute(500)]
        [DisplayName("Nombre")]
        public string nombre { get; set; }


        [Obligado]
        [DefaultValue(true)]
        public bool vigente { get; set; }

    }
}
