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

namespace com.cpp.calypso.proyecto.aplicacion.Dto
{
    [AutoMap(typeof(Servicio))]
    [Serializable]
    public class ServicioDto : EntityDto
    {
        [Obligado]
        [DisplayName("Código")]
        public string codigo { get; set; }

        [Obligado]
        [DisplayName("Nombre")]
        [LongitudMayorAttribute(200)]
        public string nombre { get; set; }

		public virtual string nombre_estado { get; set; }

		public virtual int nro { get; set; }
	}
}
