using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.dominio.CertificacionIngenieria
{

    [Serializable]
    public class ComentarioCertificado : Entity, IFullAudited
    {
        [Required]
        public int ProyectoId { get; set; }

        public Proyecto Proyecto { get; set; }

        public int? CertificadoId { get; set; }
        public DateTime FechaCarga { get; set; }

     
        public string Comentario { get; set; }





        public bool IsDeleted { get; set; }

        public long? CreatorUserId { get; set; }

        public DateTime CreationTime { get; set; }

        public long? LastModifierUserId { get; set; }

        public DateTime? LastModificationTime { get; set; }

        public long? DeleterUserId { get; set; }

        public DateTime? DeletionTime { get; set; }
    }
}
