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
    [Serializable]
    public class Documento : Entity, IFullAudited
    {
        [Required]
        public string Codigo { get; set; }

        [Required]
        public string Nombre { get; set; }

        public Carpeta Carpeta { get; set; }

        [Required]
        public int CarpetaId { get; set; }

        [Required]
        public int CantidadPaginas { get; set; }

        [Required]
        public int TipoDocumentoId { get; set; }

        public Catalogo TipoDocumento { get; set; }

        [Required]
        public bool EsImagen { get; set; }

        public string Imagen { get; set; }

        public List<Seccion> Secciones { get; set; }

        public int? DocumentoPadreId { get; set; }

        public Documento DocumentoPadre { get; set; }

        public int orden { get; set; } = 1;

        public bool IsDeleted { get; set; }

        public long? CreatorUserId { get; set; }

        public DateTime CreationTime { get; set; }

        public long? LastModifierUserId { get; set; }

        public DateTime? LastModificationTime { get; set; }

        public long? DeleterUserId { get; set; }

        public DateTime? DeletionTime { get; set; }
    }
}
