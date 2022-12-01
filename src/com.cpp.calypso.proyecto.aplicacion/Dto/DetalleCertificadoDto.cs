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

namespace com.cpp.calypso.proyecto.aplicacion.Dto
{
    [AutoMap(typeof(DetalleCertificado))]
    [Serializable]
    public class DetalleCertificadoDto : EntityDto
    {

        [Obligado]
        [DisplayName("Certificado")]
        public int CertificadoId { get; set; }

        public virtual Certificado Certificado { get; set; }

        [Obligado] [DisplayName("Computo")] public int ComputoId { get; set; }
        public virtual Computo Computo { get; set; }

        [DisplayName("Cantidad Presupuestada")]
        public virtual decimal cantidad_presupuestada { get; set; }

        [DisplayName("Cantidad Certificada")] public virtual decimal cantidad_certificada { get; set; }

        [DisplayName("Cantidad Pendiente Certificar")]
        public virtual decimal cantidad_pendiente_certificar { get; set; }

        [DisplayName("Cantidad Avance")] public virtual decimal cantidad_avance { get; set; }

        [DisplayName("Monto a Certificar")] public virtual decimal monto_a_certificar { get; set; }

        [DisplayName("Estatus Item")] public virtual decimal estatus_item { get; set; }

        [Obligado] public virtual bool vigente { get; set; } = true;

        public virtual Oferta Oferta { get; set; }
        public virtual Item Item { get; set; }

        [DisplayName("Avance Referencia")]
        public string avanceid_referencia { get; set; }

        [DisplayName("Tipo de Avance")]
        public int tipoavance { get; set; }

    }
}
