using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.dominio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion
{
    [AutoMap(typeof(Destinatario))]
    [Serializable]
    public class DestinatarioDto :EntityDto
    {
        [Obligado]
        [LongitudMayorAttribute(500)]
        [DisplayName("Institución")]
        public string intitucion { get; set; }
        [Obligado]
        [LongitudMayorAttribute(100)]
        [DisplayName("Cargo")]
        public string cargo { get; set; }
        [Obligado]
        [LongitudMayorAttribute(500)]
        [DisplayName("Nombre")]
        public string nombre { get; set; }

        [Obligado]
        [DefaultValue(true)]
        public bool vigente { get; set; }

    }
}
