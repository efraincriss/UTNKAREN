using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.dominio.Models
{
    public class RdoDatos
    {
        public string UM { get; set; }

        public string tipo { get; set; }


        public string codigo_preciario { get; set; }

        public string nombre_actividad { get; set; }

        public decimal porcentaje_presupuesto_total { get; set; }

        public decimal porcentaje_costo_eac_total { get; set; }

        public Decimal presupuesto_total { get; set; }

        public decimal cantidad_planificada { get; set; }

        public decimal cantidad_eac { get; set; }

        public Decimal precio_unitario { get; set; }

        public Decimal costo_presupuesto { get; set; }

        public Decimal costo_eac { get; set; }

        public Decimal ac_anterior { get; set; }

        public Decimal ac_diario { get; set; }

        public Decimal ac_actual { get; set; }

        public Decimal ev_anterior { get; set; }

        public Decimal ev_diario { get; set; }

        public Decimal ev_actual { get; set; }

        public Decimal ern_value { get; set; }

        public Decimal pv_costo_planificado { get; set; }

        public DateTime? fecha_inicio_prevista { get; set; }

        public DateTime? fecha_fin_prevista { get; set; }

        public DateTime? fecha_inicio_real { get; set; }

        public DateTime? fecha_fin_real { get; set; }

        public decimal cantidad_anterior { get; set; }

        public decimal cantidad_diaria { get; set; }

        public decimal cantidad_acumulada { get; set; }

        public decimal porcentaje_avance_anterior { get; set; }

        public decimal porcentaje_avance_diario { get; set; }

        public decimal porcentaje_avance_actual_acumulado { get; set; }

        public decimal porcentaje_avance_previsto_acumulado { get; set; }

        public bool ItemsPresupuestados { get; set; }

        public string CodigoNivelWbs { get; set; }

        public int OfertaId { get; set; }

        public string color { get; set; }
        public virtual string codigo_especialidad { get; set; }
        public virtual string codigo_grupo { get; set; }

        public bool principal { get; set; }
        public bool es_temporal { get; set; }

        /* Costo Budget Version Anterior*/
        public decimal costo_budget_version_anterior { get; set; }
        public decimal ev_actual_version_anterior { get; set; }

        public string id_rubro { get; set; }

        public string codigo_ordenamiento { get; set; }

        public int computoId { get; set; } 

        public decimal costo_eac_descuento { get; set; }

        public decimal ac_actual_descuento { get; set; }
    }


}
