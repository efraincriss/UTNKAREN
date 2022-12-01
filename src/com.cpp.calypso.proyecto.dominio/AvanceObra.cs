using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities;
using com.cpp.calypso.comun.dominio;

namespace com.cpp.calypso.proyecto.dominio
{
    [Serializable]
    public class AvanceObra : Entity { 
        [Obligado]
        [DisplayName("Oferta")]
        public int OfertaId { get; set; }

        public virtual Oferta Oferta { get; set; }

        [LongitudMayor(800)]
        [Obligado]
        [DisplayName("Descripción")]
        public string descripcion { get; set; }


        [LongitudMayor(100)]
        [Obligado]
        [DisplayName("Alcance")]
        public string alcance { get; set; }


        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DisplayName("Fecha Presentación")]
        public DateTime? fecha_presentacion { get; set; }


        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DisplayName("Fecha desde")]
        public DateTime? fecha_desde { get; set; }


        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DisplayName("Fecha hasta")]
        public DateTime? fecha_hasta { get; set; }


        [Obligado]
        [DisplayName("Certificado")]
        public int certificado { get; set; }


        [LongitudMayor(800)]
        [DisplayName("Comentario")]
        public string comentario { get; set; }

        [Obligado]
        [DisplayName("Monto Ingeniería")]
        public decimal monto_ingenieria { get; set; }

        [Obligado]
        [DisplayName("Monto Suministros")]
        public decimal monto_suministros { get; set; }

        [Obligado]
        [DisplayName("Monto Construcción")]
        public decimal monto_construccion { get; set; }

        [Obligado]
        [DisplayName("Monto Total")]
        public decimal monto_total { get; set; }


        [Obligado]
        public bool vigente { get; set; }

        [Obligado]
        [DisplayName("Aprobado?")]
        public bool aprobado { get; set; }

        [Obligado] [DisplayName("Emitido?")] public bool emitido { get; set; } = false;

        [DisplayName("Estado")]
        public EstadoAvanceObra estado { get; set; }

  



    }

    public enum EstadoAvanceObra
    {
        Generado = 1,
        Aprobado = 2,
        CertEmitido = 3,
        certAprobado = 4,
    }
}
