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
    [AutoMap(typeof(AdminRotacion))]
    [Serializable]
    public class AdminRotacionDto : EntityDto, IFullAudited
    {
        [Obligado]
        [LongitudMayor(10)]
        [DisplayName("Código")]
        public string codigo { get; set; }

        [Obligado]
        [DisplayName("Nombre")]
        public string nombre { get; set; }

        [Obligado]
        [DisplayName("Días laborables en campo")]
        public int dias_laborablesC { get; set; }

        [Obligado]
        [DisplayName("Días laborables en oficina")]
        public int dias_laborablesO { get; set; }

        [Obligado]
        [DisplayName("Días de descanso")]
        public int dias_descanso { get; set; }

        [Obligado]
        [DisplayName("Estado")]
        public bool vigente { get; set; } = true;

		public virtual string nombre_estado { get; set; }
        public virtual int nro { get; set; }


        public long? CreatorUserId { get; set; }
        public DateTime CreationTime { get; set; }
        public long? LastModifierUserId { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public long? DeleterUserId { get; set; }
        public DateTime? DeletionTime { get; set; }
        public bool IsDeleted { get; set; }
    }
}
