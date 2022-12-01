using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using Abp.Domain.Entities;
using com.cpp.calypso.comun.dominio;
using Newtonsoft.Json;

namespace com.cpp.calypso.proyecto.dominio
{
    [Serializable]
    public class TipoCatalogo : Entity
    {
        [Obligado]
        [LongitudMayor(200)]
        [DisplayName("Nombre")]
        public string nombre { get; set; }

        [Obligado]
        [LongitudMayor(50)]
        [DisplayName("Código")]
        public string codigo { get; set; }

        [Obligado]
        public bool vigente { get; set; }


        [Obligado]
        [LongitudMayor(3)]
        [DisplayName("Tipo Ordinal")]
        public string tipo_ordenamiento { get; set; }


        [Obligado]
        public bool editable { get; set; }

        [JsonIgnore]
        [ScriptIgnore]
        [DisplayName("Módulo")]
        public int? ModuloId { get; set; }

  
        [JsonIgnore]
        [ScriptIgnore]
        public virtual Modulo Modulo { get; set; }
    }
}
