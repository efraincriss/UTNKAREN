using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Entities.Auditing;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.dominio;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion.Dto
{
    [AutoMap(typeof(RequisitoServicio))]
    [Serializable]
    public class RequisitoServicioDto : EntityDto, IFullAudited
    {
		[CanBeNull]
		[DisplayName("Proveedor")]
		public int? ProveedorId { get; set; }

		[CanBeNull]
		[DisplayName("Tipo Servicio")]
		public int catalogo_servicio_id { get; set; }

        public string servicio_nombre { get; set; }

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

        public virtual string nombre_estado { get; set; }
        public virtual int nro { get; set; }
        public virtual string servicio { get; set; }
        public virtual string requisito { get; set; }
        public virtual string nombre_obligatorio { get; set; }

    }
}
