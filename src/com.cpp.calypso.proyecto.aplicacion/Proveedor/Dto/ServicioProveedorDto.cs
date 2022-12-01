using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.dominio;
using System;
using System.ComponentModel;

namespace com.cpp.calypso.proyecto.aplicacion.Dto
{
    [AutoMap(typeof(ServicioProveedor))]
	[Serializable]
	public class ServicioProveedorDto : EntityDto
	{

        [Obligado]
        [DisplayName("Servicio")]
        public int ServicioId { get; set; }
        public string servicio_nombre { get; set; }

        [Obligado]
        [DisplayName("Proveedor")]
        public int ProveedorId { get; set; }
      

        [Obligado]
		[DisplayName("Estado")]
		public ServicioEstado estado { get; set; }

        public bool IsDeleted { get; set; }

        public string estado_nombre { get; set; }

	    public string servicio_codigo { get; set; }

    }
}
