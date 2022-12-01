using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using com.cpp.calypso.comun.dominio;

namespace com.cpp.calypso.proyecto.dominio.Proveedor
{
    [Serializable]
    public class EspacioHabitacion : Entity, IFullAudited
    {
        [Obligado]
        [DisplayName("Habitación")]
        public int HabitacionId { get; set; }
        public Habitacion Habitacion { get; set; }


        [Obligado]
        [DisplayName("Código del Espacio")]
        [StringLength(50)]
        public string codigo_espacio { get; set; }


        [Obligado]
        [DisplayName("Estado")]
        public bool estado { get; set; }


        [Obligado] [DisplayName("Activo")] public bool activo { get; set; } = true;

        public string GetEstadoNombre()
        {
            return this.estado ? "Libre" : "Ocupado";
        }

        public string GetActivoNombre()
        {
            return this.activo ? "Activo" : "Inactivo";
        }

        public string GetHabitacionEspacioCodigo()
        {
            return this.Habitacion.numero_habitacion + " - " + this.codigo_espacio;
        }

        public DateTime CreationTime { get; set; }

        public long? CreatorUserId { get; set; }

        public DateTime? LastModificationTime { get; set; }

        public long? LastModifierUserId { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletionTime { get; set; }

        public long? DeleterUserId { get; set; }
    }
}
