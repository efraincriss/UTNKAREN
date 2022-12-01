using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.dominio.Models
{
    public class MontosTotalesRDO
    {
        public decimal porcentajeBudget { get; set; }
        public decimal porcentajeEAC { get; set; }
        public decimal costoBudget { get; set; }
        public decimal costoEAC { get; set; }
        public decimal ac_anterior { get; set; }

        public decimal ac_diario { get; set; }
        public decimal ac_actual { get; set; }
        public decimal ev_diario { get; set; }
        public decimal ev_anterior { get; set; }
        public decimal ev_actual { get; set; }
        public decimal ern_value { get; set; }
        public decimal pv_costo_planificado { get; set; }
        public DateTime fecha_inicio_prevista { get; set; }
        public DateTime fecha_fin_prevista { get; set; }
        public DateTime? fecha_inicio_real { get; set; }
        public DateTime? fecha_fin_real{ get; set; }
        public decimal avance_Acumulado_Anterior { get; set; }
        public decimal avance_Diario { get; set; }
        public decimal avance_Actual_Acumulado { get; set; }
        public decimal Avance_Previsto_Acumulado { get; set; }

        public decimal spi { get; set; }
        public decimal cpi { get; set; }

        public decimal numero_horas_disruptivo { get; set; }

        public int plazo_previsto { get; set; }
        public decimal plazo_ajustado { get; set; }



    }
}
