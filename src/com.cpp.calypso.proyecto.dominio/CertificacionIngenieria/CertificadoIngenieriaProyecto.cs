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
    public class CertificadoIngenieriaProyecto : Entity, IFullAudited
    {


        [Required]
        public int GrupoCertificadoIngenieriaId { get; set; }

        public GrupoCertificadoIngenieria GrupoCertificadoIngenieria { get; set; }



        [Required]
        public int ProyectoId { get; set; }

        public Proyecto Proyecto { get; set; }

       public  EstadoCertificadoProyecto EstadoId { get; set; }

        public int NumeroCertificado { get; set; }


        public int? OrdenServicioId { get; set; }

        public OrdenServicio OrdenServicio { get; set; }


        public decimal AvanceRealIngenieria{ get; set; }
        public decimal HorasPresupuestadas { get; set; }
        public decimal PorcentajeAsbuilt { get; set; }

        public decimal MontoAnteriorCertificado { get; set; }
        public decimal MontoActualCertificado { get; set; }
        public decimal HorasAnteriorCertificadas { get; set; }
        public decimal HorasActualCertificadas { get; set; }

        public bool DistribucionDirectos { get; set; }




        public bool IsDeleted { get; set; }

        public long? CreatorUserId { get; set; }

        public DateTime CreationTime { get; set; }

        public long? LastModifierUserId { get; set; }

        public DateTime? LastModificationTime { get; set; }

        public long? DeleterUserId { get; set; }

        public DateTime? DeletionTime { get; set; }


        public decimal PorcentajeParticipacionDirectos { get; set; }
    }

    public enum EstadoCertificadoProyecto
    {
        [Description("Inicial")]
        Inicial = 0,

        [Description("Aprobado")]
        Aprobado = 1,

    }
}
