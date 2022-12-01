using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Entities.Auditing;
using com.cpp.calypso.proyecto.dominio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion.Dto
{
	[AutoMap(typeof(ColaboradoresComida))]
	[Serializable]
	public class ColaboradoresComidaDto : EntityDto
    {
		[DisplayName("Colaborador")]
		public int ColaboradorServicioId { get; set; }
		public virtual ColaboradorServicio ColaboradorServicio { get; set; }

		[DisplayName("Tipo comida")]
		public int tipo_alimentacion_id { get; set; }

        //public long? CreatorUserId { get; set; }
        //public DateTime CreationTime { get; set; }
        //public long? LastModifierUserId { get; set; }
        //public DateTime? LastModificationTime { get; set; }
        //public long? DeleterUserId { get; set; }
        //public DateTime? DeletionTime { get; set; }
        //public bool IsDeleted { get; set; }
    }
}
