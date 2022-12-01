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
    [AutoMap(typeof(ProcesoNotificacion))]
    [Serializable]
    public class ProcesoNotificacionDto : EntityDto
    {
        [LongitudMayor(100)]
        [Obligado]
        [DisplayName("Nombre")]
        public string nombre { get; set; }

        [LongitudMayor(1000)]
        [Obligado]
        [DisplayName("Formato")]
        public string formato { get; set; }

        [Obligado]
        [DisplayName("Estado")]
        public bool estado { get; set; } = true;

        [DisplayName("Tipo de Proceso")]
        public ProcesoNotificacion.TipoProceso Tipo { get; set; }

        [Obligado]
        public bool vigente { get; set; } = true;
    }
}
