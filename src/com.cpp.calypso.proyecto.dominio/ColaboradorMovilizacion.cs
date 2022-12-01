using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.dominio
{
    [Serializable]
    public class ColaboradorMovilizacion: Entity
    {
        [DisplayName("Colaborador")]
        public int ColaboradorServicioId { get; set; }
        public virtual ColaboradorServicio ColaboradorServicio { get; set; }

        [DisplayName("Parroquia")]
        public int? ParroquiaId { get; set; }
        public virtual Parroquia Parroquia { get; set; }

        [DisplayName("Comunidad")]
        public int? ComunidadId { get; set; }
        public virtual Comunidad Comunidad { get; set; }

        [DisplayName("Tipo comida")]
        public int catalogo_tipo_movilizacion_id { get; set; }

        //public long? CreatorUserId { get; set; }
        //public DateTime CreationTime { get; set; }
        //public long? LastModifierUserId { get; set; }
        //public DateTime? LastModificationTime { get; set; }
        //public long? DeleterUserId { get; set; }
        //public DateTime? DeletionTime { get; set; }
        //public bool IsDeleted { get; set; }
    }
}
