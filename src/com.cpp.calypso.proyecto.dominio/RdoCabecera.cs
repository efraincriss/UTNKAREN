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
   public class RdoCabecera:Entity
    {
        
        [Obligado]
        [DisplayName("Proyecto")]
        public int ProyectoId { get; set; }


        public virtual Proyecto Proyecto { get; set; }

        [Obligado]
        [DisplayName("Código Rdo")]
        public string codigo_rdo { get; set; }

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
        [DisplayName("Definitiva?")]
        public bool es_definitivo { get; set; }

        [DisplayName("Observaciones")]
        public string observacion { get; set; }


        [Obligado]
        [DefaultValue(true)]
        public bool vigente { get; set; }

        public List<RdoDetalle> RdoDetalles { get; set; }

        [Obligado] public bool estado { get; set; } = true;

        [Obligado] [DisplayName("Emitido?")] public bool emitido { get; set; } = false;


        public  decimal avance_real_acumulado { get; set; }
    }
}
