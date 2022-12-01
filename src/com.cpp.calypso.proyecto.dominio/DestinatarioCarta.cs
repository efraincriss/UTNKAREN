using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities;
using com.cpp.calypso.comun.dominio;

namespace com.cpp.calypso.proyecto.dominio
{
    [Serializable]
    public class DestinatarioCarta : Entity
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
