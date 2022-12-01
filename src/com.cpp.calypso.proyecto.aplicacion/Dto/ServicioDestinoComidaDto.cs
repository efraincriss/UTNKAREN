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
	[AutoMap(typeof(ServicioDestinoComida))]
	[Serializable]
	public class ServicioDestinoComidaDto: EntityDto
    {
		[Obligado]
		[DisplayName("Servicio Destino")]
		public int ServicioDestinoId { get; set; }
		public virtual ServicioDestino ServicioDestino { get; set; }

		[Obligado]
		[DisplayName("Tipo comida")]
		public int tipo_comida { get; set; }
		
		[Obligado]
		[DisplayName("Estado")]
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
