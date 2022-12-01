using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.dominio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion.Dto
{
    [AutoMap(typeof(RequisitoProveedor))]
    [Serializable]
    public class RequisitoProveedorDto : EntityDto
    {

		[Obligado]
		[DisplayName("Requisito")]
		public int RequisitosId { get; set; }

        public string requisito_nombre { get; set; }

        
        [Obligado]
		[DisplayName("Proveedor")]
		public int ProveedorId { get; set; }
		 

		[Obligado]
		[DisplayName("Cumple")]
		public RequisitoEstado cumple { get; set; }
        public string cumple_nombre { get; set; }

        [Obligado]
		[StringLength(500)]
		[DisplayName("Observaciones")]
		public string observaciones { get; set; }

		 

	}
}
