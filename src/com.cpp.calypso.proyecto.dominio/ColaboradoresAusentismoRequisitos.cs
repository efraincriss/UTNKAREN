using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using com.cpp.calypso.comun.dominio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.dominio
{
    [Serializable]
    public class ColaboradoresAusentismoRequisitos : Entity, IFullAudited
    {
        [Obligado]
        [DisplayName("Colaborador")]
        [ForeignKey("ColaboradorAusentismo")]
        public int colaborador_ausentismo_id { get; set; }

        public virtual ColaboradoresAusentismo ColaboradorAusentismo { get; set; }

        [Obligado]
        [DisplayName("Requisito")]
        [ForeignKey("Requisitos")]
        public int requisito_id { get; set; }
        public virtual Requisitos Requisitos { get; set; }

        [DisplayName("Archivo")]
        [ForeignKey("Archivo")]
        public int? archivo_id { get; set; }
        public virtual Archivo Archivo { get; set; }

        [Obligado]
        [DisplayName("Cumple")]
        public bool cumple { get; set; } = true;

        public long? CreatorUserId { get; set; }
        public DateTime CreationTime { get; set; }
        public long? LastModifierUserId { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public long? DeleterUserId { get; set; }
        public DateTime? DeletionTime { get; set; }
        public bool IsDeleted { get; set; }
    }
}
