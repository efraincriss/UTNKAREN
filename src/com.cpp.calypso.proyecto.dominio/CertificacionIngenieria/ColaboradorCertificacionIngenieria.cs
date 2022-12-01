using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.dominio.CertificacionIngenieria
{
    [Serializable]
    public class ColaboradorCertificacionIngenieria : Entity, IFullAudited
    {
        public int ColaboradorId { get; set; }

        public Colaboradores Colaborador { get; set; }

        public DateTime FechaDesde { get; set; }

        public DateTime? FechaHasta { get; set; }

        public int? DisciplinaId { get; set; }

        public Catalogo Disciplina { get; set; }

        public int ModalidadId { get; set; }

        public Catalogo Modalidad { get; set; }

        public int UbicacionId { get; set; }

        public Catalogo Ubicacion { get; set; }

        public string CategoriaID { get; set; }

        public int HorasPorDia { get; set; } = 8;

        public bool AplicaViatico { get; set; } = false;
        public bool AplicaViaticoDirecto { get; set; } = false;

        public bool EsJornal { get; set; } = false;

        public bool EsGastoDirecto { get; set; } = false;


        public Area AreaId { get; set; } = 0;
        public bool IsDeleted { get; set; }

        public long? CreatorUserId { get; set; }

        public DateTime CreationTime { get; set; }

        public long? LastModifierUserId { get; set; }

        public DateTime? LastModificationTime { get; set; }

        public long? DeleterUserId { get; set; }

        public DateTime? DeletionTime { get; set; }
    }
    public enum Area
    {
        [Description("Ingeniería")]
        Ingenieria = 0,

        [Description("PyCP")]
        PyCP = 1,

    }
}
