using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using com.cpp.calypso.proyecto.dominio;
using com.cpp.calypso.proyecto.dominio.CertificacionIngenieria;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion.CertificacionIngenieria.Dto
{
    [AutoMap(typeof(AvancePorcentajeProyecto))]
    [Serializable]
    public class AvancePorcentajeProyectoDto : EntityDto
    {
        [Required]
        public int ProyectoId { get; set; }


        public int? CertificadoId { get; set; }
        public DateTime? FechaCertificado { get; set; }


        public decimal AvancePrevistoAnteriorIB { get; set; }
        public decimal AvanceRealAnteriorIB { get; set; }
        public decimal AvancePrevistoActualIB { get; set; }
        public decimal AvanceRealActualIB { get; set; }

        public decimal AvancePrevistoAnteriorID { get; set; }
        public decimal AvanceRealAnteriorID { get; set; }
        public decimal AvancePrevistoActualID { get; set; }
        public decimal AvanceRealActualID { get; set; }


       public decimal AsbuiltAnterior { get; set; }
        public decimal AsbuiltActual { get; set; }

        public string Justificacion { get; set; }

        public bool unaFase { get; set; } = false;
        public bool IsDeleted { get; set; }

        public long? CreatorUserId { get; set; }

        public DateTime CreationTime { get; set; }

        public long? LastModifierUserId { get; set; }

        public DateTime? LastModificationTime { get; set; }

        public long? DeleterUserId { get; set; }

        public DateTime? DeletionTime { get; set; }


        public virtual string  nombreProyecto { get; set; }
        public virtual string nombreContrato { get; set; }
        public virtual string  nombreCertificado { get; set; }
        public virtual string formatFechaCertificado { get; set; }


    }
}
