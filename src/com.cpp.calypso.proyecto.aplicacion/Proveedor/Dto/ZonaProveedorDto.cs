using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.dominio;
using System;
using System.ComponentModel;

namespace com.cpp.calypso.proyecto.aplicacion.Dto
{
    [AutoMapTo(typeof(ZonaProveedor))]
    [Serializable]
    public class ZonaProveedorDto : EntityDto
    {
        [Obligado]
        [DisplayName("Proveedor")]
        public int ProveedorId { get; set; }

        [Obligado]
		[DisplayName("Zona")]
		public int ZonaId { get; set; }

        public string zona_nombre { get; set; }


        [Obligado]
		[DisplayName("Estado")]
		public int estado { get; set; }

		

	}
}
