using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using com.cpp.calypso.comun.dominio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.dominio
{
    [Serializable]
    public class ColaboradoresAusentismoReintegros : Entity, IFullAudited
    {
        
        [DisplayName("Archivo")]
        [ForeignKey("ArchivoId")]
        public int? archivo_id { get; set; }
        public virtual Archivo ArchivoId { get; set; }
        
        [DisplayName("Ausentismo")]
        [ForeignKey("ColaboradoresAusentismo")]
        public int colaborador_ausentismo_id { get; set; }
        public virtual ColaboradoresAusentismo ColaboradoresAusentismo { get; set; }

        [Obligado]
        [DataType(DataType.Date)]
        [DisplayName("Fecha de Reintegro")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? fecha_reintegro { get; set; }

        [Obligado]
        [DisplayName("Motivo")]
        public String motivo_reintegro { get; set; }

        [Obligado]
        [DisplayName("Vigencia")]
        public bool vigente { get; set; } = true;

        public long? CreatorUserId { get; set; }
        public DateTime CreationTime { get; set; }
        public long? LastModifierUserId { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public long? DeleterUserId { get; set; }
        public DateTime? DeletionTime { get; set; }
        public bool IsDeleted { get; set; }
    }
}
