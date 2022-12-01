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

    [AutoMap(typeof(DetalleCertificadoIngenieria))]
    [Serializable]
    public class DetalleCertificadoIngenieriaDto : EntityDto
    {

        [Obligado]
        [DisplayName("Certificado")]
        public int CertificadoId { get; set; }
        public virtual Certificado Certificado { get; set; }

        [Obligado]
        [DisplayName("Computo")]
        public int DetalleAvanceIngenieriaId { get; set; }
        public virtual DetalleAvanceIngenieria DetalleAvanceIngenieria { get; set; }

        [DisplayName("Cantidad Presupuestada")]
        public decimal cantidad_presupuestada { get; set; }

        [DisplayName("Cantidad Certificada")]
        public decimal cantidad_certificada { get; set; }

        [DisplayName("Cantidad Pendiente Certificar")]
        public decimal cantidad_pendiente_certificar { get; set; }

        [DisplayName("Cantidad Avance")]
        public decimal cantidad_avance { get; set; }

        [DisplayName("Monto a Certificar")]
        public decimal monto_a_certificar { get; set; }

        [DisplayName("Estatus Item")]
        public decimal estatus_item { get; set; }

        [Obligado] public bool vigente { get; set; } = true;

        public virtual Oferta Oferta { get; set; }
    }
}
