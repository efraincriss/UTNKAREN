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
    public class ColaboradoresAusentismo : Entity, IFullAudited
    {
        [Obligado]
        [DisplayName("Colaborador")]
        [ForeignKey("Colaborador")]
        public int colaborador_id { get; set; }
        public virtual Colaboradores Colaborador { get; set; }

        [Obligado]
        [DisplayName("Tipo Ausentismo")]
        [ForeignKey("TipoAusentismo")]
        public int catalogo_tipo_ausentismo_id { get; set; }
        public virtual Catalogo TipoAusentismo { get; set; }

        [Obligado]
        [DataType(DataType.Date)]
        [DisplayName("Fecha Inicio")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? fecha_inicio { get; set; }

        [Obligado]
        [DataType(DataType.Date)]
        [DisplayName("Fecha Fin")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? fecha_fin { get; set; }

        [Obligado]
        [DisplayName("Estado")]
        public String estado { get; set; }

        [Obligado]
        [DisplayName("Vigencia")]
        public bool vigente { get; set; } = true;

        public string observacion { get; set; }

        public long? CreatorUserId { get; set; }
        public DateTime CreationTime { get; set; }
        public long? LastModifierUserId { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public long? DeleterUserId { get; set; }
        public DateTime? DeletionTime { get; set; }
        public bool IsDeleted { get; set; }
    }
}
