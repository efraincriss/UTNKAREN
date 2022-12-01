using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.cpp.calypso.comun.dominio;

namespace com.cpp.calypso.proyecto.dominio
{
    [Serializable]
    public class InstitucionFinanciera : Entity
    {

        [Obligado]
        [LongitudMayorAttribute(100)]
        public string nombre { get; set; }

        [Obligado]
        [LongitudMayorAttribute(100)]
        public string direccion { get; set; }

        [Obligado]
        [LongitudMayorAttribute(20)]
        public string telefono { get; set; }

        [Obligado]
        [LongitudMayorAttribute(100)]
        public string persona_contrato { get; set; }

        [Obligado]
        [LongitudMayorAttribute(800)]
        public string concepto { get; set; }
        [Obligado]
        [DefaultValue(true)]
        public bool vigente { get; set; }



    }
}
