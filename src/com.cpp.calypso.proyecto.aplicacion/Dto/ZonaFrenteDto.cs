using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using com.cpp.calypso.comun.dominio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion
{
    [AutoMap(typeof(ZonaFrente))]
    [Serializable]
    public class ZonaFrenteDto : EntityDto
    {

        [Obligado]
        [DisplayName("Zonas")]
        public int ZonaId { get; set; }

        public virtual Zona Zona { get; set; }

        [Obligado]
        [DisplayName("Codigo")]
        [LongitudMayorAttribute(10)]
        public string codigo { get; set; }

        [Obligado]
        [DisplayName("Nombre")]
        [LongitudMayorAttribute(60)]
        public string nombre { get; set; }

        [Obligado]
        [DisplayName("Descripcion")]
        [LongitudMayorAttribute(400)]
        public string descripcion { get; set; }

        [Obligado]
        [DisplayName("Coredenada X")]
        [LongitudMayorAttribute(50)]
        public string cordenada_x { get; set; }

        [Obligado]
        [DisplayName("Cordenada Y")]
        [LongitudMayorAttribute(50)]
        public string cordenada_y { get; set; }

        [Obligado]
        public bool vigente { get; set; } = true;

    }
}
