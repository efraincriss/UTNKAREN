using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.dominio;
using com.cpp.calypso.proyecto.dominio.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion.Dto
{
    [AutoMap(typeof(CertificadoIngenieria))]
    [Serializable]
    public class CertificadoIngenieriaDto : EntityDto
    {

        [DisplayName("Número Certificado")]
        public virtual string numero_certificado { get; set; }

        [Obligado]
        [DisplayName("Proyecto")]
        public int ProyectoId { get; set; }
        public Proyecto Proyecto { get; set; }

        [DisplayName("Fecha Corte")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime fechaCorte { get; set; }

        [DisplayName("Fecha Emisión")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime fechaEmision { get; set; }


        [DisplayName("Fecha de envío")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? fechaEnvio { get; set; }

        [DisplayName("Definitiva?")]
        public bool esDefinitivo { get; set; }

        [DisplayName("Versión")]
        public string version { get; set; }
        [DisplayName("Fase")]
        public string fase { get; set; }
        [DisplayName("Orden Servicio")]
        public string ordenServicio { get; set; }

        [DisplayName("Porcentaje Avance")]
        public decimal porcentajeAvance { get; set; }


        public bool vigente { get; set; } = true;
        public EstadoCertificadoIngenieria estado { get; set; }
        public virtual string nombreProyecto { get; set; }
        public virtual string periodo { get; set; }
        public virtual string formatFechaCorte { get; set; }
        public virtual string formatFechaEmision { get; set; }
        public virtual string formatFechaEnvio { get; set; }
        public virtual string formatDefinitivo { get; set; }
        public virtual string formatEstado { get; set; }
        public virtual List<CertificadoIngenieriaDetalle> detalles { get; set; }
    }
}
