using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using com.cpp.calypso.comun.dominio;
using JetBrains.Annotations;
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
	public class ColaboradorRequisito : Entity, IFullAudited
    {
		[DisplayName("Colaborador")]
		public int ColaboradoresId { get; set; }
		public virtual Colaboradores Colaboradores { get; set; }

		[DisplayName("Requisito")]
		public int RequisitosId { get; set; }
		public virtual Requisitos Requisitos { get; set; }

		[DisplayName("Archivo")]
		public int? ArchivoId { get; set; }
        public virtual Archivo Archivo { get; set; }

        [DisplayName("Cumple")]
		public bool cumple { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Fecha Emision")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? fecha_emision { get; set; }

        [DataType(DataType.Date)]
		[DisplayName("Fecha Caducidad")]
		[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
		public DateTime? fecha_caducidad { get; set; }

        [CanBeNull]
        [DisplayName("Observacion")]
		public string observacion { get; set; }

		[Obligado]
		[DisplayName("Vigente")]
		public bool vigente { get; set; } = true;

        /*[DisplayName("Acción")]
        [ForeignKey("Accion")]
        public int catalogo_accion_id { get; set; }
        public virtual Catalogo Accion { get; set; }*/

        [Obligado]
        [DisplayName("Activo")]
        public bool activo { get; set; } = true;

        public string GetCumple()
        {
            return cumple ? "SI" : "No";
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
