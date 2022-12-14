using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using com.cpp.calypso.comun.dominio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.dominio.Transporte
    {
    [Serializable]
    public class RutaHorario : Entity, IFullAudited
    {

        [Obligado]
        [DisplayName("Ruta")]
        public int RutaId { get; set; }
        public Ruta Ruta { get; set; }

        [Obligado]
        [DisplayName("Horario")]
        public TimeSpan Horario { get; set; }

        public long? CreatorUserId { get; set; }
        public DateTime CreationTime { get; set; }
        public long? LastModifierUserId { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public long? DeleterUserId { get; set; }
        public DateTime? DeletionTime { get; set; }
        public bool IsDeleted { get; set; }
    }
}
