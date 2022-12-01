using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using com.cpp.calypso.proyecto.dominio.CertificacionIngenieria;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.dominio
{
    [Serializable]

    public class ResumenCertificacion : Entity, IFullAudited
    {
        
        [Required]
        public int GrupoCertificadoIngenieriaId { get; set; }

        public GrupoCertificadoIngenieria GrupoCertificadoIngenieria { get; set; }
        
        [Required]
        public int ProyectoId { get; set; }

        public Proyecto Proyecto { get; set; }

        public string Descripcion { get; set; }

        public decimal TotalEjecutadoHH { get; set; }
        public decimal TotalEjecutadoUSD { get; set; }
        public string CLASE { get; set; }

        public decimal USD_BDG { get; set; }
        public decimal HH_BDG { get; set; }

        public string N_Oferta { get; set; }


        public decimal USD_AB { get; set; }
        public decimal HH_AB { get; set; }


        public decimal EAC_USD { get; set; }

        public decimal EAC_HH { get; set; }

        public decimal TOTAL_PREVISTO { get; set; }
        public decimal IB_PREVISTO { get; set; }
        public decimal ID_PREVISTO { get; set; }

        public decimal TOTAL_REAL { get; set; }

        public decimal IB_REAL { get; set; }
        public decimal ID_REAL { get; set; }

        public decimal AB_REAL { get; set; }

        public decimal PORCENTAJE_AVANCE_FÍSICO_PREVISTO_IB_ID_AB { get; set; }

        public decimal PORCENTAJE_AVANCE_FÍSICO_REAL_IB_ID_AB { get; set; }

        public decimal DESVIO_BDG_EAC_USD{ get; set; }

        public decimal HH_DISPONIBLES { get; set; }

        public decimal PLANNED_VALUE_PV_ { get; set; }
        public decimal EARN_VALUE_EV { get; set; }
        public decimal ACTUAL_COST_AC { get; set; }
        public decimal SPI { get; set; }

        public decimal CPI { get; set; }

        public string Comentarios { get; set; }

        public int anioProyecto { get; set; } = DateTime.Now.Year;

        public bool unaFase { get; set; } = false;
        public bool IsDeleted { get; set; }

        public long? CreatorUserId { get; set; }

        public DateTime CreationTime { get; set; }

        public long? LastModifierUserId { get; set; }

        public DateTime? LastModificationTime { get; set; }

        public long? DeleterUserId { get; set; }

        public DateTime? DeletionTime { get; set; }
    }
}
