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
using Newtonsoft.Json;

namespace com.cpp.calypso.proyecto.aplicacion
{
    [AutoMap(typeof(AvanceIngenieria))]
    [Serializable]
    public class AvanceIngenieriaDto : EntityDto
    {
        [Obligado]
        [DisplayName("Oferta")]
        public int OfertaId { get; set; }

        [JsonIgnore]
        public virtual Oferta Oferta { get; set; }

        [DisplayName("Certificado")]
        public int CertificadoId { get; set; }

        [DisplayName("Descripción")]
        [LongitudMayor(800)]
        public string descripcion { get; set; }

        [DisplayName("Alcance")]
        public string alcance { get; set; }

        [DisplayName("Comentario")]
        [LongitudMayor(800)]
        public string comentario { get; set; }

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

        [Obligado]
        [DisplayName("Monto Ingeniería")]
        public decimal monto_ingenieria { get; set; }

        [Obligado]
        [DisplayName("Aprobado")]
        public bool aprobado { get; set; } = false;


        [DisplayName("Estado")]
        public AvanceIngenieria.EstadoIngenieria estado { get; set; }

        [Obligado] public bool vigente { get; set; } = true;

        // Virtuales
        public virtual string codigo_proyecto { get; set; }

        public virtual string codigo_oferta { get; set; }

        public virtual int VnumeroItems { get; set; }
    }
}
