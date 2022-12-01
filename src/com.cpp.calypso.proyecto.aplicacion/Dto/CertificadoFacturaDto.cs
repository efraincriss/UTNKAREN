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
    [AutoMap(typeof(CertificadoFactura))]
    [Serializable]
   public class CertificadoFacturaDto :EntityDto
    {
        [Obligado]
        [DisplayName("Factura")]
        public virtual int FacturaId { get; set; }
        public virtual Factura Factura { get; set; }

        [Obligado]
        [DisplayName("Certificado")]
        public virtual int CertificadoId { get; set; }
        public virtual Certificado Certificado { get; set; }


        [Obligado]
        [DefaultValue(true)]
        public virtual bool vigente { get; set; }
    }
}
