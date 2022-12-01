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
    public class DetalleGR : Entity
    {
        [Obligado]
        [DisplayName("GR")]
        public int GRId { get; set; }

        public virtual GR GR { get; set; }

        [Obligado]
        [DisplayName("Certificado")]
        public int CertificadoId { get; set; }

        public virtual Certificado Certificado { get; set; }

        public virtual bool vigente { get; set; } = true;
    }
}
