using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities;
using com.cpp.calypso.comun.dominio;
using Newtonsoft.Json;

namespace com.cpp.calypso.proyecto.dominio
{
    [Serializable]
    public class GR : Entity
    {

        [DataType(DataType.Date)]
        [DisplayName("Fecha de Registro")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public virtual DateTime fecha_registro { get; set; }

        [Obligado]
        [DisplayName("Proyecto")]
        public int ProyectoId { get; set; }

        [JsonIgnore]
        public virtual Proyecto Proyecto { get; set; }

        public virtual Cliente Cliente { get; set; }

        [LongitudMayorAttribute(50)]
        [Obligado]
        [DisplayName("Número GR")]
        public virtual string numero_gr { get; set; }


        [Obligado]
        [DisplayName("Monto Ingeniería")]
        public decimal monto_ingenieria { get; set; }

        [Obligado]
        [DisplayName("Monto Construcción")]
        public decimal monto_construccion { get; set; }

        [Obligado]
        [DisplayName("Monto Suministros")]
        public decimal monto_suministros { get; set; }


        [Obligado]
        [DisplayName("Monto Total")]
        public virtual decimal monto_total { get; set; }

        public virtual bool vigente { get; set; } = true;
    }
}
