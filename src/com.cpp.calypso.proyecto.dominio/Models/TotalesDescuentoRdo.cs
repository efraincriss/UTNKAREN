using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.dominio.Models
{
    public class TotalesDescuentoRdo
    {

        public decimal VALOR_COSTO_DIRECTO_OBRAS_CIVILES { get; set; }
        public decimal VALOR_COSTO_DIRECTO_OBRAS_MECANICAS { get; set; }
        public decimal VALOR_COSTO_DIRECTO_OBRAS_ELECTRICAS { get; set; }
        public decimal VALOR_COSTO_DIRECTO_OBRAS_INSTRUMENTO_Y_CONTROL { get; set; }
        public decimal VALOR_COSTO_DIRECTO_SERVICIOS_ESPECIALES { get; set; }


        public decimal D_VALOR_COSTO_DIRECTO_OBRAS_CIVILES { get; set; }
        public decimal D_VALOR_COSTO_DIRECTO_OBRAS_MECANICAS { get; set; }
        public decimal D_VALOR_COSTO_DIRECTO_OBRAS_ELECTRICAS { get; set; }
        public decimal D_VALOR_COSTO_DIRECTO_OBRAS_INSTRUMENTO_Y_CONTROL { get; set; }
        public decimal D_VALOR_COSTO_DIRECTO_SERVICIOS_ESPECIALES { get; set; }

        public decimal TOTAL_DESCUENTO_APLICADO { get; set; }

    }
}
