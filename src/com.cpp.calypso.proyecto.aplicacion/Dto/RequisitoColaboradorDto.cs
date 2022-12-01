using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Entities.Auditing;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.dominio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion.Dto
{
    [AutoMap(typeof(RequisitoColaborador))]
    [Serializable]
    public class RequisitoColaboradorDto : EntityDto, IFullAudited
    {

		[Obligado]
		[DisplayName("Tipo Usuario")]
		public int tipo_usuarioId { get; set; }

		[Obligado]
		[DisplayName("Requisito")]
		public int RequisitosId { get; set; }
		public virtual Requisitos Requisitos { get; set; }

		[Obligado]
		[DisplayName("Descripción")]
		public string descripcion { get; set; }

		[DisplayName("Rol / Función")]
		[Obligado]
		public int rolId { get; set; }

		[Obligado]
		[DisplayName("Obligatorio")]
		public bool obligatorio { get; set; }

		[Obligado]
		[DisplayName("Requiere Archivo")]
		public bool requiere_archivo { get; set; }
        
        [DisplayName("Tipo Ausentismo")]
        public int? catalogo_tipo_ausentismo_id { get; set; }

        [Obligado]
		[DisplayName("Estado")]
		public bool vigente { get; set; } = true;
        
        [DisplayName("Activo")]
        public bool activo { get; set; } = true;

        public long? CreatorUserId { get; set; }
        public DateTime CreationTime { get; set; }
        public long? LastModifierUserId { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public long? DeleterUserId { get; set; }
        public DateTime? DeletionTime { get; set; }
        public bool IsDeleted { get; set; }

        public virtual string nombre_estado { get; set; }
        public virtual int nro { get; set; }
        public virtual string nombre_obligatorio { get; set; }
        public virtual string nombre_requiere { get; set; }
        public virtual string nombre_rol { get; set; }
        public virtual string nombre_accion { get; set; }
        public virtual string usuario { get; set; }
        public virtual string requisito { get; set; }

        public virtual string[] frentes { get; set; }

    }
}
