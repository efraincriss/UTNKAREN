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
    public class Habitacion : Entity, IFullAudited
    {
        [Obligado]
        [DisplayName("Proveedor")]
        public int ProveedorId { get; set; }
        public Proveedor Proveedor { get; set; }

        [Obligado]
        [DisplayName("Número de Habitación")]
        [StringLength(50)]
        public string numero_habitacion { get; set; }


        [Obligado]
        [DisplayName("Tipo de Habitación")]
        public int TipoHabitacionId { get; set; }
        public Catalogo TipoHabitacion { get; set; }


        [Obligado]
        [DisplayName("Capacidad")]
        public int capacidad { get; set; }


        [Obligado]
        [DisplayName("Estado")]
        public bool estado { get; set; }


        [Obligado]
        [DisplayName("Aprobado")]
        public bool aprobado { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Fecha de Aprobación")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? fecha_aprobacion { get; set; }

        public string GetEstadoNombre()
        {
            return this.estado ? "ACTIVO" : "INACTIVO";
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
