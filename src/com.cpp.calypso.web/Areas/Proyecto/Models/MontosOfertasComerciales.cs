using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace com.cpp.calypso.web.Areas.Proyecto.Models
{
    public class MontosOfertasComerciales
    {
        public decimal monto_ofertado { get; set; }
        public decimal monto_aprobado { get; set; }

        public decimal monto_pendiente_aprobacion { get; set; }


        public decimal monto_ingenieria { get; set; }
        public decimal monto_construccion { get; set; }
        public decimal monto_suminitros { get; set; }
        public decimal monto_subcontratos { get; set; }
        public decimal monto_total_os { get; set; }
    }
}