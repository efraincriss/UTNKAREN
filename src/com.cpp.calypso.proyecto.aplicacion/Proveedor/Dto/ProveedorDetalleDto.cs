using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.dominio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace com.cpp.calypso.proyecto.aplicacion.Dto
{
    [AutoMap(typeof(dominio.Proveedor.Proveedor))]
    [Serializable]
    public class ProveedorDetalleDto : EntityDto
    {
        [Obligado]
        [DisplayName("Contacto")]
        public int contacto_id { get; set; }

        [DisplayName("Tipo Identificación")]
        public dominio.Proveedor.ProveedorTipoIdentificacion tipo_identificacion { get; set; }
        public string tipo_identificacion_nombre { get; set; }

        [Obligado]
        [StringLength(20)]
        [DisplayName("Identificación")]
        public string identificacion { get; set; }

        [Obligado]
        [StringLength(100)]
        [DisplayName("Razón Social")]
        public string razon_social { get; set; }

        [Obligado]
        [DisplayName("Código SAP")]
        public string codigo_sap { get; set; }


        [Obligado]
        [DisplayName("Correo Electrónico")]
        [StringLength(200)]
        public string correo_electronico { get; set; }

        [Obligado]
        [DisplayName("Teléfono")]
        [StringLength(20)]
        public string telefono_convencional { get; set; }

        [Obligado]
        [DisplayName("Estado")]
        public int estado { get; set; }
        public string estado_nombre { get; set; }

        public int? documentacion_id { get; set; }

        public bool tiene_servicio_hospedaje { get; set; }
        public bool tiene_servicio_lavanderia { get; set; }

        public virtual ICollection<ContratoProveedorDto> contratos { get; set; }

        public virtual ICollection<ServicioProveedorDto> servicios { get; set; }

        public virtual ICollection<NovedadProveedorDto> novedades { get; set; }

        public virtual ICollection<ZonaProveedorDto> zonas { get; set; }

        public virtual ICollection<RequisitoProveedorDto> requisitos { get; set; }

    }
}
