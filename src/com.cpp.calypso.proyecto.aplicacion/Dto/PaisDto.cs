using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.dominio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion
{
    [AutoMap(typeof(Pais))]
    [Serializable]
    public class PaisDto : EntityDto
    {

        [Obligado]
        [DisplayName("Codigo")]
        [LongitudMayorAttribute(5)]
        public string codigo { get; set; }

        [Obligado]
        [DisplayName("Nombre")]
        [LongitudMayorAttribute(200)]
        public string nombre { get; set; }

        [Obligado]
        public bool vigente { get; set; } = true;

    }
}
