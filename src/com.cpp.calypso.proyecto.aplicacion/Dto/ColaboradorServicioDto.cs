using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Entities.Auditing;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.dominio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion.Dto
{
	[AutoMap(typeof(ColaboradorServicio))]
	[Serializable]
	public class ColaboradorServicioDto : EntityDto
    {
		[DisplayName("Colaborador")]
		public int ColaboradoresId { get; set; }
		public virtual Colaboradores Colaboradores { get; set; }

		[DisplayName("Servicio")]
        [ForeignKey("Catalogo")]
        public int ServicioId { get; set; }
        public virtual Catalogo Catalogo { get; set; }

        [Obligado]
		public bool vigente { get; set; } = true;

        public virtual string nombre { get; set; }

        //public long? CreatorUserId { get; set; }
        //public DateTime CreationTime { get; set; }
        //public long? LastModifierUserId { get; set; }
        //public DateTime? LastModificationTime { get; set; }
        //public long? DeleterUserId { get; set; }
        //public DateTime? DeletionTime { get; set; }
        //public bool IsDeleted { get; set; }
    }
}
