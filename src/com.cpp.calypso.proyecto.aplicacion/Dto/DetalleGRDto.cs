using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.dominio;

namespace com.cpp.calypso.proyecto.aplicacion
{
    [AutoMap(typeof(DetalleGRDto))]
    [Serializable]
    public class DetalleGRDto : EntityDto
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
