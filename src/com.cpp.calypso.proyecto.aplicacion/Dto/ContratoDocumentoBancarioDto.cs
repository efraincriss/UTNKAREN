using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.dominio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion
{
    [AutoMap(typeof(ContratoDocumentoBancario))]
    [Serializable]
    public class ContratoDocumentoBancarioDto : EntityDto
    {
        [Obligado]
        [DisplayName("Contrato")]
        public int ContratoId { get; set; }
        public  Contrato Contrato { get; set; }
        [Obligado]
        [DisplayName("Archivo")]
        public int ArchivosContratoId { get; set; }
        public virtual ArchivosContrato ArchivosContrato { get; set; }
        [Obligado]
        [DisplayName("Institución Financiera")]
        public int InstitucionFinancieraId { get; set; }

        [Obligado]
        [DisplayName("Tipo de Documento")]
        public int tipo_documento { get; set; }

        [Obligado]
        [LongitudMayorAttribute(200)]
        [DisplayName("Concepto")]
        public string concepto { get; set; }
        [Obligado]
        [LongitudMayorAttribute(20)]
        [DisplayName("Código")]
        public string codigo { get; set; }


        [Obligado]
        [DisplayName("Fecha Emisión")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime fecha_emision { get; set; }

        [Obligado]
        [DisplayName("Fecha Vencimiento")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? fecha_vencimiento { get; set; }

        [Obligado]
        [DisplayName("Notificado al  Cliente?")]
        [DefaultValue(true)]
        public bool notificado_cliente { get; set; }

        [Obligado]
        [DisplayName("Estado")]
        public int estado { get; set; }

        [DisplayName("Fecha Notificación")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? fecha_notificacion { get; set; }

        [Obligado]
        [DisplayName("Vigente")]
        [DefaultValue(true)]
        public bool vigente { get; set; }
       
        public virtual InstitucionFinanciera InstitucionFinanciera { get; set; }
        public virtual ICollection<InstitucionFinanciera> InstitucionesFinancieras { get; set; }
    }

}
