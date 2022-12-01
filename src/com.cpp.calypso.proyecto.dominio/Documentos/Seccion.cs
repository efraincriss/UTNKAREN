using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using com.cpp.calypso.comun.dominio;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.dominio.Documentos
{
    public class Seccion : Entity, IFullAudited
    {
        [Required]

        public string NombreSeccion { get; set; }

        public string Codigo { get; set; }

        public string Contenido { get; set; }

        public string Contenido_Plano { get; set; }

        public Documento Documento { get; set; }

        public int DocumentoId { get; set; }

        public Seccion SeccionPadre { get; set; }

        public int? SeccionPadreId { get; set; }

        [Required]
        [MaxLength(10)]
        public string NumeroPagina { get; set; }

        public int Ordinal { get; set; }

        public bool IsDeleted { get; set; }

        public long? CreatorUserId { get; set; }

        public DateTime CreationTime { get; set; }

        public long? LastModifierUserId { get; set; }

        public DateTime? LastModificationTime { get; set; }

        public long? DeleterUserId { get; set; }

        public DateTime? DeletionTime { get; set; }
    }
}
