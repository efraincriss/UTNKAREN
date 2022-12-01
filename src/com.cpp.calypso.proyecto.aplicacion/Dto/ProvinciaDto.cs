using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.dominio;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion
{
    [AutoMap(typeof(Provincia))]
    [Serializable]
    public class ProvinciaDto : EntityDto
    {

        [Obligado]
        [DisplayName("Pais")]
        public int PaisId { get; set; }

        public virtual Pais Pais { get; set; }

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

		[Obligado]
		public bool? region_amazonica { get; set; }

    }
}
