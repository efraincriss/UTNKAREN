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
    public class GastoDirectoCertificado : Entity, IFullAudited
    {

        [Required]
        public int CertificadoIngenieriaProyectoId { get; set; }

        public CertificadoIngenieriaProyecto CertificadoIngenieriaProyecto { get; set; }


        public TipoGasto TipoGastoId { get; set; }


        [Required]
        public int ColaboradorId { get; set; }

        public Colaboradores Colaborador { get; set; }


        public int? RubroId { get; set; }

        public DetallePreciario Rubro { get; set; }


        
        public int? UnidadId { get; set; }
        public Catalogo Unidad { get; set; }

        public decimal TotalHoras { get; set; }
        public decimal TarifaHoras { get; set; }
        public decimal Tarifa { get; set; }

        public bool EsDistribucionE500 { get; set; }
       
        public bool migrado { get; set; }

        public int? UbicacionId { get; set; }
        public Catalogo Ubicacion { get; set; }
        public int? EspecialidadId { get; set; }
        public Catalogo Especialidad { get; set; }

        public string UbicacionTrabajo { get; set; } //UIO u OIT

        public string Area { get; set; } //Ingenieria o PyCP

        public string NombreEspecialidad { get; set; } //Ingenieria o PyCP
        public string NombreEtapa { get; set; } //ETAP ID IB AB

        public bool EsViatico { get; set; } = false;
        public bool AplicaViatico { get; set; } = false;

        public bool IsDeleted { get; set; }

        public long? CreatorUserId { get; set; }

        public DateTime CreationTime { get; set; }

        public long? LastModifierUserId { get; set; }

        public DateTime? LastModificationTime { get; set; }

        public long? DeleterUserId { get; set; }

        

        public DateTime? DeletionTime { get; set; }
    }

    public enum TipoGasto
    {
        [Description("Directo")]
        Directo = 0,

        [Description("Indirecto")]
        Indirecto = 1,

    }

}
