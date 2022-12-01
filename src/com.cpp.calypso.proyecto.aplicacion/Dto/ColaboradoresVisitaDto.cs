using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Entities.Auditing;
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
    [AutoMap(typeof(ColaboradoresVisita))]
    [Serializable]
    public class ColaboradoresVisitaDto : EntityDto, IFullAudited
    {
        [DisplayName("Colaborador")]
        [ForeignKey("Colaboradores")]
        public int? ColaboradoresId { get; set; }
        public virtual Colaboradores Colaboradores { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Fecha Desde")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? fecha_desde { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Fecha Hasta")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? fecha_hasta { get; set; }

        [DisplayName("Colaborador")]
        [ForeignKey("ColaboradoresResponsable")]
        public int? colaborador_responsable_id { get; set; }
        public virtual Colaboradores ColaboradoresResponsable { get; set; }

        [DisplayName("Motivo")]
        public string motivo { get; set; }

        [DisplayName("Estado")]
        public string estado { get; set; }

        [DisplayName("Vigente")]
        public bool vigente { get; set; } = true;

        [DisplayName("Empresa")]
        public string empresa { get; set; }

        public long? CreatorUserId { get; set; }
        public DateTime CreationTime { get; set; }
        public long? LastModifierUserId { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public long? DeleterUserId { get; set; }
        public DateTime? DeletionTime { get; set; }
        public bool IsDeleted { get; set; }

        public virtual int destino { get; set; }
        public virtual int nro { get; set; }

        public virtual string apellidos_nombres { get; set; }
        public virtual string nombres_apellidos { get; set; }
        public virtual string nombre_identificacion { get; set; }
        public virtual string nombres { get; set; }
        public virtual string numero_identificacion { get; set; }
        public virtual string estado_colaborador { get; set; }
        public virtual string nombreTipo{ get; set; }
    }
}
