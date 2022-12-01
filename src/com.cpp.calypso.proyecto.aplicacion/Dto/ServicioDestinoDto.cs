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
	[AutoMap(typeof(ServicioDestino))]
	[Serializable]
	public class ServicioDestinoDto : EntityDto
    {
		[Obligado]
		[DisplayName("Destino")]
		public int destinoId { get; set; }

		[Obligado]
		[DisplayName("Alojamiento")]
		public bool alojamiento { get; set; }

		[Obligado]
		[DisplayName("Lavandería")]
		public bool lavanderia { get; set; }

		[Obligado]
		[DisplayName("Movilización")]
		public bool movilizacion { get; set; }

		[Obligado]
		[DisplayName("Alimentación")]
		public bool alimentacion { get; set; }

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

        public virtual int nro { get; set; }
		public virtual string[] comidas { get; set; }
		public virtual string destino { get; set; }
		public virtual string nombre_alojamiento { get; set; }
		public virtual string nombre_movilizacion { get; set; }
		public virtual string nombre_lavanderia { get; set; }
		public virtual string nombre_alimentacion { get; set; }
	}
}
