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
    public class DetalleDirectoE500 : Entity, IFullAudited
    {

        public string Identificacion { get; set; }

        [Required]
        public int ColaboradorId { get; set; }

        public Colaboradores Colaborador { get; set; }

        [Required]
        public int ClienteId { get; set; }

        public Cliente Cliente { get; set; }


        public int? TipoRegistroId { get; set; }

        public Catalogo TipoRegistro { get; set; }


        public decimal NumeroHoras { get; set; }
    
        public string NombreEjecutante { get; set; }


        [DisplayName("FechaTrabajo")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime FechaTrabajo { get; set; }

        public string Observaciones { get; set; }


        public int? EtapaId { get; set; }

        public Catalogo Etapa { get; set; }




        public int? EspecialidadId { get; set; }

        public Catalogo Especialidad { get; set; }


        [Required]
        public int EstadoRegistroId { get; set; }

        public Catalogo EstadoRegistro { get; set; }


        public int? LocacionId { get; set; }

        public Catalogo Locacion { get; set; }


        public int? ModalidadId { get; set; }
        public Catalogo Modalidad { get; set; }


        public int? CertificadoId { get; set; }


        public DateTime FechaCarga { get; set; } = DateTime.Now;

        public int? Secuencial { get; set; }

        public bool IsDeleted { get; set; }

        public long? CreatorUserId { get; set; }

        public DateTime CreationTime { get; set; }

        public long? LastModifierUserId { get; set; }

        public DateTime? LastModificationTime { get; set; }

        public long? DeleterUserId { get; set; }

        public DateTime? DeletionTime { get; set; }
    }
}

