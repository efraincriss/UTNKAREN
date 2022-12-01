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
	[AutoMap(typeof(ColaboradorFormacionEducativa))]
	[Serializable]
	public class ColaboradorFormacionEducativaDto : EntityDto
    {
		[DisplayName("Colaborador")]
		public int ColaboradoresId { get; set; }
		public virtual Colaboradores Colaboradores { get; set; }

		[CanBeNull]
		[DisplayName("Formación")]
		public string formacion { get; set; }

		[CanBeNull]
		[DisplayName("Institución Educativa")]
		public string institucion_educativa { get; set; }

		[DisplayName("Título")]
		public int? catalogo_titulo_id { get; set; }

		[DataType(DataType.Date)]
		[DisplayName("Fecha Ingreso")]
		[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
		public DateTime? fecha_registro_senecyt { get; set; }

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
