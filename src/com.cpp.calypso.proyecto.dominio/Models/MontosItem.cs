using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.dominio.Models
{
    public class MontosItem
    {
        public bool success { get; set; }
        public decimal monto_ingenieria { get; set; } = 0;
        public decimal monto_contruccion { get; set; } = 0;
        public decimal monto_suministros { get; set; } = 0;
        public decimal monto_subcontratos{ get; set; } = 0;
        public decimal monto_total { get; set; } = 0;
    }
}
