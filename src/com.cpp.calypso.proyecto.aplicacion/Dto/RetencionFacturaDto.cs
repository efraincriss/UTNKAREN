using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.dominio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion
{
    [AutoMap(typeof(RetencionFactura))]
    [Serializable]
   public class RetencionFacturaDto : EntityDto
    {


        [Obligado]
        [DisplayName("Factura")]
        public int FacturaId { get; set; }
        public virtual Factura Factura { get; set; }

        [Obligado]
        [ForeignKey("Catalogo")]
        [DisplayName("Tipo Retencion")]
        public int tipo_retencion { get; set; }

        public virtual Catalogo Catalogo { get; set; }


        [DisplayName("Descuento")]
        public decimal total { get; set; }


        [DisplayName("Porcentaje Retencion")]
        public decimal porcentaje_retencion { get; set; }


        [Obligado]
        [DefaultValue(true)]
        public bool vigente { get; set; }
    }
}
