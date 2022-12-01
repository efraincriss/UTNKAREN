using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using com.cpp.calypso.comun.dominio;
using JetBrains.Annotations;
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
	public class ColaboradorDiscapacidad : Entity
    {
		[CanBeNull]
		[DisplayName("Colaborador")]
		public int? ColaboradoresId { get; set; }
		public virtual Colaboradores Colaboradores { get; set; }

		[CanBeNull]
		[DisplayName("Carga Social")]
		public int? ColaboradorCargaSocialId { get; set; }
		public virtual ColaboradorCargaSocial ColaboradorCargaSocial { get; set; }

		[DisplayName("Tipo Discapacidad")]
		[ForeignKey("CatalogoTipoDiscapacidad")]
		public int catalogo_tipo_discapacidad_id { get; set; }
        public Catalogo CatalogoTipoDiscapacidad { get; set; }

		[DisplayName("Catálogo Porcentaje")]
		public int catalogo_porcentaje_id { get; set; }

		[Obligado]
		public bool vigente { get; set; } = true;

        //public long? CreatorUserId { get; set; }
        //public DateTime CreationTime { get; set; }
        //public long? LastModifierUserId { get; set; }
        //public DateTime? LastModificationTime { get; set; }
        //public long? DeleterUserId { get; set; }
        //public DateTime? DeletionTime { get; set; }
        //public bool IsDeleted { get; set; }
    }
}
