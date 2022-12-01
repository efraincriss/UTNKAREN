using Abp.Domain.Entities;
using com.cpp.calypso.comun.dominio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.dominio
{
    [Serializable]
    public class Factura : Entity
    {

        [Obligado]
        public int EmpresaId { get; set; }
        public virtual Empresa Empresa { get; set; }

        [Obligado]
        [LongitudMayorAttribute(20)]
        [DisplayName("Tipo de Documento")]
        public String tipo_documento { get; set; }

        [Obligado]
        [LongitudMayorAttribute(20)]
        [DisplayName("Nro de Documento")]
        public String numero_documento { get; set; }

        [Obligado]
        [DisplayName("Fecha Emisión")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime fecha_emision { get; set; }
        
               
        [DisplayName("Fecha Vencimiento")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? fecha_vencimiento { get; set; }

        [Obligado]
        public int ClienteId { get; set; }
        public virtual Cliente Cliente { get; set; }


        [DisplayName("Descripcion")]
        public string descripcion { get; set; }

        [DisplayName("Valor Importe")]
        public decimal valor_importe { get; set; }

        [DisplayName("Valor Iva")]
        public decimal valor_iva { get; set; }

        [DisplayName("Valor Total")]
        public decimal valor_total { get; set; }

        [DisplayName("Valor a Cobrar")]
        public decimal valor_a_cobrar { get; set; }

        [DisplayName("Código Sap")]
        public string numero_retencion { get; set; }

        [DisplayName("Retención Iva")]
        public decimal retencion_iva { get; set; }

        [DisplayName("Retención Ir")]
        public decimal retencion_ir { get; set; }

        [DisplayName("Documento Compensación")]
        public string doc_compensacion { get; set; }

        [DisplayName("Valor Cobrado")]
        public decimal valor_cobrado { get; set; }

        [DisplayName("Documento Pago")]
        public string documento_pago { get; set; }

        [DisplayName("Código Sap")]
        public string codigosap { get; set; }

        [Obligado]
        [ForeignKey("Catalogo")]
        [DisplayName("Estado")]
        public int estado { get; set; }

        public virtual Catalogo Catalogo { get; set; }

        [DisplayName("Orden de Servicio")]
        public string orden_servicio { get; set; }

        [DisplayName("OV")]
        public string ov { get; set; }

        [DisplayName("OS")]
        public string os { get; set; }


        [DefaultValue(true)]
        public bool vigente { get; set; } = true;
    }
}
