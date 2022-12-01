using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.dominio.Documentos
{
    public class Carpeta : Entity, IFullAudited
    {
        [Required]
        [MaxLength(20)]
        public string Codigo { get; set; }

        [Required]
       // [MaxLength(100)]
        public string NombreCorto { get; set; }

        [Required]
       // [MaxLength(1000)]
        public string NombreCompleto { get; set; }

        [Required]
       // [MaxLength(3000)]
        public string Descripcion { get; set; }

        public Catalogo Estado { get; set; }

        public int EstadoId { get; set; }

        [Required]
        public bool Publicado { get; set; } = false;

        public List<Documento> Documentos { get; set; }

        public bool IsDeleted { get; set; }

        public long? CreatorUserId { get; set; }

        public DateTime CreationTime { get; set; }

        public long? LastModifierUserId { get; set; }

        public DateTime? LastModificationTime { get; set; }

        public long? DeleterUserId { get; set; }

        public DateTime? DeletionTime { get; set; }
    }
}
