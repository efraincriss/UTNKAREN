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
    [AutoMap(typeof(ZonaProvincia))]
    [Serializable]
    public class ZonaProvinciaDto : EntityDto
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
