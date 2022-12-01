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
    [AutoMap(typeof(CentrodecostosContrato))]
    [Serializable]
 public   class CentrocostosContratoDto: EntityDto
    {
        [Obligado]
        [DisplayName("Contrato")]
        public int ContratoId { get; set; }

        public virtual Contrato Contrato { get; set; }
        [Obligado]
        [DisplayName("Centro de Costos")]
        public int id_centrocostos { get; set; }

        [Obligado]
        [LongitudMayorAttribute(800)]
        [DisplayName("Observaciones")]
        public string observaciones { get; set; }


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
