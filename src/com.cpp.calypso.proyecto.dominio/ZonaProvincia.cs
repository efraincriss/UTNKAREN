using Abp.Domain.Entities;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.dominio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto
{
    [Serializable]
    public class ZonaProvincia : Entity
    {

        [Obligado]
        [DisplayName("Zonas")]
        public int ZonaId { get; set; }

        public virtual Zona Zona { get; set; }

        [Obligado]
        [DisplayName("Provincias")]
        public int ProvinciaId { get; set; }

        public virtual Provincia Provincia { get; set; }


    }
}
