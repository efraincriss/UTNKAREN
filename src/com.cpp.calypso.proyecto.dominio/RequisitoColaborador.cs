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
    public class RequisitoColaborador : Entity, IFullAudited
    {
        
		[DisplayName("Tipo Usuario")]
        [ForeignKey("TipoGrupoPersonal")]
        public int tipo_usuarioId { get; set; }
        public virtual Catalogo TipoGrupoPersonal { get; set; }
        
		[DisplayName("Requisito")]
		public int RequisitosId { get; set; }
		public virtual Requisitos Requisitos { get; set; }

	    [Obligado]
		[DisplayName("Descripción")]
		public string descripcion { get; set; }

		[DisplayName("Acción")]
        [ForeignKey("Accion")]
        public int rolId { get; set; }
        public virtual Catalogo Accion { get; set; }

        [Obligado]
		[DisplayName("Obligatorio")]
		public bool obligatorio { get; set; }

		[Obligado]
		[DisplayName("Requiere Archivo")]
		public bool requiere_archivo { get; set; }
        
        [DisplayName("Tipo Ausentismo")]
        [ForeignKey("TipoAusentismo")]
        public int? catalogo_tipo_ausentismo_id { get; set; }
        public virtual Catalogo TipoAusentismo { get; set; }

        [DisplayName("Motivo de Baja")]
        [ForeignKey("MotivoBaja")]
        public int? catalogo_motivo_baja_id { get; set; }
        public virtual Catalogo MotivoBaja { get; set; }

        [Obligado]
		[DisplayName("Estado")]
		public bool vigente { get; set; } = true;

        [Obligado]
        [DisplayName("Activo")]
        public bool activo { get; set; } = true;

        public string GetObligatorio()
        {
            return obligatorio ? "SI" : "No";
        }

        public long? CreatorUserId { get; set; }
        public DateTime CreationTime { get; set; }
        public long? LastModifierUserId { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public long? DeleterUserId { get; set; }
        public DateTime? DeletionTime { get; set; }
        public bool IsDeleted { get; set; }
    }
}
