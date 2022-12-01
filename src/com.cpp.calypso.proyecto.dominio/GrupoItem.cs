using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities;
using com.cpp.calypso.comun.dominio;

namespace com.cpp.calypso.proyecto.dominio
{
    [Serializable]
    public class GrupoItem: Entity
    {
        [LongitudMayor(100)]
        [Obligado]
        [DisplayName("Descripción")]
        public string descripcion { get; set; }


        [DefaultValue(true)]

        public bool vigente { get; set; } = true;

        [DisplayName("Código")]
        public string codigo { get; set; } = "";
    }
}
