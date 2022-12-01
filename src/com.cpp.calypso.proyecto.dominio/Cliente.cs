using Abp.Domain.Entities;
using Abp.Runtime.Validation;
using com.cpp.calypso.comun.dominio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities.Auditing;

namespace com.cpp.calypso.proyecto.dominio
{
    [Serializable]
    public  class Cliente: Entity
    {
        [Obligado]
        [DisplayName("Tipo de Identificación")]
        public TipoDeIdentificacion tipo_identificacion { get; set; }

        [LongitudMayorAttribute(20)]
        [Obligado]
        [DisplayName("Identificación")]
        public string identificacion { get; set; }

        [LongitudMayorAttribute(100)]
        [Obligado]
        [DisplayName("Razón Social")]
        public string razon_social { get; set; }

        [Obligado]
        [DisplayName("Fecha de Registro")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime fecha_registro { get; set; }


        [DisplayName("Fecha de Fin")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? fecha_fin { get; set; }

        [LongitudMayorAttribute(15)]
        [Obligado]
        [DisplayName("Teléfono")]
        public string telefono { get; set; }

        [LongitudMayorAttribute(255)]
        [Obligado]
        [DisplayName("Dirección")]
        public string direccion { get; set; }

        [LongitudMayorAttribute(100)]
        [Obligado]
        [DisplayName("Correo Electrónico")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string correo { get; set; }
       
        [Obligado]
        [DisplayName("Pertenece al Grupo TECHINT?")]
        [DefaultValue(true)]
        public bool es_grupo { get; set; }

        [Obligado]
        [DefaultValue(true)]
        [DisplayName("Tiene Contrato?")]
        public bool tiene_contrato { get; set; }

        [Obligado]
        [DisplayName("Tipo de Contribuyente")]
        public TipoDeContribuyente tipo_contribuyente { get; set; }
        
        [Obligado]
        [DefaultValue(true)]
        public bool vigente { get; set; }
        
      
        [DisplayName("Representante Legal")]
        [LongitudMayorAttribute(100)]
        public string representate_legal { get; set; }


        [Obligado]
        [DefaultValue(true)]
        [DisplayName("Estado (Activo/Inactivo)")]
        public bool  estado { get; set; }

        [Obligado]
        [DisplayName("Código ")]
        [LongitudMayorAttribute(20)]
        public string codigoasignado { get; set; }

        [DisplayName("Project Manager")]
        [LongitudMayorAttribute(100)]
        public string projectmanager { get; set; }


        [DisplayName("Código Sap")]
        [LongitudMayorAttribute(200)]
        public string codigo_sap { get; set; }

        [DisplayName("Dias Plazo")]
        public int dias_plazo { get; set; } = 0;

        [DisplayName("Lider Operaciones ")]
        public string lider_operaciones { get; set; }

    }
}

