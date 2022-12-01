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
    [AutoMap(typeof(Zona))]
    [Serializable]
    public class ZonaDto : EntityDto
    {
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
        public bool vigente { get; set; } = true;

    }
}
