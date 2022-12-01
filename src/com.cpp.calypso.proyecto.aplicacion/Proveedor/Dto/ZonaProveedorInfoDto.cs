using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.dominio;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace com.cpp.calypso.proyecto.aplicacion.Dto
{
   
    [Serializable]
    public class ZonaProveedorInfoDto : EntityDto
    {
        [Obligado]
        [DisplayName("Proveedor")]
        public int ProveedorId { get; set; }

        [Obligado]
        [StringLength(20)]
        [DisplayName("Identificación")]
        public string identificacion { get; set; }

        [Obligado]
        [StringLength(100)]
        [DisplayName("Razón Social")]
        public string razon_social { get; set; }

        [Obligado]
        [DisplayName("Zona")]
        public int ZonaId { get; set; }

        public string zona_nombre { get; set; }

    }
}
