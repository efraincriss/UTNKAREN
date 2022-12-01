using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using com.cpp.calypso.proyecto.dominio.CertificacionIngenieria;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion.CertificacionIngenieria.Dto
{

    [AutoMap(typeof(GrupoCertificadoIngenieria))]
    [Serializable]
    public class GrupoCertificadoIngenieriaDto : EntityDto {

        [Required]
        public int ClienteId { get; set; }

       public DateTime FechaCertificado { get; set; }

        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }

        public DateTime FechaGeneracion { get; set; } = DateTime.Now.Date;

        public EstadoGrupoCertificado EstadoId { get; set; }


        public int Mes { get; set; }
        public int Anio { get; set; }


        public bool IsDeleted { get; set; }

        public long? CreatorUserId { get; set; }

        public DateTime CreationTime { get; set; }

        public long? LastModifierUserId { get; set; }

        public DateTime? LastModificationTime { get; set; }

        public long? DeleterUserId { get; set; }

        public DateTime? DeletionTime { get; set; }


        public virtual string NombreCliente { get; set; }
        public virtual string FechaCertificadoDate { get; set; }
        public virtual string FechaInicioDate { get; set; }
        public virtual string FechaFinDate { get; set; }
        public virtual string FechaGeneracionDate { get; set; }
        public virtual string EstadoString { get; set; }
    }
}
