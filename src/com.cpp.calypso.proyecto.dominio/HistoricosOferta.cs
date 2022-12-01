using Abp.Domain.Entities;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.dominio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace com.cpp.calypso.proyecto
{
    [Serializable]
    public class HistoricosOferta : Entity
    {

        [Obligado]
        [DisplayName("Oferta")]
        public int OfertaId { get; set; }
        [JsonIgnore]
        public virtual Oferta Oferta { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Fecha")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime fecha { get; set; }

        [DisplayName("Observaciones")]
        [LongitudMayor(1000)]
        public string observaciones { get; set; }

        [DisplayName("Usuario")]
        public string usuario { get; set; }

        [Obligado]
        [DefaultValue(true)]
        public bool vigente { get; set; }

    }
}
