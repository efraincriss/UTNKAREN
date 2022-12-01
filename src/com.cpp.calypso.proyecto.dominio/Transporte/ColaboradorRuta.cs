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
    public class ColaboradorRuta : Entity, IFullAudited
    {
        [Obligado]
        [DisplayName("Colaborador")]
        public int ColaboradorId { get; set; }
        public Colaboradores Colaborador { get; set; }

        [Obligado]
        [DisplayName("Rutas Horarios")]
        public int RutaHorarioId { get; set; }
        public RutaHorario RutaHorario { get; set; }

        [DisplayName("Observacion")]
        [StringLength(100)]
        public string Observacion { get; set; }

        [DisplayName("Estado")]
        public ColaboradorRutaAsignada Estado { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Fecha Asignacion")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime FechaAsignacion { get; set; } = DateTime.Now;

        [DisplayName("Usuario Asignacion")]
        [StringLength(100)]
        public string UsuarioAsignacion { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Fecha DesAsignacion")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? FechaDesAsignacion { get; set; }

        [DisplayName("Usuario Asignacion")]
        [StringLength(100)]
        public string UsuarioDesAsignacion { get; set; }

        
        public long? CreatorUserId { get; set; }
        public DateTime CreationTime { get; set; }
        public long? LastModifierUserId { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public long? DeleterUserId { get; set; }
        public DateTime? DeletionTime { get; set; }
        public bool IsDeleted { get; set; }
    }

    public enum ColaboradorRutaAsignada
    {
        [Description("Asignado")]
        Asignado = 1,

        [Description("Quitado")]
        Quitado = 0,
    }
}
