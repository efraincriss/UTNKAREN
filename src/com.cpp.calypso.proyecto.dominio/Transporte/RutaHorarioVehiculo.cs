using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using com.cpp.calypso.comun.dominio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.dominio.Transporte
{
    [Serializable]
    public class RutaHorarioVehiculo : Entity, IFullAudited
    {

        [Obligado]
        [DisplayName("Ruta")]
        public int RutaHorarioId { get; set; }
        public RutaHorario RutaHorario { get; set; }


        [Obligado]
        [DisplayName("Vehiculo")]
        public int VehiculoId { get; set; }
        public Vehiculo Vehiculo { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Fecha Desde")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime FechaDesde { get; set; } = DateTime.Now;

        [DataType(DataType.Date)]
        [DisplayName("Fecha Hasta")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime FechaHasta { get; set; } = DateTime.Now;

        [Obligado]
        [DisplayName("Horario")]
        public TimeSpan HorarioSalida { get; set; }

        [Obligado]
        [DisplayName("Duración")]
        public int Duracion { get; set; }

        [Obligado]
        [DisplayName("Horario")]
        public TimeSpan HoraLlegada { get; set; }


        [DisplayName("Observacion")]
        [StringLength(100)]
        public string Observacion { get; set; }

        [Obligado]
        [DisplayName("Estado")]
        public EstadoAsignacionVehiculo Estado { get; set; }


        public long? CreatorUserId { get; set; }
        public DateTime CreationTime { get; set; }
        public long? LastModifierUserId { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public long? DeleterUserId { get; set; }
        public DateTime? DeletionTime { get; set; }
        public bool IsDeleted { get; set; }
    }
    public enum EstadoAsignacionVehiculo
    {
        [Description("Asignado")]
        Asignado = 1,

        [Description("Quitado")]
        Quitado = 0,
    }

}
