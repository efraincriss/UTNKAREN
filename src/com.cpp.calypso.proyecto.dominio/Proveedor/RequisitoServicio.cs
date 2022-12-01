using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using com.cpp.calypso.comun.dominio;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.dominio
{
    [Serializable]
    public class RequisitoServicio : Entity, IFullAudited
    {
		[CanBeNull]
		[DisplayName("Proveedor")]
		public int? ProveedorId { get; set; }

		[CanBeNull]
		[DisplayName("Tipo Servicio")]
		public int catalogo_servicio_id { get; set; }

        [CanBeNull]
		[DisplayName("Requisito")]
		public int RequisitosId { get; set; }
		//public virtual Requisitos Requisitos { get; set; }

		[DisplayName("Obligatorio")]
		public bool? obligatorio { get; set; }

		[Obligado]
		[DisplayName("Estado")]
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
