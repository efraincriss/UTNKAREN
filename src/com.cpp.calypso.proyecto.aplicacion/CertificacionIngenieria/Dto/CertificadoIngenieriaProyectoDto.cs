using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using com.cpp.calypso.proyecto.dominio.CertificacionIngenieria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion.CertificacionIngenieria.Dto

{
    [AutoMap(typeof(CertificadoIngenieriaProyecto))]
    [Serializable]
    public class CertificadoIngenieriaProyectoDto : EntityDto
    {

        public int GrupoCertificadoIngenieriaId { get; set; }
        public int ProyectoId { get; set; }
        public EstadoCertificadoProyecto EstadoId { get; set; }
        public int NumeroCertificado { get; set; }

        public int? OrdenServicioId { get; set; }
        public decimal AvanceRealIngenieria { get; set; }
        public decimal HorasPresupuestadas { get; set; }
        public decimal PorcentajeAsbuilt { get; set; }

        public decimal MontoAnteriorCertificado { get; set; }
        public decimal MontoActualCertificado { get; set; }
        public decimal HorasAnteriorCertificadas { get; set; }
        public decimal HorasActualCertificadas { get; set; }
        public bool DistribucionDirectos { get; set; }

        public decimal PorcentajeParticipacionDirectos { get; set; }

        public bool IsDeleted { get; set; }

        public long? CreatorUserId { get; set; }

        public DateTime CreationTime { get; set; }

        public long? LastModifierUserId { get; set; }

        public DateTime? LastModificationTime { get; set; }

        public long? DeleterUserId { get; set; }

        public DateTime? DeletionTime { get; set; }

        public virtual string NombreContrato { get; set; }
        public virtual string NombreProyecto { get; set; }
        public virtual string EstadoString { get; set; }
        public virtual string NumeroCertificadoString { get; set; }
        public virtual string GrupoCertificadoIngenieriaString { get; set; }
        public virtual string DistribucionDirectosString{ get; set; }
        public virtual decimal TotalHorasDirectos { get; set; }
        public virtual decimal TotalHorasIndirectos { get; set; }
    }
}
