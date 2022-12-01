using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.dominio
{
public class FacturaExcel
    {
        public int id { get; set; }
        public string cuenta { get; set; }
        public string sociedad { get; set; }
        public string cliente { get; set; }
        public DateTime fecha_documento { get; set; }
        public string numero_documento { get; set; }
        public string referencia { get; set; }
        public string detalle { get; set; }
        public string clase_documento { get; set; }
        public string documento_compensacion { get; set; }
        public decimal importe_moneda { get; set; }
        public DateTime fecha_compensacion { get; set; }
        public DateTime fecha_pago { get; set; }
     
        public string orden_servicio { get; set; }
        public string ov { get; set; }
        public string os { get; set; }


    }
}

