using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace com.cpp.calypso.proyecto.dominio
{


    [Serializable]
    public class Katz : Entity, IFullAudited

    {
 

        [Required]
        public int PacienteId { get; set; }

        public Paciente Paciente { get; set; }

        public bool Bano { get; set; }
        public bool Vestido { get; set; }

        public bool Sanitario { get; set; }

        public bool Transferencias { get; set; }
        public bool Continencia { get; set; }
        public bool Alimentacion { get; set; }

        public string Calificacion { get; set; }
        public decimal Puntuacion { get; set; }


        public bool IsDeleted { get; set; }

        public long? CreatorUserId { get; set; }

        public DateTime CreationTime { get; set; }

        public long? LastModifierUserId { get; set; }

        public DateTime? LastModificationTime { get; set; }

        public long? DeleterUserId { get; set; }

        public DateTime? DeletionTime { get; set; }
    }

}
