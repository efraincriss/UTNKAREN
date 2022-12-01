using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.dominio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion.Dto
{
    [AutoMap(typeof(RsoCabecera))]
    [Serializable]
    public class RsoCabeceraDto : EntityDto
    {
        [Obligado]
        [DisplayName("Proyecto")]
        public int ProyectoId { get; set; }

        public virtual Proyecto Proyecto { get; set; }

        [Obligado]
        [DisplayName("Código Rdo")]
        public string codigo_rdo { get; set; }

        public DateTime? fecha_inicio { get; set; }

        [Obligado]
        [DisplayName("Fecha de Rdo")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime fecha_rdo { get; set; }

        [DisplayName("Fecha de envío")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? fecha_envio { get; set; }

        [Obligado]
        [DisplayName("Versión")]
        [LongitudMayor(10)]
        public string version { get; set; }

        [Obligado]
        [DisplayName("Definitia?")]
        public bool es_definitivo { get; set; }

        [DisplayName("Observaciones")]
        public string observacion { get; set; }



        [Obligado]
        [DefaultValue(true)]
        public bool vigente { get; set; }

      
        [Obligado] public bool estado { get; set; } = true;

        [Obligado] [DisplayName("Emitido?")] public bool emitido { get; set; } = false;

        public decimal avance_real_acumulado { get; set; }


        public virtual string FormatEstado { get; set; }
    }
}

