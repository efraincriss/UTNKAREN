using Abp.Domain.Entities;
using com.cpp.calypso.comun.dominio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace com.cpp.calypso.proyecto.dominio
{
    [Serializable]
    public class CentrodecostosContrato : Entity
    {
        [Obligado]
        [DisplayName("Contrato")]
        public int ContratoId { get; set; }

        [JsonIgnore]
        public virtual Contrato Contrato { get; set; }


        [Obligado]
        [DisplayName("Centro de Costos")]
        public int id_centrocostos{ get; set; }

        [Obligado]
        [LongitudMayorAttribute(800)]
        [DisplayName("Observaciones")]
        public string observaciones{ get; set; }


        [Obligado]
        [DisplayName("Estado (Activo/Inactivo)")]
        [DefaultValue(true)]
        public bool estado { get; set; }

        [Obligado]
        [DefaultValue(true)]
        [DisplayName("Vigente")]
        public bool vigente { get; set; }

  
    }
}
