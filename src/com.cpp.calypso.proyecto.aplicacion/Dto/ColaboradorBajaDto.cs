using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Entities.Auditing;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.dominio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion.Dto
{
    [AutoMap(typeof(ColaboradorBaja))]
    [Serializable]
    public class ColaboradorBajaDto: EntityDto, IFullAudited
    {
        [DisplayName("Colaborador")]
        public int ColaboradoresId { get; set; }
        public virtual Colaboradores Colaboradores { get; set; }

        [DisplayName("Archivo")]
        public int? ArchivoId { get; set; }
        public virtual Archivo Archivo { get; set; }

        [DisplayName("Motivo de baja")]
        public int? catalogo_motivo_baja_id { get; set; }

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

        public virtual string motivo_baja { get; set; }
        public virtual string motivo_baja_codigo { get; set; }
        public virtual string apellidos_nombres { get; set; }
        public virtual string nombre_grupo_personal { get; set; }
        public virtual string nombre_identificacion { get; set; }
        public virtual string liquidado { get; set; }
        public virtual string estado_baja { get; set; }
        public virtual int nro { get; set; }

        public long? CreatorUserId { get; set; }
        public DateTime CreationTime { get; set; }
        public long? LastModifierUserId { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public long? DeleterUserId { get; set; }
        public DateTime? DeletionTime { get; set; }
        public bool IsDeleted { get; set; }
    }
}
