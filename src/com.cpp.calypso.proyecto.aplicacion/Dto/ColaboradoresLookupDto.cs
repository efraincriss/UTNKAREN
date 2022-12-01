using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.dominio;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion.Dto
{
    [AutoMap(typeof(Colaboradores))]
    [Serializable]
    public class ColaboradoresLookupDto : EntityDto
    {
 	 
		[DisplayName("Nro Identificación:")]
		public string nro_identificacion { get; set; }

	 	public string nombres { get; set; }
        public string apellidos { get; set; }
        public string nombres_completos { get; set; }
    }
}
