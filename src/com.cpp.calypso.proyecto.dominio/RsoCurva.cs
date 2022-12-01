using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.dominio
{
    public class RsoCurva
    {
        public DateTime fecha { get; set; }
        public decimal valor_previsto { get; set; }
        public decimal valor_previsto_acumulado { get; set; }

        public decimal valor_real { get; set; }
        public decimal valor_real_acumulado { get; set; }

        public DateTime fecha_inicial { get; set; }
        public DateTime fecha_final { get; set; }
        public decimal costo_presupuesto { get; set; }
        public decimal monto_total_presupuestado { get; set; }
    }
}
