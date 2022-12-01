using Abp.Domain.Entities;
using com.cpp.calypso.comun.dominio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.dominio
{
    [Serializable]
   public class CobroFactura: Entity
    {

        [Obligado]
        [DisplayName("Factura")]
        public int FacturaId { get; set; }
        public virtual Factura Factura { get; set; }

        [Obligado]
        [DisplayName("Cobro")]
        public int CobroId { get; set; }
        public virtual Cobro Cobro { get; set; }


        [DisplayName("Monto")]
        public decimal monto { get; set; }

        [Obligado]
        public bool vigente { get; set; } = true;


    }
}
