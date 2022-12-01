using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.dominio;

namespace com.cpp.calypso.proyecto.aplicacion
{
    [AutoMap(typeof(AvanceProcura))]
    [Serializable]
    public class AvanceProcuraDto : EntityDto

    { 
        [Obligado]
        [DisplayName("Oferta")]
        public int OfertaId { get; set; }

        public  virtual Oferta Oferta { get; set; }

        [DisplayName("Certificado")]
        public int CertificadoId { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Fecha Presentación")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime fecha_presentacion { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Fecha Desde")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime fecha_desde { get; set; }


        [DataType(DataType.Date)]
        [DisplayName("Fecha Hasta")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime fecha_hasta { get; set; }


        [DisplayName("Aprobado")]
        public bool aprobado { get; set; } = false;


        [DisplayName("Monto Procura")]
        public decimal monto_procura { get; set; }


        [DisplayName("Estado")]
        public AvanceProcura.EstadoProcura estado { get; set; }

        [Obligado] public bool vigente { get; set; } = true;

        public virtual Proyecto Proyecto { get; set; }

        public virtual decimal VnumItems { get; set; }

    }
}
