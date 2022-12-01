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
    [AutoMap(typeof(HistoricosOfertaComercial))]
    [Serializable]
    public class HistoricosOfertaComercialDto
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
