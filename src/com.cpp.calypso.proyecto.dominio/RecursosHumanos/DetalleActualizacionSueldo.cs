using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;

namespace com.cpp.calypso.proyecto.dominio.RecursosHumanos
{
    [Serializable]
    public class DetalleActualizacionSueldo : Entity, IFullAudited
    {
        [Required]
        public int CategoriaEncargadoId { get; set; }

        public CategoriasEncargado CategoriaEncargado { get; set; }

        [Required]
        public int ActualizacionSueldoId { get; set; }

        public ActualizacionSueldo ActualizacionSueldo { get; set; }

        [Required]
        public decimal ValorSueldo { get; set; }

        public DateTime CreationTime { get; set; }

        public long? CreatorUserId { get; set; }

        public DateTime? LastModificationTime { get; set; }

        public long? LastModifierUserId { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletionTime { get; set; }

        public long? DeleterUserId { get; set; }
    }
}
