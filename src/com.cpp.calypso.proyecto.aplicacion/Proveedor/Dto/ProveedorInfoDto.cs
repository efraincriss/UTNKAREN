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
    public class ProveedorInfoDto : EntityDto
    {
 
        [Obligado]
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
        [DisplayName("Estado")]
        public string estado_nombre { get; set; }
         

        [DisplayName("Tipo Proveedor")]
        public string tipo_proveedor_nombre { get; set; }


        [Obligado]
        [DisplayName("Código SAP")]
        public string codigo_sap { get; set; }
 
    }
}
