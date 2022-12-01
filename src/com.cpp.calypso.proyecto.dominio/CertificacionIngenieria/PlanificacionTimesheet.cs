using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.dominio.CertificacionIngenieria
{
    [Serializable]
    public class PlanificacionTimesheet : Entity, IFullAudited
    {
        public DateTime Fecha { get; set; }

        public TipoPlanificacion TipoPlanificacion { get; set; }

        public string Descripcion { get; set; }

        public bool IsDeleted { get; set; }

        public long? CreatorUserId { get; set; }

        public DateTime CreationTime { get; set; }

        public long? LastModifierUserId { get; set; }

        public DateTime? LastModificationTime { get; set; }

        public long? DeleterUserId { get; set; }

        public DateTime? DeletionTime { get; set; }
    }

    public enum TipoPlanificacion
    {
        CorteIngenieria,
        EnvioTsParaSr,
        RsIngenieria,
        CorteIngenieriaMensual,
        RsIngenieriaCsMensual,
        Certificacion
    };
}
