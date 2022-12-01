
using Abp.Domain.Entities;
using com.cpp.calypso.comun.dominio;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.dominio
{
    [Serializable]
    public class RdoDetalle :Entity
    {
        [DisplayName("Unidad de Medida")]
        public string  UM { get; set; }

        public int  WbsId { get; set; }

        [Obligado]
        [DisplayName("Rdo Cabecera")]
        public int RdoCabeceraId { get; set; }

        public virtual RdoCabecera RdoCabecera { get; set; }

        [Obligado]
        [DisplayName("Computo")]
        public int ComputoId { get; set; }

        public virtual Computo Computo { get; set; }

        [Obligado]
        [DisplayName("Item")]
        public int ItemId { get; set; }

        public virtual Item Item { get; set; }

        [Obligado]
        [DisplayName("Código Preciario")]
        public string codigo_preciario { get; set; }

        [Obligado]
        [DisplayName("Nombre Actividad")]
        public string nombre_actividad { get; set; }

        [Obligado]
        [DisplayName("Porcentaje Presupuesto Total")]
        public decimal porcentaje_presupuesto_total { get; set; }


        [Obligado]
        [DisplayName("Porcentaje Costo eac Total")]
        public decimal porcentaje_costo_eac_total { get; set; }

        [Obligado]
        [DisplayName("Presupuesto Total")]
        public Decimal presupuesto_total { get; set; }

        [Obligado]
        [DisplayName("Cantidad Planificada")]
        public decimal cantidad_planificada { get; set; }

        [Obligado]
        [DisplayName("Cantidad EAC")]
        public decimal cantidad_eac { get; set; }

        [Obligado]
        [DisplayName("Precio Unitario")]
        public Decimal precio_unitario { get; set; }

        [Obligado]
        [DisplayName("Costo Presupuesto")]
        public Decimal costo_presupuesto { get; set; }

        [Obligado]
        [DisplayName("Costo EAC")]
        public Decimal costo_eac { get; set; }

        [Obligado]
        [DisplayName("Ac Anterior")]
        public Decimal ac_anterior { get; set; }

        [Obligado]
        [DisplayName("Ac Diario")]
        public Decimal ac_diario { get; set; }

        [Obligado]
        [DisplayName("Ac Actual")]
        public Decimal ac_actual { get; set; }

        [Obligado]
        [DisplayName("Ev Anterior")]
        public Decimal ev_anterior { get; set; }

        [Obligado]
        [DisplayName("Ev Diario")]
        public Decimal ev_diario { get; set; }

        [Obligado]
        [DisplayName("Ev Actual")]
        public Decimal ev_actual { get; set; }

        [DisplayName("Ev Actual")]
        public Decimal ern_value { get; set; } // actualice

        [Obligado]
        [DisplayName("PV Costo Planificado")]
        public Decimal pv_costo_planificado { get; set; }

        [DisplayName("Fecha Inicio Prevista")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? fecha_inicio_prevista { get; set; }

        [DisplayName("Fecha Fin Prevista")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? fecha_fin_prevista { get; set; }


        [DisplayName("Fecha Inicio Real")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? fecha_inicio_real { get; set; }


        [DisplayName("Fecha Fin Real")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? fecha_fin_real { get; set; }

        [Obligado]
        [DisplayName("Cantidad Anterior")]
        public decimal cantidad_anterior { get; set; }

        [Obligado]
        [DisplayName("Cantidad Diaria")]
        public decimal cantidad_diaria { get; set; }

        [Obligado]
        [DisplayName("Cantidad Acumulada")]
        public decimal cantidad_acumulada { get; set; }

        [Obligado]
        [DisplayName("Porcentaje Avance Anterior")]
        public Decimal porcentaje_avance_anterior { get; set; }

        [Obligado]
        [DisplayName("Porcentaje Avance Diario")]
        public Decimal porcentaje_avance_diario { get; set; }

        [Obligado]
        [DisplayName("Porcentaje Avance Actual Acumulado")]
        public Decimal porcentaje_avance_actual_acumulado { get; set; }

        [Obligado]
        [DisplayName("Porcentaje Avance Previsto Acumulado")]
        public Decimal porcentaje_avance_previsto_acumulado { get; set; }

        [Obligado]
        [DefaultValue(true)]
        public bool vigente { get; set; }

       
    }
}
