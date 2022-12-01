using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Entities.Auditing;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.dominio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion.Dto
{
    [AutoMap(typeof(Requisitos))]
    [Serializable]
    public class RequisitosDto : EntityDto, IFullAudited
    {

        [DisplayName("Tipo Requisito")]
        public int requisitoId { get; set; }

        [Obligado]
        [LongitudMayor(10)]
        [DisplayName("Código")]
        public string codigo { get; set; }

        [Obligado]
		[DisplayName("Nombre")]
		public string nombre { get; set; }

		[Obligado]
		[DisplayName("Descripción")]
		public string descripcion { get; set; }

		[Obligado]
        [DisplayName("Responsable de requisito")]
        public int responsableId { get; set; }

        [DisplayName("Aplica caducidad")]
		public bool? caducidad { get; set; }

		[DisplayName("Tiempo de vigencia (días)")]
		public int? tiempo_vigencia { get; set; }

        [DisplayName("Inicio Alerta (días)")]
        public int? dias_inicio_alerta { get; set; }

        [Obligado]
		[DisplayName("Estado (Activo/Inactivo)")]
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

        public bool genera_notificacion { get; set; } = false;
        public int? CatalogoFrecuenciaNotificacionId { get; set; }
        public int? tiempo_inicio_notificacion { get; set; }
        public int? dia_envio_notificacion { get; set; }


        public virtual string nombre_estado { get; set; }

        public virtual string nombre_caducidad { get; set; }

        public virtual string nombre_requisito { get; set; }
        public virtual string nombre_activo { get; set; }

        public virtual int nro { get; set; }

    }
}
