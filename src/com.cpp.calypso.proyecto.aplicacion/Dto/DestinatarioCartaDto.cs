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
    [AutoMap(typeof(DestinatarioCarta))]
    [Serializable]
    public  class DestinatarioCartaDto :EntityDto
    {
        [DisplayName("Carta")]
        public int CartaId { get; set; }

        public virtual Carta Carta { get; set; }

        [DisplayName("Destinatario")]
        public int DestinatarioId { get; set; }

        public virtual Destinatario Destinatario { get; set; }

        [Obligado]
        [DisplayName("Estado")]
        public bool estado { get; set; }

        [Obligado]
        [DefaultValue(true)]
        public bool vigente { get; set; }
    }
}
