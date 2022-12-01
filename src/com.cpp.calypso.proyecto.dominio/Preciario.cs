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
    public class Preciario: Entity
    {
        [Obligado]
        [DisplayName("Contrato")]
        public int ContratoId { get; set; }
       
        public  Contrato Contrato { get; set; }
        [Obligado]
       
        [DisplayName("Fecha Desde")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime fecha_desde { get; set; }

        [Obligado]
        [DisplayName("Fecha Hasta")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime fecha_hasta { get; set; }

        [Obligado]
        [DisplayName("Estado (Activo/Inactivo)")]
        [DefaultValue(true)]
        public bool estado { get; set; }

        [Obligado]
        [DisplayName("Vigente")]
        [DefaultValue(true)]
        public bool vigente { get; set; }


    }
}
