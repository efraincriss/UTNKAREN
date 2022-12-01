using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.dominio.CertificacionIngenieria
{
    [Serializable]
    public class GrupoCertificadoIngenieria : Entity, IFullAudited
    {

        [Required]
        public int ClienteId { get; set; }

        public Cliente Cliente { get; set; }


        public DateTime FechaCertificado { get; set; }

        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }

        public DateTime FechaGeneracion { get; set; } = DateTime.Now.Date;
        public EstadoGrupoCertificado EstadoId{ get; set; }

        public int Mes { get; set; }
        public int Anio { get; set; }

        public bool IsDeleted { get; set; }

        public long? CreatorUserId { get; set; }

        public DateTime CreationTime { get; set; }

        public long? LastModifierUserId { get; set; }

        public DateTime? LastModificationTime { get; set; }

        public long? DeleterUserId { get; set; }

        public DateTime? DeletionTime { get; set; }
    }
    public enum EstadoGrupoCertificado
    {
        [Description("Generado")]
        Generado = 0,

        [Description("Aprobado")]
        Aprobado = 1,

    }

}