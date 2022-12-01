using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.dominio.CertificacionIngenieria
{
    [Serializable]
    public class DistribucionCertificadoIngenieria : Entity, ISoftDelete
    {
        [Required]
        public int GrupoCertificadoId { get; set; }

        public GrupoCertificadoIngenieria GrupoCertificado { get; set; }

        [Required]
        public int ProyectoId { get; set; }

        public Proyecto Proyecto { get; set; }

        public bool AplicaViatico { get; set; }

        public bool AplicaIndirecto { get; set; }
        public bool AplicaE500 { get; set; }

        public bool IsDeleted { get ; set ; }
    }
}
