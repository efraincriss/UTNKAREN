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
    public class Capacitacion : Entity, IFullAudited
    {
        [Required]
        public int ColaboradoresId { get; set; }

        public Colaboradores Colaboradores { get; set; }

        [Required]
        public int CatalogoTipoCapacitacionId { get; set; }

        public Catalogo CatalogoTipoCapacitacion { get; set; }

        [Required]
        public decimal Horas { get; set; }

        [Required]
        public int CatalogoNombreCapacitacionId { get; set; }

        public Catalogo CatalogoNombreCapacitacion { get; set; }

        public string Observaciones { get; set; }

        public string Fuente { get; set; }

        public DateTime Fecha { get; set; }

        public DateTime CreationTime { get; set; }

        public long? CreatorUserId { get; set; }

        public DateTime? LastModificationTime { get; set; }

        public long? LastModifierUserId { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletionTime { get; set; }

        public long? DeleterUserId { get; set; }
    }
}
