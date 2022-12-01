using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using com.cpp.calypso.comun.dominio;

namespace com.cpp.calypso.proyecto.dominio.Acceso
{
    [Serializable]
    public class TarjetaAcceso : Entity, IFullAudited
    {
        [Obligado]
        [DisplayName("Colaborador")]
        public int ColaboradorId { get; set; }
        public Colaboradores Colaborador { get; set; }

        [Obligado]
        [DisplayName("Secuencial")]
        public int secuencial { get; set; }

        [DisplayName("Solicitud PAM")]
        [StringLength(50)]
        public string solicitud_pam { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Fecha Emisión")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime fecha_emision { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Fecha Vencimiento")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime fecha_vencimiento { get; set; }

        [Obligado]
        [DisplayName("Entregada?")]
        public bool entregada { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Fecha Reporte")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? fecha_reporte { get; set; }

        [DisplayName("Observaciones")]
        [StringLength(200)]
        public string observaciones { get; set; }

        [Obligado]
        [DisplayName("Estado")]
        public TarjetaEstado estado { get; set; }

        [DisplayName("Archivo Pdf")]
        [ForeignKey("DocumentoRespaldo")]
        public int? DocumentoRespaldoId { get; set; }
        public virtual Archivo DocumentoRespaldo { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime CreationTime { get; set; }

        public long? CreatorUserId { get; set; }

        public DateTime? LastModificationTime { get; set; }

        public long? LastModifierUserId { get; set; }

        public DateTime? DeletionTime { get; set; }

        public long? DeleterUserId { get; set; }

        public string Entragada()
        {
            return(entregada ? "SI" : "NO");
        }

        
    }

    public enum TarjetaEstado
    {
        [Description("Activa")]
        Activo = 1,

        [Description("Inactiva")]
        Inactivo = 2, 
    }
}
