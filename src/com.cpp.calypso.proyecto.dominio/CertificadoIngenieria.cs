using Abp.Domain.Entities;
using com.cpp.calypso.comun.dominio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.dominio
{

    [Serializable]
    public class CertificadoIngenieria : Entity
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


        public decimal totalacumulado { get; set; } = 0;
        public decimal totalusd { get; set; } = 0;
        public bool vigente { get; set; } = true;
         
        public EstadoCertificadoIngenieria estado { get; set; }


    }
    public enum EstadoCertificadoIngenieria
    {
        [Description("Anulado")]
        Anulado = 0,

        [Description("Registrado")]
        Registrado = 1,
        [Description("Enviado")]
        Aprobado = 2,
    }

}
