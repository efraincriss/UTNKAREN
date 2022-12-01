using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.dominio.Models
{
   public  class ReportVencimientosContrato
    {

        public string razon_social { get; set; }
        public string fechaInicio { get; set; }
        public string fechaFin { get; set; }
        public string codigo { get; set; }
        public string ordenCompra { get; set; }
        public int diasVencimiento { get; set; }
        public string estado { get; set; }


    }
}
