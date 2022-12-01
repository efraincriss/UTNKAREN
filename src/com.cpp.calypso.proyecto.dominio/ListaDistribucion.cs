using Abp.Domain.Entities;
using com.cpp.calypso.comun.dominio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.dominio
{
    [Serializable]
    public class ListaDistribucion : Entity
    {

        [LongitudMayor(100)]
        [Obligado]
        [DisplayName("Nombre")]
        public string nombre { get; set; }

        [Obligado]
        [DisplayName("Estado")]
        public bool estado { get; set; } = true;

        [Obligado]
        public bool vigente { get; set; } = true;

        public string codigo { get; set; } = ".";

    }
}
