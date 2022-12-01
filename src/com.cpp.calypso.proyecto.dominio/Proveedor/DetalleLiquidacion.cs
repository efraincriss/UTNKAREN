using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using com.cpp.calypso.comun.dominio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.dominio.Proveedor
{
    [Serializable]
    public class DetalleLiquidacion:Entity, IFullAudited
    {
        [Obligado]
        [DisplayName("Liquidacion")]
        public int LiquidacionId { get; set; }
        public LiquidacionServicio Liquidacion { get; set; }


        [DataType(DataType.Date)]
        [DisplayName("Fecha Inicio")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public virtual DateTime Fecha { get; set; }

        [DisplayName("Descripcion")]
        public string Descripcion { get; set; }

        [DisplayName("Valor")]
        [DefaultValue(0.0)]
        public decimal Valor { get; set; }


        public long? CreatorUserId { get; set; }
        public DateTime CreationTime { get; set; }
        public long? LastModifierUserId { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public long? DeleterUserId { get; set; }
        public DateTime? DeletionTime { get; set; }
        public bool IsDeleted { get; set; }
    }
}
