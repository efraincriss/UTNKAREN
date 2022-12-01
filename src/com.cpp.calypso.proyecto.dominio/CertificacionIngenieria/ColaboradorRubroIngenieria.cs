using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.dominio.CertificacionIngenieria
{
    [Serializable]
    public class ColaboradorRubroIngenieria : Entity, IFullAudited
    {
        [Required]
        public int ContratoId { get; set; }

        public Contrato Contrato { get; set; }

        [Required]
        public int ColaboradorId { get; set; }

        public Colaboradores Colaborador { get; set; }

        [Required]
        public int RubroId { get; set; }

        public DetallePreciario Rubro { get; set; }

        public decimal Tarifa { get; set; }

        public DateTime FechaInicio { get; set; }

        public DateTime? FechaFin { get; set; }

        public bool IsDeleted { get; set; }

        public long? CreatorUserId { get; set; }

        public DateTime CreationTime { get; set; }

        public long? LastModifierUserId { get; set; }

        public DateTime? LastModificationTime { get; set; }

        public long? DeleterUserId { get; set; }

        public DateTime? DeletionTime { get; set; }
    }
}
