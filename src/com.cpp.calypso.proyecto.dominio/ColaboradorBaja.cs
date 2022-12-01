using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using com.cpp.calypso.comun.dominio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.dominio
{
    [Serializable]
    public class ColaboradorBaja: Entity, IFullAudited
    {
        [DisplayName("Colaborador")]
        public int ColaboradoresId { get; set; }
        public virtual Colaboradores Colaboradores { get; set; }

        [DisplayName("Archivo")]
        public int? ArchivoId { get; set; }
        public virtual Archivo Archivo { get; set; }

        [DisplayName("Motivo de baja")]
        [ForeignKey("MotivoBaja")]
        public int? catalogo_motivo_baja_id { get; set; }
        public virtual Catalogo MotivoBaja { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Fecha de Baja")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? fecha_baja { get; set; }

        [DisplayName("Detalle de baja")]
        public string detalle_baja { get; set; }

        [Obligado]
        [DisplayName("Requiere Entrevista")]
        public bool requiere_entrevista { get; set; } = false;

        [Obligado]
        [DisplayName("Requiere Entrevista")]
        public bool tiene_encuesta { get; set; } = false;

        [DataType(DataType.Date)]
        [DisplayName("Fecha Pago Liquidacion")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? fecha_pago_liquidacion { get; set; }

        [DisplayName(" Estado")]
        public BajaEstado estado { get; set; }

        [Obligado]
        [DisplayName("Vigente")]
        public bool vigente { get; set; } = true;

        [Obligado]
        [DisplayName("Registro Masivo")]
        public bool registro_masivo { get; set; } = false;

        [DisplayName(" Estado")]
        public string motivo_desestimacion { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Fecha Generación Archivo Sap")]
        public DateTime? fecha_generacion_archivo_sap { get; set; }

        [Obligado]
        [DisplayName("Envío Manual")]
        public bool envio_manual { get; set; } = false;

        [DisplayName("Archivo Liquidacion")]
        [ForeignKey("ArchivoLiquidacion")]
        public int? archivo_liquidacion_id { get; set; }

        [Obligado]
        [DisplayName("Enviado Archivo IESS")]
        public bool enviado_archivo_iess { get; set; } = false;

        [DataType(DataType.Date)]
        [DisplayName("Fecha Envio Archivo IESS")]
        public DateTime? fecha_envio_archivo_iess { get; set; }

        [DisplayName("Generación Archivo IESS")]
        public string generacion_archivo_iess { get; set; }

        [DisplayName("Motivo de Edicion")]
        public string motivo_edicion { get; set; }
        public virtual Archivo ArchivoLiquidacion { get; set; }

        public long? CreatorUserId { get; set; }
        public DateTime CreationTime { get; set; }
        public long? LastModifierUserId { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public long? DeleterUserId { get; set; }
        public DateTime? DeletionTime { get; set; }
        public bool IsDeleted { get; set; }

    }

    public enum BajaEstado
    {
        [Description("ENVIAR SAP")]
        ENVIAR_SAP = 1,

        [Description("ENVIADO SAP")]
        ENVIADO_SAP = 2,

        [Description("POR LIQUIDADO")]
        POR_LIQUIDAR = 3,

        [Description("LIQUIDADO")]
        LIQUIDADO = 4,

        [Description("DESESTIMADA")]
        DESESTIMADA = 0
    }
}
