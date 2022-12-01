using com.cpp.calypso.comun.dominio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using com.cpp.calypso.proyecto.dominio;

namespace com.cpp.calypso.proyecto.aplicacion
{
    [AutoMap(typeof(InstitucionFinanciera))]
    [Serializable]
    public class InstitucionFinancieraDto : EntityDto
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
