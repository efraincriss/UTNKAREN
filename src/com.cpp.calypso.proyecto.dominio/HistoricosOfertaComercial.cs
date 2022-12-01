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
{       [Serializable]
    public class HistoricosOfertaComercial: Entity
    {

        [DataType(DataType.Date)]
        [DisplayName("Fecha registro")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? fecha_registro { get; set; }

        [LongitudMayorAttribute(1000)]
        [DisplayName("Observación")]
        public string observacion { get; set; }


        [DisplayName("Vigente")]
        public bool vigente { get; set; } = true;
    }
}
