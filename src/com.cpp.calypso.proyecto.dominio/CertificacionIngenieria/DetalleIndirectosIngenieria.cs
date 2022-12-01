using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.dominio.CertificacionIngenieria
{
    [Serializable]
    public class DetalleIndirectosIngenieria : Entity, IFullAudited
    {
        public DateTime FechaDesde { get; set; }

        public DateTime FechaHasta { get; set; }

        public int ColaboradorRubroId { get; set; }

        public ColaboradorRubroIngenieria ColaboradorRubro { get; set; }

        public decimal DiasLaborados { get; set; }

        public decimal HorasLaboradas { get; set; }

        public bool EsViatico { get; set; }

        public bool Certificado { get; set; }

        public bool IsDeleted { get; set; }

        public long? CreatorUserId { get; set; }

        public DateTime CreationTime { get; set; }

        public long? LastModifierUserId { get; set; }

        public DateTime? LastModificationTime { get; set; }

        public long? DeleterUserId { get; set; }

        public DateTime? DeletionTime { get; set; }

        public string distribucion_proyectos { get; set; }
        public decimal saldo { get; set; }

        public int? CertificadoId { get; set; }

       

    }


}

